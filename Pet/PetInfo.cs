using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssistAnt
{

    public class PetInfo
    {
        public string Name;
        public bool CloseWhenFormHidden;
        public bool Autostart;
        public int Status; // -2 - закрыто, -1 - недоступен, 0 - неизвестно, 1 - хорошо, 2 - плохо/альтернативно
        public string StatusTest;
        public string Log;
        public DateTime TimeStart;
        public DateTime TimeUpdate;
        public DateTime TimeOK;

        public event Action OnUpdate;

        private Action<string> UpdateStatus = null;
        private PetProcess Pet = null;
        private string Command = null;

        private void ParseText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                StatusTest = text.Length > 200 ? text.Substring(0, 200) : text;
                var tc = text.ToLower();
                if (tc.Contains("offline") || tc.Contains("error"))
                    Status = -1;
                else
                    Status = 2;
            }
        }

        private void ParsePercent(string text)
        {
            var ancor = text.LastIndexOf("%");
            if (ancor >= 3)
            {
                var progress = text.Substring(ancor - 3, 4);
                StatusTest = progress == "100%" ? "Ready" : progress;
                Status = progress == "100%" ? 1 : 2;
            }
            else
            {
                StatusTest = "No data";
                Status = 0;
            }
        }

        private void ParsePing(string text)
        {
            var ancor = text.LastIndexOf("TTL=");
            var ancor1 = ancor >= 3 ? text.LastIndexOf("=", ancor) : -1;
            if (ancor1 >= 3)
            {
                var ping = text.Substring(ancor1 + 1, text.IndexOf(" ", ancor1) - (ancor1 + 1));
                StatusTest = "ping " + ping;
                while (ping.Length > 0 && !char.IsDigit(ping[ping.Length - 1])) ping = ping.Remove(ping.Length - 1, 1);
                if (int.TryParse(ping, out var pingint)) Status = pingint < 300 ? 1 : 2;
                else Status = -1;
            }
            else
            {
                StatusTest = "No ping";
                Status = -1;
            }
        }

        public PetInfo(string name, string command, string pattern, bool closeWhenFormHidden, bool autostart)
        { 
            Name = name;
            CloseWhenFormHidden = closeWhenFormHidden;
            Autostart = autostart;

            Pet = new PetProcess();
            //Pet.ProcessExec = "ping";
            //Pet.ProcessArgs = "/t 8.8.8.8";
            //Pet.ProcessExec = "cmd";
            //Pet.ProcessArgs = "/c ping /t 8.8.8.8";
            //Pet.ProcessExec = "E:\\Soft\\StableDiffusion\\webui-user.bat";
            //Pet.ProcessExec = "cmd";
            //Pet.ProcessArgs = "/c E:\\Soft\\StableDiffusion\\webui-user.bat";
            //Pet.ProcessDirectory = "E:\\Soft\\StableDiffusion";
            //Pet.Readln = (msg) => { this.Invoke((Action)(() => { textBox1.Text += msg + Environment.NewLine; })); };
            Pet.Readln = SetText;

            if (pattern?.Trim().ToLower() == "percent") UpdateStatus = ParsePercent;
            else if (pattern?.Trim().ToLower() == "ping") UpdateStatus = ParsePing;
            else if (pattern?.Trim().ToLower() == "text") UpdateStatus = ParseText;
            else if (command.ToLower().Contains("ping ")) UpdateStatus = ParsePing;
            else UpdateStatus = ParsePercent;

            StatusTest = "Closed";
            Status = -2;

            Command = command;
        }

        public void Start()
        {
            Log = null;

            TimeStart = DateTime.Now;
            StatusTest = "No data";
            Status = 0;

            Pet.Start(Command);
        }

        public void Restart()
        {
            TimeStart = DateTime.Now;
            Pet.Close();
            Start();
        }

        public void Close()
        {
            Pet.Close();
            StatusTest = "Closed";
            Status = -2;
            if (OnUpdate != null) OnUpdate();
            Thread.Sleep(400);
            StatusTest = "Closed";
            Status = -2;
            if (OnUpdate != null) OnUpdate();
        }

        private void SetText(string text)
        {
            if (Log != null && Log.Length > 3_000_000) Log = Log.Substring(Log.Length - 1_000_000);
            Log += text + Environment.NewLine;
            if (UpdateStatus != null) UpdateStatus(text);
            TimeUpdate = DateTime.Now;
            if (Status > 0) TimeOK = TimeUpdate;
            if (OnUpdate != null) OnUpdate();
        }
    }
}
