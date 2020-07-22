using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Security.Authentication;

namespace QPOS2008
{
	public partial class FormDPSHIT : Form
	{
		private string m_sHost = "https://uat.paymentexpress.com/pxmi3/pos.aspx";
		private string m_sUser = "";
		private string m_sKey = "";
		private string m_sStationId = "";
		private string m_sVendorId = "";
		private string m_sTxnRef = "";
		
		public string m_sRef = "";
		public double m_dAmount = 0;
		public double m_dCashOut = 0;
		public string m_sTxnType = "Purchase";
		public string m_sCurrency = "NZD";
		public string m_sDeviceId = "1";
		public string m_sPosName = "GPOS";
		public string m_sPosVersion = "4";
		public string m_sReceipt = "";
		public bool m_bAuthorized = false;
				
		public FormDPSHIT()
		{
			InitializeComponent();
		}
		public bool GetHITConfig()
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "dpshit.ini";
			if (!File.Exists(sPath))
			{
				return false;
			}
			StreamReader sr = File.OpenText(sPath);
			string s = sr.ReadToEnd() + "\r";
			sr.Close();
			m_sHost = Program.GetValue(s, "host");
			m_sUser = Program.GetValue(s, "user");
			m_sKey = Program.GetValue(s, "key");
			m_sStationId = Program.GetValue(s, "stationid");
			m_sVendorId = Program.GetValue(s, "vendorid");
			if(m_sHost == "" || m_sUser == "" || m_sKey == "" || m_sStationId == "" || m_sVendorId == "" )
				return false;
			return true;
		}
		private void FormDPSHIT_Load(object sender, EventArgs e)
		{
			int boundWidth = Screen.PrimaryScreen.Bounds.Width;
			int boundHeight = Screen.PrimaryScreen.Bounds.Height;
			int x = boundWidth - this.Width;
			int y = boundHeight - this.Height;
			this.Location = new Point(x / 2, y / 2);

			if (!GetHITConfig())
			{
				Program.MsgBox("DPS HIT config error, please check dpshit.ini");
				return;
			}
			DoPurchase();
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			if(btnOK.Text == "Close")
				this.Close();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			CheckStatus();
		}
		private bool AnaResponse(string sRet)
		{
			string txnType = Program.GetXmlTagValue(sRet, "TxnType");
			string statusId = Program.GetXmlTagValue(sRet, "StatusId");	
			string txnStatusId = Program.GetXmlTagValue(sRet, "TxnStatusId");	
			string complete = Program.GetXmlTagValue(sRet, "Complete");
			string DL1 = Program.GetXmlTagValue(sRet, "DL1");
			string DL2 = Program.GetXmlTagValue(sRet, "DL2");
			string B1 = Program.GetXmlTagValue(sRet, "B1 en=\"1\"");
			string B2 = Program.GetXmlTagValue(sRet, "B2 en=\"1\"");
			string rcpt = Program.GetXmlTagValue(sRet, "Rcpt");
			
			txtStatus.Text = DL1;
			txtMsg.Text = DL2;
			if(B1 != "")
			{
				btnCancel.Text = B1;
				btnCancel.Visible = true;
			}
			else
			{
				btnCancel.Visible = false;
			}
			if(B2 != "")
			{
				btnOK.Text = B2;
				btnOK.Visible = true;
			}
			else
			{
				btnOK.Visible = false;
			}
			if(complete == "1")
			{
				m_bAuthorized = true;
				m_sReceipt = rcpt;
				btnCancel.Visible = false;
				btnOK.Visible = true;
				btnOK.Text = "Close";
				return true;
			}
			return false;
		}
		private bool DoPurchase()
		{
			double dAmount = m_dAmount + m_dCashOut;
			string sAmount = dAmount.ToString("N2");
			m_bAuthorized = false;
			m_sReceipt = "";
			m_sTxnRef = m_sRef;// + "_" + DateTime.Now.ToOADate().ToString();
			string s = "<?xml version=1.0 encoding=UTF-8 ?>\r\n";
//			s += "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">";
//			s += "<soapenv:Body>";
			s += "<Scr action=\"doScrHIT\" user=\"" + m_sUser + "\" key=\"" + m_sKey + "\">\r\n";
			s += "<Amount>" + sAmount + "</Amount>\r\n";
			s += "<Cur>" + m_sCurrency + "</Cur>\r\n";
			s += "<TxnType>" + m_sTxnType + "</TxnType>\r\n";
			s += "<Station>" + m_sStationId + "</Station>\r\n";
			s += "<TxnRef>" + m_sTxnRef + "</TxnRef>\r\n";
			s += "<DeviceId>" + m_sDeviceId + "</DeviceId>\r\n";
			s += "<PosName>" + m_sPosName + "</PosName>\r\n";
			s += "<PosVersion>" + m_sPosVersion + "</PosVersion>\r\n";
			s += "<VendorId>" + m_sVendorId + "</VendorId>\r\n";
			s += "<MRef>" + m_sRef + "</MRef>\r\n";	
			s += "</Scr>";
//			s += "</soapenv:Body>";
//			s += "</soapenv:Envelope>";
			
			string sRet = GetPostResponse(m_sHost, s);
			if(sRet == "")
				return false;
				
			AnaResponse(sRet);
			
			timer1.Interval = 3000;
			timer1.Start();
			return true;
		}
		private bool CheckStatus()
		{
			string s = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n";
//			s += "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">";
//			s += "<soapenv:Body>";
			s += "<Scr action=\"doScrHIT\" user=\"" + m_sUser + "\" key=\"" + m_sKey + "\">\r\n";
			s += "<Station>" + m_sStationId + "</Station>\r\n";
			s += "<TxnType>Status</TxnType>\r\n";
			s += "<TxnRef>" + m_sTxnRef + "</TxnRef>\r\n";
			s += "</Scr>";
//			s += "</soapenv:Body>";
//			s += "</soapenv:Envelope>";
			
			string sRet = GetPostResponse(m_sHost, s);
			if(sRet == "")
				return false;
			if(!AnaResponse(sRet)) //not completed
			{
				timer1.Interval = 1000;
				timer1.Start();
			}
			return true;
		}
		private string GetPostResponse(string host, string data)
		{
			string sRet = "";
			try
			{
				const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
				const SslProtocols _Tls11 = (SslProtocols)0x00000300;
				const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
				const SecurityProtocolType Tls11 = (SecurityProtocolType)_Tls11;
				ServicePointManager.SecurityProtocol = Tls12; 
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
//				req.Timeout = 30000;
				req.ContentType = "text/xml; encoding='utf-8'";
				req.Method = "POST";
				byte[] postBytes = Encoding.UTF8.GetBytes(data);
				req.ContentLength = postBytes.Length;
				Stream postStream = req.GetRequestStream();
				postStream.Write(postBytes, 0, postBytes.Length);
				postStream.Close();

				WebResponse resp = req.GetResponse();
				Stream stream = resp.GetResponseStream();
				StreamReader sr = new StreamReader(stream);
				sRet = sr.ReadToEnd();
				stream.Close();
			}
			catch (Exception e)
			{
				Program.MsgBox("post data failed, e=" + e.ToString());
				txtStatus.Text = "EFTPOS ERROR";
				txtMsg.Text = "CONTACT SYS ADMIN";
				btnOK.Text = "Close";
				btnOK.Visible = true;
				return "";
			}
			return sRet;
		}
	}
}
