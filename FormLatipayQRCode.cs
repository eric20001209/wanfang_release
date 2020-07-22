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
//using Newtonsoft.Json;

namespace QPOS2008
{
    public partial class FormLatipayQRCode : Form
    {
		public FormLatipayQRCode()
        {
            InitializeComponent();
			this.ControlBox = false;
        }
        public bool m_bSuccess = false;
        public double m_dAmount = 0.01;
		//public string m_sStatus = "";
		public string m_sInvoiceNumber = "";
        public string m_sPaymentType = "Latipay";
		private string m_sUserId = Program.m_sLatiPayUserID;
		private string m_sApiKey = Program.m_sLatiPayApiKey;
		private string m_sWalletId = Program.m_sLatiPayWalletID;
		private string m_sInvoiceId = "";
		private string m_sCustomerOrderId = "";

		private void FormLatipayQRCode_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            m_bSuccess = false;
            //this.Size = new Size(804, 748);
			this.Size = new Size(1024, 748);
            this.Text = m_sPaymentType;

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
                this.Location = new Point(200, 0);
            }
			CreateInvoice();
        }
        private void CreateInvoice()
        {
			string host = Program.m_sLatiPayUrlInvoice;
			string userId = m_sUserId;
			string walletId = m_sWalletId;
			string amount = m_dAmount.ToString();
			string productName = "";
			string periodTime = "";
			string maxOpenCount = "";
			string app = "1";
			string customerOrderId = DateTime.Now.ToString("yyyyMMdd-hhmmss") + "-" + m_sInvoiceNumber;
			string customerReference = "";
			string returnUrl = "";
			string notifyUrl = "";
			string qrCode = "true";

			//Rearrange all parameters alphabetically (except parameters with value of null or empty string) and join them with &, and concat the value of api_key in the end.
			string message = "amount=" + amount;
			if (app != "")
				message += "&app=" + app;
			if (customerOrderId != "")
			{
				message += "&customer_order_id=" + customerOrderId;
				m_sCustomerOrderId = customerOrderId;
			}
			if (customerReference != "")
				message += "&customer_reference=" + customerReference;
			if (maxOpenCount != "")
				message += "&max_open_count=" + maxOpenCount;
			if (notifyUrl != "")
				message += "&notify_url=" + notifyUrl;
			if (periodTime != "")
				message += "&period_time=" + periodTime;
			if (productName != "")
				message += "&product_name=" + productName;
			message += "&qrcode=" + qrCode;
			if (returnUrl != "")
				message += "&return_url=" + returnUrl;
			if (userId != "")
				message += "&user_id=" + userId;
			if (walletId != "")
				message += "&wallet_id=" + walletId;

			string apiKey = m_sApiKey;
			byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
			byte[] secret = Encoding.UTF8.GetBytes(apiKey);
			byte[] SHA256HMACSignature = HashHMAC(secret, msg);
			string signature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower();

			string json = "{\"user_id\": \"" + userId + "\",";
			json += "\"wallet_id\": \"" + walletId + "\",";
			json += "\"amount\": \"" + amount + "\",";
			json += "\"product_name\": \"" + productName + "\",";
			json += "\"period_time\": \"" + periodTime + "\",";
			json += "\"max_open_count\": \"" + maxOpenCount + "\",";
			json += "\"app\": \"" + app + "\",";
			json += "\"customer_order_id\": \"" + customerOrderId + "\",";
			json += "\"customer_reference\": \"" + customerReference + "\",";
			json += "\"return_url\": \"" + returnUrl + "\",";
			json += "\"notify_url\": \"" + notifyUrl + "\",";
			json += "\"qrcode\": \"" + qrCode + "\",";
			json += "\"signature\": \"" + signature + "\"}";

			string sRet = Http.post(host, json);
			if (sRet == null)
			{
				MessageBox.Show("network anomaly", "prompt");
				return;
			}
			txtDebug.Text = sRet;
			//Latipay.CreateInvoice ci = (Latipay.CreateInvoice)JsonConvert.DeserializeObject<Latipay.CreateInvoice>(sRet);
			int p = 0;
			string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
			string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p);
			//if (ci.code == 0)
			if (rCode == "0")
			{
				//if (ci.message.ToLower() != "success")
				if (rMessage.ToLower() != "success")
				{
					MessageBox.Show("create invoice failed", "prompt");
					return;
				}
				string rInvoiceId = JsonGetSimpleValue(sRet, "\"invoice_id\":", ref p);
				string rQRcodePic = JsonGetSimpleValue(sRet, "\"qrcode_pic\":", ref p);
				//m_sInvoiceId = ci.invoice_id;
				m_sInvoiceId = rInvoiceId;
				wb.DocumentText = "<html><head></head>" +
										"<body style='background-color:white;font-family: \"Microsoft YaHei\" ! important;width: 1024px;'><center>" +
											"<div style=\"position:relative;height: 500;\">" +
												"<br><h2><span style=\"font-weight: 300;\">Please scan the QR code below to pay</span></h2>" +
												"<h1><span style=\"color: green;font-size: 50px;\">$" + m_dAmount.ToString("0.00") + "</span><span style=\"font-weight: 300;font-size: 20px;\">NZD</span></h1>" +
												"<div style=\"position:absolute;z-index:1;left:365px;top:36%;\">" +
												"<img style=\"width: 300px;\" src='" + Program.m_sLatiPayQRCodeBackground + "'></div>" +
												"<div style=\"position:absolute;z-index:2;left:410px;top:46%;\">" +
												//"<img style=\"width: 210px;\" src='" + ci.qrcode_pic + "'/></div>" +
												"<img style=\"width: 210px;\" src='" + rQRcodePic + "'/></div>" +
											"</div>" +
											"<div><h2><span style=\"font-weight: 300; color: silver;\">*Payment with WeChat/Alipay may result in a certain service charge</span></h2></div>" +
										"</center></body>" +
									"</html>";
				timer1.Interval = 5000;
				timer1.Start();
			}
			else
			{
				//MessageBox.Show("create invoice failed: " + ci.message, "prompt");
				MessageBox.Show("create invoice failed: " + rMessage, "prompt");
				return;
			}
        }
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			QueryAnInvoice();
			if (m_bSuccess)
			{
				this.Close();
				return;
			}
		}
		private void QueryAnInvoice()
		{
			string apiKey = m_sApiKey;
			string userId = m_sUserId;
			string message = "";
			if (m_sInvoiceId != "")
				message += "invoice_id=" + m_sInvoiceId;
			else if (m_sCustomerOrderId != "")
				message += "customer_order_id=" + m_sCustomerOrderId;
			else
			{
				MessageBox.Show("invoice_id or customer_order_id blank", "prompt");
				return;
			}
			message += "&user_id=" + userId;

			byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
			byte[] secret = Encoding.UTF8.GetBytes(apiKey);
			byte[] SHA256HMACSignature = HashHMAC(secret, msg);
			string signature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower(); ;

			string host = Program.m_sLatiPayUrlInvoice + "?" + message + "&signature=" + signature;
			string sRet = Http.get(host);
			//Latipay.QueryAnInvoice qai = (Latipay.QueryAnInvoice)JsonConvert.DeserializeObject<Latipay.QueryAnInvoice>(sRet);
			int p = 0;
			string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
			string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p);
			string rReturnQuery = JsonGetSimpleValue(sRet, "\"return_query\":", ref p);

			//if (qai.code == 0)
			if (rCode == "0")
			{
				
				//if (qai.message.ToLower() != "success")
				if (rMessage.ToLower() != "success")
				{
					MessageBox.Show("query an invoice failed", "prompt");
					return;
				}
				//m_sStatus = qai.status.ToLower();
				p = 0;
				//qai.return_query = "merchant_reference=&payment_method=wechat&status=paid&currency=NZD&amount=1.00&signature=2a0fc700e00ab989e5671196833635f961d6a8678fbf77ca15d83db9f921dc80";
				string rStatus = "";
				//if (qai.return_query != null)
				//	rStatus = GetSimpleValue(qai.return_query, "status=", ref p);
				if (rReturnQuery != null)
					rStatus = GetSimpleValue(rReturnQuery, "status=", ref p);
				if (rStatus == "paid")
				{
					m_bSuccess = true;
					return;
				}
				timer1.Interval = 5000; //wait for 5 seconds
				timer1.Start();
			}
			else
			{
				//MessageBox.Show("query an invoice failed: " + qai.message);
				MessageBox.Show("query an invoice failed: " + rMessage);
				return;
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
		//private void TransactionInterface()
		//{
		//    string sRet = "";
		//    string host = "https://api.latipay.net/v2/transaction";

		//    string walletId = "W000005482";
		//    string userId = m_sUserId;
		//    string amount = m_dAmount.ToString();
		//    string merchantReference = m_sInvoiceNumber;
		//    string paymentMethod = "alipay";//m_sPaymentType;
		//    string returnUrl = "1";
		//    string callbackUrl = "1";
		//    string ip = "127.0.0.1";
		//    string version = "2.0";
		//    string productName = "hello";
		//    string hostType = "1";
		//    string apiKey = m_sApiKey;

		//    //Rearrange all parameters alphabetically (except parameters with value of null or empty string) and join them with &, and concat the value of api_key in the end.
		//    string message = "amount=" + amount;
		//    if (callbackUrl != "")
		//        message += "&callback_url=" + callbackUrl;
		//    if (hostType != "")
		//        message += "&host_type=" + hostType;
		//    if (ip != "")
		//        message += "&ip=" + ip;
		//    if (merchantReference != "")
		//        message += "&merchant_reference=" + merchantReference;
		//    if (paymentMethod != "")
		//        message += "&payment_method=" + paymentMethod;
		//    if (productName != "")
		//        message += "&product_name=" + productName;
		//    if (returnUrl != "")
		//        message += "&return_url=" + returnUrl;
		//    if (userId != "")
		//        message += "&user_id=" + userId;
		//    if (version != "")
		//        message += "&version=" + version;
		//    if (walletId != "")
		//        message += "&wallet_id=" + walletId;

		//    byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
		//    byte[] secret = Encoding.UTF8.GetBytes(apiKey);
		//    byte[] SHA256HMACSignature = HashHMAC(secret, msg);
		//    string signature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower(); ;

		//    try
		//    {
		//        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
		//        req.ContentType = "application/json; charset=UTF-8";
		//        req.Timeout = 30000;
		//        req.AllowAutoRedirect = true;
		//        req.Method = "POST";

		//        using (StreamWriter streamWriter = new StreamWriter(req.GetRequestStream()))
		//        {
		//            string json = "{\"wallet_id\": \"" + walletId + "\"," +
		//                          "\"amount\": \"" + amount + "\"," +
		//                          "\"user_id\": \"" + userId + "\"," +
		//                          "\"merchant_reference\": \"" + merchantReference + "\"," +
		//                          "\"return_url\": \"" + returnUrl + "\"," +
		//                          "\"callback_url\": \"" + callbackUrl + "\"," +
		//                          "\"ip\": \"" + ip + "\"," +
		//                          "\"version\": \"" + version + "\"," +
		//                          "\"product_name\": \"" + productName + "\"," +
		//                          "\"payment_method\": \"" + paymentMethod + "\"," +
		//                          "\"host_type\": \"" + hostType + "\"," +
		//                          "\"signature\": \"" + signature + "\"}";
		//            streamWriter.Write(json);
		//        }

		//        HttpWebResponse httpResponse = (HttpWebResponse)req.GetResponse();
		//        using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
		//        {
		//            sRet = streamReader.ReadToEnd();
		//            txtDebug.Text = sRet;
		//        }
		//    }
		//    catch (Exception e1)
		//    {
		//        MessageBox.Show(e1.ToString());
		//    }

		//    int p = 0;
		//    string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
		//    string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();
		//    string rNonce = JsonGetSimpleValue(sRet, "\"nonce\":", ref p);
		//    string rHostUrl = JsonGetSimpleValue(sRet, "\"host_url\":", ref p);
		//    string rSignature = JsonGetSimpleValue(sRet, "\"signature\":", ref p);
		//    if (rMessage != "success")
		//    {
		//        MessageBox.Show("transaction interface failed");
		//        return;
		//    }
		//    byte[] rMsg = Encoding.UTF8.GetBytes(rNonce + rHostUrl);
		//    byte[] rSHA256HMACSignature = HashHMAC(secret, rMsg);
		//    string cSignature = BitConverter.ToString(rSHA256HMACSignature).Replace("-", "").ToLower();
		//    if (cSignature != rSignature)
		//    {
		//        MessageBox.Show("transaction interface signature failed");
		//        return;
		//    }
		//    PaymentInterface(rHostUrl, rNonce);
		//}
		
		//private void PaymentInterface(string hostUrl, string nonce)
		//{
		//    string sRet = "";
		//    string host = hostUrl + "/" + nonce;
		//    try
		//    {
		//        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
		//        req.Timeout = 30000;
		//        req.AllowAutoRedirect = true;
		//        req.Method = "GET";

		//        WebResponse resp = req.GetResponse();
		//        Stream stream = resp.GetResponseStream();
		//        StreamReader sr = new StreamReader(stream, Encoding.UTF8, true);
		//        sRet = sr.ReadToEnd();
		//        txtDebug.Text = sRet;
		//    }
		//    catch (Exception e1)
		//    {
		//        MessageBox.Show(e1.ToString());
		//    }

		//    int p = 0;
		//    string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
		//    string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();
		//    if (rMessage != "success")
		//    {
		//        MessageBox.Show("payment interface failed");
		//        return;
		//    }
		//    string sData = JsonGetNodeValue(sRet, "data", '{', '}', ref p);
		//    if (sData == "")
		//    {
		//        MessageBox.Show("payment interface failed, blank data Object");
		//        return;
		//    }

		//    p = 0;
		//    string rOrderId = JsonGetSimpleValue(sData, "\"order_id\":", ref p);
		//    string rNonce = JsonGetSimpleValue(sData, "\"nonce\":", ref p);
		//    string rPaymentMethod = JsonGetSimpleValue(sData, "\"payment_method\":", ref p);
		//    string rAmount = JsonGetSimpleValue(sData, "\"amount\":", ref p);
		//    string rAmount_cny = JsonGetSimpleValue(sData, "\"amount_cny\":", ref p);
		//    string rCurrency = JsonGetSimpleValue(sData, "\"currency\":", ref p);
		//    string rProductName = JsonGetSimpleValue(sData, "\"product_name\":", ref p);
		//    string rOrganisationId = JsonGetSimpleValue(sData, "\"organisation_id\":", ref p);
		//    string rOrganisationName = JsonGetSimpleValue(sData, "\"organisation_name\":", ref p);
		//    string rUserId = JsonGetSimpleValue(sData, "\"user_id\":", ref p);
		//    string rUserName = JsonGetSimpleValue(sData, "\"user_name\":", ref p);
		//    string rWalletId = JsonGetSimpleValue(sData, "\"wallet_id\":", ref p);
		//    string rWalletName = JsonGetSimpleValue(sData, "\"wallet_name\":", ref p);
		//    string rQRCode = JsonGetSimpleValue(sData, "\"qr_code\":", ref p);
		//    string rQRCodeUrl = JsonGetSimpleValue(sData, "\"qr_code_url\":", ref p);
		//    string rCurrencyRate = JsonGetSimpleValue(sData, "\"currency_rate\":", ref p);
		//    string rMerchantReference = JsonGetSimpleValue(sData, "\"merchant_reference\":", ref p);
		//    string rSignature = JsonGetSimpleValue(sData, "\"signature\":", ref p);

		//    string message = "amount=" + rAmount;
		//    message += "&amount_cny=" + rAmount_cny;
		//    message += "&currency=" + rCurrency;
		//    message += "&currency_rate=" + rCurrencyRate;
		//    message += "&merchant_reference=" + rMerchantReference;
		//    message += "&nonce=" + rNonce;
		//    message += "&order_id=" + rOrderId;
		//    message += "&organisation_id=" + rOrganisationId;
		//    message += "&organisation_name=" + rOrganisationName;
		//    message += "&payment_method=" + rPaymentMethod;
		//    message += "&product_name=" + rProductName;
		//    message += "&qr_code=" + rQRCode;
		//    message += "&qr_code_url=" + rQRCodeUrl;
		//    message += "&user_id=" + rUserId;
		//    message += "&user_name=" + rUserName;
		//    message += "&wallet_id=" + rWalletId;
		//    message += "&wallet_name=" + rWalletName;

		//    string apiKey = m_sApiKey;
		//    byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
		//    byte[] secret = Encoding.UTF8.GetBytes(apiKey);
		//    byte[] rSHA256HMACSignature = HashHMAC(secret, msg);
		//    string cSignature = BitConverter.ToString(rSHA256HMACSignature).Replace("-", "").ToLower();
		//    if (cSignature != rSignature)
		//    {
		//        MessageBox.Show("payment interface signature failed");
		//        return;
		//    }

		//    wb.DocumentText = "<html><head></head><body style='background-color:#000000'><center><br><h1><font color=red>" + m_sPaymentType + "</font></h1><br><br><img src='" + rQRCodeUrl + "'/></body></html>";
		//    timer1.Interval = 5000;
		//    timer1.Start();
		//}
		//private void timer1_Tick(object sender, EventArgs e)
		//{
		//    //Payment Result Interface
		//    timer1.Stop();
		//    if (m_bSuccess)
		//    {
		//        this.Close();
		//        return;
		//    }

		//    string sRet = "";
		//    string userId = m_sUserId;
		//    string merchantReference = m_sInvoiceNumber;
		//    string message = "merchant_reference=" + merchantReference + "&user_id=" + userId;
		//    string apiKey = m_sApiKey;
		//    byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
		//    byte[] secret = Encoding.UTF8.GetBytes(apiKey);
		//    byte[] SHA256HMACSignature = HashHMAC(secret, msg);
		//    string signature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower();
		//    string host = "https://api.latipay.net/v2/transaction/" + m_sInvoiceNumber + "?user_id=" + userId + "&signature=" + signature;

		//    try
		//    {
		//        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
		//        req.Timeout = 30000;
		//        req.AllowAutoRedirect = true;
		//        req.Method = "GET";

		//        WebResponse resp = req.GetResponse();
		//        Stream stream = resp.GetResponseStream();
		//        StreamReader sr = new StreamReader(stream, Encoding.UTF8, true);
		//        sRet = sr.ReadToEnd();
		//        txtDebug.Text = sRet;
		//    }
		//    catch (Exception e1)
		//    {
		//        MessageBox.Show(e1.ToString());
		//    }

		//    int p = 0;
		//    string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
		//    string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p);
		//    string rMessageCN = JsonGetSimpleValue(sRet, "\"messageCN\":", ref p);
		//    string rMerchantReference = JsonGetSimpleValue(sRet, "\"merchant_reference\":", ref p);
		//    string rStatus = JsonGetSimpleValue(sRet, "\"status\":", ref p);
		//    string rCurrency = JsonGetSimpleValue(sRet, "\"currency\":", ref p);
		//    string rAmount = JsonGetSimpleValue(sRet, "\"amount\":", ref p);
		//    string rSignature = JsonGetSimpleValue(sRet, "\"signature\":", ref p);
		//    string rPaymentMethod = JsonGetSimpleValue(sRet, "\"payment_method\":", ref p);
		//    string rTransactionId = JsonGetSimpleValue(sRet, "\"transaction_id\":", ref p);
		//    string rOrderId = JsonGetSimpleValue(sRet, "\"order_id\":", ref p);
		//    string rPayTime = JsonGetSimpleValue(sRet, "\"pay_time\":", ref p);


		//    //message = "merchant_referencer=" + rMerchantReference + "&payment_method=" + rPaymentMethod + "&status=" + rStatus + "&currency=" + rCurrency + "&amount=" + rAmount;
		//    message = rMerchantReference + rPaymentMethod + rStatus + rCurrency + rAmount;
		//    msg = Encoding.UTF8.GetBytes(message);
		//    secret = Encoding.UTF8.GetBytes(apiKey);
		//    byte[] rSHA256HMACSignature = HashHMAC(secret, msg);
		//    string cSignature = BitConverter.ToString(rSHA256HMACSignature).Replace("-", "").ToLower();

		//    if (cSignature != rSignature)
		//    {
		//        MessageBox.Show("Payment Result Interface signature failed");
		//        return;
		//    }

		//    if (rStatus == "paid")
		//    {
		//        if (Program.MyDoubleParse(rAmount) >= m_dAmount)
		//        {
		//            m_bSuccess = true;
		//            return;
		//        }
		//        timer1.Interval = 5000; //wait for 5 seconds
		//        timer1.Start();
		//    }
		//    else
		//    {
		//        MessageBox.Show("payment status: " + rStatus);
		//        return;
		//    }
		//}
    }
}
