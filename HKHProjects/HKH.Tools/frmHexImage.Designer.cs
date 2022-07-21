
namespace HKH.Tools
{
    partial class frmHexImage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtHexString = new System.Windows.Forms.TextBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.btnHexToImage = new System.Windows.Forms.Button();
            this.btnImageToHex = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHexString
            // 
            this.txtHexString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHexString.Location = new System.Drawing.Point(3, 3);
            this.txtHexString.Multiline = true;
            this.txtHexString.Name = "txtHexString";
            this.tableLayoutPanel1.SetRowSpan(this.txtHexString, 4);
            this.txtHexString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHexString.Size = new System.Drawing.Size(475, 619);
            this.txtHexString.TabIndex = 0;
            // 
            // pbImage
            // 
            this.pbImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbImage.Location = new System.Drawing.Point(584, 3);
            this.pbImage.Name = "pbImage";
            this.tableLayoutPanel1.SetRowSpan(this.pbImage, 4);
            this.pbImage.Size = new System.Drawing.Size(475, 619);
            this.pbImage.TabIndex = 1;
            this.pbImage.TabStop = false;
            // 
            // btnHexToImage
            // 
            this.btnHexToImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHexToImage.Location = new System.Drawing.Point(484, 238);
            this.btnHexToImage.Name = "btnHexToImage";
            this.btnHexToImage.Size = new System.Drawing.Size(94, 23);
            this.btnHexToImage.TabIndex = 2;
            this.btnHexToImage.Text = "Hex To Image >";
            this.btnHexToImage.UseVisualStyleBackColor = true;
            this.btnHexToImage.Click += new System.EventHandler(this.btnHexToImage_Click);
            // 
            // btnImageToHex
            // 
            this.btnImageToHex.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnImageToHex.Location = new System.Drawing.Point(484, 363);
            this.btnImageToHex.Name = "btnImageToHex";
            this.btnImageToHex.Size = new System.Drawing.Size(94, 23);
            this.btnImageToHex.TabIndex = 3;
            this.btnImageToHex.Text = "< Image To Hex";
            this.btnImageToHex.UseVisualStyleBackColor = true;
            this.btnImageToHex.Click += new System.EventHandler(this.btnImageToHex_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.txtHexString, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbImage, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnImageToHex, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnHexToImage, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1062, 625);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // frmHexImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 649);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmHexImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图片/字符串转换";
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtHexString;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Button btnHexToImage;
        private System.Windows.Forms.Button btnImageToHex;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}