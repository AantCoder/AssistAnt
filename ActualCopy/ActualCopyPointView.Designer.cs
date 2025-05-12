namespace AssistAnt.ActualCopy
{
    partial class ActualCopyPointView
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
            this.panelItem = new System.Windows.Forms.Panel();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonExcludeHelp = new System.Windows.Forms.Button();
            this.buttonToSelect = new System.Windows.Forms.Button();
            this.buttonFromSelect = new System.Windows.Forms.Button();
            this.buttonDel = new System.Windows.Forms.Button();
            this.textBoxExclude = new System.Windows.Forms.TextBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.labelExclude = new System.Windows.Forms.Label();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.labelLog = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelFrom = new System.Windows.Forms.Label();
            this.checkBoxActive = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panelItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelItem
            // 
            this.panelItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelItem.Controls.Add(this.buttonAdd);
            this.panelItem.Controls.Add(this.buttonExcludeHelp);
            this.panelItem.Controls.Add(this.buttonToSelect);
            this.panelItem.Controls.Add(this.buttonFromSelect);
            this.panelItem.Controls.Add(this.buttonDel);
            this.panelItem.Controls.Add(this.textBoxExclude);
            this.panelItem.Controls.Add(this.textBoxLog);
            this.panelItem.Controls.Add(this.textBoxTo);
            this.panelItem.Controls.Add(this.labelExclude);
            this.panelItem.Controls.Add(this.textBoxFrom);
            this.panelItem.Controls.Add(this.labelLog);
            this.panelItem.Controls.Add(this.labelTo);
            this.panelItem.Controls.Add(this.labelFrom);
            this.panelItem.Controls.Add(this.checkBoxActive);
            this.panelItem.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelItem.Location = new System.Drawing.Point(0, 0);
            this.panelItem.Name = "panelItem";
            this.panelItem.Size = new System.Drawing.Size(764, 167);
            this.panelItem.TabIndex = 2;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(15, 128);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(100, 30);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonExcludeHelp
            // 
            this.buttonExcludeHelp.Location = new System.Drawing.Point(252, 69);
            this.buttonExcludeHelp.Name = "buttonExcludeHelp";
            this.buttonExcludeHelp.Size = new System.Drawing.Size(31, 25);
            this.buttonExcludeHelp.TabIndex = 1;
            this.buttonExcludeHelp.Text = "?";
            this.buttonExcludeHelp.UseVisualStyleBackColor = true;
            this.buttonExcludeHelp.Click += new System.EventHandler(this.buttonExcludeHelp_Click);
            // 
            // buttonToSelect
            // 
            this.buttonToSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToSelect.Location = new System.Drawing.Point(719, 38);
            this.buttonToSelect.Name = "buttonToSelect";
            this.buttonToSelect.Size = new System.Drawing.Size(31, 24);
            this.buttonToSelect.TabIndex = 1;
            this.buttonToSelect.Text = "...";
            this.buttonToSelect.UseVisualStyleBackColor = true;
            this.buttonToSelect.Click += new System.EventHandler(this.buttonToSelect_Click);
            // 
            // buttonFromSelect
            // 
            this.buttonFromSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFromSelect.Location = new System.Drawing.Point(719, 10);
            this.buttonFromSelect.Name = "buttonFromSelect";
            this.buttonFromSelect.Size = new System.Drawing.Size(31, 24);
            this.buttonFromSelect.TabIndex = 1;
            this.buttonFromSelect.Text = "...";
            this.buttonFromSelect.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonFromSelect.UseVisualStyleBackColor = true;
            this.buttonFromSelect.Click += new System.EventHandler(this.buttonFromSelect_Click);
            // 
            // buttonDel
            // 
            this.buttonDel.Location = new System.Drawing.Point(15, 93);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(100, 30);
            this.buttonDel.TabIndex = 1;
            this.buttonDel.Text = "Удалить";
            this.buttonDel.UseVisualStyleBackColor = true;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // textBoxExclude
            // 
            this.textBoxExclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExclude.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "ExcludesText", true));
            this.textBoxExclude.Location = new System.Drawing.Point(289, 66);
            this.textBoxExclude.Multiline = true;
            this.textBoxExclude.Name = "textBoxExclude";
            this.textBoxExclude.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxExclude.Size = new System.Drawing.Size(461, 43);
            this.textBoxExclude.TabIndex = 2;
            this.textBoxExclude.WordWrap = false;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "LastLogMessage", true));
            this.textBoxLog.Location = new System.Drawing.Point(289, 115);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.Size = new System.Drawing.Size(461, 43);
            this.textBoxLog.TabIndex = 2;
            // 
            // textBoxTo
            // 
            this.textBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "Target", true));
            this.textBoxTo.Location = new System.Drawing.Point(204, 38);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(509, 22);
            this.textBoxTo.TabIndex = 2;
            // 
            // labelExclude
            // 
            this.labelExclude.AutoSize = true;
            this.labelExclude.Location = new System.Drawing.Point(121, 69);
            this.labelExclude.Name = "labelExclude";
            this.labelExclude.Size = new System.Drawing.Size(91, 16);
            this.labelExclude.TabIndex = 1;
            this.labelExclude.Text = "Исключения:";
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFrom.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "Source", true));
            this.textBoxFrom.Location = new System.Drawing.Point(204, 10);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(509, 22);
            this.textBoxFrom.TabIndex = 2;
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(121, 118);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(130, 16);
            this.labelLog.TabIndex = 1;
            this.labelLog.Text = "Последний статус:";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(121, 41);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(39, 16);
            this.labelTo.TabIndex = 1;
            this.labelTo.Text = "Куда";
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(121, 13);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(55, 16);
            this.labelFrom.TabIndex = 1;
            this.labelFrom.Text = "Откуда";
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.AutoSize = true;
            this.checkBoxActive.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource1, "Active", true));
            this.checkBoxActive.Location = new System.Drawing.Point(15, 12);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new System.Drawing.Size(84, 20);
            this.checkBoxActive.TabIndex = 0;
            this.checkBoxActive.Text = "Активно";
            this.checkBoxActive.UseVisualStyleBackColor = true;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(AssistAnt.ActualCopy.ACPoint);
            // 
            // ActualCopyPointView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelItem);
            this.Name = "ActualCopyPointView";
            this.Size = new System.Drawing.Size(764, 167);
            this.panelItem.ResumeLayout(false);
            this.panelItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelItem;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonExcludeHelp;
        private System.Windows.Forms.Button buttonToSelect;
        private System.Windows.Forms.Button buttonFromSelect;
        private System.Windows.Forms.Button buttonDel;
        private System.Windows.Forms.TextBox textBoxExclude;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.Label labelExclude;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.CheckBox checkBoxActive;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
