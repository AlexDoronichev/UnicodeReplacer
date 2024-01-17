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
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace UnicodeReplacer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            fileParams.SetControlsLinks(filenameChars, replaceChars, replaceFilenames);
            filenameChars.SetControlsLinks(fileParams, replaceChars, replaceFilenames);
            replaceChars.SetControlsLinks(fileParams, filenameChars, replaceFilenames);
            replaceFilenames.SetControlsLinks(fileParams, filenameChars, replaceChars);

            // Setting controls
            splitContainer1.Panel1Collapsed = true;
            splitContainer2.Panel2Collapsed = true;
        }

        private void btnReplaceCharsTable_Click(object sender, EventArgs e)
        {
            splitContainer2.Panel2Collapsed = !splitContainer2.Panel2Collapsed;
        }

        private void btnReplaceFilenamesTable_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
        }
    }
}
