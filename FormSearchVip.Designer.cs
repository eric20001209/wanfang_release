namespace QPOS2008
{
    partial class FormSearchVip
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
			this.kw = new System.Windows.Forms.TextBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.cc_barcode = new System.Windows.Forms.ColumnHeader();
			this.cc_name = new System.Windows.Forms.ColumnHeader();
			this.cc_phone = new System.Windows.Forms.ColumnHeader();
			this.cc_mobile = new System.Windows.Forms.ColumnHeader();
			this.cc_address = new System.Windows.Forms.ColumnHeader();
			this.cc_edit = new System.Windows.Forms.ColumnHeader();
			this.cc_id = new System.Windows.Forms.ColumnHeader();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnKeyBoard = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnBackward = new System.Windows.Forms.Button();
			this.btnForward = new System.Windows.Forms.Button();
			this.btnPercent = new System.Windows.Forms.Button();
			this.btnAt = new System.Windows.Forms.Button();
			this.btnDash = new System.Windows.Forms.Button();
			this.btnHash = new System.Windows.Forms.Button();
			this.btnStar = new System.Windows.Forms.Button();
			this.btnDEL = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnEnter = new System.Windows.Forms.Button();
			this.btnSpace = new System.Windows.Forms.Button();
			this.btnM = new System.Windows.Forms.Button();
			this.btnN = new System.Windows.Forms.Button();
			this.btnB = new System.Windows.Forms.Button();
			this.btnV = new System.Windows.Forms.Button();
			this.btnC = new System.Windows.Forms.Button();
			this.btnX = new System.Windows.Forms.Button();
			this.btnL = new System.Windows.Forms.Button();
			this.btnK = new System.Windows.Forms.Button();
			this.btnJ = new System.Windows.Forms.Button();
			this.btnH = new System.Windows.Forms.Button();
			this.btnP = new System.Windows.Forms.Button();
			this.btnO = new System.Windows.Forms.Button();
			this.btnI = new System.Windows.Forms.Button();
			this.btnU = new System.Windows.Forms.Button();
			this.btnY = new System.Windows.Forms.Button();
			this.btnG = new System.Windows.Forms.Button();
			this.btnT = new System.Windows.Forms.Button();
			this.btnF = new System.Windows.Forms.Button();
			this.btnR = new System.Windows.Forms.Button();
			this.btnD = new System.Windows.Forms.Button();
			this.btnE = new System.Windows.Forms.Button();
			this.btnS = new System.Windows.Forms.Button();
			this.btnZ = new System.Windows.Forms.Button();
			this.btnA = new System.Windows.Forms.Button();
			this.btnW = new System.Windows.Forms.Button();
			this.btnQ = new System.Windows.Forms.Button();
			this.btnAddNew = new System.Windows.Forms.Button();
			this.cc_balance = new System.Windows.Forms.ColumnHeader();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// kw
			// 
			this.kw.Font = new System.Drawing.Font("Verdana", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.kw.Location = new System.Drawing.Point(793, 11);
			this.kw.Name = "kw";
			this.kw.Size = new System.Drawing.Size(219, 50);
			this.kw.TabIndex = 0;
			this.kw.Click += new System.EventHandler(this.kw_Click);
			this.kw.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKWKeyDown);
			// 
			// listView1
			// 
			this.listView1.AutoArrange = false;
			this.listView1.BackColor = System.Drawing.Color.Lavender;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cc_barcode,
            this.cc_name,
            this.cc_phone,
            this.cc_mobile,
            this.cc_address,
            this.cc_balance,
            this.cc_edit,
            this.cc_id});
			this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(12, 12);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(775, 396);
			this.listView1.TabIndex = 2;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
			this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
			this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnListKeyDown);
			this.listView1.Click += new System.EventHandler(this.listView1_Click);
			// 
			// cc_barcode
			// 
			this.cc_barcode.Text = "Barcode";
			this.cc_barcode.Width = 91;
			// 
			// cc_name
			// 
			this.cc_name.Text = "Name";
			this.cc_name.Width = 126;
			// 
			// cc_phone
			// 
			this.cc_phone.Text = "Phone";
			this.cc_phone.Width = 100;
			// 
			// cc_mobile
			// 
			this.cc_mobile.Text = "Mobile";
			this.cc_mobile.Width = 100;
			// 
			// cc_address
			// 
			this.cc_address.Text = "Address";
			this.cc_address.Width = 200;
			// 
			// cc_edit
			// 
			this.cc_edit.Text = "Edit";
			// 
			// cc_id
			// 
			this.cc_id.Text = "";
			this.cc_id.Width = 90;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Location = new System.Drawing.Point(800, 141);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(212, 267);
			this.flowLayoutPanel1.TabIndex = 4;
			// 
			// btnKeyBoard
			// 
			this.btnKeyBoard.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnKeyBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnKeyBoard.FlatAppearance.BorderSize = 0;
			this.btnKeyBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKeyBoard.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnKeyBoard.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnKeyBoard.Location = new System.Drawing.Point(865, 104);
			this.btnKeyBoard.Name = "btnKeyBoard";
			this.btnKeyBoard.Size = new System.Drawing.Size(148, 31);
			this.btnKeyBoard.TabIndex = 6;
			this.btnKeyBoard.Text = "Keyboard";
			this.btnKeyBoard.UseVisualStyleBackColor = true;
			this.btnKeyBoard.Click += new System.EventHandler(this.btnKeyBoard_Click);
			// 
			// btnStart
			// 
			this.btnStart.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnStart.FlatAppearance.BorderSize = 0;
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStart.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStart.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnStart.Location = new System.Drawing.Point(793, 67);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(66, 31);
			this.btnStart.TabIndex = 5;
			this.btnStart.Text = "Search";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.buttonCancel.FlatAppearance.BorderSize = 0;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonCancel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonCancel.ForeColor = System.Drawing.Color.MidnightBlue;
			this.buttonCancel.Location = new System.Drawing.Point(793, 104);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(66, 31);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Close";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnBackward);
			this.panel1.Controls.Add(this.btnForward);
			this.panel1.Controls.Add(this.btnPercent);
			this.panel1.Controls.Add(this.btnAt);
			this.panel1.Controls.Add(this.btnDash);
			this.panel1.Controls.Add(this.btnHash);
			this.panel1.Controls.Add(this.btnStar);
			this.panel1.Controls.Add(this.btnDEL);
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.btnEnter);
			this.panel1.Controls.Add(this.btnSpace);
			this.panel1.Controls.Add(this.btnM);
			this.panel1.Controls.Add(this.btnN);
			this.panel1.Controls.Add(this.btnB);
			this.panel1.Controls.Add(this.btnV);
			this.panel1.Controls.Add(this.btnC);
			this.panel1.Controls.Add(this.btnX);
			this.panel1.Controls.Add(this.btnL);
			this.panel1.Controls.Add(this.btnK);
			this.panel1.Controls.Add(this.btnJ);
			this.panel1.Controls.Add(this.btnH);
			this.panel1.Controls.Add(this.btnP);
			this.panel1.Controls.Add(this.btnO);
			this.panel1.Controls.Add(this.btnI);
			this.panel1.Controls.Add(this.btnU);
			this.panel1.Controls.Add(this.btnY);
			this.panel1.Controls.Add(this.btnG);
			this.panel1.Controls.Add(this.btnT);
			this.panel1.Controls.Add(this.btnF);
			this.panel1.Controls.Add(this.btnR);
			this.panel1.Controls.Add(this.btnD);
			this.panel1.Controls.Add(this.btnE);
			this.panel1.Controls.Add(this.btnS);
			this.panel1.Controls.Add(this.btnZ);
			this.panel1.Controls.Add(this.btnA);
			this.panel1.Controls.Add(this.btnW);
			this.panel1.Controls.Add(this.btnQ);
			this.panel1.Location = new System.Drawing.Point(2, 142);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(20, 22);
			this.panel1.TabIndex = 7;
			this.panel1.TabStop = true;
			this.panel1.Visible = false;
			// 
			// btnBackward
			// 
			this.btnBackward.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnBackward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnBackward.FlatAppearance.BorderSize = 0;
			this.btnBackward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBackward.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBackward.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnBackward.Location = new System.Drawing.Point(624, 145);
			this.btnBackward.Name = "btnBackward";
			this.btnBackward.Size = new System.Drawing.Size(56, 61);
			this.btnBackward.TabIndex = 36;
			this.btnBackward.Text = "\\";
			this.btnBackward.UseVisualStyleBackColor = true;
			this.btnBackward.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnForward
			// 
			this.btnForward.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnForward.FlatAppearance.BorderSize = 0;
			this.btnForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnForward.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnForward.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnForward.Location = new System.Drawing.Point(624, 78);
			this.btnForward.Name = "btnForward";
			this.btnForward.Size = new System.Drawing.Size(56, 61);
			this.btnForward.TabIndex = 35;
			this.btnForward.Text = "/";
			this.btnForward.UseVisualStyleBackColor = true;
			this.btnForward.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnPercent
			// 
			this.btnPercent.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnPercent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnPercent.FlatAppearance.BorderSize = 0;
			this.btnPercent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPercent.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPercent.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnPercent.Location = new System.Drawing.Point(624, 9);
			this.btnPercent.Name = "btnPercent";
			this.btnPercent.Size = new System.Drawing.Size(56, 61);
			this.btnPercent.TabIndex = 34;
			this.btnPercent.Text = "%";
			this.btnPercent.UseVisualStyleBackColor = true;
			this.btnPercent.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnAt
			// 
			this.btnAt.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnAt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnAt.FlatAppearance.BorderSize = 0;
			this.btnAt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAt.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAt.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnAt.Location = new System.Drawing.Point(67, 143);
			this.btnAt.Name = "btnAt";
			this.btnAt.Size = new System.Drawing.Size(56, 61);
			this.btnAt.TabIndex = 33;
			this.btnAt.Text = "@";
			this.btnAt.UseVisualStyleBackColor = true;
			this.btnAt.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnDash
			// 
			this.btnDash.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnDash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnDash.FlatAppearance.BorderSize = 0;
			this.btnDash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDash.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDash.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnDash.Location = new System.Drawing.Point(5, 143);
			this.btnDash.Name = "btnDash";
			this.btnDash.Size = new System.Drawing.Size(56, 61);
			this.btnDash.TabIndex = 32;
			this.btnDash.Text = "~";
			this.btnDash.UseVisualStyleBackColor = true;
			this.btnDash.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnHash
			// 
			this.btnHash.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnHash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnHash.FlatAppearance.BorderSize = 0;
			this.btnHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnHash.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnHash.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnHash.Location = new System.Drawing.Point(562, 143);
			this.btnHash.Name = "btnHash";
			this.btnHash.Size = new System.Drawing.Size(56, 61);
			this.btnHash.TabIndex = 31;
			this.btnHash.Text = "#";
			this.btnHash.UseVisualStyleBackColor = true;
			this.btnHash.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnStar
			// 
			this.btnStar.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnStar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnStar.FlatAppearance.BorderSize = 0;
			this.btnStar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStar.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStar.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnStar.Location = new System.Drawing.Point(5, 76);
			this.btnStar.Name = "btnStar";
			this.btnStar.Size = new System.Drawing.Size(56, 61);
			this.btnStar.TabIndex = 30;
			this.btnStar.Text = "*";
			this.btnStar.UseVisualStyleBackColor = true;
			this.btnStar.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnDEL
			// 
			this.btnDEL.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnDEL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnDEL.FlatAppearance.BorderSize = 0;
			this.btnDEL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDEL.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDEL.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnDEL.Location = new System.Drawing.Point(688, 9);
			this.btnDEL.Name = "btnDEL";
			this.btnDEL.Size = new System.Drawing.Size(97, 61);
			this.btnDEL.TabIndex = 29;
			this.btnDEL.Text = "DEL";
			this.btnDEL.UseVisualStyleBackColor = true;
			this.btnDEL.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnClose.FlatAppearance.BorderSize = 0;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClose.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnClose.Location = new System.Drawing.Point(688, 143);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(97, 61);
			this.btnClose.TabIndex = 28;
			this.btnClose.Text = "CLOSE";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnEnter
			// 
			this.btnEnter.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnEnter.FlatAppearance.BorderSize = 0;
			this.btnEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEnter.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnEnter.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnEnter.Location = new System.Drawing.Point(688, 76);
			this.btnEnter.Name = "btnEnter";
			this.btnEnter.Size = new System.Drawing.Size(97, 61);
			this.btnEnter.TabIndex = 27;
			this.btnEnter.Text = "ENTER";
			this.btnEnter.UseVisualStyleBackColor = true;
			this.btnEnter.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnSpace
			// 
			this.btnSpace.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnSpace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnSpace.FlatAppearance.BorderSize = 0;
			this.btnSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSpace.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSpace.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnSpace.Location = new System.Drawing.Point(236, 210);
			this.btnSpace.Name = "btnSpace";
			this.btnSpace.Size = new System.Drawing.Size(155, 57);
			this.btnSpace.TabIndex = 26;
			this.btnSpace.Text = "SPACE";
			this.btnSpace.UseVisualStyleBackColor = true;
			this.btnSpace.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnM
			// 
			this.btnM.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnM.FlatAppearance.BorderSize = 0;
			this.btnM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnM.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnM.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnM.Location = new System.Drawing.Point(501, 143);
			this.btnM.Name = "btnM";
			this.btnM.Size = new System.Drawing.Size(56, 61);
			this.btnM.TabIndex = 25;
			this.btnM.Text = "M";
			this.btnM.UseVisualStyleBackColor = true;
			this.btnM.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnN
			// 
			this.btnN.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnN.FlatAppearance.BorderSize = 0;
			this.btnN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnN.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnN.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnN.Location = new System.Drawing.Point(439, 143);
			this.btnN.Name = "btnN";
			this.btnN.Size = new System.Drawing.Size(56, 61);
			this.btnN.TabIndex = 24;
			this.btnN.Text = "N";
			this.btnN.UseVisualStyleBackColor = true;
			this.btnN.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnB
			// 
			this.btnB.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnB.FlatAppearance.BorderSize = 0;
			this.btnB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnB.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnB.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnB.Location = new System.Drawing.Point(377, 143);
			this.btnB.Name = "btnB";
			this.btnB.Size = new System.Drawing.Size(56, 61);
			this.btnB.TabIndex = 23;
			this.btnB.Text = "B";
			this.btnB.UseVisualStyleBackColor = true;
			this.btnB.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnV
			// 
			this.btnV.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnV.FlatAppearance.BorderSize = 0;
			this.btnV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnV.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnV.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnV.Location = new System.Drawing.Point(315, 143);
			this.btnV.Name = "btnV";
			this.btnV.Size = new System.Drawing.Size(56, 61);
			this.btnV.TabIndex = 22;
			this.btnV.Text = "V";
			this.btnV.UseVisualStyleBackColor = true;
			this.btnV.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnC
			// 
			this.btnC.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnC.FlatAppearance.BorderSize = 0;
			this.btnC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnC.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnC.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnC.Location = new System.Drawing.Point(253, 143);
			this.btnC.Name = "btnC";
			this.btnC.Size = new System.Drawing.Size(56, 61);
			this.btnC.TabIndex = 21;
			this.btnC.Text = "C";
			this.btnC.UseVisualStyleBackColor = true;
			this.btnC.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnX
			// 
			this.btnX.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnX.FlatAppearance.BorderSize = 0;
			this.btnX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnX.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnX.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnX.Location = new System.Drawing.Point(191, 143);
			this.btnX.Name = "btnX";
			this.btnX.Size = new System.Drawing.Size(56, 61);
			this.btnX.TabIndex = 20;
			this.btnX.Text = "X";
			this.btnX.UseVisualStyleBackColor = true;
			this.btnX.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnL
			// 
			this.btnL.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnL.FlatAppearance.BorderSize = 0;
			this.btnL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnL.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnL.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnL.Location = new System.Drawing.Point(562, 76);
			this.btnL.Name = "btnL";
			this.btnL.Size = new System.Drawing.Size(56, 61);
			this.btnL.TabIndex = 19;
			this.btnL.Text = "L";
			this.btnL.UseVisualStyleBackColor = true;
			this.btnL.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnK
			// 
			this.btnK.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnK.FlatAppearance.BorderSize = 0;
			this.btnK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnK.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnK.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnK.Location = new System.Drawing.Point(500, 76);
			this.btnK.Name = "btnK";
			this.btnK.Size = new System.Drawing.Size(56, 61);
			this.btnK.TabIndex = 18;
			this.btnK.Text = "K";
			this.btnK.UseVisualStyleBackColor = true;
			this.btnK.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnJ
			// 
			this.btnJ.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnJ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnJ.FlatAppearance.BorderSize = 0;
			this.btnJ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnJ.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnJ.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnJ.Location = new System.Drawing.Point(438, 76);
			this.btnJ.Name = "btnJ";
			this.btnJ.Size = new System.Drawing.Size(56, 61);
			this.btnJ.TabIndex = 17;
			this.btnJ.Text = "J";
			this.btnJ.UseVisualStyleBackColor = true;
			this.btnJ.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnH
			// 
			this.btnH.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnH.FlatAppearance.BorderSize = 0;
			this.btnH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnH.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnH.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnH.Location = new System.Drawing.Point(376, 76);
			this.btnH.Name = "btnH";
			this.btnH.Size = new System.Drawing.Size(56, 61);
			this.btnH.TabIndex = 16;
			this.btnH.Text = "H";
			this.btnH.UseVisualStyleBackColor = true;
			this.btnH.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnP
			// 
			this.btnP.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnP.FlatAppearance.BorderSize = 0;
			this.btnP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnP.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnP.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnP.Location = new System.Drawing.Point(562, 9);
			this.btnP.Name = "btnP";
			this.btnP.Size = new System.Drawing.Size(56, 61);
			this.btnP.TabIndex = 15;
			this.btnP.Text = "P";
			this.btnP.UseVisualStyleBackColor = true;
			this.btnP.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnO
			// 
			this.btnO.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnO.FlatAppearance.BorderSize = 0;
			this.btnO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnO.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnO.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnO.Location = new System.Drawing.Point(500, 9);
			this.btnO.Name = "btnO";
			this.btnO.Size = new System.Drawing.Size(56, 61);
			this.btnO.TabIndex = 14;
			this.btnO.Text = "O";
			this.btnO.UseVisualStyleBackColor = true;
			this.btnO.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnI
			// 
			this.btnI.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnI.FlatAppearance.BorderSize = 0;
			this.btnI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnI.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnI.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnI.Location = new System.Drawing.Point(438, 9);
			this.btnI.Name = "btnI";
			this.btnI.Size = new System.Drawing.Size(56, 61);
			this.btnI.TabIndex = 13;
			this.btnI.Text = "I";
			this.btnI.UseVisualStyleBackColor = true;
			this.btnI.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnU
			// 
			this.btnU.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnU.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnU.FlatAppearance.BorderSize = 0;
			this.btnU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnU.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnU.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnU.Location = new System.Drawing.Point(376, 9);
			this.btnU.Name = "btnU";
			this.btnU.Size = new System.Drawing.Size(56, 61);
			this.btnU.TabIndex = 12;
			this.btnU.Text = "U";
			this.btnU.UseVisualStyleBackColor = true;
			this.btnU.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnY
			// 
			this.btnY.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnY.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnY.FlatAppearance.BorderSize = 0;
			this.btnY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnY.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnY.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnY.Location = new System.Drawing.Point(314, 9);
			this.btnY.Name = "btnY";
			this.btnY.Size = new System.Drawing.Size(56, 61);
			this.btnY.TabIndex = 11;
			this.btnY.Text = "Y";
			this.btnY.UseVisualStyleBackColor = true;
			this.btnY.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnG
			// 
			this.btnG.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnG.FlatAppearance.BorderSize = 0;
			this.btnG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnG.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnG.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnG.Location = new System.Drawing.Point(314, 76);
			this.btnG.Name = "btnG";
			this.btnG.Size = new System.Drawing.Size(56, 61);
			this.btnG.TabIndex = 10;
			this.btnG.Text = "G";
			this.btnG.UseVisualStyleBackColor = true;
			this.btnG.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnT
			// 
			this.btnT.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnT.FlatAppearance.BorderSize = 0;
			this.btnT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnT.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnT.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnT.Location = new System.Drawing.Point(252, 9);
			this.btnT.Name = "btnT";
			this.btnT.Size = new System.Drawing.Size(56, 61);
			this.btnT.TabIndex = 9;
			this.btnT.Text = "T";
			this.btnT.UseVisualStyleBackColor = true;
			this.btnT.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnF
			// 
			this.btnF.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnF.FlatAppearance.BorderSize = 0;
			this.btnF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnF.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnF.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnF.Location = new System.Drawing.Point(252, 76);
			this.btnF.Name = "btnF";
			this.btnF.Size = new System.Drawing.Size(56, 61);
			this.btnF.TabIndex = 8;
			this.btnF.Text = "F";
			this.btnF.UseVisualStyleBackColor = true;
			this.btnF.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnR
			// 
			this.btnR.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnR.FlatAppearance.BorderSize = 0;
			this.btnR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnR.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnR.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnR.Location = new System.Drawing.Point(190, 9);
			this.btnR.Name = "btnR";
			this.btnR.Size = new System.Drawing.Size(56, 61);
			this.btnR.TabIndex = 7;
			this.btnR.Text = "R";
			this.btnR.UseVisualStyleBackColor = true;
			this.btnR.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnD
			// 
			this.btnD.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnD.FlatAppearance.BorderSize = 0;
			this.btnD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnD.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnD.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnD.Location = new System.Drawing.Point(190, 76);
			this.btnD.Name = "btnD";
			this.btnD.Size = new System.Drawing.Size(56, 61);
			this.btnD.TabIndex = 6;
			this.btnD.Text = "D";
			this.btnD.UseVisualStyleBackColor = true;
			this.btnD.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnE
			// 
			this.btnE.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnE.FlatAppearance.BorderSize = 0;
			this.btnE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnE.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnE.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnE.Location = new System.Drawing.Point(128, 9);
			this.btnE.Name = "btnE";
			this.btnE.Size = new System.Drawing.Size(56, 61);
			this.btnE.TabIndex = 5;
			this.btnE.Text = "E";
			this.btnE.UseVisualStyleBackColor = true;
			this.btnE.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnS
			// 
			this.btnS.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnS.FlatAppearance.BorderSize = 0;
			this.btnS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnS.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnS.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnS.Location = new System.Drawing.Point(128, 76);
			this.btnS.Name = "btnS";
			this.btnS.Size = new System.Drawing.Size(56, 61);
			this.btnS.TabIndex = 4;
			this.btnS.Text = "S";
			this.btnS.UseVisualStyleBackColor = true;
			this.btnS.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnZ
			// 
			this.btnZ.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnZ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnZ.FlatAppearance.BorderSize = 0;
			this.btnZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnZ.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnZ.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnZ.Location = new System.Drawing.Point(129, 143);
			this.btnZ.Name = "btnZ";
			this.btnZ.Size = new System.Drawing.Size(56, 61);
			this.btnZ.TabIndex = 3;
			this.btnZ.Text = "Z";
			this.btnZ.UseVisualStyleBackColor = true;
			this.btnZ.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnA
			// 
			this.btnA.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnA.FlatAppearance.BorderSize = 0;
			this.btnA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnA.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnA.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnA.Location = new System.Drawing.Point(66, 76);
			this.btnA.Name = "btnA";
			this.btnA.Size = new System.Drawing.Size(56, 61);
			this.btnA.TabIndex = 2;
			this.btnA.Text = "A";
			this.btnA.UseVisualStyleBackColor = true;
			this.btnA.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnW
			// 
			this.btnW.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnW.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnW.FlatAppearance.BorderSize = 0;
			this.btnW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnW.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnW.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnW.Location = new System.Drawing.Point(67, 9);
			this.btnW.Name = "btnW";
			this.btnW.Size = new System.Drawing.Size(56, 61);
			this.btnW.TabIndex = 1;
			this.btnW.Text = "W";
			this.btnW.UseVisualStyleBackColor = true;
			this.btnW.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnQ
			// 
			this.btnQ.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnQ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnQ.FlatAppearance.BorderSize = 0;
			this.btnQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnQ.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnQ.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnQ.Location = new System.Drawing.Point(5, 9);
			this.btnQ.Name = "btnQ";
			this.btnQ.Size = new System.Drawing.Size(56, 61);
			this.btnQ.TabIndex = 0;
			this.btnQ.Text = "Q";
			this.btnQ.UseVisualStyleBackColor = true;
			this.btnQ.Click += new System.EventHandler(this.btnKey_Click);
			// 
			// btnAddNew
			// 
			this.btnAddNew.BackgroundImage = global::QPOS2008.Properties.Resources.blue;
			this.btnAddNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnAddNew.FlatAppearance.BorderSize = 0;
			this.btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddNew.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAddNew.ForeColor = System.Drawing.Color.MidnightBlue;
			this.btnAddNew.Location = new System.Drawing.Point(865, 67);
			this.btnAddNew.Name = "btnAddNew";
			this.btnAddNew.Size = new System.Drawing.Size(147, 31);
			this.btnAddNew.TabIndex = 8;
			this.btnAddNew.Text = "Add New";
			this.btnAddNew.UseVisualStyleBackColor = true;
			this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
			// 
			// cc_balance
			// 
			this.cc_balance.Text = "Balance";
			this.cc_balance.Width = 100;
			// 
			// FormSearchVip
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(1024, 421);
			this.Controls.Add(this.btnAddNew);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnKeyBoard);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.kw);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormSearchVip";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Search Item";
			this.Load += new System.EventHandler(this.FormSearch_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormSearch_KeyDown);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox kw;
        private System.Windows.Forms.ListView listView1;
        //private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader cc_barcode;
        private System.Windows.Forms.ColumnHeader cc_mobile;
        private System.Windows.Forms.ColumnHeader cc_name;
        //private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader cc_phone;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnKeyBoard;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnW;
        private System.Windows.Forms.Button btnQ;
        private System.Windows.Forms.Button btnA;
        private System.Windows.Forms.Button btnS;
        private System.Windows.Forms.Button btnZ;
        private System.Windows.Forms.Button btnE;
        private System.Windows.Forms.Button btnR;
        private System.Windows.Forms.Button btnD;
        private System.Windows.Forms.Button btnY;
        private System.Windows.Forms.Button btnG;
        private System.Windows.Forms.Button btnT;
        private System.Windows.Forms.Button btnF;
        private System.Windows.Forms.Button btnO;
        private System.Windows.Forms.Button btnI;
        private System.Windows.Forms.Button btnU;
        private System.Windows.Forms.Button btnK;
        private System.Windows.Forms.Button btnJ;
        private System.Windows.Forms.Button btnH;
        private System.Windows.Forms.Button btnP;
        private System.Windows.Forms.Button btnC;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.Button btnL;
        private System.Windows.Forms.Button btnB;
        private System.Windows.Forms.Button btnV;
        private System.Windows.Forms.Button btnN;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnSpace;
        private System.Windows.Forms.Button btnM;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDEL;
        private System.Windows.Forms.Button btnStar;
        private System.Windows.Forms.Button btnDash;
        private System.Windows.Forms.Button btnHash;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnPercent;
        private System.Windows.Forms.Button btnAt;
        private System.Windows.Forms.Button btnBackward;
		private System.Windows.Forms.ColumnHeader cc_address;
		private System.Windows.Forms.ColumnHeader cc_edit;
		private System.Windows.Forms.ColumnHeader cc_id;
		private System.Windows.Forms.Button btnAddNew;
		private System.Windows.Forms.ColumnHeader cc_balance;
    }
}