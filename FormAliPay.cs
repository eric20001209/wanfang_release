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

namespace QPOS2008
{
	public partial class FormAliPay : Form
	{
		public FormAliPay()
		{
			InitializeComponent();
		}
		public bool m_bSuccess = false;
		public double m_dAmount = 0.01;
		public string m_sTradeNumber = "";
		public string m_sPaymentType = "Alipay";
		public string m_sScanningMethod = "";
		public string m_sBarcode = "";
		private string m_sUri = "";
		private string m_sTransNo = "";
		private string m_sCodeUrl = "";
		private string m_sImgUrl = "";

		private void btnGo_Click(object sender, EventArgs e)
		{
			GetWebSite(txtURL.Text.Trim());
		}
		private void FormAliPay_Load(object sender, EventArgs e)
		{
			timer1.Stop();
			m_bSuccess = false;
			this.Size = new Size(804, 748);
			this.Text = m_sPaymentType;

			if (m_sScanningMethod == "Generate QR Code")
			{
				Screen[] screens = Screen.AllScreens;
				if (screens.Length == 2)
				{
					List<Screen> lstScreen = new List<Screen>();
					foreach (Screen screen in Screen.AllScreens)
					{
						if (screen.Primary == false)
							lstScreen.Add(screen);
					}
					this.Location = lstScreen[0].WorkingArea.Location;
					this.Width = lstScreen[0].WorkingArea.Width;
					this.Height = lstScreen[0].WorkingArea.Height;
					this.FormBorderStyle = FormBorderStyle.None; //no title, no border   
					txtDebug.Visible = false;
				}
				else
				{
					//				int nScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
					//				int nScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
					this.Location = new Point(200, 0);
				}
				//			GetWebSite("");
				DoPayment();
			}
			else
			{
				this.WindowState = FormWindowState.Minimized;
				DoBarcodePayment();
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			if (m_bSuccess)
			{
				this.Close();
				return;
			}

			string sRet = "";
			string host = Program.m_sAttractPayUriCheck;
			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
				req.ContentType = "application/x-www-form-urlencoded";
				req.Timeout = 30000;
				req.AllowAutoRedirect = true;
				//				req.ClientCertificates = GetCertificate(certName);
				req.Method = "POST";

				string data = "channel=" + m_sPaymentType + "&store_id=" + Program.m_sAttractPayStoreID + "&total_amount=" + m_dAmount + "&trans_no=" + m_sTransNo;
				string sod = data + "&authentication_code=" + Program.m_sAttractPayAuthCode;
				string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(sod, "md5");
				string postData = data + "&sign=" + sign;
				byte[] postBytes = Encoding.UTF8.GetBytes(postData);
				req.ContentLength = postBytes.Length;

				Stream postStream = req.GetRequestStream();
				postStream.Write(postBytes, 0, postBytes.Length);
				postStream.Flush();
				postStream.Close();

				WebResponse resp = req.GetResponse();
				Stream stream = resp.GetResponseStream();
				StreamReader sr = new StreamReader(stream, Encoding.UTF8, true);
				sRet = sr.ReadToEnd();
				txtDebug.Text = sRet;
			}
			catch (Exception e1)
			{
				MessageBox.Show(e1.ToString());
			}

			int p = 0;
			string code = JsonGetSimpleValue(sRet, "\"code\":", ref p).ToLower();
			string message = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();
			if (message == "success")
			{
				string sData = JsonGetNodeValue(sRet, "dataObject", '{', '}', ref p);
				if (sData == "")
				{
					MessageBox.Show("payment failed, blank dataObject");
					return;
				}
				if (sRet.IndexOf("transactionData") != -1)
				{
					sData = JsonGetNodeValue(sRet, "transactionData", '{', '}', ref p);
					if (sData == "")
					{
						MessageBox.Show("payment failed, blank transactionData");
						return;
					}
				}

				p = 0;
				string trans_no = JsonGetSimpleValue(sData, "\"trans_no\":", ref p).ToLower();
				double dAmount = Program.MyDoubleParse(JsonGetSimpleValue(sData, "\"amount\":", ref p));
				string status = JsonGetSimpleValue(sData, "\"status\":", ref p).ToLower();

				if (status == "s" && trans_no == m_sTransNo && dAmount >= m_dAmount)
				{
					m_bSuccess = true;
				}
				timer1.Interval = 5000; //wait for 5 seconds
				timer1.Start();
			}
			else
			{
				MessageBox.Show("payment failed");
				return;
			}
		}
		private string GetTransactionStatus(string s)
		{
			s = s.ToLower().Replace("\"", "");
			string[] sa = s.Split(',');
			for (int i = 0; i < sa.Length; i++)
			{
				string[] sb = sa[i].Split(':');
				if (sb.Length < 2)
					continue;
				if (sb[0] == "transaction_status")
					return sb[1];
			}
			return "";
		}
		private void GetWebSite(string url)
		{
			System.Net.ServicePointManager.Expect100Continue = false;

			string sAmount = m_dAmount.ToString("F2");
			string suri = Program.m_sAliPayUri + "?action=pay&merchant_id=" + Program.m_sAliPayMerchantID + "&merchant_trade_no=" + m_sTradeNumber;
			suri += "&cy=NZD&total_amount=" + sAmount + "&create_time=" + System.Web.HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			m_sUri = suri;
			txtURL.Text = suri;

			Uri uri = new Uri(suri, true);
			string v = wb.Version.ToString();
			txtDebug.Text = v;
			try
			{
				wb.Navigate(uri);
			}
			catch (System.UriFormatException e)
			{
				MessageBox.Show("error, url=" + suri + ",\r\ne=" + e.ToString());
				return;
			}
			timer1.Interval = 5000;
			timer1.Start();
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
				q = s.IndexOf(',', p);
				if (q == -1) // not found
				{
					q = s.Length;
				}
				if (q > p)
					return s.Substring(p, q - p).Trim().Replace("\"", "").Replace("}", "");
			}
			return "";
		}
		private void DoPayment()
		{
			string sRet = "";
			string host = Program.m_sAttractPayUriRequest;
			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
				req.ContentType = "application/x-www-form-urlencoded";
				req.Timeout = 30000;
				req.AllowAutoRedirect = true;
				//				req.ClientCertificates = GetCertificate(certName);
				req.Method = "POST";

				string postData = "store_id=" + Program.m_sAttractPayStoreID + "&amount=" + m_dAmount + "&currency=NZD&comment=&channel=" + m_sPaymentType;
				byte[] postBytes = Encoding.UTF8.GetBytes(postData);
				req.ContentLength = postBytes.Length;

				Stream postStream = req.GetRequestStream();
				postStream.Write(postBytes, 0, postBytes.Length);
				postStream.Flush();
				postStream.Close();

				WebResponse resp = req.GetResponse();
				Stream stream = resp.GetResponseStream();
				StreamReader sr = new StreamReader(stream, Encoding.UTF8, true);
				sRet = sr.ReadToEnd();
				txtDebug.Text = sRet;
			}
			catch (Exception e1)
			{
				MessageBox.Show(e1.ToString());
			}

