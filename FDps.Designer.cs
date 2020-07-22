namespace QPOS2008
{
	partial class FDps
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FDps));
			this.axDpsEftX = new AxDPSEFTXLib.AxDpsEftX();
			((System.ComponentModel.ISupportInitialize)(this.axDpsEftX)).BeginInit();
			this.SuspendLayout();
			// 
			// axDpsEftX
			// 
			this.axDpsEftX.Location = new System.Drawing.Point(12, 12);
			this.axDpsEftX.Name = "axDpsEftX";
			this.axDpsEftX.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDpsEftX.OcxState")));
			this.axDpsEftX.Size = new System.Drawing.Size(47, 28);
			this.axDpsEftX.TabIndex = 0;
			this.axDpsEftX.StatusChangedEvent += new System.EventHandler(this.axDpsEftX_StatusChangedEvent);
			// 
			// FDps
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(120, 76);
			this.Controls.Add(this.axDpsEftX);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FDps";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "DPS";
			this.Load += new System.EventHandler(this.FDps_Load);
			((System.ComponentModel.ISupportInitialize)(this.axDpsEftX)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private AxDPSEFTXLib.AxDpsEftX axDpsEftX;
	}
}