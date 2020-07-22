namespace QPOS2008
{
	partial class FormOtherPayment
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOtherPayment));
			this.rbWechat = new System.Windows.Forms.RadioButton();
			this.rbAlipay = new System.Windows.Forms.RadioButton();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblAmount = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.btnPay = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pbWechatPayPlus = new System.Windows.Forms.PictureBox();
			this.rbWechatPayPlus = new System.Windows.Forms.RadioButton();
			this.pbWechat = new System.Windows.Forms.PictureBox();
			this.pbAlipayDirect = new System.Windows.Forms.PictureBox();
			this.pbAlipay = new System.Windows.Forms.PictureBox();
			this.rbAlipayDirect = new System.Windows.Forms.RadioButton();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.lblScanningMethod = new System.Windows.Forms.Label();
			this.txtBarcode = new System.Windows.Forms.TextBox();
			this.btnSwitch = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbWechatPayPlus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbWechat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbAlipayDirect)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbAlipay)).BeginInit();
			this.SuspendLayout();
			// 
			// rbWechat
			// 
			this.rbWechat.AutoSize = true;
			this.rbWechat.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbWechat.Location = new System.Drawing.Point(25, 73);
			this.rbWechat.Name = "rbWechat";
			this.rbWechat.Size = new System.Drawing.Size(113, 29);
			this.rbWechat.TabIndex = 0;
			this.rbWechat.TabStop = true;
			this.rbWechat.Text = "WeChat";
			this.rbWechat.UseVisualStyleBackColor = true;
			this.rbWechat.Visible = false;
			this.rbWechat.Click += new System.EventHandler(this.rbWechat_Click);
			// 
			// rbAlipay
			// 
			this.rbAlipay.AutoSize = true;
			this.rbAlipay.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbAlipay.Location = new System.Drawing.Point(201, 73);
			this.rbAlipay.Name = "rbAlipay";
			this.rbAlipay.Size = new System.Drawing.Size(97, 29);
			this.rbAlipay.TabIndex = 3;
			this.rbAlipay.TabStop = true;
			this.rbAlipay.Text = "AliPay";
			this.rbAlipay.UseVisualStyleBackColor = true;
			this.rbAlipay.Visible = false;
			this.rbAlipay.Click += new System.EventHandler(this.rbAlipay_Click);
			// 
			// btnClose
			// 
			this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClose.Location = new System.Drawing.Point(466, 418);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(112, 78);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(36, 418);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(116, 78);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(29, 257);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 25);
			this.label1.TabIndex = 5;
			this.label1.Text = "Amount : ";
			// 
			// lblAmount
			// 
			this.lblAmount.AutoSize = true;
			this.lblAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAmount.Location = new System.Drawing.Point(147, 257);
			this.lblAmount.Name = "lblAmount";
			this.lblAmount.Size = new System.Drawing.Size(97, 25);
			this.lblAmount.TabIndex = 5;
			this.lblAmount.Text = "$888.88";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(29, 295);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 25);
			this.label2.TabIndex = 5;
			this.label2.Text = "Status : ";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus.Location = new System.Drawing.Point(147, 295);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(101, 25);
			this.lblStatus.TabIndex = 5;
			this.lblStatus.Text = "Success";
			// 
			// btnPay
			// 
			this.btnPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPay.Location = new System.Drawing.Point(252, 418);
			this.btnPay.Name = "btnPay";
			this.btnPay.Size = new System.Drawing.Size(116, 78);
			this.btnPay.TabIndex = 4;
			this.btnPay.Text = "Pay";
			this.btnPay.UseVisualStyleBackColor = true;
			this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.pbWechatPayPlus);
			this.panel1.Controls.Add(this.rbWechatPayPlus);
			this.panel1.Controls.Add(this.pbWechat);
			this.panel1.Controls.Add(this.pbAlipayDirect);
			this.panel1.Controls.Add(this.pbAlipay);
			this.panel1.Controls.Add(this.rbAlipayDirect);
			this.panel1.Controls.Add(this.rbWechat);
			this.panel1.Controls.Add(this.rbAlipay);
			this.panel1.Location = new System.Drawing.Point(35, 12);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(543, 216);
			this.panel1.TabIndex = 6;
			// 
			// pbWechatPayPlus
			// 
			this.pbWechatPayPlus.Image = global::QPOS2008.Properties.Resources.wechat;
			this.pbWechatPayPlus.Location = new System.Drawing.Point(366, 14);
			this.pbWechatPayPlus.Name = "pbWechatPayPlus";
			this.pbWechatPayPlus.Size = new System.Drawing.Size(131, 46);
			this.pbWechatPayPlus.TabIndex = 4;
			this.pbWechatPayPlus.TabStop = false;
			this.pbWechatPayPlus.Visible = false;
			this.pbWechatPayPlus.Click += new System.EventHandler(this.pbWechatPayPlus_Click);
			// 
			// rbWechatPayPlus
			// 
			this.rbWechatPayPlus.AutoSize = true;
			this.rbWechatPayPlus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbWechatPayPlus.Location = new System.Drawing.Point(366, 73);
			this.rbWechatPayPlus.Name = "rbWechatPayPlus";
			this.rbWechatPayPlus.Size = new System.Drawing.Size(116, 29);
			this.rbWechatPayPlus.TabIndex = 5;
			this.rbWechatPayPlus.TabStop = true;
			this.rbWechatPayPlus.Text = "PayPlus";
			this.rbWechatPayPlus.UseVisualStyleBackColor = true;
			this.rbWechatPayPlus.Visible = false;
			this.rbWechatPayPlus.Click += new System.EventHandler(this.rbWechatPayPlus_Click);
			// 
			// pbWechat
			// 
			this.pbWechat.Image = global::QPOS2008.Properties.Resources.wechat;
			this.pbWechat.Location = new System.Drawing.Point(11, 14);
			this.pbWechat.Name = "pbWechat";
			this.pbWechat.Size = new System.Drawing.Size(131, 46);
			this.pbWechat.TabIndex = 0;
			this.pbWechat.TabStop = false;
			this.pbWechat.Visible = false;
			this.pbWechat.Click += new System.EventHandler(this.pbWechat_Click);
			// 
			// pbAlipayDirect
			// 
			this.pbAlipayDirect.Image = global::QPOS2008.Properties.Resources.alipay;
			this.pbAlipayDirect.Location = new System.Drawing.Point(25, 125);
			this.pbAlipayDirect.Name = "pbAlipayDirect";
			this.pbAlipayDirect.Size = new System.Drawing.Size(131, 46);
			this.pbAlipayDirect.TabIndex = 1;
			this.pbAlipayDirect.TabStop = false;
			this.pbAlipayDirect.Visible = false;
			this.pbAlipayDirect.Click += new System.EventHandler(this.pbAlipayDirect_Click);
			// 
			// pbAlipay
			// 
			this.pbAlipay.Image = global::QPOS2008.Properties.Resources.alipay;
			this.pbAlipay.Location = new System.Drawing.Point(186, 14);
			this.pbAlipay.Name = "pbAlipay";
			this.pbAlipay.Size = new System.Drawing.Size(131, 46);
			this.pbAlipay.TabIndex = 1;
			this.pbAlipay.TabStop = false;
			this.pbAlipay.Visible = false;
			this.pbAlipay.Click += new System.EventHandler(this.pbAlipay_Click);
			// 
			// rbAlipayDirect
			// 
			this.rbAlipayDirect.AutoSize = true;
			this.rbAlipayDirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbAlipayDirect.Location = new System.Drawing.Point(5, 174);
			this.rbAlipayDirect.Name = "rbAlipayDirect";
			this.rbAlipayDirect.Size = new System.Drawing.Size(166, 29);
			this.rbAlipayDirect.TabIndex = 6;
			this.rbAlipayDirect.TabStop = true;
			this.rbAlipayDirect.Text = "AliPay Direct";
			this.rbAlipayDirect.UseVisualStyleBackColor = true;
			this.rbAlipayDirect.Visible = false;
			this.rbAlipayDirect.CheckedChanged += new System.EventHandler(this.rbAlipayDirect_CheckedChanged);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(30, 333);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(216, 25);
			this.label3.TabIndex = 9;
			this.label3.Text = "Scanning Method : ";
			this.label3.Visible = false;
			// 
			// lblScanningMethod
			// 
			this.lblScanningMethod.AutoSize = true;
			this.lblScanningMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblScanningMethod.ForeColor = System.Drawing.Color.Blue;
			this.lblScanningMethod.Location = new System.Drawing.Point(235, 335);
			this.lblScanningMethod.Name = "lblScanningMethod";
			this.lblScanningMethod.Size = new System.Drawing.Size(188, 24);
			this.lblScanningMethod.TabIndex = 10;
			this.lblScanningMethod.Text = "Generate QR Code";
			this.lblScanningMethod.Visible = false;
			// 
			// txtBarcode
			// 
			this.txtBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtBarcode.Location = new System.Drawing.Point(36, 369);
			this.txtBarcode.Name = "txtBarcode";
			this.txtBarcode.Size = new System.Drawing.Size(542, 38);
			this.txtBarcode.TabIndex = 0;
			this.txtBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtBarcode.UseSystemPasswordChar = true;
			this.txtBarcode.Visible = false;
			this.txtBarcode.Click += new System.EventHandler(this.txtBarcode_Click);
			// 
			// btnSwitch
			// 
			this.btnSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSwitch.Location = new System.Drawing.Point(479, 329);
			this.btnSwitch.Name = "btnSwitch";
			this.btnSwitch.Size = new System.Drawing.Size(99, 37);
			this.btnSwitch.TabIndex = 11;
			this.btnSwitch.Text = "Switch";
			this.btnSwitch.UseVisualStyleBackColor = true;
			this.btnSwitch.Visible = false;
			this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
			// 
			// FormOtherPayment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(611, 509);
			this.Controls.Add(this.btnSwitch);
			this.Controls.Add(this.txtBarcode);
			this.Controls.Add(this.lblScanningMethod);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lblAmount);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnPay);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormOtherPayment";
			this.Text = "Other Payment Method";
			this.Load += new System.EventHandler(this.FormOtherPayment_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbWechatPayPlus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbWechat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbAlipayDirect)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbAlipay)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbWechat;
		private System.Windows.Forms.PictureBox pbAlipay;
		private System.Windows.Forms.RadioButton rbWechat;
		private System.Windows.Forms.RadioButton rbAlipay;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblAmount;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Button btnPay;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.PictureBox pbWechatPayPlus;
		private System.Windows.Forms.RadioButton rbWechatPayPlus;
		private System.Windows.Forms.PictureBox pbAlipayDirect;
		private System.Windows.Forms.RadioButton rbAlipayDirect;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblScanningMethod;
		private System.Windows.Forms.TextBox txtBarcode;
		private System.Windows.Forms.Button btnSwitch;
	}
}