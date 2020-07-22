using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace QPOS2008
{
	public partial class FormAA : Form
	{
		public double m_dAmount = 0;
		public string m_sInvoiceNumber = "";

//		private SqlDataAdapter myAdapter;
		private SqlConnection myConnection;
//		private SqlCommand myCommand;
		DataSet dst = new DataSet();

		private PrintDocument printDoc = new PrintDocument();
		private string m_sPrintBuffer = "";

		public FormAA()
		{
			InitializeComponent();
		}
		private void FormAA_Load(object sender, EventArgs e)
		{
			if (Program.m_sPrinterName != "")
			{
				printDoc.PrinterSettings.PrinterName = Program.m_sPrinterName;
			}
			printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);

			if (m_dAmount > 0)
			{
				txtAmount.Text = m_dAmount.ToString();
//				txtAmount.Visible = false;
				txtCardNumber.Focus();
			}
			else
			{
				txtAmount.Focus();
			}
		}
		private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
		{
			float fSize = (float)Program.MyDoubleParse(Program.m_sFontSize);
			if (fSize == 0)
				fSize = 10;
			Font printFont = new Font(Program.m_sFontName, fSize, Program.m_tFontStyle);
			Font printFontBig = new Font(Program.m_sFontName, fSize + 6, Program.m_tFontStyle);
			Font m_PrintFont = new Font("Times New Roman", 45);
			Font m_PrintFontON = new Font("Times New Roman", 25);
			int y = 0;
			int lineHeight = 20;
			string s = "1";
			int receiptEnd = 0;
			int p = m_sPrintBuffer.IndexOf("[b]");
			int q = m_sPrintBuffer.IndexOf("==== Kitchen Copy ======");
			for (int c = 0; c < m_sPrintBuffer.Length; c++)
			{
				if (m_sPrintBuffer.Substring(c, 1) == "\r")
					receiptEnd++;
			}
			if (p < 0 && q < 0)
			{
				s = m_sPrintBuffer;
				e.Graphics.DrawString(m_sPrintBuffer, printFont, Brushes.Black, 0, y);
				return;
			}
			int pEnd = m_sPrintBuffer.IndexOf("[/b]", p + 3);

			if (p > 0)
			{
				s = m_sPrintBuffer.Substring(0, p);
				e.Graphics.DrawString(s, printFontBig, Brushes.Black, 0, y);
				y += lineHeight;
			}
			if (pEnd > 0)//big font end
			{
				s = m_sPrintBuffer.Substring(p + 3, pEnd - p - 3); //sub string in big font
				e.Graphics.DrawString(s, printFontBig, Brushes.Black, 0, y);
				y += lineHeight;
				s = m_sPrintBuffer.Substring(pEnd + 4, m_sPrintBuffer.Length - pEnd - 4); //rest of string, in regular size
				e.Graphics.DrawString(s, printFont, Brushes.Black, 0, y);
			}
			else
			{
				s = m_sPrintBuffer.Substring(pEnd + 4, m_sPrintBuffer.Length - pEnd - 4); //rest of string, in regular size
				e.Graphics.DrawString(m_sPrintBuffer, printFontBig, Brushes.Black, 0, y);
			}
			y *= receiptEnd;
		}
		private void btnSubmit_Click(object sender, EventArgs e)
		{
			string cardNumber = txtCardNumber.Text.Trim();
			if (cardNumber == "")
			{
				MessageBox.Show("Please scan AA card");
				return;
			}
			double dAmount = Double.Parse(txtAmount.Text);
			if (dAmount <= 0)
			{
				MessageBox.Show("Amount cannot be less than zero");
				return;
			}
			dAmount = Math.Round(dAmount, 2);
			if(DoAAService(cardNumber, dAmount, m_sInvoiceNumber))
			{
				this.Close();
			}
		}
		private void txtAmount_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				txtCardNumber.Focus();
		}
		private void txtCardNumber_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if(txtAmount.Text != "")
				{
					btnSubmit.Focus();
					btnSubmit_Click(null, null);
				}
				else
				{
					txtAmount.Focus();
				}
			}
		}
		private bool DoAAService(string cardNumber, double dAmount, string sInvoiceNumber)
		{
			string aaUri = "https://service.loyaltysystems.co.nz/doaccumulation";
			string partner = "";
			string terminalId = "";
			double dAmountMin = 20;
			if (Registry.CurrentUser.OpenSubKey(Program.m_sRegKey, false) != null)
			{
				aaUri = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_service_uri", ""));
				partner = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_partner", ""));
				terminalId = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_terminal_id", ""));
				dAmountMin = Program.MyDoubleParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_amount_min", "20")));
			}
			if (aaUri == "")
			{
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_service_uri", "http://servicetest.aa.loyaltysystems.co.nz/doaccumulation");
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_partner", "");
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_terminal_id", "");
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_amount_min", "20");
			}
			if (terminalId == "")
			{
				MessageBox.Show("Error, please enter AA terminal ID");
				return false;
			}

			if (dAmount < dAmountMin)
				return true;

			string sAmount = Math.Round(dAmount * 100, 0).ToString();
			string sfTransactionId = DateTime.Now.ToOADate().ToString().Replace(".", "");
			string siteDate = DateTime.Now.ToString("yyyyMMdd");
			string siteTime = DateTime.Now.ToString("HHmmss");

			String result = "";
            string card = "";
            string discount = "";
            string balance = "";
            string AAreceipt = "         [b]AA Smartfuel[/b]           \r\n";
			String strPost = "partner=" + partner + "&terminalId=" + terminalId + "&sfTransactionId=" + sfTransactionId;
			strPost += "&siteDate=" + siteDate + "&siteTime=" + siteTime + "&cardNumber=" + cardNumber;
			strPost += "&saleAmount=" + sAmount;

			if (sInvoiceNumber != "")
				strPost += AAApendItem(sInvoiceNumber);

			StreamWriter myWriter = null;
			HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(aaUri);
			objRequest.Method = "POST";
			objRequest.ContentLength = strPost.Length;
			objRequest.ContentType = "application/x-www-form-urlencoded";
			try
			{
				myWriter = new StreamWriter(objRequest.GetRequestStream());
				myWriter.Write(strPost);
			}
			catch (Exception e)
			{
				string msg = e.ToString();
				MessageBox.Show(msg);
				return false;
			}
			finally
			{
				myWriter.Close();
			}

			HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
			using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();

                Regex regcard = new Regex("<cardNumber>(.+?)</cardNumber>");
                Match rescard = regcard.Match(result);
                if (rescard.Success)
                {
                    card = rescard.Value;                              //此为匹配出的值

                    card = card.Substring(12, card.Length - 25);
                    card = "Card Number:    "+"...." + card.Substring(card.Length - 4) + "\r\n";
                }

                Regex regdis = new Regex("<discount>(.+?)</discount>");
                Match resdis = regdis.Match(result);
                if (resdis.Success)
                {
                    discount = resdis.Value;                              //此为匹配出的值

                    discount = discount.Substring(10, discount.Length - 21);
                    discount = "Discount Given:    " + discount + " cpl\r\n";
                }

                Regex regbal = new Regex("<balance>(.+?)</balance>");
                Match resbal = regbal.Match(result);
                if (resbal.Success)
                {
                    balance = resbal.Value;                              //此为匹配出的值

                    balance = balance.Substring(9, balance.Length -19);
                    balance = "Card Balance:    " + balance + " cpl\r\n";
                }
                AAreceipt += card;
                AAreceipt += discount;
                AAreceipt += balance;

            }
