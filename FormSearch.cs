using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormSearch : Form
	{
		public string m_sCode = "";
		public string m_sBarcode = "";
		public string m_sName = "";
		public string m_sNameCN = "";
		public string m_sPrice = "";
		public double m_dQty = 1;
		public string m_skw = "";
		public string m_sSubCode = "";
		public string m_kw = "";
//		private bool m_bWriteOff = false;
		private bool m_bStart = false;
        private bool m_bListSelected = false;
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private DataSet dst = new DataSet();
		public FormSearch()
		{
			InitializeComponent();
		}
		private void DoSelectItem()
		{
            m_bListSelected = false;
            ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
			foreach (ListViewItem item in items)
			{
				m_sCode = item.SubItems[1].Text.Trim();
				if(m_sCode == "")
					m_sCode = item.SubItems[0].Text.Trim();
			}
			if (m_sCode != "")
				this.Close();
		}
		private void OnListKeyDown(object sender, KeyEventArgs e)
		{
			string code_name = Program.LookupHotKey(e);
			if (code_name == "add_barcode")
			{
			 
				ShowAddBarcodeWindow();
				return;
			}
			switch (e.KeyCode)
			{
				case Keys.Return:
					DoSelectItem();
					break;
				default:
					FormSearch_KeyDown(sender, e);
					break;
			}
		}
		private void OnKWKeyDown(object sender, KeyEventArgs e)
		{
			
			switch (e.KeyCode)
			{
				case Keys.Return:
					DoSearchItem();
					break;
				case Keys.Down:
					this.listView1.Focus();
					break;
			   
				default:
					FormSearch_KeyDown(sender, e);
					break;
			}
		}
		private void buttonSearch_Click(object sender, EventArgs e)
		{
            if (m_bListSelected)
                DoSelectItem();
            else
                DoSearchItem();
		}
		private bool DoSearchItem()
		{
			string kw = this.kw.Text;
			string Str = this.kw.Text.Trim();
			int Num;
			int iKeyInLenght = kw.Length;
			bool isNum = int.TryParse(Str, out Num);
			string top = " top 100 ";

			if (kw != "")
			{
				m_kw = kw;
				top = "";
			}
			string sc = " SELECT " + top + " c.code, c.name, c.name_cn, c.price1, b.package_price, c.supplier_code, b.barcode, c.barcode AS main_barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code";
			sc += " WHERE 1 = 1 ";
            sc += " AND c.cat <> 'ServiceItem'";
			if (kw != "")
			{
				sc += " AND ( ";
				if (!isNum)
				{
					if(m_bStart)
					{
						sc += " Substring(LOWER(c.name), 1, " + iKeyInLenght + ") LIKE N'%" +Program.EncodeQuote(m_kw).ToLower()+"%'";
						sc += " OR Substring(LOWER(c.name_cn), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
						sc += " OR Substring(LOWER(c.supplier_code), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
					}
					else
					{
						sc += " LOWER(c.name) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
						sc += " OR LOWER(c.name_cn) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
						sc += " OR c.supplier_code LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
					}
				}
				else
				{
					sc += " Substring(c.supplier_code, 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
					sc += " OR c.code = " + kw;
				}
				sc += " OR c.supplier_code LIKE '%" + Program.EncodeQuote(kw) + "%' OR b.barcode LIKE  '%" + Program.EncodeQuote(kw) + "%' OR c.barcode LIKE  '%" + Program.EncodeQuote(kw) + "%' ";
				sc += " ) ";
			}
			if (!isNum )
				sc += " ORDER BY c.supplier_code ";

			if(dst.Tables["item"] != null)	
				dst.Tables["item"].Clear();
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "item");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return false;
			}
			if (dst.Tables["item"].Rows.Count <= 0)
			{
				if (m_skw == "")
				{
					MessageBox.Show("Item not found", "Search Result");
					this.kw.Focus();
					return false;

				}
				else
				{
					Console.Beep(2000, 300);
					Console.Beep(2000, 300);
					this.Close();
				}
			}
			this.listView1.Items.Clear();
			foreach(DataRow dr in dst.Tables["item"].Rows)
			{
				string s_scode = dr["supplier_code"].ToString();
				string code = dr["code"].ToString();
				string sBarcode = dr["main_barcode"].ToString().Trim();
	//			if (sBarcode == "")
					sBarcode = dr["barcode"].ToString();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
				double dPrice1 = 0;
				dPrice1 = Double.Parse(dr["price1"].ToString());
				double dPackagePrice = 0;
				if (dr["package_price"].ToString() == null || dr["package_price"].ToString() == "")
					dPackagePrice = 0;
				else	
					dPackagePrice = Double.Parse(dr["package_price"].ToString());
				
				double dPrice = 0;
				if (dPackagePrice == 0)
					dPrice = dPrice1;
				else if (dPackagePrice != 0)
					dPrice = dPackagePrice;	
				
				try
				{
					//dPrice = Double.Parse(dr["price1"].ToString());
				}
				catch
				{
					MessageBox.Show("error parse price1");
				}

                ListViewItem item = new ListViewItem(code);
				//item.SubItems.Add(barcode);

				item.SubItems.Add(sBarcode);
				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add(dPrice.ToString("c"));
				//item.SubItems.Add(dQty.ToString());
				item.SubItems.Add(sBarcode);
				this.listView1.Items.Add(item);    
			}
			return true;
		}
		private void FormSearch_Load(object sender, EventArgs e)
		{
			for (int n = 0; n <= 2; n++)
			{
				int s = 0;
				int en = 0;
				if (n == 0)
				{
					s = 7;
					en = 9;
				}
				if (n == 1)
				{
					s = 4;
					en = 6;

				}
				if (n == 2)
				{
					s = 1;
					en = 3;
				}
				for (int i = s; i <= en; i++)
				{

					Button NewTableButton = new Button();
					//NewTableButton.BackColor = System.Drawing.Color.White;
					//if (i == 1)
					NewTableButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
					NewTableButton.BackgroundImageLayout = ImageLayout.Stretch;
					//else
					//	NewTableButton.BackgroundImage = global::WindowsFormsApplication5.Properties.Resources.tableblue;

					NewTableButton.Width = 60;
					NewTableButton.Height = 60;
					NewTableButton.AutoSize = false;
					NewTableButton.ForeColor = System.Drawing.Color.Black;
					NewTableButton.Text = i.ToString();
					NewTableButton.Font = new Font("Arial", 18, FontStyle.Bold);
					NewTableButton.Name = "contRightInsertOKButton";
					NewTableButton.Click += new EventHandler(contRightInsertOKButton_Click);
					flowLayoutPanel1.Controls.Add(NewTableButton);
				}
			}

			Button ZButton = new Button();
			ZButton.Text = "0";
			ZButton.Font = new Font("Arial", 18, FontStyle.Bold);
			ZButton.BackColor = System.Drawing.Color.Transparent;
			ZButton.ForeColor = System.Drawing.Color.Black;
			ZButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
			ZButton.BackgroundImageLayout = ImageLayout.Stretch;
			ZButton.AutoSize = false;
			ZButton.Width = 60;
			ZButton.Height = 60;
			ZButton.Name = "zero";
			ZButton.Click += new EventHandler(Z_Click);
			flowLayoutPanel1.Controls.Add(ZButton);

			Button DZButton = new Button();
			DZButton.Text = "BACK";
			DZButton.Font = new Font("Arial", 11, FontStyle.Bold);
			DZButton.BackColor = System.Drawing.Color.Transparent;
			DZButton.ForeColor = System.Drawing.Color.Black;
			DZButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
			DZButton.BackgroundImageLayout = ImageLayout.Stretch;
			DZButton.AutoSize = false;
			DZButton.Width = 60;
			DZButton.Height = 60;
			DZButton.Name = "back";
			DZButton.Click += new EventHandler(backspace_Click);
			flowLayoutPanel1.Controls.Add(DZButton);

			Button DButton = new Button();
			DButton.Text = "ENT";
			DButton.Font = new Font("Arial", 14, FontStyle.Bold);
			DButton.BackColor = System.Drawing.Color.Transparent;
			DButton.ForeColor = System.Drawing.Color.Black;
			DButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
			DButton.BackgroundImageLayout = ImageLayout.Stretch;
			DButton.AutoSize = false;
			DButton.Width = 60;
			DButton.Height = 60;
			DButton.Name = "enter";
			DButton.Click += new EventHandler(buttonSearch_Click);
			flowLayoutPanel1.Controls.Add(DButton);
			DoSearchItem();
			this.kw.Focus();
		
		}
		
		private void backspace_Click(object sender, EventArgs e)
		{
			Button CurrentBackSpace = (Button)sender;
			string currentDigit = this.kw.Text;
			if (currentDigit != "")
			{
				currentDigit = currentDigit.Substring(0, currentDigit.Length - 1);
				this.kw.Text = currentDigit;
			}
		}

		private void contRightInsertOKButton_Click(object sender, EventArgs e)
		{
			Button CurrentNumber = (Button)sender;
			this.kw.Text = this.kw.Text + CurrentNumber.Text;

		}
		private void Z_Click(object sender, EventArgs e)
		{
			Button CurrentZ = (Button)sender;
			this.kw.Text = this.kw.Text + CurrentZ.Text;
		}
		private void DZ_Click(object sender, EventArgs e)
		{
			Button CurrentDZ = (Button)sender;
			this.kw.Text = this.kw.Text + CurrentDZ.Text;
		}
		private void D_Click(object sender, EventArgs e)
		{
			Button CurrentD = (Button)sender;
			this.kw.Text = this.kw.Text + CurrentD.Text;
		}
		
		private void FormSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}
		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			DoSelectItem();
		}
		private void ShowAddBarcodeWindow()
		{
			ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
			if (items.Count <= 0)
			{
				MessageBox.Show("Please select an item first", "Add Barcode");
				return;
			}
			FormBarcode fb = new FormBarcode();
			fb.m_sCode = items[0].SubItems[0].Text;
			fb.m_sSubCode = items[0].SubItems[4].Text;
			fb.ShowDialog();
            if (m_bListSelected)
                DoSelectItem();
            else
                DoSearchItem();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void kw_Click(object sender, EventArgs e)
		{
			panel1.Visible = true;
			panel1.Width = 792;
			panel1.Height = 275;
		}

		private void btnBack_Click(object sender, EventArgs e)
		{
			Button CurrentBackSpace = (Button)sender;
			string currentDigit = this.kw.Text;
			if (currentDigit != "")
			{
				currentDigit = currentDigit.Substring(0, currentDigit.Length - 1);
				this.kw.Text = currentDigit;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnENT_Click(object sender, EventArgs e)
		{
			if(kw.Text != "")
				DoSearchItem();
		}

		private void btnKeyBoard_Click(object sender, EventArgs e)
		{
            m_bListSelected = false;
            panel1.Visible = true;
			panel1.Width = 792;
			panel1.Height = 275;
		}

		private void btnKey_Click(object sender, EventArgs e)
		{
			string kwInput = this.kw.Text;
			Button key = (Button)sender;
			if(key.Text.ToLower() == "del")
		    {
				if(kwInput == "")
					return;
				kwInput = kwInput.Substring(0, kwInput.Length -1);
				this.kw.Text = kwInput;
			}
			else if(key.Text.ToLower() == "enter")
			{
				panel1.Visible = false;
				kw.Focus();
				DoSearchItem();
			}
			else if(key.Text.ToLower() == "close")
			{
				panel1.Visible = false;
				kw.Focus();
				kw.Clear();
			}
			else if(key.Text.ToLower() == "space")
			{
				this.kw.Text = this.kw.Text + " " ;
			}
			else
				this.kw.Text = this.kw.Text + key.Text;
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			m_bStart = true;
			DoSearchItem();
			m_bStart = false;
		}

        private void listView1_Click(object sender, EventArgs e)
        {
            m_bListSelected = true;
        }
	}
}
