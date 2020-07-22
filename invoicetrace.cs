using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class invoicetrace : Form
	{
		public string m_kw = "";
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		private PrintDocument printDoc = new PrintDocument();
		private Control m_lastFocused = null;
		private string InvDateFrom = "";
		private string InvDateTo = "";
/*		private string InvTimeFrom = "";
		private string InvTimeTo = "";
		private string InvTimeFormatFrom = "";
		private string InvTimeFormatTo = "";
*/
		private string Invoice = "";
		private string m_pks = "";
		private string m_s = "";
		private string m_sPrintBuffer = "";
		private int SelectedInv = 0;
		private int SelectedT = 0;
		private double From = 0;
		private double To = 0;

		public invoicetrace()
		{
			InitializeComponent();
			printDoc.PrinterSettings.PrinterName = Program.m_sPrinterName;
			printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
		}
		private void invoicetrace_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			this.txtKW.Text = m_kw;
			dpFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
			dpTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
			dpFrom.Format = DateTimePickerFormat.Custom;
			dpFrom.CustomFormat = "dd-MM-yyyy";
			dpTo.Format = DateTimePickerFormat.Custom;
			dpTo.CustomFormat = "dd-MM-yyyy";
			DoSearchInv();
			txtKW.Select();
		}
		private void DoSearchInv()
		{
			int rows = 0;
			string top = " top 10 ";
			m_kw = this.txtKW.Text;
			InvDateFrom = this.dpFrom.Text;
			InvDateTo = this.dpTo.Text;
			if(InvDateFrom != "" || InvDateTo != "")
				top = "";
			if (dst.Tables["inv"] != null)
				dst.Tables["inv"].Clear();
			string sc = "";
//			if (InvTimeFrom != "")
			sc += " SET DATEFORMAT dmy ";
			sc += " SELECT "+ top + " o.station_id, o.invoice_number, o.record_date , i.total as total ";
			sc += " FROM orders o JOIN invoice i ON i.invoice_number = o.invoice_number";
			sc += " WHERE 1=1 ";
			if (m_kw != "")
			{
				if (m_kw.Length <= 2)
					sc += " AND o.station_id = " + m_kw;
				else
					sc += " AND o.invoice_number LIKE '%" + m_kw + "%'";
			}
			else
			{
				if (InvDateFrom != "")
					sc += " AND o.record_date >= '" + InvDateFrom + "' ";
				if (InvDateTo != "")
					sc += " AND o.record_date <= '" + InvDateTo + " 23:59' ";
			}
			sc += " ORDER BY o.invoice_number DESC";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "inv");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return;
			}
			this.listView1.Items.Clear();
			string invoice_number = "";
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["inv"].Rows[i];
				string station_id = dr["station_id"].ToString();
				invoice_number = dr["invoice_number"].ToString();
				string record_date = dr["record_date"].ToString();
				string total = dr["total"].ToString();

				ListViewItem item = new ListViewItem(station_id);
				item.SubItems.Add(invoice_number);
				item.SubItems.Add(record_date);
				item.SubItems.Add(double.Parse(total).ToString("c"));
				this.listView1.Items.Add(item);
			}
			if(rows == 1)
			{
				m_s = Program.BuildReceiptFromInvoice(invoice_number, false, ref m_pks, false);
				Invoice = m_s;
				this.rtbView.Text = Invoice;
				this.txtKW.Focus();
			}
		}
		private void txtKW_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Return:
					DoSearchInv();
					break;
			}
		}
		private void cmbTimeFrom_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			DoSearchInv();
		}
		private void DoSelectedInv()
		{
			ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
			foreach (ListViewItem item in items)
			{
				SelectedT = int.Parse(item.SubItems[0].Text);
				SelectedInv = int.Parse(item.SubItems[1].Text);
			}
			m_s = Program.BuildReceiptFromInvoice(SelectedInv.ToString(), false, ref m_pks, false);
			Invoice = m_s;
			this.rtbView.Text = Invoice;
			this.txtKW.Focus();
		}
		private string ReadSitePage(string name)
		{
			DataSet dsr = new DataSet();
			if (dsr.Tables["rsp"] != null)
				dsr.Tables["rsp"].Clear();
			string sc = " SELECT id, text FROM site_pages WHERE name = '" + name + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dsr, "rsp");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return ""; ;
			}
			if (dsr.Tables["rsp"].Rows.Count > 0)
			{
				DataRow dr = dsr.Tables["rsp"].Rows[0];
				//Debug.Write("id = " + dr["id"].ToString());
				return dr["text"].ToString();
			}
			return "";
		}
		private void btnView_Click(object sender, EventArgs e)
		{
			DoSelectedInv();
			this.rtbView.Text = Invoice;
			this.txtKW.Focus();
		}
		private void btnPrint_Click(object sender, EventArgs e)
		{
			DoSelectedInv();
			PrintInv();
		}
		private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
		{
			float fSize = (float)Program.MyDoubleParse(Program.m_sFontSize);
			Font printFont = new Font(Program.m_sFontName, fSize, Program.m_tFontStyle);
			Font printFontBig = new Font(Program.m_sFontName, fSize + 6, Program.m_tFontStyle);
			Font printFontJiant = new Font(Program.m_sFontName, fSize + 20, Program.m_tFontStyle);
			Font m_BarcodeFont = new Font("3 of 9 Barcode", 19);
			Font m_PrintFont = new Font("Times New Roman", 50);
			int y = 0; //vertical position;
			int lineHeight = 14;
			int lineHeightBig = 30;
			int lineHeightJiant = 50;
			string s = "1";
			int receiptEnd = 0;
			for (int c = 0; c < m_sPrintBuffer.Length; c++)
			{
				if (m_sPrintBuffer.Substring(c, 1) == "\r")
					receiptEnd++;
			}

			m_sPrintBuffer = m_sPrintBuffer.Replace("\r", "");

			int b = -1;
			int j = -1;
			string[] aLine = m_sPrintBuffer.Split('\n');
			for (int i = 0; i < aLine.Length; i++)
			{
				string sLine = aLine[i] + "\r\n";
				b = sLine.IndexOf("[b]");
				j = sLine.IndexOf("[j]");
				if (b >= 0)
				{
					sLine = sLine.Replace("[b]", "").Replace("[/b]", "");
					e.Graphics.DrawString(sLine, printFontBig, Brushes.Black, 0, y);
					y += lineHeightBig;
				}
				else if (j >= 0)
				{
					sLine = sLine.Replace("[j]", "").Replace("[/j]", "");
					e.Graphics.DrawString(sLine, printFontJiant, Brushes.Black, 0, y);
					y += lineHeightJiant;
				}
				else
				{
					e.Graphics.DrawString(sLine, printFont, Brushes.Black, 0, y);
					y += lineHeight;
				}
			}
		
/*			float fSize = (float)Program.MyDoubleParse(Program.m_sFontSize);
			Font printFont = new Font(Program.m_sFontName, fSize, Program.m_tFontStyle);
			Font printFontBig = new Font(Program.m_sFontName, fSize + 6, Program.m_tFontStyle);
			int y = 0; //vertical position;
			int lineHeight = 20;
			string s = "1";
			int o = 1;

			int p = m_sPrintBuffer.IndexOf("[b]");
			if (p < 0)
			{
				s = m_sPrintBuffer;
				e.Graphics.DrawString(m_sPrintBuffer, printFont, Brushes.Black, 0, y);

				return;
			}
			int pEnd = m_sPrintBuffer.IndexOf("[/b]", p + 3);

			if (p > 0)
			{
				s = m_sPrintBuffer.Substring(0, p);
				o = s.Length * 2;
				e.Graphics.DrawString(s, printFontBig, Brushes.Black, 0, y);
				y += lineHeight;
			}
			if (pEnd > 0)//big font end
			{
				s = m_sPrintBuffer.Substring(p + 3, pEnd - p - 3); //sub string in big font
				e.Graphics.DrawString(s, printFontBig, Brushes.Black, o, y);
				y += lineHeight;
				s = m_sPrintBuffer.Substring(pEnd + 4, m_sPrintBuffer.Length - pEnd - 4); //rest of string, in regular size
				e.Graphics.DrawString(s, printFont, Brushes.Black, 0, y);
			}
			else
			{
				s = m_sPrintBuffer.Substring(pEnd + 4, m_sPrintBuffer.Length - pEnd - 4); //rest of string, in regular size
				e.Graphics.DrawString(m_sPrintBuffer, printFontBig, Brushes.Black, 0, y);
			}
*/ 
		}
		private void PrintInv()
		{
			if (Invoice == "")
				return;
			m_sPrintBuffer = Invoice;
			printDoc.Print();
			//MessageBox.Show(Invoice);
			this.txtKW.Focus();
		}
		private void PrintPks()
		{
			if (m_pks == "")
				return;
			m_sPrintBuffer = m_pks;
			printDoc.Print();
			this.txtKW.Focus();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void listView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13)
			{
				DoSelectedInv();
				this.rtbView.Text = Invoice;
			}
		}
		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			DoSelectedInv();
			this.rtbView.Text = Invoice;
		}
		private void btnExport_Click(object sender, EventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.listView1.SelectedItems;
			if(items.Count <= 0)
				return;
			SelectedT = int.Parse(items[0].SubItems[0].Text);
			SelectedInv = int.Parse(items[0].SubItems[1].Text);
			
			if(!ChangeToExportInvoice(SelectedInv.ToString()))
				return;

			m_s = Program.BuildReceiptFromInvoice(SelectedInv.ToString(), true, ref m_pks,false);
			this.rtbView.Text = Invoice;
			PrintInv();
		}
		private bool ChangeToExportInvoice(string invoice_number)
		{
			int nRows = 0;
			if (dst.Tables["item"] != null)
				dst.Tables["item"].Clear();
			string sc = " SELECT oi.* ";
			sc += " FROM orders o ";
			sc += " JOIN order_item oi ON oi.id = o.id ";
			sc += " WHERE o.invoice_number = " + invoice_number;
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand1.Fill(dst, "item");
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			double dTax = 0;
			sc = "";
			for (int i = 0; i < dst.Tables["item"].Rows.Count; i++)
			{
				DataRow dr = dst.Tables["item"].Rows[i];
				string kid = dr["kid"].ToString();
				string code = dr["code"].ToString();
				double dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				string sTaxCode = dr["tax_code"].ToString();//GetItemTaxCode(code);
				double dCommitPrice = Program.MyDoubleParse(dr["commit_price"].ToString());
				double qty = Program.MyDoubleParse(dr["quantity"].ToString());
				dTax += Math.Round(dCommitPrice * dTaxRate * qty, 4);
				double dp = Program.MyDoubleParse(dr["order_total"].ToString());
				dp = Math.Round(dp, 4);
				double dPrice = dCommitPrice + dTax;
				dPrice = Math.Round(dPrice, 4);
				sc += " UPDATE order_item SET commit_price = " + dPrice + ", tax_rate = 0, tax_code = '' WHERE kid = " + kid;
			}
			if(dTax == 0)
			{
				MessageBox.Show("Error, this invoice has no tax, cannot change to export invoice");
				return false;
			}

			sc += " UPDATE sales SET commit_price = commit_price * (1 + tax_rate), tax_rate = 0 WHERE invoice_number = " + invoice_number;
			sc += " UPDATE invoice SET price = price + tax, tax = 0 WHERE invoice_number = " + invoice_number;
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
		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			DoSelectedInv();
			this.rtbView.Text = Invoice;
			this.txtKW.Focus();
		}

		private void txtKW_Click(object sender, EventArgs e)
		{
			FormPad np = new FormPad();
			np.Location = new Point(70,125);
			np.ShowDialog();
			txtKW.Text = np.m_sChangeQty;
			txtKW.SelectAll();
		}

		private void txtKW_Leave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}
	}
}
