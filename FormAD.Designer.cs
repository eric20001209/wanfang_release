namespace QPOS2008
{
	partial class FormAD
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAD));
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.label1 = new System.Windows.Forms.Label();
			this.TotalPrice = new System.Windows.Forms.Label();
			this.cart = new System.Windows.Forms.DataGridView();
			this.cc_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_last = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ShowTotalSave = new System.Windows.Forms.Label();
			this.totalsavelable = new System.Windows.Forms.Label();
			this.citem1 = new System.Windows.Forms.Label();
			this.citem2 = new System.Windows.Forms.Label();
			this.showchange = new System.Windows.Forms.Label();
			this.labelChange = new System.Windows.Forms.Label();
			this.lblSurCharge = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.cart)).BeginInit();
			this.SuspendLayout();
			// 
			// webBrowser1
			// 
			this.webBrowser1.Location = new System.Drawing.Point(571, 6);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.ScrollBarsEnabled = false;
			this.webBrowser1.Size = new System.Drawing.Size(574, 697);
			this.webBrowser1.TabIndex = 0;
			this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 492);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label1.Size = new System.Drawing.Size(285, 37);
			this.label1.TabIndex = 3;
			this.label1.Text = "DUE BALANCE : ";
			// 
			// TotalPrice
			// 
			this.TotalPrice.AutoSize = true;
			this.TotalPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TotalPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TotalPrice.Location = new System.Drawing.Point(351, 485);
			this.TotalPrice.Name = "TotalPrice";
			this.TotalPrice.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.TotalPrice.Size = new System.Drawing.Size(93, 44);
			this.TotalPrice.TabIndex = 4;
			this.TotalPrice.Text = "0.00";
			// 
			// cart
			// 
			this.cart.AllowUserToAddRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LavenderBlush;
			this.cart.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.cart.BackgroundColor = System.Drawing.Color.Snow;
			this.cart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSkyBlue;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.cart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.cart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.cart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cc_name,
            this.cc_price,
            this.cc_discount,
            this.cc_qty,
            this.cc_total,
            this.cc_last});
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = System.Drawing.Color.LightCyan;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.cart.DefaultCellStyle = dataGridViewCellStyle6;
			this.cart.Location = new System.Drawing.Point(1, 6);
			this.cart.MultiSelect = false;
			this.cart.Name = "cart";
			this.cart.ReadOnly = true;
			this.cart.RowHeadersVisible = false;
			this.cart.RowTemplate.Height = 32;
			this.cart.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.cart.Size = new System.Drawing.Size(564, 473);
			this.cart.TabIndex = 5;
			// 
			// cc_name
			// 
			this.cc_name.HeaderText = "Description";
			this.cc_name.Name = "cc_name";
			this.cc_name.ReadOnly = true;
			this.cc_name.Width = 250;
			// 
			// cc_price
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.cc_price.DefaultCellStyle = dataGridViewCellStyle3;
			this.cc_price.HeaderText = "Price";
			this.cc_price.Name = "cc_price";
			this.cc_price.ReadOnly = true;
			this.cc_price.Width = 90;
			// 
			// cc_discount
			// 
			this.cc_discount.HeaderText = "Disct";
			this.cc_discount.Name = "cc_discount";
			this.cc_discount.ReadOnly = true;
			this.cc_discount.Width = 50;
			// 
			// cc_qty
			// 
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
			this.cc_qty.DefaultCellStyle = dataGridViewCellStyle4;
			this.cc_qty.HeaderText = "Qty";
			this.cc_qty.Name = "cc_qty";
			this.cc_qty.ReadOnly = true;
			this.cc_qty.Width = 70;
			// 
			// cc_total
			// 
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.cc_total.DefaultCellStyle = dataGridViewCellStyle5;
			this.cc_total.HeaderText = "Total";
			this.cc_total.Name = "cc_total";
			this.cc_total.ReadOnly = true;
			// 
			// cc_last
			// 
			this.cc_last.HeaderText = "";
			this.cc_last.Name = "cc_last";
			this.cc_last.ReadOnly = true;
			this.cc_last.Width = 5;
			// 
			// ShowTotalSave
			// 
			this.ShowTotalSave.AutoSize = true;
			this.ShowTotalSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ShowTotalSave.Location = new System.Drawing.Point(351, 536);
			this.ShowTotalSave.Name = "ShowTotalSave";
			this.ShowTotalSave.Size = new System.Drawing.Size(38, 42);
			this.ShowTotalSave.TabIndex = 6;
			this.ShowTotalSave.Text = "..";
			this.ShowTotalSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// totalsavelable
			// 
			this.totalsavelable.AutoSize = true;
			this.totalsavelable.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.totalsavelable.Location = new System.Drawing.Point(13, 540);
			this.totalsavelable.Name = "totalsavelable";
			this.totalsavelable.Size = new System.Drawing.Size(38, 42);
			this.totalsavelable.TabIndex = 7;
			this.totalsavelable.Text = "..";
			// 
			// citem1
			// 
			this.citem1.AutoSize = true;
			this.citem1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.citem1.Location = new System.Drawing.Point(25, 633);
			this.citem1.Name = "citem1";
			this.citem1.Size = new System.Drawing.Size(22, 24);
			this.citem1.TabIndex = 8;
			this.citem1.Text = "..";
			// 
			// citem2
			// 
			this.citem2.AutoSize = true;
			this.citem2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.citem2.Location = new System.Drawing.Point(21, 660);
			this.citem2.Name = "citem2";
			this.citem2.Size = new System.Drawing.Size(30, 31);
			this.citem2.TabIndex = 9;
			this.citem2.Text = "..";
			// 
			// showchange
			// 
			this.showchange.AutoSize = true;
			this.showchange.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.showchange.Location = new System.Drawing.Point(350, 578);
			this.showchange.Name = "showchange";
			this.showchange.Size = new System.Drawing.Size(38, 42);
			this.showchange.TabIndex = 10;
			this.showchange.Text = "..";
			// 
			// labelChange
			// 
			this.labelChange.AutoSize = true;
			this.labelChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelChange.Location = new System.Drawing.Point(14, 582);
			this.labelChange.Name = "labelChange";
			this.labelChange.Size = new System.Drawing.Size(38, 42);
			this.labelChange.TabIndex = 11;
			this.labelChange.Text = "..";
			// 
			// lblSurCharge
			// 
			this.lblSurCharge.AutoSize = true;
			this.lblSurCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSurCharge.Location = new System.Drawing.Point(289, 649);
			this.lblSurCharge.Name = "lblSurCharge";
			this.lblSurCharge.Size = new System.Drawing.Size(30, 31);
			this.lblSurCharge.TabIndex = 12;
			this.lblSurCharge.Text = "..";
			// 
			// FormAD
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1166, 706);
			this.Controls.Add(this.lblSurCharge);
			this.Controls.Add(this.labelChange);
			this.Controls.Add(this.showchange);
			this.Controls.Add(this.citem2);
			this.Controls.Add(this.citem1);
			this.Controls.Add(this.totalsavelable);
			this.Controls.Add(this.ShowTotalSave);
			this.Controls.Add(this.cart);
			this.Controls.Add(this.TotalPrice);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.webBrowser1);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "FormAD";
			this.ShowInTaskbar = false;
			this.Text = "GPOS ";
			this.Load += new System.EventHandler(this.FormAD_Load);
			((System.ComponentModel.ISupportInitialize)(this.cart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.Label TotalPrice;
        public System.Windows.Forms.Label ShowTotalSave;
        public System.Windows.Forms.Label totalsavelable;
        public System.Windows.Forms.Label citem1;
        public System.Windows.Forms.Label citem2;
        public System.Windows.Forms.Label showchange;
        public System.Windows.Forms.Label labelChange;
		public System.Windows.Forms.DataGridView cart;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_name;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_price;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_discount;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_qty;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_total;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_last;
        public System.Windows.Forms.Label lblSurCharge;
        
       
       
        
       
        
	}
}