using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Web;
using System.Web.UI.HtmlControls;
using Sektor.Vault.Common;
using Sektor.Vault.POSInterface;
using PCEFTIPInterface;
using System.Text.RegularExpressions;

namespace QPOS2008
{
	public partial class btncompanyok : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);
		
		public const int WM_LBUTTONDOWN = 0x0201; 
		public const int WM_LBUTTONUP = 0x0202;
		
		private SqlDataAdapter myAdapter;
		private SqlConnection myConnection;
		private SqlCommand myCommand;
		DataSet dst = new DataSet();

		private PrintDocument printDoc = new PrintDocument();
		private PrintDocument printDocST = new PrintDocument();

		//vip account print
		private PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
		private PrintDocument printDocVA = new PrintDocument();
		private string documentContents;// Declare a string to hold the entire document contents. 
		private string stringToPrint;// Declare a variable to hold the portion of the document that is not printed. 		

		public string m_sStaff = "";

		private string m_sCode = "";
		private string m_sItemName = "";
		private double m_dTaxRate = 0;
		private string m_sSales = "";
		private bool m_bShowSpecial = false;
		private int m_iSelectedMainItem =-1;
		private string m_card_id = "";
		private string m_sVip_id = "";
		private string m_sItemCode = "";
		private int m_iDateSelect = 1;
		private int exportReportType = 0;
		private string selectedTabName = "";
		private int payment_report_case = 1;
		private string m_sPrintBuffer = "";
		private int print_trans = 0;
		private double print_cash = 0;
		private double print_eftpos = 0;
		private double print_cc = 0;
		private double print_account = 0;
		private double print_account_paid = 0;
		private double print_cheque = 0;
		private double print_cashout = 0;
		private double print_rounding = 0;
		private double print_totalitemsales = 0;

		private string m_sPromoid = "";
		private string m_sSubButtonid = "";
		private string m_sTaxId = "";

//		private bool m_bJustSelectedItem = false; //set if selected item by click in the item list view
		private string[] m_nameEn = new string[512];
		private string[] m_nameCn = new string[512];
//        private string m_CatNameEn = "";
		private string m_sButton_id = "";
		OpenFileDialog dlg = new OpenFileDialog();
		Image b = new Bitmap(95, 125);

		/**************************/
		private DataGridView dataGridView;
		//		private PrintDocument printDocument;
		private PrintDocument printDocument = new PrintDocument();
//		private PageSetupDialog pageSetupDialog;
//		private PrintPreviewDialog printPreviewDialog;

		private int dgvIndex = 0;

		private int rowCount = 0;
		private int colCount = 0;
		private int x = 0;
		private int y = 0;
		int i = 0;

		private int rowGap = 60;
		private int leftMargin = 50;
		private Font font = new Font("Arial", 10);
		private Font headingFont = new Font("Arial", 11, FontStyle.Underline);
		private Font captionFont = new Font("Arial", 10, FontStyle.Bold);
		private Brush brush = new SolidBrush(Color.Black);
		private string cellValue = string.Empty;
		
		private static int page = 1;
		private int size = 100;
		private int totalitem = 0;
		private string sql="";

		private static int pageB = 1;
		private int sizeB = 30;
		private int totalitemB = 0;
//		private string sqlB = "";

		private static int pageS = 1;
		private int sizeS = 100;
		private int totalitemS = 0;
		private string sqlS = "";

		private static int pageP = 1;
		private int sizeP = 20;
		private int totalitemP = 0;

		public string m_sVipCardId = "";
		public bool m_bPay = false;
		public double m_dPayAmount = 0;
		public string m_sInvoices = "";
		public string m_sAmounts = "";
		
		private string m_sBranchId = "";
		int m_nTotalItems = 0;

		private bool bWebService = false;
		private bool bWeChat = false;
		private bool bNVR = false;
		private bool bEftpos = false;
		private bool bScale = false;
		/**************************/

		Control m_hLastFocusedControl;
		public btncompanyok()
		{
			InitializeComponent();
			foreach(Control pnl in Controls)
			{
				if (pnl is Panel || pnl is GroupBox || pnl is TabControl)
				{
					foreach (Control ctrl in pnl.Controls)
					{
						if (ctrl is TextBox || ctrl is RichTextBox)
						{
							ctrl.MouseUp += delegate(object sender, MouseEventArgs e)
							{
								m_hLastFocusedControl = (Control)sender;
							};
						}
						else if (ctrl is Panel || ctrl is GroupBox || ctrl is TabPage)
						{
							foreach (Control cctr in ctrl.Controls)
							{
								if (cctr is TextBox || cctr is RichTextBox)
								{
									cctr.MouseUp += delegate(object sender, MouseEventArgs e)
									{
										m_hLastFocusedControl = (Control)sender;
									};
								}
								else if (cctr is Panel || cctr is GroupBox)
								{
									foreach (Control ccct in cctr.Controls)
									{
										if(ccct is TextBox || ccct is RichTextBox)
										{
											ccct.MouseUp += delegate(object sender, MouseEventArgs e)
											{
												m_hLastFocusedControl = (Control)sender;
											};
										}
									}
								}
							}
						}
					}
				}
			}
			if (Program.m_sPrinterName != "")
			{
				printDoc.PrinterSettings.PrinterName = Program.m_sPrinterName;
			}
			printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
			printDocST.PrintPage += new PrintPageEventHandler(printDoc_PrintPageST);
			printDocVA.PrintPage += new PrintPageEventHandler(printDocVA_PrintPage);
		}
		private void Admin_Panel_Load(object sender, EventArgs e)
		{
			btnspeciallist.Visible = true;
			if (!Program.m_bAccessAdmin)
			{
				if(!Program.m_bAccessProduct)
				{
					gpProduct.Enabled = false;
					btnspeciallist.Visible = false;
				}
				if (!Program.m_bAccessStock)
					gpStock.Enabled = false;
				if (!Program.m_bAccessReport)
					gpReport.Enabled = false;
                if (!Program.m_bAccessSetting)
                    //gpSetting.Enabled = false;
                    btnCompany.Enabled = false;
				if (!Program.m_bAccessDatabase)
					gpDatabase.Enabled = false;
			}

			this.gpSpecial.Location = new Point(12, 140);
			this.gpDiscount.Location = new Point(12, 140);
			this.groupBox21.Location = new Point(12, 140);
			this.groupBox15.Location = new Point(12, 140);
			this.groupBox19.Location = new Point(12, 140);
			this.groupBox20.Location = new Point(12, 140);

			this.PrinterName.Text = Program.m_sPrinterName;
			this.PaperWidth.Text = Program.m_nPaperWidth.ToString();
			this.fontName.Text = Program.m_sFontName;
			this.fontSize.Text = Program.m_sFontSize;
			this.fontStyle.Text = Program.m_sFontStyle;
			//this.ScaleName.Text = Program.m_sScaleName;
			//this.ScannerName.Text = Program.m_sScannerName;
			this.textStationID.Text = Program.m_sStationID;
			this.textTimeout.Text = Program.m_nTimeout.ToString();
			this.chkCustomerDis.Checked = bool.Parse(Program.m_sDMoniter.ToString());
			this.ckbEpson.Checked = bool.Parse(Program.m_sEpson.ToString());

			this.txtBranch_id.Text = Program.m_sBranchID_sync;

			this.txtgroceryitem.Text = Program.m_sgroceryitem;
			this.txtgroceryweight.Text = Program.m_sgroceryweight;
			this.txtvoidscale.Text = Program.m_svoidscale;
			this.txtgroceryitemaddup.Text = Program.m_sgroceryitemaddup;
			this.txtgroceryweightitemaddup.Text = Program.m_sgroceryweightitemaddup;

			this.txtvoiddisc.Text = Program.m_svoiddis;
			this.ckbSpecialDis.Checked = bool.Parse(Program.m_bEnableSpecialDis.ToString());
			this.ckbPic.Checked = bool.Parse(Program.m_bEnablePic.ToString());
			this.txtPic.Text = Program.m_sPicroot;
			this.txtTare.Text = Program.m_sTare;
			this.txtAA.Text = Program.m_sAA;
			this.chkCustomerDis.Checked = bool.Parse(Program.m_sDMoniter.ToString());
			this.ckbEpson.Checked = bool.Parse(Program.m_sEpson.ToString());
			this.rdbmonitorr.Checked = bool.Parse(Program.m_bRegularMonitor.ToString());
			this.rdbmonitors.Checked = bool.Parse(Program.m_bSmallMonitor.ToString());
			this.textBoxAdUrl.Text = Program.m_sAdUrl;
			this.cbSerialDevicePort.Text = Program.m_sSerialDevicePort;
			this.chkEnableAttractpay.Checked = bool.Parse(Program.m_bEnableAttractPay.ToString());
			this.chkEnableAlipayDirect.Checked = bool.Parse(Program.m_bEnableAlipayDirect.ToString());
			this.chkEnableMyPosMate.Checked = bool.Parse(Program.m_bEnableMyPosMate.ToString());
			//this.txtMyPosMateMerchantId.Text = Program.m_sMyPosMateMerchantId;
			//this.txtMyPosMateConfigId.Text = Program.m_sMyPosMateConfigId;
			//this.ckbScale.Checked = bool.Parse(Program.m_senablescale.ToString());

			//this.chkEnableVipDis.Checked = bool.Parse(Program.m_bEnableVipDis.ToString());
			this.rbDis.Checked = bool.Parse(Program.m_bEnableVipDis.ToString());
			this.ckbAutoTare.Checked = bool.Parse(Program.m_bEnableAutoTare.ToString());
			this.ckbFloating.Checked = bool.Parse(Program.m_bEnableFloating.ToString());
			this.ckbAutoSync.Checked = bool.Parse(Program.m_bEnableAutoSync.ToString());
			this.ckbScanCode.Checked = Program.m_bScanInCode;

			this.cbEnableLatipayGiftCard.Checked = Program.m_bEnableLatiPayGiftCard;
			this.cbEnableLatipay.Checked = Program.m_bEnableLatiPay;
			this.txtLatipayBackgroundUrl.Text = Program.m_sLatiPayQRCodeBackground;

			this.ckbScanSupplierCode.Checked = Program.m_bScanInSupplierCode;
			this.ckbSalesLogin.Checked = bool.Parse(Program.m_bEnableSalesLogin.ToString());
			this.ckbUpdatePrice.Checked = bool.Parse(Program.m_bEnableUpdatePrice.ToString());
			this.ckbCalQty.Checked = bool.Parse(Program.m_bEnableCalQty.ToString());
			this.ckbMargin.Checked = bool.Parse(Program.m_bEnableMargin.ToString());
			this.cbIdCheckPasswordControl.Checked = Program.m_bIdCheckPasswordControl;
			
			//this.ckbLevelPrice.Checked = bool.Parse(Program.m_bEnableLevelPrice.ToString());
			this.rbLevel.Checked = bool.Parse(Program.m_bEnableLevelPrice.ToString());
			this.ckbSimpleInv.Checked = bool.Parse(Program.m_bEnableSimpleInvoice.ToString());
			this.ckbAA.Checked = bool.Parse(Program.m_bEnableAA.ToString());

			this.ckbDiscount.Checked = bool.Parse(Program.m_bEnableDisControl.ToString());
			this.txtgroceryitem1.Text = Program.m_sgroceryitem;
	
			this.ckbPic.Checked = bool.Parse(Program.m_bEnablePic.ToString());
			this.txtPic.Text = Program.m_sPicroot;
			this.ckbEnableBK.Checked = Program.m_backupdata;
			this.cbCallOfOrderNumber.Checked = Program.m_bCallOfOrderNumber;
			this.cbAutoClosePopupMenu .Checked = Program.m_bAutoClosePopupMenu;
			this.ckbsync.Checked = Program.m_sync;

			//Second Printer Setting
			this.EnableSecondPrinter.Checked = Program.m_bEnableSecondPrinter;
			this.SecondPrinterCatalog.Text = Program.m_sSecondPrinterCatalog;
			this.SecondPrinterName.Text = Program.m_sSecondPrinterName;
			this.SecondPrinterReceiptQTY.Text = Program.m_sSecondPrinterReceiptQTY.ToString();
			this.SecondPrinterPaperWidth.Text = Program.m_nSecondPrinterPaperWidth.ToString();
			this.SecondPrinterEpson.Checked = Program.m_sSecondPrinterEpson;
			this.SecondPrinterSize.Text = Program.m_sSecondPrinterFontSize.ToString();
			this.SecondPrinterFontName.Text = Program.m_sSecondPrinterFontName;
			this.SecondPrinterStyle.Text = Program.m_sSecondPrinterFontStyle;
			this.SecondPrinterTimeout.Text = Program.m_nSecondPrinterTimeout.ToString();

			this.cbEnableSelfService.Checked = Program.m_bEnableSelfService;
			this.cbEnableQtyPassword.Checked = Program.m_bEnableQtyPassword; 

			RegistryKey subKey = Registry.CurrentUser.OpenSubKey(@"" + Program.m_sRegKey + "");
			string[] keyValueNames = subKey.GetValueNames();
			subKey.Close();
			//bool bWebService = false;
			//bool bWeChat = false;
			//bool bNVR = false;
			//bool bEftpos = false;
/*********webservice******************/
			foreach (string keyValueName in keyValueNames)
			{
				if (keyValueName == "enable_web_service")
				{
					bWebService = true;
					break;
				}
				else
				{
					bWebService = false;
				}
			}
			if (!bWebService)
			{
				this.cbEnableWebService.Visible = false;
				this.ckbSyncWithCloud.Visible = false;
				this.txtWebServiceURL.Visible = false;
				this.label143.Visible = false;
			}
			else
			{
				this.cbEnableWebService.Visible = true;
				this.ckbSyncWithCloud.Visible = true;
				this.txtWebServiceURL.Visible = true;
				this.label143.Visible = true;
				this.cbEnableWebService.Checked = Program.m_bEnableWebService;
				this.ckbSyncWithCloud.Checked = Program.m_bSyncWithCloud;
				this.txtWebServiceURL.Text = Program.m_sWebServiceUrl;
			}
/*************wechat***********************/
			foreach (string keyValueName in keyValueNames)
			{
				if (keyValueName == "wechat_payment")
				{
					bWeChat = true;
					break;
				}
				else
				{
					bWeChat = false;
				}
			}
			if (!bWeChat)
			{
				this.cbWechatPayment.Visible = false;
				this.label154.Visible = false;
				this.label155.Visible = false;
				this.label156.Visible = false;
				this.txtWechatUri.Visible = false;
				this.txtWechatMerchantID.Visible = false;
				this.txtWechatSignature.Visible = false;
			}
			else
			{
				this.cbWechatPayment.Visible = true;
				this.label154.Visible = true;
				this.label155.Visible = true;
				this.label156.Visible = true;
				this.txtWechatUri.Visible = true;
				this.txtWechatMerchantID.Visible = true;
				this.txtWechatSignature.Visible = true;
				this.cbWechatPayment.Checked = bool.Parse(Program.m_bEnableWechatPayment.ToString());
				this.txtWechatUri.Text = Program.m_sWeChatUri;
				this.txtWechatMerchantID.Text = Program.m_sWeChatMerchantID;
				this.txtWechatSignature.Text = Program.m_sWeChatSignature;
			}
/*****************************************************************/
			this.Server.Text = Program.m_sServer;
			this.User.Text = Program.m_sUser;
			this.Password.Text = Program.m_sPass;
			this.Catalog.Text = Program.m_sCompanyName;
			this.txtiis.Text = Program.m_sIISServer;

			/**********NVR*************/
			foreach (string keyValueName in keyValueNames)
			{
				if (keyValueName == "nvr_enabled")
				{
					bNVR = true;
					break;
				}
				else
				{
					bNVR = false;
				}
			}
			if (!bNVR)
			{
				this.groupBox1.Visible = false;
			}
			else
			{
				this.groupBox1.Visible = true;
				this.cbEnableNVR.Checked = Program.m_bEnableNVR;
				this.txtNVRIP.Text = Program.m_sNVRIP;
				this.txtNVRPort.Text = Program.m_sNVRPort;
			}
			/************Scale&Scanner***********/
			foreach (string keyValueName in keyValueNames)
			{
				if (keyValueName == "enablescale")
				{
					bScale = true;
					break;
				}
				else
				{
					bScale = false;
				}
			}
			if (!bScale)
			{
				this.groupBox17.Visible = false;
			}
			else
			{
				this.groupBox17.Visible = true;
				this.ckbScale.Checked = bool.Parse(Program.m_senablescale.ToString());
				this.ScaleName.Text = Program.m_sScaleName;
				this.ScannerName.Text = Program.m_sScannerName;
			}
			
			ckbCasScale.Checked = Program.m_bCasScale;
			cbCasScalePort.Text = Program.m_sCasScalePort;
			
			/************Eftpos****************/
			foreach (string keyValueName in keyValueNames)
			{
				if (keyValueName == "enableeftpos")
				{
					bEftpos = true;
					break;
				}
				else
				{
					bEftpos = false;
				}
			}
			if (!bEftpos)
			{
				this.groupBox18.Visible = false;
			}
			else
			{
				this.groupBox18.Visible = true;

				this.chkEnableEftpos.Checked = bool.Parse(Program.m_bEnableEftpos.ToString());
				this.eftposCom.Text = Program.m_sEftposCom;
				this.cbEftposType.Text = Program.m_eftposType;
				this.txtSmartpayIP.Text = Program.m_sSmartpayIP;
				this.txtSmartpayPort.Text = Program.m_sSmartpayPort;
			}

			this.Server.Focus();

			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			string sc = " SELECT top 1* FROM settings where name like 'total_no_of_receipt_printout' ";
			int rows = 0;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "receiptQty");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DataRow dr = dst.Tables["receiptQty"].Rows[0];
			this.tbReceiptQTY.Text = dr["value"].ToString();

			sc = " SELECT top 1* FROM settings where name like 'QPOS_MAX_DISCOUNT_PERCENTAGE' ";
			rows = 0;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "maxDis");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DataRow dr1 = dst.Tables["maxDis"].Rows[0];
			this.txtMaxDis.Text = dr1["value"].ToString();

			GetVoucherSetting();
			GetPriceBarcodeSetting();
			/********************************************************************/

			DoBuildDepart();
			DoBuildCat();

			adminaddp_Click(null, null);
//			ProductList();
			TaxcodeList();
			
			int nTop = 7;
			int nLeft = 165;
			int nWidth = 840;
			int nHeight = 700;

			gpSpecial.Size = new Size(200, 75);
			this.gpSpecial.Location = new Point(12, 140);

			GBCombo.Location = new Point(12, 110);
			GBCombo.Size = new Size(590, 130);
			
			pnlcats.Size = new Size(nWidth, nHeight);
			pnlcats.Location = new Point(nLeft, nTop);

			pnlcattxt.Size = new Size(397, 120);
			pnlcattxt.Location = new Point(221, 350);

			pnlTax.Size = new Size(620, 580);
			pnlTax.Location = new Point(nLeft, nTop);

			tccompany.Size = new Size(nWidth, 678);
			tccompany.Location = new Point(nLeft, nTop);

			panelreports.Size = new Size(nWidth, nHeight);
			panelreports.Location = new Point(nLeft, nTop);
			
			gbTouchScreenLayout.Size = new Size(nWidth, nHeight);
			gbTouchScreenLayout.Location = new Point(nLeft, nTop);

			lvrpayment.Size = new Size(823, 440);
			lvrpayment.Location = new Point(4, 6);

			pnlButton.Size = new Size(nWidth, nHeight);
			pnlButton.Location = new Point(nLeft, nTop);

			pnlsystemsetting.Size = new Size(nWidth, nHeight);
			pnlsystemsetting.Location = new Point(nLeft, nTop);

			btnspeciallist.Size = new Size(nWidth, nHeight);
			btnspeciallist.Location = new Point(nLeft, nTop);

			pnlcleanup.Location = new Point(300,250);
			pnlcleanup.Size = new Size(321, 49);

			groupboxPromotion.Location = new Point(12,245);
			groupboxPromotion.Size = new Size(800,165);

			groupBox33.Location = new Point(12,410);
			groupBox33.Size = new Size(800, 275);

			groupBox20.Location = new Point(320, 113);
			groupBox20.Size = new Size(470,130);

			lvPromotion.Location = new Point(12, 50);
			lvPromotion.Size = new Size(760, 105);

			//label119.Location = new Point(50, 165);
			//comboBox3.Location = new Point(100, 160);
			//comboBox2.Location = new Point(210, 160);
			//comboBox1.Location = new Point(320, 160);
			//label97.Location = new Point(490, 165);
			//txtS.Location = new Point(550, 160);
			//button4.Location = new Point(680, 160);

			lvItem.Location = new Point(12, 48);
			lvItem.Size = new Size(760, 180);
			lvItem.Columns[0].DisplayIndex = 8;
//			lvItem.Columns[0].DisplayIndex = lvItem.Columns.Count - 1;

			btnFirstP.Location = new Point(30, 235);
			btnPreP.Location = new Point(160, 235);
			btnNextP.Location = new Point(290, 235);
			btnLastP.Location = new Point(420, 235);
			btnClearPromo.Location = new Point(600, 235);
			btnAssign.Location = new Point(700, 235);

			lbloldbarcode.Visible = false;

			pnlPromotion.Size = new Size(nWidth, nHeight);
			pnlPromotion.Location = new Point(nLeft, nTop);
			pnlStockTake.Size = new Size(nWidth, nHeight);
			pnlStockTake.Location = new Point(nLeft, nTop);
			pnlStockInput.Size = new Size(nWidth, nHeight);
			pnlStockInput.Location = new Point(nLeft, nTop);
			pnlVipAccount.Size = new Size(nWidth, nHeight);
			pnlVipAccount.Location = new Point(nLeft, nTop);

			this.btnFirst.Enabled = false;
			this.btnPre.Enabled = false;

			HideAllPanel();
			pnlproedit.Visible = true;
//			btnspeciallist.Visible = true;
			if (!vipPonitsFunctionSupport())
			{
				this.rbvip.Checked = false;
				this.rbvip.Enabled = false;
				this.txtvipvouchervalue.Enabled = false;
				this.txtVipStartPoints.Enabled = false;
			}
/*			if (!Program.m_bAccessAdmin && !Program.m_bAccessProduct)
			{
				this.btnspeciallist.Visible = false;
			}
			else
			{
				HideAllPanel();
				btnspeciallist.Visible = true;
			}
*/		}
		private void GetVoucherSetting()
		{
/*			if (dst.Tables["vouchersetting"] != null)
				dst.Tables["vouchersetting"].Clear();
			string sc = "";
			sc += " SELECT * from settings where name = 'voucher_expire_day' ";
			sc += " or name = 'voucher_rate'";
			sc += " or name = 'voucher_start_value'";
			sc += " or name = 'voucher_enabled'";
			sc += " or name = 'vip_voucher_enabled'";
			sc += " or name = 'vip_voucher_start_points' ";
			sc += " or name = 'no_voucher'";
			sc += " or name = 'vip_voucher_value'";
			sc += " ORDER BY name ";

			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "vouchersetting") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			string bvipvoucher = dst.Tables["vouchersetting"].Rows[1]["value"].ToString();
			this.rbvip.Checked = Program.MyBooleanParse(bvipvoucher);  
			string vipstartpoints = dst.Tables["vouchersetting"].Rows[2]["value"].ToString();
			this.txtVipStartPoints.Text = vipstartpoints;
			this.txtvipvouchervalue.Text = dst.Tables["vouchersetting"].Rows[3]["value"].ToString();

			string bvoucher = dst.Tables["vouchersetting"].Rows[4]["value"].ToString();
			//this.cbVoucher.Checked = Program.MyBooleanParse(bvoucher);
			this.rbVoucher.Checked = Program.MyBooleanParse(bvoucher);

			this.txtVoucherRate.Text = dst.Tables["vouchersetting"].Rows[6]["value"].ToString();
			this.txtVoucherStartValue.Text = dst.Tables["vouchersetting"].Rows[7]["value"].ToString();
			this.txtVoucherexpireday.Text = dst.Tables["vouchersetting"].Rows[5]["value"].ToString();

			string bnovoucher = dst.Tables["vouchersetting"].Rows[0]["value"].ToString();
			this.rbnovoucher.Checked = Program.MyBooleanParse(bnovoucher);  
 */
			bool bEnabled = Program.MyBooleanParse(Program.GetSiteSettings("voucher_enabled")); 
			rbvip.Checked = Program.MyBooleanParse(Program.GetSiteSettings("vip_voucher_enabled"));
			rbWebVoucher.Checked = Program.m_bVoucherUseWebService;
			txtVipStartPoints.Text = Program.GetSiteSettings("vip_voucher_start_points");
			txtvipvouchervalue.Text = Program.GetSiteSettings("vip_voucher_value");
			txtVoucherRate.Text = Program.GetSiteSettings("voucher_rate");
			txtVoucherStartValue.Text = Program.GetSiteSettings("voucher_start_value");
			txtVoucherexpireday.Text = Program.GetSiteSettings("voucher_expire_day");
			rbnovoucher.Checked = Program.MyBooleanParse(Program.GetSiteSettings("no_voucher"));
			rbVoucher.Checked = Program.MyBooleanParse(Program.GetSiteSettings("voucher_enabled"));
		}
		private void GetPriceBarcodeSetting()
		{ 
			if (dst.Tables["pricebarcodesetting"] != null)
				dst.Tables["pricebarcodesetting"].Clear();
			string sc = "";
			sc += " SELECT * from settings where name = 'BarcodeLabel_BarcodeLength' ";
			sc += " or name = 'BarcodeLabel_BarcodeStartDigit'";
			sc += " or name = 'BarcodeLabel_DecimalPoint'";
			sc += " or name = 'BarcodeLabel_PriceLength'";
			sc += " or name = 'BarcodeLabel_PriceStartDigit'";
			sc += " ORDER BY name ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "pricebarcodesetting") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			this.txtbarcodelength.Text = dst.Tables["pricebarcodesetting"].Rows[0]["value"].ToString();
			this.txtbarcodestart.Text = dst.Tables["pricebarcodesetting"].Rows[1]["value"].ToString();
			this.txtdecimal.Text = dst.Tables["pricebarcodesetting"].Rows[2]["value"].ToString();
			this.txtpricelength.Text = dst.Tables["pricebarcodesetting"].Rows[3]["value"].ToString();
			this.txtpricestart.Text = dst.Tables["pricebarcodesetting"].Rows[4]["value"].ToString();
		}
		private void btnFirst_Click(object sender, EventArgs e)
		{
			this.btnNext.Enabled = true;
			this.btnLast.Enabled = true;
		
			this.btnFirst.Enabled =false;
			this.btnPre.Enabled = false;
			page = 1;
			ProductList();
		}
		private void btnPre_Click(object sender, EventArgs e)
		{
			string cat = cbCatFilter.Text;
			this.btnNext.Enabled = true;
			this.btnLast.Enabled = true;
//			if(page <= 1)
//			{
//				this.btnPre.Enabled = false;
//				this.btnFirst.Enabled = false;
//			}
			page = page - 1;

			if (totalitem % 100 == 0)
			{
				if (page < 1)
				{
					this.btnFirst.Enabled = false;
					this.btnPre.Enabled = false;
				}
			}
			else
			{
				if (page < 2)
				{
					this.btnFirst.Enabled = false;
					this.btnPre.Enabled = false;
				}

			}
			this.lvitemlist.Items.Clear();
			this.lvitemlist.Scrollable = true;
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtsearch.Text.ToLower();
			string sc = " SELECT ";
			if (cat == "" && kw == "" && m_nTotalItems > 1000)
				sc += " TOP 100 ";
			sc += " * FROM(";
//			sc += " SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand, b.barcode, sq.qty AS stock ";
			//sc += " SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand, c.barcode, sq.qty AS stock ";
			sc += " SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand ";
			//sc += ", c.barcode ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", sq.qty AS stock ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			//			sc += " LEFT OUTER JOIN code_branch cb ON cb.code = c.code";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE 1 = 1 ";

			if (cbcat.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + cbcat.Text.ToLower() + "'";
			if (cb2cat.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + cb2cat.Text.ToLower() + "'";
			if (cb3cat.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + cb3cat.Text.ToLower() + "'";
			if (cbdepart.Text.ToLower() != "")
				sc += " AND LOWER(c.brand) = N'" + cbdepart.Text.ToLower() + "'";

			if (lblshowall.Text == "0")
			{
				try
				{
					int.Parse(kw);
					sc += " AND (c.code ='" + kw + "' OR b.barcode = '" + kw + "')";
				}
				catch (Exception ex)
				{
					string sex = ex.ToString();
					sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
					sc += " OR LOWER(b.barcode) LIKE '%" + kw + "%')";
					sc += " OR LOWER(c.barcode) LIKE '%" + kw + "%'";
				}
			}
			if (m_bShowSpecial)
				sc += " AND c.is_special = 1 ";
			//			sc += " AND c.code = 1003 or c.code=1004 or c.code=1005 or c.code = 1006 or c.code = 1007 or c.code = 1008 ";
			sc += " And c.code > 1009 ";
            sc += " or c.code < 1001 ";
            sc += " AND c.code > 0 ";
			sc += " ORDER BY c.code  DESC";
			sc += " ) a ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			write_listview(dst.Tables["list"]);
			cbPromotion.Items.Clear();
			cbPromotion.Text = "";
		}
		private void btnNext_Click(object sender, EventArgs e)
		{
			string cat = cbCatFilter.Text;
			this.btnFirst.Enabled = true;
			this.btnPre.Enabled = true;
			
			if (totalitem % 100 == 0)
			{
				if (page > totalitem / 100 - 2)
				{
					this.btnNext.Enabled = false;
					this.btnLast.Enabled = false;
				}
			}
			else
			{
				if (page > totalitem / 100 - 1)
				{
					this.btnNext.Enabled = false;
					this.btnLast.Enabled = false;
				}

			}
			page = page + 1;
			
			this.lvitemlist.Items.Clear();
			this.lvitemlist.Scrollable = true;
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtsearch.Text.ToLower();
			string sc = " SELECT ";
			if (cat == "" && kw == "" && m_nTotalItems > 100)
				sc += " TOP 100 ";
//			sc += " * FROM( SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand, c.barcode, sq.qty AS stock ";

			sc += " * FROM( SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand ";
			//sc += ", c.barcode ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", sq.qty AS stock ";

			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			//			sc += " LEFT OUTER JOIN code_branch cb ON cb.code = c.code";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE 1 = 1 ";

			if (cbcat.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + cbcat.Text.ToLower() + "'";
			if (cb2cat.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + cb2cat.Text.ToLower() + "'";
			if (cb3cat.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + cb3cat.Text.ToLower() + "'";
			if (cbdepart.Text.ToLower() != "")
				sc += " AND LOWER(c.brand) = N'" + cbdepart.Text.ToLower() + "'";

			if (lblshowall.Text == "0")
			{
				try
				{
					int.Parse(kw);
					sc += " AND (c.code ='" + kw + "' OR b.barcode = '" + kw + "')";
				}
				catch (Exception ex)
				{
					string sex = ex.ToString();
					sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
					sc += " OR LOWER(b.barcode) LIKE '%" + kw + "%')";
					sc += " OR LOWER(c.barcode) LIKE '%" + kw + "%'";
				}
			}
			if (m_bShowSpecial)
				sc += " AND c.is_special = 1 ";
			//			sc += " AND c.code = 1003 or c.code=1004 or c.code=1005 or c.code = 1006 or c.code = 1007 or c.code = 1008 ";
			sc += " And c.code > 1009 ";
            sc += " or c.code < 1001 ";
            sc += " AND c.code > 0 ";
			sc += " ORDER BY c.code DESC";
			sc += " ) a ORDER BY code  "; 
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");

				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			write_listview(dst.Tables["list"]);
			cbPromotion.Items.Clear();
			cbPromotion.Text = "";
		}
		private void btnLast_Click(object sender, EventArgs e)
		{
			string cat = cbCatFilter.Text;
			this.btnNext.Enabled = false;
			this.btnLast.Enabled = false;
			this.btnFirst.Enabled = true;
			this.btnPre.Enabled = true;

			if (totalitem % 100 == 0)
			{
				page =totalitem / 100;
			}
			else
			{

				page = totalitem / 100 +1;
			}

			this.lvitemlist.Items.Clear();
			this.lvitemlist.Scrollable = true;
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtsearch.Text.ToLower();
			string sc = " SELECT ";
			if (cat == "" && kw == "" && m_nTotalItems > 100)
				sc += " TOP 100 ";
			sc += " * FROM( ";
//			sc += " SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand, c.barcode, sq.qty AS stock ";
			sc += " SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand ";
			//sc += ", c.barcode ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", sq.qty AS stock ";
			
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			//			sc += " LEFT OUTER JOIN code_branch cb ON cb.code = c.code";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE 1 = 1 ";

			if (cbcat.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + cbcat.Text.ToLower() + "'";
			if (cb2cat.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + cb2cat.Text.ToLower() + "'";
			if (cb3cat.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + cb3cat.Text.ToLower() + "'";
			if (cbdepart.Text.ToLower() != "")
				sc += " AND LOWER(c.brand) = N'" + cbdepart.Text.ToLower() + "'";

			if (lblshowall.Text == "0")
			{
				try
				{
					int.Parse(kw);
					sc += " AND (c.code ='" + kw + "' OR b.barcode = '" + kw + "')";
				}
				catch (Exception ex)
				{
					string sex = ex.ToString();
					sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
					sc += " OR LOWER(b.barcode) LIKE '%" + kw + "%')";
					sc += " OR LOWER(c.barcode) LIKE '%" + kw + "%'";
				}
			}
			if (m_bShowSpecial)
				sc += " AND c.is_special = 1 ";
			//			sc += " AND c.code = 1003 or c.code=1004 or c.code=1005 or c.code = 1006 or c.code = 1007 or c.code = 1008 ";
			sc += " And c.code > 1009 ";
            sc += " or c.code < 1001 ";
            sc += " AND c.code > 0 ";
			sc += " ORDER BY c.code DESC";
			sc += " ) a ORDER BY code  ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");

				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			write_listview(dst.Tables["list"]);
			cbPromotion.Items.Clear();
			cbPromotion.Text = "";
		}
		private void btnGo_Click(object sender, EventArgs e)
		{
			string cat = cbCatFilter.Text;
			int maxpage = 0;
			if (totalitem % 100 == 0)
			{
				maxpage = totalitem / 100;
			}
			else
			{

				maxpage = totalitem / 100 + 1;
			}
			if(int.Parse(this.txtpage.Text) > maxpage)
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Sorry, over page number! ";
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.ShowDialog();
				return;
			}	
				
			page = int.Parse(this.txtpage.Text);	
			this.lvitemlist.Items.Clear();
			this.lvitemlist.Scrollable = true;
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtsearch.Text.ToLower();
			string sc = " SELECT ";
			if (cat == "" && kw == "" && m_nTotalItems > 100)
				sc += " TOP 100 ";
//			sc += " * FROM (SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand, c.barcode, sq.qty AS stock ";
			sc += " * FROM( SELECT DISTINCT top " + (size * page) + " c.name, c.name_cn, c.code, c.price1,c.supplier_code,c.tax_rate,c.tax_code, c.special_price, c.is_special, c.brand ";
			//sc += ", c.barcode ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", sq.qty AS stock ";

			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			//			sc += " LEFT OUTER JOIN code_branch cb ON cb.code = c.code";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE 1 = 1 ";

			if (cbcat.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + cbcat.Text.ToLower() + "'";
			if (cb2cat.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + cb2cat.Text.ToLower() + "'";
			if (cb3cat.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + cb3cat.Text.ToLower() + "'";
			if (cbdepart.Text.ToLower() != "")
				sc += " AND LOWER(c.brand) = N'" + cbdepart.Text.ToLower() + "'";

			if (lblshowall.Text == "0")
			{
				try
				{
					int.Parse(kw);
					sc += " AND (c.code ='" + kw + "' OR b.barcode = '" + kw + "')";
				}
				catch (Exception ex)
				{
					string sex = ex.ToString();
					sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
					sc += " OR LOWER(b.barcode) LIKE '%" + kw + "%')";
					sc += " OR LOWER(c.barcode) LIKE '%" + kw + "%'";
				}
			}
			if (m_bShowSpecial)
				sc += " AND c.is_special = 1 ";
			//			sc += " AND c.code = 1003 or c.code=1004 or c.code=1005 or c.code = 1006 or c.code = 1007 or c.code = 1008 ";
			sc += " And c.code > 1009 or c.code < 1001 and c.code > 0";
			sc += " ORDER BY c.code DESC";
			sc += " ) a ORDER BY code  ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");

				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			write_listview(dst.Tables["list"]);
			cbPromotion.Items.Clear();
			cbPromotion.Text = "";
		}
		private void ProductList()
		{
			string cat = cbCatFilter.Text;
			page = 1;
			this.lvitemlist.Items.Clear();
			this.lvitemlist.Scrollable = true;
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtsearch.Text.ToLower().Trim();
            if (dst.Tables["items"] != null)
                dst.Tables["items"].Clear();
			string sc = " SELECT COUNT(code) AS items FROM code_relations c WHERE 1 = 1 ";
            if (cat != "")
                sc += " AND LOWER(c.cat) = N'" + cat.ToLower() + "'";
            sc += " AND (";
            sc += " LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
            sc += " OR LOWER(c.barcode) LIKE '%" + kw + "%' ";
            sc += " OR c.code IN (SELECT item_code FROM barcode WHERE LOWER(barcode) = '" + kw + "') ";
            sc += " OR LOWER(c.supplier_code) LIKE '%" + kw + "' ";
            try
            {
                int.Parse(kw);
                sc += " OR c.code = " + kw + " ";
            }
            catch (Exception ex)
            {
                string sex = ex.ToString();
            }
            sc += ") ";
            if (m_bShowSpecial)
                sc += " AND c.is_special = 1 ";
            sc += " And c.code > 1009 or c.code < 1001 and c.code > 0";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "items") > 0)
				{
					string snItems = dst.Tables["items"].Rows[0]["items"].ToString();
					this.label36.Text = "Total Items :" + snItems;
					m_nTotalItems = Program.MyIntParse(snItems);
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			sc = " SELECT DISTINCT ";
			if (cat == "" && kw == "" && m_nTotalItems > 100)
				sc += " TOP 100 ";
			sc += " c.name, c.name_cn, c.code, c.price1, c.supplier_code, c.tax_rate, c.tax_code ";
			sc += ", c.special_price, c.is_special, c.brand" ;
  //        sc += ", c.barcode ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", sq.qty AS stock ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += " WHERE 1 = 1 ";
            if (cat != "")
                sc += " AND LOWER(c.cat) = N'" + cat.ToLower() + "'";
			//if (cbdepart.Text.ToLower() != "")
			//    sc += " AND LOWER(c.brand) = N'" + cbdepart.Text.ToLower() + "'";
			sc += " AND (";
			sc += " LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
			sc += " OR LOWER(c.barcode) LIKE '%" + kw + "%' ";
			sc += " OR c.code IN (SELECT item_code FROM barcode WHERE LOWER(barcode) = '" + kw + "') ";
			sc += " OR LOWER(c.supplier_code) LIKE '%" + kw + "' ";
			try
			{
				int.Parse(kw);
				sc += " OR c.code = " + kw + " ";
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
			}
			sc += ") ";
			if (m_bShowSpecial)
				sc += " AND c.is_special = 1 ";
			sc += " And c.code > 1009 or c.code < 1001 and c.code > 0";
			sc += " ORDER BY c.code DESC";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");
				sql = sc;
				totalitem = m_nTotalItems;
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string sc1 = sc;
			try
			{
				myAdapter = new SqlDataAdapter(sc1, myConnection);
				rows = myAdapter.Fill(dst, "list");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc1, ex);
				myConnection.Close();
				return;
			}

			write_listview(dst.Tables["list"]);
			cbPromotion.Items.Clear();
			cbPromotion.Text = "";

			if(rows == 1)
			{
				m_sCode = dst.Tables["list"].Rows[0]["code"].ToString();
				lblcode.Text = m_sCode;
				Program.m_sCode = m_sCode;
				DoSelectEditItem();
			}
		}
		private void write_listview(DataTable dt)
		{
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];
				string barcode = dr["barcode"].ToString();
				string code = dr["code"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name_cn"].ToString();
				string stock = dr["stock"].ToString();
				string description = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string tax = dr["tax_rate"].ToString();
				string tax_code = dr["tax_code"].ToString();
				string special_price = dr["special_price"].ToString();
				bool special = Program.MyBooleanParse(dr["is_special"].ToString());
			
				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(supplier_code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(description);
				item.SubItems.Add(stock);
				item.SubItems.Add(Program.MyDoubleParse(price1).ToString("c"));
				item.SubItems.Add(tax_code);
				item.SubItems.Add(tax);
				item.SubItems.Add(Program.MyDoubleParse(special_price).ToString("c"));
				this.lvitemlist.Items.Add(item);
			}
		} 
		private void lvitemlist_SelectedIndexChanged(object sender, EventArgs e)
		{
//			m_bJustSelectedItem = true;
			DoSelectItem();
			DoSelectEditItem();
//			m_bJustSelectedItem = false;
			if (Program.MyDoubleParse(m_sCode) < 1020)
				btndelete.Enabled = false;
			else
				btndelete.Enabled = true;

			this.btnLevelPrice.Enabled = true;
            this.groupBox32.Enabled = true;
		}
		private void DoSelectItem()
		{
			ListView.SelectedListViewItemCollection items = this.lvitemlist.SelectedItems;
			if(items.Count <= 0)
				return;
			m_sCode = items[0].SubItems[0].Text.Trim();
			lblcode.Text = m_sCode;
			/*************************/
			Program.m_sCode = m_sCode;
		}
		private void DoSelectEditItem()
		{
			if (m_sCode.Trim() == "")
				return;
			if (dst.Tables["edititem"] != null)
				dst.Tables["edititem"].Clear();
			string sc = " SELECT c.code, c.name, c.name_cn, c.supplier_code, c.price1, c.special_price, c.is_special AS special, c.brand, c.cat ";
			sc += ", c.s_cat, c.ss_cat, c.manual_cost_frd,c.average_cost,c.has_scale, c.is_barcodeprice, c.is_id_check,  ISNULL(c.special_price_end_date ";
			sc += ", GETDATE()) AS special_price_end_date ";
			sc += ",ISNULL(c.special_price_start_date , GETDATE()) AS special_price_start_date ";
			sc += ", c.promo_id, pl.promo_desc, sq.qty AS stock, c.tax_rate, c.tax_code, c.tax_code, c.barcode, c.bom_id ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += "  left outer join promo pr on c.code = pr.code  ";
			sc += " LEFT OUTER JOIN promotion_list pl ON pl.promo_id = pr.promo_id ";
//			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code AND item_qty = 1 ";
//			sc += " LEFT OUTER JOIN code_branch cb ON c.code = cb.code ";
			sc += " WHERE 1 = 1 ";
//          sc += " and cb.branch_id = 1 ";
			sc += " AND c.code ='" + m_sCode + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "edititem") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			cbdepart.SelectedText = "";
			cbcat.SelectedText = "";
			cb2cat.SelectedText = "";
			cb3cat.SelectedText = "";
			cbTaxCode.SelectedText = "";
			cbTaxCode.Text = "";
			cbcat.Text = "";
			cb2cat.Text = "";
			cb3cat.Text = "";
			txtbarcode.Text = "";
			txtBarcodeQty.Text = "";

			if (dst.Tables["edititem"].Rows.Count >= 1)
			{
				DataRow dr = dst.Tables["edititem"].Rows[0];
				string code = dr["code"].ToString();
				string name = dr["name_cn"].ToString();
				string description = dr["name"].ToString();
				m_sItemName = dr["name"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string price = dr["price1"].ToString();
				string special_price = dr["special_price"].ToString();

				string special_price_start_date = dr["special_price_start_date"].ToString();
				string special_price_end_date = dr["special_price_end_date"].ToString();

				bool special = Program.MyBooleanParse(dr["special"].ToString());
				string depart = dr["brand"].ToString();
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();
				string cost = dr["average_cost"].ToString();//dr["manual_cost_frd"].ToString();
				string barcode = dr["barcode"].ToString();
				bool has_scale = Program.MyBooleanParse(dr["has_scale"].ToString());

				bool is_barcodeprice = Program.MyBooleanParse(dr["is_barcodeprice"].ToString());
				bool is_id_check = Program.MyBooleanParse(dr["is_id_check"].ToString());
				m_dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				string tax_code = dr["tax_code"].ToString();
				
				string stock = dr["stock"].ToString();
				string bom_id = dr["bom_id"].ToString();
				if(bom_id == "")
					btnBom.Text = "Add BOM";
				else
					btnBom.Text = "Edit BOM";
                if (cost == null || cost == "")
                    cost = "0";
				txtBarcodeItem.Text = barcode;
				lbloldbarcode.Text = barcode;
				txtcost.Text = cost;
				txtcostgst.Text = (double.Parse(cost) * (1 + m_dTaxRate)).ToString("N2");
				txtname.Text = name;
				rchdiscription.Text = description;
				txtSupplierCode.Text = supplier_code;
				txtprice.Text = price;
				txtspecialprice.Text = special_price;
				chkspecial.Checked = special;
				cbAutoweigh.Checked = has_scale;
				cbpricebarcode.Checked = is_barcodeprice;
				cbcat.SelectedItem = cat;
				cb2cat.SelectedItem = s_cat;
				cb3cat.SelectedItem = ss_cat;
				cbTaxCode.SelectedItem = tax_code;
//                cbcat.SelectedText = cbcat.SelectedItem.ToString();
//                cb2cat.SelectedText = s_cat;
//                cb3cat.SelectedText = ss_cat;
				cbdepart.SelectedItem = depart;
				btnspeciallist.Text = "CODE: " + lblcode.Text;
				txtItemStock.Text = stock;
				ckbID.Checked = is_id_check;
				txtTaxRate.Text = (m_dTaxRate * 100).ToString();
				BuildPromotion(promo_desc);
				if (File.Exists(Program.m_sPicroot + "\\" + code + ".bmp"))
				{
					string Path = Program.m_sPicroot + "\\" + code + ".bmp";
					this.pictureBox1.Width = 72;
					this.pictureBox1.Height = 72;
					
//					Image b = new Bitmap(Image.FromFile(Path), new Size(100, 130));
//					pictureBox1.Show();
//					pictureBox1.Image = b;
					Image b;
					using (var bmpTemp = new Bitmap(Path))
					{
						b = new Bitmap(bmpTemp);
					}
					this.pictureBox1.Image = b;
					pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
				}
				else
				{
					pictureBox1.Image = null;
				}
				if (special)
				{
					dtpStartSpecial.Enabled = true;
					dtpEndSpecial.Enabled = true;
					txtspecialprice.Enabled = true;
				}
				else
				{
					dtpStartSpecial.Enabled = false;
					dtpEndSpecial.Enabled = false;
					txtspecialprice.Enabled = false;
				}
				dtpStartSpecial.Value = DateTime.Parse(special_price_start_date);
				dtpEndSpecial.Value = DateTime.Parse(special_price_end_date);
				btnBarcodeAdd.Enabled = true;
			}
			RefreshItemBarcode();
			txtcat.Visible = false;
			txt2cat.Visible = false;
			txt3cat.Visible = false;
		}
		private void btnBom_Click(object sender, EventArgs e)
		{
			if(m_sCode == "" || m_sCode == null)
			{
				FormMSG errMsg = new FormMSG();
				errMsg.btnNo.Visible = false;
				errMsg.btnYes.Visible = false;
				errMsg.m_sMsg = " Select an item,please! ";
				errMsg.ShowDialog();
				return;
			}	
			FormBOM fm = new FormBOM();
			fm.m_code = m_sCode;
			fm.m_name = m_sItemName;
			fm.ShowDialog();
		}
		private void btnProductNewItem_Click(object sender, EventArgs e)
		{
			m_sCode = "";
            groupBox32.Enabled = false;
			btnBarcodeAdd.Enabled = false;
			cbdepart.SelectedText = "";
			cbAutoweigh.Checked = false;
			cbpricebarcode.Checked = false;
			lbloldbarcode.Text = "";
			txtcost.Text = "";
			txtcostgst.Text = "";
			txtname.Text = "";
			rchdiscription.Text = "";
			txtSupplierCode.Text = "";
			txtprice.Text = "";
			txtspecialprice.Text = "";
			chkspecial.Checked = false;


			cbcat.SelectedItem = "";
			cb2cat.SelectedItem = "";
			cb3cat.SelectedItem = "";
			cbdepart.SelectedItem = "";
			cbcat.SelectedText = "";
			cb2cat.SelectedText = "";
			cb3cat.SelectedText = "";
			cbcat.Text = "";
			cb2cat.Text = "";
			cb3cat.Text = "";
			cbTaxCode.Text = "";
			pictureBox1.Image = null;

			btnspeciallist.Text = "CODE: NEW";
			txtItemStock.Text = "";
			BuildPromotion("");
			txtbarcode.Text = "";
			txtBarcodeItem.Text = "";
			txtBarcodeQty.Text = "";
			txtTaxRate.Text = "15";
			ckbID.Checked = false;
			lvBarcode.Items.Clear();
			rchdiscription.Focus();
			txtcat.Visible = false;
			txt2cat.Visible = false;
			txt3cat.Visible = false;
		}
		private void TaxcodeList()
		{
			this.cbTaxCode.Items.Clear();
			if (dst.Tables["TaxcodeList"] != null)
				dst.Tables["TaxcodeList"].Clear();
			string sc = "";
			int rows = 0;
			sc += " SELECT * FROM tax_code WHERE 1=1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "TaxcodeList");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			//			this.cbTaxCode.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["TaxcodeList"].Rows[i];
				string tax_code = dr["tax_code"].ToString();
				string tax_rate = dr["tax_rate"].ToString();
				this.cbTaxCode.Items.Add(tax_code);
			}
		}
		
		private bool CheckSuppliercode(string supplier_code)
		{
			DataRow dr = null;
			if (dst.Tables["CheckSuppliercode"] != null)
				dst.Tables["CheckSuppliercode"].Clear();
			string sc = "";
			int rows = 0;
			string code = "";
			string barcode = "";
			string name = "";
			sc += " SELECT * FROM code_relations WHERE 1=1 AND supplier_code = '" + supplier_code + "' AND  supplier_code != ''";
			try
			{
				myAdapter = new SqlDataAdapter(sc,myConnection);
				rows = myAdapter.Fill(dst,"CheckSuppliercode");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (rows > 0)
			{
				dr = dst.Tables["CheckSuppliercode"].Rows[0];
				code  = dr["code"].ToString();
				name = dr["name"].ToString();
				string msg = "Conflict found for this supplier code!\r\nItem Code: " + code + "\r\nDescription:" + name + "\r\n";
				MessageBox.Show(msg);
				return false;
			}	
			return true;
		}

		private bool CheckSuppliercodeForUpdate(string scode, string supplier_code)
		{
			DataRow dr = null;
			if (dst.Tables["CheckSuppliercode"] != null)
				dst.Tables["CheckSuppliercode"].Clear();
			string sc = "";
			int rows = 0;
			string code = "";
			string barcode = "";
			string name = "";
			sc += " SELECT * FROM code_relations WHERE 1=1 AND supplier_code = '" + supplier_code + "' AND  supplier_code != '' AND code != '"+scode+"'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "CheckSuppliercode");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (rows > 0)
			{
				dr = dst.Tables["CheckSuppliercode"].Rows[0];
				code = dr["code"].ToString();
				name = dr["name"].ToString();
				string msg = "Conflict found for this supplier code!\r\nItem Code: " + code + "\r\nDescription:" + name + "\r\n";
				MessageBox.Show(msg);
				return false;
			}
			return true;
		}
		
		private void btnAddNewItem_Click(object sender, EventArgs e)
		{
			if(m_sCode == "" || m_sCode == "Code")
			{

				if (txtBarcodeItem.Text != "" && ExistBarcode(txtBarcodeItem.Text))
				{
					FormMSG fm = new FormMSG();
					fm.m_sMsg = "Sorry, Barcode exist!";
					fm.btnYes.Visible = false;
					fm.btnNo.Visible = false;
					fm.ShowDialog();
					return;
				}
				//if (txtSupplierCode.Text == "")
				//{
				//    FormMSG msg = new FormMSG();
				//    msg.btnYes.Visible = false;
				//    msg.btnNo.Visible = false;
				//    msg.m_sMsg = "Sorry, Supplier_Code cannot be blank!";
				//    msg.ShowDialog();
				//    return;
				//}
					
				//if(txtSupplierCode.Text != "" && !CheckSuppliercode(txtSupplierCode.Text))
				//{
				//    FormMSG msg = new FormMSG();
				//    msg.btnYes.Visible = false;
				//    msg.btnNo.Visible = false;
				//    msg.m_sMsg = "Sorry, this supplier_code exists!\r\n";
				//    msg.ShowDialog();
				//    return;
				//}
				//else
				{
					AddNewItem();
				}
			}
			else 
			{
				if (m_sCode != "" && m_sCode != "Code")
				{
					if (txtBarcodeItem.Text != "" && ExistBarcode(txtBarcodeItem.Text))
					{
						FormMSG fm = new FormMSG();
						fm.m_sMsg = "Sorry, Barcode exist!";
						fm.btnYes.Visible = false;
						fm.btnNo.Visible = false;
						fm.ShowDialog();
						return;
					}

					//if (txtSupplierCode.Text == "")
					//{
					//    FormMSG msg = new FormMSG();
					//    msg.btnYes.Visible = false;
					//    msg.btnNo.Visible = false;
					//    msg.m_sMsg = "Sorry, Supplier_Code cannot be blank!";
					//    msg.ShowDialog();
					//    return;
					//}

					//if (!CheckSuppliercodeForUpdate(m_sCode,txtSupplierCode.Text))
					//{
					//    FormMSG msg = new FormMSG();
					//    msg.btnYes.Visible = false;
					//    msg.btnNo.Visible = false;
					//    msg.m_sMsg = "Sorry, this supplier_code exists!\r\n";
					//    msg.ShowDialog();
					//    return;
					//}
					//else
					{
						UpdateItem();
					}
				}
			}
			btnBarcodeAdd.Enabled = true;
		}


		private void RefreshItemBarcode()
		{
			lvBarcode.Items.Clear();
			int nRows = 0;
			if (dst.Tables["ib"] != null)
				dst.Tables["ib"].Clear();
			string sc = " SELECT b.barcode, b.item_qty, b.package_price ";
			sc += " FROM barcode b ";
			sc += " WHERE b.item_code = " + m_sCode;// + " AND item_qty <> 1 ";
			sc += " ORDER BY b.id ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "ib");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (nRows <= 0)
				return;
			for (int i = 0; i < nRows; i++)
			{
				
				DataRow dr = dst.Tables["ib"].Rows[i];
				string barcode = dr["barcode"].ToString();
				string qty = dr["item_qty"].ToString();
				string package_price = dr["package_price"].ToString();
				if (i == 0)
					labelPrintBarcode.Text = barcode;
				//if (qty == "1")
				//    package_price = "";
				ListViewItem item = new ListViewItem(barcode);
				item.SubItems.Add(qty);
				item.SubItems.Add(package_price);
				item.SubItems.Add("del");
				this.lvBarcode.Items.Add(item);
			}
			this.txtbarcode.Text = "";
			this.txtBarcodeQty.Text = "";
			this.txtPackPrice.Text = "";
		}
		private void txtbarcode_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(txtbarcode.Text != "" && e.KeyChar == '\r')
				txtBarcodeQty.Select();
		}
		private void txtBarcodeQty_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == '\r')
			{
				DoAddBarcode();
			}
		}
		private void btnBarcodeAdd_Click(object sender, EventArgs e)
		{
			if(m_sCode == "")
			{
				MessageBox.Show("Please save new item first");
				return;
			}
			if(txtbarcode.Text == "")
			{
				MessageBox.Show("Please enter barcode and qty");
				return;
			}
			if (ExistBarcode(txtbarcode.Text))
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Sorry, Barcode exist!";
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.ShowDialog();
			}
			else
				DoAddBarcode();
		}
		private void lvBarcode_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvBarcode.SelectedItems;
			if (items.Count <= 0)
				return;
			if (items[0].GetSubItemAt(e.X, e.Y).Text != "del")
				return;
			string barcode = items[0].SubItems[0].Text;
//			if (barcode.Trim() == "")
//				return;
			if (MessageBox.Show("Are you sure you want to delete this barcode? click Yes to delete", "Confirm deleting", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return;
			}
			DoDeleteBarcode(barcode);
		}
		private void DoDeleteBarcode(string barcode)
		{
			string sc = " DELETE FROM barcode WHERE barcode = '" + Program.EncodeQuote(barcode) + "' ";
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
				return;
			}
			RefreshItemBarcode();
		}
		private bool ExistBarcode(string barcode)
		{
			DataRow dr = null;
			string id = "";
			string code = "";
			string name = "";
			string qty = "";
			if (dst.Tables["esistbarcode"] != null)
				dst.Tables["esistbarcode"].Clear();
			string sc = "";
			sc = " SELECT b.id, b.barcode, c.code, c.name, b.item_qty ";
			sc += " FROM barcode b ";
			sc += " LEFT OUTER JOIN code_relations c ON c.code = b.item_code ";
			sc += " WHERE b.barcode <> '' AND b.barcode = N'" + Program.EncodeQuote(barcode) + "' ";
			//sc += " OR c.barcode = N'" + Program.EncodeQuote(barcode) + "' ";
			sc += " AND b.item_code <> '" + m_sCode + "'";
			//sc += " AND b.item_qty <> 1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "esistbarcode") > 0)
				{
					dr = dst.Tables["esistbarcode"].Rows[0];
					id = dr["id"].ToString();
					name = dr["name"].ToString();
					code = dr["code"].ToString();
					qty = dr["item_qty"].ToString();
					string msg = "Conflict found for this barcode!\r\nItem Code: " + code + "\r\nDescription:" + name + "\r\nBarcode QTY:" + qty + "\r\nDo you want to delete this barcode entry?";
					if(MessageBox.Show(msg, "Barcode Exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						sc = " DELETE FROM barcode WHERE id = " + id;
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
							return true;
						}
						return false; //wrong barcode deleted
					}
					return true;
				}
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return true;
			}
//            sc = " SELECT code, name, supplier_code ";
//            sc += " FROM code_relations ";
//            sc += " WHERE barcode = N'" + Program.EncodeQuote(barcode) + "' ";
////			sc += " AND code <> '" + m_sCode + "' ";
//            try
//            {
//                myAdapter = new SqlDataAdapter(sc, myConnection);
//                if(myAdapter.Fill(dst, "esistbarcode") > 0)
//                {
//                    dr = dst.Tables["esistbarcode"].Rows[0];
//                    name = dr["name"].ToString();
//                    code = dr["code"].ToString();
//                    string mpn = dr["supplier_code"].ToString();
//                    string msg = "Error, found item uses this barcode as main barcode!\r\nItem Code: " + code + "\r\nSupplier Code:" + mpn + "\r\nDescription:" + name + "\r\nDo you want to delete this item?";
//                    if(MessageBox.Show(msg, "Barcode Exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
//                    {
//                        sc = " DELETE FROM code_relations WHERE code = " + code;
//                        try
//                        {
//                            myCommand = new SqlCommand(sc);
//                            myCommand.Connection = myConnection;
//                            myCommand.Connection.Open();
//                            myCommand.ExecuteNonQuery();
//                            myCommand.Connection.Close();
//                        }
//                        catch (Exception e)
//                        {
//                            myConnection.Close();
//                            Program.ShowExp(sc, e);
//                            return true;
//                        }
//                        return false; //wrong barcode deleted
//                    }
//                    return true;
//                }
//            }
//            catch (Exception e1)
//            {
//                myConnection.Close();
//                Program.ShowExp(sc, e1);
//                return true;
//            }
			return false;
		}
		private void DoAddBarcode()
		{
			string barcode = txtbarcode.Text.Trim();
			string pack_price = "";
			if(this.txtPackPrice.Text != "" && txtPackPrice.Text != null)
				pack_price = this.txtPackPrice.Text;
			else
				pack_price = this.txtprice.Text;
			if(barcode == "")
				return;
			bool bExists = doCheckExistBarcode(barcode);
			double dQty = Program.MyDoubleParse(txtBarcodeQty.Text);
			if(dQty == 0)
				dQty = 1;
			string sc = "";
			if(bExists)
			{
				if (dQty == 1)
					sc = " UPDATE barcode SET item_qty = " + dQty + " WHERE item_code = " + m_sCode + " AND barcode = '" + Program.EncodeQuote(barcode) + "' ";
				else if (dQty > 1)
					sc = " UPDATE barcode SET item_qty = " + dQty + ", SET package_price = " + pack_price + " WHERE item_code = " + m_sCode + " AND barcode = '" + Program.EncodeQuote(barcode) + "' ";
			}
			else
			{
				if (dQty == 1)
					sc = " INSERT INTO barcode(item_code, barcode, item_qty) VALUES(" + m_sCode + ", '" + Program.EncodeQuote(barcode) + "', " + dQty + ") ";
				else if (dQty > 1)
					sc = " INSERT INTO barcode(item_code, barcode, item_qty, package_price) VALUES(" + m_sCode + ", '" + Program.EncodeQuote(barcode) + "', " + dQty + ", " + pack_price + ") ";
			}
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
				return;
			}
			RefreshItemBarcode();
		}
		private bool SetSiteSettings(string sSettingName, string sCode)
		{
			if (sSettingName.Trim() == "")
				return false;
			if (Program.m_sCompanyName.Trim() == "")
				return false;
			string sc = " BEGIN TRANSACTION ";
			sc += " IF NOT EXISTS( SELECT  value FROM settings WHERE 1=1 ";
			sc += " AND name = N'" + sSettingName + "' ";
			sc += " AND cat = 'Funcation Key')";
			sc += " INSERT INTO settings(cat, name, value, description )VALUES('Funcation Key', '" + sSettingName + "', '" + sCode + "', '" + sSettingName + "')";
			sc += " ELSE UPDATE settings SET value= '" + sCode + "' WHERE name = N'" + sSettingName + "' ";
			sc += " COMMIT";
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
		private string GetSiteSettings(string sSettingName)
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
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return ""; ;
			}
			if (dst.Tables["rsp"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["rsp"].Rows[0];
				return dr["value"].ToString();
			}
			return "";
		}
		private void DoBuildDepart()
		{
			cbdepart.Items.Clear();
			int rows = 0;
			if (dst.Tables["depart"] != null)
				dst.Tables["depart"].Clear();
			string sc = " SELECT  s_cat FROM catalog ";
			sc += " WHERE LOWER(cat) = N'brands'";
			sc += " GROUP BY s_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "depart");
				if (rows <= 0)
					return ;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return ;
			}
			cbdepart.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["depart"].Rows[i];
				string depart = dr["s_cat"].ToString();
				cbdepart.Items.Add(depart);
			}
		}
		private void DoBuildCat()
		{
			cbcat.Items.Clear();
			cbDealerPriceCat.Items.Clear();
			int rows = 0;
			if (dst.Tables["cat"] != null)
				dst.Tables["cat"].Clear();
			string sc = " SELECT DISTINCT cat FROM catalog ";
			sc += " WHERE LOWER(cat) <> N'brands' ";
			sc += " AND LOWER(cat) <> 'serviceitem' ";
			sc += " ORDER BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "cat");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			cbcat.Items.Add("");
			cbDealerPriceCat.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["cat"].Rows[i];
				string cat = dr["cat"].ToString();
				cbcat.Items.Add(cat);
				cbDealerPriceCat.Items.Add(cat);
			}
		}
		private void BuildPromotion(string name_current)
		{
			int rows = 0;
			if (dst.Tables["promo"] != null)
				dst.Tables["promo"].Clear();
			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT * FROM promotion_list WHERE 1=1 ";
//			sc += " AND promo_branch_id = " + Session["branch_id"] + " ";
			sc += " AND promo_active = 1 ";
			sc += " AND promo_end_date >= GETDATE()";
			sc += " ORDER BY promo_desc ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "promo");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			cbPromotion.Items.Clear();
			cbPromotion.Items.Add("");
			cbPromotion.SelectedIndex = 0;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["promo"].Rows[i];
				string id = dr["promo_id"].ToString();
				string name = dr["promo_desc"].ToString();
				cbPromotion.Items.Add(name);
				if(name == name_current)
					cbPromotion.SelectedIndex = i + 1;
			}
		}
		private string GetPromotionId(string name)
		{
			int rows = 0;
			if (dst.Tables["promo"] != null)
				dst.Tables["promo"].Clear();
			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT * FROM promotion_list WHERE promo_desc = N'" + Program.EncodeQuote(name) + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "promo");
				if (rows <= 0)
					return "0";
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return "0";
			}
			return dst.Tables["promo"].Rows[0]["promo_id"].ToString();
		}
        private string GetPromoIdByItemCode(string code)
        {
            string table_name = "GetPromoIdByItemCode";
            int rows = 0;
            if (dst.Tables[table_name] != null)
                dst.Tables[table_name].Clear();
            string sc = " SET DATEFORMAT dmy ";
            sc += " SELECT * FROM promo WHERE code = N'" + Program.EncodeQuote(code) + "' ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                rows = myAdapter.Fill(dst, table_name);
                if (rows <= 0)
                    return "0";
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return "0";
            }
            return dst.Tables[table_name].Rows[0]["promo_id"].ToString();
        }
		private void adminaddp_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			pnlproedit.Visible = true;
			btnspeciallist.Visible = true;
            groupBox32.Enabled = false;
			btnspeciallist.Text = "CODE";
			m_sCode = "";
			txtbarcode.Text = "";
			txtcost.Text = "";
			txtcostgst.Text = "";
			txtname.Text = "";
			rchdiscription.Text = "";
			txtSupplierCode.Text = "";
			txtprice.Text = "";
			txtTaxRate.Text = "15";
			txtspecialprice.Text = "";
			chkspecial.Checked = false;
			cbcat.SelectedItem = "";
			cb2cat.SelectedItem = "";
			cb3cat.SelectedItem = "";
			cbdepart.SelectedItem = "";
			lblcode.Text = "Code";
			cbTaxCode.Text = "";
			DoBuildDepart();
			DoBuildCat();
			lvitemlist.Items.Clear();
			DoBuildCatFilter();
			DoCheckBarcodeForOldDB();
			ProductList();
		}
		bool DoCheckBarcodeForOldDB()
		{
			string sc = " UPDATE code_relations SET barcode = ";
			sc += " (ISNULL((SELECT top 1 barcode FROM barcode WHERE item_code = code_relations.code AND item_qty = 1), '')) ";
			sc += " WHERE code_relations.barcode = '' OR code_relations.barcode IS NULL ";
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
		private void DoBuildCatFilter()
		{
			cbCatFilter.Items.Clear();
			int rows = 0;
			if (dst.Tables["cat"] != null)
				dst.Tables["cat"].Clear();
			string sc = " SELECT DISTINCT cat FROM catalog ";
			sc += " WHERE LOWER(cat) <> N'brands' ";
			sc += " ORDER BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "cat");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			cbCatFilter.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["cat"].Rows[i];
				string cat = dr["cat"].ToString();
				cbCatFilter.Items.Add(cat);
			}
		}
		private void listView1_KeyDown(object sender, KeyEventArgs e)
		{

			DoSelectItem();
			DoSelectEditItem();
			if(Program.MyDoubleParse(m_sCode) < 1020)
				btndelete.Enabled = false;
			else
				btndelete.Enabled = true;		
			this.btnLevelPrice.Enabled = true;	
		}
		private bool PromotionExists(string code)
		{
			if(code.Trim() == "")
				return false;
			string sc = " SELECT pg.id ";
			sc += " FROM promotion_group pg ";
			sc += " JOIN barcode b ON b.barcode = pg.barcode ";
			sc += " JOIN code_relations c ON c.code = b.item_code ";
			sc += " WHERE c.code = " + code;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "promo_check") > 0)
				{
					MessageBox.Show("error, this item is already in a promotion group");
					return true;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return true;
			}
			return false;
		}
		private bool ValidateMoney(TextBox[] ac)
		{
			for(int i=0; i<ac.Length; i++)
			{
				string v = ac[i].Text.Replace("$", "").Trim();
				if(v == "")
					continue;
				try
				{
					double d = Double.Parse(v);
				}
				catch(Exception e)
				{
					TextBox tb = (TextBox)ac[i];
					tb.SelectAll();
					tb.Focus();
					Program.MsgBox("Invalid input @ " + tb.Name + ", please try again");
					return false;
				}
			}
			return true;
		}
		private bool UpdateItem()
		{
			if(!ValidateMoney(new TextBox[]{txtItemStock, txtTaxRate, txtprice, txtcost, txtspecialprice}))
				return false;
			string promo_desc = cbPromotion.Text;
			string promo_id = "0";
			if(promo_desc != "")
				promo_id = GetPromotionId(promo_desc);
			if (chkspecial.Checked && promo_desc != "")
			{
//				FormMSG fm = new FormMSG();
//				fm.m_sMsg = "Cannot apply promotion while item is on special";
//				fm.ShowDialog();
				MessageBox.Show("Item on promotion, promoton_id " + promo_id + ". \r\n System take special price as first choice.");
				//promo_id = "0";
				//return false;
			}
			//if(promo_id != "0")     19/02/2018 change for activita,cuz want to change selling price while item is on promotion.
			//{
			//    if(PromotionExists(m_sCode))
			//    {
			//        return false;
			//    }
			//}
			string cat = cbcat.Text;
			string cat2 = cb2cat.Text;
			string cat3 = cb3cat.Text;
			bool bUpdateCatalog = false;
			if(txtcat.Text != "")
			{
				cat = txtcat.Text;
				bUpdateCatalog = true;
			}
			if (txt2cat.Text != "")
			{
				cat2 = txt2cat.Text;
				bUpdateCatalog = true;
			}
			if (txt3cat.Text != "")
			{
				cat3 = txt3cat.Text;
				bUpdateCatalog = true;
			}
			string sBarcode = txtBarcodeItem.Text.Trim();
			double dStock = Program.MyDoubleParse(txtItemStock.Text);
			double dTaxRate = Program.MyDoubleParse(txtTaxRate.Text) / 100;
			string sc = " SET DATEFORMAT dmy ";
			sc += " UPDATE code_relations SET name_cn = N'" + txtname.Text + "', name = N'" + rchdiscription.Text + "'";
			sc += " , price1 ='"+ txtprice.Text + "', manual_cost_frd ='"+ txtcost.Text + "'";
			sc += " , cat = N'" + Program.EncodeQuote(cat) + "', s_cat = N'" + Program.EncodeQuote(cat2) + "' ";
			sc += ", ss_cat = N'" + Program.EncodeQuote(cat3) + "' ";
			if(cbdepart.Text.Trim() != "")
			  sc += " , brand =N'" + cbdepart.Text + "'";
			//sc += " , supplier_price ='" + txtcost.Text + "', average_cost ='" + txtcost.Text + "'";
			sc += ", average_cost ='" + txtcost.Text + "'";
			sc += ", supplier_code = '" + txtSupplierCode.Text + "'";
			sc += ", has_scale = '" + this.cbAutoweigh.Checked + "'";
			sc += ", is_barcodeprice = '" +this.cbpricebarcode.Checked+ "'";
			sc += ", is_id_check = '" + this.ckbID.Checked + "'";
			sc += ", is_special = '" + chkspecial.Checked.ToString() + "' ";
			sc += ", special_price = " + Program.MyMoneyParse(txtspecialprice.Text).ToString() + " ";

			sc += ", special_price_start_date = '" + dtpStartSpecial.Value.ToShortDateString() + "' ";
			sc += ", special_price_end_date = '" + dtpEndSpecial.Value.ToShortDateString() + " 23:59:59.000' ";

			sc += ", promo_id = " + promo_id;
			sc += ", tax_rate = " + dTaxRate;
			sc += ", tax_code = N'" + this.cbTaxCode.Text + "' ";
			sc += ", barcode = '" + Program.EncodeQuote(sBarcode) + "' ";
			sc += " WHERE code = '" + m_sCode + "' ";
			sc += " UPDATE code_relations SET cat = '' WHERE 1=1 AND cat is null ";
//			sc += " UPDATE code_branch SET price1 ='" + txtprice.Text + "', special ='" + chkspecial.Checked.ToString() + "',";
//			sc += " special_price ='"+txtspecialprice.Text+"'";
//			sc += ", special_price_end_date = '" + dtpEndSpecial.Value.ToShortDateString() + "'";
//			sc += " WHERE code ='" + m_sCode + "'";
/*
			if(sBarcode != "")
			{
				sc += " DELETE FROM barcode WHERE item_code = " + m_sCode + " AND item_qty = 1 ";
				sc += " INSERT INTO barcode(item_code, barcode) VALUES(" + m_sCode + ", '" + Program.EncodeQuote(sBarcode) + "') ";
			}
*/
			sc += " IF NOT EXISTS(SELECT id FROM stock_qty WHERE branch_id = 1 AND code = " + m_sCode + ") ";
			sc += " INSERT INTO stock_qty(branch_id, code, qty) VALUES(1, " + m_sCode + ", " + dStock + ") ";
			sc += " ELSE ";
			sc += " UPDATE stock_qty SET qty = " + dStock + " WHERE branch_id = 1 AND code = " + m_sCode;
//			sc += " UPDATE stock_qty SET qty = " + dStock + " WHERE branch_id = 1 AND code = " + m_sCode;
			if (bUpdateCatalog)
			{
				sc += " IF NOT EXISTS(SELECT id FROM catalog ";
				sc += " WHERE cat = N'" + Program.EncodeQuote(cat) + "' ";
				sc += " AND s_cat = N'" + Program.EncodeQuote(cat2) + "' ";
				sc += " AND ss_cat = N'" + Program.EncodeQuote(cat3) + "') ";
				sc += " INSERT INTO catalog(cat, s_cat, ss_cat) VALUES(";
				sc += " N'" + Program.EncodeQuote(cat) + "' ";
				sc += ", N'" + Program.EncodeQuote(cat2) + "' ";
				sc += ", N'" + Program.EncodeQuote(cat3) + "') ";
			}
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
			MessageBox.Show("Item Update Done");
//			btnAddNewItem.Text = "Add New Item";
			ProductList();
//			adminaddp_Click(null, null);
			return true;
		}
		private bool AddNewItem()
		{
			if (dst.Tables["selectcode"] != null)
				dst.Tables["selectcode"].Clear();
			string newCode = "1010";
			string sc = " SELECT TOP 1 code FROM code_relations where code > 1009";
			sc += " ORDER BY code DESC ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "selectcode") == 1)
				{
					newCode = (1 + (Program.MyDoubleParse(dst.Tables["selectcode"].Rows[0]["code"].ToString()))).ToString();
					sc = " DELETE FROM code_relations WHERE id = '" + newCode + "' ";
					sc += " UPDATE code_relations SET cat = '' WHERE 1=1 AND cat is null ";
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
						myCommand.Connection.Close();
						Program.ShowExp(sc, e);
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (cbcat.Text.Trim() == "" && txtcat.Text.Trim() == "")
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false; 
				fm.m_sMsg = "First Catalog Cannot be Blank";
				fm.ShowDialog();
				return false;
			}
			lblcode.Text = newCode;
			string pid = cbdepart.Text + newCode;
			m_sCode = lblcode.Text;

			if (txtBarcodeItem.Text.Trim() == "")
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = true;
				fm.btnNo.Visible = true;
				fm.btnOK.Visible = false;
				fm.m_sMsg = "Barcode Cannot be Blank, if the new item does not have barcode, the barcode will default to the system code.";
				fm.ShowDialog();
				if (fm.m_bYes)
				{
					if (ExistBarcode(m_sCode))
						return false;
				}
				else
					return false;
			}

			string promo_id = cbPromotion.Text;
			if (chkspecial.Checked && promo_id != "0")
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Item on promotion, promoton_id " + promo_id + ". \r\n System take special price as first choice.";
				fm.ShowDialog();
				//promo_id = "0";
				//return false;
			}
			if(promo_id == "")
				promo_id = "0";
			string cat = cbcat.Text;
			string cat2 = cb2cat.Text;
			string cat3 = cb3cat.Text;
			if (txtcat.Text != "")
				cat = txtcat.Text;
			if (txt2cat.Text != "")
				cat2 = txt2cat.Text;
			if (txt3cat.Text != "")
				cat3 = txt3cat.Text;
			double dStock = Program.MyDoubleParse(txtItemStock.Text);
			string barcode = this.txtBarcodeItem.Text.Trim();// txtBarcodeItem.Text.Trim();
			double dBarcodeQty = Program.MyDoubleParse(txtBarcodeQty.Text);
			string supplier_code = txtSupplierCode.Text.Trim();
			if (supplier_code == null || supplier_code == "")
				supplier_code = m_sCode;
			if (barcode == "")
				barcode = m_sCode;
			if (dBarcodeQty == 0)
				dBarcodeQty = 1;
//            if (tbgstrate.Text == "" || tbgstrate.Text == null)
			tbgstrate.Text = GetSiteSettings("gst_rate_percent");
            double dTaxRate = Program.MyDoubleParse(txtTaxRate.Text) / 100;
//			sc = " INSERT INTO code_relations (id, code, name_cn, name , manual_cost_frd,manual_cost_nzd, cat, s_cat, ss_cat, barcode,brand, supplier_price, average_cost, price1, supplier_code, promo_id)";
			sc = " INSERT INTO code_relations (id, code, barcode, name_cn, name , manual_cost_frd,manual_cost_nzd, cat, s_cat, ss_cat, has_scale, is_barcodeprice, brand, supplier_price, average_cost, price1, supplier_code, promo_id, is_id_check, tax_rate, tax_code)";
			sc += " VALUES ( '" + pid + "', '" + m_sCode + "', '" + Program.EncodeQuote(barcode) + "', N'" + txtname.Text.Replace("'", "") + "', N'" + rchdiscription.Text.Replace("'", "") + "'";
			sc += " , '" + txtcost.Text + "', '" + txtcost.Text + "', N'" + Program.EncodeQuote(cat) + "' ";
			sc += ", N'" + Program.EncodeQuote(cat2) + "', N'" + Program.EncodeQuote(cat3) + "' ";
			sc += " , '" + this.cbAutoweigh.Checked + "'";
			sc += " , '" + this.cbpricebarcode.Checked + "'"; 
			sc += " , N'" + cbdepart.Text + "', '" + txtcost.Text + "', '" + txtcost.Text + "' ";
			sc += " , '"+ txtprice.Text +"'";
//			sc += " , '" + m_sCode + "'";
			sc += " , '" + supplier_code + "'";
			sc += ", " + promo_id;
			sc += ", '" + this.ckbID.Checked + "'";
			sc += ", " + dTaxRate;
			sc += ", N'" + this.cbTaxCode.Text + "'";
			sc += " )";
			sc += " INSERT INTO product (code, name, brand, cat, s_cat, ss_cat, hot, price, eta, supplier, supplier_code, supplier_price, price_dropped, price_age, allocated_stock, popular, real_stock, name_cn)";
			sc += " SELECT c.code, c.name, c.brand, c.cat, c.s_cat, c.ss_cat, 1, c.price1, '', c.supplier, c.supplier_code, c.supplier_price, 0, GETDATE(), '0', 1, '0', c.name_cn ";
			sc += " FROM code_relations c where c.code ='" + m_sCode + "'";
			sc += " INSERT INTO stock_qty (code, qty, branch_id, supplier_price, allocated_stock, average_cost, qpos_price, sp_start_date, sp_end_date)";
			sc += " SELECT c.code, " + dStock + ", 1, c.manual_cost_frd, 0, c.manual_cost_frd, c.price1, GETDATE(), GETDATE() FROM code_relations c WHERE c.code='" + m_sCode + "'";

			if(barcode != "")
			{
				sc += " IF NOT EXISTS (SELECT id FROM barcode WHERE item_code = " + m_sCode + " AND barcode = N'" + Program.EncodeQuote(barcode) + "')";
				sc += " INSERT INTO barcode (item_code, barcode, item_qty) VALUES(" + m_sCode + ", N'" + Program.EncodeQuote(barcode) + "', " + dBarcodeQty + ") ";
			}
 
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
				myCommand.Connection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			MessageBox.Show("Item Added");
			btnspeciallist.Text = "CODE: " + newCode;
			ProductList();
			if (MessageBox.Show("New Item Added. Are you sure you want to add another one?", "Add New Item", MessageBoxButtons.YesNo) == DialogResult.Yes)
				adminaddp_Click(null, null);
			return true;
		}
		private void txtcost_KeyUp(object sender, KeyEventArgs e)
		{
			if (!FieldValidate("Num", txtcost.Text))
				txtcost.Text = "";
			txtcostgst.Text = (Program.MyDoubleParse(txtcost.Text) * (1 + Program.MyDoubleParse(txtTaxRate.Text)/100)).ToString();
		}
		private void btndelete_Click(object sender, EventArgs e)
		{
			btnLevelPrice.Enabled = false;	
			if (m_sCode == "" || m_sCode == "Code")
				return;
			if (MessageBox.Show("Are you sure you want to delete this item : "+ m_sCode+ "?", "Delete Item", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				string sc = "";//UPDATE ebutton SET code ='', name= '', cat = '' WHERE code='" + m_sCode + "'";
				sc += " DELETE FROM stock_qty WHERE code ='" + m_sCode + "'";
//				sc += " DELETE FROM code_branch WHERE code ='" + m_sCode + "'";
				sc += " DELETE FROM product WHERE code ='" + m_sCode + "'";
				sc += " DELETE FROM code_relations WHERE code ='" + m_sCode + "'";
				sc += " DELETE FROM barcode WHERE item_code ='" + m_sCode + "'";
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception ex)
				{
					myConnection.Close();
					Program.ShowExp(sc, ex);
					return ;
				}
				ProductList();
				RefreshItemBarcode();
				btnProductNewItem_Click(sender, e);
				adminaddp_Click(sender, e);
			}
		}
		private bool FieldValidate(string inputType, string value)
		{
			if(value == "")
				return true;
			if (inputType == "Num")
			{
				try
				{
					double.Parse(value);
				}
				catch (Exception ex)
				{
					string sex = ex.ToString();
					FormMSG fm = new FormMSG();
					fm.btnNo.Visible = false;
					fm.btnYes.Visible = false;
					fm.m_sMsg = " Please Enter Number Only ";
					fm.ShowDialog();
					return false;
				}
			}
			return true;
		}
		private void txtTaxRate_KeyUp(object sender, KeyEventArgs e)
		{
			if (!FieldValidate("Num", txtTaxRate.Text))
			{
				txtTaxRate.Text = "15";
				return;
			}
			m_dTaxRate = Program.MyDoubleParse(txtTaxRate.Text) / 100;
			txtcost.Text = (Math.Round(Program.MyDoubleParse(this.txtcostgst.Text) / (1 + m_dTaxRate), 2)).ToString();
		}
		private void txtprice_KeyUp(object sender, KeyEventArgs e)
		{
			if(!FieldValidate("Num", txtprice.Text))
				txtprice.Text = "";
		}
		private void txtcostgst_KeyUp(object sender, KeyEventArgs e)
		{
			if (!FieldValidate("Num", txtcostgst.Text))
				txtcostgst.Text = "";
//			txtcost.Text = (Math.Round(Program.MyDoubleParse(this.txtcostgst.Text) / (1 + m_dTaxRate), 2)).ToString();txtTaxRate.Text
			txtcost.Text = (Math.Round(Program.MyDoubleParse(this.txtcostgst.Text) / (1 + Program.MyDoubleParse(this.txtTaxRate.Text)/100), 2)).ToString();
		}
		private void txtspecialprice_KeyUp(object sender, KeyEventArgs e)
		{
			if (!FieldValidate("Num", txtspecialprice.Text))
				txtspecialprice.Text = "";
		}
		private void btnSearch_Click(object sender, EventArgs e)
		{
			lblshowall.Text = "0";
			ProductList();
		}
		private void txtsearch_KeyUp(object sender, KeyEventArgs e)
		{
			lblshowall.Text = "0";
			if(e.KeyCode == Keys.Return)
			{
				ProductList();
				txtsearch.Select(0, txtsearch.Text.Length);
			}
		}
		private void HideAllPanel()
		{
			pnlproedit.Visible = false;
			pnlcats.Visible = false;
			panelreports.Visible = false;
			pnlcats.Visible = false;
			pnlsystemsetting.Visible = false;
			btnspeciallist.Visible = false;
			tccompany.Visible = false;
			pnlPromotion.Visible = false;
			pnlButton.Visible = false;
			gbTouchScreenLayout.Visible = false;
			pnlStockTake.Visible = false;
			pnlStockInput.Visible = false;
			pnlTax.Visible = false;
			pnlVipAccount.Visible = false;
			pnlGroupPOption.Visible = false;
			GBCombo.Visible = false;
		}
		private void adminpe_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			pnlcats.Visible = true;
			pnlcats.BringToFront();
			//if(!RefreshCatalogTable())
			//    return;
//          InsertCatalog();
			DoBuildDepartList();
			DoBuildCatList();
			DoBuildSCatList();
			DoBuildSSCatList();
		}
        private void InsertCatalog()
        {
            string sc = "";
            sc = "DELETE from catalog WHERE 1=1 ";
            sc += " Insert into catalog(cat, s_cat, ss_cat) ";
            sc += " SELECT distinct cat, s_cat, ss_cat From code_relations where 1=1 ";
            sc += " AND cat is not null and cat != ''";
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
                return;
            }

        }
		private void DoBuildDepartList()
		{
			listdepart.Items.Clear();
			int rows = 0;
			if (dst.Tables["depart"] != null)
				dst.Tables["depart"].Clear();
			string sc = " SELECT s_cat FROM catalog ";
			sc += " WHERE LOWER(cat) = N'brands' ";
			sc += " GROUP BY s_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "depart");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["depart"].Rows[i];
				string depart = dr["s_cat"].ToString();
				listdepart.Items.Add(depart);
			}
		}
		private bool RefreshCatalogTable()
		{
			int nBrands = 0;
			if(dst.Tables["rcbrand"] != null)
				dst.Tables["rcbrand"].Clear();
			string sc = " SELECT DISTINCT brand, s_cat FROM code_relations WHERE brand <> '' ORDER BY brand ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nBrands = myAdapter.Fill(dst, "rcbrand");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			int nCats = 0;
			if (dst.Tables["rccat"] != null)
				dst.Tables["rccat"].Clear();
			sc = " SELECT DISTINCT cat, s_cat, ss_cat FROM code_relations WHERE cat <> '' ORDER BY cat, s_cat, ss_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nCats = myAdapter.Fill(dst, "rccat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}

			sc = " DELETE FROM catalog WHERE 1 = 1 ";
			for (int i = 0; i < nBrands; i++)
			{
				string brand = dst.Tables["rcbrand"].Rows[i]["brand"].ToString();
				string s_cat = dst.Tables["rcbrand"].Rows[i]["s_cat"].ToString();
				sc += " INSERT INTO catalog(cat, s_cat, ss_cat) VALUES('Brands', N'" + Program.EncodeQuote(brand) + "', N'" + Program.EncodeQuote(s_cat) + "') ";
			}
			for (int i = 0; i < nCats; i++)
			{
				DataRow dr = dst.Tables["rccat"].Rows[i];
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();
				sc += " INSERT INTO catalog(cat, s_cat, ss_cat) VALUES(N'" + Program.EncodeQuote(cat) + "' ";
				sc += ", N'" + Program.EncodeQuote(s_cat) + "', N'" + Program.EncodeQuote(ss_cat) + "') ";
			}
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return false;
			}
			return true;
		}
		private void DoBuildCatList()
		{
			listcat.Items.Clear();
			int rows = 0;
			if (dst.Tables["addcat"] != null)
				dst.Tables["addcat"].Clear();
			string sc = " SELECT DISTINCT cat FROM catalog ";
			sc += " WHERE LOWER(cat) <> N'brands' ";
			sc += " ORDER BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "addcat");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
		   
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["addcat"].Rows[i];
				string cat = dr["cat"].ToString().Trim();
				if(cat != "")
					listcat.Items.Add(cat);
			}
		}
		private void DoBuildSCatList()
		{
			listscat.Items.Clear();
			string cat = listcat.Text.Trim();
			if(cat == "")
				return;
			int rows = 0;
			if (dst.Tables["addscat"] != null)
				dst.Tables["addscat"].Clear();
			string sc = " SELECT distinct s_cat FROM catalog ";
			sc += " WHERE cat = N'" + Program.EncodeQuote(cat) + "' ";
			sc += " ORDER BY s_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "addscat");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["addscat"].Rows[i];
				string scat = dr["s_cat"].ToString().Trim();
				if(scat != "")
					listscat.Items.Add(scat);
			}
		}
		private void DoBuildSSCatList()
		{
			listsscat.Items.Clear();
			string cat = listcat.Text.Trim();
			string scat = listscat.Text.Trim();
			if(cat == "" || scat == "")
				return;
			int rows = 0;
			if (dst.Tables["addsscat"] != null)
				dst.Tables["addsscat"].Clear();
			string sc = " SELECT distinct ss_cat FROM catalog ";
			sc += " WHERE cat = N'" + Program.EncodeQuote(cat) + "' ";
			sc += " AND s_cat = N'" + Program.EncodeQuote(scat) + "' ";
			sc += " ORDER BY ss_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "addsscat");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["addsscat"].Rows[i];
				string sscat = dr["ss_cat"].ToString().Trim();
				if(sscat != "")
					listsscat.Items.Add(sscat);
			}
		}
		private void listcat_SelectedIndexChanged(object sender, EventArgs e)
		{	
			DoBuildSCatList();
			DoBuildSSCatList();
		}
		private void listscat_SelectedIndexChanged(object sender, EventArgs e)
		{
			DoBuildSSCatList();
		}
		private void btnadddepart_Click(object sender, EventArgs e)
		{
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "Department";
			lblcattitlehide.Text = "";
			txtinputcat.Focus();
		}
		private void btncat_Click(object sender, EventArgs e)
		{
			if (txtinputcat.Text.Trim() == "")
				return;
			if (lblcattitlehide.Text.Trim() == "")
			{
				string sc = " INSERT INTO catalog (seq, ";
				if (lblcathide.Text == "Department")
					sc += " cat, s_cat ";
				else if (lblcathide.Text == "Category")
					sc += " cat ";
				else if (lblcathide.Text == "s_cat")
					sc += " cat, s_cat ";
				else if (lblcathide.Text == "ss_cat")
					sc += " cat, s_cat, ss_cat";
				sc += " ) VALUES";
				if (lblcathide.Text == "Department")
					sc += "  ('99', 'Brands', N'" + txtinputcat.Text + "')";
				else if (lblcathide.Text == "Category")
					sc += " ('99',N'" + txtinputcat.Text + "')";
				else if (lblcathide.Text == "s_cat")
//					sc += " ('99','Others',N'" + txtinputcat.Text + "')";
					sc += " ('99',N'" + listcat .SelectedItem + "',N'" + txtinputcat.Text + "')";
				else if (lblcathide.Text == "ss_cat")
//					sc += " ('99', 'Others', 'Others', N'" + txtinputcat.Text + "')";
					sc += " ('99', '" + listcat.SelectedItem + "', '"+listscat .SelectedItem+"', N'" + txtinputcat.Text + "')";
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception ex)
				{
					myConnection.Close();
					Program.ShowExp(sc, ex);
					return;
				}
			}
			else
			{
				string sc = " UPDATE catalog ";
				if (lblcathide.Text == "Department")
				{
					sc += " SET s_cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(cat) = N'brands' AND LOWER(s_cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
				   
					sc += " UPDATE code_relations SET brand = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(brand) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE product SET brand = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(brand) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE printer_mapping SET depart = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(depart) = N'" + lblcattitlehide.Text.ToLower() + "'";
				}
				else if (lblcathide.Text == "Category")
				{
					sc += " SET cat =N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE code_relations SET cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE product SET cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
				}
				else if (lblcathide.Text == "s_cat")
				{
					sc += " SET s_cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(s_cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE code_relations SET s_cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(s_cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE product SET s_cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(s_cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
				}
				else if (lblcathide.Text == "ss_cat")
				{
					sc += " SET ss_cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(ss_cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE code_relations SET ss_cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(ss_cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
					sc += " UPDATE product SET s_cat = N'" + txtinputcat.Text + "'";
					sc += " WHERE LOWER(ss_cat) = N'" + lblcattitlehide.Text.ToLower() + "'";
				}
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception ex)
				{
					myConnection.Close();
					Program.ShowExp(sc, ex);
					return;
				}
				this.pnlcattxt.Visible = false;
			}
			if (lblcathide.Text == "Department")
				DoBuildDepartList();
			if (lblcathide.Text == "Category")
				DoBuildCatList();
			if (lblcathide.Text == "s_cat")
				DoBuildSCatList();
			if (lblcathide.Text == "ss_cat")
				DoBuildSSCatList();
			txtinputcat.Text = "";
			txtinputcat.Focus();
		}
		private void btnclosered_Click(object sender, EventArgs e)
		{
			pnlcattxt.Visible = false;
			lblcattitlehide.Text = "";
			txtinputcat.Text = "";
			btncat.Text = "Add";
		}
		private void btnaddcat_Click(object sender, EventArgs e)
		{
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "Category";
			lblcattitlehide.Text = "";
			txtinputcat.Focus();
		}
		private void btnaddscat_Click(object sender, EventArgs e)
		{
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "s_cat";
			lblcattitlehide.Text = "";
			txtinputcat.Focus();
		}
		private void btnaddsscat_Click(object sender, EventArgs e)
		{
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "ss_cat";
			lblcattitlehide.Text = "";
			txtinputcat.Focus();
		}
		private void btneditdepart_Click(object sender, EventArgs e)
		{
			try
			{
				listdepart.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From Department List ";
				fm.ShowDialog();
					return;
			}
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "Department";
			lblcattitlehide.Text = listdepart.SelectedItem.ToString();
			txtinputcat.Text = listdepart.SelectedItem.ToString();
			btncat.Text = "Update";
			txtinputcat.Focus();
		}
		private void btneditcat_Click(object sender, EventArgs e)
		{
			try
			{
				listcat.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From 1st Catalog List ";
				fm.ShowDialog();
				return;
			}
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "Category";
			lblcattitlehide.Text = listcat.SelectedItem.ToString();
			txtinputcat.Text = listcat.SelectedItem.ToString();
			btncat.Text = "Update";
			txtinputcat.Focus();
		}
		private void btneditscat_Click(object sender, EventArgs e)
		{
			try
			{
				listscat.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From 2nd Catalog List ";
				fm.ShowDialog();
				return;
			}
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "s_cat";
			lblcattitlehide.Text = listscat.SelectedItem.ToString();
			txtinputcat.Text = listscat.SelectedItem.ToString();
			btncat.Text = "Update";
			txtinputcat.Focus();
		}
		private void btneditsscat_Click(object sender, EventArgs e)
		{
			try
			{
				listsscat.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From 3rd Catalog List ";
				fm.ShowDialog();
				return;
			}
			pnlcattxt.Visible = true;
			pnlcattxt.BringToFront();
			lblcathide.Text = "ss_cat";
			lblcattitlehide.Text = listsscat.SelectedItem.ToString();
			txtinputcat.Text = listsscat.SelectedItem.ToString();
			btncat.Text = "Update";
			txtinputcat.Focus();
		}
		private void btndeldepart_Click(object sender, EventArgs e)
		{
			try
			{
				listdepart.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From Department List ";
				fm.ShowDialog();
				return;
			}
			string selectedItem = listdepart.SelectedItem.ToString();
		   
			if(dst.Tables["checkdepart"] != null)
				dst.Tables["checkdepart"].Clear();
			string sc = " SELECT * FROM code_relations WHERE LOWER(brand) = N'" + listdepart.SelectedItem.ToString().ToLower() + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkdepart") > 0)
				{
					sc += " UPDATE product SET brand = 'Others' WHERE LOWER(brand) =N'" + selectedItem.ToLower() + "' AND code > 1009";
					sc += " UPDATE code_relations SET brand = 'Others' WHERE LOWER(brand) = N'" + selectedItem.ToLower() + "' AND code > 1009";
					sc += " UPDATE catalog SET s_cat ='Others' WHERE LOWER(cat) = N'brands' AND LOWER(s_cat) = N'" + selectedItem.ToLower() + "'";
					sc += " UPDATE printer_mapping SET depart ='Others' WHERE LOWER(depart) = N'" + selectedItem.ToLower() + "'";

					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
				else
				{
					if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButtons.YesNo) != DialogResult.Yes)
						return;
					sc = " DELETE catalog WHERE LOWER(cat) = N'brands' AND LOWER(s_cat) = N'" + listdepart.SelectedItem.ToString().ToLower() + "'";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DoBuildDepartList();
		}
		private void btndelcat_Click(object sender, EventArgs e)
		{
			try
			{
				listcat.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From Catalog List ";
				fm.ShowDialog();
				return;
			}
	 
			if (dst.Tables["checkcat"] != null)
				dst.Tables["checkcat"].Clear();
			string sc = " SELECT * FROM code_relations WHERE LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkcat") > 0)
				{
					sc += " UPDATE product SET cat = 'Others' WHERE LOWER(cat) =N'" + listcat.SelectedItem.ToString().ToLower() + "' AND code > 1009";
					sc += " UPDATE code_relations SET cat = 'Others' WHERE LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "' AND code > 1009";
					sc += " UPDATE catalog SET cat ='Others' WHERE LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "'";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
				else
				{
					if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButtons.YesNo) != DialogResult.Yes)
						return;
					sc += " DELETE catalog WHERE  LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "'";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
		   
			DoBuildCatList();
		}
		private void btndelscat_Click(object sender, EventArgs e)
		{
			try
			{
				listscat.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From Sub Catalog List ";
				fm.ShowDialog();
				return;
			}
		   
			if (dst.Tables["checkcat"] != null)
				dst.Tables["checkcat"].Clear();
			string sc = " SELECT * FROM code_relations WHERE LOWER(s_cat) = N'" + listscat.SelectedItem.ToString().ToLower() + "'";
			sc += " AND LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkcat") > 0)
				{
					sc += " UPDATE product SET s_cat = 'Others' WHERE LOWER(s_cat) =N'" + listscat.SelectedItem.ToString().ToLower() + "' AND code > 1009";
					sc += " UPDATE code_relations SET s_cat = 'Others' WHERE LOWER(s_cat) = N'" + listscat.SelectedItem.ToString().ToLower() + "' AND code > 1009";
					sc += " UPDATE catalog SET s_cat ='Others' WHERE LOWER(s_cat) = N'" + listscat.SelectedItem.ToString().ToLower() + "'";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
				else
				{
					if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButtons.YesNo) != DialogResult.Yes)
						return;
					sc = " DELETE catalog WHERE  LOWER(s_cat) = N'" + listscat.SelectedItem.ToString().ToLower() + "'";
					sc += " AND LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "'";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DoBuildSCatList();
		}
		private void btndelsscat_Click(object sender, EventArgs e)
		{
			try
			{
				listsscat.SelectedItem.ToString();
			}
			catch
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please Select Item From Sub Sub Catalog List ";
				fm.ShowDialog();
				return;
			}
		   
			if (dst.Tables["checkcat"] != null)
				dst.Tables["checkcat"].Clear();
			string sc = " SELECT * FROM code_relations WHERE LOWER(ss_cat) = N'" + listsscat.SelectedItem.ToString().ToLower() + "'";
			sc += " AND LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "'";
			sc += " AND LOWER(s_cat) = N'" + listscat.SelectedItem.ToString().ToLower() + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkcat") > 0)
				{
					sc += " UPDATE product SET ss_cat = 'Others' WHERE LOWER(ss_cat) =N'" + listsscat.SelectedItem.ToString().ToLower() + "' AND code > 1009";
					sc += " UPDATE code_relations SET ss_cat = 'Others' WHERE LOWER(ss_cat) = N'" + listsscat.SelectedItem.ToString().ToLower() + "' AND code > 1009";
					sc += " UPDATE catalog SET ss_cat ='Others' WHERE LOWER(ss_cat) = N'" + listsscat.SelectedItem.ToString().ToLower() + "'";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
				else
				{
					if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Category", MessageBoxButtons.YesNo) != DialogResult.Yes)
						return;
					sc = " DELETE catalog WHERE  LOWER(ss_cat) = N'" + listsscat.SelectedItem.ToString().ToLower() + "'";
					sc += " AND LOWER(cat) = N'" + listcat.SelectedItem.ToString().ToLower() + "'";
					sc += " AND LOWER(s_cat) = N'" + listscat.SelectedItem.ToString().ToLower() + "'";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception ex)
					{
						myConnection.Close();
						Program.ShowExp(sc, ex);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DoBuildSSCatList();
		}
		private void cbdepart_Click(object sender, EventArgs e)
		{
			DoBuildDepart();
		}
		private void btncatdone_Click(object sender, EventArgs e)
		{
			
			pnlcats.Visible = false;
//			pnlproedit.Visible = true;
			btnspeciallist.Visible = true;
			lvitemlist.Visible = true;
//			pnlproedit.BringToFront();
		}
		private void tp1_Click(object sender, EventArgs e)
		{
		}
		private void ShowUserList()
		{
			dgvuser.Rows.Clear();
			int rows = 0;
			if (dst.Tables["userlist"] != null)
				dst.Tables["userlist"].Clear();
			string sc = " SELECT * FROM card WHERE type = 4 and id != 1 ";
			if(textBox2.Text != "")
				sc += " AND (name like N'%" + textBox2.Text + "%' or trading_name like '%" + textBox2.Text + "%' or short_name like '%" + textBox2.Text + "%'or barcode = '" + textBox2.Text + "')";
			
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "userlist");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["userlist"].Rows[i];
				string name = dr["name"].ToString();
				string barcode = dr["barcode"].ToString();
				string phone = dr["phone"].ToString();
				string mobile = dr["mobile"].ToString();
				if(name.Trim().ToLower() == "admin")
					dgvuser.Rows.Add(barcode, name, phone, mobile, "Edit", "");
				else
					dgvuser.Rows.Add(barcode, name, phone, mobile, "Edit", "X");
			}
		}

		private void ShowVipList()
		{
//			dgvuser.Rows.Clear();
			dgvvip.Rows.Clear();
			int rows = 0;
			if (dst.Tables["userlist"] != null)
				dst.Tables["userlist"].Clear();
			string sc = " SELECT * FROM card WHERE type = 6";
			if (textBox3.Text != "")
				sc += " AND (name like N'%" + textBox3.Text + "%' or trading_name like '%" + textBox3.Text + "%' or short_name like '%" + textBox3.Text + "%'or barcode = '" + textBox3.Text + "')";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "userlist");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["userlist"].Rows[i];
				string name = dr["name"].ToString();
				string email = dr["email"].ToString();
				string barcode = dr["barcode"].ToString();
				string phone = dr["phone"].ToString();
				string mobile = dr["mobile"].ToString();
				string id = dr["id"].ToString();
//				dgvuser.Rows.Add(barcode, name, phone, mobile, "Edit", "X");
				dgvvip.Rows.Add(barcode, name, phone, mobile, "Edit", "X", id);
			}
		}
		private void dgvuser_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 4)
			{
				string userBarcode = dgvuser.Rows[e.RowIndex].Cells["cd_barcode"].Value.ToString();
				AddUser au = new AddUser();
				if (btnAdduser.Text == "Add New VIP")
				{
					au.m_bVIP = true;
					au.m_sTitle = "Edit VIP";
				}
				au.m_bUpdate = true;
				au.m_sBarcode = userBarcode;
				au.ShowDialog();
			   
			   
			}
			else if (e.ColumnIndex == 5)
			{
				if (MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;

				if (dgvuser.Rows[e.RowIndex].Cells["cd_name"].Value.ToString().ToLower() == "admin")
				{
					MessageBox.Show("Sorry, Unable to delete master user ");
					return;
				}
				string userBarcode = dgvuser.Rows[e.RowIndex].Cells["cd_barcode"].Value.ToString();
				DoRemoveUser(userBarcode);
			   
			}
			
		   if (btnShowUser.Text == "Show User List")
		   {
			   btnShowUser.Text = "Show User List";
			   btnAdduser.Text = "Add New VIP";
			   ShowVipList();

		   }
		   else
		   {
			   btnShowUser.Text = "Show VIP List";
			   btnAdduser.Text = "Add New User";
			   ShowUserList();
		   }
			  
		}
		private bool DoRemoveUser(string userBar)
		{
			string sc = " DELETE FROM card WHERE barcode ='" + userBar + "'";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return false;
			}
			return true;
		}
		private bool DoSaveGST()
		{
			string sc = " BEGIN TRANSACTION";
			sc += " UPDATE settings SET ";
			sc += " value = '" + tbgstrate.Text + "'";
			sc += " WHERE name = 'gst_rate_percent' ";

			sc += " UPDATE code_relations SET tax_rate = '" + Program.MyDoubleParse(tbgstrate.Text)/100 + "' WHERE code <1020";
			sc += " COMMIT ";
			
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return false;
			}
			return true;
		}
		private bool DoSaveCompanyDetail()
		{
			string sc = " UPDATE card SET ";
			sc += " trading_name = '" + tbtradingname.Text + "'";
			sc += ", ap_name = '" + tbgst.Text + "'";
			sc += ", gst_rate = '" +tbgstrate.Text + "'";
			sc += ", address1 = N'" + tbaddr1.Text + "'";
			sc += ", address2 = N'" + tbaddr2.Text + "'";
			sc += ", address3 = N'" + tbaddr3.Text + "'";
			sc += ", city = N'" + tbcity.Text + "'";
			sc += ", country = N'" + tbcounrty.Text + "'";
			sc += ", phone = '" + tbphone.Text + "'";
			sc += ", fax = '" + tbfax.Text + "'";
			sc += ", email ='" + tbemail.Text + "'";
			sc += ", address2B ='" + tbpostcode.Text + "'";
			sc += ", postal1 = N'" + tbpostageaddress.Text + "'";
			sc += ", postal2 = N'" + tbpostageaddress2.Text + "'";
			sc += ", postal3 = N'" + tbpostageaddress3.Text + "'";
			sc += ", cityB = N'" + tbpostagecity.Text + "'";
			sc += ", countryB = '" + tbpostagecountry.Text + "'";
			sc += ", address1B = '" + tbpostcodenum.Text + "'";
			sc += ", sm_name = N'" + tbmanagername.Text + "'";
			sc += ", sm_email = N'" + tbmanageremail.Text + "'";
			sc += ", sm_ddi = '" + tbmanagerphone.Text + "'";
			sc += ", sm_mobile = '" + tbmanagermobile.Text + "'";
			sc += " WHERE id = 1 ";
			if(m_sBranchId != "")
				sc += " UPDATE branch SET name = N'" + Program.EncodeQuote(txtLocalBranchName.Text) + "' WHERE id = " + m_sBranchId;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return false;
			}
			Program.m_sTradingName = tbtradingname.Text;
			return true;
		}
		private void InsertCompanyID()
		{ 
			string sc="";

			sc += " INSERT INTO card (id) VALUES";
			sc += " (1)";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return;
			}
			return;
		}
		private void DoShowCompanyDetail()
		{
			if (dst.Tables["companydetail"] != null)
				dst.Tables["companydetail"].Clear();
			string sc = " SELECT c.*, b.name AS branch_name ";
			sc += " FROM card c ";
			sc += " LEFT OUTER JOIN branch b ON b.id = c.our_branch ";
			sc += " WHERE c.id = 1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "companydetail") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (dst.Tables["companydetail"].Rows.Count == 1)
			{
				tbtradingname.Text = dst.Tables["companydetail"].Rows[0]["trading_name"].ToString();
				tbgst.Text = dst.Tables["companydetail"].Rows[0]["ap_name"].ToString();
				tbaddr1.Text = dst.Tables["companydetail"].Rows[0]["address1"].ToString();
				tbaddr2.Text = dst.Tables["companydetail"].Rows[0]["address2"].ToString();
				tbaddr3.Text = dst.Tables["companydetail"].Rows[0]["address3"].ToString();
				tbcity.Text = dst.Tables["companydetail"].Rows[0]["city"].ToString();
				tbcounrty.Text = dst.Tables["companydetail"].Rows[0]["country"].ToString();
				tbphone.Text = dst.Tables["companydetail"].Rows[0]["phone"].ToString();
				tbfax.Text = dst.Tables["companydetail"].Rows[0]["fax"].ToString();
				tbemail.Text = dst.Tables["companydetail"].Rows[0]["email"].ToString();

//              tbgstrate.Text = dst.Tables["companydetail"].Rows[0]["gst_rate"].ToString();
				tbgstrate.Text = GetSiteSettings("gst_rate_percent");

				tbpostcode.Text = dst.Tables["companydetail"].Rows[0]["address2B"].ToString();
				tbpostageaddress.Text = dst.Tables["companydetail"].Rows[0]["postal1"].ToString();
				tbpostageaddress2.Text = dst.Tables["companydetail"].Rows[0]["postal2"].ToString();
				tbpostageaddress3.Text = dst.Tables["companydetail"].Rows[0]["postal3"].ToString();
				tbpostcodenum.Text = dst.Tables["companydetail"].Rows[0]["address1B"].ToString();
				tbpostagecity.Text = dst.Tables["companydetail"].Rows[0]["cityB"].ToString();
				tbpostagecountry.Text = dst.Tables["companydetail"].Rows[0]["countryB"].ToString();
				tbmanageremail.Text = dst.Tables["companydetail"].Rows[0]["sm_email"].ToString();
				tbmanagername.Text = dst.Tables["companydetail"].Rows[0]["sm_name"].ToString();
				tbmanagermobile.Text = dst.Tables["companydetail"].Rows[0]["sm_mobile"].ToString();
				tbmanagerphone.Text = dst.Tables["companydetail"].Rows[0]["sm_ddi"].ToString();
				txtLocalBranchName.Text = dst.Tables["companydetail"].Rows[0]["branch_name"].ToString();
				m_sBranchId = dst.Tables["companydetail"].Rows[0]["our_branch"].ToString();
			}
		}
		private void btncompanyapply_Click(object sender, EventArgs e)
		{
			if (DoSaveCompanyDetail() && DoSaveGST())
			{
				DoShowInvoiceHeaderFooter();
			}
			btnUpdateInvoicelayout_Click(sender,e);
			btnCompany_Click(sender,e);
			SaveSettings();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DoShowCompanyDetail();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			//btncompanyapply_Click(sender, e);
			tccompany.Visible = false;
		}
		private void DoShowInvoiceHeaderFooter()
		{
			string header = "";
			string sc = "";
			int nRows =0;

			if (dst.Tables["invoiceheader"] != null)
				dst.Tables["invoiceheader"].Clear();
			sc = " SELECT ssp.text FROM site_sub_pages ssp ";
			sc += " JOIN site_pages sp ON sp.id = ssp.id ";
			sc += " WHERE sp.name ='pos_receipt_header'";
			sc += " AND ssp.inuse = 1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "invoiceheader");
				if (myAdapter.Fill(dst, "invoiceheader") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			header = dst.Tables["invoiceheader"].Rows[0]["text"].ToString();
//			header = header.Replace("\n", "\r\n");

			if (dst.Tables["invoicefooter"] != null)
				dst.Tables["invoicefooter"].Clear();
			sc = " SELECT ssp.text FROM site_sub_pages ssp ";
			sc += " JOIN site_pages sp ON sp.id = ssp.id ";
			sc += " WHERE sp.name ='pos_receipt_footer'";
			sc += " AND ssp.inuse = 1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "invoicefooter") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			string footer = dst.Tables["invoicefooter"].Rows[0]["text"].ToString();

			if (nRows == 0)
			{
				if (dst.Tables["invoicebuildheader"] != null)
					dst.Tables["invoicebuildheader"].Clear();
				sc = " SELECT * FROM card WHERE id =1 ";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "invoicebuildheader") <= 0)
						return;
				}
				catch (Exception ex)
				{
					Program.ShowExp(sc, ex);
					myConnection.Close();
					return;
				}
				header = " [b]Tax Invoice[/b]" + "\r\n";
				header += dst.Tables["invoicebuildheader"].Rows[0]["trading_name"].ToString() + "\r\n";
				if (dst.Tables["invoicebuildheader"].Rows[0]["address1"].ToString().Trim() != "")
					header += dst.Tables["invoicebuildheader"].Rows[0]["address1"].ToString() + "\r\n";
				if (dst.Tables["invoicebuildheader"].Rows[0]["address2"].ToString().Trim() != "")
					header += dst.Tables["invoicebuildheader"].Rows[0]["address2"].ToString() +"\r\n";
				if (dst.Tables["invoicebuildheader"].Rows[0]["address3"].ToString().Trim() != "")
					header += dst.Tables["invoicebuildheader"].Rows[0]["address3"].ToString() + "\r\n";
				if (dst.Tables["invoicebuildheader"].Rows[0]["city"].ToString().Trim() != "")
					header += dst.Tables["invoicebuildheader"].Rows[0]["city"].ToString() + "\r\n";
				if (dst.Tables["invoicebuildheader"].Rows[0]["ap_name"].ToString().Trim() != "")
					header += "GST: " + dst.Tables["invoicebuildheader"].Rows[0]["ap_name"].ToString() + "\r\n" ;
				header += "Phone: " + dst.Tables["invoicebuildheader"].Rows[0]["phone"].ToString()+ "\r\n";
				header += "Sales: @@sales" + "\r\n";
				header += "Inv: @@inv_num" + "\r\n";
				header += "Date: @@date @@time" + "\r\n\r\n";
			}
 
			rtbinvoiceheader.Text = header;
			rtbinvoicefooter.Text = footer;
			
			txtNVRHeader.Text = Program.ReadSitePage("nvr_header");
			string stheader = Program.ReadSitePage("statement_header");
			string stfooter = Program.ReadSitePage("statement_footer");
			rtbStatementHeader.Text = stheader;
			rtbStatementFooter.Text = stfooter;
		}
		private void btnUpdateInvoicelayout_Click(object sender, EventArgs e)
		{
			if(rtbinvoiceheader.Text == "")
				return;
			string sc = "";
			sc += " UPDATE site_sub_pages SET text = N'" + rtbinvoiceheader.Text + "'";
			sc += " WHERE id = (SELECT id FROM site_pages WHERE name ='pos_receipt_header'";
			sc += " AND site_sub_pages.inuse = 1) ";
			sc += " UPDATE site_pages SET text = N'" + rtbinvoiceheader.Text + "'";
			sc += " WHERE name = 'pos_receipt_header'";

			sc += " UPDATE site_sub_pages SET text = N'" + this.txtNVRHeader.Text + "'";
			sc += " WHERE id = (SELECT id FROM site_pages WHERE name ='nvr_header'";
			sc += " AND site_sub_pages.inuse = 1) ";
			sc += " UPDATE site_pages SET text = N'" + txtNVRHeader.Text + "'";
			sc += " WHERE name = 'nvr_header'";

			sc += " UPDATE site_sub_pages SET text = N'" + rtbinvoicefooter.Text + "'";
			sc += " WHERE id = (SELECT id FROM site_pages WHERE name ='pos_receipt_footer'";
			sc += " AND site_sub_pages.inuse = 1)";
			sc += " UPDATE site_pages SET text = N'" + rtbinvoicefooter.Text + "'";
			sc += " WHERE name = 'pos_receipt_footer'";

			sc += " UPDATE site_pages SET text = N'" + rtbStatementHeader.Text + "'";
			sc += " WHERE name ='statement_header'";
			sc += " UPDATE site_pages SET text = N'" + rtbStatementFooter.Text + "'";
			sc += " WHERE name = 'statement_footer'";

			sc += " UPDATE site_sub_pages SET text = N'" + rtbExportFooter.Text + "'";
			sc += " WHERE id = (SELECT id FROM site_pages WHERE name ='pos_export_footer'";
			sc += " AND site_sub_pages.inuse = 1)";
			sc += " UPDATE site_pages SET text = N'" + rtbExportFooter.Text + "'";
			sc += " WHERE name = 'pos_export_footer'";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return;
			}
			FormMSG fm = new FormMSG();
			fm.btnYes.Visible = false;
			fm.btnNo.Visible = false;
			fm.m_sMsg = "Detail Saved";
			fm.ShowDialog();
		}
		private void dtpfrom_ValueChanged(object sender, EventArgs e)
		{
			//lblfrom.Visible = true;
			lblfrom.Text = dtpfrom.Value.ToString("dd/MM/yyyy");
		}
		private void dtpto_ValueChanged(object sender, EventArgs e)
		{
			//lblto.Visible = true;
//			lblto.Text = dtpto.Value.ToString("dd/MM/yyyy 23:59:59");
			lblto.Text = dtpto.Value.ToString("dd/MM/yyyy");
		}
		private void DoSalesReport(int m_nPeriod)
		{
			this.lvSales.Items.Clear();
			int rows = 0;
			string m_dateSql = "";
			if (dst.Tables["salesreport"] != null)
				dst.Tables["salesreport"].Clear();
			if (m_nPeriod == 5)
			{
				if (lblfrom.Text == "From")
				{
					MessageBox.Show("Please select date");
					lblfrom.Text = "                      ";
					//lblfrom.Visible = true;
					lblfrom.BackColor = System.Drawing.Color.Red;
					lblfrom.Size = new Size(200, 20);

					return;
				}
				if (lblto.Text == "To")
				{
					MessageBox.Show("Please select date");
					lblto.Text = "                         ";
					//lblto.Visible = true;
					lblto.BackColor = System.Drawing.Color.Red;
					lblto.Size = new Size(200, 20);
					return;
				}
			}
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 1 ";
					break;
				case 3:
					//DateTime dt = DateTime.Now;
					//int m_nDays = (int)dt.DayOfWeek;
					m_dateSql = " AND DATEDIFF(week, i.commit_date, GETDATE()) = 0"; ;
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, i.commit_date, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND i.commit_date BETWEEN '" + lblfrom.Text + "' AND '"+lblto.Text+" 23:59:59' ";
					break;
				default:
					break;
			}
			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT o.sales AS id, i.sales AS name, count(i.id) AS count, SUM(i.total) AS total ";
//			sc += ", SUM(((s.commit_price * (1 - s.discount_percent/100))- s.supplier_price) * s.quantity) AS profit ";
			sc += " FROM invoice i ";
			sc += " JOIN orders o on o.invoice_number = i.invoice_number ";
//			sc += " JOIN sales s ON s.invoice_number = i.invoice_number ";
			sc += " WHERE 1 = 1 ";
			sc += m_dateSql;
			sc += " group by o.sales, i.sales ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "salesreport");
				if (rows <= 0)
				{
	//				MessageBox.Show("No Record");
					return;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}
			double totalInvQty = 0;
			double dTotalProfit = 0;
			double TotalSalesAmount = 0;
			this.lvSales.Items.Clear();
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["salesreport"].Rows[i];
				string id = dr["id"].ToString();
				string name = dr["name"].ToString();
				string count = dr["count"].ToString();
				//string order_id = dr["order_id"].ToString();
				double totalAmount = Program.MyDoubleParse(dr["total"].ToString());// *1.15;
				double dProfit = 0;//Program.MyDoubleParse(dr["profit"].ToString());// *1.15;

				totalInvQty += Program.MyDoubleParse(count);
				TotalSalesAmount += totalAmount;
				dTotalProfit += dProfit;

				ListViewItem item = new ListViewItem(id);
				item.SubItems.Add(name);
				item.SubItems.Add(count);
				item.SubItems.Add(totalAmount.ToString("c"));
//				item.SubItems.Add(dProfit.ToString("c"));
				lvSales.Items.Add(item);
			}
			ListViewItem sp = new ListViewItem("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			lvSales.Items.Add(sp);

			ListViewItem sum = new ListViewItem("");
			sum.SubItems.Add("");
			sum.SubItems.Add("Total Inv : " + totalInvQty.ToString());
			sum.SubItems.Add("Total Amount : " + TotalSalesAmount.ToString("c"));
//			sum.SubItems.Add("Total Profit : " + dTotalProfit.ToString("c"));
			lvSales.Items.Add(sum);
			sum.Font = new Font("Arial", 9, FontStyle.Bold);
		}
		private void DoDelItemReport(int m_nPeriod)
		{
			this.lvDelReport.Items.Clear();
			int rows = 0;
			string m_dateSql = "";
			if (dst.Tables["DoDelItemReport"] != null)
				dst.Tables["DoDelItemReport"].Clear();
			if (m_nPeriod == 5)
			{
				if (lblfrom.Text == "From")
				{
					MessageBox.Show("Please select date");
					lblfrom.Text = "                      ";
					//lblfrom.Visible = true;
					lblfrom.BackColor = System.Drawing.Color.Red;
					lblfrom.Size = new Size(200, 20);

					return;
				}
				if (lblto.Text == "To")
				{
					MessageBox.Show("Please select date");
					lblto.Text = "                         ";
					//lblto.Visible = true;
					lblto.BackColor = System.Drawing.Color.Red;
					lblto.Size = new Size(200, 20);
					return;
				}
			}
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, di.time, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, di.time, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, di.time, GETDATE()) = 0"; ;
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, di.time, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND di.time BETWEEN '" + lblfrom.Text + "' AND '" + lblto.Text + " 23:59:59' ";
					break;
				default:
					break;
			}
			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT di.*, c.name AS sales_name ";
			sc += " FROM delete_item di ";
			sc += " JOIN card c on di.sales = c.id ";
			sc += " WHERE 1 = 1 ";
			sc += m_dateSql;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "DoDelItemReport");
				if (rows <= 0)
				{
					return;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}
			
			double totalInvQty = 0;
			double dTotalProfit = 0;
			double TotalSalesAmount = 0;
			
			this.lvDelReport.Items.Clear();
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["DoDelItemReport"].Rows[i];
				string code = dr["code"].ToString();
				string name = dr["name"].ToString();
				string till = dr["till_number"].ToString();
				
				string qty = dr["qty"].ToString();
				double totalAmount = Program.MyDoubleParse(dr["amount"].ToString());
				
				string sales = dr["sales_name"].ToString();
				string time = dr["time"].ToString();

				totalInvQty += Program.MyDoubleParse(qty);
				TotalSalesAmount += totalAmount;
	

				ListViewItem item = new ListViewItem(till);
				item.SubItems.Add(code);
				item.SubItems.Add(name);
				item.SubItems.Add(qty);
				item.SubItems.Add(totalAmount.ToString("c"));
				item.SubItems.Add(sales);
				item.SubItems.Add(time);
				lvDelReport.Items.Add(item);
			}
			ListViewItem sp = new ListViewItem("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			lvDelReport.Items.Add(sp);

			ListViewItem sum = new ListViewItem("");
			sum.SubItems.Add("");

			sum.SubItems.Add("Total Qty : " + totalInvQty.ToString());
			sum.SubItems.Add("Total Amount : " + TotalSalesAmount.ToString("c"));
			//			sum.SubItems.Add("Total Profit : " + dTotalProfit.ToString("c"));
			lvDelReport.Items.Add(sum);
			sum.Font = new Font("Arial", 9, FontStyle.Bold);
		}
		
		private void DoPaymentReportExport()
		{
			this.lvrpayment.Items.Clear();
//			payment_report_case = dateRang;
			lblcash.Text = "Cash";
			lbleftpos.Text = "Eftpos";
			lblcc.Text = "Credit card";
			lblaccount.Text = "Account";
			lblCheque.Text = "Cheque";
			lblcashout.Text = "Cashout";
			lbltotalinvoice.Text = "Total Invoice";
			lblaverage.Text = "Average";
			lbltotalAmount.Text = "Total Amount";
			lblcashacc.Text = "";
			lblchequeacc.Text = "";
			lblaccpaid.Text = "";
			lblccacc.Text = "";
			lbleftposacc.Text = "";
			
			int rows = dst.Tables["paymentreport"].Rows.Count;
			double dTotal = 0;
			double dCash = 0;
			double dCashOut = 0;
			double dEftpos = 0;
			double dCC = 0;
			double dCredit = 0;
			double dCharge = 0;
			double dCheque = 0;
			double dDirectDebit = 0;
			double dRounding = 0;
			double dTips = 0;
			double dCashTotal = 0;
			double dCashOutTotal = 0;
			double dEftposTotal = 0;
			double dCCTotal = 0;
			double dChequeTotal = 0;
			double dChargeTotal = 0;
			double dRoundingTotal = 0;
			double dTipsTotal = 0;
			double dSum = 0;
			double dPaymentTotal = 0;
			double dCreditTotal = 0;
			double dDirectDebitTotal = 0;
			string sales = "";
			string sdate = "";
			string oldsales = "";
			string inv_old = "";

			this.lvrpayment.Items.Clear();
			int c = 0;
			string station = "";
			this.lvrpayment.CheckBoxes = true;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["paymentreport"].Rows[i];
				string invoice = dr["invoice_number"].ToString();
				sales = dr["sales"].ToString();

				if (invoice != inv_old)
				{
					if (inv_old != "")
					{
						if (dCash != 0 && dEftpos == 0 && dCC == 0 && dCheque == 0)
							dTotal = Math.Round(dTotal, 1);
						dPaymentTotal = dCash + dEftpos + dCC + dCheque + dCharge - dCashOut;
						dPaymentTotal = Math.Round(dPaymentTotal, 2);

	//					if (i < 100)
						{
							if (dCharge != 0)
							{
								ListViewItem lvi = new ListViewItem("Exclusion");
								lvi.SubItems.Add(station);
								lvi.SubItems.Add(inv_old);
								lvi.SubItems.Add(sdate);
								lvi.SubItems.Add(oldsales);
								lvi.SubItems.Add(dTotal.ToString("c"));
								lvi.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c"));
								lvi.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c"));
								lvi.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c"));
								lvi.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c"));
								lvi.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c"));
								lvi.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c"));
								lvi.SubItems.Add(dPaymentTotal.ToString("c"));
								this.lvrpayment.Items.Add(lvi);
								if (dPaymentTotal != dTotal)
									lvi.BackColor = System.Drawing.Color.Red;
								else
								{
									lvi.BackColor = System.Drawing.Color.White;
									lvi.ForeColor = System.Drawing.Color.Red;
								}
							}
							else
							{
								ListViewItem lvi = new ListViewItem("");
								lvi.SubItems.Add(station);
								lvi.SubItems.Add(inv_old);
								lvi.SubItems.Add(sdate);
								lvi.SubItems.Add(oldsales);
								lvi.SubItems.Add(dTotal.ToString("c"));
								lvi.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c"));
								lvi.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c"));
								lvi.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c"));
								lvi.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c"));
								lvi.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c"));
								lvi.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c"));
								lvi.SubItems.Add(dPaymentTotal.ToString("c"));
								this.lvrpayment.Items.Add(lvi);
								if (dPaymentTotal != dTotal)
									lvi.BackColor = System.Drawing.Color.Red;
								else
									lvi.BackColor = System.Drawing.Color.White;
							}
						}
					}
					dCashTotal += dCash;
					dCashOutTotal += dCashOut;
					dEftposTotal += dEftpos;
					dCCTotal += dCC;
					dChequeTotal += dCheque;
					dDirectDebitTotal += dDirectDebit;
					dCreditTotal += dCredit;
					dChargeTotal += dCharge;
					dRoundingTotal += dRounding;
					dTipsTotal = dTips;

					dSum += dTotal;

					dCash = 0;
					dCashOut = 0;
					dEftpos = 0;
					dCC = 0;
					dCheque = 0;
					dCredit = 0;
					dCharge = 0;
					dRounding = 0;
					dTips = 0;
					inv_old = invoice;
					if (oldsales != sales)
					{
						oldsales = sales;
					}
					c++;
				}

				string date = DateTime.Parse(dr["trans_date"].ToString()).ToString(); //.ToString("dd-MM-yyyy");

				string invoice_total = dr["total"].ToString();
				string payment_type = dr["name"].ToString().ToUpper();
				string payment_eid = dr["eid"].ToString();
				station = dr["station_id"].ToString();
				double dAmount = Program.MyDoubleParse(dr["amount"].ToString());
				dTotal = Program.MyDoubleParse(dr["total"].ToString());
				double bindTotal = 0;
				sdate = date;
				m_sSales = sales;

				if (dAmount != dTotal)
				{
					if (payment_type != "CREDIT APPLLY")
						bindTotal = dAmount;
					else
						bindTotal = dTotal;
				}
				else
					bindTotal = dTotal;
				if (payment_eid == "1")
				{
					if (dAmount < 0 && dTotal >= 0)
						dCashOut -= dAmount;
					else
						dCash += bindTotal;
				}
				else if (payment_eid == "2")
					dCheque += bindTotal;
				else if (payment_eid == "3")
					dCC += bindTotal;
				else if (payment_eid == "6")
					dEftpos += bindTotal;
				else if (payment_eid == "10")
					dCashOut += bindTotal;
				else if (payment_eid == "11")
					dCharge += bindTotal;
				else if (payment_eid == "12")
					dRounding = bindTotal;
				else if (payment_eid == "13")
					dTips = bindTotal;
			}
			dCashTotal += dCash;
			dCashOutTotal += dCashOut;
			dEftposTotal += dEftpos;
			dCCTotal += dCC;
			dChequeTotal += dCheque;
			dDirectDebitTotal += dDirectDebit;
			dCreditTotal += dCredit;
			dChargeTotal += dCharge;
			dRoundingTotal += dRounding;
			dTipsTotal = dTips;
			dPaymentTotal = dCash + dEftpos + dCC + dCheque + dCharge - dCashOut;
			dPaymentTotal = Math.Round(dPaymentTotal, 2);
			if (dCharge != 0)
			{
				ListViewItem lvis = new ListViewItem("Exclusion");
				lvis.SubItems.Add(station);
				lvis.SubItems.Add(inv_old);
				lvis.SubItems.Add(sdate);
				lvis.SubItems.Add(oldsales);
				lvis.SubItems.Add(dTotal.ToString("c").Replace(",",""));
				lvis.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dPaymentTotal.ToString("c").Replace(",", ""));
				this.lvrpayment.Items.Add(lvis);
				if (dPaymentTotal != dTotal)
					lvis.BackColor = System.Drawing.Color.Red;
				else
				{
					lvis.BackColor = System.Drawing.Color.White;
					lvis.ForeColor = System.Drawing.Color.Red;
				}
			}
			else
			{
				ListViewItem lvis = new ListViewItem("");
				lvis.SubItems.Add(station);
				lvis.SubItems.Add(inv_old);
				lvis.SubItems.Add(sdate);
				lvis.SubItems.Add(oldsales);
				lvis.SubItems.Add(dTotal.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c").Replace(",", ""));
				lvis.SubItems.Add(dPaymentTotal.ToString("c"));
				this.lvrpayment.Items.Add(lvis);
				if (dPaymentTotal != dTotal)
					lvis.BackColor = System.Drawing.Color.Red;
				else
					lvis.BackColor = System.Drawing.Color.White;
			}

			dSum += dTotal;

			ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("TOTAL ");
			sum.SubItems.Add("TRANS");
			sum.SubItems.Add(c.ToString());
			sum.SubItems.Add(dSum.ToString("c").Replace(",", ""));
			sum.SubItems.Add(dCashTotal.ToString("c").Replace(",", ""));
			sum.SubItems.Add(dEftposTotal.ToString("c").Replace(",", ""));
			sum.SubItems.Add(dCCTotal.ToString("c").Replace(",", ""));
			sum.SubItems.Add(dChargeTotal.ToString("c").Replace(",", ""));
			sum.SubItems.Add(dChequeTotal.ToString("c").Replace(",", ""));
			sum.SubItems.Add(dCashOutTotal.ToString("c").Replace(",", ""));
			sum.SubItems.Add((dCashTotal + dEftposTotal + dCCTotal + dChargeTotal + dChequeTotal - dCashOutTotal).ToString("c").Replace(",", ""));
			this.lvrpayment.Items.Add(sum);
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.SteelBlue;
			 
			DoPamentReportSummary(c, payment_report_case, dCashTotal, dEftposTotal, dChequeTotal, dCCTotal, dChargeTotal, dCashOutTotal, dRoundingTotal, dTipsTotal);
			lbltotalinvoice.Text = c.ToString();
			print_trans = c;
		}
		
		private void DoPaymentReport(int dateRang)
		{
			this.lvrpayment.Items.Clear();
			payment_report_case = dateRang;
			lblcash.Text = "Cash";
			lbleftpos.Text = "Eftpos";
			lblcc.Text = "Credit card";
			lblaccount.Text = "Account";
			lblCheque.Text = "Cheque";
			lblcashout.Text = "Cashout";
			lbltotalinvoice.Text = "Total Invoice";
			lblaverage.Text = "Average";
			lbltotalAmount.Text = "Total Amount";
			lblcashacc.Text = "";
			lblchequeacc.Text = "";
			lblaccpaid.Text = "";
			lblccacc.Text = "";
			lbleftposacc.Text = "";

			string m_dateSql = "";
			switch (dateRang)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, d.trans_date, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, d.trans_date, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, d.trans_date, GETDATE()) = 0 ";
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, d.trans_date, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND d.trans_date BETWEEN '" + lblfrom.Text + "' AND '"+lblto.Text+" 23:59:59' ";
					break;
			}

			int rows = 0;
			if (dst.Tables["paymentreport"] != null)
				dst.Tables["paymentreport"].Clear();
			string sc = " SET DATEFORMAT dmy ";
//			sc += " UPDATE invoice SET station_id = (SELECT TOP 1 station_id FROM orders WHERE invoice_number = invoice.invoice_number) WHERE station_id IS NULL ";
			sc += " SELECT DISTINCT c.id AS custID, c.company, c.name AS Customer, isnull(d.trans_date, i.commit_date) AS trans_date ";
			sc += ", i.commit_date AS invoice_date, t.amount, t.amount AS amount2, e.name, i.invoice_number, i.total, e.id as eid ";
			sc += ", i.sales, i.station_id ";
			sc += " FROM tran_detail d ";
			sc += " LEFT OUTER JOIN invoice i ON i.invoice_number = d.invoice_number ";
			sc += " LEFT OUTER JOIN trans t ON t.id = d.id ";
			sc += " LEFT OUTER JOIN enum e ON e.id = d.payment_method AND e.class = 'payment_method' ";
			sc += " LEFT OUTER JOIN card c ON c.id = i.card_id ";
//			sc += " LEFT OUTER JOIN orders o ON o.invoice_number = i.invoice_number ";
			sc += " WHERE 1=1 ";
			sc += m_dateSql;
			
			sc += " UNION ";
			sc += " SELECT DISTINCT c.id AS custID, c.company, c.name AS Customer, i.commit_date AS trans_date, i.commit_date AS invoice_date ";
			sc += ", 0 AS amount, 0 AS amount2, '' AS name, i.invoice_number, i.total, 0 as eid ";
			sc += " , i.sales, i.station_id ";
			sc += " FROM invoice i ";
			sc += " LEFT OUTER JOIN card c ON c.id = i.card_id ";
			sc += " WHERE 1 = 1 ";
			sc += " AND i.amount_paid = 0 ";
			sc += m_dateSql.Replace("d.trans_date", "i.commit_date");
			sc += " ORDER BY trans_date";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "paymentreport");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			
			if(rows > 100)
			{
				MessageBox.Show("Payment Report : too much data, displaying top 100, please use export to see all records.");
			}

			double dTotal = 0;
			double dCash = 0;
			double dCashOut = 0;
			double dEftpos = 0;
			double dCC = 0;
			double dCredit = 0;
			double dCharge = 0;
			double dCheque = 0;
			double dDirectDebit = 0;
			double dRounding = 0;
			double dTips = 0;
			double dCashTotal = 0;
			double dCashOutTotal = 0;
			double dEftposTotal = 0;
			double dCCTotal = 0;
			double dChequeTotal = 0;
			double dChargeTotal = 0;
			double dRoundingTotal = 0;
			double dTipsTotal = 0;
			double dSum = 0;
			double dPaymentTotal = 0;
			double dCreditTotal = 0;
			double dDirectDebitTotal = 0;
			string sales = "";
			string sdate = "";
			string oldsales = "";
			string inv_old = "";
		 
			this.lvrpayment.Items.Clear();
			int c = 0;
			string station = "";
			this.lvrpayment.CheckBoxes = true;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["paymentreport"].Rows[i];
				string invoice = dr["invoice_number"].ToString();
				sales = dr["sales"].ToString();
			   
				if (invoice != inv_old)
				{
					if (inv_old != "")
					{
						if (dCash != 0 && dEftpos == 0 && dCC == 0 && dCheque == 0)
							dTotal = Math.Round(dTotal, 1);
                        //dPaymentTotal = dCash + dEftpos + dCC + dCheque + dCharge - dCashOut;
                        dPaymentTotal = dCash + dEftpos + dCC + dCheque - dCashOut; //+ dCharge
						dPaymentTotal = Math.Round(dPaymentTotal, 2);
						
						if(i < 100)
						{
							if (dCharge != 0)
							{
								ListViewItem lvi = new ListViewItem("Exclusion");
								lvi.SubItems.Add(station);
								lvi.SubItems.Add(inv_old);
								lvi.SubItems.Add(sdate);
								lvi.SubItems.Add(oldsales);
								lvi.SubItems.Add(dTotal.ToString("c"));
								lvi.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c"));
								lvi.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c"));
								lvi.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c"));
								lvi.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c"));
								lvi.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c"));
								lvi.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c"));
								lvi.SubItems.Add(dPaymentTotal.ToString("c"));
								this.lvrpayment.Items.Add(lvi);
								if (dPaymentTotal != dTotal)
									lvi.BackColor = System.Drawing.Color.Red;
								else
								{
									lvi.BackColor = System.Drawing.Color.White;
									lvi.ForeColor = System.Drawing.Color.Red;
								}
							}
							else
							{
								ListViewItem lvi = new ListViewItem("");
								lvi.SubItems.Add(station);
								lvi.SubItems.Add(inv_old);
								lvi.SubItems.Add(sdate);
								lvi.SubItems.Add(oldsales);
								lvi.SubItems.Add(dTotal.ToString("c"));
								lvi.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c"));
								lvi.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c"));
								lvi.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c"));
								lvi.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c"));
								lvi.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c"));
								lvi.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c"));
								lvi.SubItems.Add(dPaymentTotal.ToString("c"));
								this.lvrpayment.Items.Add(lvi);
								if (dPaymentTotal != dTotal)
									lvi.BackColor = System.Drawing.Color.Red;
								else
									lvi.BackColor = System.Drawing.Color.White;
							}
						}
					}
					dCashTotal += dCash;
					dCashOutTotal += dCashOut;
					dEftposTotal += dEftpos;
					dCCTotal += dCC;
					dChequeTotal += dCheque;
					dDirectDebitTotal += dDirectDebit;
					dCreditTotal += dCredit;
					dChargeTotal += dCharge;
					dRoundingTotal += dRounding;
					dTipsTotal = dTips;

					dSum += dTotal;

					dCash = 0;
					dCashOut = 0;
					dEftpos = 0;
					dCC = 0;
					dCheque = 0;
					dCredit = 0;
					dCharge = 0;
					dRounding = 0;
					dTips = 0;
					inv_old = invoice;
					if (oldsales != sales)
					{
						oldsales = sales;
					}
					c++;
				}

				string date = DateTime.Parse(dr["trans_date"].ToString()).ToString(); //.ToString("dd-MM-yyyy");
			   
				string invoice_total = dr["total"].ToString();
				string payment_type = dr["name"].ToString().ToUpper();
                if (payment_type == "" || payment_type == null)
                    payment_type = "charge";
				string payment_eid = dr["eid"].ToString();
                if (payment_eid == null || payment_eid == "" || payment_eid == "0")
                    payment_eid = "11";
				station = dr["station_id"].ToString();
				double dAmount = Program.MyDoubleParse(dr["amount"].ToString());
				dTotal = Program.MyDoubleParse(dr["total"].ToString());
				double bindTotal = 0;
				sdate = date;
				m_sSales = sales;

				if (dAmount != dTotal)
				{
					if (payment_type != "CREDIT APPLLY" && payment_type != "charge")
						bindTotal = dAmount;
					else
						bindTotal = dTotal;
				}
				else
					bindTotal = dTotal;
				if (payment_eid == "1")
				{
					if (dAmount < 0 && dTotal >= 0)
						dCashOut -= dAmount;
					else
						dCash += bindTotal;
				}
				else if (payment_eid == "2")
					dCheque += bindTotal;
				else if (payment_eid == "3")
					dCC += bindTotal;
				else if (payment_eid == "6")
					dEftpos += bindTotal;       
				else if (payment_eid == "10")
					dCashOut += bindTotal;
				else if (payment_eid == "11")
					dCharge += bindTotal;
				else if (payment_eid == "12")
					dRounding = bindTotal;
				else if (payment_eid == "13")
					dTips = bindTotal;
			}
			dCashTotal += dCash;
			dCashOutTotal += dCashOut;
			dEftposTotal += dEftpos;
			dCCTotal += dCC;
			dChequeTotal += dCheque;
			dDirectDebitTotal += dDirectDebit;
			dCreditTotal += dCredit;
			dChargeTotal += dCharge;
			dRoundingTotal += dRounding;
			dTipsTotal = dTips;
            //dPaymentTotal = dCash + dEftpos + dCC + dCheque + dCharge - dCashOut;
            dPaymentTotal = dCash + dEftpos + dCC + dCheque - dCashOut; //+ dCharge
			dPaymentTotal = Math.Round(dPaymentTotal, 2);
			if (dCharge != 0)
			{
				ListViewItem lvis = new ListViewItem("Exclusion");
				lvis.SubItems.Add(station);
				lvis.SubItems.Add(inv_old);
				lvis.SubItems.Add(sdate);
				lvis.SubItems.Add(oldsales);
				lvis.SubItems.Add(dTotal.ToString("c"));
				lvis.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c"));
				lvis.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c"));
				lvis.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c"));
				lvis.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c"));
				lvis.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c"));
				lvis.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c"));
				lvis.SubItems.Add(dPaymentTotal.ToString("c"));
				this.lvrpayment.Items.Add(lvis);
				if (dPaymentTotal != dTotal)
					lvis.BackColor = System.Drawing.Color.Red;
				else
				{
					lvis.BackColor = System.Drawing.Color.White;
					lvis.ForeColor = System.Drawing.Color.Red;
				}
			}
			else
			{
				ListViewItem lvis = new ListViewItem("");
				lvis.SubItems.Add(station);
				lvis.SubItems.Add(inv_old);
				lvis.SubItems.Add(sdate);
				lvis.SubItems.Add(oldsales);
				lvis.SubItems.Add(dTotal.ToString("c"));
				lvis.SubItems.Add(dCash == 0 ? "" : dCash.ToString("c"));
				lvis.SubItems.Add(dEftpos == 0 ? "" : dEftpos.ToString("c"));
				lvis.SubItems.Add(dCC == 0 ? "" : dCC.ToString("c"));
				lvis.SubItems.Add(dCharge == 0 ? "" : dCharge.ToString("c"));
				lvis.SubItems.Add(dCheque == 0 ? "" : dCheque.ToString("c"));
				lvis.SubItems.Add(dCashOut == 0 ? "" : dCashOut.ToString("c"));
				lvis.SubItems.Add(dPaymentTotal.ToString("c"));
				this.lvrpayment.Items.Add(lvis);
				if (dPaymentTotal != dTotal)
					lvis.BackColor = System.Drawing.Color.Red;
				else
					lvis.BackColor = System.Drawing.Color.White;
			}
  
			dSum += dTotal;
/*
			ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("TOTAL ");
			sum.SubItems.Add("TRANS");
			sum.SubItems.Add(c.ToString());
		  
			sum.SubItems.Add(dSum.ToString("c"));
			sum.SubItems.Add(dCashTotal.ToString("c"));
			sum.SubItems.Add(dEftposTotal.ToString("c"));
			sum.SubItems.Add(dCCTotal.ToString("c"));
			sum.SubItems.Add(dChargeTotal.ToString("c"));
			sum.SubItems.Add(dChequeTotal.ToString("c"));
			sum.SubItems.Add(dCashOutTotal.ToString("c"));
			sum.SubItems.Add((dCashTotal + dEftposTotal + dCCTotal + dChargeTotal + dChequeTotal - dCashOutTotal).ToString("c"));
			this.lvrpayment.Items.Add(sum);
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.SteelBlue;
 */ 
			DoPamentReportSummary(c, payment_report_case, dCashTotal, dEftposTotal, dChequeTotal, dCCTotal, dChargeTotal, dCashOutTotal, dRoundingTotal, dTipsTotal);
			lbltotalinvoice.Text = c.ToString();
			print_trans = c;
		}
		private void DoPamentReportSummary(int trans_num, int dateRange, double dCash, double dEftpos, double dCheque, double dCC, double dCharge, double dCashout, double dRounding, double dTips)
		{
			string sc = "";
			string inv_num = "";
			int rows = 0;
/*			double dCash = 0;
			double dEftpos = 0;
			double dCheque = 0;
			double dCC = 0;
			double dCharge = 0;
			double dCashout = 0;
			double dRounding = 0;
			double dTips = 0;
*/ 
			double dCashAccount = 0;
			double dEftposAccount = 0;
			double dChequeAccount = 0;
			double dCCAccount = 0;
/*			if (lvrpayment.Items.Count - 1 > 0)
			{
				inv_num = " d.invoice_number = '" + lvrpayment.Items[0].SubItems[2].Text + "'";
				//inv_num += " OR d.invoice_number = '" + lvrpayment.Items[0].SubItems[2].Text + ",'";
				for (int i = 1; i < lvrpayment.Items.Count - 1; i++)
				{
					inv_num += " OR d.invoice_number = '" + lvrpayment.Items[i].SubItems[2].Text + "'";
					//inv_num += " OR d.invoice_number = '" + lvrpayment.Items[i].SubItems[2].Text + ",'";
				}
			}
			
			if (dst.Tables["payment_summary"] != null)
				dst.Tables["payment_summary"].Clear();
			
			sc = " SELECT e.name AS payment_method, SUM(t.amount) AS amount";
			sc += " FROM trans t JOIN tran_detail d ON d.id = t.id";
			sc += " JOIN enum e ON e.id = d.payment_method AND e.class ='payment_method'";
			sc += " WHERE 1 = 1 AND " + inv_num;
			sc += " GROUP by e.name ";
			sc += " OPTION(MERGE JOIN) ";

			//MessageBox.Show(sc);
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "payment_summary");
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}
			for (int i = 0; i < dst.Tables["payment_summary"].Rows.Count; i++)
			{
				DataRow dr = dst.Tables["payment_summary"].Rows[i];
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
				else if (pm == "charge")
					dCharge = dAmount;
				else if (pm == "cash out")
					dCashout = dAmount;
				else if (pm == "cash rounding")
					dRounding = dAmount;
				else if (pm == "tips")
					dTips = dAmount;
			}
*/			
			if (dst.Tables["payment_summary_account"] != null)
				dst.Tables["payment_summary_account"].Clear();
			string m_dateSql = "";
			sc = " SET DATEFORMAT dmy";
			sc += " SELECT payment_method, amount FROM account_payment";
			sc += " WHERE 1=1 ";
			switch (dateRange)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, recorded, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, recorded, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, recorded, GETDATE()) = 0 ";
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, recorded, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND recorded BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
			}
			sc += m_dateSql;
			//sc += " GROUP by payment_method ";
			sc += " ORDER BY recorded";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "payment_summary_account");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (rows > 0)
			{
				for (int i = 0; i < dst.Tables["payment_summary_account"].Rows.Count; i++)
				{
					DataRow dr = dst.Tables["payment_summary_account"].Rows[i];
					string pm = dr["payment_method"].ToString().ToLower();
					double dAmount = Program.MyDoubleParse(dr["amount"].ToString());
					if (pm == "cash")
						dCashAccount += dAmount;
					else if (pm == "eftpos")
						dEftposAccount += dAmount;
					else if (pm == "cheque")
						dChequeAccount += dAmount;
					else if (pm == "credit card")
						dCCAccount += dAmount;
				}
			}

			if (dCashAccount != 0)
				lblcashacc.Text = "Acc paid: " + dCashAccount.ToString("c");
			if (dEftposAccount != 0)
				lbleftposacc.Text = "Acc paid: " + dEftposAccount.ToString("c");
			if (dChequeAccount != 0)
				lblchequeacc.Text = "Acc paid: " + dChequeAccount.ToString("c");
			if (dCCAccount != 0)
				lblccacc.Text = "Acc paid: " + dCCAccount.ToString("c");
			
            //lbltotalAmount.Text = (dCash + dEftpos + dCC + dCheque + dCashAccount + dEftposAccount + dCCAccount + dChequeAccount - dCashout).ToString("c");
            //lblaverage.Text = (((dCash + dEftpos + dCC + dCheque + dCashAccount + dEftposAccount + dCCAccount + dChequeAccount - dCashout)) / trans_num).ToString("c");
            lbltotalAmount.Text = (dCash + dEftpos + dCC + dCheque + dCharge + dCashAccount + dEftposAccount + dCCAccount + dChequeAccount - dCashout).ToString("c");
            lblaverage.Text = (((dCash + dEftpos + dCC + dCheque + dCharge + dCashAccount + dEftposAccount + dCCAccount + dChequeAccount - dCashout)) / trans_num).ToString("c");
			lblaccpaid.Text = "Paid: " + (dCashAccount + dEftposAccount + dChequeAccount + dCCAccount).ToString("c");
			lblcash.Text = dCash.ToString("c");
			lbleftpos.Text = dEftpos.ToString("c");
			lblcc.Text = dCC.ToString("c");
			lblaccount.Text = dCharge.ToString("c");
			lblcashout.Text = dCashout.ToString("c");
			lblCheque.Text = dCheque.ToString("c");
			print_cash = dCash + dCashAccount;
			print_eftpos = dEftpos + dEftposAccount;
			print_cc = dCC + dCCAccount;
			print_cheque = dCheque + dChequeAccount;
			print_account = dCharge;
			print_account_paid = dCashAccount + dEftposAccount + dChequeAccount + dCCAccount;
			print_cashout = dCashout;
			print_rounding = dRounding;
		}
		private void btnreport1_Click(object sender, EventArgs e)
		{
			panelreports.BringToFront();
			if (panelreports.Visible)
				panelreports.Visible = false;
			else
				panelreports.Visible = true;
			tccompany.Visible = false;
			lblfrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
//			lblto.Text = DateTime.Now.ToString("dd-MM-yyyy 23:59:59");
			lblto.Text = DateTime.Now.ToString("dd-MM-yyyy");
		}
		private void btnnow_Click(object sender, EventArgs e)
		{
			DoPaymentReport(1);
			DoItemSummary(1);
			DoVipReport(1);
			DoSalesReport(1);
			showTimeSheetSum(1);
			showTimeSheetDetail(1);
			showStockReportSum(1);
			DoDelItemReport(1);
			m_iDateSelect = 1;
		}
		private void btnyesterday_Click(object sender, EventArgs e)
		{
			DoPaymentReport(2);
			DoItemSummary(2);
			DoVipReport(2);
			DoSalesReport(2);
			showTimeSheetSum(2);
			showTimeSheetDetail(2);
			showStockReportSum(2);
			DoDelItemReport(2);
			m_iDateSelect = 2;
		}
		private void btnthisweek_Click(object sender, EventArgs e)
		{
			DoPaymentReport(3);
			DoItemSummary(3);
			DoVipReport(3);
			DoSalesReport(3);
			showTimeSheetSum(3);
			showTimeSheetDetail(3);
			showStockReportSum(3);
			DoDelItemReport(3);
			m_iDateSelect = 3;
		}
		private void btnmonth_Click(object sender, EventArgs e)
		{
			DoPaymentReport(4);
			DoItemSummary(4);
			DoVipReport(4);
			DoSalesReport(4);
			showTimeSheetSum(4);
			showTimeSheetDetail(4);
			showStockReportSum(4);
			DoDelItemReport(4);
			m_iDateSelect = 4;
		}
		private void btnview_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor; 
			DoPaymentReport(5);
			DoItemSummary(5);
			DoCatSummary(5);
			DoVipReport(5);
			DoSalesReport(5);
			showTimeSheetSum(5);
			showTimeSheetDetail(5);
			showStockReportSum(5);
			DoDelItemReport(5);
			m_iDateSelect = 5;
			Cursor.Current = Cursors.Default;
		}
		private void DoVipReport(int m_nPeriod)
		{
			this.lvVipsum.Items.Clear();
			this.listView1.Items.Clear();
			this.listView2.Items.Clear();
			int rows = 0;
			string m_dateSql = "";
			if (dst.Tables["vipreport"] != null)
				dst.Tables["vipreport"].Clear();
			if (m_nPeriod == 5)
			{
				if (lblfrom.Text == "From")
				{
					MessageBox.Show("Please select date");
					lblfrom.Text = "                      ";
					//lblfrom.Visible = true;
					lblfrom.BackColor = System.Drawing.Color.Red;
					lblfrom.Size = new Size(200, 20);

					return;
				}
				if (lblto.Text == "To")
				{
					MessageBox.Show("Please select date");
					lblto.Text = "                         ";
					//lblto.Visible = true;
					lblto.BackColor = System.Drawing.Color.Red;
					lblto.Size = new Size(200, 20);
					return;
				}


			}
			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT i.card_id as id, c.name , count(*) as count, sum(i.total) as total ";
			sc += " FROM invoice i ";
			sc += " LEFT OUTER JOIN card c on c.id = i.card_id ";
			sc += " WHERE 1=1 ";
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 1 ";
					break;
				case 3:
					//DateTime dt = DateTime.Now;
					//int m_nDays = (int)dt.DayOfWeek;
					m_dateSql = " AND DATEDIFF(week, i.commit_date, GETDATE()) = 0"; ;
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, i.commit_date, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND i.commit_date BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;
			sc += " group by i.card_id, c.name ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "vipreport");
				if (rows <= 0)
				{
//					MessageBox.Show("No Record");
					return;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}
			double totalInvQty = 0;
			double TotalSalesAmount = 0;
			this.lvVipsum.Items.Clear();
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["vipreport"].Rows[i];
				string id = dr["id"].ToString();
				string name = dr["name"].ToString();
				string count = dr["count"].ToString();
				//string order_id = dr["order_id"].ToString();
				double totalAmount = Program.MyDoubleParse(dr["total"].ToString());// *1.15;

				totalInvQty += Program.MyDoubleParse(count);
				TotalSalesAmount += totalAmount;

				ListViewItem item = new ListViewItem(id);
				item.SubItems.Add(name);
				item.SubItems.Add(count);
				item.SubItems.Add(totalAmount.ToString("c"));
				//item.SubItems.Add(order_id);
				lvVipsum.Items.Add(item);
			}
			ListViewItem sp = new ListViewItem("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			lvVipsum.Items.Add(sp);

			ListViewItem sum = new ListViewItem("");
			sum.SubItems.Add("");
			sum.SubItems.Add("Total Inv : " + totalInvQty.ToString());
			sum.SubItems.Add("Total Amount : " + TotalSalesAmount.ToString("c"));
			//sum.SubItems.Add("");
			lvVipsum.Items.Add(sum);
			sum.Font = new Font("Arial", 9, FontStyle.Bold);
		}
		private void DoVipCatSummary(int m_nPeriod)
		{
			this.listView2.Items.Clear();
			int rows = 0;
			string m_dateSql = "";
			if (dst.Tables["DoVipCatSummary"] != null)
				dst.Tables["DoVipCatSummary"].Clear();
			if (m_nPeriod == 5)
			{
				if (lblfrom.Text == "From")
				{
					MessageBox.Show("Please select date");
					lblfrom.Text = "                      ";
					lblfrom.BackColor = System.Drawing.Color.Red;
					lblfrom.Size = new Size(200, 20);
					return;
				}
				if (lblto.Text == "To")
				{
					MessageBox.Show("Please select date");
					lblto.Text = "                         ";
					lblto.BackColor = System.Drawing.Color.Red;
					lblto.Size = new Size(200, 20);
					return;
				}
			}

			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT  c.cat, SUM(s.sales_total) AS sales_amount ";
			sc += ", sum(((s.commit_price * (1-s.discount_percent/100))- s.supplier_price) * s.quantity) AS rough_profit ";
			sc += ", sum(s.supplier_price * s.quantity) AS supplier_price, sum(s.quantity) AS sales_qty ";
			sc += ", SUM(s.sales_total - s.sales_total / (1 + s.tax_rate)) AS tax ";
			sc += " FROM sales s ";
			sc += " JOIN invoice i ON i.invoice_number = s.invoice_number ";
			sc += " JOIN code_relations c ON c.code = s.code";
			sc += " JOIN orders o ON o.invoice_number = s.invoice_number ";
			sc += " WHERE 1=1 ";
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, i.commit_date, GETDATE()) = 0"; ;
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, i.commit_date, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND i.commit_date BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;
			if (m_sVip_id != "")
				sc += " AND i.card_id = " + m_sVip_id;
			sc += " GROUP BY c.cat ";
			sc += " ORDER BY sum(s.quantity) DESC ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "DoVipCatSummary");
				if (rows <= 0)
				{
					return;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}
			double totalSalesQty = 0;
			double TotalSalesAmount = 0;
			double dTotalTax = 0;
			double dTaxRate = 0.15;
			this.lvitemsum.Items.Clear();
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["DoVipCatSummary"].Rows[i];
				string cat = dr["cat"].ToString();
				string qty = dr["sales_qty"].ToString();
				double totalAmount = Program.MyDoubleParse(dr["sales_amount"].ToString());// *1.15;
//				double dTax = Math.Round(totalAmount - totalAmount / (1 + dTaxRate), 4);
				double dTax = Program.MyDoubleParse(dr["tax"].ToString());

				totalSalesQty += Program.MyDoubleParse(qty);
				TotalSalesAmount += totalAmount;
				dTotalTax += dTax;

				ListViewItem item = new ListViewItem(cat);
				item.SubItems.Add(qty);
				item.SubItems.Add(totalAmount.ToString("c"));
				item.SubItems.Add(dTax.ToString("c"));
				listView2.Items.Add(item);
			}
			ListViewItem sp = new ListViewItem("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			listView2.Items.Add(sp);

			ListViewItem sum = new ListViewItem("Total");
			sum.SubItems.Add(totalSalesQty.ToString());
			sum.SubItems.Add(TotalSalesAmount.ToString("c"));
			sum.SubItems.Add(dTotalTax.ToString("c"));
			listView2.Items.Add(sum);
		}
		private void DoVipItemSummary(int m_nPeriod)
		{
			this.listView1.Items.Clear();
			int rows = 0;
			string m_dateSql = "";
			if (dst.Tables["DoVipItemSummary"] != null)
				dst.Tables["DoVipItemSummary"].Clear();

			if (m_nPeriod == 5)
			{
				if (lblfrom.Text == "From")
				{
					MessageBox.Show("Please select date");
					lblfrom.Text = "                      ";
					//lblfrom.Visible = true;
					lblfrom.BackColor = System.Drawing.Color.Red;
					lblfrom.Size = new Size(200, 20);

					return;
				}
				if (lblto.Text == "To")
				{
					MessageBox.Show("Please select date");
					lblto.Text = "                         ";
					//lblto.Visible = true;
					lblto.BackColor = System.Drawing.Color.Red;
					lblto.Size = new Size(200, 20);
					return;
				}
			}

			string sc = " SET DATEFORMAT dmy ";
			//			sc += " SELECT  s.code, s.code, c.name, c.name_cn, SUM(s.commit_price * s.quantity) AS sales_amount  ";
			sc += " SELECT  s.code, s.code, c.name, c.name_cn, c.tax_code, SUM(s.sales_total) AS sales_amount ";
			sc += ", sum(((s.commit_price * (1-s.discount_percent/100))- s.supplier_price) * s.quantity) AS rough_profit ";
			sc += ", sum(s.supplier_price * s.quantity) AS supplier_price, sum(s.quantity) AS sales_qty, s.tax_rate ";
			sc += " FROM sales s ";
			sc += " JOIN invoice i ON i.invoice_number = s.invoice_number ";
			sc += " JOIN code_relations c ON c.code = s.code";
			sc += " JOIN orders o ON o.invoice_number = s.invoice_number ";
			sc += " WHERE 1=1 ";
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 1 ";
					break;
				case 3:
					//DateTime dt = DateTime.Now;
					//int m_nDays = (int)dt.DayOfWeek;
					m_dateSql = " AND DATEDIFF(week, i.commit_date, GETDATE()) = 0"; ;
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, i.commit_date, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND i.commit_date BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;
			if (m_sVip_id != "")
				sc += " AND i.card_id = " + m_sVip_id;
			sc += " GROUP BY s.code, s.supplier_code, c.name, c.name_cn,c.tax_code, s.tax_rate ";
			sc += " ORDER BY sum(s.quantity) DESC ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "DoVipItemSummary");
				if (rows <= 0)
				{
	//				MessageBox.Show("No Record");
					return;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}
			double totalSalesQty = 0;
			double TotalSalesAmount = 0;
			double dTotalTax = 0;
			this.lvitemsum.Items.Clear();
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["DoVipItemSummary"].Rows[i];
				string code = dr["code"].ToString();
				string name = dr["name_cn"].ToString();
				string name_cn = dr["name"].ToString();
				string qty = dr["sales_qty"].ToString();
				string tax_code = dr["tax_code"].ToString();
				//string order_id = dr["order_id"].ToString();
				double totalAmount = Program.MyDoubleParse(dr["sales_amount"].ToString());// *1.15;
				double dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				double dTax = Math.Round(totalAmount - totalAmount / (1 + dTaxRate), 4);

				totalSalesQty += Program.MyDoubleParse(qty);
				TotalSalesAmount += totalAmount;
				dTotalTax += dTax;

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(name_cn);
				item.SubItems.Add(name);
				item.SubItems.Add(qty);
				item.SubItems.Add(totalAmount.ToString("c"));
				item.SubItems.Add(tax_code);
				item.SubItems.Add(dTax.ToString("c"));
				listView1.Items.Add(item);
			}
			ListViewItem sp = new ListViewItem("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			//sp.SubItems.Add("");
			listView1.Items.Add(sp);

			ListViewItem sum = new ListViewItem("");
			sum.SubItems.Add("");
			sum.SubItems.Add("Total");
			sum.SubItems.Add(totalSalesQty.ToString());
			sum.SubItems.Add(TotalSalesAmount.ToString("c"));
			sum.SubItems.Add("");
			sum.SubItems.Add(dTotalTax.ToString("c"));
			//sum.SubItems.Add("");
			listView1.Items.Add(sum);
			sum.Font = new Font("Arial", 9, FontStyle.Bold);
			print_totalitemsales = TotalSalesAmount;
		}
		private void DoItemSummary(int m_nPeriod)
		{
			this.lvitemsum.Items.Clear();
			int rows = 0;
			string m_dateSql = "";
			if (dst.Tables["itemsummary"] != null)
				dst.Tables["itemsummary"].Clear();

			if (m_nPeriod == 5)
			{
				if (lblfrom.Text == "From")
				{
					MessageBox.Show("Please select date");
					lblfrom.Text = "                      ";
					//lblfrom.Visible = true;
					lblfrom.BackColor = System.Drawing.Color.Red;
					lblfrom.Size = new Size(200, 20);
					
					return;
				}
				if (lblto.Text == "To")
				{
					MessageBox.Show("Please select date");
					lblto.Text = "                         ";
					//lblto.Visible = true;
					lblto.BackColor = System.Drawing.Color.Red;
					lblto.Size = new Size(200, 20);
					return;
				}
			}
			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT s.code, s.code, s.name, s.name as name_cn, s.tax_code, SUM(s.sales_total) AS sales_amount ";
			sc += ", SUM(((s.commit_price * (1 - s.discount_percent/100))- s.supplier_price) * s.quantity) AS rough_profit ";
			sc += ", SUM(s.supplier_price * s.quantity) AS supplier_price, sum(s.quantity) AS sales_qty, s.tax_rate ";
			sc += " FROM sales s ";
			sc += " JOIN invoice i ON i.invoice_number = s.invoice_number ";
			sc += " JOIN orders o ON o.invoice_number = s.invoice_number ";
			sc += " WHERE 1=1 ";
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, i.commit_date, GETDATE()) = 0"; ;
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, i.commit_date, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND i.commit_date BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;
			sc += " GROUP BY s.code, s.supplier_code, s.name, s.tax_code, s.tax_rate ";
			sc += " ORDER BY sum(s.quantity) DESC ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "itemsummary");
				if (rows <= 0)
				{
	//				MessageBox.Show("No Record");
					return;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return ;
			}
			
			//if (rows > 100)
			//{
			//    MessageBox.Show("Item Summary too much data, displaying top 100, please use export to see all records.");
			//}

			double totalSalesQty = 0;
			double TotalSalesAmount = 0;
			double TotalProfit = 0;
			double dTotalTax = 0;
			this.lvitemsum.Items.Clear();
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["itemsummary"].Rows[i];
				string code = dr["code"].ToString();
				string name = dr["name_cn"].ToString();
				string name_cn = dr["name"].ToString();
				string qty = dr["sales_qty"].ToString();
				string rough_profit = dr["rough_profit"].ToString();
				string tax_code = dr["tax_code"].ToString();
				//string order_id = dr["order_id"].ToString();
				double totalAmount = Program.MyDoubleParse(dr["sales_amount"].ToString());// *1.15;
				double droght_profit = Program.MyDoubleParse(dr["rough_profit"].ToString());
				double dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				double dTax = Math.Round(totalAmount- totalAmount/(1+ dTaxRate), 4);

				totalSalesQty += Program.MyDoubleParse(qty);
				TotalSalesAmount += totalAmount;
				TotalProfit += droght_profit;
				dTotalTax += dTax;

				//if(i < 100)
				//{
					ListViewItem item = new ListViewItem(code);
					item.SubItems.Add(name_cn);
					item.SubItems.Add(name);
					item.SubItems.Add(qty);
					item.SubItems.Add(totalAmount.ToString("c"));
					item.SubItems.Add(droght_profit.ToString("c"));
					item.SubItems.Add(tax_code);
					item.SubItems.Add(dTax.ToString("c"));
					lvitemsum.Items.Add(item);
				//}
			}
			ListViewItem sp = new ListViewItem("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			sp.SubItems.Add("");
			//sp.SubItems.Add("");
			lvitemsum.Items.Add(sp);

			ListViewItem sum = new ListViewItem("");
			sum.SubItems.Add("");
			sum.SubItems.Add("Total");
			sum.SubItems.Add(totalSalesQty.ToString());
			sum.SubItems.Add(TotalSalesAmount.ToString("c"));
			sum.SubItems.Add(TotalProfit.ToString("c"));
			sum.SubItems.Add("");
			sum.SubItems.Add(dTotalTax.ToString("c"));
			//sum.SubItems.Add("");
			sum.Font = new Font("Arial", 9, FontStyle.Bold);
			sum.BackColor = Color.Azure;
			lvitemsum.Items.Add(sum);
 
			print_totalitemsales = TotalSalesAmount;
		}
		private void DoCatSummary(int m_nPeriod)
		{
			this.lvCatSum.Items.Clear();
			int nRows = 0;
			string m_dateSql = "";
			if (dst.Tables["catsummary"] != null)
				dst.Tables["catsummary"].Clear();
			if (m_nPeriod == 5)
			{
				if (lblfrom.Text == "From")
				{
					MessageBox.Show("Please select date");
					lblfrom.Text = "                      ";
					//lblfrom.Visible = true;
					lblfrom.BackColor = System.Drawing.Color.Red;
					lblfrom.Size = new Size(200, 20);
					return;
				}
				if (lblto.Text == "To")
				{
					MessageBox.Show("Please select date");
					lblto.Text = "                         ";
					//lblto.Visible = true;
					lblto.BackColor = System.Drawing.Color.Red;
					lblto.Size = new Size(200, 20);
					return;
				}
			}
			string sc = " SET DATEFORMAT dmy ";
			sc += " SELECT s.cat, s.code, s.supplier_code, s.name, SUM(s.sales_total) AS sales_amount ";
			sc += ", sum(s.quantity) AS sales_qty ";
			sc += " FROM sales s ";
			sc += " JOIN invoice i ON i.invoice_number = s.invoice_number ";
			sc += " WHERE 1=1 ";
			//sc += " AND s.cat = 'BEVERAGE' ";
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, i.commit_date, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, i.commit_date, GETDATE()) = 0"; ;
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, i.commit_date, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND i.commit_date BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;
			sc += " GROUP BY s.cat, s.code, s.supplier_code, s.name ";
			sc += " ORDER BY cat, name ";
			sc += " OPTION (MERGE JOIN) ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "catsummary");
				if (nRows <= 0)
				{
	//				MessageBox.Show("No Record");
					return;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}

			string cat = "";
			string cat_old = "-1";
			double dCatQty = 0;
			double dCatAmount = 0;
			int nCats = 0;
			string[] saCatName = new string[999];
			double[] daCatQty = new double[999];
			double[] daCatAmount = new double[999];
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["catsummary"].Rows[i];
				cat = dr["cat"].ToString().Replace(",", " ").Trim();
				if(cat == "")
					cat = "BLANK";
				double dQty = Program.MyDoubleParse(dr["sales_qty"].ToString());
				double dAmount = Program.MyDoubleParse(dr["sales_amount"].ToString());
				if(cat != cat_old && cat_old != "-1")
				{
					saCatName[nCats] = cat_old;
					daCatQty[nCats] = dCatQty;
					daCatAmount[nCats] = dCatAmount;
					dCatQty = 0;
					dCatAmount = 0;
					nCats++;
				}
				dCatQty += dQty;
				dCatAmount += dAmount;
				cat_old = cat;
			}
			saCatName[nCats] = cat;
			daCatQty[nCats] = dCatQty;
			daCatAmount[nCats] = dCatAmount;
			nCats++;

			double totalSalesQty = 0;
			double TotalSalesAmount = 0;
			this.lvCatSum.Items.Clear();
			int nCatIndex = 0;
			cat = "";
			cat_old = "";
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["catsummary"].Rows[i];
				cat = dr["cat"].ToString().Trim();
				string code = dr["code"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString().Replace(",", " ");
				string qty = dr["sales_qty"].ToString();
				double totalAmount = Program.MyDoubleParse(dr["sales_amount"].ToString());// *1.15;

				totalSalesQty += Program.MyDoubleParse(qty);
				TotalSalesAmount += totalAmount;

				if(cat != cat_old || i == 0)
				{
					ListViewItem itemc = new ListViewItem(saCatName[nCatIndex]);
					itemc.SubItems.Add("");
					itemc.SubItems.Add("");
					itemc.SubItems.Add(daCatQty[nCatIndex].ToString());
					itemc.SubItems.Add(daCatAmount[nCatIndex].ToString("$0.00"));
					itemc.Font = new Font("Arial", 9, FontStyle.Bold);
					itemc.BackColor = Color.Azure;
					lvCatSum.Items.Add(itemc);
					nCatIndex++;
				}

				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(supplier_code);
				item.SubItems.Add(name);
				item.SubItems.Add(qty);
				item.SubItems.Add(totalAmount.ToString("$0.00"));
				item.Font = new Font("Arial", 9, FontStyle.Regular);
				lvCatSum.Items.Add(item);
				cat_old = cat;
			}

			ListViewItem sum = new ListViewItem("");
			sum.SubItems.Add("");
			sum.SubItems.Add("Total");
			sum.SubItems.Add(totalSalesQty.ToString());
			sum.SubItems.Add(TotalSalesAmount.ToString("$0.00"));
			sum.Font = new Font("Arial", 9, FontStyle.Bold);
			sum.BackColor = Color.Azure;
			lvCatSum.Items.Add(sum);
			print_totalitemsales = TotalSalesAmount;
		}
		private void adminbkup_Click(object sender, EventArgs e)
		{
			if (File.Exists("autobakup.exe"))
			{
				Process dbbackup = new Process();
				dbbackup.StartInfo.FileName = "autobakup.exe";
				dbbackup.StartInfo.Arguments = "-auto";
				dbbackup.Start();
			}
			else
			{
				MessageBox.Show("Sorry, Backup Application doesn't exist!");
			}
		}
		private void sync_Click(object sender, EventArgs e)
		{
			if (File.Exists("ezFSyncC.exe"))
			{
				Process dbbackup = new Process();
				dbbackup.StartInfo.FileName = "ezFSyncC.exe";
				dbbackup.StartInfo.Arguments = "-auto";
				dbbackup.Start();
			}
			else
			{
				MessageBox.Show("Sorry, Sync Application doesn't exist!");
			}
		}
		private void doSysSetting()
		{
			int rows = 0;
			if (dst.Tables["syssetting"] != null)
				dst.Tables["syssetting"].Clear();
			string sc = " SELECT * FROM settings WHERE hidden = 0 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "syssetting");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
		}
		private void button3_Click(object sender, EventArgs e)
		{
		}
		private bool DoEditSetting(string id, string value, string title, string note)
		{
			string sc = "";
			if (value.Trim() == "" || title.Trim() == "")
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Name or Value Cannot Blank";
				fm.ShowDialog();
				return false;
			}
			if (id.Trim() != "")
			{
				sc += " UPDATE settings SET value = '" + value + "'";
				sc += " ,name = N'" + title + "'";
				sc += " ,description = N'" + note + "'";
				sc += " WHERE id ='" + id + "'";
			}
			else
			{
				sc += " INSERT INTO settings (name, value, description) VALUES";
				sc += " (N'" + title + "', N'" + value + "', N'"+note+"')";
			}
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			doSysSetting();
			return true;
		}
		private bool DoDelSetting(string id)
		{
			string sc = " DELETE FROM settings WHERE id ='" + id + "'";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			doSysSetting();

			return true;
		}
		private void btnrestore_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("Are you sure you want to restore database, you will lose all current data .", "Database Restore", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;
			string sPath = AppDomain.CurrentDomain.BaseDirectory;
			Process dbrestore = new Process();
			dbrestore.StartInfo.FileName = sPath + "restore.exe";
			dbrestore.Start();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void label38_Click(object sender, EventArgs e)
		{

		}
		private void lblcc_Click(object sender, EventArgs e)
		{
		}
		private void lblcash_Click(object sender, EventArgs e)
		{
		}
		private void groupBox3_Enter(object sender, EventArgs e)
		{
		}
		private void btnresetprinter_Click(object sender, EventArgs e)
		{
			string sc = " TRUNCATE TABLE printer_mapping ";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return;
			}
			FormMSG fm = new FormMSG();
			fm.m_sMsg = " Reset Printer Mapping Complete";
			fm.ShowDialog();
		}
		private void btnshowall_Click(object sender, EventArgs e)
		{
			btnBarcodeAdd.Enabled = false;
			cbdepart.SelectedText = "";
			cbAutoweigh.Checked = false;
			cbpricebarcode.Checked = false;
			lbloldbarcode.Text = "";
			txtcost.Text = "";
			txtcostgst.Text = "";
			txtname.Text = "";
			rchdiscription.Text = "";
			txtSupplierCode.Text = "";
			txtprice.Text = "";
			txtspecialprice.Text = "";
			txtsearch.Text = "";
			chkspecial.Checked = false;

			btnLevelPrice.Enabled = false;	

//          cbcat.SelectedItem = "";
//          cb2cat.SelectedItem = "";
//          cb3cat.SelectedItem = "";
			cbdepart.SelectedItem = "";
			cbcat.SelectedText = "";
			cb2cat.SelectedText = "";
			cb3cat.SelectedText = "";
			cbcat.Text = "";
			cb2cat.Text = "";
			cb3cat.Text = "";
			cbCatFilter.Text = "";

			btnspeciallist.Text = "CODE: NEW";
			txtItemStock.Text = "";
			BuildPromotion("");
			txtbarcode.Text = "";
			txtBarcodeQty.Text = "";
			ckbID.Checked = false;
			lvBarcode.Items.Clear();
			rchdiscription.Focus();

			lblshowall.Text = "1";
			ProductList();

			this.btnNext.Enabled = true;
			this.btnLast.Enabled = true;

			this.btnFirst.Enabled = false;
			this.btnPre.Enabled = false;
		}
		private void pnlcats_Paint(object sender, PaintEventArgs e)
		{
		}
		private void button4_Click(object sender, EventArgs e)
		{

		}
		private void btnAdduser_Click(object sender, EventArgs e)
		{
			AddUser au = new AddUser();
			if (btnAdduser.Text == "Add New VIP")
			{
				au.m_bVIP = true;
				au.m_sTitle = "Add New VIP";
				au.ShowDialog();
				ShowVipList();
				return;
			}
			else
			{
				au.ShowDialog();
				ShowUserList();
			}
		}
		private bool SaveSettings()
		{
			string station_id = this.textStationID.Text;
			int nid = Program.MyIntParse(station_id);
			if (nid <= 0 || nid.ToString() != station_id)
			{
				MessageBox.Show("Incorrect Station ID, must be an integer and greater than 0");
				this.textStationID.Focus();
				return false;
			}
			int nTimeout = Program.MyIntParse(this.textTimeout.Text);
			if (nTimeout <= 0 || nTimeout > 3000)
			{
				MessageBox.Show("Incorrect timeout value, must between 0 and 3000");
				this.textTimeout.Focus();
				return false;
			}

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_server", this.Server.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "iis_server", this.txtiis.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_user", this.User.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_pass", this.Password.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_catalog", this.Catalog.Text);
			
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_backup", this.ckbEnableBK.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_sync", this.ckbsync.Checked);
			if (bWebService)
			{
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_web_service", this.cbEnableWebService.Checked);
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "sync_with_cloud", this.ckbSyncWithCloud.Checked);
			}
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "web_service_url", this.txtWebServiceURL.Text);

			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "db_server", this.Server.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "db_user", this.User.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "db_pass", this.Password.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "db_catalog", this.Catalog.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "file_path", this.txtpath.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "sync_path", this.txtsyncpath.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "device_path", this.txtcopy.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\EZNZ\\AUTOBACKUP\\", "days", this.txtdays.Text);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "branch_name", this.txtbranchname.Text);

			/*****************************************/
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "printer_name", this.PrinterName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "paper_width", Program.MyIntParse(this.PaperWidth.Text).ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "font_name", this.fontName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "font_size", this.fontSize.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "font_style", this.fontStyle.Text);
			//Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "scale_name", this.ScaleName.Text);
			//Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "scanner_name", this.ScannerName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "station_id", station_id);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "opos_timeout", nTimeout.ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "dMonitor", this.chkCustomerDis.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "epson", this.ckbEpson.Checked);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_pic", this.ckbPic.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "pic_root", this.txtPic.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "tareoff", this.txtTare.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_amount_min", this.txtAA.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "autotare", this.ckbAutoTare.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "float", this.ckbFloating.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "scanincode", this.ckbScanCode.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "scaninsuppliercode", this.ckbScanSupplierCode.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "autosync", this.ckbAutoSync.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "Latipay_enabled", this.cbEnableLatipay.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "Latipay_gift_card_enabled", this.cbEnableLatipayGiftCard.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "Latipay_qrcodeBackground", this.txtLatipayBackgroundUrl.Text);
			if (bWeChat)
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "wechat_payment", this.cbWechatPayment.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "wechat_uri", this.txtWechatUri.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "wechat_merchant_id", this.txtWechatMerchantID.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "wechat_signature", this.txtWechatSignature.Text);
			
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "saleslogin", this.ckbSalesLogin.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "updateprice", this.ckbUpdatePrice.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "calqty", this.ckbCalQty.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "margin", this.ckbMargin.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "id_check_password_control", this.cbIdCheckPasswordControl.Checked);
			
			//Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "levelprice", this.ckbLevelPrice.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "levelprice", this.rbLevel.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "simpleinvoice", this.ckbSimpleInv.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_aa", this.ckbAA.Checked);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "branch_id_sync", this.txtBranch_id.Text);

			//Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "vip_discount", this.chkEnableVipDis.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "vip_discount", this.rbDis.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "dis_control", this.ckbDiscount.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "voiddis", this.txtvoiddisc.Text);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryitem", this.txtgroceryitem.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryweight", this.txtgroceryweight.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryitemaddup", this.txtgroceryitemaddup.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryweightitemaddup", this.txtgroceryweightitemaddup.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "voidscale", this.txtvoidscale.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "special_discount", this.ckbSpecialDis.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "dMonitor", this.chkCustomerDis.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "regular_monitor", this.rdbmonitorr.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "small_monitor", this.rdbmonitors.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "ad_url", this.textBoxAdUrl.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "serialDevicePort", this.cbSerialDevicePort.Text);


			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "AttractPay_enabled", this.chkEnableAttractpay.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "AlipayDirect_enabled", this.chkEnableAlipayDirect.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "myposmate_enable", this.chkEnableMyPosMate.Checked);
			//Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "myposmate_merchant_id", this.txtMyPosMateMerchantId.Text);
			//Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "myposmate_config_id", this.txtMyPosMateConfigId.Text);

			if (bEftpos)
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enableeftpos", this.chkEnableEftpos.Checked);
			if (bScale)
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enablescale", this.ckbScale.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "cas_scale", this.ckbCasScale.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "cas_scale_port", this.cbCasScalePort.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "eftposCom", this.eftposCom.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "eftpos_type", this.cbEftposType.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryitem", this.txtgroceryitem1.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_pic", this.ckbPic.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "pic_root", this.txtPic.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "tareoff", this.txtTare.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_amount_min", this.txtAA.Text);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "sync", this.ckbsync.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "backup_Database", this.ckbEnableBK.Checked);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "call_of_order_enabled", this.cbCallOfOrderNumber.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "auto_close_popup_menu", this.cbAutoClosePopupMenu.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "smartpay_ip", this.txtSmartpayIP.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "smartpay_port", this.txtSmartpayPort.Text);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_enable", this.EnableSecondPrinter.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_catalog", this.SecondPrinterCatalog.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_name", this.SecondPrinterName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_receipt_qty", this.SecondPrinterReceiptQTY.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_paper_width", this.SecondPrinterPaperWidth.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_epson", this.SecondPrinterEpson.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_size", this.SecondPrinterSize.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_font", this.SecondPrinterFontName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_style", this.SecondPrinterStyle.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "second_printer_timeout", this.SecondPrinterTimeout.Text);
			
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_self_service", this.cbEnableSelfService.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_qty_password", this.cbEnableQtyPassword.Checked);
			
			if(bNVR)
				Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "nvr_enabled", this.cbEnableNVR.Checked.ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "nvr_ip", this.txtNVRIP.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "nvr_port", this.txtNVRPort.Text);

			Program.m_sVoucherServiceURL = txtVoucherServiceUrl.Text;
			Program.SaveSiteSettings("voucher_service_url", Program.m_sVoucherServiceURL);

			int nReceiptQty = Program.MyIntParse(tbReceiptQTY.Text);
			string sc = "";
			sc += " UPDATE settings SET value = " + nReceiptQty.ToString() + " WHERE name LIKE 'total_no_of_receipt_printout' ";
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
			sc = " UPDATE settings SET value = " + this.txtMaxDis.Text + " WHERE name LIKE 'QPOS_MAX_DISCOUNT_PERCENTAGE' ";
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
			SetVoucher();
			SetBarcodeprice();
			/*************************************************************************/
			SetSiteSettings("no_vip_discount_catalog", txtNoVIPDiscountCatalog.Text.Trim().ToLower());
			Program.m_sNoVipDiscountCatalog = txtNoVIPDiscountCatalog.Text.Trim().ToLower();

			Program.GetSettings(); //apply new settings
			return true;
		}

		bool vipPonitsFunctionSupport()
		{
			int nRows = 0;
			if (dst.Tables["vipPonitsFunctionSupport"] != null)
				dst.Tables["vipPonitsFunctionSupport"].Clear();
			string sc = "SELECT Name FROM SysColumns WHERE id=Object_Id('card') AND Name = 'points'";
			try
			{
				//SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "vipPonitsFunctionSupport");
				if (nRows > 0)
					return true;
				else
					return false;
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				//myConnection.Close();
				return false;
			}
			return true;
		}
		private void SetVoucher()
		{
			Program.m_bVoucherUseWebService = rbWebVoucher.Checked;
			string sc = "";
			//sc += " UPDATE settings SET value = '" + this.cbVoucher.Checked + "' WHERE name = 'voucher_enabled' ";
			sc += " UPDATE settings SET value = '" + this.rbVoucher.Checked + "' WHERE name = 'voucher_enabled' ";
			sc += " UPDATE settings SET value = '" + this.txtVoucherexpireday.Text + "' WHERE name = 'voucher_expire_day' ";
			sc += " UPDATE settings SET value = '" + this.txtVoucherRate.Text + "' WHERE name = 'voucher_rate' ";
			sc += " UPDATE settings SET value = '" + this.txtVoucherStartValue.Text + "' WHERE name = 'voucher_start_value' ";
			sc += " UPDATE settings SET value = '" + this.rbvip.Checked + "' WHERE name = 'vip_voucher_enabled' ";
			sc += " UPDATE settings SET value = '" + this.rbWebVoucher.Checked + "' WHERE name = 'voucher_use_webservice' ";
			sc += " UPDATE settings SET value = '" + Program.MyIntParse(this.txtVipStartPoints.Text) + "' WHERE name = 'vip_voucher_start_points' ";
			sc += " UPDATE settings SET value = '" + Program.MyIntParse(this.txtvipvouchervalue.Text) + "' WHERE name = 'vip_voucher_value' ";
			sc += " UPDATE settings SET value = '" + this.rbnovoucher.Checked + "' WHERE name = 'no_voucher'";
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
				return;
			}
		}
		private void SetBarcodeprice()
		{
			string sc = "";
			sc += " UPDATE settings SET value = '" + this.txtbarcodelength.Text + "' WHERE name = 'BarcodeLabel_BarcodeLength' ";
			sc += " UPDATE settings SET value = '" + this.txtbarcodestart.Text + "' WHERE name = 'BarcodeLabel_BarcodeStartDigit' ";
			sc += " UPDATE settings SET value = '" + this.txtdecimal.Text + "' WHERE name = 'BarcodeLabel_DecimalPoint' ";
			sc += " UPDATE settings SET value = '" + this.txtpricelength.Text + "' WHERE name = 'BarcodeLabel_PriceLength' ";
			sc += " UPDATE settings SET value = '" + this.txtpricestart.Text + "' WHERE name = 'BarcodeLabel_PriceStartDigit' ";
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
				return;
			}
		}
		private void User_Click(object sender, EventArgs e)
		{
		
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox2.Checked)
				btndbreset.Enabled = true;
			else
				btndbreset.Enabled = false;
		}
		private void btndbreset_Click(object sender, EventArgs e)
		{
			adminLogin al = new adminLogin();
			al.ShowDialog();
/*			
			if (al.m_bPass)
			{
				adminbkup_Click(sender, e);
				timer1.Start();
			}
*/
			if (al.m_bPass)
			{
				if(doDbCleanup())
				{
					MessageBox.Show("Database reset complete");
					pnlcleanup.Visible = false;
					return;
				}
			}
		}
		private bool DoCheckRunProgram()
		{
			Process[] pname = Process.GetProcessesByName("autobakup");
			if (pname.Length > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		private bool doDbCleanup()
		{
			timer1.Stop();
			if (MessageBox.Show("Database cleanup might take a few minutes.Please click ok to start the database cleanup", "Data Cleanup Message", MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				timer1.Start();
			}
			else
			{
				MessageBox.Show("Database cleanup has been ternimated");
				timer1.Stop();
				return false;
			}
//			if(myCommand.Connection.State == ConnectionState.Open)
//				myCommand.Connection.Close();
			
			string sc = " ";
			sc += " DELETE FROM code_relations WHERE code > 1019 ";
			sc += " DELETE FROM product WHERE code > 1019 ";
			sc += " DELETE FROM code_branch WHERE code > 1019 ";
			sc += " DELETE FROM stock_qty WHERE code > 1019 ";
			sc += " DELETE FROM barcode WHERE 1 = 1 AND item_code > 1019 ";
//			sc += " DELETE FROM button WHERE 1 = 1 ";
//			sc += " DELETE FROM button_item WHERE 1 = 1 ";
//			sc += " DELETE FROM code_extra WHERE 1=1";
//			sc += " DELETE FROM code_extra_option WHERE 1=1";
//			sc += " DELETE FROM code_extra_data WHERE 1=1";
//			sc += " DELETE FROM table_item WHERE 1=1";
			sc += " DELETE FROM card WHERE type<> 4 AND id <> 1";
			sc += " TRUNCATE TABLE catalog ";
			sc += " TRUNCATE TABLE trans ";
			sc += " TRUNCATE TABLE tran_detail ";
			sc += " TRUNCATE TABLE tran_invoice ";
			sc += " TRUNCATE TABLE invoice";
			sc += " TRUNCATE TABLE orders ";
			sc += " TRUNCATE TABLE sales ";
			sc += " TRUNCATE TABLE order_item ";
			sc += " TRUNCATE TABLE till ";
//			sc += " TRUNCATE TABLE extra_option";
//			sc += " TRUNCATE TABLE extra_option_cat ";
			sc += " TRUNCATE TABLE eftposlog ";
//			sc += " TRUNCATE TABLE printer_mapping";
//			sc += " TRUNCATE TABLE printting";
			sc += " TRUNCATE TABLE work_time";
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
/*
			int rows = 0;
			if (dst.Tables["ebutton"] != null)
				dst.Tables["ebutton"].Clear();
			string sce = "SELECT * FROM ebutton WHERE 1=1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sce, myConnection);
				rows = myAdapter.Fill(dst, "ebutton");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			for (int i = 0; i < rows; i++)
			{
				string  scu = "UPDATE ebutton SET btn_id = id, code = '', name='',cat= '' WHERE 1=1";
				try
				{
					myCommand = new SqlCommand(scu);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					myConnection.Close();
					Program.ShowExp(scu, e);
					return false;
				}
	
			}
 */
			string sce = " DELETE FROM button_item WHERE 1=1 ";
			try
			{
				myCommand = new SqlCommand(sce);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sce, e);
				return false;
			}
			return true;
		}
		private void btnlocalsetting_Click(object sender, EventArgs e)
		{
			SaveSettings();
			MessageBox.Show("Setting Saved");
		}
		private void btndbtest_Click(object sender, EventArgs e)
		{
			Program.MsgBox("Function disabled for redundant server");
/*			string server = this.Server.Text;
			string user = this.User.Text;
			string pass = this.Password.Text;
			string catalog = this.Catalog.Text;
			string securityString = "User id=" + user + ";Password=" + pass + ";Integrated Security=false;";
			if (user == "")
				securityString = "Integrated Security=SSPI;";
			SqlConnection myConnection = new SqlConnection("Initial Catalog=" + catalog + ";data source=" + server + ";" + securityString);
			this.btndbtest.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				myConnection.Open();
			}
			catch
			{
				MessageBox.Show("Error connectiong to SQL Server, please check your settings and try again.\r\n", "Database Error");
				this.btndbtest.Enabled = true;
				Cursor.Current = Cursors.Default;
				return;
			}
			finally
			{
				myConnection.Close();
			}
			Cursor.Current = Cursors.Default;
			this.btndbtest.Enabled = true;
			SaveSettings();
			MessageBox.Show("Database successfully connected, settings are saved", "Good News");
*/ 
		}

		private void btndbbackup_Click(object sender, EventArgs e)
		{
			adminbkup_Click(sender, e);

			if (MessageBox.Show("Database Backup Ok? .", "Database backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
			  SaveSettings();
		}
		private void GetLayoutBillHeader()
		{
			rtbExportFooter.Text = Program.ReadSitePage("pos_export_footer");
		}
		private void txtname_Click(object sender, EventArgs e)
		{
		}
		private void btnShowUser_Click(object sender, EventArgs e)
		{
			if (btnShowUser.Text == "Show VIP List")
			{
				btnShowUser.Text = "Show User List";
				btnAdduser.Text = "Add New VIP";
				ShowVipList();

			}
			else
			{
				btnShowUser.Text = "Show VIP List";
				btnAdduser.Text = "Add New User";
				ShowUserList();
			}
		}
		private void chkspecial_Click(object sender, EventArgs e)
		{
			if (chkspecial.Checked)
			{
		//		txtprice.Text = "0";
				txtspecialprice.Enabled = true;
				dtpStartSpecial.Enabled = true;
				dtpEndSpecial.Enabled = true;
				txtspecialprice.Focus();
				txtspecialprice.SelectAll();
			}
			else
			{
				txtspecialprice.Enabled = false;
				dtpStartSpecial.Enabled = false;
				dtpEndSpecial.Enabled = false;
			}
		}
		private void button5_Click(object sender, EventArgs e)
		{
			lblshowall.Text = "1";
			m_bShowSpecial = true;
			ProductList();
			m_bShowSpecial = false;
			btnLevelPrice.Enabled = false;	
		}
		private void dgmain_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			m_iSelectedMainItem = e.RowIndex;
		}
		private void showTimeSheetSum(int m_nPeriod)
		{
			this.lvtssum.Items.Clear();
			this.lvtimesheetdetail.Items.Clear();
			string m_dateSql = "";
		 
			int m_iRows = 0;
			if (dst.Tables["timesum"] != null)
				dst.Tables["timesum"].Clear();
			string sc = " ";
			sc = " SET DATEFORMAT dmy ";
			sc += " SELECT SUM(w.hours) AS total_hours, c.name AS staff_name, b.name AS branch_name, w.card_id, c.barcode";
//			sc += " FROM work_time w JOIN card c ON c.id = w.card_id JOIN branch b ON b.id = c.our_branch ";
			sc += " FROM work_time w JOIN card c ON c.id = w.card_id JOIN branch b ON b.id = c.our_branch ";
			sc += " WHERE 1=1 ";


			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, w.record_time, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, w.record_time, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, w.record_time, GETDATE()) = 0 ";
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, w.record_time, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND w.record_time BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc +=m_dateSql;
		   
		   // if (m_card_id != "" && m_card_id != null && m_card_id != "all")
		  //      sc += " AND w.card_id = " + m_card_id;
			sc += " GROUP BY c.name, b.name, w.card_id, c.barcode, c.id";
			sc += " ORDER BY b.name, c.name ";
			//DEBUG("sc=", sc);	
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				m_iRows = myAdapter.Fill(dst, "timesum");

			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (m_iRows <= 0)
				return;

			for (int i = 0; i < m_iRows; i++)
			{
				DataRow dr = dst.Tables["timesum"].Rows[i];
				string code = dr["card_id"].ToString();
				string barcode = dr["barcode"].ToString();
				string name = dr["staff_name"].ToString();
				string total = dr["total_hours"].ToString();

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(total);
				lvtssum.Items.Add(item);
			}
		}
		
		

		private void showTimeSheetDetail(int m_nPeriod)
		{
			this.lvtimesheetdetail.Items.Clear();
			string m_dateSql = "";
			
			int m_iRows = 0;
			if (dst.Tables["timedetail"] != null)
				dst.Tables["timedetail"].Clear();
			string sc = " ";
			sc = " SET DATEFORMAT dmy ";
			sc += " SELECT w.record_time, w.hours, w.is_checkin, c.name AS staff_name, b.name AS branch_name, w.card_id, c.barcode ";
			sc += " FROM work_time w JOIN card c ON c.id = w.card_id JOIN branch b ON b.id = c.our_branch ";
			sc += " WHERE 1=1 ";
	   
			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, w.record_time, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, w.record_time, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, w.record_time, GETDATE()) = 0 ";
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, w.record_time, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND w.record_time BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;

			if (m_card_id != "")
				sc += " AND w.card_id = " + m_card_id;
			sc += " ORDER BY c.name, w.record_time ";
			//DEBUG("sc=", sc);	
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				m_iRows = myAdapter.Fill(dst, "timedetail");

			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (m_iRows <= 0)
				return;

			for (int i = 0; i < m_iRows; i++)
			{
				DataRow dr = dst.Tables["timedetail"].Rows[i];
				string code = dr["card_id"].ToString();
				string barcode = dr["barcode"].ToString();
				string name = dr["staff_name"].ToString();
				string time = dr["record_time"].ToString();
				string hours = dr["hours"].ToString();
				bool is_checkin = Program.MyBooleanParse(dr["is_checkin"].ToString());
				string clockin = "";
				string clockout = time;
				if (is_checkin)
				{
					clockin = time;
					clockout = "";
				}
				if (hours == "0")
					hours = "";

				ListViewItem item = new ListViewItem(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(clockin);
				item.SubItems.Add(clockout);
				item.SubItems.Add(hours);
				lvtimesheetdetail.Items.Add(item);
			}
		}

		private void showStockReportSum(int m_nPeriod)
		{
			this.lvStockReport.Items.Clear();
			this.lvStockReportDetail.Items.Clear();
			string m_dateSql = "";

			int m_iRows = 0;
			if (dst.Tables["showStockReportSum"] != null)
				dst.Tables["showStockReportSum"].Clear();
			string sc = " ";
			sc = " SET DATEFORMAT dmy ";
			sc += " SELECT SUM(si.qty) AS qty, si.code, c.name AS product, c.barcode";
			sc += " FROM stock_input si left outer JOIN code_relations c ON c.code = si.code ";
			sc += " WHERE 1=1 ";


			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, si.record_time, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, si.record_time, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, si.record_time, GETDATE()) = 0 ";
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, si.record_time, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND si.record_time BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;

			// if (m_card_id != "" && m_card_id != null && m_card_id != "all")
			//      sc += " AND w.card_id = " + m_card_id;
			sc += " GROUP BY si.code, c.name,c.barcode ";
			sc += " ORDER BY c.name, c.barcode ";
			//DEBUG("sc=", sc);	
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				m_iRows = myAdapter.Fill(dst, "showStockReportSum");

			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (m_iRows <= 0)
				return;

			for (int i = 0; i < m_iRows; i++)
			{
				DataRow dr = dst.Tables["showStockReportSum"].Rows[i];
				string qty = dr["qty"].ToString();
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string name = dr["product"].ToString();
	//			string total = dr["total_hours"].ToString();

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(name);
				item.SubItems.Add(barcode);
				item.SubItems.Add(qty);
				lvStockReport.Items.Add(item);
			}
		}

		private void showInputReportDetail(int m_nPeriod)
		{
			this.lvStockReportDetail.Items.Clear();
			string m_dateSql = "";

			int m_iRows = 0;
			if (dst.Tables["showInputReportDetail"] != null)
				dst.Tables["showInputReportDetail"].Clear();
			string sc = " ";
			sc = " SET DATEFORMAT dmy ";
			sc += " SELECT si.record_time,si.qty, si.code,c.name AS product, c.barcode, cc.name AS staff_name  ";
			sc += " FROM stock_input si ";
			sc += " LEFT OUTER JOIN code_relations c ON si.code = c.code ";
			sc += " LEFT OUTER JOIN card cc ON si.staff = cc.id ";
			sc += " WHERE 1=1 ";

			switch (m_nPeriod)
			{
				case 1:
					m_dateSql = " AND DATEDIFF(day, si.record_time, GETDATE()) = 0 ";
					break;
				case 2:
					m_dateSql = " AND DATEDIFF(day, si.record_time, GETDATE()) = 1 ";
					break;
				case 3:
					m_dateSql = " AND DATEDIFF(week, si.record_time, GETDATE()) = 0 ";
					break;
				case 4:
					m_dateSql = " AND DATEDIFF(month, si.record_time, GETDATE()) = 0 ";
					break;
				case 5:
					if (lblto.Text == "To" || lblfrom.Text == "From")
						return;
					m_dateSql = " AND si.record_time BETWEEN '" + lblfrom.Text + "' AND DATEADD(day, 1, '" + lblto.Text + "') ";
					break;
				default:
					break;
			}
			sc += m_dateSql;

			if (m_sItemCode != "")
				sc += " AND si.code = " + m_sItemCode;
			sc += " ORDER BY c.name, si.record_time ";
			//DEBUG("sc=", sc);	
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				m_iRows = myAdapter.Fill(dst, "showInputReportDetail");

			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (m_iRows <= 0)
				return;

			for (int i = 0; i < m_iRows; i++)
			{
				DataRow dr = dst.Tables["showInputReportDetail"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string product = dr["product"].ToString();
				string qty = dr["qty"].ToString();
				string record_time = dr["record_time"].ToString();
				string staff = dr["staff_name"].ToString();

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(product);
				item.SubItems.Add(qty);
				item.SubItems.Add(record_time);
				item.SubItems.Add(staff);
				lvStockReportDetail.Items.Add(item);
			}
		}
		
		
		private void selectVIP()
		{
			string sVIP = "";
			ListView.SelectedListViewItemCollection items = this.lvVipsum.SelectedItems;
			foreach (ListViewItem item in items)
			{
				sVIP = item.SubItems[0].Text;
			}
			if (sVIP.Trim() == "")
				return;
			m_sVip_id = sVIP;
		}
		private void selectStaff()
		{
			ListView.SelectedListViewItemCollection items = this.lvtssum.SelectedItems;
			foreach (ListViewItem item in items)
			{
				m_sCode = item.SubItems[0].Text;
			}
			if (m_sCode.Trim() == "")
				return;
			m_card_id = m_sCode;
		}
		
		private void selectCode()
		{
			string sCode = "";
			ListView.SelectedListViewItemCollection items = this.lvStockReport.SelectedItems;
			foreach (ListViewItem item in items)
			{
				sCode = item.SubItems[0].Text;
			}
			if (sCode.Trim() == "")
				return;
			m_sItemCode = sCode;
		}
		

		private void lvtssum_MouseClick(object sender, MouseEventArgs e)
		{
			selectStaff();
			showTimeSheetDetail(m_iDateSelect);
			m_card_id = "";
		}

		private void tabcreport_Click(object sender, EventArgs e)
		{
		  
			int TPC = tabcreport.TabPages.Count;
			string TPN = tabcreport.TabPages[0].Text;
			if (TPN == "Item Summary" || TPN == "Time Sheet")
			{
				this.fakePrint.Visible = false;
				this.invoice_del.Visible = false;
			}
			else
			{
				this.fakePrint.Visible = true;
				this.invoice_del.Visible = true;
			}
			m_card_id = "";
			showTimeSheetSum(m_iDateSelect);
			showTimeSheetDetail(m_iDateSelect);
		}
		private void btnimport_Click(object sender, EventArgs e)
		{
			Item_Import ii = new Item_Import();
			ii.ShowDialog();
		}
		private void btnexport_Click(object sender, EventArgs e)
		{
			DateTime date_start = new DateTime();
			DateTime date_end = new DateTime();
			if(lblto.Text.Trim() == "")
			{
				MessageBox.Show("Please select 'To' date");
				return;
			}
			if (payment_report_case == 1)
			{
				date_start = DateTime.Today;
				date_end = DateTime.Now;
			}
			else if (payment_report_case == 2)
			{
				date_start = DateTime.Today.AddDays(-1);
				date_end = DateTime.Today;
			}
			else if (payment_report_case == 3)
			{
				System.DateTime dt = System.DateTime.Today;
				System.DayOfWeek dmon = System.DayOfWeek.Monday;
				int span = dt.DayOfWeek - dmon;
				dt = dt.AddDays(-span);
				date_start = dt;
				date_end = DateTime.Now;
			}
			else if (payment_report_case == 4)
			{
				date_start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
				date_end = DateTime.Now;
			}
			else if (payment_report_case == 5)
			{
				date_start = DateTime.Parse(lblfrom.Text.ToString());
				date_end = DateTime.Parse(lblto.Text.ToString()).AddHours(23).AddMinutes(59).AddSeconds(59);
			}
			if (selectedTabName.ToLower().Trim() == "time sheet")
			{
				export_time_sheet ets = new export_time_sheet();
				ets.ShowDialog();
				exportReportType = ets.extimetype;
			}

			if (selectedTabName.ToLower().Trim() == "vip report")
			{
				exportVip ets = new exportVip();
				ets.ShowDialog();
				exportReportType = ets.exportViptimetype;
			}
			if (selectedTabName.ToLower().Trim() == "cat sum")
			{
				exportReportType = 7;
			}
			
			SaveFileDialog SFD = new SaveFileDialog();
			SFD.Filter = "CSV Files(*.csv)|*.csv";
			if (SFD.ShowDialog() == DialogResult.OK)
			{
				StringBuilder sb = new StringBuilder();
				switch (exportReportType)
				{
					case 0:
						DoPaymentReportExport();
						foreach (ColumnHeader CH in lvrpayment.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in lvrpayment.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					case 1:
						sb.Append("From: ," + date_start + ",  To: ," + date_end + ", \r\n");
						foreach (ColumnHeader CH in lvitemsum.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in lvitemsum.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					case 2:
						foreach (ColumnHeader CH in lvVipsum.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in lvVipsum.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					case 3:
						foreach (ColumnHeader CH in lvSales.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in lvSales.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					case 4:
						foreach (ColumnHeader CH in lvtssum.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in lvtssum.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					case 5:
						foreach (ColumnHeader CH in lvtimesheetdetail.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in lvtimesheetdetail.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					case 6:
						foreach (ColumnHeader CH in listView1.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in this.listView1.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					case 7:
						foreach (ColumnHeader CH in lvCatSum.Columns)
						{
							sb.Append(CH.Text + ",");
						}
						sb.AppendLine();
						foreach (ListViewItem lv in this.lvCatSum.Items)
						{
							foreach (ListViewItem.ListViewSubItem lvs in lv.SubItems)
							{
								if (lvs.Text.Trim() != "")
								{
									sb.Append(lvs.Text.Replace(",", "") + ",");
								}
								else
									sb.Append("" + ",");
							}
							sb.AppendLine();
						}
						break;
					default:
						break;

				}
				StreamWriter sw = new StreamWriter(SFD.FileName);
				sw.Write(sb.ToString());
				sw.Close();
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!DoCheckRunProgram())
			{
				if (doDbCleanup())
				{
					timer1.Stop();
					MessageBox.Show("Database reset complete");
					pnlcleanup.Visible = false;
					return;
				}
			}
		}
		private void tabcreport_Selected(object sender, TabControlEventArgs e)
		{
			exportReportType = e.TabPageIndex;
			selectedTabName = tabcreport.TabPages[e.TabPageIndex].Text;
			if (selectedTabName == "Item Summary" || selectedTabName == "Time Sheet")
			{
				this.fakePrint.Enabled = false;
				this.invoice_del.Enabled = false;
			}
			else if(selectedTabName == "Payment Report")
			{
				this.fakePrint.Enabled = true;
				this.invoice_del.Enabled = true;
			}
		}

		private string GetItemCode(string barcode)
		{
			if (dst.Tables["code"] != null)
				dst.Tables["code"].Clear();
			string sc = " SELECT item_code FROM barcode WHERE barcode = '" + Program.EncodeQuote(barcode) + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "code") <= 0)
					return "";
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return "";
			}
			return dst.Tables["code"].Rows[0]["item_code"].ToString();
		}
		private bool doCheckExistExtra(string name)
		{
			if (dst.Tables["existextra"] != null)
				dst.Tables["existextra"].Clear();
			string sc = " SELECT * FROM code_extra WHERE name = N'" + name + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "existextra") <= 0)
					return false;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			return true;
		}
		private bool doCheckExistExtraOption(string extra_id_check, string optionName)
		{
			if (dst.Tables["existextraoption"] != null)
				dst.Tables["existextraoption"].Clear();
			string sc = " SELECT * FROM code_extra_option WHERE extra_id = " + extra_id_check + " AND LOWER(name) = N'" + optionName + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "existextraoption") > 0)
					return false;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			return true;
		}
		private bool doCheckExistBarcode(string barcode)
		{
			if(barcode.Trim() == "")
				return false;
			if (dst.Tables["checkbarcode"] != null)
				dst.Tables["checkbarcode"].Clear();
//			string sc = " SELECT * FROM barcode WHERE barcode = '" + barcode + "' AND item_code = " + m_sCode;
			string sc = " SELECT * FROM barcode WHERE barcode = '" + barcode + "'";// AND item_code = " + m_sCode;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkbarcode") <= 0)
					return false;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			return true;
		}
		private void printermapping_DataError(object sender, DataGridViewDataErrorEventArgs anError)
		{

		 //   MessageBox.Show("Error happened " + anError.Context.ToString());

			if (anError.Context == DataGridViewDataErrorContexts.Commit)
			{
				MessageBox.Show("Commit error");
			}
			if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
			{
				MessageBox.Show("Cell change");
			}
			if (anError.Context == DataGridViewDataErrorContexts.Parsing)
			{
				MessageBox.Show("parsing error");
			}
			if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
			{
				MessageBox.Show("leave control error");
			}

			if ((anError.Exception) is ConstraintException)
			{
				DataGridView view = (DataGridView)sender;
				view.Rows[anError.RowIndex].ErrorText = "an error";
				view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

				anError.ThrowException = false;
			}
		}

		private void invoice_del_click(object sender, EventArgs e)
		{
			string del_check = "0";
			if (lvrpayment.Items.Count <= 0)
			{
				MessageBox.Show("Please select a RANGE of report!!!");
				return;
			}
			else
			{
//				for (int i = 0; i < lvrpayment.Items.Count-1; i++)
				for (int i = 0; i < lvrpayment.Items.Count; i++)
				{
					if (lvrpayment.Items[i].SubItems[9].Text.Trim() != "" && lvrpayment.Items[i].SubItems[9].Text != null)
						lvrpayment.Items[i].Checked = false;

					if (lvrpayment.Items[i].Checked)
					{
					  del_check = "1";
					  break;
					}
				}
			}
			if (del_check != "1")
			{
				MessageBox.Show("Please select the INVOICE!!!");
				return;
			}
			else
			{
				if (MessageBox.Show("Are you sure you want to delete current selected INVOICEs???", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;
				else
				{
	//				for (int i = 0; i < lvrpayment.Items.Count - 1; i++)
					for (int i = 0; i < lvrpayment.Items.Count; i++)
					{
						if (lvrpayment.Items[i].Checked)
							DoDeleteInvoice(lvrpayment.Items[i].SubItems[2].Text.Trim());
					}
				}
			   
			}
			DoPaymentReport(payment_report_case);
			DoItemSummary(payment_report_case);
			DoVipReport(payment_report_case);
			showTimeSheetSum(payment_report_case);
			DoSalesReport(payment_report_case);
			showTimeSheetDetail(payment_report_case);
		}
		private void DoDeleteInvoice(string invoice_number)
		{
			int rows = 0;
			if (dst.Tables["payment_delete"] != null)
				dst.Tables["payment_delete"].Clear();
			string sc = " SELECT t.id as id FROM tran_detail td LEFT OUTER JOIN trans t ON t.id = td.id WHERE td.invoice_number = '" + invoice_number + "'";
//            sc += " OR td.invoice_number = '" + invoice_number + ",'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "payment_delete");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DataRow dr = dst.Tables["payment_delete"].Rows[0];
			string id = dr["id"].ToString();
		  
			string sc_d = " DELETE FROM invoice WHERE invoice_number = '" + invoice_number + "'";
			sc_d += " DELETE FROM orders WHERE invoice_number = '" + invoice_number + "'";
			sc_d += " DELETE FROM sales WHERE invoice_number = '" + invoice_number + "'";
//			sc_d += " DELETE FROM trans WHERE id = '" + id + "'";
			sc_d += " DELETE FROM trans WHERE id in (SELECT id FROM tran_detail WHERE invoice_number = '"+invoice_number+"')";
//			sc_d += " DELETE FROM tran_detail WHERE id = '" + id + "'";
			sc_d += " DELETE FROM tran_detail WHERE invoice_number = '" + invoice_number + "'";
			try
			{
				myCommand = new SqlCommand(sc_d);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc_d, ex);
				myCommand.Connection.Close();
				return;
			}
		}

		private void lvrpayment_MouseClick(object sender, MouseEventArgs e)
		{
			string select_all = "0";
			if (lvrpayment.Items[lvrpayment.Items.Count - 1].Checked)
				select_all = "1";
			ListViewHitTestInfo info = this.lvrpayment.HitTest(e.Location);
			if (info.SubItem.Text == "Exclusion")
			{
				MessageBox.Show("It doesn't allow to delete Account Invoice!!!");
				info.Item.Checked = true;
			   
			}
			else if (info.SubItem.Text == "Select All")
			{
				if (select_all == "0")
				{
					for (int i = 0; i < lvrpayment.Items.Count - 1; i++)
					{
						if (lvrpayment.Items[i].SubItems[9].Text != "" && lvrpayment.Items[i].SubItems[9].Text != null)
						   continue;
						lvrpayment.Items[i].Checked = true;
					}
				}
				else
				{
					for (int i = 0; i < lvrpayment.Items.Count - 1; i++)
					{
						lvrpayment.Items[i].Checked = false;
					}
				}
			}
			else
			{
				select_all = "0";
				lvrpayment.Items[lvrpayment.Items.Count - 1].Checked = false;
			   
			}
		}
		private void fakePrint_Click(object sender, EventArgs e)
		{
			if (lvrpayment.Items.Count <= 0)
			{
	//			MessageBox.Show("No Record to Print!!!");
				return;
			}
			else
				DoFakePrint();
		}
		private void DoFakePrint()
		{
			this.m_sPrintBuffer = PrintFakeTotal(Program.m_sStationID);
			try
			{
				printDoc.Print();
			}
			catch (Exception e)
			{
				MsgPrintError(printDoc, e);
			}
		}
		private string PrintFakeTotal(string sStationID)
		{
		   
			DateTime date_start = new DateTime();
			DateTime date_end = new DateTime();
			double dtotal_tansnum = 0;
			double dCash = 0;
			double dEftpos = 0;
			double dCheque = 0;
			double dCreditCard = 0;
			double dCharge = 0;
			double dCharge_paid = 0;
			double dTotalitemsales = 0;
			double dCashout = 0;
			double dTotalDicount = 0;
			double dRounding = 0;
			dCash = print_cash;
			dEftpos = print_eftpos;
			dCheque = print_cheque;
			dCreditCard = print_cc;
			dCharge = print_account;
			dCharge_paid = print_account_paid;
			dCashout = print_cashout;
			dRounding = print_rounding;
			dTotalitemsales = print_totalitemsales;
			dtotal_tansnum = print_trans;
			if (payment_report_case == 1)
			{
				date_start = DateTime.Today;
				date_end = DateTime.Now;
			}
			else if (payment_report_case == 2)
			{
				date_start = DateTime.Today.AddDays(-1);
				date_end = DateTime.Today;
			}
			else if (payment_report_case == 3)
			{
				System.DateTime dt = System.DateTime.Today;
				System.DayOfWeek dmon = System.DayOfWeek.Monday;
				int span = dt.DayOfWeek - dmon;
				dt = dt.AddDays(-span);
				date_start = dt;
				date_end = DateTime.Now;
			}
			else if (payment_report_case == 4)
			{
				date_start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
				date_end = DateTime.Now;
			}
			else if (payment_report_case == 5)
			{
				date_start = DateTime.Parse(lblfrom.Text.ToString());
				date_end = DateTime.Parse(lblto.Text.ToString()).AddHours(23).AddMinutes(59).AddSeconds(59);

			}
				

			
			string s = "TOTAL REPORT \r\n";
			s += "Till No:" + sStationID + "\r\n";
			s += "Date From:" + date_start.ToString() +"\r\n";
			s += "Date To:" + date_end.ToString() + "\r\n";
			s += "Total Transaction(s): " + dtotal_tansnum + "\r\n";
//			s += "Branch: " + m_sBranchName + "\r\n";
			s += Program.PrintPadding("", "", "=") + "\r\n";

			s += Program.PrintPaddingMid("CASH:", dCash.ToString("c"), " ") + "\r\n";
			s += Program.PrintPaddingMid("EFTPOS:", dEftpos.ToString("c"), " ") + "\r\n";
			s += Program.PrintPaddingMid("CHEQUE:", dCheque.ToString("c"), " ") + "\r\n";
			s += Program.PrintPaddingMid("CREDIT CARD:", dCreditCard.ToString("c"), " ") + "\r\n";
			s += "**********\r\n";
			s += Program.PrintPaddingMid("TOTAL:", (dCash + dEftpos + dCheque + dCreditCard).ToString("c"), " ") + "\r\n";
			s += Program.PrintPadding("", "", "=") + "\r\n";
			s += Program.PrintPaddingMid("ROUNDING ALLOWANCE:", dRounding.ToString("c"), " ") + "\r\n";
			s += Program.PrintPaddingMid("WITHDRAW CASH:", dCashout.ToString("c"), " ") + "\r\n";
			s += Program.PrintPaddingMid("CASH ON HAND:", (dCash - dCashout).ToString("c"), " ") + "\r\n";
			s += "**********\r\n";
			s += Program.PrintPaddingMid("TOTAL SALES:", (dCash + dEftpos + dCheque + dCreditCard - dCashout - dRounding).ToString("c"), " ") + "\r\n";
			s += Program.PrintPadding("", "", "=") + "\r\n";
			s += Program.PrintPaddingMid("CHARGE:", dCharge.ToString("c"), " ") + "\r\n";
			s += Program.PrintPaddingMid("CHARGE PAID:", dCharge_paid.ToString("c"), " ") + "\r\n";
			s += "**********\r\n";
			s += Program.PrintPaddingMid("CHARGE BAL:", (dCharge - dCharge_paid).ToString("c"), " ") + "\r\n";
			s += Program.PrintPadding("", "", "=") + "\r\n";
			
			string sc = "";
			string invoice_condition = "";
			for (int i = 0; i < lvrpayment.Items.Count - 1; i++)
			{
				if (i == 0)
					invoice_condition += " invoice_number = '" + lvrpayment.Items[i].SubItems[2].Text.Trim() + "'";
				else
					invoice_condition += " OR invoice_number = '" + lvrpayment.Items[i].SubItems[2].Text.Trim() + "'";
			}
			if (dst.Tables["discount"] != null)
				dst.Tables["discount"].Clear();
			sc = " SELECT total_discount FROM orders";
			sc += " WHERE 1=1 AND (" + invoice_condition + ")";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "discount");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "";
			}
			for (int i = 0; i < dst.Tables["discount"].Rows.Count; i++)
			{
				DataRow dr = dst.Tables["discount"].Rows[i];
				double dDiscount = Program.MyDoubleParse(dr["total_discount"].ToString());
				dTotalDicount += dDiscount;
			}
			
			if (dst.Tables["cat"] != null)
				dst.Tables["cat"].Clear();
			sc = " SELECT cat FROM catalog WHERE cat <> N'Brands' GROUP BY cat, seq ORDER BY ABS(seq)";

			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "cat");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "";
			}
			double dCatTotal_others = 0;
			double dCatSalesTotal = 0;
			string salesQty = "0";
			
			for (int ii = 0; ii < dst.Tables["cat"].Rows.Count; ii++)
			{

				DataRow dr = dst.Tables["cat"].Rows[ii];
				//string catCode = drs["code"].ToString();
				//string catName = drs["name_cn"].ToString();
				string catName = dr["cat"].ToString();
				catName = catName.ToUpper();
				dCatSalesTotal = double.Parse(catSalesTotal(catName, invoice_condition));
				salesQty = itemSaleTotal(catName, invoice_condition);
				if (dCatSalesTotal == 0)
					continue;
				if (catName != "Others" && catName != "others" && catName != "OTHERS")
				{
					s += Program.PrintPaddingMid(catName + "(" + salesQty + ")", " ", "  ") + "\r\n";
					s += Program.PrintPaddingMid(" ", dCatSalesTotal.ToString("c"), " ") + "\r\n";
				}
				else
					dCatTotal_others += dCatSalesTotal;
			}
			s += Program.PrintPaddingMid("Others: ", dCatTotal_others.ToString("c"), " ") + "\r\n";
			s += "**********\r\n";
			s += Program.PrintPaddingMid("TOTAL DISCOUNT GIVEN: ", dTotalDicount.ToString("c"), " ") + "\r\n";
			s += Program.PrintPaddingMid("ALL ITEM(S) SALES TOTAL:", dTotalitemsales.ToString("c"), " ") + "\r\n";
			s += Program.PrintPadding("", "", "=") + "\r\n";
			s += "Have a nice day!\r\n";
			return s;
		}
		private string catSalesTotal(string cName, string sql_note)
		{
			double dCatSales = 0;
			if (dst.Tables["catsum"] != null)
				dst.Tables["catsum"].Clear();

			string sc = " SELECT SUM(sales_total) AS cat_order_total";
			sc += " FROM sales WHERE 1 = 1";
			sc += " AND cat = N'" + cName + "'";
			sc += " AND (" + sql_note + ")";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "catsum");
			}

			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}

			for (int i = 0; i < dst.Tables["catsum"].Rows.Count; i++)
			{
				DataRow dr = dst.Tables["catsum"].Rows[i];
				dCatSales = Program.MyDoubleParse(dr["cat_order_total"].ToString());
			}
			return dCatSales.ToString();
		}

		private string itemSaleTotal(string cName, string sql_note)
		{
			string qty = "";
			if (dst.Tables["sqty"] != null)
				dst.Tables["sqty"].Clear();

			string sc = " SELECT SUM(quantity) AS qty";
			sc += " FROM sales WHERE 1 = 1";
			sc += " AND cat= N'" + cName + "'";
			sc += " AND (" + sql_note + ")";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "sqty");
			}

			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return ""; ;
			}

			for (int i = 0; i < dst.Tables["sqty"].Rows.Count; i++)
			{
				DataRow dr = dst.Tables["sqty"].Rows[i];
				qty = dr["qty"].ToString();
			}
			return qty;
		}
		private void MsgPrintError(PrintDocument pd, Exception e)
		{
			string sDebugInfo = e.ToString();
			string sMsg = "Printing Failed, Please check printer : ";
			sMsg += pd.PrinterSettings.PrinterName;
			sMsg += ", make sure the name is correct and properly attached to this computer.";
			MessageBox.Show(sMsg, "Printer Error");
		}
		private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
		{

			float fSize = (float)Program.MyDoubleParse(Program.m_sFontSize);
			if (fSize == 0)
				fSize = 10;
			Font printFont = new Font(Program.m_sFontName, fSize, Program.m_tFontStyle);
			Font printFontBig = new Font(Program.m_sFontName, fSize + 6, Program.m_tFontStyle);
			Font m_PrintFont = new Font("Times New Roman", 45);
			Font m_PrintFontON = new Font("Times New Roman", 25);
			int y = 0;
			int lineHeight = 20;
			string s = "1";
			int receiptEnd = 0;
			int p = m_sPrintBuffer.IndexOf("[b]");
			int q = m_sPrintBuffer.IndexOf("==== Kitchen Copy ======");
			for (int c = 0; c < m_sPrintBuffer.Length; c++)
			{
				if (m_sPrintBuffer.Substring(c, 1) == "\r")
					receiptEnd++;
			}
			if (p < 0 && q < 0)
			{
				s = m_sPrintBuffer;
				e.Graphics.DrawString(m_sPrintBuffer, printFont, Brushes.Black, 0, y);
				return;
			}
			int pEnd = m_sPrintBuffer.IndexOf("[/b]", p + 3);

			if (p > 0)
			{
				s = m_sPrintBuffer.Substring(0, p);
				e.Graphics.DrawString(s, printFontBig, Brushes.Black, 0, y);
				y += lineHeight;
			}


			if (pEnd > 0)//big font end
			{
				s = m_sPrintBuffer.Substring(p + 3, pEnd - p - 3); //sub string in big font
				e.Graphics.DrawString(s, printFontBig, Brushes.Black, 0, y);
				y += lineHeight;
				s = m_sPrintBuffer.Substring(pEnd + 4, m_sPrintBuffer.Length - pEnd - 4); //rest of string, in regular size
				e.Graphics.DrawString(s, printFont, Brushes.Black, 0, y);
			}
			else
			{
				s = m_sPrintBuffer.Substring(pEnd + 4, m_sPrintBuffer.Length - pEnd - 4); //rest of string, in regular size
				e.Graphics.DrawString(m_sPrintBuffer, printFontBig, Brushes.Black, 0, y);
			}
			y *= receiptEnd;
		}
		private void cbdepart_SelectedIndexChanged(object sender, EventArgs e)
		{
	//		if (!m_bJustSelectedItem)
	//			ProductList();
		}
		private void cbcat_SelectedIndexChanged(object sender, EventArgs e)
		{
	//		if (!m_bJustSelectedItem)
	//			ProductList();
			UpdateCat2("product_page");
			UpdateCat3("product_page");
		}
		private void cb2cat_SelectedIndexChanged(object sender, EventArgs e)
		{
	//		if (!m_bJustSelectedItem)
	//			ProductList();
			UpdateCat3("product_page");
		}
		private void cbDealerPriceCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateCat2("dealer_price_page");
			UpdateCat3("dealer_price_page");
			cbDealerPriceSCat.Text = "";
			cbDealerPriceSSCat.Text = "";
		}
		private void cbDealerPriceSCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateCat3("dealer_price_page");
			cbDealerPriceSSCat.Text = "";
		}
		private void UpdateCat2(string type)
		{
			string cat = "";
			if(type =="product_page")
				cat = Program.EncodeQuote(cbcat.Text);
			else if(type == "dealer_price_page")
				cat = Program.EncodeQuote(cbDealerPriceCat.Text);

			int nRows = 0;
			if(dst.Tables["s_cat"] != null)
				dst.Tables["s_cat"].Clear();
			string sc = " SELECT DISTINCT s_cat FROM catalog ";
			sc += " WHERE cat = N'" + cat + "' ";
			sc += " ORDER BY s_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "s_cat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			cb2cat.Items.Clear();
			cbDealerPriceSCat.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["s_cat"].Rows[i];
				string s_cat = dr["s_cat"].ToString();
				cb2cat.Items.Add(s_cat);
				cbDealerPriceSCat.Items.Add(s_cat);
			}
		}
		private void UpdateCat3(string type)
		{
			string cat = "";
			string s_cat = "";
			if (type == "product_page")
			{
				cat = Program.EncodeQuote(cbcat.Text);
				s_cat = Program.EncodeQuote(cb2cat.Text);
			}
			else if (type == "dealer_price_page")
			{
				cat = Program.EncodeQuote(cbDealerPriceCat.Text);
				s_cat = Program.EncodeQuote(cbDealerPriceSCat.Text);
			}

			int nRows = 0;
			if (dst.Tables["ss_cat"] != null)
				dst.Tables["ss_cat"].Clear();
			string sc = " SELECT DISTINCT ss_cat FROM catalog ";
			sc += " WHERE cat = N'" + cat + "' ";
			sc += " AND s_cat = N'" + s_cat + "' ";
			sc += " ORDER BY ss_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "ss_cat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			cb3cat.Items.Clear();
			cbDealerPriceSSCat.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["ss_cat"].Rows[i];
				string ss_cat = dr["ss_cat"].ToString();
				cb3cat.Items.Add(ss_cat);
				cbDealerPriceSSCat.Items.Add(ss_cat);
			}
		}
		private void panel2_Paint(object sender, PaintEventArgs e)
		{

		}
		private void btnsync_Click(object sender, EventArgs e)
		{
			sync_Click(sender, e);

		//    if (MessageBox.Show("Database Backup Ok? .", "Database backup", MessageBoxButtons.YesNo) == DialogResult.Yes)
				SaveSettings();
		}
		private void btnCompany_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			txtNoVIPDiscountCatalog.Text = Program.GetSiteSettings("no_vip_discount_catalog");
			tccompany.BringToFront();
			tccompany.Visible = true;
			txtVoucherServiceUrl.Text = Program.m_sVoucherServiceURL;
			ShowUserList();
			ShowVipList();
			RefreshLvCurrency();
			DoShowCompanyDetail();
			DoShowInvoiceHeaderFooter();
			GetLayoutBillHeader();
//			SaveSettings();
			doSysSetting();
			richTextBox1.Text = "CAUTION" + "\r\n";
			richTextBox1.Text += "This function is for professional technician use only." + "\r\n";
			richTextBox1.Text += "All the data will be losed if this function is performed" + "\r\n";
			richTextBox1.Text += "Please must be careful by using this function." + "\r\n";
			richTextBox1.Text += "Auto database backup is compulsoried running while click on this button." + "\r\n";
		}
		private void btnPromoEdit_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			pnlPromotion.Visible = true;
			PrintPromoTypeOptions();
			PrintBranchOptions();
			RefreshPromoList();
			APUpdateCat();
			RefreshItemList();
			btnPromotionNew_Click(null, null);
			groupBox33.Visible = true;
		}
		private void RefreshItemList()
		{
			string kw = txtS.Text.ToLower().Trim();
			double Num;
			bool bIsDigit = double.TryParse(kw, out Num);
			int nRows = 0;
			if (dst.Tables["itemlist"] != null)
				dst.Tables["itemlist"].Clear();
			string sc = " SELECT DISTINCT ";
			if(comboBox3.Text == "" && kw == "")
				sc += " TOP 100 ";
			sc += " c.code, c.supplier_code, c.name, c.price1, c.cat,c.s_cat, c.ss_cat, pr.promo_id, p.promo_desc, c.barcode ";
			sc += " FROM code_relations c ";
			sc += " Left outer join barcode b on b.item_code = c.code ";
			sc += " LEFT OUTER JOIN promo pr on b.item_code = pr.code ";
			sc += " LEFT OUTER JOIN promotion_list p ON p.promo_id = pr.promo_id ";

			sc += " WHERE 1 = 1 and c.is_special = 0 AND c.cat != 'ServiceItem'";
			if (kw != "")
			{
				sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
				sc += " OR b.barcode = N'" + kw + "' ";
				if(bIsDigit)
					sc += " OR c.code = " + kw + " ";
				sc += ")";
			}
			if (comboBox3.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) = N'" + comboBox3.Text.ToLower() + "'";
			if (comboBox2.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + comboBox2.Text.ToLower() + "'";
			if (comboBox1.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + comboBox1.Text.ToLower() + "'";
			sc += " ORDER BY c.code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "itemlist");
				totalitemP = nRows;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			lvItem.CheckBoxes = true;
			lvItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["itemlist"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cat = dr["cat"].ToString();
				string scat = dr["s_cat"].ToString();
				string sscat = dr["ss_cat"].ToString();
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(price1);
				item.SubItems.Add(cat);
				item.SubItems.Add(scat);
				item.SubItems.Add(sscat);
				item.SubItems.Add(promo_desc);
				item.SubItems.Add(promo_id);
				this.lvItem.Items.Add(item);

//				if (lvItem.Items[i].SubItems[8].Text != "" && lvItem.Items[i].SubItems[8].Text != "0" && lvItem.Items[i].SubItems[8].Text != null)
//					lvItem.Items[i].Checked = true;
			}

			ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			this.lvItem.Items.Add(sum);
			sum.SubItems.Add("");
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.Red;
		}
		private void RefreshPromoList()
		{
			KeyValuePair<int, string> kv = (KeyValuePair<int, string>)cbPromoType.SelectedItem;
			string promo_type = kv.Key.ToString().Trim();
			if (promo_type == "1")
			{
				gpSpecial.Visible = true;
				gblimit.Visible = true;
				gpDiscount.Visible = false;
				groupBox19.Visible = false;
				groupBox15.Visible = false;
				groupBox21.Visible = false;
				groupBox20.Visible = false;
				pnlGroupPOption.Visible = false;
				groupBox33.Visible = true;
				GBCombo.Visible = false;
				btnPromoAddCatDisc.Visible = false;
			}
			else if (promo_type == "2")
			{
				gpSpecial.Visible = false;
				gblimit.Visible = false;
				gpDiscount.Visible = true;
				groupBox19.Visible = false;
				groupBox15.Visible = false;
				groupBox21.Visible = false;
				groupBox20.Visible = false;
				pnlGroupPOption.Visible = false;
				groupBox33.Visible = true;
				GBCombo.Visible = false;
				btnPromoAddCatDisc.Visible = false;
			}
			else if (promo_type == "3")
			{
				gpSpecial.Visible = false;
				gpDiscount.Visible = false;
				groupBox19.Visible = false;
				groupBox15.Visible = true;
				gblimit.Visible = false;
				groupBox21.Visible = false;
				groupBox20.Visible = false;
				pnlGroupPOption.Visible = false;
				groupBox33.Visible = true;
				GBCombo.Visible = false;
				btnPromoAddCatDisc.Visible = false;
			}
			else if (promo_type == "4")
			{
				gpSpecial.Visible = false;
				gpDiscount.Visible = false;
				groupBox19.Visible = false;
				groupBox15.Visible = false;
				groupBox21.Visible = true;
				gblimit.Visible = false;
				groupBox20.Visible = false;
				pnlGroupPOption.Visible = false;
				groupBox33.Visible = true;
				GBCombo.Visible = false;
				btnPromoAddCatDisc.Visible = false;
			}
			else if (promo_type == "5")
			{
				gpSpecial.Visible = false;
				gblimit.Visible = false;
				gpDiscount.Visible = false;
				groupBox19.Visible = true;
				groupBox15.Visible = false;
				groupBox21.Visible = false;
				groupBox20.Visible = false;
				pnlGroupPOption.Visible = false;
				groupBox33.Visible = true;
				GBCombo.Visible = false;
				btnPromoAddCatDisc.Visible = false;
			}
			else if (promo_type == "6")
			{
				gpSpecial.Visible = false;
				gpDiscount.Visible = false;
				groupBox19.Visible = false;
				groupBox15.Visible = false;
				groupBox21.Visible = false;
				gblimit.Visible = false;
				groupBox20.Visible = true ;
				pnlGroupPOption.Visible = true;
				groupBox33.Visible = true;
				GBCombo.Visible = false;
				btnPromoAddCatDisc.Visible = false;
			}
			else if (promo_type == "7")
			{
				gpSpecial.Visible = false;
				gpDiscount.Visible = false;
				groupBox19.Visible = false;
				groupBox15.Visible = false;
				groupBox21.Visible = false;
				gblimit.Visible = false;
				groupBox20.Visible = false;
				pnlGroupPOption.Visible = false;
				groupBox33.Visible = false;
				GBCombo.Visible =  true;
				btnPromoAddCatDisc.Visible = false;
			}
			else if(promo_type == "8")
			{
				gpSpecial.Visible = false;
				gpDiscount.Visible = true;
				groupBox19.Visible = false;
				groupBox15.Visible = false;
				groupBox21.Visible = false;
				gblimit.Visible = false;
				groupBox20.Visible = false;
				pnlGroupPOption.Visible = false;
				groupBox33.Visible = true;
				GBCombo.Visible = false;
				btnPromoAddCatDisc.Visible = true;
			}

			int nRows = 0;
			if (dst.Tables["promolist"] != null)
				dst.Tables["promolist"].Clear();
			string sc = " SELECT p.*,  br.name AS branch_name , e.name AS stype ";
			sc += ", CONVERT(varchar(99), p.promo_start_date, 103) AS sstart_date ";
			sc += ", CONVERT(varchar(99), p.promo_end_date, 103) AS send_date ";
			sc += " FROM promotion_list p JOIN branch br ON br.id = p.promo_branch_id  ";
			sc += " JOIN enum e ON e.id = p.promo_type AND e.class = 'promotion_type' ";
			sc += " WHERE 1 = 1 ";
			if(promo_type != "0")
				sc += " AND p.promo_type = " + promo_type;
			if (txtPromo.Text != "")
				sc += " AND p.promo_desc like N'%" + txtPromo.Text + "%'";
			//sc += " ORDER BY p.promo_id DESC ";
			sc += " ORDER BY p.promo_desc ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "promolist");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			lvPromotion.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["promolist"].Rows[i];
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				string branch_name = dr["branch_name"].ToString();
				promo_type = dr["stype"].ToString();
				string start_date = dr["sstart_date"].ToString();
				string end_date = dr["send_date"].ToString();
				string active = dr["promo_active"].ToString();
				string member_only = dr["promo_member_only"].ToString();
				string limit = dr["limit"].ToString();
				string discount_percentage = dr["discount_percentage"].ToString();

				ListViewItem item = new ListViewItem(branch_name);
				item.SubItems.Add(promo_id);
				item.SubItems.Add(promo_desc);
				item.SubItems.Add(promo_type);
				item.SubItems.Add(start_date);
				item.SubItems.Add(end_date);
				item.SubItems.Add(active);
				item.SubItems.Add(member_only);
				item.SubItems.Add("del");
				item.SubItems.Add(discount_percentage);
				this.lvPromotion.Items.Add(item);
			}
		}
		private void btnPromoAddCatDisc_Click(object sender, EventArgs e)
		{
			int nDiscountPercent = Program.MyIntParse(txtPromoDiscountPercent.Text.Trim());
			if(nDiscountPercent == 0)
			{
				Program.MsgBox("Please enter a valid Discount Percentage");
				return;
			}
			string sCat = comboBox3.Text.Trim();
			if(sCat == "")
			{
				Program.MsgBox("Please select category");
				return;
			}
			string sc = " IF NOT EXISTS(SELECT promo_id FROM promotion_list WHERE promo_desc = N'" + Program.EncodeQuote(sCat) + "') ";
			sc += " INSERT INTO promotion_list (promo_desc, promo_type, discount_percentage, promo_active, promo_start_date, promo_end_date) ";
			sc += " VALUES(N'" + Program.EncodeQuote(sCat) + "', 8, " + nDiscountPercent + ", 1, GETDATE(), GETDATE()) ";
			sc += " ELSE ";
			sc += " UPDATE promotion_list SET discount_percentage = " + nDiscountPercent + " WHERE promo_type = 8 AND promo_desc = N'" + Program.EncodeQuote(sCat) + "' ";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
			}
			RefreshPromoList();
		}
		private bool m_bDontRefreshPromoList = false;
		private void cbPromoType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_bDontRefreshPromoList)
			{
				RefreshPromoList();
				Refresh();
			}
		}
		public override void Refresh()
		{
//            int nTypeId = 0;
//            int nBranchId = 1;
			string id = "-1";
			txtPromoDesc.Text = "";
			txtPromoDiscountPercent.Text = "";
			txtPromoDiscountPrice.Text = "";
			txtPromoDiscountQty.Text = "";
			txtPromoDiscountQty.Text = "";
			txtPromoRequiredQty.Text = "";
			txtPromoRewardQty.Text = "";
			txtPromoRequiredQtyOther.Text = "";
			txtPromoRewardQtyOther.Text = "";
			txtPromoFreeItemCode.Text = "";
			txtPromoSpecialPrice.Text = "";
			dtpPromoStart.Text = "";
			dtpPromoEnd.Text = "";
			txtgroupproPrice.Text = "";
			txtgroupproQTY.Text = "";
			txtPromoDiscountQty.Text = "";
			tbComboPrice.Text = "";
			txtlimit.Text = "";
			cbDay1.Checked = true;
			cbDay2.Checked = true;
			cbDay3.Checked = true;
			cbDay4.Checked = true;
			cbDay5.Checked = true;
			cbDay6.Checked = true;
			cbDay7.Checked = true;
			cbPromoActivate.Checked = true;
			cbPromoMemberOnly.Checked = false;
			lblPromoId.Text = id;
			lblPromotion.Text = "Edit Promotion NEW";
			RefreshPromoGroup(id);
		}

		private void btnPromotionNew_Click(object sender, EventArgs e)
		{
			int nTypeId = 0;
			int nBranchId = 1;
			string id = "-1";
			SelectComboBoxById(ref cbBranch, nBranchId);
			SelectComboBoxById(ref cbPromoType, nTypeId);
			txtPromoDesc.Text = "";
			txtPromoDiscountPercent.Text = "";
			txtPromoDiscountPrice.Text = "";
			txtPromoDiscountQty.Text = "";
			txtPromoDiscountQty.Text = "";
			txtPromoRequiredQty.Text = "";
			txtPromoRewardQty.Text = "";
			txtPromoRequiredQtyOther.Text = "";
			txtPromoRewardQtyOther.Text = "";
			txtPromoFreeItemCode.Text = "";
			txtPromoSpecialPrice.Text = "";
			dtpPromoStart.Text = "";
			dtpPromoEnd.Text = "";
			tbComboPrice.Text = "";
			txtgroupproPrice.Text = "";
			txtgroupproQTY.Text = "";
			txtPromoDiscountQty.Text = "";
			cbDay1.Checked = true;
			cbDay2.Checked = true;
			cbDay3.Checked = true;
			cbDay4.Checked = true;
			cbDay5.Checked = true;
			cbDay6.Checked = true;
			cbDay7.Checked = true;
			
			gpDiscount.Visible = false;
			groupBox19.Visible = false;
			groupBox21.Visible = false;
			gblimit.Visible = false;
			groupBox15.Visible = false;
			label41.Visible = false;
			groupBox20.Visible = false;
			gpSpecial.Visible = false;
			
			cbPromoActivate.Checked = true;
			cbPromoMemberOnly.Checked = false;
			lblPromoId.Text = id;
			lblPromotion.Text = "Edit Promotion NEW";
			RefreshPromoGroup(id);
		}
		private void btnPromoSave_Click(object sender, EventArgs e)
		{
			string promo_id = lblPromoId.Text.Trim();
			if (promo_id != "-1")
			{
				if (!CheckDuplicatePromotionItem(dtpPromoStart.Text, dtpPromoEnd.Text, promo_id))
					return;
			}
			KeyValuePair<int, string> kv = (KeyValuePair<int, string>)cbPromoType.SelectedItem;
			string promo_type = kv.Key.ToString().Trim();
			if (Program.MyIntParse(promo_id) <= 0)
			{
				if (!DoSaveNewPromo(txtPromoDesc.Text, promo_type))
					return;
			}
			if(!DoUpdatePromo())
				return;
			MessageBox.Show("Data successfully saved");
			btnPromoEdit_Click(null, null);
			if (promo_id != "")
				DoSelectPromotion(promo_id);
//			btnPromotionNew_Click(sender,e);
		}

		private bool CheckDuplicatePromotionItem(string start, string end, string promotion_id)
		{ 
			string sc = "";
			int rows = 0;

			if (dst.Tables["GetPromotions"] != null)
				dst.Tables["GetPromotions"].Clear();
			sc =  " SELECT * FROM promotion_list WHERE 1=1 ";
			sc += " AND promo_id = '" + promotion_id + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "GetPromotions");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (rows == 1)
			{
				DataRow dr = dst.Tables["GetPromotions"].Rows[0];
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				string promo_type = dr["promo_type"].ToString();

				if (promo_type == "6")
				{
					if (dst.Tables["GetGroupPromotionItems"] != null)
						dst.Tables["GetGroupPromotionItems"].Clear();
					int rrow = 0;
					string scc = "";
					scc =  " SELECT * FROM promotion_group WHERE 1=1 ";
					scc += " AND promo_id = '" + promo_id + "'";
					try
					{
						myAdapter = new SqlDataAdapter(scc, myConnection);
						rrow = myAdapter.Fill(dst, "GetGroupPromotionItems");
					}
					catch (Exception ex)
					{
						Program.ShowExp(sc, ex);
						myConnection.Close();
						return false;
					}
					if (rrow > 0)
					{
						for (int i = 0; i < rrow; i++)
						{
							DataRow drGroup = dst.Tables["GetGroupPromotionItems"].Rows[i];
							string barcode = drGroup["barcode"].ToString();
							if (!checkBarcodeInGroupPromoton(promo_id, barcode, start, end))
								return false;
						}
					}
					else
						return true;
				}
				else if (promo_type == "1" || promo_type == "2" || promo_type == "3" || promo_type == "4" || promo_type == "5")
				{
					if (dst.Tables["GetOtherPromotionItems"] != null)
						dst.Tables["GetOtherPromotionItems"].Clear();
					string sc1 = "";
					int rows1 = 0;

					sc1 = " SELECT c.code, b.barcode, c.name, c.promo_id, c.is_special from code_relations c ";
					sc1 += " Join promo pr on c.code = pr.code ";
					sc1 += " JOIN barcode b on c.code = b.item_code ";
					sc1 += " where 1=1 ";
					sc1 += " AND pr.promo_id = '" + promo_id + "'";
					try
					{
						myAdapter = new SqlDataAdapter(sc1, myConnection);
						rows1 = myAdapter.Fill(dst, "GetOtherPromotionItems");
					}
					catch (Exception ex)
					{
						Program.ShowExp(sc, ex);
						myConnection.Close();
						return false;
					}
					if (rows1 > 0)
					{
						for (int i = 0; i < rows1; i++)
						{
							string promoid = "0";
							DataRow dr1 = dst.Tables["GetOtherPromotionItems"].Rows[i];
							string code = dr1["code"].ToString();
							string barcode = dr1["barcode"].ToString();
							string name = dr1["name"].ToString();
							if (checkItemPromoton(promo_id, code, start, end, ref promoid))
							{
								MessageBox.Show("error, this item is already in an acitive group promotion : " + promoid + "");
								return false;
							}
						}
					}
					else
						return true;
				}
			}
			else
			{
				return false;
			}
			return true;
		}

		private bool checkItemPromoton(string promotion_id, string code, string start, string end, ref string promoid)
		{
			string sc = "";
			int rows = 0;
			if (dst.Tables["checkItemPromoton"] != null)
				dst.Tables["checkItemPromoton"].Clear();
			sc = " set dateformat dmy ";
			sc += " SELECT c.code, c.name, pr.promo_id FROM code_relations c ";
			sc += " JOIN barcode b on b.item_code = c.code ";
			sc += "   join promo pr on pr.code = c.code ";
			sc += " JOIN promotion_list pl on pl.promo_id = pr.promo_id ";
			sc += " WHERE 1=1 ";
			sc += " AND c.code = '" + code + "'";
			sc += " AND pr.promo_id <> '" + promotion_id + "'";

			sc += " AND pl.promo_start_date <= '" + end + "' ";
			sc += " AND pl.promo_end_date >= '" + start + "' ";
			//sc += " OR ";
			//sc += " (pl.promo_end_date >= '" + start + "' ";
			//sc += " AND pl.promo_start_date <= '" + end + "') ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "checkBarcodeInGroupPromoton");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (rows > 0)
			{
				//string promo_id = dst.Tables["checkBarcodeInGroupPromoton"].Rows[0]["promo_id"].ToString();
				//MessageBox.Show("error, this item is already in an acitive group promotion : " + promo_id + "");
				promoid = dst.Tables["checkBarcodeInGroupPromoton"].Rows[0]["promo_id"].ToString();
				return true;
			}
			return false;
		}

		private bool checkBarcodeInGroupPromoton(string promotion_id, string barcode,string start, string end)
		{
			string sc = "";
			int rows = 0;
			if (dst.Tables["checkBarcodeInGroupPromoton"] != null)
				dst.Tables["checkBarcodeInGroupPromoton"].Clear();
			sc = " set dateformat dmy ";
			sc += " SELECT pg.id, pl.promo_id, pl.promo_start_date, pl.promo_end_date FROM promotion_group pg ";
			sc += " JOIN promotion_list pl on pg.promo_id = pl.promo_id ";
			sc += " WHERE 1=1 ";
			sc += " AND pg.barcode = '" + barcode + "'";
			sc += " AND pg.promo_id <> '" + promotion_id + "'";
			sc += " AND pl.promo_start_date <= '" + end + "' ";
			sc += " AND pl.promo_end_date >= '" + start + "' ";
			//sc += " OR ";
			//sc += " (pl.promo_end_date >= '" + start + "' ";
			//sc += " AND pl.promo_end_date <= '" + end + "') ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "checkBarcodeInGroupPromoton");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (rows > 0)
			{
				string promo_id = dst.Tables["checkBarcodeInGroupPromoton"].Rows[0]["promo_id"].ToString();
				MessageBox.Show("error, this barcode is already in an acitive group promotion : " + promo_id + "");
				return false;
			}
			return true;
		}
		private bool DoUpdatePromo()
		{
			string promo_id = lblPromoId.Text.Trim();
			string promo_active = "0";
			string member_only = "0";
			string day1 = "0";
			string day2 = "0";
			string day3 = "0";
			string day4 = "0";
			string day5 = "0";
			string day6 = "0";
			string day7 = "0";
			if (cbPromoActivate.Checked)
				promo_active = "1";
			if (cbPromoMemberOnly.Checked)
				member_only = "1";
			if (cbDay1.Checked)
				day1 = "1";
			if (cbDay2.Checked)
				day2 = "1";
			if (cbDay3.Checked)
				day3 = "1";
			if (cbDay4.Checked)
				day4 = "1";
			if (cbDay5.Checked)
				day5 = "1";
			if (cbDay6.Checked)
				day6 = "1";
			if (cbDay7.Checked)
				day7 = "1";
			KeyValuePair<int, string> kv = (KeyValuePair<int, string>)cbBranch.SelectedItem;
			string branch_id = kv.Key.ToString().Trim();
			kv = (KeyValuePair<int, string>)cbPromoType.SelectedItem;
			string promo_type = kv.Key.ToString().Trim();
			if (branch_id == "")
				branch_id = "1";
			if (promo_type == "")
				promo_type = "1";
			string sc = " SET DATEFORMAT dmy ";
			sc += " UPDATE promotion_list ";
			sc += " SET promo_desc = N'" + Program.EncodeQuote(txtPromoDesc.Text) + "' ";
			sc += ", promo_type = " + promo_type;
			sc += ", promo_start_date = '" + dtpPromoStart.Text + "' ";
			sc += ", promo_end_date = '" + dtpPromoEnd.Text + " 23:59:59.000' ";

			DateTime promo_start_date = DateTime.Parse(dtpPromoStart.Text);
			DateTime promo_end_date = DateTime.Parse(dtpPromoEnd.Text + " 23:59:59.000");
			DateTime today = DateTime.Now;
			if (System.DateTime.Compare(today, promo_end_date) > 0 )
			{
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = "Sorry, Promotion End Date must be no earlier than today! ";
				fm.ShowDialog();
				return false ;
			}
			if (System.DateTime.Compare(promo_end_date, promo_start_date) < 0)
			{
                FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = "Sorry, Promotion End Date must be no earlier than Promotion Start Date! ";
				fm.ShowDialog();
				return false ;
			}

			sc += ", promo_active = " + promo_active;
			sc += ", promo_member_only = " + member_only;
			sc += ", promo_day1 = " + day1;
			sc += ", promo_day2 = " + day2;
			sc += ", promo_day3 = " + day3;
			sc += ", promo_day4 = " + day4;
			sc += ", promo_day5 = " + day5;
			sc += ", promo_day6 = " + day6;
			sc += ", promo_day7 = " + day7;
			sc += ", limit = "+ Program.MyIntParse(txtlimit.Text);
			sc += ", special_price = " + Program.MyMoneyParse(txtPromoSpecialPrice.Text);
			sc += ", discount_percentage = " + Program.MyMoneyParse(txtPromoDiscountPercent.Text);
			sc += ", free_qty_required_qty = " + Program.MyIntParse(txtPromoRequiredQty.Text);
			sc += ", free_qty_reward_qty = " + Program.MyIntParse(txtPromoRewardQty.Text);
			if(promo_type == "4")
				sc += ", volumn_discount_qty = " + Program.MyMoneyParse(txtPromoDiscountQty.Text);
			else if(promo_type == "6")
				sc += ", volumn_discount_qty = " + Program.MyMoneyParse(txtgroupproQTY.Text);
			if(promo_type == "4")
				sc += ", volumn_discount_price_total = " + Program.MyMoneyParse(txtPromoDiscountPrice.Text);
			else if(promo_type == "6")
				sc += ", volumn_discount_price_total = " + Program.MyMoneyParse(txtgroupproPrice.Text);
			/*************/
			else if(promo_type == "7")
				sc += ", volumn_discount_price_total = " + Program.MyMoneyParse(tbComboPrice.Text);
			/*************/

			sc += ", free_item_required_qty = " + Program.MyIntParse(txtPromoRequiredQtyOther.Text);
			sc += ", free_item_required_item_code = " + Program.MyIntParse(txtPromoFreeItemCode.Text);
			sc += ", free_item_reward_qty = " + Program.MyIntParse(txtPromoRewardQtyOther.Text);
			sc += ", promo_branch_id = " + branch_id;
			sc += " WHERE promo_id = " + promo_id;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return false;
			}
			return true;
		}
		private bool DoSaveNewPromo(string sName, string sType)
		{
			if (sType == "")
				sType = "1";
			string promo_id = "";
			string sc = " SET DATEFORMAT dmy ";
			sc += " BEGIN TRANSACTION ";
			sc += " INSERT INTO promotion_list (promo_desc, promo_type, promo_start_date, promo_end_date) VALUES(";
			sc += " N'" + sName + "', " + sType + ", GETDATE(), GETDATE() ";
			sc += ") ";
			sc += " SELECT IDENT_CURRENT('promotion_list') AS id ";
			sc += " COMMIT ";
			if (dst.Tables["promo_new"] != null)
				dst.Tables["promo_new"].Clear();
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "promo_new") <= 0)
				{
					MessageBox.Show("add new promotion failed");
					return false;
				}
				promo_id = dst.Tables["promo_new"].Rows[0]["id"].ToString();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			lblPromoId.Text = promo_id;
			return true;
		}
		private void ShowPromoItems(string id)
		{
			string kw = txtS.Text.ToLower();
			int nRows = 0;
			if (dst.Tables["itemlist"] != null)
				dst.Tables["itemlist"].Clear();
			string sc = " SELECT distinct c.code, c.supplier_code, c.name, c.price1, c.cat,c.s_cat, c.ss_cat, c.barcode, p.promo_desc ";
			sc += " , pr.promo_id ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b on b.item_code = c.code ";
			sc += " LEFT OUTER JOIN promotion_list p on p.promo_id = c.promo_id ";
			sc += " JOIN promo pr on pr.code = b.item_code ";
			sc += " WHERE 1 = 1 and c.is_special = 0 ";
			sc += " AND c.promo_id = " + id;
//			sc += " AND pr.promo_id = '" + id + "'";
			if (txtS.Text != "" && txtS.Text != null)
			{
				sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
				sc += " OR b.barcode = N'" + kw + "')";
			}
			if (comboBox3.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + comboBox3.Text.ToLower() + "'";
			if (comboBox2.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + comboBox2.Text.ToLower() + "'";
			if (comboBox1.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + comboBox1.Text.ToLower() + "'";

			sc += " ORDER BY c.code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "itemlist");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			lvItem.CheckBoxes = true;
			lvItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["itemlist"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cat = dr["cat"].ToString();
				string scat = dr["s_cat"].ToString();
				string sscat = dr["ss_cat"].ToString();
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(price1);
				item.SubItems.Add(cat);
				item.SubItems.Add(scat);
				item.SubItems.Add(sscat);
				item.SubItems.Add(promo_desc);
				this.lvItem.Items.Add(item);
			}
            ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			this.lvItem.Items.Add(sum);
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.Red;
		}
		private void GetPromotionStatus(string id)
		{
			int nRows = 0;
			string sPromoType = "";
			if (dst.Tables["GetPromotionStatus"] != null)
				dst.Tables["GetPromotionStatus"].Clear();
			string sc = "";
			sc += " SELECT * FROM promotion_list WHERE promo_id =" +id;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "GetPromotionStatus");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			DataRow dr = dst.Tables["GetPromotionStatus"].Rows[0];
			this.cbDay1.Checked = Program.MyBooleanParse(dr["promo_day1"].ToString());
			this.cbDay2.Checked = Program.MyBooleanParse(dr["promo_day2"].ToString());
			this.cbDay3.Checked = Program.MyBooleanParse(dr["promo_day3"].ToString());
			this.cbDay4.Checked = Program.MyBooleanParse(dr["promo_day4"].ToString());
			this.cbDay5.Checked = Program.MyBooleanParse(dr["promo_day5"].ToString());
			this.cbDay6.Checked = Program.MyBooleanParse(dr["promo_day6"].ToString());
			this.cbDay7.Checked = Program.MyBooleanParse(dr["promo_day7"].ToString());

			this.cbPromoActivate.Checked = Program.MyBooleanParse(dr["promo_active"].ToString());
			sPromoType = dr["promo_type"].ToString();
			switch(sPromoType)
			{
				case "1":
					gpDiscount.Visible = false;
					groupBox19.Visible = false;
					groupBox21.Visible = false;
					gblimit.Visible = false;
					groupBox15.Visible = false;
					label41.Visible = false;
					groupBox20.Visible = false;
					gpSpecial.Visible = false;
					gpSpecial.Visible = true;
					gblimit.Visible = true;;
					GBCombo.Visible = false;
					break;
				case "2":
					gpDiscount.Visible = false;
					groupBox19.Visible = false;
					groupBox21.Visible = false;
					gblimit.Visible = false;
					groupBox15.Visible = false;
					label41.Visible = false;
					groupBox20.Visible = false;
					gpSpecial.Visible = false;
					gpDiscount.Visible = true;
					GBCombo.Visible = false;
					break;
				case "3":
					gpDiscount.Visible = false;
					groupBox19.Visible = false;
					groupBox21.Visible = false;
					gblimit.Visible = false;
					groupBox15.Visible = false;
					label41.Visible = false;
					groupBox20.Visible = false;
					gpSpecial.Visible = false;
					groupBox15.Visible = true;
					GBCombo.Visible = false;
					break;
				case "4":
					gpDiscount.Visible = false;
					groupBox19.Visible = false;
					groupBox21.Visible = false;
					gblimit.Visible = false;
					groupBox15.Visible = false;
					label41.Visible = false;
					groupBox20.Visible = false;
					gpSpecial.Visible = false;
					groupBox21.Visible = true;
					GBCombo.Visible = false;
					break;
				case "5":
					gpDiscount.Visible = false;
					groupBox19.Visible = false;
					groupBox21.Visible = false;
					gblimit.Visible = false;
					groupBox15.Visible = false;
					label41.Visible = false;
					groupBox20.Visible = false;
					gpSpecial.Visible = false;
					groupBox19.Visible = true;
					GBCombo.Visible = false;
					break;
				default:
					break;
			}
		}
		private void lvPromotion_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvPromotion.SelectedItems;
			if (items == null || items.Count <= 0)
				return;
			string id = items[0].SubItems[1].Text;
			if (id.Trim() == "")
				return;
			if(items[0].GetSubItemAt(e.X, e.Y) == null)
				return;
			if (items[0].GetSubItemAt(e.X, e.Y).Text != "del" && this.cbPromoType.Text != "Group Promotion" && this.cbPromoType.Text != "Combo")
			{
				GetPromotionStatus(id);
				ShowPromoItems(id);
				m_sPromoid = id;
				return;
			}
			else if (items[0].GetSubItemAt(e.X, e.Y).Text == "del")
				DoDeletePromotion(id);
			else if (this.cbPromoType.Text == "Group Promotion" || this.cbPromoType.Text == "Combo")
			{
				GetPromotionStatus(id);
				RefreshPromoList();
//				ShowPromoItems(id);
				m_sPromoid = id;
			}
			txtS.Text = "";
			txtPromo.Text = "";
		}
		private void lvPromotion_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvPromotion.SelectedItems;
			if (items.Count <= 0)
				return;
			string id = items[0].SubItems[1].Text;
			if (id.Trim() == "")
				return;
			m_bDontRefreshPromoList = true;
//          RefreshPromoList();
			DoSelectPromotion(id);
			/***************************
			lvPromotion.BackColor = Color.White;
			ShowPromoItems(id);
			items[0].BackColor = Color.YellowGreen;
			MessageBox.Show("This Promotion Selected!!");
			/***************************/
			m_bDontRefreshPromoList = false;
			this.txtS.Text = "";
			this.txtPromo.Text = "";
		}
		private void DoDeletePromotion(string id)
		{
			if (MessageBox.Show("Are you sure you want to delete this promotion?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
				return;
			string sc = " DELETE FROM promotion_list WHERE promo_id = " + id;
			sc += " DELETE FROM promotion_group WHERE promo_id = " + id;
			sc += " DELETE FROM promo WHERE 1=1 AND promo_id = '" + id + "'";
			sc += " UPDATE code_relations SET promo_id = '' WHERE 1=1 AND promo_id = '" + id + "'";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return;
			}
			MessageBox.Show("Data successfully deleted");
			btnPromoEdit_Click(null, null);
			m_sPromoid = "";
		}
		private void DoSelectPromotion(string id)
		{
			lblPromotion.Text = "Edit Promotion #" + id;
			lblPromoId.Text = id;
			
			int nRows = 0;
			if (dst.Tables["promo"] != null)
				dst.Tables["promo"].Clear();
			string sc = " SELECT p.*,  br.name AS branch_name , e.name AS stype ";
			sc += ", CONVERT(varchar(99), p.promo_start_date, 103) AS sstart_date ";
			sc += ", CONVERT(varchar(99), p.promo_end_date, 103) AS send_date ";
			sc += " FROM promotion_list p JOIN branch br ON br.id = p.promo_branch_id  ";
			sc += " JOIN enum e ON e.id = p.promo_type AND e.class = 'promotion_type' ";
			sc += " WHERE p.promo_id = " + id;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "promo");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if(nRows <= 0)
				return;
			DataRow dr = dst.Tables["promo"].Rows[0];
			int nTypeId = Program.MyIntParse(dr["promo_type"].ToString());
			int nBranchId = Program.MyIntParse(dr["promo_branch_id"].ToString());
			SelectComboBoxById(ref cbBranch, nBranchId);
			SelectComboBoxById(ref cbPromoType, nTypeId);
			txtPromoDesc.Text = dr["promo_desc"].ToString();
			txtPromoSpecialPrice.Text = dr["special_price"].ToString();
			txtPromoDiscountPercent.Text = dr["discount_percentage"].ToString();
			txtPromoDiscountPrice.Text = dr["volumn_discount_price_total"].ToString();
			txtPromoDiscountQty.Text = dr["volumn_discount_qty"].ToString();

			txtgroupproQTY.Text = dr["volumn_discount_qty"].ToString();
			txtgroupproPrice.Text = dr["volumn_discount_price_total"].ToString();
			/***********/
			tbComboPrice.Text = dr["volumn_discount_price_total"].ToString();
			/*********/
			txtlimit.Text = dr["limit"].ToString();

			txtPromoRequiredQty.Text = dr["free_qty_required_qty"].ToString();
			txtPromoRewardQty.Text = dr["free_qty_reward_qty"].ToString();
			txtPromoRequiredQtyOther.Text = dr["free_item_required_qty"].ToString();
			txtPromoRewardQtyOther.Text = dr["free_item_reward_qty"].ToString();
			txtPromoFreeItemCode.Text = dr["free_item_required_item_code"].ToString();
			dtpPromoStart.Text = dr["sstart_date"].ToString();
			dtpPromoEnd.Text = dr["send_date"].ToString();
			
			if(Program.MyBooleanParse(dr["promo_day1"].ToString()))
				cbDay1.Checked = true;
			if(Program.MyBooleanParse(dr["promo_day2"].ToString()))
				cbDay2.Checked = true;
			if(Program.MyBooleanParse(dr["promo_day3"].ToString()))
				cbDay3.Checked = true;
			if(Program.MyBooleanParse(dr["promo_day4"].ToString()))
				cbDay4.Checked = true;
			if(Program.MyBooleanParse(dr["promo_day5"].ToString()))
				cbDay5.Checked = true;
			if(Program.MyBooleanParse(dr["promo_day6"].ToString()))
				cbDay6.Checked = true;
			if(Program.MyBooleanParse(dr["promo_day7"].ToString()))
				cbDay7.Checked = true;
			if(Program.MyBooleanParse(dr["promo_active"].ToString()))
				cbPromoActivate.Checked = true;
			if(Program.MyBooleanParse(dr["promo_member_only"].ToString()))
				cbPromoMemberOnly.Checked = true;
			RefreshPromoGroup(id);
		}
		private void RefreshPromoGroup(string promo_id)
		{
			lvPromoGB.Items.Clear();
			lvCombo.Items.Clear();
			int nRows = 0;
			if (dst.Tables["pb"] != null)
				dst.Tables["pb"].Clear();
			string sc = " SELECT  pg.barcode ";
			sc += " , pg.promo_type ";
			sc += " , c1.code, c1.name, c1.price1 ";
			sc += " , c2.code AS code2, c2.name AS name2, c2.price1 AS price1_2";
			sc += " FROM promotion_group pg ";
			sc += " LEFT OUTER JOIN barcode b on b.barcode = pg.barcode ";
			sc += " LEFT OUTER JOIN code_relations c1 ON c1.code = b.item_code ";
			sc += " LEFT OUTER JOIN code_relations c2 ON c2.barcode = pg.barcode ";
			sc += " WHERE pg.promo_id = " + promo_id;
			sc += " AND b.barcode <> '' ";
			sc += " ORDER BY pg.id ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "pb");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (nRows <= 0)
				return;
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["pb"].Rows[i];
				string barcode = dr["barcode"].ToString();
				string name = dr["name"].ToString();
				string promo_type = dr["promo_type"].ToString();
				double dPrice = Program.MyDoubleParse(dr["price1"].ToString());
				if(name == "")
				{
					name = dr["name2"].ToString();
					dPrice = Program.MyDoubleParse(dr["price1_2"].ToString());
				}

				ListViewItem item = new ListViewItem(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(dPrice.ToString("c"));
				item.SubItems.Add("del");
				if (promo_type == "6")
				{
					this.lvPromoGB.Items.Add(item);
				}
				else if (promo_type == "7")
				{
					this.lvCombo.Items.Add(item);
				}
			}
		}		
		private void txtPromoBarcode_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\r')
				return;
			DoAddGroupPromotionItem();
		}
		private void txtgroupproQTY_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\r')
				return;
			txtgroupproPrice.Focus();
		}
		private void txtgroupproPrice_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\r')
				return;
			btnPromoSave.Focus();
		}
		private void btnAddGPBarcode_Click(object sender, EventArgs e)
		{
			DoAddGroupPromotionItem();
		}
		private bool DoAddComboItem()
		{
			string promo_type = "";
			DateTime promo_start_date = DateTime.Parse(dtpPromoStart.Text);
			DateTime promo_end_date = DateTime.Parse(dtpPromoEnd.Text + " 23:59:59.000");
			DateTime today = DateTime.Now;
			KeyValuePair<int, string> kv = (KeyValuePair<int, string>)cbPromoType.SelectedItem;
			promo_type = kv.Key.ToString().Trim();
			if (Program.MyIntParse(lblPromoId.Text) <= 0)
			{

				if (!DoSaveNewPromo(txtPromoDesc.Text, promo_type))
				{
					//					MessageBox.Show("Failed to save new promotion");
					return false;
				}
				if (!DoUpdatePromo())
					return false;
			}
			string barcode = this.tbComboBarcode.Text;
			if (barcode == "")
			{
				Program.MsgBox("Please Keyin barcode to Combo promotion");
				return true;
			}
			int nRows = 0;
			if (dst.Tables["ab"] != null)
				dst.Tables["ab"].Clear();
			string sc = " SELECT TOP 1 c.code, c.name, c.price1 ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE c.barcode = '" + Program.EncodeQuote(barcode) + "' ";
			sc += " OR b.barcode = '" + Program.EncodeQuote(barcode) + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "ab");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (nRows <= 0)
			{
				MessageBox.Show("barcode not found");
				return false;
			}
			double dPrice = Program.MyDoubleParse(dst.Tables["ab"].Rows[0]["price1"].ToString());
			string code = dst.Tables["ab"].Rows[0]["code"].ToString();
/***check if this item is in another promotion****/
			if (dst.Tables["abb"] != null)
				dst.Tables["abb"].Clear();
			sc = " set dateformat dmy ";
			sc += " SELECT pg.id, pl.promo_id, pl.promo_start_date, pl.promo_end_date FROM promotion_group pg ";
			sc += " JOIN promotion_list pl on pg.promo_id = pl.promo_id ";
			sc += " WHERE pg.barcode = '" + Program.EncodeQuote(barcode) + "' ";
			sc += " AND pl.promo_start_date <= '" + dtpPromoEnd.Text + "' ";
			sc += " AND pl.promo_end_date >= '" + dtpPromoStart.Text + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "abb") > 0)
				{
					string promo_id = dst.Tables["abb"].Rows[0]["promo_id"].ToString();
					MessageBox.Show("error, this barcode is already in an acitive promotion : " + promo_id + "");
					return false;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
/*************/
			sc = " IF NOT EXISTS(SELECT id FROM promotion_group WHERE promo_id = " + lblPromoId.Text + " ";
			/*******/
			sc += " AND promo_type = '" + promo_type + "'";
			/********/
			sc += " AND barcode = '" + Program.EncodeQuote(barcode) + "') ";
			sc += " INSERT INTO promotion_group(promo_id, barcode, promo_type) VALUES(" + lblPromoId.Text + ", '" + Program.EncodeQuote(barcode) + "', '" + promo_type + "') ";
			/******/
			sc += " IF NOT EXISTS(select code from promo WHERE 1=1 AND promo_id = '" + lblPromoId.Text + "' AND code = '" + code + "')";
			sc += " INSERT INTO promo(promo_id, code)";
			sc += " VALUES('" + lblPromoId.Text + "', '" + code + "')";
			/******/
			sc += " UPDATE code_relations SET promo_id = " + lblPromoId.Text + " WHERE barcode = '" + Program.EncodeQuote(barcode) + "' ";
			sc += " IF NOT EXISTS(SELECT id FROM barcode WHERE item_code = '" + Program.EncodeQuote(code) + "' ";
			sc += " AND barcode = '" + Program.EncodeQuote(barcode) + "') ";
			sc += " INSERT INTO barcode(item_code, barcode, item_qty)VALUES(" + Program.EncodeQuote(code) + ", " + Program.EncodeQuote(barcode) + ", '1')";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return false;
			}
			RefreshPromoGroup(lblPromoId.Text);
			MessageBox.Show("Barcode successfully added");
			this.tbComboBarcode.Text = "";
			return true;
		}
		private bool DoAddGroupPromotionItem()
		{
			string promo_type = "";
			DateTime promo_start_date = DateTime.Parse(dtpPromoStart.Text);
			DateTime promo_end_date = DateTime.Parse(dtpPromoEnd.Text + " 23:59:59.000");
			DateTime today = DateTime.Now;
			KeyValuePair<int, string> kv = (KeyValuePair<int, string>)cbPromoType.SelectedItem;
			promo_type = kv.Key.ToString().Trim();

			if (Program.MyIntParse(lblPromoId.Text) <= 0)
			{
				if (!DoSaveNewPromo(txtPromoDesc.Text, promo_type))
				{
//					MessageBox.Show("Failed to save new promotion");
					return false;
				}
				if (!DoUpdatePromo())
					return false;
			}
			string barcode = txtPromoBarcode.Text;
			return DoAddGroupPromotionItemByBarcode(barcode);
		}
		private bool DoAddGroupPromotionItemByBarcode(string barcode)
		{
			if(barcode == "")
			{
				Program.MsgBox("Please enter barcode to add to group promotion");
				return true;
			}

			KeyValuePair<int, string> kv = (KeyValuePair<int, string>)cbPromoType.SelectedItem;
			string promo_type = kv.Key.ToString().Trim();

			int nRows = 0;
			if (dst.Tables["ab"] != null)
				dst.Tables["ab"].Clear();
			string sc = " SELECT TOP 1 c.code, c.name, c.price1 ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE c.barcode = '" + Program.EncodeQuote(barcode) + "' ";
			sc += " OR b.barcode = '" + Program.EncodeQuote(barcode) + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "ab");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			if (nRows <= 0)
			{
				MessageBox.Show("barcode not found");
				return false;
			}
			double dPrice = Program.MyDoubleParse(dst.Tables["ab"].Rows[0]["price1"].ToString());
			string code = dst.Tables["ab"].Rows[0]["code"].ToString();

			if (dst.Tables["abb"] != null)
				dst.Tables["abb"].Clear();
			//sc = " SELECT id FROM promotion_group WHERE barcode = '" + Program.EncodeQuote(barcode) + "' ";
			sc = " set dateformat dmy ";
			sc += " SELECT pg.id, pl.promo_id, pl.promo_start_date, pl.promo_end_date FROM promotion_group pg ";
			sc += " JOIN promotion_list pl on pg.promo_id = pl.promo_id ";
			sc += " WHERE pg.barcode = '" + Program.EncodeQuote(barcode) + "' ";
            sc += " AND pl.promo_start_date <= '" + dtpPromoEnd.Text + "' ";
            sc += " AND pl.promo_start_date >= '" + dtpPromoStart.Text + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "abb") > 0)
				{
					string promo_id = dst.Tables["abb"].Rows[0]["promo_id"].ToString();
					MessageBox.Show("error, this barcode is already in an acitive group promotion : " + promo_id + "");
					return false;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}

			if (dst.Tables["abb"] != null)
				dst.Tables["abb"].Clear();
			sc = " set dateformat dmy ";
			sc += " SELECT pg.id, pl.promo_id, pl.promo_start_date, pl.promo_end_date FROM promotion_group pg ";
			sc += " JOIN promotion_list pl on pg.promo_id = pl.promo_id ";
			sc += " WHERE pg.barcode = '" + Program.EncodeQuote(barcode) + "' ";
            sc += " AND pl.promo_end_date >= '" + dtpPromoStart.Text + "' ";
            sc += " AND pl.promo_end_date <= '" + dtpPromoEnd.Text + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "abb") > 0)
				{
					string promo_id = dst.Tables["abb"].Rows[0]["promo_id"].ToString();
					MessageBox.Show("error, this barcode is already in an acitive group promotion : " + promo_id + "");
					return false;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}

			sc = " IF NOT EXISTS(SELECT id FROM promotion_group WHERE promo_id = " + lblPromoId.Text + " ";
			sc += " AND promo_type = '"+promo_type+"'";
			sc += " AND barcode = '" + Program.EncodeQuote(barcode) + "') ";
			sc += " INSERT INTO promotion_group(promo_id, barcode, promo_type) VALUES(" + lblPromoId.Text + ", '" + Program.EncodeQuote(barcode) + "', '"+promo_type+"') ";

			/******/
			sc += " IF NOT EXISTS(select code from promo WHERE 1=1 AND promo_id = '" + lblPromoId.Text + "' AND code = '" + code + "')";
			sc += " INSERT INTO promo(promo_id, code)";
			sc += " VALUES('" + lblPromoId.Text + "', '" + code + "')";
			/******/
			sc += " UPDATE code_relations SET promo_id = " + lblPromoId.Text + " WHERE barcode = '" + Program.EncodeQuote(barcode) + "' ";
			sc +=  " IF NOT EXISTS(SELECT id FROM barcode WHERE item_code = '" + Program.EncodeQuote(code) + "' ";
			sc += " AND barcode = '" + Program.EncodeQuote(barcode) + "') ";
			sc += " INSERT INTO barcode(item_code, barcode, item_qty)VALUES(" +Program.EncodeQuote(code)+ ", "+Program.EncodeQuote(barcode)+ ", '1')";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return false;
			}
			RefreshPromoGroup(lblPromoId.Text);
			MessageBox.Show("Barcode successfully added");
			txtPromoBarcode.Text = "";
			return true;
		}

		private string Getcodebybarcode(string barcode)
		{
			if (dst.Tables["Getcodebybarcode"] != null)
				dst.Tables["Getcodebybarcode"].Clear();
			string sc = "";
			int rows = 0;
			string code = "0";
			sc = " SELECT item_code as code FROM barcode WHERE 1=1 ";
			sc += " AND barcode = '" + barcode + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "Getcodebybarcode") > 0)
				{
					code = dst.Tables["Getcodebybarcode"].Rows[0]["code"].ToString();
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return "0";
			}
			return code;
		}

		private void lvPromoGB_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvPromoGB.SelectedItems;
			if (items.Count <= 0)
				return;
			if (items[0].GetSubItemAt(e.X, e.Y).Text != "del")
				return;
			string barcode = items[0].SubItems[0].Text;
//			if (barcode.Trim() == "")
//				return;
			DoDeletePromotionGB(lblPromoId.Text, barcode);
		}
		private void DoDeletePromotionGB(string promo_id, string barcode)
		{
			string sc = " DELETE FROM promotion_group WHERE promo_id = " + promo_id + " AND barcode = '" + Program.EncodeQuote(barcode) + "' ";
			sc += " DELETE FROM promo WHERE 1=1 ";
			sc += " AND promo_id = " + promo_id + " AND code = '" + Getcodebybarcode(Program.EncodeQuote(barcode)) + "' ";
			sc += " UPDATE code_relations SET promo_id = NULL WHERE promo_id = " + promo_id + " AND barcode = '" + Program.EncodeQuote(barcode) + "' ";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myCommand.Connection.Close();
				return;
			}
			RefreshPromoGroup(lblPromoId.Text);
			MessageBox.Show("Barcode successfully deleted");
		}
		private void SelectComboBoxById(ref ComboBox cb, int nId)
		{
			cb.SelectedIndex = 0;
			for (int i = 0; i < cb.Items.Count; i++)
			{
				KeyValuePair<int, string> kv = (KeyValuePair<int, string>)cb.Items[i];
				int cbId = kv.Key;
				if (cbId == nId)
				{
					cb.SelectedIndex = i;
					break;
				}
			}
		}
		private void PrintBranchOptions()
		{
			if (dst.Tables["branch"] != null)
				dst.Tables["branch"].Clear();
			int nRows = 0;
			string sc = " SELECT id, name FROM branch WHERE activated = 1 ORDER BY name ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "branch");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			var dict = new Dictionary<int, string>();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["branch"].Rows[i];
				int nId = Program.MyIntParse(dr["id"].ToString());
				string name = dr["name"].ToString();
				dict.Add(nId, name);
			}
			cbBranch.DataSource = new BindingSource(dict, null);
			cbBranch.DisplayMember = "Value";
			cbBranch.ValueMember = "Key";
		}
		private void PrintPromoTypeOptions()
		{
			int nRows = 0;
			if (dst.Tables["enum"] != null)
				dst.Tables["enum"].Clear();
			string sc = " SELECT id, name FROM enum WHERE class = 'promotion_type' ORDER BY id ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "enum");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if(nRows <= 0)
				return;
			var dict = new Dictionary<int, string>();
			dict.Add(0, "");
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["enum"].Rows[i];
				int nId = Program.MyIntParse(dr["id"].ToString());
				string name = dr["name"].ToString();
				dict.Add(nId, name);
			}
			cbPromoType.DataSource = new BindingSource(dict, null);
			cbPromoType.DisplayMember = "Value";
			cbPromoType.ValueMember = "Key";
		}
		private void btnButtonEdit_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			BuildMenuButtons();
			pnlButton.Visible = true;

			int nWidth = 826;
			int nHeight = 670;
			int nLeft = 3;
			int nTop = 3;
			gbCategory.Size = new Size(nWidth, nHeight);
			gbCategory.Location = new Point(nLeft, nTop);
//			gbTouchScreenLayout.Size = new Size(nWidth, nHeight);
//			gbTouchScreenLayout.Location = new Point(nLeft, nTop);
			gbBatchAdd.Size = new Size(nWidth, nHeight);
			gbBatchAdd.Location = new Point(nLeft, nTop);
			gbButtonEditButton.Size = new Size(nWidth, nHeight);
			gbButtonEditButton.Location = new Point(nLeft, nTop);
			
			panelMenuPopup.Size = new Size(540, 235);
			panelMenuPopup.Location = new Point(31, 195);

			PanelButtonHideAll();
			gbTouchScreenLayout.Location = new Point(1, 1);
			gbTouchScreenLayout.Visible = true;
			lblButtonButtonId.Text = "-1";
			lblButtonButtonItemId.Text = "-1";
		}
		private bool BuildMenuButtons()
		{
//			gbTouchScreenLayout.Size = new Size(826, 670);
//			gbTouchScreenLayout.Location = new Point(3, 3);
			gbTouchScreenLayout.Visible = true;
			panelMenuTopRight.Controls.Clear();
			panelMenuLeft.Controls.Clear();
			int nRows = 0;
			if (dst.Tables["menu_buttons"] != null)
				dst.Tables["menu_buttons"].Clear();
			string sc = " SELECT * FROM button ORDER BY id ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "menu_buttons");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false; ;
			}

			int nWidth = 60;
			int nHeight = 40;
			//build top right menu
//			for (int i = 0; i < 18; i++)
			for (int i = 0; i < 6; i++)
			{
				string id = "";
				string name = "";
				string name_en = "";

				if (i < nRows)
				{
					DataRow dr = dst.Tables["menu_buttons"].Rows[i];
					id = dr["id"].ToString();
					name = dr["name"].ToString();
					name_en = dr["name_en"].ToString();
//                  m_bIndivisual = Program.MyBooleanParse(dr["is_indivisual"].ToString());
					m_nameEn[i] = name_en;
					m_nameCn[i] = name;
				}
				if (Program.m_bLanguage_en)
					name = name_en;
				if (name == "")
					name = id;
				//if(i == 0) //disabled first button, used for Phone Card
				//{
				//    name_en = "Phone Card";
				//    name = name_en;
				//}
				Button newCatButton = new Button();
				newCatButton.BackColor = System.Drawing.Color.Transparent;
				newCatButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
				newCatButton.ForeColor = System.Drawing.Color.MidnightBlue;

				if (name_en.ToLower() == "del")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.red;
					newCatButton.ForeColor = System.Drawing.Color.White;
				}
				else if (name_en.ToLower() == "up" || name_en.ToLower() == "down" || name_en.ToLower() == "lang" || name_en.ToLower() == "cn/en" || name_en.ToLower() == "lang" || name_en.ToLower() == "item disc" || name_en.ToLower() == "box" || name_en.ToLower() == "receipt" || name_en.ToLower() == "search" || name_en.ToLower() == "weight" || name_en.ToLower() == "dump sale")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.green;
				}
				newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
				newCatButton.FlatStyle = FlatStyle.Flat;
				newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
				newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
				newCatButton.Width = nWidth;
				newCatButton.Height = nHeight;
				newCatButton.AutoSize = false;
				newCatButton.BackColor = System.Drawing.Color.Transparent;
				newCatButton.FlatAppearance.BorderSize = 0;
				newCatButton.Text = name;
				newCatButton.Font = new Font("Arial", 8, FontStyle.Regular);
				newCatButton.Name = "Cat" + id;
				newCatButton.UseVisualStyleBackColor = true;
				//if(i > 0)
					newCatButton.Click += new EventHandler(newCatButton_Click);
//				newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);
				panelMenuTopRight.Controls.Add(newCatButton);
			}
			//build left menu
			for (int i = 18; i < 50; i++)
			{
				string id = "";
				string name = "";
				string name_en = "";
				if (i < nRows)
				{
					DataRow dr = dst.Tables["menu_buttons"].Rows[i];
					id = dr["id"].ToString();
					name = dr["name"].ToString();
					name_en = dr["name_en"].ToString();
//                  m_bIndivisual = Program.MyBooleanParse(dr["is_indivisual"].ToString());
					m_nameEn[i] = name_en;
					m_nameCn[i] = name;
				}
				if (Program.m_bLanguage_en)
					name = name_en;
				if (name == "")
					name = id;
				Button newCatButton = new Button();
				newCatButton.BackColor = System.Drawing.Color.Transparent;
				newCatButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
				if (name_en.ToLower() == "receipt")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.green;
				}
				newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
				newCatButton.FlatStyle = FlatStyle.Flat;
				newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
				newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
				newCatButton.Width = nWidth;
				newCatButton.Height = nHeight;
				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					newCatButton.Height = 56;
				}
				newCatButton.AutoSize = false;
				newCatButton.BackColor = System.Drawing.Color.Transparent;
				newCatButton.FlatAppearance.BorderSize = 0;
				newCatButton.ForeColor = System.Drawing.Color.MidnightBlue;
				newCatButton.Text = name;
				newCatButton.Font = new Font("Arial", 8, FontStyle.Regular);
				newCatButton.Name = "Cat" + id;
				newCatButton.UseVisualStyleBackColor = true;
				newCatButton.Click += new EventHandler(newCatButton_Click);
//				newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);
				panelMenuLeft.Controls.Add(newCatButton);
			}
			return true;
		}
		private bool getIndi(string button_id)
		{
			int nRows = 0;
			string indi = "";
			if (dst.Tables["getIndi"] != null)
				dst.Tables["getIndi"].Clear();
			string sc = " SELECT is_indivisual FROM button WHERE 1=1 ";
			sc += " AND id = " + button_id;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "getIndi");
				
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false; ;
			}
			indi = dst.Tables["getIndi"].Rows[0]["is_indivisual"].ToString();
			if (Program.MyBooleanParse(indi))
				return true;
			else
				return false;
		}
		private void newCatButton_Click(object sender, EventArgs e)
		{
			Button CurrentButton = (Button)sender;
			string btn_name = CurrentButton.Name;
			if (btn_name.Length <= 3)
				return;
			string button_id = btn_name.Substring(3, btn_name.Length - 3);
			m_sButton_id = button_id;
			ckbIndivisual.Checked = getIndi(button_id);

			if (!ckbIndivisual.Checked)
			{
				pictureBox2.Visible = true;
				btnCatOpen.Visible = true;
				btnCatUpload.Visible = true;
				btnDel2.Visible = true;
				ckbIndivisual.Visible = true;
			
				ButtonBuildCategory(button_id);
				PanelButtonHideAll();
				gbCategory.Visible = true;
				panelii.Visible = false;
				if (File.Exists(Program.m_sPicroot + "\\" + txtButtonCatName.Text + ".bmp"))
				{
					string Path = Program.m_sPicroot + "\\" + txtButtonCatName.Text + ".bmp";
					Image b;
					using (var bmpTemp = new Bitmap(Path))
					{
						b = new Bitmap(bmpTemp);
					}
					this.pictureBox2.Image = b;//new Bitmap(Path);
					pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
				}
				else
				{
					pictureBox2.Image = null;
				}
			}
			else
			{
				pictureBox2.Visible = true;
				btnCatOpen.Visible = true;
				btnCatUpload.Visible = true;
				btnDel2.Visible = true;
				ckbIndivisual.Visible = true;
				
				panelMenuPopup.Controls.Clear();
				string btn_name_en = m_nameEn[Program.MyIntParse(button_id) - 1];
				string btn_name_cn = m_nameCn[Program.MyIntParse(button_id) - 1];
				lblButtonButtonId.Text = button_id;
				txtButtonCatName.Text = btn_name_en;
				txtButtonCatNameCN.Text = btn_name_cn;
				txtCodeii.Text = GetSiteSettings("button_id_"+button_id);
				panelii.Visible = true;
				PanelButtonHideAll();
				gbCategory.Visible = true;
				if (File.Exists(Program.m_sPicroot + "\\" + txtButtonCatName.Text + ".bmp"))
				{
					string Path = Program.m_sPicroot + "\\" + txtButtonCatName.Text + ".bmp";
					Image b;
					using (var bmpTemp = new Bitmap(Path))
					{
						b = new Bitmap(bmpTemp);
					}
					this.pictureBox2.Image = b;//new Bitmap(Path);
					pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
				}
				else
				{
					pictureBox2.Image = null;
				}
			}
		}
		private void ButtonBuildCategory(string button_id)
		{
			panelMenuPopup.Controls.Clear();
			string btn_name_en = m_nameEn[Program.MyIntParse(button_id) - 1];
			string btn_name_cn = m_nameCn[Program.MyIntParse(button_id) - 1];
			lblButtonButtonId.Text = button_id;
			txtButtonCatName.Text = btn_name_en;
			txtButtonCatNameCN.Text = btn_name_cn;
			int nRows = 0;
			if (dst.Tables["menu_buttons"] != null)
				dst.Tables["menu_buttons"].Clear();
			string sc = " SELECT i.*, c.supplier_code ";
			sc += " FROM button_item i JOIN code_relations c ON c.code = i.code ";
			sc += " WHERE i.button_id = " + button_id;
			sc += " ORDER BY i.id ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "menu_buttons");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			int nWidth = 60;
			int nHeight = 40;
			if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
			{
				nHeight = 72;
			}
			for (int i = 0; i < 40; i++)
			{
				string id = "";
				string name = "";
				string barcode = "";
				string location = "";
				string name_en = "";

				if (!Program.checkLocation())
				{
					if (i < nRows)
					{
						DataRow dr = dst.Tables["menu_buttons"].Rows[i];
						id = dr["id"].ToString();
						name = dr["name"].ToString();
						name_en = dr["name_en"].ToString();
						barcode = dr["supplier_code"].ToString();
					}
				}
				else
				{
					for (int j = 0; j < nRows; j++)
					{
						DataRow dr = dst.Tables["menu_buttons"].Rows[j];
						location = dr["location"].ToString();
						if (i == Program.MyIntParse(location))
						{
							id = dr["id"].ToString();
							name = dr["name"].ToString();
							name_en = dr["name_en"].ToString();
							barcode = dr["supplier_code"].ToString();
						}
						else
						{

						}
					}
				}
				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					if (i == 23)
					{
						barcode = "";
						name = "关闭";
						name_en = "CLOSE";
						
					}
				}
				if (i == 39) //the last one
				{
					barcode = "";
					name = "关闭";
					name_en = "CLOSE";
				}
				if (Program.m_bLanguage_en)
					name = name_en;
				if(id == "")
					id = "-" + i.ToString();
				Button newItemButton = new Button();
				newItemButton.BackColor = System.Drawing.Color.Transparent;
				newItemButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
				newItemButton.BackgroundImageLayout = ImageLayout.Stretch;
				newItemButton.FlatStyle = FlatStyle.Flat;
				newItemButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
				newItemButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
				newItemButton.Width = nWidth;
				newItemButton.Height = nHeight;
				newItemButton.AutoSize = false;
				newItemButton.BackColor = System.Drawing.Color.Transparent;
				newItemButton.FlatAppearance.BorderSize = 0;
				newItemButton.ForeColor = System.Drawing.Color.MidnightBlue;
				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					if(i == 23)
						newItemButton.ForeColor = System.Drawing.Color.Red;
				}
				else if (i == 39)
					newItemButton.ForeColor = System.Drawing.Color.Red;
				newItemButton.Text = name;
				newItemButton.Font = new Font("Arial", 8, FontStyle.Regular);
				if (!Program.checkLocation())
					newItemButton.Name = "Item" + id;
				else
					newItemButton.Name = "Item" + i;
				newItemButton.UseVisualStyleBackColor = true;
				newItemButton.Click += new EventHandler(newItemButton_Click);
//				newItemButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);
				panelMenuPopup.Controls.Add(newItemButton);
			}
		}
		private void PanelButtonHideAll()
		{
			gbTouchScreenLayout.Visible = false;
			gbCategory.Visible = false;
			gbBatchAdd.Visible = false;
			gbButtonEditButton.Visible = false;
		}
		private void btnCatBack_Click(object sender, EventArgs e)
		{
			PanelButtonHideAll();
			BuildMenuButtons();
			gbTouchScreenLayout.Visible = true;
			txtCodeii.Text = "";
			txtkey.Text = "";
		}
		private void btnCatCatSave_Click(object sender, EventArgs e)
		{
			string id = lblButtonButtonId.Text;
			string name = txtButtonCatName.Text;
			string name_cn = txtButtonCatNameCN.Text;
			bool indivisual = ckbIndivisual.Checked;
			string sii = "";
			if (indivisual)
				sii = "1";
			else
				sii = "0";
			string sc = " UPDATE button SET name = N'" + Program.EncodeQuote(name_cn) + "' ";
			sc += ", name_en = N'" + Program.EncodeQuote(name) + "' ";
			sc += ", is_indivisual = " + sii;
			sc += " WHERE id = " + id;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			MessageBox.Show("Button settings saved!");
		}
		private void btnCatBatchAdd_Click(object sender, EventArgs e)
		{
			PanelButtonHideAll();
			gbBatchAdd.Visible = true;
			string kw = txtButtonCatName.Text;
			txtButtonKW.Text = kw;
			PanelButtonDoBatchAddSearch(kw);
		}
		private void btbButtonBatchSearch_Click(object sender, EventArgs e)
		{
			string kw = txtButtonKW.Text.Trim();
			PanelButtonDoBatchAddSearch(kw);
		}
		private void PanelButtonDoBatchAddSearch(string kw)
		{
			int nRows = 0;
			if(dst.Tables["batch_items"] != null)
				dst.Tables["batch_items"].Clear();
			string sc = " SELECT TOP 300 c.cat, c.s_cat, c.code, c.supplier_code ";
			sc += ", c.name, c.name_cn, i.id AS biid ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
			sc += " WHERE 1 = 1 ";
			if (kw != "")
			{
				sc += " AND c.name LIKE N'%" + Program.EncodeQuote(kw) + "%' COLLATE SQL_Latin1_General_CP1_CI_AS ";
				sc += " OR c.name_cn LIKE N'%" + Program.EncodeQuote(kw) + "%' COLLATE SQL_Latin1_General_CP1_CI_AS ";
			}
			sc += " ORDER BY c.name ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "batch_items");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			for(int i=0; i<nRows; i++)
			{
				DataRow dr = dst.Tables["batch_items"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
				string biid = dr["biid"].ToString();
				name = name.Replace("'", "");
				name_cn = name_cn.Replace("'", "");

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add("");
				if(biid != "")
					item.Selected = true;
				this.lvButtonBatchItem.Items.Add(item);
			}
		}
		private void btnButtonBatchAddSelected_Click(object sender, EventArgs e)
		{
			ListView.SelectedListViewItemCollection items = lvButtonBatchItem.SelectedItems;

			string bid = lblButtonButtonId.Text;
			string sc = "";
			for(int i=0; i<items.Count; i++)
			{
				string code = items[i].SubItems[0].Text;
				string name = items[i].SubItems[2].Text;
				string name_cn = items[i].SubItems[3].Text;
				sc += " IF NOT EXISTS(SELECT id FROM button_item WHERE button_id = " + bid + " AND code = " + code + ") ";
				sc += " INSERT INTO button_item (button_id, code, name, name_en) VALUES(";
				sc += bid + ", " + code + ", N'" + Program.EncodeQuote(name_cn) + "', N'" + Program.EncodeQuote(name) + "'); ";
			}
			if(sc == "")
				return;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch(Exception e1) 
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
			}
		}
		private void btnButtonBatchAddBack_Click(object sender, EventArgs e)
		{
			PanelButtonHideAll();
			string button_id = lblButtonButtonId.Text;
			ButtonBuildCategory(button_id);
			lblButtonEditItemCode.Text = "";
			txtButtonEditSearchKW.Text = "";
			txtButtonEditSearchBarcode.Text = "";
		}
		private void btnButtonEditBack_Click(object sender, EventArgs e)
		{
			PanelButtonHideAll();
			gbTouchScreenLayout.Visible = true;
			string button_id = lblButtonButtonId.Text;
			ButtonBuildCategory(button_id);
			lblButtonEditItemCode.Text = "";
			txtButtonEditSearchKW.Text = "";
			txtButtonEditSearchBarcode.Text = "";
		}
		private void newItemButton_Click(object sender, EventArgs e)
		{
			Button CurrentButton = (Button)sender;
			string biid = "";
			string name = CurrentButton.Name.Trim();
			if (name.Length <= 4)
				return;
			biid = name.Substring(4, name.Length - 4);
			m_sSubButtonid = biid;
			PanelButtonHideAll();
			gbButtonEditButton.Visible = true;
			
			string item_name = "";
			string item_name_cn = "";
			string button_name = "";
			string button_name_cn = "";
			string barcode = "";
			string code = "";

			if(Program.MyIntParse(biid) >= 0)
			{
				lblButtonButtonItemId.Text = biid;
				
				if(dst.Tables["bi"] != null)
					dst.Tables["bi"].Clear();
				int nRows = 0;
				string sc = " SELECT bi.code, bi.name_en AS button_name, bi.name AS button_name_cn ";
				sc += ", c.name AS item_name, c.name_cn AS item_name_cn, b.barcode ";
				sc += " FROM button_item bi ";
				sc += " LEFT OUTER JOIN code_relations c ON c.code = bi.code ";
				sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
				if (!Program.checkLocation())
					sc += " WHERE bi.id = " + biid;
				else
				{
					sc += " WHERE bi.button_id = " + m_sButton_id;
					sc += " AND bi.location = " + biid;
				}
				try
				{
					SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
					nRows = myCommand.Fill(dst, "bi");
				}
				catch (Exception e1)
				{
					myConnection.Close();
					Program.ShowExp(sc, e1);
					return;
				}

				if(nRows > 0)
				{
					DataRow dr = dst.Tables["bi"].Rows[0];
					item_name = dr["item_name"].ToString();
					item_name_cn = dr["item_name_cn"].ToString();
					button_name = dr["button_name"].ToString();
					button_name_cn = dr["button_name_cn"].ToString();
					barcode = dr["barcode"].ToString();
					barcode = dr["code"].ToString();
				}
			}
			lblButtonEditItemCode.Text = code;
			txtButtonEditName.Text = item_name;
			txtButtonEditNameCN.Text = item_name_cn;
			txtButtonEditButtonName.Text = button_name;
			txtButtonEditButtonNameCN.Text = button_name_cn;
			txtButtonEditSearchBarcode.Text = barcode;
			PanelButtonEditDoSearch();
		}
		private void PanelButtonEditDoSearch()
		{
			if(dst.Tables["be_items"] != null)
				dst.Tables["be_items"].Clear();
			int nRows = 0;
			string kw = txtButtonEditSearchKW.Text.Trim();
			string barcode = txtButtonEditSearchBarcode.Text.Trim();
			string sc = " SELECT DISTINCT ";
			if(kw == "" && barcode == "")
				sc += " TOP 100 ";
			sc += " c.cat, c.s_cat, c.code, c.supplier_code ";
//			sc += ", c.name, c.name_cn, b.barcode, i.id AS biid ";
			sc += ", c.name, c.name_cn, b.barcode, c.barcode AS main_barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
//			sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
			sc += " WHERE 1 = 1 ";
			if (kw != "")
				sc += " AND (c.name collate Chinese_PRC_CI_AS_WS LIKE N'%" + kw + "%' OR c.name_cn collate Chinese_PRC_CI_AS_WS LIKE N'%" + kw + "%') ";
			else if (barcode != "")
				sc += " AND (c.supplier_code = '" + barcode + "' OR c.barcode =  '" + barcode + "' OR b.barcode =  '" + barcode + "') ";
			sc += " AND c.cat <> 'ServiceItem'";
			sc += " ORDER BY c.name ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "be_items");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			totalitemB = nRows;
			lvButtonEditItem.Items.Clear();
			for(int i=0; i<nRows; i++)
//			for (int i = 0; i < sizeB; i++)
			{
				DataRow dr = dst.Tables["be_items"].Rows[i];
				string code = dr["code"].ToString();
				string sBarcode = dr["barcode"].ToString().Trim();
				if(sBarcode == "")
					sBarcode = dr["barcode"].ToString();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
//				string biid = dr["biid"].ToString();
				name = name.Replace("'", "").Trim();
				name_cn = name_cn.Replace("'", "").Trim();
//				if (barcode == "" || (name == "" && name_cn == ""))
//					continue;

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(sBarcode);
				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add("");
//				if (biid != "" && biid == lblButtonButtonItemId.Text)
//					item.Selected = true;
				lvButtonEditItem.Items.Add(item);
			}
		}
		private void btnButtonEditScan_Click(object sender, EventArgs e)
		{
			txtButtonEditSearchKW.Text = "";
			PanelButtonEditDoSearch();
		}
		private void btnButtonEditSearch_Click(object sender, EventArgs e)
		{
			txtButtonEditSearchBarcode.Text = "";
			PanelButtonEditDoSearch();
		}
		private void btnButtonEditSave_Click(object sender, EventArgs e)
		{
			if (lblButtonEditItemCode.Text == "item code hidden" || lblButtonEditItemCode.Text.Trim() == "")
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please select an item!";
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.ShowDialog();
				return;
			}
			string bid = lblButtonButtonId.Text.Trim();
			int nBiid = Program.MyIntParse(lblButtonButtonItemId.Text);
			string biid = lblButtonButtonItemId.Text.Trim();
			if(nBiid < 0)
				biid = "-1";
			string code = lblButtonEditItemCode.Text;
			string name = Program.EncodeQuote(txtButtonEditButtonName.Text);
			string name_cn = Program.EncodeQuote(txtButtonEditButtonNameCN.Text);
			string sc = "";
			if (!Program.checkLocation())
				sc = " IF NOT EXISTS(SELECT id FROM button_item WHERE id = " + biid + ") ";
			else
				sc = " IF NOT EXISTS(SELECT id FROM button_item WHERE location = " + biid + " AND button_id = "+bid+") ";
			sc += " INSERT INTO button_item (button_id, code, name_en, name ";
			if(Program.checkLocation())
				sc += ", location ";
			sc += " ) VALUES(" + bid + ", " + code + ", N'" + name + "', N'" + name_cn + "'";
			if (Program.checkLocation())
				sc += ", '" + m_sSubButtonid + "'";
			sc += ") ";
			sc += " ELSE ";
			sc += " UPDATE button_item SET code = '" + code +"'";
			sc += ", name_en = N'" + name + "' ";
			sc += ", name = N'" + name_cn + "' ";
			if (Program.checkLocation())
			{
				sc += ", location = '" + m_sSubButtonid + "'";
				sc += " WHERE location = " + biid + " AND button_id = " + bid + "";
			}
			else
				sc += " WHERE id = " + biid; 
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
			}
			lblButtonEditItemCode.Text = "";
			txtButtonEditSearchKW.Text = "";
			txtButtonEditSearchBarcode.Text = "";
			btnButtonEditBack_Click(null, null);
		}
		private void btnButtonEditDelete_Click(object sender, EventArgs e)
		{
			string biid = lblButtonButtonItemId.Text;
			string sc = "";
			if (!Program.checkLocation())
				sc = " DELETE FROM button_item WHERE id = " + biid;
			else
			{
				sc = " DELETE FROM button_item WHERE location = " + biid;
				sc += " AND button_id = " + m_sButton_id;
			}
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
			}
			lblButtonButtonItemId.Text = "";
			lblButtonEditItemCode.Text = "";
			txtButtonEditSearchKW.Text = "";
			txtButtonEditSearchBarcode.Text = "";
			btnButtonEditBack_Click(null, null);
		}
		private void lvButtonEditItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListView.SelectedListViewItemCollection items = lvButtonEditItem.SelectedItems;
			if (items.Count <= 0)
				return;
			string code = items[0].SubItems[0].Text;
			string barcode = items[0].SubItems[1].Text;
			string name = items[0].SubItems[2].Text;
			string name_cn = items[0].SubItems[3].Text;
			if (code.Trim() == "")
				return;
			lblButtonEditItemCode.Text = code;
			txtButtonEditName.Text = name;
			txtButtonEditNameCN.Text = name_cn;
			txtButtonEditSearchBarcode.Text = barcode;
		}
		private void btnStockTake_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			STUpdateItemList();
			pnlStockTake.Visible = true;
			STUpdateCat();
		}
		private void STUpdateCat()
		{
			int nRows = 0;
			if (dst.Tables["cat"] != null)
				dst.Tables["cat"].Clear();
			string sc = " SELECT DISTINCT cat FROM catalog ";
			sc += " WHERE cat <> N'Brands' ";//cat = N'" + Program.EncodeQuote(cbcat.Text) + "' ";
			sc += " ORDER BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "cat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			cbSTCat.Items.Clear();
			cbSTCat.Items.Add("");
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["cat"].Rows[i];
				string s_cat = dr["cat"].ToString();
				cbSTCat.Items.Add(s_cat);
			}
		}
		private void STUpdateSCat()
		{
			int nRows = 0;
			if (dst.Tables["s_cat"] != null)
				dst.Tables["s_cat"].Clear();
			string sc = " SELECT DISTINCT s_cat FROM catalog ";
			sc += " WHERE cat = N'" + Program.EncodeQuote(cbSTCat.Text) + "' ";
			sc += " ORDER BY s_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "s_cat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			cbSTSCat.Items.Clear();
			cbSTSCat.Items.Add("");
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["s_cat"].Rows[i];
				string s_cat = dr["s_cat"].ToString();
				cbSTSCat.Items.Add(s_cat);
			}
		}
		private void STUpdateSSCat()
		{
			int nRows = 0;
			if (dst.Tables["ss_cat"] != null)
				dst.Tables["ss_cat"].Clear();
			string sc = " SELECT DISTINCT ss_cat FROM catalog ";
			sc += " WHERE cat = N'" + Program.EncodeQuote(cbSTCat.Text) + "' ";
			sc += " AND s_cat = N'" + Program.EncodeQuote(cbSTSCat.Text) + "' ";
			sc += " ORDER BY ss_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "ss_cat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			cbSTSSCat.Items.Clear();
			cbSTSSCat.Items.Add("");
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["ss_cat"].Rows[i];
				string ss_cat = dr["ss_cat"].ToString();
				cbSTSSCat.Items.Add(ss_cat);
			}
		}

		private void cbSTCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			cbSTSCat.SelectedItem = "";
			cbSTSCat.SelectedText = "";
			cbSTSCat.SelectedValue = "";
			STUpdateSCat();
			cbSTSSCat.SelectedItem = "";
			cbSTSSCat.SelectedText = "";
			cbSTSSCat.SelectedValue = "";
			STUpdateSSCat();

			STUpdateItemList();
		}
		private void cbSTSCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			STUpdateSSCat();
			STUpdateItemList();
		}
		private void cbSTSSCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			STUpdateItemList();
		}
		private void txtSTkw_TextChanged(object sender, EventArgs e)
		{
//			STUpdateItemList();
		}
		private void btnSTSearch_Click(object sender, EventArgs e)
		{
			STUpdateItemList();
			txtSTkw.Text = "";
		}
		private void btnSTPrint_Click(object sender, EventArgs e)
		{
			//			pageSetupDialog = new PageSetupDialog();
			//			pageSetupDialog.Document = printDocST;
			//			pageSetupDialog.ShowDialog();

			PrintDialog printDialog = new PrintDialog();
			printDialog.Document = printDocST;
			printDialog.UseEXDialog = true;
			if (DialogResult.OK == printDialog.ShowDialog())
			{
				printDocST.Print();
			}
		}
		private void printDoc_PrintPageST(Object sender, PrintPageEventArgs e)
		{
//			Bitmap bm = new Bitmap(dgvST.Width, dgvST.Height);
//			dgvST.DrawToBitmap(bm, new Rectangle(0, 0, dgvST.Width, dgvST.Height));
//			e.Graphics.DrawImage(bm, 0, 0);
			dataGridView = this.dgvST;
			for (; dgvIndex < 1; dgvIndex++)
			{
				rowCount = dataGridView.Rows.Count - 1;
				colCount = dataGridView.ColumnCount;

				//print headings 
				y += rowGap;
				x = leftMargin;
				for (int j = 0; j < colCount; j++)
				{
					if (dataGridView.Columns[j].Width > 0)
					{
						cellValue = dataGridView.Columns[j].HeaderText;
						e.Graphics.FillRectangle(new SolidBrush(Color.LightGray), x, y, dataGridView.Columns[j].Width, rowGap);
						e.Graphics.DrawRectangle(Pens.Black, x, y, dataGridView.Columns[j].Width, rowGap);
						e.Graphics.DrawString(cellValue, headingFont, brush, x, y);
						x += dataGridView.Columns[j].Width;
					}
				}
				//print all rows 
				for (; i < rowCount; i++)
				{
					y += rowGap;
					x = leftMargin;
					for (int j = 0; j < colCount; j++)
					{
						if (dataGridView.Columns[j].Width > 0)
						{
							cellValue = dataGridView.Rows[i].Cells[j].Value.ToString();
							e.Graphics.DrawRectangle(Pens.Black, x, y, dataGridView.Columns[j].Width, rowGap);
							e.Graphics.DrawString(cellValue, font, brush, x, y);
							x += dataGridView.Columns[j].Width;
						}
					}
					if (y >= e.PageBounds.Height - 140)
					{
						// 允許多頁打印
						y = 0;
						e.HasMorePages = true;
						i++;

						return;
					}
				}
				y += rowGap;
				for (int j = 0; j < colCount; j++)
				{
					e.Graphics.DrawString(" ", font, brush, x, y);
				}
				i = 0;
			}
			e.HasMorePages = false;

		}
		private void dgvST_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			int n = e.RowIndex;
			string code = dgvST.Rows[n].Cells["st_code"].Value.ToString();
			if(code == "")
				return;
			string name = dgvST.Rows[n].Cells["st_name"].Value.ToString();
			double dPrice = Program.MyMoneyParse(dgvST.Rows[n].Cells["st_price"].Value.ToString());
			double dQty = Program.MyMoneyParse(dgvST.Rows[n].Cells["st_qty"].Value.ToString());
			double dCost = Program.MyMoneyParse(dgvST.Rows[n].Cells["st_cost"].Value.ToString());
			string sc = " UPDATE code_relations SET name = N'" + Program.EncodeQuote(name) + "', price1 = " + dPrice + "";
			sc += ", manual_cost_frd = "+dCost;
			sc += ", manual_cost_nzd = manual_exchange_rate * "+dCost;
			sc += ", average_cost = " + dCost;
			sc += " WHERE code = " + code;
			sc += " UPDATE product SET name = N'" + Program.EncodeQuote(name) + "' WHERE code = " + code;
			sc += " IF NOT EXISTS(SELECT id FROM stock_qty WHERE branch_id = 1 AND code = " + code + ") ";
			sc += " INSERT INTO stock_qty(branch_id, code, qty) VALUES(1, " + code + ", " + dQty + ") ";
			sc += " ELSE ";
			sc += " UPDATE stock_qty SET qty = " + dQty + " WHERE branch_id = 1 AND code = " + code;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
			}
//			dgvST.Refresh();
//			btnStockTake_Click(sender,e);
		}

        private void dgdUpdateCat()
        {
            int nRows = 0;
            if (dst.Tables["dgdUpdateCat"] != null)
                dst.Tables["dgdUpdateCat"].Clear();
            string sc = " SELECT DISTINCT cat FROM catalog ";
            sc += " WHERE cat <> N'Brands' ";//cat = N'" + Program.EncodeQuote(cbcat.Text) + "' ";
            sc += " ORDER BY cat ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                nRows = myAdapter.Fill(dst, "dgdUpdateCat");
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return;
            }
            cbC.Items.Clear();
            cbC.Items.Add("");
            for (int i = 0; i < nRows; i++)
            {
                DataRow dr = dst.Tables["dgdUpdateCat"].Rows[i];
                string s_cat = dr["cat"].ToString();
                cbC.Items.Add(s_cat);
            }
        }
        private void dgdUpdateSCat()
        {
            int nRows = 0;
            if (dst.Tables["dgdUpdateSCat"] != null)
                dst.Tables["dgdUpdateSCat"].Clear();
            string sc = " SELECT DISTINCT s_cat FROM catalog ";
            sc += " WHERE cat = N'" + Program.EncodeQuote(cbC.Text) + "' ";
            sc += " ORDER BY s_cat ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                nRows = myAdapter.Fill(dst, "dgdUpdateSCat");
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return;
            }

            cbS.Items.Clear();
            cbS.Items.Add("");
            for (int i = 0; i < nRows; i++)
            {
                DataRow dr = dst.Tables["dgdUpdateSCat"].Rows[i];
                string s_cat = dr["s_cat"].ToString();
                cbS.Items.Add(s_cat);
            }
        }
        private void dgdUpdateSSCat()
        {
            int nRows = 0;
            if (dst.Tables["dgdUpdateSSCat"] != null)
                dst.Tables["dgdUpdateSSCat"].Clear();
            string sc = " SELECT DISTINCT ss_cat FROM catalog ";
            sc += " WHERE cat = N'" + Program.EncodeQuote(cbC.Text) + "' ";
            sc += " AND s_cat = N'" + Program.EncodeQuote(cbS.Text) + "' ";
            sc += " ORDER BY ss_cat ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                nRows = myAdapter.Fill(dst, "dgdUpdateSSCat");
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return;
            }
            cbSS.Items.Clear();
            cbSS.Items.Add("");
            for (int i = 0; i < nRows; i++)
            {
                DataRow dr = dst.Tables["dgdUpdateSSCat"].Rows[i];
                string ss_cat = dr["ss_cat"].ToString();
                cbSS.Items.Add(ss_cat);
            }
        }


        private void dgdItemList()
        {
            dgvDefault.Rows.Clear();
            int rows = 0;
            if (dst.Tables["dgdItemList"] != null)
                dst.Tables["dgdItemList"].Clear();
            string kw = this.txtSIBarcode.Text.ToLower().Trim();
            string sc = " SELECT top 200 ";
            sc += " * ";
            sc += " FROM ";
            sc += " (SELECT DISTINCT c.name, c.name_cn, c.code, c.price1,c.average_cost as cost, c.cat, c.s_cat, c.ss_cat, sq.qty AS stock ";
			//			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", b.barcode ";
			sc += ", c.barcode AS item_barcode ";
            sc += " FROM code_relations c ";
			sc += " left outer join barcode b on c.code = b.item_code ";
            sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
            sc += " WHERE 1 = 1 AND c.code > 1009 ";
            sc += " ) AS DerivedItem ";
            sc += " WHERE 1 = 1 ";
            try
            {
                int.Parse(kw);
                sc += " AND (code ='" + kw + "' OR LOWER(barcode) = '" + kw + "' OR LOWER(item_barcode) = '" + kw + "')";
            }
            catch (Exception ex)
            {
                string sex = ex.ToString();
                sc += " AND (LOWER(name) LIKE N'%" + kw + "%' OR LOWER(name_cn) LIKE N'%" + kw + "%'";
                sc += " OR LOWER(barcode) LIKE '%" + kw + "%' OR LOWER(item_barcode) = '" + kw + "')";
                if (cbC.Text.ToLower() != "")
                    sc += " AND LOWER(cat) = N'" +this.cbC.Text.ToLower() + "'";
                if (cbS.Text.ToLower() != "")
                    sc += " AND  LOWER(s_cat) = N'" + this.cbS.Text.ToLower() + "'";
                if (cbSS.Text.ToLower() != "")
                    sc += " AND LOWER(ss_cat) = N'" + this.cbSS.Text.ToLower() + "'";
            }
            sc += " ORDER BY code DESC ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                rows = myAdapter.Fill(dst, "dgdItemList");

                sqlS = sc;
                totalitemS = rows;
                if (rows <= 0)
                    return;
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return;
            }

            for (int i = 0; i < rows; i++)
            {
                DataRow dr = dst.Tables["dgdItemList"].Rows[i];
                string barcode = dr["barcode"].ToString();
                if (barcode == "")
                    barcode = dr["item_barcode"].ToString();
                string code = dr["code"].ToString();
                string name = dr["name"].ToString();
                string stock = dr["stock"].ToString();
                string description = dr["name"].ToString();
                string price1 = dr["price1"].ToString();
                string cost = dr["cost"].ToString();
                string cat = dr["cat"].ToString();
                string s_cat = dr["s_cat"].ToString();
                string ss_cat = dr["ss_cat"].ToString();

                double dQty = Program.MyDoubleParse(stock);
                double dPrice = Program.MyDoubleParse(price1);
                double dCost = Program.MyDoubleParse(cost);

                string[] row = { code, barcode, cat, s_cat, ss_cat, name, stock, dCost.ToString("c"), "Select"};
                this.dgvDefault.Rows.Add(row);
            }

            if (rows == 1)
            {
                this.txtSIPrice.Text = Math.Round(Program.MyDoubleParse(dst.Tables["dgdItemList"].Rows[0]["cost"].ToString()), 2).ToString() ;
                this.btnSIInput.Enabled = true;
            }
            else
            {
                this.btnSIInput.Enabled = false;
                this.txtSIPrice.Text = "";
            }
        }

		private void STUpdateItemList()
		{
			dgvST.Rows.Clear();
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtSTkw.Text.ToLower().Trim();
			string sc = " SELECT ";
//			if (kw == "" && cbSTCat.Text.Trim() == "")
//				sc += " TOP 100 ";
			sc += " * ";
			sc += " FROM ";
			sc += " (SELECT DISTINCT c.name, c.name_cn, c.code, c.price1,c.manual_cost_frd, c.cat, c.s_cat, c.ss_cat, sq.qty AS stock ";
			//			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", b.barcode ";
			sc += ", c.barcode AS item_barcode ";
			sc += " FROM code_relations c ";
			sc += " left outer join barcode b on b.item_code = c.code ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += " WHERE 1 = 1 AND c.code > 1009 ";
			sc += " ) AS DerivedItem ";
			sc += " WHERE 1 = 1 ";
			try
			{
				int.Parse(kw);
				sc += " AND (code ='" + kw + "' OR LOWER(barcode) = '" + kw + "' OR LOWER(item_barcode) = '" + kw + "')";
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
				sc += " AND (LOWER(name) LIKE N'%" + kw + "%' OR LOWER(name_cn) LIKE N'%" + kw + "%'";
				sc += " OR LOWER(barcode) LIKE '%" + kw + "%' OR LOWER(item_barcode) = '" + kw + "')";
				if (cbSTCat.Text.ToLower() != "")
					sc += " AND LOWER(cat) = N'" + cbSTCat.Text.ToLower() + "'";
				if (cbSTSCat.Text.ToLower() != "")
					sc += " AND  LOWER(s_cat) = N'" + cbSTSCat.Text.ToLower() + "'";
				if (cbSTSSCat.Text.ToLower() != "")
					sc += " AND LOWER(ss_cat) = N'" + cbSTSSCat.Text.ToLower() + "'";
			}
			sc += " ORDER BY code DESC ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");

				sqlS = sc;
				totalitemS = rows;
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			double dSubQty = 0;
			double dSubtotal = 0;
			double dSubCostTotal = 0;
			if (kw == "" && cbSTCat.Text.Trim() == "" && rows > 500)
				rows = 500;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["list"].Rows[i];
				string barcode = dr["barcode"].ToString();
				if(barcode == "")
					barcode = dr["item_barcode"].ToString();
				string code = dr["code"].ToString();
				string name = dr["name"].ToString();
				string stock = dr["stock"].ToString();
				string description = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cost = dr["manual_cost_frd"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();
				
				double dQty = Program.MyDoubleParse(stock);
				double dPrice = Program.MyDoubleParse(price1);
				double dCost = Program.MyDoubleParse(cost);
				
				dSubQty += dQty;
				dSubtotal += dPrice * dQty;
				dSubCostTotal += dCost * dQty;

				string[] row = { code, barcode, cat, s_cat, ss_cat, name, stock, dPrice.ToString("c"), dCost.ToString("c") };
				dgvST.Rows.Add(row);
			}
			string[] rowe = { "", "", "", "", "", "Sub Total", dSubQty.ToString(), dSubtotal.ToString("c"), dSubCostTotal.ToString("c")};
			dgvST.Rows.Add(rowe);
		}
		private void btnStockInput_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			dgvSI.Rows.Clear();
			txtSIBarcode.Text = "";
			txtSIQty.Text = "";
			pnlStockInput.Visible = true;
            dgdUpdateCat();
			txtSIBarcode.Focus();
			CheckAppDataForStockInput();
		}
		private void txtSIBarcode_KeyPress(object sender, KeyPressEventArgs e)
		{
            if (e.KeyChar == '\r' && txtSIBarcode.Text != "" && dst.Tables["dgdItemList"].Rows.Count == 1)
                //				txtSIQty.Select();
                txtSIPrice.Select();
		}
		private void txtSIQty_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r' && txtSIBarcode.Text != "" && txtSIQty.Text != "")
				DoStockInput();
		}
		private void btnSIInput_Click(object sender, EventArgs e)
		{
			DoStockInput();

            cbC.SelectedItem = "";
            cbC.SelectedText = "";
            cbC.SelectedValue = "";
            dgdUpdateCat();

            cbS.SelectedItem = "";
            cbS.SelectedText = "";
            cbS.SelectedValue = "";
            dgdUpdateSCat();
            cbSS.SelectedItem = "";
            cbSS.SelectedText = "";
            cbSS.SelectedValue = "";
            dgdUpdateSSCat();

            dgvDefault.Rows.Clear();
			CheckAppDataForStockInput();
		}

		private void btnLoadAppData_Click(object sender, EventArgs e)
		{
			if (!CheckAppDataForStockInput())
				return;
			string table_name = "LoadAppDataForStockInput";
			int nRows = dst.Tables[table_name].Rows.Count;
			int i = 0;
			int rows = dgvSI.Rows.Count;
			for (i = 0; i < rows; )
			{
				DataGridViewRow row = this.dgvSI.Rows[i];
				string dataType = row.Cells[0].Value.ToString();
				if (dataType == "app")
				{
					dgvSI.Rows.Remove(row);
					rows--;
				}
				else
					i++;
			}
			for (i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables[table_name].Rows[i];
				string id = dr["id"].ToString();
				string code = dr["code"].ToString();
				string barcode = dr["main_barcode"].ToString();
				string name = dr["name"].ToString();
				string stock = dr["qty"].ToString();
				string description = dr["name"].ToString();
				string cost = dr["cost"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();
				string[] row = { "app", id, code, barcode, cat, s_cat, ss_cat, name, stock, cost, "Remove" };
				dgvSI.Rows.Insert(0, row);
			}
			txtSIBarcode.Text = "";
			txtSIQty.Text = "";
			txtSIBarcode.Focus();
			btnSIDoInput.Enabled = true;
			TotalInput();
		}
		private bool CheckAppDataForStockInput()
		{
			string table_name = "LoadAppDataForStockInput";
			int nRows = 0;
			if (dst.Tables[table_name] != null)
				dst.Tables[table_name].Clear();
			string sc = " IF EXISTS (SELECT table_name from INFORMATION_SCHEMA.TABLES WHERE table_name = 'app_stockinput' ) ";
			sc += " SELECT a.id, c.code, c.name, c.cat, c.s_cat, c.ss_cat, a.cost, a.qty, c.barcode AS main_barcode ";
			sc += " FROM app_stockinput a ";
			sc += " LEFT OUTER JOIN code_relations c ON c.code = a.code ";
			sc += " WHERE 1=1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, table_name);
				if (nRows <= 0)
				{
					btnLoadAppData.Enabled = false;
					return false;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			btnLoadAppData.Enabled = true;
			return true;
		}
        private void TotalInput()
        {
			CheckAppDataForStockInput();
            double dSubQty = 0;
            double dSubtotal = 0;
            int rows = dgvSI.Rows.Count;
            if (rows ==0)
                return;
			string last_row_name = dgvSI.Rows[rows - 1].Cells["si_name"].Value.ToString();
			if (rows == 1 && last_row_name == "Sub Total:")
            {
                dgvSI.Rows.Clear();
                return;
            }
			if (rows > 1 && last_row_name == "Sub Total:")
            {
                dgvSI.Rows.RemoveAt(rows - 1);
                rows = rows - 1;
            }

            for (int i = 0; i < rows; i++)
            {
                dSubQty += Program.MyDoubleParse(dgvSI.Rows[i].Cells["si_qty"].Value.ToString());
                dSubtotal += Program.MyDoubleParse(dgvSI.Rows[i].Cells["si_qty"].Value.ToString()) * Program.MyDoubleParse(dgvSI.Rows[i].Cells["si_price"].Value.ToString());
            }

			string[] row = { "","", "", "", "", "", "", "Sub Total:", dSubQty.ToString(), dSubtotal.ToString(), "" };
            dgvSI.Rows.Insert(rows, row);
        }

		private void DoStockInput()
		{
			int a = 0;
			if(txtSIBarcode.Text == "")
				return;
			string barcode = txtSIBarcode.Text.Trim();
			double dQty = Program.MyDoubleParse(txtSIQty.Text);
			
			int nRows = 0;
			if(dst.Tables["si"] != null)
				dst.Tables["si"].Clear();
			string sc = " SELECT top 1 c.code, c.name, c.cat, c.s_cat, c.ss_cat, c.price1, b.barcode, c.barcode AS main_barcode ";
			sc += " FROM code_relations c ";
			sc += " JOIN barcode b ON b.item_code = c.code ";
			sc += " WHERE 1=1 ";
 ///        sc += " AND b.barcode = N'" + Program.EncodeQuote(barcode) + "' OR c.barcode = N'" + Program.EncodeQuote(barcode) + "' ";;
            try
            {
                int.Parse(Program.EncodeQuote(barcode));
                sc += " AND (c.code ='" + Program.EncodeQuote(barcode) + "' OR LOWER(b.barcode) = '" + Program.EncodeQuote(barcode) + "' OR LOWER(c.barcode) = '" + Program.EncodeQuote(barcode) + "')";
            }
            catch (Exception ex)
            {
                string sex = ex.ToString();
                sc += " AND (LOWER(c.name) LIKE N'%" + Program.EncodeQuote(barcode) + "%' OR LOWER(c.name_cn) LIKE N'%" + Program.EncodeQuote(barcode) + "%'";
                sc += " OR LOWER(b.barcode) LIKE '%" + Program.EncodeQuote(barcode) + "%' OR LOWER(c.barcode) = '" + Program.EncodeQuote(barcode) + "')";
                if (cbC.Text.ToLower() != "")
                    sc += " AND LOWER(cat) = N'" + this.cbC.Text.ToLower() + "'";
                if (cbS.Text.ToLower() != "")
                    sc += " AND  LOWER(s_cat) = N'" + this.cbS.Text.ToLower() + "'";
                if (cbSS.Text.ToLower() != "")
                    sc += " AND LOWER(ss_cat) = N'" + this.cbSS.Text.ToLower() + "'";
            }

            //sc += " OR c.supplier_code =N'"+Program.EncodeQuote(barcode)+"'";
            //sc += " OR c.name like N'%" + barcode + "%'";
            //sc += " OR c.name_cn like N'%" + barcode + "%'";
			//if (barcode.Length <= 10)
            //if (int.TryParse(barcode, out a))
            //    sc += " OR c.code = '" + Program.EncodeQuote(barcode) + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "si");
				if (nRows <= 0)
				{
					MessageBox.Show("item not found");
					return;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DataRow dr = dst.Tables["si"].Rows[0];
			string code = dr["code"].ToString();
			string sBarcode = dr["main_barcode"].ToString();
			if(sBarcode == "")
				sBarcode = dr["barcode"].ToString();
			string name = dr["name"].ToString();
			string stock = dQty.ToString();
			string description = dr["name"].ToString();
            string price1 = "";
            try
            {
                double.Parse(this.txtSIPrice.Text);
                price1 = double.Parse(this.txtSIPrice.Text).ToString();
            }
            catch (Exception)
            {
                price1 = dr["price1"].ToString();
            }

			string cat = dr["cat"].ToString();
			string s_cat = dr["s_cat"].ToString();
			string ss_cat = dr["ss_cat"].ToString();

			string[] row = { "local","", code, sBarcode, cat, s_cat, ss_cat, name, stock, price1, "Remove" };
			dgvSI.Rows.Insert(0, row);
			
			txtSIBarcode.Text = "";
			txtSIQty.Text = "";
			txtSIBarcode.Focus();
			btnSIDoInput.Enabled = true;
            TotalInput();
		}
		private void btnSIDoInput_Click(object sender, EventArgs e)
		{
			double dQtyTotal = 0;
			string sc = "";
            int rows = 0;
            rows = dgvSI.Rows.Count;
            if (rows > 1)
            {
                rows = rows - 1;
            }

			for(int i=0; i<rows; i++)
			{
				string dataType = dgvSI.Rows[i].Cells[0].Value.ToString();
				string AppDataId = dgvSI.Rows[i].Cells[1].Value.ToString();
				if(dataType == "app")
					sc += " DELETE FROM app_stockinput WHERE id = " + AppDataId + " ";

				string code = dgvSI.Rows[i].Cells["si_code"].Value.ToString();
				string name = dgvSI.Rows[i].Cells["si_name"].Value.ToString();
				double dPrice = Program.MyMoneyParse(dgvSI.Rows[i].Cells["si_price"].Value.ToString());
				double dQty = Program.MyMoneyParse(dgvSI.Rows[i].Cells["si_qty"].Value.ToString());
				dQtyTotal += dQty;

                sc += " UPDATE code_relations SET name = N'" + Program.EncodeQuote(name) + "', manual_cost_frd = " + dPrice + "";
				sc += ", manual_cost_nzd = manual_exchange_rate * " + dPrice + "";
				sc += ", average_cost = " + dPrice + "";
                sc += " WHERE code = " + code;
                sc += " UPDATE product SET name = N'" + Program.EncodeQuote(name) + "' WHERE code = " + code;

				sc += " IF NOT EXISTS(SELECT id FROM stock_qty WHERE branch_id = 1 AND code = " + code + ") ";
				sc += " INSERT INTO stock_qty(branch_id, code, qty) VALUES(1, " + code + ", '" + dQty + "') ";
				sc += " ELSE ";
				sc += " UPDATE stock_qty SET qty = qty + " + dQty + " WHERE branch_id = 1 AND code = '" + code + "'";
				/*********************/
				sc += " INSERT INTO stock_input(branch_id,code, qty, staff) VALUES(1, '" + code + "', '" + dQty + "', '" + Program.m_sSalesId + "')";
				/*********************/
				
			}
			if(sc == "")
				return;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return ;
			}
            MessageBox.Show(rows.ToString() + " items qty total:" + dQtyTotal.ToString() + " successfully input");
			dgvSI.Rows.Clear();
			btnSIDoInput.Enabled = false;
			CheckAppDataForStockInput();
		}
		private void buttonSave_Click(object sender, EventArgs e)
		{
			if (SaveSettings())
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = " Setting Saved ";
				fm.ShowDialog();
//                Program.m_formconfigshow = false;
//                this.Close();
			}
			CheckSqlTables();
		}
		bool CheckSqlTables()
		{
			string sc = "";
			int nStationId = Program.MyIntParse(textStationID.Text);
			if(nStationId != 0)
			{
				sc = " IF NOT EXISTS(SELECT id FROM till WHERE station_id = " + nStationId + ") INSERT INTO till (station_id) VALUES(" + nStationId + ") ";
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e1)
				{
					myConnection.Close();
					Program.ShowExp(sc, e1);
					return false;
				}
			}
			sc = " SELECT TOP 1 till_number FROM delete_item ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "checktable");
			}
			catch (Exception ex)
			{
//				Program.ShowExp(sc, ex);
				myConnection.Close();
				string ee = ex.ToString().ToLower();
				if(ee.IndexOf("invalid object name 'delete_item'") >= 0)
				{
					sc = @" 
CREATE TABLE [dbo].[delete_item](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[till_number] [int] NOT NULL CONSTRAINT [DF_delete_item_till_number]  DEFAULT ((1)),
	[code] [int] NULL,
	[name] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[name_cn] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[qty] [float] NULL CONSTRAINT [DF_delete_item_qty]  DEFAULT ((1)),
	[amount] [float] NULL,
	[sales] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[time] [datetime] NOT NULL CONSTRAINT [DF_delete_item_time]  DEFAULT (getdate()),
 CONSTRAINT [PK_delete_item] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception e1)
					{
						myConnection.Close();
						Program.ShowExp(sc, e1);
						return false;
					}
				}
				else if(ee.IndexOf("invalid column name 'till_number'") >= 0)
				{
					sc = " ALTER TABLE delete_item ADD till_number INT NULL ";
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception e1)
					{
						myConnection.Close();
						Program.ShowExp(sc, e1);
						return false;
					}
				}
			}
			return true;
		}
		private void APUpdateCat()
		{
			int nRows = 0;
			if (dst.Tables["apcat"] != null)
				dst.Tables["apcat"].Clear();
			string sc = " SELECT DISTINCT cat FROM catalog ";
			sc += " WHERE cat <> N'Brands' ";//cat = N'" + Program.EncodeQuote(cbcat.Text) + "' ";
			sc += " ORDER BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "apcat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			comboBox3.Items.Clear();
			comboBox3.Items.Add("");
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["apcat"].Rows[i];
				string s_cat = dr["cat"].ToString();
				comboBox3.Items.Add(s_cat);
			}
		}
		private void APUpdateSCat()
		{
			int nRows = 0;
			if (dst.Tables["aps_cat"] != null)
				dst.Tables["aps_cat"].Clear();
			string sc = " SELECT DISTINCT s_cat FROM catalog ";
			sc += " WHERE cat = N'" + Program.EncodeQuote(comboBox3.Text) + "' ";
			sc += " ORDER BY s_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "aps_cat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			comboBox2.Items.Clear();
			comboBox2.Items.Add("");
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["aps_cat"].Rows[i];
				string s_cat = dr["s_cat"].ToString();
				comboBox2.Items.Add(s_cat);
			}
		}
		private void APUpdateSSCat()
		{
			int nRows = 0;
			if (dst.Tables["apss_cat"] != null)
				dst.Tables["apss_cat"].Clear();
			string sc = " SELECT DISTINCT ss_cat FROM catalog ";
			sc += " WHERE cat = N'" + Program.EncodeQuote(comboBox3.Text) + "' ";
			sc += " AND s_cat = N'" + Program.EncodeQuote(comboBox2.Text) + "' ";
			sc += " ORDER BY ss_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "apss_cat");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			comboBox1.Items.Clear();
			comboBox1.Items.Add("");
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["apss_cat"].Rows[i];
				string ss_cat = dr["ss_cat"].ToString();
				comboBox1.Items.Add(ss_cat);
			}
		}
		private void groupBox10_Enter(object sender, EventArgs e)
		{
		}
		private void groupBox16_Enter(object sender, EventArgs e)
		{
		}
		private void groupBox24_Enter(object sender, EventArgs e)
		{
		}
		private void groupBox28_Enter(object sender, EventArgs e)
		{
		}
		private void txtdecimal_TextChanged(object sender, EventArgs e)
		{
		}
		private void txtpricelength_TextChanged(object sender, EventArgs e)
		{
		}
		private void button2_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}
		private void textBoxAdUrl_TextChanged(object sender, EventArgs e)
		{
		}
		private void groupboxPromotion_Enter(object sender, EventArgs e)
		{
		}
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			APUpdateSCat();
			RefreshItemList();
		}
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			APUpdateSSCat();
			RefreshItemList();
		}
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshItemList();
		}

		private void button4_Click_1(object sender, EventArgs e)
		{
			RefreshItemList();
		}
		private void lvItem_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
		}
		private void lvItem_MouseClick(object sender, MouseEventArgs e)
		{
			string select_all = "0";
			if (lvItem.Items[lvItem.Items.Count - 1].Checked)
				select_all = "1";
			ListViewHitTestInfo info = this.lvItem.HitTest(e.Location);
			if(info.SubItem == null)
				return;
			if (info.SubItem.Text == "Select All")
			{
				if (select_all == "0")
				{
					for (int i = 0; i < lvItem.Items.Count - 1; i++)
					{
//                        if (lvItem.Items[i].SubItems[7].Text != "" && lvItem.Items[i].SubItems[7].Text != null)
//                            continue;
						lvItem.Items[i].Checked = true;
					}
				}
				else
				{
					for (int i = 0; i < lvItem.Items.Count - 1; i++)
					{
						lvItem.Items[i].Checked = false;
					}
				}
			}
			else
			{
				select_all = "0";
				lvItem.Items[lvItem.Items.Count - 1].Checked = false;
			}
		}
		private void button6_Click(object sender, EventArgs e)
		{
			string select_check = "0";
			if (lvItem.Items.Count <= 0)
			{
				MessageBox.Show("Please select a RANGE of report!!!");
				return;
			}
			else
			{
				for (int i = 0; i < lvItem.Items.Count - 1; i++)
				{
					if (lvItem.Items[i].Checked)
					{
						select_check = "1";
						break;
					}
				}
			}
			if (select_check != "1")
			{
				MessageBox.Show("Please select at lease one item!!!");
				return;
			}
			else
			{
				if (MessageBox.Show("Are you sure you want to clear current selected Promotion?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;
				else
				{
					for (int i = 0; i < lvItem.Items.Count - 1; i++)
					{
                        if (lvItem.Items[i].Checked)
                        {
                            if (m_sPromoid != "")
                                DelPromotionToItem(lvItem.Items[i].SubItems[1].Text.Trim(), m_sPromoid);

                            string checkedItemPromoId = GetPromoIdByItemCode(lvItem.Items[i].SubItems[1].Text.Trim());
                            if(checkedItemPromoId != "0")
                                DelPromotionToItem(lvItem.Items[i].SubItems[1].Text.Trim(), checkedItemPromoId);
                        }
							
					}
				}
			}
			m_sPromoid = "";
			RefreshItemList();
			DoColor();
		}
		private void btnAssign_Click(object sender, EventArgs e)
		{
			string select_check = "0";
			if (lvItem.Items.Count <= 0)
			{
				MessageBox.Show("Please select a RANGE of report!!!");
				return;
			}
			else
			{
				for (int i = 0; i < lvItem.Items.Count - 1; i++)
				{
					if (lvItem.Items[i].Checked)
					{
						select_check = "1";
						break;
					}
				}
			}
			if (select_check != "1")
			{
				MessageBox.Show("Please select at lease one item!!!");
				return;
			}
			else
			{
				if (m_sPromoid == "" || m_sPromoid == null || m_sPromoid == "0")
					return;
				if (MessageBox.Show("Are you sure you want to add current selected Promotion?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;
				else
				{
					for (int i = 0; i < lvItem.Items.Count - 1; i++)
					{
						if (lvItem.Items[i].Checked)
						{
							if (ItemsInPromotion(lvItem.Items[i].SubItems[1].Text.Trim(), m_sPromoid, dtpPromoStart.Text, dtpPromoEnd.Text))
							{
								FormMSG fm = new FormMSG();
								fm.m_sMsg = "Sorry, this item is in another promotion \r\n";
								fm.m_sMsg += " Active Date conflict !";
								fm.btnNo.Visible = false;
								fm.btnYes.Visible = false;
								fm.ShowDialog();
								return; 
							}
							string code = lvItem.Items[i].SubItems[1].Text.Trim();
							string barcode = lvItem.Items[i].SubItems[2].Text.Trim();
							if (this.cbPromoType.Text == "Group Promotion")
								DoAddGroupPromotionItemByBarcode(barcode);
                            else if (this.cbPromoType.Text == "Category Discount")
                            {
                                AddPromotionToItem(code, m_sPromoid, true, comboBox3.Text);
                            }
                            else
                                AddPromotionToItem(code, m_sPromoid, false, "");
						}
						//else
						//    DelPromotionToItem(lvItem.Items[i].SubItems[1].Text.Trim(), m_sPromoid);
					}
				}
			}
//			m_sPromoid = "";
			txtS.Text = "";
			txtPromo.Text = "";
			RefreshItemList();
			DoColor();
		}
		private bool ItemsInPromotion(string code, string promo_id, string start, string end)
		{
			string sc = "";
			int rows = 0;
			if (dst.Tables["ItemsInPromotion"] != null)
				dst.Tables["ItemsInPromotion"].Clear();
			sc = "SET dateformat dmy ";
			sc += " SELECT pl.promo_id, pl.promo_desc, pl.promo_start_date, pl.promo_end_date ";
			sc += " FROM promotion_list pl ";
			sc += " JOIN promo pr on pr.promo_id = pl.promo_id ";
			sc += " WHERE 1=1 ";
			sc += " AND pl.promo_id <> '" + promo_id + "'";
			sc += " AND pr.code = '" + code + "'";

			sc += " AND pl.promo_start_date <= '" + end + "' ";
			sc += " AND pl.promo_end_date >= '" + start + "' ";
			//sc += " OR ";
			//sc += " (pl.promo_end_date >= '" + start + "' ";
			//sc += " AND pl.promo_start_date <= '" + end + "') ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "ItemsInPromotion");
				if (rows <= 0)
					return false;
				else
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
		}
		private void AddPromotionToItem(string code, string promo_id, bool isCatDis, string cat)
		{
			string sc_d = " SET DATEFORMAT dmy ";
			sc_d += " IF NOT EXISTS(select code from promo WHERE 1=1 AND promo_id = '" + promo_id + "' AND code = '"+code+"')";
			sc_d += " INSERT INTO promo(promo_id, code)";
			sc_d += " VALUES('"+promo_id+"', '"+code+"')";

            sc_d += " UPDATE code_relations SET promo_id = '" + promo_id + "' WHERE 1=1 ";
            sc_d += " AND code = '"+code+"'";

            if (isCatDis && cat.Trim() != "")
            {
                sc_d += " UPDATE promotion_list set promo_desc = N'" + cat + "' WHERE 1=1 and promo_id = '" + promo_id + "'";
            }

			try
			{
				myCommand = new SqlCommand(sc_d);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc_d, ex);
				myCommand.Connection.Close();
				return;
			}
		}
		private void DelPromotionToItem(string code, string promo_id)
		{
			//string sc_d = " UPDATE code_relations SET promo_id =  '" + promo_id + "' ";
			//sc_d += " WHERE code = " + code;
			string sc_d = "SET DATEFORMAT dmy ";
			sc_d += " DELETE FROM promo WHERE 1=1 ";
			sc_d += " AND code = '"+code+"' AND promo_id = '"+promo_id+"'";
            sc_d += " UPDATE code_relations SET promo_id = '' WHERE 1=1 AND code = '" + code + "' AND promo_id = '" + promo_id + "'"; 
			try
			{
				myCommand = new SqlCommand(sc_d);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc_d, ex);
				myCommand.Connection.Close();
				return;
			}
		}

		private void ClearPromotionToItem(string code, string promo_id)
		{
			string sc_d = " UPDATE code_relations SET promo_id =  '' ";
			sc_d += " WHERE code = " + code;
			try
			{
				myCommand = new SqlCommand(sc_d);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc_d, ex);
				myCommand.Connection.Close();
				return;
			}
		}

		private void lvPromotion_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			
		}

		private void panel2_Paint_1(object sender, PaintEventArgs e)
		{

		}

		private void btnSaveii_Click(object sender, EventArgs e)
		{

			string id = lblButtonButtonId.Text;
			string name = txtButtonCatName.Text;
			string name_cn = txtButtonCatNameCN.Text;
			bool indivisual = ckbIndivisual.Checked;
			string sii = "";
			if (indivisual)
				sii = "1";
			else
				sii = "0";
			string sc = " UPDATE button SET name = N'" + Program.EncodeQuote(name_cn) + "' ";
			sc += ", name_en = N'" + Program.EncodeQuote(name) + "' ";
			sc += ", is_indivisual = " + sii;
			sc += " WHERE id = " + id;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			if(SetSiteSettings("button_id_" + m_sButton_id, this.txtCodeii.Text.Trim()))
				MessageBox.Show(" Setting Updated!");
			else
				MessageBox.Show(" Error!");
		}
		private void btnPicOpen_Click(object sender, EventArgs e)
		{
//          OpenFileDialog dlg = new OpenFileDialog();
//          dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			dlg.Title = "Open Image";
			dlg.Filter = "bmp files (*.bmp)|*.bmp";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
//              Image b = new Bitmap(95, 125);
				string FileName = dlg.FileName;
				b = new Bitmap(Image.FromFile(dlg.FileName), new Size(72,72)); 
				pictureBox1.Height = 72;
				pictureBox1.Width = 72;
				pictureBox1.Show();
				pictureBox1.Image = b;
			}
			dlg.Dispose();
		}
		private void btnPicUpload_Click(object sender, EventArgs e)
		{
			if(dlg.FileName == "" || dlg.FileName == null)
				return;
			string filepath = Program.m_sPicroot + "\\" + m_sCode + ".bmp";
			File.Copy(dlg.FileName, filepath, true);
			FormMSG fm = new FormMSG();
			fm.btnNo.Visible = false;
			fm.btnYes.Visible = false;
			fm.m_sMsg = "Picture Uploaded!";
			fm.ShowDialog();
		}
		private void btnCatOpen_Click(object sender, EventArgs e)
		{
			dlg.Title = "Open Image";
			dlg.Filter = "bmp files (*.bmp)|*.bmp";

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				//              Image b = new Bitmap(95, 125);
				string FileName = dlg.FileName;
				b = new Bitmap(Image.FromFile(dlg.FileName), new Size(72, 72));
//				pictureBox2.Height = 72;
//				pictureBox2.Width = 72;
				pictureBox2.Show();
				pictureBox2.Image = b;
			}
			dlg.Dispose();
		}
		private void btnCatUpload_Click(object sender, EventArgs e)
		{
			if (dlg.FileName == "" || dlg.FileName == null)
				return;
			string filepath = Program.m_sPicroot + "\\" + txtButtonCatName.Text + ".bmp";
			File.Copy(dlg.FileName, filepath, true);
			FormMSG fm = new FormMSG();
			fm.btnNo.Visible = false;
			fm.btnYes.Visible = false;
			fm.m_sMsg = "Picture Uploaded!";
			fm.ShowDialog();
		}
		private void lvrpayment_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		private void groupBox23_Enter(object sender, EventArgs e)
		{
		}
		private void GetTaxRecord()
		{
			this.lvTaxRate.Items.Clear();
			this.lvTaxRate.Scrollable = true;
			int rows = 0;
			string sc = "";
			if (dst.Tables["GetTaxRecord"] != null)
				dst.Tables["GetTaxRecord"].Clear();
			sc += " SELECT * FROM tax_code WHERE 1=1 ORDER BY id ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "GetTaxRecord");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["GetTaxRecord"].Rows[i];
				string id = dr["id"].ToString();
				string tax_code = dr["tax_code"].ToString();
				string tax_rate = dr["tax_rate"].ToString();

				ListViewItem item = new ListViewItem(id);
				item.SubItems.Add(tax_code);
				item.SubItems.Add(tax_rate);
//				item.SubItems.Add("");
				item.SubItems.Add("del");
				this.lvTaxRate.Items.Add(item);
			}
			this.lvTaxRate.Show();
		}

		private void btnTaxCode_Click(object sender, EventArgs e)
		{
			HideAllPanel();
			this.pnlTax.Visible = true;
			this.txtTaxCode.Focus();
			GetTaxRecord();
		}

		private bool DoDeleteTax(string id)
		{
			string sc = "";
			sc = " DELETE FROM tax_code WHERE 1=1 ";
			sc += " AND id =" + id;
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
			GetTaxRecord();
			return true;
		}

		private void btnTaxCode1_Click(object sender, EventArgs e)
		{
			if (this.btnTaxCode1.Text == "UPDATE")
			{
				if (UpdateTaxCode(m_sTaxId))
				{
					FormMSG fm = new FormMSG();
					fm.btnNo.Visible = false;
					fm.btnYes.Visible = false;
					fm.m_sMsg = "Tax Code Updated!";
					fm.ShowDialog();
					this.btnTaxCode1.Text = "Add New";
					this.gbTaxCode.Text = "Tax Code Edit";
					this.txtTaxCode.Text = "";
					this.txtTaxRate1.Text = "";
				}

			}
			else
			{
				if (AddNewTaxCode())
				{
					FormMSG fm = new FormMSG();
					fm.btnNo.Visible = false;
					fm.btnYes.Visible = false;
					fm.m_sMsg = "Tax Code Added!";
					fm.ShowDialog();
					this.txtTaxCode.Text = "";
					this.txtTaxRate1.Text = "";
				}
			}
			this.txtTaxCode.Focus();
			GetTaxRecord();
		}

		private void lvTaxRate_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvTaxRate.SelectedItems;
			if (items.Count <= 0)
				return;
			string id = "";
			string tax_code = "";
			string tax_rate = "";
			foreach (ListViewItem item in items)
			{
				m_sTaxId = item.SubItems[0].Text;
			}
			/***********************************/
			if (items[0].GetSubItemAt(e.X, e.Y).Text == "del")
			{
				if (MessageBox.Show("Are you sure you want to delete this barcode? click Yes to delete", "Confirm deleting", MessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					return;
				}
				DoDeleteTax(m_sTaxId);
			}
			/************************************/
			if (dst.Tables["taxcode"] != null)
				dst.Tables["taxcode"].Clear();
			string sc = " SELECT * FROM tax_code WHERE id = " + m_sTaxId;

			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "taxcode") <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (dst.Tables["taxcode"].Rows.Count == 1)
			{
				DataRow dr = dst.Tables["taxcode"].Rows[0];
				id = dr["id"].ToString();
				tax_code = dr["tax_code"].ToString();
				tax_rate = dr["tax_rate"].ToString();
				this.txtTaxCode.Text = tax_code;
				this.txtTaxRate1.Text = (double.Parse(tax_rate) * 100).ToString();
				this.gbTaxCode.Text = "ID: " + id + " Code :" + tax_code + " Rate :" + tax_rate;
				this.btnTaxCode1.Text = "UPDATE";
			}
			
		}

		private bool UpdateTaxCode(string id)
		{
			double dTaxrate = 0;
			string sc = "";
			if (this.txtTaxRate1.Text == "" || this.txtTaxRate1.Text == null)
				dTaxrate = 0;
			else
				dTaxrate = Math.Round(double.Parse(this.txtTaxRate1.Text) / 100, 2);
			sc = " UPDATE tax_code SET tax_code= N'" + this.txtTaxCode.Text + "', tax_rate = '" + dTaxrate.ToString() + "'";
			sc += " WHERE 1=1 AND id =" + id;
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
		private bool AddNewTaxCode()
		{
			double dTaxrate = 0;
			if (this.txtTaxRate1.Text == "" || this.txtTaxRate1.Text == null)
				dTaxrate = 0;
			else
				dTaxrate = Math.Round(double.Parse(this.txtTaxRate1.Text) / 100, 2);
			string sc = "";
			sc += " INSERT INTO tax_code (tax_code, tax_rate)VALUES( N'";
			sc += this.txtTaxCode.Text + "'";
			sc += ", '" + dTaxrate.ToString() + "'";
			sc += ")";
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

		private void getGstRatebygstcode()
		{
			if (dst.Tables["getGstRatebygstcode"] != null)
				dst.Tables["getGstRatebygstcode"].Clear();
			string sc = "";
			int rows = 0;
			sc += " SELECT TOP 1* FROM tax_code WHERE 1=1 ";
			sc += " AND tax_code = N'" + this.cbTaxCode.Text + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "getGstRatebygstcode");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			string tax_rate = dst.Tables["getGstRatebygstcode"].Rows[0]["tax_rate"].ToString();
			this.txtTaxRate.Text = (double.Parse(tax_rate) * 100).ToString();
		}

		private void cbTaxCode_SelectedIndexChanged(object sender, EventArgs e)
		{
			getGstRatebygstcode();
		}

		private void ckbAutoTare_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void btnBFirst_Click(object sender, EventArgs e)
		{
			this.btnBNext.Enabled = true;
			this.btnBLast.Enabled = true;

			this.btnBFirst.Enabled = false;
			this.btnBPre.Enabled = false;
			pageB = 1;
			PanelButtonEditDoSearch();

		}
		private void btnBPre_Click(object sender, EventArgs e)
		{
			this.btnBNext.Enabled = true;
			this.btnBLast.Enabled = true;
			//			if(page <= 1)
			//			{
			//				this.btnPre.Enabled = false;
			//				this.btnFirst.Enabled = false;
			//			}
			if (pageB >= 1)
				pageB = pageB - 1;
			else
				return;	

			if (totalitemB % sizeB == 0)
			{
				if (pageB < 1)
				{
					this.btnBFirst.Enabled = false;
					this.btnBPre.Enabled = false;
				}
			}
			else
			{
				if (pageB < 2)
				{
					this.btnBFirst.Enabled = false;
					this.btnBPre.Enabled = false;
				}

			}
			this.lvButtonEditItem.Scrollable = true;
			if (dst.Tables["be_items"] != null)
				dst.Tables["be_items"].Clear();
			int nRows = 0;
			string kw = txtButtonEditSearchKW.Text;
			string barcode = txtButtonEditSearchBarcode.Text;
			string sc = " SELECT TOP " + sizeB + "* FROM (";
			sc += " SELECT DISTINCT TOP " + (sizeB * pageB) + " ";
			sc += " c.cat, c.s_cat, c.code, c.supplier_code ";
			//			sc += ", c.name, c.name_cn, b.barcode, i.id AS biid ";
			sc += ", c.name, c.name_cn, b.barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			//			sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
			sc += " WHERE 1 = 1 ";
			if (kw != "")
				sc += " AND (c.name LIKE N'%" + Program.EncodeQuote(kw) + "%' OR c.name_cn LIKE N'%" + Program.EncodeQuote(kw) + "%') ";
			else if (barcode != "")
				sc += " AND (c.supplier_code LIKE '%" + Program.EncodeQuote(barcode) + "%' OR b.barcode LIKE  '%" + Program.EncodeQuote(barcode) + "%') ";
			sc += " AND c.cat <> 'ServiceItem'";
			sc += " ORDER BY c.name desc";
			sc += " )a ORDER BY name ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "be_items");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			lvButtonEditItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["be_items"].Rows[i];
				string code = dr["code"].ToString();
				string sBarcode = dr["barcode"].ToString().Trim();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
				//				string biid = dr["biid"].ToString();
				name = name.Replace("'", "").Trim();
				name_cn = name_cn.Replace("'", "").Trim();
				//				if (barcode == "" || (name == "" && name_cn == ""))
				//					continue;

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(sBarcode);
				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add("");
				//				if (biid != "" && biid == lblButtonButtonItemId.Text)
				//					item.Selected = true;
				lvButtonEditItem.Items.Add(item);
			}
		}
		private void btnBNext_Click(object sender, EventArgs e)
		{
			this.btnBFirst.Enabled = true;
			this.btnBPre.Enabled = true;

			if (totalitemB % sizeB == 0)
			{
				if (pageB > totalitemB / sizeB - 2)
				{
					this.btnBNext.Enabled = false;
					this.btnBLast.Enabled = false;
				}
			}
			else
			{
				if (pageB > totalitemB / sizeB - 1)
				{
					this.btnBNext.Enabled = false;
					this.btnBLast.Enabled = false;
				}

			}
			pageB = pageB + 1;
			
			
			this.lvButtonEditItem.Scrollable = true;
			if (dst.Tables["be_items"] != null)
				dst.Tables["be_items"].Clear();
			int nRows = 0;
			string kw = txtButtonEditSearchKW.Text;
			string barcode = txtButtonEditSearchBarcode.Text;
			string sc = " SELECT TOP " + sizeB + "* FROM (";
			sc += " SELECT DISTINCT TOP " + (sizeB * pageB) + " ";
			sc += " c.cat, c.s_cat, c.code, c.supplier_code ";
			//			sc += ", c.name, c.name_cn, b.barcode, i.id AS biid ";
			sc += ", c.name, c.name_cn, b.barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			//			sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
			sc += " WHERE 1 = 1 ";
			if (kw != "")
				sc += " AND (c.name LIKE N'%" + Program.EncodeQuote(kw) + "%' OR c.name_cn LIKE N'%" + Program.EncodeQuote(kw) + "%') ";
			else if (barcode != "")
				sc += " AND (c.supplier_code LIKE '%" + Program.EncodeQuote(barcode) + "%' OR b.barcode LIKE  '%" + Program.EncodeQuote(barcode) + "%') ";
			sc += " AND c.cat <> 'ServiceItem'";
			sc += " ORDER BY c.name desc";
			sc += " )a ORDER BY name ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "be_items");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			lvButtonEditItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["be_items"].Rows[i];
				string code = dr["code"].ToString();
				string sBarcode = dr["barcode"].ToString().Trim();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
				//				string biid = dr["biid"].ToString();
				name = name.Replace("'", "").Trim();
				name_cn = name_cn.Replace("'", "").Trim();
				//				if (barcode == "" || (name == "" && name_cn == ""))
				//					continue;

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(sBarcode);
				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add("");
				//				if (biid != "" && biid == lblButtonButtonItemId.Text)
				//					item.Selected = true;
				lvButtonEditItem.Items.Add(item);
			}
		}
		private void btnBLast_Click(object sender, EventArgs e)
		{
			this.btnBNext.Enabled = false;
			this.btnBLast.Enabled = false;
			this.btnBFirst.Enabled = true;
			this.btnBPre.Enabled = true;

			if (totalitemB % sizeB == 0)
			{
				pageB = totalitemB / sizeB;
			}
			else
			{

				pageB = totalitemB / sizeB + 1;
			}
			this.lvButtonEditItem.Scrollable = true;
			if (dst.Tables["be_items"] != null)
				dst.Tables["be_items"].Clear();
			int nRows = 0;
			string kw = txtButtonEditSearchKW.Text;
			string barcode = txtButtonEditSearchBarcode.Text;
			string sc = " SELECT TOP " + sizeB + "* FROM (";
			sc += " SELECT DISTINCT TOP " + (sizeB * pageB) + " ";
			sc += " c.cat, c.s_cat, c.code, c.supplier_code ";
			//			sc += ", c.name, c.name_cn, b.barcode, i.id AS biid ";
			sc += ", c.name, c.name_cn, b.barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			//			sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
			sc += " WHERE 1 = 1 ";
			if (kw != "")
				sc += " AND (c.name LIKE N'%" + Program.EncodeQuote(kw) + "%' OR c.name_cn LIKE N'%" + Program.EncodeQuote(kw) + "%') ";
			else if (barcode != "")
				sc += " AND (c.supplier_code LIKE '%" + Program.EncodeQuote(barcode) + "%' OR b.barcode LIKE  '%" + Program.EncodeQuote(barcode) + "%') ";
			sc += " AND c.cat <> 'ServiceItem'";
			sc += " ORDER BY c.name desc";
			sc += " )a ORDER BY name ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "be_items");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			lvButtonEditItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["be_items"].Rows[i];
				string code = dr["code"].ToString();
				string sBarcode = dr["barcode"].ToString().Trim();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
				name = name.Replace("'", "").Trim();
				name_cn = name_cn.Replace("'", "").Trim();

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(sBarcode);
				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add("");
				lvButtonEditItem.Items.Add(item);
			}
		}
		private void ckbIndivisual_CheckedChanged(object sender, EventArgs e)
		{
			bool indivisual = ckbIndivisual.Checked;
			if(indivisual)
			{
				panelii.Visible = true;
				panelMenuPopup.Visible = false;
				panelii.BringToFront();
			}
			else
			{
				panelii.Visible = false;
				panelMenuPopup.Visible = true;
				panelMenuPopup.BringToFront();
			}
		}
		private void btnFirstS_Click(object sender, EventArgs e)
		{
			this.btnNextS.Enabled = true;
			this.btnLastS.Enabled = true;
			this.btnFirstS.Enabled = false;
			this.btnPreS.Enabled = false;
			pageS = 1;
//			STUpdateItemList();
			if (totalitemS % sizeS == 0)
			{
				if (pageS < 1)
				{
					this.btnFirstS.Enabled = false;
					this.btnPreS.Enabled = false;
				}
			}
			else
			{
				if (pageS < 2)
				{
					this.btnFirstS.Enabled = false;
					this.btnPreS.Enabled = false;
				}

			}
			int rows = 0;
			dgvST.Rows.Clear();
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtSTkw.Text.ToLower();
			string sc = " SELECT top " + sizeS + " * ";
			sc += " FROM ";
			sc += " (SELECT DISTINCT top " + (sizeS * pageS) + " c.name, c.name_cn, c.code, c.price1,c.manual_cost_frd, c.cat, c.s_cat, c.ss_cat, sq.qty AS stock ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += " WHERE 1 = 1 AND c.code > 1009 ";
//			sc += " ) AS DerivedItem ";
//			sc += " WHERE 1 = 1 ";
			try
			{
				int.Parse(kw);
				sc += " AND (code ='" + kw + "' OR barcode = '" + kw + "')";
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
				sc += " AND (LOWER(name) LIKE N'%" + kw + "%' OR LOWER(name_cn) LIKE N'%" + kw + "%'";
				sc += " OR LOWER(barcode) LIKE '%" + kw + "%')";
				if (cbSTCat.Text.ToLower() != "")
					sc += " AND LOWER(cat) =N'" + cbSTCat.Text.ToLower() + "'";
				if (cbSTSCat.Text.ToLower() != "")
					sc += " AND  LOWER(s_cat) = N'" + cbSTSCat.Text.ToLower() + "'";
				if (cbSTSSCat.Text.ToLower() != "")
					sc += " AND LOWER(ss_cat) = N'" + cbSTSSCat.Text.ToLower() + "'";
			}
			sc += " ORDER BY code DESC ";
			sc += ") a ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			double dSubQty = 0;
			double dSubtotal = 0;
			double dSubCostTotal = 0;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["list"].Rows[i];
				string barcode = dr["barcode"].ToString();
				string code = dr["code"].ToString();
				string name = dr["name"].ToString();
				string stock = dr["stock"].ToString();
				string description = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cost = dr["manual_cost_frd"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();

				double dQty = Program.MyDoubleParse(stock);
				double dPrice = Program.MyDoubleParse(price1);
				double dCost = Program.MyDoubleParse(cost);

				dSubQty += dQty;
				dSubtotal += dPrice * dQty;
				dSubCostTotal += dCost * dQty;

				string[] row = { code, barcode, cat, s_cat, ss_cat, name, stock, dPrice.ToString("c"), dCost.ToString("c") };
				dgvST.Rows.Add(row);
			}
			string[] rowe = { "", "", "", "", "", "Sub Total", dSubQty.ToString(), dSubtotal.ToString("c"), dSubCostTotal.ToString("c") };
			dgvST.Rows.Add(rowe);
		}

		private void btnPreS_Click(object sender, EventArgs e)
		{
			this.btnNextS.Enabled = true;
			this.btnLastS.Enabled = true;
			//			if(page <= 1)
			//			{
			//				this.btnPre.Enabled = false;
			//				this.btnFirst.Enabled = false;
			//			}
			pageS = pageS - 1;

			if (totalitemS % sizeS == 0)
			{
				if (pageS < 1)
				{
					this.btnFirstS.Enabled = false;
					this.btnPreS.Enabled = false;
				}
			}
			else
			{
				if (pageS < 2)
				{
					this.btnFirstS.Enabled = false;
					this.btnPreS.Enabled = false;
				}

			}
			int rows = 0;
			dgvST.Rows.Clear();
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtSTkw.Text.ToLower();
			string sc = " SELECT top " + sizeS+ " * ";
			sc += " FROM ";
			sc += " (SELECT DISTINCT top " + (sizeS * pageS) + " c.name, c.name_cn, c.code, c.price1,c.manual_cost_frd, c.cat, c.s_cat, c.ss_cat, sq.qty AS stock ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += " WHERE 1 = 1 AND c.code > 1009 ";
			//			sc += " ) AS DerivedItem ";
			//			sc += " WHERE 1 = 1 ";
			try
			{
				int.Parse(kw);
				sc += " AND (code ='" + kw + "' OR barcode = '" + kw + "')";
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
				sc += " AND (LOWER(name) LIKE N'%" + kw + "%' OR LOWER(name_cn) LIKE N'%" + kw + "%'";
				sc += " OR LOWER(barcode) LIKE '%" + kw + "%')";
				if (cbSTCat.Text.ToLower() != "")
					sc += " AND LOWER(cat) =N'" + cbSTCat.Text.ToLower() + "'";
				if (cbSTSCat.Text.ToLower() != "")
					sc += " AND  LOWER(s_cat) = N'" + cbSTSCat.Text.ToLower() + "'";
				if (cbSTSSCat.Text.ToLower() != "")
					sc += " AND LOWER(ss_cat) = N'" + cbSTSSCat.Text.ToLower() + "'";
			}
			sc += " ORDER BY code DESC ";
			sc += ") a ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			double dSubQty = 0;
			double dSubtotal = 0;
			double dSubCostTotal = 0;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["list"].Rows[i];
				string barcode = dr["barcode"].ToString();
				string code = dr["code"].ToString();
				string name = dr["name"].ToString();
				string stock = dr["stock"].ToString();
				string description = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cost = dr["manual_cost_frd"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();

				double dQty = Program.MyDoubleParse(stock);
				double dPrice = Program.MyDoubleParse(price1);
				double dCost = Program.MyDoubleParse(cost);

				dSubQty += dQty;
				dSubtotal += dPrice * dQty;
				dSubCostTotal += dCost * dQty;

				string[] row = { code, barcode, cat, s_cat, ss_cat, name, stock, dPrice.ToString("c"), dCost.ToString("c") };
				dgvST.Rows.Add(row);
			}
			string[] rowe = { "", "", "", "", "", "Sub Total", dSubQty.ToString(), dSubtotal.ToString("c"), dSubCostTotal.ToString("c") };
			dgvST.Rows.Add(rowe);

		}

		private void btnNextS_Click(object sender, EventArgs e)
		{
			this.btnFirstS.Enabled = true;
			this.btnPreS.Enabled = true;

			if (totalitemS % sizeS == 0)
			{
				if (pageS > totalitemS / sizeS - 2)
				{
					this.btnNextS.Enabled = false;
					this.btnLastS.Enabled = false;
				}
			}
			else
			{
				if (pageS > totalitemS / sizeS - 1)
				{
					this.btnNextS.Enabled = false;
					this.btnLastS.Enabled = false;
				}

			}
			pageS = pageS + 1;

			int rows = 0;
			dgvST.Rows.Clear();
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtSTkw.Text.ToLower();
			string sc = " SELECT top " + sizeS + " * ";
			sc += " FROM ";
			sc += " (SELECT DISTINCT top " + (sizeS * pageS) + " c.name, c.name_cn, c.code, c.price1,c.manual_cost_frd, c.cat, c.s_cat, c.ss_cat, sq.qty AS stock ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += " WHERE 1 = 1 AND c.code > 1009 ";
			//			sc += " ) AS DerivedItem ";
			//			sc += " WHERE 1 = 1 ";
			try
			{
				int.Parse(kw);
				sc += " AND (code ='" + kw + "' OR barcode = '" + kw + "')";
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
				sc += " AND (LOWER(name) LIKE N'%" + kw + "%' OR LOWER(name_cn) LIKE N'%" + kw + "%'";
				sc += " OR LOWER(barcode) LIKE '%" + kw + "%')";
				if (cbSTCat.Text.ToLower() != "")
					sc += " AND LOWER(cat) =N'" + cbSTCat.Text.ToLower() + "'";
				if (cbSTSCat.Text.ToLower() != "")
					sc += " AND  LOWER(s_cat) = N'" + cbSTSCat.Text.ToLower() + "'";
				if (cbSTSSCat.Text.ToLower() != "")
					sc += " AND LOWER(ss_cat) = N'" + cbSTSSCat.Text.ToLower() + "'";
			}
			sc += " ORDER BY code DESC ";
			sc += ") a ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");

				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			double dSubQty = 0;
			double dSubtotal = 0;
			double dSubCostTotal = 0;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["list"].Rows[i];
				string barcode = dr["barcode"].ToString();
				string code = dr["code"].ToString();
				string name = dr["name"].ToString();
				string stock = dr["stock"].ToString();
				string description = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cost = dr["manual_cost_frd"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();

				double dQty = Program.MyDoubleParse(stock);
				double dPrice = Program.MyDoubleParse(price1);
				double dCost = Program.MyDoubleParse(cost);

				dSubQty += dQty;
				dSubtotal += dPrice * dQty;
				dSubCostTotal += dCost * dQty;

				string[] row = { code, barcode, cat, s_cat, ss_cat, name, stock, dPrice.ToString("c"), dCost.ToString("c") };
				dgvST.Rows.Add(row);
			}
			string[] rowe = { "", "", "", "", "", "Sub Total", dSubQty.ToString(), dSubtotal.ToString("c"), dSubCostTotal.ToString("c") };
			dgvST.Rows.Add(rowe);
		}

		private void btnLastS_Click(object sender, EventArgs e)
		{
			this.btnNextS.Enabled = false;
			this.btnLastS.Enabled = false;
			this.btnFirstS.Enabled = true;
			this.btnPreS.Enabled = true;

			if (totalitemS % sizeS == 0)
			{
				pageS = totalitemS / sizeS;
			}
			else
			{

				pageS = totalitemS / sizeS + 1;
			}


			int rows = 0;
			dgvST.Rows.Clear();
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string kw = txtSTkw.Text.ToLower();
			string sc = " SELECT top " + sizeS + " * ";
			sc += " FROM ";
			sc += " (SELECT DISTINCT top " + (sizeS * pageS) + " c.name, c.name_cn, c.code, c.price1,c.manual_cost_frd, c.cat, c.s_cat, c.ss_cat, sq.qty AS stock ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += " WHERE 1 = 1 AND c.code > 1009 ";
			//			sc += " ) AS DerivedItem ";
			//			sc += " WHERE 1 = 1 ";
			try
			{
				int.Parse(kw);
				sc += " AND (code ='" + kw + "' OR barcode = '" + kw + "')";
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
				sc += " AND (LOWER(name) LIKE N'%" + kw + "%' OR LOWER(name_cn) LIKE N'%" + kw + "%'";
				sc += " OR LOWER(barcode) LIKE '%" + kw + "%')";
				if (cbSTCat.Text.ToLower() != "")
					sc += " AND LOWER(cat) =N'" + cbSTCat.Text.ToLower() + "'";
				if (cbSTSCat.Text.ToLower() != "")
					sc += " AND  LOWER(s_cat) = N'" + cbSTSCat.Text.ToLower() + "'";
				if (cbSTSSCat.Text.ToLower() != "")
					sc += " AND LOWER(ss_cat) = N'" + cbSTSSCat.Text.ToLower() + "'";
			}
			sc += " ORDER BY code DESC ";
			sc += ") a ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");

				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			double dSubQty = 0;
			double dSubtotal = 0;
			double dSubCostTotal = 0;
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["list"].Rows[i];
				string barcode = dr["barcode"].ToString();
				string code = dr["code"].ToString();
				string name = dr["name"].ToString();
				string stock = dr["stock"].ToString();
				string description = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cost = dr["manual_cost_frd"].ToString();
				string cat = dr["cat"].ToString();
				string s_cat = dr["s_cat"].ToString();
				string ss_cat = dr["ss_cat"].ToString();

				double dQty = Program.MyDoubleParse(stock);
				double dPrice = Program.MyDoubleParse(price1);
				double dCost = Program.MyDoubleParse(cost);

				dSubQty += dQty;
				dSubtotal += dPrice * dQty;
				dSubCostTotal += dCost * dQty;

				string[] row = { code, barcode, cat, s_cat, ss_cat, name, stock, dPrice.ToString("c"), dCost.ToString("c") };
				dgvST.Rows.Add(row);
			}
			string[] rowe = { "", "", "", "", "", "Sub Total", dSubQty.ToString(), dSubtotal.ToString("c"), dSubCostTotal.ToString("c") };
			dgvST.Rows.Add(rowe);
		}
		


		private void btnSTExport_Click(object sender, EventArgs e)
		{
			SaveFileDialog SFD = new SaveFileDialog();
			SFD.Filter = "CSV Files(*.csv)|*.csv";
			if (SFD.ShowDialog() == DialogResult.OK)
			{
				StringBuilder sb = new StringBuilder();

				foreach (DataGridViewColumn CH in dgvST.Columns)
				{
					sb.Append(CH.HeaderText.ToString() + ",");
				}
				sb.AppendLine();
				foreach (DataGridViewRow dgv in dgvST.Rows)
				{
					foreach (DataGridViewCell lvs in dgv.Cells)
					{
						if (lvs.Value.ToString().Trim() != "")
						{
							sb.Append(lvs.Value + ",");
						}
						else
							sb.Append("" + ",");
					}
					sb.AppendLine();
				}
				StreamWriter sw = new StreamWriter(SFD.FileName);
				sw.Write(sb.ToString());
				sw.Close();

			}
		}
		private void btnDel_Click(object sender, EventArgs e)
		{
			string path = Program.m_sPicroot + "\\" + m_sCode+ ".bmp";
			if (File.Exists(path))
			{
				Image image = pictureBox1.Image;
				pictureBox1.Image = null;
				image.Dispose();
				File.Delete(path);
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = " Pic Deleted! ";
				fm.ShowDialog();
				return;
			}
		}
		private void btnDel2_Click(object sender, EventArgs e)
		{
			string path = Program.m_sPicroot + "\\" + txtButtonCatName.Text + ".bmp";
			if (File.Exists(path))
			{
				Image image = pictureBox2.Image;
				pictureBox2.Image = null;
				image.Dispose();
				File.Delete(path);
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = " Pic Deleted! ";
				fm.ShowDialog();
				return;
			}
		}
		private void btnFirstP_Click(object sender, EventArgs e)
		{
			this.btnNextP.Enabled = true;
			this.btnLastP.Enabled = true;
			this.btnFirstP.Enabled = false;
			this.btnPreP.Enabled = false;
			pageP = 1;
			//			STUpdateItemList();
			if (totalitemP % sizeP == 0)
			{
				if (pageP < 1)
				{
					this.btnFirstP.Enabled = false;
					this.btnPreP.Enabled = false;
				}
			}
			else
			{
				if (pageP < 2)
				{
					this.btnFirstP.Enabled = false;
					this.btnPreP.Enabled = false;
				}

			}
			
			string kw = txtS.Text.ToLower();
			int nRows = 0;
			if (dst.Tables["itemlist"] != null)
				dst.Tables["itemlist"].Clear();
			string sc = " SELECT top "+sizeP+ "* FROM ("	;
			sc += " SELECT distinct top " + (sizeP * pageP) + " c.code, c.barcode, c.supplier_code, c.name, c.price1, c.cat,c.s_cat, c.ss_cat,  p.promo_desc ";
			sc += " ,pr.promo_id";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN promotion_list p on p.promo_id = c.promo_id ";
			sc += " Left outer join barcode b on b.item_code = c.code ";
			/*******/
			sc += " join promo pr on c.code = pr.code ";
			if(m_sPromoid != "-1")
				sc += " and pr.promo_id = '" + m_sPromoid + "'";
			/*******/
			sc += " WHERE 1 = 1 and c.is_special = 0 and c.cat != 'ServiceItem'";
			if (txtS.Text != "" && txtS.Text != null)
			{
				sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
				sc += " OR b.barcode = N'" + kw + "')";
			}
			if (comboBox3.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + comboBox3.Text.ToLower() + "'";
			if (comboBox2.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + comboBox2.Text.ToLower() + "'";
			if (comboBox1.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + comboBox1.Text.ToLower() + "'";

			sc += " ORDER BY c.code DESC ) a ";
			sc += " ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "itemlist");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			lvItem.CheckBoxes = true;
			lvItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["itemlist"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cat = dr["cat"].ToString();
				string scat = dr["s_cat"].ToString();
				string sscat = dr["ss_cat"].ToString();
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(price1);
				item.SubItems.Add(cat);
				item.SubItems.Add(scat);
				item.SubItems.Add(sscat);
				item.SubItems.Add(promo_desc);
				this.lvItem.Items.Add(item);
			}

			ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			this.lvItem.Items.Add(sum);
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.Red;
		}
		private void btnPreP_Click(object sender, EventArgs e)
		{
			this.btnNextP.Enabled = true;
			this.btnLastP.Enabled = true;
			pageP = pageP - 1;

			if (totalitemP % sizeP == 0)
			{
				if (pageP < 1)
				{
					this.btnFirstP.Enabled = false;
					this.btnPreP.Enabled = false;
				}
			}
			else
			{
				if (pageP < 2)
				{
					this.btnFirstP.Enabled = false;
					this.btnPreP.Enabled = false;
				}

			}
			string kw = txtS.Text.ToLower();
			int nRows = 0;
			if (dst.Tables["itemlist"] != null)
				dst.Tables["itemlist"].Clear();
			string sc = " SELECT top " + sizeP + " * FROM (";
			sc += " SELECT distinct top " + (sizeP * pageP) + " c.code, c.supplier_code, c.name, c.price1, c.cat,c.s_cat, c.ss_cat,  p.promo_desc, c.barcode ";
			sc += " ,pr.promo_id";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN promotion_list p on p.promo_id = c.promo_id ";
			sc += " Left outer join barcode b on b.item_code = c.code ";
			/***********/
			sc += " join promo pr on c.code = pr.code ";
			if (m_sPromoid != "-1")
				sc += " and pr.promo_id = '" + m_sPromoid + "'";
			/*******/
			sc += " WHERE 1 = 1 and c.is_special = 0 and c.cat != 'ServiceItem'";
			if (txtS.Text != "" && txtS.Text != null)
			{
				sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
				sc += " OR b.barcode = N'" + kw + "')";
			}
			if (comboBox3.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + comboBox3.Text.ToLower() + "'";
			if (comboBox2.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + comboBox2.Text.ToLower() + "'";
			if (comboBox1.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + comboBox1.Text.ToLower() + "'";

			sc += " ORDER BY c.code DESC ) a ";
			sc += " ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "itemlist");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			lvItem.CheckBoxes = true;
			lvItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["itemlist"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cat = dr["cat"].ToString();
				string scat = dr["s_cat"].ToString();
				string sscat = dr["ss_cat"].ToString();
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(price1);
				item.SubItems.Add(cat);
				item.SubItems.Add(scat);
				item.SubItems.Add(sscat);
				item.SubItems.Add(promo_desc);
				this.lvItem.Items.Add(item);
			}

			ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			this.lvItem.Items.Add(sum);
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.Red;
		}
		private void btnNextP_Click(object sender, EventArgs e)
		{
			this.btnFirstP.Enabled = true;
			this.btnPreP.Enabled = true;

			if (totalitemP % sizeP == 0)
			{
				if (pageP > totalitemP / sizeP - 2)
				{
					this.btnNextP.Enabled = false;
					this.btnLastP.Enabled = false;
				}
			}
			else
			{
				if (pageP > totalitemP / sizeP - 1)
				{
					this.btnNextP.Enabled = false;
					this.btnLastP.Enabled = false;
				}

			}
			pageP = pageP + 1;

			string kw = txtS.Text.ToLower();
			int nRows = 0;
			if (dst.Tables["itemlist"] != null)
				dst.Tables["itemlist"].Clear();
			string sc = " SELECT top " + sizeP + "* FROM (";
			sc += " SELECT distinct top " + (sizeP * pageP) + " c.code, c.supplier_code, c.name, c.price1, c.cat,c.s_cat, c.ss_cat,  p.promo_desc, c.barcode ";
			sc += " ,pr.promo_id";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN promotion_list p on p.promo_id = c.promo_id ";
			sc += " Left outer join barcode b on b.item_code = c.code ";
			/***********/
			sc += " join promo pr on c.code = pr.code ";
			if (m_sPromoid != "-1")
				sc += " and pr.promo_id = '" + m_sPromoid + "'";
			/*******/
			sc += " WHERE 1 = 1 and c.is_special = 0 and c.cat != 'ServiceItem'";
			if (txtS.Text != "" && txtS.Text != null)
			{
				sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
				sc += " OR b.barcode = N'" + kw + "')";
			}
			if (comboBox3.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + comboBox3.Text.ToLower() + "'";
			if (comboBox2.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + comboBox2.Text.ToLower() + "'";
			if (comboBox1.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + comboBox1.Text.ToLower() + "'";

			sc += " ORDER BY c.code DESC ) a ";
			sc += " ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "itemlist");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			lvItem.CheckBoxes = true;
			lvItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["itemlist"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cat = dr["cat"].ToString();
				string scat = dr["s_cat"].ToString();
				string sscat = dr["ss_cat"].ToString();
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(price1);
				item.SubItems.Add(cat);
				item.SubItems.Add(scat);
				item.SubItems.Add(sscat);
				item.SubItems.Add(promo_desc);
				this.lvItem.Items.Add(item);
			}

			ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			this.lvItem.Items.Add(sum);
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.Red;
		}

		private void btnLastP_Click(object sender, EventArgs e)
		{
			this.btnNextP.Enabled = false;
			this.btnLastP.Enabled = false;
			this.btnFirstP.Enabled = true;
			this.btnPreP.Enabled = true;

			if (totalitemP % sizeP == 0)
			{
				pageP = totalitemP / sizeP;
			}
			else
			{

				pageP = totalitemP / sizeP + 1;
			}

			string kw = txtS.Text.ToLower();
			int nRows = 0;
			if (dst.Tables["itemlist"] != null)
				dst.Tables["itemlist"].Clear();
			string sc = " SELECT top " + sizeP + "* FROM (";
			sc += " SELECT distinct top " + (sizeP * pageP) + " c.code, c.supplier_code, c.name, c.price1, c.cat,c.s_cat, c.ss_cat,  p.promo_desc, c.barcode ";
			sc += " ,pr.promo_id";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN promotion_list p on p.promo_id = c.promo_id ";
			sc += " Left outer join barcode b on b.item_code = c.code ";
			/***********/
			sc += " join promo pr on c.code = pr.code ";
			if (m_sPromoid != "-1")
				sc += " and pr.promo_id = '" + m_sPromoid + "'";
			/*******/
			sc += " WHERE 1 = 1 and c.is_special = 0 and c.cat != 'ServiceItem'";
			if (txtS.Text != "" && txtS.Text != null)
			{
				sc += " AND (LOWER(c.name) LIKE N'%" + kw + "%' OR LOWER(c.name_cn) LIKE N'%" + kw + "%'";
				sc += " OR b.barcode = N'" + kw + "')";
			}
			if (comboBox3.Text.ToLower() != "")
				sc += " AND LOWER(c.cat) =N'" + comboBox3.Text.ToLower() + "'";
			if (comboBox2.Text.ToLower() != "")
				sc += " AND  LOWER(c.s_cat) = N'" + comboBox2.Text.ToLower() + "'";
			if (comboBox1.Text.ToLower() != "")
				sc += " AND LOWER(c.ss_cat) = N'" + comboBox1.Text.ToLower() + "'";

			sc += " ORDER BY c.code DESC ) a ";
			sc += " ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "itemlist");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			lvItem.CheckBoxes = true;
			lvItem.Items.Clear();
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["itemlist"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["barcode"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string name = dr["name"].ToString();
				string price1 = dr["price1"].ToString();
				string cat = dr["cat"].ToString();
				string scat = dr["s_cat"].ToString();
				string sscat = dr["ss_cat"].ToString();
				string promo_id = dr["promo_id"].ToString();
				string promo_desc = dr["promo_desc"].ToString();
				ListViewItem item = new ListViewItem("");
				item.SubItems.Add(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(price1);
				item.SubItems.Add(cat);
				item.SubItems.Add(scat);
				item.SubItems.Add(sscat);
				item.SubItems.Add(promo_desc);
				this.lvItem.Items.Add(item);
			}

			ListViewItem sum = new ListViewItem("Select All");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			sum.SubItems.Add("");
			sum.SubItems.Add(" ");
			sum.SubItems.Add("");
			this.lvItem.Items.Add(sum);
			sum.Font = new Font("Arial", 10, FontStyle.Bold);
			sum.BackColor = System.Drawing.Color.Red;
		}

		private void groupBox2_Enter(object sender, EventArgs e)
		{

		}

		private void btnSearchC_Click(object sender, EventArgs e)
		{
			string kw = txtkey.Text.Trim();
			DoSelect(kw);
		}
		private void DoSelect(string kw)
		{
			int nRows = 0;
			if (dst.Tables["DoSelect"] != null)
				dst.Tables["DoSelect"].Clear();
			if (kw == "" || kw == null)
			{
				kw = txtCodeii.Text.Trim();
			}
			if (kw == "" || kw == null)
				return;	
			string sc = " SELECT c.cat, c.s_cat, c.code, c.supplier_code, c.barcode AS main_barcode, b.barcode ";
			sc += ", c.name, c.name_cn, i.id AS biid ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " LEFT OUTER JOIN button_item i ON i.code = c.code ";
			sc += " WHERE 1 = 1 ";
			if (kw != "")
			{
				sc += " AND c.name LIKE N'%" + Program.EncodeQuote(kw) + "%' COLLATE SQL_Latin1_General_CP1_CI_AS ";
				sc += " OR c.name_cn LIKE N'%" + Program.EncodeQuote(kw) + "%' COLLATE SQL_Latin1_General_CP1_CI_AS ";
				sc += " OR c.supplier_code LIKE N'%" + Program.EncodeQuote(kw) + "%' ";
				sc += " OR c.barcode LIKE N'%" + Program.EncodeQuote(kw) + "%' ";
				sc += " OR b.barcode LIKE N'%" + Program.EncodeQuote(kw) + "%' ";
				sc += " OR c.code LIKE N'%" + Program.EncodeQuote(kw) + "%' ";
			}
			sc += " ORDER BY c.name ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "DoSelect");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["DoSelect"].Rows[i];
				string code = dr["code"].ToString();
				string barcode = dr["main_barcode"].ToString().Trim();
				if(barcode == "")
					barcode = dr["barcode"].ToString();
				string name = dr["name"].ToString();
				string name_cn = dr["name_cn"].ToString();
				string biid = dr["biid"].ToString();
				name = name.Replace("'", "");
				name_cn = name_cn.Replace("'", "");

				ListViewItem item = new ListViewItem(code);
				item.SubItems.Add(barcode);
				item.SubItems.Add(name);
				item.SubItems.Add(name_cn);
				item.SubItems.Add("");
				if (biid != "")
					item.Selected = true;
				this.lvkey.Items.Add(item);
			}
		}

		private void lvkey_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListView.SelectedListViewItemCollection items = lvkey.SelectedItems;
			if (items.Count <= 0)
				return;
			string code = items[0].SubItems[0].Text;
			string barcode = items[0].SubItems[1].Text;
			string name = items[0].SubItems[2].Text;
			string name_cn = items[0].SubItems[3].Text;
			if (code.Trim() == "")
				return;
			txtCodeii.Text = code; //barcode;
		}
		private void button6_Click_1(object sender, EventArgs e)
		{
			AddUser au = new AddUser();
			au.m_bVIP = true;
			au.m_sTitle = "Add New VIP";
			au.ShowDialog();
			ShowVipList();
			return;
		}
		private void dgvvip_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 4)
			{
				string userBarcode = dgvvip.Rows[e.RowIndex].Cells["dgvbarcode"].Value.ToString();
				string userID = dgvvip.Rows[e.RowIndex].Cells["card_id"].Value.ToString();
				AddUser au = new AddUser();
				au.m_sBarcode = userBarcode;
				au.m_sCard_id = userID;
				au.m_bVIP = true;
				au.m_sTitle = "Edit VIP";
				au.m_bUpdate = true;
				au.m_sBarcode = userBarcode;
				au.m_sCard_id = userID;
				au.ShowDialog();


			}
			else if (e.ColumnIndex == 5)
			{
				if (MessageBox.Show("Are you sure you want to delete this VIP?", "Delete VIP", MessageBoxButtons.YesNo) != DialogResult.Yes)
					return;
				string userBarcode = dgvvip.Rows[e.RowIndex].Cells["dgvbarcode"].Value.ToString();
				DoRemoveUser(userBarcode);

			}
			ShowVipList();
		}
		private void button7_Click(object sender, EventArgs e)
		{
			ShowUserList();
			this.textBox2.Text = "";
			this.textBox3.Text = "";
		}
		private void button8_Click(object sender, EventArgs e)
		{
			ShowVipList();
			this.textBox2.Text = "";
			this.textBox3.Text = "";
		}
		private void lvPromotion_Click(object sender, EventArgs e)
		{
			DoColor();
		}
		private void DoColor()
		{
			for (int i = 0; i < lvPromotion.Items.Count; i++)
			{
				if (this.lvPromotion.Items[i].Selected)
					this.lvPromotion.Items[i].BackColor = System.Drawing.Color.Red;
				else
					this.lvPromotion.Items[i].BackColor = System.Drawing.Color.Transparent;
			}
		}
		private void button9_Click(object sender, EventArgs e)
		{
			btnStockTake_Click(sender,e);
		}
		private void lvVipsum_MouseClick(object sender, MouseEventArgs e)
		{
			selectVIP();
			DoVipItemSummary(m_iDateSelect);
			DoVipCatSummary(m_iDateSelect);
			m_sVip_id = "";
		}
		private void pnlPromotion_Paint(object sender, PaintEventArgs e)
		{

		}
		private void btnLevelPrice_Click(object sender, EventArgs e)
		{
			FormLevelPrice fl = new FormLevelPrice();
			fl.ShowDialog();
		}
		private void btnVIP_Click(object sender, EventArgs e)
		{
			DoLoadVIP();
			HideAllPanel();
			pnlVipAccount.Visible = true;
		}
		private void btnSearchVip_Click(object sender, EventArgs e)
		{
			DoLoadVIP();
			HideAllPanel();
			pnlVipAccount.Visible = true;
		}
		private bool DoLoadVIP()
		{
			FormSearchVip fm = new FormSearchVip();
			fm.ShowDialog();
			m_sVipCardId = fm.m_sCardId;
			if (m_sVipCardId == "")
				return false;
			return DoLoadVIPInv(false);
		}
		private bool DoLoadVIPInv(bool bPaid)
		{
			if (dst.Tables["vip"] != null)
				dst.Tables["vip"].Clear();
			int nRows = 0;
			string sc = " SELECT name, barcode, phone, email, address1, address2, address3 ";
			sc += " FROM card WHERE id = " + m_sVipCardId;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "vip");
				if (nRows <= 0)
					return false;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			DataRow dr = dst.Tables["vip"].Rows[0];
			lblVAName.Text = dr["name"].ToString();
			lblVABarcode.Text = dr["barcode"].ToString();

			if (dst.Tables["vipinv"] != null)
				dst.Tables["vipinv"].Clear();

			sc = " SELECT invoice_number, total, amount_paid, CONVERT(varchar(99), commit_date, 103) AS sdate ";
			sc += " FROM invoice ";
			sc += " WHERE card_id = " + m_sVipCardId;
			if (bPaid)
				sc += " AND amount_paid = total ";
			else
				sc += " AND amount_paid <> total ";
			sc += " ORDER BY commit_date ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "vipinv");
				if (nRows <= 0)
					return true;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}

			dgvVA.Rows.Clear();
			double dDue = 0;
			for (int i = 0; i < nRows; i++)
			{
				dr = dst.Tables["vipinv"].Rows[i];
				string inv = dr["invoice_number"].ToString();
				string sdate = dr["sdate"].ToString();
				double dTotal = Program.MyDoubleParse(dr["total"].ToString());
				double dPaid = Program.MyDoubleParse(dr["amount_paid"].ToString());
				double dBalance = dTotal - dPaid;
				dDue += dBalance;
				string[] aRow = { inv, sdate, dTotal.ToString("c"), dPaid.ToString("c"), dBalance.ToString("c"), "true" };
				dgvVA.Rows.Add(aRow);
			}
			lblVAAmount.Text = dDue.ToString("c");
			return true;
		}
		private void dgvVA_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == cc_check.Index)
			{
				bool bChecked = Program.MyBooleanParse(dgvVA.Rows[e.RowIndex].Cells["cc_check"].Value.ToString());
				dgvVA.Rows[e.RowIndex].Cells["cc_check"].Value = (!bChecked).ToString();
				VACalcTotal();
			}
		}
		private void VACalcTotal()
		{
			double dDue = 0;
			for(int i=0; i<dgvVA.Rows.Count; i++)
			{
				if(!Program.MyBooleanParse(dgvVA.Rows[i].Cells["cc_check"].Value.ToString()))
					continue;
				double dBalance = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_balance"].Value.ToString());
				dDue += dBalance;
			}
			lblVAAmount.Text = dDue.ToString("c");
		}
		void printDocVA_PrintPage(object sender, PrintPageEventArgs e)
		{
			int charactersOnPage = 0;
			int linesPerPage = 0;

			// Sets the value of charactersOnPage to the number of characters  
			// of stringToPrint that will fit within the bounds of the page.
			e.Graphics.MeasureString(stringToPrint, this.Font,
				e.MarginBounds.Size, StringFormat.GenericTypographic,
				out charactersOnPage, out linesPerPage);

			// Draws the string within the bounds of the page.
			e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
			e.MarginBounds, StringFormat.GenericTypographic);

			// Remove the portion of the string that has been printed.
			stringToPrint = stringToPrint.Substring(charactersOnPage);

			// Check to see if more pages are to be printed.
			e.HasMorePages = (stringToPrint.Length > 0);

			// If there are no more pages, reset the string to be printed. 
			if (!e.HasMorePages)
				stringToPrint = documentContents;
		}
		private void btnVAPrint_Click(object sender, EventArgs e)
		{
/*			VABuildStatement();
			WebBrowser webBrowserForPrinting = new WebBrowser();
			webBrowserForPrinting.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebPrintDocument);
			string uri = "file:///" + HttpUtility.UrlEncode(Application.StartupPath) + "/statement.html";
			webBrowserForPrinting.Url = new Uri(uri);
*/
			m_sPrintBuffer = VABuildStatementForReceiptPrinter();
			try
			{
				printDoc.Print();
			}
			catch (Exception e1)
			{
				MsgPrintError(printDoc, e1);
			}
		}
		private string VABuildStatementForReceiptPrinter()
		{
			string header = Program.ReadSitePage("statement_header");
			string footer = Program.ReadSitePage("statement_footer");
			header = header.Replace("@@DATE", DateTime.Now.ToString("dd-MM-yyyy"));

			DataRow dr = dst.Tables["vip"].Rows[0];
			string s = "[b]VIP ACCOUNT[/b]\r\n";
			s += "Name:" + dr["name"].ToString() + "\r\n";
			s += "Barcode:" + dr["barcode"].ToString() + "\r\n";
			s += " \r\n";
			s += "Inv_num  Date     Paid     Balance  \r\n";
/*			s += "Address:" + dr["address1"].ToString() + " " + dr["address2"].ToString() + " " + dr["address3"].ToString() + "\r\n";
			s += "Phone:" + dr["phone"].ToString() + "\r\n";
			s += "Email:" + dr["email"].ToString() + "\r\n";
			s += "\r\n";
*/			double dDue = 0;
			for (int i = 0; i < dgvVA.Rows.Count; i++)
			{
				if (!Program.MyBooleanParse(dgvVA.Rows[i].Cells["cc_check"].Value.ToString()))
					continue;
				string inv = dgvVA.Rows[i].Cells["cc_invoice"].Value.ToString();
				string sdate = dgvVA.Rows[i].Cells["cc_date"].Value.ToString();
				double dAmount = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_amount"].Value.ToString());
				double dPaid = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_amount_paid"].Value.ToString());
				double dBalance = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_balance"].Value.ToString());

				s += "" + inv + "  " + sdate + "  " + dPaid.ToString("c") + "  " +dBalance.ToString("c") + "\r\n";
				dDue += dBalance;
			}
			s += "Total:" + dDue.ToString("c") + "\r\n";
			return s;
		}
		private void VABuildStatement()
		{
			string header = Program.ReadSitePage("statement_header");
			string footer = Program.ReadSitePage("statement_footer");
			header = header.Replace("@@DATE", DateTime.Now.ToString("dd-MM-yyyy"));
			
			DataRow dr = dst.Tables["vip"].Rows[0];
			string s = "";
			s += @"
<html><head>
<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
<style type=text/css> 
body{background:#FFFFFF;font-size:10pt;font-family:verdana;}
td{FONT-WEIGHT:300;FONT-SIZE:10PT;FONT-FAMILY:vardana;background-color:inherit;}
tr{FONT-WEIGHT:300;font-size:10pt;font-family:verdana;background-color:inherit;align:left;}
.t{background-color:inherit;font-family:verdana;font-size:10pt;border-width:0px;border-style:Solid;border-collapse:collapse;fixed;}
</style></head><body>
";
			s += "<center>";
			s += header;
			s += "<table width=90% class=t border=0><tr><td>";
			s += "<table class=t border=0>";
			s += "<tr><td>To:</td><td>" + dr["name"].ToString() + "</td></tr>";
			s += "<tr><td>Barcode:</td><td>" + dr["barcode"].ToString() + "</td></tr>";
			s += "<tr><td>Address:</td><td>" + dr["address1"].ToString() + " " + dr["address2"].ToString() + " " + dr["address3"].ToString() + "</td></tr>";
			s += "<tr><td>Phone:</td><td>" + dr["phone"].ToString() + "</td></tr>";
			s += "<tr><td>Email:</td><td>" + dr["email"].ToString() + "</td></tr>";
			s += "<table>";
			s += "</td></tr>";
			s += "<tr><td><hr height=1 width=100%</td></tr>";
			s += "</table>";
			
			s += "<table width=90% class=t border=0><tr><th>Inv#</th><th>Date</th><th>Amount</th><th>Paid</th><th>Balance</th></tr>";
			s += "<tr><td colspan=5><hr height=1 width=100%</td></tr>";
			double dDue = 0;
			for (int i = 0; i < dgvVA.Rows.Count; i++)
			{
				if (!Program.MyBooleanParse(dgvVA.Rows[i].Cells["cc_check"].Value.ToString()))
					continue;
				string inv = dgvVA.Rows[i].Cells["cc_invoice"].Value.ToString();
				string sdate = dgvVA.Rows[i].Cells["cc_date"].Value.ToString();
				double dAmount = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_amount"].Value.ToString());
				double dPaid = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_amount_paid"].Value.ToString());
				double dBalance = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_balance"].Value.ToString());
				
				s += "<tr><td align=center>" + inv + "</td><td align=center>" + sdate + "</td><td align=right>" + dAmount.ToString("c") + "</td>";
				s += "<td align=right>" + dPaid.ToString("c") + "</td><td align=right>" + dBalance.ToString("c") + "</td></tr>";
				dDue += dBalance;
			}
			s += "<tr><td colspan=5><hr height=1 width=100%</td></tr>";
			s += "<tr><td colspan=4 align=right>Sub Total:</td><td align=right>" + dDue.ToString("c") + "</td></tr>";
			s += "</table>";
			s += footer.Replace("@@AMOUNTDUE", dDue.ToString("c"));
			s += "</html>";
			string path = Application.StartupPath + "\\statement.html";
			if(File.Exists(path))
				File.Delete(path);
			File.AppendAllText(path, s);	
		}
		private void PrintHelpPage()
		{
		}
		private void WebPrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			((WebBrowser)sender).ShowPrintPreviewDialog();
			// Print the document now that it is fully loaded.
//			((WebBrowser)sender).Print();
			// Dispose the WebBrowser now that the task is complete. 
//			((WebBrowser)sender).Dispose();
		}
		private void btnVAPay_Click(object sender, EventArgs e)
		{
			m_dPayAmount = 0;
			for (int i = 0; i < dgvVA.Rows.Count; i++)
			{
				if (!Program.MyBooleanParse(dgvVA.Rows[i].Cells["cc_check"].Value.ToString()))
					continue;
				string inv = dgvVA.Rows[i].Cells["cc_invoice"].Value.ToString();
				double dBalance = Program.MyMoneyParse(dgvVA.Rows[i].Cells["cc_balance"].Value.ToString());
				if(inv != "")
				{
					m_sInvoices += ",";
					m_sAmounts += ",";
				}
				m_sInvoices += inv;
				m_sAmounts += dBalance.ToString();
				m_dPayAmount += dBalance;
			}
			m_bPay = true; //bring up payment panel after close			
			this.Close();
		}
		private void txtsearch_TextChanged(object sender, EventArgs e)
		{

		}
		private void label140_Click(object sender, EventArgs e)
		{

		}
		private void btnEftPosControlPanel_Click(object sender, EventArgs e)
		{
			if (Program.m_eftposType == "verifone")
			{
//						CVault cv = new CVault();
//						VaultSession vs = new VaultSession();
//						vs.DisplayAdminMenu();
				using (VaultSession vs = new VaultSession("CHECKOUT1"))
				{
					vs.DisplayAdminMenu();
				}
			}
			else if (Program.m_eftposType == "nitro")
			{
				((form1)this.Owner).ShowNitroCP();
/*				CNitro cn = new CNitro();
				cn.Connect();
				cn.DisplayControlPanel();
				cn.Disconnect();
				cn = null;
*/ 
			}
		}
		private void tabcreport_MouseClick(object sender, MouseEventArgs e)
		{
		}
		private void lvStockReport_MouseClick(object sender, MouseEventArgs e)
		{
			selectCode();
			showInputReportDetail(m_iDateSelect);
			m_sItemCode = "";
		}
		public void ReceiveKeyboardKey(string s)
		{
			Control c = m_hLastFocusedControl;
			if(c == null)
				c = Program.FindFocusedControl(this);
			if(c == null)
				return;
				
			int p = 0;
			if(c is TextBox)
				p = ((TextBox)c).SelectionStart;
			else if(c is RichTextBox)
				p = ((RichTextBox)c).SelectionStart;
			string sf = c.Text.Substring(0, p);
			string sb = c.Text.Substring(p, c.Text.Length - p);
			string sr = "";
			int pNew = p;
			if(s == "del")
			{
				if(sf.Length > 0)
				{
					sr = sf.Substring(0, sf.Length - 1) + sb;
					pNew = p - 1;
				}
			}
			else
			{
				sr = sf + s + sb;
				pNew = p + s.Length;
			}
			c.Text = sr;
			if (c is TextBox)
				((TextBox)c).SelectionStart = pNew;
			else if (c is RichTextBox)
				((RichTextBox)c).SelectionStart = pNew;
		}
		private void btnKeyboard_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("osk.exe");
/*			FormKeyboard fm = new FormKeyboard();
			fm.m_hDest = this;
			fm.m_x = this.Bounds.X + 175;
			fm.m_y = this.Bounds.Y + 370;
			fm.Show();
*/ 
		}
		private void gbCategory_Enter(object sender, EventArgs e)
		{
		}
		private void btnSTExportAll_Click(object sender, EventArgs e)
		{
			ExportAllProduct();
			int nRows = dst.Tables["list"].Rows.Count;
			
			SaveFileDialog SFD = new SaveFileDialog();
			SFD.Filter = "CSV Files(*.csv)|*.csv";
			if (SFD.ShowDialog() == DialogResult.OK)
			{
				StringBuilder sb = new StringBuilder();

				foreach (DataGridViewColumn CH in dgvST.Columns)
				{
					sb.Append(CH.HeaderText.ToString() + ",");
				}
				sb.AppendLine();
				for (int i = 0; i < nRows; i++)
				{
					DataRow dr = dst.Tables["list"].Rows[i];
					string barcode = dr["barcode"].ToString();
					string code = dr["code"].ToString();
					string name = dr["name"].ToString();
					string stock = dr["stock"].ToString();
					string description = dr["name"].ToString();
					string price1 = dr["price1"].ToString();
					string cost = dr["manual_cost_frd"].ToString();
					string cat = dr["cat"].ToString();
					string s_cat = dr["s_cat"].ToString();
					string ss_cat = dr["ss_cat"].ToString();
					double dQty = Program.MyDoubleParse(stock);
					double dPrice = Program.MyDoubleParse(price1);
					double dCost = Program.MyDoubleParse(cost);
					
					sb.Append(code + "," + barcode + ","+cat+","+s_cat+","+ss_cat+","+name+","+dQty.ToString()+","+dPrice.ToString()+","+dCost.ToString()+",");
					sb.AppendLine();
				}		
				StreamWriter sw = new StreamWriter(SFD.FileName);
				sw.Write(sb.ToString());
				sw.Close();
			}
			FormMSG fm = new FormMSG();
			fm.m_sMsg = "Export All Items!";
			fm.btnNo.Visible = false;
			fm.btnYes.Visible = false;
			fm.btnOK.Visible = true;
			fm.ShowDialog();

		}
		private void ExportAllProduct()
		{
			int rows = 0;
			if (dst.Tables["list"] != null)
				dst.Tables["list"].Clear();
			string sc = " SELECT ";
			sc += "  c.code ";
			sc += ", ISNULL((SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code), '') AS barcode ";
			sc += ", c.cat, c.s_cat, c.ss_cat, c.name ,  sq.qty AS stock,  c.price1,c.manual_cost_frd ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN stock_qty sq ON sq.code = c.code AND sq.branch_id = 1 ";
			sc += " WHERE 1 = 1 AND c.code > 1009 ";
			sc += " ORDER BY code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "list");

				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			double dSubQty = 0;
			double dSubtotal = 0;
			double dSubCostTotal = 0;
		}
		private void lvPromotion_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		private void btnAssign_Click_1(object sender, EventArgs e)
		{
		}
		private void btnComboAdd_Click(object sender, EventArgs e)
		{
			DoAddComboItem();
		}
		private void lvCombo_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvCombo.SelectedItems;
			if (items.Count <= 0)
				return;
			if (items[0].GetSubItemAt(e.X, e.Y).Text != "del")
				return;
			string barcode = items[0].SubItems[0].Text;
			//			if (barcode.Trim() == "")
			//				return;
			DoDeletePromotionGB(lblPromoId.Text, barcode);
		}
		private void btnPromoSearch_Click(object sender, EventArgs e)
		{
			RefreshPromoList();
		}
		private void btnVIPUnpaidInv_Click(object sender, EventArgs e)
		{
			DoLoadVIPInv(false);
		}
		private void btnVIPPaidInv_Click(object sender, EventArgs e)
		{
			DoLoadVIPInv(true);
		}

		private void btnDealerPriceApply_Click(object sender, EventArgs e)
		{
			string cat = Program.EncodeQuote(cbDealerPriceCat.Text);
			string s_cat = Program.EncodeQuote(cbDealerPriceSCat.Text);
			string ss_cat = Program.EncodeQuote(cbDealerPriceSSCat.Text);
			string selected_category = "";
			if (ss_cat != "")
				selected_category = "'" + ss_cat + "' category";
			else if (s_cat != "")
				selected_category = "'" + s_cat + "' category";
			else if (cat != "")
				selected_category = "'" + cat + "' category";
			else
				selected_category = "all categories";

			FormMSG fm1 = new FormMSG();
			fm1.m_sMsg = "Are you sure you want to apply for all new level price to all products under " + selected_category + " ? click Yes to apply.";
			fm1.btnNo.Visible = true;
			fm1.btnYes.Visible = true;
			fm1.btnOK.Visible = false;
			fm1.ShowDialog();
			if (!fm1.m_bYes)
				return;
			
			//if (MessageBox.Show("Are you sure you want to apply for all new level price to all products under " + selected_category + " ? click Yes to apply", "Confirm to apply", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				double rate1 = Program.MyDoubleParse(tbLevelRate1.Text) / 100;
				double rate2 = Program.MyDoubleParse(tbLevelRate2.Text) / 100;
				double rate3 = Program.MyDoubleParse(tbLevelRate3.Text) / 100;
				double rate4 = Program.MyDoubleParse(tbLevelRate4.Text) / 100;
				double rate5 = Program.MyDoubleParse(tbLevelRate5.Text) / 100;
				double rate6 = Program.MyDoubleParse(tbLevelRate6.Text) / 100;

				double d_taxrate = 0.15;
				string sc = " UPDATE code_relations SET ";
				sc += "  level_price1 = " + rate1 + " * price1 / " + (1 + d_taxrate);
				sc += ", level_price2 = " + rate2 + " * price1 / " + (1 + d_taxrate);
				sc += ", level_price3 = " + rate3 + " * price1 / " + (1 + d_taxrate);
				sc += ", level_price4 = " + rate4 + " * price1 / " + (1 + d_taxrate);
				sc += ", level_price5 = " + rate5 + " * price1 / " + (1 + d_taxrate);
				sc += ", level_price6 = " + rate6 + " * price1 / " + (1 + d_taxrate);
				sc += " WHERE 1=1 ";
				if (ss_cat != "")
					sc += " AND ss_cat= N'" + ss_cat + "' ";
				else if (s_cat != "")
					sc += " AND s_cat= N'" + s_cat + "' ";
				else if (cat != "")
					sc += " AND cat= N'" + cat + "' ";
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e1)
				{
					Program.ShowExp(sc, e1);
					return;
				}

				FormMSG fm2 = new FormMSG();
				fm2.m_sMsg = "Level price applied successfully.";
				fm2.btnNo.Visible = false;
				fm2.btnYes.Visible = false;
				fm2.ShowDialog();
				return;
			}
		}



        private void txtSIBarcode_Click(object sender, EventArgs e)
        {

        }

        private void txtSIBarcode_TextChanged(object sender, EventArgs e)
        {
            dgdItemList();
        }

        private void cbC_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbS.SelectedItem = "";
            cbS.SelectedText = "";
            cbS.SelectedValue = "";
            dgdUpdateSCat();
            cbSS.SelectedItem = "";
            cbSS.SelectedText = "";
            cbSS.SelectedValue = "";
            dgdUpdateSSCat();

            dgdItemList();
        }

        private void cbS_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbSS.SelectedItem = "";
            cbSS.SelectedText = "";
            cbSS.SelectedValue = "";
            dgdUpdateSSCat();
            dgdItemList();
        }

        private void cbSS_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgdItemList();
        }

        private void dgvDefault_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
                return;
            DataGridViewRow row = this.dgvDefault.Rows[e.RowIndex];
            if (row != null && e.ColumnIndex == 8)
            {
                string barcode = row.Cells[1].Value.ToString();
                this.txtSIBarcode.Text = barcode;
                txtSIQty.Select();
            }
            return;
        }

        private void dgvSI_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            DataGridViewRow row = this.dgvSI.Rows[e.RowIndex];
			string dataType = row.Cells[0].Value.ToString();
			string AppDataId = row.Cells[1].Value.ToString();
            if (row != null && e.ColumnIndex == 10)
            {
				if (dataType == "app")
				{
					string sc = " DELETE FROM app_stockinput WHERE id = " + AppDataId;
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception e1)
					{
						myConnection.Close();
						Program.ShowExp(sc, e1);
						return;
					}
				}	
				dgvSI.Rows.Remove(row);
            }
            TotalInput();
            return;
        }

		private void dgvSI_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
				return;
			DataGridViewRow row = this.dgvSI.Rows[e.RowIndex];
			string dataType = row.Cells[0].Value.ToString();
			string AppDataId = row.Cells[1].Value.ToString();
			if (row != null && (e.ColumnIndex == 8 || e.ColumnIndex == 9))
			{
				if (dataType == "app")
				{
					string newQty = row.Cells[8].Value.ToString();
					string newCost = row.Cells[9].Value.ToString();
					string sc = " UPDATE app_stockinput SET qty = " + newQty + ",cost = " + newCost + " WHERE id = " + AppDataId;
					try
					{
						myCommand = new SqlCommand(sc);
						myCommand.Connection = myConnection;
						myCommand.Connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception e1)
					{
						myConnection.Close();
						Program.ShowExp(sc, e1);
						return;
					}
				}
			}
			TotalInput();
		}
		private void txtSIPrice_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r' && txtSIBarcode.Text != "")
				txtSIQty.Select();
		}
		private void btnLabelPrint_Click(object sender, EventArgs e)
		{
			string lpBarcode = labelPrintBarcode.Text;
			if (lpBarcode == "label Print Barcode")
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Please select an item.";
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.ShowDialog();
				return;
			}
			string sPath = AppDomain.CurrentDomain.BaseDirectory;
			string fn = sPath + "LabelPrint.exe";
			if (!File.Exists(fn))
			{
				MessageBox.Show("LabelPrint.exe not found");
				return;
			}
			IntPtr hWnd = Program.MyFindWindow("GPOS Label Print");
			if(hWnd != IntPtr.Zero)
			{
//				string fnb = sPath + "LabelPrintBarcode.txt";
//				File.WriteAllText(sPath, lpBarcode);
				int WM_KEYDOWN = 0x0100;
				Message msg = Message.Create(hWnd, WM_KEYDOWN, new IntPtr(0x01), new IntPtr(0));
				PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
				for (int i = 0; i < lpBarcode.Length; i++)
				{
					int nKey = lpBarcode[i];
					msg = Message.Create(hWnd, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
					PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
				}
				msg = Message.Create(hWnd, WM_KEYDOWN, new IntPtr(0x02), new IntPtr(0));
				PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
				Program.MsgBox("Barcode added to LabelPrint Window");
				return;
			}
			try
			{
				Process backup = new Process();
				backup.StartInfo.FileName = fn;
				backup.StartInfo.Arguments = "barcode=" + lpBarcode;
				backup.Start();
			}
			catch (Exception e1)
			{
				MessageBox.Show(e1.ToString());
			}
		}

		private void BtnAddCurrency_Click(object sender, EventArgs e)
		{
			if (this.txtCurrency.Text == "" || this.txtCurrency.Text == null || this.txtRate.Text == "" || this.txtRate.Text == null)
				return;
			var currency = this.txtCurrency.Text.ToUpper();
			double rates = 0;
			if (Regex.IsMatch(this.txtRate.Text, @"^\d+(\.\d+)?$"))
				rates = double.Parse(this.txtRate.Text);
			else
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "Foreign Currency Rates must be numbers!";
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.ShowDialog();
				return;
			}
			if (insertCurrency(currency, rates, ""))
			{
				FormMSG fm = new FormMSG();
				fm.m_sMsg = "New Currency Setting Added!";
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.ShowDialog();
			}
			RefreshLvCurrency();
		}

		private bool insertCurrency(string currency, double rates, string insertBy)
		{
			string sc = "";
			sc = " if not exists (select * from currency where currency_name = '" + currency + "')";
			sc += " insert into currency(currency_name, rates, insert_by)values('" + currency + "', '" + rates + "', '"+insertBy+"')";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();

			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return false ;
			}
			return true;
		}

		private void LvCurrency_MouseClick(object sender, MouseEventArgs e)
		{
			string id = "";
			ListView.SelectedListViewItemCollection items = this.lvCurrency.SelectedItems;
			if (items.Count <= 0)
				return;
			if (items[0].GetSubItemAt(e.X, e.Y).Text != "del")
				return;
			id = items[0].SubItems[0].Text;
			if (MessageBox.Show("Are you sure you want to delete this currency? click Yes to delete", "Confirm deleting", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return;
			}
			DoDeleteCurrency(id);
		}

		private void DoDeleteCurrency(string id)
		{
			string sc = " DELETE FROM currency WHERE id = '" + id + "' ";
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
				return;
			}
			RefreshLvCurrency();
		}

		private void RefreshLvCurrency()
		{
			this.txtCurrency.Text = "";
			this.txtRate.Text = "";
			lvCurrency.Items.Clear();
			int rows = 0;
			if (dst.Tables["RefreshLvCurrency"] != null)
				dst.Tables["RefreshLvCurrency"].Clear();
			string sc = "";
			sc += " SElect * from currency where 1=1 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "RefreshLvCurrency");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			if (rows == 0)
				return;

			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["RefreshLvCurrency"].Rows[i];
				string id = dr["id"].ToString();
				string currency = dr["currency_name"].ToString();
				string rate = dr["rates"].ToString();

				ListViewItem item = new ListViewItem(id);
				item.SubItems.Add(currency);
				item.SubItems.Add(rate);
				item.SubItems.Add("del");
				this.lvCurrency.Items.Add(item);
			}

		}
	}
}
