namespace AssistAnt
{
    partial class PetProcessForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PetProcessForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxSettings = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageStatus = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageStatus)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBoxSettings);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.imageStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 28);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PetProcessForm_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PetProcessForm_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PetProcessForm_MouseUp);
            // 
            // pictureBoxSettings
            // 
            this.pictureBoxSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxSettings.ErrorImage = null;
            this.pictureBoxSettings.Image = global::AssistAnt.Properties.Resources.imgSetting;
            this.pictureBoxSettings.InitialImage = null;
            this.pictureBoxSettings.Location = new System.Drawing.Point(28, 0);
            this.pictureBoxSettings.Name = "pictureBoxSettings";
            this.pictureBoxSettings.Size = new System.Drawing.Size(25, 28);
            this.pictureBoxSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxSettings.TabIndex = 3;
            this.pictureBoxSettings.TabStop = false;
            this.pictureBoxSettings.Click += new System.EventHandler(this.pictureBoxSettings_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::AssistAnt.Properties.Resources.imgX;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(222, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 28);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // imageStatus
            // 
            this.imageStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageStatus.ErrorImage = null;
            this.imageStatus.Image = global::AssistAnt.Properties.Resources.imgPin;
            this.imageStatus.InitialImage = null;
            this.imageStatus.Location = new System.Drawing.Point(0, 0);
            this.imageStatus.Name = "imageStatus";
            this.imageStatus.Size = new System.Drawing.Size(28, 28);
            this.imageStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageStatus.TabIndex = 1;
            this.imageStatus.TabStop = false;
            this.imageStatus.Click += new System.EventHandler(this.imageStatus_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(221, 52);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(220, 24);
            this.toolStripMenuItem1.Text = "Открыть настройки";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(220, 24);
            this.toolStripMenuItem2.Text = "Сохранить позицию";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.позицияToolStripMenuItem_Click);
            // 
            // PetProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(250, 28);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PetProcessForm";
            this.ShowInTaskbar = false;
            this.Text = "Consoles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PetProcessForm_FormClosing);
            this.Load += new System.EventHandler(this.PetProcessForm_Load);
            this.VisibleChanged += new System.EventHandler(this.PetProcessForm_VisibleChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PetProcessForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PetProcessForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PetProcessForm_MouseUp);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageStatus)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox imageStatus;
        private System.Windows.Forms.PictureBox pictureBoxSettings;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}