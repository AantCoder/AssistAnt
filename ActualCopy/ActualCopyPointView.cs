using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistAnt.ActualCopy
{
    public partial class ActualCopyPointView : UserControl
    {
        public ACPoint Point;
        public Action<ActualCopyPointView> Delete;
        public Action<ActualCopyPointView> Add;

        public ActualCopyPointView(ACPoint point)
        {
            Point = point;
            InitializeComponent(); 
            bindingSource1.DataSource = Point;
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            Delete(this);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Add(this);
        }

        private void buttonExcludeHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Перечислите файлы или папки, которые не будут синхронизироваться. 
Если содержит \ в любом месте, то строка ищется в имени пути относительно Source (строка пути начинается с символа \), иначе поиск по имени файла с поддержкой *.
Например для Source = ""C:\Source"":
""\Hlam"" - игнорирует папку C:\Source\Hlam
""Hlam\"" - игнорирует все папки Hlam на любой вложенности
""*.zip"" - игнорирует файлы *.zip на любой вложенности
""Lol"" - игнорирует файлы Lol без расширения на любой вложенности"
                , "Исключения");
        }

        private void buttonFromSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                textBoxFrom.Text = Point.Source = folderBrowserDialog1.SelectedPath;
            }
        }

        private void buttonToSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                textBoxTo.Text = Point.Target = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
