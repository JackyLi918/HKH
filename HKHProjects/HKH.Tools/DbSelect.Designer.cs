namespace HKH.Tools
{
    partial class DbSelect
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblServerName = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblDbName = new System.Windows.Forms.Label();
            this.tbDbName = new System.Windows.Forms.TextBox();
            this.cbServerName = new System.Windows.Forms.ComboBox();
            this.cbDbName = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.rbWindow = new System.Windows.Forms.RadioButton();
            this.rbSQLServer = new System.Windows.Forms.RadioButton();
            this.gbLoginModel = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gbLoginModel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(33, 21);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(53, 12);
            this.lblServerName.TabIndex = 0;
            this.lblServerName.Text = "服务器：";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(29, 74);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(53, 12);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "用户名：";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(29, 109);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 12);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "密  码：";
            // 
            // tbUserName
            // 
            this.tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUserName.Enabled = false;
            this.tbUserName.Location = new System.Drawing.Point(82, 74);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(100, 21);
            this.tbUserName.TabIndex = 3;
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Enabled = false;
            this.tbPassword.Location = new System.Drawing.Point(82, 106);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 21);
            this.tbPassword.TabIndex = 4;
            // 
            // lblDbName
            // 
            this.lblDbName.AutoSize = true;
            this.lblDbName.Location = new System.Drawing.Point(33, 220);
            this.lblDbName.Name = "lblDbName";
            this.lblDbName.Size = new System.Drawing.Size(53, 12);
            this.lblDbName.TabIndex = 5;
            this.lblDbName.Text = "数据库：";
            // 
            // tbDbName
            // 
            this.tbDbName.Location = new System.Drawing.Point(86, 220);
            this.tbDbName.Name = "tbDbName";
            this.tbDbName.Size = new System.Drawing.Size(100, 21);
            this.tbDbName.TabIndex = 6;
            // 
            // cbServerName
            // 
            this.cbServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbServerName.FormattingEnabled = true;
            this.cbServerName.Location = new System.Drawing.Point(86, 21);
            this.cbServerName.Name = "cbServerName";
            this.cbServerName.Size = new System.Drawing.Size(121, 20);
            this.cbServerName.TabIndex = 7;
            this.cbServerName.DropDown += new System.EventHandler(this.cbServerName_DropDown);
            this.cbServerName.SelectedIndexChanged += new System.EventHandler(this.cbServerName_SelectedIndexChanged);
            // 
            // cbDbName
            // 
            this.cbDbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDbName.FormattingEnabled = true;
            this.cbDbName.Location = new System.Drawing.Point(86, 221);
            this.cbDbName.Name = "cbDbName";
            this.cbDbName.Size = new System.Drawing.Size(121, 20);
            this.cbDbName.TabIndex = 8;
            this.cbDbName.DropDown += new System.EventHandler(this.cbDbName_DropDown);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(35, 257);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "测试连接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // rbWindow
            // 
            this.rbWindow.AutoSize = true;
            this.rbWindow.Checked = true;
            this.rbWindow.Location = new System.Drawing.Point(32, 20);
            this.rbWindow.Name = "rbWindow";
            this.rbWindow.Size = new System.Drawing.Size(113, 16);
            this.rbWindow.TabIndex = 11;
            this.rbWindow.TabStop = true;
            this.rbWindow.Text = "Windows方式登录";
            this.rbWindow.UseVisualStyleBackColor = true;
            this.rbWindow.CheckedChanged += new System.EventHandler(this.rbWindow_CheckedChanged);
            // 
            // rbSQLServer
            // 
            this.rbSQLServer.AutoSize = true;
            this.rbSQLServer.Location = new System.Drawing.Point(32, 42);
            this.rbSQLServer.Name = "rbSQLServer";
            this.rbSQLServer.Size = new System.Drawing.Size(131, 16);
            this.rbSQLServer.TabIndex = 12;
            this.rbSQLServer.TabStop = true;
            this.rbSQLServer.Text = "SQL Server方式登录";
            this.rbSQLServer.UseVisualStyleBackColor = true;
            // 
            // gbLoginModel
            // 
            this.gbLoginModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLoginModel.Controls.Add(this.tbPassword);
            this.gbLoginModel.Controls.Add(this.rbSQLServer);
            this.gbLoginModel.Controls.Add(this.lblUserName);
            this.gbLoginModel.Controls.Add(this.rbWindow);
            this.gbLoginModel.Controls.Add(this.lblPassword);
            this.gbLoginModel.Controls.Add(this.tbUserName);
            this.gbLoginModel.Location = new System.Drawing.Point(35, 58);
            this.gbLoginModel.Name = "gbLoginModel";
            this.gbLoginModel.Size = new System.Drawing.Size(200, 141);
            this.gbLoginModel.TabIndex = 13;
            this.gbLoginModel.TabStop = false;
            this.gbLoginModel.Text = "登录方式";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(213, 21);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // DbSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.gbLoginModel);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.cbDbName);
            this.Controls.Add(this.cbServerName);
            this.Controls.Add(this.tbDbName);
            this.Controls.Add(this.lblDbName);
            this.Controls.Add(this.lblServerName);
            this.Name = "DbSelect";
            this.Size = new System.Drawing.Size(299, 324);
            this.gbLoginModel.ResumeLayout(false);
            this.gbLoginModel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblDbName;
        private System.Windows.Forms.TextBox tbDbName;
        private System.Windows.Forms.ComboBox cbServerName;
        private System.Windows.Forms.ComboBox cbDbName;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.RadioButton rbWindow;
        private System.Windows.Forms.RadioButton rbSQLServer;
        private System.Windows.Forms.GroupBox gbLoginModel;
        private System.Windows.Forms.Button btnRefresh;
    }
}
