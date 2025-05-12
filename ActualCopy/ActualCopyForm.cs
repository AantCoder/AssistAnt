using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistAnt.ActualCopy
{
    public partial class ActualCopyForm : Form
    {
        public ActualCopyService Service;

        public ActualCopyForm(ActualCopyService service)
        {
            this.Service = service;
            InitializeComponent();
            Service.GetSetting();
            Init();
        }

        public void Init()
        {
            panelDoc.Controls.Clear();
            for (int i = 0; i < Service.Setting.Points.Count; i++)
            {
                var point = Service.Setting.Points[i];

                var actualCopyPointView1 = new ActualCopyPointView(point);
                actualCopyPointView1.Dock = System.Windows.Forms.DockStyle.Top;
                actualCopyPointView1.Location = new System.Drawing.Point(0, 0);
                actualCopyPointView1.Name = "actualCopyPointView" + i.ToString();
                actualCopyPointView1.Size = new System.Drawing.Size(764, 167);
                actualCopyPointView1.TabIndex = 0;

                actualCopyPointView1.Add = AddPoint;
                actualCopyPointView1.Delete = DeletePoint;

                if ((Service.Setting.Points.Count - i) % 2 != 0)
                {
                    actualCopyPointView1.BackColor = Color.FromArgb(
                        (int)(actualCopyPointView1.BackColor.R * 0.9)
                        , (int)(actualCopyPointView1.BackColor.G * 0.9)
                        , (int)(actualCopyPointView1.BackColor.B * 0.9));
                }

                panelDoc.Controls.Add(actualCopyPointView1);
            }
            labelLastRun.Text = Service.Setting.LastRunDate == DateTime.MinValue ? "нет" : Service.Setting.LastRunDate.ToString("u").Replace("Z", "");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddPoint(null);
        }

        private void AddPoint(ActualCopyPointView actualCopyPointView)
        {
            var index = actualCopyPointView == null ? -1 : panelDoc.Controls.IndexOf(actualCopyPointView);
            if (index < 0) index = Service.Setting.Points.Count;

            var point = new ACPoint()
            {
                Active = true,
                Source = "",
                Target = "",
                Excludes = new List<string>() { "" },
            };

            Service.Setting.Points.Insert(index, point);
            Init();
        }

        private void DeletePoint(ActualCopyPointView actualCopyPointView)
        {
            var index = actualCopyPointView == null ? -1 : panelDoc.Controls.IndexOf(actualCopyPointView);
            if (index < 0) return;

            Service.Setting.Points.RemoveAt(index);
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Service.SetSetting();
            var error = Service.Synchronization();
            Init();
            if (error == null) MessageBox.Show("Завершено");
            else MessageBox.Show(error, "Завершено с ошибкой");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Service.SetSetting();
            MessageBox.Show("Сохранено" + Environment.NewLine + Environment.NewLine + Service.SettingFileName);
        }
    }
}
