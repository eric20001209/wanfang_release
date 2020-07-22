namespace QPOS2008
{
	partial class FormBOM
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBOM));
			this.label1 = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lvBOM = new System.Windows.Forms.ListView();
			this.cbCode = new System.Windows.Forms.ColumnHeader();
			this.cbName = new System.Windows.Forms.ColumnHeader();
			this.cbQty = new System.Windows.Forms.ColumnHeader();
			this.cbEdit = new System.Windows.Forms.ColumnHeader();
			this.cbDel = new System.Windows.Forms.ColumnHeader();
			this.label2 = new System.Windows.Forms.Label();
			this.lvitemlist = new System.Windows.Forms.ListView();
			this.cc_code = new System.Windows.Forms.ColumnHeader();
			this.cc_depart = new System.Windows.Forms.ColumnHeader();
			this.cc_cat = new System.Windows.Forms.ColumnHeader();
			this.cc_scat = new System.Windows.Forms.ColumnHeader();
			this.cc_barcode = new System.Windows.Forms.ColumnHeader();
			this.cc_mpn = new System.Windows.Forms.ColumnHeader();
			this.cc_name = new System.Windows.Forms.ColumnHeader();
			this.cc_act = new System.Windows.Forms.ColumnHeader();
			this.txtsearch = new System.Windows.Forms.TextBox();
			this.label140 = new System.Windows.Forms.Label();
			this.cbCatFilter = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnSearch = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnClose = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(15, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(126, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Description:";
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDescription.Location = new System.Drawing.Point(147, 9);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(166, 25);
			this.lblDescription.TabIndex = 1;
			this.lblDescription.Text = "Item Description";
			// 
			// lvBOM
			// 
			this.lvBOM.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cbCode,
            this.cbName,
            this.cbQty,
            this.cbEdit,
            this.cbDel});
			this.lvBOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvBOM.FullRowSelect = true;
			this.lvBOM.GridLines = true;
			this.lvBOM.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvBOM.Location = new System.Drawing.Point(14, 12);
			this.lvBOM.Name = "lvBOM";
			this.lvBOM.Size = new System.Drawing.Size(477, 205);
			this.lvBOM.TabIndex = 46;
			this.lvBOM.UseCompatibleStateImageBehavior = false;
			this.lvBOM.View = System.Windows.Forms.View.Details;
			this.lvBOM.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvBOM_MouseClick);
			// 
			// cbCode
			// 
			this.cbCode.Text = "Code";
			this.cbCode.Width = 150;
			// 
			// cbName
			// 
			this.cbName.Text = "Name";
			this.cbName.Width = 160;
			// 
			// cbQty
			// 
			this.cbQty.Text = "Qty";
			// 
			// cbEdit
			// 
			this.cbEdit.Text = "";
			this.cbEdit.Width = 40;
			// 
			// cbDel
			// 
			this.cbDel.Text = "";
			this.cbDel.Width = 40;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(11, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Add Item:";
			// 
			// lvitemlist
			// 
			this.lvitemlist.BackColor = System.Drawing.Color.White;
			this.lvitemlist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cc_code,
            this.cc_depart,
            this.cc_cat,
            this.cc_scat,
            this.cc_barcode,
            this.cc_mpn,
            this.cc_name,
            this.cc_act});
			this.lvitemlist.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvitemlist.FullRowSelect = true;
			this.lvitemlist.GridLines = true;
			this.lvitemlist.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvitemlist.HideSelection = false;
			this.lvitemlist.Location = new System.Drawing.Point(14, 43);
			this.lvitemlist.MultiSelect = false;
			this.lvitemlist.Name = "lvitemlist";
			this.lvitemlist.Size = new System.Drawing.Size(877, 317);
			this.lvitemlist.TabIndex = 47;
			this.lvitemlist.UseCompatibleStateImageBehavior = false;
			this.lvitemlist.View = System.Windows.Forms.View.Details;
			this.lvitemlist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvitemlist_MouseClick);
			this.lvitemlist.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvitemlist_MouseClick);
			// 
			// cc_code
			// 
			this.cc_code.Text = "Code";
			// 
			// cc_depart
			// 
			this.cc_depart.Text = "Dept";
			this.cc_depart.Width = 90;
			// 
			// cc_cat
			// 
			this.cc_cat.Text = "Cat";
			this.cc_cat.Width = 90;
			// 
			// cc_scat
			// 
			this.cc_scat.Text = "SubCat";
			this.cc_scat.Width = 90;
			// 
			// cc_barcode
			// 
			this.cc_barcode.Text = "Barcode";
			this.cc_barcode.Width = 90;
			// 
			// cc_mpn
			// 
			this.cc_mpn.Text = "SupplierCode";
			this.cc_mpn.Width = 110;
			// 
			// cc_name
			// 
			this.cc_name.Text = "Description";
			this.cc_name.Width = 270;
			// 
			// cc_act
			// 
			this.cc_act.Text = "";
			// 
			// txtsearch
			// 
			this.txtsearch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtsearch.Location = new System.Drawing.Point(569, 10);
			this.txtsearch.Name = "txtsearch";
			this.txtsearch.Size = new System.Drawing.Size(201, 26);
			this.txtsearch.TabIndex = 50;
			// 
			// label140
			// 
			this.label140.AutoSize = true;
			this.label140.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label140.ForeColor = System.Drawing.Color.Black;
			this.label140.Location = new System.Drawing.Point(379, 16);
			this.label140.Name = "label140";
			this.label140.Size = new System.Drawing.Size(50, 15);
			this.label140.TabIndex = 48;
			this.label140.Text = "Catalog";
			// 
			// cbCatFilter
			// 
			this.cbCatFilter.BackColor = System.Drawing.Color.WhiteSmoke;
			this.cbCatFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbCatFilter.FormattingEnabled = true;
			this.cbCatFilter.Location = new System.Drawing.Point(435, 11);
			this.cbCatFilter.Name = "cbCatFilter";
			this.cbCatFilter.Size = new System.Drawing.Size(128, 24);
			this.cbCatFilter.TabIndex = 49;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lvBOM);
			this.panel1.Location = new System.Drawing.Point(16, 49);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(504, 234);
			this.panel1.TabIndex = 52;
			// 
			// btnSearch
			// 
			this.btnSearch.BackgroundImage = global::QPOS2008.Properties.Resources._115_30;
			this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSearch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSearch.ForeColor = System.Drawing.Color.White;
			this.btnSearch.Location = new System.Drawing.Point(776, 7);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(115, 30);
			this.btnSearch.TabIndex = 51;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.lvitemlist);
			this.panel2.Controls.Add(this.btnSearch);
			this.panel2.Controls.Add(this.cbCatFilter);
			this.panel2.Controls.Add(this.txtsearch);
			this.panel2.Controls.Add(this.label140);
			this.panel2.Location = new System.Drawing.Point(16, 297);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(903, 374);
			this.panel2.TabIndex = 53;
			// 
			// btnClose
			// 
			this.btnClose.BackgroundImage = global::QPOS2008.Properties.Resources._115_30;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(804, 677);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(115, 30);
			this.btnClose.TabIndex = 54;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// FormBOM
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(934, 718);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormBOM";
			this.Text = "Edit BOM";
			this.Load += new System.EventHandler(this.FormBOM_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.ListView lvBOM;
		private System.Windows.Forms.ColumnHeader cbCode;
		private System.Windows.Forms.ColumnHeader cbQty;
		private System.Windows.Forms.ColumnHeader cbDel;
		private System.Windows.Forms.ColumnHeader cbName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListView lvitemlist;
		private System.Windows.Forms.ColumnHeader cc_depart;
		private System.Windows.Forms.ColumnHeader cc_code;
		private System.Windows.Forms.ColumnHeader cc_name;
		private System.Windows.Forms.ColumnHeader cc_act;
		private System.Windows.Forms.ColumnHeader cc_cat;
		private System.Windows.Forms.TextBox txtsearch;
		private System.Windows.Forms.Label label140;
		private System.Windows.Forms.ComboBox cbCatFilter;
		private System.Windows.Forms.ColumnHeader cc_barcode;
		private System.Windows.Forms.ColumnHeader cc_scat;
		private System.Windows.Forms.ColumnHeader cc_mpn;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ColumnHeader cbEdit;
		private System.Windows.Forms.Button btnClose;
	}
}