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
	public partial class FormBarcode : Form
	{
		public string m_sCode = "";
        public string m_sSubCode = "";

		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();

		private void FormBarcode_Load(object sender, EventArgs e)
		{
			if(m_sCode == "")
			{
				MessageBox.Show("Item code needed");
				return;
			}
           
			string sc = " SELECT c.name, c.name_cn, c.price1 FROM code_relations c WHERE c.supplier_code = '" + m_sCode+"' OR c.code ='"+ m_sSubCode +"'";
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "item");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				Close();
				return;
			}
			if (dst.Tables["item"].Rows.Count <= 0)
			{
				MessageBox.Show("Item not found, code = " + m_sCode);
				Close();
				return;
			}
			this.labelItemName.Text = dst.Tables["item"].Rows[0]["name"].ToString();
			this.labelItemNameCN.Text = dst.Tables["item"].Rows[0]["name_cn"].ToString();
			this.textBarcode.Focus();
		}
		public FormBarcode()
		{
			InitializeComponent();
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			if(DoAddBarcode())
				this.Close();
		}
		private bool DoAddBarcode()
		{
			string barcode = this.textBarcode.Text;
			if(barcode == "")
				return true;
			double dQty = Program.MyDoubleParse(this.textQty.Text);
			if(dQty < 1)
				dQty = 1;
			string sc = " SELECT c.name, c.name_cn, b.item_qty FROM code_relations c JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE b.barcode = '" + Program.EncodeQuote(barcode) + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "barcode") > 0)
				{
					string sname = dst.Tables["barcode"].Rows[0]["name"].ToString() + "\r\n" + dst.Tables["barcode"].Rows[0]["name_cn"].ToString();
					string sQty = Program.MyDoubleParse(dst.Tables["barcode"].Rows[0]["item_qty"].ToString()).ToString();
					MessageBox.Show("Error, this barcode is already assigned to item:\r\n" + sname + "\r\nQty : " + sQty + ".");
					return false;
				}
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				Close();
				return false;
			}
			if (dst.Tables["item"].Rows.Count <= 0)
			{
				MessageBox.Show("Item not found, code = " + m_sCode);
				Close();
				return false;
			}
          
            sc += " INSERT INTO barcode (item_code, barcode, item_qty, supplier_code) VALUES('" + m_sSubCode + "', '" + Program.EncodeQuote(barcode) + "', " + dQty + ", '"+ m_sCode+"') ";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			//catch (Exception e)
            catch (Exception e )
			{
				Program.ShowExp(sc, e);
              //  MessageBox.Show("Error, The Barcode Can Not Be Added, Error Code:10005");
				return false;
			}
			MessageBox.Show("Barcode added successfully, click OK to continue");
			return true;			
		}

		private void TextBarcode_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
			{
				if(DoAddBarcode())
					this.Close();
			}
			else
			{
				FormBarcode_KeyDown(sender, e);
			}
		}

		private void FormBarcode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}
	}
}
