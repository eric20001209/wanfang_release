using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Web.Services.Protocols;

namespace QPOS2008
{
	public partial class FormWeChat : Form
	{
		public bool m_bSuccess = false;
		public double m_dAmount = 0.01;
		public string m_sInvoiceNumber = "1666584555";
		private string m_sUri = "";
		private string m_sTradeNumber = "";
		
		public FormWeChat()
		{
			InitializeComponent();
		}
		private void btnGo_Click(object sender, EventArgs e)
		{
			GetWebSite(txtURL.Text.Trim());
		}
		private void FormWeChat_Load(object sender, EventArgs e)
		{
			m_bSuccess = false;
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
			}
			else
			{
//				int nScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
//				int nScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
				this.Location = new Point(200, 0);
			}
			GetWebSite("");
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_bSuccess = false;
			Close();
		}
		private bool GetTradeNumber()
		{
			string sRet = wb.DocumentText;
			txtDebug.Text = sRet;
			int p = sRet.IndexOf("<input type=\"hidden\" name=\"out_trade_no\"");
			if (p > 0)
			{
				int start = sRet.IndexOf("value=\"", p);
				if (start > p)
				{
					start += 7;
					int end = sRet.IndexOf("\">", start);
					if (end > start)
					{
						m_sTradeNumber = sRet.Substring(start, end - start);
						return true;
					}
				}
			}
			return false;
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			if(m_bSuccess)
			{
				this.Close();
				return;
			}
			
			if(m_sTradeNumber == "")
			{
				if(!GetTradeNumber())
					return;
			}
			
			string suri = Program.m_sWeChatUri.Replace("gposqr", "gposcheck");
			string skey = CalculateMerchantVerifier(Program.m_sWeChatMerchantID, "", m_sTradeNumber, Program.m_sWeChatSignature);
			suri += "/" + Program.m_sWeChatMerchantID + "/" + m_sTradeNumber + "?key=" + skey;
			
			Uri uri = new Uri(suri, true);
			WebClient wc = new WebClient();
//			wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
			string sRet = "";
			try
			{
				sRet = wc.DownloadString(uri);
				txtDebug.Text = sRet;
			}
			catch (Exception e1)
			{
				MessageBox.Show("open uri error: url=" + m_sUri + ", e=" + e1.ToString());
			}
			if (sRet.ToLower().IndexOf("success") >= 0)
			{
				timer1.Stop();
				m_bSuccess = true;
				timer1.Interval = 3000; //wait for 3 seconds
				timer1.Start();
			}
		}
		private void GetWebSite(string url)
		{
			System.Net.ServicePointManager.Expect100Continue = false;

			string sAmount = m_dAmount.ToString("F2");
			string skey = CalculateMerchantVerifier(Program.m_sWeChatMerchantID, sAmount, m_sInvoiceNumber, Program.m_sWeChatSignature);
			string suri = Program.m_sWeChatUri + "/" + Program.m_sWeChatMerchantID + "/" + sAmount + "/" + m_sInvoiceNumber + "?key=" + skey;
			suri = suri.Trim();
			if(url != "")
				suri = url;
			m_sUri = suri;
			txtURL.Text = suri;

//			suri = System.Web.HttpUtility.UrlEncode(suri);
			
			Uri uri = new Uri(suri, true);
/*			bool bRet = Uri.TryCreate(suri, UriKind.Absolute, out uri);
			if(!bRet)
			{
				MessageBox.Show("create URL failed, url=" + suri);
				return;
			}
*/ 
/*
			MyWebClient wc = new MyWebClient();
			wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
			string sRet = "";
			try
			{
				sRet = wc.DownloadString(uri);
			}
			catch (Exception e1)
			{
				MessageBox.Show("open uri error: url=" + suri + ", e=" + e1.ToString());
			}

			
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(suri);
			request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
			request.Credentials = CredentialCache.DefaultCredentials;
			WebResponse response;
			try
			{
				response = request.GetResponse();
			}
			catch (Exception e1)
			{
				MessageBox.Show("open uri error: url=" + suri + ", e=" + e1.ToString());
				return;
			}
	
			string sta = ((HttpWebResponse)response).StatusDescription;
			Stream dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
			reader.Close();
			response.Close();
*/			
			string v = wb.Version.ToString();
			this.Text = v;
			try
			{
				wb.Navigate(uri);
			}
			catch (System.UriFormatException e)
			{
				MessageBox.Show("error, url=" + suri + ",\r\ne=" + e.ToString());
				return;
			}
			timer1.Interval = 3000;
			timer1.Start();
		}
		public string CalculateMerchantVerifier(string accountId, string amount, string saleid,  string integrationSecretHashKey)
		{
			string concatenatedValue = accountId.Trim() + amount.Trim() + saleid.Trim() + integrationSecretHashKey.Trim();
			byte[] valueToHash = System.Text.Encoding.UTF8.GetBytes(concatenatedValue);
			SHA1 sha = new SHA1Managed();
			byte[] result = sha.ComputeHash(valueToHash);
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
			{
				sb.Append(result[i].ToString("x2"));
			}
			byte[] toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(sb.ToString());
			string sRet = Convert.ToBase64String(toEncodeAsBytes);
			sRet = UpperCaseUrlEncode(sRet);
			return sRet;
		}
		public string UpperCaseUrlEncode(string s)
		{
			char[] temp = System.Web.HttpUtility.UrlEncode(s).ToCharArray();
			for (int i = 0; i < temp.Length - 2; i++)
			{
				if (temp[i] == '%')
				{
					temp[i + 1] = char.ToUpper(temp[i + 1]);
					temp[i + 2] = char.ToUpper(temp[i + 2]);
				}
			}
			return new string(temp);
		}
		public string MyToHex( byte[] bytes)
		{
			char[] c = new char[bytes.Length * 2];

			byte b;

			for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
			{
				b = ((byte)(bytes[bx] >> 4));
				c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

				b = ((byte)(bytes[bx] & 0x0F));
				c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
			}

			return new string(c);
		}
		public string EncodeTo64(string toEncode)
		{
			byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(toEncode);
			string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
			return returnValue;
		}
		public string GetSHA1Sum(string str)
		{
			// First we need to convert the string into bytes, which
			// means using a text encoder.
			Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

			// Create a buffer large enough to hold the string
			byte[] unicodeText = new byte[str.Length * 2];
			enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

			// Now that we have a byte array we can ask the CSP to hash it
//			MD5 md5 = new MD5CryptoServiceProvider();
//			byte[] result = md5.ComputeHash(unicodeText);

			SHA1 sha = new SHA1Managed();
			byte[] result = sha.ComputeHash(unicodeText);

			// Build the final string by converting each byte
			// into hex and appending it to a StringBuilder
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
			{
				sb.Append(result[i].ToString("X2"));
			}

			// And return it
			return sb.ToString();
		}
	}
	public class MyWebClient : WebClient
	{
		Uri _responseUri;
		public Uri ResponseUri
		{
			get { return _responseUri; }
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = null;
			response = base.GetWebResponse(request);
			_responseUri = response.ResponseUri; // provide the final targetUri as a property
			return response;
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			HttpWebRequest wr = base.GetWebRequest(address) as HttpWebRequest;
			wr.ServicePoint.Expect100Continue = false; // set a specialized header
			wr.KeepAlive = false;
			return wr as WebRequest;
		}
	}
}
