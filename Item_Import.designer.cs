namespace QPOS2008
{
    partial class Item_Import
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Item_Import));
            this.dgvimport = new System.Windows.Forms.DataGridView();
            this.ii_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ii_mapto = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ii_mapto_index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ii_name_db = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtfilename = new System.Windows.Forms.TextBox();
            this.btncopy = new System.Windows.Forms.Button();
            this.txtUploadFileName = new System.Windows.Forms.Label();
            this.lv_list = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlMapping = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnmappinglast = new System.Windows.Forms.Button();
            this.btnmappingnext = new System.Windows.Forms.Button();
            this.pnltest = new System.Windows.Forms.Panel();
            this.btntestlast = new System.Windows.Forms.Button();
            this.btntestnext = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.pnlprocess = new System.Windows.Forms.Panel();
            this.lblpleasewait = new System.Windows.Forms.Label();
            this.btnprocess = new System.Windows.Forms.Button();
            this.btnrestart = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblRows = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvimport)).BeginInit();
            this.pnlMapping.SuspendLayout();
            this.pnltest.SuspendLayout();
            this.pnlprocess.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvimport
            // 
            this.dgvimport.AllowUserToAddRows = false;
            this.dgvimport.AllowUserToDeleteRows = false;
            this.dgvimport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvimport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ii_name,
            this.ii_mapto,
            this.ii_mapto_index,
            this.ii_name_db});
            this.dgvimport.Location = new System.Drawing.Point(230, 60);
            this.dgvimport.MultiSelect = false;
            this.dgvimport.Name = "dgvimport";
            this.dgvimport.RowHeadersVisible = false;
            this.dgvimport.Size = new System.Drawing.Size(477, 227);
            this.dgvimport.TabIndex = 0;
            this.dgvimport.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvimport_CellClick_1);
            // 
            // ii_name
            // 
            this.ii_name.HeaderText = "Column Name";
            this.ii_name.Name = "ii_name";
            this.ii_name.ReadOnly = true;
            this.ii_name.Width = 180;
            // 
            // ii_mapto
            // 
            this.ii_mapto.HeaderText = "Map To";
            this.ii_mapto.Name = "ii_mapto";
            this.ii_mapto.Width = 180;
            // 
            // ii_mapto_index
            // 
            this.ii_mapto_index.HeaderText = "index";
            this.ii_mapto_index.Name = "ii_mapto_index";
            this.ii_mapto_index.Visible = false;
            this.ii_mapto_index.Width = 30;
            // 
            // ii_name_db
            // 
            this.ii_name_db.HeaderText = "Column1";
            this.ii_name_db.Name = "ii_name_db";
            this.ii_name_db.Visible = false;
            this.ii_name_db.Width = 30;
            // 
            // txtfilename
            // 
            this.txtfilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtfilename.Location = new System.Drawing.Point(219, 217);
            this.txtfilename.Name = "txtfilename";
            this.txtfilename.Size = new System.Drawing.Size(463, 29);
            this.txtfilename.TabIndex = 1;
            // 
            // btncopy
            // 
            this.btncopy.Location = new System.Drawing.Point(802, 7);
            this.btncopy.Name = "btncopy";
            this.btncopy.Size = new System.Drawing.Size(68, 33);
            this.btncopy.TabIndex = 3;
            this.btncopy.Text = "Upload File";
            this.btncopy.UseVisualStyleBackColor = true;
            this.btncopy.Visible = false;
            this.btncopy.Click += new System.EventHandler(this.btncopy_Click);
            // 
            // txtUploadFileName
            // 
            this.txtUploadFileName.AutoSize = true;
            this.txtUploadFileName.Location = new System.Drawing.Point(412, 406);
            this.txtUploadFileName.Name = "txtUploadFileName";
            this.txtUploadFileName.Size = new System.Drawing.Size(67, 13);
            this.txtUploadFileName.TabIndex = 4;
            this.txtUploadFileName.Text = "Imported File";
            this.txtUploadFileName.Visible = false;
            // 
            // lv_list
            // 
            this.lv_list.FullRowSelect = true;
            this.lv_list.Location = new System.Drawing.Point(11, 133);
            this.lv_list.MultiSelect = false;
            this.lv_list.Name = "lv_list";
            this.lv_list.Size = new System.Drawing.Size(878, 310);
            this.lv_list.TabIndex = 8;
            this.lv_list.UseCompatibleStateImageBehavior = false;
            this.lv_list.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(381, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "Step 1: Select File";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(393, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Step 3: Testing";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(370, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "Step 2: Mapping Column";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(371, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Step 4: Process";
            // 
            // pnlMapping
            // 
            this.pnlMapping.Controls.Add(this.label5);
            this.pnlMapping.Controls.Add(this.btnmappinglast);
            this.pnlMapping.Controls.Add(this.btnmappingnext);
            this.pnlMapping.Controls.Add(this.label3);
            this.pnlMapping.Controls.Add(this.dgvimport);
            this.pnlMapping.Location = new System.Drawing.Point(154, 28);
            this.pnlMapping.Name = "pnlMapping";
            this.pnlMapping.Size = new System.Drawing.Size(152, 107);
            this.pnlMapping.TabIndex = 13;
            this.pnlMapping.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(216, 305);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(371, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "*Column name with red color means  required columns. All others are  optional";
            // 
            // btnmappinglast
            // 
            this.btnmappinglast.BackColor = System.Drawing.Color.Transparent;
            this.btnmappinglast.BackgroundImage = global::QPOS2008.Properties.Resources._118_50;
            this.btnmappinglast.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnmappinglast.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnmappinglast.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnmappinglast.Location = new System.Drawing.Point(219, 342);
            this.btnmappinglast.Name = "btnmappinglast";
            this.btnmappinglast.Size = new System.Drawing.Size(118, 50);
            this.btnmappinglast.TabIndex = 13;
            this.btnmappinglast.Text = "Pre : Import File";
            this.btnmappinglast.UseVisualStyleBackColor = false;
            this.btnmappinglast.Click += new System.EventHandler(this.btnmappinglast_Click);
            // 
            // btnmappingnext
            // 
            this.btnmappingnext.BackColor = System.Drawing.Color.Transparent;
            this.btnmappingnext.BackgroundImage = global::QPOS2008.Properties.Resources._118_50;
            this.btnmappingnext.Enabled = false;
            this.btnmappingnext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnmappingnext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnmappingnext.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnmappingnext.Location = new System.Drawing.Point(561, 341);
            this.btnmappingnext.Name = "btnmappingnext";
            this.btnmappingnext.Size = new System.Drawing.Size(118, 50);
            this.btnmappingnext.TabIndex = 12;
            this.btnmappingnext.Text = "Next : Testing";
            this.btnmappingnext.UseVisualStyleBackColor = false;
            this.btnmappingnext.Click += new System.EventHandler(this.btnmappingnext_Click);
            // 
            // pnltest
            // 
            this.pnltest.Controls.Add(this.btntestlast);
            this.pnltest.Controls.Add(this.btntestnext);
            this.pnltest.Controls.Add(this.label2);
            this.pnltest.Controls.Add(this.btnTest);
            this.pnltest.Controls.Add(this.lv_list);
            this.pnltest.Location = new System.Drawing.Point(5, 119);
            this.pnltest.Name = "pnltest";
            this.pnltest.Size = new System.Drawing.Size(114, 301);
            this.pnltest.TabIndex = 14;
            this.pnltest.Visible = false;
            // 
            // btntestlast
            // 
            this.btntestlast.BackColor = System.Drawing.Color.Transparent;
            this.btntestlast.BackgroundImage = global::QPOS2008.Properties.Resources._155_55;
            this.btntestlast.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btntestlast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btntestlast.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btntestlast.Location = new System.Drawing.Point(11, 447);
            this.btntestlast.Name = "btntestlast";
            this.btntestlast.Size = new System.Drawing.Size(155, 55);
            this.btntestlast.TabIndex = 12;
            this.btntestlast.Text = "Pre : Column Mapping";
            this.btntestlast.UseVisualStyleBackColor = false;
            this.btntestlast.Click += new System.EventHandler(this.btntestlast_Click);
            // 
            // btntestnext
            // 
            this.btntestnext.BackColor = System.Drawing.Color.Transparent;
            this.btntestnext.BackgroundImage = global::QPOS2008.Properties.Resources._155_55;
            this.btntestnext.Enabled = false;
            this.btntestnext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btntestnext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btntestnext.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btntestnext.Location = new System.Drawing.Point(734, 447);
            this.btntestnext.Name = "btntestnext";
            this.btntestnext.Size = new System.Drawing.Size(155, 55);
            this.btntestnext.TabIndex = 11;
            this.btntestnext.Text = "Next : Process";
            this.btntestnext.UseVisualStyleBackColor = false;
            this.btntestnext.Click += new System.EventHandler(this.btntestnext_Click);
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.Transparent;
            this.btnTest.BackgroundImage = global::QPOS2008.Properties.Resources._136_55;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTest.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnTest.Location = new System.Drawing.Point(384, 73);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(136, 55);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // pnlprocess
            // 
            this.pnlprocess.Controls.Add(this.lblpleasewait);
            this.pnlprocess.Controls.Add(this.label4);
            this.pnlprocess.Controls.Add(this.btnprocess);
            this.pnlprocess.Location = new System.Drawing.Point(349, 7);
            this.pnlprocess.Name = "pnlprocess";
            this.pnlprocess.Size = new System.Drawing.Size(536, 230);
            this.pnlprocess.TabIndex = 15;
            this.pnlprocess.Visible = false;
            // 
            // lblpleasewait
            // 
            this.lblpleasewait.AutoSize = true;
            this.lblpleasewait.ForeColor = System.Drawing.Color.Red;
            this.lblpleasewait.Location = new System.Drawing.Point(341, 214);
            this.lblpleasewait.Name = "lblpleasewait";
            this.lblpleasewait.Size = new System.Drawing.Size(192, 13);
            this.lblpleasewait.TabIndex = 13;
            this.lblpleasewait.Text = "Please Wait, System is importing data...";
            this.lblpleasewait.Visible = false;
            // 
            // btnprocess
            // 
            this.btnprocess.BackColor = System.Drawing.Color.Transparent;
            this.btnprocess.BackgroundImage = global::QPOS2008.Properties.Resources._152_107;
            this.btnprocess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnprocess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnprocess.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnprocess.Location = new System.Drawing.Point(361, 67);
            this.btnprocess.Name = "btnprocess";
            this.btnprocess.Size = new System.Drawing.Size(152, 107);
            this.btnprocess.TabIndex = 7;
            this.btnprocess.Text = "Process";
            this.btnprocess.UseVisualStyleBackColor = false;
            this.btnprocess.Click += new System.EventHandler(this.btnprocess_Click);
            // 
            // btnrestart
            // 
            this.btnrestart.BackColor = System.Drawing.Color.Transparent;
            this.btnrestart.BackgroundImage = global::QPOS2008.Properties.Resources.adminBTN_red;
            this.btnrestart.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnrestart.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnrestart.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnrestart.Location = new System.Drawing.Point(12, 7);
            this.btnrestart.Name = "btnrestart";
            this.btnrestart.Size = new System.Drawing.Size(96, 33);
            this.btnrestart.TabIndex = 13;
            this.btnrestart.Text = "Start Over";
            this.btnrestart.UseVisualStyleBackColor = false;
            this.btnrestart.Visible = false;
            this.btnrestart.Click += new System.EventHandler(this.btnrestart_Click);
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.Transparent;
            this.btnImport.BackgroundImage = global::QPOS2008.Properties.Resources._125_92;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnImport.Location = new System.Drawing.Point(384, 278);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(125, 92);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Browse File";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(48, 601);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(13, 13);
            this.lblRows.TabIndex = 16;
            this.lblRows.Text = "0";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(109, 598);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(752, 23);
            this.progressBar.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 601);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Rows:";
            // 
            // Item_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 634);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblRows);
            this.Controls.Add(this.btnrestart);
            this.Controls.Add(this.pnlprocess);
            this.Controls.Add(this.pnltest);
            this.Controls.Add(this.pnlMapping);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUploadFileName);
            this.Controls.Add(this.btncopy);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtfilename);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Item_Import";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Import";
            this.Load += new System.EventHandler(this.Item_Import_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvimport)).EndInit();
            this.pnlMapping.ResumeLayout(false);
            this.pnlMapping.PerformLayout();
            this.pnltest.ResumeLayout(false);
            this.pnltest.PerformLayout();
            this.pnlprocess.ResumeLayout(false);
            this.pnlprocess.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.DataGridView dgvimport;
        private System.Windows.Forms.TextBox txtfilename;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btncopy;
		private System.Windows.Forms.Label txtUploadFileName;
		private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnprocess;
		private System.Windows.Forms.ListView lv_list;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel pnlMapping;
		private System.Windows.Forms.Button btnmappingnext;
		private System.Windows.Forms.Panel pnltest;
		private System.Windows.Forms.Button btntestnext;
		private System.Windows.Forms.Panel pnlprocess;
		private System.Windows.Forms.Button btnmappinglast;
		private System.Windows.Forms.Button btntestlast;
		private System.Windows.Forms.Button btnrestart;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblpleasewait;
		private System.Windows.Forms.Label lblRows;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.DataGridViewTextBoxColumn ii_name;
		private System.Windows.Forms.DataGridViewComboBoxColumn ii_mapto;
		private System.Windows.Forms.DataGridViewTextBoxColumn ii_mapto_index;
		private System.Windows.Forms.DataGridViewTextBoxColumn ii_name_db;
    }
}