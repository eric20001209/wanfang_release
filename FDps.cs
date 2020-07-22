using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FDps : Form
	{
		public string m_sSettlement = "";
		public bool m_bSettlement = false;
		public bool m_bAuthorized = false;
		public string m_sMsg = "";
		public string m_sReceipt = "";

		public double m_dPurchase = 0;
		public double m_dRefund = 0;
		public double m_dCashOut = 0;
		public string m_sRef = "";

		public FDps()
		{
			InitializeComponent();
			this.Top = 3000;
		}
		private void FDps_Load(object sender, EventArgs e)
		{
			this.Left = -16348;
			axDpsEftX.AuthorizeEvent += new EventHandler(this.axDpsEftX_AuthorizeEvent);
			if (m_dPurchase > 0 || m_dCashOut > 0)
				Purchase(m_dPurchase, m_dCashOut, m_sRef);
			else if (m_dPurchase < 0)
				Refund(0 - m_dPurchase, "");
		}
		public void Purchase(double dPurchase, double dCashOut, string sRefNum)
		{
			m_dCashOut = dCashOut;
			DoTCmd(dPurchase, 0,dCashOut, sRefNum, "Purchase");
		}
		public void Refund(double dAmount, string sRefNum)
		{
Program.g_log.Info("DPS Refund, amount=" + dAmount.ToString());
			DoTCmd(0, dAmount, 0, sRefNum, "Refund");
		}
		public void CashOut(double dAmount, string sRefNum)
		{
			m_dCashOut = dAmount;
			DoTCmd(0, 0, dAmount, sRefNum, "Purchase");
		}
		public void DoSettlementCutOver()
		{
			axDpsEftX.Parameter1 = "Cutover";
			axDpsEftX.DoSettlement();
		}
		private void DoTCmd(double dDueTotal, double dRefund, double dCashOut, string sRef, string sTransactionType)
		{
            axDpsEftX.EnablePrintReceipt = false;
            axDpsEftX.EnablePrintSlip = true;
			axDpsEftX.EnableSavingAccount = 1;
//          axDpsEftX.ReceiptIsSeparate = false;

			axDpsEftX.Amount = dDueTotal.ToString();
			if(sTransactionType == "Refund")
				axDpsEftX.Amount = dRefund.ToString();
			axDpsEftX.AmountCashOut = dCashOut.ToString();
			axDpsEftX.TxnType = sTransactionType;
			axDpsEftX.TxnRef = sRef;

			m_sMsg = "";
Program.g_log.Info("DPS DoAuthorize, TxnRef=" + sRef + ", Amount=" + axDpsEftX.Amount + ", TxnType=" + axDpsEftX.TxnType);
			axDpsEftX.DoAuthorize();
		}
		private void axDpsEftX_StatusChangedEvent(object sender, EventArgs e)
		{
			Program.g_log.Info("DPS Status Changed, Ready=" + axDpsEftX.Ready.ToString() + ", ReadyLink=" + axDpsEftX.ReadyLink.ToString() + ", ReadyPinPad=" + axDpsEftX.ReadyPinPad.ToString());
			if (!axDpsEftX.Ready)
			{
				string s = "EFTPOS Error\r\n" + axDpsEftX.StatusText;
				Program.MsgBox(s);
				this.Close();
			}
		}
		private void axDpsEftX_AuthorizeEvent(object sender, EventArgs e)
		{
			if(CheckResponse())
				this.Close();
		}
		private bool CheckResponse()
		{
			if (m_bSettlement)
				m_sSettlement = axDpsEftX.ResponseText;
			if(axDpsEftX.TxnRef != m_sRef)
			{
				m_bAuthorized = false;
				m_sMsg = "Last Transaction recovered, please hit Enter again.";
Program.g_log.Info("DPS Response dropped, TxnRef different:" + m_sRef + ":" + axDpsEftX.TxnRef + ", authorized=" + m_bAuthorized.ToString() + ", msg=" + m_sMsg);
				return true;
			}
			m_bAuthorized = axDpsEftX.Authorized;
			m_sMsg = axDpsEftX.ResponseText;
Program.g_log.Info("DPS Response, TxnRef=" + axDpsEftX.TxnRef + ", authorized=" + m_bAuthorized.ToString() + ", msg=" + m_sMsg);
			if(!m_bAuthorized)
				return true;

			double m_iCardNumberLength = Program.MyDoubleParse(axDpsEftX.CardNumber.Length.ToString());
			double m_iCardNumberFront = m_iCardNumberLength - 4;
			string m_iLastFourDigit = "";//
			if(axDpsEftX.CardNumber.Length > 4)
				m_iLastFourDigit = axDpsEftX.CardNumber.Substring(int.Parse(m_iCardNumberFront.ToString()), 4);
			string m_sPurchase = "NZD " + (Program.MyDoubleParse(axDpsEftX.Amount)).ToString("N2");
			string m_sCashOut = "NZD " + (Program.MyDoubleParse(axDpsEftX.AmountCashOut)).ToString("N2");
			double m_dTotalAmountTrans = Program.MyDoubleParse(axDpsEftX.Amount) + Program.MyDoubleParse(axDpsEftX.AmountCashOut);
			m_dTotalAmountTrans = Math.Round(m_dTotalAmountTrans, 2);
			string m_sTotalAmountTrans = "NZD " + (m_dTotalAmountTrans).ToString("N2");
			int m_dPurchaseLength = m_sPurchase.Length;
			double m_dMaxLine = 30;
			double m_dSpacing = m_dMaxLine - Program.MyDoubleParse(m_dPurchaseLength.ToString()) - axDpsEftX.TxnType.ToUpper().Length;
			double m_dSpacingTotal = m_dMaxLine - (Program.MyDoubleParse(m_sTotalAmountTrans.Length.ToString()));
			double m_dSpacingCashout = m_dMaxLine - m_sCashOut.Length;

			m_sReceipt = "*---------- EFTPOS ----------*" + "\r\n";
			m_sReceipt += axDpsEftX.Receipt.Substring(150, 30) + "\r\n";
			m_sReceipt += axDpsEftX.Receipt.Substring(180, 30) + "\r\n";
			m_sReceipt += axDpsEftX.CardType + "                  " + "...." + m_iLastFourDigit + "\r\n";

			m_sReceipt += axDpsEftX.Receipt.Substring(240, 30) + "\r\n";
			m_sReceipt += axDpsEftX.TxnType.ToUpper();
			for (int s = 0; s < m_dSpacing; s++)
			{
				m_sReceipt += " ";
			}
			m_sReceipt += m_sPurchase + "\r\n";

			if (m_dCashOut > 0)
			{
				m_sReceipt += "CASH OUT";
				for (int s = 0; s < m_dSpacingCashout - 8; s++)
				{
					m_sReceipt += " ";
				}
				m_sReceipt += m_sCashOut + "\r\n";
			}

			m_sReceipt += "TOTAL";
			for (int s = 0; s < m_dSpacingTotal - 5; s++)
			{
				m_sReceipt += " ";
			}
			m_sReceipt += m_sTotalAmountTrans + "\r\n";
			m_sReceipt += "            " + axDpsEftX.ResponseText + "        " + "\r\n";
			m_sReceipt += "*----------------------------*" + "\r\n";
			return true;
		}
	}
}
