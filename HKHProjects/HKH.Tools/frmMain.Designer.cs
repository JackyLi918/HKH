namespace HKH.Tools
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbAppSetting = new System.Windows.Forms.ToolStripButton();
            this.tsbDbBackupAndRestore = new System.Windows.Forms.ToolStripButton();
            this.tsbImageConvert = new System.Windows.Forms.ToolStripButton();
            this.tsbJsonValidate = new System.Windows.Forms.ToolStripButton();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAppSetting,
            this.tsbDbBackupAndRestore,
            this.tsbImageConvert,
            this.tsbJsonValidate});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(511, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsbAppSetting
            // 
            this.tsbAppSetting.Image = ((System.Drawing.Image)(resources.GetObject("tsbAppSetting.Image")));
            this.tsbAppSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAppSetting.Name = "tsbAppSetting";
            this.tsbAppSetting.Size = new System.Drawing.Size(105, 22);
            this.tsbAppSetting.Text = "应用程序配置";
            this.tsbAppSetting.Visible = false;
            this.tsbAppSetting.Click += new System.EventHandler(this.tsbAppSetting_Click);
            // 
            // tsbDbBackupAndRestore
            // 
            this.tsbDbBackupAndRestore.Image = ((System.Drawing.Image)(resources.GetObject("tsbDbBackupAndRestore.Image")));
            this.tsbDbBackupAndRestore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDbBackupAndRestore.Name = "tsbDbBackupAndRestore";
            this.tsbDbBackupAndRestore.Size = new System.Drawing.Size(66, 22);
            this.tsbDbBackupAndRestore.Text = "数据库";
            this.tsbDbBackupAndRestore.ToolTipText = "数据库连接测试";
            this.tsbDbBackupAndRestore.Click += new System.EventHandler(this.tsbDbBackupAndRestore_Click);
            // 
            // tsbImageConvert
            // 
            this.tsbImageConvert.Image = ((System.Drawing.Image)(resources.GetObject("tsbImageConvert.Image")));
            this.tsbImageConvert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImageConvert.Name = "tsbImageConvert";
            this.tsbImageConvert.Size = new System.Drawing.Size(118, 22);
            this.tsbImageConvert.Text = "图片字符串转换";
            this.tsbImageConvert.Click += new System.EventHandler(this.tsbImageConvert_Click);
            // 
            // tsbJsonValidate
            // 
            this.tsbJsonValidate.Image = ((System.Drawing.Image)(resources.GetObject("tsbJsonValidate.Image")));
            this.tsbJsonValidate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbJsonValidate.Name = "tsbJsonValidate";
            this.tsbJsonValidate.Size = new System.Drawing.Size(81, 22);
            this.tsbJsonValidate.Text = "JSON验证";
            this.tsbJsonValidate.Click += new System.EventHandler(this.tsbJsonValidate_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 425);
            this.Controls.Add(this.tsMain);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "黑瞳工具箱";
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbAppSetting;
        private System.Windows.Forms.ToolStripButton tsbDbBackupAndRestore;
		private System.Windows.Forms.ToolStripButton tsbImageConvert;
        private System.Windows.Forms.ToolStripButton tsbJsonValidate;
    }
}

