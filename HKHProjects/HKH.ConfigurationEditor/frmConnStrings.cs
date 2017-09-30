using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Xml;
using HKH.Data.Configuration;

namespace HKH.ConfigurationEditor
{
	public partial class frmConnStrings : Form
	{
		const string ProtectedProviderName_HKH = "HKHProtectedConfigurationProvider";
		const string ProtectedProviderName_DPAPI = "DataProtectionConfigurationProvider";

		private System.Configuration.Configuration config = null;
		private ConnectionStringsSection connStrings = null;
		private HKHConnectionStringsSection hkhConnStrings = null;
		private ConnectionStringSettings currentConnString = null;
		private string oriFileName = string.Empty;

		public frmConnStrings()
		{
			InitializeComponent();
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Configuration file(*.config)|*.config";
			ofd.Multiselect = false;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				txtConfigFile.Text = ofd.FileName;
				string filePath = ofd.FileName;

				//load configuration
				if (filePath.EndsWith(".exe.config", StringComparison.OrdinalIgnoreCase))
				{
					filePath = filePath.Substring(0, filePath.Length - 7);
					if (!File.Exists(filePath))
					{
						MessageBox.Show("The config is not a valid exe config.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					oriFileName = ofd.FileName;
					config = ConfigurationManager.OpenExeConfiguration(filePath);
				}
				//else if (oriFileName.EndsWith(@"\web.config", StringComparison.OrdinalIgnoreCase))
				//{
				//     oriFileName = ofd.FileName;
				//    config = WebConfigurationManager.OpenWebConfiguration(oriFileName.Substring(0, oriFileName.Length - 11));
				//}
				else
				{
					MessageBox.Show("The config is not a valid exe config.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					//MessageBox.Show("The config is not a valid exe or web config.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

                cbSectionName.SelectedIndex = 0;
				BindConnectionStrings();
			}
		}

		private void BindConnectionStrings()
		{
			connStrings = config.ConnectionStrings;

			if (connStrings != null)
			{
				if (connStrings.SectionInformation.IsProtected)
				{
					if (connStrings.SectionInformation.ProtectionProvider.Name == ProtectedProviderName_HKH)
					{
						rbHKH.Checked = true;
						rbDPAPI.Checked = false;
					}

					if (!connStrings.ElementInformation.IsLocked)
					{
						// Unprotect the section.
						connStrings.SectionInformation.UnprotectSection();
					}
					else
					{
						MessageBox.Show(string.Format("Can't unprotect, section {0} is locked", connStrings.SectionInformation.Name), "Error",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
				else
				{
					rbHKH.Checked = false;
					rbDPAPI.Checked = false;
					rbNone.Checked = true;
				}

				cbConfigs.SelectedIndexChanged -= cbConfigs_SelectedIndexChanged;
				cbConfigs.Items.Clear();

				if (connStrings.ConnectionStrings.Count > 0)
				{
					foreach (ConnectionStringSettings connString in connStrings.ConnectionStrings)
					{
						cbConfigs.Items.Add(connString.Name);
					}

					cbConfigs.SelectedIndex = 0;
				}

				cbConfigs.SelectedIndexChanged += cbConfigs_SelectedIndexChanged;
				cbConfigs_SelectedIndexChanged(cbConfigs, EventArgs.Empty);
			}
			else
			{
				MessageBox.Show("Can't get the section ConnectionStrings", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

		}

        private void cbSectionName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

		private void cbConfigs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbConfigs.SelectedItem != null && !string.IsNullOrEmpty(cbConfigs.SelectedItem.ToString()))
			{
				currentConnString = connStrings.ConnectionStrings[cbConfigs.SelectedItem.ToString()];
				txtPlain.Text = currentConnString.ConnectionString;
			}
			else
			{
				txtPlain.Text = string.Empty;
			}
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
            if (currentConnString == null)
                return;

			EntityConnectionStringBuilder entityConnStrBuilder = null;
			SqlConnectionStringBuilder sqlConnStrBuilder = null;

			string providerName = currentConnString.ProviderName;

			if (providerName == Constants.PROVIDERNAME_ENTITY)
			{
				entityConnStrBuilder = new EntityConnectionStringBuilder(txtPlain.Text);
				if (entityConnStrBuilder.Provider == Constants.PROVIDERNAME_SQLSERVER)
					sqlConnStrBuilder = new SqlConnectionStringBuilder(entityConnStrBuilder.ProviderConnectionString);
				else
				{
					MessageBox.Show("Only Sql-server connection string is supportted on this version.");
					return;
				}
			}
			else if (providerName == Constants.PROVIDERNAME_SQLSERVER)
				sqlConnStrBuilder = new SqlConnectionStringBuilder(txtPlain.Text);
			else
			{
				MessageBox.Show("Only Sql-server connection string is supportted on this version.");
				return;
			}

			frmConnStrEditor connStrEditor = new frmConnStrEditor();
			connStrEditor.ConnectionStringBuilder = sqlConnStrBuilder;
			if (connStrEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if (entityConnStrBuilder != null)
				{
					entityConnStrBuilder.ProviderConnectionString = sqlConnStrBuilder.ConnectionString;
					currentConnString.ConnectionString = entityConnStrBuilder.ConnectionString;
				}
				else
				{
					currentConnString.ConnectionString = sqlConnStrBuilder.ConnectionString;
				}

				txtPlain.Text = currentConnString.ConnectionString;
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveConfig(null);
		}

		private void btnSaveAs_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Configuration file(*.config)|*.config";
			sfd.DefaultExt = "config";
			sfd.CheckPathExists = true;
			sfd.FileName = oriFileName;

			if (sfd.ShowDialog() == DialogResult.OK)
				SaveConfig(sfd.FileName);
		}

		private void SaveConfig(string filePath)
		{
			if (connStrings.SectionInformation.IsProtected)
			{
				if (!connStrings.ElementInformation.IsLocked)
				{
					connStrings.SectionInformation.UnprotectSection();

					if (!rbNone.Checked)
					{
						//Protect the section.
						string provider = ProtectedProviderName_DPAPI;
						if (rbHKH.Checked)
						{
							EnsureHKHProvider();
							provider = ProtectedProviderName_HKH;
						}

						connStrings.SectionInformation.ProtectSection(provider);
					}

					connStrings.SectionInformation.ForceSave = true;

					if (string.IsNullOrEmpty(filePath))
						config.Save(ConfigurationSaveMode.Full);
					else
						config.SaveAs(filePath, ConfigurationSaveMode.Full);
				}
				else
				{
					MessageBox.Show(string.Format("Can't protect, section {0} is locked", connStrings.SectionInformation.Name), "Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			else
			{
				if (!connStrings.ElementInformation.IsLocked)
				{
					if (!rbNone.Checked)
					{
						//Protect the section.
						string provider = ProtectedProviderName_DPAPI;
						if (rbHKH.Checked)
						{
							EnsureHKHProvider();
							provider = ProtectedProviderName_HKH;
						}

						connStrings.SectionInformation.ProtectSection(provider);
					}

					connStrings.SectionInformation.ForceSave = true;

					if (string.IsNullOrEmpty(filePath))
						config.Save(ConfigurationSaveMode.Full);
					else
						config.SaveAs(filePath, ConfigurationSaveMode.Full);
				}
				else
				{
					MessageBox.Show(string.Format("Can't protect, section {0} is locked", connStrings.SectionInformation.Name), "Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			MessageBox.Show("Save Success");
		}

		private void EnsureHKHProvider()
		{
			ProtectedConfigurationSection providerSection = config.GetSection("configProtectedData") as ProtectedConfigurationSection;
			bool hasHKHProvider = false;
			foreach (ProviderSettings provider in providerSection.Providers)
			{
				if (provider.Name == ProtectedProviderName_HKH)
				{
					hasHKHProvider = true;
					break;
				}
			}

			if (!hasHKHProvider)
			{
				providerSection.Providers.Add(new ProviderSettings(ProtectedProviderName_HKH, "HKH.Configuration.HKHProtectedConfigurationProvider, HKH.Common"));
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
			Application.Exit();
		}
	}
}
