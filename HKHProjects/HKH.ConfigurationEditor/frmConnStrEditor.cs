using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HKH.ConfigurationEditor
{
	public partial class frmConnStrEditor : Form
	{
		public frmConnStrEditor()
		{
			InitializeComponent();
		}

		public SqlConnectionStringBuilder ConnectionStringBuilder
		{
			get { return dbSelect1.ConnectionStringBuilder; }
			set { dbSelect1.ConnectionStringBuilder = value; }
		}

		private void dbSelect1_ChangeAccepted(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void dbSelect1_ChangeCanceled(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
	}
}
