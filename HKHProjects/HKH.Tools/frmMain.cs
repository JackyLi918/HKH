using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HKH.Tools
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void tsbAppSetting_Click(object sender, EventArgs e)
        {
            new frmAppSetting().ShowDialog();
        }

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			new frmGenerateProxy().ShowDialog();
		}
    }
}
