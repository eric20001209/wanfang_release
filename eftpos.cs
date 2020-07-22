using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Media;

namespace QPOS2008
{
	public partial class EFTPOS : Form
	{
		private PrintDocument printDoc = new PrintDocument();
		private string m_sPrintBuffer = "";
		private EFTPOS_INGENICO m_ee = null;
		private EFTPOS_SMARTPAY m_es = null;
		private EFTPOS_VXLINK m_evx = null;
		private EFTPOS_VLLINK m_evl = null;
		private FDps m_dps = null;
		private CVault m_valut = null;
		private CNitro m_nitro = null;
		private string sResponseTxt = "";
		private int m_nCount = 0;
		private int m_left = 0;

		public string m_sRefNum = "";
		public string m_sMonitorData = "";
		public double m_dAmount = 0;
		public double m_dCashOut = 0;
		public double m_dRefundAmount = 0;
		public double m_dChequeAmount = 0;
		public int m_iType = 1;

		public bool m_bAdmin = false;
		public bool m_bAccepted = false;

		public char m_cCMD = 'T';
		public string m_sReceipt = "";
		public string m_sReceiptCust = "";
		public bool m_bEnableCancel = false;

		public EFTPOS()
		{
			if(!Program.m_bEnableEftpos)
				return;
			InitializeComponent();
			try
			{
				if (Program.m_eftposType == "ingenico")
					m_ee = new EFTPOS_INGENICO();
				else if (Program.m_eftposType == "smartpay")
				{
					m_es = new EFTPOS_SMARTPAY();
					m_bEnableCancel = true;
				}
				//				else if (Program.m_eftposType == "dps")
				//					m_dps = new FDps();
				else if (Program.m_eftposType == "verifone")
				{
					m_valut = new CVault();
					m_valut.OpenSession();
				}
				else if (Program.m_eftposType == "nitro")
					m_nitro = new CNitro();
				else if (Program.m_eftposType == "vxlink")
					m_evx = new EFTPOS_VXLINK();
				else if (Program.m_eftposType == "vllink")
					m_evl = new EFTPOS_VLLINK();
			}
			catch (Exception e)
			{
				MessageBox.Show("EFTPOS Error\r\n" + e.ToString());
				return;
			}
			printDoc.PrinterSettings.PrinterName = Program.m_sPrinterName;
			printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
		}
		private void eftpos_Load(object sender, EventArgs e)
		{
			m_left = this.Left;

			if (Program.m_eftposType == "ingenico")
			{
				if (m_sRefNum.Length > 3)
					m_sRefNum = m_sRefNum.Substring(m_sRefNum.Length - 3, 3);
				else
					m_sRefNum = Program.MyIntParse(Program.m_sStationID).ToString("D3");
			}
			else
			{
				if (Program.m_eftposType != "dps" || Program.m_eftposType != "dps_hit")
					m_sRefNum = Program.MyIntParse(Program.m_sStationID).ToString("D3");
			}
			if (Program.m_eftposType == "nitro")
			{
				this.Left = -16348;
				//				this.ShowInTaskbar = false;
				//				timer3.Start(); //hide main window
				if (m_nitro == null)
					m_nitro = new CNitro();
				m_sReceipt = "";
			}
			if (m_bEnableCancel)
				btnCancel.Visible = true;
			else
				btnCancel.Visible = false;
			if (!m_bAdmin)
			{
				if (Program.m_eftposType == "ingenico")
				{
					if (Program.m_sEftposCom == "0" || Program.m_sEftposCom == "" || Program.m_sEftposCom == null)
					{
						FormMSG fm = new FormMSG();
						//						fm.btnNo.Visible = false;
						//						fm.btnYes.Visible = false;
						fm.m_sMsg = "Please check Port!";
						fm.ShowDialog();
						return;
					}
					m_ee.Purchase(m_dAmount, m_dCashOut, m_sRefNum);
				}
				else if (Program.m_eftposType == "smartpay")
				{
					if (m_dAmount < 0)
						m_es.Refund(0 - m_dAmount, m_sRefNum);
					else
						m_es.Purchase(m_dAmount, m_dCashOut, m_sRefNum);
					txtResposneEftpos.Text = "SWIPE CARD";
				}
				else if (Program.m_eftposType == "vxlink")
				{
					if (Program.MyCheckPrinter(Program.m_sPrinterName))
					{
						timer1.Stop();
						MyMessageBox fm = new MyMessageBox();
						fm.m_title = "Printer Error";
						fm.m_msg = "Printer Error, please check printer first before using EFTPOS";
						fm.ShowDialog();
						this.Close();
						return;
					}
					m_evx.ConfigurePrinting();
					m_evx.Purchase(m_dAmount, m_dCashOut, m_sRefNum);
				}
				else if (Program.m_eftposType == "vllink")
					m_evl.Purchase(m_dAmount, m_dCashOut, m_sRefNum);
				else if (Program.m_eftposType == "dps")
				{
					m_dps = new FDps();
					this.Left = -16348;
					m_dps.m_dPurchase = m_dAmount;
					m_dps.m_dCashOut = m_dCashOut;
					m_dps.m_sRef = m_sRefNum;
					m_dps.ShowDialog();
					if (m_dps.m_bAuthorized)
					{
						sResponseTxt = "ACCEPTED";
						m_bAccepted = true;
						m_sReceipt = m_dps.m_sReceipt;
					}
					this.Close();
					return;
				}
				else if (Program.m_eftposType == "dps_hit")
				{
					FormDPSHIT fm = new FormDPSHIT();
					this.Left = -16348;
					fm.m_dAmount = m_dAmount;
					fm.m_dCashOut = m_dCashOut;
					fm.m_sRef = m_sRefNum;
					fm.ShowDialog();
					if (fm.m_bAuthorized)
					{
						sResponseTxt = "ACCEPTED";
						m_bAccepted = true;
						m_sReceipt = fm.m_sReceipt;
					}
					this.Close();
					return;
				}
				else if (Program.m_eftposType == "verifone")
				{
					this.Left = -16348;
					string sRet = m_valut.Purchase(m_dAmount, m_dCashOut, m_dChequeAmount, m_sRefNum);
					if (sRet == "ACCEPTED")
					{
						sResponseTxt = "ACCEPTED";
						m_bAccepted = true;
						m_sReceipt = m_valut.GetReceiptText();
					}
					else
					{
						sResponseTxt = "DECLINED";
						m_bAccepted = false;
						m_sReceipt = m_valut.GetReceiptText();
					}
					this.Close();
					return;
				}
				else if (Program.m_eftposType == "nitro")
				{
					Program.g_log.Info("nitro start, amount=" + m_dAmount.ToString("c"));
					if (!m_nitro.Connect())
					{
						this.Left = m_left;
						this.Close();
						return;
					}
					if (m_dAmount > 0)
					{
						if (!m_nitro.Purchase(m_dAmount, m_dCashOut, m_dChequeAmount, m_sRefNum))
						{
							this.Close();
							return;
						}
					}
					else
					{
						if (!m_nitro.Refund(0 - m_dAmount, m_sRefNum))
						{
							this.Close();
							return;
						}
					}
				}
				m_nCount = 0;
				timer1.Interval = 500;
				timer1.Start();
				btnok.Visible = false;
			}
			if (m_bAdmin)
			{
				btnInt.Visible = true;
				btnSettle.Visible = true;
			}
		}
		public void DoClose()
		{
			if (Program.m_eftposType == "nitro")
			{
				if (m_nitro != null)
					m_nitro.Disconnect();
				m_nitro = null;
			}
			else if (Program.m_eftposType == "verifone")
			{
				m_valut.CloseSession();
			}
		}
		public void CheckVault()
		{
			if (Program.m_eftposType == "verifone")
				m_valut.HandlePowerLinkFailure();
		}
		private void EFTPOS_FormClosed(object sender, FormClosedEventArgs e)
		{
			//			if (Program.m_eftposType == "ingenico")
			//				m_ee.ClosePort();
			//			else if (Program.m_eftposType == "smartpay")
			//				m_es.ClosePort();
			//			else if (Program.m_eftposType == "vxlink")
			//				m_evx.ClosePort();
		}
		private void btnSettle_Click(object sender, EventArgs e)
		{
			if (Program.m_eftposType == "ingenico")
				m_ee.DoSettlementCutOver();
			else if (Program.m_eftposType == "dps")
				m_dps.DoSettlementCutOver();
			m_nCount = 0;
			timer1.Interval = 100;
			timer1.Start();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			m_nCount++;
			//Program.g_log.Info("Eftpose response, nCount = " + m_nCount.ToString());
			string sRet = "";
			if (Program.m_eftposType == "ingenico")
				sRet = m_ee.CheckResponse();
			else if (Program.m_eftposType == "vxlink")
				sRet = m_evx.CheckResponse();
			else if (Program.m_eftposType == "vllink")
				sRet = m_evl.CheckResponse();
			else if (Program.m_eftposType == "smartpay")
				sRet = m_es.CheckResponse();
			else if (Program.m_eftposType == "nitro")
			{
				if (m_nitro.m_bSocketFailed)
				{
					timer1.Stop();
					Program.MsgBox("PINpad Offline\r\nCheck PINPad and Cable\r\nat POS\r\n");
					txtResposneEftpos.Text = "terminal error";
					txtResponseEftLine2.Text = "";
					m_nCount = 0;
					btnok.Visible = true;
					this.Left = m_left;
					//					m_nitro.Disconnect();
					m_nitro = null;
					return;
				}
				sRet = m_nitro.CheckResponse();
			}
			if (sRet == "")// || Program.m_eftposType == "nitro")
			{
				int nTimeoutCount = 560;
				if (Program.m_eftposType == "vxlink")
					nTimeoutCount = 10000; //vxlink doesn't like timeout
				if (m_nCount > nTimeoutCount)
				{
					txtResposneEftpos.Text = "terminal timed out";
					m_nCount = 0;
					btnok.Visible = true;
					timer1.Stop();
					//Program.g_log.Info("Eftpose timer1 stopped, (timed out)");
					this.Left = m_left;
					if (Program.m_eftposType == "nitro")
					{
						m_nitro.Disconnect();
						m_nitro = null;
						return;
					}
				}
				if (Program.m_eftposType != "nitro")
					return;
			}
			//Program.g_log.Info("Eftpose response, text = " + sRet);
			if (Program.m_eftposType == "ingenico")
			{
				if (m_bAccepted)
				{
					timer1.Stop();
					return;
				}
				AnalyzeResponseIngenico(sRet);
			}
			else if (Program.m_eftposType == "vxlink" || Program.m_eftposType == "vllink")
				AnalyzeResponseVxlink(sRet);
			else if (Program.m_eftposType == "smartpay")
				AnalyzeResponseSmartPay(sRet);
			else if (Program.m_eftposType == "nitro")
				AnalyzeResponseNitro(sRet);
		}
		private void AnalyzeResponseSmartPay(string sResponse)
		{
			sResponse = sResponse.ToUpper();
			if (sResponse.IndexOf("ACCEPTED") >= 0)
			{
				sResponseTxt = "ACCEPTED";
				txtResposneEftpos.Text = "ACCEPTED";
				m_bAccepted = true;
				timer1.Stop();
				btnCancel.Visible = false;
				btnok.Visible = true;
				m_sReceipt = m_es.m_sReceipt;
				m_sReceiptCust = m_sReceipt;
				return;
			}
			else if (sResponse.IndexOf("DECLINED") >= 0)
			{
				sResponseTxt = "DECLINED";
				txtResposneEftpos.Text = "DECLINED";
				//				sResponseCode = "10";
				timer1.Stop();
				btnCancel.Visible = false;
				btnok.Visible = true;
				return;
			}
			else
			{
				int n = sResponse.IndexOf("ACCEPT WITH SIG");
				if (n >= 0)
				{
					if (sResponse.IndexOf("ACCEPT WITH SIG", n + 15) > 0)
					{
						sResponseTxt = "ACCEPTED";
						txtResposneEftpos.Text = "ACCEPTED";
						m_bAccepted = true;
						timer1.Stop();
						btnCancel.Visible = false;
						btnok.Visible = true;
						return;
					}
					m_sReceipt = m_es.m_sReceipt;
					m_sReceiptCust = m_sReceipt;
				}
			}
		}
		private void AnalyzeResponseVxlink(string sResponse)
		{
			sResponse = sResponse.ToUpper();
			//			txtResposneEftpos.Text = sResponse;
			//			if (sResponse.ToUpper().IndexOf("PROGRESS") >= 0)
			if (sResponse == "PRINTER OK")
			{
				m_evx.Purchase(m_dAmount, m_dCashOut, m_sRefNum);
				return;
			}
			else if (sResponse.ToUpper().IndexOf("PROCESSING") >= 0)
			{
				txtResposneEftpos.Text = "PROCESSING";
				txtResponseEftLine2.Text = "";
				//				m_evx.GetResult();
				return;
			}
			else if (sResponse.IndexOf("APPROVED") >= 0 || sResponse.IndexOf("ACCEPTED") >= 0)
			{
				sResponseTxt = "ACCEPTED";
				txtResposneEftpos.Text = "ACCEPTED";
				m_bAccepted = true;
				timer1.Stop();
				btnok.Visible = true;
				m_sReceipt = m_evx.m_sReceipt;
				m_sReceiptCust = m_sReceipt;
				return;
			}
			else if (sResponse.IndexOf("DECLINE") >= 0)
			{
				sResponseTxt = "DECLINED";
				txtResposneEftpos.Text = "DECLINED";
				btnok.Visible = true;
				timer1.Stop();
				m_sReceipt = m_evx.m_sReceipt;
				m_sReceiptCust = m_sReceipt;
				m_sPrintBuffer = m_sReceipt;
				Program.g_log.Info("VxLink Print, text = " + m_sPrintBuffer);
				try
				{
					printDoc.Print();
				}
				catch (Exception e)
				{
					MessageBox.Show("Printer error, print receipt failed." + e.ToString());
				}
				return;
			}
			else if (sResponse.IndexOf("CANCELLED") >= 0)
			{
				sResponseTxt = "CANCELLED";
				txtResposneEftpos.Text = "CANCELLED";
				btnok.Visible = true;
				timer1.Stop();
				return;
			}
			else if (sResponse.IndexOf("SIGNATURE") >= 0)
			{
				sResponseTxt = "SIGNATURE";
				txtResposneEftpos.Text = "REQUIRED";
				m_sReceipt = m_evx.m_sReceipt;
				m_sReceiptCust = m_sReceipt;
				m_sPrintBuffer = m_sReceipt;
				Program.g_log.Info("VxLink Print, text = " + m_sPrintBuffer);
				try
				{
					printDoc.Print();
				}
				catch (Exception e)
				{
					MessageBox.Show("Printer error, print receipt failed." + e.ToString());
				}
				return;
			}
			else
			{
				int nStart = sResponse.IndexOf(m_sRefNum.ToUpper());
				if (nStart <= 0)
					return;
				nStart += m_sRefNum.Length + 6;
				if (nStart >= sResponse.Length)
					return;
				txtResposneEftpos.Text = sResponse.Substring(nStart, sResponse.Length - nStart);
				txtResponseEftLine2.Text = "";
				btnok.Visible = true;
				timer1.Stop();
				return;
			}
		}
		private void AnalyzeResponseIngenico(string sResponse)
		{
			if (sResponse.Length < 1)
			{
				Program.g_log.Info("ingenico received error, length < 1, Response: " + sResponse);
				return;
			}

			if (sResponse.IndexOf("DEBUG") >= 0)
			{
				MessageBox.Show(sResponse);
				return;
			}
			else if (sResponse.IndexOf("timed out") >= 0)
			{
				sResponseTxt = sResponse;
				btnok.Visible = true;
				return;
			}

			string sCmdCode = sResponse.Substring(0, 1);
			Program.g_log.Info("ingenico sCmdCode=" + sCmdCode + ", sResponse=" + sResponse);
			if (sCmdCode == "D") //require to display
			{
				if (sResponse.Length > 21)
					txtResposneEftpos.Text = sResponse.Substring(1, 20).ToString().Trim();
				if (sResponse.Length > 40)
					txtResponseEftLine2.Text = sResponse.Substring(21, sResponse.Length - 21).ToString().Trim();
				if (sResponse.ToUpper().IndexOf("MARK") >= 0 || sResponse.IndexOf("OVER") >= 0)
					btnok.Visible = true;
				return;
			}
			else if (sCmdCode == "T") //result
			{
				if (sResponse.Length < 26)
				{
					Program.g_log.Info("ingenico received error on T result, length < 26, Response:" + sResponse);
					return;
				}
				string sResponseCode = sResponse.Substring(1, 2);
				string sResponseText = sResponse.Substring(3, 20);
				string sRefNum = sResponse.Substring(23, 3);
				Program.g_log.Info("Response code:00, ResponseText:" + sResponseText + ", RefNum:" + sRefNum);
				if (sResponseCode == "00" || sResponseCode == "08")
				{
					if (sRefNum != m_sRefNum)
					{
						Program.g_log.Info("ingenico received error on T result, incorrect reference number, Response:" + sResponse + ", RefNum:" + sRefNum + ", expected:" + m_sRefNum);
						return;
					}
					sResponseTxt = "ACCEPTED";
					txtResposneEftpos.Text = sResponseText;
					txtResponseEftLine2.Text = "";
					m_bAccepted = true;
					btnok.Visible = true;
					Program.g_log.Info("ingenico ACCEPTED, Response:" + sResponse + ", RefNum:" + sRefNum + ", expected:" + m_sRefNum);
				}
				else if (sResponse.IndexOf("DECLINED") >= 0)
				{
                    PlayTransactionFailureSound();
					sResponseTxt = "DECLINED";
					txtResposneEftpos.Text = sResponseText;
					txtResponseEftLine2.Text = "";
					btnok.Visible = true;
					Program.g_log.Info("ingenico DECLINED, Response:" + sResponse + ", RefNum:" + sRefNum + ", expected:" + m_sRefNum);
				}
				else
				{
					Program.g_log.Info("ingenico received error on T result, Reponse Code not 00 or 08, Response Code:" + sResponseCode + ", Response:" + sResponse);
					txtResposneEftpos.Text = sResponseText;
					btnok.Visible = true;
				}
			}
			return;
		}
		private void AnalyzeResponseNitro(string sResponse)
		{
			Program.g_log.Info("Eftpos AnalyzeResponseNitro\r\nbPrint=" + m_nitro.m_bPrint.ToString() + "\r\nReceipt=" + m_nitro.m_sReceipt + "\r\nReceiptCust=" + m_nitro.m_sReceiptCust + " \r\nResponse=" + sResponse);
			//handle client printing
			if (m_nitro.m_sReceiptCust != "")
				m_sReceiptCust = m_nitro.m_sReceiptCust;
			if (m_nitro.m_bPrint && m_nitro.m_sReceipt != "")
			{
				m_nitro.m_bPrint = false;
				m_sReceipt = m_nitro.m_sReceipt;
				m_sPrintBuffer = m_sReceipt;
				Program.g_log.Info("Nitro Print, text = " + m_sPrintBuffer);
				try
				{
					printDoc.Print();
				}
				catch (Exception e)
				{
					MessageBox.Show("Printer error, print receipt failed." + e.ToString());
				}
			}

			if (m_nitro.m_bError)
			{
				this.Left = m_left;
				txtResposneEftpos.Text = m_nitro.m_sResponseText1;
				txtResponseEftLine2.Text = m_nitro.m_sResponseText2;
				btnok.Visible = true;
				timer1.Stop();
				m_nitro.Disconnect();
				m_sReceipt = m_nitro.m_sReceipt;
				m_nitro = null;
				return;
			}
			sResponse = sResponse.ToUpper();
			//			Program.g_log.Info("Nitro analyze, rest = " + sResponse);
			if (sResponse.IndexOf("ACCEPTED") >= 0)
			{
				this.Left = m_left;
				sResponseTxt = "ACCEPTED";
				txtResposneEftpos.Text = "ACCEPTED";
				m_bAccepted = true;
				timer1.Stop();
				btnok.Visible = true;
				m_sReceipt = m_nitro.m_sReceipt;
				m_nitro.Disconnect();
				m_nitro = null;
				return;
			}
			else if (sResponse.IndexOf("DECLINE") >= 0)
			{
				this.Left = m_left;
				sResponseTxt = "DECLINED";
				txtResposneEftpos.Text = m_nitro.m_sResponseText1;
				txtResponseEftLine2.Text = m_nitro.m_sResponseText2;
				m_bAccepted = false;
				timer1.Stop();
				btnok.Visible = true;
				m_sReceipt = m_nitro.m_sReceipt;
				m_nitro.Disconnect();
				m_nitro = null;
				return;
			}
			else
			{
				txtResposneEftpos.Text = m_nitro.m_sResponseText1;
				txtResponseEftLine2.Text = m_nitro.m_sResponseText2;
				m_sReceipt = m_nitro.m_sReceipt;
				//				if(sResponse != "")
				//					this.Left = m_left;
				return;
			}
			if (sResponse.IndexOf("CANCEL") >= 0)
			{
				this.Left = m_left;
			}
		}
		private void stopAll()
		{
			//			m_bEFTInital = false;
			m_nCount = 0;
			timer1.Stop();
			//			m_bCanClose = true; //can close eftpos panel from now on
			txtResposneEftpos.Text = "";
			txtResponseEftLine2.Text = "";
			btnCancel.Visible = false;
			this.Close();
		}
		private void btnInt_Click(object sender, EventArgs e)
		{
		}
		private void btnok_Click_1(object sender, EventArgs e)
		{
			timer2.Stop();
			stopAll();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			timer2.Stop();
			if (DialogResult.Yes == MessageBox.Show("Is the transaction on EFTPOS terminal already canceld?", "Cancel", MessageBoxButtons.YesNo))
				stopAll();
		}
		private void txtResponseEftLine2_TextChanged(object sender, EventArgs e)
		{

		}
		private void timer2_Tick(object sender, EventArgs e)
		{
			timer2.Stop();
			stopAll();
		}
		private void timer3_Tick(object sender, EventArgs e)
		{
			//			this.Hide();
			this.Visible = false;
			timer3.Stop();
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
		public void ShowNitroCP()
		{
			if (Program.m_eftposType != "nitro")
				return;
			if (m_nitro == null)
				m_nitro = new CNitro();
			if (!m_nitro.Connect())
				return;
			m_nitro.DisplayControlPanel();
			Thread.Sleep(5000);
			m_nitro.Disconnect();
			m_nitro = null;
		}
        public void PlayTransactionFailureSound()
        {
            //Console.Beep(300, 200);
            //Console.Beep(900, 200);
            Console.Beep(1200, 200);
            Console.Beep(1200, 200);
            Console.Beep(1200, 200);
            Console.Beep(1200, 200);
            Console.Beep(1200, 200);
            Console.Beep(1200, 200);
        }
	}
}
