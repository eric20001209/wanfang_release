using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Web;
using System.Web.UI.HtmlControls;

namespace QPOS2008
{
	public partial class FormVIP : Form
	{
		private SqlDataAdapter myAdapter;
		private SqlConnection myConnection;
		private SqlCommand myCommand;
		DataSet dst = new DataSet();

		public bool m_bPay = false;
		public double m_dPayAmount = 0;
		public string m_sInvoices = "";
		public string m_sAmounts = "";
		public string m_sVipCardId = "";
		public string m_sVipName = "";
		//vip account print
		private PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
		private PrintDocument printDocVA = new PrintDocument();
		private string documentContents;// Declare a string to hold the entire document contents. 
		private string stringToPrint;// Declare a variable to hold the portion of the document that is not printed. 		

		public FormVIP()
		{
			InitializeComponent();
		}
		private void FormVIP_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			DoLoadVIP();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void btnSearchVip_Click(object sender, EventArgs e)
		{
			DoLoadVIP();
		}
		private bool DoLoadVIP()
		{
			FormSearchVip fm = new FormSearchVip();
			fm.ShowDialog();
			m_sVipCardId = fm.m_sCardId;
			if (m_sVipCardId == "")
				return false;

			if (dst.Tables["vip"] != null)
				dst.Tables["vip"].Clear();
			int nRows = 0;
			string sc = " SELECT name, barcode, phone, email, address1, address2, address3 ";
			sc += " FROM card WHERE id = " + m_sVipCardId;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "vip");
				if (nRows <= 0)
					return false;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			DataRow dr = dst.Tables["vip"].Rows[0];
			lblVAName.Text = dr["name"].ToString();
			lblVABarcode.Text = dr["barcode"].ToString();
			m_sVipName = dr["name"].ToString();

			if (dst.Tables["vipinv"] != null)
				dst.Tables["vipinv"].Clear();

			sc = " SELECT invoice_number, total, amount_paid, CONVERT(varchar(99), commit_date, 103) AS sdate ";
			sc += " FROM invoice ";
			sc += " WHERE card_id = " + m_sVipCardId;
			sc += " AND paid = 0 ";
//			sc += " AND amount_paid < total ";
			sc += " ORDER BY commit_date ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "vipinv");
				if (nRows <= 0)
					return true;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}

			dgvVA.Rows.Clear();
			double dDue = 0;
			for (int i = 0; i < nRows; i++)
			{
				dr = dst.Tables["vipinv"].Rows[i];
				string inv = dr["invoice_number"].ToString();
				string sdate = dr["sdate"].ToString();
				double dTotal = Program.MyDoubleParse(dr["total"].ToString());
				double dPaid = Program.MyDoubleParse(dr["amount_paid"].ToString());
				double dBalance = dTotal - dPaid;
				dDue += dBalance;
				string[] aRow = { inv, sdate, dTotal.ToString("c"), dPaid.ToString("c"), dBalance.ToString("c"), "true" };
				dgvVA.Rows.Add(aRow);
			}
			lblVAAmount.Text = dDue.ToString("c");
			return true;
		}
		private void dgvVA_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == cc_check.Index)
			{
				bool bChecked = Program.MyBooleanParse(dgvVA.Rows[e.RowIndex].Cells["cc_check"].Value.ToString());
				dgvVA.Rows[e.RowIndex].Cells["cc_check"].Value = (!bChecked).ToString();
				VACalcTotal();
			}
		}
		private void VACalcTotal()
		{
			double dDue = 0;
			for (int i = 0; i < dgvVA.Rows.Count; i++)
			{
				if (!Program.MyBooleanParse(dgvVA.Rows[i].Cells["cc_check"].Value.ToString()))
					continue;
				double dBalance = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_balance"].Value.ToString());
				dDue += dBalance;
			}
			lblVAAmount.Text = dDue.ToString("c");
		}
		void printDocVA_PrintPage(object sender, PrintPageEventArgs e)
		{
			int charactersOnPage = 0;
			int linesPerPage = 0;

			// Sets the value of charactersOnPage to the number of characters  
			// of stringToPrint that will fit within the bounds of the page.
			e.Graphics.MeasureString(stringToPrint, this.Font,
				e.MarginBounds.Size, StringFormat.GenericTypographic,
				out charactersOnPage, out linesPerPage);

			// Draws the string within the bounds of the page.
			e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
			e.MarginBounds, StringFormat.GenericTypographic);

			// Remove the portion of the string that has been printed.
			stringToPrint = stringToPrint.Substring(charactersOnPage);

			// Check to see if more pages are to be printed.
			e.HasMorePages = (stringToPrint.Length > 0);

			// If there are no more pages, reset the string to be printed. 
			if (!e.HasMorePages)
				stringToPrint = documentContents;
		}
		private void btnVAPrint_Click(object sender, EventArgs e)
		{
			VABuildStatement();
			string path = Application.StartupPath + "\\statement.html";
			WebBrowser webBrowserForPrinting = new WebBrowser();
			webBrowserForPrinting.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebPrintDocument);
			webBrowserForPrinting.DocumentText = System.IO.File.ReadAllText(path);			
