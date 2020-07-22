using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;
using Microsoft.Win32;
using System.Web.Security;

namespace QPOS2008
{
	partial class FormAddNewItem : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);


		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		private DataSet dsCart = new DataSet();

		public string mBarcode = "";

		public FormAddNewItem()
		{
			InitializeComponent();

//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			doBuideBrands();
			doBuideCat();
            doBuideSCat();
            doBuideSSCat();
			form1 Form1 = new form1();
			this.CreateNewBarcodeTB.Text = Program.m_Barcode;

		}


		private void FormAddNewItem_Load(object sender, EventArgs e)
		{
			//this.CreateNewItemPanel.Visible = true;
			doBuideBrands();
			doBuideCat();
            doBuideSCat();
            doBuideSSCat();
			form1 Form1 = new form1();
			this.CreateNewBarcodeTB.Text = Form1.barcode.Text;
			
				
		}

		private void labelCompanyName_Click(object sender, EventArgs e)
		{
		}
		private bool doCreateNewItem()
		{
			if (this.CreateNewNameTB.Text.Trim() == "" || this.CreateNewNameTB.Text.Trim() == null || this.CreateNewBarcodeTB.Text.Trim() == "" || this.CreateNewBarcodeTB.Text.Trim() == null)
				return false;
			string barcodeNew = this.CreateNewBarcodeTB.Text.Trim();
            string supplierCode = this.txtSupplierCode.Text.Trim();
			string name = this.CreateNewNameTB.Text.Trim();
			string otherName = this.CreateNewOtherNameTB.Text.Trim();
			string brand = "";
			if (this.BrandComboBox.Text.Trim() != null && this.BrandComboBox.Text.Trim() != "")
				brand = this.BrandComboBox.Text.Trim();
			else
				brand = this.CreateNewBrandTB.Text.Trim();
			string catnew = "";
			if (this.catselectionCB.Text.Trim() != null && this.catselectionCB.Text.Trim() != "")
				catnew = this.catselectionCB.Text.Trim();
			else
				catnew = this.CreateNewCatTB.Text.Trim();
/***********************************/
            string scatnew = "";
			if (this.scatselectionCB.Text.Trim() != null && this.scatselectionCB.Text.Trim() != "")
				scatnew = this.scatselectionCB.Text.Trim();
			else
				scatnew = this.CreateNewSCatTB.Text.Trim();
            string sscatnew = "";
			if (this.sscatselectionCB.Text.Trim() != null && this.sscatselectionCB.Text.Trim() != "")
				sscatnew = this.sscatselectionCB.Text.Trim();
			else
				sscatnew = this.CreateNewSSCatTB.Text.Trim();
/***********************************/
			string priceNew = "0";
			if (this.CreateNewSellPriceTB.Text.Trim() != "" && this.CreateNewSellPriceTB.Text.Trim() != null)
				priceNew = this.CreateNewSellPriceTB.Text.Trim();
			string Mcost = this.CreateNewMcostTB.Text.Trim();
			string codeNew = GetNextCode().ToString();

			//MessageBox.Show("code now", codeNew);

			string sc = "BEGIN TRANSACTION ";
			sc += "INSERT INTO code_relations (id, code, supplier_code, name, name_cn, brand, cat, price1, manual_cost_frd, supplier_price) ";
			sc += " VALUES ";
			sc += " ('" + DateTime.Now.ToOADate() + "'";
			sc += ", ";
			sc +=  codeNew;
            sc += ", '" + supplierCode + "'"; 
			sc += ", N'" + Program.EncodeQuote(name) + "'";
			sc += ", N'" + Program.EncodeQuote(otherName) + "'";
			sc += ", N'" + Program.EncodeQuote(brand) + "'";
			sc += ", N'" + Program.EncodeQuote(catnew) + "'";
			sc += ", ";
			sc += "'" + priceNew + "'";
			sc += ", '" + Mcost + "'";
			sc += ", '" + Mcost + "'";
			sc += ")";
			//sc += " SELECT IDENT_CURRENT('code_relations') AS id ";
            if (supplierCode == "" || supplierCode == null)
			    sc += " UPDATE code_relations SET supplier_code = code WHERE code = " + codeNew;
			sc += " INSERT INTO product (code, name, name_cn, brand, cat, s_cat, ss_cat, hot, price, stock, eta, supplier, supplier_code, supplier_price, price_dropped, price_age, allocated_stock, popular, real_stock) SELECT ";
			sc += " c.code, c.name, c.name_cn,c.brand,  c.cat, c.s_cat, c.ss_cat, '0', c.price1, 0,'', c.supplier, c.supplier_code, c.supplier_price, '0', getdate(), 0, '1', '0' FROM";
			sc += " code_relations c WHERE c.code = " + codeNew;

			sc += " INSERT INTO stock_qty (code, qty, branch_id, supplier_price, allocated_stock, average_cost, qpos_price, special_price, sp_start_date, sp_end_date) SELECT ";
			sc += " p.code, 0, 1, p.supplier_price,  p.allocated_stock, 0, p.price, '0', getdate(), getdate() FROM product p WHERE p.code =" + codeNew;

			if (catnew != "" && (this.catselectionCB.Text.Trim() == "" || this.catselectionCB.Text.Trim() == null))
				sc += " INSERT INTO catalog (seq, cat) VALUES ('99', N'" + catnew + "')";
            /************************/
            if (catnew != "" && (this.catselectionCB.Text.Trim() == "" || this.catselectionCB.Text.Trim() == null))
                sc += " INSERT INTO catalog (seq, cat,s_cat) VALUES ('99', N'" + catnew + "', N'" + scatnew + "')";
            if (catnew != "" && (this.sscatselectionCB.Text.Trim() == "" || this.sscatselectionCB.Text.Trim() == null))
                sc += " INSERT INTO catalog (seq, cat, s_cat, ss_cat) VALUES ('99', N'" + catnew + "', N'" + scatnew + ",' N'" + sscatnew + "')";
            /*************************/
			if (brand != "" && (this.BrandComboBox.Text.Trim() == "" || this.BrandComboBox.Text.Trim() == null))
				sc += " INSERT INTO catalog (seq, cat, s_cat) VALUES ('99', N'Brands', N'" + brand + "')";
			sc += "  COMMIT";
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

			sc = "IF NOT EXISTS(SELECT * FROM barcode WHERE barcode ='" + barcodeNew + "') ";
			sc += " BEGIN ";
			sc += " INSERT INTO barcode (item_code, barcode, item_qty, carton_barcode, box_qty, package_price, supplier_code)";
			sc += " SELECT c.code, '" + barcodeNew + "', 1, 0, 0, 0, c.supplier_code FROM code_relations c WHERE c.code='" + codeNew + "'";
			sc += " END ";
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

		private int GetNextCode()
		{
	//		int next_code = -1;
			int next_code = 1021;
			if (dst.Tables["get_code"] != null)
				dst.Tables["get_code"].Clear();
			int rows;
			string sc = "SELECT TOP 1 code FROM code_relations ORDER BY code DESC";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "get_code");
				if (rows > 0)
					next_code = int.Parse(dst.Tables["get_code"].Rows[0]["code"].ToString()) + 1;
				else
	//				next_code = 1001;
					next_code = 1021;
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
			}
			return next_code;
		}

		private void doClearCreateForm()
		{
			this.CreateNewNameTB.Text = "";
			this.CreateNewBrandTB.Text = "";
			this.CreateNewOtherNameTB.Text = "";
			this.CreateNewCatTB.Text = "";
			this.CreateNewMcostTB.Text = "";
			this.catselectionCB.Text = "";
			this.BrandComboBox.Text = "";
			this.CreateNewSellPriceTB.Text = "";
		}

		private void doBuideBrands()
		{
			this.BrandComboBox.Items.Clear();
			int rows = 0;
			if (dst.Tables["brand_selection"] != null)
				dst.Tables["brand_selection"].Clear();
			string sc = " SELECT  cat, s_cat FROM catalog ";
			sc += " WHERE LOWER(cat) = 'brands'";
			sc += " GROUP BY s_cat, cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "brand_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			this.BrandComboBox.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["brand_selection"].Rows[i];
				string cat = dr["s_cat"].ToString();
				if (cat.ToLower() == "zzzothers")
					continue;
				this.BrandComboBox.Items.Add(cat);
			}
		}

		private void doBuideCat()
		{
			this.catselectionCB.Items.Clear();
			int rows = 0;
			if (dst.Tables["cat_selection"] != null)
				dst.Tables["cat_selection"].Clear();
			string sc = " SELECT  cat FROM catalog ";
			sc += " WHERE 1=1 AND LOWER(cat) <> 'brands'";
			sc += " GROUP BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "cat_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			this.catselectionCB.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["cat_selection"].Rows[i];
				string cat = dr["cat"].ToString();
				this.catselectionCB.Items.Add(cat);
			}
		}


        private void doBuideSCat()
        {
            this.catselectionCB.Items.Clear();
            int rows = 0;
            if (dst.Tables["scat_selection"] != null)
                dst.Tables["scat_selection"].Clear();
            string sc = " SELECT  s_cat FROM catalog ";
            sc += " WHERE 1=1 AND LOWER(cat) <> 'brands'";
            sc += " AND cat = '" + catselectionCB.SelectedItem + "'";
            sc += " GROUP BY s_cat ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                rows = myAdapter.Fill(dst, "scat_selection");
                if (rows <= 0)
                    return;
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return;
            }
            this.scatselectionCB.Items.Add("");
            for (int i = 0; i < rows; i++)
            {
                DataRow dr = dst.Tables["scat_selection"].Rows[i];
                string scat = dr["s_cat"].ToString();
                this.scatselectionCB.Items.Add(scat);
            }
        }

        private void doBuideSSCat()
        {
            this.sscatselectionCB.Items.Clear();
            int rows = 0;
            if (dst.Tables["sscat_selection"] != null)
                dst.Tables["sscat_selection"].Clear();
            string sc = " SELECT  ss_cat FROM catalog ";
            sc += " WHERE 1=1 AND LOWER(cat) <> 'brands'";
            sc += " AND cat = '" + catselectionCB.SelectedItem + "'";
            sc += " AND scat = '" + scatselectionCB.SelectedItem + "'";
            sc += " GROUP BY ss_cat ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                rows = myAdapter.Fill(dst, "sscat_selection");
                if (rows <= 0)
                    return;
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return;
            }
            this.catselectionCB.Items.Add("");
            for (int i = 0; i < rows; i++)
            {
                DataRow dr = dst.Tables["sscat_selection"].Rows[i];
                string sscat = dr["ss_cat"].ToString();
                this.sscatselectionCB.Items.Add(sscat);
            }
        }


		private void button16_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure to create the New Item with the Barcode???", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				if (!doCreateNewItem())
					MessageBox.Show("Error, New Item Creation Failed");
				else
				{
					MessageBox.Show("New Item Creation Complete!");
					this.Close();
					//this.CreateNewItemPanel1.Visible = false;
					//this.barcode.Text = this.CreateNewBarcodeTB.Text.Trim();
					doClearCreateForm();
					form1 lb = new form1();
					lb.barcode.Text = CreateNewBarcodeTB.Text;
					this.CreateNewBarcodeTB.Text = "";
					return;
					//DoScanBarcode();
				}

			}
		}

		private void button15_Click(object sender, EventArgs e)
		{
			//this.CreateNewItemPanel1.Visible = false;
			doClearCreateForm();
			this.CreateNewBarcodeTB.Text = "";
			this.Close();
			form1 lb = new form1();
			//lb.m_sTotalPoint
			//this.barcode.Focus();
		}

        private void label1_Click(object sender, EventArgs e)
        {

        }
	}
}
