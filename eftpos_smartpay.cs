using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Specialized;
using Smartpay.Eftpos;
using Smartpay.Eftpos.Logging;

namespace QPOS2008
{
	public class EFTPOS_SMARTPAY
	{
		public string m_sStatus = "";
		public string m_sReceipt = "";
//		private ITerminalSession session;
		private string m_sMsg = "";
		public bool m_bError = false;
		private ISimplePaymentProvider provider;

		public EFTPOS_SMARTPAY()
		{
			Init();
		}
		private bool Init()
		{
			if(Program.m_sSmartpayIP == "")
			{
				MessageBox.Show("Please config terminal IP and Port");
				return false;
			}
			provider = new SimplePaymentProvider(Program.m_sSmartpayIP, Program.MyIntParse(Program.m_sSmartpayPort), LogEventLevel.Info);
			provider.OnLog += ProviderOnLog;
			provider.OnPrint += ProviderOnPrint;
			provider.OnResponse += ProviderOnResponse;
			
/*			try
			{
				ServerAgent.Initialise("GPOS", null);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return false;
			}
			session = ServerAgent.CreateSession();
			string terminalID = "GPOS-TERMINAL";
			string posLable = "GPOS-LANE";
			if (!session.Open(terminalID, posLable))
			{
				MessageBox.Show("open session failed");
				m_bError = true;
				return false;
			}
 */ 
			return true;
		}
#region oldversion		
/*		private void Close()
		{
			session.Close();
			ServerAgent.Finalise();
		}
		public void Purchase(double dPurchase, double dCashOut, string sRefNum)
		{
			if(!Init())
				return;
			NameValueCollection args = new NameValueCollection();
			args["Function"] = "Card.Purchase";
			args["Amount.Total"] = ((dPurchase + dCashOut) * 100).ToString();
			args["PosReference"] = sRefNum;
			
			ExecuteFunctionResult result = session.ExecuteFunction(args);
			args = result.ReturnValues;
			if(args != null)
			{
				m_sMsg = args["Result"];
				m_sReceipt = args["ReceiptData"];
			}
			Close();
		}
		public void Refund(double dAmount, string sRefNum)
		{
			if (!Init())
				return;
			NameValueCollection args = new NameValueCollection();
			args["Function"] = "Card.Refund";
			args["Amount.Total"] = ((0 - dAmount) * 100).ToString();
			args["PosReference"] = sRefNum;

			ExecuteFunctionResult result = session.ExecuteFunction(args);
			args = result.ReturnValues;
			if (args != null)
			{
				m_sMsg = args["Result"];
				m_sReceipt = args["ReceiptData"];
			}
			Close();
		}
		public void CashOut(double dAmount, string sRefNum)
		{
		}
		public string CheckResponse()
		{
			string sRet = m_sMsg;
			return sRet;
		}
 */ 
 #endregion
		public void Purchase(double dPurchase, double dCashOut, string sRefNum)
		{
			m_sMsg = "";
			uint purchaseAmount = (uint)(dPurchase * 100);
			uint cashoutAmount = (uint)(dCashOut * 100);

//			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
//			{
				if(dCashOut == 0)
				{
					WriteToOutput("Beginning Purchase");
					provider.Purchase(purchaseAmount, sRefNum);
				}
				else
				{
					WriteToOutput("Beginning Purchase + Cashout");
					provider.PurchasePlusCash(purchaseAmount, cashoutAmount, sRefNum);
				}
//			});
		} 
		public void Refund(double dAmount, string sRefNum)
		{
			m_sMsg = "";
			uint refundAmount = (uint)(dAmount * 100);
//			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
//			{
				WriteToOutput("Beginning Refund");
				provider.Refund(refundAmount, sRefNum);
//			});
		}
		public void Logon()
		{
			string sVersionString = "3.7";
			provider.Logon(sVersionString);
		} 
		public string CheckResponse()
		{
/*			string posRequestId = DateTime.Now.ToOADate().ToString();
			MessageObject request = provider.CreateRequest(Function.SettlementInquiry, posRequestId); 
			request[MessageField.Date] = DateTime.Now.ToString(); 
			provider.ProcessRequestAsync(request); 
*/			return m_sMsg;
		}
		private void ProviderOnLog(object sender, LogEventArgs e)
		{
			string msg = e.Text;
			Program.g_log.Info("smartpay log:" + e.Text);
		}
		private void ProviderOnPrint(object sender, PrintEventArgs e)
		{
			m_sReceipt = e.Text;
			Program.g_log.Info("smartpay onprint:" + e.Text);
		}
		private void ProviderOnResponse(object sender, ResponseEventArgs e)
		{
			// Close signature capture view if needed. This should already be 
			// closed except if the transaction was cancelled on the terminal 
			// before the signature capture was complete 
//			if (yourAppCapturesSignatures) 
//				EndSignatureCapture(); 
			Program.g_log.Info("smartpay res:" + e.Response[MessageField.TransactionResult] + e.Response[MessageField.ResultText]);
			if (e.Response[MessageField.Result] == Result.Ok) 
			{ 
				if (e.Response[MessageField.TransactionResult] == TransactionResult.OkAccepted) 
				{ 
//					DoSomethingOnSuccess(); 
					m_sMsg = "ACCEPTED";
					return; 
				} 
				else
				{ 
					m_sMsg = e.Response[MessageField.TransactionResult] + e.Response[MessageField.ResultText]; 
				} 
			} 
			else //failed
			{ 
//				yourAppLogger.Log("Payment interface failed: {0} ({1})",  
				m_sMsg = e.Response[MessageField.Result] + e.Response[MessageField.ResultText]; 
			} 
//			DoSomethingOnFail(); 		
		}
		

#region Private Methods
		private void SetControlsEnabledState(bool enabled)
		{
/*			if (InvokeRequired)
			{
				Invoke(new Action(delegate { SetControlsEnabledState(enabled); }));
			}
			else
			{
				gbTerminalConnection.Enabled = enabled;
				gbAcquirerFunctions.Enabled = enabled;
				gbCardFunctions.Enabled = enabled;
				gbJournalFunctions.Enabled = enabled;
				gbTerminalFunctions.Enabled = enabled;
			}
*/ 
		}
		private void PingTerminal()
		{
			bool result = false;
			string exceptionMessage = null;

			try
			{
				using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
				{
					System.Net.NetworkInformation.PingReply pingReply = ping.Send(Program.m_sSmartpayIP);
					result = (pingReply.Status == System.Net.NetworkInformation.IPStatus.Success);
				}
			}
			catch (Exception ex)
			{
				exceptionMessage = ex.Message;
			}
			MessageBox.Show(result.ToString());
/*
			MessageBox.Show(
				this,
				String.Format(
					"Result: {0}" + Environment.NewLine +
					"Additional Info: {1}",
					result ? "Success" : "Failed",
					exceptionMessage ?? "None"),
				"Name",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
 */ 
		}
		private void CallPaymentProvider(Action<ISimplePaymentProvider> paymentFunction)
		{
			try
			{
//				SetControlsEnabledState(false);
//				ISimplePaymentProvider provider =
//					new SimplePaymentProvider(tbIPAddress.Text, int.Parse(tbPort.Text), LogEventLevel.Debug);
//				provider.OnLog += WriteToLog;
//				provider.OnPrint += provider_OnPrint;
//				provider.OnResponse += provider_OnResponse;
				paymentFunction(provider);
			}
			catch (Exception ex)
			{
				WriteToOutput(ex.ToString());
			}
		}
/*		private void provider_OnResponse(object sender, ResponseEventArgs e)
		{
			WriteToOutput(String.Format("Response received: {0}", e.Response));

			SetControlsEnabledState(true);
		}

		private void provider_OnPrint(object sender, PrintEventArgs e)
		{
			WriteToOutput(String.Format("Print request received ({0}):{1}{2}", e.ReceiptType, Environment.NewLine, e.Text));
		}
*/
		private void WriteToOutput(string text)
		{
			Program.g_log.Info("smartpay output:" + text);
//			WriteToTextBox(tbOutput, text);
		}
		private void WriteToLog(object sender, LogEventArgs e)
		{
			Program.g_log.Info("smartpay log:" + e.Text);
//			WriteToTextBox(tbLogMessages, String.Format("{0}: {1}", e.Level, e.Text));
		}
/*		private void WriteToTextBox(TextBox tb, string text)
		{
			if (tb.InvokeRequired)
				tb.BeginInvoke(new Action(delegate { WriteToTextBox(tb, text); }));
			else
				tb.AppendText(text + Environment.NewLine);
		}
 */ 
		#endregion

