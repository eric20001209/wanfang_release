using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Data;
using System.Management;
using System.Text;
using System.IO;
using System.Security.Cryptography;
//using System.Data.SqlClient;
using System.Reflection;                //Assembly
using System.Threading;                 //Mutex
using System.Security.AccessControl;    //MutexAccessRule
using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;
using SplitAndInput;

namespace QPOS2008
{
	public static class SingleApplicationDetector
	{
		public static bool IsRunning()
		{
			string guid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
			var semaphoreName = @"Global\" + guid;
			try
			{
				__semaphore = Semaphore.OpenExisting(semaphoreName, SemaphoreRights.Synchronize);

				Close();
				return true;
			}
			catch (Exception ex)
			{
				__semaphore = new Semaphore(0, 1, semaphoreName);
				return false;
			}
		}
		public static void Close()
		{
			if (__semaphore != null)
			{
				__semaphore.Close();
				__semaphore = null;
			}
		}
		private static Semaphore __semaphore;
	}
	public class MyDataGridView : DataGridView
	{
	}
	static class Program
	{
		[DllImport("User32.dll", EntryPoint = "FindWindow")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
		
		public static string m_sServer1 = "";
		public static string m_sServer2 = "";
		public static string m_sDBName1 = "";
		public static string m_sDBName2 = "";
		public static string m_sUser1 = "";
		public static string m_sUser2 = "";
		public static string m_sPass1 = "";
		public static string m_sPass2 = "";
		public static bool m_bSql1Good = false;
		public static bool m_bSql2Good = false;
		public static bool m_bServer2AsBackup = true; //only active if server 1 down
		public static System.Data.SqlClient.SqlConnection myConnection1;
		public static System.Data.SqlClient.SqlConnection myConnection2;

		public static string m_sServer = "localhost";
//		public static string m_sServer2 = "";
		public static string m_sUser = "";
		public static string m_sPass = "";
		public static string m_sDataSource = ";data source=192.168.1.1;";
		public static string m_sDataSource2 = ";data source=192.168.1.1;";
		//		public static string m_sDataSource = ";data source=localhost;";
		public static string m_sSecurityString = "User id=eznz;Password=9seqxtf7;Integrated Security=false;Connect Timeout=30";
		//		public static string m_sSecurityString = "Integrated Security=SSPI;";
		public static string m_sCompanyName = "taiping";	//site identifer, used for cache, sql db name etc, highest priority

		public static string m_sDataSourceLoyalty = ";data source=192.168.1.119;";
		public static string m_sSecurityStringLoyalty = "User id=eznz;Password=9seqxtf7;Integrated Security=false;Connect Timeout=30";
		public static string m_sCompanyNameLoyalty = "loyalty";	//site identifer, used for cache, sql db name etc, highest priority

		private static SqlConnection myConnection;
//		private static SqlConnection myConnection2;
		private static SqlDataAdapter myAdapter;
		private static SqlCommand myCommand;
		private static DataSet ds = new DataSet();
		private static DataSet dst = new DataSet();

		public static string m_Barcode = "";
		public static string m_sBranchID = "1";
		public static string m_sBranchID_sync = "1";
		public static string m_sBranchName = "1";
		public static string m_sInv = "20000";
		public static string m_sFtpURL = "";
		public static string m_sFtpUser = "";
		public static string m_sFtpPassword = "";
		public static string m_sFtpRemotePath = "";
		public static string m_sMainDBName = "franchise";
		public static bool m_bSku = false;
		public static bool m_bItem = false;
		public static bool m_bInvoice = true;
		public static bool m_bButton = false;

		//		private static SqlConnection myConnectionLoyalty;
		//		private static SqlDataAdapter myAdapterLoyalty;
		//		private static SqlCommand myCommandLoyalty;

		public static bool m_bDebug = false;
		public static string m_sPrinterName = "Generic / Text Only";
		public static int m_nPaperWidth = 30;
		public static string m_sFontName = "Arial";
		public static string m_sFontSize = "8";
		public static string m_sFontStyle = "Regular";
		public static FontStyle m_tFontStyle = FontStyle.Regular;
		public static string m_sScaleName = "DL_RS232Scale";
		public static string m_sScannerName = "DL_RS232Scanner";
		public static string m_sStationID = "1";
		public static int m_nTimeout = 500;
		public static string m_sAdUrl = "www.eznz.com";
		public static string m_sSerialDevicePort = "";

		// public static string m_sDMoniter = "1";
		public static bool m_sDMoniter = false;
		public static bool m_sEpson = false;

		public static string m_sgroceryitem = "1001";
		public static string m_sgroceryweight = "1001";
		public static string m_sgroceryitemaddup = "1";
		public static string m_sgroceryweightitemaddup = "0";
		public static string m_svoidscale = "1";
		public static string m_sbarcodewithprice1 = "1";
		public static string m_sbarcodewithprice2 = "1";
		public static string m_sbarcodewithprice3 = "1";
		public static string m_sbarcodelength1 = "1";
		public static string m_sbarcodelength2 = "1";
		public static string m_sbarcodelength3 = "1";
		public static string m_spricelength1 = "1";
		public static string m_spricelength2 = "1";
		public static string m_spricelength3 = "1";
		public static string m_spoint1 = "1";
		public static string m_spoint2 = "1";
		public static string m_spoint3 = "1";
		public static string m_svoiddis = "";

		public static bool m_bEnableEftpos = false;
		public static bool m_senablescale = false;

		public static string m_sEftposCom = "";
		public static string m_sSalesId = "";
		public static DataTable m_dtHotkey = null;
		public static string m_sStoreDetail = "";
		public static string m_sVoucherServiceURL = "";
		public static bool m_bVoucherUseWebService = false;
		public static string m_sServerBkIp = "";
		public static bool m_bServer2 = false;
		public static string m_bReprintTotal = "";
		public static bool m_bEnableEftposLog = false;
		public static string m_sEftposLogDay = "3";
		public static bool m_bRegularMonitor = false;
		public static bool m_bSmallMonitor = false;
		public static bool m_bEnableVipDis = false;
		public static bool m_bEnableAutoTare = false;
		public static bool m_bEnableFloating = false;
		public static bool m_bScanInCode = false;
		public static bool m_bScanInSupplierCode = false;
		public static bool m_bEnableAutoSync = false;
		public static bool m_bEnableSalesLogin = false;
		public static bool m_bEnableUpdatePrice = false;
		public static bool m_bEnableCalQty = false;
		public static bool m_bEnableMargin = false;
		public static bool m_bEnableLevelPrice = false;
		public static bool m_bEnableSimpleInvoice = false;
		public static bool m_bEnableAA = false;
		public static bool m_bEnableDisControl = false;
		public static bool m_bEnableSpecialDis = false;
		public static string m_sRegKey = "Software\\EZNZ\\RSTTH";
		public static string m_sRegKeyS = "Software\\EZNZ\\ezFsyncC";
		public static string m_sIISServer = "localhost";
		public static bool m_bEnablePic = false;
		public static string m_sPicroot = "";
		public static string m_sTare = "";
		public static string m_sAA = "";
		public static bool m_backupdata = false;
		public static string m_backuproot = "";
		public static bool m_sync = false;
		public static bool m_bEnableWebService = false;
        public static bool m_bSyncWithCloud = false;
		public static string m_sWebServiceUrl = "";
		public static string m_syncroot = "";
		public static bool m_formconfigshow = false;
		public static string m_eftposType = "";

		public static string m_sRegLicenseKey = "Software\\Microsoft\\DARCY";
		public static string m_sSecrectValue = "Software\\Microsoft\\Windows NT\\CurrentVersion\\Accessibility\\Utility Manager\\Magnifier";
		public static bool m_bEnableVipDiscount = false;
		public static bool m_bLaybyVoucher = false;
		public static string m_sLicenseKey = "";
		public static string m_sDuration = "";
		public static string m_sKeyType = "";
		public static string m_sKeyTypeMode = "1";
		public static string m_sTrial = "1";
		public static string m_sTrialDay = "30";
		private static string keyword = "";
		public static string m_sTrialDayLife = "";
		private static string m_sProductId = "";
		private static string m_sKayHasExpireDate = "1";
		private static string m_sExpireDate = "";
		public static bool m_bLanguage_en = true;

		public static string m_sCode = "";
		public static CEzLog g_log;
		public static string m_debugLogType = "None";
		public static string g_sActivataStoreCode = "990001";

		public static bool m_bCallOfOrderNumber = false;
		public static bool m_bAutoClosePopupMenu = true;
		public static int m_nCallOfOrderNumber = 1;
		public static string m_sOrderIdCurrent = "";
		public static string m_sTradingName = "";

		//access, default all true, assign access if not admin
		public static bool m_bAccessAdmin = true;
		public static bool m_bAccessDeleteItem = true;
		public static bool m_bAccessRefund = true;
		public static bool m_bAccessDiscount = true;
		public static bool m_bAccessCashdraw = true;
		public static bool m_bAccessXTotal = true;
		public static bool m_bAccessReport = true;
		public static bool m_bAccessVipPayment = true;
		public static bool m_bAccessProduct = true;
		public static bool m_bAccessStock = true;
		public static bool m_bAccessSetting = true;
		public static bool m_bAccessDatabase = true;
		public static bool m_bAccessAdminZone = true;

		public static string m_sSmartpayIP = "";
		public static string m_sSmartpayPort = "";

		public static string m_sNoVipDiscountCatalog = "";

		public static bool m_bEnableWechatPayment = false;
		public static string m_sWeChatUri = "http://api.payplus.co.nz/getwxqr/gposqr";
//		public static string m_sWeChatStatusUri = "http://api.payplus.co.nz/getwxqr/gposcheck";
		public static string m_sWeChatMerchantID = "29";
		public static string m_sWeChatSignature = "gpI3wXspE49xfYXUmmIoz9xRjEOxXzzVwPGDOsxDzuGFAa4xnM9X8dUceWCVQPTc";

		public static bool m_bEnableAliPay = false;
		public static string m_sAliPayUri = "http://online.attractpay.co.nz/alipay";
		public static string m_sAliPayMerchantID = "420152";
		public static string m_sAliPaySignature = "GPOS";
		
		public static bool m_bEnableNVR = false;
		public static string m_sNVRIP = "";
		public static string m_sNVRPort = "";
		public static bool m_bIdCheckPasswordControl = true;

		//AttractPay
		public static bool m_bEnableAttractPay = false;
		public static string m_sAttractPayUriRequest = "http://pay.attractpay.co.nz/offline/qrcode/app";
		public static string m_sAttractPayUriRequestBarcode = "http://pay.attractpay.co.nz/offline/barcode/app";
		public static string m_sAttractPayUriCheck = "http://api.attractpay.co.nz/api/store/query_trans_by_store";
		public static string m_sAttractPayStoreID = "941";
		public static string m_sAttractPayAuthCode = "5ee39cc44f1e2000c049b7ea95ef7123";

		//Alipay Direct
		public static bool m_bEnableAlipayDirect = false;
		//public static string m_sAlipayDirectUriRequest = "https://globalmapi.alipay.com/gateway.do"; //Production environment
		public static string m_sAlipayDirectUriRequest = "https://mapi.alipaydev.com/gateway.do"; //test environment
		public static string m_sAlipayDirectPartnerID = "";
		public static string m_sAlipayDirectKey = "";

		//LatiPay
		public static bool m_bEnableLatiPay = false;
		public static bool m_bEnableLatiPayGiftCard = false;
		public static string m_sLatiPayApiKey = "";
		public static string m_sLatiPayUserID = "";
		public static string m_sLatiPayWalletID = "";
		public static string m_sLatiPayQRCodeBackground = "";
		public static string m_sLatiPayUrlInvoice = "https://api.latipay.net/v2/invoice";
		public static string m_sLatiPayUrlGiftCardFreeze = "https://api-staging.latipay.net/v2/gift-card/freeze";
		public static string m_sLatiPayUrlGiftCardRedeem = "https://api-staging.latipay.net/v2/gift-card/redeem";
		public static string m_sLatiPayUrlGiftCardDrawback = "https://api-staging.latipay.net/v2/gift-card/drawback";

		//MyPosMate
		public static bool m_bEnableMyPosMate = false;
		public static string m_sMyPosMateMerchantId = "";
		public static string m_sMyPosMateMerchantAccountId = "";
		public static string m_sMyPosMateConfigId = "";
		public static string m_sMyPosMateUrlPosPay = "https://myposmate.com/api/v2/pos/posPay";
		public static string m_sMyPosMateUrlGetTransactionDetails = "https://myposmate.com/api/v2/pos/getTransactionDetails";
		public static string m_sMyPosMateUrlRefund = "https://myposmate.com/api/v2/pos/refund";

		//Second Printer Setting
		public static bool m_bEnableSecondPrinter = false;
		public static string m_sSecondPrinterCatalog = "";
		public static string m_sSecondPrinterName = "second_receipt";
		public static int m_sSecondPrinterReceiptQTY = 1;
		public static int m_nSecondPrinterPaperWidth = 30;
		public static bool m_sSecondPrinterEpson = false;
		public static int m_sSecondPrinterFontSize = 8;
		public static string m_sSecondPrinterFontName = "Arial";
		public static string m_sSecondPrinterFontStyle = "Regular";
		public static int m_nSecondPrinterTimeout = 500;

		public static bool m_bEnableSelfService = false;
		public static bool m_bEnableQtyPassword = false;
		public static bool m_bCasScale = false;
		public static string m_sCasScalePort = "COM1";
		
		/// <summary>
		/// 应用程序的主入口点。
		[STAThread]
		static void Main()
		{
			if (SingleApplicationDetector.IsRunning())
			{
				MessageBox.Show("Error, GPOS is already running.");
				return;
			}

			try
			{
				GetSettings();
				GetSqlConfig();
				GetActivataStroeCode();
				g_log = new CEzLog();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			foreach (string arg in System.Environment.GetCommandLineArgs())
			{
				if (arg.ToLower().IndexOf("-debug") >= 0)
				{
					m_bDebug = true;
				}
			}

			if (m_bDebug)
			{
				try
				{
					Application.Run(new FormDebug());
				}
				catch (Exception e)
				{
					MessageBox.Show(e.ToString());
				}
				return;
			}

			if (m_sServer1 == "" && m_sServer2 == "")
			{
				Application.EnableVisualStyles();
				Application.Run(new FormSQL());
				SingleApplicationDetector.Close();
				return;
			}

			//dummy
			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);

			if (!CheckSqlServers())
			{
				MsgBox("All sql connections are bad, please reconfig");
				Application.EnableVisualStyles();
				Application.Run(new FormSQL());
				SingleApplicationDetector.Close();
				return;
			}
/*
			if (m_sServer == "")
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new FormConfig());
				SingleApplicationDetector.Close();
				return;
			}
			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			myConnection2 = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource2 + Program.m_sSecurityString);
			if (!DoTestDBConnection())
			{
				SingleApplicationDetector.Close();
				return;
			}
 */ 
			GetHotkey();
			m_sNoVipDiscountCatalog = GetSiteSettings("no_vip_discount_catalog").ToLower();
			m_sVoucherServiceURL = GetSiteSettings("voucher_service_url");
			m_bVoucherUseWebService = MyBooleanParse(GetSiteSettings("voucher_use_webservice"));
			Application.EnableVisualStyles();
			//            Application.SetCompatibleTextRenderingDefault(false);

			if (!m_bDebug)
				Application.Run(new form1());
			SingleApplicationDetector.Close();
		}
		public static bool DoTestDBConnection()
		{
			try
			{
				myConnection.Open();
			}
			catch
			{
				//				m_sServerBkIp = m_sServer;
				//				m_sServer = m_sServer2;
				//				m_sServer2 = m_sServerBkIp;
				//				m_bServer2 = false;
				MessageBox.Show("Error connectiong to SQL Server, please check your settings and try again 1.\r\n", "Database Error");
				FormConfig setup = new FormConfig();
				setup.ShowDialog();
				return false;
			}
			finally
			{
				myConnection.Close();

			}
			return true;
		}
		public static bool TestConnection(System.Data.SqlClient.SqlConnection myConnection)
		{
			try
			{
				myConnection.Open();
			}
			catch
			{
				return false;
			}
			finally
			{
				myConnection.Close();

			}
			return true;
		}
		public static string GetSqlInvNum(System.Data.SqlClient.SqlConnection myConnection)
		{
			System.Data.SqlClient.SqlDataAdapter adapter;
			if (ds.Tables["gt"] != null)
				ds.Tables["gt"].Clear();
			string sc = " SELECT MAX(id) AS id FROM invoice ";
			try
			{
				adapter = new System.Data.SqlClient.SqlDataAdapter(sc, myConnection);
				if (adapter.Fill(ds, "gt") <= 0)
					return "";
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return ""; ;
			}
			DataRow dr = ds.Tables["gt"].Rows[0];
			return dr["id"].ToString();
		}
		public static string GetSqlLastCode(System.Data.SqlClient.SqlConnection myConnection)
		{
			System.Data.SqlClient.SqlDataAdapter adapter;
			if (ds.Tables["gt"] != null)
				ds.Tables["gt"].Clear();
			string sc = " SELECT MAX(code) AS code FROM code_relations ";
			try
			{
				adapter = new System.Data.SqlClient.SqlDataAdapter(sc, myConnection);
				if (adapter.Fill(ds, "gt") <= 0)
					return "";
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return ""; ;
			}
			DataRow dr = ds.Tables["gt"].Rows[0];
			return dr["code"].ToString();
		}
		public static string GetSiteSettingWithConnection(System.Data.SqlClient.SqlConnection myConnection, string sName, bool bIntValue)
		{
			System.Data.SqlClient.SqlDataAdapter adapter;
			if (ds.Tables["gt"] != null)
				ds.Tables["gt"].Clear();
			string sc = " SELECT value AS v FROM settings WHERE name = '" + sName + "' ";
			if (bIntValue)
				sc = " SELECT access AS v FROM settings WHERE name = '" + sName + "' ";
			try
			{
				adapter = new System.Data.SqlClient.SqlDataAdapter(sc, myConnection);
				if (adapter.Fill(ds, "gt") <= 0)
					return "";
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return ""; ;
			}
			DataRow dr = ds.Tables["gt"].Rows[0];
			string sRet = dr["v"].ToString();
			return sRet;
		}
		public static bool IsDatabaseError(string se)
		{
			/*
			//network exception
			A network-related or instance-specific error occurred while establishing a connection to SQL Server. 
			The server was not found or was not accessible. 
			Verify that the instance name is correct and that SQL Server is configured to allow remote connections. 
			(provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)"}	
			System.Exception {System.Data.SqlClient.SqlException}

	 
			 //database exception
			Cannot open database \"rst372a\" requested by the login. The login failed.\r\nLogin failed for user 'eznz'."}	
			System.Exception {System.Data.SqlClient.SqlException}
			*/

			se = se.ToLower();
			if (se.IndexOf("server was not found") >= 0 || se.IndexOf("cannot open database") >= 0 || se.IndexOf("transport-level error") >= 0)
				return true;
			return false;
		}
		public static bool CheckSqlServers()
		{
			string sAccess1 = "";
			string sIndex1 = "";
			string sInv1 = "";
			string sCode1 = "";
			string sAccess2 = "";
			string sIndex2 = "";
			string sInv2 = "";
			string sCode2 = "";

			string sSecurityString = "User id=" + Program.m_sUser1 + ";Password=" + Program.m_sPass1 + ";Integrated Security=false;Connect Timeout=3";
			if (Program.m_sUser1 == "")
				sSecurityString = "Integrated Security=SSPI;";
			if (Program.m_sServer1 == "")
			{
				m_bSql1Good = false;
				Program.MsgBox("Warning, SQL Server 1 is not configured!");
			}
			else
			{
				myConnection1 = new System.Data.SqlClient.SqlConnection("Initial Catalog=" + Program.m_sDBName1 + ";data source=" + Program.m_sServer1 + ";" + sSecurityString);
				if (TestConnection(myConnection1))
				{
					m_bSql1Good = true;
					sAccess1 = GetSiteSettingWithConnection(myConnection1, "server_last_access_time", false);
//					sIndex1 = GetSiteSettingWithConnection(myConnection1, "server_last_update_index", true);
					sInv1 = GetSqlInvNum(myConnection1);
					sCode1 = GetSqlLastCode(myConnection1);
				}
				else
				{
					m_bSql1Good = false;
				}
			}

			sSecurityString = "User id=" + Program.m_sUser2 + ";Password=" + Program.m_sPass2 + ";Integrated Security=false;Connect Timeout=3";
			if (Program.m_sUser2 == "")
				sSecurityString = "Integrated Security=SSPI;";
			if (Program.m_sServer2 == "")
			{
				m_bSql2Good = false;
			}
			else
			{
				myConnection2 = new System.Data.SqlClient.SqlConnection("Initial Catalog=" + Program.m_sDBName2 + ";data source=" + Program.m_sServer2 + ";" + sSecurityString);
				if (TestConnection(myConnection2))
				{
					m_bSql2Good = true;
					sAccess2 = GetSiteSettingWithConnection(myConnection2, "server_last_access_time", false);
//					sIndex2 = GetSiteSettingWithConnection(myConnection2, "server_last_update_index", true);
					sInv2 = GetSqlInvNum(myConnection2);
					sCode2 = GetSqlLastCode(myConnection2);
				}
				else
				{
					m_bSql2Good = false;
				}
			}

			if (!m_bSql1Good && !m_bSql2Good)
			{
				return false;
			}
/*
			if (sIndex1 != sIndex2)
			{
				if (Program.MyIntParse(sIndex1) > Program.MyIntParse(sIndex2))
				{
					if (m_bSql1Good)
						m_bSql2Good = false;
				}
				else
				{
					if (m_bSql2Good)
						m_bSql1Good = false;
				}
			}
 */ 
			if (sInv1 != sInv2)
			{
				if (Program.MyIntParse(sInv1) > Program.MyIntParse(sInv2))
				{
					if (m_bSql1Good)
						m_bSql2Good = false;
				}
				else
				{
					if (m_bSql2Good)
						m_bSql1Good = false;
				}
			}
			else if (sCode1 != sCode2)
			{
				if (Program.MyIntParse(sCode1) > Program.MyIntParse(sCode2))
				{
					if (m_bSql1Good)
						m_bSql2Good = false;
				}
				else
				{
					if (m_bSql2Good)
						m_bSql1Good = false;
				}
			}
			if (!m_bSql1Good && m_sServer1 != "")
			{
				Program.MsgBox("Warning, Server 1 is down, please contact manager immediatly !!");
			}
			if (!m_bSql2Good && m_sServer2 != "")
			{
				Program.MsgBox("Warning, Server 2 is down, please contact manager immediatly !!");
			}
			return true;
		}
		public static void MsgBox(string msg)
		{
			MsgBox(msg, "");
		}
		public static void MsgBox(string msg, string title)
		{
			MyMessageBox mb = new MyMessageBox();
			mb.m_title = title;
			mb.m_msg = msg;
			mb.ShowDialog();
		}
		public static string GetSiteSettings(string sSettingName)
		{
			if (dst.Tables["rsp"] != null)
				dst.Tables["rsp"].Clear();
			if (sSettingName.Trim() == "")
				return "";
			if (Program.m_sCompanyName.Trim() == "")
				return "";
			string sc = " SELECT value FROM settings WHERE name = N'" + sSettingName + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "rsp");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return ""; ;
			}
			if (dst.Tables["rsp"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["rsp"].Rows[0];
				return dr["value"].ToString();
			}
			else
			{
				sc = " INSERT INTO settings (name, value) VALUES('" + sSettingName + "', '') ";
				try
				{
					myCommand = new SqlCommand(sc, myConnection);
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					myConnection.Close();
					Program.ShowExp(sc, e);
					return "";
				}
			}
			return "";
		}
		public static bool SaveSiteSettings(string name, string value)
		{
			string sc = " IF NOT EXISTS(SELECT value FROM settings WHERE name = '" + name + "') ";
			sc += " INSERT INTO settings (name, value) VALUES('" + name + "', '" + value + "') ";
			sc += " ELSE ";
			sc += " UPDATE settings SET value='" + value + "' WHERE name = '" + name + "' ";
			try
			{
				myCommand = new SqlCommand(sc, myConnection);
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			return true;
		}
		public static void GetSettings()
		{
			if (Registry.CurrentUser.OpenSubKey(m_sRegKey, false) != null)
			{
				m_bDebug = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "debug", "")));
				m_sServer = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "db_server", ""));
				m_sIISServer = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "iis_server", ""));
