namespace QPOS2008
{
	partial class FormLatipayQRCode
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLatipayQRCode));
			this.txtDebug = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.wb = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// txtDebug
			// 
			this.txtDebug.Location = new System.Drawing.Point(1036, 9);
			this.txtDebug.Multiline = true;
			this.txtDebug.Name = "txtDebug";
			this.txtDebug.Size = new System.Drawing.Size(366, 768);
			this.txtDebug.TabIndex = 9;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// wb
			// 
			this.wb.Location = new System.Drawing.Point(9, 9);
			this.wb.Margin = new System.Windows.Forms.Padding(0);
			this.wb.MinimumSize = new System.Drawing.Size(20, 20);
			this.wb.Name = "wb";
			this.wb.ScrollBarsEnabled = false;
			this.wb.Size = new System.Drawing.Size(1024, 768);
			this.wb.TabIndex = 5;
			// 
			// FormLatipayQRCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1037, 783);
			this.Controls.Add(this.txtDebug);
			this.Controls.Add(this.wb);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormLatipayQRCode";
			this.Text = "Latipay QRCode";
			this.Load += new System.EventHandler(this.FormLatipayQRCode_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDebug;
		private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.WebBrowser wb;

    }
}