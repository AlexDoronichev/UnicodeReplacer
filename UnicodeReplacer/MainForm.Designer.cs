
namespace UnicodeReplacer
{
    partial class MainForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpenReplaceCharsTable = new System.Windows.Forms.Button();
            this.btnOpenReplaceFilenamesTable = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.filenameChars = new UnicodeReplacer.FilenameCharsControl();
            this.fileParams = new UnicodeReplacer.FileParamsControl();
            this.replaceChars = new UnicodeReplacer.ReplaceCharsControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.replaceFilenames = new UnicodeReplacer.ReplaceFilenamesControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpenReplaceCharsTable
            // 
            this.btnOpenReplaceCharsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenReplaceCharsTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenReplaceCharsTable.Location = new System.Drawing.Point(638, 383);
            this.btnOpenReplaceCharsTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOpenReplaceCharsTable.Name = "btnOpenReplaceCharsTable";
            this.btnOpenReplaceCharsTable.Size = new System.Drawing.Size(129, 32);
            this.btnOpenReplaceCharsTable.TabIndex = 6;
            this.btnOpenReplaceCharsTable.Text = "Список символов";
            this.btnOpenReplaceCharsTable.UseVisualStyleBackColor = true;
            this.btnOpenReplaceCharsTable.Click += new System.EventHandler(this.btnReplaceCharsTable_Click);
            // 
            // btnOpenReplaceFilenamesTable
            // 
            this.btnOpenReplaceFilenamesTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenReplaceFilenamesTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOpenReplaceFilenamesTable.Location = new System.Drawing.Point(3, 382);
            this.btnOpenReplaceFilenamesTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOpenReplaceFilenamesTable.Name = "btnOpenReplaceFilenamesTable";
            this.btnOpenReplaceFilenamesTable.Size = new System.Drawing.Size(135, 32);
            this.btnOpenReplaceFilenamesTable.TabIndex = 5;
            this.btnOpenReplaceFilenamesTable.Text = "Список названий";
            this.btnOpenReplaceFilenamesTable.UseVisualStyleBackColor = true;
            this.btnOpenReplaceFilenamesTable.Click += new System.EventHandler(this.btnReplaceFilenamesTable_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer2.Panel1.Controls.Add(this.filenameChars);
            this.splitContainer2.Panel1.Controls.Add(this.btnOpenReplaceFilenamesTable);
            this.splitContainer2.Panel1.Controls.Add(this.btnOpenReplaceCharsTable);
            this.splitContainer2.Panel1.Controls.Add(this.fileParams);
            this.splitContainer2.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer2.Panel2.Controls.Add(this.replaceChars);
            this.splitContainer2.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer2.Size = new System.Drawing.Size(959, 562);
            this.splitContainer2.SplitterDistance = 772;
            this.splitContainer2.TabIndex = 6;
            // 
            // filenameChars
            // 
            this.filenameChars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filenameChars.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filenameChars.Location = new System.Drawing.Point(0, 417);
            this.filenameChars.MinimumSize = new System.Drawing.Size(660, 2);
            this.filenameChars.Name = "filenameChars";
            this.filenameChars.Size = new System.Drawing.Size(772, 145);
            this.filenameChars.TabIndex = 8;
            // 
            // fileParams
            // 
            this.fileParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileParams.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileParams.EntryChangesCount = 0;
            this.fileParams.Location = new System.Drawing.Point(0, 0);
            this.fileParams.Name = "fileParams";
            this.fileParams.Size = new System.Drawing.Size(772, 418);
            this.fileParams.TabIndex = 7;
            // 
            // replaceChars
            // 
            this.replaceChars.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.replaceChars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.replaceChars.Location = new System.Drawing.Point(0, 0);
            this.replaceChars.MinimumSize = new System.Drawing.Size(150, 500);
            this.replaceChars.Name = "replaceChars";
            this.replaceChars.Size = new System.Drawing.Size(183, 562);
            this.replaceChars.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer1.Panel1.Controls.Add(this.replaceFilenames);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Size = new System.Drawing.Size(1184, 562);
            this.splitContainer1.SplitterDistance = 221;
            this.splitContainer1.TabIndex = 7;
            // 
            // replaceFilenames
            // 
            this.replaceFilenames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.replaceFilenames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.replaceFilenames.Location = new System.Drawing.Point(0, 0);
            this.replaceFilenames.MinimumSize = new System.Drawing.Size(150, 500);
            this.replaceFilenames.Name = "replaceFilenames";
            this.replaceFilenames.Size = new System.Drawing.Size(221, 562);
            this.replaceFilenames.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 562);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "UnicodeCharsFinder";
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnOpenReplaceFilenamesTable;
        private System.Windows.Forms.Button btnOpenReplaceCharsTable;
        private FilenameCharsControl filenameChars;
        private FileParamsControl fileParams;
        private ReplaceCharsControl replaceChars;
        private ReplaceFilenamesControl replaceFilenames;
    }
}

