using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Security;
using System.Globalization;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace QPOS2008
{
    public partial class FormMyPosMatePosPay : Form
    {
		public FormMyPosMatePosPay()
        {
            InitializeComponent();
			this.ControlBox = false;
			this.Visible = false;
			this.WindowState = FormWindowState.Minimized;
		}
        public bool m_bSuccess = false;
        public double m_dAmount = 0.01;
		public string m_sInvoiceNumber = "";
        public string m_sTradeMessage = "";
		public string m_sType = "";
		public string m_sRefundPassword = "";
		public string m_sRefundReferenceNo = "";

		string merchant_id = "";
		string config_id = "";
		string reference_id = "";
		string grand_total = "";
		string random_str = "";
		string refData1 = "";
		string refData2 = "";
		string terminal_id = "";
		string merchant_account_id = Program.m_sMyPosMateMerchantAccountId;

		private void FormMyPosMatePosPay_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            m_bSuccess = false;
			//this.Size = new Size(804, 748);
			//this.Size = new Size(1024, 748);
			//this.Text = m_sPaymentType;

			//Screen[] screens = Screen.AllScreens;
			//if (screens.Length == 2)
			//{
			//    List<Screen> lstScreen = new List<Screen>();
			//    foreach (Screen screen in Screen.AllScreens)
			//    {
			//        if (screen.Primary == false)
			//            lstScreen.Add(screen);
			//    }
			//    this.Location = lstScreen[0].WorkingArea.Location;
			//    this.Width = lstScreen[0].WorkingArea.Width;
			//    this.Height = lstScreen[0].WorkingArea.Height;
			//    this.FormBorderStyle = FormBorderStyle.None; //no title, no border   
			//    txtDebug.Visible = false;
			//}
			//else
			//{
			//    this.Location = new Point(200, 0);
			//}
			if (m_sType == "Pay")
				PosPay();
			else if (m_sType == "Refund")
				PosRefund();
        }
        private void PosPay()
        {
			merchant_id = Program.m_sMyPosMateMerchantId;
			config_id = Program.m_sMyPosMateConfigId;
			reference_id = "INV" + "-" + m_sInvoiceNumber + "-" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
			grand_total = m_dAmount.ToString();
			random_str = m_sInvoiceNumber + "-" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
			refData1 = "";
			refData2 = "";
			terminal_id = "";

			string formatted_string = "config_id=" + config_id;
			formatted_string += "&grand_total=" + grand_total;
			formatted_string += "&merchant_id=" + merchant_id;
			formatted_string += "&random_str=" + random_str;
			if (refData1 != "")
				formatted_string += "&refData1=" + refData1;
			if (refData2 != "")
				formatted_string += "&refData2=" + refData2;
			formatted_string += "&reference_id=" + reference_id;
			if (terminal_id != "")
				formatted_string += "&terminal_id=" + terminal_id;
			string final_data = formatted_string + merchant_account_id;
			string signature = "";

			using (MD5 md5Hash = MD5.Create())
			{
				signature = GetMd5Hash(md5Hash, final_data);

				Console.WriteLine("The MD5 hash of " + final_data + " is: " + signature + ".");

				Console.WriteLine("Verifying the hash...");

				if (VerifyMd5Hash(md5Hash, final_data, signature))
				{
					Console.WriteLine("The hashes are the same.");
				}
				else
				{
					Console.WriteLine("The hashes are not same.");
				}
			}
			string host = Program.m_sMyPosMateUrlPosPay + "?" + formatted_string + "&signature=" + signature;
			string sRet = Http.get(host);
			if (sRet == null)
			{
				MessageBox.Show("network anomaly", "prompt");
				return;
			}
			//txtDebug.Text = sRet;
			MyPosMate.PosPayResponse pp = (MyPosMate.PosPayResponse)JsonConvert.DeserializeObject<MyPosMate.PosPayResponse>(sRet);
			//int p = 0;
			//string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
			//string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p);
			if (pp.status == true)
			{
				//if (MPMPosPay.message.ToLower() != "success")
				//if (rMessage.ToLower() != "success")
				//{
				//	MessageBox.Show("create invoice failed", "prompt");
				//	return;
				//}
				//string rInvoiceId = JsonGetSimpleValue(sRet, "\"invoice_id\":", ref p);
				//string rQRcodePic = JsonGetSimpleValue(sRet, "\"qrcode_pic\":", ref p);
				//m_sInvoiceId = ci.invoice_id;
				//m_sInvoiceId = rInvoiceId;
				//wb.DocumentText = "<html><head></head>" +
				//						"<body style='background-color:white;font-family: \"Microsoft YaHei\" ! important;width: 1024px;'><center>" +
				//							"<div style=\"position:relative;height: 500;\">" +
				//								"<br><h2><span style=\"font-weight: 300;\">Please scan the QR code below to pay</span></h2>" +
				//								"<h1><span style=\"color: green;font-size: 50px;\">$" + m_dAmount.ToString("0.00") + "</span><span style=\"font-weight: 300;font-size: 20px;\">NZD</span></h1>" +
				//								"<div style=\"position:absolute;z-index:1;left:365px;top:36%;\">" +
				//								"<img style=\"width: 300px;\" src='" + Program.m_sLatiPayQRCodeBackground + "'></div>" +
				//								"<div style=\"position:absolute;z-index:2;left:410px;top:46%;\">" +
				//								//"<img style=\"width: 210px;\" src='" + ci.qrcode_pic + "'/></div>" +
				//								"<img style=\"width: 210px;\" src='" + rQRcodePic + "'/></div>" +
				//							"</div>" +
				//							"<div><h2><span style=\"font-weight: 300; color: silver;\">*Payment with WeChat/Alipay may result in a certain service charge</span></h2></div>" +
				//						"</center></body>" +
				//					"</html>";
				timer1.Interval = 5000;
				timer1.Start();
			}
			else
			{
				MessageBox.Show("pay failed: " + pp.message, "prompt");
				return;
			}
        }
		private void PosRefund()
		{
			merchant_id = Program.m_sMyPosMateMerchantId;
			config_id = Program.m_sMyPosMateConfigId;
			reference_id = m_sRefundReferenceNo;
			string refund_amount = (-m_dAmount).ToString();
			random_str = m_sInvoiceNumber + "-" + DateTime.Now.ToString("yyyyMMdd-hhmmss");
			refData1 = "";
			refData2 = "";
			terminal_id = "";
			

			string formatted_string = "config_id=" + config_id;
			formatted_string += "&merchant_id=" + merchant_id;
			formatted_string += "&random_str=" + random_str;
			formatted_string += "&reference_id=" + reference_id;
			formatted_string += "&refund_amount=" + refund_amount;
			formatted_string += "&refund_password=" + m_sRefundPassword;
			if (terminal_id != "")
				formatted_string += "&terminal_id=" + terminal_id;
			string final_data = formatted_string + merchant_account_id;
			string signature = "";

			using (MD5 md5Hash = MD5.Create())
			{
				signature = GetMd5Hash(md5Hash, final_data);

				Console.WriteLine("The MD5 hash of " + final_data + " is: " + signature + ".");

				Console.WriteLine("Verifying the hash...");

				if (VerifyMd5Hash(md5Hash, final_data, signature))
				{
					Console.WriteLine("The hashes are the same.");
				}
				else
				{
					Console.WriteLine("The hashes are not same.");
				}
			}
			string host = Program.m_sMyPosMateUrlRefund + "?" + formatted_string + "&signature=" + signature;
			string sRet = Http.get(host);
			if (sRet == null)
			{
				MessageBox.Show("network anomaly", "prompt");
				return;
			}
			//txtDebug.Text = sRet;
			MyPosMate.PosRefundResponse pp = (MyPosMate.PosRefundResponse)JsonConvert.DeserializeObject<MyPosMate.PosRefundResponse>(sRet);
			//int p = 0;
			//string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
			//string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p);
			if (pp.status == true)
			{
				m_bSuccess = true;
				return;
			}
			else
			{
				//MessageBox.Show("refund failed: " + pp.message, "prompt");
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = "refund failed: " + pp.message;
				fm.ShowDialog();
				return;
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			GetTransactionDetails();
			if (m_bSuccess && m_sTradeMessage == "TRADE_SUCCESS" && m_sType == "Pay")
			{
				this.Close();
				return;
			}
			else if (!m_bSuccess && m_sTradeMessage == "TRADE_CLOSED" && m_sType == "Pay")
			{
				this.Close();
				return;
			}

		}
		private void GetTransactionDetails()
		{
			string formatted_string = "config_id=" + config_id;
			formatted_string += "&merchant_id=" + merchant_id;
			formatted_string += "&random_str=" + random_str;
			if (refData1 != "")
				formatted_string += "&refData1=" + refData1;
			if (refData2 != "")
				formatted_string += "&refData2=" + refData2;
			formatted_string += "&reference_id=" + reference_id;
			if (terminal_id != "")
				formatted_string += "&terminal_id=" + terminal_id;
			string final_data = formatted_string + merchant_account_id;
			string signature = "";

			using (MD5 md5Hash = MD5.Create())
			{
				signature = GetMd5Hash(md5Hash, final_data);

				Console.WriteLine("The MD5 hash of " + final_data + " is: " + signature + ".");

				Console.WriteLine("Verifying the hash...");

				if (VerifyMd5Hash(md5Hash, final_data, signature))
				{
					Console.WriteLine("The hashes are the same.");
				}
				else
				{
					Console.WriteLine("The hashes are not same.");
				}
			}

			string host = Program.m_sMyPosMateUrlGetTransactionDetails + "?" + formatted_string + "&signature=" + signature;
			string sRet = Http.get(host);
			MyPosMate.getTransactionDetailsResponse pp = (MyPosMate.getTransactionDetailsResponse)JsonConvert.DeserializeObject<MyPosMate.getTransactionDetailsResponse>(sRet);
			if(pp.status_description == null)
				m_sTradeMessage = "";
			else
				m_sTradeMessage = pp.status_description;

			if (m_sTradeMessage == "TRADE_SUCCESS")
			{
				
				if (pp.status == true)
				{
					m_bSuccess = true;
					return;
				}
			}
			if (m_sTradeMessage == "TRADE_CLOSED")
			{
				if (pp.status == true)
				{
					m_bSuccess = false;
					return;
				}
			}
			
			else
			{
				timer1.Interval = 5000; //wait for 5 seconds
				timer1.Start();
			}
		}

		private string GetMd5Hash(MD5 md5Hash, string input)
		{

			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}

		// Verify a hash against a string.
		private bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
		{
			// Hash the input.
			string hashOfInput = GetMd5Hash(md5Hash, input);

			// Create a StringComparer an compare the hashes.
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if (0 == comparer.Compare(hashOfInput, hash))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private byte[] HashHMAC(byte[] key, byte[] message)
		{
			var hash = new HMACSHA256(key);
			return hash.ComputeHash(message);
		}
		public string JsonGetNodeValue(string sj, string tag, char spLeft, char spRight, ref int q)
		{
			int p = sj.IndexOf("\"" + tag + "\"");
			if (p < 0)
				return "";
			p = sj.IndexOf(spLeft, p);
			if (p < 0)
				return "";
			q = JsonFindEnd(sj, spLeft, spRight, p);
			//DEBUG("q=", q);	
			if (q < p)
				return "";
			p++;
			string so = sj.Substring(p, q - p).Trim();
			return so;
		}
		public int JsonFindEnd(string sj, char tLeft, char tRight, int nBegin)
		{
			//DEBUG("FindEnd, sj=", sj);
			int nLeft = 0;
			for (int i = nBegin + 1; i < sj.Length; i++)
			{
				//DEBUG("sji=", sj[i].ToString());		
				if (sj[i] == tLeft)
				{
					nLeft++;
					//DEBUG("nLeft=", nLeft);
				}
				else
				{
					if (sj[i] == tRight)
					{
						//DEBUG("nLeft=", nLeft);
						//DEBUG("sj=", sj.Substring(nBegin + 1, i - nBegin - 1));
						if (nLeft == 0)
							return i; //found
						else
							nLeft--;
					}
				}
			}
			return -1;
		}
		public string JsonGetSimpleValue(string s, string tag, ref int p)
		{
			int q = s.IndexOf(tag, p);
			if (q >= p)
			{
				p = q + tag.Length;
				int h = 0;
				if (tag == "\"qrcode_pic\":")
					h = 23;
				q = s.IndexOf(',', p + h);
				if (q == -1) // not found
				{
					q = s.Length;
				}
				if (q > p)
				{
					string r = s.Substring(p, q - p);
					r = r.Trim();
					r = r.Replace("\"", "");
					r = r.Replace("}", "");
					return r;
				}
			}
			return "";
		}
		public string GetSimpleValue(string s, string tag, ref int p)
		{
			int q = s.IndexOf(tag, p);
			if (q >= p)
			{
				p = q + tag.Length;
				int h = 0;
				q = s.IndexOf('&', p + h);
				if (q == -1) // not found
				{
					q = s.Length;
				}
				if (q > p)
				{
					string r = s.Substring(p, q - p);
					r = r.Trim();
					r = r.Replace("\"", "");
					r = r.Replace("}", "");
					return r;
				}
			}
			return "";
		}
    }
}
