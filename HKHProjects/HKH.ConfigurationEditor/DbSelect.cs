using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HKH.ConfigurationEditor.DbLocator;
using System.Data.SqlClient;

namespace HKH.ConfigurationEditor
{
    public partial class DbSelect : UserControl
    {
		SqlConnectionStringBuilder _connStrBuilder = new SqlConnectionStringBuilder ();
        public DbSelect()
        {
            InitializeComponent();
        }

		public event EventHandler ChangeAccepted;
		public event EventHandler ChangeCanceled;

		public SqlConnectionStringBuilder ConnectionStringBuilder
		{
			get
			{ return _connStrBuilder; 
			}
			set
			{
				_connStrBuilder = value;

				cbServerName.Text = _connStrBuilder.DataSource ;
				rbWindow.Checked = _connStrBuilder.IntegratedSecurity;
				rbSQLServer.Checked = !_connStrBuilder.IntegratedSecurity;
				if (rbSQLServer.Checked)
				{
					tbUserName.Text = _connStrBuilder.UserID;
					tbPassword.Text = _connStrBuilder.Password;
				}
				tbDbName.Text = _connStrBuilder.InitialCatalog;
			}
		}

        private void cbServerName_DropDown(object sender, EventArgs e)
        {
            if (cbServerName.Items.Count == 0)
                cbServerName.DataSource = SqlServerLocator.GetServers();
        }

        private void cbServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDbName.DataSource = null;
            cbDbName.Text = string.Empty;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cbServerName.DataSource = SqlServerLocator.GetServers();
        }

        private void cbDbName_DropDown(object sender, EventArgs e)
        {
            if (cbDbName.Items.Count == 0 && !(string.IsNullOrEmpty(cbServerName.Text.Trim()) || string.IsNullOrEmpty(tbUserName.Text.Trim()) || string.IsNullOrEmpty(tbPassword.Text.Trim())))
                cbDbName.DataSource = SqlServerLocator.GetDatabases(cbServerName.Text, rbSQLServer.Checked, tbUserName.Text, tbPassword.Text);
        }

        private void rbWindow_CheckedChanged(object sender, EventArgs e)
        {
            tbUserName.Enabled = !rbWindow.Checked;
            tbPassword.Enabled = !rbWindow.Checked;
            cbDbName.DataSource = null;
            cbDbName.Text = string.Empty;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if(SqlServerLocator.TestConnection(cbServerName.Text, cbDbName.Text, rbSQLServer.Checked, tbUserName.Text, tbPassword.Text))
                MessageBox.Show("Connect Success！");
            else
				MessageBox.Show("Connect Fail！");
        }

		private void btnOk_Click(object sender, EventArgs e)
		{
			_connStrBuilder.DataSource = cbServerName.Text;
			_connStrBuilder.InitialCatalog = tbDbName.Text;
			_connStrBuilder.IntegratedSecurity = rbWindow.Checked;
			if (!_connStrBuilder.IntegratedSecurity)
			{
				_connStrBuilder.UserID = tbUserName.Text;
				_connStrBuilder.Password = tbPassword.Text;
			}
			
			if (ChangeAccepted  != null)
				ChangeAccepted(sender, e);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (ChangeCanceled != null)
				ChangeCanceled(sender, e);
		}
    }
}
