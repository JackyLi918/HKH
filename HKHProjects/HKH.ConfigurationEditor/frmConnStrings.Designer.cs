namespace HKH.ConfigurationEditor
{
	partial class frmConnStrings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.cbSectionName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbDPAPI = new System.Windows.Forms.RadioButton();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.rbHKH = new System.Windows.Forms.RadioButton();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cbConfigs = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtConfigFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPlain = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Controls.Add(this.cbSectionName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rbDPAPI);
            this.groupBox1.Controls.Add(this.btnSaveAs);
            this.groupBox1.Controls.Add(this.rbHKH);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.cbConfigs);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.txtConfigFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtPlain);
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 377);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit Connection String";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Location = new System.Drawing.Point(112, 306);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 20;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // cbSectionName
            // 
            this.cbSectionName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSectionName.FormattingEnabled = true;
            this.cbSectionName.Items.AddRange(new object[] {
            "connectionStrings",
            "hkhConnectionStrings"});
            this.cbSectionName.Location = new System.Drawing.Point(76, 76);
            this.cbSectionName.Name = "cbSectionName";
            this.cbSectionName.Size = new System.Drawing.Size(129, 21);
            this.cbSectionName.TabIndex = 19;
            this.cbSectionName.SelectedIndexChanged += new System.EventHandler(this.cbSectionName_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Section";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Protected By";
            // 
            // rbDPAPI
            // 
            this.rbDPAPI.AutoSize = true;
            this.rbDPAPI.Location = new System.Drawing.Point(307, 306);
            this.rbDPAPI.Name = "rbDPAPI";
            this.rbDPAPI.Size = new System.Drawing.Size(132, 17);
            this.rbDPAPI.TabIndex = 16;
            this.rbDPAPI.Text = "Windows Data Protect";
            this.rbDPAPI.UseVisualStyleBackColor = true;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(273, 339);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(56, 25);
            this.btnSaveAs.TabIndex = 11;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // rbHKH
            // 
            this.rbHKH.AutoSize = true;
            this.rbHKH.Location = new System.Drawing.Point(171, 306);
            this.rbHKH.Name = "rbHKH";
            this.rbHKH.Size = new System.Drawing.Size(128, 17);
            this.rbHKH.TabIndex = 15;
            this.rbHKH.Text = "HKH Custom Provider";
            this.rbHKH.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(446, 75);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(37, 23);
            this.btnEdit.TabIndex = 10;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // cbConfigs
            // 
            this.cbConfigs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigs.FormattingEnabled = true;
            this.cbConfigs.Location = new System.Drawing.Point(275, 76);
            this.cbConfigs.Name = "cbConfigs";
            this.cbConfigs.Size = new System.Drawing.Size(165, 21);
            this.cbConfigs.TabIndex = 9;
            this.cbConfigs.SelectedIndexChanged += new System.EventHandler(this.cbConfigs_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Connection";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(446, 36);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(37, 23);
            this.btnSelect.TabIndex = 7;
            this.btnSelect.Text = "...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtConfigFile
            // 
            this.txtConfigFile.Location = new System.Drawing.Point(76, 37);
            this.txtConfigFile.Name = "txtConfigFile";
            this.txtConfigFile.ReadOnly = true;
            this.txtConfigFile.Size = new System.Drawing.Size(364, 20);
            this.txtConfigFile.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Config File";
            // 
            // txtPlain
            // 
            this.txtPlain.BackColor = System.Drawing.SystemColors.Window;
            this.txtPlain.Location = new System.Drawing.Point(17, 121);
            this.txtPlain.Multiline = true;
            this.txtPlain.Name = "txtPlain";
            this.txtPlain.ReadOnly = true;
            this.txtPlain.Size = new System.Drawing.Size(466, 170);
            this.txtPlain.TabIndex = 3;
            this.txtPlain.Text = "Here is plain text";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(419, 339);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(56, 25);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(346, 339);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmConnStrings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 399);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConnStrings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration Edtior - Connection String";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbSectionName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbDPAPI;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.RadioButton rbHKH;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ComboBox cbConfigs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtConfigFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPlain;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.RadioButton rbNone;

    }
}