//			MessageBox.Show(result);
            m_sPrintBuffer = AAreceipt; // result; // AAreceipt;
			try
			{
				printDoc.Print();
			}
			catch (Exception e)
			{
				MessageBox.Show("Printer error, print AA receipt failed." + e.ToString());
			}
			return true;
		}
		string AAApendItem(string sInvoiceNumber)
		{
			int nRows = 0;
			if (dst.Tables["aaainv"] != null)
				dst.Tables["aaainv"].Clear();
			string sc = " SELECT code, quantity, commit_price FROM sales WHERE invoice_number = " + sInvoiceNumber;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "aaainv");
			}
			catch (Exception e)
			{
				MessageBox.Show("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return "";
			}
			string s = "";
			for (int i = 0; i < nRows && i < 99; i++)
			{
				DataRow dr = dst.Tables["aaainv"].Rows[i];
				string code = dr["code"].ToString();
				double dQty = Program.MyDoubleParse(dr["quantity"].ToString());
				double dPrice = Program.MyDoubleParse(dr["commit_price"].ToString());
				string sAmount = Math.Round(dPrice * dQty * 100, 0).ToString();
				string n = (i + 1).ToString("D2");
				s += "&itemCode" + n + "=" + code + "&itemAmount" + n + "=" + sAmount + "&itemQuantity" + n + "=" + dQty.ToString();
			}
			return s;
		}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}
