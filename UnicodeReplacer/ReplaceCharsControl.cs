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
    public partial class ReplaceCharsControl : UserControl
    {
        string progName = "UnicodeReplacer";

        DataSet dataSet = new DataSet("ReplaceCharsStore");
        DataTable dataTable = new DataTable("ReplaceCharsTable");

        FileParamsControl fileParams;
        FilenameCharsControl filenameChars;
        ReplaceFilenamesControl replaceFilenames;

        public Dictionary<char, string> dict = new Dictionary<char, string>();

        public static int maxReplChars = 2;

        // If DataTable and Dictionary has differences - true, otherwise - false
        private bool haveChanges;

        public bool HasChanges
        {
            get { return haveChanges; }
            private set
            {
                if (value != haveChanges)
                {
                    haveChanges = value;
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

        public ReplaceCharsControl()
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
            dataTable.Columns.Add("Юникод-символ", typeof(string));
            dataTable.Columns.Add("Замена", typeof(string));

            // DataGrid parameters
            dataGrid.RowHeadersWidth = 50;
            dataGrid.Columns["Юникод-символ"].HeaderText = "Юникод символ";
            dataGrid.Columns["Юникод-символ"].Width = 60;
            //dataGrid.AutoResizeColumn(0);
            dataGrid.Columns["Замена"].HeaderText = "Замена";
            dataGrid.Columns["Замена"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Getting content for table
            ReadReplaceCharsDB();
            UpdateTable();

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

        public void SetControlsLinks(FileParamsControl fileParams, FilenameCharsControl filenameChars, ReplaceFilenamesControl replaceFilenames)
        {
            this.fileParams = fileParams;
            this.filenameChars = filenameChars;
            this.replaceFilenames = replaceFilenames;
        }

        // DataTable and Dictionary operations
        public void UpdateTable()
        {
            // Clearing the datatable
            dataTable.Rows.Clear();

            // Filling datatable with content
            foreach (KeyValuePair<char, string> entry in dict)
                dataTable.Rows.Add(new string[] { entry.Key.ToString(), entry.Value });

            if (dataTable.Rows.Count == 0)
                dataTable.Rows.Add();

            HasChanges = false;
        }

        public void EditEntry(char unicodeChar, string cyrilicChar)
        {
            if (dict.ContainsKey(unicodeChar))
            {
                dict[unicodeChar] = cyrilicChar;
                EditEntryInTable(unicodeChar, cyrilicChar);
                EditEntryToReplaceCharsDB(unicodeChar, cyrilicChar);
            }
            else
            {
                dict.Add(unicodeChar, cyrilicChar);
                AddEntryToDataTable(unicodeChar, cyrilicChar);
                AddEntryToReplaceCharsDB(unicodeChar, cyrilicChar);
            }

            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void AddEntryToDataTable(char unicodeChar, string cyrilicChar)
        {
            dataTable.Rows.Add(unicodeChar, cyrilicChar);

            if (dataGrid.Rows.Count == 2)
            {
                string unicodeStr = dataGrid.Rows[0].Cells["Юникод-символ"].Value as string;
                string cyrilicStr = dataGrid.Rows[0].Cells["Замена"].Value as string;
                if (String.IsNullOrEmpty(unicodeStr) && String.IsNullOrEmpty(cyrilicStr))
                    dataGrid.Rows.RemoveAt(0);
            }
        }

        private void EditEntryInTable(char newUnicodeChar, string newCyrilicChar)
        {
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string unicodeChar = dataGrid.Rows[i].Cells["Юникод-символ"].Value as string;
                if (!String.IsNullOrEmpty(unicodeChar) && newUnicodeChar.Equals(unicodeChar[0]))
                {
                    dataGrid.Rows[i].Cells["Замена"].Value = newCyrilicChar;
                    break;
                }
            }
        }

        public bool SaveChanges() // Saving changes from DataGrid to Dictionary
        {
            int entriesAdded = 0;
            int entriesEdited = 0;
            int entriesDeleted = 0;

            // Writing entries to Dictionary
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string newUnicodeChar = dataGrid.Rows[i].Cells["Юникод-символ"].Value as string;
                string newCyrilicStr = dataGrid.Rows[i].Cells["Замена"].Value as string;

                // Deleting incomplete pairs
                if (String.IsNullOrEmpty(newUnicodeChar) || String.IsNullOrEmpty(newCyrilicStr))
                {
                    if (dataGrid.Rows.Count > 1)
                    {
                        dataGrid.Rows.RemoveAt(i--);
                        continue;
                    }
                    else if (dataGrid.Rows.Count == 1)
                    {
                        dataGrid.Rows[0].Cells["Юникод-символ"].Value = string.Empty;
                        dataGrid.Rows[0].Cells["Замена"].Value = string.Empty;
                        break;
                    }
                }

                // Filtering wrong length of inputs
                if (newUnicodeChar.Length != 1 || !Enumerable.Range(1, 5).Contains(newCyrilicStr.Length))
                    continue;

                if (dict.ContainsKey(newUnicodeChar[0]))
                {
                    // Editing entries in Dictionary, DB and DataTable
                    string oldCyrilicStr = dict[newUnicodeChar[0]];

                    if (!newCyrilicStr.Equals(oldCyrilicStr))
                    {
                        dict[newUnicodeChar[0]] = newCyrilicStr;
                        EditEntryToReplaceCharsDB(newUnicodeChar[0], newCyrilicStr);
                        entriesEdited++;
                    }
                }
                else
                {
                    // Adding entries to Dictionary, DB and DataTable
                    dict.Add(newUnicodeChar[0], newCyrilicStr);
                    AddEntryToReplaceCharsDB(newUnicodeChar[0], newCyrilicStr);
                    entriesAdded++;
                }
            }

            // Deleting entries from Dictionary
            foreach (char unicodeChar in dict.Keys)
            {
                bool existInDataGrid = false;
                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    DataGridViewCell cell = dataGrid.Rows[i].Cells["Юникод-символ"];
                    if (unicodeChar.ToString().Equals(cell.Value as string))
                    {
                        existInDataGrid = true;
                        break;
                    }
                }

                if (!existInDataGrid)
                {
                    dict.Remove(unicodeChar);
                    DeleteEntryFromReplaceCharsDB(unicodeChar);
                    entriesDeleted++;
                }
            }

            // Notifing about changes
            string entriesAddedMes = String.Format("Добавлено записей: {0}", entriesAdded);
            string entriesEditedMes = String.Format("Отредактировано записей: {0}", entriesEdited);
            string entriesDeletedMes = String.Format("Удалено записей: {0}", entriesDeleted);
            int changesCount = entriesAdded + entriesEdited + entriesDeleted;
            if (changesCount > 0)
                MessageBox.Show(String.Join("\n", new string[] { entriesAddedMes, entriesEditedMes, entriesDeletedMes }), progName);

            HasChanges = false;
            return changesCount > 0;
        }

        // Database operations
        private async void ReadReplaceCharsDB()
        {
            dict.Clear();

            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                SqliteDataReader dataReader = null;

                try
                {
                    SqliteCommand command = new SqliteCommand(
                        "CREATE TABLE IF NOT EXISTS ReplaceChars(Id INT NOT NULL PRIMARY KEY, UnicodeChar CHAR NOT NULL, CyrilicChar CHAR(5) NOT NULL, UNIQUE(Id, UnicodeChar))",
                        connection);
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "SELECT * FROM ReplaceChars";

                    dataReader = await command.ExecuteReaderAsync();

                    while (await dataReader.ReadAsync())
                    {
                        string unicodeChar = Convert.ToString(dataReader["UnicodeChar"]);
                        string cyrilicStr = Convert.ToString(dataReader["CyrilicChar"]);
                        dict.Add(unicodeChar[0], cyrilicStr);
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

        private async void AddEntryToReplaceCharsDB(char unicodeChar, string cyrilicStr)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                try
                {
                    using (SqliteCommand command = new SqliteCommand())
                    {
                        command.Connection = connection;

                        // Adding entry into DB
                        command.CommandText = "INSERT INTO [ReplaceChars] (Id, UnicodeChar, CyrilicChar) " +
                                              "VALUES (@Id, @UnicodeChar, @CyrilicChar)";

                        command.Parameters.AddWithValue("@Id", (int)unicodeChar);
                        command.Parameters.AddWithValue("@UnicodeChar", unicodeChar);
                        command.Parameters.AddWithValue("@CyrilicChar", cyrilicStr);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
            }
        }

        private async void EditEntryToReplaceCharsDB(char unicodeChar, string cyrilicStr)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                try
                {
                    using (SqliteCommand command = new SqliteCommand())
                    {
                        command.Connection = connection;

                        // Updating entry in DB
                        command.CommandText = "UPDATE ReplaceChars SET CyrilicChar = @CyrilicChar WHERE UnicodeChar LIKE @UnicodeChar";
                        command.Parameters.AddWithValue("@UnicodeChar", unicodeChar);
                        command.Parameters.AddWithValue("@CyrilicChar", cyrilicStr);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
            }
        }

        private async void DeleteEntryFromReplaceCharsDB(char unicodeChar)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                try
                {
                    SqliteCommand command = new SqliteCommand(
                        "DELETE FROM ReplaceChars WHERE UnicodeChar = @UnicodeChar",
                        connection);

                    command.Parameters.AddWithValue("@UnicodeChar", unicodeChar);

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

            // Checking correction of user input
            if (dataGrid.Columns[colInd].HeaderText.Equals("Юникод символ"))
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
                    if (dict.ContainsKey(userInputStr[0]))
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

                if (userInputStr.Length > maxReplChars)
                {
                    dataGrid.Rows[rowInd].ErrorText = String.Format("Введите не более {0} символов", maxReplChars);
                    dataGrid.CancelEdit();
                }
                else if (!TextHandlers.IsCharCyrilic(userInputStr[0]))
                {
                    dataGrid.Rows[rowInd].ErrorText = "Введите символы, входящие в кириллическую кодировку";
                    dataGrid.CancelEdit();
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
                    dataTable.Rows.RemoveAt(rowInd);
                    HasChanges = true;
                }
            }

            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void btnAcceptChanges_Click(object sender, EventArgs e)
        {
            bool madeChanged = SaveChanges();
            if (madeChanged)
                filenameChars.UpdateReplaceRow();

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
