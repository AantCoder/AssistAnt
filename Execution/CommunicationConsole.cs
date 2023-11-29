using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace AssistAnt
{

    public class Communication : IDisposable
    {
        private readonly NamedPipeServerStream Server;

        public Communication(int code)
        {
            Server = new NamedPipeServerStream("AssistAntPipe" + code, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        }

        public async static Task<byte[]> ClientSend(int code, byte[] data)
        {
            using (var client = new NamedPipeClientStream("AssistAntPipe" + code))
            {
                //await client.ConnectAsync();
                client.Connect();
                SendData(client, data);
                return await ReceiveData(client);
            }
        }

        public async Task ServerRead(Func<byte[], byte[]> processMessage)
        {
            await Server.WaitForConnectionAsync();
            var data = await ReceiveData(Server);
            data = processMessage(data) ?? new byte[0];
            SendData(Server, data);
            Server.WaitForPipeDrain();
            Server.Disconnect();
        }

        private static async Task<byte[]> ReceiveData(PipeStream stream)
        {
            var buf = new byte[sizeof(int)];
            //await stream.ReadAsync(buf, 0, buf.Length);
            stream.Read(buf, 0, buf.Length);
            var len = BitConverter.ToInt32(buf, 0);
            buf = new byte[len];
            stream.Read(buf, 0, buf.Length);
            return buf;
        }

        private static void SendData(PipeStream stream, byte[] data)
        {
            var buf = BitConverter.GetBytes(data.Length);
            stream.Write(buf, 0, buf.Length);
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
