namespace QPOS2008
{
	partial class FormLatipayGiftCardHistory
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
			System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
			System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            ""}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLatipayGiftCardHistory));
			this.lvGiftCard = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.txtGiftCardCode = new System.Windows.Forms.TextBox();
			this.lblFrm = new System.Windows.Forms.Label();
			this.lblTo = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.rtbView = new System.Windows.Forms.RichTextBox();
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnView = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.dpFrom = new System.Windows.Forms.DateTimePicker();
			this.dpTo = new System.Windows.Forms.DateTimePicker();
			this.lblTotalQty = new System.Windows.Forms.Label();
			this.lblTotalValue = new System.Windows.Forms.Label();
			this.totalQty = new System.Windows.Forms.Label();
			this.totalValue = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lvGiftCard
			// 
			this.lvGiftCard.BackColor = System.Drawing.Color.White;
			this.lvGiftCard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this.lvGiftCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvGiftCard.FullRowSelect = true;
			this.lvGiftCard.HideSelection = false;
			this.lvGiftCard.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
			this.lvGiftCard.Location = new System.Drawing.Point(12, 106);
			this.lvGiftCard.Margin = new System.Windows.Forms.Padding(9);
			this.lvGiftCard.MultiSelect = false;
			this.lvGiftCard.Name = "lvGiftCard";
			this.lvGiftCard.Size = new System.Drawing.Size(451, 364);
			this.lvGiftCard.TabIndex = 0;
			this.lvGiftCard.UseCompatibleStateImageBehavior = false;
			this.lvGiftCard.View = System.Windows.Forms.View.Details;
			this.lvGiftCard.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
			this.lvGiftCard.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.lvGiftCard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Till";
			this.columnHeader1.Width = 40;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Invoice";
			this.columnHeader2.Width = 80;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Gift Card Code";
			this.columnHeader3.Width = 120;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Value";
			this.columnHeader4.Width = 70;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Date Time";
			this.columnHeader5.Width = 180;
			// 
			// txtGiftCardCode
			// 
			this.txtGiftCardCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtGiftCardCode.ForeColor = System.Drawing.Color.Gainsboro;
			this.txtGiftCardCode.Location = new System.Drawing.Point(12, 12);
			this.txtGiftCardCode.Name = "txtGiftCardCode";
			this.txtGiftCardCode.Size = new System.Drawing.Size(266, 40);
			this.txtGiftCardCode.TabIndex = 1;
			this.txtGiftCardCode.Text = "Gift Card Code";
			this.txtGiftCardCode.Click += new System.EventHandler(this.txtKW_Click);
			this.txtGiftCardCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKW_KeyDown);
			this.txtGiftCardCode.Leave += new System.EventHandler(this.txtKW_Leave);
			// 
			// lblFrm
			// 
			this.lblFrm.AutoSize = true;
			this.lblFrm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFrm.Location = new System.Drawing.Point(12, 57);
			this.lblFrm.Name = "lblFrm";
			this.lblFrm.Size = new System.Drawing.Size(43, 16);
			this.lblFrm.TabIndex = 6;
			this.lblFrm.Text = "From";
			// 
			// lblTo
			// 
			this.lblTo.AutoSize = true;
			this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTo.Location = new System.Drawing.Point(148, 57);
			this.lblTo.Name = "lblTo";
			this.lblTo.Size = new System.Drawing.Size(27, 16);
			this.lblTo.TabIndex = 8;
			this.lblTo.Text = "To";
			// 
			// btnSearch
			// 
			this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSearch.Location = new System.Drawing.Point(288, 14);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(174, 85);
			this.btnSearch.TabIndex = 10;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// rtbView
			// 
			this.rtbView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtbView.Location = new System.Drawing.Point(468, 14);
			this.rtbView.Name = "rtbView";
			this.rtbView.ReadOnly = true;
			this.rtbView.Size = new System.Drawing.Size(455, 558);
			this.rtbView.TabIndex = 11;
			this.rtbView.Text = "";
			// 
			// btnPrint
			// 
			this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPrint.Location = new System.Drawing.Point(4, 512);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(132, 64);
			this.btnPrint.TabIndex = 12;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnView
			// 
			this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnView.Location = new System.Drawing.Point(166, 512);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(132, 64);
			this.btnView.TabIndex = 13;
			this.btnView.Text = "View";
			this.btnView.UseVisualStyleBackColor = true;
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// btnClose
			// 
			this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClose.Location = new System.Drawing.Point(328, 512);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(132, 64);
			this.btnClose.TabIndex = 14;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// dpFrom
			// 
			this.dpFrom.Location = new System.Drawing.Point(15, 74);
			this.dpFrom.Name = "dpFrom";
			this.dpFrom.Size = new System.Drawing.Size(121, 20);
			this.dpFrom.TabIndex = 16;
			this.dpFrom.Value = new System.DateTime(2016, 4, 12, 0, 0, 0, 0);
			// 
			// dpTo
			// 
			this.dpTo.Location = new System.Drawing.Point(151, 74);
			this.dpTo.Name = "dpTo";
			this.dpTo.Size = new System.Drawing.Size(115, 20);
			this.dpTo.TabIndex = 17;
			// 
			// lblTotalQty
			// 
			this.lblTotalQty.AutoSize = true;
			this.lblTotalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTotalQty.Location = new System.Drawing.Point(96, 479);
			this.lblTotalQty.Name = "lblTotalQty";
			this.lblTotalQty.Size = new System.Drawing.Size(91, 20);
			this.lblTotalQty.TabIndex = 18;
			this.lblTotalQty.Text = "Total Qty :";
			// 
			// lblTotalValue
			// 
			this.lblTotalValue.AutoSize = true;
			this.lblTotalValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTotalValue.Location = new System.Drawing.Point(260, 479);
			this.lblTotalValue.Name = "lblTotalValue";
			this.lblTotalValue.Size = new System.Drawing.Size(110, 20);
			this.lblTotalValue.TabIndex = 19;
			this.lblTotalValue.Text = "Total Value :";
			// 
			// totalQty
			// 
			this.totalQty.AutoSize = true;
			this.totalQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.totalQty.Location = new System.Drawing.Point(193, 479);
			this.totalQty.Name = "totalQty";
			this.totalQty.Size = new System.Drawing.Size(49, 20);
			this.totalQty.TabIndex = 20;
			this.totalQty.Text = "9999";
			// 
			// totalValue
			// 
			this.totalValue.AutoSize = true;
			this.totalValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.totalValue.Location = new System.Drawing.Point(376, 479);
			this.totalValue.Name = "totalValue";
			this.totalValue.Size = new System.Drawing.Size(84, 20);
			this.totalValue.TabIndex = 21;
			this.totalValue.Text = "$9999.00";
			// 
			// FormLatipayGiftCardHistory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(935, 584);
			this.Controls.Add(this.totalValue);
			this.Controls.Add(this.totalQty);
			this.Controls.Add(this.lblTotalValue);
			this.Controls.Add(this.lblTotalQty);
			this.Controls.Add(this.dpTo);
			this.Controls.Add(this.dpFrom);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnView);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.rtbView);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.lblTo);
			this.Controls.Add(this.lblFrm);
			this.Controls.Add(this.txtGiftCardCode);
			this.Controls.Add(this.lvGiftCard);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormLatipayGiftCardHistory";
			this.Text = "Gift Card History Record";
			this.Load += new System.EventHandler(this.FormLatipayGiftCardHistory_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lvGiftCard;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.TextBox txtGiftCardCode;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Label lblFrm;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.RichTextBox rtbView;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button btnView;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.DateTimePicker dpFrom;
		private System.Windows.Forms.DateTimePicker dpTo;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Label lblTotalQty;
		private System.Windows.Forms.Label lblTotalValue;
		private System.Windows.Forms.Label totalQty;
		private System.Windows.Forms.Label totalValue;

	}
}