using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HKH.Common;

namespace HKH.Tools
{
    public partial class frmHexImage : Form
    {
        public frmHexImage()
        {
            InitializeComponent();
        }

        private void btnHexToImage_Click(object sender, EventArgs e)
        {
            string hexStr = txtHexString.Text.Trim();
            if (string.IsNullOrEmpty(hexStr))
            {
                pbImage.Image = null;
                return;
            }

            try
            {
                pbImage.Image = hexStr.ToImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Invalid Hex string", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbImage.Image = null;
            }
        }

        private void btnImageToHex_Click(object sender, EventArgs e)
        {
            Image image = pbImage.Image;
            if (image==null)
            {
                txtHexString.Text = string.Empty;
                return;
            }

            try
            {
                txtHexString.Text = image.ToHexString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHexString.Text = string.Empty;
            }
        }
    }
}
