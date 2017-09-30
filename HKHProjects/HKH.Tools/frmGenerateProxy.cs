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
	public partial class frmGenerateProxy : Form
	{
		public frmGenerateProxy()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				HKH.WCF.WebServiceProxyGenerator.Generate(textBox1.Text, textBox2.Text, textBox3.Text);
				MessageBox.Show("OK");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
