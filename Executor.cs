using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistAnt
{
    public class Executor : IDisposable
    {
        public const string AboutText = @"AssistAnt Версия 1.1 от 2022.02.07 https://github.com/AantCoder/AssistAnt

Программа для распознавания текста и перевода. Есть 3 варианта использования программы:

* Через контекстное меню в трее можно только распознать текст с изображения в буфере, либо распознать и перевести, либо, если в буфере уже текст, только перевести. Результат выводится в сообщении и сохраняется в буфер обмена.

* Можно зажать клавиши Win+Alt, передвинуть мышь и отпустить. Выделенная область экрана будет распознана, переведена и показана во всплывающей подсказке. Она пропадет, если увести мышь в сторону. Буфер не используется.

* Можно нажать и сразу отпустить клавиши Win+Alt. Экран перейдет в режим выделения области с помощью мыши. Изображение будет скопированно в буфер обмена, а его перевод показан во всплывающей подсказке.

Программа использует компонент распознавания текста Tesseract OCR https://github.com/tesseract-ocr.

И онлайн переводчик Google Translate Rest API (Free) с помощью GTranslatorAPI https://github.com/franck-gaspoz/GTranslatorAPI.

Автор Иванов Василий Сергеевич, 2022г. emAnt@mail.ru
Распространяется под лицензией MIT
";

        public const string AboutURL = "https://github.com/AantCoder/AssistAnt";
        public Action<string> TestLog = s => { };
        private int StatusKeyDown = 0;
        private bool AnyKeyDown = false;
        private bool FuncKeyDowned = false;
        private Form SelectForm;
        private ToolTip SelectOutText;
        private bool SelectFormIsShow = false;
        private Point SelectStartPoint;
        private Point SelectEndPoint;

        public Executor()
        {
            InterceptKeys.Hook += InterceptKeys_Hook;
            InterceptKeys.InitializeComponent();

#if DEBUG
            var testForm = new Form1();
            testForm.Show();
            TestLog = s => testForm.textBox1.Text += Environment.NewLine + s;
#endif
            SelectForm = new FormLite();
            SelectForm.TopMost = true;
            SelectForm.ShowInTaskbar = false;
            
            SelectForm.FormBorderStyle = FormBorderStyle.None;
            SelectForm.Opacity = .5;
            SelectForm.Paint += SelectForm_Paint;
            SelectForm.Show();
            SelectForm.Hide();
        }

        //private Pen SelectForm_Pen = new Pen(Color.FromArgb(41, 136, 126));
        private Brush SelectForm_Brushes = new SolidBrush(Color.FromArgb(42, 209, 191));
        private void SelectForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(SelectForm_Brushes, e.ClipRectangle);
            //g.DrawRectangle(SelectForm_Pen, e.ClipRectangle);
        }

        public void Dispose()
        {
            InterceptKeys.UnhookComponent();
        }

        private void InterceptKeys_Hook(int nCode, int vkCode, int wParam)
        {
            if (FormScreen.IsShow)
            {
                AnyKeyDown = true;
                StatusKeyDown = 0;
                return;
            }

            var innerKeyCode = 0;
            switch ((Keys)vkCode)
            {
                case Keys.LWin:
                case Keys.RWin: 
                    innerKeyCode = 1; 
                    break;
                case Keys.LMenu:
                case Keys.RMenu: 
                    innerKeyCode = 2; 
                    break;
                default: 
                    AnyKeyDown = true;
                    StatusKeyDown = 0;
                    if (FuncKeyDowned) FuncKeyCancel();
                    return;
            }
            if (wParam == InterceptKeys.WM_KEYDOWN || wParam == InterceptKeys.WM_SYSKEYDOWN)
            {
                //перед нажатием на первую кнопку из двух сбрасываем флаги
                if (StatusKeyDown == 0)
                {
                    AnyKeyDown = false; //дальше проверяем, чтобы не было других нажатий
                    FuncKeyDowned = false; //сбрасываем флаг свершившегося вызова FuncKeyDown 
                }
                StatusKeyDown |= innerKeyCode;
                if (StatusKeyDown == 3 && !AnyKeyDown)
                {
                    if (!FuncKeyDowned)
                    {
                        FuncKeyDowned = true;
                        FuncKeyDown();
                    }
                    else
                    {
                        FuncKeyDowning();
                    }
                }
                return;
            }
            if (StatusKeyDown > 0 && StatusKeyDown < 3)
            {
                //когда отжали только одну из кнопок
                AnyKeyDown = true;
                StatusKeyDown = 0;
                if (FuncKeyDowned) FuncKeyCancel();
                return;
            }
            //когда одна из нужных кнопок отжимается
            if (StatusKeyDown == 3 && !AnyKeyDown) //и были нажаты обе, и не было нажато других кнопок
            {
                StatusKeyDown = 0;
                FuncKeyUp();
            }

            #region testdata
            //TestLog($"nCode={nCode} vkCode={vkCode} wParam={wParam} '{(Keys)vkCode}'");
            //nCode = 0 vkCode = 91 wParam = 256 'LWin'
            //nCode = 0 vkCode = 91 wParam = 257 'LWin'

            //nCode = 0 vkCode = 164 wParam = 260 'LMenu'
            //nCode = 0 vkCode = 164 wParam = 257 'LMenu'

            //nCode = 0 vkCode = 91 wParam = 256 'LWin'
            //nCode = 0 vkCode = 164 wParam = 260 'LMenu'
            //nCode = 0 vkCode = 91 wParam = 261 'LWin'
            //nCode = 0 vkCode = 164 wParam = 257 'LMenu'

            //nCode = 0 vkCode = 164 wParam = 260 'LMenu'
            //nCode = 0 vkCode = 91 wParam = 260 'LWin'
            //nCode = 0 vkCode = 91 wParam = 261 'LWin'
            //nCode = 0 vkCode = 164 wParam = 257 'LMenu'

            //nCode = 0 vkCode = 164 wParam = 260 'LMenu'
            //nCode = 0 vkCode = 91 wParam = 260 'LWin'
            //nCode = 0 vkCode = 164 wParam = 257 'LMenu'
            //nCode = 0 vkCode = 91 wParam = 257 'LWin'
            #endregion
        }

        private void FuncKeyDown()
        {
#if DEBUG
            TestLog($"FuncKeyDown");
#endif
            SelectStartPoint = Cursor.Position;
        }

        private Rectangle GetSelectArea()
        {
            var res = new Rectangle();
            SelectEndPoint = Cursor.Position;
            if (SelectEndPoint.X < SelectStartPoint.X)
            {
                res.X = SelectEndPoint.X;
                res.Width = SelectStartPoint.X - SelectEndPoint.X;
            }
            else
            {
                res.X = SelectStartPoint.X;
                res.Width = SelectEndPoint.X - SelectStartPoint.X;
            }
            if (SelectEndPoint.Y < SelectStartPoint.Y)
            {
                res.Y = SelectEndPoint.Y;
                res.Height = SelectStartPoint.Y - SelectEndPoint.Y;
            }
            else
            {
                res.Y = SelectStartPoint.Y;
                res.Height = SelectEndPoint.Y - SelectStartPoint.Y;
            }
            return res;
        }

        private void FuncKeyDowning()
        {
#if DEBUG
            TestLog($"...");
#endif
            var area = GetSelectArea();

            SelectForm.Top = area.Y;
            SelectForm.Left = area.X;
            SelectForm.Width = area.Width;
            SelectForm.Height = area.Height;

            if (!SelectFormIsShow)
            {
                SelectFormIsShow = true;
                SelectForm.Show();
            }
        }

        private void FuncKeyUp()
        {
#if DEBUG
            TestLog($"FuncKeyUp");
#endif
            //FuncKeyCancel();
            var area = GetSelectArea();

            if (area.Width <= 1 || area.Height <= 1)
            {
                FuncKeyCancel();

                //если область не была выделена, то режим разметки как в Ножницах
                FormScreen.ScreenShow(bm =>
                {
                    try
                    {
                        Clipboard.SetImage(bm);
                    }
                    catch { }

                    //открываем форму, чтобы привязать к ней высплывающую подсказку
                    SelectEndPoint = Cursor.Position;

                    SelectForm.Top = SelectEndPoint.Y;
                    SelectForm.Left = SelectEndPoint.X;
                    SelectForm.Width = 1;
                    SelectForm.Height = 1;

                    if (!SelectFormIsShow)
                    {
                        SelectFormIsShow = true;
                        SelectForm.Show();
                    }

                    ShowToolTip(bm, 0, 0);
                });
                return;
            }

            Bitmap bmp = new Bitmap(area.Width, area.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(area.X, area.Y, 0, 0, new Size(area.Width, area.Height));
            }

            ShowToolTip(bmp, 0, area.Height);
        }

        private void FuncKeyCancel()
        {
#if DEBUG
            TestLog($"XXXXX");
#endif
            if (SelectFormIsShow)
            {
                if (SelectOutText != null) SelectOutText.Hide(SelectForm);
                SelectFormIsShow = false;
                SelectForm.Hide();
            }
        }

        private int Dist(Point p1, Point p2)
        {
            return Math.Max(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }

        private void ShowToolTip(Bitmap bmp, int offsetToolTipX, int offsetToolTipY)
        { 
            var txt = TranslatorImage(bmp, "eng", "rus");

            SelectOutText = new ToolTip()
            {
                ShowAlways = false,
                UseAnimation = false,
                UseFading = false,
            };
            SelectOutText.Show("Перевод:" + Environment.NewLine + txt, SelectForm, offsetToolTipX, offsetToolTipY);

            new Thread(WaitShowToolTip).Start();
        }

        private void WaitShowToolTip()
        {
            do
            {
                Thread.Sleep(100);
            }
            while (Dist(SelectEndPoint, Cursor.Position) < 100);

            SelectForm.Invoke((Action)FuncKeyCancel);
        }

        public string TranslatorImage(Image image, string sourceLanguage, string targetLanguage)
        {
            var source = new Tesseract(sourceLanguage).GetTextFromImage(image);
            if (source == null) return null;

            var tr = new TranslatorText(sourceLanguage, targetLanguage);
            return tr.Translate(source);
        }

        public string Translator(string sourceLanguage, string targetLanguage)
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

        public string GetTextFromClipboardImage(string language)
        {
            return new Tesseract(language).GetTextFromClipboardImage();
        }


        #region util functions

        public class FormLite : Form
        {
            protected override bool ShowWithoutActivation
            {
                get { return true; }
            }

            private const int WS_EX_TOPMOST = 0x00000008;
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= WS_EX_TOPMOST;
                    return createParams;
                }
            }
        }

        private static Control GetNullControl = null;
        public static void ShowMenu(ContextMenu menu)
        {
            if (GetNullControl == null)
            {
                GetNullControl = new FormLite();
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

        public string GetActiveWindowTitle()
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

#endregion

    }
}
