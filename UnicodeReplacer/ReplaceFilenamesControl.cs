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
    public partial class ReplaceFilenamesControl : UserControl
    {
        string progName = "UnicodeReplacer";

        DataSet dataSet = new DataSet("ReplaceFilenamesStore");
        DataTable dataTable = new DataTable("ReplaceFilenamesTable");

        FileParamsControl fileParams;
        FilenameCharsControl filenameChars;
        ReplaceCharsControl replaceChars;

        public Dictionary<string, string> dict = new Dictionary<string, string>();

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

        public ReplaceFilenamesControl()
        {
            InitializeComponent();

            // Display options
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            // Data sources
            dataSet.Tables.Add(dataTable);
            dataGrid.DataSource = dataSet.Tables[0];

            // DataTable columns
            dataTable.Columns.Add("Название", typeof(string));
            dataTable.Columns.Add("Замена", typeof(string));

            // Parameters
            dataGrid.RowHeadersWidth = 50;
            dataGrid.Columns["Название"].HeaderText = "Название";
            dataGrid.Columns["Название"].Width = 80;
            //dataGrid.AutoResizeColumn(0);
            dataGrid.Columns["Замена"].HeaderText = "Замена";
            dataGrid.Columns["Замена"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Getting content for table
            ReadReplaceFilenamesDB();
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

        public void SetControlsLinks(FileParamsControl fileParams, FilenameCharsControl filenameChars, ReplaceCharsControl replaceChars)
        {
            this.fileParams = fileParams;
            this.filenameChars = filenameChars;
            this.replaceChars = replaceChars;
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

        public void EditEntry(string unicodeName, string cyrilicName)
        {
            if (dict.ContainsKey(unicodeName))
            {
                dict[unicodeName] = cyrilicName;
                EditEntryInTable(unicodeName, cyrilicName);
                EditEntryToReplaceFilenamesDB(unicodeName, cyrilicName);
            }
            else
            {
                dict.Add(unicodeName, cyrilicName);
                AddEntryToDataTable(unicodeName, cyrilicName);
                AddEntryToReplaceFilenamesDB(unicodeName, cyrilicName);
            }

            btnDeleteRow.Enabled = dataGrid.Rows.Count > 1;
        }

        private void AddEntryToDataTable(string unicodeName, string cyrilicName)
        {
            dataTable.Rows.Add(unicodeName, cyrilicName);

            if (dataGrid.Rows.Count == 2)
            {
                string unicodeStr = dataGrid.Rows[0].Cells["Название"].Value as string;
                string cyrilicStr = dataGrid.Rows[0].Cells["Замена"].Value as string;
                if (String.IsNullOrEmpty(unicodeStr) && String.IsNullOrEmpty(cyrilicStr))
                    dataGrid.Rows.RemoveAt(0);
            }
        }

        private void EditEntryInTable(string newUnicodeName, string newCyrilicName)
        {
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                string unicodeName = dataGrid.Rows[i].Cells["Название"].Value as string;
                if (!String.IsNullOrEmpty(unicodeName) && newUnicodeName.Equals(unicodeName))
                {
                    dataGrid.Rows[i].Cells["Замена"].Value = newCyrilicName;
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
                string newUnicodeName = dataGrid.Rows[i].Cells["Название"].Value as string;
                string newCyrilicName = dataGrid.Rows[i].Cells["Замена"].Value as string;

                // Deleting incomplete pairs
                if (String.IsNullOrEmpty(newUnicodeName) || String.IsNullOrEmpty(newCyrilicName))
                {
                    if (dataGrid.Rows.Count > 1)
                    {
                        dataGrid.Rows.RemoveAt(i--);
                        continue;
                    }
                    else if (dataGrid.Rows.Count == 1)
                    {
                        dataGrid.Rows[0].Cells["Название"].Value = string.Empty;
                        dataGrid.Rows[0].Cells["Замена"].Value = string.Empty;
                        break;
                    }
                }

                if (dict.ContainsKey(newUnicodeName))
                {
                    // Adding entries to Dictionary
                    string oldCyrilicStr = dict[newUnicodeName];

                    if (!newCyrilicName.Equals(oldCyrilicStr))
                    {
                        dict[newUnicodeName] = newCyrilicName;
                        EditEntryToReplaceFilenamesDB(newUnicodeName, newCyrilicName);
                        entriesEdited++;
                    }
                }
                else
                {
                    // Editing entries of Dictionary
                    dict.Add(newUnicodeName, newCyrilicName);
                    AddEntryToReplaceFilenamesDB(newUnicodeName, newCyrilicName);
                    entriesAdded++;
                }
            }

            // Deleting entries from Dictionary
            foreach (string unicodeName in dict.Keys)
            {
                bool existInDataGrid = false;
                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    DataGridViewCell cell = dataGrid.Rows[i].Cells["Название"];
                    if (unicodeName.Equals(cell.Value as string))
                    {
                        existInDataGrid = true;
                        break;
                    }
                }

                if (!existInDataGrid)
                {
                    dict.Remove(unicodeName);
                    DeleteEntryFromReplaceFilenamesDB(unicodeName);
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
        private async void ReadReplaceFilenamesDB()
        {
            dict.Clear();

            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                SqliteDataReader dataReader = null;

                try
                {
                    SqliteCommand command = new SqliteCommand(
                        "CREATE TABLE IF NOT EXISTS ReplaceFilenames(Id INTEGER PRIMARY KEY, UnicodeName VARCHAR NOT NULL, CyrilicName VARCHAR NOT NULL, UNIQUE(UnicodeName))",
                        connection);
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "SELECT * FROM ReplaceFilenames";

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

        private async void AddEntryToReplaceFilenamesDB(string unicodeName, string cyrilicName)
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
                        command.CommandText = "INSERT INTO [ReplaceFilenames] (UnicodeName, CyrilicName) " +
                                              "VALUES (@UnicodeName, @CyrilicName)";
                        command.Parameters.AddWithValue("@UnicodeName", unicodeName);
                        command.Parameters.AddWithValue("@CyrilicName", cyrilicName);

                        await command.ExecuteScalarAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
            }
        }

        private async void EditEntryToReplaceFilenamesDB(string unicodeName, string cyrilicName)
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
                        command.CommandText = "UPDATE ReplaceFilenames SET CyrilicName = @CyrilicName WHERE UnicodeName LIKE @UnicodeName";
                        command.Parameters.AddWithValue("@UnicodeName", unicodeName);
                        command.Parameters.AddWithValue("@CyrilicName", cyrilicName);

                        await command.ExecuteScalarAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, progName);
                }
            }
        }

        private async void DeleteEntryFromReplaceFilenamesDB(string unicodeName)
        {
            using (SqliteConnection connection = new SqliteConnection("Data Source=ReplaceDB.db"))
            {
                await connection.OpenAsync();

                try
                {
                    SqliteCommand command = new SqliteCommand(
                        "DELETE FROM ReplaceFilenames WHERE UnicodeName = @UnicodeName",
                        connection);

                    command.Parameters.AddWithValue("@UnicodeName", unicodeName);

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
            if (dataGrid.Columns[colInd].HeaderText.Equals("Название"))
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
                fileParams.UpdateSelectedCellInFileParamsTable();

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