		#region Button Click Handlers

		private void btnPing_Click(object sender, EventArgs e)
		{
			PingTerminal();
		}
		private void btnLogon_Click(object sender, EventArgs e)
		{
			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
			{
				string posVersionString =
					"SLDemo" + Application.ProductVersion.Replace(".", "");

				WriteToOutput("Beginning Logon");

				provider.Logon(posVersionString);
			});
		}
/*		private void btnSettlementInquiry_Click(object sender, EventArgs e)
		{
			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
			{
				MessageObject request = provider.CreateRequest(Function.SettlementInquiry);

				WriteToOutput("Beginning SettlementInquiry");

				provider.ProcessRequestAsync(request);
			});
		}
		private void btnSettlementCutover_Click(object sender, EventArgs e)
		{
			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
			{
				MessageObject request = provider.CreateRequest(Function.SettlementCutover);

				WriteToOutput("Beginning SettlementCutover");

				provider.ProcessRequestAsync(request);
			});
		}
		private void btnClearAmounts_Click(object sender, EventArgs e)
		{
			tbPurchaseAmount.Text = string.Empty;
			tbPurchaseAmount.BackColor = SystemColors.Window;

			tbCashAmount.Text = string.Empty;
			tbCashAmount.BackColor = SystemColors.Window;
		}
		private void btnClearMessages_Click(object sender, EventArgs e)
		{
			tbOutput.Clear();
			tbLogMessages.Clear();
		}
		private void btnPurchase_Click(object sender, EventArgs e)
		{
			uint purchaseAmount = 0;

			if (GetAmount(tbPurchaseAmount, out purchaseAmount))
			{
				CallPaymentProvider(delegate(ISimplePaymentProvider provider)
				{
					WriteToOutput("Beginning Purchase");

					provider.Purchase(purchaseAmount);
				});
			}
		}
		private void btnPurchasePlusCash_Click(object sender, EventArgs e)
		{
			uint purchaseAmount = 0;
			uint cashAmount = 0;

			bool success = 
				GetAmount(tbPurchaseAmount, out purchaseAmount, 0.01M) &&
				GetAmount(tbCashAmount, out cashAmount, 0.01M);

			if (success)
			{
				CallPaymentProvider(delegate(ISimplePaymentProvider provider)
				{
					WriteToOutput("Beginning Purchase+Cash");

					provider.PurchasePlusCash(purchaseAmount, cashAmount);
				});
			}
		}

		private void btnCashOnly_Click(object sender, EventArgs e)
		{
			uint cashAmount = 0;

			if (GetAmount(tbCashAmount, out cashAmount, 0.01M))
			{
				CallPaymentProvider(delegate(ISimplePaymentProvider provider)
				{
					WriteToOutput("Beginning CashAdvance");

					provider.CashAdvance(cashAmount);
				});
			}
		}

		private void btnRefund_Click(object sender, EventArgs e)
		{
			uint refundAmount;

			if (GetAmount(tbPurchaseAmount, out refundAmount, 0.01M))
			{
				CallPaymentProvider(delegate(ISimplePaymentProvider provider)
				{
					WriteToOutput("Beginning Refund");

					provider.Refund(refundAmount);
				});
			}
		}

		private void btnGetLastTransactionResult_Click(object sender, EventArgs e)
		{
			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
			{
				WriteToOutput("Beginning GetTransactionResult");

				provider.GetTransactionResult();
			});
		}

		private void btnReprintLastReceipt_Click(object sender, EventArgs e)
		{
			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
			{
				WriteToOutput("Beginning ReprintReceipt");

				provider.ReprintReceipt();
			});
		}

		private void btnGetStatus_Click(object sender, EventArgs e)
		{
			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
			{
				MessageObject request = provider.CreateRequest(Function.GetStatus);

				request[MessageField.StatusType] = StatusType.Terminal;

				WriteToOutput("Beginning GetStatus");

				provider.ProcessRequestAsync(request);
			});
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			CallPaymentProvider(delegate(ISimplePaymentProvider provider)
			{
				WriteToOutput("Beginning PrintOnTerminal");

				provider.PrintOnTerminal(SampleText);
			});
		}
*/
		# endregion

	}
}
