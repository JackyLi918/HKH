using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using HKH.Common.Security;
using HKH.Data.Configuration;

namespace HKH.DataProvider.EncryptTool
{
    public partial class frmEncrypt : Form
    {
        private IEncryption encryption = null;
        private XmlNode xnConnectionStrings = null;

        public frmEncrypt()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                tbTarget.Text = encryption.Encrypt(tbSource.Text.Trim());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                tbTarget.Text = encryption.Decrypt(tbSource.Text.Trim());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".Net config file(*.config)|*.config";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtConfigFile.Text = ofd.FileName;
                //load configuration
                ConfigXmlDocument cxd = new ConfigXmlDocument();
                cxd.Load(txtConfigFile.Text);
                xnConnectionStrings = cxd.SelectSingleNode("/configuration/" + HKHConnectionStringsSection.TagName);
                
                cbConfigs.SelectedIndexChanged -= cbConfigs_SelectedIndexChanged;
                cbConfigs.Items.Clear();
                foreach (XmlNode xnConfig in xnConnectionStrings.ChildNodes)
                {
                    cbConfigs.Items.Add(xnConfig.Attributes["name"].Value);
                    //default select 
                    if (xnConfig.Attributes["isDefault"] != null && xnConfig.Attributes["isDefault"].Value.ToLower() == "true")
                        cbConfigs.SelectedIndex = cbConfigs.Items.Count - 1;
                }

                //select first item if no default setting
                if (cbConfigs.Items.Count > 0 && cbConfigs.SelectedIndex == -1)
                    cbConfigs.SelectedIndex = 0;
                cbConfigs.SelectedIndexChanged += cbConfigs_SelectedIndexChanged;

                cbConfigs_SelectedIndexChanged(cbConfigs, EventArgs.Empty);
            }
        }

        private void cbConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlNode xnConfig = xnConnectionStrings.SelectSingleNode("add[@name=\"" + cbConfigs.SelectedItem + "\"]");
            HKHConnectionStringElement hse = new HKHConnectionStringElement(xnConfig.Attributes["name"].Value, xnConfig.Attributes["connectionString"].Value);
            //set encryption
            if (xnConfig.Attributes["encrypt"] != null && xnConfig.Attributes["encrypt"].Value.ToLower() == "true")
                hse.Encrypt = true;
            if (xnConfig.Attributes["algo"] != null)
                hse.Algo = xnConfig.Attributes["algo"].Value;

            encryption = hse.GetEncryptionAlgo();
        }
    }
}
