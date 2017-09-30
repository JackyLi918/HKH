namespace HKH.ConfigurationEditor
{
	partial class frmConnStrEditor
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
			this.dbSelect1 = new HKH.ConfigurationEditor.DbSelect();
			this.SuspendLayout();
			// 
			// dbSelect1
			// 
			this.dbSelect1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dbSelect1.ConnectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder("");
			this.dbSelect1.Location = new System.Drawing.Point(1, 0);
			this.dbSelect1.Name = "dbSelect1";
			this.dbSelect1.Size = new System.Drawing.Size(299, 325);
			this.dbSelect1.TabIndex = 0;
			this.dbSelect1.ChangeAccepted += new System.EventHandler(this.dbSelect1_ChangeAccepted);
			this.dbSelect1.ChangeCanceled += new System.EventHandler(this.dbSelect1_ChangeCanceled);
			// 
			// frmConnStrEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(301, 326);
			this.Controls.Add(this.dbSelect1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmConnStrEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Database";
			this.ResumeLayout(false);

		}

		#endregion

		private ConfigurationEditor.DbSelect dbSelect1;
	}
}