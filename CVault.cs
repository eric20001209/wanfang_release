using System;
using System.Collections.Generic;
using System.Text;
using Sektor.Vault.Common;
using Sektor.Vault.POSInterface;

namespace QPOS2008
{
	public class CVault
	{
		private VaultSession vs;
		public static string m_sReceipt = "";
//		private VaultSession vs = new VaultSession("CHECKOUT1", OnPrint);
		
		private static bool OnPrint(string text, PrintType printType)
		{
			bool bPrintOK = true;
			m_sReceipt = text;
			return bPrintOK;
		}
		public void OpenSession()
		{
			vs = new VaultSession("CHECKOUT1");
		}
		public void CloseSession()
		{
			vs.Dispose();
		}
		public string GetReceiptText()
		{
			return m_sReceipt;
		}
		public string Purchase(double dPurchase, double dCashout, double dCheque, string sRef)
		{
//			using (VaultSession vs = new VaultSession("CHECKOUT1"))
			{
				sRef += DateTime.Now.ToOADate().ToString();
				TransactionResult ret = new TransactionResult();

				if(dPurchase > 0)
				{
					PurchaseTransaction txp = new PurchaseTransaction(sRef, (decimal)(dPurchase));
					if(dCashout > 0)
						txp.CashOutAmount = (decimal)dCashout;
					ret = vs.ExecuteTransaction(txp);

					// Check to see if the result is Unknown - note that ExecuteTransaction 
					// will only ever return TransactionResult.Unknown if the session is 
					// reset or if the VaultSession.EnableAutoComplete property is false. 
					// If the POS Application does not call reset session and leaves 
					// auto-completion enabled then this test is not necessary. 
					while (ret == TransactionResult.Unknown)
					{
						ret = vs.CompleteTransaction(txp);
					}
				}
				else if(dPurchase < 0)
				{
					RefundTransaction txr = new RefundTransaction(sRef, (decimal)(0 - dPurchase));
					ret = vs.ExecuteTransaction(txr);
				}
				else if(dCheque > 0)
				{
					ChequeAuthTransaction txca = new ChequeAuthTransaction(sRef, (decimal)dCheque);
					ret = vs.ExecuteTransaction(txca);
				}
				else
				{
					return "INVALID AMOUNT";
				}
				if (ret == TransactionResult.Success)
				{
					return "ACCEPTED";
				}
			}
			return "FAILED";
		}
		
		/// <summary> 
		/// Shows how to get the result of a transaction after a power/link failure. 
		/// </summary> 
		/// <param name="session">The Vault session to use.</param> 		
		public void HandlePowerLinkFailure()
		{
//			VaultSession session = new VaultSession("CHECKOUT1");
//			using (VaultSession vs = new VaultSession("CHECKOUT1"))
			{
//				FinancialTransaction tx = GetCurrentTransaction();
				UnknownVaultTransaction tx = GetCurrentTransaction();
				if (tx != null)
				{
					TransactionResult result = vs.CompleteTransaction((VaultTransaction)tx);
					if (result == TransactionResult.Success)
					{
						// Close the sale - customer can leave with goods 
//						Program.MsgBox("PowerLinkFailure detected, last EFTPOS transaction ACCEPTED");
					}
					else
					{
						// Leave the sale open - customer will have to retender. 
//						Program.MsgBox("PowerLinkFailure detected, last EFTPOS transaction not finished, result:" + result.ToString());
					}
				}
				else
				{
	//				Program.MsgBox("Vault OK");
					// There was no transaction pending during the power/link failure 
				}
			}
		}
		/// <summary> 
		/// This method returns the currently executing transaction (if any). 
		/// </summary> 
		/// <returns>Returns null if no transaction pending.</returns> 
		public UnknownVaultTransaction GetCurrentTransaction()
		{
			// The code below is shows how to create the transaction object 
			// ready for use in the CompleteTransaction method. In real 
			// life this code would retrieve the transaction 
			// ID from a persistent store. 
			UnknownVaultTransaction ut = new UnknownVaultTransaction("#10");
//			if(ut.TxInfo == null)
//				return null;
			return ut;
//			return (FinancialTransaction)ut;
		}
	}
}
