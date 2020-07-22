namespace QPOS2008
{
    partial class EFTPOS
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EFTPOS));
			this.txtResposneEftpos = new System.Windows.Forms.TextBox();
			this.txtResponseEftLine2 = new System.Windows.Forms.TextBox();
			this.btnInt = new System.Windows.Forms.Button();
			this.btnSettle = new System.Windows.Forms.Button();
			this.btnok = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnCancel = new System.Windows.Forms.Button();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.timer3 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// txtResposneEftpos
			// 
			this.txtResposneEftpos.BackColor = System.Drawing.SystemColors.Control;
			this.txtResposneEftpos.Enabled = false;
			this.txtResposneEftpos.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtResposneEftpos.Location = new System.Drawing.Point(28, 12);
			this.txtResposneEftpos.Name = "txtResposneEftpos";
			this.txtResposneEftpos.Size = new System.Drawing.Size(469, 49);
			this.txtResposneEftpos.TabIndex = 100;
			this.txtResposneEftpos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// txtResponseEftLine2
			// 
			this.txtResponseEftLine2.BackColor = System.Drawing.SystemColors.Control;
			this.txtResponseEftLine2.Enabled = false;
			this.txtResponseEftLine2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtResponseEftLine2.Location = new System.Drawing.Point(28, 67);
			this.txtResponseEftLine2.Name = "txtResponseEftLine2";
			this.txtResponseEftLine2.Size = new System.Drawing.Size(469, 49);
			this.txtResponseEftLine2.TabIndex = 101;
			this.txtResponseEftLine2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtResponseEftLine2.TextChanged += new System.EventHandler(this.txtResponseEftLine2_TextChanged);
			// 
			// btnInt
			// 
			this.btnInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnInt.Location = new System.Drawing.Point(28, 196);
			this.btnInt.Name = "btnInt";
			this.btnInt.Size = new System.Drawing.Size(106, 58);
			this.btnInt.TabIndex = 2;
			this.btnInt.Text = "Int";
			this.btnInt.UseVisualStyleBackColor = true;
			this.btnInt.Visible = false;
			this.btnInt.Click += new System.EventHandler(this.btnInt_Click);
			// 
			// btnSettle
			// 
			this.btnSettle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSettle.Location = new System.Drawing.Point(377, 196);
			this.btnSettle.Name = "btnSettle";
			this.btnSettle.Size = new System.Drawing.Size(120, 58);
			this.btnSettle.TabIndex = 3;
			this.btnSettle.Text = "Settlement";
			this.btnSettle.UseVisualStyleBackColor = true;
			this.btnSettle.Visible = false;
			this.btnSettle.Click += new System.EventHandler(this.btnSettle_Click);
			// 
			// btnok
			// 
			this.btnok.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnok.Location = new System.Drawing.Point(205, 196);
			this.btnok.Name = "btnok";
			this.btnok.Size = new System.Drawing.Size(117, 58);
			this.btnok.TabIndex = 4;
			this.btnok.Text = "OK";
			this.btnok.UseVisualStyleBackColor = true;
			this.btnok.Click += new System.EventHandler(this.btnok_Click_1);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(205, 123);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(117, 67);
			this.btnCancel.TabIndex = 102;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Visible = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// timer2
			// 
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// timer3
			// 
			this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
			// 
			// EFTPOS
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(531, 266);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnok);
			this.Controls.Add(this.btnSettle);
			this.Controls.Add(this.btnInt);
			this.Controls.Add(this.txtResponseEftLine2);
			this.Controls.Add(this.txtResposneEftpos);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EFTPOS";
			this.Text = "eftpos - GPOS";
			this.Load += new System.EventHandler(this.eftpos_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EFTPOS_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.TextBox txtResposneEftpos;
        private System.Windows.Forms.TextBox txtResponseEftLine2;
        private System.Windows.Forms.Button btnInt;
        private System.Windows.Forms.Button btnSettle;
        private System.Windows.Forms.Button btnok;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer timer2;
		private System.Windows.Forms.Timer timer3;
    }
}