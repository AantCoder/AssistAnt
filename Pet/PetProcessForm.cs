using AssistAnt.Pet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AssistAnt
{
    public partial class PetProcessForm : Form
    {
        public string SettingFileName;
        public SettingStorage Setting;

        //[DllImport("user32.dll")]
        //public static extern IntPtr GetForegroundWindow();
        //[DllImport("user32.dll")]
        //public static extern UInt32 GetWindowThreadProcessId(IntPtr hwnd, ref Int32 pid);
        public static void ShowForm(ref PetProcessForm PetForm)
        {
            //int pidActive = 0;
            //GetWindowThreadProcessId(GetForegroundWindow(), ref pidActive);
            //&& pidActive == Process.GetCurrentProcess().Id
            //Application.DoEvents();
            //Thread.Sleep(1200);
            //Application.DoEvents();

            if (PetForm == null) PetForm = new PetProcessForm();

            if (PetForm.Visible)
            {
                PetForm.Left = Screen.PrimaryScreen.Bounds.Width - 100 - PetForm.Width;
                PetForm.Top = Screen.PrimaryScreen.Bounds.Height - 100 - PetForm.Height;
            }
            PetForm.Show();
            PetForm.Activate();
        }

        public PetProcessForm()
        {
            InitializeComponent();

            SettingFileName = Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "AssistAntSetting.xml");
            Setting = GetSetting();

            for(int i = Setting.Consoles.Count - 1; i >= 0; i--) 
            {
                AddPet(Setting.Consoles[i].Name
                    , Setting.Consoles[i].Command
                    , Setting.Consoles[i].Pattern
                    , Setting.Consoles[i].CloseWhenFormHidden
                    , Setting.Consoles[i].Autostart);
            }

            if (!Setting.WinHideOnStart) this.Show();

            //AddPet("192.168.55.1", "ping /t 192.168.55.1");
            //AddPet("8.8.8.8", "ping /t 8.8.8.8");
            //AddPet("SD", "E:\\Soft\\StableDiffusion\\webui-user.bat");
        }

        private void PetProcessForm_Load(object sender, EventArgs e)
        {
            if (Setting.WinPosX != 0 || Setting.WinPosY != 0)
            {
                this.Left = Setting.WinPosX;
                this.Top = Setting.WinPosY;
            }
            this.TopMost = Setting.WinTopMost;
        }

        private void AddPet(string name, string command, string pattern, bool closeWhenFormHidden, bool start)
        {
            /*
            var pet = new PetProcess();
            //pet.ProcessExec = "ping";
            //pet.ProcessArgs = "/t 8.8.8.8";
            //pet.ProcessExec = "cmd";
            //pet.ProcessArgs = "/c ping /t 8.8.8.8";
            //pet.ProcessExec = "E:\\Soft\\StableDiffusion\\webui-user.bat";
            pet.ProcessExec = "cmd";
            pet.ProcessArgs = "/c E:\\Soft\\StableDiffusion\\webui-user.bat";
            pet.ProcessDirectory = "E:\\Soft\\StableDiffusion";
            pet.Readln = (msg) => { this.Invoke((Action)(() => { textBox1.Text += msg + Environment.NewLine; })); };
            pet.Start();
            */
            var info = new PetInfo(name, command, pattern, closeWhenFormHidden, start);
            //info.Start("E:\\Soft\\StableDiffusion\\webui-user.bat");
            if (start && !closeWhenFormHidden) info.Start();
            var view = new PetProcessView(info);
            view.CanvasMouseDown += PetProcessForm_MouseDown;
            view.CanvasMouseMove += PetProcessForm_MouseMove;
            view.CanvasMouseUp += PetProcessForm_MouseUp;
            view.Dock = DockStyle.Top;
            this.Controls.Add(view);
        }

        private void PetProcessForm_VisibleChanged(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                var view = item as PetProcessView;
                if (view?.Data?.CloseWhenFormHidden == true)
                {
                    if (this.Visible)
                    {
                        //форма показалась
                        if (view.Data.Status == -2
                            && view.Data.Autostart)
                            view.Data.Restart();
                    }
                    else
                    {
                        //форма закрылась
                        if (view.Data.Status != -2)
                            view.Data.Close();
                    }
                    Application.DoEvents();
                }
            }

        }

        private void PetProcessForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private Point? MoveStart;
        private void PetProcessForm_MouseDown(object sender, MouseEventArgs e)
        {
            MoveStart = e.Location;
        }

        private void PetProcessForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveStart == null) return;
            this.Left += e.X - MoveStart.Value.X;
            this.Top += e.Y - MoveStart.Value.Y;
        }

        private void PetProcessForm_MouseUp(object sender, MouseEventArgs e)
        {
            MoveStart = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void imageStatus_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(sender as Control, new Point());
        }

        private SettingStorage GetSetting()
        {
            SettingStorage setting = null;
            var xmlSerializer = new XmlSerializer(typeof(SettingStorage));
            if (File.Exists(SettingFileName))
            {
                using (FileStream fs = new FileStream(SettingFileName, FileMode.OpenOrCreate))
                {
                    setting = xmlSerializer.Deserialize(fs) as SettingStorage;
                }
            }
            if (setting == null)
            {
                setting = new SettingStorage();
                setting.Consoles.Add(new PetRecord() { Name = "internet", Command = "ping /t 8.8.8.8", Pattern = "ping", Autostart = true });
            }
            return setting;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSetting();
            Process.Start("notepad", SettingFileName);
        }

        private void позицияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingStorage setting = GetSetting();

            setting.WinPosX = this.Left; 
            setting.WinPosY = this.Top;
            setting.WinTopMost = this.TopMost;

            var xmlSerializer = new XmlSerializer(typeof(SettingStorage));
            if (File.Exists(SettingFileName)) File.Delete(SettingFileName);
            using (FileStream fs = new FileStream(SettingFileName, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, setting);
            }
        }

    }
}
