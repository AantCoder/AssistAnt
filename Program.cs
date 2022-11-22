using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AssistAnt
{
    static class Program
    {
        private static NotifyIcon TrayIcon { get; set; }
        private static ContextMenu TrayMenu { get; set; }

        private static Executor Executor { get; set; }
        private static IdleTimeControl IdleTime { get; set; }


        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));

            IdleTime = new IdleTimeControl()
            {
                EventIdle = () =>
                {
                    try
                    {
                        Executor.Dispose();
                        TrayIcon.Visible = false;
                        TrayIcon.Dispose();
                    }
                    catch
                    { }
                    Process.Start(Application.ExecutablePath);
                    Environment.Exit(0);
                }
            };

            Executor = new Executor(IdleTime);

            TrayIcon = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath), //SystemIcons.Asterisk;
                Text = "AssistAnt - Распознование текста на скриншотах"
            };
            TrayMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Выход", Exit),
                });
            TrayMenu.Popup += IconMenuPopup;
            TrayIcon.ContextMenu = TrayMenu;
            TrayIcon.Visible = true;
            TrayIcon.DoubleClick += AboutTheProgram;

            Application.Run();
        }

        private static void IconMenuPopup(object sender, EventArgs e)
        {
            TrayMenu.MenuItems.Clear();

            var ci = Clipboard.ContainsImage();
            if (ci || Clipboard.ContainsText())
            {
                TrayMenu.MenuItems.Add(new MenuItem("Перевести eng->rus", TranslatorEngRus));
                TrayMenu.MenuItems.Add(new MenuItem("Перевести rus->eng", TranslatorRusEng));
            }
            if (ci)
            {
                TrayMenu.MenuItems.Add(new MenuItem("Распознать текст eng", ImageEng));
                TrayMenu.MenuItems.Add(new MenuItem("Распознать текст rus", ImageRus));
            }
            TrayMenu.MenuItems.Add(new MenuItem("-"));
            TrayMenu.MenuItems.Add(new MenuItem("Добавть в автозагрузку", WindowsAutorun));
            TrayMenu.MenuItems.Add(new MenuItem("О программе", AboutTheProgram));
            TrayMenu.MenuItems.Add(new MenuItem("Выход", Exit));
        }

        private static void AboutTheProgram(object sender, EventArgs e)
        {
            ShowText(Executor.AboutText, "О программе", Executor.AboutURL);
        }

        private static void WindowsAutorun(object sender, EventArgs e)
        {
            const string applicationName = "AssistAnt";
            const string pathRegistryKeyStartup = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

            using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
            {
                registryKeyStartup.SetValue(applicationName, string.Format("\"{0}\""
                    , System.Reflection.Assembly.GetExecutingAssembly().Location));
            }

            MessageBox.Show("Добавлено в автозагрузку", "Готово" + " - AssistAnt");
        }

        private static void TranslatorEngRus(object sender, EventArgs e)
        {
            var txt = Executor.Translator("eng", "rus");
            ShowText(txt, "Перевод в буфере");
        }

        private static void TranslatorRusEng(object sender, EventArgs e)
        {
            var txt = Executor.Translator("rus", "eng");
            ShowText(txt, "Перевод в буфере");
        }

        private static void ImageEng(object sender, EventArgs e)
        {
            var txt = Executor.GetTextFromClipboardImage("eng", out int confidence, out string comment);  
            ShowText(txt, $"Текст в буфере ({confidence}%{comment})");
        }

        private static void ImageRus(object sender, EventArgs e)
        {
            var txt = Executor.GetTextFromClipboardImage("rus", out int confidence, out string comment);
            ShowText(txt, $"Текст в буфере ({confidence}%{comment})");
        }

        private static void Exit(object sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            Executor.Dispose();
            Application.Exit();
        }
        private static void ShowText(string text, string caption, string copy = null)
        {
            if (text == null) return;
            if (string.IsNullOrEmpty(copy ?? text)) return;
            Clipboard.SetText(copy ?? text);
            MessageBox.Show(text, caption + " - AssistAnt");
        }

    }
}
