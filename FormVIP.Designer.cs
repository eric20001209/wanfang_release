namespace QPOS2008
{
	partial class FormVIP
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVIP));
			this.pnlVipAccount = new System.Windows.Forms.Panel();
			this.btnSearchVip = new System.Windows.Forms.Button();
			this.btnVAPrint = new System.Windows.Forms.Button();
			this.btnVAPay = new System.Windows.Forms.Button();
			this.lblVAAmount = new System.Windows.Forms.Label();
			this.label138 = new System.Windows.Forms.Label();
			this.label136 = new System.Windows.Forms.Label();
			this.dgvVA = new System.Windows.Forms.DataGridView();
			this.cc_invoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_amount_paid = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.lblVABarcode = new System.Windows.Forms.Label();
			this.label137 = new System.Windows.Forms.Label();
			this.lblVAName = new System.Windows.Forms.Label();
			this.label134 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.pnlVipAccount.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvVA)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlVipAccount
			// 
			this.pnlVipAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlVipAccount.Controls.Add(this.btnSearchVip);
			this.pnlVipAccount.Controls.Add(this.btnVAPrint);
			this.pnlVipAccount.Controls.Add(this.btnVAPay);
			this.pnlVipAccount.Controls.Add(this.lblVAAmount);
			this.pnlVipAccount.Controls.Add(this.label138);
			this.pnlVipAccount.Controls.Add(this.label136);
			this.pnlVipAccount.Controls.Add(this.dgvVA);
			this.pnlVipAccount.Controls.Add(this.lblVABarcode);
			this.pnlVipAccount.Controls.Add(this.label137);
			this.pnlVipAccount.Controls.Add(this.lblVAName);
			this.pnlVipAccount.Controls.Add(this.label134);
			this.pnlVipAccount.Location = new System.Drawing.Point(6, 6);
			this.pnlVipAccount.Name = "pnlVipAccount";
			this.pnlVipAccount.Size = new System.Drawing.Size(813, 674);
			this.pnlVipAccount.TabIndex = 48;
			// 
			// btnSearchVip
			// 
			this.btnSearchVip.Location = new System.Drawing.Point(246, 9);
			this.btnSearchVip.Name = "btnSearchVip";
			this.btnSearchVip.Size = new System.Drawing.Size(75, 23);
			this.btnSearchVip.TabIndex = 10;
			this.btnSearchVip.Text = "Search VIP";
			this.btnSearchVip.UseVisualStyleBackColor = true;
			this.btnSearchVip.Click += new System.EventHandler(this.btnSearchVip_Click);
			// 
			// btnVAPrint
			// 
			this.btnVAPrint.Location = new System.Drawing.Point(641, 634);
			this.btnVAPrint.Name = "btnVAPrint";
			this.btnVAPrint.Size = new System.Drawing.Size(75, 23);
			this.btnVAPrint.TabIndex = 9;
			this.btnVAPrint.Text = "A4 Print";
			this.btnVAPrint.UseVisualStyleBackColor = true;
			this.btnVAPrint.Click += new System.EventHandler(this.btnVAPrint_Click);
			// 
			// btnVAPay
			// 
			this.btnVAPay.Location = new System.Drawing.Point(717, 634);
			this.btnVAPay.Name = "btnVAPay";
			this.btnVAPay.Size = new System.Drawing.Size(75, 23);
			this.btnVAPay.TabIndex = 8;
			this.btnVAPay.Text = "Pay";
			this.btnVAPay.UseVisualStyleBackColor = true;
			this.btnVAPay.Click += new System.EventHandler(this.btnVAPay_Click);
			// 
			// lblVAAmount
			// 
			this.lblVAAmount.AutoSize = true;
			this.lblVAAmount.Location = new System.Drawing.Point(744, 611);
			this.lblVAAmount.Name = "lblVAAmount";
			this.lblVAAmount.Size = new System.Drawing.Size(48, 13);
			this.lblVAAmount.TabIndex = 7;
			this.lblVAAmount.Text = "$amount";
			// 
			// label138
			// 
			this.label138.AutoSize = true;
			this.label138.Location = new System.Drawing.Point(635, 611);
			this.label138.Name = "label138";
			this.label138.Size = new System.Drawing.Size(83, 13);
			this.label138.TabIndex = 6;
			this.label138.Text = "Amount To Pay:";
			// 
			// label136
			// 
			this.label136.AutoSize = true;
			this.label136.Location = new System.Drawing.Point(12, 39);
			this.label136.Name = "label136";
			this.label136.Size = new System.Drawing.Size(79, 13);
			this.label136.TabIndex = 5;
			this.label136.Text = "Unpaid Invoice";
			// 
			// dgvVA
			// 
			this.dgvVA.AllowUserToAddRows = false;
			this.dgvVA.AllowUserToDeleteRows = false;
			this.dgvVA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvVA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cc_invoice,
            this.cc_date,
            this.cc_amount,
            this.cc_amount_paid,
            this.cc_balance,
            this.cc_check});
			this.dgvVA.Location = new System.Drawing.Point(15, 60);
			this.dgvVA.Name = "dgvVA";
			this.dgvVA.ReadOnly = true;
			this.dgvVA.Size = new System.Drawing.Size(775, 537);
			this.dgvVA.TabIndex = 4;
			this.dgvVA.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVA_CellClick);
			// 
			// cc_invoice
			// 
			this.cc_invoice.HeaderText = "INV#";
			this.cc_invoice.Name = "cc_invoice";
			this.cc_invoice.ReadOnly = true;
			// 
			// cc_date
			// 
			this.cc_date.HeaderText = "Date";
			this.cc_date.Name = "cc_date";
			this.cc_date.ReadOnly = true;
			this.cc_date.Width = 150;
			// 
			// cc_amount
			// 
			this.cc_amount.HeaderText = "Amount";
			this.cc_amount.Name = "cc_amount";
			this.cc_amount.ReadOnly = true;
			this.cc_amount.Width = 130;
			// 
			// cc_amount_paid
			// 
			this.cc_amount_paid.HeaderText = "Paid";
			this.cc_amount_paid.Name = "cc_amount_paid";
			this.cc_amount_paid.ReadOnly = true;
			this.cc_amount_paid.Width = 130;
			// 
			// cc_balance
			// 
			this.cc_balance.HeaderText = "Balance";
			this.cc_balance.Name = "cc_balance";
			this.cc_balance.ReadOnly = true;
			this.cc_balance.Width = 130;
			// 
			// cc_check
			// 
			this.cc_check.HeaderText = "Select";
			this.cc_check.Name = "cc_check";
			this.cc_check.ReadOnly = true;
			this.cc_check.Width = 60;
			// 
			// lblVABarcode
			// 
			this.lblVABarcode.AutoSize = true;
			this.lblVABarcode.Location = new System.Drawing.Point(178, 14);
			this.lblVABarcode.Name = "lblVABarcode";
			this.lblVABarcode.Size = new System.Drawing.Size(46, 13);
			this.lblVABarcode.TabIndex = 3;
			this.lblVABarcode.Text = "barcode";
			// 
			// label137
			// 
			this.label137.AutoSize = true;
			this.label137.Location = new System.Drawing.Point(127, 14);
			this.label137.Name = "label137";
			this.label137.Size = new System.Drawing.Size(50, 13);
			this.label137.TabIndex = 2;
			this.label137.Text = "Barcode:";
			// 
			// lblVAName
			// 
			this.lblVAName.AutoSize = true;
			this.lblVAName.Location = new System.Drawing.Point(53, 14);
			this.lblVAName.Name = "lblVAName";
			this.lblVAName.Size = new System.Drawing.Size(35, 13);
			this.lblVAName.TabIndex = 1;
			this.lblVAName.Text = "Name";
			// 
			// label134
			// 
			this.label134.AutoSize = true;
			this.label134.Location = new System.Drawing.Point(12, 14);
			this.label134.Name = "label134";
			this.label134.Size = new System.Drawing.Size(38, 13);
			this.label134.TabIndex = 0;
			this.label134.Text = "Name:";
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(744, 686);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 11;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// FormVIP
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(842, 726);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.pnlVipAccount);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormVIP";
			this.Text = "VIP";
			this.Load += new System.EventHandler(this.FormVIP_Load);
			this.pnlVipAccount.ResumeLayout(false);
			this.pnlVipAccount.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvVA)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlVipAccount;
		private System.Windows.Forms.Button btnSearchVip;
		private System.Windows.Forms.Button btnVAPrint;
		private System.Windows.Forms.Button btnVAPay;
		private System.Windows.Forms.Label lblVAAmount;
		private System.Windows.Forms.Label label138;
		private System.Windows.Forms.Label label136;
		private System.Windows.Forms.DataGridView dgvVA;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_invoice;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_date;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_amount;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_amount_paid;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_balance;
		private System.Windows.Forms.DataGridViewCheckBoxColumn cc_check;
		private System.Windows.Forms.Label lblVABarcode;
		private System.Windows.Forms.Label label137;
		private System.Windows.Forms.Label lblVAName;
		private System.Windows.Forms.Label label134;
		private System.Windows.Forms.Button btnClose;
	}
}