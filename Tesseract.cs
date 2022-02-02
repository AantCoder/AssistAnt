using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tesseract;

namespace AssistAnt
{
    public class Tesseract
    {
        private string Language;

        public Tesseract(string language)
        {
            Language = language ?? "eng";
        }

        public Image GetClipboardImage()
        {
            Image returnImage = null;
            if (Clipboard.ContainsImage())
            {
                returnImage = Clipboard.GetImage();
            }
            return returnImage;
        }

        public string GetTextFromClipboardImage()
        {
            var image = GetClipboardImage(); //new Bitmap("Test.PNG");
            if (image == null) return null;
            return GetTextFromImage(image);
        }

        public string GetTextFromImage(Image image)
        {
            var bm = PrepareBitmap(image);
            return GetText(bm);
        }

        private Bitmap PrepareBitmap(Image source, float resize = 3)
        {
            var img = BitmapResize(source, resize, InterpolationMode.HighQualityBicubic);
            return img;
        }

        private Bitmap BitmapResize(Image source, float resize, InterpolationMode interMode)
        {
            var img = new Bitmap((int)(source.Width * resize), (int)(source.Height * resize));

            using (Graphics g = Graphics.FromImage(img))
            {
                //параметры масштабирования
                g.CompositingQuality = CompositingQuality.HighSpeed;// HighQuality;
                g.InterpolationMode = interMode; // InterpolationMode.HighQualityBicubic;
                //рисуем с новым размером
                Rectangle src = new Rectangle(0, 0, source.Width, source.Height);
                Rectangle dst = new Rectangle(0, 0, img.Width, img.Height);

                g.DrawImage(source, dst, src, GraphicsUnit.Pixel);
            }

            return img;
        }

        public string GetText(Bitmap imgsource)
        {
            var ocrtext = string.Empty; //@"./tessdata"  @"c:\W\AssistAnt\bin\Debug\tessdata"  "eng" rus"
            //Environment.SetEnvironmentVariable("TESSDATA_PREFIX", Environment.CurrentDirectory + @"\tessdata");
            using (var engine = new TesseractEngine(@"./tessdata", Language, EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(imgsource))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }
            }

            return ocrtext.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
        }

    }
}
