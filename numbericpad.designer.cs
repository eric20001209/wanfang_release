namespace QPOS2008
{
	partial class numbericpad
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnclose = new System.Windows.Forms.Button();
			this.btnent = new System.Windows.Forms.Button();
			this.btnclear = new System.Windows.Forms.Button();
			this.btndot = new System.Windows.Forms.Button();
			this.btn00 = new System.Windows.Forms.Button();
			this.btn0 = new System.Windows.Forms.Button();
			this.btndel = new System.Windows.Forms.Button();
			this.btn3 = new System.Windows.Forms.Button();
			this.btn6 = new System.Windows.Forms.Button();
			this.btn5 = new System.Windows.Forms.Button();
			this.btn4 = new System.Windows.Forms.Button();
			this.btn9 = new System.Windows.Forms.Button();
			this.btn8 = new System.Windows.Forms.Button();
			this.btn7 = new System.Windows.Forms.Button();
			this.btn2 = new System.Windows.Forms.Button();
			this.btn1 = new System.Windows.Forms.Button();
			this.txtdsp = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.DarkGray;
			this.panel1.Controls.Add(this.btnclose);
			this.panel1.Controls.Add(this.btnent);
			this.panel1.Controls.Add(this.btnclear);
			this.panel1.Controls.Add(this.btndot);
			this.panel1.Controls.Add(this.btn00);
			this.panel1.Controls.Add(this.btn0);
			this.panel1.Controls.Add(this.btndel);
			this.panel1.Controls.Add(this.btn3);
			this.panel1.Controls.Add(this.btn6);
			this.panel1.Controls.Add(this.btn5);
			this.panel1.Controls.Add(this.btn4);
			this.panel1.Controls.Add(this.btn9);
			this.panel1.Controls.Add(this.btn8);
			this.panel1.Controls.Add(this.btn7);
			this.panel1.Controls.Add(this.btn2);
			this.panel1.Controls.Add(this.btn1);
			this.panel1.Location = new System.Drawing.Point(7, 96);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(413, 339);
			this.panel1.TabIndex = 0;
			// 
			// btnclose
			// 
			this.btnclose.BackColor = System.Drawing.Color.Transparent;
			this.btnclose.BackgroundImage = global::QPOS2008.Properties.Resources.keyboardicon;
			this.btnclose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnclose.FlatAppearance.BorderSize = 0;
			this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnclose.Location = new System.Drawing.Point(223, 251);
			this.btnclose.Name = "btnclose";
			this.btnclose.Size = new System.Drawing.Size(104, 85);
			this.btnclose.TabIndex = 15;
			this.btnclose.UseVisualStyleBackColor = false;
			this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
			// 
			// btnent
			// 
			this.btnent.BackColor = System.Drawing.Color.Transparent;
			this.btnent.BackgroundImage = global::QPOS2008.Properties.Resources._return;
			this.btnent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnent.FlatAppearance.BorderSize = 0;
			this.btnent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnent.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnent.ForeColor = System.Drawing.Color.White;
			this.btnent.Location = new System.Drawing.Point(323, 156);
			this.btnent.Name = "btnent";
			this.btnent.Size = new System.Drawing.Size(90, 180);
			this.btnent.TabIndex = 14;
			this.btnent.Text = "Ent";
			this.btnent.UseVisualStyleBackColor = false;
			this.btnent.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btnclear
			// 
			this.btnclear.BackColor = System.Drawing.Color.Transparent;
			this.btnclear.BackgroundImage = global::QPOS2008.Properties.Resources.entbg;
			this.btnclear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnclear.FlatAppearance.BorderSize = 0;
			this.btnclear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnclear.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnclear.ForeColor = System.Drawing.Color.White;
			this.btnclear.Location = new System.Drawing.Point(3, 251);
			this.btnclear.Name = "btnclear";
			this.btnclear.Size = new System.Drawing.Size(212, 85);
			this.btnclear.TabIndex = 13;
			this.btnclear.Text = "Clear";
			this.btnclear.UseVisualStyleBackColor = false;
			this.btnclear.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btndot
			// 
			this.btndot.BackColor = System.Drawing.Color.Transparent;
			this.btndot.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btndot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btndot.FlatAppearance.BorderSize = 0;
			this.btndot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btndot.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btndot.Location = new System.Drawing.Point(221, 191);
			this.btndot.Name = "btndot";
			this.btndot.Size = new System.Drawing.Size(104, 58);
			this.btndot.TabIndex = 12;
			this.btndot.Text = ".";
			this.btndot.UseVisualStyleBackColor = false;
			this.btndot.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn00
			// 
			this.btn00.BackColor = System.Drawing.Color.Transparent;
			this.btn00.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn00.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn00.FlatAppearance.BorderSize = 0;
			this.btn00.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn00.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn00.Location = new System.Drawing.Point(113, 190);
			this.btn00.Name = "btn00";
			this.btn00.Size = new System.Drawing.Size(104, 58);
			this.btn00.TabIndex = 11;
			this.btn00.Text = "00";
			this.btn00.UseVisualStyleBackColor = false;
			this.btn00.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn0
			// 
			this.btn0.BackColor = System.Drawing.Color.Transparent;
			this.btn0.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn0.FlatAppearance.BorderSize = 0;
			this.btn0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn0.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn0.Location = new System.Drawing.Point(3, 190);
			this.btn0.Name = "btn0";
			this.btn0.Size = new System.Drawing.Size(104, 58);
			this.btn0.TabIndex = 10;
			this.btn0.Text = "0";
			this.btn0.UseVisualStyleBackColor = false;
			this.btn0.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btndel
			// 
			this.btndel.BackColor = System.Drawing.Color.Transparent;
			this.btndel.BackgroundImage = global::QPOS2008.Properties.Resources._return;
			this.btndel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btndel.FlatAppearance.BorderSize = 0;
			this.btndel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btndel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btndel.ForeColor = System.Drawing.Color.White;
			this.btndel.Location = new System.Drawing.Point(323, 6);
			this.btndel.Name = "btndel";
			this.btndel.Size = new System.Drawing.Size(90, 144);
			this.btndel.TabIndex = 9;
			this.btndel.Text = "Del";
			this.btndel.UseVisualStyleBackColor = false;
			this.btndel.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn3
			// 
			this.btn3.BackColor = System.Drawing.Color.Transparent;
			this.btn3.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn3.FlatAppearance.BorderSize = 0;
			this.btn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn3.Location = new System.Drawing.Point(221, 128);
			this.btn3.Name = "btn3";
			this.btn3.Size = new System.Drawing.Size(104, 58);
			this.btn3.TabIndex = 8;
			this.btn3.Text = "3";
			this.btn3.UseVisualStyleBackColor = false;
			this.btn3.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn6
			// 
			this.btn6.BackColor = System.Drawing.Color.Transparent;
			this.btn6.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn6.FlatAppearance.BorderSize = 0;
			this.btn6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn6.Location = new System.Drawing.Point(221, 65);
			this.btn6.Name = "btn6";
			this.btn6.Size = new System.Drawing.Size(104, 58);
			this.btn6.TabIndex = 7;
			this.btn6.Text = "6";
			this.btn6.UseVisualStyleBackColor = false;
			this.btn6.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn5
			// 
			this.btn5.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn5.FlatAppearance.BorderSize = 0;
			this.btn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn5.Location = new System.Drawing.Point(111, 65);
			this.btn5.Name = "btn5";
			this.btn5.Size = new System.Drawing.Size(104, 58);
			this.btn5.TabIndex = 6;
			this.btn5.Text = "5";
			this.btn5.UseVisualStyleBackColor = true;
			this.btn5.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn4
			// 
			this.btn4.BackColor = System.Drawing.Color.Transparent;
			this.btn4.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn4.FlatAppearance.BorderSize = 0;
			this.btn4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn4.Location = new System.Drawing.Point(3, 65);
			this.btn4.Name = "btn4";
			this.btn4.Size = new System.Drawing.Size(104, 58);
			this.btn4.TabIndex = 5;
			this.btn4.Text = "4";
			this.btn4.UseVisualStyleBackColor = false;
			this.btn4.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn9
			// 
			this.btn9.BackColor = System.Drawing.Color.Transparent;
			this.btn9.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn9.FlatAppearance.BorderSize = 0;
			this.btn9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn9.Location = new System.Drawing.Point(221, 3);
			this.btn9.Name = "btn9";
			this.btn9.Size = new System.Drawing.Size(104, 58);
			this.btn9.TabIndex = 4;
			this.btn9.Text = "9";
			this.btn9.UseVisualStyleBackColor = false;
			this.btn9.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn8
			// 
			this.btn8.BackColor = System.Drawing.Color.Transparent;
			this.btn8.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn8.FlatAppearance.BorderSize = 0;
			this.btn8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn8.Location = new System.Drawing.Point(111, 3);
			this.btn8.Name = "btn8";
			this.btn8.Size = new System.Drawing.Size(104, 58);
			this.btn8.TabIndex = 3;
			this.btn8.Text = "8";
			this.btn8.UseVisualStyleBackColor = false;
			this.btn8.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn7
			// 
			this.btn7.BackColor = System.Drawing.Color.Transparent;
			this.btn7.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn7.FlatAppearance.BorderSize = 0;
			this.btn7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn7.Location = new System.Drawing.Point(3, 3);
			this.btn7.Name = "btn7";
			this.btn7.Size = new System.Drawing.Size(104, 58);
			this.btn7.TabIndex = 2;
			this.btn7.Text = "7";
			this.btn7.UseVisualStyleBackColor = false;
			this.btn7.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn2
			// 
			this.btn2.BackColor = System.Drawing.Color.Transparent;
			this.btn2.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn2.FlatAppearance.BorderSize = 0;
			this.btn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn2.Location = new System.Drawing.Point(111, 128);
			this.btn2.Name = "btn2";
			this.btn2.Size = new System.Drawing.Size(104, 58);
			this.btn2.TabIndex = 1;
			this.btn2.Text = "2";
			this.btn2.UseVisualStyleBackColor = false;
			this.btn2.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// btn1
			// 
			this.btn1.BackColor = System.Drawing.Color.Transparent;
			this.btn1.BackgroundImage = global::QPOS2008.Properties.Resources.keybg1;
			this.btn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btn1.FlatAppearance.BorderSize = 0;
			this.btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn1.Location = new System.Drawing.Point(3, 128);
			this.btn1.Name = "btn1";
			this.btn1.Size = new System.Drawing.Size(104, 58);
			this.btn1.TabIndex = 0;
			this.btn1.Text = "1";
			this.btn1.UseVisualStyleBackColor = false;
			this.btn1.Click += new System.EventHandler(this.btnPinpad_Click);
			// 
			// txtdsp
			// 
			this.txtdsp.BackColor = System.Drawing.SystemColors.Window;
			this.txtdsp.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtdsp.Location = new System.Drawing.Point(7, 47);
			this.txtdsp.Name = "txtdsp";
			this.txtdsp.Size = new System.Drawing.Size(405, 49);
			this.txtdsp.TabIndex = 1;
			this.txtdsp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtdsp.UseSystemPasswordChar = true;
			this.txtdsp.Click += new System.EventHandler(this.txtdsp_Click);
			this.txtdsp.TextChanged += new System.EventHandler(this.txtdsp_TextChanged);
			this.txtdsp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtdsp_KeyDown);
			this.txtdsp.Leave += new System.EventHandler(this.onControlLeave);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(4, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 31);
			this.label1.TabIndex = 2;
			this.label1.Text = "    ";
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.BackColor = System.Drawing.Color.DarkGray;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(12, 9);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(85, 29);
			this.lblTitle.TabIndex = 3;
			this.lblTitle.Text = "label1";
			// 
			// numbericpad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DarkGray;
			this.ClientSize = new System.Drawing.Size(424, 440);
			this.ControlBox = false;
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtdsp);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "numbericpad";
			this.Text = "numbericpad";
			this.Load += new System.EventHandler(this.numbericpad_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numbericpad_KeyDown);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btn1;
		private System.Windows.Forms.Button btn8;
		private System.Windows.Forms.Button btn7;
		private System.Windows.Forms.Button btn2;
		private System.Windows.Forms.Button btndel;
		private System.Windows.Forms.Button btn3;
		private System.Windows.Forms.Button btn6;
		private System.Windows.Forms.Button btn5;
		private System.Windows.Forms.Button btn4;
		private System.Windows.Forms.Button btn9;
		private System.Windows.Forms.Button btndot;
		private System.Windows.Forms.Button btn00;
		private System.Windows.Forms.Button btn0;
		private System.Windows.Forms.Button btnent;
		private System.Windows.Forms.Button btnclear;
		private System.Windows.Forms.TextBox txtdsp;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblTitle;
	}
}