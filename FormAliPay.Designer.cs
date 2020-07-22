namespace QPOS2008
{
	partial class FormAliPay
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAliPay));
			this.txtDebug = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnGo = new System.Windows.Forms.Button();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.wb = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// txtDebug
			// 
			this.txtDebug.Location = new System.Drawing.Point(797, 9);
			this.txtDebug.Multiline = true;
			this.txtDebug.Name = "txtDebug";
			this.txtDebug.Size = new System.Drawing.Size(605, 715);
			this.txtDebug.TabIndex = 9;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(751, 721);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(40, 23);
			this.btnGo.TabIndex = 8;
			this.btnGo.Text = "GO";
			this.btnGo.UseVisualStyleBackColor = true;
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(9, 723);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(736, 20);
			this.txtURL.TabIndex = 7;
			// 
			// wb
			// 
			this.wb.Location = new System.Drawing.Point(9, 9);
			this.wb.Margin = new System.Windows.Forms.Padding(0);
			this.wb.MinimumSize = new System.Drawing.Size(20, 20);
			this.wb.Name = "wb";
			this.wb.ScrollBarsEnabled = false;
			this.wb.Size = new System.Drawing.Size(784, 737);
			this.wb.TabIndex = 5;
			// 
			// FormAliPay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1443, 783);
			this.Controls.Add(this.txtDebug);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.wb);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormAliPay";
			this.Text = "AliPay";
			this.Load += new System.EventHandler(this.FormAliPay_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtDebug;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.WebBrowser wb;

	}
}