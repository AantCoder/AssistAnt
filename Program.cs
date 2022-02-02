using ClipboardAssist;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private static ContextMenu AutoMenu { get; set; } = null;

        private const string AboutText = @"AssistAnt Версия 1.0 от 2022.02.02 https://github.com/AantCoder/AssistAnt

Программа для распознавания текста и перевода. Источником служит изображение с текстом или простой текст в буфере обмена.
Для работы воспользуйтесь контекстным меню в трее или включите режим автоматического предложения. В этом режиме вырежьте часть изображения стандартным приложением Windows (системным или Ножницами), сразу после этого откроется предложение по распознаванию и переводу.
Программа использует компонент распознавания текста Tesseract OCR https://github.com/tesseract-ocr.
И онлайн переводчик Google Translate Rest API (Free) с помощью GTranslatorAPI https://github.com/franck-gaspoz/GTranslatorAPI.

Автор Иванов Василий Сергеевич, 2022г. emAnt@mail.ru
Распространяется под лицензией MIT";

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var clipboard = new ClipboardMonitor();
            clipboard.ClipboardChanged += Clipboard_ClipboardChanged;

            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); //SystemIcons.Asterisk;
            TrayIcon.Text = "AssistAnt - Распознование текста на скриншотах";
            TrayMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Выход", Exit),
                });
            TrayMenu.Popup += IconMenuPopup;
            TrayIcon.ContextMenu = TrayMenu;
            TrayIcon.Visible = true;

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var arg = args[1].Replace("-", "").Replace("/", "");
                if (arg == "am")
                {
                    AutoMenuOn(null, null);
                }
            }

            Application.Run(); // new Form1());
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
            var mia = new MenuItem("Предлагать автоматически", AutoMenuSwitch);
            mia.Checked = AutoMenu != null;
            TrayMenu.MenuItems.Add(mia);
            TrayMenu.MenuItems.Add(new MenuItem("Добавть в автозагрузку", WindowsAutorun));
            TrayMenu.MenuItems.Add(new MenuItem("О программе", AboutTheProgram));
            TrayMenu.MenuItems.Add(new MenuItem("Выход", Exit));
        }

        private static void AutoMenuOn(object sender, EventArgs e)
        {
            AutoMenu = new ContextMenu(new MenuItem[]
                {
                    new MenuItem("Перевести eng->rus", TranslatorEngRus),
                    new MenuItem("Перевести rus->eng", TranslatorRusEng),
                    new MenuItem("Распознать текст eng", ImageEng),
                    new MenuItem("Распознать текст rus", ImageRus),
                    new MenuItem("-"),
                    new MenuItem("Не предлагать автоматически", AutoMenuOff) { Checked = true },
                    new MenuItem("Отмена"),
                });
        }

        private static void AutoMenuOff(object sender, EventArgs e)
        {
            AutoMenu = null;
        }

        private static void AutoMenuSwitch(object sender, EventArgs e)
        {
            if (AutoMenu == null) AutoMenuOn(sender, e);
            else AutoMenuOff(sender, e);
        }

        private static void AboutTheProgram(object sender, EventArgs e)
        {
            ShowText(AboutText, "О программе", "https://github.com/AantCoder/AssistAnt");
        }

        private static void WindowsAutorun(object sender, EventArgs e)
        {
            const string applicationName = "AssistAnt";
            const string pathRegistryKeyStartup = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

            using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
            {
                registryKeyStartup.SetValue(applicationName, string.Format("\"{0}\" {1}"
                    , System.Reflection.Assembly.GetExecutingAssembly().Location
                    , AutoMenu == null ? "" : "-am"));
            }

            MessageBox.Show("Добавлено в автозагрузку" + (AutoMenu == null ? "" : " с опцией предлагать автоматически")
                , "Готово" + " - AssistAnt");
        }

        private static void Clipboard_ClipboardChanged(object sender, ClipboardChangedEventArgs e)
        {
            if (AutoMenu == null) return;
            if (!Clipboard.ContainsImage()) return;

            var activeWindow = GetActiveWindowTitle();
            if (activeWindow == null)
            {
                Thread.Sleep(100);
                activeWindow = GetActiveWindowTitle();
            }
            if (activeWindow == null) return;
            if (activeWindow != "Ножницы"
                && activeWindow != "Создание фрагмента экрана"
                && !activeWindow.Contains("Sketch tool") //not test it
                && !activeWindow.Contains("Snipping")) //not test it
            {
                return;
            }

            AutoMenu.ShowMenu();
        }

        private static void TranslatorEngRus(object sender, EventArgs e)
        {
            var txt = Translator("eng", "rus");
            ShowText(txt, "Перевести");
        }

        private static void TranslatorRusEng(object sender, EventArgs e)
        {
            var txt = Translator("rus", "eng");
            ShowText(txt, "Перевести");
        }

        private static string Translator(string sourceLanguage, string targetLanguage)
        {
            string source;
            if (Clipboard.ContainsText())
            {
                source = Clipboard.GetText();
            }
            else
            {
                source = new Tesseract(sourceLanguage).GetTextFromClipboardImage();
            }
            if (source == null) return null;

            var tr = new TranslatorText(sourceLanguage, targetLanguage);
            return tr.Translate(source);
        }

        private static void ImageEng(object sender, EventArgs e)
        {
            var txt = new Tesseract("eng").GetTextFromClipboardImage();
            ShowText(txt, "Распознать текст");
        }

        private static void ImageRus(object sender, EventArgs e)
        {
            var txt = new Tesseract("rus").GetTextFromClipboardImage();
            ShowText(txt, "Распознать текст");
        }

        private static void Exit(object sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            Application.Exit();
        }
        private static void ShowText(string text, string caption, string copy = null)
        {
            if (text == null) return;
            Clipboard.SetText(copy ?? text);
            MessageBox.Show(text, caption + " - AssistAnt");
        }

        #region utils

        private static Control GetNullControl = null;
        private static void ShowMenu(this ContextMenu menu)
        {
            if (GetNullControl == null)
            {
                GetNullControl = new Form();
                ((Form)GetNullControl).FormBorderStyle = FormBorderStyle.None;
                GetNullControl.Show();
                GetNullControl.Hide();
            }
            var pos = Cursor.Position;
            GetNullControl.Width = 1;
            GetNullControl.Height = 1;
            GetNullControl.Left = pos.X;
            GetNullControl.Top = pos.Y;
            GetNullControl.Show();
            menu.Show(GetNullControl, new Point());
            GetNullControl.Hide();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        #endregion utils
    }
}
