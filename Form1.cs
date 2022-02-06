using GTranslatorAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace AssistAnt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = new Tesseract("eng").GetTextFromClipboardImage();
            button1_Click_1(null, null);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var tr = new TranslatorText("eng", "rus");
            textBox2.Text = tr.Translate(textBox1.Text);
        }

    }
}
