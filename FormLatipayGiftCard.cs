using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace QPOS2008
{
	public partial class FormLatipayGiftCard : Form
	{
		private SqlConnection myConnection;
		private SqlCommand myCommand;
		DataSet dst = new DataSet();

		public string m_sTillNumber = "1";
		public string m_sCartNumber = "1";
		public string[] m_sVerifiedCardCode;
		public string[] m_sVerifiedCardFaceValue;
		private string m_sUserId = Program.m_sLatiPayUserID;
		private string m_sWalletId = Program.m_sLatiPayWalletID;
		private string m_sApiKey = Program.m_sLatiPayApiKey;
		private string m_sMerchantReference = "";
		private string m_sGiftCardCode = "";
		private string m_sFaceValue = "";

		public FormLatipayGiftCard()
		{
			InitializeComponent();
		}
		private void FormLatipayGiftCard_Load(object sender, EventArgs e)
		{
			txtCardNumber.Focus();
			btnUse.Visible = false;
			lblVerifiedCardCode.Text = "None";
			lblFaceValue.Text = "None";
			lblStatus.Text = "None";
			ShowVerifiedCardList();
		}
		private void btnVerify_Click(object sender, EventArgs e)
		{
			string cardNumber = txtCardNumber.Text.Trim();
			if (cardNumber == "")
			{
				MessageBox.Show("Please scan gift card number.");
				return;
			}
			VerifyGiftCard(cardNumber);
		}
		private void txtCardNumber_KeyUp(object sender, KeyEventArgs e)
		{
		    if (e.KeyCode == Keys.Enter)
		    {
				btnVerify_Click(null, null);
		    }
		}
		private void VerifyGiftCard(string cardNumber)
		{
			if (Program.m_bEnableLatiPayGiftCard)
			{
				if (Program.m_sLatiPayApiKey == "" || Program.m_sLatiPayWalletID == "" || Program.m_sLatiPayUserID == "")
				{
					Program.MsgBox("Latipay is not set up, please contact your dealer.");
					return;
				}
			}
			else
				return;

			string host = Program.m_sLatiPayUrlGiftCardFreeze;
			string user_id = m_sUserId;
			string wallet_id = m_sWalletId;
			string merchant_reference = DateTime.Now.ToString("yyyyMMdd-hhmmss");
			string gift_card_code = cardNumber;
			string freeze_type = "freeze";
			string apiKey = m_sApiKey;

			string message = "freeze_type=" + freeze_type;
			message += "&gift_card_code=" + gift_card_code;
			message += "&merchant_reference=" + merchant_reference;
			message += "&user_id=" + user_id;
			message += "&wallet_id=" + wallet_id;

			byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
			byte[] secret = Encoding.UTF8.GetBytes(apiKey);
			byte[] SHA256HMACSignature = HashHMAC(secret, msg);
			string signature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower();

			string json = "{\"user_id\": \"" + user_id + "\",";
			json += "\"wallet_id\": \"" + wallet_id + "\",";
			json += "\"merchant_reference\": \"" + merchant_reference + "\",";
			json += "\"gift_card_code\": \"" + gift_card_code + "\",";
			json += "\"freeze_type\": \"" + freeze_type + "\",";
			json += "\"signature\": \"" + signature + "\"}";

			string sRet = Http.post(host, json);
			if (sRet == null)
			{
				MessageBox.Show("network anomaly", "prompt");
				return;
			}
			int p = 0;
			string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
			string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();
			string rFaceValue = JsonGetSimpleValue(sRet, "\"face_value\":", ref p);

			lblVerifiedCardCode.Text = cardNumber;
			lblStatus.Text = rMessage;

			if (rMessage == "success")
			{
				btnUse.Visible = true;
				lblFaceValue.Text = rFaceValue;
				m_sMerchantReference = merchant_reference;
				m_sGiftCardCode = gift_card_code;
				m_sFaceValue = rFaceValue;
			}
			else
			{
				btnUse.Visible = false;
				lblFaceValue.Text = "None";
				m_sMerchantReference = "";
				m_sGiftCardCode = "";
				m_sFaceValue = "";
			}
			return;
		}
        private void btnClose_Click(object sender, EventArgs e)
        {
			if (GetVerifiedCard())
				this.Close();
			else
				MessageBox.Show("Close error,try again please.");
        }
		private bool GetVerifiedCard()
		{
			string sTableName = "VerifiedCardCode";
			if (dst.Tables[sTableName] != null)
				dst.Tables[sTableName].Clear();
			int nRows = 0;
			string sc = "";
			sc += " SELECT * ";
			sc += " FROM latipay_gift_card ";
			sc += " WHERE 1=1 ";
			sc += " AND till_number ='" + m_sTillNumber + "'";
			sc += " AND cart_number ='" + m_sCartNumber + "'";
			sc += " AND invoice_number IS NULL ";
			sc += " ORDER BY id ";

			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, sTableName);
			}
			catch (Exception e)
			{
				MessageBox.Show("GetVerifiedCardCode Error: " + e.ToString() + "\r\nsc=" + sc);
				myConnection.Close();
				return false;
			}
			//if (nRows <= 0)
			//	return false;

			List<string> vcc = new List<string>();
			List<string> vcfv = new List<string>();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables[sTableName].Rows[i];
				string gift_card_code = dr["gift_card_code"].ToString();
				string merchant_reference = dr["merchant_reference"].ToString();
				string face_value = dr["face_value"].ToString();

				vcc.Add(gift_card_code);
				vcfv.Add(face_value);
			}
			m_sVerifiedCardCode = vcc.ToArray();
			m_sVerifiedCardFaceValue = vcfv.ToArray();
			return true;
		}
		private void ShowVerifiedCardList()
		{
			dgvVerifiedCardList.Rows.Clear();
			string sTableName = "dgvVerifiedCardList";
			int nRows = 0;
			if (dst.Tables[sTableName] != null)
				dst.Tables[sTableName].Clear();
			string sc = " SELECT * FROM latipay_gift_card WHERE invoice_number IS NULL";
			sc += " AND till_number = " + m_sTillNumber;
			sc += " AND cart_number = " + m_sCartNumber;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, sTableName);
				if (nRows <= 0)
					return;
			}
			catch (Exception e)
			{
				MessageBox.Show("ShowVerifiedCardList Error: " + e.ToString() + "\r\nsc=" + sc);
				myConnection.Close();
				return;
			}
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables[sTableName].Rows[i];
				string id = dr["id"].ToString();
				string user_id = dr["user_id"].ToString();
				string wallet_id = dr["wallet_id"].ToString();
				string gift_card_code = dr["gift_card_code"].ToString();
				string merchant_reference = dr["merchant_reference"].ToString();
				string face_value = dr["face_value"].ToString();
				string till_number = dr["till_number"].ToString();
				string cart_number = dr["cart_number"].ToString();
				dgvVerifiedCardList.Rows.Add(gift_card_code, merchant_reference, face_value, "Cancel", id);
			}
		}
		private void dgvVerifiedCardList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 3)
			{
				if (MessageBox.Show("Are you sure you want to cancel this gift card?", "Cancel Gift Card", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;

				string gift_card_code = dgvVerifiedCardList.Rows[e.RowIndex].Cells["dgvGiftCardCode"].Value.ToString();
				string merchant_reference = dgvVerifiedCardList.Rows[e.RowIndex].Cells["dgvReference"].Value.ToString();
				if(CancelGiftCard(gift_card_code, merchant_reference))
					ShowVerifiedCardList();
			}
		}
		private void btnUse_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Do you want to use this gift card.", "Confirm to use gift card", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				string host = Program.m_sLatiPayUrlGiftCardRedeem;
				string user_id = m_sUserId;
				string wallet_id = m_sWalletId;
				string merchant_reference = m_sMerchantReference;
				string confirm_codes = m_sGiftCardCode;
				string apiKey = m_sApiKey;

				string message = "confirm_codes=" + confirm_codes;
				message += "&merchant_reference=" + merchant_reference;
				message += "&user_id=" + user_id;
				message += "&wallet_id=" + wallet_id;

				byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
				byte[] secret = Encoding.UTF8.GetBytes(apiKey);
				byte[] SHA256HMACSignature = HashHMAC(secret, msg);
				string signature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower();

				string json = "{\"user_id\": \"" + user_id + "\",";
				json += "\"wallet_id\": \"" + wallet_id + "\",";
				json += "\"confirm_codes\": \"" + confirm_codes + "\",";
				json += "\"merchant_reference\": \"" + merchant_reference + "\",";
				json += "\"signature\": \"" + signature + "\"}";

				string sRet = Http.post(host, json);
				if (sRet == null)
				{
					btnUse.Visible = true;
					MessageBox.Show("network anomaly, try again please.", "prompt");
					return;
				}
				int p = 0;
				string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
				string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();

				if (rMessage == "success")
				{
					if (RecordLatipayGiftCard())
					{
						MessageBox.Show("This gift card ( " + m_sGiftCardCode + " ) successfully was used.", "prompt");
						ShowVerifiedCardList();
					}
					else
					{
						MessageBox.Show("Record gift card ,try again please.", "prompt");
						btnTryAgain.Visible = true;
						return;
					}
					btnUse.Visible = false;
					lblVerifiedCardCode.Text = "None";
					lblFaceValue.Text = "None";
					lblStatus.Text = "None";
				}
				else
				{
					btnUse.Visible = true;
					MessageBox.Show(rMessage + ",try again please.", "prompt");
				}
				return;
			}
		}
		private bool RecordLatipayGiftCard()
		{
			string sc = " INSERT INTO latipay_gift_card (user_id, wallet_id, merchant_reference, gift_card_code, face_value, till_number, cart_number)";
			sc += " VALUES('" + m_sUserId + "','" + m_sWalletId + "','" + m_sMerchantReference + "','" + m_sGiftCardCode + "', '" + m_sFaceValue + "','" + m_sTillNumber + "','" + m_sCartNumber +"')";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			return true;
		}
		private void btnTryAgain_Click(object sender, EventArgs e)
		{
			if (RecordLatipayGiftCard())
			{
				btnTryAgain.Visible = false;
				MessageBox.Show("This gift card (" + m_sGiftCardCode + ") successfully was used.", "prompt");
				ShowVerifiedCardList();
			}
			else
			{
				MessageBox.Show("Record gift card ,try again please.", "prompt");
				btnTryAgain.Visible = true;
				return;
			}
		}
		private bool CancelGiftCard(string giftCardCode, string merchantReference)
		{
			string host = Program.m_sLatiPayUrlGiftCardDrawback;
			string user_id = m_sUserId;
			string wallet_id = m_sWalletId;
			string merchant_reference = merchantReference;
			string apiKey = m_sApiKey;

			string message = "merchant_reference=" + merchant_reference;
			message += "&user_id=" + user_id;
			message += "&wallet_id=" + wallet_id;

			byte[] msg = Encoding.UTF8.GetBytes(message + apiKey);
			byte[] secret = Encoding.UTF8.GetBytes(apiKey);
			byte[] SHA256HMACSignature = HashHMAC(secret, msg);
			string signature = BitConverter.ToString(SHA256HMACSignature).Replace("-", "").ToLower();

			string json = "{\"user_id\": \"" + user_id + "\",";
			json += "\"wallet_id\": \"" + wallet_id + "\",";
			json += "\"merchant_reference\": \"" + merchant_reference + "\",";
			json += "\"signature\": \"" + signature + "\"}";

			string sRet = Http.post(host, json);
			if (sRet == null)
			{
				MessageBox.Show("network anomaly, try again please.", "prompt");
				return false;
			}
			int p = 0;
			string rCode = JsonGetSimpleValue(sRet, "\"code\":", ref p);
			string rMessage = JsonGetSimpleValue(sRet, "\"message\":", ref p).ToLower();

			btnUse.Visible = false;
			lblVerifiedCardCode.Text = "None";
			lblFaceValue.Text = "None";
			lblStatus.Text = "None";

			if (rMessage == "success")
			{
				string sc = " DELETE FROM latipay_gift_card WHERE gift_card_code = '" + giftCardCode + "' AND merchant_reference = '" + merchantReference + "'";
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					Program.ShowExp(sc, e);
					MessageBox.Show("network anomaly, try again please.", "prompt");
					return false;
				}
				return true;
			}
			else
			{
				MessageBox.Show(rMessage + ",try again please.", "prompt");
				return false;
			}
			
		}
		private byte[] HashHMAC(byte[] key, byte[] message)
		{
			var hash = new HMACSHA256(key);
			return hash.ComputeHash(message);
		}
		public string JsonGetNodeValue(string sj, string tag, char spLeft, char spRight, ref int q)
		{
			int p = sj.IndexOf("\"" + tag + "\"");
			if (p < 0)
				return "";
			p = sj.IndexOf(spLeft, p);
			if (p < 0)
				return "";
			q = JsonFindEnd(sj, spLeft, spRight, p);
			//DEBUG("q=", q);	
			if (q < p)
				return "";
			p++;
			string so = sj.Substring(p, q - p).Trim();
			return so;
		}
		public int JsonFindEnd(string sj, char tLeft, char tRight, int nBegin)
		{
			//DEBUG("FindEnd, sj=", sj);
			int nLeft = 0;
			for (int i = nBegin + 1; i < sj.Length; i++)
			{
				//DEBUG("sji=", sj[i].ToString());		
				if (sj[i] == tLeft)
				{
					nLeft++;
					//DEBUG("nLeft=", nLeft);
				}
				else
				{
					if (sj[i] == tRight)
					{
						//DEBUG("nLeft=", nLeft);
						//DEBUG("sj=", sj.Substring(nBegin + 1, i - nBegin - 1));
						if (nLeft == 0)
							return i; //found
						else
							nLeft--;
					}
				}
			}
			return -1;
		}
		public string JsonGetSimpleValue(string s, string tag, ref int p)
		{
			int q = s.IndexOf(tag, p);
			if (q >= p)
			{
				p = q + tag.Length;
				int h = 0;
				if (tag == "\"qrcode_pic\":")
					h = 23;
				q = s.IndexOf(',', p + h);
				if (q == -1) // not found
				{
					q = s.Length;
				}
				if (q > p)
				{
					string r = s.Substring(p, q - p);
					r = r.Trim();
					r = r.Replace("\"", "");
					r = r.Replace("}", "");
					return r;
				}
			}
			return "";
		}

		private void txtCardNumber_Click(object sender, EventArgs e)
		{
			txtCardNumber.Text = "";
			txtCardNumber.Focus();
		}

		private void btnHistoryRecord_Click(object sender, EventArgs e)
		{
			FormLatipayGiftCardHistory flgch = new FormLatipayGiftCardHistory();
			flgch.ShowDialog();
		}
		//private string GetNextGiftCardId()
		//{
		//    string sTableName = "GetNextGiftCardId";
		//    if (dst.Tables[sTableName] != null)
		//        dst.Tables[sTableName].Clear();
		//    int nRows = 0;
		//    string sc = " SELECT TOP 1 id FROM latipay_gift_card ORDER BY id DESC ";
		//    try
		//    {
		//        SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
		//        nRows = myCommand.Fill(dst, sTableName);
		//    }
		//    catch (Exception e)
		//    {
		//        MessageBox.Show("GetNextGiftCardId Error: " + e.ToString() + "\r\nsc=" + sc);
		//        myConnection.Close();
		//    }
		//    string s = "";
		//    if (nRows > 0)
		//    {
		//        string id = dst.Tables[sTableName].Rows[0]["id"].ToString();
		//        int nId = Program.MyIntParse(id) + 1;
		//        s = nId.ToString();
		//    }
		//    return s;
		//}
	}
}
