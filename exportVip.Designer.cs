namespace QPOS2008
{
	partial class exportVip
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
			this.btnexporttimedetail = new System.Windows.Forms.Button();
			this.btnexportsum = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnexporttimedetail
			// 
			this.btnexporttimedetail.Location = new System.Drawing.Point(163, 12);
			this.btnexporttimedetail.Name = "btnexporttimedetail";
			this.btnexporttimedetail.Size = new System.Drawing.Size(117, 65);
			this.btnexporttimedetail.TabIndex = 3;
			this.btnexporttimedetail.Text = "Export Vip Detail";
			this.btnexporttimedetail.UseVisualStyleBackColor = true;
			this.btnexporttimedetail.Click += new System.EventHandler(this.btnexporttimedetail_Click);
			// 
			// btnexportsum
			// 
			this.btnexportsum.Location = new System.Drawing.Point(12, 12);
			this.btnexportsum.Name = "btnexportsum";
			this.btnexportsum.Size = new System.Drawing.Size(117, 65);
			this.btnexportsum.TabIndex = 2;
			this.btnexportsum.Text = "Export Vip Summary";
			this.btnexportsum.UseVisualStyleBackColor = true;
			this.btnexportsum.Click += new System.EventHandler(this.btnexportsum_Click);
			// 
			// exportVip
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 95);
			this.Controls.Add(this.btnexporttimedetail);
			this.Controls.Add(this.btnexportsum);
			this.Name = "exportVip";
			this.Text = "exportVip";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnexporttimedetail;
		private System.Windows.Forms.Button btnexportsum;
	}
}