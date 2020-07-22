using System;
using System.Collections.Generic;
using System.Text;

namespace QPOS2008
{
    class eftpos_wolfstrike
    {
        public string m_sStatus = "";
		private MySerialPort m_ssp = null;
		private bool m_bWaitForACK = false;
		private int m_nCount = 0;
		private int m_nRetried = 0;

        public eftpos_wolfstrike()
		{
			m_ssp = new MySerialPort();
			m_ssp.PortNumber = "COM" + Program.m_sEftposCom;
			if (!m_ssp.Open())
			{
				m_sStatus = "Open port failed";
			}
		}
		public void ClosePort()
		{
			m_ssp.Close();
		}
		private void DoTCmd(double dPurchaseAmount, double dCashOutAmount, double dRefundAmount, string sRefNum, char tt)
		{
			int nRefNumLength = sRefNum.Length;
			string sPurchaseAmount = ((int)(Math.Round(dPurchaseAmount,2) * 100)).ToString("D12"); //12 bytes left zero padding eg: $12.00 to 000000001200
			string sCashoutAmount = ((int)(Math.Round(dCashOutAmount,2) * 100)).ToString("D12");
			string sRefundAmount = ((int)(Math.Round(dRefundAmount,2) * 100)).ToString("D12");

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
			m_ssp.SendCmd(buf, 0, p);
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
			m_ssp.SendCmd(buf, 0, i);
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
			m_ssp.SendCmd(buf, 0, c);
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
			m_ssp.SendCmd(buf, 0, s);
		}
		private void sendAck()
		{
			byte[] buf = new byte[16];
			buf[0] = 0x06;
			m_ssp.SendCmd(buf, 0, 1);
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
			m_ssp.SendCmd(buf, 0, j);
			m_bWaitForACK = true;
			m_nCount = 0;
		}
		public void Purchase(double dPurchase, double dCashOut, string sRefNum)
		{	
			if(dPurchase<0 && dCashOut ==0)
				DoTCmd(0, 0, 0-dPurchase, sRefNum, 'R');
//			else if (dPurchase >=0 && dCashOut >0)
//				DoTCmd(dPurchase, dCashOut, 0, sRefNum, 'C');
			else
				DoTCmd(dPurchase, dCashOut, 0, sRefNum, 'P');
//			m_sStatus = "cmd sent";
		}
/*		
		public void Refund(double dAmount, string sRefNum)
		{
			DoTCmd(0, 0, dAmount, sRefNum, 'R');
		}
		public void CashOut(double dAmount, string sRefNum)
		{
			DoTCmd(0, dAmount, 0, sRefNum, 'C');
		}
 */ 
		public string CheckResponse()
		{
			if(m_bWaitForACK)
			{
				if(m_ssp.m_nIndex <= 0 || m_ssp.m_buffer[0] != 0x06) //ACK
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
						SendD00();
						m_nRetried++;
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
			if (m_ssp.m_nIndex <= 0)
				return "";
			int nETX = 0;
			for(int i=0; i<m_ssp.m_nIndex; i++)
			{
				if(m_ssp.m_buffer[i] == 0x03) //ETX
				{
					nETX = i;
					break;
				}
			}
			if(nETX == 0)
				return "";
				
			string sRet = "";	
			sRet = System.Text.Encoding.GetEncoding(1251).GetString(m_ssp.m_buffer, 3, nETX - 3);
			SendD00();
//          sendAck();
			return sRet;
		}
    }
}
