namespace QPOS2008
{
    partial class FormMSG
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
			this.btnOK = new System.Windows.Forms.Button();
			this.txt = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.btnYes = new System.Windows.Forms.Button();
			this.btnNo = new System.Windows.Forms.Button();
			this.txtmsg = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.Color.PeachPuff;
			this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.Location = new System.Drawing.Point(333, 242);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(182, 73);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new System.EventHandler(this.button1_Click);
			// 
			// txt
			// 
			this.txt.BackColor = System.Drawing.Color.Red;
			this.txt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txt.Location = new System.Drawing.Point(184, 284);
			this.txt.Name = "txt";
			this.txt.Size = new System.Drawing.Size(29, 13);
			this.txt.TabIndex = 2;
			this.txt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Transparent;
			this.button2.FlatAppearance.BorderSize = 0;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(732, 297);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(60, 55);
			this.button2.TabIndex = 3;
			this.button2.Text = "conf";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Visible = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// btnYes
			// 
			this.btnYes.BackColor = System.Drawing.Color.PeachPuff;
			this.btnYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
			this.btnYes.Location = new System.Drawing.Point(145, 242);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(182, 73);
			this.btnYes.TabIndex = 4;
			this.btnYes.Text = "YES";
			this.btnYes.UseVisualStyleBackColor = false;
			this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
			// 
			// btnNo
			// 
			this.btnNo.BackColor = System.Drawing.Color.PeachPuff;
			this.btnNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
			this.btnNo.Location = new System.Drawing.Point(521, 242);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(182, 73);
			this.btnNo.TabIndex = 5;
			this.btnNo.Text = "NO";
			this.btnNo.UseVisualStyleBackColor = false;
			this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// 
			// txtmsg
			// 
			this.txtmsg.BackColor = System.Drawing.Color.Red;
			this.txtmsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtmsg.ForeColor = System.Drawing.Color.White;
			this.txtmsg.Location = new System.Drawing.Point(43, 32);
			this.txtmsg.Multiline = true;
			this.txtmsg.Name = "txtmsg";
			this.txtmsg.Size = new System.Drawing.Size(788, 169);
			this.txtmsg.TabIndex = 6;
			this.txtmsg.Text = "txtmsg";
			// 
			// FormMSG
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Red;
			this.ClientSize = new System.Drawing.Size(893, 352);
			this.Controls.Add(this.txtmsg);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.btnYes);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.txt);
			this.Controls.Add(this.btnOK);
			this.Name = "FormMSG";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormMSG";
			this.Load += new System.EventHandler(this.FormMSG_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.TextBox txt;
		private System.Windows.Forms.Button button2;
		public System.Windows.Forms.Button btnYes;
		public System.Windows.Forms.Button btnNo;
		private System.Windows.Forms.TextBox txtmsg;
		public System.Windows.Forms.Button btnOK;
    }
}