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

        public string GetTextFromClipboardImage(out int confidence, out string comment)
        {
            var image = GetClipboardImage(); //new Bitmap("Test.PNG");
            if (image == null)
            {
                confidence = 0;
                comment = string.Empty;
                return null;
            }
            return GetTextFromImage(image, out confidence, out comment);
        }

        public string GetTextFromImage(Image image, out int confidence, out string comment)
        {
            return GetTextWithPrepare(image, out confidence, out comment);
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

        private Bitmap AutoPrepareBitmapLite(Image source)
        {
            var img = BitmapResize(source, 3, InterpolationMode.HighQualityBicubic);
            img = BitmapSharpen.Monochrome(img);
            img = BitmapSharpen.Sharpen(img);
            return img;
        }

        private Bitmap AutoPrepareBitmap(Image source)
        {
            int resize = 2;

            //Увеличиваем картинку в 2 раза
            var img0 = new Bitmap((int)(source.Width * resize), (int)(source.Height * resize));
            using (Graphics g = Graphics.FromImage(img0))
            {
                //параметры масштабирования
                g.CompositingQuality = CompositingQuality.HighSpeed;// HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // InterpolationMode.HighQualityBicubic;
                //рисуем с новым размером
                Rectangle src = new Rectangle(0, 0, source.Width, source.Height);
                Rectangle dst = new Rectangle(0, 0, img0.Width, img0.Height);

                g.DrawImage(source, dst, src, GraphicsUnit.Pixel);
            }

            //делаем чернобелой
            source = BitmapSharpen.Monochrome(img0);
            var bg = (source as Bitmap).GetPixel(source.Width - 1, source.Height / 2);
            var border = 7;
            var rigth = 200;

            //source.Save("C:\\T\\1.png");

            //расширяем в длинну добавляя справа пустоту
            var img1 = new Bitmap((int)(source.Width + rigth + border), (int)(source.Height + border * 2));
            using (Graphics g = Graphics.FromImage(img1))
            {
                //параметры масштабирования
                g.CompositingQuality = CompositingQuality.HighSpeed;// HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // InterpolationMode.HighQualityBicubic;
                //рисуем с новым размером
                g.FillRectangle(new SolidBrush(bg), 0, 0, img1.Width, img1.Height);

                Rectangle src = new Rectangle(0, 0, source.Width, source.Height);
                Rectangle dst = new Rectangle(border, border, source.Width, source.Height);
                g.DrawImage(source, dst, src, GraphicsUnit.Pixel);
            }

            //img1.Save("C:\\T\\2.png");

            //увеличиваем резкость
            source = BitmapSharpen.Sharpen(img1);

            //увеличиваем картинку ещё в 2 раза
            var img = new Bitmap((int)(source.Width * resize), (int)(source.Height * resize));
            using (Graphics g = Graphics.FromImage(img))
            {
                //параметры масштабирования
                g.CompositingQuality = CompositingQuality.HighSpeed;// HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic; // InterpolationMode.HighQualityBicubic;
                //рисуем с новым размером
                Rectangle src = new Rectangle(0, 0, source.Width, source.Height);
                Rectangle dst = new Rectangle(0, 0, img.Width, img.Height);

                g.DrawImage(source, dst, src, GraphicsUnit.Pixel);
            }

            //увеличиваем резкость
            img = BitmapSharpen.Sharpen(img);

            return img;
        }

        public string GetTextWithPrepare(Image imgsource, out int confidence, out string comment)
        {
            //очень большие изображение не обрабатываем даже минимально
            if (imgsource.Width * imgsource.Height >= 1_000_000)
            {
                //долго обрабатывается уже несколько секунд, в этом случае убираем долгую предобработку 
                var bm0 = imgsource as Bitmap ?? PrepareBitmap(imgsource, 1);
                var txt = GetText(bm0, out var confidence0);

                confidence = (int)(confidence0 * 100);
                comment = "";
                return txt;
            }

            //размер 200_000 обрабатывается уже несколько секунд, в этом случае убираем долгую предобработку
            //уменьшаем порог с запасом
            if (imgsource.Width * imgsource.Height > 20_000)
            {
                //долго обрабатывается уже несколько секунд, в этом случае убираем долгую предобработку 
                var bm0 = PrepareBitmap(imgsource);
                var txt = GetText(bm0, out var confidence0);

                confidence = (int)(confidence0 * 100);
                comment = "*";
                return txt;
            }

            //маленькие изображения дополнительно обрабатываем
            var bm = AutoPrepareBitmap(imgsource);
            var txt1 = GetText(bm, out var confidence1);
            comment = "*1";

            if (confidence1 < .9)
            {
                comment = "*2";
                bm = AutoPrepareBitmapLite(imgsource);
                var txt2 = GetText(bm, out var confidence2);
                if (confidence2 > confidence1)
                {
                    txt1 = txt2;
                    confidence1 = confidence2;
                }

                if (confidence1 < .9)
                {
                    comment = "*3";
                    bm = PrepareBitmap(imgsource);
                    var txt3 = GetText(bm, out var confidence3);
                    if (confidence3 > confidence1)
                    {
                        txt1 = txt3;
                        confidence1 = confidence3;
                    }
                }
            }

            confidence = (int)(confidence1 * 100);
            return txt1;
        }

        public string GetText(Bitmap imgsource)
        {
            return GetText(imgsource, out var _);
        }

        public string GetText(Bitmap imgsource, out float meanConfidence)
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
                        meanConfidence = page.GetMeanConfidence();
                    }
                }
            }

            return ocrtext.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
        }

    }
}
