namespace HKH.Tools
{
    partial class frmDbBackupAndRestore
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
            this.dbSelect1 = new HKH.Tools.DbSelect();
            this.SuspendLayout();
            // 
            // dbSelect1
            // 
            this.dbSelect1.Location = new System.Drawing.Point(12, 2);
            this.dbSelect1.Name = "dbSelect1";
            this.dbSelect1.Size = new System.Drawing.Size(299, 324);
            this.dbSelect1.TabIndex = 0;
            // 
            // frmDbBackupAndRestore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 328);
            this.Controls.Add(this.dbSelect1);
            this.Name = "frmDbBackupAndRestore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库连接测试";
            this.ResumeLayout(false);

        }

        #endregion

        private DbSelect dbSelect1;
    }
}