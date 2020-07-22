using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PCEFTIPInterface;

namespace QPOS2008
{
	class CNitro
	{
		private EFTClientIP nitroClientIP;
		public string m_sResponseCode = "";
		public string m_sResponseText1 = "";
		public string m_sResponseText1Old = "";
		public string m_sResponseText2 = "";
		public bool m_bError = false;
		public bool m_bSocketFailed = false;
		public string m_sReceipt = "";
		public string m_sReceiptCust = "";
		public bool m_bPrint = false;

		private int m_nPrintCount = 1;

		public CNitro()
		{
			nitroClientIP = new EFTClientIP();
			nitroClientIP.Application = TerminalApplication.EFTPOS;
			nitroClientIP.HostName = "127.0.0.1";
			nitroClientIP.HostPort = 2011;//52076;
			//			nitroClientIP.UseSSL = false;

			nitroClientIP.OnSocketFail += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnSocketFail);
			nitroClientIP.OnDisplay += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnDisplay);
			nitroClientIP.OnDisplayControlPanel += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnDisplayControlPanel);
			nitroClientIP.OnLogon += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnLogon);
			nitroClientIP.OnSettlement += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnSettlement);
			nitroClientIP.OnReceipt += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnReceipt);
			nitroClientIP.OnTerminated += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnTerminated);
			nitroClientIP.OnTransaction += new EFTClientIP.EFTClientIPEventHandler(nitroClientIP_OnTransaction);
		}
		void nitroClientIP_OnSocketFail(object sender, EFTClientIPEventArgs e)
		{
			m_bSocketFailed = true;
			//			Program.MsgBox("PINpad Offline\r\nCheck PINPad and Cable\r\nat POS\r\n");
			//			MessageBox.Show("PINpad Offline\r\nCheck PINPad and Cable\r\nat POS\r\n", "EFTPOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//			MessageBox.Show("Socket Failed, e=" + e.ErrorMessage, "NITRO IP Interface", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//			nitroClientIP.Disconnect();
		}
		void nitroClientIP_OnDisplay(object sender, EFTClientIPEventArgs e)
		{
			//			m_sResponseText1 = e.Display.DisplayText[0].Trim();
			//			m_sResponseText2 = e.Display.DisplayText[1].Trim();
			//			Program.g_log.Info("Nitro OnDisplay, text1=" + m_sResponseText1 + ", text2=" + m_sResponseText2);
			/*			string msg = m_sResponseText1 + "\r\n" + m_sResponseText2;
						if (m_sResponseCode == "")
						{
							if(e.Display.OKKeyFlag || e.Display.AcceptYesKeyFlag || e.Display.DeclineNoKeyFlag)
								m_bError = true;
						}
			//			MessageBox.Show(msg, "NITRO IP Interface -- OnDisplay", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			*/
		}
		void nitroClientIP_OnDisplayControlPanel(object sender, EFTClientIPEventArgs e)
		{
			if (e.EFTTransaction == null)
				return;
			bool bRet = e.EFTTransaction.SuccessResponse.Success; // 0 = Fail 1= Success
			string msg = e.EFTTransaction.SuccessResponse.ResponseCode;
			msg += "\r\n" + e.EFTTransaction.SuccessResponse.ResponseText;
			//			MessageBox.Show(msg, "NITRO IP Interface -- OnDisplayControlPanel", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		void nitroClientIP_OnLogon(object sender, EFTClientIPEventArgs e)
		{
			string msg = "Response code=" + e.EFTTransaction.SuccessResponse.ResponseCode;
			msg += ", text=" + e.EFTTransaction.SuccessResponse.ResponseText;
			Program.g_log.Info("Nitro OnLogon " + msg);
			m_sReceipt = e.EFTTransaction.SuccessResponse.ResponseText;
			m_bPrint = true;
		}
		void nitroClientIP_OnSettlement(object sender, EFTClientIPEventArgs e)
		{
			//			string msg = e.EFTTransaction.SuccessResponse.ResponseCode;
			//			msg += "\r\n" + e.EFTTransaction.SuccessResponse.ResponseText;
			//			MessageBox.Show(msg, "NITRO IP Interface -- OnSettlement", MessageBoxButtons.OK, MessageBoxIcon.Error);
			string msg = "Response code=" + e.EFTTransaction.SuccessResponse.ResponseCode;
			msg += ", text=" + e.EFTTransaction.SuccessResponse.ResponseText;
			Program.g_log.Info("Nitro OnSettlement " + msg);
			m_sReceipt = e.EFTTransaction.SuccessResponse.ResponseText;
			m_bPrint = true;
		}
		void nitroClientIP_OnReceipt(object sender, EFTClientIPEventArgs e)
		{
			string text = "";
			for (int i = 0; i < e.Receipt.ReceiptText.Length; i++)
			{
				text += e.Receipt.ReceiptText[i].Trim() + "\r\n";
			}
			bool bLogon = false;
			if (text.ToLower().IndexOf("logon") >= 0)
				bLogon = true;
			//			bool bPrintByEftpos = nitroClientIP.ReceiptAutoPrint;
			//			Program.g_log.Info("Nitro ReceiptAutoPrint = " + bPrintByEftpos.ToString());
			//			if (bPrintByEftpos && !bLogon)
			//				return;
			if ((m_nPrintCount <= 0 || text.ToLower().IndexOf("customer copy") >= 0) && text.ToLower().IndexOf("declined") < 0)
			{
				m_sReceiptCust = text;
				m_bPrint = false;
				Program.g_log.Info("Nitro OnReceipt(skipped), nPrintCount=" + m_nPrintCount.ToString() + ", bPrint=" + m_bPrint.ToString() + ", text = " + text);
				return;
			}
			m_sReceipt = text;
			m_bPrint = true;
			m_nPrintCount--;
			Program.g_log.Info("Nitro OnReceipt, nPrintCount=" + m_nPrintCount.ToString() + ", bPrint=" + m_bPrint.ToString() + ", text = " + text);
			//			MessageBox.Show(text);
		}
		void nitroClientIP_OnTerminated(object sender, EFTClientIPEventArgs e)
		{
			//			MessageBox.Show("IP Connect to client was terminated", "NITRO IP Interface -- OnTerminated", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			Program.g_log.Info("Nitro teminated");
		}
		void nitroClientIP_OnTransaction(object sender, EFTClientIPEventArgs e)
		{
			m_sResponseCode = e.EFTTransaction.SuccessResponse.ResponseCode;
			m_sResponseText1 = e.EFTTransaction.SuccessResponse.ResponseText.Trim();
			if (m_sResponseCode != "00" && m_sResponseCode != "08")
				m_bError = true;
			if (m_sResponseCode == "08")
				m_sResponseText1 = "ACCEPTED"; //signature accepted
			//			MessageBox.Show(msg, "NITRO IP Interface OnTrasaction", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Program.g_log.Info("Nitro OnTransaction response, code = " + m_sResponseCode + ", text = " + m_sResponseText1);
		}
		public bool Connect()
		{
			m_sResponseText1 = "";
			m_sResponseText1Old = "";
			m_bError = false;
			bool bRet = false;
			bRet = nitroClientIP.Connect();
			if (!bRet)
			{
				MessageBox.Show("Connect to the Nitro Client IP interface failed.", "NITRO IP Interface", MessageBoxButtons.OK, MessageBoxIcon.Error);
				m_bSocketFailed = true;
				return false;
			}
			/*			if(!nitroClientIP.DoLogon())
						{
							MessageBox.Show("Logon failed to the Nitro Client IP interface failed.", "NITRO IP Interface", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}
			*/
			m_bSocketFailed = false;
			return true;
		}
		public void Disconnect()
		{
			try
			{
				nitroClientIP.Disconnect();
			}
			catch (Exception e)
			{
				MessageBox.Show("Disconnect. e=" + e.ToString(), "NITRO IP Interface", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			m_bSocketFailed = false;
		}
		public void DisplayControlPanel()
		{
			ControlPanelRequest cpr = new ControlPanelRequest();
			cpr.ControlPanelType = ControlPanelType.Full;
			bool bRet = nitroClientIP.DoDisplayControlPanel(cpr);
			if (!bRet)
			{
				MessageBox.Show("Display control panel failed.");
			}
		}
		public string CheckResponse()
		{
			/*			if(m_sResponseText1 == m_sResponseText1Old || m_sResponseText1Old == "")
						{
							m_sResponseText1Old = m_sResponseText1;
							return "";
						}
						m_sResponseText1Old = m_sResponseText1;
			 */
			return m_sResponseText1;

			string sRet = "";
			if (m_sResponseCode == "00")
				sRet = "ACCEPTED";
			else if (m_sResponseCode == "08")
				sRet = "ACCEPTED";
			return sRet;
		}
		public bool Purchase(double dPurchase, double dCashout, double dCheque, string sRef)
		{
			Program.g_log.Info("Nitro purchase start, amount = " + dPurchase.ToString("c") + ", cashout = " + dCashout.ToString("c"));
			m_nPrintCount = 1;
			m_sResponseCode = "";
			m_sResponseText1 = "";
			m_sResponseText2 = "";
			m_bError = false;
			m_sReceipt = "";
			m_sReceiptCust = "";

			EFTTransactionRequest nitroRequest = new EFTTransactionRequest();
			//			nitroRequest.TrainingMode = true;
			nitroRequest.AmountPurchase = (decimal)dPurchase;
			nitroRequest.AmountCash = (decimal)dCashout;
			nitroRequest.CardPANSource = PANSource.Default;
			nitroRequest.ReferenceNumber = sRef;
			nitroRequest.Type = TransactionType.PurchaseCash;

			if (!nitroClientIP.DoTransaction(nitroRequest))
			{
				MessageBox.Show("DoTransaction send to the Nitro Client IP interface failed.", "NITRO IP Interface", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			m_bPrint = false;
			Program.g_log.Info("Nitro purchase done, amount = " + dPurchase.ToString("c") + ", cashout = " + dCashout.ToString("c"));
			return true;
		}
		public bool Refund(double dPurchase, string sRef)
		{
			Program.g_log.Info("Nitro refund start, amount = " + dPurchase.ToString("c"));
			m_nPrintCount = 1;
			m_sResponseCode = "";
			m_sResponseText1 = "";
			m_sResponseText2 = "";
			m_bError = false;
			m_sReceipt = "";
			m_sReceiptCust = "";

			EFTTransactionRequest nitroRequest = new EFTTransactionRequest();
			//			nitroRequest.TrainingMode = true;
			nitroRequest.AmountPurchase = (decimal)dPurchase;
			nitroRequest.AmountCash = 0;
			nitroRequest.CardPANSource = PANSource.Default;
			nitroRequest.ReferenceNumber = sRef;
			nitroRequest.Type = TransactionType.Refund;

			if (!nitroClientIP.DoTransaction(nitroRequest))
			{
				MessageBox.Show("DoTransaction send to the Nitro Client IP interface failed.", "NITRO IP Interface", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			m_bPrint = false;
			Program.g_log.Info("Nitro refund done, amount = " + dPurchase.ToString("c"));
			return true;
		}
	}
}
