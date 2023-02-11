using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace HKH.Tools
{
    public partial class frmJsonValidate : Form
    {
        public frmJsonValidate()
        {
            InitializeComponent();
        }

        private void btnSelectSchema_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "json file(*.json)|*.json";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtSchemaFile.Text = ofd.FileName;
            }
        }

        private void btnSelectJson_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "json file(*.json)|*.json";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtJsonFile.Text = ofd.FileName;
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSchemaFile.Text) && !string.IsNullOrEmpty(txtJsonFile.Text))
            {
                string data = File.ReadAllText(txtJsonFile.Text);
                string schema = File.ReadAllText(txtSchemaFile.Text);

                var model = JObject.Parse(data);
                var json_schema = JSchema.Parse(schema);

                IList<string> messages;
                if (model.IsValid(json_schema, out messages))
                {
                    MessageBox.Show("Valid");
                }
                else
                {
                    MessageBox.Show("error");
                }
            }
            else
                MessageBox.Show("Please select schema and json file.");
        }
    }
}