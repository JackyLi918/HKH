namespace HKH.DataProvider.EncryptTool
{
	partial class frmEncrypt
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbConfigs = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSelect = new System.Windows.Forms.Button();
			this.txtConfigFile = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbTarget = new System.Windows.Forms.TextBox();
			this.tbSource = new System.Windows.Forms.TextBox();
			this.btnQuit = new System.Windows.Forms.Button();
			this.btnDecrypt = new System.Windows.Forms.Button();
			this.btnEncrypt = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbConfigs);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.btnSelect);
			this.groupBox1.Controls.Add(this.txtConfigFile);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.tbTarget);
			this.groupBox1.Controls.Add(this.tbSource);
			this.groupBox1.Controls.Add(this.btnQuit);
			this.groupBox1.Controls.Add(this.btnDecrypt);
			this.groupBox1.Controls.Add(this.btnEncrypt);
			this.groupBox1.Location = new System.Drawing.Point(25, 17);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(507, 388);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Encrypt/Decrypt Connection String";
			// 
			// cbConfigs
			// 
			this.cbConfigs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbConfigs.FormattingEnabled = true;
			this.cbConfigs.Location = new System.Drawing.Point(92, 85);
			this.cbConfigs.Name = "cbConfigs";
			this.cbConfigs.Size = new System.Drawing.Size(327, 21);
			this.cbConfigs.TabIndex = 9;
			this.cbConfigs.SelectedIndexChanged += new System.EventHandler(this.cbConfigs_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(25, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Connection";
			// 
			// btnSelect
			// 
			this.btnSelect.Location = new System.Drawing.Point(429, 37);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(46, 23);
			this.btnSelect.TabIndex = 7;
			this.btnSelect.Text = "...";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// txtConfigFile
			// 
			this.txtConfigFile.Location = new System.Drawing.Point(92, 37);
			this.txtConfigFile.Name = "txtConfigFile";
			this.txtConfigFile.ReadOnly = true;
			this.txtConfigFile.Size = new System.Drawing.Size(327, 20);
			this.txtConfigFile.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Config File";
			// 
			// tbTarget
			// 
			this.tbTarget.BackColor = System.Drawing.SystemColors.Window;
			this.tbTarget.Location = new System.Drawing.Point(23, 238);
			this.tbTarget.Multiline = true;
			this.tbTarget.Name = "tbTarget";
			this.tbTarget.ReadOnly = true;
			this.tbTarget.Size = new System.Drawing.Size(460, 85);
			this.tbTarget.TabIndex = 4;
			// 
			// tbSource
			// 
			this.tbSource.Location = new System.Drawing.Point(23, 132);
			this.tbSource.Multiline = true;
			this.tbSource.Name = "tbSource";
			this.tbSource.Size = new System.Drawing.Size(460, 85);
			this.tbSource.TabIndex = 3;
			this.tbSource.Text = "Here is source string to encrypt/decrypt.";
			// 
			// btnQuit
			// 
			this.btnQuit.Location = new System.Drawing.Point(400, 341);
			this.btnQuit.Name = "btnQuit";
			this.btnQuit.Size = new System.Drawing.Size(75, 25);
			this.btnQuit.TabIndex = 2;
			this.btnQuit.Text = "Quit";
			this.btnQuit.UseVisualStyleBackColor = true;
			this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
			// 
			// btnDecrypt
			// 
			this.btnDecrypt.Location = new System.Drawing.Point(302, 341);
			this.btnDecrypt.Name = "btnDecrypt";
			this.btnDecrypt.Size = new System.Drawing.Size(75, 25);
			this.btnDecrypt.TabIndex = 1;
			this.btnDecrypt.Text = "Decrypt";
			this.btnDecrypt.UseVisualStyleBackColor = true;
			this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
			// 
			// btnEncrypt
			// 
			this.btnEncrypt.Location = new System.Drawing.Point(204, 341);
			this.btnEncrypt.Name = "btnEncrypt";
			this.btnEncrypt.Size = new System.Drawing.Size(75, 25);
			this.btnEncrypt.TabIndex = 0;
			this.btnEncrypt.Text = "Encrypt";
			this.btnEncrypt.UseVisualStyleBackColor = true;
			this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
			// 
			// frmEncrypt
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(556, 423);
			this.Controls.Add(this.groupBox1);
			this.Name = "frmEncrypt";
			this.Text = "Encrypt";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox tbTarget;
		private System.Windows.Forms.TextBox tbSource;
		private System.Windows.Forms.Button btnQuit;
		private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.ComboBox cbConfigs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtConfigFile;
        private System.Windows.Forms.Label label1;
	}
}

