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
	public partial class search : Form
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
		public bool bIs_close = false;
		//		private bool m_bWriteOff = false;
		private bool m_bStart = false;
		private bool m_bListSelected = false;
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private DataSet dst = new DataSet();
		public search()
		{
			InitializeComponent();
		}

		private void DoSelectItem()
		{
			m_bListSelected = false;
			ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
			foreach (ListViewItem item in items)
			{
				m_sCode = item.SubItems[0].Text;
				m_sBarcode = item.SubItems[1].Text;
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
					search_KeyDown(sender, e);
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
					search_KeyDown(sender, e);
					break;
			}
		}
		private bool DoSearchItem()
		{
			string kw = m_kw; //this.kw.Text;
			string Str = m_kw.Trim(); //this.kw.Text.Trim();
			int Num;
			int iKeyInLenght = kw.Length;
			bool isNum = int.TryParse(Str, out Num);
			string top = " top 100 ";

			if (kw != "")
			{
				m_kw = kw;
				top = "";
			}
			string sc = " SELECT DISTINCT " + top + " c.code, c.name, c.name_cn, c.price1, c.supplier_code ";
			sc += ", b.barcode, b.item_qty";
			sc += ", c.barcode AS main_barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code";
			sc += " WHERE 1 = 1 ";
			//sc += " AND c.cat <> 'ServiceItem'";
			if (kw != "")
			{
				//sc += " AND ( ";
				//if (!isNum)
				//{
					sc += " AND (c.barcode = '" + Program.EncodeQuote(m_kw) + "' ";
					sc += " OR b.barcode = '" + Program.EncodeQuote(m_kw) + "' ";
					if(Program.m_bScanInSupplierCode)
						sc += " OR c.supplier_code = '" + Program.EncodeQuote(m_kw) + "'";
					if (isNum && Program.m_bScanInCode)
						sc += " OR c.code = '" + Program.EncodeQuote(m_kw) + "' ";
					sc += " ) ";
				//}
				//else
				//{
				//    sc += " Substring(c.supplier_code, 1, " + iKeyInLenght + ") LIKE N'%" + Program.EncodeQuote(m_kw).ToLower() + "%' ";
				//}
				//sc += " OR c.supplier_code LIKE '%" + Program.EncodeQuote(kw) + "%' OR b.barcode LIKE  '%" + Program.EncodeQuote(kw) + "%' OR c.barcode LIKE  '%" + Program.EncodeQuote(kw) + "%' ";
				//sc += " ) ";
			}
			if (!isNum)
				sc += " ORDER BY c.supplier_code ";

			if (dst.Tables["item"] != null)
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

			int nItems = 0;
			this.listView1.Items.Clear();
			foreach (DataRow dr in dst.Tables["item"].Rows)
			{
				string s_scode = dr["supplier_code"].ToString();
				string code = dr["code"].ToString();
				string sBarcode = dr["main_barcode"].ToString().Trim();
	//			if (sBarcode == "")
				  sBarcode = dr["barcode"].ToString();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
				string qty = dr["item_qty"].ToString();
				double dPrice = 0;
				dPrice = Double.Parse(dr["price1"].ToString());
				try
				{
					//dPrice = Double.Parse(dr["price1"].ToString());
				}
				catch
				{
					MessageBox.Show("error parse price1");
				}

				if (kw != code && kw.Trim() != s_scode.Trim() && kw != sBarcode)
					continue;
					
				nItems++;
				m_sCode = code;
				m_sBarcode = sBarcode;

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(sBarcode);

				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add(qty);
				//item.SubItems.Add("");
				this.listView1.Items.Add(item);
			}
			if(nItems == 1)
			{
				this.Close();
			}
			return true;
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
		private void search_Load(object sender, EventArgs e)
		{
			DoSearchItem();
		}

		private void search_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
			bIs_close = true;
		}

		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			DoSelectItem();
		}

		private void listView1_Click(object sender, EventArgs e)
		{
			m_bListSelected = true;
		}
	}
}
