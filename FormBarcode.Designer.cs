namespace QPOS2008
{
	partial class FormBarcode
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
			this.label1 = new System.Windows.Forms.Label();
			this.textBarcode = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.labelItemName = new System.Windows.Forms.Label();
			this.labelItemNameCN = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textQty = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(31, 16);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Item : ";
			// 
			// textBarcode
			// 
			this.textBarcode.Location = new System.Drawing.Point(80, 81);
			this.textBarcode.Name = "textBarcode";
			this.textBarcode.Size = new System.Drawing.Size(202, 23);
			this.textBarcode.TabIndex = 1;
			this.textBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBarcode_KeyDown);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label2.Location = new System.Drawing.Point(23, 86);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Barcode : ";
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(347, 108);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(75, 23);
			this.buttonAdd.TabIndex = 3;
			this.buttonAdd.Text = "Add";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// labelItemName
			// 
			this.labelItemName.AutoSize = true;
			this.labelItemName.Location = new System.Drawing.Point(77, 16);
			this.labelItemName.Name = "labelItemName";
			this.labelItemName.Size = new System.Drawing.Size(79, 16);
			this.labelItemName.TabIndex = 4;
			this.labelItemName.Text = "item_name";
			// 
			// labelItemNameCN
			// 
			this.labelItemNameCN.AutoSize = true;
			this.labelItemNameCN.Location = new System.Drawing.Point(77, 37);
			this.labelItemNameCN.Name = "labelItemNameCN";
			this.labelItemNameCN.Size = new System.Drawing.Size(103, 16);
			this.labelItemNameCN.TabIndex = 5;
			this.labelItemNameCN.Text = "item_name_cn";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label3.Location = new System.Drawing.Point(295, 86);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Qty : ";
			// 
			// textQty
			// 
			this.textQty.Location = new System.Drawing.Point(325, 81);
			this.textQty.Name = "textQty";
			this.textQty.Size = new System.Drawing.Size(97, 23);
			this.textQty.TabIndex = 7;
			this.textQty.Text = "1";
			// 
			// FormBarcode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightBlue;
			this.ClientSize = new System.Drawing.Size(460, 143);
			this.Controls.Add(this.textQty);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelItemNameCN);
			this.Controls.Add(this.labelItemName);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBarcode);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "FormBarcode";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add New Barcode - QPOS2008";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FormBarcode_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormBarcode_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBarcode;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonAdd;
		private System.Windows.Forms.Label labelItemName;
		private System.Windows.Forms.Label labelItemNameCN;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textQty;
	}
}