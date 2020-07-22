using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Alipay;
using System.Xml.Linq;
using System.Linq;

namespace QPOS2008
{
	public partial class FormAlipayDirect : Form
	{
		public bool m_bSuccess = false;
		public double m_dAmount = 0.01;
		public string m_sTradeNumber = "";
		public string m_sPaymentType = "AlipayDirect";
		private string m_sBuyerId = "";

		public FormAlipayDirect()
		{
			InitializeComponent();
		}
		private void FormAlipayDirect_Load(object sender, EventArgs e)
		{
			txtBarcode.Focus();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void txtBarcode_TextChanged(object sender, EventArgs e)
		{
		}
		private void txtBarcode_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				m_sBuyerId = txtBarcode.Text.Trim();
				if (m_sBuyerId == "")
				{
					Program.MsgBox("Please scan customer Barcode.");
					return;
				}
				this.Cursor = Cursors.WaitCursor;
				txtBarcode.Cursor = Cursors.WaitCursor;
				timer1.Interval = 500;
				timer1.Start();
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Stop();
			DoAlipayDirect(Program.m_sAlipayDirectPartnerID, m_sTradeNumber, m_sBuyerId, m_dAmount);
			txtBarcode.Cursor = Cursors.Default;
			this.Cursor = Cursors.Default;
			this.Close();
		}
		private bool DoAlipayDirect(string sPartnerId, string sInvNumber, string sBuyerBarcode, double dAmount)
		{
			bool bRet = false;
			//Same value with partner ID
			string alipay_seller_id = Program.m_sAlipayDirectPartnerID;
			//Quantity of commodity
			string quantity = "1";// WIDquantity.Text.Trim();
								  //The name of the transaction which will be shown in the transaction record’s list.
			string trans_name = "store_sales";// WIDtrans_name.Text.Trim();
											  //The transaction Id on the partner system which could be a sale order id and payment order id.
			string partner_trans_id = sInvNumber;// WIDpartner_trans_id.Text.Trim();
											   //The currency used for labelling the price of the transaction. this is also the settlement currency Alipay settled to the partner.
			string currency = "NZD";
			//the transaction amount in the currency given above;
			string trans_amount = Math.Round(dAmount, 2).ToString();
			//Used as identification of an Alipay user.
			string buyer_identity_code = m_sBuyerId;
			//identity_code_type could be QRcode or barcode
			string identity_code_type = "barcode";
			//biz_data The time that the partner system creates the transaction.Format：YYYYMMDDHHMMSS
			string trans_create_time = DateTime.Now.ToString("yyyyMMddHHmmss");
			//Transaction notes
			string memo = "";
			//biz_product Product name. For now it’s an static value which is mandatory Value: OVERSEAS_MBARCODE_PAY
			string biz_product = "OVERSEAS_MBARCODE_PAY";
			string extend_info = "";

			////////////////////////////////////////////////////////////////////////////////////////////////

			//把请求参数打包成数组
			SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
			sParaTemp.Add("partner", alipay_seller_id);
			sParaTemp.Add("alipay_seller_id", alipay_seller_id);
			sParaTemp.Add("quantity", quantity);
			sParaTemp.Add("service", "alipay.acquire.overseas.spot.pay");
			sParaTemp.Add("trans_name", trans_name);
			sParaTemp.Add("partner_trans_id", partner_trans_id);
			sParaTemp.Add("currency", currency);
			sParaTemp.Add("trans_amount", trans_amount);
			sParaTemp.Add("buyer_identity_code", buyer_identity_code);
			sParaTemp.Add("identity_code_type", identity_code_type);
			sParaTemp.Add("trans_create_time", trans_create_time);
			sParaTemp.Add("memo", memo);
			sParaTemp.Add("biz_product", biz_product);
			sParaTemp.Add("extend_info", extend_info);
			sParaTemp.Add("_input_charset", "utf-8");

			string sHtmlText = "";
			try
			{
				sHtmlText = Submit.BuildRequest(Program.m_sAlipayDirectUriRequest, Program.m_sAlipayDirectKey, sParaTemp);
			}
			catch(Exception e)
			{
				Program.g_log.Log("Error connection to Alipay Server\r\ne=" + e.ToString());
				bRet = CheckStatus(partner_trans_id);
				Program.MsgBox("Payment Failed, network error.\r\n");
				return bRet;
			}
			Dictionary<string, string> dc = new Dictionary<string, string>();
			try
			{
				XDocument doc = XDocument.Parse(sHtmlText);
				foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
				{
					int keyInt = 0;
					string keyName = element.Name.LocalName;
					while (dc.ContainsKey(keyName))
					{
						keyName = element.Name.LocalName + "_" + keyInt++;
					}
					dc.Add(keyName, element.Value);
				}
			}
			catch (Exception e)
			{
				Program.MsgBox("Parsing XML error, please check response:\r\n" + sHtmlText);
				return false;
			}
			string is_success = "";
			string error = "";
			string result_code = "";
			if(dc.ContainsKey("is_success"))
				is_success = dc["is_success"];
			if(dc.ContainsKey("error"))
				error = dc["error"];
			if(dc.ContainsKey("result_code"))
				result_code = dc["result_code"];

			if (is_success == "T" && result_code == "SUCCESS")
			{
				m_bSuccess = true;
				return true;
			}
			if ( (is_success == "F" && error == "SYSTEM_ERROR")
				|| (is_success == "T" && result_code == "FAILE" && error == "SYSTEM_ERROR")
				|| (is_success == "T" && result_code == "UNKNOW"))
			{
				bRet = CheckStatus(partner_trans_id);
				Program.MsgBox("Payment Failed\r\n" + error);
				return bRet;
			}
			Program.MsgBox("Unkown Result\r\nis_success:" + is_success + "\r\nerror:" + error + "\r\nresult_code:" + result_code);
			Program.g_log.Log("unhandled alipayDirect, html=" + sHtmlText);
			return true;
		}
		private bool CheckStatus(string partner_trans_id)
		{

			SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
			sParaTemp.Add("partner", Program.m_sAlipayDirectPartnerID);
			sParaTemp.Add("service", "alipay.acquire.overseas.query");
			sParaTemp.Add("_input_charset", "utf-8");
			sParaTemp.Add("partner_trans_id", partner_trans_id);

			string sHtmlText = "";
			try
			{
				sHtmlText = Submit.BuildRequest(Program.m_sAlipayDirectUriRequest, Program.m_sAlipayDirectKey, sParaTemp);
			}
			catch (Exception e)
			{
				Program.g_log.Log("Error connection to Alipay Server\r\ne=" + e.ToString());
				return false;
			}
			Dictionary<string, string> dc = new Dictionary<string, string>();
			try
			{
				XDocument doc = XDocument.Parse(sHtmlText);
				foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
				{
					int keyInt = 0;
					string keyName = element.Name.LocalName;
					while (dc.ContainsKey(keyName))
					{
						keyName = element.Name.LocalName + "_" + keyInt++;
					}
					dc.Add(keyName, element.Value);
				}
			}
			catch (Exception e)
			{
				Program.MsgBox("Parsing XML error, please check response:\r\n" + sHtmlText);
				return false;
			}
			string status = "";
			if (dc.ContainsKey("alipay_trans_status"))
				status = dc["alipay_trans_status"];
			if (status == "TRADE_SUCCESS")
			{
				m_bSuccess = true;
				return true;
			}
			else
			{
				Program.g_log.Log("query status result:" + status + ", html=" + sHtmlText);
				return CancelTransaction(partner_trans_id);
			}
			return false;
		}
		private bool CancelTransaction(string partner_trans_id)
		{
			SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
			sParaTemp.Add("partner", Program.m_sAlipayDirectPartnerID);
			sParaTemp.Add("service", "alipay.acquire.cancel");
			sParaTemp.Add("_input_charset", "utf-8");
			sParaTemp.Add("out_trade_no", partner_trans_id);

			string sHtmlText = "";
			try
			{
				sHtmlText = Submit.BuildRequest(Program.m_sAlipayDirectUriRequest, Program.m_sAlipayDirectKey, sParaTemp);
			}
			catch (Exception e)
			{
				Program.g_log.Log("Error connection to Alipay Server\r\ne=" + e.ToString());
				return false;
			}
			Dictionary<string, string> dc = new Dictionary<string, string>();
			try
			{
				XDocument doc = XDocument.Parse(sHtmlText);
				foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
				{
					int keyInt = 0;
					string keyName = element.Name.LocalName;
					while (dc.ContainsKey(keyName))
					{
						keyName = element.Name.LocalName + "_" + keyInt++;
					}
					dc.Add(keyName, element.Value);
				}
			}
			catch (Exception e)
			{
				Program.MsgBox("Parsing XML error, please check response:\r\n" + sHtmlText);
				return false;
			}
			string is_success = "";
			string error = "";
			string result_code = "";
			if (dc.ContainsKey("is_success"))
				is_success = dc["is_success"];
			if (dc.ContainsKey("error"))
				error = dc["error"];
			if (dc.ContainsKey("result_code"))
				result_code = dc["result_code"];

			if (is_success == "T" && result_code == "SUCCESS")
			{
				Program.g_log.Log("transaction " + partner_trans_id + " successfully cancelled.");
				return true;
			}
			else
			{
				Program.g_log.Log("cancel transaction + " + partner_trans_id + " failed, html=" + sHtmlText);
				return false;
			}
			return false;
		}
	}
}
