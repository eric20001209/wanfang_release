namespace QPOS2008
{
	partial class FormLogin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
			this.label1 = new System.Windows.Forms.Label();
			this.barcode = new System.Windows.Forms.TextBox();
			this.buttonLogin = new System.Windows.Forms.Button();
			this.pass = new System.Windows.Forms.MaskedTextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.version = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Courier New", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.SeaShell;
			this.label1.Location = new System.Drawing.Point(163, 197);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 73);
			this.label1.TabIndex = 0;
			// 
			// barcode
			// 
			this.barcode.BackColor = System.Drawing.Color.LightCyan;
			this.barcode.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.barcode.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.barcode.Location = new System.Drawing.Point(166, 324);
			this.barcode.Margin = new System.Windows.Forms.Padding(10);
			this.barcode.Name = "barcode";
			this.barcode.Size = new System.Drawing.Size(318, 56);
			this.barcode.TabIndex = 1;
			this.barcode.UseSystemPasswordChar = true;
			this.barcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.barcode_KeyDown);
			// 
			// buttonLogin
			// 
			this.buttonLogin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonLogin.BackColor = System.Drawing.Color.Transparent;
			this.buttonLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.buttonLogin.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonLogin.FlatAppearance.BorderSize = 0;
			this.buttonLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonLogin.Image = ((System.Drawing.Image)(resources.GetObject("buttonLogin.Image")));
			this.buttonLogin.Location = new System.Drawing.Point(1018, 761);
			this.buttonLogin.Margin = new System.Windows.Forms.Padding(4);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(60, 28);
			this.buttonLogin.TabIndex = 4;
			this.buttonLogin.UseVisualStyleBackColor = false;
			this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
			// 
			// pass
			// 
			this.pass.BackColor = System.Drawing.Color.Gray;
			this.pass.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pass.ForeColor = System.Drawing.Color.White;
			this.pass.Location = new System.Drawing.Point(1002, 743);
			this.pass.Mask = "99999999999999";
			this.pass.Name = "pass";
			this.pass.Size = new System.Drawing.Size(10, 13);
			this.pass.TabIndex = 5;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(519, 194);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(324, 379);
			this.flowLayoutPanel1.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(292, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(390, 39);
			this.label2.TabIndex = 9;
			this.label2.Text = "GPOS   Retail  System";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(400, 144);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(28, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "TM";
			// 
			// version
			// 
			this.version.AutoSize = true;
			this.version.BackColor = System.Drawing.Color.Transparent;
			this.version.Location = new System.Drawing.Point(888, 740);
			this.version.Name = "version";
			this.version.Size = new System.Drawing.Size(0, 16);
			this.version.TabIndex = 11;
			// 
			// button1
			// 
			this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.button1.Location = new System.Drawing.Point(958, 721);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(35, 35);
			this.button1.TabIndex = 12;
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Arial", 29F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.Gainsboro;
			this.label4.Location = new System.Drawing.Point(158, 265);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(315, 45);
			this.label4.TabIndex = 13;
			this.label4.Text = "Enter Passcode";
			this.label4.Click += new System.EventHandler(this.label4_Click);
			// 
			// FormLogin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.BackgroundImage = global::QPOS2008.Properties.Resources.loginform;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(1024, 768);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.version);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.pass);
			this.Controls.Add(this.buttonLogin);
			this.Controls.Add(this.barcode);
			this.Controls.Add(this.label1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "FormLogin";
			this.Opacity = 0.9;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "EZNZ GPOS 2008 - login";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FormLogin_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox barcode;
		private System.Windows.Forms.Button buttonLogin;
		private System.Windows.Forms.MaskedTextBox pass;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label version;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label4;
	}
}