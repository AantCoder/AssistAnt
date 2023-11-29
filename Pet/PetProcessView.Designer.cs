namespace AssistAnt
{
    partial class PetProcessView
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageStatus = new System.Windows.Forms.PictureBox();
            this.textName = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перезапуститьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.завершитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.textStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imageStatus)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // imageStatus
            // 
            this.imageStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageStatus.ErrorImage = null;
            this.imageStatus.InitialImage = null;
            this.imageStatus.Location = new System.Drawing.Point(0, 0);
            this.imageStatus.Name = "imageStatus";
            this.imageStatus.Size = new System.Drawing.Size(28, 28);
            this.imageStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageStatus.TabIndex = 0;
            this.imageStatus.TabStop = false;
            // 
            // textName
            // 
            this.textName.AutoSize = true;
            this.textName.Dock = System.Windows.Forms.DockStyle.Left;
            this.textName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textName.Location = new System.Drawing.Point(28, 0);
            this.textName.Name = "textName";
            this.textName.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.textName.Size = new System.Drawing.Size(30, 21);
            this.textName.TabIndex = 2;
            this.textName.Text = "Slot";
            this.textName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.textName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textStatus_MouseDown);
            this.textName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textStatus_MouseMove);
            this.textName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textStatus_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.перезапуститьToolStripMenuItem,
            this.завершитьToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 76);
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.открытьToolStripMenuItem.Text = "Открыть лог";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // перезапуститьToolStripMenuItem
            // 
            this.перезапуститьToolStripMenuItem.Name = "перезапуститьToolStripMenuItem";
            this.перезапуститьToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.перезапуститьToolStripMenuItem.Text = "Перезапустить";
            this.перезапуститьToolStripMenuItem.Click += new System.EventHandler(this.перезапуститьToolStripMenuItem_Click);
            // 
            // завершитьToolStripMenuItem
            // 
            this.завершитьToolStripMenuItem.Name = "завершитьToolStripMenuItem";
            this.завершитьToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.завершитьToolStripMenuItem.Text = "Завершить";
            this.завершитьToolStripMenuItem.Click += new System.EventHandler(this.завершитьToolStripMenuItem_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.Image = global::AssistAnt.Properties.Resources.imgSetting;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(188, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 28);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // textStatus
            // 
            this.textStatus.AutoEllipsis = true;
            this.textStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textStatus.Location = new System.Drawing.Point(58, 0);
            this.textStatus.Name = "textStatus";
            this.textStatus.Size = new System.Drawing.Size(130, 28);
            this.textStatus.TabIndex = 4;
            this.textStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.textStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textStatus_MouseDown);
            this.textStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textStatus_MouseMove);
            this.textStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textStatus_MouseUp);
            // 
            // PetProcessView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.textStatus);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.imageStatus);
            this.Name = "PetProcessView";
            this.Size = new System.Drawing.Size(213, 28);
            ((System.ComponentModel.ISupportInitialize)(this.imageStatus)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imageStatus;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label textName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem завершитьToolStripMenuItem;
        private System.Windows.Forms.Label textStatus;
        private System.Windows.Forms.ToolStripMenuItem перезапуститьToolStripMenuItem;
    }
}
