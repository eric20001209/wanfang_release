namespace QPOS2008
{
	partial class FormWeChat
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWeChat));
			this.wb = new System.Windows.Forms.WebBrowser();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.txtDebug = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// wb
			// 
			this.wb.Location = new System.Drawing.Point(1, 2);
			this.wb.Margin = new System.Windows.Forms.Padding(0);
			this.wb.MinimumSize = new System.Drawing.Size(20, 20);
			this.wb.Name = "wb";
			this.wb.ScrollBarsEnabled = false;
			this.wb.Size = new System.Drawing.Size(784, 737);
			this.wb.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.BackgroundImage = global::QPOS2008.Properties.Resources._110_38_red;
			this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.btnCancel.Location = new System.Drawing.Point(294, 664);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(168, 65);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(1, 716);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(736, 20);
			this.txtURL.TabIndex = 2;
			this.txtURL.Visible = false;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(743, 714);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(40, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "GO";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Visible = false;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// txtDebug
			// 
			this.txtDebug.Location = new System.Drawing.Point(789, 2);
			this.txtDebug.Multiline = true;
			this.txtDebug.Name = "txtDebug";
			this.txtDebug.Size = new System.Drawing.Size(605, 715);
			this.txtDebug.TabIndex = 4;
			// 
			// FormWeChat
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(786, 741);
			this.Controls.Add(this.txtDebug);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.wb);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormWeChat";
			this.Text = "WeChat Payment";
			this.Load += new System.EventHandler(this.FormWeChat_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.WebBrowser wb;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.TextBox txtDebug;
	}
}