//				m_sServer2 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "db_server_2", ""));
                //if (m_sServer == "") //new installation? bring up setup form
                //    return;
//				m_sDataSource = ";data source=" + m_sServer + ";";
//				m_sDataSource2 = ";data source=" + m_sServer2 + ";";
				m_sUser = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "db_user", ""));
				m_sPass = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "db_pass", ""));
				m_sCompanyName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "db_catalog", ""));
//				m_sSecurityString = "User id=" + m_sUser + ";Password=" + m_sPass + ";Integrated Security=false;Connect Timeout=10";
//				if (m_sUser == "")
//					m_sSecurityString = " Integrated Security=SSPI; ";
				m_sPrinterName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "printer_name", ""));
				m_nPaperWidth = MyIntParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "paper_width", "")));
				m_sFontName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "font_name", ""));
				m_sFontSize = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "font_size", ""));
				m_sFontStyle = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "font_style", ""));
				m_sScaleName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "scale_name", ""));
				m_sScannerName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "scanner_name", ""));
				m_sStationID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "station_id", ""));
				m_nTimeout = MyIntParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "opos_timeout", "")));
				m_sAdUrl = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "ad_url", ""));

				m_sDMoniter = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "dMonitor", "")));
				m_sEpson = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "epson", "")));
				/*************sync*****************/
				m_sFtpURL = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "ftp_url", ""));
				m_sFtpUser = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "ftp_user", ""));
				m_sFtpPassword = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "ftp_password", ""));
				m_sFtpPassword = EDS(m_sFtpPassword);
				m_sFtpRemotePath = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "ftp_remote_path", ""));
				m_sMainDBName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "main_db_name", ""));
				m_sBranchID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "branch_id", ""));

				if (m_sBranchID == "")
					m_sBranchID = "1";
				//              m_sServer = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "db_server", ""));
				//              m_sDataSource = ";data source=" + m_sServer + ";";
				//              m_sUser = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "db_user", ""));
				//              m_sPass = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "db_pass", ""));
				//              m_sCompanyName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "db_catalog", ""));
				m_sSecurityString = "User id=" + m_sUser + ";Password=" + m_sPass + ";Integrated Security=false;";
				if (m_sUser == "")
					m_sSecurityString = "Integrated Security=SSPI;";
				m_bSku = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "sku_item", "")));
				m_bItem = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "sync_item", "")));
				m_bButton = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "sync_button", "")));
				m_bInvoice = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKeyS + "\\", "sync_invoice", "")));
				/******************************/
				m_sgroceryitem = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "groceryitem", ""));
				m_sgroceryweight = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "groceryweight", ""));
				m_sgroceryitemaddup = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "groceryitemaddup", ""));
				m_sgroceryweightitemaddup = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "groceryweightitemaddup", ""));
				m_svoidscale = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "voidscale", ""));
				m_sbarcodewithprice1 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "barcodewithprice1", ""));
				m_sbarcodewithprice2 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "barcodewithprice2", ""));
				m_sbarcodewithprice3 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "barcodewithprice3", ""));
				m_sbarcodelength1 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "barcodelength1", ""));
				m_sbarcodelength2 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "barcodelength2", ""));
				m_sbarcodelength3 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "barcodelength3", ""));
				m_spricelength1 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "pricelength1", ""));
				m_spricelength2 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "pricelength2", ""));
				m_spricelength3 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "pricelength3", ""));
				m_spoint1 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "point1", ""));
				m_spoint2 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "point2", ""));
				m_spoint3 = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "point3", ""));
				m_svoiddis = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "voiddis", ""));
				m_bEnableEftpos = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enableeftpos", "")));
				m_senablescale = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enablescale", "")));
				m_bCasScale = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "cas_scale", "")));
				m_sCasScalePort = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "cas_scale_port", ""));
				m_sEftposCom = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "eftposCom", ""));
				m_sEftposLogDay = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "eftpos_log_day", ""));
				m_bEnableEftposLog = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enable_eftpos_log", "")));
				m_bRegularMonitor = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "regular_monitor", "")));
				m_bSmallMonitor = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "small_monitor", "")));
				m_bSmallMonitor = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "small_monitor", "")));
				m_sSerialDevicePort = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "serialDevicePort", ""));
				m_bEnableAutoTare = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "autotare", "")));
				m_bEnableFloating = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "float", "")));
				m_bEnableAutoSync = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "autosync", "")));
				m_bScanInCode = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "scanincode", "")));
				m_bScanInSupplierCode = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "scaninsuppliercode", ""))); 

				m_bEnableWechatPayment = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "wechat_payment", "")));
				m_sWeChatMerchantID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "wechat_merchant_id", ""));
				m_sWeChatUri = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "wechat_uri", ""));
				m_sWeChatSignature = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "wechat_signature", ""));
				m_bEnableAliPay = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "alipay_payment", "")));
				m_sAliPayMerchantID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "alipay_merchant_id", ""));
				m_sAliPayUri = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "alipay_uri", ""));

				m_bEnableSalesLogin = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "saleslogin", "")));
				m_bEnableUpdatePrice = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "updateprice", "")));
				m_bEnableCalQty = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "calqty", "")));
				m_bEnableMargin = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "margin", "")));
				m_bEnableLevelPrice = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "levelprice", "")));
				m_bEnableSimpleInvoice = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "simpleinvoice", "")));
				m_bEnableAA = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enable_aa", "")));
				m_sBranchID_sync = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "branch_id_sync", ""));
				m_bEnableVipDis = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "vip_discount", "")));
				m_bEnableDisControl = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "dis_control", "")));
				m_bEnableSpecialDis = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "special_discount", "")));

				m_bEnablePic = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enable_pic", "")));
				m_sPicroot = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "pic_root", ""));
				m_sTare = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "tareoff", ""));
				m_sAA = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "aa_amount_min", ""));
				m_backupdata = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "backup_Database", "")));
				m_backuproot = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "backup_root", ""));
				m_sync = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "sync", "")));
				m_bEnableWebService = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enable_web_service", "")));
                m_bSyncWithCloud = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "sync_with_cloud", "")));
				m_syncroot = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "sync_root", ""));
				m_sWebServiceUrl = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "web_service_url", ""));

				m_eftposType = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "eftpos_type", ""));
				m_debugLogType = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "debug_log_type", ""));
				if (m_debugLogType == "")
					m_debugLogType = "Error";

				string sCallOfOrderDate = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_date", ""));
				string sCallOfOrderNumber = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_number", ""));
				string sAutoClosePopupMenu = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "auto_close_popup_menu", ""));
				m_bAutoClosePopupMenu = Program.MyBooleanParse(sAutoClosePopupMenu);
				string sCallOfOrder = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_enabled", ""));
				m_bCallOfOrderNumber = Program.MyBooleanParse(sCallOfOrder);
				m_nCallOfOrderNumber = Program.MyIntParse(sCallOfOrderNumber);
				string sToday = DateTime.Now.ToString("dd-MM-yyyy");
				if (sCallOfOrderDate != sToday)
				{
					Registry.SetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_date", sToday);
					Registry.SetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_number", "1");
					m_nCallOfOrderNumber = 1;
				}
				if (m_nCallOfOrderNumber == 0)
					m_nCallOfOrderNumber = 1;

				m_sLicenseKey = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "key", ""));
				m_sDuration = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "duration", ""));
				m_sKeyType = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "key_type", ""));
				m_sKeyTypeMode = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "key_type_mode", ""));
				m_sProductId = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "productid", ""));
				m_sTrialDayLife = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "expired_day", ""));
				m_sKayHasExpireDate = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "has_expired_date", ""));
				m_sExpireDate = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "expire_date", ""));
				if (m_sExpireDate == "")
				{
					m_sExpireDate = DateTime.Now.AddDays(30).ToString("dd-MM-yyyy");
					Registry.SetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "expire_date", m_sExpireDate);
				}
				Registry.SetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "expired_day", m_sTrialDayLife);
				Registry.SetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "has_expired_date", m_sKayHasExpireDate);
				if (m_sLicenseKey == "" || m_sLicenseKey == "trial")
				{
					Registry.SetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "key", "trial");
					if (m_sTrialDayLife == "")
					{
						Registry.SetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "trial_day", "30");
						Registry.SetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "expired_day", "30");
					}
					Registry.SetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "has_expired_date", "True");
					Registry.SetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "key_type", "Free Key Mode");
				}
				try
				{
					m_sTrial = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "trial", ""));
					m_sTrialDay = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegLicenseKey + "\\", "trial_day", ""));
				}
				catch
				{
					m_sTrial = "-1";
					m_sTrialDay = "-1";
				}

				System.IFormatProvider format = new System.Globalization.CultureInfo("en-NZ", false);
				DateTime dtExpireDate = DateTime.Parse(m_sExpireDate, format, System.Globalization.DateTimeStyles.NoCurrentDateDefault);
				m_sTrialDayLife = (dtExpireDate - DateTime.Now).Days.ToString();
				Registry.SetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "expired_day", MyDoubleParse(m_sTrialDayLife).ToString());

				m_sSmartpayIP = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "smartpay_ip", ""));
				m_sSmartpayPort = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "smartpay_port", ""));
				if (m_sSmartpayIP == "")
				{
					m_sSmartpayIP = "127.0.0.1";
					m_sSmartpayPort = "11234";
				}

				if (m_nTimeout <= 0)
					m_nTimeout = 1000; //default to 1000

				if (m_sFontName == "")
					m_sFontName = "Arial";
				if (m_sFontSize == "")
					m_sFontSize = "8";
				switch (m_sFontStyle)
				{
					case "Bold":
						m_tFontStyle = FontStyle.Bold;
						break;
					case "Italic":
						m_tFontStyle = FontStyle.Italic;
						break;
					case "Strikeout":
						m_tFontStyle = FontStyle.Strikeout;
						break;
					case "Underline":
						m_tFontStyle = FontStyle.Underline;
						break;
					default:
						m_tFontStyle = FontStyle.Regular;
						break;
				}

				m_bEnableNVR = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "nvr_enabled", "")));
				m_sNVRIP = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "nvr_ip", ""));
				m_sNVRPort = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "nvr_port", ""));
				m_bIdCheckPasswordControl = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "id_check_password_control", "")));

				m_bEnableAttractPay = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AttractPay_enabled", "")));
				m_sAttractPayStoreID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AttractPay_store_id", ""));
				m_sAttractPayAuthCode = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AttractPay_auth_code", ""));
				m_sAttractPayUriRequest = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AttractPay_uri_request", ""));
				m_sAttractPayUriRequestBarcode = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AttractPay_uri_request_barcode", ""));
				m_sAttractPayUriCheck = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AttractPay_uri_check", ""));

				m_bEnableAlipayDirect = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AlipayDirect_enabled", "")));
				m_sAlipayDirectUriRequest = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AlipayDirect_uri_request", ""));
				m_sAlipayDirectPartnerID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AlipayDirect_partner_id", ""));
				m_sAlipayDirectKey = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "AlipayDirect_key", ""));

				//Latipay
				m_bEnableLatiPay = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_enabled", "")));
				m_bEnableLatiPayGiftCard = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_gift_card_enabled", "")));
				m_sLatiPayWalletID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_wallet_id", ""));
				m_sLatiPayUserID = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_user_id", ""));
				m_sLatiPayApiKey = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_apiKey", ""));
				m_sLatiPayQRCodeBackground = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_qrcodeBackground", ""));
				m_sLatiPayUrlInvoice = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_url_invoice", ""));
				m_sLatiPayUrlGiftCardFreeze = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_url_gc_freeze", ""));
				m_sLatiPayUrlGiftCardRedeem = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_url_gc_redeem", ""));
				m_sLatiPayUrlGiftCardDrawback = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "Latipay_url_gc_drawback", ""));

				//MyPosMate
				m_bEnableMyPosMate = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "myposmate_enable", "")));
				m_sMyPosMateMerchantId = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "myposmate_merchant_id", ""));
				m_sMyPosMateMerchantAccountId = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "myposmate_merchant_account_id", ""));
				m_sMyPosMateConfigId = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "myposmate_config_id", ""));
				m_sMyPosMateUrlPosPay = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "myposmate_url_pospay", ""));
				m_sMyPosMateUrlGetTransactionDetails = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "myposmate_url_get_transaction_details", ""));
				m_sMyPosMateUrlRefund = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "myposmate_url_refund", ""));

				//Second Printer Setting
				m_bEnableSecondPrinter = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_enable", "")));
				m_sSecondPrinterCatalog = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_catalog", ""));
				m_sSecondPrinterName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_name", ""));
				m_sSecondPrinterReceiptQTY = MyIntParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_receipt_qty", "")));
				m_nSecondPrinterPaperWidth =  MyIntParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_paper_width", "")));
				m_sSecondPrinterEpson = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_epson", "")));
				m_sSecondPrinterFontSize =  MyIntParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_size", "")));
				m_sSecondPrinterFontName = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_font", ""));
				m_sSecondPrinterFontStyle = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_style", ""));
				m_nSecondPrinterTimeout =  MyIntParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "second_printer_timeout", "")));
				m_bEnableSelfService = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enable_self_service", "")));
				m_bEnableQtyPassword = MyBooleanParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "enable_qty_password", "")));
			}
			else
			{
				Registry.CurrentUser.CreateSubKey(m_sRegKey);
				//				FormConfig fm = new FormConfig();
				//				fm.ShowDialog();
			}
		}
		public static string GetCallOfOrderNumber(string order_id)
		{
			string s = m_nCallOfOrderNumber.ToString("D3");
			if (order_id == "" || order_id == "0" || order_id == m_sOrderIdCurrent)
				return s;
			m_sOrderIdCurrent = order_id;
			if (!m_bCallOfOrderNumber)
				return "";
			string sCallOfOrderNumber = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_number", ""));
			m_nCallOfOrderNumber = Program.MyIntParse(sCallOfOrderNumber);
			s = m_nCallOfOrderNumber.ToString("D3");
			m_nCallOfOrderNumber++;
			Registry.SetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_number", m_nCallOfOrderNumber.ToString());
			return s;
		}
		public static bool checkLocation()
		{
			if (ds.Tables["checkLocation"] != null)
				ds.Tables["checkLocation"].Clear();
			string sc = "";
			int nRows = 0;
			sc = " SELECT * FROM syscolumns where id=object_id('button_item') and name='location'";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(ds, "checkLocation");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return false;
			}
			if (nRows > 0)
				return true;
			else
				return false;
		}
		public static bool SqlTableExists(string table_name)
		{
			string sc = " SELECT table_name from INFORMATION_SCHEMA.TABLES WHERE table_name = '" + table_name + "' ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				if (myCommand.Fill(ds, "sqltableexists") > 0)
					return true;
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			return false;
		}
		public static string EDS(string p)
		{
			if (p == null || p == "")
				return p;
			string key = "9aysdata";
			byte[] bs = Encoding.GetEncoding("gb2312").GetBytes(p);
			byte[] bk = Encoding.GetEncoding("gb2312").GetBytes(key);
			for (int i = 0; i < bs.Length; i++)
			{
				for (int m = 0; m < bk.Length; m++)
					bs[i] = (byte)(bs[i] ^ bk[m]);
			}
			return Encoding.GetEncoding("gb2312").GetString(bs);
		}
		public static string[] SplitCSV(string input)
		{
			Regex csvSplit = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))", RegexOptions.Compiled);
			List<string> aStr = new List<string>();
			foreach (Match match in csvSplit.Matches(input))
			{
				aStr.Add(match.Value.TrimStart(','));
			}
			return aStr.ToArray();
		}
		public static string[] MySplitCSV(string sLine)
		{
			char[] cb = sLine.ToCharArray();
			int pos = 0;
			//			string[] av = new string[255];
			List<string> av = new List<string>();
			while (pos < cb.Length)
			{
				char[] cbr = new char[cb.Length];
				int i = 0;
				if (cb[pos] == '\"')
				{
					while (true)
					{
						pos++;
						if (pos == cb.Length)
							break;
						if (cb[pos] == '\"')
						{
							pos++;
							if (pos >= cb.Length)
								break;
							if (cb[pos] == '\"')
							{
								cbr[i++] = '\"';
								continue;
							}
							else if (cb[pos] != ',')
							{
								//								ShowMsg("Error, CSV file corrupt, comma not followed quote. Line=" + new string(cb));
								break;
							}
							else
							{
								pos++;
								break;
							}
						}
						cbr[i++] = cb[pos];
					}
				}
				else
				{
					while (cb[pos] != ',')
					{
						cbr[i++] = cb[pos];
						pos++;
						if (pos >= cb.Length)
							break;
					}
					pos++;
				}
				string s = new string(cbr, 0, i);
				av.Add(s);
				if (pos == cb.Length && cb[pos - 1] == ',')
				{
					av.Add("");
					break;
				}
			}
			return av.ToArray();
		}
		public static bool GetHotkey()
		{
			if (ds.Tables["hotkey"] != null)
				ds.Tables["hotkey"].Clear();
			string sc = " SELECT * FROM hotkey ORDER BY name ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "hotkey");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			m_dtHotkey = ds.Tables["hotkey"];
			if (ds.Tables["card"] != null)
				ds.Tables["card"].Clear();
			sc = " SELECT trading_name FROM card WHERE id = 1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(ds, "card") > 0)
					m_sTradingName = ds.Tables["card"].Rows[0]["trading_name"].ToString();
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			return true;
		}
		public static string LookupHotKey(KeyEventArgs e)
		{
			if (m_dtHotkey == null)
				return "";
			string ctrl = "0";
			string alt = "0";
			string shift = "0";
			if (e.Control)
				ctrl = "1";
			if (e.Alt)
				alt = "1";
			if (e.Shift)
				shift = "1";
			DataRow[] drs = m_dtHotkey.Select("key_code = " + e.KeyValue.ToString() + " AND ctrl = " + ctrl + " AND alt = " + alt + " AND shift = " + shift);
			if (drs.Length <= 0)
				return "";
			return drs[0]["code_name"].ToString();
		}
		public static System.Windows.Forms.Control FindFocusedControl(System.Windows.Forms.Control container)
		{
			foreach (System.Windows.Forms.Control childControl in container.Controls)
			{
				if (childControl.Focused)
				{
					return childControl;
				}
			}
			foreach (System.Windows.Forms.Control childControl in container.Controls)
			{
				System.Windows.Forms.Control maybeFocusedControl = FindFocusedControl(childControl);
				if (maybeFocusedControl != null)
				{
					return maybeFocusedControl;
				}
			}
			return null; // Couldn't find any, darn!
		}
		public static void ShowParseException(string s)
		{
			MessageBox.Show("Input string is not in correct format:" + s);
		}
		public static long MyLongParse(string s)
		{
			s = s.Trim();
			if (s == null || s == "")
				return 0;

			long n = 0;
			try
			{
				n = long.Parse(s);
			}
			catch
			{
				ShowParseException(s);
			}
			return n;
		}
		public static int MyIntParse(string s)
		{
			s = s.Trim();
			if (s == null || s == "")
				return 0;
			return (int)MyDoubleParse(s);
		}
		public static double MyDoubleParse(string s)
		{
			if (s == null)
				return 0;
			s = s.Trim();
			if (s == "")
				return 0;
			s = s.Trim();
			if (s.IndexOf("(") == 0 && s.IndexOf(")") == s.Length - 1)
			{
				s = s.Replace("(", "");
				s = s.Replace(")", "");
				s = "-" + s;
			}
			double d = 0;
			try
			{
				d = double.Parse(s);
			}
			catch
			{
				ShowParseException(s);
			}
			return d;
		}
		public static bool MyBooleanParse(string s)
		{
			if (s == null)
				return false;
			s = s.Trim();
			if (s == "" || s == "0")
				return false;
			else if (s == "1")
				return true;
			else if (s == "on")
				return true;
			else if (s == "true")
				return true;
			else if (s == "True")
				return true;
			else if (s == "On")
				return true;
			else if (s == "ON")
				return true;
			else if (s == "TRUE")
				return true;
			else if (s == "off")
				return false;

			bool b = false;
			try
			{
				b = Boolean.Parse(s);
			}
			catch
			{
				ShowParseException(s);
			}
			return b;
		}
		public static void Trim(ref string s)
		{
			if (s == null)
				return;
			s = s.TrimStart(null);
			s = s.TrimEnd(null);
		}
		public static double MyMoneyParse(string s)
		{
			if (s == null)
				return 0;
			s = s.Trim();
			if (s == "")
				return 0;

			double d = 0;
			try
			{
				d = double.Parse(s, NumberStyles.Currency, null);
			}
			catch
			{
				ShowParseException(s);
			}
			return d;
		}
		public static void ShowExp(string sc, Exception e)
		{
			MessageBox.Show("SQL error:" + e.ToString() + "\r\nsc=" + sc, "Database Query Error");
			g_log.Log("SQL error:" + e.ToString() + "\r\nsc=" + sc);
			g_log.LogSqlErr(e.ToString() + "\r\nsc=" + sc);
		}
		public static string EncodeQuote(string s)
		{
			if (s == null)
				return null;
			string ss = "";
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '\'')
					ss += '\''; //double it for SQL query
				ss += s[i];
			}
			return ss;
		}
		public static bool ZipOneFile(string FileToZip, string ZipedFile, int CompressionLevel, int BlockSize)
		{
			if (!System.IO.File.Exists(FileToZip))
			{
				MessageBox.Show("The specified file " + FileToZip + " could not be found. Zipping aborderd");
				return false;
			}

			//			Crc32 crc32 = new Crc32();
			ZipOutputStream zos = new ZipOutputStream(File.Create(ZipedFile));
			zos.SetLevel(9); // 9 = highest compression
			ZipEntry entry = new ZipEntry(Path.GetFileName(FileToZip));
			entry.DateTime = DateTime.Now;
			using (FileStream fs = File.OpenRead(FileToZip))
			{
				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				entry.Size = fs.Length;
				fs.Close();
				//				crc32.Reset();
				//				crc32.Update(buffer);
				//				entry.Crc = crc32.Value;
				zos.PutNextEntry(entry);
				zos.Write(buffer, 0, buffer.Length);
			}
			zos.Finish();
			zos.Close();
			return true;
		}
		public static bool UnZipOneFile(string SrcFile, string DstFile)
		{
			if (!System.IO.File.Exists(SrcFile))
			{
				MessageBox.Show("The specified file " + SrcFile + " could not be found. unzipping aborderd");
				return false;
			}
			FileStream fileStreamIn = new FileStream(SrcFile, FileMode.Open, FileAccess.Read);
			ZipInputStream zipInStream = new ZipInputStream(fileStreamIn);
			ZipEntry entry;
			try
			{
				entry = zipInStream.GetNextEntry();
			}
			catch (Exception e)
			{
				zipInStream.Close();
				fileStreamIn.Close();
				return false;
			}
			//			FileStream fileStreamOut = new FileStream(DstFile + @"\" + entry.Name, FileMode.Create, FileAccess.Write);
			FileStream fileStreamOut = new FileStream(DstFile, FileMode.Create, FileAccess.Write);
			int size = 0;
			byte[] buffer = new byte[51200];
			do
			{
				size = zipInStream.Read(buffer, 0, buffer.Length);
				fileStreamOut.Write(buffer, 0, size);
			} while (size > 0);
			zipInStream.Close();
			fileStreamOut.Close();
			fileStreamIn.Close();
			return true;
		}
		public static string CSVNextColumn(char[] cb, ref int pos)
		{
			if (pos == cb.Length)
			{
				pos++;
				return "";
			}
			if (pos > cb.Length)
				return null;

			char[] cbr = new char[cb.Length];
			int i = 0;
			if (cb[pos] == '\"')
			{
				while (true)
				{
					pos++;
					if (pos == cb.Length)
						break;
					if (cb[pos] == '\"')
					{
						pos++;
						if (pos >= cb.Length)
							break;
						if (cb[pos] == '\"')
						{
							cbr[i++] = '\"';
							continue;
						}
						else if (cb[pos] != ',')
						{
							//							ShowMsg("Error, CSV file corrupt, comma not followed quote. Line=" + new string(cb));
							break;
						}
						else
						{
							pos++;
							break;
						}
					}
					cbr[i++] = cb[pos];
				}
			}
			else
			{
				while (cb[pos] != ',')
				{
					cbr[i++] = cb[pos];
					pos++;
					if (pos == cb.Length)
						break;
				}
				pos++;
			}
			return new string(cbr, 0, i);
		}
		public static string ReadSitePage(string name)
		{
			DataSet ds = new DataSet();
			name = name.ToLower();
			string s = "";
			string sc = " SELECT id, text, cat FROM site_pages WHERE name = N'" + Program.EncodeQuote(name) + "' ";
			int nRows = 0;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(ds);
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
			}
			if (nRows > 0)
			{
				s = ds.Tables[0].Rows[0]["text"].ToString();
				return s;
			}

			sc = " BEGIN TRANSACTION ";
			sc += " INSERT INTO site_pages (name, text) VALUES('" + Program.EncodeQuote(name) + "', '') ";
			sc += " SELECT IDENT_CURRENT('site_pages') AS id ";
			sc += " COMMIT ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(ds, "rsp") <= 0)
					return "";
			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return "";
			}
			string sid = ds.Tables["rsp"].Rows[0]["id"].ToString();
			sc = " INSERT INTO site_sub_pages (id, inuse, text) VALUES(" + sid + ", 1, '') ";
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
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "";
			}
			return "";
		}
		public static bool IsInt(string s)
		{
			int n;
			return int.TryParse(s, out n);
		}
		public static double RoundUp(double dIn)
		{
			double dOut = dIn;
			double dCents = Math.Abs(Math.Round(dIn - Math.Round(dIn, 1), 2));
			dOut -= dCents;
			if (dCents > 0)
				dOut += 0.1;
			return dOut;
		}
		public static double RoundCents(double dIn, string sRoundingCentSetting) //eg dIn = 10.26
		{
			double dOut = 0;
			double dSubIn = dIn;
			if (dIn < 0)
				dIn = 0 - dIn;
			string sIn = dIn.ToString("N2");			//sIn = 10.26
			int p = sIn.IndexOf(".");				// p = 2
			if (p <= 0 || (sIn.Length - p) <= 2)		//sIn.Length = 5
				return dIn;
			p = p + 2; //to first digits after pointer	// p = 3
			string sLead = sIn.Substring(0, p);	//sLead = 10.2
			dOut = Program.MyDoubleParse(sLead);	//dOut = 10.2
			string sCents = sIn.Substring(p, 1);	//sCents = 6
			int nCents = Program.MyIntParse(sCents);//nCents = 6
			if (nCents >= int.Parse(sRoundingCentSetting))
				dOut += 0.1;
			if (dSubIn < 0)
				dOut = 0 - dOut;					//dOut = 10.3
			return dOut;
		}
		public static string md5(string input)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
			bs = x.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs)
			{
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString().ToUpper();
		}
		public static bool IsNumberKey(char k)
		{
			if (k >= 'a' && k <= 'z')
				return false;
			if (k >= 'A' && k <= 'A')
				return false;
			if (k < '0' || k > '9')
			{
				if (k != '\b' && k != '.' && k != '-')
				{
					return false;
				}
			}
			return true;
		}
		public static string PrintProduct(string sIn, string sIn2, string sFill)
		{
			string s = sIn;
			int nWidth = m_nPaperWidth;
			int len = nWidth - sIn.Length - sIn2.Length;
			for (int i = 0; i < len; i++)
			{
				s += sFill;
			}
			s += sIn2;
			return s;
		}
		public static string PrintPadding(string sIn, string sIn2, string sFill)
		{
			string s = sIn;
			int nWidth = m_nPaperWidth;
			int len = nWidth - sIn.Length - sIn2.Length;
			for (int i = 0; i < len; i++)
			{
				s += sFill;
			}
			s += sIn2;
			return s;
		}
		public static string PrintPaddingSave(string sIn, string sIn2, string sFill)
		{
			string s = sIn;
			int nWidth = m_nPaperWidth;
			int len = nWidth - sIn.Length - sIn2.Length - 2;
			for (int i = 0; i < len; i++)
			{
				s += sFill;
			}
			s += sIn2;
			return s;
		}
		public static string PrintPaddingMid(string sIn, string sIn2, string sFill)
		{
			string s = sIn;

			int nWidth = m_nPaperWidth;
			int len = nWidth - sIn.Length - sIn2.Length;// +sp.Length;
			for (int i = 0; i < len; i++)
			{
				s += sFill;
			}
			s += sIn2;
			return s;
		}
		public static bool ResetTillDataAfterZTotal(string sStationID)
		{
			//do clean up
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = "1";//default to 1 in case not set
			string sc = " UPDATE orders SET order_deleted = 1, paid =1 WHERE station_id = " + sStationID + " ";
            sc += " UPDATE tran_invoice SET is_settled = 1 where Station_id = " + sStationID;
			sc += " UPDATE till SET last_z_total_time = GETDATE(), draw_opens_no_sales = 0, total_void = 0, total_cancel_sales = 0 ";
			sc += ", last_order_id = NULL, total_orders = 0, date_start = NULL, total_refund=0, total_refund_times=0 , Voucher=0, floating=0  ";
			sc += " WHERE station_id = " + sStationID;
//          Registry.SetValue("HKEY_CURRENT_USER\\" + m_sSecrectValue + "\\", "expired_day", (double.Parse(m_sTrialDayLife) - 1).ToString());
			//if (!CleanData(sStationID))
			//    MessageBox.Show("Sorry, Clean Up Date Error Occured, Code:10025, Till Number:"+sStationID+"", "Data Error : 10025");
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
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			Registry.SetValue("HKEY_CURRENT_USER\\" + m_sRegKey + "\\", "call_of_order_number", "1");
			m_nCallOfOrderNumber = 1;
			return true;
		}
		public static bool OpenTillDate(int sid)
		{
			string sc = " UPDATE till SET date_start = GETDATE() WHERE station_id =" + sid;
			sc += " AND date_start IS NULL";
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
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			return true;
		}
		public static bool CleanData(string sStationID)
		{
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = "1";//default to 1 in case not set
			string sc1 = " DELETE FROM trans WHERE id IN (SELECT td.id FROM tran_detail td JOIN orders o ON td.invoice_number = o.invoice_number WHERE o.station_id = " + sStationID + ") ";
			sc1 += " DELETE FROM tran_detail WHERE invoice_number IN (SELECT invoice_number FROM orders WHERE station_id=" + sStationID + ")";
			sc1 += " DELETE FROM tran_invoice WHERE invoice_number IN (select invoice_number from orders where station_id=" + sStationID + ")";
			sc1 += " DELETE FROM sales WHERE invoice_number IN (SELECT invoice_number FROM orders WHERE station_id=" + sStationID + ")";
			sc1 += " DELETE FROM order_item WHERE id IN (SELECT id FROM orders WHERE station_id=" + sStationID + ")";
			sc1 += " DELETE FROM orders WHERE station_id=" + sStationID;
			sc1 += " DELETE FROM eftposlog WHERE station_id=" + sStationID;

			try
			{
				myCommand = new SqlCommand(sc1);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc1, e);
				return false;
			}
			return true;
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/*
		Meaning
		Total : All payments, exclude Charge, minus rounding
		All Item Sales : orders.commit_price
		Charge:	Charged orders
		Account:Charged orders balance, paid only but may not paid in full

		Coding
		Charge order : 	order_deleted = 0, invoice.amount_paid = 0, no trans record
		VIP Payment:	order_deleted = 2, has trans record

		Total : 	order_deleted = 0, invoice.amount_paid = 0, = SUM(trans.amount) = (Cash + Eftpos + CreditCard + Cheque) 
		charge : 	order_deleted IN(2, 3), SUM(invoice.total) 
		Total Account:	order_deleted = 2, = SUM(invoice.total - amount_paid)
		Total Sales :	Total - Rounding - Cashout - Total Account + Charge

		Charge code:
		1. Pay by Charge : set order_deleted = 3
		2. VIP Payment : set order_deleted = 2
		 */
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public static string PrintXTotal(string sStationID, string sReportTitle, int bid)
		{
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = "1";//default to 1 in case not set

			string sDateStart = "";
			string sDateEnd = "";
			string sTotalOrders = "";
			double dCash = 0;
			double dEftpos = 0;
			double dCashout = 0;
			double dCheque = 0;
			double dCharge = 0;
			double dCC = 0;
			double dTotal = 0;
			double dRounding = 0;
			double dTotalSales = 0;
			double dCashOnHand = 0;
			double dTotalDiscount = 0;
			double dTotalSpecial = 0;
			double dTotalVoid = 0;
			double dTotalCancel = 0;
			double dLessTotal = 0;
			int nDrawOpen = 0;
			string sLastZTotalTime = "";
			string sLastOrderID = "";
			double dCatTotal = 0;
			//string sCat = "";
			double dTotalRefund = 0;
			int iTotalRefundTimes = 0;
			double s_dCatSalesTotal = 0;
			double sTotalVoucher = 0;
			double dFloating = 0;

			//payment data
			if (ds.Tables["payment"] != null)
				ds.Tables["payment"].Clear();
			string sc = " SELECT e.name AS payment_method, SUM(t.amount) AS amount ";
			sc += " FROM trans t JOIN tran_detail td ON td.id = t.id ";
			sc += " JOIN enum e ON e.id = td.payment_method AND e.class = 'payment_method' ";
			sc += " JOIN tran_invoice ti ON ti.tran_id = t.id ";
			sc += " JOIN orders o ON o.invoice_number = ti.invoice_number AND o.order_deleted <> 1 ";
            sc += " AND ti.is_settled = 0 "; 
			//			sc += " JOIN orders o ON o.invoice_number = ti.invoice_number AND o.order_deleted = 0 ";
			sc += " WHERE o.station_id = " + sStationID;
			sc += " GROUP BY e.name ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "payment");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}
			int i = 0;
			for (i = 0; i < ds.Tables["payment"].Rows.Count; i++)
			{
				DataRow dr = ds.Tables["payment"].Rows[i];
				string pm = dr["payment_method"].ToString().ToLower();
				double dAmount = Program.MyDoubleParse(dr["amount"].ToString());
				if (pm == "cash")
					dCash = dAmount;
				else if (pm == "eftpos")
					dEftpos = dAmount;
				else if (pm == "cheque")
					dCheque = dAmount;
				else if (pm == "credit card")
					dCC = dAmount;
				//				else if(pm == "charge")
				//					dCharge = dAmount;
				else if (pm == "cash out")
					dCashout = dAmount;
				else if (pm == "cash rounding")
					dRounding = dAmount;

			}

			if (ds.Tables["charge"] != null)
				ds.Tables["charge"].Clear();
			sc = " SELECT SUM(i.total) AS amount ";
			sc += " FROM invoice i ";
			sc += " JOIN orders o ON o.invoice_number = i.invoice_number AND o.order_deleted IN(2, 3) ";
            sc += " AND o.paid = 0 ";
			sc += " WHERE o.station_id = " + sStationID;
			//			sc += " AND i.amount_paid = 0 ";
			/*			sc =  " SELECT ISNULL(SUM(i.total),0) AS amount FROM invoice i";
						sc += " JOIN orders o ON o.invoice_number = i.invoice_number AND o.order_deleted = 0 ";
						sc += " JOIN tran_detail td ON td.invoice_number = o.invoice_number ";
						sc += " JOIN enum e on e.id = td.payment_method ";
						sc += " WHERE o.station_id =" + sStationID;
						sc += " AND i.amount_paid = 0 ";
						sc += " AND td.payment_method = 11 ";
			*/
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(ds, "charge") > 0)
					dCharge = Program.MyDoubleParse(ds.Tables["charge"].Rows[0]["amount"].ToString());
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}

			//			dTotal = dCash + dEftpos + dCharge + dCC + dCheque;
			dTotal = dCash + dEftpos + dCC + dCheque;
			//			if (dTotal == 0)
			//				return "0";
			dCashOnHand = dCash - dCashout;
			//orders data
			if (ds.Tables["total"] != null)
				ds.Tables["total"].Clear();
			sc = " SELECT SUM(o.total_discount) AS discount, SUM(o.total_special) AS special ";
			sc += " FROM orders o ";
			sc += " WHERE o.station_id = " + sStationID + " AND o.order_deleted = 0 ";
			sc += " GROUP BY o.station_id, o.order_deleted ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "total");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}
			if (ds.Tables["total"].Rows.Count > 0)
			{
				DataRow dr = ds.Tables["total"].Rows[0];
				dTotalDiscount = Program.MyDoubleParse(dr["discount"].ToString());
				dTotalSpecial = Program.MyDoubleParse(dr["special"].ToString());
			}

			//group promotion discount
			double dGPDiscount = 0;
			if (ds.Tables["gpdis"] != null)
				ds.Tables["gpdis"].Clear();
			sc = " SELECT SUM(s.commit_price * (1 + s.tax_rate)) AS amount ";
			sc += " FROM orders o ";
			sc += " JOIN invoice i ON i.invoice_number = o.invoice_number ";
			sc += " JOIN sales s ON s.invoice_number = o.invoice_number ";
			sc += " WHERE o.station_id = " + sStationID + " AND o.order_deleted <> 1 ";
			sc += " AND s.code = -1001 ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "gpdis");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "";
			}
			if (ds.Tables["gpdis"].Rows.Count > 0)
			{
				DataRow dr = ds.Tables["gpdis"].Rows[0];
				dGPDiscount = 0 - Program.MyDoubleParse(dr["amount"].ToString());
			}
			dTotalDiscount += dGPDiscount;

			//total sales
			if (ds.Tables["totalsales"] != null)
				ds.Tables["totalsales"].Clear();
			sc = " SELECT SUM(i.total) AS total_sales ";
			sc += " FROM orders o ";//LEFT OUTER JOIN order_item oi ON oi.id = o.id ";
			sc += " JOIN invoice i ON i.invoice_number = o.invoice_number ";
			sc += " WHERE o.station_id = " + sStationID + "";
            sc += " AND o.order_deleted <> 1 ";
            sc += " AND o.paid = 0 ";
			//			sc += " GROUP BY o.station_id, o.order_deleted";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "totalsales");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}
			if (ds.Tables["totalsales"].Rows.Count > 0)
			{
				DataRow dr = ds.Tables["totalsales"].Rows[0];
				dTotalSales = Program.MyDoubleParse(dr["total_sales"].ToString());
			}

			//account 
			double dAccountTotal = 0;
			if (ds.Tables["totalaccount"] != null)
				ds.Tables["totalaccount"].Clear();
			sc = " SELECT SUM(t.amount) AS amount ";
			sc += " FROM trans t ";
			sc += " JOIN tran_invoice ti ON ti.tran_id = t.id ";
			sc += " JOIN orders o ON o.invoice_number = ti.invoice_number ";
			sc += " WHERE o.station_id = " + sStationID + " AND o.order_deleted = 2 ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "totalaccount");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}

			dAccountTotal = Program.MyDoubleParse(ds.Tables["totalaccount"].Rows[0]["amount"].ToString());
			//			dLessTotal = dRounding + dCashout - dCharge + dAccountTotal;
			//			dTotalSales = dTotal - dLessTotal;
			//			dTotalSales = dTotal - dRounding - dCashout - dAccountTotal + dCharge;

			//till data
			if (ds.Tables["till"] != null)
				ds.Tables["till"].Clear();
			sc = " SELECT draw_opens_no_sales, total_orders, total_void, total_cancel_sales ";
			sc += ", last_order_id, last_z_total_time, date_start, getdate() AS date_end, total_refund, total_refund_times , Voucher, floating ";
			sc += " FROM till ";
			sc += " WHERE station_id = " + sStationID + " ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "till");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}
			if (ds.Tables["till"].Rows.Count > 0)
			{
				DataRow dr = ds.Tables["till"].Rows[0];
				dTotalVoid = Program.MyDoubleParse(dr["total_void"].ToString());
				dTotalCancel = Program.MyDoubleParse(dr["total_cancel_sales"].ToString());
				nDrawOpen = Program.MyIntParse(dr["draw_opens_no_sales"].ToString());
				sLastOrderID = dr["last_order_id"].ToString();
				sLastZTotalTime = dr["last_z_total_time"].ToString();
				sTotalOrders = dr["total_orders"].ToString();
				sDateStart = dr["date_start"].ToString();
				sDateEnd = dr["date_end"].ToString();
				dTotalRefund = Program.MyDoubleParse(dr["total_refund"].ToString());
				sTotalVoucher = Program.MyDoubleParse(dr["Voucher"].ToString());
				iTotalRefundTimes = int.Parse(dr["total_refund_times"].ToString());
				dFloating = Program.MyDoubleParse(dr["floating"].ToString());
			}
			string sRounding = Math.Abs(dRounding).ToString("c");
			dRounding = Math.Round(dRounding, 2);
			string s = sReportTitle + "-TOTAL REPORT \r\n";
			s += "Till No:" + sStationID + "\r\n";
			s += "Date From:" + sDateStart + "\r\n";
			s += "Date To:" + sDateEnd + "\r\n";
			s += "Total Transaction: " + sTotalOrders + "\r\n";
			s += "Branch:" + getBranchName(bid) + "\r\n";
			s += PrintPadding("", "", "=") + "\r\n";
			s += PrintPaddingMid("CASH:", dCash.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("EFTPOS:", dEftpos.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("CHEQUE:", dCheque.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("CREDIT CARD:", dCC.ToString("c"), " ") + "\r\n";


			s += "**********\r\n";
			s += PrintPaddingMid("TOTAL:", dTotal.ToString("c"), " ") + "\r\n";
			s += PrintPadding("", "", "=") + "\r\n";
			s += PrintPaddingMid("ROUNDING ALLOWANCE:", sRounding, " ") + "\r\n";
			s += PrintPaddingMid("WITHDRAW CASH:", "-" + dCashout.ToString("c"), " ") + "\r\n";
			s += "**********\r\n";

//			if (dCharge != 0)
				s += PrintPaddingMid("CHARGE ACCOUNT:", "+" + dCharge.ToString("c"), " ") + "\r\n";
//			if (dAccountTotal != 0)
				s += PrintPaddingMid("ACCOUNT PAYMENT:", "-" + dAccountTotal.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("TOTAL SALES:", dTotalSales.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("CASH ON HAND:", dCashOnHand.ToString("c"), " ") + "\r\n";
			s += PrintPadding("", "", "=") + "\r\n";
			s += PrintPaddingMid("TOTAL DISCOUNT GIVEN:", dTotalDiscount.ToString("c"), " ") + "\r\n";
			//			s += PrintPaddingMid("TOTAL SPECIAL GIVEN:", dTotalSpecial.ToString("c"), " ") + "\r\n";
			s += PrintPadding("", "", "=") + "\r\n";
			if (Program.m_bEnableFloating)
				s += PrintPaddingMid("FLOATING:", dFloating.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("VOUCHER:", sTotalVoucher.ToString("c"), " ") + "\r\n";
			s += PrintPadding("", "", "=") + "\r\n";
			if (ds.Tables["cat"] != null)
				ds.Tables["cat"].Clear();
			//sc = " SELECT cat FROM catalog WHERE cat <>'Brands' GROUP BY cat , seq ORDER BY ABS(seq)";
			//sc = " SELECT c.cat FROM code_relations c LEFT OUTER JOIN catalog ca ON c.cat = ca.cat GROUP BY c.cat, ca.seq ORDER BY ABS(ca.seq)";
			sc = " SELECT DISTINCT c.cat FROM code_relations c ORDER BY c.cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "cat");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "";
			}
			sc += "',";
			for (int ii = 0; ii < ds.Tables["cat"].Rows.Count; ii++)
			{
				DataRow drs = ds.Tables["cat"].Rows[ii];
				string catName = drs["cat"].ToString();
				if (catName == "")
					catName = "OTHERS";

				s_dCatSalesTotal = Program.MyDoubleParse(catSalesTotal(catName, sStationID));
				if (s_dCatSalesTotal == 0)
					continue;
				string catSaleTotalLen = s_dCatSalesTotal.ToString("c");

				dCatTotal += s_dCatSalesTotal;
				dCatTotal = Math.Round(dCatTotal, 4);
				s += PrintPaddingMid(catName, s_dCatSalesTotal.ToString("c"), " ") + "\r\n";
				catName = "";
			}
			s += "**********\r\n";
			s += PrintPaddingMid("ALL ITEM SALES TOTAL:", dCatTotal.ToString("c"), " ") + "\r\n";
			s += PrintPadding("", "", "=") + "\r\n";
			s += PrintPaddingMid("TOTAL VOIDED:", Math.Round(dTotalVoid, 1).ToString(), " ") + "\r\n";
			s += PrintPaddingMid("TOTAL AMOUNT VOIDED:", dTotalCancel.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("TOTAL REFUNDED:", iTotalRefundTimes.ToString(), " ") + "\r\n";
			s += PrintPaddingMid("TOTAL AMOUNT REFUNDED:", dTotalRefund.ToString("c"), " ") + "\r\n";
			s += PrintPaddingMid("TOTAL OPEN DRAW WITHOUT SALES:", nDrawOpen.ToString(), " ") + "\r\n";
			s += "Last Z Total:" + sLastZTotalTime + "\r\n";
			m_bReprintTotal = s;
			return s;
		}
		public static string rePrintTotal(int till, int bhid)
		{
			DataSet dsi = new DataSet();
			if (dsi.Tables["reprinttotal"] != null)
				dsi.Tables["reprinttotal"].Clear();
			string sc = " SELECT * FROM ztotal WHERE till_no=" + till;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dsi, "reprinttotal");
			}
			catch (Exception)
			{
				return "0";
			}
			DataRow dsr = dsi.Tables["reprinttotal"].Rows[0];
			string rTillNo = dsr["till_no"].ToString();
			string rTillType = dsr["till_report_type"].ToString();
			string rTilldate1 = dsr["till_datetime1"].ToString();
			string rTilldate2 = dsr["till_datetime2"].ToString();
			string rTillTrans = dsr["till_trans"].ToString();
			string rTillBranch = dsr["till_branch"].ToString();
			string rTillCash = dsr["till_cash"].ToString();
			string rTillEftpos = dsr["till_eftpos"].ToString();
			string rTillCC = dsr["till_cc"].ToString();
			string rTillCheque = dsr["till_cheque"].ToString();
			string rTillCharge = dsr["till_charge"].ToString();
			string rTillRounding = dsr["till_rounding"].ToString();
			string rTillCashout = dsr["till_cashout"].ToString();
			string rTillSalesTotal = dsr["till_sales_total"].ToString();
			string rTillCashOnHand = dsr["till_cash_onhand"].ToString();
			string rTillDiscount = dsr["till_discount"].ToString();
			string rTillSpecial = dsr["till_special"].ToString();
			string rTillBBQ = dsr["till_bbq"].ToString();
			string rTillFish = dsr["till_fish"].ToString();
			string rTillButchery = dsr["till_butchery"].ToString();
			string rTillFrozen = dsr["till_frozen"].ToString();
			string rTillOthers = dsr["till_others"].ToString();
			string rTillTaiping = dsr["till_taiping"].ToString();
			string rTillHomecity = dsr["till_homecity"].ToString();
			string rTillVega = dsr["till_vega"].ToString();
			string rTillDeptTotal = dsr["till_dept_total"].ToString();
			string rTillAvoidTotal = dsr["till_total_avoided"].ToString();
			string rTillAvoidTotalAmount = dsr["till_total_avoided_amount"].ToString();
			string rTillRefund = dsr["till_refund"].ToString();
			string rTillRefundAmount = dsr["till_refund_amount"].ToString();
			string rTillCashDraw = dsr["till_total_open_cashdraw"].ToString();
			string rTillFooter = dsr["till_footer"].ToString();
			double rTillTotal = Program.MyDoubleParse(rTillCash) + Program.MyDoubleParse(rTillEftpos) + Program.MyDoubleParse(rTillCheque) + Program.MyDoubleParse(rTillCC) + Program.MyDoubleParse(rTillCharge);
			rTillTotal = Math.Round(rTillTotal, 2);
			string pt = rTillType + "-TOTAL REPORT \r\n";
			pt += "Till No:" + rTillNo + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
			pt += "Date From:" + rTilldate1 + "\r\n";
			pt += "Date To:" + rTilldate2 + "\r\n";
			pt += "Total Transaction: " + rTillTrans + "\r\n";
			pt += "Branch:" + getBranchName(bhid) + "\r\n";
			pt += PrintPadding("", "", "=") + "\r\n";
			pt += PrintPaddingMid("CASH:", Program.MyDoubleParse(rTillCash).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("EFTPOS:", Program.MyDoubleParse(rTillEftpos).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("CHEQUE:", Program.MyDoubleParse(rTillCheque).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("CREDIT CARD:", Program.MyDoubleParse(rTillCC).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("CHARGE:", Program.MyDoubleParse(rTillCharge).ToString("c"), " ") + "\r\n";
			pt += "**********\r\n";
			pt += PrintPaddingMid("TOTAL:", rTillTotal.ToString("c"), " ") + "\r\n";
			pt += PrintPadding("", "", "=") + "\r\n";
			pt += PrintPaddingMid("ROUNDING ALLOWANCE:", Program.MyDoubleParse(rTillRounding).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("WITHDRAW CASH:", Program.MyDoubleParse(rTillCashout).ToString("c"), " ") + "\r\n";
			pt += "**********\r\n";
			pt += PrintPaddingMid("TOTAL SALES:", Program.MyDoubleParse(rTillSalesTotal).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("CASH ON HAND:", Program.MyDoubleParse(rTillCashOnHand).ToString("c"), " ") + "\r\n";
			pt += PrintPadding("", "", "=") + "\r\n";
			pt += PrintPaddingMid("TOTAL DISCOUNT GIVEN:", Program.MyDoubleParse(rTillDiscount).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("TOTAL SPECIAL GIVEN:", Program.MyDoubleParse(rTillSpecial).ToString("c"), " ") + "\r\n";
			pt += PrintPadding("", "", "=") + "\r\n";
			pt += "**********\r\n";

			pt += PrintPaddingMid("BBQ:", Program.MyDoubleParse(rTillBBQ).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("FISH:", Program.MyDoubleParse(rTillFish).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("BUTCHERY:", Program.MyDoubleParse(rTillButchery).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("VEGA:", Program.MyDoubleParse(rTillVega).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("FROZEN:", Program.MyDoubleParse(rTillFrozen).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("OTHERS:", Program.MyDoubleParse(rTillOthers).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("TAIPING:", Program.MyDoubleParse(rTillTaiping).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("HomeCity:", Program.MyDoubleParse(rTillHomecity).ToString("c"), " ") + "\r\n";

			pt += PrintPaddingMid("ALL ITEM SALES TOTAL:", Program.MyDoubleParse(rTillDeptTotal).ToString("c"), " ") + "\r\n";
			pt += PrintPadding("", "", "=") + "\r\n";
			pt += PrintPaddingMid("TOTAL VOIDED:", rTillAvoidTotal, " ") + "\r\n";
			pt += PrintPaddingMid("TOTAL AMOUNT VOIDED:", Program.MyDoubleParse(rTillAvoidTotalAmount).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("TOTAL REFUNDED:", rTillRefund, " ") + "\r\n";
			pt += PrintPaddingMid("TOTAL AMOUNT REFUNDED:", Program.MyDoubleParse(rTillRefundAmount).ToString("c"), " ") + "\r\n";
			pt += PrintPaddingMid("TOTAL OPEN DRAW WITHOUT SALES:", rTillCashDraw, "  ") + "\r\n";
			pt += "Last Z Total:" + rTillFooter + "\r\n";
			return pt;
		}
		public static string getBranchName(int brid)
		{
			if (ds.Tables["branch"] != null)
				ds.Tables["branch"].Clear();
			string sc = " SELECT name FROM branch WHERE id=" + brid;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(ds, "branch") < 0)
					return "Infinity";
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "Infinity";
			}
			string m_sBName = ds.Tables["branch"].Rows[0]["name"].ToString();
			return m_sBName;
		}
		public static string catSalesTotal(string cName, string stationID)
		{
			double dCatSales = 0;
			if (cName == "OTHERS")
				cName = "";
			//Catagories 
			if (ds.Tables["catsum"] != null)
				ds.Tables["catsum"].Clear();
			string sc = " SELECT SUM(oi.order_total) AS cat_order_total";
			sc += " FROM order_item oi JOIN orders o ON o.id = oi.id ";
			sc += " WHERE oi.station_id =" + stationID;
			sc += " AND oi.cat = N'" + cName + "'";
            sc += "  collate Chinese_PRC_CS_AI  ";
            sc += " AND o.order_deleted <> 1 ";
            sc += " AND o.paid = 0 ";
			sc += " AND o.invoice_number !='' ";
			sc += " GROUP BY oi.cat";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "catsum");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}
			for (int e = 0; e < ds.Tables["catsum"].Rows.Count; e++)
			{

				DataRow dr = ds.Tables["catsum"].Rows[e];
				dCatSales = Program.MyDoubleParse(dr["cat_order_total"].ToString());
			}
			return dCatSales.ToString();
		}
		public static bool RecordTillData(string sColName, string sValue)
		{
			string sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = "1";//default to 1 in case not set
			string sc = " UPDATE till SET " + sColName + " = " + sColName + " + '" + sValue + "' WHERE station_id = " + sStationID;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception)
			{
				//Program.ShowExp(sc, e);
				MessageBox.Show("Delete Item Error. Error Code :10006");
				myCommand.Connection.Close();
				return false;
			}
			return true;
		}

		public static bool RecordTillData1(string sColName, string sValue)
		{
			string sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = "1";//default to 1 in case not set
			string sc = "";
			sc += " IF NOT EXISTS(SELECT * FROM till WHERE station_id = " + sStationID + ")";
			sc += " INSERT INTO till(station_id)VALUES('" + sStationID + "')";
			//			sc += " ELSE ";
			sc += " UPDATE till SET " + sColName + " = '" + sValue + "' WHERE station_id = " + sStationID;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception)
			{
				//Program.ShowExp(sc, e);
				MessageBox.Show("Delete Item Error. Error Code :10006");
				myCommand.Connection.Close();
				return false;
			}
			return true;
		}
		//Avoid Log
		public static bool RecordVoidLog(string m_dAmount, string m_sStaff)
		{
			string tillNo = Program.m_sStationID;
			if (Program.MyIntParse(tillNo) == 0)
				tillNo = Program.m_sStationID;
			if (Program.MyIntParse(tillNo) == 0)
				tillNo = "1";//default to 1 in case not set
			string sc = " INSERT INTO void_log (station_id, amount, staff) VALUES (" + Program.MyIntParse(tillNo) + ", '" + m_dAmount + "', '" + m_sStaff + "')";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception)
			{
				//Program.ShowExp(sc, e);
				MessageBox.Show("Delete Item Error. Error Code :10007");
				myCommand.Connection.Close();
				return false;
			}
			return true;
		}
		public static bool CheckTillStart()
		{
			string sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = Program.m_sStationID;
			if (Program.MyIntParse(sStationID) == 0)
				sStationID = "1";//default to 1 in case not set
			string sc = " IF (SELECT date_start FROM till WHERE station_id = " + sStationID + ") IS NULL ";
			sc += " UPDATE till SET date_start = GETDATE() WHERE station_id = " + sStationID;
			try
			{
				myCommand = new SqlCommand(sc, myConnection);
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			return true;
		}
		public static string CheckLicense()
		{
			//return "ok";		
			string sRes = "ok";
			if (m_sLicenseKey.Length <= 25 || m_sLicenseKey == "trial")
				return sRes = "1";

			switch (m_sKeyType)
			{
				case "Free Key Mode":
					if (!DoFreeKeyModeValidate())
						sRes = "2";
					else
						sRes = "ok";
					break;
				case "Encript Key Mode":
					if (!DoEncriptKeyMode())
						sRes = "2";
					else
						sRes = "ok";
					break;
				case "Auto Encript Key Mode":
					if (!DoEncriptKeyMode())
						sRes = "2";
					else
						sRes = "ok";
					break;
				default:
					break;
			}
			return sRes;
		}
		public static bool CheckKeyDate()
		{
			if (bool.Parse(m_sKayHasExpireDate))
			{
				return true;
			}
			else
				return false;
		}
		private static bool DoFreeKeyModeValidate()
		{
			try
			{
				int.Parse(m_sDuration);
			}
			catch
			{
				return false;
			}
			string keyTime = int.Parse(m_sDuration).ToString("D3");
			string keySet = m_sProductId + GetMotherboardSerialNumber();
			string keySetValue = Program.md5(keySet).Substring(8, 20);
			string keyFinal = keySetValue + keyTime;
			string finalKey = Program.md5(keyFinal).Substring(0, 25);
			string keyDecript = Decrypt(m_sLicenseKey);
			string newKey = "";
			for (int i = 0; i < keyDecript.Length; i++)
			{
				newKey = keyDecript.Replace("-", "");
			}
			if (newKey == finalKey)
				return true;
			else
				return false;
		}
		private static bool DoEncriptKeyMode()
		{

			string m_sSoftWareSSID = GetMotherboardSerialNumber();
			if (m_sKeyTypeMode == "2")
				m_sSoftWareSSID = GetCpuId();
			try
			{
				int.Parse(m_sDuration);
			}
			catch
			{
				return false;
			}
			string keyTime = int.Parse(m_sDuration).ToString("D3");
			string keySet = m_sSoftWareSSID + keyTime;
			string keySetValue = Program.md5(keySet).Substring(0, 25);
			string keyDecript = Decrypt(m_sLicenseKey);
			string newKey = "";
			for (int i = 0; i < keyDecript.Length; i++)
			{
				newKey = keyDecript.Replace("-", "");
			}
			if (newKey == keySetValue)
				return true;
			else
				return false;
		}

		private static readonly byte[] IV =
		 new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };
		public static string Decrypt(string s)
		{

			if (s == null || s.Length == 0)
				return string.Empty;
			if (s.Length != 44)
				return "";
			string result = string.Empty;
			try
			{
				byte[] buffer = Convert.FromBase64String(s);
				TripleDESCryptoServiceProvider des =
					new TripleDESCryptoServiceProvider();
				MD5CryptoServiceProvider MD5 =
					new MD5CryptoServiceProvider();
				des.Key =
					MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(keyword));
				des.IV = IV;
				result = Encoding.ASCII.GetString(
					des.CreateDecryptor().TransformFinalBlock(
					buffer, 0, buffer.Length));
			}
			catch
			{
				throw;
			}
			return result;
		}
		private static string GetCpuId()
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
		private static string GetMotherboardSerialNumber()
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
		public static string GetKeySettings(string name)
		{
			if (ds.Tables["keysetting"] != null)
				ds.Tables["keysetting"].Clear();
			string sc = "SELECT value FROM settings WHERE name ='" + name + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "keysetting");
			}
			catch (Exception ex)
			{
				myConnection.Close();
				ShowExp(sc, ex);
				return "";
			}
			return ds.Tables["keysetting"].Rows[0]["value"].ToString();
		}
		public static bool UpdateKeySettings(string name, string value)
		{
			if (ds.Tables["keysetting"] != null)
				ds.Tables["keysetting"].Clear();
			string sc = "UPDATE settings SET value='" + value + "' WHERE name ='" + name + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(ds, "keysetting");
			}
			catch (Exception ex)
			{
				myConnection.Close();
				ShowExp(sc, ex);
				return false;
			}
			return true;
		}
		public static string EncodeDoubleQuote(string s)
		{
			if (s == null)
				return null;
			string ss = "";
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == '\"')
					ss += '\"'; //double it
				if (s[i] == 8220 || s[i] == 8221) //chinese double quote
				{
					ss += "\"\""; //add double quote
					continue; //skip this
				}

				ss += s[i];
			}
			return ss;
		}
		public static string Encrypt(string s)
		{
			if (s == null || s.Length == 0)
				return string.Empty;
			string result = string.Empty;
			try
			{
				byte[] buffer = Encoding.ASCII.GetBytes(s);
				TripleDESCryptoServiceProvider des =
					new TripleDESCryptoServiceProvider();
				MD5CryptoServiceProvider MD5 =
					new MD5CryptoServiceProvider();
				des.Key =
					MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(keyword));
				des.IV = IV;
				result = Convert.ToBase64String(
					des.CreateEncryptor().TransformFinalBlock(
						buffer, 0, buffer.Length));
			}
			catch
			{
				throw;
			}
			return result;
		}
		public static string GetXmlTagValue(string sBody, string sKey)
		{
			string s = sBody.ToLower();
			string k = "<" + sKey.ToLower() + ">";
			int p = s.IndexOf(k);
			if (p < 0)
				return "";
			k = "</" + sKey.ToLower() + ">";
			p = p + sKey.Length + 2;
			int q = s.IndexOf(k, p);
			if (q <= p)
				return "";
			s = sBody.Substring(p, q - p);
			return s;
		}
		public static bool CheckSqlConfig()
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "sql.ini";
			if (File.Exists(sPath))
				return true;

			m_sServer1 = m_sServer;
			m_sUser1 = m_sUser;
			m_sPass1 = m_sPass;
			m_sDBName1 = m_sCompanyName;

			string s = "";
			s += "server1=" + m_sServer1 + "\r\n";
			s += "name1=" + m_sDBName1 + "\r\n";
			s += "user1=" + m_sUser + "\r\n";
			s += "pass1=" + m_sPass1 + "\r\n";
			s += "server2_as_backup=1\r\n";
			try
			{
				File.AppendAllText(sPath, s);
			}
			catch (Exception e)
			{
			}
			return true;
		}
		public static bool GetSqlConfig()
		{
			CheckSqlConfig();
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "sql.ini";
			if (!File.Exists(sPath))
			{
				return false;
			}
			StreamReader sr = File.OpenText(sPath);
			string s = sr.ReadToEnd() + "\r";
			sr.Close();
			string sBackup = GetValue(s, "server2_as_backup");
			if(!Program.MyBooleanParse(sBackup))
				m_bServer2AsBackup = false;
			m_sServer1 = GetValue(s, "server1");
			m_sDBName1 = GetValue(s, "name1");
			m_sUser1 = GetValue(s, "user1");
			m_sPass1 = GetValue(s, "pass1");
			m_sServer2 = GetValue(s, "server2");
			m_sDBName2 = GetValue(s, "name2");
			m_sUser2 = GetValue(s, "user2");
			m_sPass2 = GetValue(s, "pass2");
			return true;
		}
		public static bool GetActivataStroeCode()
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "gbox.ini";
			if (!File.Exists(sPath))
			{
				return true;
			}
			StreamReader sr = File.OpenText(sPath);
			string s = sr.ReadToEnd() + "\r";
			sr.Close();
			g_sActivataStoreCode = GPOSDecodePwd(GetValue(s, "storecode"));
			return true;
		}
		public static string GetValue(string s, string skey)
		{
			string sl = s.ToLower();
			skey = skey.ToLower() + "=";
			string sRet = "";
			int p = sl.IndexOf(skey);
			if (p >= 0)
			{
				p += skey.Length;
				int q = sl.IndexOf("\r", p);
				if (q <= p)
					return "";
				sRet = s.Substring(p, q - p);
			}
			return sRet;
		}
		public static string GPOSEncodePwd(string s)
		{
			string sOut = "";
			int j = 0;
			for (int i = 0; i < s.Length; i++)
			{
				char c = (char)(s[i] + j);
				j++;
				if (j > 7)
					j = 0;
				int n = c;
				sOut += n.ToString("D3");
			}
			return sOut;
		}
		public static string GPOSDecodePwd(string s)
		{
			if (s.Length % 3 != 0)
				return "";
			string sOut = "";
			int j = 0;
			for (int i = 0; i < s.Length; i += 3)
			{
				string si = s.Substring(i, 3);
				int n = Program.MyIntParse(si) - j;
				if (n <= 0)
					return "";
				sOut += (char)n;
				//				i += 2;	
				j++;
				if (j > 7)
					j = 0;
			}
			return sOut;
		}
		public static bool MyCheckPrinter(string printerToCheck)
		{
			ManagementObjectSearcher searcher = new
				ManagementObjectSearcher("SELECT * FROM   Win32_Printer");

			bool IsReady = false;
			foreach (ManagementObject printer in searcher.Get())
			{
				if (printer["Name"].ToString().ToLower().Equals(printerToCheck))
				{
					if (printer["WorkOffline"].ToString().ToLower().Equals("false"))
					{
						IsReady = true;
					}
				}
			}
			return IsReady;
		}
		public static string BuildReceiptFromInvoice(string invoice_number, bool bExport, ref string pks_return, bool show_points)
		{
			if (ds.Tables["build_receipt"] != null)
				ds.Tables["build_receipt"].Clear();
			int nRows = 0;
			double cash = 0;
			double others = 0;
			double credit = 0;
			double eftpos = 0;
			double cashout = 0;
			double cheque = 0;
			double charge = 0;
            double change = 0;
			double insurance = 0;
			double web = 0;
			double deposit = 0;
			double finance = 0;
			double Total = 0;
			double taxTotal = 0;
			string card_id = "";
            string points = "";
			DataSet dsi = new DataSet();
			string sc = " SELECT i.sales_note, s.*, c.name_cn, c.name AS pname, c.barcode, i.card_id, i.station_id, i.sales ";
            sc += ", isnull(cd.points,0) as points ";
			sc += ", CONVERT(varchar(88), i.commit_date, 120) AS commit_date ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 1 GROUP BY ti.invoice_number, d.payment_method),0) AS cash ";
            /**change**/
//          sc += ", ISNULL((SELECT sum(d.change) as change from tran_detail d join tran_invoice ti ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 1 GROUP BY ti.invoice_number, d.payment_method),0) AS change ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 2 GROUP BY ti.invoice_number, d.payment_method),0) AS cheque ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 9 GROUP BY ti.invoice_number, d.payment_method),0) AS others ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 3 GROUP BY ti.invoice_number, d.payment_method),0) AS credit  ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 6 GROUP BY ti.invoice_number, d.payment_method),0) AS eftpos ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 10 GROUP BY ti.invoice_number, d.payment_method),0) AS cashout ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 11 GROUP BY ti.invoice_number, d.payment_method),0) AS charge ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 13 GROUP BY ti.invoice_number, d.payment_method),0) AS insurance ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 14 GROUP BY ti.invoice_number, d.payment_method),0) AS web ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 15 GROUP BY ti.invoice_number, d.payment_method),0) AS deposit ";
			sc += ", ISNULL((SELECT SUM(ti.amount_applied) AS amount FROM tran_invoice ti JOIN tran_detail d ON d.id = ti.tran_id WHERE ti.invoice_number = s.invoice_number AND d.payment_method = 16 GROUP BY ti.invoice_number, d.payment_method),0) AS finance ";
			sc += " FROM invoice i ";
			sc += " JOIN sales s ON s.invoice_number = i.invoice_number ";
            sc += " LEFT OUTER JOIN card cd on cd.id = i.card_id ";
			sc += " JOIN code_relations c ON c.code = s.code";
			sc += " WHERE s.invoice_number = " + invoice_number;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(ds, "build_receipt");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return "";
			}
			if (nRows <= 0)
			{
				MessageBox.Show("invoice not found");
				return "";
			}
			DataRow dr = ds.Tables["build_receipt"].Rows[0];
			string station_id = dr["station_id"].ToString();

			string s = "";
			string sHeader = ReadSitePage("pos_receipt_header");
			sHeader = sHeader.Replace("@@sales", dr["sales"].ToString());
			sHeader = sHeader.Replace("@@inv_num", invoice_number);
			sHeader = sHeader.Replace("@@date", dr["commit_date"].ToString());
			sHeader = sHeader.Replace("@@time", "");
			sHeader = sHeader.Replace("@@station", dr["station_id"].ToString());
			s += sHeader;
			string pks = "";
			pks += Program.PrintPadding("", "", "=") + "\r\n";
			string note = "";
			for (int d = 0; d < nRows; d++)
			{
				DataRow drs = ds.Tables["build_receipt"].Rows[d];
				if (d == 0)
					note = drs["sales_note"].ToString();
				string pname = drs["pname"].ToString();
				string name = drs["name"].ToString();
				string barcode = drs["barcode"].ToString();
				string qty = drs["quantity"].ToString();
                points = drs["points"].ToString();
				double dQty = Program.MyDoubleParse(qty);
				string tax_rate = drs["tax_rate"].ToString();
				double dPrice = Program.MyDoubleParse(dr["commit_price"].ToString());
				string sales_total = drs["sales_total"].ToString();
				double dAtPrice = dPrice;
				if (dQty != 0)
					dAtPrice = Program.MyDoubleParse(sales_total) / Program.MyDoubleParse(qty);
				double tax = Program.MyDoubleParse(sales_total) - Program.MyDoubleParse(sales_total) / (1 + Program.MyDoubleParse(tax_rate));
				string name_cn = drs["name_cn"].ToString();
				card_id = drs["card_id"].ToString();
				cash = Program.MyDoubleParse(drs["cash"].ToString());
				cheque = Program.MyDoubleParse(drs["cheque"].ToString());
				others = Program.MyDoubleParse(drs["others"].ToString());
				credit = Program.MyDoubleParse(drs["credit"].ToString());
				eftpos = Program.MyDoubleParse(drs["eftpos"].ToString());
				cashout = Program.MyDoubleParse(drs["cashout"].ToString());
				charge = Program.MyDoubleParse(drs["charge"].ToString());
 //             change = Program.MyDoubleParse(drs["change"].ToString());
				insurance = Program.MyDoubleParse(drs["insurance"].ToString());
				web = Program.MyDoubleParse(drs["web"].ToString());
				deposit = Program.MyDoubleParse(drs["deposit"].ToString());
				finance = Program.MyDoubleParse(drs["finance"].ToString());
				Total += Program.MyDoubleParse(sales_total);
				taxTotal += tax;

				SplitAndInput.SplitAndInput en_name_Split = new SplitAndInput.SplitAndInput();
				List<string> new_en_name = en_name_Split.listBoxWordWrapComa(name, Program.m_nPaperWidth);
				if (new_en_name.Count > 0)
				{
					name = "";
					for (int m = 0; m < new_en_name.Count; m++)
					{
						if (m < new_en_name.Count - 1)
							name += new_en_name[m] + "\r\n";
						else if (m == new_en_name.Count - 1)
							name += new_en_name[m];
					}
					//foreach (var line in new_en_name)
					//{
					//    name += line + "\r\n";
					//}
				}

				//				if (!Program.m_bEnableSimpleInvoice)
				{
					if (name != "")
					{
					//	if (name.Length < Program.m_nPaperWidth)
						{
							pks += name + "\r\n";
						}
						//else
						//{
						//	string name1 = name.Substring(0, Program.m_nPaperWidth);
						//	string name2 = name.Substring(Program.m_nPaperWidth);
						//	string name3 = "";
						//	int nSpace = name1.LastIndexOf(' ');
						//	if (nSpace < name1.Length - 1)
						//	{
						//		name1 = name.Substring(0, nSpace);
						//		name2 = name.Substring(nSpace + 1); //remove space
						//	}

						//	if (name2.Length >= Program.m_nPaperWidth)
						//	{
						//		string name_rest = name2;
						//		name2 = name_rest.Substring(0, Program.m_nPaperWidth);
						//		name3 = name_rest.Substring(Program.m_nPaperWidth);
						//		nSpace = name2.LastIndexOf(' ');
						//		if (nSpace < name2.Length - 1)
						//		{
						//			name2 = name_rest.Substring(0, nSpace);
						//			name3 = name_rest.Substring(nSpace + 1); //remove space
						//		}
						//	}
						//	pks += name1 + "\r\n" + name2 + "\r\n" + name3.Replace("(", "\r\n(") + "\r\n";
						//}
					}
					if (name_cn != "")
					{
						pks += name_cn;
						pks += "\r\n";
					}
				}
				//				else
				//				{
				//					if (name != "")
				//						pks += name + "\r\n";
				//				}
				//				if (name_cn != "")
				//					pks += name_cn + "\r\n";
				if (!Program.m_bEnableSimpleInvoice)
					pks += Program.PrintPadding("          " + qty + " @ " + dAtPrice.ToString("c") + "", Program.MyDoubleParse(sales_total).ToString("c"), " ") + "\r\n";
				else
					pks += Program.PrintPadding("          " + qty + " @ " + dAtPrice.ToString("c") + "", Program.MyDoubleParse(sales_total).ToString("c"), " ") + "\r\n";
//				pks += "Item NO. " + barcode + "\r\n";
			}
			if (note != "")
			{
				//				pks += "\r\n" + note + "\r\n\r\n";
				string sNote = note;
				pks += "\r\n";
				if (sNote.Length < Program.m_nPaperWidth)
				{
					pks += sNote + "\r\n";
				}
				else
				{
					string name1 = sNote.Substring(0, Program.m_nPaperWidth);
					string name2 = sNote.Substring(Program.m_nPaperWidth);
					string name3 = "";
					int nSpace = name1.LastIndexOf(' ');
					if (nSpace < name1.Length - 1)
					{
						name1 = sNote.Substring(0, nSpace);
						name2 = sNote.Substring(nSpace + 1); //remove space
					}

					if (name2.Length >= Program.m_nPaperWidth)
					{
						string name_rest = name2;
						name2 = name_rest.Substring(0, Program.m_nPaperWidth);
						name3 = name_rest.Substring(Program.m_nPaperWidth);
						nSpace = name2.LastIndexOf(' ');
						if (nSpace < name2.Length - 1)
						{
							name2 = name_rest.Substring(0, nSpace);
							name3 = name_rest.Substring(nSpace + 1); //remove space
						}
					}
					pks += name1 + "\r\n" + name2 + "\r\n" + name3.Replace("(", "\r\n(") + "\r\n";
				}
			}
			pks += Program.PrintPadding("", "", "=") + "\r\n";
			s += pks;

			s += Program.PrintPadding("TOTAL:", Total.ToString("c"), " ") + "\r\n";

			if (cash != 0 && cash != null)
				s += Program.PrintPadding("CASH:", cash.ToString("c"), " ") + "\r\n";
            //else if (cash != 0 && change != 0)
            //{
            //    s += Program.PrintPadding("CASH IN:", (cash + change).ToString("c"), " ") + "\r\n";
            //    s += Program.PrintPadding("CHANGE:", change.ToString("c"), " ") + "\r\n";
            //}
			if (cheque != 0)
				s += Program.PrintPadding("DIGITAL:", cheque.ToString("c"), " ") + "\r\n";
			if (others != 0)// && cheque != null)
				s += Program.PrintPadding("OTHER PAYMENT:", others.ToString("c"), " ") + "\r\n";
			if (credit != 0)// && credit != null)
				s += Program.PrintPadding("CREDIT CARD:", credit.ToString("c"), " ") + "\r\n";
			if (eftpos != 0)// && eftpos != null)
				s += Program.PrintPadding("EFTPOS:", eftpos.ToString("c"), " ") + "\r\n";
			if (cashout != 0)// && cashout != null)
				s += Program.PrintPadding("CASHOUT:", cashout.ToString("c"), " ") + "\r\n";
			if (charge != 0)// && charge != null)
				s += Program.PrintPadding("CHARGE:", charge.ToString("c"), " ") + "\r\n";
			if (cash == 0 && others == 0 && credit == 0 && eftpos == 0 && cashout == 0 && cheque == 0)
				s += Program.PrintPadding("CHARGE:", Total.ToString("c"), " ") + "\r\n";
			if (insurance != 0)
				s += Program.PrintPadding("INSURANCE BALANCE:", insurance.ToString("c"), " ") + "\r\n";
			if (web != 0)
				s += Program.PrintPadding("WEB ORDER:", web.ToString("c"), " ") + "\r\n";
			if (deposit != 0)
				s += Program.PrintPadding("BANK DEPOSIT:", deposit.ToString("c"), " ") + "\r\n";
			if (finance != 0)
				s += Program.PrintPadding("FINANCE:", finance.ToString("c"), " ") + "\r\n";

			if (Total < 0)
				s = s.Replace("Tax Invoice", "Credit Note");

			s += Program.PrintPadding("INCLUDE TAX:", taxTotal.ToString("c"), " ") + "\r\n";
			if (taxTotal == 0)
				s = s.Replace("Tax Invoice", "Export Invoice");
			if (card_id != "" && card_id != "0")
			{
				string barcodevip = Program.GetVipBarcode(card_id);
				string namevip = Program.GetVipName(card_id);
				string vipinfo = Program.GetVipInfo(barcodevip);
				s += Program.PrintPadding("", "", "-") + "\r\n";
				s += Program.PrintPadding("Membership ID:", barcodevip, " ") + "\r\n";
				s += Program.PrintPadding("Name:", namevip, " ") + "\r\n";
				if(show_points)
					s += Program.PrintPadding("Points:", points, " ") + "\r\n";
				s += vipinfo + "\r\n";
			}
			s += Program.PrintPadding("", "", "-") + "\r\n";
			string sFooter = ReadSitePage("pos_receipt_footer");
			if (bExport)
				sFooter = ReadSitePage("pos_export_footer");
			sFooter = sFooter.Replace("@@member_points_msg", "");
			s += sFooter;
			s += "\r\n=====Customer Duplicate Receipt =======";
			s += " \r\n \r\n \r\n";

			pks += Program.PrintPadding("TOTAL:", Total.ToString("c"), " ") + "\r\n";
			pks += " \r\n \r\n \r\n";
			pks_return = "[b]Packing Slip [/b]\r\n" + pks;
			return s;
		}
		public static string GetVipBarcode(string card_id)
		{
			string barcode = "";
			string sc = "";
			int rows = 0;
			string table_name = "GetVipBarcode";
			if (ds.Tables[table_name] != null)
				ds.Tables[table_name].Clear();
			sc += " SELECT TOP 1 barcode FROM card WHERE 1=1 ";
			sc += " AND id = '" + card_id + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(ds, table_name);
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return "0";
			}
			if (rows == 1)
			{
				DataRow drs = ds.Tables[table_name].Rows[0];
				barcode = drs["barcode"].ToString();
			}
			return barcode;
		}
		public static string GetVipName(string card_id)
		{
			string name = "";
			string sc = "";
			int rows = 0;
			DataSet ds = new DataSet();
			if (ds.Tables["getvipname"] != null)
				ds.Tables["getvipname"].Clear();
			sc += " SELECT TOP 1 name FROM card WHERE 1=1 ";
			sc += " AND id = '" + card_id + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(ds, "getvipname");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return "0";
			}
			if (rows == 1)
			{
				DataRow drs = ds.Tables["getvipname"].Rows[0];
				name = drs["name"].ToString();
			}
			return name;
		}
		public static string GetVipInfo(string sBarcode)
		{
			if (ds.Tables["gvi"] != null)
				ds.Tables["gvi"].Clear();
			string sc = " SELECT * FROM card WHERE barcode = '" + Program.EncodeQuote(sBarcode) + "' ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				if (myCommand.Fill(ds, "gvi") <= 0)
					return "";
			}
			catch (Exception e)
			{
				MessageBox.Show("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return "";
			}
			DataRow dr = ds.Tables["gvi"].Rows[0];
			string barcode = dr["barcode"].ToString();
			string name = dr["name"].ToString();
			string phone = dr["phone"].ToString();
			string email = dr["email"].ToString();
			string address1 = dr["address1"].ToString();
			string address2 = dr["address2"].ToString();
			string address3 = dr["address3"].ToString();
			string address = address1 + " " + address2 + " " + address3;
			string s = "";//To:" + name + "\r\n";
			s += address + "\r\n";
			s += "Phone:" + phone;
			return s;
		}
		public static string GetCatByItemCode(string item_code)
		{
			string cat = "";
			int nRows = 0;
			string table_name = "GetCatByItemCode";
			DataSet ds = new DataSet();
			if (ds.Tables[table_name] != null)
				ds.Tables[table_name].Clear();
			string sc = " SELECT TOP 1 cat FROM code_relations WHERE 1=1 ";
			sc += " AND code = '" + item_code + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(ds, table_name);
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return "";
			}
			if (nRows == 1)
			{
				DataRow drs = ds.Tables[table_name].Rows[0];
				cat = drs["cat"].ToString();
			}
			return cat;
		}
		public static bool IsIndexOfNoVipDiscountCatalog(string cat)
		{
			bool isIndexOfNoVipDiscountCatalog = false;
			string[] catArr = m_sNoVipDiscountCatalog.Split(',');
			for (int i = 0; i < catArr.Length; i++)
			{
				string catName = catArr[i];
				if (catName == cat)
					return true;
			}
			return isIndexOfNoVipDiscountCatalog;
		}
		public static IntPtr MyFindWindow(string sTitle)
		{
			IntPtr hWnd = FindWindow(null, sTitle);
			if (hWnd == null || (long)hWnd == 0)
				return IntPtr.Zero;
			return hWnd;
		}
		public static IntPtr MyFindChildWindow(string sMainTitle, string sClass, string sChildTitle)
		{
			IntPtr hWnd = FindWindow(null, sMainTitle);
			if (hWnd == null || (long)hWnd == 0)
				return IntPtr.Zero;
			IntPtr hChild = FindWindowEx(hWnd, IntPtr.Zero, sClass, sChildTitle);
			return hChild;
		}
	}
	public class RawPrinterHelper
	{
		// Structure and API declarions:
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public class DOCINFOA
		{
			[MarshalAs(UnmanagedType.LPStr)]
			public string pDocName;
			[MarshalAs(UnmanagedType.LPStr)]
			public string pOutputFile;
			[MarshalAs(UnmanagedType.LPStr)]
			public string pDataType;
		}
		[DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

		[DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool ClosePrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

		[DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool EndDocPrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool StartPagePrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool EndPagePrinter(IntPtr hPrinter);

		[DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

		// SendBytesToPrinter()
		// When the function is given a printer name and an unmanaged array
		// of bytes, the function sends those bytes to the print queue.
		// Returns true on success, false on failure.
		public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
		{
			Int32 dwError = 0, dwWritten = 0;
			IntPtr hPrinter = new IntPtr(0);
			DOCINFOA di = new DOCINFOA();
			bool bSuccess = false; // Assume failure unless you specifically succeed.

			di.pDocName = "My C#.NET RAW Document";
			di.pDataType = "RAW";

			// Open the printer.
			if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
			{
				// Start a document.
				if (StartDocPrinter(hPrinter, 1, di))
				{
					// Start a page.
					if (StartPagePrinter(hPrinter))
					{
						// Write your bytes.
						bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
						EndPagePrinter(hPrinter);
					}
					EndDocPrinter(hPrinter);
				}
				ClosePrinter(hPrinter);
			}
			// If you did not succeed, GetLastError may give more information
			// about why not.
			if (bSuccess == false)
			{
				dwError = Marshal.GetLastWin32Error();
			}
			return bSuccess;
		}
		public static bool SendStringToPrinter(string szString)
		{
			return SendStringToPrinter(Program.m_sPrinterName, szString);
		}
		public static bool SendStringToPrinter(string szPrinterName, string szString)
		{
			IntPtr pBytes;
			Int32 dwCount;
			// How many characters are in the string?
			dwCount = szString.Length;
			// Assume that the printer is expecting ANSI text, and then convert
			// the string to ANSI text.
			pBytes = Marshal.StringToCoTaskMemAnsi(szString);
			// Send the converted ANSI string to the printer.
			SendBytesToPrinter(szPrinterName, pBytes, dwCount);
			Marshal.FreeCoTaskMem(pBytes);
			return true;
		}
	}
}