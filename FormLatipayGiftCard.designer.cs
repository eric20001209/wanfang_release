namespace QPOS2008
{
	partial class FormLatipayGiftCard
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLatipayGiftCard));
			this.btnUse = new System.Windows.Forms.Button();
			this.txtCardNumber = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnVerify = new System.Windows.Forms.Button();
			this.lblFaceValue = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblVerifiedCardCode = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox38 = new System.Windows.Forms.GroupBox();
			this.btnTryAgain = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.dgvVerifiedCardList = new System.Windows.Forms.DataGridView();
			this.dgvGiftCardCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvFaceValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvAction = new System.Windows.Forms.DataGridViewButtonColumn();
			this.dgvId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnHistoryRecord = new System.Windows.Forms.Button();
			this.groupBox38.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvVerifiedCardList)).BeginInit();
			this.SuspendLayout();
			// 
			// btnUse
			// 
			this.btnUse.BackColor = System.Drawing.Color.Green;
			this.btnUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUse.ForeColor = System.Drawing.Color.White;
			this.btnUse.Location = new System.Drawing.Point(461, 73);
			this.btnUse.Name = "btnUse";
			this.btnUse.Size = new System.Drawing.Size(100, 33);
			this.btnUse.TabIndex = 9;
			this.btnUse.Text = "Use";
			this.btnUse.UseVisualStyleBackColor = false;
			this.btnUse.Click += new System.EventHandler(this.btnUse_Click);
			// 
			// txtCardNumber
			// 
			this.txtCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCardNumber.Location = new System.Drawing.Point(169, 12);
			this.txtCardNumber.Name = "txtCardNumber";
			this.txtCardNumber.Size = new System.Drawing.Size(310, 31);
			this.txtCardNumber.TabIndex = 6;
			this.txtCardNumber.Click += new System.EventHandler(this.txtCardNumber_Click);
			this.txtCardNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCardNumber_KeyUp);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(24, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 25);
			this.label2.TabIndex = 7;
			this.label2.Text = "Gift Card No.";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(6, 111);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 25);
			this.label1.TabIndex = 5;
			this.label1.Text = "Face Value : ";
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.Red;
			this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(345, 493);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(240, 33);
			this.btnClose.TabIndex = 23;
			this.btnClose.Text = "Close And Update Cart";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnVerify
			// 
			this.btnVerify.BackColor = System.Drawing.SystemColors.MenuHighlight;
			this.btnVerify.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnVerify.ForeColor = System.Drawing.Color.White;
			this.btnVerify.Location = new System.Drawing.Point(485, 11);
			this.btnVerify.Name = "btnVerify";
			this.btnVerify.Size = new System.Drawing.Size(100, 33);
			this.btnVerify.TabIndex = 24;
			this.btnVerify.Text = "Verify";
			this.btnVerify.UseVisualStyleBackColor = false;
			this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
			// 
			// lblFaceValue
			// 
			this.lblFaceValue.AutoSize = true;
			this.lblFaceValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFaceValue.ForeColor = System.Drawing.Color.Black;
			this.lblFaceValue.Location = new System.Drawing.Point(147, 111);
			this.lblFaceValue.Name = "lblFaceValue";
			this.lblFaceValue.Size = new System.Drawing.Size(60, 25);
			this.lblFaceValue.TabIndex = 25;
			this.lblFaceValue.Text = "$100";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStatus.ForeColor = System.Drawing.Color.Black;
			this.lblStatus.Location = new System.Drawing.Point(99, 73);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(100, 25);
			this.lblStatus.TabIndex = 27;
			this.lblStatus.Text = "Available";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.Black;
			this.label5.Location = new System.Drawing.Point(6, 73);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(91, 25);
			this.label5.TabIndex = 26;
			this.label5.Text = "Status : ";
			// 
			// lblVerifiedCardCode
			// 
			this.lblVerifiedCardCode.AutoSize = true;
			this.lblVerifiedCardCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVerifiedCardCode.ForeColor = System.Drawing.Color.Black;
			this.lblVerifiedCardCode.Location = new System.Drawing.Point(195, 34);
			this.lblVerifiedCardCode.Name = "lblVerifiedCardCode";
			this.lblVerifiedCardCode.Size = new System.Drawing.Size(156, 25);
			this.lblVerifiedCardCode.TabIndex = 29;
			this.lblVerifiedCardCode.Text = "789456123789";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.Black;
			this.label7.Location = new System.Drawing.Point(6, 34);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(194, 25);
			this.label7.TabIndex = 28;
			this.label7.Text = "Verified Card No. : ";
			// 
			// groupBox38
			// 
			this.groupBox38.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox38.Controls.Add(this.btnTryAgain);
			this.groupBox38.Controls.Add(this.label7);
			this.groupBox38.Controls.Add(this.lblVerifiedCardCode);
			this.groupBox38.Controls.Add(this.label1);
			this.groupBox38.Controls.Add(this.btnUse);
			this.groupBox38.Controls.Add(this.lblFaceValue);
			this.groupBox38.Controls.Add(this.lblStatus);
			this.groupBox38.Controls.Add(this.label5);
			this.groupBox38.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox38.ForeColor = System.Drawing.Color.SteelBlue;
			this.groupBox38.Location = new System.Drawing.Point(24, 61);
			this.groupBox38.Name = "groupBox38";
			this.groupBox38.Size = new System.Drawing.Size(565, 151);
			this.groupBox38.TabIndex = 30;
			this.groupBox38.TabStop = false;
			this.groupBox38.Text = "Currently Verified Card Info";
			// 
			// btnTryAgain
			// 
			this.btnTryAgain.BackColor = System.Drawing.Color.Orange;
			this.btnTryAgain.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTryAgain.ForeColor = System.Drawing.Color.White;
			this.btnTryAgain.Location = new System.Drawing.Point(448, 111);
			this.btnTryAgain.Name = "btnTryAgain";
			this.btnTryAgain.Size = new System.Drawing.Size(113, 33);
			this.btnTryAgain.TabIndex = 30;
			this.btnTryAgain.Text = "Try Again";
			this.btnTryAgain.UseVisualStyleBackColor = false;
			this.btnTryAgain.Visible = false;
			this.btnTryAgain.Click += new System.EventHandler(this.btnTryAgain_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.dgvVerifiedCardList);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.ForeColor = System.Drawing.Color.SteelBlue;
			this.groupBox1.Location = new System.Drawing.Point(24, 218);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(565, 269);
			this.groupBox1.TabIndex = 31;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Used Card List";
			// 
			// dgvVerifiedCardList
			// 
			this.dgvVerifiedCardList.AllowUserToAddRows = false;
			this.dgvVerifiedCardList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvVerifiedCardList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvGiftCardCode,
            this.dgvReference,
            this.dgvFaceValue,
            this.dgvAction,
            this.dgvId});
			this.dgvVerifiedCardList.Location = new System.Drawing.Point(6, 21);
			this.dgvVerifiedCardList.Name = "dgvVerifiedCardList";
			this.dgvVerifiedCardList.RowHeadersVisible = false;
			this.dgvVerifiedCardList.RowTemplate.Height = 50;
			this.dgvVerifiedCardList.Size = new System.Drawing.Size(553, 242);
			this.dgvVerifiedCardList.TabIndex = 3;
			this.dgvVerifiedCardList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVerifiedCardList_CellClick);
			// 
			// dgvGiftCardCode
			// 
			this.dgvGiftCardCode.HeaderText = "Gift Card Code";
			this.dgvGiftCardCode.Name = "dgvGiftCardCode";
			this.dgvGiftCardCode.ReadOnly = true;
			this.dgvGiftCardCode.Width = 180;
			// 
			// dgvReference
			// 
			this.dgvReference.HeaderText = "Reference";
			this.dgvReference.Name = "dgvReference";
			this.dgvReference.ReadOnly = true;
			this.dgvReference.Width = 180;
			// 
			// dgvFaceValue
			// 
			this.dgvFaceValue.HeaderText = "Face Value";
			this.dgvFaceValue.Name = "dgvFaceValue";
			this.dgvFaceValue.ReadOnly = true;
			this.dgvFaceValue.Width = 120;
			// 
			// dgvAction
			// 
			this.dgvAction.HeaderText = "Action";
			this.dgvAction.Name = "dgvAction";
			this.dgvAction.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvAction.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvAction.Width = 70;
			// 
			// dgvId
			// 
			this.dgvId.HeaderText = "id";
			this.dgvId.Name = "dgvId";
			this.dgvId.Visible = false;
			this.dgvId.Width = 60;
			// 
			// btnHistoryRecord
			// 
			this.btnHistoryRecord.BackColor = System.Drawing.Color.Maroon;
			this.btnHistoryRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnHistoryRecord.ForeColor = System.Drawing.Color.White;
			this.btnHistoryRecord.Location = new System.Drawing.Point(24, 493);
			this.btnHistoryRecord.Name = "btnHistoryRecord";
			this.btnHistoryRecord.Size = new System.Drawing.Size(97, 33);
			this.btnHistoryRecord.TabIndex = 32;
			this.btnHistoryRecord.Text = "History";
			this.btnHistoryRecord.UseVisualStyleBackColor = false;
			this.btnHistoryRecord.Click += new System.EventHandler(this.btnHistoryRecord_Click);
			// 
			// FormLatipayGiftCard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(609, 528);
			this.ControlBox = false;
			this.Controls.Add(this.btnHistoryRecord);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox38);
			this.Controls.Add(this.btnVerify);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.txtCardNumber);
			this.Controls.Add(this.label2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormLatipayGiftCard";
			this.Text = "Latipay Gift Card";
			this.Load += new System.EventHandler(this.FormLatipayGiftCard_Load);
			this.groupBox38.ResumeLayout(false);
			this.groupBox38.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvVerifiedCardList)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnUse;
		private System.Windows.Forms.TextBox txtCardNumber;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnVerify;
		private System.Windows.Forms.Label lblFaceValue;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblVerifiedCardCode;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox38;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGridView dgvVerifiedCardList;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvGiftCardCode;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvReference;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvFaceValue;
		private System.Windows.Forms.DataGridViewButtonColumn dgvAction;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvId;
		private System.Windows.Forms.Button btnTryAgain;
		private System.Windows.Forms.Button btnHistoryRecord;
	}
}