using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace HKH.WCF
{
    public partial class Form1 : Form
    {
        private ServiceHost _host = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_host == null)
                _host = new ServiceHost(typeof(BroadCastService));

            if (_host.State != CommunicationState.Opened)
            {
                _host.Open();
                HKH.Tasks.ParallelTaskPool.Start();
                this.label1.Text = "BroadCastService Opened.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_host != null && _host.State == CommunicationState.Opened)
            {
                _host.Close();
                _host = null;
                this.label1.Text = "BroadCastService Closed.";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BroadCastManager.BroadCastServer (new BroadCastMessage { Action = "Test", Body = "test broadcast" });
        }
    }
}
