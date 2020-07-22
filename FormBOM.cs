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
	public partial class FormBOM : Form
	{
		public string m_code = "";
		public string m_name = "";
		private string m_id = "";

		private SqlDataAdapter myAdapter;
		private SqlConnection myConnection;
		private SqlCommand myCommand;
		DataSet dst = new DataSet();
		
		public FormBOM()
		{
			InitializeComponent();
		}
		private void FormBOM_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			lblDescription.Text = m_name;
			GetBOM();
			GetProductList();
		}
		private bool GetBOM()
		{
			if(m_code == "")
			{
				MessageBox.Show("invalid item code");
				return false;
			}
			if(dst.Tables["bom"] != null)
				dst.Tables["bom"].Clear();
			string sc = " SELECT i.bom_id, i.code, c.name, i.qty ";
			sc += " FROM bom b ";
			sc += " LEFT OUTER JOIN bom_item i ON i.bom_id = b.id ";
			sc += " LEFT OUTER JOIN code_relations c ON c.code = i.code ";
			sc += " WHERE b.code = " + m_code;
			int nRows = 0;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "bom");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			
			lvBOM.Items.Clear();
			if (nRows <= 0)
				return true;
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["bom"].Rows[i];
				m_id = dr["bom_id"].ToString();
				string code = dr["code"].ToString();
				string name = dr["name"].ToString();
				string qty = dr["qty"].ToString();
				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(name);
				item.SubItems.Add(qty);
				item.SubItems.Add("edit");
				item.SubItems.Add("del");
				this.lvBOM.Items.Add(item);
			}
			return true;
		}
		private void lvBOM_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvBOM.SelectedItems;
			if (items.Count <= 0)
				return;
			string code = items[0].SubItems[0].Text;
			string name = items[0].SubItems[1].Text;
			string qty = items[0].SubItems[2].Text;
			string sAction = items[0].GetSubItemAt(e.X, e.Y).Text;
			if(sAction == "del")
			{
				if (MessageBox.Show("Do you want to delete this item from BOM? click Yes to delete", "Confirm deleting", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;
				DoDeleteCode(code);
			}
			else if(sAction == "edit")
			{
				DoEditItemQty(code, name, qty);
			}
		}
		private void DoEditItemQty(string code, string name, string qty)
		{
			FormBOMItem fm = new FormBOMItem();
			fm.m_sName = name;
			fm.m_dQty = Program.MyDoubleParse(qty);
			fm.ShowDialog();
			if (fm.m_dQty == -1)
				return;
			else if (fm.m_dQty == 0)
				DoDeleteCode(code);
			else
				DoUpdateBomItemQty(code, fm.m_dQty);
		}
		private bool DoUpdateBomItemQty(string code, double dQty)
		{
			if (m_id == "")
			{
				if (!DoCreateBOM())
					return false;
			}
			string sc = " IF NOT EXISTS(SELECT id FROM bom_item WHERE bom_id = " + m_id + " AND code = " + code + ") ";
			sc += " INSERT INTO bom_item(bom_id, code, qty) VALUES(" + m_id + ", " + code + ", " + dQty + ") ";
			sc += " ELSE ";
			sc += " UPDATE bom_item SET qty = " + dQty + " WHERE bom_id = " + m_id + " AND code = " + code;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			return GetBOM();
		}
		private bool DoDeleteCode(string code)
		{
			if(m_id == "")
				return true;
			string sc = " DELETE FROM bom_item WHERE code = '" + code + "' AND bom_id = '" + m_id + "'";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			return GetBOM();
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			GetProductList();
		}
		private void GetProductList()
		{
			string cat = cbCatFilter.Text;
			this.lvitemlist.Items.Clear();
			this.lvitemlist.Scrollable = true;
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtsearch.Text.ToLower().Trim();
			string sc = " SELECT DISTINCT c.brand, c.cat, c.s_cat, c.name, c.name_cn, c.code, c.supplier_code, c.barcode ";
			sc += " FROM code_relations c ";
			sc += " WHERE c.code <> + " + m_code + " AND c.bom_id IS NULL ";
			if (cat != "")
				sc += " AND LOWER(c.cat) = N'" + cat.ToLower() + "'";
			sc += " AND (";
			sc += " LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
			sc += " OR LOWER(c.barcode) LIKE '%" + kw + "%' ";
			sc += " OR c.code IN (SELECT item_code FROM barcode WHERE LOWER(barcode) = '" + kw + "') ";
			sc += " OR LOWER(c.supplier_code) LIKE '%" + kw + "' ";
			try
			{
				int.Parse(kw);
				sc += " OR c.code = " + kw + " ";
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
			}
			sc += ") ";
			sc += " And c.code > 1009 ";
			sc += " ORDER BY c.brand, c.cat, c.s_cat, c.name ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string sc1 = sc.Replace("DISTINCT", "DISTINCT top 100 ");
			try
			{
				myAdapter = new SqlDataAdapter(sc1, myConnection);
				rows = myAdapter.Fill(dst, "list");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc1, ex);
				myConnection.Close();
				return;
			}
			write_listview(dst.Tables["list"]);
		}
		private void write_listview(DataTable dt)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];
				string brand = dr["brand"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string barcode = dr["barcode"].ToString();
				string code = dr["code"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(brand);
				item.SubItems.Add(cat);
				item.SubItems.Add(s_cat);
				item.SubItems.Add(barcode);
				item.SubItems.Add(supplier_code);
				item.SubItems.Add(name);
				item.SubItems.Add("Add");
				this.lvitemlist.Items.Add(item);
			}
		}
		private void lvitemlist_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvitemlist.SelectedItems;
			if (items.Count <= 0)
				return;
			if (items[0].GetSubItemAt(e.X, e.Y).Text != "Add")
				return;
			string code = items[0].SubItems[0].Text;
			string name = items[0].SubItems[6].Text;
//			if (MessageBox.Show("Do you want to add this item to BOM? click Yes to add", "Confirm Add", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				DoEditItemQty(code, name, "1");
				return;
			}
		}
		private bool DoCreateBOM()
		{
			if(dst.Tables["id"] != null)
				dst.Tables["id"].Clear();
			string sc = " BEGIN TRANSACTION ";
			sc += " INSERT INTO bom (code) VALUES(" + m_code + ") ";
			sc += " SELECT IDENT_CURRENT('bom') AS id ";
			sc += " COMMIT ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "id") <= 0)
					return false;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			m_id = dst.Tables["id"].Rows[0]["id"].ToString();
			sc = " UPDATE code_relations SET bom_id = '" + m_id + "' WHERE code = " + m_code;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			return true;
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}


	}
}
