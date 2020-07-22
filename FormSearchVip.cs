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
	public partial class FormSearchVip : Form
	{
		public string m_sCardId = "";
		public string m_sBarcode = "";
		public string m_sName = "";
		public string m_sNameCN = "";
		public string m_sPrice = "";
		public double m_dQty = 1;
		public string m_skw = "";
		public string m_sSubCode = "";
		public string m_kw = "";
//        private bool m_bWriteOff = false;
		private bool m_bStart = false;
		private bool m_bListSelected = false;
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private DataSet dst = new DataSet();
		public FormSearchVip()
		{
			InitializeComponent();
		}
		private void DoSelectItem()
		{
			m_bListSelected = false;
			ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
			foreach (ListViewItem item in items)
			{
				m_sCardId = item.SubItems[7].Text;
				m_sBarcode = item.SubItems[0].Text;
			}
			if (m_sBarcode != "")
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
					DoSearchCard();
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
				DoSearchCard();
		}
		private bool DoSearchCard()
		{
			string kw = this.kw.Text;
			string Str = this.kw.Text.Trim();
			double Num;
			int iKeyInLenght = kw.Length;
			bool isNum = double.TryParse(Str, out Num);
			string top = " top 100 ";

			if (kw != "")
			{
				m_kw = kw;
				top = "";
			}
			string sc = " SELECT " + top + " c.barcode, c.name, c.phone, c.mobile, c.email, c.id, c.address1, c.address2, c.address3 ";
			sc += " FROM card c ";
			sc += " WHERE 1 = 1 ";
			if (kw != "")
			{
				sc += " AND ( ";
				if (!isNum)
				{
					if (m_bStart)
					{
						sc += " Substring(LOWER(c.name), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
						sc += " OR Substring(LOWER(c.barcode), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
						sc += " OR Substring(LOWER(c.phone), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
						sc += " OR Substring(LOWER(c.email), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
                        sc += " OR Substring(LOWER(c.mobile), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
                        sc += " OR Substring(LOWER(c.address1), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
                        sc += " OR Substring(LOWER(c.address2), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";
                        sc += " OR Substring(LOWER(c.address3), 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%'";

					}
					else
					{
						sc += " LOWER(c.name) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
						sc += " OR LOWER(c.barcode) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
						sc += " OR LOWER(c.phone) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
						sc += " OR LOWER(c.email) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                        sc += " OR LOWER(c.mobile) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                        sc += " OR LOWER(c.address1) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                        sc += " OR LOWER(c.address2) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                        sc += " OR LOWER(c.address3) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
					}
				}
				else
				{
					sc += " LOWER(c.name) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
					sc += " OR LOWER(c.barcode) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
					sc += " OR LOWER(c.phone) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
					sc += " OR LOWER(c.email) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                    sc += " OR LOWER(c.mobile) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                    sc += " OR LOWER(c.address1) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                    sc += " OR LOWER(c.address2) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
                    sc += " OR LOWER(c.address3) LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
				}
				sc += " ) ";
			}
			sc += " AND c.type in (1, 6) ";
			if (!isNum)
				sc += " ORDER BY c.name ";
			if (dst.Tables["vip"] != null)
				dst.Tables["vip"].Clear();
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "vip");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return false;
			}
			if (dst.Tables["vip"].Rows.Count <= 0)
			{
				if (m_skw == "")
				{
					MessageBox.Show("Vip not found", "Search Result");
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
			foreach (DataRow dr in dst.Tables["vip"].Rows)
			{
				string id = dr["id"].ToString();
				string barcode = dr["barcode"].ToString();
				string name = dr["name"].ToString();
				string phone = dr["phone"].ToString();
				string mobile = dr["mobile"].ToString();
				string email = dr["email"].ToString();
				string balance = getbalance(id);
				string address1 = dr["address1"].ToString();
				string address2 = dr["address2"].ToString();
				string address3 = dr["address3"].ToString();
				string address = address1 + " " + address2 + " " + address3;

				ListViewItem item = new ListViewItem(barcode);
				item.SubItems.Add(name);
//				item.SubItems.Add(email);
				item.SubItems.Add(phone);
				item.SubItems.Add(mobile);
				item.SubItems.Add(address);
				item.SubItems.Add(balance);
				item.SubItems.Add("edit");
				item.SubItems.Add(id);
				this.listView1.Items.Add(item);
			}
			return true;
		}
		
		string getbalance(string card_id)
		{
			string sc = "";
			int nRows = 0;
			if (dst.Tables["getbalance"] != null)
				dst.Tables["getbalance"].Clear();


			sc = " SELECT invoice_number, total, amount_paid, CONVERT(varchar(99), commit_date, 103) AS sdate ";
			sc += " FROM invoice ";
			sc += " WHERE card_id = " + card_id;
			sc += " AND paid = 0 ";
			sc += " ORDER BY commit_date ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "getbalance");
				if (nRows <= 0)
					return "";
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return "";
			}

			double dDue = 0;
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["getbalance"].Rows[i];
				string inv = dr["invoice_number"].ToString();
				string sdate = dr["sdate"].ToString();
				double dTotal = Program.MyDoubleParse(dr["total"].ToString());
				double dPaid = Program.MyDoubleParse(dr["amount_paid"].ToString());
				double dBalance = dTotal - dPaid;
				dDue += dBalance;

			}
			return dDue.ToString("c");
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
			DoSearchCard();
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
				DoSearchCard();
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
			if (kw.Text != "")
				DoSearchCard();
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
			if (key.Text.ToLower() == "del")
			{
				if (kwInput == "")
					return;
				kwInput = kwInput.Substring(0, kwInput.Length - 1);
				this.kw.Text = kwInput;
			}
			else if (key.Text.ToLower() == "enter")
			{
				panel1.Visible = false;
				kw.Focus();
				DoSearchCard();
			}
			else if (key.Text.ToLower() == "close")
			{
				panel1.Visible = false;
				kw.Focus();
				kw.Clear();
			}
			else if (key.Text.ToLower() == "space")
			{
				this.kw.Text = this.kw.Text + " ";
			}
			else
				this.kw.Text = this.kw.Text + key.Text;
		}
		private void btnStart_Click(object sender, EventArgs e)
		{
			m_bStart = true;
			DoSearchCard();
			m_bStart = false;
		}
		private void listView1_Click(object sender, EventArgs e)
		{
			m_bListSelected = true;
		}
		private void listView1_MouseClick(object sender, MouseEventArgs e)
		{
			Point mousePosition = listView1.PointToClient(Control.MousePosition);
			ListViewHitTestInfo hit = listView1.HitTest(mousePosition);
			int columnindex = hit.Item.SubItems.IndexOf(hit.SubItem);
			if(columnindex == 6) //edit
			{
				ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
				foreach (ListViewItem item in items)
				{
					m_sCardId = item.SubItems[7].Text;
				}
				AddUser au = new AddUser();
				au.m_bUpdate = true;
				au.m_bVIP = true;
				au.m_sTitle = "Edit VIP";
				au.m_sBarcode = m_sBarcode;
				au.m_sCard_id = m_sCardId;
				au.ShowDialog();
				if (au.m_sBarcode != "")
				{
					m_sCardId = au.m_sCard_id;
					m_sBarcode = au.m_sBarcode;
					Close();
				}
			}
		}
		private void btnAddNew_Click(object sender, EventArgs e)
		{
/*			FormEditCard fm = new FormEditCard();
			fm.ShowDialog();
			if (fm.m_sBarcode != "")
			{
				m_sCardId = fm.m_sCardId;
				m_sBarcode = fm.m_sBarcode;
				Close();
			}
*/
			AddUser au = new AddUser();
			au.m_bVIP = true;
			au.m_sTitle = "Add New VIP";
			au.m_sBarcode = "";
			au.ShowDialog();
			if (au.m_sBarcode != "")
			{
				m_sCardId = au.m_sCard_id;
				m_sBarcode = au.m_sBarcode;
				Close();
			}
		}
	}
}

