namespace QPOS2008
{
	partial class FormDebug
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
			this.btnLanuch = new System.Windows.Forms.Button();
			this.txtSql = new System.Windows.Forms.TextBox();
			this.btnQuery = new System.Windows.Forms.Button();
			this.dgv = new System.Windows.Forms.DataGridView();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblTime = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
			this.SuspendLayout();
			// 
			// btnLanuch
			// 
			this.btnLanuch.Location = new System.Drawing.Point(175, 236);
			this.btnLanuch.Name = "btnLanuch";
			this.btnLanuch.Size = new System.Drawing.Size(75, 23);
			this.btnLanuch.TabIndex = 0;
			this.btnLanuch.Text = "Launch App";
			this.btnLanuch.UseVisualStyleBackColor = true;
			this.btnLanuch.Click += new System.EventHandler(this.btnLanuch_Click);
			// 
			// txtSql
			// 
			this.txtSql.Location = new System.Drawing.Point(12, 12);
			this.txtSql.Multiline = true;
			this.txtSql.Name = "txtSql";
			this.txtSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSql.Size = new System.Drawing.Size(824, 217);
			this.txtSql.TabIndex = 1;
			this.txtSql.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSql_KeyUp);
			this.txtSql.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSql_KeyPress);
			// 
			// btnQuery
			// 
			this.btnQuery.Location = new System.Drawing.Point(12, 236);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(75, 23);
			this.btnQuery.TabIndex = 2;
			this.btnQuery.Text = "Query";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			// 
			// dgv
			// 
			this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Location = new System.Drawing.Point(13, 265);
			this.dgv.Name = "dgv";
			this.dgv.Size = new System.Drawing.Size(823, 406);
			this.dgv.TabIndex = 3;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(94, 236);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lblTime
			// 
			this.lblTime.AutoSize = true;
			this.lblTime.Location = new System.Drawing.Point(269, 241);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(0, 13);
			this.lblTime.TabIndex = 5;
			// 
			// FormDebug
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(848, 699);
			this.Controls.Add(this.lblTime);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.dgv);
			this.Controls.Add(this.btnQuery);
			this.Controls.Add(this.txtSql);
			this.Controls.Add(this.btnLanuch);
			this.Name = "FormDebug";
			this.Text = "DEBUG TOOL";
			this.Load += new System.EventHandler(this.FormDebug_Load);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormDebug_KeyPress);
			this.Resize += new System.EventHandler(this.FormDebug_Resize);
			((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnLanuch;
		private System.Windows.Forms.TextBox txtSql;
		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.DataGridView dgv;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblTime;
	}
}