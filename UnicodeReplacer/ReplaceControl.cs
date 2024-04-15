using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace UnicodeReplacer
{
    public partial class ReplaceControl : UserControl
    {
        private string progName = "UnicodeReplacer";

        private DataSet dataSet = new DataSet("Store");
        private DataTable dataTable = new DataTable("Table");

        private FileParamsControl fileParams;
        private FilenameCharsControl filenameChars;

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        private static int maxUnicodeChars;
        private static int maxReplaceChars;

        public enum ControlType
        {
            Chars,
            Filenames
        }

        private ControlType type;

        public ControlType Type
        {
            get { return type; }
            set
            {
                type = value;
                TypeDependentBlock(value);
            }
        }

        // If DataTable and Dictionary has differences - true, otherwise - false
        private bool hasChanges;
        private bool HasChanges
        {
            get { return hasChanges; }
            set
            {
                if (value != hasChanges)
                {
                    hasChanges = value;
                    if (HasChangesValueChanged != null)
                        HasChangesValueChanged(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler HasChangesValueChanged;

        private ToolTip toolTipAddRow = new ToolTip();
        private ToolTip toolTipDeleteRow = new ToolTip();
        private ToolTip toolTipAcceptChanges = new ToolTip();
        private ToolTip toolTipDeclineChanges = new ToolTip();

        public ReplaceControl()
        {
            InitializeComponent();

            // Options
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            // Data sources
            dataSet.Tables.Add(dataTable);
            dataGrid.DataSource = dataSet.Tables[0];

            // DataTable columns
            dataTable.Columns.Add("Юникод", typeof(string));
            dataTable.Columns.Add("Замена", typeof(string));

            // DataGrid parameters
            dataGrid.RowHeadersWidth = 50;
            dataGrid.Columns["Юникод"].HeaderText = "Юникод";
            dataGrid.Columns["Юникод"].Width = 60;
            dataGrid.Columns["Замена"].HeaderText = "Замена";
            dataGrid.Columns["Замена"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            // Buttons
            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;

            // Events
            dataGrid.CellValidating += DataGrid_CellValidating;
            dataGrid.CellValueChanged += DataGrid_CellValueChanged;
            dataGrid.UserDeletedRow += UserDeletedRowHandler;
            dataGrid.UserAddedRow += UserAddedRowHandler;
            HasChangesValueChanged += (s, e) =>
            {
                dataGrid.CellValueChanged -= DataGrid_CellValueChanged;
                dataGrid.UserDeletedRow -= UserDeletedRowHandler;
                dataGrid.UserAddedRow -= UserAddedRowHandler;
                if (!HasChanges)
                {
                    dataGrid.CellValueChanged += DataGrid_CellValueChanged;
                    dataGrid.UserDeletedRow += UserDeletedRowHandler;
                    dataGrid.UserAddedRow += UserAddedRowHandler;
                }

                btnAcceptChanges.Enabled = HasChanges;
                btnDeclineChanges.Enabled = HasChanges;
            };

            // Tooltips
            toolTipAddRow.SetToolTip(btnAddRow, "Добавить новую строку");
            toolTipDeleteRow.SetToolTip(btnDeleteRow, "Удалить выбранные строки");
            toolTipAcceptChanges.SetToolTip(btnAcceptChanges, "Подтвердить изменения");
            toolTipDeclineChanges.SetToolTip(btnDeclineChanges, "Отменить изменения");
        }

        private void TypeDependentBlock(ControlType curType)
        {
            // Setting titles
            tableLabel.Text = curType == ControlType.Chars ? "Замена символов" : "Замена названий";
            dataGrid.Columns["Юникод"].HeaderText = curType == ControlType.Chars ? "Юникод-символ" : "Юникод-название";

            // Getting content for table
            ReadReplaceDB();
            UpdateTable();

            // Setting options
            maxUnicodeChars = curType == ControlType.Chars ? 1 : 255;
            maxReplaceChars = curType == ControlType.Chars ? 5 : 255;
        }

        public void SetControlsLinks(FileParamsControl fileParams, FilenameCharsControl filenameChars)
        {
            this.fileParams = fileParams;
            this.filenameChars = filenameChars;
        }

        // Operations with fields
        public static int GetMaxUnicodeChars()
        {
            return maxUnicodeChars;
        }

        public static int GetMaxReplaceChars()
        {
            return maxReplaceChars;
        }

        public bool DictContainsKey(string key)
        {
            return dict.ContainsKey(key);
        }

        public string DictGetValue(string key)
        {
            return dict[key];
        }

        // DataTable and Dictionary operations
        public void UpdateTable()
        {
            // Clearing the datatable
            dataTable.Rows.Clear();

            // Filling datatable with content
            foreach (KeyValuePair<string, string> entry in dict)
                dataTable.Rows.Add(new string[] { entry.Key.ToString(), entry.Value });

            if (dataTable.Rows.Count == 0)
                dataTable.Rows.Add();

            HasChanges = false;
        }

        public void EditEntry(string unicodeUnit, string cyrilicUnit)
        {
            if (dict.ContainsKey(unicodeUnit))
            {
                dict[unicodeUnit] = cyrilicUnit;
                EditEntryInTable(unicodeUnit, cyrilicUnit);
                EditEntryToReplaceDB(unicodeUnit, cyrilicUnit);
            }
            else
            {
                dict.Add(unicodeUnit, cyrilicUnit);
                AddEntryToDataTable(unicodeUnit, cyrilicUnit);
                AddEntryToReplaceDB(unicodeUnit, cyrilicUnit);
            }

            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void AddEntryToDataTable(string unicodeName, string cyrilicName)
        {
            dataTable.Rows.Add(unicodeName, cyrilicName);

            if (dataGrid.Rows.Count == 2)
            {
                string unicodeStr = dataGrid.Rows[0].Cells["Юникод"].Value as string;
                string cyrilicStr = dataGrid.Rows[0].Cells["Замена"].Value as string;
                if (String.IsNullOrEmpty(unicodeStr) && String.IsNullOrEmpty(cyrilicStr))
                    dataGrid.Rows.RemoveAt(0);
            }
        }

        private void EditEntryInTable(string newUnicodeName, string newCyrilicName)
        {
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string unicodeName = dataGrid.Rows[i].Cells["Юникод"].Value as string;
                if (!String.IsNullOrEmpty(unicodeName) && newUnicodeName.Equals(unicodeName))
                {
                    dataGrid.Rows[i].Cells["Замена"].Value = newCyrilicName;
                    break;
                }
            }
        }

        private bool SaveChanges() // Saving changes from DataGrid to Dictionary
        {
            int entriesAdded = 0;
            int entriesEdited = 0;
            int entriesDeleted = 0;

            // Writing entries to Dictionary
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string newUnicodeUnit = dataGrid.Rows[i].Cells["Юникод"].Value as string;
                string newCyrilicUnit = dataGrid.Rows[i].Cells["Замена"].Value as string;

                // Deleting incomplete pairs
                if (String.IsNullOrEmpty(newUnicodeUnit) || String.IsNullOrEmpty(newCyrilicUnit))
                {
                    if (dataGrid.Rows.Count > 1)
                    {
                        dataGrid.Rows.RemoveAt(i--);
                        continue;
                    }
                    else if (dataGrid.Rows.Count == 1)
                    {
                        dataGrid.Rows[0].Cells["Юникод"].Value = string.Empty;
                        dataGrid.Rows[0].Cells["Замена"].Value = string.Empty;
                        break;
                    }
                }

                // Filtering wrong length of inputs
                if (newUnicodeUnit.Length > maxUnicodeChars || newCyrilicUnit.Length > maxReplaceChars)
                    continue;

                if (dict.ContainsKey(newUnicodeUnit))
                {
                    // Adding entries to Dictionary
                    string oldCyrilicUnit = dict[newUnicodeUnit];

                    if (!newCyrilicUnit.Equals(oldCyrilicUnit))
                    {
                        dict[newUnicodeUnit] = newCyrilicUnit;
                        EditEntryToReplaceDB(newUnicodeUnit, newCyrilicUnit);
                        entriesEdited++;
                    }
                }
                else
                {
                    // Editing entries of Dictionary
                    dict.Add(newUnicodeUnit, newCyrilicUnit);
                    AddEntryToReplaceDB(newUnicodeUnit, newCyrilicUnit);
                    entriesAdded++;
                }
            }

            // Deleting entries from Dictionary
            foreach (string unicodeName in dict.Keys)
            {
                bool existInDataGrid = false;
                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    DataGridViewCell cell = dataGrid.Rows[i].Cells["Юникод"];
                    if (unicodeName.Equals(cell.Value as string))
                    {
                        existInDataGrid = true;
                        break;
                    }
                }

                if (!existInDataGrid)
                {
                    dict.Remove(unicodeName);
                    DeleteEntryFromReplaceDB(unicodeName);
                    entriesDeleted++;
                }
            }

            // Notifing about changes
            string entriesAddedMes = String.Format("Добавлено записей: {0}", entriesAdded);
            string entriesEditedMes = String.Format("Изменено записей: {0}", entriesEdited);
            string entriesDeletedMes = String.Format("Удалено записей: {0}", entriesDeleted);
            int changesCount = entriesAdded + entriesEdited + entriesDeleted;
            if (changesCount > 0)
                MessageBox.Show(String.Join("\n", new string[] { entriesAddedMes, entriesEditedMes, entriesDeletedMes }), progName);

            HasChanges = false;
            return changesCount > 0;
        }

        // Database operations
        private async void ReadReplaceDB()
        {
            dict.Clear();

            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                SqliteDataReader dataReader = null;

                try
                {
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = connection;

                    if (Type == ControlType.Chars)
                    {
                        command.CommandText = "CREATE TABLE IF NOT EXISTS ReplaceChars(Id INT NOT NULL PRIMARY KEY, " +
                            "UnicodeChar CHAR NOT NULL, CyrilicChar CHAR(5) NOT NULL, UNIQUE(Id, UnicodeChar))";
                    }
                    else
                    {
                        command.CommandText = "CREATE TABLE IF NOT EXISTS ReplaceNames(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                            "UnicodeName VARCHAR NOT NULL, CyrilicName VARCHAR NOT NULL, UNIQUE(Id, UnicodeName))";
                    }

                    await command.ExecuteNonQueryAsync();

                    command.CommandText = Type == ControlType.Chars ? "SELECT * FROM ReplaceChars" : "SELECT * FROM ReplaceNames";

                    dataReader = await command.ExecuteReaderAsync();

                    while (await dataReader.ReadAsync())
                    {
                        string unicodeFilename = Convert.ToString(dataReader["UnicodeName"]);
                        string cyrilicFilename = Convert.ToString(dataReader["CyrilicName"]);
                        dict.Add(unicodeFilename, cyrilicFilename);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
                finally
                {
                    if (dataReader != null && !dataReader.IsClosed)
                        dataReader.Close();
                }
            }
        }

        private async void AddEntryToReplaceDB(string unicodeUnit, string cyrilicUnit)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                try
                {
                    using (SqliteCommand command = new SqliteCommand())
                    {
                        command.Connection = connection;

                        if (Type == ControlType.Chars)
                        {
                            command.CommandText = "INSERT INTO [ReplaceChars] (Id, UnicodeChar, CyrilicChar) " +
                                              "VALUES (@Id, @UnicodeChar, @CyrilicChar)";

                            command.Parameters.AddWithValue("@Id", (int)unicodeUnit[0]);
                            command.Parameters.AddWithValue("@UnicodeChar", unicodeUnit[0]);
                            command.Parameters.AddWithValue("@CyrilicChar", cyrilicUnit);
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO [ReplaceNames] (UnicodeName, CyrilicName) " +
                                              "VALUES (@UnicodeName, @CyrilicName)";
                            command.Parameters.AddWithValue("@UnicodeName", unicodeUnit);
                            command.Parameters.AddWithValue("@CyrilicName", cyrilicUnit);
                        }

                        await command.ExecuteScalarAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
            }
        }

        private async void EditEntryToReplaceDB(string unicodeUnit, string cyrilicUnit)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                try
                {
                    using (SqliteCommand command = new SqliteCommand())
                    {
                        command.Connection = connection;

                        if (Type == ControlType.Chars)
                        {
                            command.CommandText = "UPDATE ReplaceChars SET CyrilicChar = @CyrilicChar WHERE UnicodeChar LIKE @UnicodeChar";
                            command.Parameters.AddWithValue("@UnicodeChar", unicodeUnit[0]);
                            command.Parameters.AddWithValue("@CyrilicChar", cyrilicUnit);
                        }
                        else
                        {
                            command.CommandText = "UPDATE ReplaceNames SET CyrilicName = @CyrilicName WHERE UnicodeName LIKE @UnicodeName";
                            command.Parameters.AddWithValue("@UnicodeName", unicodeUnit);
                            command.Parameters.AddWithValue("@CyrilicName", cyrilicUnit);
                        }

                        await command.ExecuteScalarAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
            }
        }

        private async void DeleteEntryFromReplaceDB(string unicodeUnit)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                try
                {
                    SqliteCommand command = new SqliteCommand();
                    command.Connection = connection;

                    if (Type == ControlType.Chars)
                    {
                        command.CommandText = "DELETE FROM ReplaceChars WHERE UnicodeChar = @UnicodeChar";
                        command.Parameters.AddWithValue("@UnicodeName", unicodeUnit[0]);
                    }
                    else
                    {
                        command.CommandText = "DELETE FROM ReplaceNames WHERE UnicodeName = @UnicodeName";
                        command.Parameters.AddWithValue("@UnicodeChar", unicodeUnit);
                    }

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
            }
        }

        // Controls handlers
        private void DataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int rowInd = e.RowIndex;
            int colInd = e.ColumnIndex;
            DataGridViewCell cell = dataGrid.Rows[rowInd].Cells[colInd];

            dataGrid.Rows[rowInd].ErrorText = String.Empty;

            // Defining the input cell
            if (rowInd < 0 || colInd < 0)
                return;

            string userInputStr = cell.EditedFormattedValue as string;
            if (String.IsNullOrEmpty(userInputStr) || userInputStr.Equals(cell.Value))
                return;

            if (Type == ControlType.Chars)
            {
                // Checking correction of user input
                if (dataGrid.Columns[colInd].HeaderText.Equals("Юникод-символ"))
                {
                    if (userInputStr.Length > 1)
                    {
                        dataGrid.Rows[rowInd].ErrorText = "Введите не более одного символа";
                        dataGrid.CancelEdit();
                    }
                    else if (TextHandlers.IsCharCyrilic(userInputStr[0]))
                    {
                        dataGrid.Rows[rowInd].ErrorText = "Введите символ, не входящий в кириллическую кодировку";
                        dataGrid.CancelEdit();
                    }
                    else
                    {
                        if (dict.ContainsKey(userInputStr))
                        {
                            dataGrid.Rows[rowInd].ErrorText = "Этот символ уже есть в таблице";
                            dataGrid.CancelEdit();
                        }
                        else
                            dataGrid.Rows[rowInd].HeaderCell.Value = (int)userInputStr[0];
                    }
                }
                else if (dataGrid.Columns[colInd].HeaderText.Equals("Замена"))
                {
                    if (userInputStr == string.Empty || userInputStr.Length == 0)
                        return;

                    if (userInputStr.Length > maxReplaceChars)
                    {
                        dataGrid.Rows[rowInd].ErrorText = String.Format("Введите не более {0} символов", maxReplaceChars);
                        dataGrid.CancelEdit();
                    }
                    else if (!TextHandlers.IsCharCyrilic(userInputStr[0]))
                    {
                        dataGrid.Rows[rowInd].ErrorText = "Введите символы, входящие в кириллическую кодировку";
                        dataGrid.CancelEdit();
                    }
                }
            }
            else
            {
                // Checking correction of user input
                if (dataGrid.Columns[colInd].HeaderText.Equals("Юникод-название"))
                {
                    if (!TextHandlers.IsUnicodeInText(userInputStr))
                    {
                        dataGrid.Rows[rowInd].ErrorText = "Введите название с юникод-символами";
                        dataGrid.CancelEdit();
                    }
                    else if (dict.ContainsKey(userInputStr))
                    {
                        dataGrid.Rows[rowInd].ErrorText = "Это название уже есть в таблице";
                        dataGrid.CancelEdit();
                    }
                }
            }
        }

        private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                HasChanges = true;
        }

        private void UserDeletedRowHandler(object sender, DataGridViewRowEventArgs e)
        {
            HasChanges = true;
            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void UserAddedRowHandler(object sender, DataGridViewRowEventArgs e)
        {
            HasChanges = true;
            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        // Buttons actions
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Add();

            HasChanges = true;
            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in dataGrid.SelectedCells)
            {
                int rowInd = cell.RowIndex;
                if (rowInd >= 0 && dataGrid.Rows.Count > 1)
                {
                    dataGrid.Rows.RemoveAt(rowInd);
                    HasChanges = true;
                }
            }

            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void btnAcceptChanges_Click(object sender, EventArgs e)
        {
            bool madeChanged = SaveChanges();
            if (madeChanged)
            {
                if (Type == ControlType.Chars)
                    filenameChars.UpdateReplaceRow();
                else
                    fileParams.UpdateSelectedCellInFileParamsTable();
            }

            HasChanges = false;
            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void btnDeclineChanges_Click(object sender, EventArgs e)
        {
            UpdateTable();

            HasChanges = false;
            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }
    }
}
