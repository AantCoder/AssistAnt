using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssistAntControlExit
{
    internal class Program
    {
        private static Process ControlProcess;
        private static int CountTask;

        static void Main(string[] args)
        {
            //69984 "C:\\Users\\User\\AppData\\Local\\Temp\\tmpB3B4.tmp"
            if (args == null || args.Length < 2) return;
            try
            {
                ControlProcess = Process.GetProcessById(int.Parse(args[0]));
            }
            catch { }
            if (ControlProcess == null) return;
            
            ControlProcess.WaitForExit();

            try
            {
                var lines = File.ReadAllLines(args[1]);
                try
                {
                    File.Delete(args[1]);
                }
                catch { }
                foreach(var line in lines) 
                {
                    if (int.TryParse(line.Trim(), out var pid) && pid != 0)
                    {
                        Interlocked.Increment(ref CountTask);
                        var task = new Thread(() => ClosePid(pid));
                        task.IsBackground = true;
                        task.Start();
                    }
                }
            }
            catch { }

            while (CountTask > 0) Thread.Sleep(0);
        }

        private static List<Process> AllProcess;
        private static void ClosePid(int pid)
        {
            try
            {
                var root = Process.GetProcessById(pid);
                if (root != null)
                {
                    try
                    {
                        KillTree(root);
                    }
                    catch { }
                    /*
                    var list = new List<Process>();

                    if (AllProcess == null) AllProcess = Process.GetProcesses().ToList();
                    for (int i = 0; i < AllProcess.Count; i++)
                    {
                        try
                        {
                            if (ParentProcessUtilities.GetParentProcess(AllProcess[i].Id)?.Id == pid)
                            {
                                list.Add(AllProcess[i]);
                            }
                        }
                        catch 
                        {
                            AllProcess.RemoveAt(i--);
                        }
                    }

                    foreach (Process p in list)
                    {
                        try
                        {
                            ForceKill(p);
                        }
                        catch { }
                    }

                    try
                    {
                        ForceKill(root);
                    }
                    catch { }
                    */
                }
            }
            catch { }
            Interlocked.Decrement(ref CountTask);
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
        public static void ForceKill(Process proc)
        {

            // Accessing ProcessName could throw an exception if the process has already been killed.
            string processName = string.Empty;
            try { processName = proc.ProcessName; } catch (Exception ex) { }

            // ProcessId can be accessed after the process has been killed but we'll do this safely anyways.
            int pId = 0;
            try { pId = proc.Id; } catch (Exception ex) { }

            // Will only work if started by this instance of the dll.
            try { proc.Kill(); }
            catch (Exception ex)
            {

                // Fallback to task kill
                if (pId > 0)
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

                // Fallback to wmic delete process.
                if (!string.IsNullOrEmpty(processName))
                {
                    // https://stackoverflow.com/a/38757852/591285
                    var wmicPsi = new ProcessStartInfo("wmic");
                    wmicPsi.Arguments = $@"process where ""name='{processName}.exe'"" delete";
                    wmicPsi.WindowStyle = ProcessWindowStyle.Hidden;
                    wmicPsi.UseShellExecute = false;
                    wmicPsi.RedirectStandardOutput = true;
                    wmicPsi.RedirectStandardError = true;
                    wmicPsi.CreateNoWindow = true;
                    var wmicProc = Process.Start(wmicPsi);
                    wmicProc.WaitForExit();
                    String wmicOutput = wmicProc.StandardOutput.ReadToEnd(); // Contains success
                    String wmicErrorOutput = wmicProc.StandardError.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// A utility class to determine a process parent.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ParentProcessUtilities
        {
            // These members must match PROCESS_BASIC_INFORMATION
            internal IntPtr Reserved1;
            internal IntPtr PebBaseAddress;
            internal IntPtr Reserved2_0;
            internal IntPtr Reserved2_1;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;

            [DllImport("ntdll.dll")]
            private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

            /// <summary>
            /// Gets the parent process of the current process.
            /// </summary>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess()
            {
                return GetParentProcess(Process.GetCurrentProcess().Handle);
            }

            /// <summary>
            /// Gets the parent process of specified process.
            /// </summary>
            /// <param name="id">The process id.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(int id)
            {
                Process process = Process.GetProcessById(id);
                return GetParentProcess(process.Handle);
            }

            /// <summary>
            /// Gets the parent process of a specified process.
            /// </summary>
            /// <param name="handle">The process handle.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(IntPtr handle)
            {
                ParentProcessUtilities pbi = new ParentProcessUtilities();
                int returnLength;
                int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
                if (status != 0)
                    throw new Win32Exception(status);

                try
                {
                    return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
                }
                catch (ArgumentException)
                {
                    // not found
                    return null;
                }
            }
        }
    }
}
