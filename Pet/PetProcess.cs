using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssistAnt
{

    public class PetProcess : IDisposable
    {
        public string ProcessExec;

        public string ProcessArgs;

        public string ProcessDirectory;

        public Action<string> Readln;

        public Action Exited;


        private Process Pet;

        public void Start(string cmd = null)
        {
            if (!string.IsNullOrEmpty(cmd))
            {
                if (cmd[0] == '"' || cmd.Contains(" "))
                {
                    if (cmd[0] == '"')
                    { 
                        var ci = cmd.IndexOf('"', 1);
                        if (ci < 0) ci = cmd.Length - 1;
                        ProcessExec = cmd.Substring(0, ci + 1);
                    }
                    else
                        ProcessExec = cmd.Substring(0, cmd.IndexOf(" "));
                    ProcessArgs = cmd.Substring(ProcessExec.Length).TrimStart();
                }
                else
                {
                    ProcessExec = cmd;
                    ProcessArgs = "";
                }
                ProcessDirectory = Path.GetDirectoryName(ProcessExec.Replace("\"", ""));
            }
            Pet = new Process();
            Pet.ErrorDataReceived += Process_ErrorDataReceived;
            Pet.OutputDataReceived += Process_OutputDataReceived;
            Pet.Exited += Process_Exited;
            Pet.EnableRaisingEvents = true;
            Pet.StartInfo = new ProcessStartInfo()
            {
                FileName = ProcessExec,
                Arguments = ProcessArgs,
                WorkingDirectory = ProcessDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardErrorEncoding = Encoding.GetEncoding(866),
                StandardOutputEncoding = Encoding.GetEncoding(866),
            };
            Pet.Start();
            Pet.BeginErrorReadLine();
            Pet.BeginOutputReadLine();
            AddProcessToControlExit(Pet);
        }

        private static string ControlExitFileName;
        public static void AddProcessToControlExit(Process process)
        { 
            if (ControlExitFileName == null)
            {
                ControlExitFileName = Path.GetTempFileName();
                Process.Start(new ProcessStartInfo(Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "AssistAntControlExit.exe")
                    , $"{Process.GetCurrentProcess().Id} {ControlExitFileName}")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                });
            }
            File.AppendAllText(ControlExitFileName, process.Id.ToString() + Environment.NewLine);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            if (Pet == null) return;
            if (Exited != null) Exited();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            if (Pet == null) return;
            if (Readln != null) Readln(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            if (Pet == null) return;
            if (Readln != null) Readln(e.Data);
        }

        public void Close()
        {
            if (Pet != null)
            {
                var pet = Pet;
                Pet = null;
                try
                {
                    KillTree(pet);
                }
                catch { }
                try
                {
                    pet.Kill();
                }
                catch { }
            }
        }

        public static void KillTree(Process proc)
        {
            var taskKilPsi = new ProcessStartInfo("taskkill");
            taskKilPsi.Arguments = $"/pid {proc.Id} /T /F";
            taskKilPsi.WindowStyle = ProcessWindowStyle.Hidden;
            taskKilPsi.UseShellExecute = false;
            taskKilPsi.RedirectStandardOutput = true;
            taskKilPsi.RedirectStandardError = true;
            taskKilPsi.CreateNoWindow = true;
            var taskKillProc = Process.Start(taskKilPsi);
            taskKillProc.WaitForExit();
            String taskKillOutput = taskKillProc.StandardOutput.ReadToEnd(); // Contains success
            String taskKillErrorOutput = taskKillProc.StandardError.ReadToEnd();
        }

        public void Dispose()
        {
            try
            {
                if (Pet != null) Close();
            }
            catch { }
        }
    }
}
