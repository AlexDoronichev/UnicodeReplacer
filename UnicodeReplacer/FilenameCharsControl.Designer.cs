
namespace UnicodeReplacer
{
    partial class FilenameCharsControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSaveSelFilenameToFile = new System.Windows.Forms.Button();
            this.btnReturnToOrigFilename = new System.Windows.Forms.Button();
            this.btnClearUserInputRow = new System.Windows.Forms.Button();
            this.btnReplaceCharsInSelFilename = new System.Windows.Forms.Button();
            this.textBoxNewName = new System.Windows.Forms.TextBox();
            this.textBoxOrigName = new System.Windows.Forms.TextBox();
            this.btnSaveToReplaceCharsTable = new System.Windows.Forms.Button();
            this.btnSaveToReplaceFilenamesTable = new System.Windows.Forms.Button();
            this.labelNewName = new System.Windows.Forms.Label();
            this.btnCopyReplaceChars = new System.Windows.Forms.Button();
            this.labelOriginalName = new System.Windows.Forms.Label();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.checkBoxShowCode = new System.Windows.Forms.CheckBox();
            this.checkBoxOnlyUnique = new System.Windows.Forms.CheckBox();
            this.checkBoxOnlyUnicode = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSaveSelFilenameToFile
            // 
            this.btnSaveSelFilenameToFile.Enabled = false;
            this.btnSaveSelFilenameToFile.Image = global::UnicodeReplacer.Properties.Resources.save_to_file;
            this.btnSaveSelFilenameToFile.Location = new System.Drawing.Point(52, 99);
            this.btnSaveSelFilenameToFile.Name = "btnSaveSelFilenameToFile";
            this.btnSaveSelFilenameToFile.Size = new System.Drawing.Size(32, 32);
            this.btnSaveSelFilenameToFile.TabIndex = 35;
            this.btnSaveSelFilenameToFile.UseVisualStyleBackColor = true;
            this.btnSaveSelFilenameToFile.Click += new System.EventHandler(this.btnSaveSelFilenameToFile_Click);
            // 
            // btnReturnToOrigFilename
            // 
            this.btnReturnToOrigFilename.Enabled = false;
            this.btnReturnToOrigFilename.Image = global::UnicodeReplacer.Properties.Resources.decline;
            this.btnReturnToOrigFilename.Location = new System.Drawing.Point(90, 99);
            this.btnReturnToOrigFilename.Name = "btnReturnToOrigFilename";
            this.btnReturnToOrigFilename.Size = new System.Drawing.Size(32, 32);
            this.btnReturnToOrigFilename.TabIndex = 34;
            this.btnReturnToOrigFilename.UseVisualStyleBackColor = true;
            this.btnReturnToOrigFilename.Click += new System.EventHandler(this.btnReturnToOrigFilename_Click);
            // 
            // btnClearUserInputRow
            // 
            this.btnClearUserInputRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearUserInputRow.Enabled = false;
            this.btnClearUserInputRow.Image = global::UnicodeReplacer.Properties.Resources.decline;
            this.btnClearUserInputRow.Location = new System.Drawing.Point(257, 107);
            this.btnClearUserInputRow.Name = "btnClearUserInputRow";
            this.btnClearUserInputRow.Size = new System.Drawing.Size(32, 32);
            this.btnClearUserInputRow.TabIndex = 33;
            this.btnClearUserInputRow.UseVisualStyleBackColor = true;
            this.btnClearUserInputRow.Click += new System.EventHandler(this.btnClearUserInputRow_Click);
            // 
            // btnReplaceCharsInSelFilename
            // 
            this.btnReplaceCharsInSelFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReplaceCharsInSelFilename.Enabled = false;
            this.btnReplaceCharsInSelFilename.Image = global::UnicodeReplacer.Properties.Resources.accept;
            this.btnReplaceCharsInSelFilename.Location = new System.Drawing.Point(219, 107);
            this.btnReplaceCharsInSelFilename.Name = "btnReplaceCharsInSelFilename";
            this.btnReplaceCharsInSelFilename.Size = new System.Drawing.Size(32, 32);
            this.btnReplaceCharsInSelFilename.TabIndex = 32;
            this.btnReplaceCharsInSelFilename.UseVisualStyleBackColor = true;
            this.btnReplaceCharsInSelFilename.Click += new System.EventHandler(this.btnReplaceCharsInSelFilename_Click);
            // 
            // textBoxNewName
            // 
            this.textBoxNewName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxNewName.Location = new System.Drawing.Point(78, 64);
            this.textBoxNewName.Name = "textBoxNewName";
            this.textBoxNewName.ReadOnly = true;
            this.textBoxNewName.Size = new System.Drawing.Size(118, 25);
            this.textBoxNewName.TabIndex = 31;
            // 
            // textBoxOrigName
            // 
            this.textBoxOrigName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxOrigName.Location = new System.Drawing.Point(78, 25);
            this.textBoxOrigName.Name = "textBoxOrigName";
            this.textBoxOrigName.ReadOnly = true;
            this.textBoxOrigName.Size = new System.Drawing.Size(118, 25);
            this.textBoxOrigName.TabIndex = 30;
            // 
            // btnSaveToReplaceCharsTable
            // 
            this.btnSaveToReplaceCharsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveToReplaceCharsTable.Enabled = false;
            this.btnSaveToReplaceCharsTable.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveToReplaceCharsTable.Image = global::UnicodeReplacer.Properties.Resources.save_right;
            this.btnSaveToReplaceCharsTable.Location = new System.Drawing.Point(696, 72);
            this.btnSaveToReplaceCharsTable.Name = "btnSaveToReplaceCharsTable";
            this.btnSaveToReplaceCharsTable.Size = new System.Drawing.Size(39, 27);
            this.btnSaveToReplaceCharsTable.TabIndex = 29;
            this.btnSaveToReplaceCharsTable.UseVisualStyleBackColor = true;
            this.btnSaveToReplaceCharsTable.Click += new System.EventHandler(this.btnSaveToReplaceCharsTable_Click);
            // 
            // btnSaveToReplaceFilenamesTable
            // 
            this.btnSaveToReplaceFilenamesTable.Enabled = false;
            this.btnSaveToReplaceFilenamesTable.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSaveToReplaceFilenamesTable.Image = global::UnicodeReplacer.Properties.Resources.save_left;
            this.btnSaveToReplaceFilenamesTable.Location = new System.Drawing.Point(7, 99);
            this.btnSaveToReplaceFilenamesTable.Name = "btnSaveToReplaceFilenamesTable";
            this.btnSaveToReplaceFilenamesTable.Size = new System.Drawing.Size(39, 32);
            this.btnSaveToReplaceFilenamesTable.TabIndex = 27;
            this.btnSaveToReplaceFilenamesTable.UseVisualStyleBackColor = true;
            this.btnSaveToReplaceFilenamesTable.Click += new System.EventHandler(this.btnSaveToReplaceFilenamesTable_Click);
            // 
            // labelNewName
            // 
            this.labelNewName.Location = new System.Drawing.Point(7, 60);
            this.labelNewName.Name = "labelNewName";
            this.labelNewName.Size = new System.Drawing.Size(65, 32);
            this.labelNewName.TabIndex = 25;
            this.labelNewName.Text = "Новое название:";
            // 
            // btnCopyReplaceChars
            // 
            this.btnCopyReplaceChars.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyReplaceChars.Enabled = false;
            this.btnCopyReplaceChars.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCopyReplaceChars.Image = global::UnicodeReplacer.Properties.Resources.copy_down;
            this.btnCopyReplaceChars.Location = new System.Drawing.Point(696, 39);
            this.btnCopyReplaceChars.Name = "btnCopyReplaceChars";
            this.btnCopyReplaceChars.Size = new System.Drawing.Size(39, 27);
            this.btnCopyReplaceChars.TabIndex = 28;
            this.btnCopyReplaceChars.UseVisualStyleBackColor = true;
            this.btnCopyReplaceChars.Click += new System.EventHandler(this.btnCopyReplaceChars_Click);
            // 
            // labelOriginalName
            // 
            this.labelOriginalName.Location = new System.Drawing.Point(7, 21);
            this.labelOriginalName.Name = "labelOriginalName";
            this.labelOriginalName.Size = new System.Drawing.Size(65, 32);
            this.labelOriginalName.TabIndex = 26;
            this.labelOriginalName.Text = "Исходное название:";
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGrid.ColumnHeadersVisible = false;
            this.dataGrid.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid.Location = new System.Drawing.Point(219, 18);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGrid.RowTemplate.Height = 25;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGrid.Size = new System.Drawing.Size(468, 80);
            this.dataGrid.TabIndex = 21;
            // 
            // checkBoxShowCode
            // 
            this.checkBoxShowCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowCode.AutoSize = true;
            this.checkBoxShowCode.Location = new System.Drawing.Point(373, 118);
            this.checkBoxShowCode.Name = "checkBoxShowCode";
            this.checkBoxShowCode.Size = new System.Drawing.Size(98, 19);
            this.checkBoxShowCode.TabIndex = 24;
            this.checkBoxShowCode.Text = "Показать код";
            this.checkBoxShowCode.UseVisualStyleBackColor = true;
            this.checkBoxShowCode.CheckedChanged += new System.EventHandler(this.checkBoxShowCode_CheckedChanged);
            // 
            // checkBoxOnlyUnique
            // 
            this.checkBoxOnlyUnique.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOnlyUnique.AutoSize = true;
            this.checkBoxOnlyUnique.Checked = true;
            this.checkBoxOnlyUnique.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnlyUnique.Location = new System.Drawing.Point(477, 118);
            this.checkBoxOnlyUnique.Name = "checkBoxOnlyUnique";
            this.checkBoxOnlyUnique.Size = new System.Drawing.Size(136, 19);
            this.checkBoxOnlyUnique.TabIndex = 22;
            this.checkBoxOnlyUnique.Text = "Только уникальные";
            this.checkBoxOnlyUnique.UseVisualStyleBackColor = true;
            this.checkBoxOnlyUnique.CheckedChanged += new System.EventHandler(this.checkBoxOnlyUnique_CheckedChanged);
            // 
            // checkBoxOnlyUnicode
            // 
            this.checkBoxOnlyUnicode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOnlyUnicode.AutoSize = true;
            this.checkBoxOnlyUnicode.Checked = true;
            this.checkBoxOnlyUnicode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnlyUnicode.Location = new System.Drawing.Point(619, 118);
            this.checkBoxOnlyUnicode.Name = "checkBoxOnlyUnicode";
            this.checkBoxOnlyUnicode.Size = new System.Drawing.Size(113, 19);
            this.checkBoxOnlyUnicode.TabIndex = 23;
            this.checkBoxOnlyUnicode.Text = "Только Unicode";
            this.checkBoxOnlyUnicode.UseVisualStyleBackColor = true;
            this.checkBoxOnlyUnicode.CheckedChanged += new System.EventHandler(this.checkBoxOnlyUnicode_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.labelOriginalName);
            this.groupBox1.Controls.Add(this.labelNewName);
            this.groupBox1.Controls.Add(this.btnSaveSelFilenameToFile);
            this.groupBox1.Controls.Add(this.btnSaveToReplaceFilenamesTable);
            this.groupBox1.Controls.Add(this.btnReturnToOrigFilename);
            this.groupBox1.Controls.Add(this.textBoxOrigName);
            this.groupBox1.Controls.Add(this.textBoxNewName);
            this.groupBox1.Location = new System.Drawing.Point(0, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 132);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Замена символов в названии";
            // 
            // FilenameCharsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClearUserInputRow);
            this.Controls.Add(this.btnReplaceCharsInSelFilename);
            this.Controls.Add(this.btnSaveToReplaceCharsTable);
            this.Controls.Add(this.btnCopyReplaceChars);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.checkBoxShowCode);
            this.Controls.Add(this.checkBoxOnlyUnique);
            this.Controls.Add(this.checkBoxOnlyUnicode);
            this.MinimumSize = new System.Drawing.Size(660, 0);
            this.Name = "FilenameCharsControl";
            this.Size = new System.Drawing.Size(735, 145);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGrid; 
        private System.Windows.Forms.Button btnSaveSelFilenameToFile;
        private System.Windows.Forms.Button btnReturnToOrigFilename;
        private System.Windows.Forms.Button btnClearUserInputRow;
        private System.Windows.Forms.Button btnReplaceCharsInSelFilename;
        private System.Windows.Forms.TextBox textBoxNewName;
        private System.Windows.Forms.TextBox textBoxOrigName;
        private System.Windows.Forms.Button btnSaveToReplaceCharsTable;
        private System.Windows.Forms.Button btnSaveToReplaceFilenamesTable;
        private System.Windows.Forms.Label labelNewName;
        private System.Windows.Forms.Button btnCopyReplaceChars;
        private System.Windows.Forms.Label labelOriginalName;
        private System.Windows.Forms.CheckBox checkBoxShowCode;
        private System.Windows.Forms.CheckBox checkBoxOnlyUnique;
        private System.Windows.Forms.CheckBox checkBoxOnlyUnicode;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
