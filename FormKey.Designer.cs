namespace QPOS2008
{
	partial class FormKey
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.vKeys = new System.Windows.Forms.DataGridView();
			this.button1 = new System.Windows.Forms.Button();
			this.cc_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_code_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_key_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_key_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cc_remove = new System.Windows.Forms.DataGridViewButtonColumn();
			this.cc_last = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.vKeys)).BeginInit();
			this.SuspendLayout();
			// 
			// vKeys
			// 
			this.vKeys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.vKeys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cc_id,
            this.cc_name,
            this.cc_code_name,
            this.cc_key_code,
            this.cc_key_name,
            this.cc_remove,
            this.cc_last});
			this.vKeys.Location = new System.Drawing.Point(12, 12);
			this.vKeys.Name = "vKeys";
			this.vKeys.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.vKeys.Size = new System.Drawing.Size(708, 656);
			this.vKeys.TabIndex = 0;
			this.vKeys.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.vKeys_CellClick);
			this.vKeys.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.vKeys_DefaultValuesNeeded);
			this.vKeys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.vKeys_KeyDown);
			this.vKeys.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.vKeys_CellContentClick);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(645, 674);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// cc_id
			// 
			this.cc_id.HeaderText = "ID";
			this.cc_id.Name = "cc_id";
			this.cc_id.Width = 40;
			// 
			// cc_name
			// 
			this.cc_name.HeaderText = "Description";
			this.cc_name.Name = "cc_name";
			this.cc_name.Width = 200;
			// 
			// cc_code_name
			// 
			this.cc_code_name.HeaderText = "CodeName";
			this.cc_code_name.Name = "cc_code_name";
			// 
			// cc_key_code
			// 
			this.cc_key_code.HeaderText = "Key Code";
			this.cc_key_code.Name = "cc_key_code";
			this.cc_key_code.ReadOnly = true;
			// 
			// cc_key_name
			// 
			this.cc_key_name.HeaderText = "Key Name";
			this.cc_key_name.Name = "cc_key_name";
			this.cc_key_name.ReadOnly = true;
			this.cc_key_name.Width = 150;
			// 
			// cc_remove
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.cc_remove.DefaultCellStyle = dataGridViewCellStyle3;
			this.cc_remove.HeaderText = "";
			this.cc_remove.Name = "cc_remove";
			this.cc_remove.ReadOnly = true;
			this.cc_remove.Width = 70;
			// 
			// cc_last
			// 
			this.cc_last.HeaderText = "";
			this.cc_last.Name = "cc_last";
			// 
			// FormKey
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(732, 702);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.vKeys);
			this.Name = "FormKey";
			this.Text = "HotKey Assignment";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FormKey_Load);
			((System.ComponentModel.ISupportInitialize)(this.vKeys)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView vKeys;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_id;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_name;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_code_name;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_key_code;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_key_name;
		private System.Windows.Forms.DataGridViewButtonColumn cc_remove;
		private System.Windows.Forms.DataGridViewTextBoxColumn cc_last;
	}
}