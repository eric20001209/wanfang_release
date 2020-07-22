using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormLatipayGiftCardHistory : Form
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
		private string Invoice = "";
		private string m_pks = "";
		private string m_s = "";
		private string m_sPrintBuffer = "";
		private int SelectedInv = 0;
		private int SelectedT = 0;

		public FormLatipayGiftCardHistory()
		{
			InitializeComponent();
			printDoc.PrinterSettings.PrinterName = Program.m_sPrinterName;
			printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
		}
		private void FormLatipayGiftCardHistory_Load(object sender, EventArgs e)
		{
			this.txtGiftCardCode.Text = m_kw;
			dpFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
			dpTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
			dpFrom.Format = DateTimePickerFormat.Custom;
			dpFrom.CustomFormat = "dd-MM-yyyy";
			dpTo.Format = DateTimePickerFormat.Custom;
			dpTo.CustomFormat = "dd-MM-yyyy";
			DoSearchLatipayGiftCard();
			txtGiftCardCode.Select();
		}
		private void DoSearchLatipayGiftCard()
		{
			m_kw = this.txtGiftCardCode.Text;
			InvDateFrom = this.dpFrom.Text;
			InvDateTo = this.dpTo.Text;

			string top = " top 10 ";
			if(InvDateFrom != "" || InvDateTo != "")
				top = "";

			int rows = 0;
			string tableName = "LatipayGiftCard";
			if (dst.Tables[tableName] != null)
				dst.Tables[tableName].Clear();
			string sc = "";
			sc += " SET DATEFORMAT dmy ";
			sc += " SELECT " + top + " lgc.till_number, lgc.gift_card_code, lgc.invoice_number, lgc.used_date , lgc.face_value";
			sc += " FROM latipay_gift_card lgc JOIN invoice i ON i.invoice_number = lgc.invoice_number";
			sc += " WHERE 1=1 ";
			if (m_kw != "")
			{
				sc += " AND lgc.invoice_number LIKE '%" + m_kw + "%'";
			}
			else
			{
				if (InvDateFrom != "")
					sc += " AND lgc.used_date >= '" + InvDateFrom + "' ";
				if (InvDateTo != "")
					sc += " AND lgc.used_date <= '" + InvDateTo + " 23:59' ";
			}
			sc += " ORDER BY lgc.merchant_reference DESC";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, tableName);
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return;
			}
			this.lvGiftCard.Items.Clear();
			string invoice_number = "";
			double total_value = 0;
			int total_qty = rows;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables[tableName].Rows[i];
				string till_number = dr["till_number"].ToString();
				invoice_number = dr["invoice_number"].ToString();
				string gift_card_code = dr["gift_card_code"].ToString();
				string used_date = dr["used_date"].ToString();
				double face_value = Program.MyDoubleParse(dr["face_value"].ToString());
				total_value += face_value;

				ListViewItem item = new ListViewItem(till_number);
				item.SubItems.Add(invoice_number);
				item.SubItems.Add(gift_card_code);
				item.SubItems.Add(face_value.ToString("c"));
				item.SubItems.Add(used_date);
				this.lvGiftCard.Items.Add(item);
			}
			if(rows == 1)
			{
				m_s = Program.BuildReceiptFromInvoice(invoice_number, false, ref m_pks, false);
				Invoice = m_s;
				this.rtbView.Text = Invoice;
				this.txtGiftCardCode.Focus();
			}
			this.totalQty.Text = total_qty.ToString();
			this.totalValue.Text = total_value.ToString("c");
		}
		private void txtKW_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Return:
					DoSearchLatipayGiftCard();
					break;
			}
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			DoSearchLatipayGiftCard();
		}
		private void DoSelectedInv()
		{
			ListView.SelectedListViewItemCollection items = this.lvGiftCard.SelectedItems;
			foreach (ListViewItem item in items)
			{
				SelectedT = int.Parse(item.SubItems[0].Text);
				SelectedInv = int.Parse(item.SubItems[1].Text);
			}
			m_s = Program.BuildReceiptFromInvoice(SelectedInv.ToString(), false, ref m_pks, false);
			Invoice = m_s;
			this.rtbView.Text = Invoice;
			this.txtGiftCardCode.Focus();
		}
		private void btnView_Click(object sender, EventArgs e)
		{
			DoSelectedInv();
			this.rtbView.Text = Invoice;
			this.txtGiftCardCode.Focus();
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
		}
		private void PrintInv()
		{
			if (Invoice == "")
				return;
			m_sPrintBuffer = Invoice;
			printDoc.Print();
			this.txtGiftCardCode.Focus();
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
		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			DoSelectedInv();
			this.rtbView.Text = Invoice;
			this.txtGiftCardCode.Focus();
		}

		private void txtKW_Click(object sender, EventArgs e)
		{
			FormPad np = new FormPad();
			np.Location = new Point(70,125);
			np.ShowDialog();
			txtGiftCardCode.Text = np.m_sChangeQty;
			txtGiftCardCode.SelectAll();
		}

		private void txtKW_Leave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}
	}
}
