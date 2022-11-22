using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistAnt
{
    public partial class FormScreen : Form
    {
        private static FormScreen SingleForm = null;
        private Action<Bitmap> Selected;
        private Point SelectStartPoint;
        private Point SelectEndPoint;
        private bool MouseDown = false;
        public static bool IsShow => SingleForm != null && SingleForm.Visible;
        protected override bool ScaleChildren => false;

        private Rectangle GetSelectArea()
        {
            var res = new Rectangle();
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

        public FormScreen()
        {
            InitializeComponent();
        }

        public static void ScreenShow(Action<Bitmap> selected)
        {
            var area = new Rectangle();
            int maxX = 0, maxY = 0;
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Bounds.X < area.X) area.X = screen.Bounds.X;
                if (screen.Bounds.Y < area.Y) area.Y = screen.Bounds.Y;
                if (screen.Bounds.X + screen.Bounds.Width > maxX) maxX = screen.Bounds.X + screen.Bounds.Width;
                if (screen.Bounds.Y + screen.Bounds.Height > maxY) maxY = screen.Bounds.Y + screen.Bounds.Height;
            }
            area.Width = maxX - area.X;
            area.Height = maxY - area.Y;

            var bm0 = new Bitmap(area.Width, area.Height);
            Graphics GH = Graphics.FromImage(bm0);
            GH.CopyFromScreen(area.X, area.Y, 0, 0, bm0.Size);

            var bm1 = new Bitmap(area.Width, area.Height);
            GH = Graphics.FromImage(bm1);
            GH.DrawImage(bm0, 0, 0);
            GH.FillRectangle(new SolidBrush(Color.FromArgb(100, 200, 200, 200)), 0, 0, area.Width, area.Height);

            if (SingleForm == null) SingleForm = new FormScreen();
            SingleForm.Top = area.Top;
            SingleForm.Left = area.Left;
            SingleForm.Width = area.Width;
            SingleForm.Height = area.Height;
            SingleForm.pictureBox1.Image = bm0;
            SingleForm.pictureBox2.Image = bm1;
            SingleForm.Selected = selected;
            SingleForm.Show();
        }

        private void ScreenClose()
        {
            var area = GetSelectArea();
            if (area.Width > 1 && area.Height > 1 && Selected != null)
            {
                var bm = new Bitmap(area.Width, area.Height);
                Graphics GH = Graphics.FromImage(bm as Image);
                GH.DrawImage(pictureBox1.Image, new Rectangle(0, 0, area.Width, area.Height), area, GraphicsUnit.Pixel);
                Selected(bm);
            }
            pictureBox1.Image = null;
            Hide();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown = true;
            SelectStartPoint = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
            SelectEndPoint = e.Location;
            ScreenClose();
        }

        private void FormScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SelectStartPoint = SelectEndPoint = new Point();
                ScreenClose();
            }
        }

        private void FormScreen_Leave(object sender, EventArgs e)
        {
            SelectStartPoint = SelectEndPoint = new Point();
            ScreenClose();
        }

        private void FormScreen_Deactivate(object sender, EventArgs e)
        {
            SelectStartPoint = SelectEndPoint = new Point();
            ScreenClose();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!MouseDown) return;
            //восстанавливаем изображение на стром квадрате
            var area = GetSelectArea();
            var GH = Graphics.FromImage(SingleForm.pictureBox2.Image);
            if (area.Width > 0 && area.Height > 0)
            {
                GH.DrawImage(SingleForm.pictureBox1.Image, area, area, GraphicsUnit.Pixel);
                GH.FillRectangle(new SolidBrush(Color.FromArgb(100, 200, 200, 200)), area);
            }
            //рисуем новый квадрат
            SelectEndPoint = e.Location;
            area = GetSelectArea();
            if (area.Width > 1 && area.Height > 1)
            {
                area.Width -= 1;
                area.Height -= 1;
                GH.DrawImage(SingleForm.pictureBox1.Image, area, area, GraphicsUnit.Pixel);
                GH.DrawRectangle(new Pen(Color.FromArgb(41, 136, 126), 1), area);
            }
            SingleForm.pictureBox2.Image = SingleForm.pictureBox2.Image;
        }
    }
}
