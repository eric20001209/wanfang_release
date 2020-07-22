using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QPOS2008
{
	public class EFTPOS_T1000
	{
		public string m_sStatus = "";

		private MySerialPort m_sp = null;
		private bool m_bWaitForACK = false;
		private int m_nCount = 0;
		private int m_nRetried = 0;
	  
		public EFTPOS_T1000()
		{
			m_sp = new MySerialPort();
			m_sp.PortNumber = "COM" + Program.m_sEftposCom;
			if (!m_sp.Open())
			{
				m_sStatus = "Open port failed";
			}
//			Init();
		}
		public void ClosePort()
		{
			m_sp.Close();
		}
		private void DoTCmd(double dPurchaseAmount, double dCashOutAmount, double dRefundAmount, string sRefNum, char tt)
		{
			int nRefNumLength = sRefNum.Length;
//			string sPurchaseAmount = ((int)(Math.Round(dPurchaseAmount,2) * 100)).ToString("D12"); //12 bytes left zero padding eg: $12.00 to 000000001200
//			string sCashoutAmount = ((int)(Math.Round(dCashOutAmount,2) * 100)).ToString("D12");
//			string sRefundAmount = ((int)(Math.Round(dRefundAmount,2) * 100)).ToString("D12");

			string sPurchaseAmount = "";
			double ddPurchaseAmount = Math.Round((dPurchaseAmount * 10), 2);
			double dddPurchaseAmount = ddPurchaseAmount * 10;
			sPurchaseAmount = ((int)dddPurchaseAmount).ToString("D12"); //12 bytes left zero padding eg: $12.00 to 000000001200

			string sCashoutAmount = "";
			double ddCashoutAmount = Math.Round((dCashOutAmount * 10), 2);
			double dddCashoutAmount = ddCashoutAmount * 10;
			sCashoutAmount = ((int)(dddCashoutAmount)).ToString("D12");

			string sRefundAmount = "";
			double ddRefundAmount = Math.Round(dRefundAmount * 10, 2);
			double dddRefundAmount = ddRefundAmount * 10;
			sRefundAmount = ((int)(dddRefundAmount)).ToString("D12");

			char cmd = 'T';
			byte[] buf = new byte[1024];
			int p = 0;
			buf[p++] = 0x02;
			buf[p++] = (byte)cmd;
			buf[p++] = (byte)tt;
			for (int i = 0; i < nRefNumLength; i++)
				buf[p++] = (byte)sRefNum[i];
			for (int i = 0; i < sPurchaseAmount.Length; i++)
				buf[p++] = (byte)sPurchaseAmount[i];
			for (int i = 0; i < sCashoutAmount.Length; i++)
				buf[p++] = (byte)sCashoutAmount[i];
			for (int i = 0; i < sRefundAmount.Length; i++)
				buf[p++] = (byte)sRefundAmount[i];
			buf[p++] = 0x03;

			int lrc = 0;
			for (int i = 1; i < p; i++)
			{
				lrc = lrc ^ buf[i];
			}
			buf[p++] = (byte)lrc;
			m_sp.SendCmd(buf, 0, p);

			string sDebug = System.Text.Encoding.ASCII.GetString(buf, 0, p);
			string sh = "";
			for (int m = 0; m < sDebug.Length; m++)
			{
				sh += "0x" + ((int)sDebug[m]).ToString("X") + " ";
			}
			Program.g_log.Info("t1000 sent " + p + " bytes:" + sDebug + " Hex:" + sh);
		}
		public void Init()
		{
			byte[] buf = new byte[4];
			int i = 0;
			buf[i++] = 0x02;
			buf[i++] = (byte)'I';
			buf[i++] = 0x03;
			int lrc = 0; //start value
			for (int r = 1; r < i; r++)
			{
				lrc = lrc ^ buf[r];
			}
			buf[i++] = (byte)lrc;
			m_sp.SendCmd(buf, 0, i);
			Program.g_log.Info("t1000 sent init");
		}
		public void DoCanCel()
		{
			byte[] buf = new byte[4];
			int c = 0;
			buf[c++] = 0x02;
			buf[c++] = (byte)'V';
			buf[c++] = 0x03;
			int lrc = 0;
			for (int i = 1; i < c; i++)
				lrc = lrc ^ buf[i];
			buf[c++] = (byte)lrc;
			m_sp.SendCmd(buf, 0, c);
		}
		public void DoSettlementCutOver()
		{
			byte[] buf = new byte[4];
			int s = 0;
			buf[s++] = 0x02;
			buf[s++] = (byte)'S';
			buf[s++] = 0x03;
			int lrc = 0;
			for (int i = 1; i < s; i++)
				lrc = lrc ^ buf[i];
			buf[s++] = (byte)lrc;
			m_sp.SendCmd(buf, 0, s);
		}
		private void sendAck()
		{
			byte[] buf = new byte[16];
			buf[0] = 0x06;
			m_sp.SendCmd(buf, 0, 1);
			Program.g_log.Info("t1000 sent ACK");
		}
		private void SendD00()
		{
			byte[] buf = new byte[16];
			int j = 0;
			buf[j++] = 0x06;
			buf[j++] = 0x02;
			buf[j++] = 0x44;
			buf[j++] = 0x30;
			buf[j++] = 0x30;
			buf[j++] = 0x03;
			buf[j++] = (byte)'G';
			m_sp.SendCmd(buf, 0, j);
			m_bWaitForACK = true;
			m_nCount = 0;
		}
		public void Purchase(double dPurchase, double dCashOut, string sRefNum)
		{
			if (dPurchase < 0 && dCashOut == 0)
				DoTCmd(0, 0, 0 - dPurchase, sRefNum, 'R');
			else
				DoTCmd(dPurchase, dCashOut, 0, sRefNum, 'P');
//			m_sStatus = "cmd sent";
		}
		public void Refund(double dAmount, string sRefNum)
		{
			DoTCmd(0, dAmount, 0, sRefNum, 'R');
		}
		public void CashOut(double dAmount, string sRefNum)
		{
			DoTCmd(0, 0, dAmount, sRefNum, 'C');
		}
		public string CheckResponse()
		{
			if(m_bWaitForACK)
			{
				if(m_sp.m_nIndex <= 0 || m_sp.m_buffer[0] != 0x06) //ACK
				{
					m_nCount++;
					if (m_nCount >= 10) //1 seconds
					{
						if(m_nRetried >= 10)
						{
//							DoCanCel();
//							Init();
							m_nRetried = 0;
							m_bWaitForACK = false;
							return "EFTPOS timed out, please retry";
						}
//						sendAck();
//						Thread.Sleep(300);
						SendD00();
						m_nRetried++;
						Program.g_log.Info("t1000 resent D00:" + m_nRetried.ToString());
						#if DEBUG
							return "DEBUG, Resent D00:" + m_nRetried.ToString();
						#else
							return "";
						#endif
					}
				}
				else
				{
					m_bWaitForACK = false;
					return "";
				}
			}
			if (m_sp.m_nIndex <= 0)
				return "";
			int nETX = 0;
			for(int i=0; i<m_sp.m_nIndex; i++)
			{
				if(m_sp.m_buffer[i] == 0x03) //ETX
				{
					nETX = i;
					break;
				}
			}
			if(nETX == 0)
				return "";
				
			string sRet = "";
			sRet = System.Text.Encoding.GetEncoding(1251).GetString(m_sp.m_buffer, 3, nETX - 3);
			string sh = "";
			for (int i = 0; i < sRet.Length; i++)
			{
				sh += "0x" + ((int)sRet[i]).ToString("X") + " ";
			}
			Program.g_log.Info("t1000 received " + sRet.Length + " bytes:" + sRet + " Hex:" + sh);

//			sendAck();
			if(sRet.IndexOf("ACCEPTED") < 0)
			{
				SendD00();
				Program.g_log.Info("t1000 sent D00");
			}

			return sRet;
/*			
			switch (m_sStatus)
			{
				case "cmd sent":
					sendAck();
					sRet = System.Text.Encoding.GetEncoding(1251).GetString(m_sp.m_buffer, 4, nETX - 4);
					m_sStatus = "processing";
					SendD00();
					break;
				case "processing":
					break;
				default:
					break;
			}
			return sRet;
*/
		}
	}
}
