using AssistAnt.Pet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace AssistAnt.ActualCopy
{
    public class ActualCopyService
    {
        public int SynchronizationDelayMinutes = 12 * 60;

        public string SettingFileName;
        public ACSettingStorage Setting;

        private Thread Worker;
        private bool WorkerIsBusy;

        public ActualCopyService()
        {
            SettingFileName = Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "AssistAntActualCopy.xml");
            GetSetting();

            WorkerIsBusy = false;
            Worker = new Thread(DoWorker);
            Worker.IsBackground = true;
            Worker.Start();
        }

        private void DoWorker()
        {
            Thread.Sleep(4000);
            while (true) 
            {
                if (!WorkerIsBusy)
                {
                    var error = SynchronizationWithCheckDelay();
                    if (error != null) MessageBox.Show(error, "Ошибка при синхронизации (AssistAnt)");
                }
                Thread.Sleep(4 * 60_000);
            }
        }

        public string SynchronizationWithCheckDelay()
        {
            GetSetting();
            if ((DateTime.Now - Setting.LastRunDate).TotalMinutes > SynchronizationDelayMinutes)
            {
                 return Synchronization();
            }
            return null;
        }

        public string Synchronization()
        {
            if (WorkerIsBusy) return "Работа ещё не завершена";
            WorkerIsBusy = true;
            GetSetting();
            string error = null;
            foreach (var point in Setting.Points)
            {
#if DEBUG
                MessageBox.Show($"Begin {point.Source} -> {point.Target}");
#endif
                var err = SynchronizationPoint(point);
                if (err != null) error += err + Environment.NewLine;
            }

            Setting.LastRunDate = DateTime.Now;
            SetSetting();
            WorkerIsBusy = false;
            return error;
        }

        private void Log(string message)
        {
#if DEBUG
            File.AppendAllText("D:\\AssistAntLog.txt", message + Environment.NewLine);
#endif
        }

        private string SynchronizationPoint(ACPoint point)
        {
            if (point == null) return null;
            if (!point.Active) return null;
            if (point.Source == null
                || !Directory.Exists(point.Source))
            {
                point.LastLogMessage = $"Не найдена папка источника " + point.Source;
                return point.LastLogMessage;
            }
            if (string.IsNullOrEmpty(point.Target))
            {
                point.LastLogMessage = $"Не задана папка назначения";
                return point.LastLogMessage;
            }
            if (point.Source.ToLower().StartsWith(point.Target.ToLower()))
            {
                point.LastLogMessage = $"Папка назначения в папке источника";
                return point.LastLogMessage;
            }
            if (point.Target.ToLower().StartsWith(point.Source.ToLower()))
            {
                point.LastLogMessage = $"Папка источника в папке назначения";
                return point.LastLogMessage;
            }
            if (point.Target.Length < 4
                || point.Target.Substring(1, 2) != @":\"
                || point.Source.Length < 4
                || point.Source.Substring(1, 2) != @":\")
            {
                point.LastLogMessage = $"Нужно указать абсолютный пусть к папкам, от корня диска, например C:\\Folder";
                return point.LastLogMessage;
            }

            //копируем, чтобы исключить возможность изменения point
            var pointParam = point;
            point = new ACPoint()
            {
                Source = point.Source,
                Target = point.Target,
                Excludes = point.Excludes.ToList(),
            };

            Log($"");
            Log($"======================================");
            Log($"Begin {point.Source} -> {point.Target}");
            Log($"Excludes {point.ExcludesText.Replace(Environment.NewLine, ", ")}");
            try
            {
                if (!Directory.Exists(point.Target))
                {
                    Log($"Directory.CreateDirectory(point.Target) {point.Target}");
#if !DEBUG
                    Directory.CreateDirectory(point.Target);
#endif
                }
            }
            catch(Exception ex)
            {
                pointParam.LastLogMessage = $"Ошибка создания папки назначения " + point.Target + ": " + ex.Message;
                return point.LastLogMessage;
            }

            int fileTotal = 0;
            int fileChange = 0;
            try
            {
                //синхронизируем папки
                var sourceFolders = Directory.GetDirectories(point.Source, "*", SearchOption.AllDirectories)
                    .Where(dir => !CheckExcludesFolder(dir, point))
                    .ToList();
                
                //удаляем
                var sourceFoldersL = sourceFolders  //c L путь относительный, строка начинается с \ и в нижнем регистре
                    .Select(file => file.Substring(point.Source.Length).ToLower())
                    .ToList();
                var targetFoldersL = Directory.GetDirectories(point.Target, "*", SearchOption.AllDirectories)
                    .Select(file => file.Substring(point.Target.Length).ToLower())
                    .ToList();
                for (int i = 0; i < targetFoldersL.Count; i++)
                {
                    var t = targetFoldersL[i];
                    if (!sourceFoldersL.Any(s => s == t))
                    {
                        //  \\?\  https://learn.microsoft.com/ru-ru/dotnet/standard/io/file-path-formats
                        Log($"Directory.Delete(point.Target + t, true) {@"\\?\" + point.Target + t}");
#if !DEBUG
                        Directory.Delete(@"\\?\" + point.Target + t, true);
#endif
                        fileChange++;
                    }
                }

                sourceFolders = new[] { point.Source }
                    .Union(sourceFolders)
                    .ToList();

                foreach (var dir in sourceFolders)
                {
                    var targetFolder = point.Target + dir.Substring(point.Source.Length);

                    //создаем
                    if (!Directory.Exists(@"\\?\" + targetFolder))
                    {
                        Log($"Directory.CreateDirectory(targetFolder) {@"\\?\" + targetFolder}");
#if !DEBUG
                        Directory.CreateDirectory(@"\\?\" + targetFolder);
#endif
                    }

                    //синхронизируем файлы
                    var sourceFiles = Directory.GetFiles(@"\\?\" + dir)
                        .Select(file => file.Replace(@"\\?\", ""))
                        .Where(file => file.Length > dir.Length + 1)
                        .Where(file => !CheckExcludesFile(file, point))
                        .ToList();

                    var sourceFilesL = sourceFiles
                        .Select(file => file.Substring(point.Source.Length).ToLower())
                        .ToList();

                    var targetFilesL = Directory.GetFiles(@"\\?\" + targetFolder)
                        .Select(file => file.Replace(@"\\?\", ""))
                        .Where(file => file.Length > targetFolder.Length + 1)
                        .Select(file => file.Substring(point.Target.Length).ToLower())
                        .ToList();

                    var change = 0;

                    //удаляем
                    for (int i = 0; i < targetFilesL.Count; i++)
                    {
                        var t = targetFilesL[i];
                        if (!sourceFilesL.Any(s => s == t))
                        {
                            Log($"File.Delete(point.Target + t) {@"\\?\" + point.Target + t}");
#if !DEBUG
                            File.Delete(@"\\?\" + point.Target + t);
#endif
                            change++;
                        }
                    }

                    //записываем
                    for (int i = 0; i < sourceFiles.Count; i++)
                    {
                        var s = sourceFiles[i];

                        int j = 0;
                        for (; j < targetFilesL.Count; j++)
                        {
                            var t = targetFilesL[j];
                            if (s.Substring(point.Source.Length).ToLower() == t)
                            {
                                if (!CheckFile(s, point.Target + t)) j = sourceFilesL.Count;

                                break;
                            }
                        }

                        if (j == targetFilesL.Count)
                        {
                            var targetFile = targetFolder + @"\" + Path.GetFileName(s);
                            if (File.Exists(@"\\?\" + targetFile))
                            {
                                Log($"File.Delete(targetFile) {@"\\?\" + targetFile}");
#if !DEBUG
                                File.Delete(@"\\?\" + targetFile);
#endif
                            }
                            Log($"File.Copy(s, targetFile) {@"\\?\" + s}, {@"\\?\" + targetFile}");
#if !DEBUG
                            File.Copy(@"\\?\" + s, @"\\?\" + targetFile);
                            var sInfo = new FileInfo(@"\\?\" + s);
                            var tInfo = new FileInfo(@"\\?\" + targetFile);
                            if (tInfo.LastWriteTime != sInfo.LastWriteTime)
                                try
                                {
                                    tInfo.LastWriteTime = sInfo.LastWriteTime;
                                }
                                catch (Exception exp)
                                {
                                    if ((tInfo.Attributes & FileAttributes.ReadOnly) > 0)
                                    {
                                        tInfo.Attributes = (int)tInfo.Attributes - FileAttributes.ReadOnly;
                                        tInfo.LastWriteTime = sInfo.LastWriteTime;
                                        tInfo.Attributes = tInfo.Attributes | FileAttributes.ReadOnly;
                                    }
                                }
#endif
                            change++;
                        }
                    }

                    fileTotal += sourceFiles.Count;
                    fileChange += change;
                }
            }
            catch (Exception ex)
            {
                pointParam.LastLogMessage = $"Ошибка синхронизации: " + ex.Message;
                return point.LastLogMessage;
            }
            pointParam.LastLogMessage = $"Синхронизация завершена {DateTime.Now.ToString("u").Replace("Z", "")} Файлов изменено {fileChange}. Файлов источника {fileTotal}";
            return null;
        }

        private bool CheckFile(string file1, string file2)
        {
            var sInfo = new FileInfo(@"\\?\" + file1);
            var tInfo = new FileInfo(@"\\?\" + file2);

            return tInfo.LastWriteTime == sInfo.LastWriteTime;
        }

        private bool CheckExcludesFolder(string name, ACPoint point)
        {
            if (point.Excludes == null || string.IsNullOrEmpty(name) || name.Length <= point.Source.Length) return false;

            name = name.ToLower();

            if (name[name.Length - 1] != '\\') name = name + @"\";

            name = name.Substring(point.Source.Length);

            for (int i = 0; i < point.Excludes.Count; i++)
            {
                var ext = point.Excludes[i].ToLower();
                if (ext.Contains(@"\") && ext.Length > 1)
                {
                    if (ext[ext.Length - 1] != '\\') ext = ext + @"\";
                    if (ext[0] == '\\') ext = point.Source + ext;
                    else if (ext.Length < 3 || ext.Substring(1, 2) != @":\") ext = @"\" + ext;
                    if (name.Contains(ext)) return true;
                }
            }

            return false;
        }

        private bool CheckExcludesFile(string name, ACPoint point)
        {
            if (point.Excludes == null || string.IsNullOrEmpty(name)) return false;

            name = name.ToLower();

            name = Path.GetFileName(name);

            for (int i = 0; i < point.Excludes.Count; i++)
            {
                var ext = point.Excludes[i].ToLower();
                if (!ext.Contains(@"\") && ext.Length > 0)
                {
                    if (FitsMask(name, ext)) return true;
                }
            }

            return false;
        }

        //Code function from https://stackoverflow.com/questions/725341/how-to-determine-if-a-file-matches-a-file-mask
        private bool FitsMask(string fileName, string fileMask)
        {
            string pattern =
                 '^' +
                 Regex.Escape(fileMask.Replace(".", "__DOT__")
                                 .Replace("*", "__STAR__")
                                 .Replace("?", "__QM__"))
                     .Replace("__DOT__", "[.]")
                     .Replace("__STAR__", ".*")
                     .Replace("__QM__", ".")
                 + '$';
            return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(fileName);
        }

        public void GetSetting()
        {
            ACSettingStorage setting = null;
            var xmlSerializer = new XmlSerializer(typeof(ACSettingStorage));
            if (File.Exists(SettingFileName))
            {
                using (FileStream fs = new FileStream(SettingFileName, FileMode.OpenOrCreate))
                {
                    setting = xmlSerializer.Deserialize(fs) as ACSettingStorage;
                }
            }
            if (setting == null)
            {
                setting = new ACSettingStorage();
                setting.Points = new List<ACPoint> { new ACPoint()
                {
                    Active = false,
                    Source = Path.Combine(Path.GetDirectoryName(SettingFileName), "TestSource"),
                    Target = Path.Combine(Path.GetDirectoryName(SettingFileName), "TestTarget"),
                    Excludes = new List<string>() { @".svn\", @".git\", @".vs\", @"bin\", @"obj\", @"packages\" },
                } };
            }
            Setting = setting;
        }

        public void SetSetting()
        {
            var xmlSerializer = new XmlSerializer(typeof(ACSettingStorage));
            if (File.Exists(SettingFileName)) File.Delete(SettingFileName);
            using (FileStream fs = new FileStream(SettingFileName, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, Setting);
            }
        }
    }
}
