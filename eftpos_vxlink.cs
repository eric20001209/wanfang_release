using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QPOS2008
{
	public class EFTPOS_VXLINK
	{
		public string m_sStatus = "";
		public string m_sReceipt = "";

		private MySerialPort m_sp = null;
		private string m_sRef = "";
//		private bool m_bWaitForResponse = false;
	  
		public EFTPOS_VXLINK()
		{
			m_sp = new MySerialPort();
			m_sp.PortNumber = "COM" + Program.m_sEftposCom;
			if (!m_sp.Open())
			{
				m_sStatus = "Open port failed";
			}
		}
		public void ConfigurePrinting()
		{
//			m_bWaitForResponse = true;
			SendMsg("CP?,ON"); //configure printing
		}
		public void ClosePort()
		{
			m_sp.Close();
		}
		private void SendMsg(string msg)
		{
			byte[] Len = BitConverter.GetBytes((short)(msg.Length));
			byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
			byte[] buf = new byte[1024];
			int lrc = 0;
			int i = 0;
			buf[i++] = (byte)'V';
			buf[i++] = (byte)'2';
			buf[i++] = Len[1];
			buf[i++] = Len[0];
			for (int j = 0; j < data.Length; j++)
			{
				buf[i++] = data[j];
				lrc = lrc ^ data[j];
			}
			buf[i++] = (byte)lrc;
			m_sp.SendCmd(buf, 0, i);
			string sDebug = System.Text.Encoding.ASCII.GetString(buf, 0, i);
			string sh = "";
			for (int m = 0; m < sDebug.Length; m++)
			{
				sh += "0x" + ((int)sDebug[m]).ToString("X") + " ";
			}
			Program.g_log.Info("vxlink sent " + i + " bytes:" + sDebug + " Hex:" + sh);
		}
		private void DoTCmd(double dPurchaseAmount, double dCashOutAmount, string sRefNum)
		{
			string cmd = "PR";
			if(dPurchaseAmount < 0)
			{
				cmd = "RF";
			}
			else if(dCashOutAmount > 0)
			{
				if(dPurchaseAmount == 0)
					cmd = "CO";
				else if(dPurchaseAmount > 0)
					cmd = "PC";
			}
			m_sRef = sRefNum;
			int nRefNumLength = sRefNum.Length;
			string sPurchaseAmount = ((int)(dPurchaseAmount * 100)).ToString();
			string sCashoutAmount = ((int)(dCashOutAmount * 100)).ToString();
			string sRefundAmount = ((int)(0 - dPurchaseAmount) * 100).ToString();

			string msg = cmd + "," + sRefNum + ",0,";
			if(cmd == "PR")
				msg += sPurchaseAmount;
			else if(cmd == "PC")
				msg += sPurchaseAmount + "," + sCashoutAmount;
			else if(cmd == "CO")
				msg += sCashoutAmount;
			else if(cmd == "RF")
				msg += sRefundAmount;
			SendMsg(msg);
		}
		public void GetResult()
		{
			string msg = "RS?," + m_sRef + ",0";
			SendMsg(msg);
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
		public void Purchase(double dPurchase, double dCashOut, string sRefNum)
		{
			m_sReceipt = "";
			DoTCmd(dPurchase, dCashOut, sRefNum);
		}
		public string CheckResponse()
		{
			if (m_sp.m_nIndex <= 0 || m_sp.m_nIndex - m_sp.m_nBegin < 5)
			{
				GetResult();
				return "";
			}

			//get payload length
			byte[] buf = new byte[4];
			buf[3] = 0x0;
			buf[2] = 0x0;
			buf[1] = m_sp.m_buffer[m_sp.m_nBegin + 2];
			buf[0] = m_sp.m_buffer[m_sp.m_nBegin + 3];
			int nLen = BitConverter.ToInt32(buf, 0);
			
			int nBegin = m_sp.m_nBegin + 4; //message id 2 bytes(V2), payload length 2 bytes
//			int nEnd = m_sp.m_nIndex - 1;	//LRC 1 byte
			int nEnd = nBegin + nLen + 1;	//LRC 1 byte

			string sBuf = System.Text.Encoding.ASCII.GetString(m_sp.m_buffer, 0, m_sp.m_nIndex);
			string sh = "";
			for (int i = 0; i < sBuf.Length; i++)
			{
				sh += "0x" + ((int)sBuf[i]).ToString("X") + " ";
			}
			Program.g_log.Info("com port buffer begin:" + m_sp.m_nBegin.ToString() + ", end: " + nEnd.ToString() + " nIndex: " + m_sp.m_nIndex.ToString() + ", Payload Length=" + nLen.ToString() + ", buffer ASCII=" + sBuf + ", HEX=" + sh);

			string sRet = "";
			if(m_sp.m_nIndex < nEnd) //not enough
			{
				Program.g_log.Info("received " + (m_sp.m_nIndex - nBegin - 1).ToString() + " bytes, expecting " + nLen.ToString());
				return "";
			}
			else if(m_sp.m_nIndex == nEnd) //just right
			{
				sRet = System.Text.Encoding.ASCII.GetString(m_sp.m_buffer, nBegin, nEnd - nBegin - 1);
				m_sp.m_nBegin = 0;
				m_sp.m_nIndex = 0;
			}
			else //more than one
			{
//				nEnd = nBegin + nLen;
//				m_sp.m_nBegin += nLen + 5;
				m_sp.m_nBegin = nEnd;
				sRet = System.Text.Encoding.ASCII.GetString(m_sp.m_buffer, nBegin, nEnd - nBegin - 1);
				string sRet2 = System.Text.Encoding.ASCII.GetString(m_sp.m_buffer, m_sp.m_nBegin, m_sp.m_nIndex - m_sp.m_nBegin);
				Program.g_log.Info("received more than one response, 1st:" + sRet + ", 2nd:" + sRet2);
			}
			
			//get payload text
//			if (nEnd - nBegin <= 0)
//				return "";
			
			sh = "";
			for (int i = 0; i < sRet.Length; i++)
			{
				sh += "0x" + ((int)sRet[i]).ToString("X") + " ";
			}
			Program.g_log.Info("vxlink received " + sRet.Length + " bytes:" + sRet + " Hex:" + sh);

			if (sRet.IndexOf("CP,ON") >= 0)
			{
				return "PRINTER OK";
			}
			else if (sRet.IndexOf("RP?") >= 0) //ready to print?
			{
				SendMsg("RP,OK");
				return "";
			}
			else if (sRet.IndexOf("PT?,") >= 0)
			{
				if (sRet.IndexOf("DECLINED") >= 0)
				{
					m_sReceipt = sRet.Substring(8, sRet.Length - 9);
					Program.g_log.Info("declined, print text:" + m_sReceipt);
					SendMsg("PT,OK");
					return "DECLINED";
				}
				if (m_sReceipt == "") //drop merchant copy, keep first copy(customer)
				{
					m_sReceipt = sRet.Substring(8, sRet.Length - 9);
					Program.g_log.Info("accepted, print text:" + m_sReceipt);
				}
				SendMsg("PT,OK");
				return "";
			}
			return sRet;
		}
	}
}