			int p = 0;
			string message = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();
			if (message != "success")
			{
				MessageBox.Show("payment failed");
				return;
			}
			string sData = JsonGetNodeValue(sRet, "dataObject", '{', '}', ref p);
			if (sData == "")
			{
				MessageBox.Show("payment failed, blank dataObject");
				return;
			}
			p = 0;
			m_sTransNo = JsonGetSimpleValue(sData, "\"trans_no\":", ref p);
			m_sCodeUrl = JsonGetSimpleValue(sData, "\"code_url\":", ref p);
			m_sImgUrl = JsonGetSimpleValue(sData, "\"img_url\":", ref p);

			wb.DocumentText = "<html><head></head><body style='background-color:#000000'><center><br><h1><font color=red>" + m_sPaymentType + "</font></h1><br><br><img src='" + m_sImgUrl + "'/></body></html>";
			timer1.Interval = 5000;
			timer1.Start();
		}
		private void DoBarcodePayment()
		{
			string sRet = "";
			string host = Program.m_sAttractPayUriRequestBarcode;
			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
				req.ContentType = "application/x-www-form-urlencoded";
				req.Timeout = 30000;
				req.AllowAutoRedirect = true;
				//				req.ClientCertificates = GetCertificate(certName);
				req.Method = "POST";

				string data = "amount=" + m_dAmount + "&barcode=" + m_sBarcode + "&channel=" + m_sPaymentType + "&comment=&currency=NZD&store_id=" + Program.m_sAttractPayStoreID;
				string sod = data + "&authentication_code=" + Program.m_sAttractPayAuthCode;
				string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(sod, "md5");
				string postData = data + "&sign=" + sign.ToLower();
				byte[] postBytes = Encoding.UTF8.GetBytes(postData);
				req.ContentLength = postBytes.Length;

				Stream postStream = req.GetRequestStream();
				postStream.Write(postBytes, 0, postBytes.Length);
				postStream.Flush();
				postStream.Close();

				WebResponse resp = req.GetResponse();
				Stream stream = resp.GetResponseStream();
				StreamReader sr = new StreamReader(stream, Encoding.UTF8, true);
				sRet = sr.ReadToEnd();
				txtDebug.Text = sRet;
			}
			catch (Exception e1)
			{
				MessageBox.Show(e1.ToString());
			}

			int p = 0;
			string code = JsonGetSimpleValue(sRet, "\"code\":", ref p).ToLower();
			string message = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();
			m_sTransNo = JsonGetSimpleValue(sRet, "\"dataObject\":", ref p).ToLower();
			if (code != "202" && code != "200")
			{
				MessageBox.Show("payment failed: " + message);
				return;
			}
			//string sData = JsonGetNodeValue(sRet, "dataObject", '{', '}', ref p);
			//if (sData == "")
			//{
			//	MessageBox.Show("payment failed, blank dataObject");
			//	return;
			//}
			//p = 0;
			//m_sTransNo = JsonGetSimpleValue(sData, "\"trans_no\":", ref p);
			//m_sCodeUrl = JsonGetSimpleValue(sData, "\"code_url\":", ref p);
			//m_sImgUrl = JsonGetSimpleValue(sData, "\"img_url\":", ref p);

			//wb.DocumentText = "<html><head></head><body style='background-color:#000000'><center><br><h1><font color=red>" + message + "</font></h1></body></html>";
			timer1.Interval = 5000;
			timer1.Start();
		}
	}
}
