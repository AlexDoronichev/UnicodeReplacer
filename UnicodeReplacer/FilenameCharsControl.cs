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
    public partial class FilenameCharsControl : UserControl
    {
        private string progName = "UnicodeReplacer";

        private DataSet dataSet = new DataSet("FilenameCharsStore");
        private DataTable dataTable = new DataTable("UnicodeCharsTable");

        private FileParamsControl fileParams;
        private ReplaceControl replaceChars;
        private ReplaceControl replaceFilenames;

        private enum CopyReplaceCharsStatus
        {
            Add,
            Replace,
            Complete
        }
        private CopyReplaceCharsStatus copyReplaceCharsStatus = CopyReplaceCharsStatus.Complete;

        public SelFileData selFileData = new SelFileData();
        public struct SelFileData
        {
            public int rowInd;
            public string format;
            public string name;
            public string unicodeChars;
            public string replaceText;
            public string origName;
        }

        private ToolTip toolTipSaveToReplaceCharsTable = new ToolTip();
        private ToolTip toolTipCopyReplaceChars = new ToolTip();
        private ToolTip toolTipReplaceSelFilename = new ToolTip();
        private ToolTip toolTipClearUserInputRow = new ToolTip();
        private ToolTip toolTipSaveToReplaceFilenamesTable = new ToolTip();
        private ToolTip toolTipSaveSelFilenameToFile = new ToolTip();
        private ToolTip toolTipReturnToOrigFilename = new ToolTip();

        public FilenameCharsControl()
        {
            InitializeComponent();

            // Parameters
            selFileData.rowInd = -1;

            // Data sources
            dataSet.Tables.Add(dataTable);
            dataGrid.DataSource = dataSet.Tables[0];

            // Events
            dataGrid.CellMouseClick += (s, e) => { if (e.RowIndex >= 0) (s as DataGridView).Rows[e.RowIndex].Selected = false; };
            dataGrid.CellFormatting += DataGrid_CellFormatting; 
            dataGrid.CellValidating += DataGrid_CellValidating;
            dataGrid.CellValueChanged += DataGrid_CellValueChanged;
            textBoxNewName.TextChanged += FilenameButtonsController;
            textBoxOrigName.TextChanged += FilenameButtonsController;

            // Tooltips
            toolTipSaveToReplaceCharsTable.SetToolTip(btnSaveToReplaceCharsTable, "Сохранить в список замен символов");
            toolTipCopyReplaceChars.SetToolTip(btnCopyReplaceChars, "Дополнить заменяющие символы");

            toolTipReplaceSelFilename.SetToolTip(btnReplaceCharsInSelFilename, "Подтвердить замену символов");
            toolTipClearUserInputRow.SetToolTip(btnClearUserInputRow, "Очистить строку ввода");

            toolTipSaveToReplaceFilenamesTable.SetToolTip(btnSaveToReplaceFilenamesTable, "Сохранить в таблицу замен названий");
            toolTipSaveSelFilenameToFile.SetToolTip(btnSaveSelFilenameToFile, "Сохранить имя файла в файл");
            toolTipReturnToOrigFilename.SetToolTip(btnReturnToOrigFilename, "Вернуть изначальное имя файла");
        }

        public void SetControlsLinks(FileParamsControl fileParams, ReplaceControl replaceChars, ReplaceControl replaceFilenames)
        {
            this.fileParams = fileParams;
            this.replaceChars = replaceChars;
            this.replaceFilenames = replaceFilenames;
        }

        public void SelectedRowParamsChanged(int selRowInd)
        {
            if (selRowInd >= 0)
            {
                string attrStr = fileParams.dataGrid.Rows[selRowInd].Cells["Атрибут"].Value as string;
                string filename = fileParams.dataGrid.Rows[selRowInd].Cells["Имя файла"].Value as string;
                string origFilename = fileParams.dataGrid.Rows[selRowInd].Cells["Исходное имя файла"].Value as string;

                if (String.IsNullOrEmpty(filename))
                {
                    selFileData.rowInd = -1;
                    return;
                }

                if (String.IsNullOrEmpty(origFilename))
                {
                    origFilename = filename;
                    fileParams.dataGrid.Rows[selRowInd].Cells["Исходное имя файла"].Value = filename;
                }

                string name = attrStr.Equals("файл") ? TextHandlers.CutFileFormat(filename) : filename;

                // Setting selected file parameters to FilenameChars control
                selFileData.rowInd = selRowInd;
                selFileData.format = attrStr.Equals("файл") ? TextHandlers.GetFileFormat(origFilename) : string.Empty;
                selFileData.name = name;
                selFileData.unicodeChars = TextHandlers.GetUnicodeFromText(name);
                selFileData.replaceText = fileParams.dataGrid.Rows[selRowInd].Cells["Замена"].Value as string;
                selFileData.origName = attrStr.Equals("файл") ? TextHandlers.CutFileFormat(origFilename) : origFilename;
            }
            else
                selFileData.rowInd = -1;

            UpdateTable();
        }

        // DataGrid edit methods
        public void UpdateTable()
        {
            // If no filename selected, clearing textboxes and disabling checkboxes and buttons
            if (selFileData.rowInd < 0)
            {
                textBoxNewName.Text = string.Empty;
                textBoxOrigName.Text = string.Empty;

                checkBoxOnlyUnicode.Enabled = false;
                checkBoxOnlyUnique.Enabled = false;
                checkBoxShowCode.Enabled = false;

                btnCopyReplaceChars.Enabled = false;
                btnSaveToReplaceCharsTable.Enabled = false;
                btnReplaceCharsInSelFilename.Enabled = false;
                btnClearUserInputRow.Enabled = false;

                return;
            }

            // Clearing the table
            dataTable.Rows.Clear();
            dataTable.Columns.Clear();

            // Getting original filename
            textBoxOrigName.Text = selFileData.origName;

            // Getting characters' set
            textBoxNewName.Text = selFileData.name;
            string filenameChars = selFileData.name.Replace(" ", "");
            if (String.IsNullOrEmpty(filenameChars))
                return;

            // Filling datatable with content
            int colCount = filenameChars.Length;
            string[] tableRow1 = new string[colCount];
            string[] tableRow2 = new string[colCount];
            string[] tableRow3 = new string[colCount];

            bool haveReplace = false;
            for (int i = 0; i < colCount; i++)
            {
                dataTable.Columns.Add();

                string filenameChar = filenameChars[i].ToString();
                tableRow1[i] = filenameChar;
                if (checkBoxShowCode.Checked)
                    tableRow1[i] += String.Format(" ({0})", (int)filenameChar[0]);

                string replaceChar = string.Empty;
                if (!TextHandlers.IsCharCyrilic(filenameChar[0]) && replaceChars.DictContainsKey(filenameChar))
                {
                    replaceChar = replaceChars.DictGetValue(filenameChar);
                    haveReplace = true;
                }
                tableRow2[i] = replaceChar;
            }

            dataTable.Rows.Add(tableRow1);
            dataTable.Rows.Add(tableRow2);
            dataTable.Rows.Add(tableRow3);

            // Detecing buttons state
            copyReplaceCharsStatus = CopyReplaceCharsStatus.Add;
            btnCopyReplaceChars.Enabled = haveReplace;
            btnSaveToReplaceCharsTable.Enabled = false;
            btnReplaceCharsInSelFilename.Enabled = false;
            btnClearUserInputRow.Enabled = false;

            // Enabling checkboxes
            checkBoxOnlyUnicode.Enabled = true;
            checkBoxOnlyUnique.Enabled = true;
            checkBoxShowCode.Enabled = true;

            // Setting DataGridView rows parameters
            dataGrid.Rows[0].ReadOnly = true;
            dataGrid.Rows[1].ReadOnly = true;
            dataGrid.Rows[0].HeaderCell.Value = checkBoxShowCode.Checked ? "Юникод-символ (код)" : "Юникод-символ";
            dataGrid.Rows[1].HeaderCell.Value = "Текущая замена";
            dataGrid.Rows[2].HeaderCell.Value = "Заменить";
            dataGrid.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

            dataGrid.Rows[0].DefaultCellStyle.ForeColor = Color.DimGray;
            dataGrid.Rows[1].DefaultCellStyle.ForeColor = Color.DimGray;
            

            // Setting DataGridView columns parameters
            dataGrid.AutoResizeColumns();
            foreach (DataGridViewColumn column in dataGrid.Columns)
                column.Width = Math.Max(column.Width, 40);

            DataGridFilenameCharsColumnsVisibilityUpdate();
        }

        public void UpdateReplaceRow()
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string unicodeChar = dataGrid.Rows[0].Cells[i].Value as string;
                if (String.IsNullOrEmpty(unicodeChar))
                    continue;

                if (!TextHandlers.IsCharCyrilic(unicodeChar[0]))
                {
                    if (replaceChars.DictContainsKey(unicodeChar))
                    {
                        string replaceChar = dataGrid.Rows[1].Cells[i].Value as string;
                        string newReplaceChar = replaceChars.DictGetValue(unicodeChar);
                        if (!newReplaceChar.Equals(replaceChar))
                        {
                            dataGrid.Rows[1].ReadOnly = false;
                            dataGrid.Rows[1].Cells[i].Value = newReplaceChar;
                            dataGrid.Rows[1].ReadOnly = true;
                        }
                    }
                    else
                    {
                        dataGrid.Rows[1].ReadOnly = false;
                        dataGrid.Rows[1].Cells[i].Value = string.Empty;
                        dataGrid.Rows[1].ReadOnly = true;
                    }
                }
            }
        }

        private void EditEntryInFilenameCharsTable(int colInd, string newCyrilicStr)
        {
            dataGrid.Rows[1].ReadOnly = false;
            dataGrid.Rows[1].Cells[colInd].Value = newCyrilicStr;
            dataGrid.Rows[1].ReadOnly = true;
        }

        private void DataGridFilenameCharsColumnsVisibilityUpdate()
        {
            HashSet<char> uniqueChars = new HashSet<char>();
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string charStr = dataGrid.Rows[0].Cells[i].Value as string;

                dataGrid.Columns[i].Visible = true;

                if (checkBoxOnlyUnicode.Checked && TextHandlers.IsCharCyrilic(charStr[0]))
                    dataGrid.Columns[i].Visible = false;
                else if (checkBoxOnlyUnique.Checked)
                    dataGrid.Columns[i].Visible = uniqueChars.Add(charStr[0]);
            }
        }

        // DataGrids handlers
        private void DataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.Value = Convert.ToString(e.Value).Trim();
        }

        private void DataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGrid = sender as DataGridView;

            // Defining the input cell
            int rowInd = e.RowIndex;
            int colInd = e.ColumnIndex;
            if (rowInd < 0 || colInd < 0)
                return;
            if (!Convert.ToString(dataGrid.Rows[rowInd].HeaderCell.Value).Equals("Заменить"))
                return;

            dataGrid.Rows[rowInd].ErrorText = String.Empty;

            // Checking user input
            string curValue = Convert.ToString(dataGrid.Rows[2].Cells[colInd].Value);
            string newUserInput = Convert.ToString(dataGrid.Rows[2].Cells[colInd].EditedFormattedValue);
            if (newUserInput.Equals(curValue))
                return;

            string filenameChar = dataGrid.Rows[0].Cells[colInd].Value as string;
            if (TextHandlers.IsCharCyrilic(filenameChar[0]))
            {
                dataGrid.Rows[rowInd].ErrorText = "Заменять можно только Юникод-символы";
                dataGrid.CancelEdit();
                return;
            }

            // Checking correction of user input
            dataGrid.Rows[rowInd].ErrorText = String.Empty;

            if (newUserInput.Length > ReplaceControl.GetMaxReplaceChars())
            {
                dataGrid.Rows[rowInd].ErrorText = "Введите до 2-х символов";
                dataGrid.CancelEdit();
                return;
            }
            else
            {
                string cyrilicStr = string.Empty;
                foreach (char c in newUserInput)
                    if (TextHandlers.IsCharCyrilic(c))
                        cyrilicStr += c;

                if (!newUserInput.Equals(cyrilicStr))
                {
                    dataGrid.Rows[rowInd].ErrorText = "Введите только кириллические символы";
                    dataGrid.CancelEdit();
                    return;
                }
            }

            // Checking matching unicode characters
            if (!newUserInput.Equals(curValue))
            {
                string curCellUnicodeChar = dataGrid.Rows[0].Cells[colInd].Value as string;
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    string unicodeChar = dataGrid.Rows[0].Cells[i].Value as string;
                    if (String.IsNullOrEmpty(unicodeChar))
                        continue;

                    // Filling user input replace character for each matching unicode character in filename
                    if (unicodeChar.Equals(curCellUnicodeChar))
                        dataGrid.Rows[2].Cells[i].Value = newUserInput;
                }
            }
        }

        private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGrid = sender as DataGridView;
            int rowInd = e.RowIndex;
            int colInd = e.ColumnIndex;
            if (rowInd < 1 || colInd < 0)
                return;

            // Checking for user input characters to enable buttons
            bool inputRowEmpty = true;
            btnSaveToReplaceCharsTable.Enabled = false;

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string replaceChar = dataGrid.Rows[1].Cells[i].Value as string;
                string userInputChar = dataGrid.Rows[2].Cells[i].Value as string;

                if (!String.IsNullOrEmpty(userInputChar))
                {
                    inputRowEmpty = false;
                    if (String.IsNullOrEmpty(replaceChar) || !userInputChar.Equals(replaceChar))
                        btnSaveToReplaceCharsTable.Enabled = true;
                }
            }

            btnClearUserInputRow.Enabled = !inputRowEmpty;
            btnReplaceCharsInSelFilename.Enabled = !inputRowEmpty;

            // Defining status for coping button
            copyReplaceCharsStatus = CopyReplaceCharsStatus.Complete;

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string replaceChar = dataGrid.Rows[1].Cells[i].Value as string;
                string userInputChar = dataGrid.Rows[2].Cells[i].Value as string;
                if (!String.IsNullOrEmpty(replaceChar))
                {
                    if (String.IsNullOrEmpty(userInputChar))
                    {
                        copyReplaceCharsStatus = CopyReplaceCharsStatus.Add;
                        toolTipCopyReplaceChars.SetToolTip(btnCopyReplaceChars, "Дополнить заменяющие символы");
                        btnCopyReplaceChars.Enabled = true;
                        return;
                    }
                    else if (!userInputChar.Equals(replaceChar))
                    {
                        if (copyReplaceCharsStatus == CopyReplaceCharsStatus.Complete)
                            copyReplaceCharsStatus = CopyReplaceCharsStatus.Replace;
                    }
                }
            }

            if (copyReplaceCharsStatus == CopyReplaceCharsStatus.Replace)
            {
                btnCopyReplaceChars.Enabled = true;
                toolTipCopyReplaceChars.SetToolTip(btnCopyReplaceChars, "Заместить введённые символы");
            }
            else if (copyReplaceCharsStatus == CopyReplaceCharsStatus.Complete)
            {
                btnCopyReplaceChars.Enabled = false;
                toolTipCopyReplaceChars.SetToolTip(btnCopyReplaceChars, null);
            }
        }

        // Controls actions
        private void checkBoxShowCode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowCode.Checked)
            {
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    string unicodeChar = dataGrid.Rows[0].Cells[i].Value as string;
                    dataGrid.Rows[0].Cells[i].Value = String.Format("{0} ({1})", unicodeChar, (int)unicodeChar[0]);
                }
                dataGrid.AutoResizeColumns();
            }
            else
            {
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    string unicodeStr = dataGrid.Rows[0].Cells[i].Value as string;
                    dataGrid.Rows[0].Cells[i].Value = unicodeStr[0].ToString();
                    dataGrid.Columns[i].Width = 40;
                }
            }
        }

        private void checkBoxOnlyUnicode_CheckedChanged(object sender, EventArgs e)
        {
            DataGridFilenameCharsColumnsVisibilityUpdate();
        }

        private void checkBoxOnlyUnique_CheckedChanged(object sender, EventArgs e)
        {
            DataGridFilenameCharsColumnsVisibilityUpdate();
        }

        private void FilenameButtonsController(object sender, EventArgs e)
        {
            bool origNameIsUnicode = TextHandlers.IsUnicodeInText(textBoxOrigName.Text);
            bool newNameIsCyrilic = !TextHandlers.IsUnicodeInText(textBoxNewName.Text);
            bool existInTable = replaceFilenames.DictContainsKey(textBoxOrigName.Text);
            btnSaveToReplaceFilenamesTable.Enabled = origNameIsUnicode && newNameIsCyrilic && !existInTable;
            btnSaveSelFilenameToFile.Enabled = !textBoxOrigName.Text.Equals(textBoxNewName.Text) && !textBoxNewName.Text.Equals(string.Empty);
            btnReturnToOrigFilename.Enabled = !textBoxOrigName.Text.Equals(textBoxNewName.Text);
        }

        private void btnReplaceCharsInSelFilename_Click(object sender, EventArgs e)
        {
            if (dataGrid.Columns.Count == 0)
                return;

            if (selFileData.unicodeChars.Length < 1)
                return;

            // Replacing all unicode characters by user's replacing characters
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string unicodeCharStr = dataGrid.Rows[0].Cells[i].Value as string;
                if (String.IsNullOrEmpty(unicodeCharStr))
                    continue;

                string userInputChar = dataGrid.Rows[2].Cells[i].Value as string;
                if (String.IsNullOrEmpty(userInputChar) || userInputChar.Trim() == string.Empty)
                    continue;

                char unicodeChar = unicodeCharStr[0]; // Cutting the code, if exist
                if (!TextHandlers.IsCharCyrilic(unicodeChar) && selFileData.name.Contains(unicodeChar))
                    selFileData.name = selFileData.name.Replace(unicodeChar.ToString(), userInputChar);
            }

            selFileData.unicodeChars = TextHandlers.GetUnicodeFromText(selFileData.name);

            // Updating filename
            string newName = selFileData.name;
            string curFilename = Convert.ToString(fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Имя файла"].Value);
            string attr = Convert.ToString(fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Атрибут"].Value);
            string curName = attr.Equals("файл") ? TextHandlers.CutFileFormat(curFilename) : curFilename;
            if (!newName.Equals(curName))
            {
                fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Имя файла"].Value = newName + selFileData.format;

                string origFilename = Convert.ToString(fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Исходное имя файла"].Value);
                string origName = attr.Equals("файл") ? TextHandlers.CutFileFormat(origFilename) : origFilename;
                if (origName.Equals(curName))
                    fileParams.EntryChangesCountInc();
                else if (origName.Equals(newName))
                    fileParams.EntryChangesCountDec();
            }

            UpdateTable();
        }

        private void btnClearUserInputRow_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string userInputChar = dataGrid.Rows[2].Cells[i].Value as string;
                if (!String.IsNullOrEmpty(userInputChar))
                    dataGrid.Rows[2].Cells[i].Value = string.Empty;
            }
        }

        private void btnCopyReplaceChars_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string replaceChar = dataGrid.Rows[1].Cells[i].Value as string;
                if (!String.IsNullOrEmpty(replaceChar))
                {
                    switch (copyReplaceCharsStatus)
                    {
                        case CopyReplaceCharsStatus.Add:
                            string userInputChar = dataGrid.Rows[2].Cells[i].Value as string;
                            if (String.IsNullOrEmpty(userInputChar))
                                dataGrid.Rows[2].Cells[i].Value = replaceChar;
                            break;
                        case CopyReplaceCharsStatus.Replace:
                            dataGrid.Rows[2].Cells[i].Value = replaceChar;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void btnSaveToReplaceCharsTable_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                string unicodeCharStr = dataGrid.Rows[0].Cells[i].Value as string;
                string replaceChar = dataGrid.Rows[1].Cells[i].Value as string;
                string userInputChar = dataGrid.Rows[2].Cells[i].Value as string;
                if (!String.IsNullOrEmpty(userInputChar) && !userInputChar.Equals(replaceChar))
                {
                    replaceChars.EditEntry(unicodeCharStr, userInputChar);
                    EditEntryInFilenameCharsTable(i, userInputChar);
                }
            }
        }

        private void btnSaveToReplaceFilenamesTable_Click(object sender, EventArgs e)
        {
            replaceFilenames.EditEntry(textBoxOrigName.Text, textBoxNewName.Text);
            fileParams.EditReplaceInFilenameCharsTable(selFileData.rowInd, textBoxNewName.Text);
        }

        private void btnSaveSelFilenameToFile_Click(object sender, EventArgs e)
        {
            // Checking file parameters
            if (selFileData.rowInd < 0)
            {
                btnSaveSelFilenameToFile.Enabled = false;
                return;
            }

            if (String.IsNullOrEmpty(selFileData.name))
            {
                MessageBox.Show("Имя файла не должно быть пустым", progName);
                return;
            }

            string filename = selFileData.name + selFileData.format;
            string origFilename = selFileData.origName + selFileData.format;
            string path = fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Путь"].Value as string;

            if (String.IsNullOrEmpty(origFilename))
            {
                origFilename = fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Исходное имя файла"].Value as string;
                if (String.IsNullOrEmpty(origFilename))
                {
                    fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Исходное имя файла"].Value = filename;
                    return;
                }
            }

            if (filename.Equals(origFilename))
                return;

            string curFilepath = Path.Combine(path, origFilename);
            string newFilepath = Path.Combine(path, filename);

            string attr = fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Атрибут"].Value as string;
            bool curEntryExists = attr == "файл" ? File.Exists(curFilepath) : Directory.Exists(curFilepath);
            
            if (!curEntryExists)
            {
                string mes;
                if (attr == "файл")
                    mes = $"Не удаётся найти файл:\n{curFilepath}";
                else
                    mes = $"Не удаётся найти папку:\n{curFilepath}";
                MessageBox.Show(mes, progName);
            }
            else
            {
                // Correcting file/folder name, if it already exists
                bool newEntryExists = attr == "файл" ? File.Exists(newFilepath) : Directory.Exists(newFilepath);
                if (newEntryExists)
                {
                    string corFilepath = TextHandlers.GetNewFilename(newFilepath);
                    int nameStart = corFilepath.LastIndexOf('\\') + 1;
                    string corFilename = corFilepath.Substring(nameStart, corFilepath.Length - nameStart);

                    string mes;
                    if (attr == "файл")
                        mes = $"\nФайл с именем \"{filename}\" уже существует.\nХотите переименовать текущий файл в \"{corFilename}\"?";
                    else
                        mes = $"\nПапка с именем \"{filename}\" уже существует.\nХотите переименовать текущую папку в \"{corFilename}\"?";

                    DialogResult result = MessageBox.Show(mes, progName, MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        filename = corFilename;
                        newFilepath = corFilepath;
                    }
                    else
                        return;
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
                }
                finally
                {
                    fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Имя файла"].Value = filename;
                    fileParams.dataGrid.Columns["Исходное имя файла"].ReadOnly = false;
                    fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Исходное имя файла"].Value = filename;
                    fileParams.dataGrid.Columns["Исходное имя файла"].ReadOnly = false;

                    if (attr == "файл")
                        MessageBox.Show("Файл успешно переименован.", progName);
                    else
                        MessageBox.Show("Папка успешно переименована.", progName);
                }
            }
        }

        private void btnReturnToOrigFilename_Click(object sender, EventArgs e)
        {
            string filename = Convert.ToString(fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Имя файла"].Value);
            string origFilename = Convert.ToString(fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Исходное имя файла"].Value);
            string attr = Convert.ToString(fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Атрибут"].Value);
            if (origFilename.Equals(filename))
            {
                textBoxOrigName.Text = TextHandlers.CutFileFormat(origFilename);
                textBoxNewName.Text = textBoxOrigName.Text;
            }
            else
            {
                fileParams.dataGrid.Rows[selFileData.rowInd].Cells["Имя файла"].Value = origFilename;
                fileParams.EntryChangesCountDec();
            }
        }
    }
}
