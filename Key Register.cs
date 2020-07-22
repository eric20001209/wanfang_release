using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;
using System.Net;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class Key_Register : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		private string m_sSoftWareSSID = "";
		private string m_sProductId = "";
		private string m_sKeyTypeMode = Program.m_sKeyTypeMode;
		private string m_sKeyType = Program.m_sKeyType;
		private int m_iKeyLive = 1;
		private bool m_bKeyExpired = true;
		public string m_sKeyInvalided = "0";
		private string m_sTradingName = "";
		private string m_sAddress = "";
		private string m_sAddress1 = "";
		private string m_sAddress2 = "";
		private string m_sCity = "";
		private string m_sPhone = "";
		private string m_sAuth = "";

		public Key_Register()
		{
			InitializeComponent();
		}
		private void Key_Register_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			try
			{
				int keyTypeMode = int.Parse(m_sKeyTypeMode);
			}
			catch
			{
				m_sKeyTypeMode = "1";
			}
			switch (m_sKeyType)
			{
				case "Free Key Mode":
					m_sSoftWareSSID = System.DateTime.Now.ToOADate().ToString();
					m_sProductId = m_sSoftWareSSID.Substring(m_sSoftWareSSID.IndexOf(".") + 1, 5);
					m_sSoftWareSSID = m_sProductId + GetMotherboardSerialNumber();
					txtvalidatekey.Text = Program.md5(m_sSoftWareSSID).Substring(8, 20);
					txtkey1.Focus();
					break;
				case "Encript Key Mode":
					m_sSoftWareSSID = GetMotherboardSerialNumber();
					if (m_sKeyTypeMode == "2")
						m_sSoftWareSSID = GetCpuId();
					txtvalidatekey.Text = m_sSoftWareSSID;
					txtkey1.Focus();
					DoEncriptKeyMode();
					break;
				case "Auto Encript Key Mode":
					m_sSoftWareSSID = GetMotherboardSerialNumber();
					if (m_sKeyTypeMode == "2")
						m_sSoftWareSSID = GetCpuId();
					txtvalidatekey.Text = m_sSoftWareSSID;
					txtkey1.Focus();
					DoEncriptKeyMode();
					break;
				case "":
					m_sSoftWareSSID = System.DateTime.Now.ToOADate().ToString();
					m_sProductId = m_sSoftWareSSID.Substring(m_sSoftWareSSID.IndexOf(".") + 1, 5);
					m_sSoftWareSSID = m_sProductId + GetMotherboardSerialNumber();
					txtvalidatekey.Text = Program.md5(m_sSoftWareSSID).Substring(8, 20);
					txtkey1.Focus();
					break;
				default:
					break;
			}
			tbtradingname.Focus();
		}
		private string GetCpuId()
		{
			string sCPU = "";
			string sQuery = "SELECT ProcessorId FROM Win32_Processor";
			ManagementObjectSearcher oManagementObjectSearcher = new ManagementObjectSearcher(sQuery);
			ManagementObjectCollection oCollection = oManagementObjectSearcher.Get();
			foreach (ManagementObject oManagementObject in oCollection)
			{
				sCPU = (string)oManagementObject["ProcessorId"];
			}
			return sCPU;
		}
		private string GetMotherboardSerialNumber()
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher
			("SELECT SerialNumber, Product FROM Win32_BaseBoard");

			ManagementObjectCollection information = searcher.Get();
			string serialNumber = string.Empty;

			foreach (ManagementObject obj in information)
			{
				if (obj.Properties["SerialNumber"].Value.ToString().Trim() != string.Empty)
					serialNumber = obj.Properties["SerialNumber"].Value.ToString().Trim();
				else
					serialNumber = obj.Properties["Product"].Value.ToString().Trim();
				break;
			}
			searcher.Dispose();
			return serialNumber;
		}
		private void DoSaveKeyValue(string name, string value)
		{
			//Do Regist Key
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegLicenseKey + "\\", name, value);

			if (dst.Tables["keyvalue"] != null)
				dst.Tables["keyvalue"].Clear();
			string sc = " INSERT INTO settings (cat, name, value, description, hidden, bool_value)";
			sc += " VALUES('Product Info','" + name + "', '" + value + "', '', 1, 0)";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "keyvalue");
			}
			catch (Exception ex)
			{
				string s = ex.ToString();
				return;
			}
		}
		private bool DoFreeKeyModeValidate()
		{
			string keyTime = txtvkey.Text;
			string keySet = txtvalidatekey.Text + keyTime;
			string keySetValue = Program.md5(keySet).Substring(0, 25);
			string keySetKeyIn = txtkey1.Text + txtkey2.Text + txtkey3.Text + txtkey4.Text + txtkey5.Text;
			if (keySetKeyIn == "")
				return true;
			try
			{
				m_iKeyLive = int.Parse(txtvkey.Text);
			}
			catch
			{
				MessageBox.Show("Invalided Key");
				return false;
			}
			if (keyTime == "000")
				m_bKeyExpired = false;
			if (keySetValue == keySetKeyIn)
			{
				DoSaveKeyValue("productid", m_sProductId);
				Regkey(Program.Encrypt(keySetValue));
				MessageBox.Show("Regist Key Complete, Please Restart Program");
				m_sKeyInvalided = "0";
				return true;
			}
			else
			{
				MessageBox.Show("Invalided Key");
				return false;
			}
		}
		private bool DoEncriptKeyMode()
		{
			m_sSoftWareSSID = GetMotherboardSerialNumber();
			if (m_sKeyTypeMode == "2")
				m_sSoftWareSSID = GetCpuId();
			string keyTime = txtvkey.Text;
			string keySet = txtvalidatekey.Text + keyTime;
			string keySetValue = Program.md5(keySet).Substring(0, 25);
			string keySetKeyIn = txtkey1.Text + txtkey2.Text + txtkey3.Text + txtkey4.Text + txtkey5.Text;
			if (keySetKeyIn == "")
				return true;
			try
			{
				m_iKeyLive = int.Parse(txtvkey.Text);
			}
			catch
			{
				MessageBox.Show("Invalided Key");
				return false;
			}
			if (keyTime == "000")
				m_bKeyExpired = false;
			if (keySetValue == keySetKeyIn)
			{
				DoSaveKeyValue("productid", m_sSoftWareSSID.Substring(0, 5));
				Regkey(Program.Encrypt(keySetValue));
				MessageBox.Show("Regist Key Complete, Please Restart Program");
				m_sKeyInvalided = "0";
				return true;
			}
			else
			{
				MessageBox.Show("Invalided Key");
				return false;
			}
		}
		private void Regkey(string key)
		{
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegLicenseKey + "\\", "key_type", m_sKeyType);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegLicenseKey + "\\", "duration", m_iKeyLive.ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegLicenseKey + "\\", "key", key);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegLicenseKey + "\\", "key_type_mode", m_sKeyTypeMode);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegLicenseKey + "\\", "has_expired_date", m_bKeyExpired.ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", (m_iKeyLive * 30).ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "has_expired_date", m_bKeyExpired.ToString());
			DeleteSubKey();
		}
		private void DeleteSubKey()
		{
			string keyName = Program.m_sRegLicenseKey;
			string keyExpireDate = Program.m_sSecrectValue;
			using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName, true))
			{
				if (key != null)
				{
					try
					{
						key.DeleteValue("trial_day");
					}
					catch
					{
					}
				}
			}
		}
		private void txtkey1_KeyUp(object sender, KeyEventArgs e)
		{
			if (txtkey1.Text.Length == 5)
				txtkey2.Focus();
		}
		private void txtkey2_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.ToString() == "Back" && txtkey2.Text.Length == 0)
				txtkey1.Focus();
			if (txtkey2.Text.Length == 5)
				txtkey3.Focus();
		}
		private void txtkey3_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.ToString() == "Back" && txtkey3.Text.Length == 0)
				txtkey2.Focus();
			if (txtkey3.Text.Length == 5)
				txtkey4.Focus();
		}
		private void txtkey4_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.ToString() == "Back" && txtkey4.Text.Length == 0)
				txtkey3.Focus();
			if (txtkey4.Text.Length == 5)
				txtkey5.Focus();
		}
		private void txtkey5_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.ToString() == "Back" && txtkey5.Text.Length == 0)
				txtkey4.Focus();
			if (txtkey5.Text.Length == 5)
				txtvkey.Focus();
		}
		private void txtvkey_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.ToString() == "Back" && txtvkey.Text.Length == 0)
				txtkey5.Focus();
		}
		private void btnregistkey_Click(object sender, EventArgs e)
		{
			string sm = "";
			m_sAuth = txtauth.Text;
			if (txtkey1.Text == "" && m_sAuth.Trim() == "")
			{
				sm = "Please enter Register Code";
//				txtauth.Focus();
			}
			if(tbtradingname.Text.Trim() == "")
			{
				sm = "Please enter Company Name";
				tbtradingname.Focus();
			}
			else if(tbaddr1.Text.Trim() == "")
			{
				sm = "Please enter Address";
				tbaddr1.Focus();
			}
			else if(tbphone.Text.Trim() == "")
			{
				sm = "Please enter contact phone number";
				tbphone.Focus();
			}
			else if(tbcity.Text.Trim() == "")
			{
				sm = "Please enter City";
				tbcity.Focus();
			}
			if(sm != "")
			{
				Program.MsgBox(sm);
				return;
			}
			
			if(!DoSaveCompanyDetails())
				return;

			if (txtkey1.Text == "")
			{
				if (!GetRespKey())
					return;
			}

			if (m_sKeyInvalided == "1" && txtkey1.Text == "")
			{
				this.Close();
				return;

			}
			string keyType = Program.m_sKeyType;
			switch (keyType)
			{
				case "Free Key Mode":
					if (DoFreeKeyModeValidate())
						this.Close();
					break;
				case "Encript Key Mode":
					if (DoEncriptKeyMode())
						this.Close();
					break;
				case "Auto Encript Key Mode":
					if (DoEncriptKeyMode())
						this.Close();
					break;
				case "":
					if (DoFreeKeyModeValidate())
						this.Close();
					break;
				default:
					break;
			}
		}
		private void btnOk_Click(object sender, EventArgs e)
		{
			m_sAuth = txtauth.Text;
			if (m_sAuth.Trim() == "")
			{
//				penAuth.Visible = false;
				return;
			}
//			penAuth.Visible = false;
			if (GetRespKey())
				btnregistkey_Click(sender, e);
		}
		private bool DoSaveCompanyDetails()
		{
			m_sTradingName = tbtradingname.Text.Trim();
			m_sAddress = tbaddr1.Text.Trim();
			m_sAddress1 = tbaddr2.Text.Trim();
			m_sAddress2 = tbaddr3.Text.Trim();
			m_sCity = tbcity.Text.Trim();
			m_sPhone = tbphone.Text.Trim();
			string sc = " UPDATE card SET trading_name = N'" + Program.EncodeQuote(m_sTradingName) + "' ";
			sc += ", address1 = N'" + Program.EncodeQuote(m_sAddress) + "' ";
			sc += ", address2 = N'" + Program.EncodeQuote(m_sAddress1) + "' ";
			sc += ", address3 = N'" + Program.EncodeQuote(m_sAddress2) + "' ";
			sc += ", city = N'" + Program.EncodeQuote(m_sCity) + "' ";
			sc += ", phone = N'" + Program.EncodeQuote(m_sPhone) + "' ";
			sc += " WHERE id = 1 ";
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
				myConnection.Close();
				return false;
			}
			return true;
		}
		private void GetCustDetail()
		{
			if (dst.Tables["customer"] != null)
				dst.Tables["customer"].Clear();
			string sc = " SELECT * FROM card WHERE id = 1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "customer") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (dst.Tables["customer"].Rows.Count == 1)
			{
				m_sTradingName = dst.Tables["customer"].Rows[0]["trading_name"].ToString();
				m_sAddress = dst.Tables["customer"].Rows[0]["address1"].ToString();
				m_sAddress1 = dst.Tables["customer"].Rows[0]["address2"].ToString();
				m_sAddress2 = dst.Tables["customer"].Rows[0]["address3"].ToString();
				m_sCity = dst.Tables["customer"].Rows[0]["city"].ToString();
				m_sPhone = dst.Tables["customer"].Rows[0]["phone"].ToString();
			}
		}
		private bool DetectInternetConnection()
		{
			HttpWebRequest req;
			HttpWebResponse resp;
			try
			{
				req = (HttpWebRequest)WebRequest.Create("http://reg.gposnz.com");
				resp = (HttpWebResponse)req.GetResponse();

				if (resp.StatusCode.ToString().Equals("OK"))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				string s = ex.ToString();
				// Program.ShowExp("Internet Connection Error", ex);
				return false;
			}
		}
		private bool GetRespKey()
		{
			GetCustDetail();

			HttpWebRequest req;
			HttpWebResponse resp;

			req = (HttpWebRequest)WebRequest.Create("http://reg.gposnz.com/reg.aspx?k=" + txtvalidatekey.Text + "&authID=" + m_sAuth + "&t=" + m_sTradingName + "&add=" + m_sAddress + "&add1" + m_sAddress1 + "&add2=" + m_sAddress2 + "&ct=" + m_sCity + "&p=" + m_sPhone + "&m=" + GetMotherboardSerialNumber());
			resp = (HttpWebResponse)req.GetResponse();
			resp.ToString();
			string responseKey = resp.ResponseUri.Query.ToString();
			int keyStart = responseKey.IndexOf("=");
			int idStart = responseKey.IndexOf("&");
			if (responseKey.IndexOf("*f") > 0)
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = "Register Error, Please contact GPOS Ltd ";
				fm.ShowDialog();
				return false;
			}
			string returnKey = responseKey.Substring(keyStart + 1, 25);
			string cid = responseKey.Substring(idStart + 4, (responseKey.Length - (idStart + 4)));
			string term = responseKey.Substring(responseKey.Length - 3, 3);
			doSaveCustomerID(cid);
			txtkey1.Text = returnKey.Substring(0, 5);
			txtkey2.Text = returnKey.Substring(5, 5);
			txtkey3.Text = returnKey.Substring(10, 5);
			txtkey4.Text = returnKey.Substring(15, 5);
			txtkey5.Text = returnKey.Substring(20, 5);
			txtvkey.Text = term;
			return true;
		}
		private void doSaveCustomerID(string id)
		{
			string sc = "IF NOT EXISTS(SELECT * FROM settings WHERE name ='customer_id')";
			sc += " INSERT INTO settings (name, value, hidden) VALUES ( 'customer_id', '" + id + "', '1')";
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
				return;
			}
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
