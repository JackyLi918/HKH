using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HKH.Tools.DbLocator;

namespace HKH.Tools
{
    public partial class DbSelect : UserControl
    {
        public DbSelect()
        {
            InitializeComponent();
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
                MessageBox.Show("连接成功！");
            else
                MessageBox.Show("连接失败！");
        }

    }
}
