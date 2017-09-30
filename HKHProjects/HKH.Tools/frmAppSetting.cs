using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace HKH.Tools
{
    public partial class frmAppSetting : Form
    {
        public frmAppSetting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //MSDASC.DataLinks mydlg = new MSDASC.DataLinks();
            //ADODB._Connection ADOcon;
            ////Cast the generic object that PromptNew returns to an ADODB._Connection.
            //ADOcon = (ADODB._Connection)mydlg.PromptNew();
            //ADOcon.Open("", "", "", 0);
            //if (ADOcon.State == 1)
            //{
            //    MessageBox.Show("Connection Opened");
            //    ADOcon.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Connection Failed");
            //}

			//MSDASC.DataLinks mydlg = new MSDASC.DataLinks();
			//OleDbConnection OleCon = new OleDbConnection();
			//ADODB._Connection ADOcon;
			////Cast the generic object that PromptNew returns to an ADODB._Connection.
			//ADOcon = (ADODB._Connection)mydlg.PromptNew();
			//OleCon.ConnectionString = ADOcon.ConnectionString;
			//OleCon.Open();
			//if (OleCon.State.ToString() == "Open")
			//{
			//	MessageBox.Show("Connection Opened");
			//	OleCon.Close();
			//}
			//else
			//{
			//	MessageBox.Show("Connection Failed");
			//}
        }
    }
}
