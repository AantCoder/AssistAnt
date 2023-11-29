using GTranslatorAPI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AssistAnt
{
    public class ExecutionMap
    {
        private readonly int CodeAgent;
        private Communication ServerCommunication;
        public Dictionary<string, Func<object, object>> MapWorks = new Dictionary<string, Func<object, object>>
        {
            { "test", (data) => "Test!" },
            { "TesseractEngine", (data) =>
                {
                    var request = data as Tesseract.ProxyRequest;
                    var text = Tesseract.GetTextEngine(request.Imgsource, out var meanConfidence, request.Language);
                    return new Tesseract.ProxyResponse()
                    {
                        Text = text,
                        MeanConfidence = meanConfidence,
                    };
                }},
        };

        [Serializable]
        private class Trans
        {
            public string Name;
            public object Value;
        }

        public ExecutionMap(int codeAgent)
        {
            this.CodeAgent = codeAgent;
        }

        public async Task<object> Client(string workName, object data)
        {
            if (!MapWorks.TryGetValue(workName, out var work)) return null;

            var pack = new Trans()
            {
                Name = workName,
                Value = data, // ()Serialize(data);
            };
            var message = Serialize(pack);
            var response = await Communication.ClientSend(CodeAgent, message);

            var obj = Deserialize(response);
            return obj;
        }

        public async Task Server()
        {
            if (ServerCommunication == null) ServerCommunication = new Communication(CodeAgent);
            await ServerCommunication.ServerRead((message) =>
            {
                var pack = (Trans)Deserialize(message);

                if (!MapWorks.TryGetValue(pack.Name, out var work)) return null;

                var request = pack.Value; // ()Deserialize(pack.Value);
                var response = work.Invoke(request);

                var back = Serialize(response);
                return back;
            });
        }

        [ThreadStatic]
        private static BinaryFormatter formatter = null;

        private static byte[] Serialize(object obj)
        {
            using (var msi = new MemoryStream())
            {
                if (formatter == null) formatter = new BinaryFormatter();
                formatter.Serialize(msi, obj);
                msi.Seek(0, SeekOrigin.Begin);
                return msi.ToArray();
            }
        }

        private static object Deserialize(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            {
                if (formatter == null) formatter = new BinaryFormatter();
                return formatter.Deserialize(msi);
            }
        }

    }
}
