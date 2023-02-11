namespace HKH.Tools
{
    partial class frmJsonValidate
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSchemaFile = new System.Windows.Forms.TextBox();
            this.txtJsonFile = new System.Windows.Forms.TextBox();
            this.btnSelectSchema = new System.Windows.Forms.Button();
            this.btnSelectJson = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Schema";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Json";
            // 
            // txtSchemaFile
            // 
            this.txtSchemaFile.Location = new System.Drawing.Point(82, 67);
            this.txtSchemaFile.Name = "txtSchemaFile";
            this.txtSchemaFile.ReadOnly = true;
            this.txtSchemaFile.Size = new System.Drawing.Size(290, 20);
            this.txtSchemaFile.TabIndex = 2;
            // 
            // txtJsonFile
            // 
            this.txtJsonFile.Location = new System.Drawing.Point(82, 108);
            this.txtJsonFile.Name = "txtJsonFile";
            this.txtJsonFile.ReadOnly = true;
            this.txtJsonFile.Size = new System.Drawing.Size(290, 20);
            this.txtJsonFile.TabIndex = 3;
            // 
            // btnSelectSchema
            // 
            this.btnSelectSchema.Location = new System.Drawing.Point(378, 64);
            this.btnSelectSchema.Name = "btnSelectSchema";
            this.btnSelectSchema.Size = new System.Drawing.Size(75, 23);
            this.btnSelectSchema.TabIndex = 4;
            this.btnSelectSchema.Text = "...";
            this.btnSelectSchema.UseVisualStyleBackColor = true;
            this.btnSelectSchema.Click += new System.EventHandler(this.btnSelectSchema_Click);
            // 
            // btnSelectJson
            // 
            this.btnSelectJson.Location = new System.Drawing.Point(378, 105);
            this.btnSelectJson.Name = "btnSelectJson";
            this.btnSelectJson.Size = new System.Drawing.Size(75, 23);
            this.btnSelectJson.TabIndex = 5;
            this.btnSelectJson.Text = "...";
            this.btnSelectJson.UseVisualStyleBackColor = true;
            this.btnSelectJson.Click += new System.EventHandler(this.btnSelectJson_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(233, 167);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 23);
            this.btnValidate.TabIndex = 6;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // frmJsonValidate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 450);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnSelectJson);
            this.Controls.Add(this.btnSelectSchema);
            this.Controls.Add(this.txtJsonFile);
            this.Controls.Add(this.txtSchemaFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmJsonValidate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JSON 验证";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSchemaFile;
        private System.Windows.Forms.TextBox txtJsonFile;
        private System.Windows.Forms.Button btnSelectSchema;
        private System.Windows.Forms.Button btnSelectJson;
        private System.Windows.Forms.Button btnValidate;
    }
}