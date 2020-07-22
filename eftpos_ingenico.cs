using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QPOS2008
{
	public class EFTPOS_INGENICO
	{
		public string m_sStatus = "";

		private MySerialPort m_sp = null;
		private bool m_bWaitForACK = false;
		private int m_nCount = 0;
		private int m_nRetried = 0;

		public EFTPOS_INGENICO()
		{
			m_sp = new MySerialPort();
			//		m_sp.PortNumber = "COM" + Program.m_eftposCom;
			m_sp.PortNumber = "COM" + Program.m_sEftposCom;
			if (!m_sp.Open())
			{
				m_sStatus = "Open port failed";
			}
		}
		public void ClosePort()
		{
			m_sp.Close();
		}
		private void DoTCmd(double dPurchaseAmount, double dCashOutAmount, double dRefundAmount, string sRefNum, char tt)
		{
			int nRefNumLength = sRefNum.Length;
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

			//		byte[] buf = new byte[60];
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
			//			m_bWaitForACK = true;
			string sDebug = System.Text.Encoding.ASCII.GetString(buf, 0, p);
			string sh = "";
			for (int m = 0; m < sDebug.Length; m++)
			{
				sh += "0x" + ((int)sDebug[m]).ToString("X") + " ";
			}
			Program.g_log.Info("ingenico sent " + p + " bytes:" + sDebug + " Hex:" + sh);
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
			m_sp.ResetBuffer();
			string sDebug = System.Text.Encoding.ASCII.GetString(buf, 0, i);
			string sh = "";
			for (int m = 0; m < sDebug.Length; m++)
			{
				sh += "0x" + ((int)sDebug[m]).ToString("X") + " ";
			}
			Program.g_log.Info("ingenico sent init " + i + " bytes:" + sDebug + " Hex:" + sh);
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
			//	byte[] buf = new byte[1];
			buf[0] = 0x06;
			m_sp.SendCmd(buf, 0, 1);
			string sDebug = System.Text.Encoding.ASCII.GetString(buf, 0, 1);
			string sh = "";
			for (int m = 0; m < sDebug.Length; m++)
			{
				sh += "0x" + ((int)sDebug[m]).ToString("X") + " ";
			}
			Program.g_log.Info("ingenico sent ACK 1 byte:" + sDebug + " Hex:" + sh);
		}
		private void SendD00()
		{
			byte[] buf = new byte[16];
			//	byte[] buf = new byte[6];
			int j = 0;
			//buf[j++] = 0x06;
			buf[j++] = 0x02;
			buf[j++] = 0x44;
			buf[j++] = 0x30;
			buf[j++] = 0x30;
			buf[j++] = 0x03;
			buf[j++] = (byte)'G';
			m_sp.SendCmd(buf, 0, j);
			//			m_bWaitForACK = true;
			m_nCount = 0;
			string sDebug = System.Text.Encoding.ASCII.GetString(buf, 0, j);
			string sh = "";
			for (int m = 0; m < sDebug.Length; m++)
			{
				sh += "0x" + ((int)sDebug[m]).ToString("X") + " ";
			}
			Program.g_log.Info("ingenico sent D00 " + j + " bytes:" + sDebug + " Hex:" + sh);
		}
		public void Purchase(double dPurchase, double dCashOut, string sRefNum)
		{
			m_sp.ResetBuffer();
			DoTCmd(dPurchase, dCashOut, 0, sRefNum, 'P');
		}
		public void Refund(double dAmount, string sRefNum)
		{
			m_sp.ResetBuffer();
			DoTCmd(0, dAmount, 0, sRefNum, 'R');
		}
		public void CashOut(double dAmount, string sRefNum)
		{
			m_sp.ResetBuffer();
			DoTCmd(0, 0, dAmount, sRefNum, 'C');
		}
		public string CheckResponse()
		{
			//			Program.g_log.Info("ingenico CheckResponse");
			/*			if (m_bWaitForACK)
						{
							Program.g_log.Info("ingenico waiting for ACK");
							if (m_sp.m_nIndex <= m_sp.m_nBegin || m_sp.m_buffer[m_sp.m_nBegin] != 0x06) //ACK
							{
								m_nCount++;
								if (m_nCount >= 10) //1 seconds
								{
									if (m_nRetried >= 10)
									{
										m_nRetried = 0;
										m_bWaitForACK = false;
										Program.g_log.Info("ingenico timed out.");
										return "EFTPOS timed out, please retry";
									}
			//						SendD00();
									m_nRetried++;
									Program.g_log.Info("ingenico waiting for ACK, retried " + m_nRetried.ToString() + " times.");
			#if DEBUG
									return "DEBUG, retried " + m_nRetried.ToString() + " times, m_nIndex=" + m_sp.m_nIndex.ToString();
			#else
									return "";
			#endif
								}
							}
							else //got ACK
							{
								m_bWaitForACK = false;
								m_sp.m_nBegin++;
								Program.g_log.Info("ingenico received ACK");
								return "";
							}
						}
			*/
			if (m_sp.m_nIndex <= m_sp.m_nBegin)
				return "";
			int nSTX = -1; //start
			int nETX = 0; //end
			byte bytePre = 0x0;
			for (int i = m_sp.m_nBegin; i < m_sp.m_nIndex - 1; i++)
			{
				bytePre = m_sp.m_buffer[i];
				if (nSTX < 0)
				{
					if (m_sp.m_buffer[i] == 0x02) //STX
					{
						if (bytePre == 0x10) //has DLE precede, not ture STX
							continue;
						nSTX = i + 1; //skip STX for result
					}
					else
					{
						continue;
					}
				}
				if (m_sp.m_buffer[i] == 0x03) //ETX
				{
					if (bytePre == 0x10) //has DLE precede, not ture ETX
						continue;
					nETX = i;
					break;
				}
			}
			if (nSTX < 0)
				return "";
			if (nETX <= nSTX)
				return "";

			string sRet = "";
			sRet = System.Text.Encoding.GetEncoding(1251).GetString(m_sp.m_buffer, nSTX, nETX - nSTX);
			m_sp.m_nBegin = nETX + 2;
			sendAck();

			string sCmdCode = sRet.Substring(0, 1);

			if (sCmdCode == "D") //require to display
				SendD00();

			string sDebug = sRet;
			string sh = "";
			for (int m = 0; m < sDebug.Length; m++)
			{
				sh += "0x" + ((int)sDebug[m]).ToString("X") + " ";
			}
			Program.g_log.Info("ingenico received " + sDebug.Length + " bytes:" + sDebug + " Hex:" + sh);
			return sRet;
		}
	}
}
