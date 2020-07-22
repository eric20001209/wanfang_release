namespace QPOS2008
{
	partial class invoicetrace
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(invoicetrace));
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.txtKW = new System.Windows.Forms.TextBox();
			this.lblFrm = new System.Windows.Forms.Label();
			this.lblTo = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.rtbView = new System.Windows.Forms.RichTextBox();
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnView = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.dpFrom = new System.Windows.Forms.DateTimePicker();
			this.dpTo = new System.Windows.Forms.DateTimePicker();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.BackColor = System.Drawing.Color.White;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listView1.FullRowSelect = true;
			this.listView1.HideSelection = false;
			this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
			this.listView1.Location = new System.Drawing.Point(12, 106);
			this.listView1.Margin = new System.Windows.Forms.Padding(9);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(451, 350);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Till";
			this.columnHeader1.Width = 49;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Invoice";
			this.columnHeader2.Width = 91;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Date Time";
			this.columnHeader3.Width = 186;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Amount";
			this.columnHeader4.Width = 119;
			// 
			// txtKW
			// 
			this.txtKW.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtKW.ForeColor = System.Drawing.Color.Gainsboro;
			this.txtKW.Location = new System.Drawing.Point(12, 12);
			this.txtKW.Name = "txtKW";
			this.txtKW.Size = new System.Drawing.Size(266, 40);
			this.txtKW.TabIndex = 1;
			this.txtKW.Text = "Invoice Number";
			this.txtKW.Click += new System.EventHandler(this.txtKW_Click);
			this.txtKW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKW_KeyDown);
			this.txtKW.Leave += new System.EventHandler(this.txtKW_Leave);
			// 
			// lblFrm
			// 
			this.lblFrm.AutoSize = true;
			this.lblFrm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFrm.Location = new System.Drawing.Point(12, 57);
			this.lblFrm.Name = "lblFrm";
			this.lblFrm.Size = new System.Drawing.Size(44, 16);
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
			this.btnPrint.Location = new System.Drawing.Point(4, 468);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(132, 103);
			this.btnPrint.TabIndex = 12;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnView
			// 
			this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnView.Location = new System.Drawing.Point(401, -6);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(98, 50);
			this.btnView.TabIndex = 13;
			this.btnView.Text = "View";
			this.btnView.UseVisualStyleBackColor = true;
			this.btnView.Visible = false;
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// btnClose
			// 
			this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClose.Location = new System.Drawing.Point(328, 468);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(132, 103);
			this.btnClose.TabIndex = 14;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnExport
			// 
			this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExport.Location = new System.Drawing.Point(166, 468);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(132, 103);
			this.btnExport.TabIndex = 15;
			this.btnExport.Text = "No GST Inv";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
			// invoicetrace
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(935, 584);
			this.Controls.Add(this.dpTo);
			this.Controls.Add(this.dpFrom);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnView);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.rtbView);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.lblTo);
			this.Controls.Add(this.lblFrm);
			this.Controls.Add(this.txtKW);
			this.Controls.Add(this.listView1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "invoicetrace";
			this.Text = "invoicetrace";
			this.Load += new System.EventHandler(this.invoicetrace_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.TextBox txtKW;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Label lblFrm;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.RichTextBox rtbView;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button btnView;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.DateTimePicker dpFrom;
		private System.Windows.Forms.DateTimePicker dpTo;

	}
}