namespace QPOS2008
{
    partial class export_time_sheet
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
            this.btnexportsum = new System.Windows.Forms.Button();
            this.btnexporttimedetail = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnexportsum
            // 
            this.btnexportsum.Location = new System.Drawing.Point(12, 12);
            this.btnexportsum.Name = "btnexportsum";
            this.btnexportsum.Size = new System.Drawing.Size(117, 65);
            this.btnexportsum.TabIndex = 0;
            this.btnexportsum.Text = "Export Time Sheet Summary";
            this.btnexportsum.UseVisualStyleBackColor = true;
            this.btnexportsum.Click += new System.EventHandler(this.btnexportsum_Click);
            // 
            // btnexporttimedetail
            // 
            this.btnexporttimedetail.Location = new System.Drawing.Point(163, 12);
            this.btnexporttimedetail.Name = "btnexporttimedetail";
            this.btnexporttimedetail.Size = new System.Drawing.Size(117, 65);
            this.btnexporttimedetail.TabIndex = 1;
            this.btnexporttimedetail.Text = "Export Time Sheet Detail";
            this.btnexporttimedetail.UseVisualStyleBackColor = true;
            this.btnexporttimedetail.Click += new System.EventHandler(this.btnexporttimedetail_Click);
            // 
            // export_time_sheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 101);
            this.Controls.Add(this.btnexporttimedetail);
            this.Controls.Add(this.btnexportsum);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "export_time_sheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "export_time_sheet";
            this.Load += new System.EventHandler(this.export_time_sheet_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnexportsum;
        private System.Windows.Forms.Button btnexporttimedetail;
    }
}