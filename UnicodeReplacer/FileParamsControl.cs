using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace UnicodeReplacer
{
    public partial class FileParamsControl : UserControl
    {
        string progName = "UnicodeReplacer";

        DataSet dataSet = new DataSet("FileStore");
        DataTable dataTable = new DataTable("FileParams");

        FilenameCharsControl filenameChars;
        ReplaceCharsControl replaceChars;
        ReplaceFilenamesControl replaceFilenames;

        bool usedOtherFormats = false;

        int entryChangesCount = 0;

        public int EntryChangesCount
        {
            get { return entryChangesCount; }
            set
            {
                if (value != entryChangesCount)
                {
                    entryChangesCount = value;
                    if (EntryChangesCountValueChanged != null)
                        EntryChangesCountValueChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler EntryChangesCountValueChanged;

        private ToolTip toolTipSaveFilenamesToFiles = new ToolTip();
        private ToolTip toolTipCopyReplaceFilenames = new ToolTip();
        private ToolTip toolTipReturnToOrigFilenames = new ToolTip();

        public FileParamsControl()
        {
            InitializeComponent();

            // Data sources
            dataSet.Tables.Add(dataTable);
            dataGrid.DataSource = dataSet.Tables[0];

            // Columns
            dataTable.Columns.Add(" ", typeof(byte[]));
            dataTable.Columns.Add("Имя файла", typeof(string));
            dataTable.Columns.Add("Юникод-символы", typeof(string));
            dataTable.Columns.Add("Замена", typeof(string));
            dataTable.Columns.Add("Путь", typeof(string));
            dataTable.Columns.Add("Исходное имя файла", typeof(string));
            dataTable.Columns.Add("Атрибут", typeof(string));

            // Parameters
            dataGrid.Columns[" "].ReadOnly = true;
            dataGrid.Columns["Юникод-символы"].ReadOnly = true;
            dataGrid.Columns["Путь"].ReadOnly = true;
            dataGrid.AutoResizeColumns();
            dataGrid.Columns[" "].Width = 25;
            dataGrid.Columns["Юникод-символы"].Width = 115;
            dataGrid.Columns["Путь"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGrid.Columns["Исходное имя файла"].Visible = false;
            dataGrid.Columns["Атрибут"].Visible = false;

            dataGrid.Columns["Юникод-символы"].DefaultCellStyle.ForeColor = Color.DimGray;
            dataGrid.Columns["Замена"].DefaultCellStyle.ForeColor = Color.DimGray;
            dataGrid.Columns["Путь"].DefaultCellStyle.ForeColor = Color.DimGray;

            // Events
            dataGrid.CellValidating += DataGrid_CellValidating;
            dataGrid.CellValueChanged += DataGrid_CellValueChanged;
            dataGrid.SelectionChanged += (s, e) =>
            {
                DataGridView dataGrid = s as DataGridView;
                int selectedCellCount = dataGrid.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    filenameChars.SelectedRowParamsChanged(dataGrid.SelectedCells[0].RowIndex);
                }
                else
                    filenameChars.SelectedRowParamsChanged(-1);
            };
            EntryChangesCountValueChanged += (s, e) =>
            {
                if (EntryChangesCount < 0)
                    EntryChangesCount = 0;
                else if (EntryChangesCount == 0)
                {
                    btnSaveFilenamesToFiles.Enabled = false;
                    btnReturnToOrigFilenames.Enabled = false;
                }
                else
                {
                    btnSaveFilenamesToFiles.Enabled = true;
                    btnReturnToOrigFilenames.Enabled = true;
                }
            };

            dataGrid.KeyPress += DataGrid_KeyPress;

            // Tooltips
            toolTipSaveFilenamesToFiles.SetToolTip(btnSaveFilenamesToFiles, "Сохранить имена файлов в файлы");
            toolTipCopyReplaceFilenames.SetToolTip(btnCopyReplaceFilenames, "Вставить предложенную замену в имя файла");
            toolTipReturnToOrigFilenames.SetToolTip(btnReturnToOrigFilenames, "Вернуть изначальные имена файлов");
        }

        public void SetControlsLinks(FilenameCharsControl filenameChars, ReplaceCharsControl replaceChars, ReplaceFilenamesControl replaceFilenames)
        {
            this.filenameChars = filenameChars;
            this.replaceChars = replaceChars;
            this.replaceFilenames = replaceFilenames;
        }

        // DataGrid edit methods
        public void UpdateSelectedCellInFileParamsTable()
        {
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string attrStr = dataGrid.Rows[i].Cells["Атрибут"].Value as string;
                string fullFilename = dataGrid.Rows[i].Cells["Имя файла"].Value as string;
                string filename = attrStr.Equals("файл") ? TextHandlers.CutFileFormat(fullFilename) : fullFilename;
                if (TextHandlers.IsUnicodeInText(filename))
                {
                    if (replaceFilenames.dict.ContainsKey(filename))
                    {
                        string replaceName = dataGrid.Rows[i].Cells["Замена"].Value as string;
                        string newReplaceName = replaceFilenames.dict[filename];
                        if (!newReplaceName.Equals(replaceName))
                        {
                            dataGrid.Columns["Замена"].ReadOnly = false;
                            dataGrid.Rows[i].Cells["Замена"].Value = newReplaceName;
                            dataGrid.Columns["Замена"].ReadOnly = true;
                        }
                    }
                    else
                    {
                        dataGrid.Columns["Замена"].ReadOnly = false;
                        dataGrid.Rows[i].Cells["Замена"].Value = string.Empty;
                        dataGrid.Columns["Замена"].ReadOnly = true;
                    }
                }
            }
        }

        public void EditReplaceInFilenameCharsTable(int editRowInd, string cyrilicName)
        {
            dataGrid.Rows[editRowInd].ReadOnly = false;
            dataGrid.Rows[editRowInd].Cells["Замена"].Value = cyrilicName;
            dataGrid.Rows[editRowInd].ReadOnly = true;
        }

        // DataGrids handlers
        private void DataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGrid = sender as DataGridView;
            int rowInd = e.RowIndex;
            int colInd = e.ColumnIndex;
            if (rowInd < 0 || colInd < 0)
                return;
            
            if (!dataGrid.Columns[colInd].HeaderText.Equals("Имя файла"))
                return;

            // Checking for changes in value
            string curFilename = Convert.ToString(dataGrid.Rows[rowInd].Cells["Имя файла"].Value);
            string newFilename = Convert.ToString(dataGrid.Rows[rowInd].Cells["Имя файла"].EditedFormattedValue);
            if (curFilename == newFilename)
                return;

            string attr = dataGrid.Rows[rowInd].Cells["Атрибут"].Value as string;

            // Checking for empty name
            string mes = string.Empty;
            if (attr == "файл")
            {
                if (newFilename == string.Empty || newFilename.LastIndexOf('.') == 0 || newFilename.All(c => c.Equals(".")))
                    mes = "Имя файла не может быть пустым.";
            }
            else
            {
                if (newFilename == string.Empty || newFilename.All(c => c.Equals(".")))
                    mes = "Имя папки не может быть пустым.";
            }

            if (mes != string.Empty)
            {
                MessageBox.Show(mes, progName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGrid.CancelEdit();
                return;
            }

            // Counting changes in entry
            string origFilename = Convert.ToString(dataGrid.Rows[rowInd].Cells["Исходное имя файла"].Value);
            if (origFilename.Equals(curFilename))
                EntryChangesCount++;
            else if (origFilename.Equals(newFilename))
                EntryChangesCount--;
        }

        private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGrid = sender as DataGridView;
            int rowInd = e.RowIndex;
            int colInd = e.ColumnIndex;
            if (rowInd < 0 || colInd < 0)
                return;

            // Updating selected row parameters for FilenameChars, if cell with changed value is selected
            int selectedCellCount = dataGrid.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                FilenameCharsControl.SelFile selFile = filenameChars.selFile;
                if (selFile.rowInd == rowInd)
                {
                    filenameChars.SelectedRowParamsChanged(rowInd);
                }
            }

            // Determing weather replace button should be enabled
            string filename = dataGrid.Rows[rowInd].Cells["Имя файла"].Value as string;
            string replaceName = dataGrid.Rows[rowInd].Cells["Замена"].Value as string;
            if (!String.IsNullOrEmpty(replaceName) && !filename.Equals(replaceName) && TextHandlers.IsUnicodeInText(filename))
                btnCopyReplaceFilenames.Enabled = true;

            string headerText = dataGrid.Columns[colInd].HeaderText;
            if (headerText.Equals("Исходное имя файла"))
                EntryChangesCount--;
            else if (headerText.Equals("Имя файла"))
            {
                dataGrid.Rows[rowInd].Cells["Юникод-символы"].Value = TextHandlers.GetUnicodeFromText(filename);
            }
        }

        private void DataGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Delete)
            {
                int selectedCellCount = dataGrid.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    dataGrid.Rows.Remove(dataGrid.CurrentRow);
                }
            }
        }

        // Controls actions
        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = folderBrowserDialog.SelectedPath;
                    if (Directory.Exists(path))
                    {
                        string[] fileEntries = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

                        dataTable.Rows.Clear();

                        foreach (string entry in fileEntries)
                        {
                            // Getting image
                            FileAttributes fileAttr = File.GetAttributes(entry);
                            ImageConverter imageConverter = new ImageConverter();
                            Bitmap entry_image = fileAttr.HasFlag(FileAttributes.Directory) ? Properties.Resources.folder_image : Properties.Resources.mp3_image;
                            byte[] imageByte = (byte[])imageConverter.ConvertTo(entry_image, typeof(byte[]));

                            // Getting name
                            int nameStart = entry.LastIndexOf('\\') + 1;
                            string filename = entry.Substring(nameStart, entry.Length - nameStart);
                            if (filename == "Thumbs.db" || filename == string.Empty)
                                continue;

                            if (!usedOtherFormats && !fileAttr.HasFlag(FileAttributes.Directory) && TextHandlers.GetFileFormat(filename) != ".mp3")
                                continue;

                            string name = fileAttr.HasFlag(FileAttributes.Directory) ? filename : TextHandlers.CutFileFormat(filename);

                            // Getting unicode characters
                            string unicodeChars = TextHandlers.GetUnicodeFromText(name);

                            // Getting replacement for filename
                            string replaceName = string.Empty;
                            if (unicodeChars != string.Empty && replaceFilenames.dict.ContainsKey(name))
                            {
                                replaceName = replaceFilenames.dict[name];
                                btnCopyReplaceFilenames.Enabled = true;
                            }

                            // Getting file directory
                            string dir = entry.Substring(0, nameStart - 1);

                            // Getting file attribute string
                            string attrStr = fileAttr.HasFlag(FileAttributes.Directory) ? "папка" : "файл";

                            dataTable.Rows.Add(new object[] { imageByte, filename, unicodeChars, replaceName, dir, filename, attrStr });
                        }
                    }
                    else
                        MessageBox.Show($"Каталога\n{path}\nне существует.", progName);
                }
            }
        }

        private void btnSaveFilenamesToFiles_Click(object sender, EventArgs e)
        {
            int filesRenamed = 0;
            int filenamesCorrected = 0;
            int renameFails = 0;

            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string filename = dataGrid.Rows[i].Cells["Имя файла"].Value as string;
                string origFilemame = dataGrid.Rows[i].Cells["Исходное имя файла"].Value as string;
                string path = dataGrid.Rows[i].Cells["Путь"].Value as string;

                if (filename.Equals(origFilemame))
                    continue;

                string curFilepath = Path.Combine(path, origFilemame);
                string newFilepath = Path.Combine(path, filename);

                string attr = dataGrid.Rows[i].Cells["Атрибут"].Value as string;
                bool curEntryExists = attr == "файл" ? File.Exists(curFilepath) : Directory.Exists(curFilepath);

                if (!curEntryExists)
                    renameFails++;
                else
                {
                    bool correction = false;

                    // Correcting file/folder name, if it already exists
                    if (File.Exists(newFilepath))
                    {
                        string corFilepath = TextHandlers.GetNewFilename(newFilepath);
                        int nameStart = corFilepath.LastIndexOf('\\') + 1;
                        string corName = corFilepath.Substring(nameStart, corFilepath.Length - nameStart);

                        filename = corName;
                        newFilepath = corFilepath;

                        correction = true;
                    }

                    // Writing new name to file/folder
                    try
                    {
                        if (attr == "файл")
                            File.Move(curFilepath, newFilepath);
                        else
                            Directory.Move(curFilepath, newFilepath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, progName);
                        renameFails++;
                    }
                    finally
                    {
                        dataGrid.Rows[i].Cells["Имя файла"].Value = filename;
                        dataGrid.Columns["Исходное имя файла"].ReadOnly = false;
                        dataGrid.Rows[i].Cells["Исходное имя файла"].Value = filename;
                        dataGrid.Columns["Исходное имя файла"].ReadOnly = true;

                        filesRenamed++;
                        if (correction)
                            filenamesCorrected++;
                    }
                }
            }

            // Notifing about changes
            string filesRenamedMes = $"Переименовано объектов: {filesRenamed}";
            string filenamesCorrectedMes = $"Совпадений имён объектов: {filenamesCorrected}";
            string renameFailsMes = $"Ошибок переименования: {renameFails}";
            int changesCount = filesRenamed + filenamesCorrected + renameFails;
            if (changesCount > 0)
                MessageBox.Show(String.Join("\n", new string[] { filesRenamedMes, filenamesCorrectedMes, renameFailsMes }), progName);
        }

        private void btnCopyReplaceFilenames_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string name = Convert.ToString(dataGrid.Rows[i].Cells["Имя файла"].Value);
                string replaceName = Convert.ToString(dataGrid.Rows[i].Cells["Замена"].Value);
                string origName = Convert.ToString(dataGrid.Rows[i].Cells["Исходное имя файла"].Value);
                string attr = Convert.ToString(dataGrid.Rows[i].Cells["Атрибут"].Value);
                string format = attr.Equals("файл") ? TextHandlers.GetFileFormat(origName) : string.Empty;

                if (!String.IsNullOrEmpty(replaceName))
                {
                    if (!name.Equals(replaceName))
                    {
                        dataGrid.Rows[i].Cells["Имя файла"].Value = replaceName + format;
                        if (origName.Equals(name))
                            EntryChangesCount++;
                        else if (origName.Equals(replaceName))
                            EntryChangesCount--;
                    }
                }
            }

            btnCopyReplaceFilenames.Enabled = false;
        }

        private void btnReturnToOrigFilenames_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string filename = Convert.ToString(dataGrid.Rows[i].Cells["Имя файла"].Value);
                string origFilename = Convert.ToString(dataGrid.Rows[i].Cells["Исходное имя файла"].Value);
                string attr = Convert.ToString(dataGrid.Rows[i].Cells["Атрибут"].Value);
                if (!origFilename.Equals(filename))
                {
                    dataGrid.Rows[i].Cells["Имя файла"].Value = origFilename;
                    EntryChangesCount--;
                }
            }
        }
    }
}