using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistAnt
{
    public partial class PetProcessView : UserControl
    {
        public PetInfo Data;

        public event Action<object, MouseEventArgs> CanvasMouseDown;
        public event Action<object, MouseEventArgs> CanvasMouseMove;
        public event Action<object, MouseEventArgs> CanvasMouseUp;

        private int lastStatus = -9999;

        private static List<Image> Images;

        static PetProcessView()
        { 
            var dir = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            Images = new List<Image>()
            {
                Image.FromFile(Path.Combine(dir, "Images\\imgClose.png")),
                Image.FromFile(Path.Combine(dir, "Images\\imgBad.png")),
                Image.FromFile(Path.Combine(dir, "Images\\imgNo.png")),
                Image.FromFile(Path.Combine(dir, "Images\\imgOK.png")),
                Image.FromFile(Path.Combine(dir, "Images\\imgNorm.png")),
            };
        }

        public PetProcessView(PetInfo data)
        {
            Data = data;
            InitializeComponent();
            Data.OnUpdate += InvokeData_OnUpdate;
            Data_OnUpdate();
        }

        private void InvokeData_OnUpdate()
        {
            if (this.InvokeRequired) this.Invoke((Action)Data_OnUpdate);
            else Data_OnUpdate();
            Thread.Sleep(100);
        }

        private void Data_OnUpdate()
        {
            textName.Text = Data.Name;
            textStatus.Text = Data.StatusTest;
            if (lastStatus != Data.Status)
            {
                lastStatus = Data.Status;
                var index = lastStatus == -2 ? 0
                    : lastStatus == -1 ? 1
                    : lastStatus == 0 ? 2
                    : lastStatus == 1 ? 3
                    : lastStatus == 2 ? 4
                    : 1;
                imageStatus.Image = Images[index];
            }
        }

        private void textStatus_MouseDown(object sender, MouseEventArgs e)
        {
            if (CanvasMouseDown != null) CanvasMouseDown(this, e);
        }

        private void textStatus_MouseMove(object sender, MouseEventArgs e)
        {
            if (CanvasMouseMove != null) CanvasMouseMove(this, e);
        }

        private void textStatus_MouseUp(object sender, MouseEventArgs e)
        {
            if (CanvasMouseUp != null) CanvasMouseUp(this, e);
        }

        private void завершитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.Close();
        }

        private void перезапуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data.Restart();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tmpFileName = Path.GetTempFileName() + ".txt";
            File.WriteAllText(tmpFileName, Data.Log ?? "");
            Process.Start(tmpFileName);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(sender as Control, new Point());
        }
    }
}
