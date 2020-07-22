namespace QPOS2008
{
	partial class FormPad
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPad));
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
			this.txtQty = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
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
			this.panel1.Location = new System.Drawing.Point(34, 118);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(414, 344);
			this.panel1.TabIndex = 12;
			// 
			// btnclose
			// 
			this.btnclose.BackColor = System.Drawing.Color.Transparent;
			this.btnclose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnclose.BackgroundImage")));
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
			this.btnent.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnent.BackgroundImage")));
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
			this.btnclear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnclear.BackgroundImage")));
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
			this.btndot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btndot.BackgroundImage")));
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
			this.btn00.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn00.BackgroundImage")));
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
			this.btn0.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn0.BackgroundImage")));
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
			this.btndel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btndel.BackgroundImage")));
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
			this.btn3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn3.BackgroundImage")));
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
			this.btn6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn6.BackgroundImage")));
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
			this.btn5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn5.BackgroundImage")));
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
			this.btn4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn4.BackgroundImage")));
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
			this.btn9.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn9.BackgroundImage")));
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
			this.btn8.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn8.BackgroundImage")));
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
			this.btn7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn7.BackgroundImage")));
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
			this.btn2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn2.BackgroundImage")));
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
			this.btn1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn1.BackgroundImage")));
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
			// txtQty
			// 
			this.txtQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.txtQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQty.Location = new System.Drawing.Point(35, 30);
			this.txtQty.Multiline = true;
			this.txtQty.Name = "txtQty";
			this.txtQty.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.txtQty.Size = new System.Drawing.Size(414, 82);
			this.txtQty.TabIndex = 11;
			this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtQty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQty_KeyDown);
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Transparent;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(185, 153);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(216, 66);
			this.button1.TabIndex = 10;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = false;
			// 
			// FormPad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(482, 493);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.txtQty);
			this.Controls.Add(this.button1);
			this.Name = "FormPad";
			this.Text = "FormPad";
			this.Load += new System.EventHandler(this.FormPad_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnclose;
		private System.Windows.Forms.Button btnent;
		private System.Windows.Forms.Button btnclear;
		private System.Windows.Forms.Button btndot;
		private System.Windows.Forms.Button btn00;
		private System.Windows.Forms.Button btn0;
		private System.Windows.Forms.Button btndel;
		private System.Windows.Forms.Button btn3;
		private System.Windows.Forms.Button btn6;
		private System.Windows.Forms.Button btn5;
		private System.Windows.Forms.Button btn4;
		private System.Windows.Forms.Button btn9;
		private System.Windows.Forms.Button btn8;
		private System.Windows.Forms.Button btn7;
		private System.Windows.Forms.Button btn2;
		private System.Windows.Forms.Button btn1;
		private System.Windows.Forms.TextBox txtQty;
		private System.Windows.Forms.Button button1;
	}
}