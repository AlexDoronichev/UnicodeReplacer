
namespace UnicodeReplacer
{
    partial class FileParamsControl
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
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.btnSaveFilenamesToFiles = new System.Windows.Forms.Button();
            this.btnReturnToOrigFilenames = new System.Windows.Forms.Button();
            this.btnCopyReplaceFilenames = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnChooseFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnChooseFolder.Location = new System.Drawing.Point(374, 315);
            this.btnChooseFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(138, 32);
            this.btnChooseFolder.TabIndex = 7;
            this.btnChooseFolder.Text = "Выбрать файлы";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.dataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGrid.Location = new System.Drawing.Point(0, 0);
            this.dataGrid.MultiSelect = false;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.RowTemplate.Height = 25;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(700, 309);
            this.dataGrid.TabIndex = 8;
            // 
            // btnSaveFilenamesToFiles
            // 
            this.btnSaveFilenamesToFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveFilenamesToFiles.Enabled = false;
            this.btnSaveFilenamesToFiles.Image = global::UnicodeReplacer.Properties.Resources.save_to_file;
            this.btnSaveFilenamesToFiles.Location = new System.Drawing.Point(226, 315);
            this.btnSaveFilenamesToFiles.Name = "btnSaveFilenamesToFiles";
            this.btnSaveFilenamesToFiles.Size = new System.Drawing.Size(32, 32);
            this.btnSaveFilenamesToFiles.TabIndex = 9;
            this.btnSaveFilenamesToFiles.UseVisualStyleBackColor = true;
            this.btnSaveFilenamesToFiles.Click += new System.EventHandler(this.btnSaveFilenamesToFiles_Click);
            // 
            // btnReturnToOrigFilenames
            // 
            this.btnReturnToOrigFilenames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReturnToOrigFilenames.Enabled = false;
            this.btnReturnToOrigFilenames.Image = global::UnicodeReplacer.Properties.Resources.decline;
            this.btnReturnToOrigFilenames.Location = new System.Drawing.Point(302, 315);
            this.btnReturnToOrigFilenames.Name = "btnReturnToOrigFilenames";
            this.btnReturnToOrigFilenames.Size = new System.Drawing.Size(32, 32);
            this.btnReturnToOrigFilenames.TabIndex = 10;
            this.btnReturnToOrigFilenames.UseVisualStyleBackColor = true;
            this.btnReturnToOrigFilenames.Click += new System.EventHandler(this.btnReturnToOrigFilenames_Click);
            // 
            // btnCopyReplaceFilenames
            // 
            this.btnCopyReplaceFilenames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopyReplaceFilenames.Enabled = false;
            this.btnCopyReplaceFilenames.Image = global::UnicodeReplacer.Properties.Resources.сopy_left;
            this.btnCopyReplaceFilenames.Location = new System.Drawing.Point(264, 315);
            this.btnCopyReplaceFilenames.Name = "btnCopyReplaceFilenames";
            this.btnCopyReplaceFilenames.Size = new System.Drawing.Size(32, 32);
            this.btnCopyReplaceFilenames.TabIndex = 11;
            this.btnCopyReplaceFilenames.UseVisualStyleBackColor = true;
            this.btnCopyReplaceFilenames.Click += new System.EventHandler(this.btnCopyReplaceFilenames_Click);
            // 
            // FileParamsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCopyReplaceFilenames);
            this.Controls.Add(this.btnReturnToOrigFilenames);
            this.Controls.Add(this.btnSaveFilenamesToFiles);
            this.Controls.Add(this.btnChooseFolder);
            this.Controls.Add(this.dataGrid);
            this.Name = "FileParamsControl";
            this.Size = new System.Drawing.Size(700, 350);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGrid; 
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.Button btnSaveFilenamesToFiles;
        private System.Windows.Forms.Button btnReturnToOrigFilenames;
        private System.Windows.Forms.Button btnCopyReplaceFilenames;
    }
}
