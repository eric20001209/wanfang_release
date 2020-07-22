using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QPOS2008
{
	public class EFTPOS_VLLINK
	{
		public string m_sStatus = "";

		private MySerialPort m_sp = null;
		private string m_sRef = "";
		private bool m_bWaitForResponse = false;
	  
		public EFTPOS_VLLINK()
		{
			m_sp = new MySerialPort();
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
		private void SendMsg(string msg)
		{
			byte[] buf = System.Text.Encoding.ASCII.GetBytes(msg);
			m_sp.SendCmd(buf, 0, buf.Length);
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
			string sPurchaseAmount = ((int)(dPurchaseAmount * 100)).ToString("D8");
			string sCashoutAmount = ((int)(dCashOutAmount * 100)).ToString("D8");
			string sRefundAmount = ((int)(0 - dPurchaseAmount) * 100).ToString("D8");

			string msg = "[" + cmd + ",0,";
			if(cmd == "PR")
				msg += sPurchaseAmount;
			else if(cmd == "PC")
				msg += sPurchaseAmount + "," + sCashoutAmount;
			else if(cmd == "CO")
				msg += sCashoutAmount;
			else if(cmd == "RF")
				msg += sRefundAmount;
			msg += "]";
			SendMsg(msg);
		}
		public void GetResult()
		{
			string msg = "[R?]";
			SendMsg(msg);
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
			DoTCmd(dPurchase, dCashOut, sRefNum);
		}
		public string CheckResponse()
		{
			if(m_bWaitForResponse)
			{
				if(m_sp.m_nIndex <= 1)
					return "";
				string sRet = System.Text.Encoding.ASCII.GetString(m_sp.m_buffer, 0, m_sp.m_nIndex);
				if(sRet.IndexOf(']') < 0)
					return "";
				m_bWaitForResponse = false;
				return sRet;
			}
			else
			{
				GetResult();
				m_bWaitForResponse = true;
				return "";
			}
		}
	}
}
