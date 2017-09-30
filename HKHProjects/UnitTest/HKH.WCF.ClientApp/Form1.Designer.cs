namespace HKH.WCF.ClientApp
{
	partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnRc = new System.Windows.Forms.Button();
            this.btnUr = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(44, 48);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(612, 500);
            this.textBox1.TabIndex = 0;
            // 
            // btnRc
            // 
            this.btnRc.Location = new System.Drawing.Point(44, 578);
            this.btnRc.Name = "btnRc";
            this.btnRc.Size = new System.Drawing.Size(75, 23);
            this.btnRc.TabIndex = 1;
            this.btnRc.Text = "Register Client";
            this.btnRc.UseVisualStyleBackColor = true;
            this.btnRc.Click += new System.EventHandler(this.btnRc_Click);
            // 
            // btnUr
            // 
            this.btnUr.Location = new System.Drawing.Point(170, 577);
            this.btnUr.Name = "btnUr";
            this.btnUr.Size = new System.Drawing.Size(75, 23);
            this.btnUr.TabIndex = 2;
            this.btnUr.Text = "ClientBroadcast";
            this.btnUr.UseVisualStyleBackColor = true;
            this.btnUr.Click += new System.EventHandler(this.btnUr_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 625);
            this.Controls.Add(this.btnUr);
            this.Controls.Add(this.btnRc);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnRc;
        private System.Windows.Forms.Button btnUr;
	}
}

