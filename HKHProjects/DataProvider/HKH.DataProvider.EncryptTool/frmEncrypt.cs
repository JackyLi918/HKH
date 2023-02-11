using System;
using System.Windows.Forms;
using HKH.Common.Security;
using HKH.Data.Configuration;
using Microsoft.Extensions.Configuration;

namespace HKH.DataProvider.EncryptTool
{
    public partial class frmEncrypt : Form
    {
        private IEncryption encryption = null;

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
            ofd.Filter = "json config file(*.json)|*.json";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtConfigFile.Text = ofd.FileName;
                //load configuration
                var configurationBuilder = new ConfigurationBuilder()
                 .AddJsonFile(txtConfigFile.Text, optional: false, reloadOnChange: true);

                IConfiguration configuration = configurationBuilder.Build();
                DataBaseConfigurationManager.Load(configuration);

                cbConfigs.SelectedIndexChanged -= cbConfigs_SelectedIndexChanged;
                cbConfigs.Items.Clear();
                foreach (var config in DataBaseConfigurationManager.Configurations.ConnectionStrings)
                {
                    cbConfigs.Items.Add(config.Name);
                    //default select 
                    if (config.IsDefault)
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
            var config = DataBaseConfigurationManager.GetConfiguration(cbConfigs.SelectedItem.ToString());
            encryption = config.GetEncryptionAlgo();
        }
    }
}