//			string curDir = Directory.GetCurrentDirectory();
//			webBrowserForPrinting.Url = new Uri(String.Format("file:///{0}/statement.html", curDir));
//			string uri = "file:///" + HttpUtility.UrlEncode(Application.StartupPath) + "/statement.html";
//			webBrowserForPrinting.Url = new Uri(uri);
//			printPreviewDialog1.Document = printDocument1;
//			printPreviewDialog1.ShowDialog();
		}
		private void VABuildStatement()
		{
			string header = Program.ReadSitePage("statement_header");
			string footer = Program.ReadSitePage("statement_footer");
			header = header.Replace("@@DATE", DateTime.Now.ToString("dd-MM-yyyy"));

			DataRow dr = dst.Tables["vip"].Rows[0];
			string s = "";
			s += @"
<html><head>
<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
<style type=text/css> 
body{background:#FFFFFF;font-size:10pt;font-family:verdana;}
td{FONT-WEIGHT:300;FONT-SIZE:10PT;FONT-FAMILY:vardana;background-color:inherit;}
tr{FONT-WEIGHT:300;font-size:10pt;font-family:verdana;background-color:inherit;align:left;}
.t{background-color:inherit;font-family:verdana;font-size:10pt;border-width:0px;border-style:Solid;border-collapse:collapse;fixed;}
</style></head><body>
";
			s += "<center>";
			s += header;
			s += "<table width=90% class=t border=0><tr><td>";
			s += "<table class=t border=0>";
			s += "<tr><td>To:</td><td>" + dr["name"].ToString() + "</td></tr>";
			s += "<tr><td>Barcode:</td><td>" + dr["barcode"].ToString() + "</td></tr>";
			s += "<tr><td>Address:</td><td>" + dr["address1"].ToString() + " " + dr["address2"].ToString() + " " + dr["address3"].ToString() + "</td></tr>";
			s += "<tr><td>Phone:</td><td>" + dr["phone"].ToString() + "</td></tr>";
			s += "<tr><td>Email:</td><td>" + dr["email"].ToString() + "</td></tr>";
			s += "<table>";
			s += "</td></tr>";
			s += "<tr><td><hr height=1 width=100%</td></tr>";
			s += "</table>";

			s += "<table width=90% class=t border=0><tr><th>Inv#</th><th>Date</th><th>Amount</th><th>Paid</th><th>Balance</th></tr>";
			s += "<tr><td colspan=5><hr height=1 width=100%</td></tr>";
			double dDue = 0;
			for (int i = 0; i < dgvVA.Rows.Count; i++)
			{
				if (!Program.MyBooleanParse(dgvVA.Rows[i].Cells["cc_check"].Value.ToString()))
					continue;
				string inv = dgvVA.Rows[i].Cells["cc_invoice"].Value.ToString();
				string sdate = dgvVA.Rows[i].Cells["cc_date"].Value.ToString();
				double dAmount = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_amount"].Value.ToString());
				double dPaid = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_amount_paid"].Value.ToString());
				double dBalance = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_balance"].Value.ToString());

				s += "<tr><td align=center>" + inv + "</td><td align=center>" + sdate + "</td><td align=right>" + dAmount.ToString("c") + "</td>";
				s += "<td align=right>" + dPaid.ToString("c") + "</td><td align=right>" + dBalance.ToString("c") + "</td></tr>";
				dDue += dBalance;
			}
			s += "<tr><td colspan=5><hr height=1 width=100%</td></tr>";
			s += "<tr><td colspan=4 align=right>Sub Total:</td><td align=right>" + dDue.ToString("c") + "</td></tr>";
			s += "</table>";
			s += footer.Replace("@@AMOUNTDUE", dDue.ToString("c"));
			s += "</html>";
			string path = Application.StartupPath + "\\statement.html";
			if (File.Exists(path))
				File.Delete(path);
			File.AppendAllText(path, s);
		}
		private void PrintHelpPage()
		{
		}
		private void WebPrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			((WebBrowser)sender).ShowPrintPreviewDialog();
			// Print the document now that it is fully loaded.
			//			((WebBrowser)sender).Print();
			// Dispose the WebBrowser now that the task is complete. 
			//			((WebBrowser)sender).Dispose();
		}
		private void btnVAPay_Click(object sender, EventArgs e)
		{
			m_dPayAmount = 0;
			for (int i = 0; i < dgvVA.Rows.Count; i++)
			{
				if (!Program.MyBooleanParse(dgvVA.Rows[i].Cells["cc_check"].Value.ToString()))
					continue;
				string inv = dgvVA.Rows[i].Cells["cc_invoice"].Value.ToString();
				double dBalance = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_balance"].Value.ToString());
				
				if (inv != "")
				{
					m_sInvoices += ",";
					m_sAmounts += ",";
				}
				m_sInvoices += inv;
				m_sAmounts += dBalance.ToString();
				m_dPayAmount += dBalance;
			}
			if (m_dPayAmount < 0)
			{
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible =false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = "Sorry, Balance is negtive!";
				fm.ShowDialog();
				return;
			}
//			m_dPayAmount = Program.RoundUp(m_dPayAmount);
			m_bPay = true; //bring up payment panel after close			
			this.Close();
		}
	}
}
