using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using HKH.WCF;

namespace HKH.WCF.ClientApp
{
	public partial class Form1 : Form
	{
        Client c = null;
		public Form1()
		{
			InitializeComponent();
		}

		public void ShowMsg(string msg)
		{
			textBox1.Text += Environment.NewLine + msg;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			c = new Client();
			BroadCastManager.RegisterClient(c);
		}

        private void btnRc_Click(object sender, EventArgs e)
        {
            //BroadCastManager.RegisterClient(c);
        }

        private void btnUr_Click(object sender, EventArgs e)
        {
            BroadCastManager.BroadCastClient (new BroadCastMessage { Action = "aa", Body = "bb" });
        }
	}

	public class Client : IBroadCastReceiverCallback 
	{        
		public void Receive(BroadCastMessage message)
		{
			Form1 frm = Application.OpenForms[0] as Form1;
			frm.ShowMsg("Action:" + message.Action + "    Body:" + message.Body);
		}
	}
}
