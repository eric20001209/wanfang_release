namespace QPOS2008
{
	partial class FormConfirm
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
			this.lblMsg = new System.Windows.Forms.Label();
			this.btnYes = new System.Windows.Forms.Button();
			this.btnNo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			this.lblMsg.AutoSize = true;
			this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMsg.Location = new System.Drawing.Point(26, 56);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(40, 42);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "..";
			this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnYes
			// 
			this.btnYes.BackColor = System.Drawing.Color.MistyRose;
			this.btnYes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnYes.FlatAppearance.BorderSize = 0;
			this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnYes.ForeColor = System.Drawing.Color.Black;
			this.btnYes.Location = new System.Drawing.Point(43, 221);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(180, 93);
			this.btnYes.TabIndex = 1;
			this.btnYes.Text = "YES";
			this.btnYes.UseVisualStyleBackColor = false;
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// btnNo
			// 
			this.btnNo.BackColor = System.Drawing.Color.MistyRose;
			this.btnNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnNo.FlatAppearance.BorderSize = 0;
			this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnNo.ForeColor = System.Drawing.Color.Black;
			this.btnNo.Location = new System.Drawing.Point(478, 221);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(181, 93);
			this.btnNo.TabIndex = 2;
			this.btnNo.Text = "NO";
			this.btnNo.UseVisualStyleBackColor = false;
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// FormConfirm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Red;
			this.ClientSize = new System.Drawing.Size(722, 362);
			this.ControlBox = false;
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.btnYes);
			this.Controls.Add(this.lblMsg);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormConfirm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormConfirm";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FormConfirm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMsg;
		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Button btnNo;
	}
}