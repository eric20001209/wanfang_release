using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;
using Microsoft.Win32;
using System.Web.Security;
using Sektor.Vault.Common;
using Sektor.Vault.POSInterface;
using PCEFTIPInterface;
using QPOS2008.nz.co.gcloud.rst;
using System.Net.Sockets;
using SplitAndInput;

namespace QPOS2008
{
	public partial class form1 : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		private DataSet dsCart = new DataSet();
		private int m_nCurrentCart = 0;
		private System.Drawing.Color sdcBackOrg = System.Drawing.Color.Gray;
		private System.Drawing.Color sdcForeOrg = System.Drawing.Color.White;
		private System.Drawing.Color sdcBackHigh = System.Drawing.Color.DarkBlue;
		private System.Drawing.Color sdcForeHigh = System.Drawing.Color.Red;

		private PrintDocument printDoc = new PrintDocument();

		//private SqlConnection myConnectionLoyalty;

		private double m_dTotal = 0;
		private double m_dTotalPaid = 0;
		private double m_dVolumnSumPrice = 0;
		private double m_dCash = 0;
		private double m_dEftpos = 0;
		private double m_dCashout = 0;
		private double m_dCredit = 0;
		private double m_dCheque = 0;
        private double m_dChange = 0;

		private bool m_bSurcharge = false;
		private double m_dSurchargeRate = 0;
		private double m_dSurcharge = 0;

		private double m_dCashTotal = 0;
		private double m_dEftposTotal = 0;
		private double m_dCashoutTotal = 0;
		private double m_dCreditTotal = 0;
		private double m_dChequeTotal = 0;
		private double m_dChargeTotal = 0;
		private double m_dSpecialTotal = 0;
		private double m_dDiscountTotal = 0;
		private double m_dRoundingTotal = 0;
		private double m_dOrderTotal = 0;
		private double m_dDiscRate = 0;

		//		private double m_sReadBarcodePriceLength1 = 0;
		//		private double m_sReadBarcodePriceLength2 = 0;
		//		private double m_sReadBarcodePriceLength3 = 0;
		private double m_dCashInTotal = 0;
		private double m_dRoundingReceipt = 0;

		private double dCustomerGst = 0.15;
		
		//		private int time = 0;
		private int m_iVoucherControl = 0;
		public static bool FormConfig_Show = false;
		private string m_sNewPriceCodeFlag = "0";
		private string m_irow = "";
		private string m_AddonQty = "0";


		private string m_sCode = "";	//used for searching return
		private string m_sCardId = ""; //vip card id
		private string m_sCardName = "";
		private string m_sCardBarcode = "";
		private string m_invoiceNumber = "";
		private string sNewPrice = "";
		private string sNewBarcode = "";
		private string m_sLastCart1 = "";
		private string m_sLastCart2 = "";
		private string m_sLastCart3 = "";
		private string m_sLastCart4 = "";
		private string sPrintLastReceipt = "";
		private string m_sPrintBuffer = "";
		private string m_sVoucherList = "";
		private string m_sLastReceipt = "";
		private string m_sLastSecondReceipt = "";
		private string m_sLastSecondReceiptOrderNumber = "";
		public string m_sRoundingNum = "4";
		public string[] m_sDiscount = new string[5];
		public string[] m_membernumber = new string[5];
		public string[] m_memberpoint = new string[5];
		public string[] m_nameEn = new string[512];
		public string[] m_isIndivisual = new string[512];
		public string m_sTotalPoint = "";
		private string m_sVoucherItemBarcode = "";
		private string m_sLoadShortKey = "";
		//EFTPOS
		private string m_sReceipt = "";
		private string m_sPosRefCurrent = "";
		private double m_sPosRefLastest;

		private int m_iEftposLog = 0;
		private int m_nCardId = 0;
		private int m_nSalesId = 0;
		private int m_nBranchId = 1;
		private int m_nOrderId = 0;
		private int m_nMemberPoints = 0;
		private double m_dPointEarnOnSales = 0;
		private int m_sCurrentCartID = 0;
		private int m_nTotalLegalPoint = 0;
		private int m_iNumReceiptPrintOut = 1;

		private bool sAllowDiscountRollBack = false;
		private bool m_bChineseDesc = false;
		private bool m_bPackageDue = false;
		private bool bIs_Refund = false;
		public bool m_bBeDisced = false;
		private bool m_bMemberPrice = false;
		private bool m_bScanMember = false;
		private bool m_bExistingMember = false;
		private bool m_bCashOutReminder = false;
		private bool m_bSpecialItemDis = false;
		private string m_sDiscountCancelled = "0";
		private string m_sAdminAction = "";
		private double m_dNewTotal = 0;
		private double m_dOldTotal = 0;
		private double m_dRedeemStartWith = 0;
		private double m_dRedeemBase = 0;

		private FormAD m_fmad = new FormAD();
		private FormADS m_fmads = new FormADS();
		private Control m_lastFocused = null;

		private string m_sVoucherAmount = "";
		private string m_sVoucherBarcode = "";
		private string m_sVoucherItem = "";
		private string m_sVoucherValidDay = "30";
		private string m_sVoucherUsedBarcode = "";
		private string m_sVoucherText = "";
		private string m_sVoucherMsg = "";
		private bool m_bVoucherEnabled = false;

        private bool m_bVipVoucherEnabled = false;
        private double m_dVipVoucherStartPoints = 500;
        private double m_dVipVoucherValue = 10;

        private bool m_bNoVoucher = true;

		private string m_sCompanyName = "GPOS SYSTEM";

		private double m_dVoucherStart = 0;
		private double m_dVoucherRate = 0;

		private bool m_bMemberDiscount = false;
		private bool m_bHasMemberExpired = false;
		private string m_sMemberDiscountRate = "0";
		private string m_sMemberPriceLevel = "0";
		DateTime m_sMemberShipExpiredDate;

		private bool m_bTrial = false;
		private string m_sTrialDateLeft = "";
		private bool m_bTrialExpired = false;
		private bool m_bEftposWaiting = false;
		private int m_nEftposWaiting = 3000;
		private EFTPOS eftPanel = new EFTPOS();

		private string m_sCashoutcontrol = "";
		private string m_sCashOverChangeControl = "";
		private string m_sButton11 = "";
		private string m_sButton12 = "";
		private string m_sDeleteItemControl = "";
        private string m_sDeleteItemBarcode = "";
		private string m_sPosRef = "";
		private string m_sClearPaymentControl = "";
		private string m_sVipPointRate = "";
		private string m_sDiscountSpecialItem = "";
		private bool m_bCheckGroupPromotion = false; //flag in timer
        private bool m_bCheckComboPromotion = false;
        private int m_iPromotionItemIndex = 0;
		private string m_sPromotionServiceItemCode = "-1001"; //for group promotion service item
		private string m_sCreditSurchargeItemCode = "1001"; //for credit surcharge service item
		private Modem m_mod = null; //for serial device data send
		private Modem.SetCallback call;

		private double m_dVipPayAmount = 0;
		private string m_sVipInvoices = "";
		private string m_sVipAmounts = "";
		private string m_sVipPaymentTypes = "";
		private string m_sNote = "";
		private string m_sSoundDonePath = "";
		private int m_nFlashCount = 0;
		private int m_nMenuFontSize = 10;
		private bool m_bQtyEditing = false;
		private string m_sTimer4Task = "";
		private string m_sTimer4Code = "";
		private double m_dTimer4Qty = 0;
		private int m_nTimer4Index = 0;
		private bool m_bIdCheckDone = false;
		private bool m_bCheckVaultDone = false;
		private string m_sMarginRate = "0";
		private string m_sReceiptVoucher = "";
		private string m_sUsingPrinterName = "";

		public form1()
		{
			//SetForegroundWindow(this.Handle);
			if (Program.m_sDMoniter == true)
			{
				if (Program.m_bRegularMonitor)
				{
					m_fmad.m_hParent = this.Handle;
					m_fmad.Show();
				}
				else if (Program.m_bSmallMonitor)
				{
					m_fmads.m_hParent = this.Handle;
					m_fmads.Show();
				}
			}
			try
			{
				InitializeComponent();
			}
			catch (Exception e)
			{
				Program.ShowExp("init form1 failed", e);
				return;
			}
			printDoc.PrinterSettings.PrinterName = Program.m_sPrinterName;
			printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
			barcode.Focus();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			DoCheckLicense();//Check Key
			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			labelBalance.Text = "";
			lbl_showhold.Text = "";
			Change.Text = "";

			this.Size = new Size(1027, 771);
			this.CreateNewItemPanel.Location = new Point(212, 0);
			this.CreateNewItemPanel.Size = new Size(595, 425);
			this.CreateNewItemPanel.BringToFront();
			this.cart.Location = new Point(5, 49);
			this.cart.Size = new Size(670, 395);
			//			cart.Visible = false;
			this.pnlNumPad.Size = new Size(334, 268);
			this.pnlNumPad.Location = new Point(680, 460);
			this.panelMenuLeft.Size = new Size(678, 263);
			this.panelMenuLeft.Location = new Point(5, 461);
			this.pnlInvVou.Size = new Size(540, 157);
			this.pnlInvVou.Location = new Point(8, 345);
			pnlkeyininv.Location = new Point(119, 121);
			pnlkeyininv.Size = new Size(486, 206);
			panelDiscount.Size = new Size(500, 250);
			panelDiscount.Location = new Point(441, 133);
			this.panelChangeTare.Size = new Size(260, 160);
			this.panelChangeTare.Location = new Point(200, 167);
			panelChangeQty.Size = new Size(260, 160);
			panelChangeQty.Location = new Point(178, 132);

			panelSurcharge.Size = new Size(260, 160);
			panelSurcharge.Location = new Point(178, 132);

			panelMenuDepartment.Size = new Size(83, 395);
			panelMenuDepartment.Location = new Point(677, 49);

			this.pnlAddOnQty.Location = new Point(70, 200);
			this.pnlAddOnQty.Size = new Size(590, 288);

			this.flAdminBtns.Visible = false;
			//			this.flAdminBtns.Size = new Size(255,333);
			this.flAdminBtns.Location = new Point(761, 247);
			panelHold.Size = new Size(271, 48);
			panelHold.Location = new Point(415, 81);
			panelHold.Visible = true;
			//			buttonLan.Text = "EN";
			labelcashout.Text = "";
			showcashout.Text = "";
			msgboard.Text = "";
			msgboard2.Text = "";
			txtpaymentinfo.Text = "";
			showItems.Text = "";
            lblQtys.Text = "";
			lblprocess.Text = "";
			if (Program.MyBooleanParse(GetSiteSettings("Allow_discount_roll_back")))
				sAllowDiscountRollBack = true;
			if (GetSiteSettings("margin_rate") != "")
				m_sMarginRate = GetSiteSettings("margin_rate");
				
			m_sCashoutcontrol = GetSiteSettings("maxAllowCashout");
			m_sCashOverChangeControl = GetSiteSettings("cash_overcharge_times");

			m_sVoucherList = GetSiteSettings("Voucher");
			m_sVoucherItemBarcode = GetSiteSettings("voucher_item_barcode");
			//			sPasswordControl = GetSiteSettings("discount_password_control");
			string voucherStart = GetSiteSettings("QPOS_REDEEM_START_WITH");
			string voucherBase = GetSiteSettings("QPOS_REDEEM_BASE");
			m_sCompanyName = GetSiteSettings("company_name");
			if (GetSiteSettings("total_no_of_receipt_printout") != "")
				m_iNumReceiptPrintOut = int.Parse(GetSiteSettings("total_no_of_receipt_printout"));
			if (voucherStart == "")
				voucherStart = "0";
			if (voucherBase == "")
				voucherBase = "0";
			m_dRedeemStartWith = double.Parse(voucherStart);
			m_dRedeemBase = double.Parse(voucherBase);

			m_sVoucherItem = GetSiteSettings("voucher_item_code");
			m_sVoucherValidDay = GetSiteSettings("voucher_expire_day");
			m_dVoucherRate = Program.MyDoubleParse(GetSiteSettings("voucher_rate"));
			m_dVoucherStart = Program.MyDoubleParse(GetSiteSettings("voucher_start_value"));
			m_bVoucherEnabled = Program.MyBooleanParse(GetSiteSettings("voucher_enabled"));

            m_bVipVoucherEnabled = Program.MyBooleanParse(GetSiteSettings("vip_voucher_enabled"));
            m_dVipVoucherStartPoints = Program.MyDoubleParse(GetSiteSettings("vip_voucher_start_points"));
            m_dVipVoucherValue = Program.MyDoubleParse(GetSiteSettings("vip_voucher_value"));

            m_bNoVoucher = Program.MyBooleanParse(GetSiteSettings("no_voucher"));

			if (Program.m_bEnableLatiPayGiftCard)
				btnLatipayGiftCard.Visible = true;

			m_bCashOutReminder = Program.MyBooleanParse(GetSiteSettings("CashOut_reminder"));
			m_sRoundingNum = GetSiteSettings("round_price_no_cent");
			m_bSpecialItemDis = Program.MyBooleanParse(GetSiteSettings("Allow_special_item_discount"));
			m_sButton11 = GetSiteSettings("button_id_11");
			m_sButton12 = GetSiteSettings("button_id_12");
			m_sDeleteItemControl = GetSiteSettings("delete_item_password_control");
			m_sPosRef = GetSiteSettings("pos_ref");
			m_sClearPaymentControl = GetSiteSettings("clear_payment_password_control");
			m_sVipPointRate = GetSiteSettings("QPOS_MEMBERSHIP_POINT_RATE");
			m_sDiscountSpecialItem = GetSiteSettings("discount_special_item");
			m_sSoundDonePath = AppDomain.CurrentDomain.BaseDirectory + "done.wav";

			call = new Modem.SetCallback(this.TakeControl);
			if (Program.m_sSerialDevicePort != "")
				m_mod = new Modem(new SerialPort(), Program.m_sSerialDevicePort, this.call);

			if (!CheckPromotionServiceItem(m_sPromotionServiceItemCode))
				return;

			this.numericpad.Size = new Size(1026, 310);
			this.numericpad.Location = new Point(1, 440);
			this.numericpad.BringToFront();

			FormLogin fm = new FormLogin();
			fm.ShowDialog();
			if (!fm.m_bLoggedin)
			{
				this.Hide();
				int WM_CLOSE = 0x0010;
				Message msg = Message.Create(this.Handle, WM_CLOSE, new IntPtr(0), new IntPtr(0));
				PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
				return;
			}

			//SetForegroundWindow(this.Handle);// Force on groud
			//this.barcode.Focus();
			Program.m_bLanguage_en = fm.m_bLanguage_en;
			m_nSalesId = Program.MyIntParse(fm.m_sSalesId);
			lblSales.Text = fm.m_sSalesName;
			SalesName.Text = fm.m_sSalesName;
			m_nBranchId = Program.MyIntParse(fm.m_sBranchId);

			timer1.Start();

#if !DEBUG
			if (m_bTrial)
			{
				FormMSG msg = new FormMSG();
				msg.btnNo.Visible = false;
				msg.btnYes.Visible = false;
				msg.m_sMsg = " Trial Version, " + Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", "")) + " Days Left !!";
				msg.ShowDialog();
				//timer1.Enabled = false;
			}
#else
			//btncompanyok AP = new btncompanyok();
			//AP.ShowDialog();
#endif

			if (Program.MyIntParse(Program.m_sStationID) == 0)
			{
				MessageBox.Show("Please fill in Till Number", "Config Error -- EZNZ QPOS 2008");
				FormConfig fmc = new FormConfig();
				fmc.ShowDialog();
			}
			Program.CheckTillStart();
			this.Location = new Point(0, 0);

			CachePromotion();
			GetPromotion();

			Station.Text = Program.m_sStationID;
			UpdateTotalPriceDiscplay();

			sdcForeOrg = buttonCart1.ForeColor; //store original color
			sdcBackOrg = buttonCart1.BackColor;
			buttonCart1.ForeColor = System.Drawing.Color.Red; //begin with cart 1
			BuildDtCart();

			//check scale, open it here for faster weight read
			if (Program.m_sScaleName != "")
			{
				if (axOPOSScale.Open(Program.m_sScaleName) == 0)
				{
					//MessageBox.Show("Open scale ok");
					axOPOSScale.ClaimDevice(Program.m_nTimeout);
					if (axOPOSScale.Claimed)
					{
						//MessageBox.Show("Scaled calimed");
						//axOPOSScale.StatusNotify = 1;
						axOPOSScale.DeviceEnabled = true;

					}
					else
					{
						// MessageBox.Show("claim scale failed");
					}
				}
				else
				{
					MessageBox.Show("Open Scale failed, name=" + Program.m_sScaleName);
				}
			}
			if(Program.m_bCasScale && Program.m_sCasScalePort != "")
			{
//				m_casScale = new FormCasScale();
			}
			/*
			if (Program.m_sScannerName != "")
			{
				if (axOPOSScanner.Open(Program.m_sScannerName) == 0)
				{
					axOPOSScanner.ClaimDevice(Program.m_nTimeout);
					if (axOPOSScanner.Claimed)
					{
						axOPOSScanner.DeviceEnabled = true;
						axOPOSScanner.DataEventEnabled = true;
						axOPOSScanner.DecodeData = true;
//						axOPOSScanner.AutoDisable = false;
					}
					else
					{
//						MessageBox.Show("Claim Scanner failed");
					}
				}
				else
				{
					MessageBox.Show("Open Scanner failed, name=" + Program.m_sScannerName);
				}
			}
*/
			if (Program.m_sEftposLogDay.Trim() != "")
				DoClearUpEftplog(Station.Text);
			RepositionPanels();
			BuildMenuButtons();
			loadScanner();
			if (Program.m_bEnableFloating)
				CheckFloat();
			barcode.Focus();
			if(Program.m_bEnableSelfService)
				SelfServiceModelMain();
		}
		public void TakeControl(Modem modcall) //modem call back
		{
		}
		private void SelfServiceModelMain()
		{
			//barcode
			barcode.Width = 615;
			qty.Visible = false;
			price.Visible = false;
			panelHold.Visible = false;

			//cart
			//this.cart.Location = new Point(5, 49);
			//this.cart.Size = new Size(670, 620);
			//panelMenuLeft.Visible = false;
			panelMenuDepartment.Visible = false;

			//total
			label21.Location = new Point(8, 315);
			lblSales.Location = new Point(109, 315);
			//int t_height = 690;
			int t_height = 415;
			label42.Location = new Point(5, t_height);
			lblQtys.Location = new Point(109, t_height);
			label20.Location = new Point(194, t_height);
			showItems.Location = new Point(288, t_height);
			label18.Location = new Point(385, t_height);
			TotalPrice.Location = new Point(472, t_height);

			//member
			label3.Location = new Point(682, 410);
			MemberShipID.Location = new Point(743, 410);
			MemberShipID.Width = 265;
			MemberShipID.Click -= new System.EventHandler(this.MemberShipID_Click);
			MemberShipName.Visible = false;
			this.pnlNumPad.Size = new Size(334, 268);
			this.pnlNumPad.Location = new Point(680, 460);
			btnBack.Location = new Point(252, 138);
			cancelPayment.Visible = false;
			btnClock.Visible = false;

			//admin
			panelMenuTopRight.Visible = false;
			btnQty.Visible = false;
			buttonDiscount.Visible = false;
			brefund.Visible = false;
			btnNote.Visible = false;
			buttonAdmin.Visible = false;
			button20.Visible = false;
			printReciptD.Visible = false;

			panelMenuTopTopRight.Location = new Point(680, 9);
			panelMenuTopTopRight.Size = new Size(340, 110);
			btnPay.Size = new Size(330, 100);

			//del button
			btnDeleteItemForSelfService.Visible = true;
			btnDeleteItemForSelfService.Location = new Point(930, 465);
		}
		private void SelfServiceModelCheckout()
		{
			//checkout
			//panelCheckout.Location
			//btn_cash.Visible = false;
			//Cash.Visible = false;
			//btn_cashout.Visible = false;
			//Cashout.Visible = false;
			//btn_cc.Visible = false;
			//Credit.Visible = false;
			//btn_charge.Visible = false;
			//charge.Visible = false;
			//btn_eftpos.Location = new Point(3, 3);
			//Eftpos.Location = new Point(147, 5);
			//tableLayoutPanel1.Controls.Clear();
			//tableLayoutPanel1.Controls.Add(this.btn_eftpos, 0, 0);
			//tableLayoutPanel1.Controls.Add(this.Eftpos, 1, 0);
			//this.tableLayoutPanel1.Controls.Add(this.btn_cheque, 0, 1);
			//this.tableLayoutPanel1.Controls.Add(this.Cheque, 1, 1);
			//btn_cheque.Location = new Point(3, 75);
			//Cheque.Location = new Point(185, 62);
			//btn_cheque.Enabled = false;
			//Cheque.Enabled = false;
			tableLayoutPanel1.Visible = false;
			panelCheckout.Controls.Add(this.btn_eftpos);
			panelCheckout.Controls.Add(this.Eftpos);
			this.btn_eftpos.Location = new Point(10, 95);
			this.Eftpos.Location = new Point(156, 95);
			labelBalance.Location = new Point(10, 7);
			labelBalance.Size = new Size(329, 79);

			panel1.Visible = false;
			panelCheckout.Controls.Add(this.labelChange);
			panelCheckout.Controls.Add(this.Change);
			panelCheckout.Controls.Add(this.txtcashouttitle);
			panelCheckout.Controls.Add(this.txtShowCashOut);
			int h = 120;
			int w = 10;
			this.labelChange.Location = new Point(w, 439 - h);
			this.Change.Location = new Point(w, 482 - h);
			this.txtcashouttitle.Location = new Point(w, 555 - h);
			this.txtShowCashOut.Location = new Point(w, 608 - h);

			btn_5.Visible = false;
			btn_10.Visible = false;
			btn_20.Visible = false;
			btn_50.Visible = false;
			btn_100.Visible = false;
			btn_200.Visible = false;
			panel3.Visible = false;
			//btn_p0.Visible = false;
			//btn_p1.Visible = false;
			//btn_p2.Visible = false;
			//btn_p3.Visible = false;
			//btn_p4.Visible = false;
			//btn_p5.Visible = false;
			//btn_p6.Visible = false;
			//btn_p7.Visible = false;
			//button14.Visible = false;
			//btn_p9.Visible = false;
			//btn_p00.Visible = false;
			//btn_pdoc.Visible = false;
			//btn_pback.Visible = false;
			//btnClose.Visible = false;
			//btnCheckoutClose.Visible = false;
			//btn_penter.Visible = false;
			panelCheckout.Controls.Add(this.btnCheckoutClose);
			panelCheckout.Controls.Add(this.btn_penter);

			btnCheckoutClose.Location = new Point(10, 650);
			btnCheckoutClose.Width = 330;
			btnCheckoutClose.BackgroundImage = QPOS2008.Properties.Resources.keybg1red;
			
			btn_penter.Location = new Point(10, 570);
			btn_penter.Width = 330;
			btn_penter.BackgroundImage = QPOS2008.Properties.Resources.keybg1blue;

			btnSurcharge.Visible = false;
			cbExport.Visible = false;
		}
		private void BuildDtCart()
		{
			for (int i = 0; i < 4; i++)
			{
				DataTable dtCart = new DataTable();
				dtCart.Columns.Add(new DataColumn("barcode", typeof(String)));
				dtCart.Columns.Add(new DataColumn("code", typeof(String)));
				dtCart.Columns.Add(new DataColumn("supplier_code", typeof(String)));
				dtCart.Columns.Add(new DataColumn("name", typeof(String)));
				dtCart.Columns.Add(new DataColumn("name_en", typeof(String)));
				dtCart.Columns.Add(new DataColumn("name_cn", typeof(String)));
				dtCart.Columns.Add(new DataColumn("price", typeof(String)));
				dtCart.Columns.Add(new DataColumn("discount", typeof(String)));
				dtCart.Columns.Add(new DataColumn("avoid_point", typeof(String)));
				dtCart.Columns.Add(new DataColumn("qty", typeof(String)));
				dtCart.Columns.Add(new DataColumn("total", typeof(String)));
				dtCart.Columns.Add(new DataColumn("subTotal", typeof(String)));
				dtCart.Columns.Add(new DataColumn("is_promotion", typeof(String)));
				dtCart.Columns.Add(new DataColumn("cardId", typeof(String)));
				dtCart.Columns.Add(new DataColumn("cardName", typeof(String)));
				dtCart.Columns.Add(new DataColumn("cardBarcode", typeof(String)));
				dtCart.Columns.Add(new DataColumn("is_package", typeof(String)));
				dtCart.Columns.Add(new DataColumn("normal_price", typeof(String)));
				dtCart.Columns.Add(new DataColumn("promotion_group_id", typeof(String)));
				/********************/
				dtCart.Columns.Add(new DataColumn("has_scale", typeof(String)));
				/********************/
				dsCart.Tables.Add(dtCart);
			}
		}
        private bool PromotionActive(string promo_id, string weekday )
        {
            if (dst.Tables["PromotionActive"] != null)
                dst.Tables["PromotionActive"].Clear();

            int rows = 0;
            string sc = "SELECT * from promotion_list ";
            sc += " WHERE 1=1 ";
            sc += " AND promo_id = '"+promo_id+"'";
            sc += " AND promo_day" + weekday + " = 1";
            sc += " AND promo_start_date <= getdate()";
            sc += " AND promo_end_date >= getdate() ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                rows = myAdapter.Fill(dst, "PromotionActive");
                if (rows == 1)
                    return true;
            }
            catch (Exception e1)
            {
                Program.ShowExp(sc, e1);
                myConnection.Close();
                return false;
            }
            return false;
        }

        private bool BarcodeInBarcodeTable(string barcode) //test if this barcode is in barcode table
        {
            string sc = "";
            int rows = 0;
            if (dst.Tables["BarcodeInBarcodeTable"] != null)
                dst.Tables["BarcodeInBarcodeTable"].Clear();
            sc = " SELECT c.barcode from code_relations c join barcode b on c.barcode = b.barcode where 1=1 and c.barcode = '"+barcode+"'";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                rows = myAdapter.Fill(dst, "BarcodeInBarcodeTable");
            }
            catch(Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return false;
            }
            if (rows > 0)
                return true;
            else
                return false;
        }
        
        private bool isInt(string str)
        {
			try
			{
				int.Parse(str);
				return true;
			}
			catch
			{
				return false;
			}
			return true;
        }
        

		private void DoScanBarcode(bool bPressButton)
		{
			int a = 0;
			if (panelCheckout.Visible)
				return;
			bool bManuallyEnteredPrice = false; //for dump sale, department sale etc, no discount and vip level price
			if (m_sNewPriceCodeFlag != "0")
			{
				if (this.price.Text != "")
				{
					if (!Program.m_bEnableUpdatePrice)
					{
						m_sNewPriceCodeFlag = "0";
					}
					else
					{
						if (MessageBox.Show("The original price of item is 0, would you like to save the $" + this.price.Text + " Price in system???", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
							m_sNewPriceCodeFlag = "0";
						else
						{
							if (!doAddNewPriceToItem())
								MessageBox.Show("Price save failed!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}
				}
				this.price.BackColor = System.Drawing.Color.White;
			}
			string sBarcode = this.barcode.Text;

			if (sBarcode == ".") //delete last item
			{
				if (!AdminOK("delete item"))
					return;
				DeleteCartItem(0);
				barcode.Text = "";
				barcode.Focus();
				return;
			}
			if (sBarcode != "" && sBarcode.IndexOf("*").ToString() != "-1")
			{
				string sNewBarcode = sBarcode;
				double ddPrice = 0;
				ddPrice = GetBarcodeWithPrice(sBarcode, "*", ref sNewBarcode);
				sBarcode = sNewBarcode;
				sBarcode = (string.Compare(sBarcode, sNewBarcode) == 0) ? sBarcode : sNewBarcode;
				this.price.Text = ddPrice.ToString("N2");
				bManuallyEnteredPrice = true;
			}
			/*********************** Read Barcode Price End **************/
			if (sBarcode == "" && m_sCode == "")
			{
				return;
			}
			if(Program.m_bVoucherUseWebService)
			{
				if (sBarcode.Length > 8 && sBarcode.Substring(0, 8) == "96232176")
				{
					DoUseVoucherService(sBarcode);
					return;
				}
			}
			double dQty = Program.MyDoubleParse(this.qty.Text);
			if (dQty == 0)
				dQty = 1;
			double dPrice = Program.MyMoneyParse(this.price.Text);

			if (dst.Tables["additem"] != null)
				dst.Tables["additem"].Clear();
			int nWeekDay = (int)DateTime.Now.DayOfWeek;
			if (nWeekDay == 0)
				nWeekDay = 7; //tee's design, Sunday is 7
				
			string sc = " SELECT ";
			if (bPressButton)
				sc += " top 1 ";
            sc += " c.supplier_code, c.code, c.barcode AS barcodeOne, c.name, c.name_cn, c.price1, isnull(c.is_barcodeprice,0) as is_barcodeprice, isnull(c.is_id_check,0) as is_id_check ";
            sc += ", ISNULL(c.special_price,0) AS special_price1, isnull(c.is_special,0) as is_special, ISNULL(c.special_price_end_date, '') AS special_price_end_date ";
            sc += ", ISNULL(c.special_price_start_date, '') AS special_price_start_date, c.cat ";
			sc += ", b.item_qty, ISNULL(b.package_price, 0) AS package_price, b.barcode";
			sc += ", p.promo_type, p.special_price, p.discount_percentage,p.limit, ISNULL(c.has_scale, 0) AS has_scale ";
            sc += ", isnull(p.promo_day1,0) as promo_day1, isnull(p.promo_day2,0) as promo_day2, isnull(p.promo_day3,0) as promo_day3, isnull(p.promo_day4,0) as promo_day4, isnull(p.promo_day5,0) as promo_day5, isnull(p.promo_day6,0) as promo_day6, isnull(p.promo_day7,0) as promo_day7 ";
			sc += ", p.free_item_required_item_code, ISNULL(p.promo_member_only, 'false') AS promo_member_only , ISNULL(c.price4, '0') AS price4 ";
			sc += ", sp.*, ISNULL(c.is_member_only , '0') AS member_only , ISNULL(c.date_range, '0') AS date_range, ISNULL(c.pick_date,'0') AS pick_date ";
			sc += ", ISNULL(c.avoid_point, '0') AS avoid_point";
            sc += ", p.promo_start_date";
            sc += ", p.promo_end_date ";
            sc += ", pr.promo_id ";
			sc += " FROM code_relations c ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " LEFT OUTER JOIN specials sp ON sp.code = c.code ";
            sc += " LEFT OUTER JOIN promo pr on pr.code = c.code ";
            sc += " LEFT OUTER JOIN promotion_list p ON p.promo_id = pr.promo_id AND p.promo_active = 1 ";
            sc += " AND p.promo_start_date <= GETDATE() AND p.promo_end_date >= GETDATE() ";
			//*** ARGUMENT FOR PICK THE THE RIGHT DAY FOR QTY PROMOTION CH 13/11/08***//
			if (CheckQtyPromotionValue("", nWeekDay, sBarcode))
			{
				sc += " AND p.promo_day" + nWeekDay.ToString() + " = 1 "; //today is in the DayOfWeek list
			}
			sc += " WHERE 1 = 1 ";
			
			string sc1 = "";
			string sFilter = "";
			if (sBarcode != "")
			{
				if (bPressButton && isInt(sBarcode) && (sBarcode.Substring(sBarcode.Length - 1, 1).IndexOf('B') == -1 && sBarcode.Substring(sBarcode.Length - 1, 1).IndexOf('b') == -1))
					sFilter = " AND c.code = '" + Program.EncodeQuote(sBarcode) + "' ";
				else
				{	
					sFilter = " AND (b.barcode = '" + Program.EncodeQuote(sBarcode) + "' "; //" AND c.barcode = '" + Program.EncodeQuote(sBarcode) + "' ";
					if (!BarcodeInBarcodeTable(sBarcode))
						sFilter += " OR c.barcode = '" + Program.EncodeQuote(sBarcode) + "' ";
					if(Program.m_bScanInSupplierCode)	
						sFilter += " OR c.supplier_code = '" + Program.EncodeQuote(sBarcode) + "'";
					if (Program.IsInt(sBarcode) && Program.m_bScanInCode)
						sFilter += " OR c.code = '" + Program.EncodeQuote(sBarcode) + "' ";
					sFilter += " ) "; 
				}
			}
			int nRows = 0;
			sc1 = sc + sFilter;
			sc1 += " ORDER BY c.code DESC ";
			try
			{
				myAdapter = new SqlDataAdapter(sc1, myConnection);
				nRows = myAdapter.Fill(dst, "additem");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc1, e1);
				myConnection.Close();
				return;
			}
            //if(nRows == 0)
            //{
            //    sFilter = " AND b.barcode = '" + Program.EncodeQuote(sBarcode) + "' ";
            //    sc1 = sc + sFilter;
            //    sc1 += " ORDER BY c.code DESC ";
            //    try
            //    {
            //        myAdapter = new SqlDataAdapter(sc1, myConnection);
            //        nRows = myAdapter.Fill(dst, "additem");
            //    }
            //    catch (Exception e1)
            //    {
            //        Program.ShowExp(sc1, e1);
            //        myConnection.Close();
            //        return;
            //    }
            //}
            //if (nRows == 0 && Program.IsInt(sBarcode))
            //{
            //    sFilter = " AND c.code = '" + Program.EncodeQuote(sBarcode) + "' ";
            //    sc1 = sc + sFilter;
            //    sc1 += " ORDER BY c.code DESC ";
            //    try
            //    {
            //        myAdapter = new SqlDataAdapter(sc1, myConnection);
            //        nRows = myAdapter.Fill(dst, "additem");
            //    }
            //    catch (Exception e1)
            //    {
            //        Program.ShowExp(sc1, e1);
            //        myConnection.Close();
            //        return;
            //    }
            //}
            if (nRows > 1)
            {
                search s = new search();
                s.m_kw = sBarcode;
                s.ShowDialog();
                if (s.bIs_close)
                {
                    this.barcode.Text = "";
                    return;
                }
                string barcode = s.m_sBarcode;
                string code = s.m_sCode;
                if (dst.Tables["additem"] != null)
                    dst.Tables["additem"].Clear();
                sc = " SELECT ";
                sc += " top 1 ";
                sc += " c.supplier_code, c.code, c.barcode AS barcodeOne, c.name, c.name_cn, c.price1, isnull(c.is_barcodeprice,0) as is_barcodeprice, isnull(c.is_id_check,0) as is_id_check ";
                sc += ", ISNULL(c.special_price,0) AS special_price1, isnull(c.is_special,0) as is_special, ISNULL(c.special_price_end_date, '') AS special_price_end_date ";
                sc += ", ISNULL(c.special_price_start_date, '') AS special_price_start_date, c.cat ";
                sc += ", b.item_qty, ISNULL(b.package_price, 0) AS package_price, b.barcode";
                sc += ", p.promo_type, p.special_price, p.discount_percentage,p.limit, ISNULL(c.has_scale, 0) AS has_scale ";
                sc += ", isnull(p.promo_day1,0) as promo_day1,  isnull(p.promo_day2,0) as promo_day2,isnull(p.promo_day3,0) as promo_day3,isnull(p.promo_day4,0) as promo_day4,isnull(p.promo_day5,0) as promo_day5,isnull(p.promo_day6,0) as promo_day6,isnull(p.promo_day7,0) as promo_day7 ";
                sc += ", p.free_item_required_item_code, ISNULL(p.promo_member_only, 'false') AS promo_member_only , ISNULL(c.price4, '0') AS price4 ";
                sc += ", sp.*, ISNULL(c.is_member_only , '0') AS member_only , ISNULL(c.date_range, '0') AS date_range, ISNULL(c.pick_date,'0') AS pick_date ";
                sc += ", ISNULL(c.avoid_point, '0') AS avoid_point";
                sc += ", p.promo_start_date";
                sc += ", p.promo_end_date ";
                sc += ", pr.promo_id ";
                sc += " FROM code_relations c ";
                sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
                sc += " LEFT OUTER JOIN specials sp ON sp.code = c.code ";
                sc += " LEFT OUTER JOIN promo pr on pr.code = c.code ";
                sc += " LEFT OUTER JOIN promotion_list p ON p.promo_id = pr.promo_id AND p.promo_active = 1 ";
                sc += " AND p.promo_start_date <= GETDATE() AND p.promo_end_date >= GETDATE() ";
                if (CheckQtyPromotionValue("", nWeekDay, sBarcode))
                {
                    sc += " AND p.promo_day" + nWeekDay.ToString() + " = 1 "; //today is in the DayOfWeek list
                }
                sc += " WHERE 1 = 1 ";
                sc += " AND b.barcode = '" + barcode + "'";
                sc += " AND c.code = '"+code+"'";
                try
                {
                    myAdapter = new SqlDataAdapter(sc, myConnection);
                    nRows = myAdapter.Fill(dst, "additem");
                }
                catch (Exception e1)
                {
                    Program.ShowExp(sc, e1);
                    myConnection.Close();
                    return;
                }
            }
			if (nRows == 1)
			{
				#region barcode_found
				DataRow dr = dst.Tables["additem"].Rows[0];
				string code = dr["code"].ToString();
                string cat = dr["cat"].ToString().ToLower();
				string barcodeOne = dr["barcodeOne"].ToString();
				string sGetSupplier_code = dr["supplier_code"].ToString();

				bool bReadWeight = bool.Parse(dr["has_scale"].ToString());
				bool bIsSpecial = bool.Parse(dr["is_special"].ToString());
				bool bMemberPrice = bool.Parse(dr["promo_member_only"].ToString());
				bool bMemberOnly = bool.Parse(dr["member_only"].ToString());
				bool bMSDateRange = bool.Parse(dr["date_range"].ToString());
				bool bMSPickDate = bool.Parse(dr["pick_date"].ToString());
				bool bAvoidPoint = bool.Parse(dr["avoid_point"].ToString());
                string promo_id = dr["promo_id"].ToString();

                if (Program.m_sNoVipDiscountCatalog.IndexOf(cat) >= 0 && cat != "" && cat != null)
                    bAvoidPoint = true;
                else
                    bAvoidPoint = false;

				bool bIsBarcodePrice = bool.Parse(dr["is_barcodeprice"].ToString());
				bool bIsIDCheck = bool.Parse(dr["is_id_check"].ToString());

				//bool bSpecialNoEndDate = true;
				string sItemBarcode = dr["barcode"].ToString();

				int nPromotionType = Program.MyIntParse(dr["promo_type"].ToString());
				int nLimit = Program.MyIntParse(dr["limit"].ToString());

				double dGetPrice = Program.MyDoubleParse(dr["price1"].ToString());
				double sPrice4 = Program.MyDoubleParse(dr["price4"].ToString());
				double dSpecialPrice = Program.MyDoubleParse(dr["special_price1"].ToString());
				double dGetPackPrice = Program.MyDoubleParse(dr["package_price"].ToString());
				double dQtyBarcode = Program.MyDoubleParse(dr["item_qty"].ToString());
                Dictionary<string, bool> promo_day = new Dictionary<string,bool>();
                promo_day.Clear();
                promo_day.Add("monday", bool.Parse(dr["promo_day1"].ToString()));
                promo_day.Add("tuesday", bool.Parse(dr["promo_day2"].ToString()));
                promo_day.Add("wednesday", bool.Parse(dr["promo_day3"].ToString()));
                promo_day.Add("thursday", bool.Parse(dr["promo_day4"].ToString()));
                promo_day.Add("friday", bool.Parse(dr["promo_day5"].ToString()));
                promo_day.Add("saturday", bool.Parse(dr["promo_day6"].ToString()));
                promo_day.Add("sunday", bool.Parse(dr["promo_day7"].ToString()));
                
//				if (barcodeOne == sBarcode)
//					dQtyBarcode = 1;
                DateTime special_price_start_date = DateTime.Parse(dr["special_price_start_date"].ToString());	
				DateTime special_price_end_date = DateTime.Parse(dr["special_price_end_date"].ToString());
				DateTime today = DateTime.Now;
                var dayofweek = today.DayOfWeek.ToString().ToLower();
                bool bDayActive = false;
                foreach (var p in promo_day)
                {
                    if (p.Key == dayofweek && p.Value)
                        bDayActive = true;
                }
				if (bIsIDCheck && !m_bIdCheckDone)
				{
					FormIdCheck fmMSG = new FormIdCheck();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08
					string p = " Please check ID ?";
					fmMSG.m_sMSG = p;
					fmMSG.ShowDialog();
					if (fmMSG.m_sConfirm == "1")
					{
						if(Program.m_bIdCheckPasswordControl)
						{
							if (!AdminOK("id_check"))
							{
								this.barcode.Text = "";
								this.price.Text = "";
								return;
							}
						}
						m_bIdCheckDone = true;
					}
					else
					{
						this.barcode.Text = "";
						this.price.Text = "";
						return;
					}
				}
				if (!bIsBarcodePrice)
				{
					if (bMemberPrice)
						m_bMemberPrice = true;
					sBarcode = "";
					m_sCode = "";
					if (dr["code"].ToString() == m_sVoucherItem)
					{
						m_sVoucherMsg = DoUseVoucher(this.barcode.Text);
						try
						{
							double vAmount = double.Parse(m_sVoucherMsg);
						}
						catch (Exception e)
						{
							string s = e.ToString();
							FormMSG errormsg = new FormMSG();
							errormsg.btnYes.Visible = false;
							errormsg.btnNo.Visible = false;
							errormsg.m_sMsg = m_sVoucherMsg;
							errormsg.Show();
							barcode.Text = "";
							qty.Text = "";
							price.Text = "";
							return;
						}
						dPrice = Program.MyMoneyParse(DoUseVoucher(this.barcode.Text));
					}
					if (Program.m_sgroceryitem.IndexOf(this.barcode.Text).ToString() != "-1" || dGetPrice == 0)
					{
						string Str = this.price.Text.Trim();
						double Num;
						bool isNum = double.TryParse(Str, out Num);

						if (TSIsNumberic(this.price.Text))
							dPrice = Program.MyDoubleParse(this.price.Text) / 100;
						else if (isNum)
							dPrice = Program.MyDoubleParse(this.price.Text);// / 100;
					}
					if (bReadWeight)
					{
						if (!DoWeight())
							return;
						dQty = Program.MyDoubleParse(this.qty.Text);
						for (int t = 0; t < 2; t++)
						{
							if (dQty == 0)
							{
								Thread.Sleep(100);
								DoWeight();
							}
							else
							{
								dQty = Program.MyDoubleParse(this.qty.Text);
								continue;
							}
						}
						if (Program.m_sgroceryweight.IndexOf(this.barcode.Text).ToString() != "-1" || dGetPrice == 0)
						{
							string Str = this.price.Text.Trim();
							double Num;
							bool isNum = double.TryParse(Str, out Num);

							if (TSIsNumberic(this.price.Text))
								dPrice = Program.MyDoubleParse(this.price.Text) / 100;
							else if (isNum)
								dPrice = Program.MyDoubleParse(this.price.Text);// / 100;
						}
						if (dQty == 0)
						{
							if (Program.m_sgroceryweight.IndexOf(this.barcode.Text).ToString() != "-1")
							{
								if (this.qty.Text != "")
								{
									dQty = Program.MyDoubleParse(this.qty.Text);
								}
								else
								{
									MessageBox.Show("Sorry, Item need to weight.", "Weight Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
									this.price.Text = "";
									this.barcode.Focus();
									this.barcode.Select();
									return;
								}
							}
							else
							{
								MessageBox.Show("Sorry, Item need to weight.", "Weight Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								this.barcode.Focus();
								this.barcode.Select();
								return;
							}
						}
					}

					if (dPrice != 0)
						dGetPrice = dPrice;

					if (AddPackage(sItemBarcode, dQtyBarcode) && dPrice == 0)
						dPrice = dGetPackPrice;

					if (dQtyBarcode > 0)
						dQty *= dQtyBarcode;

					int nIndex = -1;
                    double dQtyInCart = GetQtyInCart(sItemBarcode, code, ref nIndex, dGetPrice, bIsSpecial);
					string sRFreeItemCode = dr["free_item_required_item_code"].ToString();

					if (doWarningItem(dGetPrice, nPromotionType))
					{
						if (dGetPrice == 0)
						{
							//MessageBox.Show("Error Item Price Is 0 , Please Check The Price Again", "Price Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							m_sNewPriceCodeFlag = code;
							this.price.BackColor = System.Drawing.Color.Red;
							this.price.Focus();
							return;
						}
						else if (dGetPrice >= 1000)
						{
							if (MessageBox.Show("Beware The Price " + dGetPrice.ToString("c") + " is too high,  would you like to continue ", "Price Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
							{
								this.barcode.Text = "";
								this.qty.Text = "";
								this.price.Text = "";
								this.barcode.Focus();
								return;
							}
						}
					}
					if (nPromotionType == 0 && !bReadWeight)
					{
						if (CheckExistingPromotItem(sGetSupplier_code))
						{
							//MessageBox.Show("b");
							nPromotionType = 5;
							sRFreeItemCode = code;
						}
						else
						{
							if (dQtyInCart > 0 && dQtyBarcode <= 1 && !bIsSpecial) //this item already in cart, increase qty
							{
								if (code == m_sVoucherItem)
								{
									Program.MsgBox("This voucher already been used");
									return;
								}
								m_irow = nIndex.ToString();
								int iSelectedRowed = nIndex;
								DeleteCartItem(iSelectedRowed);
								AddToCart(sItemBarcode, code, dr["name"].ToString(), dr["name_cn"].ToString(), dGetPrice, (dQtyInCart), "0", false, sGetSupplier_code, 0, false, false);
								doAddQty(dQty, "0");
								qty.Text = "";
								barcode.Focus();
								timer3.Interval = 500;
								timer3.Start();
								if (this.m_AddonQty == "1")
	//							if (Program.m_sgroceryitem.IndexOf(this.barcode.Text).ToString() != "-1" || dGetPrice == 0)
								{
									dQtyInCart = Program.MyDoubleParse(this.txtQty.Text);
									dQty += dQtyInCart;
									double dPriceCurrent = Program.MyDoubleParse(cart.Rows[nIndex].Cells["cc_price"].Value.ToString());
									double dSub = Math.Round(dPriceCurrent * dQty, 2);
									cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
									cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
									m_fmad.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
									m_fmad.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
									m_fmad.cart.Rows[nIndex].Cells["cc_discount"].Value = "";
									m_fmads.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
									m_fmads.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
									m_fmads.cart.Rows[nIndex].Cells["cc_discount"].Value = "";
									cart.Rows[nIndex].Cells["has_scale"].Value = bReadWeight.ToString();

									//show current item on duplicate screen //
									string s_dPrice = cart.Rows[nIndex].Cells["cc_price"].Value.ToString();
									string s_dQty = cart.Rows[nIndex].Cells["cc_qty"].Value.ToString();
									string s_dItemName = cart.Rows[nIndex].Cells["cc_name"].Value.ToString();
									int i_dItemName = s_dItemName.Length;
									if (i_dItemName >= 45)
										i_dItemName = 45;
									//MessageBox.Show(i_dItemName1.ToString());
									if (i_dItemName > 30)
										s_dItemName = s_dItemName.Substring(0, 30) + "\r\n" + s_dItemName.Substring(31) + " ...";
									m_fmad.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
									m_fmad.citem2.Text = s_dItemName;

									m_fmads.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
									m_fmads.citem2.Text = s_dItemName;

									CalcCartTotal();
									UpdateDtCart(m_nCurrentCart);
									CheckQtyPromotion(code, dQty, nIndex, barcode.Text).ToString(); //promotion type 3,4,5
									CheckGroupPromotion();
                                    CheckComboPromotion();
									barcode.Text = "";
									qty.Text = "";
									price.Text = "";
									barcode.Focus();
									this.pnlAddOnQty.Visible = false;
									return; //no more action
								}
								else
								{
									m_irow = "";
									this.barcode.Text = "";
									this.price.Text = "";
									this.pnlAddOnQty.Visible = false;
									return;
								}
							}
						}
					}
					else
					{
                        if (dQtyInCart > 0 && dQtyBarcode <= 1 && (dr["limit"].ToString() == "" || dr["limit"].ToString() == null || dr["limit"].ToString() == "0")) //this item already in cart, increase qty
    //                    if (dQtyInCart > 0 && dr["promo_type"].ToString() != "6" && dQtyBarcode <= 1 && (dr["limit"].ToString() == "" || dr["limit"].ToString() == null || dr["limit"].ToString() == "0")) //this item already in cart, increase qty
						{
							dQty += dQtyInCart;
							double dPriceCurrent = Program.MyDoubleParse(cart.Rows[nIndex].Cells["cc_price"].Value.ToString());
							double dSub = Math.Round(dPriceCurrent * dQty, 2);
							cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
                            cart.Rows[nIndex].Cells["cc_total"].Value = dSub.ToString("N2");
							m_fmad.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
							m_fmad.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
							m_fmads.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
							m_fmads.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
							string s_dPrice = cart.Rows[nIndex].Cells["cc_price"].Value.ToString();
							string s_dQty = cart.Rows[nIndex].Cells["cc_qty"].Value.ToString();
							string s_dItemName = cart.Rows[nIndex].Cells["cc_name"].Value.ToString();
							int i_dItemName = s_dItemName.Length;
							if (i_dItemName >= 45)
								i_dItemName = 45;
							//MessageBox.Show(i_dItemName1.ToString());
							if (i_dItemName > 30)
								s_dItemName = s_dItemName.Substring(0, 30) + "\r\n" + s_dItemName.Substring(31) + " ...";
							m_fmad.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
							m_fmad.citem2.Text = s_dItemName;
							m_fmads.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
							m_fmads.citem2.Text = s_dItemName;
							CalcCartTotal();
							UpdateDtCart(m_nCurrentCart);
							CheckQtyPromotion(code, dQty, nIndex, barcode.Text).ToString(); //promotion type 3,4,5
							CheckGroupPromotion();
                            CheckComboPromotion();
							UpdateDtCart(m_nCurrentCart);
							barcode.Text = "";
							qty.Text = "";
							price.Text = "";
							barcode.Focus();
							m_irow = nIndex.ToString();
							timer3.Interval = 500;
							timer3.Start();
							return; //no more action
						}
					}
                    m_irow = "";
					if (dPrice == 0)  // avoid do weight qty ch 04-12-08
						dPrice = dGetPrice;
					if (dGetPackPrice.ToString() != "" && dGetPackPrice.ToString() != null && dGetPackPrice.ToString() != "0" && dQtyBarcode > 1)
					{
						dPrice = dGetPackPrice;
					}	
					else if (bIsSpecial)
					{
						if (dGetPackPrice.ToString() != "" && dGetPackPrice.ToString() != null && dGetPackPrice.ToString() != "0")
						{
							dPrice = dGetPackPrice;
						}
						else
						{
							if (bCheckMemberSpecial(sGetSupplier_code, bMSDateRange, bMSPickDate, nWeekDay))
							{
								/*****************************************
								 * Member SpecialItemNormalPrice *
								 ************************************/
								if (bMemberOnly)
								{
									if (ScanMembership())
									{
										dPrice = dSpecialPrice;
										nPromotionType = -1;
									}
									else
									{
										dPrice = dGetPrice;
									}

								}
								else
								{
									if (System.DateTime.Compare(today, special_price_end_date) < 0 && System.DateTime.Compare(today, special_price_start_date) >= 0)
									{
										dPrice = dSpecialPrice;
										nPromotionType = -1;
									}
									else
									{
										dPrice = dGetPrice;
									}
								}
							}
							else
							{
								dPrice = dGetPrice;
							}
						}

					}
                    else if (nPromotionType == 1 && bDayActive) //special price discount
					{
						double dDiscount = Program.MyDoubleParse(dr["special_price"].ToString());
						int limit = Program.MyIntParse(dr["limit"].ToString());
						int scode = Program.MyIntParse(dr["code"].ToString());
						if (m_bMemberPrice)
						{
							if (ScanMembership())
								dPrice = dDiscount;
						}
						else
						{
							if (!overLimit(scode, limit))
								dPrice = dDiscount; // FIXED THE BUG, THE DISCOUNT FOR SPECIAL ITEM CH 13/11/2008
						}
					}
					else if (nPromotionType == 2) //percentage discount
					{
						double dRate = Program.MyDoubleParse(dr["discount_percentage"].ToString()) / 100;
						if (m_bMemberPrice)
						{
							if (ScanMembership())
								dPrice = Math.Round(dGetPrice * (1 - dRate), 2);
						}
                        else if (PromotionActive(promo_id,nWeekDay.ToString()))
							dPrice = Math.Round(dGetPrice * (1 - dRate), 2);
					}

					if (sPrice4 != 0)
						dPrice = sPrice4;

					string sCCode = dr["code"].ToString();
					string packagepromotype = "0";

                    if (dQtyBarcode > 1 && !bIsBarcodePrice)
					{
						packagepromotype = nPromotionType.ToString();
						m_bPackageDue = true;
					}
					else
					{
						m_bPackageDue = false;
					}
					if (Program.MyDoubleParse(qty.Text) >= 2 && nPromotionType != 0)
						packagepromotype = nPromotionType.ToString();
					
					if(nPromotionType == -1) //on special
						packagepromotype = "1"; //no category discount

					if (!CheckQtyPromotion(sCCode, dQty, 0, barcode.Text))
					{
						if (dr["code"].ToString() == m_sVoucherItem)
							AddToCart(barcode.Text, dr["code"].ToString(), (-dPrice).ToString("c") + " VOUCHER", "", dPrice, 1, "0", false, "VOUCHER", 0, false, false);
						else
							AddToCart(barcode.Text, dr["code"].ToString(), dr["name"].ToString(), dr["name_cn"].ToString(), dPrice, dQty, packagepromotype, m_bPackageDue, dr["supplier_code"].ToString(), 0, bAvoidPoint, bReadWeight);
						if (MemberShipID.Text != "" && !bManuallyEnteredPrice)
							DoApplyLevelPriceForCart(0);
                        if (nPromotionType == 1 && dQty > nLimit && nLimit > 0)
                        {
                            doChangeQty(dQty, m_irow);
                            m_irow = "";
                        }
					}
					string s_dPrice1 = cart.Rows[0].Cells["cc_price"].Value.ToString();
					if(bIsSpecial)
						cart.Rows[0].Cells["cc_is_promotion"].Value = "1";					
					string s_dQty1 = cart.Rows[0].Cells["cc_qty"].Value.ToString();
					string s_dItemName1 = cart.Rows[0].Cells["cc_name"].Value.ToString();
					int i_dItemName1 = s_dItemName1.Length;
					if (i_dItemName1 >= 45)
						i_dItemName1 = 45;
					//MessageBox.Show(i_dItemName1.ToString());
					if (i_dItemName1 > 30)
						s_dItemName1 = s_dItemName1.Substring(0, 30) + "\r\n" + s_dItemName1.Substring(31) + " ...";
					m_fmad.citem1.Text = s_dQty1 + " @ " + Program.MyDoubleParse(s_dPrice1).ToString("c");
					m_fmad.citem2.Text = s_dItemName1;
					m_fmads.citem1.Text = s_dQty1 + " @ " + Program.MyDoubleParse(s_dPrice1).ToString("c");
					m_fmads.citem2.Text = s_dItemName1;
					this.msgboard2.Text = "";

					CalcCartTotal();
					UpdateDtCart(m_nCurrentCart);
				}
				else if (bIsBarcodePrice)
				{
					if (File.Exists(m_sSoundDonePath))
					{
						SoundPlayer error = new SoundPlayer(m_sSoundDonePath);
						error.Play();
					}
					string p = "Sorry, Product Found !!";
					p += " \r\nPlease Check The Detail !!";
					FormMSG fmMSG = new FormMSG();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08
					if (this.barcode.Text.Length >= 14)
						p = DoUseVoucher(this.barcode.Text);
					fmMSG.m_sMsg = p;
					fmMSG.btnNo.Visible = false;
					fmMSG.btnYes.Visible = false;
					fmMSG.ShowDialog();
                    
					// MessageBox.Show("Item Not Found ");
					this.barcode.Focus();
					if (this.barcode.Text.Length >= 10)
						p = "";
					this.msgboard2.Text = p;
                    return;
				}
                CheckCategoryDiscount(0, cat);
				CheckGroupPromotion();
                CheckComboPromotion();

				if (cart.Rows.Count == 1)
					Change.Text = "";
				barcode.Text = "";
				qty.Text = "";
				price.Text = "";
				#endregion barcode_found
			}
			else if (dst.Tables["additem"].Rows.Count == 0)
			{
				#region not_found
				if (dst.Tables["LabelBarcode"] != null)
					dst.Tables["LabelBarcode"].Clear();
				sc = " SELECT * FROM settings WHERE name like 'BarcodeLabel_BarcodeLength' or name like 'BarcodeLabel_BarcodeStartDigit' or name like 'BarcodeLabel_DecimalPoint' ";
				sc += " or name like 'BarcodeLabel_PriceLength' or name like 'BarcodeLabel_PriceStartDigit' order by name ";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					myAdapter.Fill(dst, "LabelBarcode");
				}
				catch (Exception e1)
				{
					myConnection.Close();
					Program.ShowExp(sc, e1);
					return;
				}
				if (dst.Tables["LabelBarcode"].Rows.Count == 5)
				{
					DataRow dr = dst.Tables["LabelBarcode"].Rows[0];
					//string BarcodeLength = dr["value"].ToString();
					int BarcodeLength = int.Parse(dr["value"].ToString());
					DataRow dr1 = dst.Tables["LabelBarcode"].Rows[1];
					int BarcodeStartDigit = int.Parse(dr1["value"].ToString());
					DataRow dr2 = dst.Tables["LabelBarcode"].Rows[2];
					int DecimalPoint = int.Parse(dr2["value"].ToString());
					DataRow dr3 = dst.Tables["LabelBarcode"].Rows[3];
					int PriceLength = int.Parse(dr3["value"].ToString());
					DataRow dr4 = dst.Tables["LabelBarcode"].Rows[4];
					int PriceStartDigit = int.Parse(dr4["value"].ToString());
					int LabelBarcodeLength = BarcodeLength + PriceLength;

					//if (sBarcode.Length == LabelBarcodeLength && Program.m_sbarcodewithprice1 == sBarcode.Substring(0, int.Parse(Program.m_sbarcodelength1)))
					if (PriceLength > 0 && BarcodeLength > 0 && sBarcode.Length >= LabelBarcodeLength)
					{
						//if (sBarcode.IndexOf("*").ToString() == "-1" && sBarcode != "" && Program.m_sbarcodewithprice1.IndexOf(sBarcode.Substring(0, int.Parse(Program.m_sbarcodelength1))).ToString() != "-1")
						if (sBarcode.IndexOf("*").ToString() == "-1" && sBarcode != "")
						{
                            if (sBarcode.Length >= (PriceLength + PriceStartDigit-1))
                            {
                                sNewPrice = GetLabelBarcodePrice(sBarcode, DecimalPoint, PriceLength, PriceStartDigit); // Get Price from function
                                sNewBarcode = GetLabelBarcodeBarcode(sBarcode, BarcodeLength, BarcodeStartDigit);
                                sBarcode = sNewBarcode;
                                if (!Program.m_bEnableCalQty)
                                    this.price.Text = sNewPrice;
                            }
                            else
                            {
                                if (GetItemFromServer(barcode.Text))
                                    return;
                                if (File.Exists(m_sSoundDonePath))
                                {
                                    SoundPlayer error = new SoundPlayer(m_sSoundDonePath);
                                    error.Play();
                                }
                                string p = "Sorry, Product Not Found !!";
                                //FormMSG fmMSG = new FormMSG();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08
                                //if (this.barcode.Text.Length >= 14)
                                //    p = DoUseVoucher(this.barcode.Text);
                                //string pNew = p + "\r\n Would you like to Create a new Item??";
                                //fmMSG.m_sYesNo = "1";
                                //fmMSG.m_sMsg = pNew;
                                //if (this.barcode.Text == m_sButton11 || this.barcode.Text == m_sButton12)
                                //{
                                //    fmMSG.m_sYesNo = "0";
                                //    fmMSG.m_sMsg = " Please Enter Price First!!";
                                //    fmMSG.btnNo.Visible = false;
                                //    fmMSG.btnYes.Visible = false;
                                //    fmMSG.ShowDialog();
                                //}
                                //if (this.barcode.Text != m_sButton11 && this.barcode.Text != m_sButton12)
                                //    fmMSG.ShowDialog();
                                //if (fmMSG.m_sCreateNewItem == "1")
                                //{
  
                                //    this.CreateNewItemPanel.Visible = true;
                                //    doBuideBrands();
                                //    doBuideCat();
                                //    doBuideTax();

                                //    this.CreateNewBarcodeTB.Text = this.barcode.Text;
                                //    fmMSG.m_sCreateNewItem = "0";

                                //}
                                //else
                                //{
                                //    this.barcode.Focus();
                                //    if (this.barcode.Text.Length >= 10)
                                //        p = "";
                                //    this.msgboard2.Text = p;
                                //}
                                
                            }
						}
						/********************copy from above*****************/
						dQty = Program.MyDoubleParse(this.qty.Text);
						if (dQty == 0)
							dQty = 1;
						dPrice = Program.MyMoneyParse(this.price.Text);

						if (dst.Tables["additem"] != null)
							dst.Tables["additem"].Clear();
						nWeekDay = (int)DateTime.Now.DayOfWeek;
						if (nWeekDay == 0)
							nWeekDay = 7; //tee's design, Sunday is 7
						sc = " SELECT top 1 c.supplier_code, c.code, c.name, c.name_cn, c.price1,  c.is_barcodeprice, c.is_id_check, ISNULL(c.special_price,0) AS special_price1, c.is_special, ISNULL(c.special_price_end_date, '') AS special_price_end_date,  b.item_qty, ISNULL(b.package_price, 0) AS package_price, b.barcode";
                        sc += ", ISNULL(c.special_price_start_date, '') AS special_price_start_date ";
						sc += ", p.promo_type, p.special_price, p.discount_percentage, ISNULL(c.has_scale, 0) AS has_scale ,p.free_item_required_item_code, ISNULL(p.promo_member_only, 'false') AS promo_member_only , ISNULL(c.price4, '0') AS price4, c.cat ";
						sc += ", sp.*, ISNULL(c.is_member_only , '0') AS member_only , ISNULL(c.date_range, '0') AS date_range, ISNULL(c.pick_date,'0') AS pick_date, ISNULL(c.avoid_point, '0') AS avoid_point";
						//			sc += ", p.volumn_discount_qty, p.volumn_discount_price_total, free_item_required_qty, free_item_required_item_code, free_item_reward_qty ";
						sc += " FROM code_relations c LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
						sc += " LEFT OUTER JOIN specials sp ON sp.code = c.code ";
						sc += " LEFT OUTER JOIN promotion_list p ON p.promo_id = c.promo_id AND p.promo_active = 1 ";
						sc += " AND p.promo_start_date <= GETDATE() AND p.promo_end_date >= GETDATE() "; //started, but not yet expired

						//*** ARGUMENT FOR PICK THE THE RIGHT DAY FOR QTY PROMOTION CH 13/11/08***//
						if (CheckQtyPromotionValue(sBarcode, nWeekDay, sBarcode))
						{
							sc += " AND p.promo_day" + nWeekDay.ToString() + " = 1 "; //today is in the DayOfWeek list
						}
						sc += " WHERE 1 = 1 ";
						if (sBarcode != "")
						{
                            sc += " AND (c.barcode = '" + Program.EncodeQuote(sBarcode) + "' OR c.supplier_code = '" + Program.EncodeQuote(sBarcode) + "' or b.barcode = '" + Program.EncodeQuote(sBarcode) + "') ";
						}
						else if (m_sCode != "")
						{
							sc += " AND  c.supplier_code ='" + m_sCode + "'";
							m_sCode = "";
						}
						try
						{
							myAdapter = new SqlDataAdapter(sc, myConnection);
							myAdapter.Fill(dst, "additem");
						}
						catch (Exception e1)
						{
							myConnection.Close();
							Program.ShowExp(sc, e1);
							return;
						}
						if (dst.Tables["additem"].Rows.Count == 0 || dst.Tables["additem"].Rows.Count > 1)
						{
							// SearchReturn();
							// return;
						}
						if (dst.Tables["additem"].Rows.Count == 1)
						{
							dr = dst.Tables["additem"].Rows[0];
							string code = dr["code"].ToString();
                            string cat = dr["cat"].ToString().ToLower();
							string sGetSupplier_code = dr["supplier_code"].ToString();

							bool bReadWeight = bool.Parse(dr["has_scale"].ToString());
							bool bIsSpecial = bool.Parse(dr["is_special"].ToString());
							bool bMemberPrice = bool.Parse(dr["promo_member_only"].ToString());
							bool bMemberOnly = bool.Parse(dr["member_only"].ToString());
							bool bMSDateRange = bool.Parse(dr["date_range"].ToString());
							bool bMSPickDate = bool.Parse(dr["pick_date"].ToString());
							bool bAvoidPoint = bool.Parse(dr["avoid_point"].ToString());

                            if (Program.m_sNoVipDiscountCatalog.IndexOf(cat) >= 0 && cat != "" && cat != null)
                                bAvoidPoint = true;
                            else
                                bAvoidPoint = false;

							bool bIsBarcodePrice = bool.Parse(dr["is_barcodeprice"].ToString());
							bool bIsIDCheck = bool.Parse(dr["is_id_check"].ToString());

							//bool bSpecialNoEndDate = true;
							string sItemBarcode = dr["barcode"].ToString();

							int nPromotionType = Program.MyIntParse(dr["promo_type"].ToString());

							double dGetPrice = Program.MyDoubleParse(dr["price1"].ToString());
							double sPrice4 = Program.MyDoubleParse(dr["price4"].ToString());
							double dSpecialPrice = Program.MyDoubleParse(dr["special_price1"].ToString());
							double dGetPackPrice = Program.MyDoubleParse(dr["package_price"].ToString());
							double dQtyBarcode = Program.MyDoubleParse(dr["item_qty"].ToString());
							// special_price_end_date = DateTime.Parse(dr["special_price_end_date"].ToString());
                            DateTime special_price_start_date = DateTime.Parse(dr["special_price_start_date"].ToString());
                            DateTime special_price_end_date = DateTime.Parse(dr["special_price_end_date"].ToString());
                            DateTime today = DateTime.Now;
							if (bIsIDCheck && !m_bIdCheckDone)
							{
								FormIdCheck fmMSG = new FormIdCheck();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08
								string p = " Please check ID ?";
								fmMSG.m_sMSG = p;
								fmMSG.ShowDialog();
								if (fmMSG.m_sConfirm == "1")
								{
									m_bIdCheckDone = true;
								}
								else
								{
									this.barcode.Text = "";
									this.price.Text = "";
									return;
								}
							}
							if (bIsBarcodePrice)
							{
								if (sBarcode.IndexOf("*").ToString() == "-1" && sBarcode != "")
								{
									this.price.Text = sNewPrice;
                                    if (Program.m_bEnableCalQty)
                                    {

                                        if (bIsSpecial && System.DateTime.Compare(today, special_price_end_date) < 0 && System.DateTime.Compare(today, special_price_start_date) >= 0)
                                            dPrice = dSpecialPrice;
                                        else
                                            dPrice = dGetPrice;
                                        //dQtyBarcode = GetLabelBarcodeWeight(dPrice, Program.MyDoubleParse(sNewPrice));
                                        dQtyBarcode = GetLabelBarcodeWeight(dPrice, Program.MyDoubleParse(sNewPrice));
                                    }
                                    else
                                        dPrice = Program.MyMoneyParse(sNewPrice);
								}
								if (bMemberPrice)
									m_bMemberPrice = true;
								sBarcode = "";
								m_sCode = "";
								if (Program.m_sgroceryitem.IndexOf(this.barcode.Text).ToString() != "-1" || dGetPrice == 0)
								{
									string Str = this.price.Text.Trim();
									double Num;
									bool isNum = double.TryParse(Str, out Num);

									if (TSIsNumberic(this.price.Text))
										dPrice = Program.MyDoubleParse(this.price.Text) / 100;
									else if (isNum)
										dPrice = Program.MyDoubleParse(this.price.Text);// / 100;
								}
								if (dPrice != 0)
									dGetPrice = dPrice;

								if (AddPackage(sItemBarcode, dQtyBarcode) && dPrice == 0)
									dPrice = dGetPackPrice;

								if (dQtyBarcode > 0)
									dQty *= dQtyBarcode;

								int nIndex = -1;
                                double dQtyInCart = GetQtyInCart(sBarcode, code, ref nIndex, dGetPrice, bIsSpecial);
								string sRFreeItemCode = dr["free_item_required_item_code"].ToString();
								if (nPromotionType == 0)
								{
									if (CheckExistingPromotItem(sGetSupplier_code))
									{
										//MessageBox.Show("b");
										nPromotionType = 5;
										sRFreeItemCode = code;
									}
									else
									{
                                        if (dQtyInCart > 0 && dQtyBarcode <= 1 && !bIsBarcodePrice) //this item already in cart, increase qty
										{
											dQty += dQtyInCart;
											double dPriceCurrent = Program.MyDoubleParse(cart.Rows[nIndex].Cells["cc_price"].Value.ToString());
											double dSub = Math.Round(dPriceCurrent * dQty, 2);
											cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
											cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
											m_fmad.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
											m_fmad.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
											m_fmads.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
											m_fmads.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;

											cart.Rows[nIndex].Cells["has_scale"].Value = bReadWeight.ToString();

											//show current item on duplicate screen //
											string s_dPrice = cart.Rows[nIndex].Cells["cc_price"].Value.ToString();
											string s_dQty = cart.Rows[nIndex].Cells["cc_qty"].Value.ToString();
											string s_dItemName = cart.Rows[nIndex].Cells["cc_name"].Value.ToString();
											int i_dItemName = s_dItemName.Length;
											if (i_dItemName >= 45)
												i_dItemName = 45;
											//MessageBox.Show(i_dItemName1.ToString());
											if (i_dItemName > 30)
												s_dItemName = s_dItemName.Substring(0, 30) + "\r\n" + s_dItemName.Substring(31) + " ...";
											m_fmad.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
											m_fmad.citem2.Text = s_dItemName;

											m_fmads.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
											m_fmads.citem2.Text = s_dItemName;

											CalcCartTotal();
											UpdateDtCart(m_nCurrentCart);
											CheckQtyPromotion(code, dQty, nIndex, barcode.Text).ToString(); //promotion type 3,4,5
											barcode.Text = "";
											qty.Text = "";
											price.Text = "";
											barcode.Focus();
											m_irow = nIndex.ToString();
											timer3.Interval = 500;
											timer3.Start();
											return; //no more action
										}
									}
								}
								else
								{
                                    if (dQtyInCart > 0 && dQtyBarcode <= 1 && !bIsBarcodePrice) //this item already in cart, increase qty
									{
										dQty += dQtyInCart;
										double dPriceCurrent = Program.MyDoubleParse(cart.Rows[nIndex].Cells["cc_price"].Value.ToString());
										double dSub = Math.Round(dPriceCurrent * dQty, 2);
										cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
										cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
										m_fmad.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
										m_fmad.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
										m_fmads.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
										m_fmads.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
										string s_dPrice = cart.Rows[nIndex].Cells["cc_price"].Value.ToString();
										string s_dQty = cart.Rows[nIndex].Cells["cc_qty"].Value.ToString();
										string s_dItemName = cart.Rows[nIndex].Cells["cc_name"].Value.ToString();
										int i_dItemName = s_dItemName.Length;
										if (i_dItemName >= 45)
											i_dItemName = 45;
										//MessageBox.Show(i_dItemName1.ToString());
										if (i_dItemName > 30)
											s_dItemName = s_dItemName.Substring(0, 30) + "\r\n" + s_dItemName.Substring(31) + " ...";
										m_fmad.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
										m_fmad.citem2.Text = s_dItemName;
										m_fmads.citem1.Text = s_dQty + " @ " + Program.MyDoubleParse(s_dPrice).ToString("c");
										m_fmads.citem2.Text = s_dItemName;
										CalcCartTotal();
										UpdateDtCart(m_nCurrentCart);
										CheckQtyPromotion(code, dQty, nIndex, barcode.Text).ToString(); //promotion type 3,4,5
										barcode.Text = "";
										qty.Text = "";
										price.Text = "";
										barcode.Focus();
										m_irow = nIndex.ToString();
										timer3.Interval = 500;
										timer3.Start();
										return; //no more action
									}
								}
								if (dPrice == 0)  // avoid do weight qty ch 04-12-08
									dPrice = dGetPrice;
								if (nPromotionType == 1) //special price discount
								{
									double dDiscount = Program.MyDoubleParse(dr["special_price"].ToString());
									if (m_bMemberPrice)
									{
										if (ScanMembership())
											dPrice = dDiscount;
									}
									else
										dPrice = dDiscount; // FIXED THE BUG, THE DISCOUNT FOR SPECIAL ITEM CH 13/11/2008
								}
								else if (nPromotionType == 2) //percentage discount
								{
									double dRate = Program.MyDoubleParse(dr["discount_percentage"].ToString()) / 100;
									if (m_bMemberPrice)
									{
										if (ScanMembership())
											dPrice = Math.Round(dGetPrice * (1 - dRate), 2);
									}
									else
										dPrice = Math.Round(dGetPrice * (1 - dRate), 2);
								}
								if (sPrice4 != 0)
									dPrice = sPrice4;

								string sCCode = dr["code"].ToString();
								string packagepromotype = "0";

                                if (dQtyBarcode > 1 && !bIsBarcodePrice)
								{
									packagepromotype = nPromotionType.ToString();
									m_bPackageDue = true;
								}
								else
								{
									m_bPackageDue = false;
								}
								if (Program.MyDoubleParse(qty.Text) >= 2 && nPromotionType != 0)
									packagepromotype = nPromotionType.ToString();


								if (!CheckQtyPromotion(sCCode, dQty, 0, barcode.Text))
									AddToCart(sBarcode, dr["code"].ToString(), dr["name"].ToString(), dr["name_cn"].ToString(), dPrice, dQty, packagepromotype, m_bPackageDue, dr["supplier_code"].ToString(), 0, bAvoidPoint, false);
								string s_dPrice1 = cart.Rows[0].Cells["cc_price"].Value.ToString();
								string s_dQty1 = cart.Rows[0].Cells["cc_qty"].Value.ToString();
								string s_dItemName1 = cart.Rows[0].Cells["cc_name"].Value.ToString();
								int i_dItemName1 = s_dItemName1.Length;
								if (i_dItemName1 >= 45)
									i_dItemName1 = 45;
								//MessageBox.Show(i_dItemName1.ToString());
								if (i_dItemName1 > 30)
									s_dItemName1 = s_dItemName1.Substring(0, 30) + "\r\n" + s_dItemName1.Substring(31) + " ...";
								m_fmad.citem1.Text = s_dQty1 + " @ " + Program.MyDoubleParse(s_dPrice1).ToString("c");
								m_fmad.citem2.Text = s_dItemName1;
								m_fmads.citem1.Text = s_dQty1 + " @ " + Program.MyDoubleParse(s_dPrice1).ToString("c");
								m_fmads.citem2.Text = s_dItemName1;
								this.msgboard2.Text = "";
								/*****************************************************/

							}
							else
							{
								if (GetItemFromServer(barcode.Text))
									return;
								if (File.Exists(m_sSoundDonePath))
								{
									SoundPlayer error = new SoundPlayer(m_sSoundDonePath);
									error.Play();
								}
								string p = "Sorry, Product Not Found !!";
								FormMSG fmMSG = new FormMSG();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08
								if (this.barcode.Text.Length >= 14)
									p = DoUseVoucher(this.barcode.Text);
								string pNew = p + "\r\n Would you like to Create a new Item??";
								fmMSG.m_sYesNo = "1";
								fmMSG.m_sMsg = pNew;
								if (this.barcode.Text == m_sButton11 || this.barcode.Text == m_sButton12)
								{
									fmMSG.m_sYesNo = "0";
									fmMSG.m_sMsg = " Please Enter Price First!!";
									fmMSG.btnNo.Visible = false;
									fmMSG.btnYes.Visible = false;
									fmMSG.ShowDialog();
								}
								if (this.barcode.Text != m_sButton11 && this.barcode.Text != m_sButton12)
									fmMSG.ShowDialog();
								if (fmMSG.m_sCreateNewItem == "1")
								{
									/********************this.CreateNewBarcodeTB.Text = this.barcode.Text;
									Program.m_Barcode = this.barcode.Text;
									FormAddNewItem fa = new FormAddNewItem();
									fa.ShowDialog();
									this.CreateNewItemPanel.Visible = true;
									//doBuideBrands();
									//doBuideCat();
									//this.CreateNewBarcodeTB.Text = this.barcode.Text;
									fmMSG.m_sCreateNewItem = "0";
									DoScanBarcode();**********************************************/
									this.CreateNewItemPanel.Visible = true;
									doBuideBrands();
									doBuideCat();
									doBuideTax();

									this.CreateNewBarcodeTB.Text = this.barcode.Text;
									fmMSG.m_sCreateNewItem = "0";

								}
								else
								{
									this.barcode.Focus();
									if (this.barcode.Text.Length >= 10)
										p = "";
									this.msgboard2.Text = p;
								}
							}
							if (cart.Rows.Count == 1)
								Change.Text = "";
							barcode.Text = "";
							qty.Text = "";
							price.Text = "";
						}
						else
						{
							if (GetItemFromServer(barcode.Text))
								return;
							if (File.Exists(m_sSoundDonePath))
							{
								SoundPlayer error = new SoundPlayer(m_sSoundDonePath);
								error.Play();
							}

							string p = "Sorry, product is not found !!";
							FormMSG fmMSG = new FormMSG();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08

							string pNew = ""; 
							if (this.barcode.Text.Length >= 14)
							{
								p = DoUseVoucher(this.barcode.Text);
								pNew += p;
								Program.MsgBox(p);
								barcode.Text = "";
								qty.Text = "";
								price.Text = "";
								return;
							}
							else
							{
								pNew += "\r\n Would you like to Create a new Item??";
							}
							fmMSG.m_sYesNo = "1";
							fmMSG.m_sMsg = pNew;
							if (this.barcode.Text == m_sButton11 || this.barcode.Text == m_sButton12)
							{
								fmMSG.m_sYesNo = "0";
								fmMSG.m_sMsg = " Please Enter Price First!!";
								fmMSG.btnNo.Visible = false;
								fmMSG.btnYes.Visible = false;
								fmMSG.ShowDialog();
							}
							if (this.barcode.Text != m_sButton11 && this.barcode.Text != m_sButton12)
								fmMSG.ShowDialog();
							if (fmMSG.m_sCreateNewItem == "1")
							{
								/********************this.CreateNewBarcodeTB.Text = this.barcode.Text;
								Program.m_Barcode = this.barcode.Text;
								FormAddNewItem fa = new FormAddNewItem();
								fa.ShowDialog();
								this.CreateNewItemPanel.Visible = true;
								//doBuideBrands();
								//doBuideCat();
								//this.CreateNewBarcodeTB.Text = this.barcode.Text;
								fmMSG.m_sCreateNewItem = "0";
								DoScanBarcode();**********************************************/
								this.CreateNewItemPanel.Visible = true;
								doBuideBrands();
								doBuideCat();
								doBuideTax();
								this.CreateNewBarcodeTB.Text = this.barcode.Text;
								fmMSG.m_sCreateNewItem = "0";
							}
							else
							{
								this.barcode.Focus();
								if (this.barcode.Text.Length >= 10)
									p = "";
								this.msgboard2.Text = p;
							}
						}
						if (cart.Rows.Count == 1)
							Change.Text = "";
						barcode.Text = "";
						qty.Text = "";
						price.Text = "";
					}
					else
					{
						if (GetItemFromServer(barcode.Text))
							return;

						if (File.Exists(m_sSoundDonePath))
						{
							SoundPlayer error = new SoundPlayer(m_sSoundDonePath);
							error.Play();
						}

						string p = "Sorry, product is not found !!";
						FormMSG fmMSG = new FormMSG();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08

						if (this.barcode.Text.Length >= 14)
							p = DoUseVoucher(this.barcode.Text);
						//fmMSG.m_sMsg = p;
						string pNew = p + "\r\n Would you like to Create a new Item??";
						fmMSG.m_sYesNo = "1";
						fmMSG.m_sMsg = pNew;
						if (this.barcode.Text == m_sButton11 || this.barcode.Text == m_sButton12)
						{
							fmMSG.m_sYesNo = "0";
							fmMSG.m_sMsg = " Please Enter Price First!!";
							fmMSG.btnNo.Visible = false;
							fmMSG.btnYes.Visible = false;
							fmMSG.ShowDialog();
						}
						if (this.barcode.Text != m_sButton11 && this.barcode.Text != m_sButton12)
							fmMSG.ShowDialog();
						if (fmMSG.m_sCreateNewItem == "1")
						{
							/********************this.CreateNewBarcodeTB.Text = this.barcode.Text;
							Program.m_Barcode = this.barcode.Text;
							FormAddNewItem fa = new FormAddNewItem();
							fa.ShowDialog();
							this.CreateNewItemPanel.Visible = true;
							//doBuideBrands();
							//doBuideCat();
							//this.CreateNewBarcodeTB.Text = this.barcode.Text;
							fmMSG.m_sCreateNewItem = "0";
							DoScanBarcode();**********************************************/
							this.CreateNewItemPanel.Visible = true;
							doBuideBrands();
							doBuideCat();
							doBuideTax();
							//                      doBuideSCat();
							//                      doBuideSSCat();
							this.CreateNewBarcodeTB.Text = this.barcode.Text;
							fmMSG.m_sCreateNewItem = "0";

						}
						else
						{
							this.barcode.Focus();
							if (this.barcode.Text.Length >= 10)
								p = "";
							this.msgboard2.Text = p;
						}
					}
					if (cart.Rows.Count == 1)
						Change.Text = "";
					barcode.Text = "";
					qty.Text = "";
					price.Text = "";
				}
				else
				{
					if (File.Exists(m_sSoundDonePath))
					{
						SoundPlayer error = new SoundPlayer(m_sSoundDonePath);
						error.Play();
					}
					string p = "Sorry, Please Check the number BarcodeLabel Settings !!";
					FormMSG fmMSG = new FormMSG();  // ADMIN CONTROL AREA, PASSWORD REQUIRED CH 14/11/08
					if (this.barcode.Text.Length >= 14)
						p = DoUseVoucher(this.barcode.Text);
					fmMSG.m_sMsg = p;
					fmMSG.btnNo.Visible = false;
					fmMSG.btnYes.Visible = false;
					fmMSG.ShowDialog();
					// MessageBox.Show("Item Not Found ");
					this.barcode.Focus();
					if (this.barcode.Text.Length >= 10)
						p = "";
					this.msgboard2.Text = p;
					barcode.Text = "";
				}
				#endregion not_found
			}
		}

        string getPromoTypeByPromoId(int id)
        {
            int rows = 0;
            string sc = "";
            if (dst.Tables["getPromoTypeByPromoId"] != null)
                dst.Tables["getPromoTypeByPromoId"].Clear();
            if (id != null)
            {
                sc = "select promo_type from promotion_list WHERE 1=1 ";
                sc += " AND promo_id = '" + id + "'";
                try
                {
                    myAdapter = new SqlDataAdapter(sc, myConnection);
                    rows = myAdapter.Fill(dst, "getPromoTypeByPromoId");
                }
                catch (Exception e1)
                {
                    myConnection.Close();
                    Program.ShowExp(sc, e1);
                    return "0";
                }
                if (rows == 1)
                {
                    DataRow dr = dst.Tables["getPromoTypeByPromoId"].Rows[0];
                    string promoType = dr["promo_type"].ToString();
                    return promoType;
                }
                else
                    return "0";
            }
            else
                return "0";
        }

		private bool overLimit(int code, int limit)
		{
			if (limit == 0)
				return false;
			int sum = 0;
			for (int i = 0; i < cart.Rows.Count; ++i)
			{
				if (cart.Rows[i].Cells["cc_code"].Value.ToString() == code.ToString())
					sum += Convert.ToInt32(cart.Rows[i].Cells[6].Value);
			}
			if (sum >= limit)
				return true;
			else
				return false;
		}
		private void doBuideBrands1()
		{
			//this.BrandComboBox.Items.Clear();
			int rows = 0;
			if (dst.Tables["brand_selection"] != null)
				dst.Tables["brand_selection"].Clear();
			string sc = " SELECT  cat, s_cat FROM catalog ";
			sc += " WHERE LOWER(cat) = 'brands'";
			sc += " GROUP BY s_cat, cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "brand_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			//this.BrandComboBox.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["brand_selection"].Rows[i];
				string cat = dr["s_cat"].ToString();
				if (cat.ToLower() == "zzzothers")
					continue;
				//	this.BrandComboBox.Items.Add(cat);
			}
		}
		private void doBuideCat1()
		{
			//this.catselectionCB.Items.Clear();
			int rows = 0;
			if (dst.Tables["cat_selection"] != null)
				dst.Tables["cat_selection"].Clear();
			string sc = " SELECT  cat FROM catalog ";
			sc += " WHERE 1=1 AND LOWER(cat) <> 'brands'";
			sc += " GROUP BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "cat_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			//	this.catselectionCB.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["cat_selection"].Rows[i];
				string cat = dr["cat"].ToString();
				//this.catselectionCB.Items.Add(cat);
			}
		}
		private bool CheckQtyPromotion(string code, double dQtyIn, int nRowIndex, string barcode)
		{
			if (dst.Tables["cqp"] != null)
				dst.Tables["cqp"].Clear();
			int nWeekDay = (int)DateTime.Now.DayOfWeek;
			if (nWeekDay == 0)
				nWeekDay = 7; //tee's design, Sunday is 7
			string sc = " SELECT c.supplier_code, c.price1,c.name,ISNULL(c.avoid_point, '0') AS avoid_point, c.name_cn, c.special_price AS special_price1, c.is_special, p.promo_type, p.volumn_discount_qty, p.volumn_discount_price_total ";
			sc += ", p.free_qty_required_qty, p.free_qty_reward_qty, ISNULL(p.promo_member_only,'false') AS promo_member_only, c.avoid_point ";
			sc += ", p.free_item_required_qty, p.free_item_required_item_code, free_item_reward_qty ";
            sc += ", isnull(p.promo_day1,0) as promo_day1, isnull(p.promo_day2,0) as promo_day2, isnull(p.promo_day3,0) as promo_day3, isnull(p.promo_day4,0) as promo_day4, isnull(p.promo_day5,0) as promo_day5, isnull(p.promo_day6,0) as promo_day6, isnull(p.promo_day7,0) as promo_day7 ";
			sc += " FROM code_relations c ";
            sc += " JOIN promo pr on pr.code = c.code ";
			sc += " JOIN promotion_list p ON p.promo_id = pr.promo_id AND p.promo_active = 1 ";
			sc += " AND p.promo_start_date <= GETDATE() AND p.promo_end_date >= GETDATE() "; //started, but not yet expired
			//*** ARGUMENT FOR PICK THE THE RIGHT DAY FOR QTY PROMOTION CH 13/11/08***//
			if (CheckQtyPromotionValue(code, nWeekDay, ""))
			{
				sc += " AND p.promo_day" + nWeekDay.ToString() + " = 1 "; //today is in the DayOfWeek list 
			}
			//  sc += " AND p.promo_type IN (3,4,5) "; //only qty promotions that we interested
			sc += " WHERE c.code = " + code;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "cqp");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}

			if (dst.Tables["cqp"].Rows.Count <= 0) //error, code not found
				return false;

			DataRow dr = dst.Tables["cqp"].Rows[0];
			double dNormalPrice = Program.MyDoubleParse(dr["price1"].ToString());
			int nPromotionType = Program.MyIntParse(dr["promo_type"].ToString());
			string sRCode = dr["free_item_required_item_code"].ToString();
			double dPromoItemRequireQty = Program.MyDoubleParse(dr["free_qty_required_qty"].ToString());
			double dPromoFreeItemQty = Program.MyDoubleParse(dr["free_qty_reward_qty"].ToString());
			string sPromoItemNameEn = dr["name"].ToString();
			string sPromoItemNameCn = dr["name_cn"].ToString();
			string sPromoItemSupplierCode = dr["supplier_code"].ToString();
			bool s_bMemberPrice = bool.Parse(dr["promo_member_only"].ToString());
			bool bIsAvoid_point = bool.Parse(dr["avoid_point"].ToString());

            bool bDayActive = false; 
            Dictionary<string, bool> promo_day = new Dictionary<string, bool>();
            promo_day.Clear();
            for (int i = 1; i < 8; i++)
            {
                promo_day.Add(i.ToString(), bool.Parse(dr["promo_day"+i].ToString()));
            }
            //promo_day.Add("monday", bool.Parse(dr["promo_day1"].ToString()));
            //promo_day.Add("tuesday", bool.Parse(dr["promo_day2"].ToString()));
            //promo_day.Add("wednesday", bool.Parse(dr["promo_day3"].ToString()));
            //promo_day.Add("thursday", bool.Parse(dr["promo_day4"].ToString()));
            //promo_day.Add("friday", bool.Parse(dr["promo_day5"].ToString()));
            //promo_day.Add("saturday", bool.Parse(dr["promo_day6"].ToString()));
            //promo_day.Add("sunday", bool.Parse(dr["promo_day7"].ToString()));
            foreach (var day in promo_day)
            {
                if (day.Key == nWeekDay.ToString() && day.Value)
                    bDayActive = true;
            }


			string sCartRows = cart.Rows.Count.ToString();
			// string supplier_code = dr["supplier_code"].ToString();
			if (s_bMemberPrice)
				m_bMemberPrice = true;
			double dKeyQty = 0;
			if (qty.Text != "" && Program.MyDoubleParse(qty.Text) >= 2)
				dKeyQty = Program.MyDoubleParse(qty.Text);

			if (AddPackage(barcode, dQtyIn) && nPromotionType != 0 && nPromotionType != 3)
			{
				return false;
			}
			Program.Trim(ref sPromoItemSupplierCode);
			if (nPromotionType == 0)
			{
				if (m_bMemberPrice)
				{
					if (ScanMembership())
					{
						if (CheckExistingPromotItem(sPromoItemSupplierCode))
						{
							//MessageBox.Show("a");
							nPromotionType = 5;
							sRCode = code;
						}
					}
				}
				else
				{
					if (CheckExistingPromotItem(sPromoItemSupplierCode))
					{
						//MessageBox.Show("a");
						nPromotionType = 5;
						sRCode = code;
					}
				}
			}

            if (nPromotionType == 3 && bDayActive) //free qty
			{
				double dTotalQtyForPromo3 = 0;
				for (int i = 0; i < cart.Rows.Count; i++)
				{
					string sQtyType3 = cart.Rows[i].Cells["cc_qty"].Value.ToString();
					dTotalQtyForPromo3 += Program.MyDoubleParse(sQtyType3);
				}

				double dQtyBoxIn = 1;
				if (qty.Text != "")
					dQtyBoxIn = Program.MyDoubleParse(qty.Text);
				if (m_bQtyEditing) //after editing qty, the total qty is qtyInCart
					dQtyBoxIn = 0;

				//				if (qty.Text != "")
				//				if (dQtyBoxIn != 0)
				{
					if (m_bMemberPrice)
					{
						if (ScanMembership())
							doPromotionKeyInValue(code, sPromoItemNameEn, sPromoItemNameCn, dNormalPrice, dQtyBoxIn
							, sPromoItemSupplierCode, dPromoItemRequireQty, dPromoFreeItemQty, nRowIndex, dTotalQtyForPromo3, bIsAvoid_point);
						else
							return false;
					}
					else
					{
						doPromotionKeyInValue(code, sPromoItemNameEn, sPromoItemNameCn, dNormalPrice, dQtyBoxIn
						, sPromoItemSupplierCode, dPromoItemRequireQty, dPromoFreeItemQty, nRowIndex, dTotalQtyForPromo3, bIsAvoid_point);
					}
					return true;
				}
				/*
								double dQtyRequired = Program.MyDoubleParse(dr["free_qty_required_qty"].ToString());
								int nReword = (int)(dQtyIn / dQtyRequired); // how many volumn, should >= 1
								double dNewQty = dQtyIn;
								double dQtyRemain = dQtyIn % dQtyRequired; //remain qty that not enough to get volumn discount
								double dQtyReward = Program.MyDoubleParse(dr["free_qty_reward_qty"].ToString()) * nReword;
								if (dQtyRemain > 0)
									dNewQty = dQtyRequired * nReword;

							   // doPromotionThree(code, sPromoItemNameEn, sPromoItemNameCn, dNormalPrice, dQtyIn, sPromoItemSupplierCode, dQtyRequired, Program.MyDoubleParse(dr["free_qty_reward_qty"].ToString()));
								if (dQtyIn <= dQtyRequired) //not enough qty to get discount
								   return false;

								if (m_bMemberPrice)
								{
									if (!ScanMembership())
										return false;
								}
								if (sCartRows != "0" && qty.Text == "")
								{
									string sSellingPrice = cart.Rows[nRowIndex].Cells["cc_price"].Value.ToString();
									double dNewSubtotal = dNewQty * Program.MyDoubleParse(sSellingPrice.ToString());
									//update current existing row

									string supplier_code = cart.Rows[nRowIndex].Cells["cc_supplier_code"].Value.ToString();
									string name = cart.Rows[nRowIndex].Cells["cc_name"].Value.ToString();
									string name_cn = cart.Rows[nRowIndex].Cells["cc_name_cn"].Value.ToString();
									string sPointAvoid = cart.Rows[nRowIndex].Cells["cc_avoid_point"].Value.ToString();
									bool bPointAvoid = bool.Parse(sPointAvoid);
						
									name = name + " (Free Qty) ";
									name_cn = name_cn + " (Free Qty) ";

									cart.Rows[nRowIndex].Cells["cc_price"].Value = dNormalPrice.ToString();
									cart.Rows[nRowIndex].Cells["cc_qty"].Value = dNewQty.ToString();
									cart.Rows[nRowIndex].Cells["cc_total"].Value = dNewSubtotal.ToString();

									cart.Rows[nRowIndex].Cells["cc_is_promotion"].Value = "3"; //mark it as a promoted item
				
									m_fmad.cart.Rows[nRowIndex].Cells["cc_price"].Value = dNormalPrice.ToString();
									m_fmad.cart.Rows[nRowIndex].Cells["cc_qty"].Value = dNewQty.ToString();

									m_fmads.cart.Rows[nRowIndex].Cells["cc_price"].Value = dNormalPrice.ToString();
									m_fmads.cart.Rows[nRowIndex].Cells["cc_qty"].Value = dNewQty.ToString();

									CalcCartTotal();

									if (dQtyRemain > 0) // not idealy, add one more item
										AddToCart(barcode, code, name, name_cn, 0, 1, "3", false, supplier_code, 0, bPointAvoid,false);
									 return true;
								}
								return false;
				 */
			}

			if (nPromotionType == 5) //free item
			{
				double dQtyRequired = Program.MyDoubleParse(dr["free_item_required_qty"].ToString());
				if (dQtyIn < dQtyRequired) //not enough qty to get discount
					return false;

				if (sCartRows != "0")
				{
					cart.Rows[nRowIndex].Cells["cc_is_promotion"].Value = "5";
					nPromotionType = 0;
					CheckPromoFreeItem(code, nRowIndex, dQtyIn, dKeyQty);

				}

				if (sRCode != sPromoItemSupplierCode)
					return false;
				int nReward = Program.MyIntParse((dQtyIn / dQtyRequired).ToString());
				double dQtyReward = 1;//Program.MyDoubleParse(dr["free_item_reward_qty"].ToString()) * nReward;
				if (qty.Text != "")
					dQtyReward = Program.MyDoubleParse(CheckFreeRewardItem(code, dQtyIn, sRCode));

				if (dst.Tables["cqp_fi"] != null)
					dst.Tables["cqp_fi"].Clear();
				sc = " SELECT supplier_code, name, name_cn, ISNULL(avoid_point, '0') AS avoid_point FROM code_relations WHERE code = " + sRCode;
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "cqp_fi") <= 0)
					{
						MessageBox.Show("Free Item not found, code = " + sRCode, "Promotion Error");
						return false;
					}
				}
				catch (Exception e)
				{
					myConnection.Close();
					Program.ShowExp(sc, e);
					return false;
				}

				DataRow dri = dst.Tables["cqp_fi"].Rows[0];
				string name = dri["name"].ToString();
				string name_cn = dri["name_cn"].ToString();
				bool bPointAvoid = bool.Parse(dri["avoid_point"].ToString());
				name = name + " (Free Item) ";
				name_cn = name_cn + " (Free Item) ";
				cart.Rows[nRowIndex].Cells["cc_is_promotion"].Value = "5";
				AddToCart(barcode, sRCode, name, name_cn, 0, dQtyReward, "5", false, dri["supplier_code"].ToString(), 0, bPointAvoid, false);
				return true;
			}
			if (nPromotionType == 4) //volumn discount
			{
				double dDiscount = Program.MyDoubleParse(dr["volumn_discount_price_total"].ToString());
				double dQtyRequired = Program.MyDoubleParse(dr["volumn_discount_qty"].ToString());
				if (qty.Text != "")
				{
					if (m_bMemberPrice)
					{
						if (ScanMembership())
							doKeyInPromotionValueFour(code, sPromoItemNameEn, sPromoItemNameCn, dNormalPrice
							, dDiscount, Program.MyDoubleParse(qty.Text), sPromoItemSupplierCode, dQtyRequired, bIsAvoid_point);
						else
							return false;
					}
					else
						doKeyInPromotionValueFour(code, sPromoItemNameEn, sPromoItemNameCn, dNormalPrice, dDiscount
						, Program.MyDoubleParse(qty.Text), sPromoItemSupplierCode, dQtyRequired, bIsAvoid_point);
					return true;
				}
				if (m_bMemberPrice)
				{
					if (!ScanMembership())
						return false;
				}
				if (dQtyIn < dQtyRequired) //not enough qty to get discount
					return false;
				int nVolumn = (int)(dQtyIn / dQtyRequired); // how many volumn, should >= 1
				double dNewQty = dQtyIn;
				double dQtyRemain = dQtyIn % dQtyRequired; //remain qty that not enough to get volumn discount
				if (dQtyRemain > 0)
					dNewQty = dQtyRequired * nVolumn;
				if (sCartRows != "0" && qty.Text == "")
				{
					string supplier_code = cart.Rows[nRowIndex].Cells["cc_supplier_code"].Value.ToString();
					string name = cart.Rows[nRowIndex].Cells["cc_name"].Value.ToString();
					string name_en = cart.Rows[nRowIndex].Cells["cc_name_en"].Value.ToString();
					string name_cn = cart.Rows[nRowIndex].Cells["cc_name_cn"].Value.ToString();
					string sAvoid_Point_item = cart.Rows[nRowIndex].Cells["cc_avoid_point"].Value.ToString();
					bool bAvoid_Point_item = bool.Parse(sAvoid_Point_item);
					cart.Rows[nRowIndex].Cells["cc_price"].Value = dNormalPrice.ToString();
					cart.Rows[nRowIndex].Cells["cc_qty"].Value = dNewQty.ToString();
					cart.Rows[nRowIndex].Cells["cc_total"].Value = Math.Round(dDiscount, 2).ToString();
					cart.Rows[nRowIndex].Cells["cc_is_promotion"].Value = "4"; //mark it as a promoted item
					m_fmad.cart.Rows[nRowIndex].Cells["cc_price"].Value = dNormalPrice.ToString();
					m_fmad.cart.Rows[nRowIndex].Cells["cc_qty"].Value = dNewQty.ToString();
					m_fmad.cart.Rows[nRowIndex].Cells["cc_total"].Value = Math.Round(dDiscount, 2).ToString();

					m_fmads.cart.Rows[nRowIndex].Cells["cc_price"].Value = dNormalPrice.ToString();
					m_fmads.cart.Rows[nRowIndex].Cells["cc_qty"].Value = dNewQty.ToString();
					m_fmads.cart.Rows[nRowIndex].Cells["cc_total"].Value = Math.Round(dDiscount, 2).ToString();

					CalcCartTotal();
					if (dQtyRemain > 0) // not idealy, add one more item
					{
						AddToCart(barcode, code, name, name_cn, dNormalPrice, dQtyRemain, "0", false, supplier_code, 0, bAvoid_Point_item, false);
					}
					return true;
				}
				return false;
			}
			return false;
		}

		private double GetQtyInCart(string sBarcode, string sCodeIn, ref int nIndex, double sPrice, bool is_special)
		{
			string s_sGroceryItemCode = Program.m_sgroceryitem;
			string s_sGroceryItemWeight = Program.m_sgroceryweight;
			string s_sQtyAddUp = Program.m_sgroceryitemaddup;
			string s_sWeightAddUp = Program.m_sgroceryweightitemaddup;
			bool s_sQtyDuoUp = false;
			bool s_sWeightDuoUp = false;
			if (s_sQtyAddUp == "1")
				s_sQtyDuoUp = true;
			if (s_sWeightAddUp == "1")
				s_sWeightDuoUp = true;
			sPrice = Math.Round(sPrice, 2);

			double dRet = 0;
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string code = cart.Rows[i].Cells["cc_code"].Value.ToString();
				string sName = cart.Rows[i].Cells["cc_name"].Value.ToString();
				string s_cc_supplier_code = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
                string barcode = cart.Rows[i].Cells["cc_barcode"].Value.ToString();
				//MessageBox.Show(s_sGroceryItemCode.IndexOf(s_cc_supplier_code).ToString() + "  " + s_cc_supplier_code + " " + s_sGroceryItemCode);
                if (code == m_sVoucherItem && barcode != sBarcode)
                    continue;
				if (code != sCodeIn)
					continue;
				int nPromotion = Program.MyIntParse(cart.Rows[i].Cells["cc_is_promotion"].Value.ToString());
                if (nPromotion != 0 && nPromotion != 6 && getPromoTypeByPromoId(nPromotion) != "4") //it's a promotion added item
					continue;
				double iPrice = Math.Round(Program.MyDoubleParse(cart.Rows[i].Cells["cc_price"].Value.ToString()), 2);
				if (!is_special && !Program.m_bEnableLevelPrice)
				{
					if (iPrice != sPrice)
						continue;
				}
				if (!s_sQtyDuoUp)
				{
					if (s_sGroceryItemCode.IndexOf(s_cc_supplier_code).ToString() != "-1")
						continue;
				}
				if (!s_sWeightDuoUp)
				{
					if (s_sGroceryItemWeight.IndexOf(s_cc_supplier_code).ToString() != "-1")
						continue;
				}
				//found it
				double dQty = Program.MyDoubleParse(cart.Rows[i].Cells["cc_qty"].Value.ToString());
				dRet += dQty;
				nIndex = i; //return row index as reference
			}
			return dRet;
		}


		private double GetQtyInCart1(string sCodeIn, ref int nIndex, double sPrice, bool is_special)
		{
			string s_sGroceryItemCode = Program.m_sgroceryitem;
			string s_sGroceryItemWeight = Program.m_sgroceryweight;
			string s_sQtyAddUp = Program.m_sgroceryitemaddup;
			string s_sWeightAddUp = Program.m_sgroceryweightitemaddup;
			bool s_sQtyDuoUp = false;
			bool s_sWeightDuoUp = false;
			if (s_sQtyAddUp == "1")
				s_sQtyDuoUp = true;
			if (s_sWeightAddUp == "1")
				s_sWeightDuoUp = true;
			sPrice = Math.Round(sPrice, 2);

			double dRet = 0;
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string code = cart.Rows[i].Cells["cc_code"].Value.ToString();
				string sName = cart.Rows[i].Cells["cc_name"].Value.ToString();
				string s_cc_supplier_code = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
				//MessageBox.Show(s_sGroceryItemCode.IndexOf(s_cc_supplier_code).ToString() + "  " + s_cc_supplier_code + " " + s_sGroceryItemCode);
				if (code != sCodeIn)
					continue;
				int nPromotion = Program.MyIntParse(cart.Rows[i].Cells["cc_is_promotion"].Value.ToString());
				if (nPromotion != 0) //it's a promotion added item
					continue;
				double iPrice = Math.Round(Program.MyDoubleParse(cart.Rows[i].Cells["cc_price"].Value.ToString()), 2);
				if (!is_special)
				{
					if (iPrice != sPrice)
						continue;
				}
				if (!s_sQtyDuoUp)
				{
					if (s_sGroceryItemCode.IndexOf(s_cc_supplier_code).ToString() != "-1")
						continue;
				}
				if (!s_sWeightDuoUp)
				{
					if (s_sGroceryItemWeight.IndexOf(s_cc_supplier_code).ToString() != "-1")
						continue;
				}
				//found it
				double dQty = Program.MyDoubleParse(cart.Rows[i].Cells["cc_qty"].Value.ToString());
				dRet += dQty;
				nIndex = i; //return row index as reference
			}
			return dRet;
		}

		private void AddToCartSurcharge(string sBarcode, string sCode, string sName, string sNameCN, double dPrice, double dQty
			, string is_promotion, bool b_packageDUE, string sSupplierCode, double dPromotionTotal, bool avoid_point, bool has_scale)
		{
			string sdName = sName;
			double dNormalPrice = GetNormalPrice(sCode);
			double m_dRowTotal = 0;
			if (dNormalPrice == 0)
				dNormalPrice = dPrice;
			if (dNormalPrice < dPrice)
				dNormalPrice = dPrice;
			if (m_bChineseDesc)
			{
				if (sNameCN != "")
					sdName = sNameCN;
			}
			if (dNormalPrice > dPrice)
				sdName = sdName + "(" + dNormalPrice.ToString("c") + ")";

			dPrice = Math.Round(dPrice, 2);
			dNormalPrice = Math.Round(dNormalPrice, 2);
			m_dRowTotal = Math.Round(dPrice * dQty, 2);
			if (is_promotion == "4" && !b_packageDUE)
			{
				m_dRowTotal = dPromotionTotal;
				dPrice = m_dRowTotal / dQty;
			}
			if (b_packageDUE)
			{
				double dUnitPrice = dPrice / dQty;
				string[] row = { sCode, sSupplierCode, sdName, Math.Round(dUnitPrice, 4).ToString(), " ", avoid_point.ToString()
				, dQty.ToString(), Math.Round(dUnitPrice * dQty, 2).ToString("N2"), "X", sName, sNameCN, is_promotion, dNormalPrice.ToString()
				, "", sBarcode, has_scale.ToString() };
				cart.Rows.Insert(cart.Rows.Count, row);
				if (is_promotion.ToString() == "true")
					cart.CurrentCell = cart.Rows[0].Cells[1];
				string[] rowad = { sdName, Math.Round(dUnitPrice, 2).ToString("N2"), "", dQty.ToString(), Math.Round(dPrice, 2).ToString("N2") };
				m_fmad.cart.Rows.Insert(0, rowad);
				m_fmad.cart.Rows[0].Selected = true;
				m_fmads.cart.Rows.Insert(0, rowad);
				m_fmads.cart.Rows[0].Selected = true;
			}
			else
			{
				string[] row = { sCode, sSupplierCode, sdName, Math.Round(dPrice, 3).ToString("N2"), " ", avoid_point.ToString()
				, dQty.ToString(), m_dRowTotal.ToString("N2"), "x", sName, sNameCN, is_promotion, dNormalPrice.ToString(), ""
				, sBarcode, has_scale.ToString() };
				cart.Rows.Insert(cart.Rows.Count, row);
				if (is_promotion.ToString() == "true")
					cart.CurrentCell = cart.Rows[0].Cells[1];
				string[] rowad = { sdName, Math.Round(dPrice, 2).ToString("N2"), "", dQty.ToString(), m_dRowTotal.ToString("N2") };
				m_fmad.cart.Rows.Insert(0, rowad);
				m_fmad.cart.Rows[0].Selected = true;
				m_fmads.cart.Rows.Insert(0, rowad);
				m_fmads.cart.Rows[0].Selected = true;
			}
			cart.CurrentCell = cart.Rows[0].Cells[1];
			cart.Update();
			sShowItemTotal();
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
			this.barcode.Focus();
			if (m_mod != null)
			{
				string ss = sdName + "," + Math.Round(dPrice, 3).ToString("N2") + ", QTY: " + dQty.ToString();
				if (ss.Length < 40)
				{
					ss = ss + "                                                  ";
					ss = ss.Substring(0, 40);
				}
				else
					ss = ss.Substring(0, 40);
				m_mod.SendCmd(ss);
			}
		}

        string getPromoidByCode(string code)
        {
            if (dst.Tables["getPromoidByCode"] != null)
                dst.Tables["getPromoidByCode"].Clear();
            string sc = "";
            int row = 0;
            string promo_id = "";
            sc  = " set dateformat dmy ";
            sc += " select top 1 p.* from promo p join promotion_list pl ";
            sc += " on p.promo_id = pl.promo_id where 1=1 ";
            sc += " and promo_start_date <=getdate() and promo_end_date >=getdate() ";
            sc += " ANd p.code = '"+code+"'";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                row = myAdapter.Fill(dst, "getPromoidByCode");
            }
            catch (Exception e1)
            {
                myConnection.Close();
                Program.ShowExp(sc, e1);
                return "";
            }
            if(row ==1)
                 promo_id = dst.Tables["getPromoidByCode"].Rows[0]["promo_id"].ToString();
            return promo_id;
        }

		private void AddToCart(string sBarcode, string sCode, string sName, string sNameCN, double dPrice, double dQty
		, string is_promotion, bool b_packageDUE, string sSupplierCode, double dPromotionTotal, bool avoid_point, bool has_scale)
		{
			string sdName = sName;
			double dNormalPrice = GetNormalPrice(sCode);
            string promo_id = getPromoidByCode(sCode);
			double dDiscount = 0;
			double m_dRowTotal = 0;
			if (dNormalPrice == 0)
				dNormalPrice = dPrice;
			if (dNormalPrice < dPrice)
				dNormalPrice = dPrice;
			if (m_bChineseDesc)
			{
				if (sNameCN != "")
					sdName = sNameCN;
			}
			if (dNormalPrice > dPrice)
				sdName = sdName + "(" + dNormalPrice.ToString("c") + ")";

			dPrice = Math.Round(dPrice, 2);
			dNormalPrice = Math.Round(dNormalPrice, 2);
			dDiscount = Math.Round(dNormalPrice - dPrice, 2);
			m_dRowTotal = Math.Round(dPrice * dQty, 2);
			if (is_promotion == "4" && !b_packageDUE)
			{
				m_dRowTotal = dPromotionTotal;
                //dPrice = m_dRowTotal / dQty;
			}
			if (b_packageDUE)
			{
				double dUnitPrice = dPrice / dQty;
                string[] row = { sCode, sCode, sdName, Math.Round(dUnitPrice, 4).ToString(), " ", avoid_point.ToString()
				, dQty.ToString(), Math.Round(dUnitPrice * dQty, 2).ToString("N2"), "X", sName, sNameCN, is_promotion, dNormalPrice.ToString()
				, "", sBarcode, has_scale.ToString(),promo_id };
				cart.Rows.Insert(0, row);
				if (is_promotion.ToString() == "true")
					cart.CurrentCell = cart.Rows[0].Cells[1];
				string[] rowad = { sdName, Math.Round(dUnitPrice, 2).ToString("N2"), "", dQty.ToString(), Math.Round(dPrice, 2).ToString("N2") };
				m_fmad.cart.Rows.Insert(0, rowad);
				m_fmad.cart.Rows[0].Selected = true;
				m_fmads.cart.Rows.Insert(0, rowad);
				m_fmads.cart.Rows[0].Selected = true;
			}
			else
			{
                string[] row = { sCode, sCode, sdName, Math.Round(dPrice, 3).ToString("N2"), dDiscount.ToString(), avoid_point.ToString()
				, dQty.ToString(), m_dRowTotal.ToString("N2"), "x", sName, sNameCN, is_promotion, dNormalPrice.ToString(), ""
//				, sBarcode, has_scale.ToString(), sBarcode};
				, sBarcode, has_scale.ToString(), promo_id};
                int insert_row_number = 0;
                if (m_iPromotionItemIndex != 0)
                    insert_row_number = m_iPromotionItemIndex;
                cart.Rows.Insert(insert_row_number, row);
				if (is_promotion.ToString() == "true")
					cart.CurrentCell = cart.Rows[0].Cells[1];
				//string[] rowad = { sdName, Math.Round(dPrice, 2).ToString("N2"), "", dQty.ToString(), m_dRowTotal.ToString("N2") };
				string[] rowad = { sdName, Math.Round(dPrice, 2).ToString("N2"), dDiscount.ToString(), dQty.ToString(), m_dRowTotal.ToString("N2") };
                m_fmad.cart.Rows.Insert(insert_row_number, rowad);
				m_fmad.cart.Rows[0].Selected = true;
                m_fmads.cart.Rows.Insert(insert_row_number, rowad);
				m_fmads.cart.Rows[0].Selected = true;
                m_iPromotionItemIndex = 0;
			}
			cart.CurrentCell = cart.Rows[0].Cells[1];
			cart.Update();
			if (m_bExistingMember)
			{
				if (Program.m_bEnableLevelPrice)
					DoLevelPrice();
				else if (Program.m_bEnableVipDis)
					DoDiscount();
			}
			sShowItemTotal();
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
			this.barcode.Focus();
			if (m_mod != null)
			{
				string ss = sdName + "," + Math.Round(dPrice, 3).ToString("N2") + ", QTY: " + dQty.ToString();
				if (ss.Length < 40)
				{
					ss = ss + "                                                  ";
					ss = ss.Substring(0, 40);
				}
				else
					ss = ss.Substring(0, 40);
				m_mod.SendCmd(ss);
			}
		}
		private void cart_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == cc_del.Index)
			{
				if (m_sDeleteItemControl == "1")
				{
					if (!AdminOK("delete item"))
						return;
					m_sAdminAction = "delete_item";
					DoAdminAction();
				}
				else
				{
					//					DeleteCartItem(-1);
					DeleteCartItem_old(-1);
				}
			}
		}
		private void DeleteCartItem(int nIndex)
		{
			string dAmount = "";
			if (nIndex == -1) //delete current selection
			{
				if (cart.CurrentRow != null)
				{
					nIndex = cart.CurrentRow.Index;
					dAmount = cart.Rows[nIndex].Cells["cc_total"].Value.ToString();
					cart.Rows.RemoveAt(nIndex);
					m_fmad.cart.Rows.RemoveAt(nIndex);
					m_fmads.cart.Rows.RemoveAt(nIndex);
				}
			}
			else //delete nIndex item
			{
				dAmount = cart.Rows[nIndex].Cells["cc_total"].Value.ToString();
				cart.Rows.RemoveAt(nIndex);
				m_fmad.cart.Rows.RemoveAt(nIndex);
				m_fmads.cart.Rows.RemoveAt(nIndex);
			}

			//			Program.RecordTillData("total_cancel_sales", dAmount);
			//			Program.RecordTillData("total_void", "1");
			Program.RecordVoidLog(dAmount, SalesName.Text);

			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart, false);
			CheckGroupPromotion();
            CheckComboPromotion();
			barcode.Focus();
			if (cart.Rows.Count >= 1)
				cart.CurrentCell = cart.Rows[0].Cells[1];
		}

		private void DeleteCartItem_old(int nIndex)
		{
			string code = "";
			string name = "";
			string name_cn = "";
			string qty = "1";
			string dAmount = "";
			if (CheckLatipayGiftCardItemInCart(nIndex, false))
			{
				MessageBox.Show("Gift card cannot be deleted, please go to the gift card interface to cancel the relevant gift card.", "prompt");
				return;
			}
			if (nIndex == -1) //delete current selection
			{
				if (cart.CurrentRow != null)
				{
					nIndex = cart.CurrentRow.Index;
					code = cart.Rows[nIndex].Cells["cc_code"].Value.ToString();
					name = cart.Rows[nIndex].Cells["cc_name_en"].Value.ToString();
					name_cn = cart.Rows[nIndex].Cells["cc_name_cn"].Value.ToString();
					qty = cart.Rows[nIndex].Cells["cc_qty"].Value.ToString();
					dAmount = cart.Rows[nIndex].Cells["cc_total"].Value.ToString();

					cart.Rows.RemoveAt(nIndex);
					m_fmad.cart.Rows.RemoveAt(nIndex);
					m_fmads.cart.Rows.RemoveAt(nIndex);
				}
			}
			else //delete nIndex item
			{
				code = cart.Rows[nIndex].Cells["cc_code"].Value.ToString();
				name = cart.Rows[nIndex].Cells["cc_name_en"].Value.ToString();
				name_cn = cart.Rows[nIndex].Cells["cc_name_cn"].Value.ToString();
				qty = cart.Rows[nIndex].Cells["cc_qty"].Value.ToString();
				dAmount = cart.Rows[nIndex].Cells["cc_total"].Value.ToString();
                m_sDeleteItemBarcode = cart.Rows[nIndex].Cells["cc_barcode"].Value.ToString();
				cart.Rows.RemoveAt(nIndex);
				m_fmad.cart.Rows.RemoveAt(nIndex);
				m_fmads.cart.Rows.RemoveAt(nIndex);
			}
			RecordDelItem(code, name, name_cn, qty, dAmount, m_nSalesId.ToString());
			Program.RecordTillData("total_cancel_sales", dAmount);
			Program.RecordTillData("total_void", "1");
			Program.RecordVoidLog(dAmount, SalesName.Text);

			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
			CheckGroupPromotion();
            CheckComboPromotion();
			barcode.Focus();
            m_sDeleteItemBarcode = "";
			if (cart.Rows.Count >= 1)
				cart.CurrentCell = cart.Rows[0].Cells[1];
		}
		private void RecordDelItem(string code, string name, string name_cn, string qty, string amount, string sales)
		{
			string sc = "";
			sc = "Insert Into delete_item (till_number, code, name,name_cn, qty, amount, sales) ";
			sc += " VALUES(" + Program.m_sStationID;
			sc += ", " + code;
			sc += ", N'" + name + "'";
			sc += ", N'" + name_cn + "'";
			sc += ", N'" + qty + "'";
			sc += ", N'" + Program.MyMoneyParse(amount).ToString() + "'";
			sc += ", N'" + sales + "'";
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
				myConnection.Close();
				Program.ShowExp(sc, e);
				return;
			}
		}
		private double CalcCartTotal()
		{
			double dItems = 0;
            double dQtys = 0;
			m_dTotal = 0;
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				//				double dPrice = Program.MyMoneyParse(cart.Rows[i].Cells["cc_price"].Value.ToString());
				double dQty = Program.MyDoubleParse(cart.Rows[i].Cells["cc_qty"].Value.ToString());
				double dRowTotal = Program.MyDoubleParse(cart.Rows[i].Cells["cc_total"].Value.ToString());
                string sCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
				m_dTotal += dRowTotal;

                if (sCode == "-1001")
                    continue;
                dItems = dItems +1;
                dQtys += dQty;
			}
			if (m_dVolumnSumPrice > 0)
				m_dTotal -= m_dVolumnSumPrice;
			m_dOrderTotal = m_dTotal;
			UpdateTotalPriceDiscplay();
			m_dTotal = Math.Round(m_dTotal, 2);
			showItems.Text = dItems.ToString();
            lblQtys.Text = dQtys.ToString();

			return m_dTotal;
		}
		private void UpdateDtCart(int nIndex)
		{
			UpdateDtCart(nIndex, true);
		}
		private void UpdateDtCart(int nIndex, bool bResetCart)
		{
            m_iPromotionItemIndex = 0;
			DataTable dt = dsCart.Tables[nIndex];
			dt.Rows.Clear();
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				DataRow dr = dt.NewRow();
			    //dr["barcode"] = cart.Rows[i].Cells["cc_barcode"].Value.ToString();
				dr["code"] = cart.Rows[i].Cells["cc_code"].Value.ToString();
				dr["supplier_code"] = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
				dr["name"] = cart.Rows[i].Cells["cc_name_en"].Value.ToString();
				dr["name_en"] = cart.Rows[i].Cells["cc_name"].Value.ToString();
				dr["name_cn"] = cart.Rows[i].Cells["cc_name_cn"].Value.ToString();
				dr["price"] = cart.Rows[i].Cells["cc_price"].Value.ToString();
				dr["discount"] = cart.Rows[i].Cells["cc_discount"].Value.ToString();
				dr["avoid_point"] = cart.Rows[i].Cells["cc_avoid_point"].Value.ToString();
				dr["qty"] = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				//dr["total"] = cart.Rows[i].Cells["cc_total"].Value.ToString();
                dr["total"] = Math.Round(Program.MyDoubleParse(cart.Rows[i].Cells["cc_total"].Value.ToString()), 2).ToString("N2");
				//dr["subTotal"] = m_dTotal.ToString();
                dr["subTotal"] = Math.Round(Program.MyDoubleParse(m_dTotal.ToString()), 2).ToString("N2");
				dr["cardId"] = m_nCardId.ToString();
				dr["cardName"] = m_sCardName;
				dr["cardBarcode"] = m_sCardBarcode;
				dr["is_promotion"] = cart.Rows[i].Cells["cc_is_promotion"].Value.ToString();
				dr["normal_price"] = cart.Rows[i].Cells["cc_normalprice"].Value.ToString();
				if (cart.Rows[i].Cells["cc_promotion_group_id"].Value != null)
					dr["promotion_group_id"] = cart.Rows[i].Cells["cc_promotion_group_id"].Value.ToString();
				//dr["is_package"] = cart.Rows[i].Cells["cc_is_package"].Value.ToString();

				dt.Rows.Add(dr);
			}
			if (cart.Rows.Count == 0 && bResetCart)
				ResetCart();
			SetHoldButtonStatus();
		}
		private void RestoreHoldOrder(int nIndex)
		{
			if (nIndex >= 4)
				return;
			ResetPaymentLabels();
			ResetCart();
			DataTable dt = dsCart.Tables[nIndex];
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				DataRow dr = dt.Rows[i];
				string price = Math.Round(Program.MyDoubleParse(dr["price"].ToString()), 2).ToString();
				string mSuppCode = dr["supplier_code"].ToString();
				string qty = Math.Round(Program.MyDoubleParse(dr["qty"].ToString()), 2).ToString();
				string total = Math.Round(Program.MyDoubleParse(dr["total"].ToString()), 2).ToString();
				string IsAvoidPointItem = dr["avoid_point"].ToString();
				bool bIsAvoidPointItem = bool.Parse(IsAvoidPointItem);

                string sBarcode = dr["promotion_group_id"].ToString();
                string is_promotion = dr["is_promotion"].ToString();

				bool bhas_scale = Program.MyBooleanParse(dr["has_scale"].ToString());

				double dNormalPrice = GetNormalPrice(dr["code"].ToString());
				if (dNormalPrice == 0)
					dNormalPrice = Program.MyDoubleParse(price);
				if (dNormalPrice < Program.MyDoubleParse(price))
					dNormalPrice = Program.MyDoubleParse(price);
				string[] sdr = { dr["code"].ToString(), dr["supplier_code"].ToString(), dr["name_en"].ToString(), price, dr["discount"].ToString()
					, bIsAvoidPointItem.ToString(), qty, total, "x", dr["name"].ToString(), dr["name_cn"].ToString(), is_promotion
					, dNormalPrice.ToString(), "", sBarcode, bhas_scale.ToString() ,sBarcode};
				cart.Rows.Add(sdr);
				string[] sdrad = { dr["name"].ToString(), price, dr["discount"].ToString(), qty, total };
				m_fmad.cart.Rows.Add(sdrad);
				m_fmads.cart.Rows.Add(sdrad);
				if (i == 0)
				{
					m_dTotal += Math.Round(Program.MyDoubleParse(dr["subTotal"].ToString()), 2);
					UpdateTotalPriceDiscplay();
					m_nCardId = Program.MyIntParse(dr["cardId"].ToString());
					m_sCardName = dr["cardName"].ToString();
					MemberShipID.Text = dr["cardBarcode"].ToString();
					MemberShipName.Text = dr["cardName"].ToString();
				}
				if (Program.MyDoubleParse(m_sDiscount[nIndex + 1]) > 0)
				{
					//MessageBox.Show(m_sDiscount[nIndex + 1].ToString());
					this.msgboard.Text = (Program.MyDoubleParse(m_sDiscount[nIndex + 1]) / 100).ToString("p") + " Total Discount Applied";
				}

				if (CheckSpecialItem("is_special", mSuppCode))
					cart.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
				if (CheckSpecialItem("is_member_only", mSuppCode))
					cart.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.GreenYellow;
			}
			this.MemberShipID.Text = m_membernumber[nIndex];
			this.labelPoints.Text = m_memberpoint[nIndex];
			m_nCurrentCart = nIndex;
			SetHoldButtonStatus();
		}
		private void cart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			int n = e.RowIndex;
			string code = cart.Rows[n].Cells["cc_code"].Value.ToString();
			double dPrice = Program.MyMoneyParse(cart.Rows[n].Cells["cc_price"].Value.ToString());
			double dQty = Program.MyMoneyParse(cart.Rows[n].Cells["cc_qty"].Value.ToString());
			double dTotal = Math.Round(dPrice * dQty, 2);
			cart.Rows[n].Cells["cc_total"].Value = dTotal.ToString();
			m_fmad.cart.Rows[n].Cells["cc_price"].Value = dPrice.ToString();
			m_fmad.cart.Rows[n].Cells["cc_qty"].Value = dQty.ToString();
			m_fmad.cart.Rows[n].Cells["cc_total"].Value = dTotal.ToString();

			m_fmads.cart.Rows[n].Cells["cc_price"].Value = dPrice.ToString();
			m_fmads.cart.Rows[n].Cells["cc_qty"].Value = dQty.ToString();
			m_fmads.cart.Rows[n].Cells["cc_total"].Value = dTotal.ToString();

			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);

			timer4.Interval = 100;
			m_sTimer4Task = "check_qty_promotion";
			m_sTimer4Code = code;
			m_dTimer4Qty = dQty;
			m_nTimer4Index = n;
			timer4.Start();

			m_bCheckGroupPromotion = true; //set flag to check in timer
            m_bCheckComboPromotion = true;
		}
		private string ShowHoldedTables()
		{
			int n = 0;
			for (int t = 0; t < 4; t++)
			{
				if (dsCart.Tables[t].Rows.Count > 0)
					n++;
			}
			return n.ToString() + " - " + (m_sCurrentCartID + 1);
		}
		private void SetHoldButtonStatus()
		{
			//set background image
			if (dsCart.Tables[0].Rows.Count > 0)
				buttonCart1.BackgroundImage = global::QPOS2008.Properties.Resources.green;
			else
				buttonCart1.BackgroundImage = global::QPOS2008.Properties.Resources.blue;

			if (dsCart.Tables[1].Rows.Count > 0)
				buttonCart2.BackgroundImage = global::QPOS2008.Properties.Resources.green;
			else
				buttonCart2.BackgroundImage = global::QPOS2008.Properties.Resources.blue;

			if (dsCart.Tables[2].Rows.Count > 0)
				buttonCart3.BackgroundImage = global::QPOS2008.Properties.Resources.green;
			else
				buttonCart3.BackgroundImage = global::QPOS2008.Properties.Resources.blue;

			if (dsCart.Tables[3].Rows.Count > 0)
				buttonCart4.BackgroundImage = global::QPOS2008.Properties.Resources.green;
			else
				buttonCart4.BackgroundImage = global::QPOS2008.Properties.Resources.blue;

			//set foreground color
			buttonCart1.ForeColor = sdcForeOrg;
			buttonCart2.ForeColor = sdcForeOrg;
			buttonCart3.ForeColor = sdcForeOrg;
			buttonCart4.ForeColor = sdcForeOrg;
			switch (m_nCurrentCart)
			{
				case 0:
					buttonCart1.ForeColor = sdcForeHigh;
					break;
				case 1:
					buttonCart2.ForeColor = sdcForeHigh;
					break;
				case 2:
					buttonCart3.ForeColor = sdcForeHigh;
					break;
				case 3:
					buttonCart4.ForeColor = sdcForeHigh;
					break;
				default:
					break;
			}
		}
		private void DoShowHelp()
		{
			string s = "";
			for (int i = 0; i < Program.m_dtHotkey.Rows.Count; i++)
			{
				DataRow dr = Program.m_dtHotkey.Rows[i];
				string name = dr["name"].ToString();
				string skey = dr["key_desc"].ToString();
				if (skey == "")
					continue;
				skey = skey.PadRight(30, '-');
				s += skey + " " + name + "\r\n";
			}
			MessageBox.Show(s, "EZNZ QPOS2008 - Hotkey", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
		}
		private void qty_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!Program.IsNumberKey(e.KeyChar))
				e.Handled = true;
		}
		private void price_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!Program.IsNumberKey(e.KeyChar))
				e.Handled = true;
		}
		private void Cash_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(Cash.Text == "")
				return;
			if(Cash.SelectionLength == Cash.Text.Length)
				return;
			int p = Cash.Text.IndexOf(".");
			if(p >= 0)
			{
				if(Cash.Text.Length - p >= 2)
					e.Handled = true; //disable 2nd digits
			}
		}
		private void Eftpos_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!Program.IsNumberKey(e.KeyChar))
				e.Handled = true;
		}
		private void Cashout_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!Program.IsNumberKey(e.KeyChar))
			{
				e.Handled = true;
			}
			else if(e.KeyChar != '\b') //backspace
			{
				string s = Cashout.Text;
				int p = s.IndexOf('.');
				if(p > 0 && p <= s.Length - 2)
				{
					e.Handled = true;
					return;
				}
			}
		}
		private void Credit_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!Program.IsNumberKey(e.KeyChar))
				e.Handled = true;
		}
		private void Cheque_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!Program.IsNumberKey(e.KeyChar))
				e.Handled = true;
		}
		private void ShowAddBarcodeWindow()
		{
		}
		private void OnFormKeyDown(object sender, KeyEventArgs e)
		{
			if (this.lblprocess.Text != "")
				return;
			m_fmad.TotalPrice.Text = this.labelBalance.Text;
			m_fmads.TotalPrice.Text = this.labelBalance.Text;
			if (e.KeyValue == 17)
			{
				if (!panelCheckout.Visible)
					DoScanBarcode(false);
				return;
			}
			switch (e.KeyCode)
			{
				case Keys.Return:
					if (m_sCode != "" || barcode.Text != "") // Modified by CH 13/11/08 REMOVE TEH BUG OF DUPLICATED QTY BY PRESS ENTER AFTER SEARCH
						DoScanBarcode(false);
					return;
				case Keys.F5:
					if(!Cash.Focused)
						return;
					FormCurrency fmc = new FormCurrency();
					fmc.m_dOrderTotal = m_dOrderTotal;
					fmc.ShowDialog();
					if(fmc.m_dPaymentTotal != 0)
					{
						Cash.Text = fmc.m_dPaymentTotal.ToString();
						m_lastFocused = Cash;
						Cash.Select();
						Cash.Focus();
						return;
					}
					break;
				case Keys.F8:
					DisplayNotePanel();
					break;
				case Keys.F12:
					if (e.Control && e.Shift && e.Alt)
					{
						FormConfig fm = new FormConfig();
						fm.ShowDialog();
						FormConfig_Show = true;
//						myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
						return;
					}
					break;
				case Keys.F11:
					if (e.Control && e.Shift && e.Alt)
					{
						FormSQL fm = new FormSQL();
						fm.ShowDialog();
						return;
					}
					break;
				case Keys.F10:
					if (e.Control && e.Shift && e.Alt && Program.m_bEnableSelfService)
					{
						this.flAdminBtns.Visible = false;
						if (!AdminOK("adminzone"))
							return;

						m_sVipAmounts = "";
						m_sVipInvoices = "";
						m_sVipPaymentTypes = "";
						m_dVipPayAmount = 0;
						btncompanyok AP = new btncompanyok();
						AP.Owner = this;
						AP.BringToFront();
						AP.ShowDialog();
						AP.BringToFront();

						if (AP.m_bPay)
						{
							m_sVipAmounts = AP.m_sAmounts;
							m_sVipInvoices = AP.m_sInvoices;
							m_dVipPayAmount = AP.m_dPayAmount;
							m_sCardId = AP.m_sVipCardId;
							MemberShipID.Text = AP.lblVABarcode.Text;
							DoVipPayment();
							return;
						}
						CachePromotion();
						GetPromotion();
						RepositionPanels();
						BuildMenuButtons();
						if (Program.m_bEnableSelfService)
							SelfServiceModelMain();
					}
					break;
				case Keys.Escape:
					if (e.Control && e.Shift && e.Alt && Program.m_bEnableSelfService)
					{
						DoSignOff();
					}
					if (qty.Text != "")
						this.qty.Text = "";
					this.barcode.Focus();
					this.barcode.Select();
					this.discountswtich.Text = "2";
					break;
				default:
					break;
			}
			string code_name = Program.LookupHotKey(e);
			switch (code_name)
			{
				case "add_barcode":
					this.barcode.Text = "";
					ShowAddBarcodeWindow();
					break;
				case "scan_barcode":
					this.barcode.Focus();
					this.discountswtich.Text = "2";
					if (m_dDiscRate == 0)
						this.msgboard.Text = "";
					ResetPayment();  // RESET PAYMENT WHEN SCANNING ITEM CH 12/11/08
					//GoPayment();
					break;
				case "search":
					DoSearchItem();
					this.barcode.Focus();
					this.barcode.Text = "";
					break;
				case "print_x_total":
					DoPrintXTotal();
					barcode.Focus();
					break;
				case "print_z_total":
					DoPrintZTotal();
					barcode.Focus();
					break;
				case "order1":
					if (m_nCurrentCart == 0)
						break;
					RestoreHoldOrder(0);
					m_sCurrentCartID = 0;
					break;
				case "order2":
					if (m_nCurrentCart == 1)
						break;
					RestoreHoldOrder(1);
					m_sCurrentCartID = 1;
					break;
				case "order3":
					if (m_nCurrentCart == 2)
						break;
					RestoreHoldOrder(2);
					m_sCurrentCartID = 2;
					break;
				case "order4":
					if (m_nCurrentCart == 3)
						break;
					RestoreHoldOrder(3);
					m_sCurrentCartID = 3;
					break;
				case "weight":
					DoWeight();
					break;
				case "open_cashdraw":
					if (!AdminOK("refund"))
						return;
					m_sAdminAction = "open_draw";
					DoAdminAction();
					break;
				case "charge":
					RefreshPayment("payment_charge");
					m_sPosRefCurrent = m_sPosRef;
					nextPosRef(int.Parse(m_sPosRefCurrent));
					charge.Text = GetBalance();
					charge.Focus();
					charge.Select();
					labelBalance.Text = charge.Text;
					m_fmad.TotalPrice.Text = this.labelBalance.Text;
					m_fmads.TotalPrice.Text = this.labelBalance.Text;
					//GoPayment();
					break;
				case "payment_cash":
					RefreshPayment("payment_cash");
					double dGetBanlance = Program.MyDoubleParse(GetBalance());
					if (m_dTotalPaid == 0)
					{
						if (m_dTotal < 0)
							this.Cash.Text = Math.Round(m_dTotal, 1).ToString();
						else
							this.Cash.Text = Program.RoundCents(dGetBanlance, m_sRoundingNum).ToString();
					}
					else
						this.Cash.Text = Program.RoundCents(dGetBanlance, m_sRoundingNum).ToString();// Cash.Text = Math.Round(dGetBanlance, 1).ToString();
					labelBalance.Text = Cash.Text;
					Cash.Focus();
					Cash.Select();
					m_fmad.TotalPrice.Text = this.labelBalance.Text;
					m_fmads.TotalPrice.Text = this.labelBalance.Text;
					break;
				case "payment_cc":
					RefreshPayment("payment_cc");
					m_sPosRefCurrent = m_sPosRef;
					nextPosRef(int.Parse(m_sPosRefCurrent));
					Credit.Text = GetBalance();
					Credit.Focus();
					Credit.Select();
					labelBalance.Text = Credit.Text;
					m_fmad.TotalPrice.Text = this.labelBalance.Text;
					m_fmads.TotalPrice.Text = this.labelBalance.Text;
					//GoPayment();
					break;
				case "payment_eftpos":
					if (m_bCashOutReminder)
					{
						FormMSG cashout = new FormMSG();
						cashout.btnNo.Visible = false;
						cashout.btnYes.Visible = false;
						cashout.m_sMsg = "Need CashOut?";
						cashout.ShowDialog();
					}
					RefreshPayment("payment_eftpos");
					m_sPosRefCurrent = m_sPosRef;
					nextPosRef(int.Parse(m_sPosRefCurrent));
					Eftpos.Text = GetBalance();
					Eftpos.Focus();
					Eftpos.Select();
					labelBalance.Text = Eftpos.Text;
					m_fmad.TotalPrice.Text = this.labelBalance.Text;
					m_fmads.TotalPrice.Text = this.labelBalance.Text;
					break;
				case "payment_cheque":
					RefreshPayment("payment_cheque");
					Cheque.Text = GetBalance();
					Cheque.Focus();
					Cheque.Select();
					labelBalance.Text = Cheque.Text;
					m_fmad.TotalPrice.Text = this.labelBalance.Text;
					m_fmads.TotalPrice.Text = this.labelBalance.Text;
					break;
				case "payment_cashout":
					Cashout.Focus();
					break;
				case "logoff":
					DoSignOff();
					break;
				case "print_receipt":
					if (m_sLastReceipt != "")
					{
						sPrintLastReceipt = m_sLastReceipt;
						if (m_sCurrentCartID == 0)
							sPrintLastReceipt = m_sLastCart1;
						else if (m_sCurrentCartID == 1)
							sPrintLastReceipt = m_sLastCart2;
						else if (m_sCurrentCartID == 2)
							sPrintLastReceipt = m_sLastCart3;
						else if (m_sCurrentCartID == 3)
							sPrintLastReceipt = m_sLastCart4;
						m_sPrintBuffer = sPrintLastReceipt; //"[b]Cash Draw Opened:[/b] Station:" + Station.Text + " Date/Time:" + DateTime.Now.ToString("dd-MM-yyyy") + DateTime.Now.ToString("HH:mm");
						printDoc.Print();
					}
					this.barcode.Focus();
					this.barcode.Text = "";
					break;
				case "help":
					this.barcode.Text = "";
					DoShowHelp();
					break;
				case "discount":
					if (!sAllowDiscountRollBack)
						m_bBeDisced = true;
					ShowDiscountPanel("", "", "");
					barcode.Focus();
					break;
				case "toggle_chinese":
					ENCNSwitch();
					break;
				case "refund_mode":
					if (cart.Rows.Count > 0)
					{
						if (!AdminOK("refund"))
							return;
						m_sAdminAction = "refund";
						DoAdminAction();
					}
					break;
				case "delete_current_order":
					DeleteCurrentOrder();
					barcode.Focus();
					break;
				case "delete_order_item":
					if (cart.Rows.Count > 0)
					{
						int iSelectedRowed = cart.CurrentRow.Index;
						if (!cart.CurrentRow.Selected)
							iSelectedRowed = 0;
						//						DeleteCartItem(iSelectedRowed);
						DeleteCartItem_old(iSelectedRowed);
					}
					else
					{
						FormMSG fError = new FormMSG();
						fError.btnYes.Visible = false;
						fError.btnNo.Visible = false;
						fError.m_sMsg = " Nothing to delete";
						fError.ShowDialog();
						this.barcode.Focus();
						//ResetPayment();
					}
					break;
				case "clear_payment":
					string s_cPasswordControl = m_sClearPaymentControl;
					if (s_cPasswordControl == "1")
					{
						if (!AdminOK(""))
							return;
						m_sAdminAction = "clear_payment";
						DoAdminAction();
					}
					else
					{
						ResetPayment();
						Cash.Focus();
					}
					break;
				case "multiply_qty_key": //////tee: 21/11/08
					if (cart.Rows.Count >= 0)
					{
						SetCurrentQTY();
						this.barcode.Text = "";
					}
					break;
				case "change_qty":
					if (cart.Rows.Count > 0)
					{
						int iSelectedRow = cart.CurrentRow.Index;
						if (!cart.CurrentRow.Selected)
							iSelectedRow = 0;
						//						doChangeQty(iSelectedRow);
					}
					this.barcode.Text = "";
					break;
				case "scan_member": //////tee: 21/11/08
					m_bScanMember = true;
					this.MemberShipID.Focus();
					break;
				case "item_discount":
					this.discountswtich.Text = "1";
					int cIndex = cart.CurrentRow.Index;
					if (!cart.CurrentRow.Selected)
						cIndex = 0;
					ShowDiscountPanel(cart.Rows[cIndex].Cells["cc_supplier_code"].Value.ToString(), cart.Rows[cIndex].Cells["cc_name"].Value.ToString(), cart.Rows[cIndex].Cells["cc_price"].Value.ToString());
					//					DoItemDiscount(cIndex);
					break;
				case "reprint_total":
					if (!AdminOK(""))
						return;
					m_sAdminAction = "reprint_total";
					DoAdminAction();
					break;
				default:
					break;
			}
		}
		private void cart_KeyDown(object sender, KeyEventArgs e)
		{
			OnFormKeyDown(sender, e);
		}
		private void OnChangeKeyDown(object sender, KeyEventArgs e)
		{
			OnFormKeyDown(sender, e);
		}
		private void OnBarcodeKeyDown(object sender, KeyEventArgs e)
		{
			string s_sBarcode = this.barcode.Text;
			if (e.KeyValue == 13 || e.KeyValue == 17)
			{
				if ((s_sBarcode == Program.m_sgroceryweight || Program.m_sgroceryitem.IndexOf(s_sBarcode).ToString() != "-1") && this.barcode.Text.IndexOf("*").ToString() == "-1" && this.barcode.Text != "")//Key In Price
				{
					if (Program.m_svoidscale != "1")
						this.qty.Text = "1";
					this.price.Focus();
					return;
				}
			}
			this.txtpaymentinfo.Text = "";
			OnFormKeyDown(sender, e);
			if (Change.Text != "")
			{
				m_fmad.showchange.Text = "";
				m_fmad.labelChange.Text = "";
				m_fmads.showchange.Text = "";
				m_fmads.labelChange.Text = "";
				showcashout.Text = "";
				labelcashout.Text = "";
				Change.ForeColor = System.Drawing.Color.Red;
			}
		}
		private void qty_KeyDown(object sender, KeyEventArgs e)
		{
			OnFormKeyDown(sender, e);
		}
		private void price_KeyDown(object sender, KeyEventArgs e)
		{
			OnFormKeyDown(sender, e);
		}
		private void DoSearchItem()
		{
			FormSearch fm = new FormSearch();
			this.barcode.Text = "";
			fm.ShowDialog();

			string barcode = fm.m_sCode;
			this.barcode.Text = barcode;
			this.m_sCode = fm.m_sCode;
			if (m_sCode != "")
				DoScanBarcode(false);
			else
				this.barcode.Focus();
		}
		private void MemberShipID_KeyDown(object sender, KeyEventArgs e)
		{
			m_bScanMember = true;
			if (e.KeyValue == 17)
			{
				if (MemberShipID.Text == "")
					this.barcode.Focus();
				else
				{
					if (ScanMembership())
					{
						b_CheckPriceMemberOnly();
						bMSSpecialCheck();
						this.barcode.Focus();
					}
					else
						this.MemberShipID.Focus();
				}
				return;
			}

			switch (e.KeyCode)
			{
				case Keys.Return:
					if (MemberShipID.Text == "")
					{
						barcode.Focus();
					}
					else
					{
						if (ScanMembership())
						{
							b_CheckPriceMemberOnly();
							bMSSpecialCheck();
							this.barcode.Focus();
						}
						else
							this.MemberShipID.Focus();
					}
					break;
				default:
					OnFormKeyDown(sender, e);
					break;
			}

			this.charge.Text = "";
		}
		private bool ScanMembership()
		{
			lblsearchvip.Visible = false;

			string id = MemberShipID.Text;
			if (id == "")
				return false;
			if (dst.Tables["sales"] != null)
				dst.Tables["sales"].Clear();
			string sc = " SELECT id, name ";
            if(m_bVipVoucherEnabled)
                sc += ", points";
            sc += ", has_expired, ISNULL(expired_date,GETDATE()) AS expired_date, m_type ";
			sc += ", m_discount_rate, trading_name, gst_rate, price_level ";
			sc += " FROM card WHERE type in (1,6) AND (barcode ='" + id + "')";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "sales");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return false;
			}
			if (dst.Tables["sales"].Rows.Count > 0)
			{
				m_bMemberDiscount = true;
				m_bExistingMember = true;
				DataRow dr = dst.Tables["sales"].Rows[0];
				MemberShipName.Text = dr["name"].ToString();
                if (m_bVipVoucherEnabled)
				    labelPoints.Text = dr["points"].ToString();
				double dDiscRate = Program.MyDoubleParse(dr["m_discount_rate"].ToString());
				if (dDiscRate > 1)
					dDiscRate = dDiscRate /100;
				m_sMemberDiscountRate = dDiscRate.ToString();
				m_sMemberPriceLevel = dr["price_level"].ToString();
				m_bHasMemberExpired = Program.MyBooleanParse(dr["has_expired"].ToString());
				dCustomerGst = Program.MyDoubleParse(dr["gst_rate"].ToString());
				//Calculate Expired Date
				DateTime CurrentTime = DateTime.Now;
				m_sMemberShipExpiredDate = DateTime.Parse(dr["expired_date"].ToString());
				TimeSpan span = CurrentTime.Subtract(m_sMemberShipExpiredDate);
				if (m_bHasMemberExpired)
				{
					if (span.Days >= -6 && span.Days <= 0)
					{
						FormMSG MemberGoingExpire = new FormMSG();
						MemberGoingExpire.btnNo.Visible = false;
						MemberGoingExpire.btnYes.Visible = false;
						if (span.Hours < -24)
							MemberGoingExpire.m_sMsg = "This membership is going to " + "\r\nexpire in " + (0 - span.Days).ToString() + " days " + m_sMemberShipExpiredDate.ToString("dd-MM-yyyy");
						else
							MemberGoingExpire.m_sMsg = "This membership is going \r\n to expire on " + m_sMemberShipExpiredDate.ToString("dd-MM-yyyy");
						MemberGoingExpire.ShowDialog();
						this.barcode.Focus();
					}
					else if (span.Days >= 1)
					{
						FormMSG MembershipExpired = new FormMSG();
						MembershipExpired.btnNo.Visible = false;
						MembershipExpired.btnYes.Visible = false;
						MembershipExpired.m_sMsg = "This membership expired\r\nNo discount for this puchase";
						MembershipExpired.ShowDialog();
						//                        m_bMemberExpired = true;
						this.barcode.Focus();
						this.MemberShipID.Text = "";
						return false;
					}
				}
				m_sCardBarcode = id;
				m_nCardId = Program.MyIntParse(dr["id"].ToString());
				m_sCardName = dr["name"].ToString();
				UpdateDtCart(m_nCurrentCart, false); //save membership info, do not reset cart on first scan
				this.discountswtich.Text = "2";
				// if (Program.MyBooleanParse(sPasswordControlTotalDiscount))

				if (Program.m_bEnableLevelPrice)
					DoLevelPrice();
				else if (Program.m_bEnableVipDis)
					DoDiscount();

				m_bExistingMember = true;
				m_bMemberDiscount = false;
				if(Program.m_bVoucherUseWebService)
				{
					DoCreateVoucherService(id);
				}
				return true;
			}
			else
			{
				FormMSG MembershipNotFound = new FormMSG();
				MembershipNotFound.btnYes.Visible = false;
				MembershipNotFound.btnNo.Visible = false;
				MembershipNotFound.m_sMsg = "This membership Not\r\n Existing";
				MembershipNotFound.ShowDialog();
				MemberShipName.Text = "";
				labelPoints.Text = "";
				m_sCardBarcode = "";
				m_sCardName = "";
				MemberShipID.Text = "";
				MemberShipID.Focus();
				return false;
			}
		}
		private bool UpdateVIPPoints(int nPoints)
		{
			string sc = " UPDATE card SET points = " + nPoints + " WHERE barcode = '" + Program.EncodeQuote(MemberShipID.Text) + "' ";
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
			this.labelPoints.Text = nPoints.ToString();
			return true;
		}

		private void doCheckOrderAmount()
		{
		}
		private void FocusEftpos()
		{
			Change.Text = "";
			Eftpos.Text = GetBalance();
			Eftpos.Focus();
			Eftpos.Select();
		}
		//*** INDIVIDUAL PAYMENT HAS BEEN APPLIED FOR EACH PAYMENT MEHTOD CH 14/11/08 **//
		private void OnCashKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyValue)
			{
				case 17:
					m_dEftpos = Program.MyMoneyParse(Eftpos.Text);
					if (m_dEftpos != 0)
					{
						Eftpos_KeyDown(sender, e);
						return;
					}
					m_dCash = Program.RoundCents(Program.MyDoubleParse(this.Cash.Text), m_sRoundingNum);//Program.MyMoneyParse(Cash.Text);
					if (!doWariningPayment("cash", m_dCash, this.labelBalance.Text))
						return;
					if (TotalPaymentOK())
					{
						doShowPaymentInfo();
						DoCheckout();
						Cash.Text = "";
						labelBalance.Text = "";
					}
					else
					{
						if (m_dCash != 0)
						{
							if (ConfirmOnePayment())
							{
								doShowPaymentInfo();
								break;
							}
						}
						// FocusEftpos();
						this.Cash.Select(); // CH 28/01/2009
						this.Cash.Focus();
					}
					break;
				case 40:
					Cash.Text = "";
					FocusEftpos();
					labelBalance.Text = Eftpos.Text;
					m_fmad.TotalPrice.Text = this.labelBalance.Text;
					m_fmads.TotalPrice.Text = this.labelBalance.Text;
					break;
				case 46:
					Cash.Text = "";
					break;
				default:
					OnFormKeyDown(sender, e);
					break;
			}
		}
		private void DoEftpos(double dPurchase, double dCashOut)
		{
//			EFTPOS eftPanel = new EFTPOS()
			eftPanel.m_bAccepted = false;
			eftPanel.m_sReceipt = "";
			eftPanel.m_sReceiptCust = "";
			eftPanel.m_dAmount = dPurchase;
			eftPanel.m_dCashOut = dCashOut;
			eftPanel.m_dChequeAmount = m_dCheque;
//			eftPanel.m_sRefNum = Program.MyIntParse(Program.m_sStationID).ToString("D3");
			string sRef = DateTime.Now.ToOADate().ToString().Replace(".", "");
//			if (sRef.Length > 8)
//				sRef = sRef.Substring(sRef.Length - 8, 8);
			eftPanel.m_sRefNum = sRef;
//			eftPanel.m_sRefNum = Program.MyIntParse(Program.m_sStationID).ToString("D3");
			Program.g_log.Info("eftpos start, type=" + Program.m_eftposType + ", purchase=" + dPurchase.ToString("c"));
			eftPanel.ShowDialog();
			m_bEftposLog(eftPanel.m_sMonitorData, eftPanel.m_bAccepted);// Eftpos Log
			if (eftPanel.m_bAccepted)
			{

				if (this.Eftpos.Text != "" && this.Credit.Text == "")
					SetEftposTotal();
				else if (this.Credit.Text != "" && this.Eftpos.Text == "")
					SetCreditTotal();
				if (TotalPaymentOK())
				{
					doShowPaymentInfo();
					DoCheckout();
				}
				else
				{
					ConfirmOnePayment();
					doShowPaymentInfo();
				}
//				MessageBox.Show("accepted");
			}
			else
			{
//				MessageBox.Show("not accepted, data=" + eftPanel.m_sMonitorData);
			}
			if (Program.m_eftposType == "ingenico")
			{
				m_bEftposWaiting = true;
				timer2.Interval = m_nEftposWaiting;
				timer2.Start();
			}
		}
		private void Eftpos_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_bEftposWaiting)
			{
				btn_penter.Visible = false;
				//				Eftpos.Text = "";
				FormMSG fmsg = new FormMSG();
				fmsg.btnNo.Visible = false;
				fmsg.btnYes.Visible = false;
				fmsg.m_sMsg = "       Eftpos terminal busy \r\n      please wait a momnent";
				fmsg.m_sbuttonFocus = "0";
				fmsg.ShowDialog();

				//		MessageBox.Show("Eftpos terminal busy, please wait a momnent");
				return;
			}

			m_dEftpos = Program.MyMoneyParse(Eftpos.Text);
			if (!doWariningPayment("eftpos", m_dEftpos, this.labelBalance.Text))
				return;
			switch (e.KeyValue)
			{
				case 17:
					if (this.Cashout.Text != "" && m_dEftpos < Program.MyDoubleParse(this.labelBalance.Text))
					{
						MessageBox.Show("Sorry, Cash Out Only Available At Eftpos Paid In Full", "Payment Error");
						this.Eftpos.Text = this.labelBalance.Text;
						m_dCashout = 0;
						m_dEftpos = 0;
						return;
					}
					if (m_dEftpos != 0)
					{
						double dBalance = Math.Round(m_dTotal - m_dTotalPaid, 2);
						if (Program.m_bEnableEftpos)
						{
							if (m_dEftpos > dBalance)
							{
								FormMSG fm = new FormMSG();
								fm.btnNo.Visible = false;
								fm.btnYes.Visible = false;
								fm.m_sMsg = "Sorry, over charge!";
								fm.ShowDialog();
								return;
							}
							if (m_dTotal < 0)
							{
								double dRefund = Program.MyMoneyParse(Eftpos.Text);
								DoEftpos(dRefund, 0);
								return;
							}
							double dPurchase = m_dEftpos;
							double dCashOut = m_dCashout;
							DoEftpos(dPurchase, dCashOut);
							return;
						}
						FormMSG fm1 = new FormMSG();
						fm1.btnYes.Visible = true;
						fm1.btnNo.Visible = true;
						fm1.btnOK.Visible = false;
						fm1.m_sMsg = "EFTPOS PAYMENT ACCEPTED?";
						fm1.ShowDialog();
						if (!fm1.m_bYes)
							return;
						//						Program.MsgBox("Please make sure EFTPOS PAYMENT ACCEPTED!");
						//						Program.MsgBox("Error, EFTPOS is not enabled!\r\n\r\nTotal=" + m_dTotal + "\r\nEftpos=" + m_dEftpos.ToString() + "\r\nCashout=" + m_dCashout + "\r\nPaid=" + m_dTotalPaid);
						//						return;
					}
					if (m_dEftpos != 0 && m_dEftpos <= Math.Round(m_dTotal - m_dTotalPaid, 2))
					{
						SetEftposTotal();
						if (TotalPaymentOK())
						{
							doShowPaymentInfo();
							DoCheckout();
							Eftpos.Text = "";
							labelBalance.Text = "";
						}
						else
						{
							ConfirmOnePayment();
							doShowPaymentInfo();
						}
					}
					else
					{
						FormMSG fm = new FormMSG();
						fm.btnYes.Visible = false;
						fm.btnNo.Visible = false;
						fm.m_sMsg = "Sorry,over charge!";
						fm.ShowDialog();
						return;
					}
					break;
				case 40:
					// Eftpos.Text = GetBalance(); 
					m_dEftpos = Program.MyDoubleParse(Eftpos.Text);
					Cashout.Focus();
					break;
				case 38:
					//Cash.Text = GetBalance();
					Eftpos.Text = "";
					double dBanlance = Program.MyDoubleParse(GetBalance());
					this.Cash.Text = Program.RoundCents(dBanlance, m_sRoundingNum).ToString();  //Math.Round(dBanlance, 1).ToString(); 
					labelBalance.Text = Cash.Text;
					Cash.Focus();
					m_fmad.TotalPrice.Text = this.labelBalance.Text;
					m_fmads.TotalPrice.Text = this.labelBalance.Text;
					break;
				case 46:
					this.Eftpos.Text = "";
					break;
				default:
					OnFormKeyDown(sender, e);
					break;
			}
		}
		private void Cashout_KeyDown(object sender, KeyEventArgs e)
		{
/*			string s = Cashout.Text;
			int p = s.IndexOf('.');
			if(p > 0 && p <= s.Length - 2)
			{
				e.Handled = true;
				return;
			}
*/			
			double m_dGetCashOut = Program.MyDoubleParse(m_sCashoutcontrol);
			m_dCashout = Program.RoundCents(Program.MyDoubleParse(Cashout.Text), m_sRoundingNum);// Math.Round(Program.MyMoneyParse(Cashout.Text), 2);
			if (!doWariningPayment("cashout", m_dCashout, ""))
				return;
			switch (e.KeyValue)
			{
				case 17:
                    if (this.Cashout.Text == "" && this.Eftpos.Text != "")
                    {
                        FormMSG fmsg = new FormMSG();
                        fmsg.btnNo.Visible = false;
                        fmsg.btnYes.Visible = false;
                        fmsg.m_sMsg = "Please key in cashout amount!";
                        fmsg.ShowDialog();
                        this.Eftpos.Focus();
                        return;
                    }
					//					if (!doWariningPayment("cashout", m_dCashout, ""))
					//						return;
					if (m_dGetCashOut == 0)
					{
						MessageBox.Show("Sorry, No Cash Out", "Cash out Error");
						if (this.Eftpos.Text != "")
						{
							this.Cashout.Text = "";
							m_dCash = 0;
							this.Eftpos.Focus();
							this.Eftpos.Select();
						}
						else if (this.Credit.Text != "")
						{
							this.Cashout.Text = "";
							m_dCash = 0;
							this.Credit.Focus();
							this.Credit.Select();
						}
						return;
					}
					if (m_dEftpos > m_dTotal)
					{
						FormMSG fm = new FormMSG();
						fm.btnNo.Visible = false;
						fm.btnYes.Visible = false;
						fm.m_sMsg = "Sorry, over charge!";
						fm.ShowDialog();
						return;
					}
					if (m_dEftpos == 0 && this.Eftpos.Text == "" && m_dCredit == 0 && this.Credit.Text == "")
					{
						MessageBox.Show("Please Select Payment");
						m_dCashout = 0;
						this.Eftpos.Text = this.labelBalance.Text;
						this.Eftpos.Select();
						this.Eftpos.Focus();
						return;
					}
					if (this.Cashout.Text != "")
					{
						if (this.Credit.Text != "")
						{
							FormMSG fmsg = new FormMSG();
							fmsg.btnNo.Visible = false;
							fmsg.btnYes.Visible = false;
							fmsg.m_sMsg = "       Sorry, No CashOut In \r\n      Credit Card Payment";
							fmsg.m_sbuttonFocus = "0";
							fmsg.ShowDialog();
							this.Cashout.Text = "";
							m_dCashout = 0;
							this.Credit.Select();
							this.Credit.Focus();
							return;
						}
						if (this.Cheque.Text != "")
						{
							FormMSG fmsg = new FormMSG();
							fmsg.btnNo.Visible = false;
							fmsg.btnYes.Visible = false;
							fmsg.m_sMsg = "       Sorry, No CashOut In \r\n      Cheque Payment";
							fmsg.m_sbuttonFocus = "0";
							fmsg.ShowDialog();
							this.Cashout.Text = "";
							m_dCashout = 0;
							this.Cheque.Select();
							this.Cheque.Focus();
							return;
						}
					}
					//					if (m_dCashout > 0)
					if (m_dCashout > 0 && m_dEftpos <= m_dTotal)
					{
						if (Program.m_bEnableEftpos)
						{
							if (m_bEftposWaiting)
							{
								btn_penter.Visible = false;
								FormMSG fmsg = new FormMSG();
								fmsg.btnNo.Visible = false;
								fmsg.btnYes.Visible = false;
								fmsg.m_sMsg = "       Eftpos terminal busy \r\n      please wait a momnent";
								fmsg.m_sbuttonFocus = "0";
								fmsg.ShowDialog();
								return;
							}
							double dPurchase = Program.MyMoneyParse(Eftpos.Text);
							double dCashOut = Program.MyMoneyParse(Cashout.Text);
							DoEftpos(dPurchase, dCashOut);
							return;
						}
						SetEftposTotal();
						Cashout.Focus();
						Cashout.Select();
						if (TotalPaymentOK())
						{
							doShowPaymentInfo();
							DoCheckout();
							Cashout.Text = "";
							labelBalance.Text = "";
						}
					}
					else if (Eftpos.Text == "")
					{
						Change.Text = "";
						Credit.Text = GetBalance();
						Credit.Focus();
					}
					break;
				case 30:
					m_dEftpos = Program.MyMoneyParse(Eftpos.Text);
					m_dCredit = Program.MyMoneyParse(Credit.Text);
					m_dCheque = Program.MyMoneyParse(Cheque.Text);
					if (m_dCredit != 0 || m_dCheque != 0)
					{
						if (m_dCashout > 0)
						{
							FormMSG fmsg = new FormMSG();
							fmsg.btnNo.Visible = false;
							fmsg.btnYes.Visible = false;
							fmsg.m_sMsg = "       Sorry, No CashOut In \r\n      Credit Card or Cheque Payment";
							fmsg.m_sbuttonFocus = "0";
							fmsg.ShowDialog();
							this.Cashout.Text = "";
							m_dCashout = 0;
							this.Credit.Select();
							this.Credit.Focus();
							return;
						}
						m_dEftpos = m_dCredit;
					}
					if (m_dEftpos >= 0 && Program.m_bEnableEftpos && m_dEftpos <= m_dTotal && m_dCheque == 0)
					{
						if (m_bEftposWaiting)
						{
							btn_penter.Visible = false;
							FormMSG fmsg = new FormMSG();
							fmsg.btnNo.Visible = false;
							fmsg.btnYes.Visible = false;
							fmsg.m_sMsg = "       Eftpos terminal busy \r\n      please wait a momnent";
							fmsg.m_sbuttonFocus = "0";
							fmsg.ShowDialog();

							//		MessageBox.Show("Eftpos terminal busy, please wait a momnent");
							return;
						}
						double dPurchase = m_dEftpos;
						double dCashOut = Program.MyDoubleParse(Cashout.Text);
						DoEftpos(dPurchase, dCashOut);
						return;
					}
					else if (m_dEftpos >= 0 && m_dEftpos > m_dTotal)
					{
						FormMSG fm = new FormMSG();
						fm.btnNo.Visible = false;
						fm.btnYes.Visible = false;
						fm.m_sMsg = "Sorry, over charge!";
						fm.ShowDialog();
						return;
					}
					else if (m_dCheque > 0)
					{
						FormMSG fm = new FormMSG();
						fm.btnNo.Visible = false;
						fm.btnYes.Visible = false;
						fm.m_sMsg = "Sorry, No Cashout in Cheque Payment!";
						fm.ShowDialog();
						return;
					}
					SetEftposTotal();
					Cashout.Focus();
					Cashout.Select();
					if (TotalPaymentOK())
					{
						doShowPaymentInfo();
						DoCheckout();
					}
					break;
				case 40:
					Cashout.Text = "";
					Eftpos.Text = "";
					Credit.Text = GetBalance();
					Credit.Focus();
					Credit.Select();
					break;
				case 38:
					Cashout.Text = "";
					Eftpos.Text = GetBalance();
					Eftpos.Focus();
					break;
				case 46:
					this.Cashout.Text = "";
					break;
				default:
					OnFormKeyDown(sender, e);
					break;
			}
		}
		private void Credit_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_bEftposWaiting)
			{
				btn_penter.Visible = false;
				FormMSG fmsg = new FormMSG();
				fmsg.btnNo.Visible = false;
				fmsg.btnYes.Visible = false;
				fmsg.m_sMsg = "       Eftpos terminal busy \r\n      please wait a momnent";
				fmsg.m_sbuttonFocus = "0";
				fmsg.ShowDialog();
				return;
			}
			switch (e.KeyValue)
			{
				case 17:
                    m_dEftpos = Program.MyMoneyParse(Eftpos.Text);
                    if (m_dEftpos != 0)
                    {

                        FormMSG fmsg = new FormMSG();
                        fmsg.btnNo.Visible = false;
                        fmsg.btnYes.Visible = false;
                        fmsg.m_sMsg = "Eftpos Payment is selected! \r\n   Please double check";
                        fmsg.ShowDialog();
                        this.Eftpos.Focus();
                        return;
                    }
					m_dCredit = Program.MyMoneyParse(Credit.Text);
					if (m_dCredit != 0)
					{
						if (!doWariningPayment("credit", m_dCredit, this.labelBalance.Text))
							return;
						if (this.Cashout.Text != "")// && m_dCredit < Program.MyDoubleParse(this.labelBalance.Text))
						{
							FormMSG fmsg = new FormMSG();
							fmsg.btnNo.Visible = false;
							fmsg.btnYes.Visible = false;
							fmsg.m_sMsg = "       Sorry, No CashOut In \r\n      Credit Card Payment";
							fmsg.m_sbuttonFocus = "0";
							fmsg.ShowDialog();
							this.Cashout.Text = "";
							m_dCashout = 0;
							this.Credit.Select();
							this.Credit.Focus();
							return;
						}
						if (Program.m_bEnableEftpos)
						{
							//			if (m_dCredit > Math.Round(m_dTotal - m_dTotalPaid, 2))
							if (m_dCredit > Math.Round(m_dTotal - m_dTotalPaid + m_dSurcharge, 2))
							{
								this.Credit.Text = "";
								FormMSG fm = new FormMSG();
								fm.btnNo.Visible = false;
								fm.btnYes.Visible = false;
								fm.m_sMsg = "Sorry, over charge!";
								fm.ShowDialog();
								return;
							}
							if (Credit.Text != "" && m_dCredit > 0 && m_dCredit <= Math.Round(m_dTotal - m_dTotalPaid + m_dSurcharge, 2))
							{
								double dPurchase = m_dCredit;
								DoEftpos(dPurchase, 0);
								return;
							}
							else if (Credit.Text != "" && m_dTotal < 0)
							{
								double dRefund = Program.MyDoubleParse(Credit.Text);
								DoEftpos(dRefund, 0);
								return;
							}
							else if (Program.MyDoubleParse(Credit.Text) > m_dTotal + m_dSurcharge && Cashout.Text == "" && m_dCashout == 0)
							{
								double dPurchase = m_dTotal;
								double dCashOut = m_dCredit - m_dTotal;
								DoEftpos(dPurchase, dCashOut);
								return;
							}
						}
						if (m_dCredit > Math.Round(m_dTotal - m_dTotalPaid + m_dSurcharge, 2))
						{
							this.Credit.Text = "";
							FormMSG fm = new FormMSG();
							fm.btnNo.Visible = false;
							fm.btnYes.Visible = false;
							fm.m_sMsg = "Sorry, over charge!";
							fm.ShowDialog();
							return;
						}
						SetCreditTotal();
						if (TotalPaymentOK())
						{
							doShowPaymentInfo();
							DoCheckout();
							Credit.Text = "";
							labelBalance.Text = "";
							//Change.Text = "0.00";
						}
						else
						{
							ConfirmOnePayment();
							doShowPaymentInfo();
						}
						//Cheque.Select();
						//Cheque.Focus();
					}
					else
					{
						Change.Text = "";
						Cheque.Text = GetBalance();
						Cheque.Select();
						Cheque.Focus();
					}
					break;
				case 40:
					Credit.Text = "";
					Cheque.Text = GetBalance();
					Cheque.Focus();
					break;
				case 38:
					Credit.Text = "";
					Cashout.Text = "";
					Cashout.Focus();
					break;
				case 46:
					this.Credit.Text = "";
					break;
				default:
					OnFormKeyDown(sender, e);
					break;
			}
		}
		private void Cheque_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyValue)
			{
				case 17:
                    m_dEftpos = Program.MyMoneyParse(Eftpos.Text);
                    m_dCredit = Program.MyMoneyParse(Credit.Text);
                    if (m_dEftpos != 0)
                    {

                        FormMSG fmsg = new FormMSG();
                        fmsg.btnNo.Visible = false;
                        fmsg.btnYes.Visible = false;
                        fmsg.m_sMsg = "Eftpos Payment is selected! \r\n   Please double check";
                        fmsg.ShowDialog();
                        this.Eftpos.Focus();
                        return;
                    }
                    else if (m_dCredit != 0)
                    {

                        FormMSG fmsg = new FormMSG();
                        fmsg.btnNo.Visible = false;
                        fmsg.btnYes.Visible = false;
                        fmsg.m_sMsg = "Credit Payment is selected! \r\n   Please double check";
                        fmsg.ShowDialog();
                        this.Credit.Focus();
                        return;
                    }
					bool bSuccess = false;
					double dGetBanlance = Program.MyDoubleParse(GetBalance());
					if (Program.m_bEnableMyPosMate)
					{
						if (Program.m_sMyPosMateMerchantId == "" || Program.m_sMyPosMateConfigId == "")
						{
							Program.MsgBox("MyPosMate is not set up, please contact your dealer.");
							return;
						}
						else
						{
							double amount = dGetBanlance;
							FormMyPosMate fmpm = new FormMyPosMate();
							fmpm.m_sInvoiceNumber = GetNextInvNumber();
							fmpm.m_dAmount = amount;
							fmpm.m_bSuccess = false;
							if (amount > 0)
								fmpm.m_sType = "Pay";
							else if (amount == 0)
							{
								Program.MsgBox("Amount cannot be zero.");
								return;
							}
							else
								fmpm.m_sType = "Refund";
							fmpm.ShowDialog();
							bSuccess = fmpm.m_bSuccess;
						}
					}
					else if (Program.m_bEnableLatiPay)
					{
						if (Program.m_sLatiPayApiKey == "" || Program.m_sLatiPayWalletID == "" || Program.m_sLatiPayUserID == "")
						{
							Program.MsgBox("Latipay is not set up, please contact your dealer.");
							return;
						}
						else
						{
							FormLatipay flp = new FormLatipay();
							flp.m_sInvoiceNumber = GetNextInvNumber();
							flp.m_dAmount = dGetBanlance;
							flp.m_bSuccess = false;
							flp.ShowDialog();
							bSuccess = flp.m_bSuccess;
						}
					}
					else if (Program.m_bEnableAttractPay || Program.m_bEnableWechatPayment || Program.m_bEnableAlipayDirect)
					{
						FormOtherPayment fm = new FormOtherPayment();
						fm.m_sInvoiceNumber = GetNextInvNumber();
						fm.m_dAmount = dGetBanlance;
						fm.m_bSuccess = false;
						fm.ShowDialog();
						bSuccess = fm.m_bSuccess;
					}
					else
					{
						FormMSG fm = new FormMSG();
						fm.btnYes.Visible = true;
						fm.btnNo.Visible = true;
						fm.btnOK.Visible = false;
						fm.m_sMsg = "DIGITAL PAYMENT ACCEPTED?";
						fm.ShowDialog();
						if (!fm.m_bYes)
							return;
						else
							bSuccess = true;
					}
					if (bSuccess)
					{
						m_dCheque = dGetBanlance;
						if (TotalPaymentOK())
						{
							doShowPaymentInfo();
							DoCheckout();
							Cash.Text = "";
							labelBalance.Text = "";
							Cheque.Text = "";
							Cheque.Focus();
							return;
						}
					}
/*					m_dCheque = Program.MyMoneyParse(Cheque.Text);
					if (!doWariningPayment("cheque", m_dCheque, this.labelBalance.Text))
						return;
//					if (m_dCheque != 0)
					if (m_dCheque != 0 && m_dCashout == 0 && m_dEftpos == 0 && m_dCredit == 0)
					{
						if (Program.m_eftposType == "verifone")
						{
//							EFTPOS eftPanel = new EFTPOS();
							eftPanel.m_dChequeAmount = m_dCheque;
							eftPanel.m_sRefNum = Program.MyIntParse(Program.m_sStationID).ToString("D3");
							eftPanel.ShowDialog();
//							m_bEftposLog(eftPanel.m_sMonitorData, eftPanel.m_bAccepted);// Eftpos Log
							if (!eftPanel.m_bAccepted)
							{
								return;
							}
						}
						SetChequeTotal();
						if (TotalPaymentOK())
						{
							doShowPaymentInfo();
							Cheque.Text = "";
							labelBalance.Text = "";
							DoCheckout();
						}
						else
						{
							ConfirmOnePayment();
							doShowPaymentInfo();
						}
					}
					else
					{
						FormMSG fm = new FormMSG();
						fm.btnYes.Visible = false;
						fm.btnNo.Visible = false;
						fm.m_sMsg = "Sorry, no other payment method in \r\n cheque payment!";
						fm.ShowDialog();
						return;
						//Change.Text = "";
						//Cheque.Text = GetBalance();
						//Cash.Select();
						//Cash.Focus();
					}*/
					break;
				case 40:
					//buttonCheckout.Focus();
					Cheque.Text = "";
					charge.Text = GetBalance();
					charge.Focus();
					break;
				case 38:
					Cheque.Text = "";
					Credit.Text = GetBalance();
					Credit.Focus();
					break;
				case 46:
					this.Cheque.Text = "";
					break;
				default:
					OnFormKeyDown(sender, e);
					break;
			}
		}
		private void charge_KeyDown(object sender, KeyEventArgs e)
		{
			if (m_dVipPayAmount != 0)
				return;
			if (!m_bScanMember && MemberShipID.Text == "")
			{
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = " Please keyin Vip ID!";
				fm.ShowDialog();
				MemberShipID.Focus();
				return;
			}
			switch (e.KeyValue)
			{
				case 17:
                    m_dEftpos = Program.MyMoneyParse(Eftpos.Text);
                    m_dCredit = Program.MyMoneyParse(Credit.Text);
                    if (m_dEftpos != 0)
                    {

                        FormMSG fmsg = new FormMSG();
                        fmsg.btnNo.Visible = false;
                        fmsg.btnYes.Visible = false;
                        fmsg.m_sMsg = "Eftpos Payment is selected! \r\n   Please double check";
                        fmsg.ShowDialog();
                        this.Eftpos.Focus();
                        return;
                    }
                    else if (m_dCredit != 0)
                    {

                        FormMSG fmsg = new FormMSG();
                        fmsg.btnNo.Visible = false;
                        fmsg.btnYes.Visible = false;
                        fmsg.m_sMsg = "Credit Payment is selected! \r\n   Please double check";
                        fmsg.ShowDialog();
                        this.Credit.Focus();
                        return;
                    }
					m_dChargeTotal = Program.MyMoneyParse(charge.Text);
					if (!doWariningPayment("charge", m_dChargeTotal, this.labelBalance.Text))
						return;
					if (m_dChargeTotal == 0)
					{
						Change.Text = "";
						Cheque.Text = GetBalance();
						Cheque.Select();
						Cheque.Focus();
					}
					else
					{
						doShowPaymentInfo();
						DoCheckout();
						charge.Text = "";
						labelBalance.Text = "";
					}
					break;
				case 40:
					charge.Text = "";
					double dBanlance = Program.MyDoubleParse(labelBalance.Text);
					this.Cash.Text = Program.RoundCents(dBanlance, m_sRoundingNum).ToString();// Math.Round(dBanlance, 1).ToString();
					labelBalance.Text = Cash.Text;
					Cash.Focus();
					Cash.Select();
					break;
				case 38:
					charge.Text = "";
					Cheque.Text = GetBalance();
					Cheque.Focus();
					break;
				case 46:
					this.charge.Text = "";
					break;
				default:
					OnFormKeyDown(sender, e);
					break;
			}
		}
		private string GetBalance()
		{
			m_dCash = Program.MyMoneyParse(Cash.Text);
			m_dEftpos = Program.MyMoneyParse(Eftpos.Text);
			m_dCashout = Program.MyMoneyParse(Cashout.Text);
			m_dCredit = Program.MyMoneyParse(Credit.Text);
			m_dCheque = Program.MyMoneyParse(Cheque.Text);
			double dTotalPayment = m_dCash + m_dEftpos + m_dCredit + m_dCheque;
			//			double dBalance = m_dTotal - m_dTotalPaid - dTotalPayment;// +m_dCashout;
			double dBalance = m_dTotal - m_dTotalPaid + m_dCashoutTotal;
			//			if(m_bSurcharge)
			//				dBalance = dBalance + m_dSurcharge;
			double dGetTotalRounding = Math.Round(m_dRoundingTotal, 2);
//			if (m_dCash == 0)
//				dGetTotalRounding = 0;
			dBalance = Math.Round(dBalance, 2);
			dBalance = dBalance + dGetTotalRounding;
			return dBalance.ToString();
		}
		private void SetEftposTotal()
		{
			if (this.Eftpos.Text != "" && m_dEftpos == 0)
				m_dEftpos = Program.MyDoubleParse(this.Eftpos.Text);
			if (this.Cashout.Text != "" && m_dCashout == 0)
				m_dCashout = Program.MyDoubleParse(this.Cashout.Text);
			double dEftposTotal = m_dEftpos + m_dCashout;
            if (m_dEftpos > m_dTotal && m_dEftpos > 0 && m_dTotal > 0)
			{
				m_dCashout = m_dEftpos - m_dTotal;
				dEftposTotal = m_dEftpos;
				this.Cashout.Text = Program.RoundCents(m_dCashout, m_sRoundingNum).ToString();
			}
			if (dEftposTotal != 0)
				labelChange.Text = "Eftpos Total";
			else
				labelChange.Text = "CHANGE";
			if (m_dEftpos <= m_dTotal)
				m_dEftposTotal += m_dCashout;
			Change.Text = dEftposTotal.ToString("c");
			labelBalance.Text = GetBalance() + m_dCashout;
		}
		private void SetCreditTotal()
		{
			//			double dCreditTotal = m_dCredit + m_dCashout 
			double dCreditTotal = m_dCredit + m_dCashout;//+ m_dSurcharge;
			labelChange.Text = "Credit Card Total";
			Change.Text = dCreditTotal.ToString("c");
			labelBalance.Text = (Program.MyDoubleParse(GetBalance()) + m_dSurcharge).ToString(); ;
		}
		private void SetChequeTotal()
		{
			double dChequeTotal = m_dCheque + m_dCashout;
			labelChange.Text = "Cheque Total";
			Change.Text = dChequeTotal.ToString("c");
			labelBalance.Text = GetBalance();
		}
		private bool TotalPaymentOK()
		{
			GetBalance();
			double dPaymentTotal = m_dCash + m_dEftpos + m_dCredit + m_dCheque;
			double d_sTotal = m_dTotal;
			double dChangeRound = 0;
			double m_dManualChange = 0;
			string s_keyInValue = this.Cash.Text;
            if (s_keyInValue != "" && s_keyInValue != null)
			{
				if (m_dTotal > 0)
					d_sTotal = Program.RoundCents(m_dTotal, m_sRoundingNum);
				else
					d_sTotal = Program.RoundCents(m_dTotal, m_sRoundingNum);
			}
			d_sTotal = Math.Round(d_sTotal, 2);
			dPaymentTotal = Math.Round(dPaymentTotal, 2);
			// double dRemain = Math.Round((m_dTotal - m_dTotalPaid - dPaymentTotal), 2);
			double dRemain = Math.Round((d_sTotal - m_dTotalPaid - dPaymentTotal), 2);

//			if (m_bSurcharge)
//				dRemain = Math.Round((d_sTotal - m_dTotalPaid - dPaymentTotal + m_dSurcharge), 2);

			double m_dEftposIn = dPaymentTotal - m_dTotal;
			m_dEftposIn = Math.Round(m_dEftposIn, 2);

			//Get Rounding Cents, Focus On Cash In Only, Key In Value Minus Order Total
            if (s_keyInValue != "" && s_keyInValue != null)
				dChangeRound = d_sTotal - m_dTotal;
			m_dRoundingTotal = dChangeRound;
			m_dRoundingTotal = Math.Round(m_dRoundingTotal, 2);
			m_dRoundingReceipt = m_dRoundingTotal;
			if (m_dRoundingReceipt < 0)
				m_dRoundingReceipt = 0 - m_dRoundingReceipt;
			m_dRoundingReceipt = Math.Round(m_dRoundingReceipt, 2);
			bool bRet = false;
			if (dRemain <= 0.05 && dRemain >= -0.05)
			{
				//				DoAAService(m_dOrderTotal, m_invoiceNumber, true);
				bRet = true; //paid in full
			}
			else if (dRemain < 0)
			{
				double dChange = 0 - dRemain;
				labelChange.Text = "CHANGE";
				if (m_dEftposIn > 0.05 && m_dEftpos != 0)
				{
					this.Change.Text = dPaymentTotal.ToString("N2");
					this.labelChange.Text = "Eftpos Total";
				}
				else
				{
					if (s_keyInValue != "")
					{
						m_dManualChange = Program.MyDoubleParse(s_keyInValue) - Program.RoundCents(Program.MyDoubleParse(labelBalance.Text), m_sRoundingNum);
						m_dManualChange = Math.Round(m_dManualChange, 2);
						//dChange = m_dManualChange;
						dChange = Program.RoundCents(m_dManualChange, m_sRoundingNum);
						if (!changeProtection(dChange))
						{
							MessageBox.Show("CH =" + dChange.ToString() + " Ca = " + m_dCash.ToString() + " Ef =" + m_dEftpos.ToString() + " CC= " + m_dCredit.ToString() + " CQ=" + m_dCheque.ToString() + " Total=" + m_dTotal.ToString() + " CI =" + this.Cash.Text + " Corr =" + Program.RoundCents(m_dCash - m_dTotal, m_sRoundingNum) + " \r\n Please Record those figure and forwards to Admin. CODE: P1006");
							ResetPayment();
							return false;
						}
					}
					this.Change.Text = dChange.ToString("c");//Math.Round(dChange, 1).ToString("c");
                    m_dChange = dChange;
				}
				if (Program.m_bEnableAA)
					DoAAService(m_dOrderTotal, m_invoiceNumber, true);
				if(m_dChange >= 0 && Program.MyDoubleParse(GetBalance()) >=0 )
					bRet = true;
			}
			if (bRet)
			{
				Cash.Text = "";
				Cashout.Text = "";
				Eftpos.Text = "";
				Credit.Text = "";
				Cheque.Text = "";
				charge.Text = "";
				labelBalance.Text = "";
			}
			return bRet;
		}
		private void ResetPaymentLabels()
		{
			Cash.Text = "";
			Eftpos.Text = "";
			Cash.Text = "";
			Cashout.Text = "";
			Credit.Text = "";
			Cheque.Text = "";
			Change.Text = "";
			charge.Text = "";
			this.m_dDiscRate = 0;
			Cash.Select();
			Cash.Focus();
			m_bScanMember = false;
			m_membernumber[m_nCurrentCart] = "";
			double dBalance = Program.MyMoneyParse(GetBalance());
			if (dBalance == 0)
				labelBalance.Text = "";
			else
				labelBalance.Text = dBalance.ToString("N2");
		}
		private void ResetCart()
		{
			m_dCash = 0;
			m_dEftpos = 0;
			m_dCashout = 0;
			m_dCredit = 0;
			m_dCheque = 0;
			m_dCashTotal = 0;
			m_dEftposTotal = 0;
			m_dCashoutTotal = 0;
			m_dCreditTotal = 0;
			m_dChequeTotal = 0;
			m_dChargeTotal = 0;
			m_dSpecialTotal = 0;
			m_dDiscountTotal = 0;
			m_dRoundingTotal = 0;

			//member point
			m_nTotalLegalPoint = 0;

			m_dTotal = 0;
			m_dTotalPaid = 0;
			m_nOrderId = 0;
			m_sNote = "";
			m_nCardId = 0;
			m_invoiceNumber = "";

			cart.Rows.Clear();
			m_fmad.cart.Rows.Clear();
			m_fmads.cart.Rows.Clear();

			MemberShipID.Text = "";
			MemberShipName.Text = "";
			m_fmad.citem1.Text = "";
			m_fmad.citem2.Text = "";
			m_fmads.citem1.Text = "";
			m_fmads.citem2.Text = "";
			labelPoints.Text = "";
			labelBalance.Text = "";
			Cash.Text = "";
			Eftpos.Text = "";
			Cashout.Text = "";
			Credit.Text = "";
			Cheque.Text = "";
			Change.Text = "";
			charge.Text = "";
			TotalPaid.Text = "";
			m_bBeDisced = false;
			m_bScanMember = false;
			m_bExistingMember = false;
			m_bMemberDiscount = false;
			bIs_Refund = false;
			this.msgboard.Text = "";
			this.showItems.Text = "";
            this.lblQtys.Text = "";
			this.lblprocess.Text = "";
			m_membernumber[m_nCurrentCart] = "";
			m_sVoucherBarcode = "";
			UpdateTotalPriceDiscplay();
			m_sReceipt = "";
			m_dDiscRate = 0;
			m_iEftposLog = 0;
			cbExport.Checked = false;
			eftPanel.m_sReceipt = "";
			eftPanel.m_sReceiptCust = "";
			barcode.Focus();
		}
		//*** ResetPayment When item deleted CH 12/11/08 ****//
		private void ResetPayment()
		{
			m_dCash = 0;
			m_dEftpos = 0;
			m_dCashout = 0;
			m_dCredit = 0;
			m_dCheque = 0;
			m_dCashTotal = 0;
			m_dEftposTotal = 0;
			m_dCashoutTotal = 0;
			m_dCreditTotal = 0;
			m_dChequeTotal = 0;
			m_dChargeTotal = 0;
			m_dSpecialTotal = 0;
			m_dDiscountTotal = 0;
			m_dRoundingTotal = 0;

			//member point
			m_nTotalLegalPoint = 0;

			m_dTotalPaid = 0;
			m_nOrderId = 0;
			m_sNote = "";
			m_nCardId = 0;
			m_invoiceNumber = "";

			Cash.Text = "";
			Eftpos.Text = "";
			Cashout.Text = "";
			Credit.Text = "";
			Cheque.Text = "";
			Change.Text = "";
			charge.Text = "";
			TotalPaid.Text = "";
			MemberShipID.Text = "";
			MemberShipName.Text = "";
			m_bExistingMember = false;
			labelChange.Text = "CHANGE";
			this.lblprocess.Text = "";
			m_bScanMember = false;
			m_sReceipt = "";
			this.txtpaymentinfo.Text = "";
			this.Cash.Focus();
			UpdateTotalPriceDiscplay();
		}
		//*** ResetPayment End ***//
		private void buttonCheckout_Click(object sender, EventArgs e)
		{
			if (discountswtich.Text == "1")
			{
				this.Cash.Text = Program.RoundCents(m_dTotal, m_sRoundingNum).ToString("N2"); // Math.Round(m_dTotal, 1).ToString();
				labelBalance.Text = Cash.Text;
				Cash.Focus();
				Cash.Select();
				discountswtich.Text = "2";
				m_fmad.TotalPrice.Text = this.labelBalance.Text;
				m_fmads.TotalPrice.Text = this.labelBalance.Text;
			}
			else
			{
				if (TotalPaymentOK())
				{
					doShowPaymentInfo();
					DoCheckout();
				}
				else
				{
					doShowPaymentInfo();
					ConfirmOnePayment();
				}
			}
		}
		private bool DoVipCheckout()
		{
			string sHeader = ReadSitePage("pos_receipt_header");
			sHeader = sHeader.Replace("Tax Invoice", "VIP Payment");
			sHeader = sHeader.Replace("@@sales", SalesName.Text);
			sHeader = sHeader.Replace("@@station", Station.Text);
			sHeader = sHeader.Replace("@@inv_num", "");
			sHeader = sHeader.Replace("@@date", DateTime.Now.ToString("dd-MM-yyyy"));
			sHeader = sHeader.Replace("@@time", DateTime.Now.ToString("HH:mm"));
			string vipinfo = "";
			if (MemberShipID.Text != "")
				vipinfo = GetVipInfo(MemberShipID.Text);
			sHeader = sHeader.Replace("@@VIP_INFO", vipinfo);

			string s = "";
			s += sHeader;
			s += Program.PrintPadding("", "", "-") + "\r\n";
			//			s += Program.PrintPadding("[b]Vip Payment[/b]", "", "") + "\r\n";
			s += Program.PrintPadding("Date:" + DateTime.Now.ToString("dd-MM-yyyy"), "", "") + "\r\n";
			s += Program.PrintPadding("", "", "-") + "\r\n";

			string[] invs = m_sVipInvoices.Split(',');
			string[] amounts = m_sVipAmounts.Split(',');
			string[] types = m_sVipPaymentTypes.Split(',');

			double dCash = m_dCashTotal;
			double dEftpos = m_dEftposTotal;
			double dCheque = m_dChequeTotal;
			double dCredit = m_dCreditTotal;

            m_sCardId = MemberShipID.Text;

			string sc = "";
			double dPaidTotal = 0;
			double dRounding = 0;
			string pm = "eftpos";
			bool bFullyPaid = false;
			for (int i = 0; i < invs.Length; i++)
			{
				bFullyPaid = false;
				dRounding = 0;
				m_invoiceNumber = invs[i];
				if (m_invoiceNumber == "")
					continue;
				double dAmount = Program.MyDoubleParse(amounts[i]);
				//			if(dCash > 0)
				if (dCash != 0)
				{
					pm = "cash";
					if (dAmount <= dCash) //pay by cash in full
					{
						bFullyPaid = true;
					}
					else  //not enought
					{
						if (dAmount - dCash < 0.1)//roudning
						{
							dRounding = dAmount - dCash;
							dAmount = dCash;
							bFullyPaid = true;
						}
						else
						{
							amounts[i] = (dAmount - dCash).ToString(); //adjust amount for next pay
							dAmount = dCash;
							bFullyPaid = false;
						}
					}
					dCash -= dAmount;
				}
				else if (dEftpos > 0)
				{
					pm = "eftpos";
					if (dAmount <= dEftpos)
					{
						bFullyPaid = true;
					}
					else
					{
						amounts[i] = (dAmount - dEftpos).ToString(); //adjust amount for next pay
						dAmount = dEftpos;
						bFullyPaid = false;
					}
					dEftpos -= dAmount;
				}
				else if (dCheque > 0)
				{
					pm = "cheque";
					if (dAmount <= dCheque)
					{
						bFullyPaid = true;
					}
					else
					{
						amounts[i] = (dAmount - dCheque).ToString(); //adjust amount for next pay
						dAmount = dCheque;
						bFullyPaid = false;
					}
					dCheque -= dAmount;
				}
				else if (dCredit > 0)
				{
					pm = "credit card";
					if (dAmount <= dCredit)
					{
						bFullyPaid = true;
					}
					else
					{
						amounts[i] = (dAmount - dCredit).ToString(); //adjust amount for next pay
						dAmount = dCredit;
						bFullyPaid = false;
					}
					dCredit -= dAmount;
				}
				else //no more money
				{
					break;
				}
				dPaidTotal += dAmount;
				if (!DoReceiveOnePayment(pm, dAmount))
					return false;
				if (dRounding != 0)
				{
					if (!DoReceiveOnePayment("cash rounding", dRounding))
						return false;
				}
				sc += " UPDATE orders SET order_deleted = 2 WHERE invoice_number = " + m_invoiceNumber + " ";

				s += Program.PrintPadding("INV# " + invs[i] + " : ", dAmount.ToString("c"), "") + "\r\n";
				if (!bFullyPaid)
				{
					i--; //continue pay this invoice
				}
			}
			if (sc != "")
			{
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
			}

			s += Program.PrintPadding("", "", "-") + "\r\n";
			s += Program.PrintPadding("VIP Name : ", m_sCardName, "") + "\r\n";
			s += Program.PrintPadding("VIP ACC# : ", m_sCardBarcode, "") + "\r\n";
			s += Program.PrintPadding("Balance : ", GetVipAccountBalance(m_sCardId).ToString("c"), "") + "\r\n";

			string sFooter = ReadSitePage("pos_receipt_footer");
			sFooter = sFooter.Replace("@@member_points_msg", "");
			//			s += sFooter;
			s += " \r\n \r\n \r\n \r\n \r\n \r\n";
			m_sLastReceipt = s;
			PrintReceipt();
			m_dVipPayAmount = 0;
			return true;
		}
		private double GetVipAccountBalance(string card_id)
		{
			if (card_id == "")
				return 0;
			if (dst.Tables["gvab"] != null)
				dst.Tables["gvab"].Clear();
			string sc = " SELECT SUM(total - amount_paid) AS balance FROM invoice WHERE card_id = " + card_id;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				if (myCommand.Fill(dst, "gvab") <= 0)
					return 0;
			}
			catch (Exception e)
			{
				MessageBox.Show("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return 0;
			}
			DataRow dr = dst.Tables["gvab"].Rows[0];
			string balance = dr["balance"].ToString();
			return Program.MyDoubleParse(balance);
		}
		private bool DoCheckout()
		{
			string sChange = "";
			string sMaxCashOut = m_sCashoutcontrol;
			m_sTotalPoint = TotalPoint();
			sChange = Change.Text;
			if (m_dCashout > 0 && Program.MyDoubleParse(sMaxCashOut) != 0)
			{
				labelcashout.Text = "CHANGE:";
				labelcashout.ForeColor = System.Drawing.Color.Red;
				showcashout.ForeColor = System.Drawing.Color.Red;
				Change.ForeColor = System.Drawing.Color.YellowGreen;
				showcashout.Text = m_dCashout.ToString("c");
				//Change.Visible = false;
				txtShowCashOut.Visible = true;
				txtcashouttitle.Visible = true;
				//txtShowCashOut.Location =new Point(17, 482);
				txtShowCashOut.Text = showcashout.Text;
			}
			if (labelChange.Text != "CHANGE")
				Change.ForeColor = System.Drawing.Color.YellowGreen;

			Change.Text = sChange;

			if (Change.Text != "")
			{
				m_fmad.showchange.Text = Change.Text;
				m_fmad.labelChange.Text = labelChange.Text;
				m_fmads.showchange.Text = Change.Text;
				m_fmads.labelChange.Text = labelChange.Text;
				SetForegroundWindow(this.Handle);
			}
			else
			{
				m_fmad.showchange.Text = "";
				m_fmad.labelChange.Text = "";
				m_fmads.showchange.Text = "";
				m_fmads.labelChange.Text = "";
			}

			RecordOnePayment(false); //record the last payment, no msg
			if (m_dVipPayAmount != 0)
			{
				if (TotalPaymentOK())
				{
					if (!DoVipCheckout())
						return false;
					ResetCart();
					ResetPaymentLabels();
					return true;
				}
				else
				{
					return false; //continue payment
					//					FormMSG fm = new FormMSG();
					//					fm.m_sMsg = "";
				}
			}
			if (m_bSurcharge)
			{
				string ascode = m_sCreditSurchargeItemCode; //promotion service item
				string asname = (m_dSurchargeRate * 100).ToString() + "%  Surcharge";
				double dasPrice = m_dSurcharge;
				/*
				if (nType == 1)
				{
					dasPrice = 0 - dItemPrice;
				}
				else if (nType == 2)
				{
					ascode = reward_item_code;
					asname = reward_item_name;
					dasPrice = 0;
				}
				*/
				AddToCartSurcharge("", ascode, asname, "", dasPrice, 1, "0", false, ascode, 0, false, false);
			}

			if (!CreateOrder())
				return false;
			Program.RecordTillData("total_orders", "1");
            if (!RecordAllPayment())
				return false;
			string sc = " UPDATE invoice SET uploaded = 0, uploaded_activata = 0 WHERE id = '" + m_invoiceNumber + "' ";
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
			if (Program.m_bEnableAutoSync)
			{
				FormSync fSync = new FormSync();
				fSync.ShowDialog();
				fSync.DoUploadInvoiceActivata();
			}
			if(!Program.m_bVoucherUseWebService)
			{
				if (m_bVipVoucherEnabled)
				{
					if (!CalcMemberPoints())
						return false;
				}
				if (m_dVoucherStart <= m_dTotal && m_bVoucherEnabled)
				{
					DoCreateVoucher();
				}
			}
			BuildReceipt("default printer");
			if (Program.m_bEnableSecondPrinter)
				BuildReceipt("second printer");
			if (m_dDiscountTotal != 0)
			{
				sc = " UPDATE orders SET total_discount = " + m_dDiscountTotal + " WHERE id = " + m_nOrderId;
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
			}

            int print_number = m_iNumReceiptPrintOut;
            //if there is a voucher, the receipt must be printed
            if (m_sVoucherText != "" && print_number == 0)
                print_number = 1;

            if (print_number == 0)
				KickCashdraw();

			KickCashdraw();
            if (print_number > 0)
			{
                for (int i = 0; i < print_number; i++)
				{
					PrintReceipt();
					m_iVoucherControl++;
				}
			}
			if (!Program.m_bVoucherUseWebService)
				DoDeleteVoucherBarcode(m_sVoucherUsedBarcode);
			//RecordLastPoints();
			if (Program.m_bEnableAA)
				DoAAService(m_dOrderTotal, m_invoiceNumber, true);
			m_bIdCheckDone = false;
			barcode.Focus();
            m_dChange = 0;
			if(Program.m_bEnableSelfService)
			{
				timer5.Interval = 10000;
				timer5.Start();
			}
			return true;
		}
		private bool RecordLastPoints() 
		{
			int last_total_points = 0;
			if (!Program.m_bVoucherUseWebService)
				last_total_points = m_nMemberPoints;
			else
				last_total_points = Program.MyIntParse(labelPoints.Text);
			string sc = " UPDATE invoice SET last_total_points = " + last_total_points.ToString();
			sc += " WHERE invoice_number = " + m_invoiceNumber;
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
		private void PrintReceipt()
		{
			//KickCashdraw();
			if (m_sLastReceipt == "")
				return;
			if (Program.m_sPrinterName == "")
				return;
			m_sPrintBuffer = m_sLastReceipt;
			if (m_sVoucherText != "")
			{
				m_sPrintBuffer += "\r\r";
				m_sPrintBuffer += m_sVoucherText; //print voucher if exists
				m_sVoucherText = ""; //only print once
			}
			try
			{
				SetDefaultPrinterSettings();
				m_sUsingPrinterName = "default printer";
				printDoc.Print();
			}
			catch (Exception e)
			{
				MessageBox.Show("Printer error, print receipt failed." + e.ToString());
			}

			if (Program.m_bEnableSecondPrinter)
			{
				if (m_sLastSecondReceipt == "")
					return;
				if (Program.m_sSecondPrinterName == "")
					return;
				m_sPrintBuffer = m_sLastSecondReceipt;
				try
				{
					SetSecondPrinterSettings();
					m_sUsingPrinterName = "second printer";
					printDoc.Print();
				}
				catch (Exception e)
				{
					MessageBox.Show("Printer error, print second receipt failed." + e.ToString());
				}
			}	
		}
		private void SetDefaultPrinterSettings()
		{
			printDoc.PrinterSettings.PrinterName = Program.m_sPrinterName;
		}
		private void SetSecondPrinterSettings() 
		{
			printDoc.PrinterSettings.PrinterName = Program.m_sSecondPrinterName;
		}
		private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
		{
			float fSize = (float)Program.MyDoubleParse(Program.m_sFontSize);
			Font printFont = new Font(Program.m_sFontName, fSize, Program.m_tFontStyle);
			Font printFontBig = new Font(Program.m_sFontName, fSize + 6, Program.m_tFontStyle);
			Font printFontJiant = new Font(Program.m_sFontName, fSize + 20, Program.m_tFontStyle);
			Font m_BarcodeFont = new Font("3 of 9 Barcode", 19);
			Font m_PrintFont = new Font("Times New Roman", 50);
			if (Program.m_bEnableSecondPrinter && m_sUsingPrinterName == "second printer")
			{
				fSize = (float)Program.m_sSecondPrinterFontSize;
				printFont = new Font(Program.m_sSecondPrinterFontName, fSize, Program.m_tFontStyle);
				printFontBig = new Font(Program.m_sSecondPrinterFontName, fSize + 6, Program.m_tFontStyle);
				printFontJiant = new Font(Program.m_sSecondPrinterFontName, fSize + 20, Program.m_tFontStyle);
			}
			int y = 0; //vertical position;
			int lineHeight = 14;
			int lineHeightBig = 30;
			int lineHeightJiant = 50;
			string s = "1";
			int receiptEnd = 0;
			for (int c = 0; c < m_sPrintBuffer.Length; c++)
			{
				if (m_sPrintBuffer[c] == '\r')
					receiptEnd++;
			}

			m_sPrintBuffer = m_sPrintBuffer.Replace("\r", "");

			int b = -1;
			int j = -1;
			string[] aLine = m_sPrintBuffer.Split('\n');
			for (int i = 0; i < aLine.Length; i++)
			{
				string sLine = aLine[i] + "\r\n";
				b = sLine.IndexOf("[b]");
				j = sLine.IndexOf("[j]");
				if (b >= 0)
				{
					sLine = sLine.Replace("[b]", "").Replace("[/b]", "");
					e.Graphics.DrawString(sLine, printFontBig, Brushes.Black, 0, y);
					y += lineHeightBig;
				}
				else if (j >= 0)
				{
					sLine = sLine.Replace("[j]", "").Replace("[/j]", "");
					e.Graphics.DrawString(sLine, printFontJiant, Brushes.Black, 0, y);
					y += lineHeightJiant;
				}
				else
				{
					e.Graphics.DrawString(sLine, printFont, Brushes.Black, 0, y);
					y += lineHeight;
				}
			}

//m_sVoucherBarcode = "962321765222";
//m_sVoucherAmount = "$100.00";
			if (m_sVoucherBarcode != "" && m_dTotal > 0 && m_iVoucherControl < 1)
			{
				e.Graphics.DrawString(m_sVoucherAmount, m_PrintFont, Brushes.Black, 10, y);
				y += 70;
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y);
				y += 20;
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y);
				y += 20;
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y);
				y += 20;
				e.Graphics.DrawString("\r\n\r\n ", printFont, Brushes.Black, 0, y);
			
//				y += receiptEnd * 6;
/*
				e.Graphics.DrawString(m_sVoucherAmount, m_PrintFont, Brushes.Black, 30, y - 180);
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y - 60);
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y - 80);
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y - 100);
				e.Graphics.DrawString("\r\n\r\n ", printFont, Brushes.Black, 0, 20);
*/ 
			}
			SetForegroundWindow(this.Handle);
			barcode.Focus();
		}
		private void printDoc_PrintPage1(Object sender, PrintPageEventArgs e)
		{
			float fSize = (float)Program.MyDoubleParse(Program.m_sFontSize);
			Font printFont = new Font(Program.m_sFontName, fSize, Program.m_tFontStyle);
			Font printFontBig = new Font(Program.m_sFontName, fSize + 6, Program.m_tFontStyle);
			Font m_BarcodeFont = new Font("3 of 9 Barcode", 19);
			Font m_PrintFont = new Font("Times New Roman", 50);
			int y = 0; //vertical position;
			int lineHeight = 20;
			string s = "1";
			// Calculate Receipt Rows //
			int receiptEnd = 0;
			for (int c = 0; c < m_sPrintBuffer.Length; c++)
			{
				if (m_sPrintBuffer.Substring(c, 1) == "\r")
					receiptEnd++;
			}

			int p = m_sPrintBuffer.IndexOf("[b]");
			if (p < 0)
			{
				s = m_sPrintBuffer;
				e.Graphics.DrawString(m_sPrintBuffer, printFont, Brushes.Black, 0, y);

				return;
			}
			int pEnd = m_sPrintBuffer.IndexOf("[/b]", p + 3);

			if (p > 0)
			{
				s = m_sPrintBuffer.Substring(0, p);
				e.Graphics.DrawString(m_sPrintBuffer, printFont, Brushes.Black, 0, y);
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

			//Print Barcode, Code Font 39 is Required // C.H
			if (m_sVoucherBarcode != "" && m_dTotal > 0 && m_iVoucherControl < 1)
			{
				y *= receiptEnd;
				// y -= 90;
				e.Graphics.DrawString(m_sVoucherAmount, m_PrintFont, Brushes.Black, 30, y - 180);
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y - 60);
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y - 80);
				e.Graphics.DrawString("*" + m_sVoucherBarcode + "*", m_BarcodeFont, Brushes.Black, 0, y - 100);
				e.Graphics.DrawString("\r\n\r\n ", printFont, Brushes.Black, 0, 20);
			}
			//Print Barcode End //
			SetForegroundWindow(this.Handle);
			barcode.Focus();
		}
		private void KickCashdraw()
		{
			//         byte[] kick = { 0x1b, 0x70, 0x30, 0x7f };//, 0x0a, 0x0};//new char[6];
			byte[] kick = new byte[6];
			int p = 0;
			kick[p++] = 0x1b;
			kick[p++] = 0x70;
			kick[p++] = 0x30;
			kick[p++] = 0x7f;
			if (Program.m_sEpson == true)
			{
				kick[p++] = 0x0a;//{ 0x1b, 0x70, 0x30, 0x7f, 0x0a, 0x0};//new char[6];
				kick[p++] = 0x0;
			}

			ASCIIEncoding encoding = new ASCIIEncoding();
			string kickout = encoding.GetString(kick);
			RawPrinterHelper.SendStringToPrinter(kickout);
			kickout = "";
		}
		private string GetVipInfo(string sBarcode)
		{
			if (dst.Tables["gvi"] != null)
				dst.Tables["gvi"].Clear();
			string sc = " SELECT * FROM card WHERE barcode = '" + Program.EncodeQuote(sBarcode) + "' ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				if (myCommand.Fill(dst, "gvi") <= 0)
					return "";
			}
			catch (Exception e)
			{
				MessageBox.Show("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return "";
			}
			DataRow dr = dst.Tables["gvi"].Rows[0];
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
			m_sCardName = name;
			m_sCardBarcode = barcode;
			return s;
		}
		private void BuildReceipt(string printer)
		{
			string sCallofOrderNumber = Program.GetCallOfOrderNumber(m_nOrderId.ToString());
				
			string sHeader = "";
			sHeader = ReadSitePage("pos_receipt_header");
			if (Program.m_bCallOfOrderNumber)
			{	
				if(printer == "default printer")
				{
					sHeader = "[j]       " + sCallofOrderNumber + "[/j]\r\n" + sHeader;
					m_sLastSecondReceiptOrderNumber = sCallofOrderNumber;
				}
				else if (printer == "second printer")
				{
					sHeader = "[j]       " + m_sLastSecondReceiptOrderNumber + "[/j]\r\n" + sHeader;
					m_sLastSecondReceiptOrderNumber = "";
				}
			}
			sHeader = sHeader.Replace("@@sales", SalesName.Text);
			sHeader = sHeader.Replace("@@station", Station.Text);
			sHeader = sHeader.Replace("@@inv_num", m_invoiceNumber);
			sHeader = sHeader.Replace("@@date", DateTime.Now.ToString("dd-MM-yyyy"));
			sHeader = sHeader.Replace("@@time", DateTime.Now.ToString("HH:mm"));
			string vipinfo = "";
			if (MemberShipID.Text != "")
				vipinfo = GetVipInfo(MemberShipID.Text);
			sHeader = sHeader.Replace("@@VIP_INFO", vipinfo);
			if (cbExport.Checked)
				//				sHeader = sHeader.Replace("Tax Invoice", "Export Invoice");
				sHeader = sHeader.Replace("Tax Invoice", "Invoice");

			string sNVRHeader = "";
			sNVRHeader = ReadSitePage("nvr_header");
			sNVRHeader = sNVRHeader.Replace("@@sales", SalesName.Text);
			sNVRHeader = sNVRHeader.Replace("@@station", Station.Text);
			sNVRHeader = sNVRHeader.Replace("@@inv_num", m_invoiceNumber);
			sNVRHeader = sNVRHeader.Replace("@@date", DateTime.Now.ToString("dd-MM-yyyy"));
			sNVRHeader = sNVRHeader.Replace("@@time", DateTime.Now.ToString("HH:mm"));

			string m_sCashMaxCashout = m_sCashoutcontrol;
			double dPrintSubTotal = 0;
			double dNorNormalTotal = showNormalTotalAmount();

			string code = "";
			string discount = "";
			string supplier_code = "";
			string name = "";
			string cn_name = "";
			string en_name = "";
			string price = "";
			string qty = "";
			string sprice = "";
			string sTitle = "";
			string sNormalPriceOnCart = "";
			//			double dRowSub = 0;
			double dNormalPrice = 0;
			double dSavePerItem = 0;
			double dDiscountRate = 0;
			double dCountTotalDiscount = 0;
			string sRowTotal = "";
			double dTax = 0;
			string s_PromtiontItem = "";
			double m_dReceiptSubTotal = 0;
			double m_dReceiptGstTotal = 0;
			string avoid_point = "";
			double m_dNormalSubTotal = 0;
			string has_scale = "";
			//            double dTax = 0;
			//            string tax= "";
			string s = "";
//			s += sHeader;
			s += Program.PrintPadding("", "", "=") + "\r\n";
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				code = cart.Rows[i].Cells["cc_code"].Value.ToString();
				if (printer == "second printer")
				{
					string cat = Program.GetCatByItemCode(code);
					if (Program.m_sSecondPrinterCatalog.IndexOf(cat) == -1 && cat != "" && cat != null)
						continue;
				}
				supplier_code = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
				name = cart.Rows[i].Cells["cc_name"].Value.ToString();
				cn_name = cart.Rows[i].Cells["cc_name_cn"].Value.ToString();
//				en_name = cart.Rows[i].Cells["cc_name_en"].Value.ToString();
				en_name = cart.Rows[i].Cells["cc_name"].Value.ToString();
				price = Program.MyMoneyParse(cart.Rows[i].Cells["cc_price"].Value.ToString()).ToString("c");
				qty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				discount = cart.Rows[i].Cells["cc_discount"].Value.ToString();
				avoid_point = cart.Rows[i].Cells["cc_avoid_point"].Value.ToString();
				sprice = cart.Rows[i].Cells["cc_price"].Value.ToString();
				sNormalPriceOnCart = cart.Rows[i].Cells["cc_normalprice"].Value.ToString();
				sRowTotal = cart.Rows[i].Cells["cc_total"].Value.ToString();

				dTax = Program.MyDoubleParse(sRowTotal) - Program.MyDoubleParse(sRowTotal) / (1 + GetItemTaxRate(code));
				if (cbExport.Checked)
					dTax = 0;

				dDiscountRate = Math.Round((Program.MyDoubleParse(discount) / Program.MyDoubleParse(sNormalPriceOnCart)), 2);
				s_PromtiontItem = cart.Rows[i].Cells["cc_is_promotion"].Value.ToString();
				m_dNormalSubTotal = Program.MyDoubleParse(sNormalPriceOnCart) * Program.MyDoubleParse(qty);
				has_scale = cart.Rows[i].Cells["has_scale"].Value.ToString();

				if (Program.MyDoubleParse(discount) > 0.01)
					sTitle = "@" + dDiscountRate.ToString("p") + " DIS";

				dNormalPrice = GetNormalPrice(code);
				if (dNormalPrice <= 0.05)
					dNormalPrice = Program.MyDoubleParse(sprice) + Program.MyDoubleParse(discount);
				if (s_PromtiontItem != "0")
				{
					price = (Program.MyDoubleParse(sRowTotal) / Program.MyDoubleParse(qty)).ToString();
					sprice = Math.Round(Program.MyDoubleParse(price), 3).ToString();
					price = Math.Round(Program.MyDoubleParse(price), 4).ToString("c");
				}
				dSavePerItem = (dNormalPrice - Program.MyDoubleParse(sprice)) * Program.MyDoubleParse(qty);
				dSavePerItem = Math.Round(dSavePerItem, 2);
				dCountTotalDiscount += dSavePerItem;

				//dRowSub = Program.MyDoubleParse(sprice) * Program.MyDoubleParse(qty);
				m_dReceiptSubTotal += Math.Round(Program.MyDoubleParse(sRowTotal), 2);
				m_dReceiptGstTotal += Math.Round(dTax, 2);


				SplitAndInput.SplitAndInput en_name_Split = new SplitAndInput.SplitAndInput();
				List<string> new_en_name = en_name_Split.listBoxWordWrapComa(en_name, Program.m_nPaperWidth);
				if (new_en_name.Count > 0)
				{
					en_name = "";
					for (int m = 0; m < new_en_name.Count; m++)
					{
						if(m < new_en_name.Count - 1)
							en_name += new_en_name[m] + "\r\n";
						else if (m == new_en_name.Count - 1)
							en_name += new_en_name[m];
					}
					//foreach (var line in new_en_name)
					//{
					//    en_name += line + "\r\n";
					//}
				}
				string sTitleCn = "";
				if (!Program.m_bEnableSimpleInvoice)
				{
					if (en_name != "")
					{
						s += en_name + "\r\n";
					}
					if (cn_name != "")
					{
						s += cn_name;
						if (dSavePerItem >= 0.05)
						{
							sTitleCn = "";
							s += sTitleCn + "\r\n";
						}
						else
						{
							s += "\r\n";
						}
					}
					string sUnitPrice = sprice;
					if (sTitle != "")
						sUnitPrice = sNormalPriceOnCart;
					if (has_scale.ToLower() != "true")
						s += Program.PrintPadding("          " + qty.ToString() + " @ " + Program.MyMoneyParse(Program.MyDoubleParse(sprice).ToString("N2")).ToString("c"), double.Parse(sRowTotal).ToString("c"), " ") + "\r\n";
					else
						s += Program.PrintPadding("          " + qty.ToString() + " kg NET @ " + Program.MyMoneyParse(Program.MyDoubleParse(sprice).ToString("N2")).ToString("c") + "/kg", double.Parse(sRowTotal).ToString("c"), " ") + "\r\n";
					//if (sTitle != "")
					//	s += Program.PrintPadding(sTitle, " ", " ") + "\r\n";
					//if (dSavePerItem >= 0.05)
					//	s += Program.PrintPaddingSave("*" + sTitle + "", "-" + dSavePerItem.ToString("c"), " ") + "\r\n";
					sTitle = "";
					sTitleCn = "";
				}
				else if (Program.m_bEnableSimpleInvoice)
				{
					string item_name = "";
					if (en_name != "")
					{
						item_name = en_name;
					}
					else
						item_name = cn_name;

					if (has_scale.ToLower() != "true")
					{
						if (item_name.Length < Program.m_nPaperWidth / 3)
							s += Program.PrintPadding(item_name + " " + qty.ToString() + "  @" + price, double.Parse(sRowTotal).ToString("c"), " ") + "\r\n";
						else
						{
							s += item_name + "\r\n";
							s += Program.PrintPadding(qty.ToString() + "  @" + price, double.Parse(sRowTotal).ToString("c"), " ") + "\r\n";
						}

					}
					else
					{
						if (item_name.Length < Program.m_nPaperWidth / 3)
							s += Program.PrintPadding(item_name + " " + qty.ToString() + "kg  @" + price + "/kg", double.Parse(sRowTotal).ToString("c"), " ") + "\r\n";
						else
						{
							s += item_name + "\r\n";
							s += Program.PrintPadding(qty.ToString() + "kg  @" + price + "/kg", double.Parse(sRowTotal).ToString("c"), " ") + "\r\n";
						}
					}
					s += Program.PrintPadding("", "", "-") + "\r\n"; ;
					sTitle = "";
					sTitleCn = "";
				}
			}
			if (printer == "default printer")
			{
				if (m_dReceiptSubTotal < 0)
					s = s.Replace("Tax Invoice", "Credit Note");
				dPrintSubTotal = Math.Round(m_dReceiptSubTotal, 2);
				m_dDiscountTotal = Math.Round(dCountTotalDiscount, 2);

				if (m_sNote != "")
				{
					s += "\r\n" + m_sNote + "\r\n";
				}

				s += "\r\n";
				//s += this.showItems.Text + "Items\r\n";
				s += cart.Rows.Count + "Items\r\n";
				//s += Program.PrintPadding("SUB-TOTAL :    ", dPrintSubTotal.ToString("c"), "  ") + "\r\n";
				//if (m_dDiscountTotal > 0.05)
				//	s += Program.PrintPadding("TOTAL DISCOUNT:",m_dDiscountTotal.ToString("c"), " ") + "\r\n";
				s += Program.PrintPadding("", "", "-") + "\r\n";
				//			s += Program.PrintPadding("", "", "=") + "\r\n";
				if (m_dTotal < 0)
				{
					s += Program.PrintPadding("REFUNDED TOTAL:", m_dTotal.ToString("c"), " ") + "\r\n";
					// dTax = m_dTotal / 115 * 15;
				}
				else
				{
					s += Program.PrintPadding("TOTAL:", m_dTotal.ToString("c"), " ") + "\r\n";
					//dTax = m_dTotal / 115 * 15;
				}
				//s += Program.PrintPadding("","", "=") + "\r\n";
				// s += Program.PrintPadding("Tax:",)
				m_dCash = Program.MyMoneyParse(Cash.Text);
				if (m_dEftposTotal == 0 && m_dCash > 0 && m_dChequeTotal == 0 && m_dCreditTotal == 0)
				{
					s += Program.PrintPadding("CASH IN:", m_dCashInTotal.ToString("c"), " ") + "\r\n";
					if (this.Change.Text != "")
						s += Program.PrintPadding("CHANGE:", this.Change.Text, " ") + "\r\n";
				}
				else
				{
					if (m_dCashTotal != 0)
						s += Program.PrintPadding("CASH:", m_dCashInTotal.ToString("c"), " ") + "\r\n";
					if (m_dEftposTotal != 0)
						s += Program.PrintPadding("EFTPOS:", m_dEftposTotal.ToString("c"), " ") + "\r\n";
					if (m_dCashoutTotal != 0 && Program.MyDoubleParse(m_sCashMaxCashout) != 0)
						s += Program.PrintPadding("CASH OUT:", m_dCashoutTotal.ToString("c"), " ") + "\r\n";
					if (m_dCreditTotal != 0)
						s += Program.PrintPadding("CREDIT CARD:", m_dCreditTotal.ToString("c"), " ") + "\r\n";
					if (m_dChequeTotal != 0)
					{
						s += Program.PrintPadding("DIGITAL:", m_dChequeTotal.ToString("c"), " ") + "\r\n";
						//s += Program.PrintPadding("CHEQUE:", m_dChequeTotal.ToString("c"), " ") + "\r\n";
					}
					if (this.Change.Text != "" && this.labelChange.Text == "CHANGE")
						s += Program.PrintPadding("CHANGE:", this.Change.Text, " ") + "\r\n";
				}
				//s += Program.PrintPadding("", "", "--") + "\r\n";
				//			s += Program.PrintPadding("TAX:"   , (m_dTotal - m_dTotal / (1 + GetItemTaxRate(code))).ToString("c"), " ") + "\r\n";
				s += Program.PrintPadding("INCLUDE TAX:", m_dReceiptGstTotal.ToString("c"), " ") + "\r\n";
				s += Program.PrintPadding("", "", "-") + "\r\n";
				if (m_dChargeTotal > 0.05)
					s += Program.PrintPadding("CHARGE:", m_dChargeTotal.ToString("c"), " ") + "\r\n";
				if (m_dSpecialTotal > 0)
				{
					s += Program.PrintPadding("", "", "-") + "\r\n";
					s += Program.PrintPadding("TOTAL SAVED:", m_dSpecialTotal.ToString("c"), " ") + "\r\n";
				}
				if (m_dRoundingTotal != 0)
					s += Program.PrintPadding("TOTAL ROUNDING:", m_dRoundingReceipt.ToString("c"), " ") + "\r\n";
				s += "\r\n";

				if (MemberShipID.Text != "" && m_bExistingMember)
				{
					s += Program.PrintPadding("", "", "-") + "\r\n";
					s += Program.PrintPadding("Membership ID:", m_sCardBarcode, " ") + "\r\n";
					s += Program.PrintPadding("Name:", m_sCardName, " ") + "\r\n";
					if (m_bVipVoucherEnabled)
						s += Program.PrintPadding("Points:", m_nMemberPoints.ToString(), " ") + "\r\n";
					else if (Program.m_bVoucherUseWebService)
						s += Program.PrintPadding("Points:", labelPoints.Text, " ") + "\r\n";
					s += vipinfo + "\r\n";
					//s += Program.PrintPadding("Points On This INV:", Program.MyIntParse(m_dPointEarnOnSales.ToString()).ToString(), " ") + "\r\n";
					//s += Program.PrintPadding("Total Points:", m_nMemberPoints.ToString(), " ") + "\r\n";
				}
				if (dPrintSubTotal < 0)
				{
					s += Program.PrintPadding("", "", "--") + "\r\n\r\n";
					s += Program.PrintPadding("Customer Name:", "____________________________", " ") + "\r\n\r\n";
					s += Program.PrintPadding("Contact PH:", "______________________________", " ") + "\r\n\r\n";
					s += Program.PrintPadding("Customer Signature:", "____________________________", " ") + "\r\n";
				}
				if (m_sReceipt != "")
				{
					s += Program.PrintPadding("", "", "-") + "\r\n\r\n";
					s += m_sReceipt + "\r\n";
				}
			}
			s += Program.PrintPadding("", "", "=") + "\r\n";
			string sNVR = sNVRHeader + s;
			if (Program.m_bEnableNVR)
				DoSendTextToNVR(sNVR);
			
			s = sHeader + s;

			string sFooter = ReadSitePage("pos_receipt_footer");
			if (cbExport.Checked)
				sFooter = Program.ReadSitePage("pos_export_footer");
			sFooter = sFooter.Replace("@@member_points_msg", "");
			s += sFooter;
			s += " \r\n\r\n";

			s += eftPanel.m_sReceiptCust;

			if (m_sReceiptVoucher != "" && printer == "default printer")
			{
				s += "\r\n" + m_sReceiptVoucher;
				m_sReceiptVoucher = "";
			}
			if (printer == "default printer")
				m_sLastReceipt = s;
			else if (printer == "second printer")
				m_sLastSecondReceipt = s;
			
			if (m_sCurrentCartID == 0)
				m_sLastCart1 = s;
			else if (m_sCurrentCartID == 1)
				m_sLastCart2 = s;
			else if (m_sCurrentCartID == 2)
				m_sLastCart3 = s;
			else if (m_sCurrentCartID == 3)
				m_sLastCart4 = s;
		}
		private bool RecordAllPayment()
		{
			if (m_dCashTotal != 0)
			{
				if (!DoReceiveOnePayment("cash", m_dCashTotal))
					return false;
			}
			if (m_dEftposTotal != 0)
			{
				if (!DoReceiveOnePayment("eftpos", m_dEftposTotal))
					return false;
			}
			if (m_dCreditTotal != 0)
			{
				if (!DoReceiveOnePayment("credit card", m_dCreditTotal))
					return false;
			}
			if (m_dChequeTotal != 0)
			{
				if (!DoReceiveOnePayment("cheque", m_dChequeTotal))
					return false;
			}
			if (m_dRoundingTotal != 0)
			{
				if (!DoReceiveOnePayment("cash rounding", m_dRoundingTotal))
					return false;
			}
			if (m_dChargeTotal != 0)
			{
				if (!DoReceiveOnePayment("charge", m_dChargeTotal))
					return false;
			}
			if (m_dCashoutTotal != 0)
			{
				if (!DoReceiveOnePayment("cash out", m_dCashoutTotal))
					return false;
			}
			return true;
		}
		private bool ConfirmOnePayment()
		{
			if (RecordOnePayment())
			{
				ResetPaymentLabels();
				return true;
			}
			return false;
		}
		private bool RecordOnePayment()
		{
			return RecordOnePayment(true);
		}
		private bool RecordOnePayment(bool bMsg)
		{
			double dRoundTotal = 0;
			if (m_dCash > 0)
				m_dCash -= Program.MyMoneyParse(Change.Text);
			m_dCashTotal += m_dCash;
			m_dEftposTotal += m_dEftpos;
			m_dCashoutTotal += m_dCashout;
			m_dCreditTotal += m_dCredit;
			m_dChequeTotal += m_dCheque;
			m_dTotalPaid = m_dCashTotal + m_dEftposTotal + m_dCreditTotal + m_dChequeTotal + m_dChargeTotal;
			TotalPaid.Text = m_dTotalPaid.ToString("c");
			dRoundTotal = m_dTotal - m_dTotalPaid;
			dRoundTotal = Math.Round(dRoundTotal, 2);
			if (dRoundTotal < 0)
				dRoundTotal = 0 - dRoundTotal;
			m_dCash = 0;
			m_dEftpos = 0;
			m_dCashout = 0;
			m_dCredit = 0;
			m_dCheque = 0;
			if (bMsg)
			{
				ResetPaymentLabels();
			}
			if (dRoundTotal > 0.05)
			{
				double dBalance = Program.MyDoubleParse(GetBalance());
				if (dBalance + m_dCashoutTotal >= 0)
					labelBalance.Text = "";
				else
					labelBalance.Text = dBalance.ToString();
				m_fmad.TotalPrice.Text = this.labelBalance.Text;
				m_fmads.TotalPrice.Text = this.labelBalance.Text;
			}
			return true;
		}
		private bool CreateOrder()
		{
			string note = "";
			if (m_sNote != "")
				note = m_sNote;
			if (eftPanel.m_sReceiptCust != "")
			{
				if (note != "")
					note += "\r\n";
				note += eftPanel.m_sReceiptCust;
			}
			//			BuildReceipt();
			DataSet dsco = new DataSet();
			string sc = "BEGIN TRANSACTION ";
			sc += " INSERT INTO orders (number, branch, card_id, sales, unchecked, station_id, order_total, total_special, total_discount, sales_note) ";
			sc += " VALUES(0, '" + m_nBranchId + "', '" + m_nCardId + "', '" + m_nSalesId.ToString() + "', 0 ";
			sc += ", " + Program.m_sStationID + ", " + m_dOrderTotal + ", " + m_dSpecialTotal + ", " + m_dDiscountTotal + ", N'" + Program.EncodeQuote(note) + "') ";
//			sc += " SELECT IDENT_CURRENT('orders') AS id";
            sc += " SELECT TOP 1 id FROM orders WHERE station_id = " + Program.m_sStationID + " ORDER BY id DESC ";
			sc += " COMMIT ";

			if (bIs_Refund)
			{
				sc += " INSERT INTO refund_list (refund_id,refund_inv, refund_total, refund_by, refund_till) SELECT IDENT_CURRENT('orders') AS id, ";
				sc += " 0, '" + m_dOrderTotal + "',N'" + this.SalesName.Text + "', " + Program.m_sStationID + "FROM orders WHERE id = IDENT_CURRENT('orders') ";
			}
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				if (myCommand1.Fill(dsco, "id") == 1)
				{
					m_nOrderId = Program.MyIntParse(dsco.Tables["id"].Rows[0]["id"].ToString());
				}
				else
				{
					MessageBox.Show("Create Order failed, error getting new order number", "Error");
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}

			//write order item
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string code = cart.Rows[i].Cells["cc_code"].Value.ToString();
				string supplier_code = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
				string name = cart.Rows[i].Cells["cc_name_en"].Value.ToString();
				string name_cn = cart.Rows[i].Cells["cc_name_cn"].Value.ToString();
				string price = cart.Rows[i].Cells["cc_price"].Value.ToString();
				string qty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				string itemSubTotal = cart.Rows[i].Cells["cc_total"].Value.ToString();
//				price = Math.Round(Program.MyDoubleParse(price) / (1 + GetItemTaxRate(code)), 4).ToString(); //Record Price without GST CH 24/11/08
				double dTaxRate = 0;//GetItemTaxRate(code);
				string sTaxCode = "";//GetItemTaxCode(code);
				if (m_sVoucherList.IndexOf(supplier_code) >= 0)
					Program.RecordTillData("Voucher", itemSubTotal);

				string supplier = "";
				string supplier_price = "";
				string cat = "";
				string s_cat = "";
				string ss_cat = "";
				string avg_cost = "";

				if (dsco.Tables["sup"] != null)
					dsco.Tables["sup"].Clear();
				sc = " SELECT TOP 1 supplier, supplier_code, supplier_price, cat, s_cat, ss_cat ";
                sc += ", tax_rate, tax_code, average_cost "; // ISNULL(manual_cost_frd, '0') AS average_cost ";
                //sc += " FROM code_relations WHERE supplier_code = '" + supplier_code + "' ";
                sc += " FROM code_relations WHERE code = '" + code + "' ";
				try
				{
					SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
					if (myCommand1.Fill(dsco, "sup") == 1)
					{
						DataRow dr = dsco.Tables["sup"].Rows[0];
						supplier = dr["supplier"].ToString();
						avg_cost = dr["average_cost"].ToString();
						supplier_price = avg_cost;
						cat = dr["cat"].ToString();
						s_cat = dr["s_cat"].ToString();
						ss_cat = dr["ss_cat"].ToString();
						dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
						sTaxCode = dr["tax_code"].ToString();
					}
					else
					{
						if(Program.MyIntParse(code) < 1020) //service item
						{
							supplier = "GPOS";
							avg_cost = "0";
							supplier_price = "0";
							cat = "ServiceItem";
							s_cat = "";
							ss_cat = "";
							dTaxRate = 0.15;
							sTaxCode = "";
						}
						else
						{
							MessageBox.Show("CreateOrder failed, error getting supplier data, code=" + code, "Error");
							return false;
						}
					}
				}
				catch (Exception e)
				{
					myConnection.Close();
					Program.ShowExp(sc, e);
					return false;
				}

				double dQty = Program.MyDoubleParse(qty);
				double dOrderTotal = Program.MyDoubleParse(itemSubTotal);
				double dCommitPrice = dOrderTotal / dQty / (1 + dTaxRate);

				if (name.Length > 255)
					name = name.Substring(0, 255);

				if (cbExport.Checked)
				{
					double dPrice = Program.MyDoubleParse(price);
					dPrice = dPrice * (1 + dTaxRate);
					price = Math.Round(dPrice, 4).ToString();
					dTaxRate = 0;
					sTaxCode = "";
				}
				sc = " INSERT INTO order_item (id, code, item_name, item_name_cn, commit_price, quantity, supplier, supplier_code, supplier_price, cat, s_cat, ss_cat ";
				sc += " ,order_total, station_id, tax_code, tax_rate) VALUES('" + m_nOrderId + "', '" + code + "' ";
				sc += ", N'" + Program.EncodeQuote(name) + "', N'" + Program.EncodeQuote(name_cn) + "', " + dCommitPrice + ", '" + qty + "' ";
				sc += ", '" + Program.EncodeQuote(supplier) + "', '" + Program.EncodeQuote(supplier_code) + "' ";
				sc += ", " + supplier_price + ", N'" + Program.EncodeQuote(cat) + "', N'" + Program.EncodeQuote(s_cat) + "',N'" + Program.EncodeQuote(ss_cat) + "' ";
				sc += ", '" + itemSubTotal + "', " + Program.m_sStationID + " ";
				sc += ", N'" + sTaxCode + "', " + dTaxRate + ") ";
				if (bIs_Refund)
				{
					sc += " INSERT INTO refund_item (item_id, item_code, item_price, item_supplier_code) ";
					sc += " VALUES ('" + m_nOrderId + "', '" + code + "',' " + price + "', N'" + supplier_code + "')";
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
					return false;
				}
			}
			//			if (this.charge.Text != null && m_dChargeTotal > 0)
			if (this.charge.Text != null && this.charge.Text != "")
			{
				if (!CreateInvoiceCharge(m_nOrderId.ToString()))
					return false;
			}
			else
			{
				if (!CreateInvoice(m_nOrderId.ToString()))
					return false;
			}
			return true;
		}
		private bool CreateInvoice(string id)
		{
			if (id == "")
				return false;

			string sc = "";

			DataRow dr = null;
			double dPrice = 0;
			double dFreight = 0;
			double dTax = 0;
			double dTotal = 0;
			double dInvoiceTotal = 0;
			int rows = 0;
			if (dst.Tables["invoice"] != null)
				dst.Tables["invoice"].Clear();
			sc = "SELECT o.*, c.name AS sales_name ";
			sc += " FROM orders o ";
			sc += " LEFT OUTER JOIN card c ON c.id = o.sales ";
			sc += " WHERE o.id = " + id;
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				rows = myCommand1.Fill(dst, "invoice");
				if (rows != 1)
				{
					MessageBox.Show("Error creating invoice, Order id = " + id + ", rows return:" + rows, "Error");
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}

			dr = dst.Tables["invoice"].Rows[0];
			string card_id = dr["card_id"].ToString();
			string po_number = dr["po_number"].ToString();
			string shippingMethod = dr["shipping_method"].ToString();
			string pickupTime = dr["pick_up_time"].ToString();
			string branchId = dr["branch"].ToString();
			string agent = dr["agent"].ToString(); //agent enable
			string station_id = dr["station_id"].ToString();
			string sales = dr["sales_name"].ToString();
			dFreight = Math.Round(Program.MyDoubleParse(dst.Tables["invoice"].Rows[0]["freight"].ToString()), 4);

			if (dst.Tables["item"] != null)
				dst.Tables["item"].Clear();
			sc = " SELECT i.*, c.bom_id ";
			sc += " FROM order_item i ";
			sc += " LEFT OUTER JOIN code_relations c ON c.code = i.code ";
			sc += " WHERE i.id = " + id;
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				rows = myCommand1.Fill(dst, "item");
				if (rows <= 0)
				{
					MessageBox.Show("Error getting order items, order id = " + id + ", rows return : " + rows, "Error");
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			dTax = 0;
			for (int i = 0; i < dst.Tables["item"].Rows.Count; i++)
			{
				dr = dst.Tables["item"].Rows[i];
				string code = dr["code"].ToString();
				double dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				string sTaxCode = dr["tax_code"].ToString();//GetItemTaxCode(code);
				double dCommitPrice = Program.MyDoubleParse(dr["commit_price"].ToString());
				double qty = Program.MyDoubleParse(dr["quantity"].ToString());
				//				dTax += Math.Round(dCommitPrice * dTaxRate * qty, 4);
				double dp = Program.MyDoubleParse(dr["order_total"].ToString());
				dTax += Math.Round(dp - dp / (1 + dTaxRate), 4);
				dp = Math.Round(dp, 4);
				dPrice += dp / (1 + dTaxRate);
				dPrice = Math.Round(dPrice, 4);
			}

			//			dTax += dFreight * dTaxRate;
			//			dTax = Math.Round(dTax, 4);
			dTotal = dPrice + dFreight + dTax;
			dTotal = Math.Round(dTotal, 2);
			dInvoiceTotal = dTotal;
			dr = dst.Tables["invoice"].Rows[0];
			string special_shipto = "0";
			if (Program.MyBooleanParse(dr["special_shipto"].ToString()))
				special_shipto = "1";

			string receipt_type = "3"; //invoice
			string sbSystem = "0";
			if (Program.MyBooleanParse(dr["system"].ToString()))
				sbSystem = "1";

			if (dst.Tables["invoice_id"] != null)
				dst.Tables["invoice_id"].Clear();

			sc = " SET DATEFORMAT dmy ";
			sc += " BEGIN TRANSACTION ";
			sc += "INSERT INTO invoice (branch, type, card_id, price, tax, total, amount_paid, paid, commit_date, special_shipto, shipto ";
			sc += ", freight, cust_ponumber, shipping_method, pick_up_time, sales, sales_note, agent, station_id, uploaded)";
			sc += " VALUES('" + branchId + "', '" + receipt_type + "', '" + card_id + "', " + dPrice + " ";
			//			sc += ", " + dTax + ", ROUND(" + dTotal + ", 2), ROUND(" + dTotal + ", 2) ";
			sc += ", " + dTax + ", ROUND(" + dTotal + ", 2), 0 "; //record paid amount in ReceiveOnePayment()
			sc += ", 1 ";
			sc += ", GETDATE(), " + special_shipto + ", N'" + Program.EncodeQuote(dr["shipto"].ToString()) + "', " + dFreight + ", '" + po_number + "' ";
			sc += ", '" + shippingMethod + "', N'" + Program.EncodeQuote(pickupTime) + "', '" + Program.EncodeQuote(sales) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["sales_note"].ToString()) + "', '" + agent + "', " + station_id + ", 1) ";
//  		sc += " SELECT IDENT_CURRENT('invoice') AS id";
            sc += " SELECT TOP 1 id FROM invoice WHERE station_id = " + station_id + " AND invoice_number = 0 ORDER BY id DESC ";
			sc += " COMMIT ";

			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				if (myCommand1.Fill(dst, "invoice_id") == 1)
				{
					m_invoiceNumber = dst.Tables["invoice_id"].Rows[0]["id"].ToString();
				}
				else
				{
					MessageBox.Show("Error get new invoice number", "Error");
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}

			bool bHasKit = false;
			//write sales item
			sc = "";
			for (int i = 0; i < dst.Tables["item"].Rows.Count; i++)
			{
				dr = dst.Tables["item"].Rows[i];
				string commit_price = dr["commit_price"].ToString();
				string quantity = dr["quantity"].ToString();
				string code = dr["code"].ToString();
				string bom_id = dr["bom_id"].ToString();
				string name = dr["item_name"].ToString();
				string name_cn = dr["item_name_cn"].ToString();
				string kit = dr["kit"].ToString();
				string krid = dr["krid"].ToString();
				string supplier = dr["supplier"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string supplier_price = dr["supplier_price"].ToString();
				string sCat = dr["cat"].ToString();
				string sS_cat = dr["s_cat"].ToString();
				string sSS_cat = dr["ss_cat"].ToString();
				string order_total = dr["order_total"].ToString();
				double dNormalPrice = GetNormalPrice(code);
				if (dNormalPrice == 0)
					dNormalPrice = Program.MyDoubleParse(commit_price);
				double dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				string sTaxCode = dr["tax_code"].ToString();

				sbSystem = "0";
				if (bool.Parse(dr["system"].ToString()))
					sbSystem = "1";

				string sKit = "0";
				if (Program.MyBooleanParse(kit))
				{
					sKit = "1";
					bHasKit = true;
				}
				string didPayment = m_nSalesId.ToString();
				if (krid == "")
					krid = "null";
				sc += "INSERT INTO sales (invoice_number, code, name, name_cn, quantity, commit_price, supplier, supplier_code, supplier_price, system, kit, krid, normal_price, cat, s_cat, ss_cat, sales_total, station_id, tax_code, tax_rate) ";
				sc += " VALUES('" + m_invoiceNumber + "', '" + code + "', N'" + Program.EncodeQuote(name) + "',  N'" + Program.EncodeQuote(name_cn) + "','" + quantity + "', " + commit_price + ", ";
				sc += " N'" + Program.EncodeQuote(supplier) + "', N'" + Program.EncodeQuote(supplier_code) + "', " + supplier_price + ", '" + sbSystem + "', '" + sKit + "', " + krid + " ";
				sc += ", " + dNormalPrice + ", N'" + sCat + "', N'" + sS_cat + "', N'" + sSS_cat + "', '" + order_total + "', " + station_id + ", '" + sTaxCode + "'," + dTaxRate + ") ";
				//				sc += " UPDATE stock_qty SET qty = qty - " + Program.MyDoubleParse(quantity) + " WHERE code = '" + code + "' AND branch_id = '" + branchId + "' ";
				double dQty = Program.MyDoubleParse(quantity);
				DoUpdateStockQty(dQty, code, bom_id, branchId);
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
				return false;
			}
			if (bHasKit)
			{
				sc = " UPDATE till SET last_order_id = " + id + ", total_orders = total_orders + 1 ";
				sc += " WHERE station_id = " + Program.m_sStationID;
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
			}
//			sc = " UPDATE invoice SET invoice_number = id, uploaded = 0, uploaded_activata = 0, barcode = '" + MemberShipID.Text + "' WHERE id = '" + m_invoiceNumber + "' ";
//			sc = " UPDATE invoice SET invoice_number = id, uploaded = 0, uploaded_activata = 0 WHERE id = '" + m_invoiceNumber + "' ";
			sc = " UPDATE invoice SET invoice_number = id WHERE id = '" + m_invoiceNumber + "' ";
			sc += " UPDATE orders SET invoice_number = '" + m_invoiceNumber + "', status = 3 WHERE id = " + id;
			if (bIs_Refund)
				sc += " UPDATE refund_list SET refund_inv = '" + m_invoiceNumber + "' WHERE refund_id = " + id;
			// log
			if (this.Cash.Text != "" && this.Change.Text != "")
			{
				sc += " INSERT INTO winpos_log (invoice_number, total, cashin, change, station, sales) ";
				sc += " VALUES (" + m_invoiceNumber + ", " + m_dTotal + ", " + Program.MyMoneyParse(this.Cash.Text).ToString() + ", " + Program.MyMoneyParse(this.Change.Text).ToString();
				sc += ", " + Program.m_sStationID + ", " + m_nSalesId.ToString() + ")";
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
				return false;
			}
			if (Program.m_bEnableLatiPayGiftCard)
				DoUpdateLatipayGiftCard(m_invoiceNumber);
			return true;
		}

		/***********************Charge 11/10/2013 by Eric***********************************/
		private bool CreateInvoiceCharge(string id)
		{
			if (dst.Tables["invoice"] != null)
				dst.Tables["invoice"].Clear();
			DataRow dr = null;
			double dPrice = 0;
			double dFreight = 0;
			double dTax = 0;
			double dTotal = 0;
			int rows = 0;
			string sc = "SELECT o.*, c.name, c.company, c.trading_name, c.address1, c.address2, c.address3 ";
			sc += ", c.phone, c.fax, c.email, c.postal1, c.postal2, c.postal3 ";
			sc += " FROM orders o LEFT OUTER JOIN card c ON c.id = o.card_id ";
			sc += " WHERE o.id=" + id;
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				rows = myCommand1.Fill(dst, "invoice");
				if (rows != 1)
				{
					MessageBox.Show("Error creating invoice!");
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			dr = dst.Tables["invoice"].Rows[0];
            string station_id = dr["station_id"].ToString();
			string card_id = dr["card_id"].ToString();
			string po_number = dr["po_number"].ToString();
			string m_shippingMethod = dr["shipping_method"].ToString();
			string m_pickupTime = dr["pick_up_time"].ToString();
			string m_branch_id = dr["branch"].ToString();
			string quote_total = dr["quote_total"].ToString();
			double dQuoteTotal = 0;
			if (quote_total != "" && quote_total != "0")
				dQuoteTotal = Program.MyDoubleParse(quote_total);
			string nip = "0";
			if (bool.Parse(dr["no_individual_price"].ToString()))
				nip = "1";
			string gst_inclusive = "0";
			if (bool.Parse(dr["gst_inclusive"].ToString()))
				gst_inclusive = "1";

			//string sales_person = dr["name"].ToString();
			if (m_branch_id == "")
				m_branch_id = "1";

			string sales = dr["sales"].ToString();
			if (sales != "")
				sales = TSGetUserNameByID(sales);

			dFreight = Program.MyDoubleParse(dst.Tables["invoice"].Rows[0]["freight"].ToString());

			if (dst.Tables["item"] != null)
				dst.Tables["item"].Clear();

			sc = "SELECT i.*, c.bom_id ";
			sc += " FROM order_item i ";
			sc += " LEFT OUTER JOIN code_relations c ON c.code = i.code ";
			sc += " WHERE i.id = " + id;
			sc += " ORDER BY i.kid ";
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				rows = myCommand1.Fill(dst, "item");
				if (rows <= 0)
				{
					MessageBox.Show("Error getting order items!");
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			dTax = 0;
			for (int i = 0; i < dst.Tables["item"].Rows.Count; i++)
			{
				dr = dst.Tables["item"].Rows[i];
				double dp = Program.MyDoubleParse(dr["commit_price"].ToString());
				double dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				double dDiscountPercent = Program.MyDoubleParse(dr["discount_percent"].ToString());
                double order_total = Program.MyDoubleParse(dr["order_total"].ToString());
				dDiscountPercent /= 100;
				dp = Math.Round(dp, 4);
				double qty = Program.MyDoubleParse(dr["quantity"].ToString());
                //dPrice += (dp * (1 - dDiscountPercent)) * qty;
                dPrice += order_total;
				dPrice = Math.Round(dPrice, 4);
				//				dTax += dp / (1 + dTaxRate);
                //dTax += Math.Round(dp * dTaxRate * qty, 4);
                dTax += Math.Round(order_total - order_total / (1 + dTaxRate));
			}
			if (dQuoteTotal != 0)
				dPrice = dQuoteTotal; //keep quotation discount

			dTax = Math.Round(dTax, 4);
//          dTotal = dPrice + dFreight + dTax;
            dTotal = dPrice;
			dr = dst.Tables["invoice"].Rows[0];
			string special_shipto = "0";
			if (bool.Parse(dr["special_shipto"].ToString()))
				special_shipto = "1";

			string receipt_type = GetEnumID("receipt_type", "invoice");
			string salesNote = dr["sales_note"].ToString();
			string sbSystem = "0";
			if (bool.Parse(dr["system"].ToString()))
				sbSystem = "1";
			if (dst.Tables["invoice_id"] != null)
				dst.Tables["invoice_id"].Clear();

			sc = "BEGIN TRANSACTION ";

			sc += " SET DATEFORMAT dmy ";
			sc += "INSERT INTO invoice (branch, type, card_id, price, tax, total, commit_date, special_shipto, shipto ";
			sc += ", name, company, trading_name, address1, address2, address3, postal1, postal2, postal3, phone, fax, email ";
			sc += ", freight, cust_ponumber, shipping_method, pick_up_time, sales, sales_note, system, payment_type ";
			sc += ", no_individual_price, gst_inclusive ";
			sc += ", customer_gst, uploaded, station_id ";
			sc += ")";
			sc += " VALUES(" + m_branch_id + ", " + receipt_type + ", " + dr["card_id"].ToString() + ", " + dPrice;
			sc += ", " + dTax + ", ROUND(" + dTotal + ", 2) ";
			sc += ", GETDATE() ";
			sc += ", N'" + special_shipto + "', N'" + Program.EncodeQuote(dr["shipto"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["name"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["company"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["trading_name"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["address1"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["address2"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["address3"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["postal1"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["postal2"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["postal3"].ToString()) + "' ";
			sc += ", N'" + Program.EncodeQuote(dr["phone"].ToString()) + "' ";
			sc += ", '" + Program.EncodeQuote(dr["fax"].ToString()) + "' ";
			sc += ", '" + Program.EncodeQuote(dr["email"].ToString()) + "' ";
			sc += ", " + dFreight + ", N'" + Program.EncodeQuote(po_number) + "', ";
			sc += m_shippingMethod + ", '" + Program.EncodeQuote(m_pickupTime) + "', '" + Program.EncodeQuote(sales) + "', N'";
			sc += Program.EncodeQuote(dr["sales_note"].ToString()) + "' ";
			sc += ", " + sbSystem + ", " + dr["payment_type"].ToString();
			sc += ", " + nip;
			sc += ", " + gst_inclusive;
			sc += ", 0, 1,"+station_id+")";
//			sc += " SELECT IDENT_CURRENT('invoice') AS id";
            sc += " SELECT TOP 1 id FROM invoice WHERE station_id = " + station_id + " AND invoice_number = 0 ORDER BY id DESC ";
			sc += " COMMIT ";
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				if (myCommand1.Fill(dst, "invoice_id") == 1)
				{
					m_invoiceNumber = dst.Tables["invoice_id"].Rows[0]["id"].ToString();
				}
				else
				{
					MessageBox.Show("Error get new invoice number");
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}

			bool bHasKit = false;
			for (int i = 0; i < dst.Tables["item"].Rows.Count; i++)
			{
				dr = dst.Tables["item"].Rows[i];
				string commit_price = dr["commit_price"].ToString();
				string quantity = dr["quantity"].ToString();
				string code = dr["code"].ToString();
				string bom_id = dr["bom_id"].ToString();
				string name = dr["item_name"].ToString();
				string kit = dr["kit"].ToString();
				string krid = dr["krid"].ToString();
				string supplier = dr["supplier"].ToString();
				string supplier_code = dr["supplier_code"].ToString();
				string supplier_price = dr["supplier_price"].ToString();

				string sCat = dr["cat"].ToString();
				string sS_cat = dr["s_cat"].ToString();
				string sSS_cat = dr["ss_cat"].ToString();
				string order_total = dr["order_total"].ToString();
//				string station_id = dr["station_id"].ToString();

				string sTaxCode = dr["tax_code"].ToString();
				double dTaxRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
				double dNormalPrice = Program.MyDoubleParse(commit_price);//price1 / (1 + dTaxRate);

				sbSystem = "0";
				if (bool.Parse(dr["system"].ToString()))
					sbSystem = "1";
				if (name.Length > 255)
					name = name.Substring(0, 255);

				string sKit = "0";
				if (Program.MyBooleanParse(kit))
				{
					sKit = "1";
					bHasKit = true;
				}
				if (krid == "")
					krid = "null";

				sc = "BEGIN TRANSACTION INSERT INTO sales (invoice_number, code, name, quantity, commit_price, supplier, supplier_code, supplier_price, system, kit, krid, cat, s_cat, ss_cat, sales_total, station_id ";
				sc += ", income_account, costofsales_account , discount_percent, normal_price, tax_code, tax_rate";
				sc += ")";
				sc += " SELECT " + m_invoiceNumber + ", " + code + ", N'" + Program.EncodeQuote(name) + "', " + quantity + ", " + commit_price + ", ";
				sc += "'" + supplier + "', '" + supplier_code + "', " + supplier_price + ", " + sbSystem + ", " + sKit + ", " + krid + " ";
				sc += ", N'" + sCat + "', N'" + sS_cat + "', N'" + sSS_cat + "', '" + order_total + "', " + station_id + " ";
				sc += ", income_account,   costofsales_account , " + dr["discount_percent"].ToString();
				sc += "," + dNormalPrice + ", '" + sTaxCode + "', '" + dTaxRate + "'";
				sc += " FROM code_relations WHERE code = " + code + " ";
				sc += " COMMIT ";
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
				double nQty = Program.MyDoubleParse(quantity);
				DoUpdateStockQty(nQty, code, bom_id, m_branch_id);
			}
			//update order to record invoice number
			sc = "";
			sc += " UPDATE orders SET invoice_number = " + m_invoiceNumber + " WHERE id = " + id;
//			sc += " UPDATE sales_serial SET invoice_number = " + m_invoiceNumber + " WHERE order_id = " + m_nOrderId.ToString();
//			sc += " UPDATE serial_trace SET invoice_number = " + m_invoiceNumber + " WHERE order_id = " + m_nOrderId.ToString();
//			sc += " UPDATE invoice SET invoice_number = id, uploaded = 0, barcode = '" + MemberShipID.Text + "' WHERE id = " + m_invoiceNumber; //this for qpos
			sc += " UPDATE invoice SET invoice_number = id WHERE id = " + m_invoiceNumber; //this for qpos
			sc += " UPDATE settings SET value = '" + (int.Parse(m_invoiceNumber) + 1).ToString() + "' WHERE name = 'qpos_next_invoice_number' ";
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
			if(Program.m_bEnableLatiPayGiftCard)
				DoUpdateLatipayGiftCard(m_invoiceNumber);

			if (bHasKit)
			{
				if (!RecordKitToInvoice(id, m_invoiceNumber))
					return true;
			}

			UpdateCardAverage(card_id, dPrice, Program.MyIntParse(DateTime.Now.ToString("MM")));
			UpdateCardBalance(card_id, dTotal);

			//****update and delete file that create in importing by file ****//
			//          doDeleteImportedFile(id);

			/*	///create a invoice for qpos///
				Response.Write("<form name=f1 ><input type=hidden name=next_inv value="+ (int.Parse(m_invoiceNumber) + 1ss).ToString()+"></form>");
				Response.Write("<script language=javascript> ");
				string s = @" 
					var fn = 'c:/qpos/qposni.txt'; 
				var inv = Number(document.f1.next_inv.value);
				fso = new ActiveXObject('Scripting.FileSystemObject'); 	
					fso.DeleteFile(fn);
					tf = fso.OpenTextFile(fn , 8, 1, -2);
					tf.Write(inv);
					tf.Close();	
					";
					Response.Write(s);
					Response.Write(" </script ");
					Response.Write(">");
					*/
			return true;
		}

		private double GetItemTaxRate(string code)
		{
			code = code.Trim();
			if (code == "")
				return 0;
			if (dst.Tables["gitr"] != null)
				dst.Tables["gitr"].Clear();
			string sc = " SELECT tax_rate FROM code_relations WHERE code = " + code;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				if (myCommand.Fill(dst, "gitr") <= 0)
					return 0;
			}
			catch (Exception e)
			{
				MessageBox.Show("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return 0;
			}
			DataRow dr = dst.Tables["gitr"].Rows[0];
			double dRate = Program.MyDoubleParse(dr["tax_rate"].ToString());
			return dRate;
		}

		private string GetItemTaxCode(string code)
		{
			code = code.Trim();
			if (code == "")
				return "0.15";
			if (dst.Tables["GetItemTaxCode"] != null)
				dst.Tables["GetItemTaxCode"].Clear();
			string sc = " SELECT tax_code FROM code_relations WHERE code = " + code;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				if (myCommand.Fill(dst, "GetItemTaxCode") <= 0)
					return "0.15";
			}
			catch (Exception e)
			{
				MessageBox.Show("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return "0.15";
			}
			DataRow dr = dst.Tables["GetItemTaxCode"].Rows[0];
			string sCode = dr["tax_code"].ToString();
			return sCode;
		}
		private DataRow GetCardData(string id)
		{
			Program.Trim(ref id);
			if (id == null || id == "")
				return null;

			DataSet dsa = new DataSet();
			string sc = "SELECT * FROM card WHERE id=" + id;

			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dsa, "card") == 1)
					return dsa.Tables["card"].Rows[0];
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return null;
			}
			return null;
		}
		private string TSGetUserNameByID(string id) //get user name from account table according to user id
		{
			DataSet dsu = new DataSet();
			string sc = "SELECT name FROM card WHERE id='" + id + "'";
			try
			{
				//		SqlConnection myConnection = new SqlConnection("Initial Catalog=eznz;" + m_sDataSource + m_sSecurityString);
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				int rows = myCommand.Fill(dsu);
				if (rows > 0)
					return dsu.Tables[0].Rows[0]["name"].ToString();
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "error";
			}
			return "user not found";
		}
		private bool RecordKitToInvoice(string orderID, string invoice_number)
		{
			DataSet dsi = new DataSet();

			string sc = "SELECT k.*, o.branch ";
			sc += " FROM order_kit k JOIN orders o ON k.id=o.id ";
			sc += " WHERE o.id=" + orderID;
			sc += " ORDER BY k.krid ";
			int rows = 0;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dsi, "kit");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}

			if (rows <= 0)
				return true;

			string branch_id = dsi.Tables["kit"].Rows[0]["branch"].ToString();

			for (int i = 0; i < dsi.Tables["kit"].Rows.Count; i++)
			{
				DataRow dr = dsi.Tables["kit"].Rows[i];

				//record kit
				string kit_id = dr["kit_id"].ToString();
				string krid = dr["krid"].ToString();
				string qty = dr["qty"].ToString();
				string name = dr["name"].ToString();
				string details = dr["details"].ToString();
				string warranty = dr["warranty"].ToString();
				string base_selling_price = dr["base_selling_price"].ToString();
				string commit_price = dr["commit_price"].ToString();

				double dQty = Program.MyDoubleParse(qty);

				sc = " INSERT INTO sales_kit (krid, invoice_number, kit_id, qty, name, details, warranty, base_selling_price, commit_price) ";
				sc += " VALUES (";
				sc += krid;
				sc += ", " + invoice_number;
				sc += ", " + kit_id;
				sc += ", " + qty;
				sc += ", '" + Program.EncodeQuote(name) + "' ";
				sc += ", '" + Program.EncodeQuote(details) + "' ";
				sc += ", '" + Program.EncodeQuote(warranty) + "' ";
				sc += ", " + base_selling_price;
				sc += ", " + commit_price;
				sc += ") ";

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
			}
			return true;
		}
		private bool DoUpdateStockQty(double qty, string code, string bom_id, string branch_id)
		{
			if (bom_id != "" && bom_id != "0")
			{
				return DoUpdateStockQtyByBom(qty, bom_id, branch_id);
			}
			string sc = " IF NOT EXISTS (SELECT code FROM stock_qty WHERE code = " + code + " AND branch_id = " + branch_id + ") ";
			sc += " INSERT INTO stock_qty (code, branch_id, qty) VALUES (" + code + ", " + branch_id + ", " + (0 - qty).ToString() + ") ";
			sc += " ELSE UPDATE stock_qty SET ";
			sc += " qty = qty - " + qty;
			sc += " WHERE code = " + code + " AND branch_id = " + branch_id;
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
		private bool DoUpdateStockQtyByBom(double dQty, string bom_id, string branch_id)
		{
			int nRows = 0;
			if (dst.Tables["bom"] != null)
				dst.Tables["bom"].Clear();
			string sc = " SELECT i.code, i.qty FROM bom_item i WHERE i.bom_id = " + bom_id;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "bom");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			sc = "";
			for (int i = 0; i < nRows; i++)
			{
				DataRow dr = dst.Tables["bom"].Rows[i];
				string code = dr["code"].ToString();
				string qty = dr["qty"].ToString();
                double dqty = dQty * Program.MyDoubleParse(qty);
				sc += " IF NOT EXISTS (SELECT code FROM stock_qty WHERE code = " + code + " AND branch_id = " + branch_id + ") ";
                sc += " INSERT INTO stock_qty (code, branch_id, qty) VALUES (" + code + ", " + branch_id + ", " + (0 - dqty).ToString() + ") ";
                sc += " ELSE UPDATE stock_qty SET qty = qty - " + dqty + " WHERE code = " + code + " AND branch_id = " + branch_id;
			}
			if (sc == "")
				return true;
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
		private bool UpdateCardBalance(string id, double dBalanceAdd)
		{
			string sc = "UPDATE card SET balance=balance+" + dBalanceAdd;
			sc += " WHERE id = " + id;
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
		private bool UpdateCardAverage(string id, double amount, int month)
		{
			int working_on = 0;

			DataSet dsf = new DataSet();
			string sc = "SELECT working_on FROM card WHERE id=" + id;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				if (myCommand.Fill(dsf, "working_on") == 1)
					working_on = Program.MyIntParse(dsf.Tables["working_on"].Rows[0]["working_on"].ToString());
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return false;
			}

			//reset next month
			if (working_on != month)
			{
				sc = "UPDATE card SET working_on=" + month;
				int start = working_on + 1;
				if (start > 12)
					start = 1;
				int i = 0;
				if (working_on < month)
				{
					for (i = start; i < month; i++)
					{
						sc += ", m" + i.ToString() + "=0 "; //no purchase in these months, reset to zero
					}
				}
				else if (!(working_on == 12 && month == 1)) //from December to Jaunary is continued, no reset
				{
					for (i = start; i <= 12; i++)
					{
						sc += ", m" + i.ToString() + "=0 "; //no purchase in these months, reset to zero
					}
					for (i = 1; i < start; i++)
					{
						sc += ", m" + i.ToString() + "=0 "; //no purchase in these months, reset to zero
					}
				}
				sc += " WHERE id=" + id;
			}

			//top up current
			sc = " UPDATE card SET m" + month + "=m" + month + "+" + amount + " WHERE id=" + id;
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

			if (working_on != month) //update average
			{
				sc += " SELECT m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12 FROM card WHERE id=" + id;
				try
				{
					SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
					if (myCommand.Fill(dsf, "average") <= 0)
						return false;
				}
				catch (Exception e)
				{
					Program.ShowExp(sc, e);
					myConnection.Close();
					return false;
				}

				int months = 0;
				double dTotal = 0;
				double dMonth = 0;
				DataRow dr = dsf.Tables["average"].Rows[0];
				for (int i = 1; i <= 12; i++)
				{
					if (i == month)
						continue;

					dMonth = Program.MyDoubleParse(dr["m" + i.ToString()].ToString());
					if (dMonth > 0)
					{
						months++;
						dTotal += dMonth;
					}
				}
				double dAverage = 0;
				if (months > 0)
					dAverage = dTotal / months;
				dAverage = Math.Round(dAverage, 2);
				sc = "UPDATE card SET purchase_average=" + dAverage + " WHERE id=" + id;
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
			}
			return true;
		}
		private bool DoReceiveOnePayment(string pm, double dAmount)
		{
			string sc = "";
			if (pm == "charge")
			{
				sc = " UPDATE orders set order_deleted = 3 where invoice_number = " + m_invoiceNumber;
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception er)
				{
					Program.ShowExp(sc, er);
					myConnection.Close();
					return true;
				}
				return true;
			}
			if (dAmount == 0)
				return true;
			string payment_method = GetEnumID("payment_method", pm);
			string sAmount = dAmount.ToString();
            string sChange = m_dChange.ToString();
			string eftposPayment = m_dEftposTotal.ToString();
			string ccPayment = m_dCreditTotal.ToString();
			string didPayment = m_nSalesId.ToString();

			//do transaction
			myCommand = new SqlCommand("eznz_payment", myConnection);
			myCommand.CommandType = CommandType.StoredProcedure;

			myCommand.Parameters.Add("@shop_branch", SqlDbType.Int).Value = m_nBranchId.ToString();
			myCommand.Parameters.Add("@Amount", SqlDbType.Money).Value = sAmount;
            //myCommand.Parameters.Add("@Change", SqlDbType.Money).Value = sChange;
			myCommand.Parameters.Add("@paid_by", SqlDbType.VarChar).Value = "";
			myCommand.Parameters.Add("@bank", SqlDbType.VarChar).Value = "";
			myCommand.Parameters.Add("@branch", SqlDbType.VarChar).Value = "";
			myCommand.Parameters.Add("@nDest", SqlDbType.Int).Value = "1116";
			myCommand.Parameters.Add("@amount_for_card_balance", SqlDbType.Money).Value = 0;
			myCommand.Parameters.Add("@staff_id", SqlDbType.Int).Value = m_nCardId.ToString();
			myCommand.Parameters.Add("@card_id", SqlDbType.Int).Value = "0"; //cash sales
			myCommand.Parameters.Add("@payment_method", SqlDbType.Int).Value = payment_method;
			myCommand.Parameters.Add("@invoice_number", SqlDbType.VarChar).Value = m_invoiceNumber;
			myCommand.Parameters.Add("@payment_ref", SqlDbType.VarChar).Value = "";
			myCommand.Parameters.Add("@note", SqlDbType.VarChar).Value = "";
			myCommand.Parameters.Add("@finance", SqlDbType.Money).Value = "0";
			myCommand.Parameters.Add("@credit", SqlDbType.Money).Value = "0";
			myCommand.Parameters.Add("@bRefund", SqlDbType.Bit).Value = 0;
			myCommand.Parameters.Add("@amountList", SqlDbType.VarChar).Value = sAmount;
			myCommand.Parameters.Add("@return_tran_id", SqlDbType.Int).Direction = ParameterDirection.Output;
            myCommand.Parameters.Add("@station_id", SqlDbType.Int).Value = Program.m_sStationID;

			try
			{
				myConnection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp("Record payment failed", e);
//				MessageBox.Show("Payemnt Error, Please stop using this till for checkout. Report to Administrator or Duty Manager. Error Code : 10001");
				return true;
			}

			DoUpdateEftposLog(m_invoiceNumber, m_iEftposLog.ToString(), Station.Text);
			/* Don't need to update payment type, already done in eznz_payment procedure D.W. 10.03.2015			
						//update payment type
						sc = " UPDATE invoice set payment_type = " + payment_method + " where invoice_number=" + m_invoiceNumber + " AND branch =" + m_nBranchId.ToString();
						sc += " UPDATE orders set payment_type = " + payment_method + " where invoice_number=" + m_invoiceNumber + " AND branch =" + m_nBranchId.ToString();
						try
						{
							myCommand = new SqlCommand(sc);
							myCommand.Connection = myConnection;
							myCommand.Connection.Open();
							myCommand.ExecuteNonQuery();
							myCommand.Connection.Close();
						}
						catch (Exception er)
						{
							Program.ShowExp(sc, er);
							return true;
						}
			*/
			return true;
		}
		private string GetEnumID(string sClass, string sName)
		{
			DataSet dsgei = new DataSet();
			if (sClass == "" || sName == "")
				return "0";
			string sc = " SELECT id FROM enum WHERE class = '" + sClass + "' AND name = '" + sName + "' ";
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				if (myCommand1.Fill(dsgei, "gei") == 1)
				{
					return dsgei.Tables["gei"].Rows[0]["id"].ToString();
				}
			}
			catch
			{
			}
			return "0";
		}
		private double GetNormalPrice(string code)
		{
			if (Program.MyIntParse(code) <= 0)
				return 0;
			DataSet dsgnp = new DataSet();
			string sc = " SELECT price1 FROM code_relations WHERE code = " + code;
			try
			{
				SqlDataAdapter myCommand1 = new SqlDataAdapter(sc, myConnection);
				if (myCommand1.Fill(dsgnp, "gnp") == 1)
				{
					string m_sItemSellingPrice = dsgnp.Tables["gnp"].Rows[0]["price1"].ToString();

					return Math.Round(Program.MyDoubleParse(m_sItemSellingPrice), 2);
				}
				else
				{
					return 0;
				}
			}
			catch
			{
			}
			return 0;
		}
		private string ReadSitePage(string name)
		{
			if (dst.Tables["rsp"] != null)
				dst.Tables["rsp"].Clear();
			string sc = " SELECT id, text FROM site_pages WHERE name = '" + name + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "rsp");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				myConnection.Close();
				return ""; ;
			}
			if (dst.Tables["rsp"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["rsp"].Rows[0];
				Debug.Write("id = " + dr["id"].ToString());
				return dr["text"].ToString();
			}
			return "";
		}
		private bool DoWeight()
		{
			if(Program.m_bCasScale)
			{
				FormCasScale fm = new FormCasScale();
				fm.ShowDialog();
				if(fm.m_sErr != "")
				{
					Program.MsgBox(fm.m_sErr);
					return false;
				}
				this.qty.Text = fm.m_dQty.ToString();
				return true;
			}
		
			this.barcode.Text = "";
			bool bReady = false;
			try
			{
				if (axOPOSScale.DeviceEnabled)
					bReady = true;
			}
			catch (Exception e)
			{
			}
			if (!bReady)
			{
				Program.MsgBox("Scale is not ready, please check hardware and settings", "Scale Error");
				return false;
			}
			int nWeight = 0;
			//nWeight = axOPOSScale.ScaleLiveWeight;
			axOPOSScale.ReadWeight(out nWeight, Program.m_nTimeout);
			//axOPOSScale.ReadWeight(out nWeight, 1000);
			double dWeight = ((double)nWeight) / 1000;
			if (nWeight > 0)
				this.qty.Text = dWeight.ToString();
			else
			{
				//	MessageBox.Show("can not weight!!", "Scale Error");
				return true;
			}
			return true;
		}
		private void CheckFloat()
		{
			if (dst.Tables["CheckFloat"] != null)
				dst.Tables["CheckFloat"].Clear();
			string sc = "";
			int nRows = 0;
			sc += " SELECT * From orders Where 1=1 ";
			sc += " AND order_deleted = 0 ";
			sc += " AND station_id = '" + Program.m_sStationID + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "CheckFloat");
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return;
			}
			if (nRows > 0)
			{
				return;
			}
			else
			{
				FormFloat ff = new FormFloat();
				ff.ShowDialog();
			}
		}

		private void loadScanner()
		{
			if (Program.m_sScannerName != "")
			{
				if (axOPOSScanner.Open(Program.m_sScannerName) == 0)
				{
					axOPOSScanner.ClaimDevice(Program.m_nTimeout);
					if (axOPOSScanner.Claimed)
					{
						axOPOSScanner.DeviceEnabled = true;
						axOPOSScanner.DataEventEnabled = true;
						axOPOSScanner.DecodeData = true;
						//						axOPOSScanner.AutoDisable = false;
					}
					else
					{
						//						MessageBox.Show("Claim Scanner failed");
					}
				}
				else
				{
					MessageBox.Show("Open Scanner failed, name=" + Program.m_sScannerName);
				}
			}
		}
		private void ENCNSwitch()
		{
			m_bChineseDesc = !m_bChineseDesc;
			if (!m_bChineseDesc)
			{
				//				buttonLan.Text = "EN";
				for (int i = 0; i < cart.Rows.Count; i++)
				{
					string scode = cart.Rows[i].Cells["cc_code"].Value.ToString();
					string sprice = cart.Rows[i].Cells["cc_price"].Value.ToString();
					string discount = cart.Rows[i].Cells["cc_discount"].Value.ToString();
					string sNormalPriceOnCart = cart.Rows[i].Cells["cc_normalprice"].Value.ToString();
					double dRate = Math.Round((Program.MyDoubleParse(discount) / Program.MyDoubleParse(sNormalPriceOnCart)), 2);

					string sNormalPrice = SpecialItemNormalPrice(scode, sprice);

					m_fmad.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_en"].Value.ToString() + sNormalPrice;
					m_fmads.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_en"].Value.ToString() + sNormalPrice;
					if (Program.MyDoubleParse(discount) > 0.01)
					{
						m_fmad.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_en"].Value.ToString() + "(" + Program.MyDoubleParse(sNormalPriceOnCart).ToString("c") + "@" + dRate.ToString("p") + ")";
						m_fmads.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_en"].Value.ToString() + "(" + Program.MyDoubleParse(sNormalPriceOnCart).ToString("c") + "@" + dRate.ToString("p") + ")";
						cart.Rows[i].Cells["cc_name"].Value = m_fmad.cart.Rows[i].Cells["cc_name"].Value;
					}
					else
					{
						cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_en"].Value.ToString() + sNormalPrice;
						m_fmad.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_en"].Value.ToString() + sNormalPrice;
						m_fmads.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_en"].Value.ToString() + sNormalPrice;
					}
					string s_ItemNameEN = cart.Rows[0].Cells["cc_name_en"].Value.ToString();
					int s_ItemNameENInt = s_ItemNameEN.Length;
					if (s_ItemNameENInt >= 45)
						s_ItemNameENInt = 45;
					if (s_ItemNameENInt > 30)
						s_ItemNameEN = s_ItemNameEN.Substring(0, 30) + "\r\n " + s_ItemNameEN.Substring(31) + "...";
					m_fmad.citem2.Text = s_ItemNameEN;
					m_fmads.citem2.Text = s_ItemNameEN;
				}
			}
			else
			{
				//				buttonLan.Text = "CN";
				for (int i = 0; i < cart.Rows.Count; i++)
				{
					string scode = cart.Rows[i].Cells["cc_code"].Value.ToString();
					string sprice = cart.Rows[i].Cells["cc_price"].Value.ToString();
					string sNormalPriceOnCart = cart.Rows[i].Cells["cc_normalprice"].Value.ToString();
					string discount = cart.Rows[i].Cells["cc_discount"].Value.ToString();
					double dRate = Math.Round((Program.MyDoubleParse(discount) / Program.MyDoubleParse(sNormalPriceOnCart)), 2);
					string sNormalPrice = SpecialItemNormalPrice(scode, sprice);
					if (Program.MyDoubleParse(discount) > 0.01)
					{
						m_fmad.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_cn"].Value.ToString() + "(" + Program.MyDoubleParse(sNormalPriceOnCart).ToString("c") + "@" + dRate.ToString("p") + ")";
						m_fmads.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_cn"].Value.ToString() + "(" + Program.MyDoubleParse(sNormalPriceOnCart).ToString("c") + "@" + dRate.ToString("p") + ")";
						cart.Rows[i].Cells["cc_name"].Value = m_fmad.cart.Rows[i].Cells["cc_name"].Value;

					}
					else
					{
						m_fmad.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_cn"].Value.ToString() + sNormalPrice;
						m_fmads.cart.Rows[i].Cells["cc_name"].Value = cart.Rows[i].Cells["cc_name_cn"].Value.ToString() + sNormalPrice;
						cart.Rows[i].Cells["cc_name"].Value = m_fmad.cart.Rows[i].Cells["cc_name"].Value;
					}
					string s_ItemNameCN = cart.Rows[0].Cells["cc_name_cn"].Value.ToString();
					int s_ItemNameCNInt = s_ItemNameCN.Length;
					if (s_ItemNameCNInt >= 45)
						s_ItemNameCNInt = 45;
					if (s_ItemNameCNInt > 30)
						s_ItemNameCN = s_ItemNameCN.Substring(0, 30) + "\r\n " + s_ItemNameCN.Substring(31) + "...";
					m_fmad.citem2.Text = s_ItemNameCN;
					m_fmads.citem2.Text = s_ItemNameCN;
				}
			}
		}
		private void buttonLan_Click(object sender, EventArgs e)
		{
			ENCNSwitch();
			barcode.Focus();
			//	panelHold.Visible = false;
			panelHold.Visible = true;
			//	lbl_showhold.Visible = true;
			lbl_showhold.Text = ShowHoldedTables();
		}
		private void UpdateTotalPriceDiscplay()
		{
			double dAmount = m_dTotal;
			this.TotalPrice.Text = dAmount.ToString("c");
			this.TotalPaid.Text = m_dTotalPaid.ToString("c");
			this.labelBalance.Text = dAmount.ToString("N2"); // UPDATE BALANCE WHEN DELETE ITEM  CH 12/11/08
			m_fmad.TotalPrice.Text = dAmount.ToString("c");
			m_fmads.TotalPrice.Text = dAmount.ToString("c");
			double dNormalTotal = showNormalTotalAmount();
			double dTotalSaveOnShow = dNormalTotal - dAmount;

			if (dTotalSaveOnShow > 0.05)
			{
				m_fmad.ShowTotalSave.Text = dTotalSaveOnShow.ToString("c");
				m_fmad.totalsavelable.Text = " TOTAL SAVED:";
			}
			else
			{
				m_fmad.ShowTotalSave.Text = "";
				m_fmad.totalsavelable.Text = "";
			}
		}
		private void DeleteCurrentOrder()
		{
			if (!AdminOK("delete item"))
				return;
			m_sAdminAction = "delete_current_order";
			DoAdminAction();
		}
		private void DoPrintXTotal()
		{
			if (!AdminOK("xtotal"))
				return;
			m_sAdminAction = "print_x_total";
			DoAdminAction();
		}
		private void DoPrintZTotal()
		{
			if (!AdminOK("xtotal"))
				return;
			m_sAdminAction = "print_z_total";
			DoAdminAction();
		}
		private void DoBackup()
		{
			Thread.Sleep(500);
			if (File.Exists("autobakup.exe"))
			{
				try
				{

					Process backup = new Process();
					backup.StartInfo.FileName = "autobakup.exe";
					backup.StartInfo.Arguments = "-auto";
					backup.Start();
					backup.WaitForExit();//关键，等待外部程序退出后才能往下执行





































				}
				catch (Exception e)
				{
					MessageBox.Show(e.ToString());
				}
			}
			else
			{
				MessageBox.Show("Sorry, Backup Application doesn't exist!");
			}

		}
		private void buttonSpot_Click(object sender, EventArgs e)
		{
			DoPrintZTotal();
			//DoBackup();
		}
		private void buttonScan_Click(object sender, EventArgs e)
		{
			this.barcode.Focus();
		}
		private void buttonWeight_Click(object sender, EventArgs e)
		{
			DoPrintXTotal();
		}
		private void buttonOpenDraw_Click(object sender, EventArgs e)
		{
			//			if (!AdminOK("open cashdraw"))
			//				return;
			KickCashdraw();
			Program.RecordTillData("draw_opens_no_sales", "1");
		}
		private void cancelPayment_Click(object sender, EventArgs e)
		{
			if (!AdminOK(""))
				return;
			m_sAdminAction = "reprint_total";
			DoAdminAction();
		}
		private void DoSignOff()
		{
			string showCon = ",";
			string holdedCart = "";
			bool has_Item = false;
			int m_iHolding = 0;
			for (int s = 0; s < 4; s++)
			{
				if (dsCart.Tables[s].Rows.Count > 0)
				{
					m_iHolding += 1;
					if (m_iHolding > 1)
						holdedCart += showCon;
					holdedCart += (s + 1).ToString();
					has_Item = true;
				}
			}
			if (has_Item)
			{
				FormMSG signOffError = new FormMSG();
				signOffError.btnNo.Visible = false;
				signOffError.btnYes.Visible = false;
				signOffError.m_sMsg = "Sorry, Cannot Sign Off\r\n Order:" + holdedCart + " still has item";
				signOffError.ShowDialog();
				return;
			}

			//			FormSync fSync = new FormSync();
			//			fSync.DoUploadInvoiceActivata();

			this.Hide();
			this.SalesName.Text = "";
			this.m_nSalesId = 0;
			Program.m_bLanguage_en = true;
			panelMenuTopRight.Controls.Clear();
			panelMenuLeft.Controls.Clear();
			panelMenuPopup.Controls.Clear();
			Program.m_sSalesId = "";
			FormLogin fm = new FormLogin();
			fm.m_sStationID = Station.Text;
			fm.ShowDialog();
			if (fm.m_bLoggedin)
			{
				if (Program.m_bEnableFloating)
					CheckFloat();
				m_nSalesId = Program.MyIntParse(fm.m_sSalesId);
				SalesName.Text = fm.m_sSalesName;
				lblSales.Text = fm.m_sSalesName;
				m_nBranchId = Program.MyIntParse(fm.m_sBranchId);
				Program.m_bLanguage_en = fm.m_bLanguage_en;
				barcode.Focus();
				barcode.Select();
				BuildMenuButtons();
				this.Show();
			}
			else
			{
				this.Hide();
				int WM_CLOSE = 0x0010;
				Message msg = Message.Create(this.Handle, WM_CLOSE, new IntPtr(0), new IntPtr(0));
				PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
				return;
			}

		}
		private void buttonSignOff_Click(object sender, EventArgs e)
		{
			DoSignOff();
		}
		private void buttonDel_Click(object sender, EventArgs e)
		{
			ShowHoldedTables();
			DeleteCurrentOrder();
			//		panelHold.Visible = false;
			panelHold.Visible = true;
			//		lbl_showhold.Visible = true;

		}
		private void ShowDiscountPanel(string iCode, string iName, string iPrice)
		{
			double dOldTotal = m_dTotal;
			string sSwitchCode = discountswtich.Text;
			string sItemCode = iCode;
			string sItemName = iName;
			if (sSwitchCode == "1")
			{
				discounttitle.Text = "Item Discount";
				textBoxNewTotal.Focus();
				itemcode.Text = "Item Code :" + sItemCode;
				label11.Text = " New Price :";
				label12.Text = " Item Price :";
				itemname.Text = sItemName;
				dOldTotal = Program.MyDoubleParse(iPrice);
				this.textBoxNewTotal.Visible = true;
			}
			else
			{
				discounttitle.Text = "Total Discount";
				textBoxDisc.Focus();
				itemcode.Text = "";
				itemname.Text = "";
				label11.Text = "";
				label12.Text = " Current Total :";
				this.textBoxNewTotal.Visible = false;
			}
			m_dOldTotal = dOldTotal;
			oldTotal.Text = Math.Round(dOldTotal, 2).ToString("c");
			panelDiscount.Visible = true;
			this.textBoxDisc.Focus();
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			m_sDiscountCancelled = "1";
			textBoxDisc.Text = "";
			textBoxNewTotal.Text = "";
			m_dDiscRate = 0;
			m_dNewTotal = 0;
			barcode.Focus();
			panelDiscount.Visible = false;
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (textBoxDisc.Text != "")
			{
				if (Program.MyDoubleParse(this.textBoxDisc.Text) > Program.MyDoubleParse(GetSiteSettings("QPOS_MAX_DISCOUNT_PERCENTAGE")))
				{
					FormMSG fm = new FormMSG();
					fm.btnNo.Visible = false;
					fm.btnYes.Visible = false;
					fm.m_sMsg = " Sorry, over " + GetSiteSettings("QPOS_MAX_DISCOUNT_PERCENTAGE") + "% discount rate!";
					fm.ShowDialog();
					this.textBoxDisc.Text = "";
					this.textBoxNewTotal.Text = "";
					return;
				}
				if (Program.MyDoubleParse(textBoxDisc.Text) < 0 || Program.MyDoubleParse(textBoxDisc.Text) > 100)
				{
					FormMSG disRateError = new FormMSG();
					disRateError.btnYes.Visible = false;
					disRateError.btnNo.Visible = false;
					disRateError.m_sMsg = " Rate Range: 0 ~ 100 ";
					disRateError.ShowDialog();
					textBoxDisc.Text = "";
					textBoxNewTotal.Text = "";
					textBoxDisc.Focus();
					textBoxDisc.SelectAll();
				}
			}
			else if (textBoxNewTotal.Text != "")
			{
				if (Program.MyDoubleParse(textBoxNewTotal.Text) < 0)
				{
					FormMSG disNewPrice = new FormMSG();
					disNewPrice.btnYes.Visible = false;
					disNewPrice.btnNo.Visible = false;
					disNewPrice.m_sMsg = " Total can not be negative ";
					disNewPrice.ShowDialog();
					textBoxNewTotal.Text = "";
					textBoxNewTotal.Focus();
					textBoxNewTotal.SelectAll();
				}
			}
			//			if (this.textBoxDisc.Text == "")
			if (this.textBoxDisc.Text == "" && textBoxNewTotal.Text != "")
			{
				m_dNewTotal = Program.MyDoubleParse(textBoxNewTotal.Text);
				if ((m_dOldTotal - m_dNewTotal) / m_dOldTotal > Program.MyDoubleParse(GetSiteSettings("QPOS_MAX_DISCOUNT_PERCENTAGE")) / 100)
				{
					FormMSG fm = new FormMSG();
					fm.btnNo.Visible = false;
					fm.btnYes.Visible = false;
					fm.m_sMsg = " Sorry, over " + GetSiteSettings("QPOS_MAX_DISCOUNT_PERCENTAGE") + "% discount rate!";
					fm.ShowDialog();
					this.textBoxDisc.Text = "";
					this.textBoxNewTotal.Text = "";
					return;
				}
			}
			else
			{
				m_dDiscRate = Program.MyDoubleParse(textBoxDisc.Text);
				if (textBoxNewTotal.Text == "")
					m_dNewTotal = Math.Round(m_dOldTotal * (1 - (m_dDiscRate / 100)), 2);
				textBoxNewTotal.Text = m_dNewTotal.ToString();
			}
			panelDiscount.Visible = false;
			DoDiscount();
			if (!panelDiscount.Visible)
			{
				//m_sDiscountCancelled = "1";
				textBoxDisc.Text = "";
				textBoxNewTotal.Text = "";
				m_dDiscRate = 0;
				m_dNewTotal = 0;
				barcode.Focus();
			}
			barcode.Focus();
		}
		private double GetCatDisRate(string card_id, string code)
		{
			if (dst.Tables["GetCatDisRate"] != null)
				dst.Tables["GetCatDisRate"].Clear();
			string sc = "";
			double dis_rate = 0;
			sc = " SELECT cd.dis_rate From cat_dis cd ";
			sc += " JOIN code_relations c ON c.cat = cd.cat ";
			sc += " WHERE 1=1 ";
			sc += " AND cd.card_id = " + card_id;
			sc += " AND c.code =" + code;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "GetCatDisRate") <= 0)
				{
					return -1;
				}
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return -1;
			}
			if (dst.Tables["GetCatDisRate"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["GetCatDisRate"].Rows[0];
				dis_rate = Program.MyDoubleParse(dr["dis_rate"].ToString());
			}
			return dis_rate;
		}

		private bool promoExpire(string promo_id)
		{
			string endDate = "";
			string beginDate = "";
			if (dst.Tables["promoExpire"] != null)
				dst.Tables["promoExpire"].Clear();
			string sc = "  SET DATEFORMAT dmy";
			sc += " SELECT promo_start_date, promo_end_date FROM promotion_list WHERE promo_id = '" + promo_id + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "promoExpire") <= 0)
				{
					return false;
				}
				else
				{
					endDate = dst.Tables["promoExpire"].Rows[0]["promo_end_date"].ToString();
					beginDate = dst.Tables["promoExpire"].Rows[0]["promo_start_date"].ToString();
					DateTime being = DateTime.Parse(beginDate);
					DateTime end = DateTime.Parse(endDate);
					int a = DateTime.Now.CompareTo(being);
					int b = DateTime.Now.CompareTo(end);
					if (a == 1 && b == -1)
						return false;
				}

			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return false;
			}
			return true;
		}
		private string getLevelPrice(string level, string code)
		{
			if (dst.Tables["getLevelPrice"] != null)
				dst.Tables["getLevelPrice"].Clear();
			string sc = "";
			string level_price = "0";
			string tax_rate = "0.15";
			double dtax_rate = 0.15;
			sc += " SELECT TOP 1 price1, level_price" + level + " as level_price, tax_rate FROM code_relations ";
			sc += " WHERE code = " + code;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "getLevelPrice") <= 0)
				{
					return "0";
				}
				level_price = Program.MyMoneyParse(dst.Tables["getLevelPrice"].Rows[0]["level_price"].ToString()).ToString();
				dtax_rate = Program.MyDoubleParse(dst.Tables["getLevelPrice"].Rows[0]["tax_rate"].ToString());
				level_price = (Program.MyMoneyParse(level_price) * (1 + dtax_rate)).ToString();

				//				if(Program.MyMoneyParse(level_price) == 0 )
				//					level_price = Program.MyMoneyParse(dst.Tables["getLevelPrice"].Rows[0]["price1"].ToString()).ToString();
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				myConnection.Close();
				return "0";
			}
			return level_price;
		}
		private void DoApplyVpDiscountForCart(int nIndex)
		{
			if (discountswtich.Text == "1")
			{
				DoItemDiscount(nIndex);
				return;
			}
			else if (discountswtich.Text == "2")
			{
				if (m_dDiscRate == 0)
				{
					m_bBeDisced = false;
					this.msgboard.Text = "";
				}
				if (m_bMemberDiscount)
					m_dDiscRate = Program.MyDoubleParse(m_sMemberDiscountRate) * 100;

				int c = nIndex;
				string sCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
				if (dst.Tables["total_dis"] != null)
					dst.Tables["total_dis"].Clear();
				string sc = " SELECT code, ISNULL(promo_id, 0) AS promo_id, ISNULL(is_special, '0') AS is_special ";
				sc += ", ISNULL(no_discount,'0') AS no_discount ";
				sc += " FROM code_relations ";
				sc += " WHERE code = " + sCode + " ";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "total_dis") <= 0)
					{
					}
				}
				catch (Exception e)
				{
					Program.ShowExp(sc, e);
					myConnection.Close();
					return;
				}

				string s_dPrice = cart.Rows[c].Cells["cc_price"].Value.ToString();
				string s_dQty = cart.Rows[c].Cells["cc_qty"].Value.ToString();
				string s_dCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
				if (m_bMemberDiscount)
				{
					double discount = GetCatDisRate(m_nCardId.ToString(), s_dCode);
					if (discount == -1)
					{
						m_dDiscRate = Program.MyDoubleParse(m_sMemberDiscountRate) * 100;
					}
					else
					{
						m_dDiscRate = discount * 100;
					}
				}

				string s_sName = cart.Rows[c].Cells["cc_name"].Value.ToString();
				string s_sDiscount = cart.Rows[c].Cells["cc_discount"].Value.ToString();
				string s_sSupplier_code = cart.Rows[c].Cells["cc_supplier_code"].Value.ToString();
				double d_NewPrice = Program.MyDoubleParse(s_dPrice);

				DataRow[] dra = dst.Tables["total_dis"].Select(("code=" + s_dCode));
				if (dra.Length <= 0)
					return;
				DataRow itdi = dra[0];//dst.Tables["total_dis"].Rows[0];
				string sIsPromotion = itdi["promo_id"].ToString();
				bool bIsOnSpecial = Program.MyBooleanParse(itdi["is_special"].ToString());
				bool bIsNoDiscount = Program.MyBooleanParse(itdi["no_discount"].ToString());
				//					if (sIsPromotion != "0")
				if (sIsPromotion != "0" && !promoExpire(sIsPromotion))
					return;
				if (bIsOnSpecial)
					return;
				if (Program.m_svoiddis.IndexOf(s_sSupplier_code).ToString() != "-1")
					return;
				if (s_sDiscount.Trim() != "" || m_dDiscRate == 0)
					return;
				if (bIsNoDiscount)
					return;
				d_NewPrice = Program.MyDoubleParse(s_dPrice) * (1 - m_dDiscRate / 100);
				string sDis = Math.Round((Program.MyDoubleParse(s_dPrice) * (m_dDiscRate / 100)), 2).ToString();
				cart.Rows[c].Cells["cc_discount"].Value = Math.Round((Program.MyDoubleParse(s_dPrice) * (m_dDiscRate / 100)), 2).ToString();
				cart.Rows[c].Cells["cc_name"].Value = s_sName + "(" + Program.MyMoneyParse(s_dPrice).ToString("c") + "@" + (m_dDiscRate / 100).ToString("p") + " DIS)";
				cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
				cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();

				m_fmad.cart.Rows[c].Cells["cc_discount"].Value = sDis;
				m_fmad.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
				m_fmad.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
				m_fmad.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;

				m_fmads.cart.Rows[c].Cells["cc_discount"].Value = sDis;
				m_fmads.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
				m_fmads.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
				m_fmads.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;
				m_fmads.citem1.Text = Program.MyDoubleParse(s_dQty).ToString() + " @ " + Math.Round(d_NewPrice, 2).ToString("c");
				m_dDiscountTotal += Program.MyDoubleParse(s_dPrice) - d_NewPrice;
				UpdateTotalPriceDiscplay();
				CalcCartTotal();
				UpdateDtCart(m_sCurrentCartID);
				this.Cash.Text = "";
				textBoxDisc.Text = "";
				textBoxNewTotal.Text = "";
			}
		}
		private void DoApplyLevelPriceForCart(int nIndex)
		{
			if (Program.m_bEnableVipDis)
			{
				DoApplyVpDiscountForCart(nIndex);
				return;
			}
			if (!Program.m_bEnableLevelPrice)
				return;
			int c = nIndex;
			string sCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
			if (dst.Tables["total_level"] != null)
				dst.Tables["total_level"].Clear();
			string sc = " SELECT code, ISNULL(promo_id, 0) AS promo_id, ISNULL(is_special, '0') AS is_special ";
			sc += ", ISNULL(no_discount,'0') AS no_discount ";
			sc += " FROM code_relations ";
			sc += " WHERE code = " + sCode + " ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "total_level") <= 0)
				{
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return;
			}

			string s_dPrice = cart.Rows[c].Cells["cc_price"].Value.ToString();
			string s_dQty = cart.Rows[c].Cells["cc_qty"].Value.ToString();
			string s_dCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
			string s_sName = cart.Rows[c].Cells["cc_name"].Value.ToString();
			string s_sDiscount = cart.Rows[c].Cells["cc_discount"].Value.ToString();
			string s_sSupplier_code = cart.Rows[c].Cells["cc_supplier_code"].Value.ToString();
			double d_NewPrice = Program.MyDoubleParse(s_dPrice);

			DataRow[] dra = dst.Tables["total_level"].Select(("code=" + s_dCode));
			if (dra.Length <= 0)
				return;
			DataRow itdi = dra[0];//dst.Tables["total_dis"].Rows[0];
			string sIsPromotion = itdi["promo_id"].ToString();
			bool bIsOnSpecial = Program.MyBooleanParse(itdi["is_special"].ToString());
			bool bIsNoDiscount = Program.MyBooleanParse(itdi["no_discount"].ToString());
			if (sIsPromotion != "0" && !promoExpire(sIsPromotion))
				return;
			if (bIsOnSpecial)
				return;
			if (Program.m_svoiddis.IndexOf(s_sSupplier_code).ToString() != "-1")
				return;
			d_NewPrice = Program.MyDoubleParse(getLevelPrice(m_sMemberPriceLevel, s_dCode));
			if (d_NewPrice == 0)
				d_NewPrice = Program.MyDoubleParse(s_dPrice);
			//			string sDis = Math.Round((Program.MyDoubleParse(s_dPrice) - d_NewPrice), 2).ToString();
			string sDis = Math.Round(Program.MyDoubleParse(s_sDiscount)).ToString();
			cart.Rows[c].Cells["cc_discount"].Value = sDis;
			if (s_sName.IndexOf("DIS)").ToString() == "-1")
				cart.Rows[c].Cells["cc_name"].Value = s_sName + "(" + (Program.MyMoneyParse(s_dPrice) - d_NewPrice).ToString("c") + " DIS)";
			else
				cart.Rows[c].Cells["cc_name"].Value = s_sName;
			cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
			cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();

			m_fmad.cart.Rows[c].Cells["cc_discount"].Value = sDis;
			m_fmad.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
			m_fmad.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
			m_fmad.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;

			m_fmads.cart.Rows[c].Cells["cc_discount"].Value = sDis;
			m_fmads.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
			m_fmads.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
			m_fmads.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;
			m_fmads.citem1.Text = Program.MyDoubleParse(s_dQty).ToString() + " @ " + Math.Round(d_NewPrice, 2).ToString("c");
			//			double dDiscount = (Program.MyDoubleParse(s_dPrice) - d_NewPrice) * Program.MyDoubleParse(s_dQty);
			//			m_dDiscountTotal += dDiscount;
			//			this.labelBalance.Text = Math.Round(m_dOrderTotal - dDiscount, 2).ToString();
			UpdateTotalPriceDiscplay();
			CalcCartTotal();
			UpdateDtCart(m_sCurrentCartID);
			this.Cash.Text = "";
		}
		private void DoLevelPrice()
		{
			string codes = "";
			for (int c = 0; c < cart.Rows.Count; c++)
			{
				string sCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
				if (c > 0)
					codes += ",";
				codes += sCode;
			}
			if (codes == "")
				return;
			if (dst.Tables["total_level"] != null)
				dst.Tables["total_level"].Clear();
			string sc = " SELECT code, ISNULL(promo_id, 0) AS promo_id, ISNULL(is_special, '0') AS is_special ";
			sc += ", ISNULL(no_discount,'0') AS no_discount ";
			sc += " FROM code_relations ";
			sc += " WHERE code IN (" + codes + ") ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "total_level") <= 0)
				{
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return;
			}

			double d_NewTotal = 0;
			for (int c = 0; c < cart.Rows.Count; c++)
			{
				string s_dPrice = cart.Rows[c].Cells["cc_price"].Value.ToString();
				string s_dQty = cart.Rows[c].Cells["cc_qty"].Value.ToString();
				string s_dCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
				string s_sName = cart.Rows[c].Cells["cc_name"].Value.ToString();
				string s_sDiscount = cart.Rows[c].Cells["cc_discount"].Value.ToString();
				string s_sSupplier_code = cart.Rows[c].Cells["cc_supplier_code"].Value.ToString();
				double d_NewPrice = Program.MyDoubleParse(s_dPrice);

				DataRow[] dra = dst.Tables["total_level"].Select(("code=" + s_dCode));
				if (dra.Length <= 0)
					continue;
				DataRow itdi = dra[0];//dst.Tables["total_dis"].Rows[0];
				string sIsPromotion = itdi["promo_id"].ToString();
				bool bIsOnSpecial = Program.MyBooleanParse(itdi["is_special"].ToString());
				bool bIsNoDiscount = Program.MyBooleanParse(itdi["no_discount"].ToString());
				//					if (sIsPromotion != "0")
				if (sIsPromotion != "0" && !promoExpire(sIsPromotion))
					continue;
				if (bIsOnSpecial)
					continue;
				/******/
				if (Program.m_sgroceryitem.IndexOf(s_sSupplier_code).ToString() != "-1" || Program.m_sgroceryitem.IndexOf(s_dCode).ToString() != "-1")
					continue;
				if (Program.m_sgroceryweight.IndexOf(s_sSupplier_code).ToString() != "-1" || Program.m_sgroceryweight.IndexOf(s_dCode).ToString() != "-1")
					continue;
				/*******/
				if (Program.m_svoiddis.IndexOf(s_sSupplier_code).ToString() != "-1")
					continue;
				//				if (s_sDiscount.Trim() != "" || m_dDiscRate == 0)
				//					continue;
				//				if (bIsNoDiscount)
				//					continue;
				//				d_NewPrice = Program.MyDoubleParse(s_dPrice) * (1 - m_dDiscRate / 100);
				d_NewPrice = Program.MyDoubleParse(getLevelPrice(m_sMemberPriceLevel, s_dCode));
				if (d_NewPrice == 0)
					d_NewPrice = Program.MyDoubleParse(s_dPrice);
				//				string sDis = Math.Round(Program.MyDoubleParse(s_sDiscount)).ToString();
				//				string sDis = Math.Round((Program.MyDoubleParse(s_dPrice) - d_NewPrice), 2).ToString();
				string sDis = Math.Round((GetNormalPrice(s_dCode) - d_NewPrice), 2).ToString();
				cart.Rows[c].Cells["cc_discount"].Value = sDis;
				if (s_sName.IndexOf("DIS)").ToString() == "-1")
					cart.Rows[c].Cells["cc_name"].Value = s_sName + "(" + (Program.MyMoneyParse(sDis)).ToString("c") + " DIS)";
				else
					cart.Rows[c].Cells["cc_name"].Value = s_sName;

				cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
				cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();

				m_fmad.cart.Rows[c].Cells["cc_discount"].Value = sDis;
				m_fmad.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
				m_fmad.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
				m_fmad.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;

				m_fmads.cart.Rows[c].Cells["cc_discount"].Value = sDis;
				m_fmads.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
				m_fmads.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
				m_fmads.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;
				m_fmads.citem1.Text = Program.MyDoubleParse(s_dQty).ToString() + " @ " + Math.Round(d_NewPrice, 2).ToString("c");
				m_dDiscountTotal = m_dOrderTotal - d_NewTotal;
				this.labelBalance.Text = Math.Round(d_NewTotal, 2).ToString();
				UpdateTotalPriceDiscplay();
				CalcCartTotal();
				UpdateDtCart(m_sCurrentCartID);
				this.Cash.Text = "";
				double dBanlance = Program.MyDoubleParse(GetBalance());
			}
			{
				this.msgboard.Text = " New Price Applied !! ";
				this.discountswtich.Text = "2";
				for (int t = 0; t < 4; t++)
				{
					if (m_sCurrentCartID == t)
						m_sDiscount[t + 1] = m_dDiscRate.ToString();
				}
			}
			textBoxDisc.Text = "";
			textBoxNewTotal.Text = "";
		}

		private void rollbackDiscount()
		{
			if (discountswtich.Text == "1")
			{
				FormMSG fmMSG = new FormMSG();
				fmMSG.m_sMsg = "Roll Back Discount on this Item?? ";
				fmMSG.m_sYesNo = "1";
				fmMSG.ShowDialog();
				if (fmMSG.m_sCreateNewItem != "1")
					return;
				RollBackDiscountOneItem();
			}
			else if (discountswtich.Text == "2")
			{
				FormMSG fmMSG = new FormMSG();
				fmMSG.m_sMsg = "Roll Back Discount?? ";
				fmMSG.m_sYesNo = "1";
				fmMSG.ShowDialog();
				if (fmMSG.m_sCreateNewItem != "1")
					return;
				vDiscountBeRollBack(false);
			}
			panelDiscount.Visible = false;
		}

		private bool doMarginRate(string code, string price)
		{
			if (dst.Tables["marginrate"] != null)
				dst.Tables["marginrate"].Clear();
			string sc = "SELECT manual_cost_frd FROM code_relations WHERE code='" + code + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "marginrate") <= 0)
					return false;
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			double cost = Program.MyDoubleParse(dst.Tables["marginrate"].Rows[0]["manual_cost_frd"].ToString());
			double rate = (Program.MyDoubleParse(price) / (1 + 0.15) - cost) / cost;
			if (rate < Program.MyDoubleParse(m_sMarginRate) / 100)
				return true;
			else
				return false;

		}
		private void DoDiscount()
		{
			if (cart.Rows.Count <= 0)
				return;
			if (discountswtich.Text == "1")
			{
				int nIndex = 0;
				nIndex = cart.CurrentRow.Index;
				DoItemDiscount(nIndex);
				return;
			}
			else if (discountswtich.Text == "2")
			{
				if (m_dDiscRate == 0)
				{
					m_bBeDisced = false;
					this.msgboard.Text = "";
				}
				//if (m_bBeDisced)
				//	return;
				if (m_bMemberDiscount || m_bExistingMember)
					m_dDiscRate = Program.MyDoubleParse(m_sMemberDiscountRate) * 100;
				m_dOrderTotal = m_dTotal;

				if (sAllowDiscountRollBack)
				{
					if (m_dDiscRate == 0)
					{
						vDiscountBeRollBack(Program.MyBooleanParse(m_sDiscountCancelled));
						return;
					}
				}

				string codes = "";
				for (int c = 0; c < cart.Rows.Count; c++)
				{
					string sCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
					if (c > 0)
						codes += ",";
					codes += sCode;
				}
				if (dst.Tables["total_dis"] != null)
					dst.Tables["total_dis"].Clear();
				string sc = " SELECT code, ISNULL(promo_id, 0) AS promo_id, ISNULL(is_special, '0') AS is_special ";
				sc += ", ISNULL(no_discount,'0') AS no_discount ";
				sc += " FROM code_relations ";
				sc += " WHERE code IN (" + codes + ") ";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "total_dis") <= 0)
					{
					}
				}
				catch (Exception e)
				{
					myConnection.Close();
					Program.ShowExp(sc, e);
					return;
				}

				double d_NewTotal = 0;
				for (int c = 0; c < cart.Rows.Count; c++)
				{
					int nPromotion = Program.MyIntParse(cart.Rows[c].Cells["cc_is_promotion"].Value.ToString());
					if (nPromotion != 0) //it's a promotion added item, no more discount
						continue;
					string s_dPrice = cart.Rows[c].Cells["cc_price"].Value.ToString();
					string s_dQty = cart.Rows[c].Cells["cc_qty"].Value.ToString();
					string s_dCode = cart.Rows[c].Cells["cc_code"].Value.ToString();
					if (m_bMemberDiscount || m_bExistingMember)
					{
						double discount = GetCatDisRate(m_nCardId.ToString(), s_dCode);
						if (discount == -1)
						{
							m_dDiscRate = Program.MyDoubleParse(m_sMemberDiscountRate) * 100;
						}
						else
						{
							m_dDiscRate = discount * 100;
						}
					}

					string s_sName = cart.Rows[c].Cells["cc_name"].Value.ToString();
					string s_sDiscount = cart.Rows[c].Cells["cc_discount"].Value.ToString();
					string s_sSupplier_code = cart.Rows[c].Cells["cc_supplier_code"].Value.ToString();
					double d_NewPrice = Program.MyDoubleParse(s_dPrice);

					DataRow[] dra = dst.Tables["total_dis"].Select(("code=" + s_dCode));
					if (dra.Length <= 0)
						continue;
					DataRow itdi = dra[0];//dst.Tables["total_dis"].Rows[0];
					string sIsPromotion = itdi["promo_id"].ToString();
					bool bIsOnSpecial = Program.MyBooleanParse(itdi["is_special"].ToString());
					bool bIsNoDiscount = Program.MyBooleanParse(itdi["no_discount"].ToString());
					//					if (sIsPromotion != "0")
					if (sIsPromotion != "0" && !promoExpire(sIsPromotion))
						continue;
					if (bIsOnSpecial)
						continue;
					if (Program.m_svoiddis.IndexOf(s_sSupplier_code).ToString() != "-1")
						continue;
                    if ((s_sDiscount.Trim() != "0" && s_sDiscount.Trim() != "") || m_dDiscRate == 0)
						continue;
					if (bIsNoDiscount)
						continue;
					if (Program.m_bEnableMargin)
					{
						if (doMarginRate(s_dCode, s_dPrice))
							continue;
					}
					d_NewPrice = Program.MyDoubleParse(s_dPrice) * (1 - m_dDiscRate / 100);
					d_NewTotal = d_NewPrice * Program.MyDoubleParse(s_dQty);
					string sDis = Math.Round((Program.MyDoubleParse(s_dPrice) * (m_dDiscRate / 100)), 2).ToString();
					cart.Rows[c].Cells["cc_discount"].Value = sDis;
					cart.Rows[c].Cells["cc_name"].Value = s_sName + "(" + Program.MyMoneyParse(s_dPrice).ToString("c") + "@" + (m_dDiscRate / 100).ToString("p") + " DIS)";
					cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
					cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();

					m_fmad.cart.Rows[c].Cells["cc_discount"].Value = sDis;
					m_fmad.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
					m_fmad.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
					m_fmad.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;

					m_fmads.cart.Rows[c].Cells["cc_discount"].Value = sDis;
					m_fmads.cart.Rows[c].Cells["cc_price"].Value = Math.Round(d_NewPrice, 2).ToString();
					m_fmads.cart.Rows[c].Cells["cc_total"].Value = Math.Round((d_NewPrice * Program.MyDoubleParse(s_dQty)), 2).ToString();
					m_fmads.cart.Rows[c].Cells["cc_name"].Value = cart.Rows[c].Cells["cc_name"].Value;
					m_fmads.citem1.Text = Program.MyDoubleParse(s_dQty).ToString() + " @ " + Math.Round(d_NewPrice, 2).ToString("c");
					m_dDiscountTotal = m_dOrderTotal - d_NewTotal;
					this.labelBalance.Text = Math.Round(d_NewTotal, 2).ToString();
					UpdateTotalPriceDiscplay();
					CalcCartTotal();
					UpdateDtCart(m_sCurrentCartID);
					this.Cash.Text = "";
					double dBanlance = Program.MyDoubleParse(GetBalance());
				}

				if (m_dDiscRate == 0)
				{
					m_bBeDisced = false;
					this.msgboard.Text = "";
				}
				else
				{
					this.msgboard.Text = (m_dDiscRate / 100).ToString("p") + " Total Discount Applied";
					this.discountswtich.Text = "2";
					for (int t = 0; t < 4; t++)
					{
						if (m_sCurrentCartID == t)
							m_sDiscount[t + 1] = m_dDiscRate.ToString();
					}
				}
				textBoxDisc.Text = "";
				textBoxNewTotal.Text = "";
			}
		}
		private void buttonDiscount_Click(object sender, EventArgs e)
		{
			if (cart.Rows.Count <= 0)
				return;
			discountswtich.Text = "2";
			//			if(sPasswordControl =="1")
			if (Program.m_bEnableDisControl)
			{
				if (!AdminOK("discount"))
					return;
				m_sAdminAction = "total_discount";
				DoAdminAction();
			}
			else
			{
				ResetPayment();
				ShowDiscountPanel("", "", "");
				if (!sAllowDiscountRollBack)
					m_bBeDisced = true;
			}
		}
		private void barcode_TextChanged(object sender, EventArgs e)
		{
			if (barcode.Text != "")
			{
				//ResetPayment();
			}
		}
		private bool CachePromotion()
		{
            int nWeekDay = (int)DateTime.Now.DayOfWeek;
            if (nWeekDay == 0)
                nWeekDay = 7; //tee's design, Sunday is 7

			if (dst.Tables["promo_cache"] != null)
				dst.Tables["promo_cache"].Clear();
			string sc = "  set dateformat dmy ";
			sc += " SELECT p.*, c.code, c.barcode, pg.barcode AS group_barcode, cr.name AS free_item_required_item_name ";
			sc += ", LOWER(p.promo_desc) AS lower_desc ";
			sc += " FROM promotion_list p ";
			sc += " LEFT OUTER JOIN promo pr on p.promo_id = pr.promo_id ";
			sc += " LEFT OUTER JOIN code_relations c ON pr.code = c.code   ";
			sc += " LEFT OUTER JOIN barcode b ON b.item_code = c.code ";
			sc += " LEFT OUTER JOIN promotion_group pg ON pg.barcode = b.barcode ";
			sc += " LEFT OUTER JOIN code_relations cr ON cr.supplier_code = p.free_item_required_item_code ";
			sc += " WHERE p.promo_active = 1 ";
			sc += " AND p.promo_start_date <= GETDATE() ";
			sc += " AND p.promo_end_date >= GETDATE() ";
			sc += " AND promo_day" + nWeekDay + "=1 "; 
			sc += " ORDER BY p.promo_id, c.barcode, pg.barcode ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "promo_cache");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return false; ;
			}
			if(!GetPromotion())
				return false;
			return true;
		}
		private bool GetPromotion()
		{
			if (dst.Tables["get_promo"] != null)
				dst.Tables["get_promo"].Clear();
			string sc = " SELECT p.promo_id, p.barcode as group_barcode, pl.promo_desc,pl.promo_type, pl.volumn_discount_price_total ";
//			sc += " , pl.volumn_discount_qty, pl.free_item_required_item_code, isnull(c.price1,0) as price1 ";
			sc += " , pl.volumn_discount_qty, pl.free_item_required_item_code ";
			sc += ", ISNULL((SELECT TOP 1 price1 FROM code_relations WHERE barcode = p.barcode), 0) AS price1 ";
			sc += ", ISNULL((SELECT TOP 1 c.price1 FROM barcode b JOIN code_relations c ON c.code = b.item_code WHERE b.barcode = p.barcode), 0) AS barcode_price1 ";
			sc += " FROM promotion_group p ";
			sc += " JOIN promotion_list pl ON p.promo_id = pl.promo_id ";
//			sc += " JOIN barcode b ON b.barcode = p.barcode ";
//			sc += " join code_relations c on b.item_code = c.code ";
			sc += " WHERE 1 = 1 ";
			sc += " AND pl.promo_start_date <= GETDATE() AND pl.promo_end_date >= GETDATE() "; //started, but not yet expired
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "get_promo");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return false; ;
			}
			return true;
		}
		private bool GetComboPromoItemByPromoID(string promo_id, string barcode)
		{
			if (dst.Tables["GetComboPromoItemByPromoIDAndBarcode"] != null)
				dst.Tables["GetComboPromoItemByPromoIDAndBarcode"].Clear();
			string sc = "";
			int rows = 0;
            sc = "SELECT distinct p.code from promo p  ";
            sc += " join barcode b on p.code = b.item_code ";
            sc += " join promotion_group pg  on b.barcode = pg.barcode ";
			sc += " JOIN promotion_list pl on pl.promo_id = pg.promo_id ";
			sc += " WHERE 1=1 AND pl.promo_type = 7 ";
			sc += " AND pl.promo_id = '" + promo_id + "'";
			sc += " AND pg.barcode in (" + barcode + ")";
            sc += " order by p.code ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "GetComboPromoItemByPromoIDAndBarcode");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return false; ;
			}

			if (dst.Tables["GetComboPromoItemByPromoID"] != null)
				dst.Tables["GetComboPromoItemByPromoID"].Clear();
			string sc1 = "";
			int rows1 = 0;
            sc1 = "  select distinct code from promo where promo_id = '" + promo_id + "' order by code ";
            //sc1 = "SELECT pg.*, pl.* from promotion_group pg ";
            //sc1 += " JOIN promotion_list pl on pl.promo_id = pg.promo_id ";
            //sc1 += " WHERE 1=1 AND pl.promo_type = 7 ";
            //sc1 += " AND pl.promo_id = '" + promo_id + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc1, myConnection);
				rows1 = myAdapter.Fill(dst, "GetComboPromoItemByPromoID");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return false; 
			}
            DataTable dt1 = dst.Tables["GetComboPromoItemByPromoIDAndBarcode"];
            DataTable dt2 = dst.Tables["GetComboPromoItemByPromoID"];

            if (dt1.Rows.Count == dt2.Rows.Count)
            {
                for(int i=0; i<dt1.Rows.Count; i++)
                {
                    if (!dt1.Rows[i]["code"].Equals(dt2.Rows[i]["code"]))
                        return false;
                    else
                        continue;
                }
                return true;
            }
			else
				return false;
		}
        private void CheckGroupPromotion()
        {
            string barcodes = "";
            if (m_sDeleteItemBarcode != "")
                barcodes = "'" + m_sDeleteItemBarcode + "'";
            for (int i = 0; i < dsCart.Tables[m_nCurrentCart].Rows.Count; i++)
            {
                string bc = cart.Rows[i].Cells["cc_barcode"].Value.ToString();
                if (barcodes != "")
                    barcodes += ",";
                barcodes += "'" + bc + "'";
            }
            if (barcodes == "")
                return;
            //get used group promotion
            DataRow[] dra = dst.Tables["promo_cache"].Select("group_barcode IN(" + barcodes + ") and promo_type = 6 ");
            if (dra.Length <= 0)
                return;
            int nMaxGroups = 256;
            string[] apid = new string[nMaxGroups];
            int nPromoCount = 0;
            string pid_old = "";

            //double dPromotionDiscountTotal = 0; // Record how much Discount given 
            //double dRemainTotal = 0;

            for (int i = 0; i < dra.Length; i++)
            {
                string pid = dra[i]["promo_id"].ToString();
                if (pid != pid_old)
                {
                    apid[nPromoCount++] = pid;
                    if (nPromoCount >= nMaxGroups)
                    {
                        MessageBox.Show("too many group promotions, program cannot handle, max " + nMaxGroups + " groups.");
                        return;
                    }
                    pid_old = pid;
                }
            }
            for (int i = 0; i < nPromoCount; i++) //loop all promotion groups
            {
                int nPromotionCount = 0; //Record how many set items are on promotion 
                string promo_id = apid[i];
                string query = "";
                double dPromotionDiscountTotal = 0; // Record how much Discount given 
                double dRemainTotal = 0;
                query = "group_barcode IN(" + barcodes + ") AND promo_id = " + promo_id;

                DataRow[] da = dst.Tables["get_promo"].Select("group_barcode IN(" + barcodes + ") AND promo_id = " + promo_id);
                if (da.Length <= 0)
                    break;

                //for (int p = 0; p < da.Length; p++)
                //{

                double dItemPrice = 0;
                string sItemCode = "";
                string sPromoDesc = da[0]["promo_desc"].ToString();
                double dPromoAmount = Program.MyDoubleParse(da[0]["volumn_discount_price_total"].ToString()); //group promotion set price e.g any 2 for $3, dPromoAmount = 3
                double dQtyR = Program.MyDoubleParse(da[0]["volumn_discount_qty"].ToString());
                double dPrice = Program.MyDoubleParse(da[0]["price1"].ToString());
                if (dPrice == 0)
                    dPrice = Program.MyDoubleParse(da[0]["barcode_price1"].ToString());
                double dQtyW = 1;
                string reward_item_code = da[0]["free_item_required_item_code"].ToString();

                double dQty = 0;
                double dAmountTotalInCart = 0;
                double dGroupAmount = 0;


                double dPromotionDiscountForThisItem = 0;
                double dRemain = 0;
                int nSet = 0;

                for (int j = 0; j < dsCart.Tables[m_nCurrentCart].Rows.Count; j++) //loop cart for this promotion grup
                {
                    string bc = cart.Rows[j].Cells["cc_barcode"].Value.ToString();
                    double dItemQty = Program.MyDoubleParse(dsCart.Tables[m_nCurrentCart].Rows[j]["qty"].ToString());
                    double dItemPriceCart = Program.MyDoubleParse(dsCart.Tables[m_nCurrentCart].Rows[j]["normal_price"].ToString());

                    for (int m = 0; m < da.Length; m++) //loop barcode in this promotion group
                    {
                        if (da[m]["group_barcode"].ToString() == bc) //found one item suitable
                        {
                            //                             dItemPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_price"].Value.ToString()); // dPrice;
                            dItemPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString());
                            sItemCode = cart.Rows[j].Cells["cc_code"].Value.ToString();
                            dAmountTotalInCart += dItemPriceCart * dItemQty;
                            cart.Rows[j].Cells["cc_promotion_group_id"].Value = promo_id;
                            dsCart.Tables[m_nCurrentCart].Rows[j]["promotion_group_id"] = promo_id;
                            dQty += dItemQty;

                            nSet = (int)(dQty / dQtyR);
                            if (nSet > 0) //数量达到grouppromotion要求 产生了promotion Discount
                            {
                                //                       dPromotionDiscountTotal += dRemainTotal + dItemPrice * (dQtyR - dRemain) * nSet - dPromoAmount * nSet; //记录此item产生的折扣amount
                                dPromotionDiscountTotal += dRemainTotal + dItemPrice * (dQtyR - dRemain) * 1 + dItemPrice * dQtyR * (nSet - 1) - dPromoAmount * nSet; //记录此item产生的折扣amount
                                nPromotionCount += nSet;
                                dRemain = dQty % dQtyR;
                                //dRemainTotal = dRemain * dItemPrice;
                                //dQty = dRemain;
                                //                                  m_iPromotionItemIndex = j+1;
                            }
                            else
                            {
                                dRemain = dRemain + dItemQty;
                                //dRemainTotal = dRemain * dItemPrice;
                                //dQty = dRemain;
                            }
                            dRemainTotal = dRemain * dItemPrice;
                            dQty = dRemain;
                        }
                    }
                }
                int nSavingQty = nPromotionCount;// (int)(dQty / dQtyR * dQtyW); //get how many set of thie group promotion is
                int iQtyInPromotion = nPromotionCount * int.Parse(dQtyR.ToString());
                if (nSavingQty <= 0) //qty not enough, delete saving item added
                {
                    for (int q = 0; q < dsCart.Tables[m_nCurrentCart].Rows.Count; q++) //loop cart for this promotion grup
                    {
                        //						string bc = dsCart.Tables[m_nCurrentCart].Rows[j]["barcode"].ToString();
                        //string bc = cart.Rows[j].Cells["cc_barcode"].Value.ToString();
                        //cart.Rows[j].Cells["cc_is_promotion"].Value = "0";
                        //if (bc == promo_id) //this is the promotion saving row added
                        //{
                        //    DeleteCartItem(j);
                        //    break;
                        //}
                        string promotion_group_id = cart.Rows[q].Cells["cc_promotion_group_id"].Value.ToString();
                        if (promotion_group_id == promo_id)
                        {
                            cart.Rows[q].Cells["cc_price"].Value = cart.Rows[q].Cells["cc_normalprice"].Value.ToString();
                            cart.Rows[q].Cells["cc_name"].Value = cart.Rows[q].Cells["cc_name_en"].Value.ToString();
                            cart.Rows[q].Cells["cc_discount"].Value = "";
                            cart.Rows[q].Cells["cc_total"].Value = (Program.MyDoubleParse(cart.Rows[q].Cells["cc_normalprice"].Value.ToString())
                                * Program.MyDoubleParse(cart.Rows[q].Cells["cc_qty"].Value.ToString())).ToString("N2");

                            //CalcCartTotal();
                            //UpdateDtCart(m_nCurrentCart);
                        }
                    }

					CalcCartTotal();
					UpdateDtCart(m_nCurrentCart);
				}
				else //qty enough to get promotion
                {
                    double dSub = 0 - dPromotionDiscountTotal;//dPromoAmount * nSavingQty - dGroupAmount;  //calculate discount total
                    double dDiscountForEachQty = dSub / iQtyInPromotion;  //discount for each item
                    dDiscountForEachQty = Math.Round(dDiscountForEachQty, 2);
                    nSavingQty = 1;
                    double dCartPrice = 0 - Math.Round(dPromotionDiscountTotal, 2); // Math.Round(dSub / nSavingQty, 2);
                    //					dSub = Math.Round(dCartPrice * nSavingQty, 2);
                    bool bFound = false;
                    int loopingstart = 0;
                    for (int j = 0; j < dsCart.Tables[m_nCurrentCart].Rows.Count; j++) //loop cart for this promotion group
                    {
                        string bc = cart.Rows[j].Cells["cc_barcode"].Value.ToString();
                        string promotion_group_id = cart.Rows[j].Cells["cc_promotion_group_id"].Value.ToString();
                        string qty = cart.Rows[j].Cells["cc_qty"].Value.ToString();
                        int iQty = int.Parse(qty);
                        string code = cart.Rows[j].Cells["cc_code"].Value.ToString();
                        string barcode = cart.Rows[j].Cells["cc_barcode"].Value.ToString();
                        string name = cart.Rows[j].Cells["cc_name_en"].Value.ToString();
                        string name_cn = cart.Rows[j].Cells["cc_name_cn"].Value.ToString();
                        double dOldPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString());

                        if (promotion_group_id == promo_id) // this item belongs to this group promotion
                        {                               //reset to normal price
                            cart.Rows[j].Cells["cc_price"].Value = cart.Rows[j].Cells["cc_normalprice"].Value.ToString();
                            cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name_en"].Value.ToString();
                            cart.Rows[j].Cells["cc_discount"].Value = "";
                            cart.Rows[j].Cells["cc_total"].Value = (Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString())
                                * Program.MyDoubleParse(cart.Rows[j].Cells["cc_qty"].Value.ToString())).ToString("N2");

                            if (iQty <= iQtyInPromotion && iQtyInPromotion > 0)
                            {
  //                            double dOldPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString());
                                double dNewPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString()) + dDiscountForEachQty;
                                string sNewPrice = dNewPrice.ToString("N2");
                                double dNewSub = Math.Round(dNewPrice * iQty, 2);

                                cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name"].Value.ToString() + " (" + "Promotion:" + sPromoDesc + ")";
                                cart.Rows[j].Cells["cc_price"].Value = sNewPrice; // (Program.MyDoubleParse(cart.Rows[j].Cells["cc_price"].Value.ToString()) + dDiscountForEachQty).ToString("N2");//dCartPrice.ToString();
                                cart.Rows[j].Cells["cc_discount"].Value = (0 - dDiscountForEachQty).ToString("N2");
                                cart.Rows[j].Cells["cc_total"].Value = dNewSub;// Math.Round(dSub, 2).ToString("N2");
                                cart.Rows[j].Cells["cc_is_promotion"].Value = "6"; //set flag so no further discount can apply
                                cart.Rows[j].Cells["cc_promotion_group_id"].Value = promo_id;// bc;

                                m_fmad.cart.Rows[j].Cells["cc_price"].Value = sNewPrice;// dCartPrice.ToString();
                                m_fmads.cart.Rows[j].Cells["cc_price"].Value = sNewPrice; // dCartPrice.ToString();

                                iQtyInPromotion = iQtyInPromotion - iQty;
                            }
                            else if (iQty > 1 && iQtyInPromotion < iQty && iQtyInPromotion > 0 )
                            {
 

 //                             DeleteCartItem(j);
                                int iQtyRemain = iQty - iQtyInPromotion;

                                double dNewPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString()) + dDiscountForEachQty;
                                string sNewPrice = dNewPrice.ToString("N2");
                                double dNewSub = Math.Round(dNewPrice * iQtyInPromotion, 2);

                                cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name"].Value.ToString() + " (" + "Promotion:" + sPromoDesc + ")";
                                cart.Rows[j].Cells["cc_price"].Value = sNewPrice; // (Program.MyDoubleParse(cart.Rows[j].Cells["cc_price"].Value.ToString()) + dDiscountForEachQty).ToString("N2");//dCartPrice.ToString();
                                cart.Rows[j].Cells["cc_discount"].Value = (0 - dDiscountForEachQty).ToString("N2");
                                cart.Rows[j].Cells["cc_total"].Value = dNewSub;// Math.Round(dSub, 2).ToString("N2");
                                cart.Rows[j].Cells["cc_is_promotion"].Value = "6"; //set flag so no further discount can apply
                                cart.Rows[j].Cells["cc_promotion_group_id"].Value = promo_id;// bc;
                                cart.Rows[j].Cells["cc_qty"].Value = iQtyInPromotion.ToString();
                                m_fmad.cart.Rows[j].Cells["cc_price"].Value = sNewPrice;// dCartPrice.ToString();
                                m_fmads.cart.Rows[j].Cells["cc_price"].Value = sNewPrice; // dCartPrice.ToString();

                                m_iPromotionItemIndex = j + 1;
                                loopingstart = m_iPromotionItemIndex;

                                //for (int m = 0; m < iQty; m++)
                                //{
                                //    AddToCart(barcode, code, name, name_cn, dOldPrice, 1, "6", false, code, 0, false, false); //if this item in group promotion, split into sigle qty
                                //}
                                AddToCart(barcode, code, name, name_cn, dOldPrice, iQtyRemain, "6", false, code, 0, false, false);
                                iQtyInPromotion = 0;
                            }
                            //if (iQtyInPromotion == 0)
                            //{
                            //    for (; loopingstart < dsCart.Tables[m_nCurrentCart].Rows.Count; loopingstart++)
                            //    {
                            //        if (promotion_group_id == promo_id)
                            //        {
                            //            cart.Rows[j].Cells["cc_price"].Value = cart.Rows[j].Cells["cc_normalprice"].Value.ToString();
                            //            cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name_en"].Value.ToString();
                            //            cart.Rows[j].Cells["cc_discount"].Value = "";
                            //            cart.Rows[j].Cells["cc_total"].Value = (Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString())
                            //                * Program.MyDoubleParse(cart.Rows[j].Cells["cc_qty"].Value.ToString())).ToString("N2");
                            //        }
                            //    }
                            //}

                            //CalcCartTotal();
                            //UpdateDtCart(m_nCurrentCart);
                        }
/*
                            if (bc == promo_id) //this is the promotion saving row added
                            {
                                bFound = true;
                                cart.Rows[j].Cells["cc_price"].Value = dCartPrice.ToString();
                                cart.Rows[j].Cells["cc_qty"].Value = nSavingQty.ToString();
                                cart.Rows[j].Cells["cc_total"].Value = Math.Round(dSub,2).ToString("N2");
                                cart.Rows[j].Cells["cc_is_promotion"].Value = "6"; //set flag so no further discount can apply
                                cart.Rows[j].Cells["cc_promotion_group_id"].Value = bc;

                                m_fmad.cart.Rows[j].Cells["cc_price"].Value = dCartPrice.ToString();
                                m_fmads.cart.Rows[j].Cells["cc_price"].Value = dCartPrice.ToString();		
		
                                CalcCartTotal();
                                UpdateDtCart(m_nCurrentCart);
                                break;
                            }
 */
                    }
                    /*
                    for (int j = 0; j < dsCart.Tables[m_nCurrentCart].Rows.Count; j++)  //loop to do discount
                    {
                        string promotion_group_id = cart.Rows[j].Cells["cc_promotion_group_id"].Value.ToString();
                        if (promotion_group_id == promo_id)
                        {
                            //reset to normal price
                            cart.Rows[j].Cells["cc_price"].Value = cart.Rows[j].Cells["cc_normalprice"].Value.ToString();
                            cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name_en"].Value.ToString();
                            cart.Rows[j].Cells["cc_discount"].Value = "";
                            cart.Rows[j].Cells["cc_total"].Value = (Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString())
                                * Program.MyDoubleParse(cart.Rows[j].Cells["cc_qty"].Value.ToString())).ToString("N2");


                            if (iQtyInPromotion > 0)
                            {
                                double dOldPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString());
                                double dNewPrice = Program.MyDoubleParse(cart.Rows[j].Cells["cc_normalprice"].Value.ToString()) + dDiscountForEachQty;
                                string sNewPrice = dNewPrice.ToString("N2");
                                double dNewSub = Math.Round(dNewPrice * 1, 2);

                                cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name"].Value.ToString() + " (" + "Promotion:" + sPromoDesc + ")";
                                cart.Rows[j].Cells["cc_price"].Value = sNewPrice; // (Program.MyDoubleParse(cart.Rows[j].Cells["cc_price"].Value.ToString()) + dDiscountForEachQty).ToString("N2");//dCartPrice.ToString();
                                cart.Rows[j].Cells["cc_discount"].Value = (0 - dDiscountForEachQty).ToString("N2");
                                cart.Rows[j].Cells["cc_total"].Value = dNewSub;// Math.Round(dSub, 2).ToString("N2");
                                cart.Rows[j].Cells["cc_is_promotion"].Value = "6"; //set flag so no further discount can apply
                                cart.Rows[j].Cells["cc_promotion_group_id"].Value = promo_id;// bc;

                                m_fmad.cart.Rows[j].Cells["cc_price"].Value = sNewPrice;// dCartPrice.ToString();
                                m_fmads.cart.Rows[j].Cells["cc_price"].Value = sNewPrice; // dCartPrice.ToString();

                                iQtyInPromotion--;
                            }

                        }

                        CalcCartTotal();
                        UpdateDtCart(m_nCurrentCart);
                    }
                    */
                    //if (!bFound)
                    //{
                    //    string ascode = m_sPromotionServiceItemCode; //promotion service item
                    //    string asname = "Promotion:" + sPromoDesc;
                    //    double dasPrice = dCartPrice;
                    //    AddToCart(promo_id, ascode, asname, "", dasPrice, nSavingQty, "6", false, ascode, 0, false, false);
                    //}
                }
                //}
            }

			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
		}
		private void CheckComboPromotion()
		{
 //         return;
			string barcodes = "";

			for (int i = 0; i < dsCart.Tables[m_nCurrentCart].Rows.Count; i++)
			{
				string bc = cart.Rows[i].Cells["cc_barcode"].Value.ToString();
				if (barcodes != "")
					barcodes += ",";
				barcodes += "'" + bc + "'";
			}
			if (barcodes == "")
				return;

			DataRow[] dra = dst.Tables["promo_cache"].Select("group_barcode IN(" + barcodes + ") and promo_type =7");
			if (dra.Length <= 0)
				return;
			int nMaxGroups = 256;
			string[] apid = new string[nMaxGroups];
			int nPromoCount = 0;
			string pid_old = "";

			for (int i = 0; i < dra.Length; i++)
			//for (int i = dra.Length-1; i > 0;i-- )
			{
				string pid = dra[i]["promo_id"].ToString();
				if (pid == "" || pid == null)
					break;
				if (pid != pid_old)
				{
					apid[nPromoCount++] = pid;
					if (nPromoCount >= nMaxGroups)
					{
						MessageBox.Show("too many combo promotions, program cannot handle, max " + nMaxGroups + " combo promotions.");
						return;
					}
					pid_old = pid;
				}
			}
			/*********/
			string comboBarcodeInCart = "";
			int thisComboInCart = 0;

			for (int i = 0; i < nPromoCount; i++)
			{
				string promo_id = apid[i];
				DataRow[] da = dst.Tables["get_promo"].Select("group_barcode IN(" + barcodes + ") AND promo_id = " + promo_id);
				if (da.Length <= 0)
					break;
				double dItemPrice = 0;
				string sItemCode = "";
				string sPromoDesc = da[0]["promo_desc"].ToString();
				double dPromoAmount = Program.MyDoubleParse(da[0]["volumn_discount_price_total"].ToString());

				/*********Combo barcode in Cart**************/
				double dQtyR = Program.MyDoubleParse(da[0]["volumn_discount_qty"].ToString());
				double dQtyW = 1;
				//string reward_item_code = da[i]["free_item_required_item_code"].ToString();
				string reward_item_name = "";//da[0]["free_item_required_item_name"].ToString();
				int nType = 0;
				double dQty = 0;
				int miniComboqty = 10000;
				int miniComboqty_old = 10000;
				string[] ComboQty = new string[da.Length];

				double dGroupAmount = 0;
				for (int j = 0; j < dsCart.Tables[m_nCurrentCart].Rows.Count; j++) //loop cart for this combo
				{
					string bc = cart.Rows[j].Cells["cc_barcode"].Value.ToString();
					double dItemQty = Program.MyDoubleParse(dsCart.Tables[m_nCurrentCart].Rows[j]["qty"].ToString());
					for (int m = 0; m < da.Length; m++) //loop barcode in this promotion group
					{
						if (da[m]["group_barcode"].ToString() == bc) //found one item suitable
						{
							if (comboBarcodeInCart != "")
								comboBarcodeInCart += ",";
							comboBarcodeInCart += "'" + bc + "'";

							dItemPrice = Program.MyDoubleParse(da[m]["price1"].ToString());
							sItemCode = cart.Rows[j].Cells["cc_code"].Value.ToString();
							dQty = Program.MyDoubleParse(cart.Rows[j].Cells["cc_qty"].Value.ToString());  //Program.MyDoubleParse(ComboQty[m]);
							cart.Rows[j].Cells["cc_promotion_group_id"].Value = promo_id;
							dsCart.Tables[m_nCurrentCart].Rows[j]["promotion_group_id"] = promo_id;

							dGroupAmount += dItemPrice;

							if (miniComboqty >= dQty)
							{
								miniComboqty = (int)dQty;
							}
						}
					}
				}
				int nSavingQty = miniComboqty; 

				if (GetComboPromoItemByPromoID(promo_id, comboBarcodeInCart))
				{
					double dCartPrice = dPromoAmount - GetComboTotalAmount(promo_id);
					double dSub = Math.Round(dCartPrice * nSavingQty, 2);
					if (nType == 1)
					{
						dSub = (0 - dItemPrice) * nSavingQty;
					}
					else if (nType == 2)
					{
						dSub = 0;
					}
					bool bFound = false;
					for (int j = 0; j < dsCart.Tables[m_nCurrentCart].Rows.Count; j++) //loop cart for this promotion group
					{
						string bc = cart.Rows[j].Cells["cc_barcode"].Value.ToString();
						m_iPromotionItemIndex = j + 1;
						if (bc == promo_id) //this is the promotion saving row added
						{
							bFound = true;
							cart.Rows[j].Cells["cc_qty"].Value = nSavingQty.ToString();
							cart.Rows[j].Cells["cc_total"].Value = dSub.ToString();
							cart.Rows[j].Cells["cc_is_promotion"].Value = "7"; //set flag so no further discount can apply

							CalcCartTotal();
							UpdateDtCart(m_nCurrentCart);
							break;
						}
					}
					if (!bFound)
					{
						string ascode = m_sPromotionServiceItemCode; //promotion service item
						string asname = "Promotion:" + sPromoDesc;
						double dasPrice = dCartPrice;

						AddToCart(promo_id, ascode, asname, "", dasPrice, nSavingQty, "7", false, ascode, 0, false, false);
					}
					//nPromoCount = 0;
					dCartPrice = 0;
				}
				else if (!GetComboPromoItemByPromoID(promo_id, comboBarcodeInCart))
				{
					for (int j = 0; j < dsCart.Tables[m_nCurrentCart].Rows.Count; j++) //loop cart for this promotion grup
					{
						string bc = cart.Rows[j].Cells["cc_barcode"].Value.ToString();
						cart.Rows[j].Cells["cc_is_promotion"].Value = "0";
						if (bc == promo_id) //this is the promotion saving row added
						{
							DeleteCartItem(j);
							break;
						}
					}
				}
 
			}
		}
		private double GetComboTotalAmount(string promo_id)
		{
			if (dst.Tables["GetComboTotalAmount"] != null)
				dst.Tables["GetComboTotalAmount"].Clear();
			double dTotalAmount = 0;
			string sc = "";
			int irows = 0;
            //sc = "SELECT pg.*, pl.* , c.price1 from promotion_group pg";
            //sc += " JOIN promotion_list pl on pl.promo_id = pg.promo_id ";
            //sc += " join barcode b on b.barcode = pg.barcode ";
            //sc += " join code_relations c on c.code =b.item_code ";
            //sc += " WHERE 1=1 AND pl.promo_type = 7 ";
            //sc += " AND pl.promo_id = '" + promo_id + "'";

            sc  = " select c.price1 from code_relations c join promo p on p.code = c.code ";
            sc += " where p.promo_id = '" + promo_id + "' ";

			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				irows = myAdapter.Fill(dst, "GetComboTotalAmount");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
				return 0;
			}
			if (irows == 0)
				return 0;
			for (int i = 0; i < irows; i++)
			{
				DataRow dr = dst.Tables["GetComboTotalAmount"].Rows[i];
				string price1 = dr["price1"].ToString();
				double dprice1 = Program.MyDoubleParse(price1);
				dTotalAmount = dTotalAmount + dprice1;
			}
			return dTotalAmount;
		}
		private void CheckCategoryDiscount(int nIndex, string cat)
		{
			int nPromotion = Program.MyIntParse(cart.Rows[nIndex].Cells["cc_is_promotion"].Value.ToString());
			if (nPromotion != 0) //it's a promotion added item, no more discount
				return; // no more discount
			string sSel = " lower_desc = '" + Program.EncodeQuote(cat).ToLower() + "' AND promo_type = 8 ";
			DataRow[] dra = dst.Tables["promo_cache"].Select(sSel);
			if (dra.Length <= 0)
				return;
            var today = DateTime.Now.DayOfWeek.ToString().ToLower();
            bool bDayActive = false;
            Dictionary<string, bool> promo_day = new Dictionary<string, bool>();
            promo_day.Clear();
            promo_day.Add("monday", bool.Parse(dra[nIndex]["promo_day1"].ToString()));
            promo_day.Add("tuesday", bool.Parse(dra[nIndex]["promo_day2"].ToString()));
            promo_day.Add("wednesday", bool.Parse(dra[nIndex]["promo_day3"].ToString()));
            promo_day.Add("thursday", bool.Parse(dra[nIndex]["promo_day4"].ToString()));
            promo_day.Add("friday", bool.Parse(dra[nIndex]["promo_day5"].ToString()));
            promo_day.Add("saturday", bool.Parse(dra[nIndex]["promo_day6"].ToString()));
            promo_day.Add("sunday", bool.Parse(dra[nIndex]["promo_day7"].ToString()));
            foreach (var day in promo_day)
            {
                if (day.Key == today && day.Value)
                    bDayActive = true;
            }
            if (!bDayActive)
                return;

			double dRate = 1 - Program.MyDoubleParse(dra[0]["discount_percentage"].ToString()) / 100;
			double dPriceOrg = Program.MyMoneyParse(cart.Rows[nIndex].Cells["cc_price"].Value.ToString());
			double dQty = Program.MyMoneyParse(cart.Rows[nIndex].Cells["cc_qty"].Value.ToString());
			double dPrice = dPriceOrg * dRate;
			dPrice = Math.Round(dPrice, 4);
			double dSub = dPrice * dQty;
			double dDiscount = dPriceOrg - dPrice;

			cart.Rows[nIndex].Cells["cc_price"].Value = dPrice.ToString("N2");
			cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
			cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
			cart.Rows[nIndex].Cells["cc_discount"].Value = dDiscount.ToString("N2");
			cart.Rows[nIndex].Cells["cc_is_promotion"].Value = "8";
			m_fmad.cart.Rows[nIndex].Cells["cc_price"].Value = dPrice.ToString("N2");
			m_fmad.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
			m_fmad.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
			m_fmad.cart.Rows[nIndex].Cells["cc_discount"].Value = dDiscount.ToString("N2");
			m_fmads.cart.Rows[nIndex].Cells["cc_price"].Value = dPrice.ToString("N2");
			m_fmads.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
			m_fmads.cart.Rows[nIndex].Cells["cc_total"].Value = dSub;
			m_fmads.cart.Rows[nIndex].Cells["cc_discount"].Value = dDiscount.ToString("N2");
			UpdateTotalPriceDiscplay();
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
		}




		private bool CheckPromotionServiceItem(string code)
		{
			string name = "promotion service item";
			string cat = "ServiceItem";
			string sc = " IF NOT EXISTS(SELECT code FROM code_relations WHERE code = " + code + " AND supplier_code = '" + code + "') ";
			sc += " INSERT INTO code_relations(id, code, supplier_code, name, cat) VALUES('" + code + "', " + code + ", '" + code + "', '" + name + "', '" + cat + "') ";
			sc += " IF NOT EXISTS(SELECT code FROM product WHERE code = " + code + " AND supplier_code = '" + code + "') ";
			sc += " INSERT INTO product(code, supplier_code, name, cat, price) VALUES(" + code + ", '" + code + "', '" + name + "', '" + cat + "', 0) ";
            sc += " UPDATE code_relations SET tax_rate = 0.15 WHERE code = " + code + " ";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception er)
			{
				myConnection.Close();
				Program.ShowExp(sc, er);
				return false;
			}
			return true;
		}
		private bool CheckQtyPromotionValue(string bCode, int nWeekDay, string sBarcode)
		{
            int a = 0;
			if (dst.Tables["promo_cache"] == null)
				CachePromotion();
			string sc = " ";
            if (bCode != "" && int.TryParse(bCode, out a))
			{
				sc += " code = '" + bCode + "'";
			}
			else if (sBarcode != "")
			{
				sc += " barcode = '" + sBarcode + "' ";
			}

			sc += "  AND (";
			for (int e = 2; e <= 7; e++)
			{
				sc += " promo_day" + e + " = 1 OR ";
			}
			sc += " promo_day1 = 1 )";

			DataRow[] dra = dst.Tables["promo_cache"].Select(sc);
			if (dra.Length > 0)
				return true;
			return false;
		}
		private string GetSiteSettings(string sSettingName)
		{
			string sc = " SELECT value FROM settings WHERE name = N'" + sSettingName + "' ";
			if (dst.Tables["rsp"] != null)
				dst.Tables["rsp"].Clear();
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
		private void SelectDiscountItem(object sender, DataGridViewCellEventArgs e)
		{
			int nIndex = 0;
			nIndex = cart.CurrentRow.Index;
			if (e.ColumnIndex == cc_del.Index)
			{
				//				DoItemDiscount(nIndex);
				ShowDiscountPanel(cart.Rows[nIndex].Cells["cc_supplier_code"].Value.ToString(), cart.Rows[nIndex].Cells["cc_name"].Value.ToString(), cart.Rows[nIndex].Cells["cc_price"].Value.ToString());
			}
		}
		private void DoItemDiscount(int nIndex)
		{
			bool sSpecialItemPWCtrl = Program.MyBooleanParse(m_sDiscountSpecialItem);
			string sSelectIedItemPrice = cart.Rows[nIndex].Cells["cc_price"].Value.ToString();
			double dSaveChangeValue = Program.MyDoubleParse(sSelectIedItemPrice);
			string dSaveQty = cart.Rows[nIndex].Cells["cc_qty"].Value.ToString();
			string sSelectedItemName = cart.Rows[nIndex].Cells["cc_name"].Value.ToString();
			string sSwitchCode = discountswtich.Text;
			string sSelectedItemCode = cart.Rows[nIndex].Cells["cc_code"].Value.ToString();
			string sSelectedItemSupplierCode = cart.Rows[nIndex].Cells["cc_supplier_code"].Value.ToString();
			double dRowSubTotal = 0;

			string s_supplier_code = cart.Rows[nIndex].Cells["cc_supplier_code"].Value.ToString();

			if (dst.Tables["item_dis"] != null)
				dst.Tables["item_dis"].Clear();
			string sc = "SET dateformat dmy ";
			sc += " SELECT ISNULL( ";
			//			sc += " c.promo_id, 0 ";
			sc += " (select top 1 p.promo_id from promo p left outer join promotion_list pl on p.promo_id = pl.promo_id where 1=1 AND pl.promo_start_date <= GETDATE() AND pl.promo_end_date >= GETDATE() and p.code = '"+ sSelectedItemCode + "')";
			sc += " ,0) AS promo_id, ISNULL(c.is_special, '0') AS is_special, ISNULL(c.no_discount,'0') AS no_discount FROM code_relations c ";
			//sc += " left outer join promo p on c.code = p.code ";
			//sc += " left outer join promotion_list pl on p.promo_id = pl.promo_id ";
			//sc += " AND pl.promo_start_date <= GETDATE() AND pl.promo_end_date >= GETDATE() ";
			sc += " WHERE 1=1 ";
			sc += " and c.code = '" + sSelectedItemCode + "'";

			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "item_dis") <= 0)
				{
					MessageBox.Show("Free Item not found, code = " + s_supplier_code + "", "Promotion Error");
					return;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return;
			}

			DataRow itdi = dst.Tables["item_dis"].Rows[0];
			string sIsPromotion = itdi["promo_id"].ToString();
			bool bIsItemOnSale = Program.MyBooleanParse(itdi["is_special"].ToString());
			bool bIsNoDiscount = Program.MyBooleanParse(itdi["no_discount"].ToString());
			if (!Program.m_bEnableSpecialDis)
			{
				if (bIsNoDiscount)
					return;
				if (bIsItemOnSale && !m_bSpecialItemDis)
				{
					FormMSG onSale = new FormMSG();
					onSale.btnNo.Visible = false;
					onSale.btnYes.Visible = false;
					onSale.m_sMsg = " Special Item";
					onSale.ShowDialog();
					return;
				}
				if (sIsPromotion != "0")
				{
					FormMSG frmProm = new FormMSG();
					frmProm.btnNo.Visible = false;
					frmProm.btnYes.Visible = false;
					frmProm.m_sMsg = "Promotion Item";
					frmProm.ShowDialog();
					this.barcode.Focus();
					this.barcode.Select();
					return;
				}
			}
			double dNewSubTotal = m_dNewTotal;
			//			if (dNewSubTotal != 0)
			dSaveChangeValue = dNewSubTotal;
			//			else
			//				return;
			double dSelectedItemDiscount = Math.Round((Program.MyDoubleParse(sSelectIedItemPrice) - dSaveChangeValue), 2);
			if (dSelectedItemDiscount != 0)
			{
				cart.Rows[nIndex].Cells["cc_discount"].Value = dSelectedItemDiscount.ToString("N2");
				m_fmad.cart.Rows[nIndex].Cells["cc_discount"].Value = dSelectedItemDiscount.ToString("N2");
				m_fmads.cart.Rows[nIndex].Cells["cc_discount"].Value = dSelectedItemDiscount.ToString("N2");
			}
			string sDiscount = cart.Rows[nIndex].Cells["cc_discount"].Value.ToString();
			double dItemDisPercent = Math.Round(Program.MyDoubleParse(sDiscount) / Program.MyDoubleParse(sSelectIedItemPrice), 2);
			cart.Rows[nIndex].Cells["cc_name"].Value = sSelectedItemName;
			if (dSelectedItemDiscount != 0)
				cart.Rows[nIndex].Cells["cc_name"].Value += "( " + Program.MyDoubleParse(sSelectIedItemPrice).ToString("c") + "@" + dItemDisPercent.ToString("p") + " DIS )";
			cart.Rows[nIndex].Cells["cc_price"].Value = dSaveChangeValue.ToString("N2");
			dRowSubTotal = dSaveChangeValue * Program.MyDoubleParse(dSaveQty);
			dRowSubTotal = Math.Round(dRowSubTotal, 3);
			cart.Rows[nIndex].Cells["cc_total"].Value = dRowSubTotal.ToString();
			m_fmad.cart.Rows[nIndex].Cells["cc_price"].Value = Math.Round(dSaveChangeValue, 2).ToString();
			m_fmad.cart.Rows[nIndex].Cells["cc_total"].Value = dRowSubTotal.ToString();
			m_fmad.cart.Rows[nIndex].Cells["cc_name"].Value = cart.Rows[nIndex].Cells["cc_name"].Value;

			m_fmads.cart.Rows[nIndex].Cells["cc_price"].Value = Math.Round(dSaveChangeValue, 2).ToString();
			m_fmads.cart.Rows[nIndex].Cells["cc_total"].Value = dRowSubTotal.ToString();
			m_fmads.cart.Rows[nIndex].Cells["cc_name"].Value = cart.Rows[nIndex].Cells["cc_name"].Value;

			UpdateTotalPriceDiscplay();
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
			double dBanlance = Program.MyDoubleParse(GetBalance());
			double dTotalItemSaveInRow = 0;
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string code = cart.Rows[i].Cells["cc_code"].Value.ToString();
				string qty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				double dItemDiscountPrice = GetNormalPrice(code);
				double dSellingPrice = Program.MyDoubleParse(cart.Rows[i].Cells["cc_price"].Value.ToString());
				//double dItemSaveInRow = (dItemDiscountPrice - dSellingPrice) * Program.MyDoubleParse(qty);
				double dItemSaveInRow = Program.MyDoubleParse(cart.Rows[nIndex].Cells["cc_discount"].Value.ToString()) * Program.MyDoubleParse(qty);
				dTotalItemSaveInRow += dItemSaveInRow;

			}
			double dItemTotalSaveAD = dTotalItemSaveInRow;

			this.discountswtich.Text = "2";
			/*******************
			 * Total Discount Control 
			 * ********************/
			if (this.discountswtich.Text == "1")
				m_bBeDisced = false;
			else if (this.discountswtich.Text == "2")
				m_bBeDisced = true;
			// m_fmad.cart.Rows[nIndex].Cells["cc_savetotal"].Value = dItemTotalSaveAD.ToString();

			textBoxNewTotal.Text = "";
			textBoxDisc.Text = "";
		}
		private void price_TextChanged(object sender, EventArgs e)
		{
		}
		private void RefreshPayment(string payment_type)
		{
			if (payment_type != "payment_cash")
			{
				m_dCash = 0;
				Cash.Text = "";
			}
			if (payment_type != "payment_eftpos")
			{
				m_dEftpos = 0;
				Eftpos.Text = "";
			}
			m_dCashout = 0;
			if (payment_type != "payment_Credit")
			{
				m_dCredit = 0;
				Credit.Text = "";
			}
			if (payment_type != "payment_Cheque")
			{
				m_dCheque = 0;
				Cheque.Text = "";
			}
			if (payment_type != "payment_charge")
			{
				charge.Text = "";
			}
			labelChange.Text = "CHANGE";
			UpdateTotalPriceDiscplay();
		}
		private void TotalPrice_Click(object sender, EventArgs e)
		{
		}
		private void printReciptD_Click(object sender, EventArgs e)
		{
			DoSignOff();
		}
		private void cart_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void Change_TextChanged(object sender, EventArgs e)
		{
		}
		private string GetLabelBarcodePrice(string barcode, int m_sDecimalPoint, int m_sPriceLength, int m_sPriceStartDigit)
		{
			string s_priceonbarcode = barcode.Substring(m_sPriceStartDigit - 1, m_sPriceLength).ToString();
			double d_sNewPrice = 0;
			try
			{
				d_sNewPrice = Double.Parse(s_priceonbarcode) / Double.Parse(m_sDecimalPoint.ToString());
			}
			catch (Exception e)
			{
				return "";
			}
			string m_sNewPrice = d_sNewPrice.ToString("N2");
			return m_sNewPrice;
		}
		private string GetLabelBarcodeBarcode(string barcode, int m_sBarcodeLength, int m_sBarcodeStartDigit)
		{
			//	int m_sbarcodeLength = int.Parse(Program.m_sbarcodelength1);
			//	int m_spriceLength = int.Parse(Program.m_spricelength1);
			//	int m_spoint = int.Parse(Program.m_spoint1);

			string s_barcodeonbarcodelabel = barcode.Substring(m_sBarcodeStartDigit - 1, m_sBarcodeLength).ToString();
			//double d_sNewPrice = Program.MyDoubleParse(s_priceonbarcode) / Program.MyDoubleParse(m_sDecimalPoint.ToString());
			//string m_sNewPrice = d_sNewPrice.ToString("N2");
			return s_barcodeonbarcodelabel;
		}
        private double GetLabelBarcodeWeight(double unitprice, double totalprice)
        {
            double weight = 0;
            if (unitprice >= 0 && totalprice >= 0)
                weight = totalprice / unitprice;
            else
                weight = 1;
            weight = Math.Round(weight, 4);
            return weight;
        }
		private double GetBarcodeWithPrice(string sInputValue, string sDelimiter, ref string sNewBarcode)
		{
			/***************************************
			 * author: tee 19/11/2008
			 * purpose: to read barcode with price from user input
			 * output: return double in price and reference new barcode value
			 * 
			 ***************************************/
			double dPrice = 0;
			int nDelimiterPosition = sInputValue.IndexOf(sDelimiter);

			if (nDelimiterPosition < 1) ///stop processing barcode with price///
				return 0;

			//  MessageBox.Show(nDelimiterPosition.ToString());
			//   MessageBox.Show(sInputValue.Length.ToString());
			string sReadPrice = sInputValue.Substring(0, nDelimiterPosition);
			//if (TSIsNumberic(sReadPrice))

			string Str = sReadPrice.Trim();
			double Num;
			bool isNum = double.TryParse(Str, out Num);

			if (TSIsNumberic(sReadPrice))
				dPrice = Program.MyDoubleParse(sReadPrice) / 100;
			else if (isNum)
				dPrice = Program.MyDoubleParse(sReadPrice);// / 100;

			sNewBarcode = sInputValue.Substring(nDelimiterPosition + 1);
			//  MessageBox.Show(sNewBarcode);
			//  MessageBox.Show(dPrice.ToString());
			return dPrice;
		}
		private bool TSIsNumberic(string inputValue)
		{
			/***************************************
		   * author: tee  19/11/2008
		   * purpose: to check value is integer or not...
		   * output: return true or false
		   * 
		   ***************************************/
			try
			{
				int nValue = int.Parse(inputValue);
			}
			catch (Exception)
			{
				//    MessageBox.Show(e.ToString());
				return false;
			}
			return true;
		}
		private void brefund_Click(object sender, EventArgs e)
		{
			if (cart.Rows.Count <= 0)
				return;
			if (!AdminOK("refund"))
				return;
			m_sAdminAction = "refund";
			DoAdminAction();
		}
		private void DoRefund()
		{
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string dQty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				double dNQty = Program.MyDoubleParse(dQty.ToString());
				dNQty = 0 - dNQty;
				double dPriceCurrent = Program.MyDoubleParse(cart.Rows[i].Cells["cc_price"].Value.ToString());
                //double dSub = Math.Round(dPriceCurrent * dNQty, 2);
                double dSub = 0-Program.MyDoubleParse(cart.Rows[i].Cells["cc_total"].Value.ToString());
				cart.Rows[i].Cells["cc_qty"].Value = dNQty;
				cart.Rows[i].Cells["cc_total"].Value = dSub;
				m_fmad.cart.Rows[i].Cells["cc_qty"].Value = dQty;
				m_fmad.cart.Rows[i].Cells["cc_total"].Value = dSub;

				m_fmads.cart.Rows[i].Cells["cc_qty"].Value = dQty;
				m_fmads.cart.Rows[i].Cells["cc_total"].Value = dSub;
			}
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
			Program.RecordTillData("total_refund_times", "1");
			/*
						if (m_bVoucherEnabled)
						{
							pnlkeyininv.Location = new Point(119, 121);
							pnlkeyininv.Size = new Size(486, 206);
							pnlkeyininv.Visible = true;
							txtoldinv.Focus();
						}
			 */
		}
		private bool CheckExistingPromotItem(string sRcode)
		{
			Program.Trim(ref sRcode);
			if (dst.Tables["prom_list"] != null)
				dst.Tables["prom_list"].Clear();
			string sc = " SELECT top 1 promo_type, free_item_required_qty,free_item_reward_qty, free_qty_required_qty FROM promotion_list WHERE free_item_required_item_code = '" + sRcode + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "prom_list") <= 0)
				{
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}

			DataRow dri = dst.Tables["prom_list"].Rows[0];
			double dQtyRequired = Program.MyDoubleParse(dri["free_qty_required_qty"].ToString());
			string sPromoTypeBelong = dri["promo_type"].ToString();
			if (sPromoTypeBelong == "5")
				dQtyRequired = Program.MyDoubleParse(dri["free_item_required_qty"].ToString());
			string sRewordItemQtyInCart = dri["free_item_reward_qty"].ToString();
			double dPromoItemTotal = 0;
			double dRewardedItemTotal = 0;
			double dRewardItemSales = 0;
			for (int e = 0; e < cart.Rows.Count; e++)
			{
				string sPromoType = cart.Rows[e].Cells["cc_is_promotion"].Value.ToString();
				string sPromoQty = cart.Rows[e].Cells["cc_qty"].Value.ToString();
				string sPromoCode = cart.Rows[e].Cells["cc_supplier_code"].Value.ToString();
				string sPromoPrice = cart.Rows[e].Cells["cc_price"].Value.ToString();
				if (sPromoType == sPromoTypeBelong && sPromoType == "5" && sPromoPrice != "0.00")
					dPromoItemTotal += Program.MyDoubleParse(sPromoQty);
				if (sRcode == sPromoCode)
				{
					if (sPromoPrice == "0.00")
						dRewardedItemTotal += Program.MyDoubleParse(sPromoQty);
					else
						dRewardItemSales += Program.MyDoubleParse(sPromoQty);

				}
			}
			double dTotalRewardTotal = dPromoItemTotal / dQtyRequired * Program.MyDoubleParse(sRewordItemQtyInCart);
			int dTotalRewardTotalInt = Program.MyIntParse(dTotalRewardTotal.ToString());
			if (Program.MyIntParse(dRewardedItemTotal.ToString()) != dTotalRewardTotalInt)
				return true;
			else
				return false;
		}
		private string CheckFreeRewardItem(string sRcode, double dKeyRewardItemQty, string promocode)
		{
			if (dst.Tables["FreeItem"] != null)
				dst.Tables["FreeItem"].Clear();
			string sc = " SELECT top 1 promo_type, free_item_required_qty,free_item_reward_qty, free_qty_required_qty FROM promotion_list WHERE free_item_required_item_code = " + sRcode;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "FreeItem") <= 0)
				{
					return "";
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "";
			}

			DataRow dri = dst.Tables["FreeItem"].Rows[0];
			string sFreeItemQty = dri["free_item_reward_qty"].ToString();
			string sFreeItemRequiredQty = dri["free_item_required_qty"].ToString();
			string sFreeItemPromoType = dri["promo_type"].ToString();
			double dPromoItemTotalQty = 0;
			double dRewardedFreeItemTotal = 0;
			double dRewardFreeItemSales = 0;
			for (int f = 0; f < cart.Rows.Count; f++)
			{
				string sCartPromoItemQty = cart.Rows[f].Cells["cc_qty"].Value.ToString();
				string sCartPromoItemCode = cart.Rows[f].Cells["cc_code"].Value.ToString();
				string sCartPromoItemPrice = cart.Rows[f].Cells["cc_price"].Value.ToString();
				string sCartPromoItemPromoType = cart.Rows[f].Cells["cc_is_promotion"].Value.ToString();
				if (sFreeItemPromoType == sCartPromoItemPromoType && sCartPromoItemPrice != "0.00")
					dPromoItemTotalQty += Program.MyDoubleParse(sCartPromoItemQty);
				if (sRcode == sCartPromoItemCode)
				{
					if (sCartPromoItemPrice == "0.00")
						dRewardedFreeItemTotal += Program.MyDoubleParse(sCartPromoItemQty);
					else
						dRewardFreeItemSales += Program.MyDoubleParse(sCartPromoItemQty);
				}
			}
			double dTotalRewardQty = dPromoItemTotalQty / Program.MyDoubleParse(sFreeItemRequiredQty) * Program.MyDoubleParse(sFreeItemQty);
			int dTotalRewardQtyInt = Program.MyIntParse(dTotalRewardQty.ToString());
			double dRestTotal = dRewardedFreeItemTotal + dKeyRewardItemQty - Program.MyDoubleParse(dTotalRewardQtyInt.ToString());// -dRewardedFreeItemTotal;
			dTotalRewardQtyInt = dTotalRewardQtyInt - Program.MyIntParse(dRewardedFreeItemTotal.ToString());
			if (dKeyRewardItemQty < dTotalRewardQty)
				dTotalRewardQtyInt = Program.MyIntParse(dKeyRewardItemQty.ToString());
			if (dst.Tables["cqp_fi"] != null)
				dst.Tables["cqp_fi"].Clear();
			string sci = " SELECT supplier_code, name, name_cn, price1, avoid_point FROM code_relations WHERE code = " + sRcode;
			try
			{
				myAdapter = new SqlDataAdapter(sci, myConnection);
				if (myAdapter.Fill(dst, "cqp_fi") <= 0)
				{
					MessageBox.Show("Free Item not found, code = " + sRcode, "Promotion Error");
					return "";
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return "";
			}

			DataRow drit = dst.Tables["cqp_fi"].Rows[0];
			string name = drit["name"].ToString();
			string name_cn = drit["name_cn"].ToString();
			string price1 = drit["price1"].ToString();

			//name = name + " (Free Item) ";
			//name_cn = name_cn + " (Free Item) ";
			if (dRestTotal > 0)
				AddToCart("", sRcode, name, name_cn, Program.MyDoubleParse(price1), dRestTotal, "0", false, drit["supplier_code"].ToString(), 0, bool.Parse(dri["avoid_point"].ToString()), false);
			return dTotalRewardQtyInt.ToString();
		}
		private void SetCurrentQTY()
		{
			/***************************************
			 * author: tee 21/11/2008
			 * purpose: to store temporary qty in mutilply_qty textfiled. this field is invisible to operator.
			 * output: store key in qty into multiply_qty textfiled for quick keyin qty.
			 * 
			 ***************************************/
			string sInputValue = this.barcode.Text;
			this.barcode.Text = "";
			this.qty.Text = sInputValue;
		}

		private double showNormalTotalAmount()
		{
			string sShowQty = "";
			double sShowTotal = 0;
			for (int s = 0; s < cart.Rows.Count; s++)
			{
				string s_DCode = cart.Rows[s].Cells["cc_code"].Value.ToString();
				string s_DPrice = cart.Rows[s].Cells["cc_price"].Value.ToString();
				string s_sName = cart.Rows[s].Cells["cc_name"].Value.ToString();
				string s_sDiscount = cart.Rows[s].Cells["cc_discount"].Value.ToString();

				double dNormalPrice = GetNormalPrice(s_DCode);
				if (dNormalPrice == 0)
					dNormalPrice = Program.MyDoubleParse(s_DPrice) + Program.MyDoubleParse(s_sDiscount);
				if (dNormalPrice < Program.MyDoubleParse(s_DPrice) && dNormalPrice != 0)
					dNormalPrice = Program.MyDoubleParse(s_DPrice);

				sShowQty = cart.Rows[s].Cells["cc_qty"].Value.ToString();
				double dShowDiscountSubTotal = dNormalPrice * Program.MyDoubleParse(sShowQty);
				sShowTotal += dShowDiscountSubTotal;
			}
			double sShowOnDupplicate = sShowTotal;
			return sShowOnDupplicate;
		}
		private bool AddPackage(string barcode, double sQty)
		{
			if (dst.Tables["package"] != null)
				dst.Tables["package"].Clear();
			if (barcode == "")
				return true;
			string sc = " SELECT  package_price, item_qty FROM barcode WHERE barcode = '" + barcode + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "package") == 0)
				{
					return false;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return false;
			}
			DataRow dri = dst.Tables["package"].Rows[0];
			string sPackageQty = dri["item_qty"].ToString();
			if (sPackageQty == sQty.ToString() && Program.MyDoubleParse(sPackageQty) > 1 && sQty > 1)
			{
				return true;
			}
			return false;
		}
		private void CheckPromoFreeItem(string code, int nIndex, double dQtyIn, double dKeyInQty)
		{
			if (dst.Tables["checkfreeitem"] != null)
				dst.Tables["checkfreeitem"].Clear();
			if (code == "")
				return;
			string sc = " SELECT pl.free_item_reward_qty,pl.free_item_required_item_code, pl.free_item_required_qty, pl.promo_type  FROM promotion_list pl ";
			//sc += " JOIN code_relations c ON pl.promo_id = c.promo_id WHERE c.code = '" + code + "'";
			sc += " JOIN code_relations c ON pl.free_item_required_item_code = c.supplier_code WHERE c.code=" + code;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkfreeitem") <= 0)
				{
					return;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return;
			}

			DataRow dri = dst.Tables["checkfreeitem"].Rows[0];
			string sFreeItemReQty = dri["free_item_reward_qty"].ToString();
			string sFreeItemReCode = dri["free_item_required_item_code"].ToString();
			string sFreeItemReqQty = dri["free_item_required_qty"].ToString();
			string sFreeItemPromotype = dri["promo_type"].ToString();
			double sFreeItemOnCartSubTotalQty = 0;
			double sFreeItemOnCartTotalQty = Program.MyDoubleParse(sFreeItemReQty);
			double dFreeReRemain = Program.MyDoubleParse(sFreeItemReQty);
			double dFreeItemOnSaleMode = 0;
			double RewardItemQtyTotal = 0;
			//Analyse Quantity //
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string sFreeItemOnCartQty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				string sFreeItemOnCartCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
				string sFreeItemOnCartPrice = cart.Rows[i].Cells["cc_price"].Value.ToString();
				string sFreeItemOnCartPromotype = cart.Rows[i].Cells["cc_is_promotion"].Value.ToString();
				//promotion item total quantity//
				//if (code == sFreeItemOnCartCode )

				if (sFreeItemPromotype == sFreeItemOnCartPromotype && sFreeItemOnCartPrice != "0.00")
					sFreeItemOnCartSubTotalQty += Program.MyDoubleParse(sFreeItemOnCartQty);
				//Free item total //

				if (sFreeItemReCode == sFreeItemOnCartCode)
				{
					if (sFreeItemOnCartPrice == "0.00")
					{
						RewardItemQtyTotal += Program.MyDoubleParse(sFreeItemOnCartQty);
					}
					else
					{
						dFreeItemOnSaleMode += Program.MyDoubleParse(sFreeItemOnCartQty);
					}
				}
			}
			string sCardFisrtCode = cart.Rows[0].Cells["cc_code"].Value.ToString();
			double dPromotionItemTotal = sFreeItemOnCartSubTotalQty + dKeyInQty;
			if (sCardFisrtCode != code && qty.Text != "")
			{
				dPromotionItemTotal = dKeyInQty;
			}
			double dFreeItemRewardTotal = dPromotionItemTotal / Program.MyDoubleParse(sFreeItemReqQty) * Program.MyDoubleParse(sFreeItemReQty);
			int dFreeItemRewardTotalInt = Program.MyIntParse(dFreeItemRewardTotal.ToString());
			dFreeReRemain = dFreeItemRewardTotalInt - RewardItemQtyTotal;
			double dFreeItemRedonen = dFreeItemOnSaleMode - dFreeReRemain;
			//Apply Free Item In To The Cart //

			for (int e = 0; e < cart.Rows.Count; e++)
			{
				string sFreeItemOnCartCode = cart.Rows[e].Cells["cc_code"].Value.ToString();
				string sFreeItemOnCartPrice = cart.Rows[e].Cells["cc_price"].Value.ToString();
				if (sFreeItemReCode == sFreeItemOnCartCode)
				{
					if (RewardItemQtyTotal == 0 && dPromotionItemTotal >= Program.MyDoubleParse(sFreeItemReqQty) && dFreeItemOnSaleMode == 1)
					{
						DeleteCartItem(e);
						AddNewFreeItem(1, sFreeItemReCode);
					}
					if (RewardItemQtyTotal == 0 && dPromotionItemTotal >= Program.MyDoubleParse(sFreeItemReqQty) && dFreeItemOnSaleMode > 1)
					{
						cart.Rows[e].Cells["cc_price"].Value = sFreeItemOnCartPrice;
						cart.Rows[e].Cells["cc_qty"].Value = dFreeItemRedonen.ToString();
						cart.Rows[e].Cells["cc_total"].Value = (Program.MyDoubleParse(sFreeItemOnCartPrice) * dFreeItemRedonen).ToString();
						m_fmad.cart.Rows[e].Cells["cc_total"].Value = (Program.MyDoubleParse(sFreeItemOnCartPrice) * dFreeItemRedonen).ToString();
						m_fmad.cart.Rows[e].Cells["cc_qty"].Value = dFreeItemRedonen.ToString();

						m_fmads.cart.Rows[e].Cells["cc_total"].Value = (Program.MyDoubleParse(sFreeItemOnCartPrice) * dFreeItemRedonen).ToString();
						m_fmads.cart.Rows[e].Cells["cc_qty"].Value = dFreeItemRedonen.ToString();

						cart.Rows[e].Cells["cc_is_promotion"].Value = "0";
						CalcCartTotal();

						if (dFreeReRemain >= dFreeItemOnSaleMode)
						{
							DeleteCartItem(e);
							AddNewFreeItem(dFreeItemOnSaleMode, sFreeItemReCode);
						}
						else
						{
							AddNewFreeItem(dFreeReRemain, sFreeItemReCode);
						}
						break;
					}
					if (RewardItemQtyTotal > 0 && sFreeItemOnCartSubTotalQty >= Program.MyDoubleParse(sFreeItemReqQty) && dFreeItemOnSaleMode == 1 && Program.MyDoubleParse(sFreeItemOnCartPrice) != 0)
					{
						DeleteCartItem(e);
						AddNewFreeItem(1, sFreeItemReCode);
					}
					if (RewardItemQtyTotal > 0 && sFreeItemOnCartSubTotalQty >= Program.MyDoubleParse(sFreeItemReqQty) && dFreeItemOnSaleMode > 1 && Program.MyDoubleParse(sFreeItemOnCartPrice) != 0)
					{
						cart.Rows[e].Cells["cc_price"].Value = sFreeItemOnCartPrice;
						cart.Rows[e].Cells["cc_qty"].Value = dFreeItemRedonen.ToString();
						cart.Rows[e].Cells["cc_total"].Value = (Program.MyDoubleParse(sFreeItemOnCartPrice) * dFreeItemRedonen).ToString();
						m_fmad.cart.Rows[e].Cells["cc_total"].Value = (Program.MyDoubleParse(sFreeItemOnCartPrice) * dFreeItemRedonen).ToString();
						m_fmad.cart.Rows[e].Cells["cc_qty"].Value = dFreeItemRedonen.ToString();

						m_fmads.cart.Rows[e].Cells["cc_total"].Value = (Program.MyDoubleParse(sFreeItemOnCartPrice) * dFreeItemRedonen).ToString();
						m_fmads.cart.Rows[e].Cells["cc_qty"].Value = dFreeItemRedonen.ToString();

						cart.Rows[e].Cells["cc_is_promotion"].Value = "0";
						CalcCartTotal();
						if (dFreeReRemain >= dFreeItemOnSaleMode)
						{
							DeleteCartItem(e);
							AddNewFreeItem(dFreeItemOnSaleMode, sFreeItemReCode);
						}
						else
						{
							AddNewFreeItem(dFreeReRemain, sFreeItemReCode);
						}
						break;
					}
				}
			}
		}
		private void AddNewFreeItem(double dFreeReRemain, string sFreeItemReCode)
		{
			if (dst.Tables["cqp_fi"] != null)
				dst.Tables["cqp_fi"].Clear();
			string sc = " SELECT supplier_code, name, name_cn, avoid_point FROM code_relations WHERE code = " + sFreeItemReCode;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "cqp_fi") <= 0)
				{
					MessageBox.Show("Free Item not found, code = " + sFreeItemReCode, "Promotion Error");
					return;
				}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return;
			}
			DataRow drit = dst.Tables["cqp_fi"].Rows[0];
			string name = drit["name"].ToString();
			string name_cn = drit["name_cn"].ToString();
			name = name + " (Free Item) ";
			name_cn = name_cn + " (Free Item) ";
			AddToCart("", sFreeItemReCode, name, name_cn, 0, dFreeReRemain, "5", false, drit["supplier_code"].ToString(), 0, bool.Parse(drit["avoid_point"].ToString()), false);
		}
		//private void SearchReturn()
		//{
		//    FormSearch fm = new FormSearch();
		//    fm.kw.Text = barcode.Text;
		//    fm.m_skw = barcode.Text;
		//    this.barcode.Text = "";
		//    fm.ShowDialog();
		//    m_sCode = fm.m_sCode;
		//    DoScanBarcode();
		//}
		private void ModifyCart(int nIndex, double dQty)
		{
			if (nIndex < 0)
				return;
			double dPrice = Program.MyMoneyParse(cart.Rows[nIndex].Cells["cc_price"].Value.ToString());
			double dTotal = dPrice * dQty;
			cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
			cart.Rows[nIndex].Cells["cc_total"].Value = dTotal;
			m_fmad.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
			m_fmad.cart.Rows[nIndex].Cells["cc_total"].Value = dTotal;
			m_fmads.cart.Rows[nIndex].Cells["cc_qty"].Value = dQty;
			m_fmads.cart.Rows[nIndex].Cells["cc_total"].Value = dTotal;
			this.barcode.Focus();
		}
		private void doPromotionKeyInValue(string code, string name, string name_cn, double price, double dQtyKeyIn, string supplier_code, double require_qty, double rewarded_qty, int nIndex, double dTotalQtyForPromo, bool bNoPoint)
		{
			/*			double dTotalQtyPromoItem = 0;
						double dTotalQtyRewardedOnCart = 0;
						double dTotalQtyUnRewardedOnCart = 0;
						double dTotalRewardQtyKeyIn = dTotalQtyPromoItem / require_qty * rewarded_qty;
						int dTotalRewardQtyKeyInInt = Program.MyIntParse(dTotalRewardQtyKeyIn.ToString());
						double dTotalRewardRemaid = Program.MyDoubleParse(dTotalRewardQtyKeyInInt.ToString()) - dTotalQtyRewardedOnCart;
						double dTotalRowsCount = Program.MyDoubleParse(cart.Rows.Count.ToString());
			*/
			int nMainIndex = -1;
			int nRewardIndex = -1;
			double dQtyInCart = 0;
			for (int k = 0; k < cart.Rows.Count; k++)
			{
				string sQty = cart.Rows[k].Cells["cc_qty"].Value.ToString();
				string sCode = cart.Rows[k].Cells["cc_code"].Value.ToString();
				string sPrice = cart.Rows[k].Cells["cc_price"].Value.ToString();
				if (code != sCode)
					continue;
				dQtyInCart += Program.MyDoubleParse(sQty);
				//				dTotalQtyPromoItem += Program.MyDoubleParse(sQty);
				if (Program.MyMoneyParse(sPrice) != 0)
				{
					//					dTotalQtyUnRewardedOnCart += Program.MyDoubleParse(sQty);
					nMainIndex = k;
				}
				else
				{
					//					dTotalQtyRewardedOnCart += Program.MyDoubleParse(sQty);
					nRewardIndex = k;
				}
			}

			double dTotalQty = dQtyInCart + dQtyKeyIn;	//6
			double dMainQty = 0;
			double dRewardQty = 0;
			double dQtyPool = 0;
			for (double q = 0; q < dTotalQty; q++)
			{
				if (dQtyPool >= require_qty)
				{
					dRewardQty++;
					dQtyPool = 0;
				}
				else
				{
					dMainQty++;
					dQtyPool++;
				}
			}
			/*			
						int nRewardTimes = (int)(dTotalQty / require_qty);			//1
						double dRewardQtyExp = rewarded_qty * nRewardTimes;			//1
						double dMainQty = require_qty * nRewardTimes;				//5
						double dQtyRemain = dTotalQty - dMainQty;					//1
						double dRewardQty = dRewardQtyExp;							//1
						if(dRewardQty > dQtyRemain)
							dRewardQty = dQtyRemain;
						dMainQty += (dQtyRemain - dRewardQty);
			*/
			if (nMainIndex < 0)
				AddToCart("", code, name, name_cn, price, dMainQty, "3", false, supplier_code, 0, bNoPoint, false);
			else
				ModifyCart(nMainIndex, dMainQty);

			if (dRewardQty > 0)
			{
				if (nRewardIndex < 0)
					AddToCart("", code, name, name_cn, 0, dRewardQty, "0", false, supplier_code, 0, bNoPoint, false);
				else
					ModifyCart(nRewardIndex, dRewardQty);
			}
			return;

			/*			
						double dItemSaleQty = dQtyKeyIn; //new sales qty need add to cart
						double dRewardQty = 0;	//new free qty need add to cart
			//			double dRemainQty = 0;	//un rewarded qty, add seperatly
			
						int nRewards = (int)(dTotalQtyUnRewardedOnCart / require_qty);  //how many times already rewarded
						double dRemainQtyInCart = dTotalQtyUnRewardedOnCart - require_qty * nRewards;		//remain qty not rewarded yet
			
						double dQty = dQtyKeyIn + dRemainQtyInCart;
						int nRewardsNew = (int)(dQty / require_qty);
						if(nRewardsNew > 0)
						{
							dItemSaleQty = require_qty * nRewardsNew - dRemainQty;
							dRemainQty = dQtyKeyIn - dItemSaleQty;
						}
						AddToCart("", code, name, name_cn, price, dItemSaleQty, "3", false, supplier_code, 0, bNoPoint, false);
						if(dRemainQty > 0)
							AddToCart("", code, name, name_cn, 0, dRemainQty, "0", false, supplier_code, 0, bNoPoint, false);
						return;
			/*			
						if (dTotalRowsCount == 0 && dTotalQtyPromoItem == 0)
							dTotalQtyPromoItem = dQtyKeyIn;
						else
							dTotalQtyPromoItem = dTotalQtyForPromo;
						double dTotalScanTimes = require_qty + rewarded_qty;
						double dRewardQtyOnTimes = (dTotalQtyPromoItem) / dTotalScanTimes;
						int dRewardQtyOnTimesInt = Program.MyIntParse(dRewardQtyOnTimes.ToString());
						double dItemSaleQty = 0;
						if (dRewardQtyOnTimes >= 1)
						{ 
							dItemSaleQty = dTotalQtyPromoItem - dRewardQtyOnTimesInt;
							AddToCart("", code, name, name_cn, price, dItemSaleQty, "3", false, supplier_code, 0, bNoPoint, false);
							AddToCart("", code, name, name_cn, 0, dRewardQtyOnTimesInt, "0", false, supplier_code, 0, bNoPoint, false);
						}
						else
						{
							if (dTotalRowsCount == 0)
								AddToCart("", code, name, name_cn, price, dQtyKeyIn, "0", false, supplier_code, 0, bNoPoint,false);
						}
			*/
			return;
		}
        private void DoKeyInPromotionValueOne(string code, string name, string name_cn, double price, double dDisPrice, double dInputQty, double limit, string s_supplier_code, double dRequried_qty, bool bISNO_Point)
        {
            double d_overlimit = dInputQty - limit;
            AddToCart("", code, name, name_cn, dDisPrice, limit, "1", false, s_supplier_code, dDisPrice * limit, bISNO_Point, false);
            AddToCart("", code, name, name_cn, price, d_overlimit, "1", false, s_supplier_code, price * d_overlimit, bISNO_Point, false);

        }
		private void doKeyInPromotionValueFour(string code, string name, string name_cn, double price, double dDisTotalPrice, double dInputQty, string s_supplier_code, double dRequried_qty, bool bISNO_Point)
		{
			double dTotalDiscountTimes = dInputQty / dRequried_qty;
			int dTotalDiscountTimesInt = Program.MyIntParse(dTotalDiscountTimes.ToString());
			double dTotalDiscountTotal = Program.MyDoubleParse(dTotalDiscountTimesInt.ToString()) * dDisTotalPrice;
			double dQualifyDiscountTotal = Program.MyDoubleParse(dTotalDiscountTimesInt.ToString()) * dRequried_qty;
			double dRestPromoQty = dInputQty - dQualifyDiscountTotal;
			double dTotalRowsCount = Program.MyDoubleParse(cart.Rows.Count.ToString());
			if (dRestPromoQty >= 1)
			{
				AddToCart("", code, name, name_cn, price, dRestPromoQty, "4", false, s_supplier_code, price * dRestPromoQty, bISNO_Point, false);
			}
			if (dTotalDiscountTimesInt >= 1)
			{
				AddToCart("", code, name, name_cn, price, dQualifyDiscountTotal, "4", false, s_supplier_code, dTotalDiscountTotal, bISNO_Point, false);
				cart.Rows[0].Cells["cc_total"].Value = dTotalDiscountTotal.ToString();
			}
			else
			{
			}
		}
		private bool doWarningItem(double sWanringPrice, int dPromotionItem)
		{
			if (sWanringPrice == 0 && dPromotionItem == 0)
				return true;
			else if (sWanringPrice >= 1000 && dPromotionItem == 0)
				return true;
			else
				return false;
		}
		private bool OverTransactionTotal()
		{
			double dCheckTotal = m_dOrderTotal + Program.MyMoneyParse(Cashout.Text);
			if (dCheckTotal > 99999.99)
			{
				MessageBox.Show("Sorry, transaction amount cannot greater than $99,999.99");
				return true;
			}
			return false;
		}
		private bool doWariningPayment(string sPaymentType, double dAmount, string sBalance)
		{
			double dRest = dAmount - Program.MyDoubleParse(sBalance);
			double dWarningTotal = Math.Round(dAmount / Program.MyDoubleParse(sBalance), 2);
			if (sPaymentType == "eftpos" || sPaymentType == "credit")
			{
				if (OverTransactionTotal())
					return false;
				if (m_sCashoutcontrol == "0") // NO CASH OUT
				{
					if (dRest > 0)
					{
						MessageBox.Show("Sorry, You are over Charge");
						if (sPaymentType == "credit")
						{
							m_dCredit = 0;
							this.Credit.Text = this.labelBalance.Text;
							this.Credit.Focus();
							this.Credit.Select();
						}
						else
						{
							m_dEftpos = 0;
							this.Eftpos.Text = this.labelBalance.Text;
							Eftpos.Focus();
							Eftpos.Select();
						}
						return false;
					}
				}
				else
				{
/*					if (dRest > Program.MyDoubleParse(m_sCashoutcontrol)) // Over cash out limit
					{
						MessageBox.Show("Sorry, Over Cash Out Limit " + Program.MyMoneyParse(m_sCashoutcontrol).ToString("N2"), "Cash Out Warning", MessageBoxButtons.OK);
						if (sPaymentType == "credit")
						{
							m_dCredit = 0;
							this.Credit.Text = this.labelBalance.Text;
							this.Credit.Focus();
							this.Credit.Select();
						}
						else
						{
							m_dEftpos = 0;
							this.Eftpos.Text = this.labelBalance.Text;
							this.Eftpos.Focus();
							this.Eftpos.Select();
						}
						return false;
					}
*/					
					/*
					else if ((m_dCashTotal != 0 || m_dCash !=0)&& dRest > 0 )
					{  
						MessageBox.Show("Sorry, No Cashout Service When Cash Received");
						this.Eftpos.Text = this.labelBalance.Text;
						this.Eftpos.Select();
						this.Eftpos.Focus();
						return false;
					}
					 */
				}
			}
			else if (sPaymentType == "cash")
			{
				if (dWarningTotal > Program.MyDoubleParse(m_sCashOverChangeControl))
				{
					if (MessageBox.Show("Sorry, Please Confirm Cash In", "Payment Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
					{
						m_dCash = 0;
						this.Cash.Text = this.labelBalance.Text;
						this.Cash.Focus();
						this.Cash.Select();
						return false;
					}
				}
			}
			else if (sPaymentType == "cashout")
			{
				if (OverTransactionTotal())
					return false;
/*				if (dAmount > Program.MyDoubleParse(m_sCashoutcontrol)) // Over cash out limit
				{
					MessageBox.Show("Sorry, Over Cash Out Limit " + Program.MyMoneyParse(m_sCashoutcontrol).ToString("N2") + " " + Program.MyMoneyParse(m_sCashoutcontrol).ToString("N2"));
					m_dCashout = 0;
					this.Cashout.Text = "";
					this.Cashout.Focus();
					this.Cashout.Select();
					return false;
				}
*/ 
			}
			else if (sPaymentType == "cheque")
			{
				if (dAmount > Program.MyDoubleParse(sBalance))
				{
					MessageBox.Show("Sorry, Please Check Cheque Payment Amount");
					this.Cheque.Text = this.labelBalance.Text;
					this.Cheque.Focus();
					this.Cheque.Select();
					return false;
				}
			}
			else if (sPaymentType == "charge")
			{
				if (dAmount > Program.MyDoubleParse(sBalance))
				{
					MessageBox.Show("Sorry, Please Check Charge Payment Amount");
					this.charge.Text = this.labelBalance.Text;
					this.charge.Select();
					this.charge.Focus();
					m_dChargeTotal = 0;
					return false;
				}
			}
			return true;
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (Program.m_bEnableEftpos && Program.m_eftposType == "verifone" && !m_bCheckVaultDone)
			{
				m_bCheckVaultDone = true;
				eftPanel.CheckVault();
			}
			if (m_bCheckGroupPromotion)
			{
				CheckGroupPromotion();
				m_bCheckGroupPromotion = false;
			}
            if (m_bCheckComboPromotion)
            {
                CheckComboPromotion();
                m_bCheckComboPromotion = false;
            }

			this.Text = "GPOS SYSTEM " + " " + DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToLongTimeString();
			if (m_bTrial)
			{
				this.Text += " TRIAL" + Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", "")) + " Days Left !!";
				btnPayByPoint.Text = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", ""));

				//if (Convert.ToDouble(Registry.GetValue("HKEY_LOCAL_MACHINE\\" + Program.m_sSecrectValue + "\\", "expired_day", "")) < 0 || m_bTrialExpired == true)
				//{
				//    timer1.Interval = 30000;
				//    FormMSG msg = new FormMSG();
				//    msg.btnNo.Visible = false;
				//    msg.btnYes.Visible = false;
				//    msg.m_sMsg = " Trial Version, " + Convert.ToString(Registry.GetValue("HKEY_LOCAL_MACHINE\\" + Program.m_sSecrectValue + "\\", "expired_day", "")) + " Days Left !!";
				//    msg.ShowDialog();
				//}
			}
		}

		private void buttonCart1_Click(object sender, EventArgs e)
		{
			RestoreHoldOrder(0);
			m_sCurrentCartID = 0;
			//     panelHold.Visible = false;
			panelHold.Visible = true;
			//		lbl_showhold.Visible = true;
			lbl_showhold.Text = ShowHoldedTables();
		}
		private void buttonCart2_Click(object sender, EventArgs e)
		{
			RestoreHoldOrder(1);
			m_sCurrentCartID = 1;
			//			panelHold.Visible = false;
			panelHold.Visible = true;
			//			lbl_showhold.Visible = true;
			lbl_showhold.Text = ShowHoldedTables();
		}
		private void buttonCart3_Click(object sender, EventArgs e)
		{
			RestoreHoldOrder(2);
			m_sCurrentCartID = 2;
			//			panelHold.Visible = false;
			panelHold.Visible = true;
			//			lbl_showhold.Visible = true;
			lbl_showhold.Text = ShowHoldedTables();
		}
		private void buttonCart4_Click(object sender, EventArgs e)
		{
			RestoreHoldOrder(3);
			m_sCurrentCartID = 3;
			//			panelHold.Visible = false;
			panelHold.Visible = true;
			//			lbl_showhold.Visible = true;
			lbl_showhold.Text = ShowHoldedTables();
		}
		private string SpecialItemNormalPrice(string code, string price)
		{
			double dGetNormalPrice = GetNormalPrice(code);
			string sItemNormalPrice = "";
			if (dGetNormalPrice > Program.MyDoubleParse(price))
				sItemNormalPrice = "(" + dGetNormalPrice.ToString("c") + ")";
			return sItemNormalPrice;
		}
		private bool CheckSpecialItem(string col, string sSupplier_Code)
		{
			if (dst.Tables["csi"] != null)
				dst.Tables["csi"].Clear();
			string sc = " SELECT " + col;
			sc += " FROM  code_relations WHERE ";
			sc += "supplier_code ='" + sSupplier_Code + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "csi") <= 0)
					return false;
			}
			catch (Exception)
			{
				return false; ;
			}
			if (dst.Tables["csi"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["csi"].Rows[0];
				bool isItemBe = Program.MyBooleanParse(dr[col].ToString());
				if (isItemBe)
					return true;
				else
					return false;
			}
			return false;
		}
		private string doBarcodeWithPrice(string barcode, int barcode_Set)
		{
			int m_sbarcodeLength = int.Parse(Program.m_sbarcodelength1);
			int m_spriceLength = int.Parse(Program.m_spricelength1);
			int m_spoint = int.Parse(Program.m_spoint1);
			if (barcode_Set == 2)
			{
				m_sbarcodeLength = int.Parse(Program.m_sbarcodelength2);
				m_spriceLength = int.Parse(Program.m_spricelength2);
				m_spoint = int.Parse(Program.m_spoint2);
			}
			else if (barcode_Set == 3)
			{
				m_sbarcodeLength = int.Parse(Program.m_sbarcodelength3);
				m_spriceLength = int.Parse(Program.m_spricelength3);
				m_spoint = int.Parse(Program.m_spoint3);
			}
			string s_priceonbarcode = barcode.Substring(m_sbarcodeLength, m_spriceLength - 1).ToString();
			double d_sNewPrice = Program.MyDoubleParse(s_priceonbarcode) / Program.MyDoubleParse(m_spoint.ToString());
			string m_sNewPrice = d_sNewPrice.ToString("N2");
			return m_sNewPrice;
		}
		private bool changeProtection(double m_dChange)
		{
			double s_CashIn = Program.MyDoubleParse(this.Cash.Text);
			s_CashIn = Math.Round(s_CashIn, 3);
			double manualCal = m_dChange + m_dTotal;
			manualCal -= m_dCashTotal;
			manualCal -= m_dEftposTotal;
			manualCal -= m_dChequeTotal;
			manualCal = Math.Round(manualCal, 2);
			double m_dChangeWarning = manualCal - s_CashIn;
			m_dChangeWarning = Math.Round(m_dChangeWarning, 2);
			if (m_dChangeWarning < 0)
				m_dChangeWarning = 0 - m_dChangeWarning;
			if (m_dChangeWarning > 0.05 && m_dChangeWarning < 1 && m_dTotal >= 0)
			{
				MessageBox.Show("manualCal " + manualCal.ToString() + "cash in " + s_CashIn + " total " + m_dTotal.ToString() + " cashind=" + s_CashIn.ToString() + " Change =" + m_dChange.ToString() + " CODE: P 1007");
				return false;
			}
			return true;
		}

		private void DoTare(double dQtyChange)
		{
			int iRows = cart.CurrentRow.Index;
			double m_dNewRowTotal = 0;
			string m_sItemUnitPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();

			string s_ChangeQtyCode = cart.Rows[iRows].Cells["cc_code"].Value.ToString();
			string s_ChangeQtyName = cart.Rows[iRows].Cells["cc_name"].Value.ToString();
			string s_ChangeQtyNameCN = cart.Rows[iRows].Cells["cc_name_cn"].Value.ToString();
			string s_ChangeQtyPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();
			string s_Avoid_point = cart.Rows[iRows].Cells["cc_avoid_point"].Value.ToString();
			string s_ChangeQtySupplier_code = cart.Rows[iRows].Cells["cc_supplier_code"].Value.ToString();

			double dQtyOld = Program.MyDoubleParse(cart.Rows[iRows].Cells["cc_qty"].Value.ToString());
			double dNewQty = dQtyOld - dQtyChange;// + dQtyOld;
			if (dNewQty == 0)
			{
				DeleteCartItem(iRows);
				CheckGroupPromotion();
                CheckComboPromotion();
				return;
			}
			if (dNewQty < 0)
				return;

			m_dNewRowTotal = Program.MyDoubleParse(m_sItemUnitPrice) * dNewQty;
			cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
			cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
			m_fmad.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
			m_fmad.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
			m_fmads.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
			m_fmads.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
			UpdateTotalPriceDiscplay();
			this.barcode.Focus();
		}

		private void doAddQty(double dQtyChange, string row)
		{
			int iRows = 0;
			if (row != null && row != "")
				iRows = cart.Rows[Program.MyIntParse(row)].Index;
			else
				iRows = cart.CurrentRow.Index;
			double m_dNewRowTotal = 0;
			string m_sItemUnitPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();

			string s_ChangeQtyCode = cart.Rows[iRows].Cells["cc_code"].Value.ToString();
			string s_ChangeQtyName = cart.Rows[iRows].Cells["cc_name"].Value.ToString();
			string s_ChangeQtyNameCN = cart.Rows[iRows].Cells["cc_name_cn"].Value.ToString();
			string s_ChangeQtyPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();
			string s_Avoid_point = cart.Rows[iRows].Cells["cc_avoid_point"].Value.ToString();
			string s_ChangeQtySupplier_code = cart.Rows[iRows].Cells["cc_supplier_code"].Value.ToString();
			string s_ChangeQtyPromotionType = "";
			string s_ChangeQtyIsPromotion = "";
			double d_ChangeQtyDisTotal = 0;
			double d_ChangeQtyRequiredQty = 0;

			double dQtyOld = Program.MyDoubleParse(cart.Rows[iRows].Cells["cc_qty"].Value.ToString());
			double dNewQty = dQtyChange + dQtyOld;
			if (dNewQty == 0)
			{
				DeleteCartItem(iRows);
				return;
			}
			if (dNewQty < 0)
				return;

			string sc = "SELECT promo_id FROM code_relations WHERE code =" + s_ChangeQtyCode;
			if (dst.Tables["crcq"] != null)
				dst.Tables["crcq"].Clear();
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "crcq");
			}
			catch (Exception)
			{
				return;
			}
			if (dst.Tables["crcq"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["crcq"].Rows[0];
				s_ChangeQtyIsPromotion = dr["promo_id"].ToString();
			}
			int nWeekDay = (int)DateTime.Now.DayOfWeek;
			if (nWeekDay == 0)
				nWeekDay = 7; //tee's design, Sunday is 7
			string sc1 = "SELECT p.volumn_discount_price_total, p.volumn_discount_qty, p.promo_type ";
			sc1 += " FROM promotion_list p JOIN code_relations c ON p.promo_id = c.promo_id WHERE 1=1"; ;
			sc1 += " AND p.promo_start_date <= GETDATE() AND p.promo_end_date >= GETDATE() "; //started, but not yet expired
//			if (CheckQtyPromotionValue(s_ChangeQtyCode, nWeekDay, ""))
				sc1 += " AND p.promo_day" + nWeekDay.ToString() + " = 1 "; //today is in the DayOfWeek list 
			sc1 += " AND c.code =" + s_ChangeQtyCode;

			if (dst.Tables["changeQty"] != null)
				dst.Tables["changeQty"].Clear();
			try
			{
				myAdapter = new SqlDataAdapter(sc1, myConnection);
				myAdapter.Fill(dst, "changeQty");
			}
			catch (Exception)
			{
				return;
			}
			if (dst.Tables["changeQty"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["changeQty"].Rows[0];
				d_ChangeQtyDisTotal = Program.MyDoubleParse(dr["volumn_discount_price_total"].ToString());
				d_ChangeQtyRequiredQty = Program.MyDoubleParse(dr["volumn_discount_qty"].ToString());
				s_ChangeQtyPromotionType = dr["promo_type"].ToString();
			}
			if (s_ChangeQtyIsPromotion != "" && s_ChangeQtyPromotionType == "4")
			{
				DeleteCartItem(iRows);
				doKeyInPromotionValueFour(s_ChangeQtyCode, s_ChangeQtyName, s_ChangeQtyNameCN, Program.MyDoubleParse(s_ChangeQtyPrice), d_ChangeQtyDisTotal, dNewQty, s_ChangeQtySupplier_code, d_ChangeQtyRequiredQty, true);
			}
			else
			{
				m_dNewRowTotal = Program.MyDoubleParse(m_sItemUnitPrice) * dNewQty;
				cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				m_fmad.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				m_fmad.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				m_fmads.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				m_fmads.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				CalcCartTotal();
				UpdateDtCart(m_nCurrentCart);
				CheckGroupPromotion();
                CheckComboPromotion();
				UpdateTotalPriceDiscplay();
				m_fmads.cart.Rows[iRows].Selected = true;

				this.barcode.Focus();
			}
		}
		private void doChangeQty(double dQtyChange, string row)
		{
			int iRows = 0;
			if (row != null && row != "")
				iRows = cart.Rows[Program.MyIntParse(row)].Index;
			else
				iRows = cart.CurrentRow.Index;
			double m_dNewRowTotal = 0;
			string m_sItemUnitPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();

			string s_ChangeQtyCode = cart.Rows[iRows].Cells["cc_code"].Value.ToString();
			string s_ChangeQtyName = cart.Rows[iRows].Cells["cc_name"].Value.ToString();
			string s_ChangeQtyNameCN = cart.Rows[iRows].Cells["cc_name_cn"].Value.ToString();
			string s_ChangeQtyPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();
			string s_Avoid_point = cart.Rows[iRows].Cells["cc_avoid_point"].Value.ToString();
			string s_ChangeQtySupplier_code = cart.Rows[iRows].Cells["cc_supplier_code"].Value.ToString();
			string s_ChangeQtyPromotionType = "";
			string s_ChangeQtyIsPromotion = "";
			double d_ChangeQtyDisTotal = 0;
			double d_ChangeQtyRequiredQty = 0;
            double d_Limit = 0;
            double d_OverLimit = 0;
            double d_price1 = 0;
            double d_special_price = 0;

			double dQtyOld = Program.MyDoubleParse(cart.Rows[iRows].Cells["cc_qty"].Value.ToString());
			double dNewQty = dQtyChange;// + dQtyOld;
			if (dNewQty == 0)
			{
				DeleteCartItem(iRows);
                CheckGroupPromotion();
                CheckComboPromotion();
				return;
			}
			if (dNewQty < 0)
				return;

			string sc = "SELECT promo_id FROM code_relations WHERE code =" + s_ChangeQtyCode;
			if (dst.Tables["crcq"] != null)
				dst.Tables["crcq"].Clear();
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "crcq");
			}
			catch (Exception)
			{
				return;
			}
			if (dst.Tables["crcq"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["crcq"].Rows[0];
				s_ChangeQtyIsPromotion = dr["promo_id"].ToString();
			}
			int nWeekDay = (int)DateTime.Now.DayOfWeek;
			if (nWeekDay == 0)
				nWeekDay = 7; //tee's design, Sunday is 7
            string sc1 = "SELECT p.volumn_discount_price_total, p.volumn_discount_qty, p.promo_type, isnull(p.limit,0) as limit, c.price1, p.special_price ";
			sc1 += " FROM promotion_list p JOIN code_relations c ON p.promo_id = c.promo_id WHERE 1=1"; ;
			sc1 += " AND p.promo_start_date <= GETDATE() AND p.promo_end_date >= GETDATE() "; //started, but not yet expired
			if (CheckQtyPromotionValue(s_ChangeQtyCode, nWeekDay, ""))
				sc1 += " AND p.promo_day" + nWeekDay.ToString() + " = 1 "; //today is in the DayOfWeek list 
			sc1 += " AND c.code =" + s_ChangeQtyCode;

			if (dst.Tables["changeQty"] != null)
				dst.Tables["changeQty"].Clear();
			try
			{
				myAdapter = new SqlDataAdapter(sc1, myConnection);
				myAdapter.Fill(dst, "changeQty");
			}
			catch (Exception)
			{
				return;
			}
			if (dst.Tables["changeQty"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["changeQty"].Rows[0];
				d_ChangeQtyDisTotal = Program.MyDoubleParse(dr["volumn_discount_price_total"].ToString());
				d_ChangeQtyRequiredQty = Program.MyDoubleParse(dr["volumn_discount_qty"].ToString());
				s_ChangeQtyPromotionType = dr["promo_type"].ToString();
                d_Limit = Program.MyDoubleParse(dr["limit"].ToString());
                d_price1 = Program.MyDoubleParse(dr["price1"].ToString());
                d_special_price = Program.MyDoubleParse(dr["special_price"].ToString());
			}
			if (s_ChangeQtyIsPromotion != "" && s_ChangeQtyPromotionType == "4")
			{
				DeleteCartItem(iRows);
				doKeyInPromotionValueFour(s_ChangeQtyCode, s_ChangeQtyName, s_ChangeQtyNameCN, Program.MyDoubleParse(s_ChangeQtyPrice), d_ChangeQtyDisTotal, dNewQty, s_ChangeQtySupplier_code, d_ChangeQtyRequiredQty, true);
			}
            else if (s_ChangeQtyIsPromotion != "" && s_ChangeQtyPromotionType == "1" && d_Limit > 0 && dNewQty > d_Limit)
            {
                DeleteCartItem(iRows);
                //d_OverLimit =  dNewQty -  d_Limit;
                DoKeyInPromotionValueOne(s_ChangeQtyCode, s_ChangeQtyName, s_ChangeQtyNameCN, d_price1, d_special_price, dNewQty, d_Limit, s_ChangeQtySupplier_code, d_ChangeQtyRequiredQty, true);

            }
			else
			{
				m_dNewRowTotal = Program.MyDoubleParse(m_sItemUnitPrice) * dNewQty;
				cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				m_fmad.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				m_fmad.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				m_fmads.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				m_fmads.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				CalcCartTotal();
				UpdateDtCart(m_nCurrentCart);
				UpdateTotalPriceDiscplay();
				m_bQtyEditing = true;
				CheckQtyPromotion(s_ChangeQtyCode, dNewQty, iRows, "");
                CheckGroupPromotion();
                CheckComboPromotion();
				m_bQtyEditing = false;
				this.barcode.Focus();
			}
		}
		private void doChangeQty1(double dQtyChange)
		{
			int iRows = cart.CurrentRow.Index;
			double m_dNewRowTotal = 0;
			string m_sItemUnitPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();

			string s_ChangeQtyCode = cart.Rows[iRows].Cells["cc_code"].Value.ToString();
			string s_ChangeQtyName = cart.Rows[iRows].Cells["cc_name"].Value.ToString();
			string s_ChangeQtyNameCN = cart.Rows[iRows].Cells["cc_name_cn"].Value.ToString();
			string s_ChangeQtyPrice = cart.Rows[iRows].Cells["cc_price"].Value.ToString();
			string s_Avoid_point = cart.Rows[iRows].Cells["cc_avoid_point"].Value.ToString();
			string s_ChangeQtySupplier_code = cart.Rows[iRows].Cells["cc_supplier_code"].Value.ToString();
			string s_ChangeQtyPromotionType = "";
			string s_ChangeQtyIsPromotion = "";
			double d_ChangeQtyDisTotal = 0;
			double d_ChangeQtyRequiredQty = 0;

			double dQtyOld = Program.MyDoubleParse(cart.Rows[iRows].Cells["cc_qty"].Value.ToString());
			double dNewQty = dQtyChange;// + dQtyOld;
			if (dNewQty == 0)
			{
				DeleteCartItem(iRows);
				CheckGroupPromotion();
                CheckComboPromotion();
				return;
			}
			if (dNewQty < 0)
				return;

			string sc = "SELECT promo_id FROM code_relations WHERE code =" + s_ChangeQtyCode;
			if (dst.Tables["crcq"] != null)
				dst.Tables["crcq"].Clear();
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "crcq");
			}
			catch (Exception)
			{
				return;
			}
			if (dst.Tables["crcq"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["crcq"].Rows[0];
				s_ChangeQtyIsPromotion = dr["promo_id"].ToString();
			}
			int nWeekDay = (int)DateTime.Now.DayOfWeek + 1;
			//			if (nWeekDay == 0)
			//				nWeekDay = 7; //tee's design, Sunday is 7, darcy changed to 1
			string sc1 = "SELECT p.volumn_discount_price_total, p.volumn_discount_qty, p.promo_type ";
			sc1 += " FROM promotion_list p JOIN code_relations c ON p.promo_id = c.promo_id WHERE 1=1"; ;
			sc1 += " AND p.promo_start_date <= GETDATE() AND p.promo_end_date >= GETDATE() "; //started, but not yet expired
			if (CheckQtyPromotionValue(s_ChangeQtyCode, nWeekDay, ""))
				sc1 += " AND p.promo_day" + nWeekDay.ToString() + " = 1 "; //today is in the DayOfWeek list 
			sc1 += " AND c.code =" + s_ChangeQtyCode;
			if (dst.Tables["changeQty"] != null)
				dst.Tables["changeQty"].Clear();
			try
			{
				myAdapter = new SqlDataAdapter(sc1, myConnection);
				myAdapter.Fill(dst, "changeQty");
			}
			catch (Exception)
			{
				return;
			}
			if (dst.Tables["changeQty"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["changeQty"].Rows[0];
				d_ChangeQtyDisTotal = Program.MyDoubleParse(dr["volumn_discount_price_total"].ToString());
				d_ChangeQtyRequiredQty = Program.MyDoubleParse(dr["volumn_discount_qty"].ToString());
				s_ChangeQtyPromotionType = dr["promo_type"].ToString();
			}
			if (s_ChangeQtyIsPromotion != "" && s_ChangeQtyPromotionType == "4")
			{
				DeleteCartItem(iRows);
				doKeyInPromotionValueFour(s_ChangeQtyCode, s_ChangeQtyName, s_ChangeQtyNameCN, Program.MyDoubleParse(s_ChangeQtyPrice), d_ChangeQtyDisTotal, dNewQty, s_ChangeQtySupplier_code, d_ChangeQtyRequiredQty, true);
			}
			else
			{
				m_dNewRowTotal = Program.MyDoubleParse(m_sItemUnitPrice) * dNewQty;
				cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				m_fmad.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				m_fmad.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				m_fmads.cart.Rows[iRows].Cells["cc_qty"].Value = dNewQty.ToString();
				m_fmads.cart.Rows[iRows].Cells["cc_total"].Value = m_dNewRowTotal.ToString("N2");
				CalcCartTotal();
				UpdateDtCart(m_nCurrentCart);
				UpdateTotalPriceDiscplay();
				this.barcode.Focus();
			}
			CheckGroupPromotion();
            CheckComboPromotion();
		}
		private void doShowPaymentInfo()
		{
			string s_Details = "";
			double m_dEftposTotalPaid = m_dEftposTotal + m_dEftpos;
			double m_dCreditTotalPaid = m_dCreditTotal + m_dCredit;
			double m_dChequeTotalPaid = m_dChequeTotal + m_dCheque;
			double m_dCashTotalPaid = m_dCashTotal + m_dCash;
			m_dCashInTotal = m_dCashTotalPaid;
			if (m_dCashTotal != 0 || this.Cash.Text != "" || m_dCashInTotal != 0)
			{
				s_Details += "Cash ";
				if (m_dCashTotal < 0 || Program.MyDoubleParse(this.Cash.Text) < 0)
					s_Details += "Refund ";
				s_Details += m_dCashTotalPaid.ToString("c") + "\r\n";
			}
			if (m_dEftposTotal != 0 || this.Eftpos.Text != "")
				s_Details += "Eftpos " + "    " + m_dEftposTotalPaid.ToString("c") + "\r\n";
			if (m_dCreditTotal != 0 || this.Credit.Text != "")
				s_Details += "Credit Card" + "    " + m_dCreditTotalPaid.ToString("c") + "\r\n";
			if (m_dChequeTotal != 0 || this.Cheque.Text != "")
				s_Details += "Cheque" + "    " + m_dChequeTotalPaid.ToString("c") + "\r\n";
			this.txtpaymentinfo.Text = s_Details;
		}
		private void sShowItemTotal()
		{
			double dQty = 0;
            double dQtys = 0;
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string sQty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
                string sCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
                if (sCode == "")
                    continue;
                dQty = dQty + 1;// Program.MyDoubleParse(sQty);
                dQtys += Program.MyDoubleParse(sQty);
			}
			this.showItems.Text = dQty.ToString();
            this.lblQtys.Text = dQtys.ToString();
		}
		private void RollBackDiscountOneItem()
		{
			int i = cart.CurrentRow.Index;
			string sSupCode = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
			string sCheckDiscountValue = cart.Rows[i].Cells["cc_discount"].Value.ToString();
			string sCheckNormalPrice = cart.Rows[i].Cells["cc_normalprice"].Value.ToString();
			string sCheckQty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
			double dRollBackTotal = Math.Round(Program.MyDoubleParse(sCheckNormalPrice) * Program.MyDoubleParse(sCheckQty), 2);
			string sNameCN = cart.Rows[i].Cells["cc_name_cn"].Value.ToString();
			string sdName = cart.Rows[i].Cells["cc_name_en"].Value.ToString();
			if (dst.Tables["total_dis1"] != null)
				dst.Tables["total_dis1"].Clear();
			string sc = " SELECT ISNULL(promo_id, 0) AS promo_id, is_special FROM code_relations WHERE code = '" + sSupCode + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "total_dis1");
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
				return;
			}
			DataRow itdi = dst.Tables["total_dis1"].Rows[0];
			string sIsPromItem = itdi["promo_id"].ToString();
			bool bIsSpe = Program.MyBooleanParse(itdi["is_special"].ToString());
			if (sIsPromItem != "0")
				return;
			if (bIsSpe)
				return;
			if (m_bChineseDesc)
			{
				if (sNameCN != "")
					sdName = sNameCN;
			}
			if (sCheckDiscountValue != "")
			{
				cart.Rows[i].Cells["cc_price"].Value = sCheckNormalPrice;
				cart.Rows[i].Cells["cc_total"].Value = dRollBackTotal.ToString();
				cart.Rows[i].Cells["cc_discount"].Value = "";
				cart.Rows[i].Cells["cc_name"].Value = sdName;
				m_fmad.cart.Rows[i].Cells["cc_price"].Value = sCheckNormalPrice;
				m_fmad.cart.Rows[i].Cells["cc_total"].Value = dRollBackTotal.ToString();
				m_fmad.cart.Rows[i].Cells["cc_discount"].Value = "";
				m_fmad.cart.Rows[i].Cells["cc_name"].Value = sdName;
				m_fmads.cart.Rows[i].Cells["cc_price"].Value = sCheckNormalPrice;
				m_fmads.cart.Rows[i].Cells["cc_total"].Value = dRollBackTotal.ToString();
				m_fmads.cart.Rows[i].Cells["cc_discount"].Value = "";
				m_fmads.cart.Rows[i].Cells["cc_name"].Value = sdName;
			}
			this.msgboard.Text = "";
			m_bBeDisced = false;
			UpdateTotalPriceDiscplay();
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
		}
		private void vDiscountBeRollBack(bool isCancelled)
		{
			if (isCancelled)
				return;
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string sSupCode = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
				string sCheckDiscountValue = cart.Rows[i].Cells["cc_discount"].Value.ToString();
				string sCheckNormalPrice = cart.Rows[i].Cells["cc_normalprice"].Value.ToString();
				string sCheckQty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				double dRollBackTotal = Math.Round(Program.MyDoubleParse(sCheckNormalPrice) * Program.MyDoubleParse(sCheckQty), 2);
				string sNameCN = cart.Rows[i].Cells["cc_name_cn"].Value.ToString();
				string sdName = cart.Rows[i].Cells["cc_name_en"].Value.ToString();
				if (dst.Tables["total_dis1"] != null)
					dst.Tables["total_dis1"].Clear();
	//			string sc = " SELECT ISNULL(promo_id, 0) AS promo_id, is_special FROM code_relations WHERE supplier_code = '" + sSupCode + "'";
                string sc = " SELECT ISNULL(promo_id, 0) AS promo_id, is_special FROM code_relations WHERE code = '" + sSupCode + "'";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					myAdapter.Fill(dst, "total_dis1");
				}
				catch (Exception e)
				{
					myConnection.Close();
					Program.ShowExp(sc, e);
					return;
				}
				DataRow itdi = dst.Tables["total_dis1"].Rows[0];
				string sIsPromItem = itdi["promo_id"].ToString();
				bool bIsSpe = Program.MyBooleanParse(itdi["is_special"].ToString());
				if (sIsPromItem != "0")
                    //return;
                    continue;
				if (bIsSpe)
                    //return;
                    continue;
				if (m_bChineseDesc)
				{
					if (sNameCN != "")
						sdName = sNameCN;
				}
				if (sCheckDiscountValue != "")
				{
					cart.Rows[i].Cells["cc_price"].Value = sCheckNormalPrice;
					cart.Rows[i].Cells["cc_total"].Value = dRollBackTotal.ToString();
					cart.Rows[i].Cells["cc_discount"].Value = "";
					cart.Rows[i].Cells["cc_name"].Value = sdName;
					m_fmad.cart.Rows[i].Cells["cc_price"].Value = sCheckNormalPrice;
					m_fmad.cart.Rows[i].Cells["cc_total"].Value = dRollBackTotal.ToString();
					m_fmad.cart.Rows[i].Cells["cc_discount"].Value = "";
					m_fmad.cart.Rows[i].Cells["cc_name"].Value = sdName;
					m_fmads.cart.Rows[i].Cells["cc_price"].Value = sCheckNormalPrice;
					m_fmads.cart.Rows[i].Cells["cc_total"].Value = dRollBackTotal.ToString();
					m_fmads.cart.Rows[i].Cells["cc_discount"].Value = "";
					m_fmads.cart.Rows[i].Cells["cc_name"].Value = sdName;
				}
			}
			this.msgboard.Text = "";
			m_bBeDisced = false;
			UpdateTotalPriceDiscplay();
			CalcCartTotal();
			UpdateDtCart(m_nCurrentCart);
		}
		private bool nextPosRef(int currentRef)
		{
			m_sPosRefLastest = Program.MyDoubleParse(currentRef.ToString()) + 1;
			int m_iPosRefFormat = int.Parse(m_sPosRefLastest.ToString());
			string sc = " UPDATE settings set value = '" + m_iPosRefFormat.ToString() + "' WHERE name ='pos_ref'";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception er)
			{
				myConnection.Close();
				Program.ShowExp(sc, er);
				return true;
			}
			return true;
		}

		private bool b_CheckPriceMemberOnly()
		{
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				int nPromotion = Program.MyIntParse(cart.Rows[i].Cells["cc_is_promotion"].Value.ToString());
				if (nPromotion != 0) //it's a promotion added item, no more discount
					continue;
				if (dst.Tables["memberprice"] != null)
					dst.Tables["memberprice"].Clear();
				string sPromoItemCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
				string sPromoItemPrice = cart.Rows[i].Cells["cc_price"].Value.ToString();
				string sPromoItemOrderQty = cart.Rows[i].Cells["cc_qty"].Value.ToString();
				string sc1 = " SELECT p.* FROM promotion_list p JOIN code_relations c ON p.promo_id = c.promo_id WHERE c.code = " + sPromoItemCode;
				sc1 += " AND p.promo_type IN (1, 2) AND p.promo_member_only=1";
				try
				{
					myAdapter = new SqlDataAdapter(sc1, myConnection);
					myAdapter.Fill(dst, "memberprice");
				}
				catch (Exception)
				{
					return false;
				}
				if (dst.Tables["memberprice"].Rows.Count > 0)
				{
					DataRow dr = dst.Tables["memberprice"].Rows[0];
					string s_mPromotionType = dr["promo_type"].ToString();
					string s_mMemberOnly = dr["promo_member_only"].ToString();
					string s_mMemberPromoPercentage = dr["discount_percentage"].ToString();
					string s_mMemberSpecial = dr["special_price"].ToString();
					string s_mPromoItem = dr["promo_id"].ToString();
					double d_mMemberNewPrice = 0;
					double d_mMemberNewRowTotal = 0;
					if (s_mPromotionType == "1")
						d_mMemberNewPrice = Math.Round(Program.MyDoubleParse(s_mMemberSpecial), 2);
					else if (s_mPromotionType == "2")
						d_mMemberNewPrice = Math.Round((1 - Program.MyDoubleParse(s_mMemberPromoPercentage) / 100) * Program.MyDoubleParse(sPromoItemPrice), 2);
					d_mMemberNewRowTotal = d_mMemberNewPrice * Program.MyDoubleParse(sPromoItemOrderQty);
					cart.Rows[i].Cells["cc_price"].Value = d_mMemberNewPrice.ToString();
					cart.Rows[i].Cells["cc_total"].Value = Math.Round(d_mMemberNewRowTotal, 2).ToString();
					m_fmad.cart.Rows[i].Cells["cc_price"].Value = d_mMemberNewPrice.ToString();
					m_fmad.cart.Rows[i].Cells["cc_total"].Value = Math.Round(d_mMemberNewRowTotal, 2).ToString();
					m_fmads.cart.Rows[i].Cells["cc_price"].Value = d_mMemberNewPrice.ToString();
					m_fmads.cart.Rows[i].Cells["cc_total"].Value = Math.Round(d_mMemberNewRowTotal, 2).ToString();
					cart.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.GreenYellow;
					UpdateTotalPriceDiscplay();
					CalcCartTotal();
					UpdateDtCart(m_nCurrentCart);
				}
			}
			return true;
		}

		private bool bMSLevelPrice()
		{
			if (cart.Rows.Count <= 0)
				return false;
			int nDay = (int)DateTime.Now.DayOfWeek;
			if (nDay == 0)
				nDay = 7;
			for (int j = 0; j < cart.Rows.Count; j++)
			{
				string cartItemCode = cart.Rows[j].Cells["cc_code"].Value.ToString();
				string cartItemSupplierCode = cart.Rows[j].Cells["cc_supplier_code"].Value.ToString();
				if (dst.Tables["MSprice"] != null)
					dst.Tables["MSprice"].Clear();
				string sc = " SELECT special_price, price1, price4 ";
				sc += ", supplier_code, ISNULL(is_special, '0') AS is_special, ISNULL(is_member_only, '0') AS is_member_only, ISNULL(date_range,'0') AS date_range, ISNULL(pick_date, '0')AS pick_date";
				sc += " FROM code_relations WHERE code=" + cartItemCode;
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "MSprice") <= 0)
						return false;
				}
				catch (Exception)
				{
					return false;
				}
				if (dst.Tables["MSprice"].Rows.Count > 0)
				{
					DataRow drr = dst.Tables["MSprice"].Rows[0];
					string msspecialprice = drr["special_price"].ToString();
					string cartOrderQty = cart.Rows[j].Cells["cc_qty"].Value.ToString();
					string membershipPrice = drr["price1"].ToString();
					bool bPickDay = Program.MyBooleanParse(drr["pick_date"].ToString());
					bool bDateRange = Program.MyBooleanParse(drr["date_range"].ToString());
					bool bIsMemberOnly = Program.MyBooleanParse(drr["is_member_only"].ToString());
					if (!bIsMemberOnly)
						continue;
					if (!bCheckMemberSpecial(cartItemSupplierCode, bDateRange, bPickDay, nDay))
						continue;
					if (Program.MyDoubleParse(drr["price4"].ToString()) > 0)
						membershipPrice = drr["price4"].ToString();
					double dNewCartOrderItemSubTotal = Math.Round(Program.MyDoubleParse(msspecialprice) * Program.MyDoubleParse(cartOrderQty), 2);
					cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name"].Value + "(" + Program.MyDoubleParse(membershipPrice).ToString("c") + ")";
					cart.Rows[j].Cells["cc_price"].Value = msspecialprice;
					cart.Rows[j].Cells["cc_total"].Value = Math.Round(dNewCartOrderItemSubTotal, 2).ToString();
					m_fmad.cart.Rows[j].Cells["cc_price"].Value = msspecialprice;
					m_fmad.cart.Rows[j].Cells["cc_name"].Value = m_fmad.cart.Rows[j].Cells["cc_name"].Value + "(" + Program.MyDoubleParse(membershipPrice).ToString("c") + ")";
					m_fmad.cart.Rows[j].Cells["cc_total"].Value = Math.Round(dNewCartOrderItemSubTotal, 2).ToString();

					m_fmads.cart.Rows[j].Cells["cc_price"].Value = msspecialprice;
					m_fmads.cart.Rows[j].Cells["cc_name"].Value = m_fmad.cart.Rows[j].Cells["cc_name"].Value + "(" + Program.MyDoubleParse(membershipPrice).ToString("c") + ")";
					m_fmads.cart.Rows[j].Cells["cc_total"].Value = Math.Round(dNewCartOrderItemSubTotal, 2).ToString();

					cart.Rows[j].DefaultCellStyle.BackColor = System.Drawing.Color.GreenYellow;
					UpdateTotalPriceDiscplay();
					CalcCartTotal();
					UpdateDtCart(m_nCurrentCart);
				}
			}
			return false;
		}
		private bool bMSSpecialCheck()
		{
			if (cart.Rows.Count <= 0)
				return false;
			int nDay = (int)DateTime.Now.DayOfWeek;
			if (nDay == 0)
				nDay = 7;
			for (int j = 0; j < cart.Rows.Count; j++)
			{
				int nPromotion = Program.MyIntParse(cart.Rows[j].Cells["cc_is_promotion"].Value.ToString());
				if (nPromotion != 0) //it's a promotion added item, no more discount
					continue;
				string cartItemCode = cart.Rows[j].Cells["cc_code"].Value.ToString();
				string cartItemSupplierCode = cart.Rows[j].Cells["cc_supplier_code"].Value.ToString();
				if (dst.Tables["MSprice"] != null)
					dst.Tables["MSprice"].Clear();
				string sc = " SELECT special_price, price1, price4 ";
				sc += ", supplier_code, ISNULL(is_special, '0') AS is_special, ISNULL(is_member_only, '0') AS is_member_only, ISNULL(date_range,'0') AS date_range, ISNULL(pick_date, '0')AS pick_date";
				sc += " FROM code_relations WHERE code=" + cartItemCode;
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "MSprice") <= 0)
						return false;
				}
				catch (Exception)
				{
					return false;
				}
				if (dst.Tables["MSprice"].Rows.Count > 0)
				{
					DataRow drr = dst.Tables["MSprice"].Rows[0];
					string msspecialprice = drr["special_price"].ToString();
					string cartOrderQty = cart.Rows[j].Cells["cc_qty"].Value.ToString();
					string membershipPrice = drr["price1"].ToString();
					bool bPickDay = Program.MyBooleanParse(drr["pick_date"].ToString());
					bool bDateRange = Program.MyBooleanParse(drr["date_range"].ToString());
					bool bIsMemberOnly = Program.MyBooleanParse(drr["is_member_only"].ToString());
					if (!bIsMemberOnly)
						continue;
					if (!bCheckMemberSpecial(cartItemSupplierCode, bDateRange, bPickDay, nDay))
						continue;
					if (Program.MyDoubleParse(drr["price4"].ToString()) > 0)
						membershipPrice = drr["price4"].ToString();
					double dNewCartOrderItemSubTotal = Math.Round(Program.MyDoubleParse(msspecialprice) * Program.MyDoubleParse(cartOrderQty), 2);
					cart.Rows[j].Cells["cc_name"].Value = cart.Rows[j].Cells["cc_name"].Value + "(" + Program.MyDoubleParse(membershipPrice).ToString("c") + ")";
					cart.Rows[j].Cells["cc_price"].Value = msspecialprice;
					cart.Rows[j].Cells["cc_total"].Value = Math.Round(dNewCartOrderItemSubTotal, 2).ToString();
					m_fmad.cart.Rows[j].Cells["cc_price"].Value = msspecialprice;
					m_fmad.cart.Rows[j].Cells["cc_name"].Value = m_fmad.cart.Rows[j].Cells["cc_name"].Value + "(" + Program.MyDoubleParse(membershipPrice).ToString("c") + ")";
					m_fmad.cart.Rows[j].Cells["cc_total"].Value = Math.Round(dNewCartOrderItemSubTotal, 2).ToString();

					m_fmads.cart.Rows[j].Cells["cc_price"].Value = msspecialprice;
					m_fmads.cart.Rows[j].Cells["cc_name"].Value = m_fmad.cart.Rows[j].Cells["cc_name"].Value + "(" + Program.MyDoubleParse(membershipPrice).ToString("c") + ")";
					m_fmads.cart.Rows[j].Cells["cc_total"].Value = Math.Round(dNewCartOrderItemSubTotal, 2).ToString();

					cart.Rows[j].DefaultCellStyle.BackColor = System.Drawing.Color.GreenYellow;
					UpdateTotalPriceDiscplay();
					CalcCartTotal();
					UpdateDtCart(m_nCurrentCart);
				}
			}
			return false;
		}
		private bool b_changeColor(string b_ChangeSupplierCode, bool special)
		{
			for (int i = 0; i < cart.Rows.Count; i++)
			{
				string rowsSupplier = cart.Rows[i].Cells["cc_supplier_code"].Value.ToString();
				string rowsCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
				if (dst.Tables["checkColor"] != null)
					dst.Tables["checkColor"].Clear();
				string sc1 = " SELECT p.* FROM promotion_list p JOIN code_relations c ON p.promo_id = c.promo_id WHERE c.code = " + rowsCode;
				sc1 += " AND p.promo_type IN (1, 2) AND p.promo_member_only = 1";
				try
				{
					myAdapter = new SqlDataAdapter(sc1, myConnection);
					myAdapter.Fill(dst, "checkColor");
				}
				catch (Exception)
				{
					return false;
				}
				if (dst.Tables["checkColor"].Rows.Count > 0)
				{
					if (b_ChangeSupplierCode == rowsSupplier)
						cart.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.GreenYellow;
					return true;
				}
				if (dst.Tables["checkMSColor"] != null)
					dst.Tables["checkMSColor"].Clear();
				string sc2 = "SELECT ISNULL(is_member_only, '0') AS is_member_only FROM code_relations WHERE supplier_code = '" + b_ChangeSupplierCode + "' AND is_member_only = 1";
				try
				{
					myAdapter = new SqlDataAdapter(sc2, myConnection);
					myAdapter.Fill(dst, "checkMSColor");
				}
				catch (Exception)
				{
					return false;
				}
				if (dst.Tables["checkMSColor"].Rows.Count > 0)
				{
					if (b_ChangeSupplierCode == rowsSupplier)
						cart.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.GreenYellow;
					return true;
				}
				if (special)
				{
					cart.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
					return true;
				}
			}
			return false;
		}
		private string TotalPoint()
		{
			//double dTotalPointVoucherAmount = 0;
			bool bIncloudeVoucher = false;
			double dTotalPointAvoidedAmount = 0;
			double m_dTotalRefund = 0;
			double m_dTotalRefundItem = 0;
			double m_dInvoiceTotal = m_dTotal;
			int dLegalPointPerRow = 0;
			for (int cc = 0; cc < cart.Rows.Count; cc++)
			{
				string sItemCode = cart.Rows[cc].Cells["cc_code"].Value.ToString();
				string sItemName = cart.Rows[cc].Cells["cc_name"].Value.ToString();
				string sItemCat = Program.GetCatByItemCode(sItemCode);
				string sNoPointItem = cart.Rows[cc].Cells["cc_avoid_point"].Value.ToString();
				bool bNoPointItem = bool.Parse(sNoPointItem);
				string sItemRowsTotal = cart.Rows[cc].Cells["cc_total"].Value.ToString();
				double dLegalPointRate = Program.MyDoubleParse(m_sVipPointRate);
				//dTotalPointAvoidedAmount = Program.MyIntParse(sItemRowsTotal) / Program.MyIntParse(dLegalPointRate.ToString());
				if (sItemCat.ToLower() == "voucher" || sItemName.ToLower().IndexOf("voucher") != -1)
				{
					//dTotalPointVoucherAmount += Program.MyDoubleParse(sItemRowsTotal);
					bIncloudeVoucher = true;
				}
				if (bNoPointItem)
					dTotalPointAvoidedAmount += Program.MyDoubleParse(sItemRowsTotal);
				m_dTotalRefundItem = Program.MyDoubleParse(sItemRowsTotal);

				if (sItemCat.ToLower() != "voucher" && sItemName.ToLower().IndexOf("voucher") == -1 && m_dTotalRefundItem < 0)
				{
					m_dTotalRefund += m_dTotalRefundItem;
					m_dTotalRefund = Math.Round(m_dTotalRefund, 2);
				}
			}

			dLegalPointPerRow = Program.MyIntParse((m_dInvoiceTotal - dTotalPointAvoidedAmount).ToString());
			m_nTotalLegalPoint = dLegalPointPerRow;
			if (!bIs_Refund && bIncloudeVoucher && m_nTotalLegalPoint < 0)
				m_nTotalLegalPoint = 0;
			//MessageBox.Show(m_nTotalLegalPoint.ToString());
			if (bIs_Refund)
				Program.RecordTillData("total_refund", m_dTotalRefund.ToString());// Record Refund Total
			return m_nTotalLegalPoint.ToString();
		}
		private void MemberShipID_MouseClick(object sender, MouseEventArgs e)
		{
			m_bScanMember = true;
		}
		public bool bCheckMemberSpecial(string mSupplier_code, bool bDateRange, bool bPickDay, int nToday)
		{
			/***** Do not need to check time frame.****/
			if (!bDateRange && !bPickDay)
				return true;
			/************* END *******************/
			if (mSupplier_code == "")
				return false;

			if (dst.Tables["checkMember"] != null)
				dst.Tables["checkMember"].Clear();
			string sc = " SELECT *  ";
			sc += " FROM specials WHERE ";
			if (mSupplier_code != "")
				sc += " supplier_code ='" + mSupplier_code + "'";
			if (bDateRange)
				sc += " AND s_start_date <= GETDATE()  AND s_end_date >= GETDATE()";
			if (bPickDay)
				sc += " AND s_d" + nToday + " = 1";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "checkMember");
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return false; ;
			}
			if (dst.Tables["checkMember"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["checkMember"].Rows[0];
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool CalcMemberPoints()
		{
			if (m_sCardBarcode == "")
				return true;
//			m_nMemberPoints = Program.MyIntParse(labelPoints.Text) + (int)m_dOrderTotal;
			m_nMemberPoints = Program.MyIntParse(labelPoints.Text) + m_nTotalLegalPoint;
			string sc = " UPDATE card SET points = " + m_nMemberPoints + " WHERE id = " + m_nCardId;
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
            if (m_nMemberPoints >= m_dVipVoucherStartPoints)
			{
                //if (m_bVoucherEnabled)
                if (m_bVipVoucherEnabled)
				{
					if (!DoCreateVoucher())
						return false;
                    //m_nMemberPoints -= (int)m_dVoucherStart;
                    m_nMemberPoints -= (int)m_dVipVoucherStartPoints;
					sc = " UPDATE card SET points = " + m_nMemberPoints + " WHERE id = " + m_nCardId;
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
				}
			}
			return true;
		}
		private void button1_Click(object sender, EventArgs e)
		{
		}
		private void MemberShipID_TextChanged(object sender, EventArgs e)
		{
		}
		private void charge_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!Program.IsNumberKey(e.KeyChar))
				e.Handled = true;
		}
		private void axOPOSScanner_DataEvent(object sender, AxOposScanner_1_8_Lib._IOPOSScannerEvents_DataEventEvent e)
		{
			string sBarcode = this.axOPOSScanner.ScanDataLabel.ToString(); //e.ToString();
			if (sBarcode != "")
			{
				if (m_bScanMember)
				{
					MemberShipID.Text = sBarcode;
					ScanMembership();
					bMSSpecialCheck();
					this.barcode.Focus();
					m_bScanMember = false;
				}
				else
				{
					barcode.Text = sBarcode;
					DoScanBarcode(false);
				}
			}
			this.axOPOSScanner.DataEventEnabled = true;
		}
		private void Cash_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			RefreshPayment("payment_cash");
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = Program.RoundCents(dGetBanlance, m_sRoundingNum).ToString("N2");
			this.Cash.Text = this.labelBalance.Text;
			this.Cashout.Text = "";
			this.Cash.SelectAll();
		}
		private void Eftpos_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			RefreshPayment("payment_eftpos");
            m_dRoundingTotal = 0;
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.Eftpos.Text = dGetBanlance.ToString("N2");
			this.Eftpos.SelectAll();
		}
		private void Cashout_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			if (this.Cash.Text != "")
			{
				FormMSG errMsg = new FormMSG();
				errMsg.btnNo.Visible = false;
				errMsg.btnYes.Visible = false;
				errMsg.m_sMsg = " Sorry , No Cash Out Allow \r\n Because Cash In Detected ";
				errMsg.ShowDialog();
				return;
			}
			if (this.Cheque.Text != "")
			{
				FormMSG errMsg = new FormMSG();
				errMsg.btnNo.Visible = false;
				errMsg.btnYes.Visible = false;
				errMsg.m_sMsg = " Sorry , No Cash Out Allow with cheque payment ";
				errMsg.ShowDialog();
				return;
			}
		}
		private void Credit_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			RefreshPayment("payment_cc");
            m_dRoundingTotal = 0;
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.Credit.Text = dGetBanlance.ToString();
			this.Credit.SelectAll();
			this.Cashout.Text = "";
		}
		private void charge_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			if (m_dVipPayAmount != 0)
				return;
			RefreshPayment("payment_charge");
			if ((!m_bScanMember && MemberShipID.Text == "") || m_nCardId == 0)
			{
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = " Please keyin Vip ID!";
				fm.ShowDialog();
				MemberShipID.Focus();
				return;
			}
            m_dRoundingTotal = 0;
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.charge.Text = dGetBanlance.ToString();
			this.charge.SelectAll();
		}
		private void Cheque_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			RefreshPayment("payment_cheque");
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.Cheque.Text = dGetBanlance.ToString();
			this.Cheque.SelectAll();
			this.Cashout.Text = "";
		}
		private void RepositionPanels()
		{
			cart.Height = 359;
			panelHold.Location = new Point(400, 1);
			panelMenuPopup.Location = new Point(110, 250);
			panelMenuPopup.Width = 630;
			panelMenuPopup.Height = 330;
			if (!Program.m_bEnablePic)
			{
				panelMenuPopup.Location = new Point(150, 250);
				panelMenuPopup.Width = 616;
				panelMenuPopup.Height = 320;
			}
		}
		private bool BuildMenuButtons()
		{
			panelMenuTopRight.Controls.Clear();
			panelMenuLeft.Controls.Clear();
			panelMenuDepartment.Controls.Clear();
			int nRows = 0;
			if (dst.Tables["menu_buttons"] != null)
				dst.Tables["menu_buttons"].Clear();
			string sc = " SELECT id, name, name_en, is_indivisual FROM button ORDER BY id ";
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
			int nWidth = 78;
			int nHeight = 60;

			//build top right menu
			for (int i = 0; i < 6; i++)
			{
				string id = "";
				string name = "";
				string name_en = "";
				string is_indivisual = "";
				if (i < nRows)
				{
					DataRow dr = dst.Tables["menu_buttons"].Rows[i];
					id = dr["id"].ToString();
					name = dr["name"].ToString();
					name_en = dr["name_en"].ToString();
					is_indivisual = dr["is_indivisual"].ToString();
					m_isIndivisual[i] = is_indivisual;
					m_nameEn[i] = name_en;

				}
				if (Program.m_bLanguage_en)
					name = name_en;
				if (name == "")
					name = id;
/*				if (i == 0) //disabled first button, used for Phone Card
				{
					name_en = "Phone Card";
					name = name_en;
				}
*/ 
				Button newCatButton = new Button();
				newCatButton.BackColor = System.Drawing.Color.Transparent;
				newCatButton.BackgroundImage = QPOS2008.Properties.Resources.btn_brown;
				newCatButton.ForeColor = System.Drawing.Color.White;
				if (m_bTrial)
				{
					if (Convert.ToDouble(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", "")) < 0 || m_bTrialExpired == true)
					{
						newCatButton.Click += new System.EventHandler(this.btnPayByPoint_Click);
						this.pnlNumPad.Visible = false;
					}
					else
					{
						newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
						newCatButton.FlatStyle = FlatStyle.Popup;
						newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
						newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
						newCatButton.Width = nWidth;
						newCatButton.Height = nHeight;
						newCatButton.AutoSize = false;
						newCatButton.BackColor = System.Drawing.Color.Transparent;
						newCatButton.FlatAppearance.BorderSize = 0;
						newCatButton.Text = name;
						newCatButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
						newCatButton.Name = "Cat" + id;
						newCatButton.UseVisualStyleBackColor = true;
						newCatButton.Click += new EventHandler(newCatButton_Click);
						newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);
						panelMenuDepartment.Controls.Add(newCatButton);
					}
				}
				else
				{
					newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
					newCatButton.FlatStyle = FlatStyle.Popup;
					newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
					newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
					newCatButton.Width = nWidth;
					newCatButton.Height = nHeight;
					newCatButton.AutoSize = false;
					newCatButton.BackColor = System.Drawing.Color.Transparent;
					newCatButton.FlatAppearance.BorderSize = 0;
					newCatButton.Text = name;
					newCatButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
                    //if(i == 0)
                    //    newCatButton.Name = name;
                    //else
						newCatButton.Name = "Cat" + id;
					newCatButton.UseVisualStyleBackColor = true;
					newCatButton.Click += new EventHandler(newCatButton_Click);
					newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);
					panelMenuDepartment.Controls.Add(newCatButton);
				}
			}
			//build function key
			for (int i = 9; i < 18; i++)
			{
				string id = "";
				string name = "";
				string name_en = "";
				string is_indivisual = "";
				if (i < nRows)
				{
					DataRow dr = dst.Tables["menu_buttons"].Rows[i];
					id = dr["id"].ToString();
					name = dr["name"].ToString();
					name_en = dr["name_en"].ToString();
					is_indivisual = dr["is_indivisual"].ToString();
					m_isIndivisual[i] = is_indivisual;
					m_nameEn[i] = name_en;

				}
				if (Program.m_bLanguage_en)
					name = name_en;
				if (name == "")
					name = id;
				Button newCatButton = new Button();
				newCatButton.BackColor = System.Drawing.Color.Transparent;
				newCatButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
				newCatButton.ForeColor = System.Drawing.Color.White;

				if (name_en.ToLower() == "del")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.btn_red;
					newCatButton.ForeColor = System.Drawing.Color.White;
				}
				else if (name_en.ToLower() == "up" || name_en.ToLower() == "down" || name_en.ToLower() == "lang" || name_en.ToLower() == "cn/en" || name_en.ToLower() == "lang" || name_en.ToLower() == "item disc" || name_en.ToLower() == "box" || name_en.ToLower() == "receipt" || name_en.ToLower() == "search" || name_en.ToLower() == "weight" || name_en.ToLower() == "dump sale" || name_en.ToLower() == "grocery")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.btn_red;
				}
				if (name_en.ToLower() == "up")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.btn_red_up;
					newCatButton.Text = "";
				}
				if (name_en.ToLower() == "down")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.btn_red_down;
					newCatButton.Text = "";
				}
				if (m_bTrial)
				{
					if (Convert.ToDouble(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", "")) < 0 || m_bTrialExpired == true)
					{
						newCatButton.Click += new System.EventHandler(this.btnPayByPoint_Click);
						this.pnlNumPad.Visible = false;
					}
					else
					{
						newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
						//                        newCatButton.FlatStyle = FlatStyle.Flat;
						newCatButton.FlatStyle = FlatStyle.Popup;

						newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
						newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
						newCatButton.Width = nWidth;
						newCatButton.Height = nHeight;
						newCatButton.AutoSize = false;
						newCatButton.BackColor = System.Drawing.Color.Transparent;
						newCatButton.FlatAppearance.BorderSize = 0;
						newCatButton.Text = name;
						if (name_en.ToLower() == "up" || name_en.ToLower() == "down")
							newCatButton.Text = "";
						newCatButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
						newCatButton.Name = "Cat" + id;
						newCatButton.UseVisualStyleBackColor = true;
						newCatButton.Click += new EventHandler(newCatButton_Click);
						newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);
						panelMenuTopRight.Controls.Add(newCatButton);
					}
				}
				else
				{
					newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
					newCatButton.FlatStyle = FlatStyle.Popup;
					newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
					newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
					newCatButton.Width = nWidth;
					newCatButton.Height = nHeight;
					newCatButton.AutoSize = false;
					newCatButton.BackColor = System.Drawing.Color.Transparent;
					newCatButton.FlatAppearance.BorderSize = 0;
					newCatButton.Text = name;
					if (name_en.ToLower() == "up" || name_en.ToLower() == "down")
						newCatButton.Text = "";
					newCatButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
					newCatButton.Name = "Cat" + id;
					newCatButton.UseVisualStyleBackColor = true;
					newCatButton.Click += new EventHandler(newCatButton_Click);
					newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);
					panelMenuTopRight.Controls.Add(newCatButton);
				}
			}
			//build left menu
			for (int i = 18; i < 50; i++)
			{
				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					nHeight = 18;
				}
				else
				{
					nHeight = 60;
				}

				string id = "";
				string name = "";
				string name_en = "";
				string is_indivisual = "";
				if (i < nRows)
				{
					DataRow dr = dst.Tables["menu_buttons"].Rows[i];
					id = dr["id"].ToString();
					name = dr["name"].ToString();
					name_en = dr["name_en"].ToString();
					is_indivisual = dr["is_indivisual"].ToString();
					m_isIndivisual[i] = is_indivisual;
					m_nameEn[i] = name_en;
				}
				if (Program.m_bLanguage_en)
					name = name_en;
				if (name == "")
					name = id;

				Button newCatButton = new Button();
				newCatButton.Margin = new Padding(1);
				newCatButton.BackColor = System.Drawing.Color.Transparent;
				newCatButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
				if (name_en.ToLower() == "receipt")
				{
					newCatButton.BackgroundImage = QPOS2008.Properties.Resources.green;
				}
				if (m_bTrial)
				{
					if (Convert.ToDouble(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", "")) < 0 || m_bTrialExpired == true)
					{
						newCatButton.Click += new System.EventHandler(this.btnPayByPoint_Click);
						this.pnlNumPad.Visible = false;
					}
					else
					{
						newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
						newCatButton.FlatStyle = FlatStyle.Flat;
						newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
						newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
						newCatButton.Width = nWidth;
						newCatButton.Height = nHeight;
						newCatButton.AutoSize = false;
						newCatButton.BackColor = System.Drawing.Color.Transparent;
						newCatButton.FlatAppearance.BorderSize = 0;
						newCatButton.ForeColor = System.Drawing.Color.MidnightBlue;
						newCatButton.Text = name;
						newCatButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
						newCatButton.Name = "Cat" + id;
						newCatButton.UseVisualStyleBackColor = true;
						newCatButton.Click += new EventHandler(newCatButton_Click);
						newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);

						/*************************************************************/
						FlowLayoutPanel flp = new FlowLayoutPanel();
						flp.Height = 83;
						flp.Width = 76;

						PictureBox pb1 = new PictureBox();
						pb1.BorderStyle = BorderStyle.FixedSingle;
						pb1.BackColor = Color.White;
						pb1.Width = 76;
						pb1.Height = 68;
						pb1.SizeMode = PictureBoxSizeMode.CenterImage;
						pb1.Name = id;
						pb1.Click += new EventHandler(newpic_Click);

						if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
						{
							string Path = Program.m_sPicroot + "\\" + name_en + ".bmp";
							if (File.Exists(Path))
							{
								//								Image b = new Bitmap(Path);
								Image b;
								using (var bmpTemp = new Bitmap(Path))
								{
									b = new Bitmap(bmpTemp);
								}
								pb1.Image = b;
								//								pb1.Height = 50;
								//								pb1.Width = 78;
								pb1.Show();
								pb1.Image = b;
							}
							/*							else
														{
															//NewMenuButton.BackgroundImage = QPOS2008.Properties.Resources.btn_cat;
															Path = Program.m_sPicroot + "\\plates.bmp";
									//                      Image b = new Bitmap(Image.FromFile(Path), new Size(78, 50));
															Image b = new Bitmap(78, 50);
							//								if (File.Exists(Program.m_sPicroot + "\\plates.jpg"))
							//									b = new Bitmap(Image.FromFile(Path), new Size(78, 50));

															pb1.Image = b;
							//								pb1.Height = 50;
							//								pb1.Width = 78;
															pb1.Show();
															pb1.Image = b;
														}
							*/
							flp.Controls.Add(pb1);
							flp.Controls.Add(newCatButton);
							panelMenuLeft.Controls.Add(flp);
						}
						/*************************************************************/
						else
							panelMenuLeft.Controls.Add(newCatButton);
					}
				}
				else
				{
					newCatButton.BackgroundImageLayout = ImageLayout.Stretch;
					newCatButton.FlatStyle = FlatStyle.Flat;
					newCatButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
					newCatButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
					newCatButton.Width = nWidth;
					newCatButton.Height = nHeight;
					newCatButton.AutoSize = false;
					newCatButton.BackColor = System.Drawing.Color.Transparent;
					newCatButton.FlatAppearance.BorderSize = 0;
					newCatButton.ForeColor = System.Drawing.Color.MidnightBlue;
					newCatButton.Text = name;
					newCatButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
					newCatButton.Name = "Cat" + id;
					newCatButton.UseVisualStyleBackColor = true;
					newCatButton.Click += new EventHandler(newCatButton_Click);
					newCatButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);

					/*************************************************************/
					FlowLayoutPanel flp = new FlowLayoutPanel();
					flp.Margin = new Padding(2);
					flp.Height = 83;
					flp.Width = 76;

					PictureBox pb1 = new PictureBox();
					pb1.Margin = new Padding(0);
					pb1.BorderStyle = BorderStyle.FixedSingle;
					pb1.BackColor = Color.White;
					pb1.Width = 76;
					pb1.Height = 68;
					pb1.SizeMode = PictureBoxSizeMode.CenterImage;
					pb1.Name = id;
					pb1.Click += new EventHandler(newpic_Click);

					if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
					{
						string Path = Program.m_sPicroot + "\\" + name_en + ".bmp";
						if (File.Exists(Path))
						{
							Image b;
							using (var bmpTemp = new Bitmap(Path))
							{
								b = new Bitmap(bmpTemp);
							}
							pb1.Image = b;
							//							pb1.Height = 50;
							//							pb1.Width = 78;
							pb1.Show();
							pb1.Image = b;
						}
						/*						else
												{
													//NewMenuButton.BackgroundImage = QPOS2008.Properties.Resources.btn_cat;
													Path = Program.m_sPicroot + "\\plates.bmp";
							//                      Image b = new Bitmap(Image.FromFile(Path), new Size(78, 50));
													Image b = new Bitmap(78, 50);
													if (File.Exists(Program.m_sPicroot + "\\plates.bmp"))
														b = new Bitmap(Image.FromFile(Path), new Size(78, 50));
													else
														b = new Bitmap(78, 50);

													pb1.Image = b;
						//							pb1.Height = 50;
						//							pb1.Width = 78;
													pb1.Show();
													pb1.Image = b;
												}
						*/
						flp.Controls.Add(pb1);
						flp.Controls.Add(newCatButton);
						panelMenuLeft.Controls.Add(flp);
					}
					/*************************************************************/
					else
						panelMenuLeft.Controls.Add(newCatButton);
				}
			}
			return true;
		}
		private void newpic_Click(object sender, EventArgs e)
		{
			PictureBox CurrentButton = (PictureBox)sender;
			string btn_name = CurrentButton.Name;
			string button_id = btn_name;
			m_sLoadShortKey = GetSiteSettings("button_id_" + button_id);
			string btn_is_indivisual = m_isIndivisual[Program.MyIntParse(button_id) - 1];
			panelMenuPopup.Controls.Clear();
			int nRows = 0;
			if (dst.Tables["menu_buttons"] != null)
				dst.Tables["menu_buttons"].Clear();
			string sc = " SELECT i.*, c.barcode, c.code, c.supplier_code ";
			sc += ", (SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code AND item_qty = 1) AS mbarcode ";
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
			if (btn_is_indivisual == "True")
			{
				Control kc = this.barcode;
				if (m_lastFocused != null)
					kc = m_lastFocused;
				this.barcode.Text += m_sLoadShortKey;
				DoScanBarcode(true);
				return;
			}
			int nWidth = 74;
			int nHeight = 60;
			if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
			{
				nHeight = 20;
			}
			else
			{
				nHeight = 60;
			}
			//build top right menu
			for (int i = 0; i < 40; i++)
			{
				string id = "";
				string name = "";
				string barcode = "";
				string code = "";
				string name_en = "";
				string location = "";
				if (!Program.checkLocation())
				{
					if (i < nRows)
					{
						DataRow dr = dst.Tables["menu_buttons"].Rows[i];
						id = dr["id"].ToString();
						name = dr["name"].ToString();
						name_en = dr["name_en"].ToString();
						//						barcode = dr["supplier_code"].ToString();
						barcode = dr["barcode"].ToString();
						if (barcode == "")
							barcode = dr["mbarcode"].ToString();
						if (barcode == "")
							barcode = dr["supplier_code"].ToString();
						code = dr["code"].ToString();
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
							//							barcode = dr["supplier_code"].ToString();
							barcode = dr["barcode"].ToString();
							if (barcode == "")
								barcode = dr["mbarcode"].ToString();
							if (barcode == "")
								barcode = dr["supplier_code"].ToString();
							code = dr["code"].ToString();
						}
						else
						{

						}
					}
				}
				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					if (i == 23) //the last one
					{
						barcode = "";
						name = "关闭";
						name_en = "CLOSE";
					}
				}
				else
				{
					if (i == 39) //the last one
					{
						barcode = "";
						name = "关闭";
						name_en = "CLOSE";
					}
				}
				if (Program.m_bLanguage_en)
					name = name_en;
				//				if(name == "")
				//					name = id;
				Button newItemButton = new Button();
				newItemButton.Margin = new Padding(1);
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
				//if(name == "Close")
				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					if (i == 23)
						newItemButton.ForeColor = System.Drawing.Color.Red;
				}
				else
				{
					if (i == 39)
						newItemButton.ForeColor = System.Drawing.Color.Red;
				}
				newItemButton.Text = name;
				newItemButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
				newItemButton.Name = "Item" + code;//barcode;
				newItemButton.UseVisualStyleBackColor = true;
				newItemButton.Click += new EventHandler(newItemButton_Click);
				newItemButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);

				/*************************************************************/
				FlowLayoutPanel flp = new FlowLayoutPanel();
				flp.Height = 105;
				flp.Width = 72;

				PictureBox pb1 = new PictureBox();
				pb1.BorderStyle = BorderStyle.FixedSingle;
				pb1.BackColor = Color.White;
				pb1.Width = 70;
				pb1.Height = 72;
				pb1.Margin = new Padding(1);
				pb1.SizeMode = PictureBoxSizeMode.CenterImage;
				pb1.Name = code;//barcode;
				pb1.Click += new EventHandler(newItemPic_Click);

				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					string Path = Program.m_sPicroot + "\\" + code + ".bmp";
					if (File.Exists(Path))
					{
						Image b;
						using (var bmpTemp = new Bitmap(Path))
						{
							b = new Bitmap(bmpTemp);
						}
						pb1.Image = b;
						//						pb1.Height = 60;
						//						pb1.Width = 78;
						pb1.Show();
						pb1.Image = b;
					}
					/*					else
										{
											Path = Program.m_sPicroot + "\\plates.bmp";
											Image b = new Bitmap(78, 60);
											if (File.Exists(Program.m_sPicroot + "\\plates.bmp"))
												b = new Bitmap(Image.FromFile(Path), new Size(78, 60));
											else
												b = new Bitmap(78, 60);

											pb1.Image = b;
					//						pb1.Height = 60;
					//						pb1.Width = 78;
											pb1.Show();
											pb1.Image = b;
										}
					*/
					flp.Controls.Add(pb1);
					flp.Controls.Add(newItemButton);
					panelMenuPopup.Controls.Add(flp);
					if (i == 23)
					{
						flp.Controls.Clear();
						newItemButton.Height = 95;
						flp.Controls.Add(newItemButton);
						panelMenuPopup.Controls.Add(flp);
					}
				}
				/*************************************************************/
				else
					panelMenuPopup.Controls.Add(newItemButton);
			}
			panelMenuPopup.BringToFront();
			panelMenuPopup.Visible = true;
		}
		private void newCatButton_Click(object sender, EventArgs e)
		{
			if (panelCheckout.Visible)
				return;
			Button CurrentButton = (Button)sender;
			string btn_name = CurrentButton.Name;
			if(btn_name.ToLower() == "phone card")
			{
				FormPhoneCard fm = new FormPhoneCard();
				fm.ShowDialog();
				return;
			}
			if (btn_name.Length <= 3)
				return;
			string button_id = btn_name.Substring(3, btn_name.Length - 3);
			string btn_txt = CurrentButton.Text;
			string btn_name_en = "";
			string btn_is_indivisual = "";
			if (Program.m_bEnableSelfService && btn_name == "btnDeleteItemForSelfService")
			{
				btn_name_en = "DEL";
				btn_is_indivisual = "";
			}
			else
			{
				btn_name_en = m_nameEn[Program.MyIntParse(button_id) - 1];
				btn_is_indivisual = m_isIndivisual[Program.MyIntParse(button_id) - 1];
			}
			switch (btn_name_en.ToLower())
			{
				case "up":
					cart.Focus();
					int WM_KEYDOWN = 0x0100;
					int nKey = 38;
					Message msg = Message.Create(cart.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
					PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
					barcode.Focus();
					return;
				case "down":
					cart.Focus();
					int WM_KEYDOWN_D = 0x0100;
					int nKey_d = 40;
					Message msg_d = Message.Create(cart.Handle, WM_KEYDOWN_D, new IntPtr(nKey_d), new IntPtr(0));
					PostMessage(msg_d.HWnd, msg_d.Msg, msg_d.WParam, msg_d.LParam);
					barcode.Focus();
					return;
				case "del":
					if (panelChangeQty.Visible == true)
					{
						panelChangeQty.Visible = false;
						barcode.Focus();
						return;
					}
					if (panelChangeTare.Visible == true)
					{
						panelChangeTare.Visible = false;
						barcode.Focus();
						return;
					}
					//					else if(panelAdmin.Visible == true)
					//					{
					//						panelAdmin.Visible = false;
					//						barcode.Focus();
					//						return;
					//					}
					if (cart.Rows.Count == 0)
						return;
					FormConfirm delConfirm = new FormConfirm();
					delConfirm.m_sMSG = " Are you sure you want to delete ?";
					delConfirm.ShowDialog();
					if (delConfirm.m_sConfirm == "0")
					{
						barcode.Focus();
						return;
					}
					int iSelectedRowed = cart.CurrentRow.Index;
					if (!cart.CurrentRow.Selected)
						iSelectedRowed = 0;
					//DeleteCartItem(iSelectedRowed);
					DeleteCartItem_old(iSelectedRowed);
					CheckGroupPromotion();
					CheckComboPromotion();
					barcode.Focus();
					return;
				/*
			case "b":
				Control nc = this.barcode;
				if (m_lastFocused != null)
					nc = m_lastFocused;
				this.barcode.Text += "B";
				DoScanBarcode();
				return;
				 * */
				case "cn/en":
					ENCNSwitch();
					return;
				case "lang":
					ENCNSwitch();
					return;
				case "item disc":
					//					if(sPasswordControl =="1")
					if (Program.m_bEnableDisControl)
					{
						if (!AdminOK("discount"))
							return;
						discountswtich.Text = "1";
						m_sAdminAction = "item_discount";
						DoAdminAction();
					}
					else
					{
						this.discountswtich.Text = "1";
						if (cart.Rows.Count == 0)
							return;
						int cIndex = cart.CurrentRow.Index;
						if (!cart.CurrentRow.Selected)
							cIndex = 0;
						ShowDiscountPanel(cart.Rows[cIndex].Cells["cc_supplier_code"].Value.ToString(), cart.Rows[cIndex].Cells["cc_name"].Value.ToString(), cart.Rows[cIndex].Cells["cc_price"].Value.ToString());
					}
					return;
				case "search":
					DoSearchItem();
					return;
				case "receipt":
					if (m_sLastReceipt != "")
					{
						sPrintLastReceipt = m_sLastReceipt;
						if (m_sCurrentCartID == 0)
							sPrintLastReceipt = m_sLastCart1;
						else if (m_sCurrentCartID == 1)
							sPrintLastReceipt = m_sLastCart2;
						else if (m_sCurrentCartID == 2)
							sPrintLastReceipt = m_sLastCart3;
						else if (m_sCurrentCartID == 3)
							sPrintLastReceipt = m_sLastCart4;
						m_sPrintBuffer = sPrintLastReceipt; //"[b]Cash Draw Opened:[/b] Station:" + Station.Text + " Date/Time:" + DateTime.Now.ToString("dd-MM-yyyy") + DateTime.Now.ToString("HH:mm");
						printDoc.Print();
					}
					if (Program.m_eftposType == "verifone")
					{
						using (VaultSession vs = new VaultSession("CHECKOUT1"))
						{
							vs.ReprintReceipt();
						}
					}
					this.barcode.Focus();
					this.barcode.Text = "";
					return;
				default:
					break;
			}
			m_sLoadShortKey = GetSiteSettings("button_id_" + button_id);
			if (m_sLoadShortKey != "")
			{
				//				if (button_id == "1" || button_id == "2" || button_id == "3" || button_id == "4" || button_id == "5" || button_id == "6" || button_id == "11" || button_id == "12" )
				if (button_id == "11" || button_id == "12")
				{
					if (this.barcode.Text == "")
					{
						FormMSG errMsg = new FormMSG();
						errMsg.btnNo.Visible = false;
						errMsg.btnYes.Visible = false;
						errMsg.m_sMsg = " Sorry , Key in price first! ";
						errMsg.ShowDialog();
						this.price.Text = "";
						this.barcode.Focus();
						this.barcode.Text = "";
						this.price.Text = "";
						this.qty.Text = "";
						return;
					}
					else
					{
						Control kc = this.barcode;
						if (m_lastFocused != null)
							kc = m_lastFocused;
                        if (Program.MyDoubleParse(kc.Text) < 0)
                        {
                            FormMSG errMsg = new FormMSG();
                            errMsg.btnNo.Visible = false;
                            errMsg.btnYes.Visible = false;
                            errMsg.m_sMsg = " Sorry , the dump sale cannot enter negative numbers! ";
                            errMsg.ShowDialog();
                            this.price.Text = "";
                            this.barcode.Focus();
                            this.barcode.Text = "";
                            this.price.Text = "";
                            this.qty.Text = "";
                            return;
                        }  
						this.barcode.Text += m_sLoadShortKey;
						DoScanBarcode(false);
						this.price.Text = "";
						this.qty.Text = "";
						return;
					}
				}
				else if (Program.MyBooleanParse(btn_is_indivisual))
				{

					if (this.barcode.Text == "" && m_sLoadShortKey.IndexOf("*") >= 0)
					{
						FormMSG errMsg = new FormMSG();
						errMsg.btnNo.Visible = false;
						errMsg.btnYes.Visible = false;
						errMsg.m_sMsg = " Sorry , Key in price first! ";
						errMsg.ShowDialog();
						this.price.Text = "";
						this.barcode.Focus();
						this.barcode.Text = "";
						this.price.Text = "";
						this.qty.Text = "";
						return;
					}
					else
					{
						Control kc = this.barcode;
						if (m_lastFocused != null)
							kc = m_lastFocused;
						this.barcode.Text += m_sLoadShortKey;
						DoScanBarcode(true);
						this.price.Text = "";
						this.qty.Text = "";
						return;
					}
				}
			}
			panelMenuPopup.Controls.Clear();
			int nRows = 0;
			if (dst.Tables["menu_buttons"] != null)
				dst.Tables["menu_buttons"].Clear();
			//string sc = " SELECT i.*, c.barcode, c.supplier_code ";
			string sc = " SELECT i.*, (SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code AND item_qty = 1) as barcode, c.supplier_code ";
			sc += ", (SELECT TOP 1 barcode FROM barcode WHERE item_code = c.code AND item_qty = 1) AS mbarcode ";
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
			int nWidth = 74;
			int nHeight = 60;
			if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
			{
				nHeight = 20;
			}
			else
			{
				nHeight = 60;
			}
			//build top right menu
			for (int i = 0; i < 40; i++)
			{
				string id = "";
				string name = "";
				string code = "";
				string barcode = "";
				string name_en = "";
				string location = "";
				if (!Program.checkLocation())
				{
					if (i < nRows)
					{
						DataRow dr = dst.Tables["menu_buttons"].Rows[i];
						id = dr["id"].ToString();
						name = dr["name"].ToString();
						name_en = dr["name_en"].ToString();
						code = dr["code"].ToString();
						barcode = dr["barcode"].ToString();
						if (barcode == "")
							barcode = dr["mbarcode"].ToString();
						if (barcode == "")
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
							code = dr["code"].ToString();
							barcode = dr["barcode"].ToString();
							if (barcode == "")
								barcode = dr["mbarcode"].ToString();
							if (barcode == "")
								barcode = dr["supplier_code"].ToString();
						}
						else
						{

						}
					}
				}
				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					if (i == 23) //the last one
					{
						barcode = "";
						name = "关闭";
						name_en = "CLOSE";
					}
				}
				else
				{
					if (i == 39) //the last one
					{
						barcode = "";
						name = "关闭";
						name_en = "CLOSE";
					}
				}
				if (Program.m_bLanguage_en)
					name = name_en;
				//				if(name == "")
				//					name = id;
				Button newItemButton = new Button();
				newItemButton.Margin = new Padding(1);
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
					if (i == 23)
						newItemButton.ForeColor = System.Drawing.Color.Red;
				}
				else
				{
					if (i == 39)
						newItemButton.ForeColor = System.Drawing.Color.Red;
				}
				newItemButton.Text = name;
				newItemButton.Font = new Font("Arial", m_nMenuFontSize, FontStyle.Bold);
				newItemButton.Name = "Item" + code; //barcode;
				newItemButton.UseVisualStyleBackColor = true;
				newItemButton.Click += new EventHandler(newItemButton_Click);
				newItemButton.KeyDown += new System.Windows.Forms.KeyEventHandler(OnFormKeyDown);

				/*************************************************************/
				FlowLayoutPanel flp = new FlowLayoutPanel();
				flp.Height = 105;
				flp.Width = 72;

				PictureBox pb1 = new PictureBox();
				pb1.BorderStyle = BorderStyle.FixedSingle;
				pb1.BackColor = Color.White;
				pb1.Width = 70;
				pb1.Height = 72;
				pb1.Margin = new Padding(1);
				pb1.SizeMode = PictureBoxSizeMode.CenterImage;
				pb1.Name = barcode;
				pb1.Click += new EventHandler(newItemPic_Click);

				if (Program.m_bEnablePic && Directory.Exists(Program.m_sPicroot))
				{
					string Path = Program.m_sPicroot + "\\" + code + ".bmp";
					if (File.Exists(Path))
					{
						Image b;
						using (var bmpTemp = new Bitmap(Path))
						{
							b = new Bitmap(bmpTemp);
						}
						pb1.Image = b;
						//						pb1.Height = 60;
						//						pb1.Width = 78;
						pb1.Show();
						pb1.Image = b;
					}
					/*					else
										{
											//NewMenuButton.BackgroundImage = QPOS2008.Properties.Resources.btn_cat;
											Path = Program.m_sPicroot + "\\plates.bmp";
					//                      Image b = new Bitmap(Image.FromFile(Path), new Size(78, 50));
											Image b = new Bitmap(78, 50);
											if (File.Exists(Program.m_sPicroot + "\\plates.bmp"))
												b = new Bitmap(Image.FromFile(Path), new Size(78, 50));
											else
												b = new Bitmap(78, 50);

											pb1.Image = b;
					//						pb1.Height = 60;
					//						pb1.Width = 78;
											pb1.Show();
											pb1.Image = b;
										}
					*/
					flp.Controls.Add(pb1);
					flp.Controls.Add(newItemButton);
					panelMenuPopup.Controls.Add(flp);
					if (i == 23)
					{
						flp.Controls.Clear();
						newItemButton.Height = 95;
						flp.Controls.Add(newItemButton);
						panelMenuPopup.Controls.Add(flp);
					}
				}
				/*************************************************************/
				else
					panelMenuPopup.Controls.Add(newItemButton);
			}
			panelMenuPopup.Visible = true;
		}

		private void newItemPic_Click(object sender, EventArgs e)
		{
			PictureBox CurrentButton = (PictureBox)sender;
			string name = CurrentButton.Name;
			//            string text = CurrentButton.Text;
			string text = CurrentButton.Name;
			if (text.Trim() != "" && (text != "关闭" && text.Trim().ToLower() != "close"))
			{
				string sbarcode = name;
				if (sbarcode != "0")
				{
					this.barcode.Text = sbarcode;
					DoScanBarcode(true);
				}
				else if (text == "关闭" || text.ToLower() == "close")
				{
					panelMenuPopup.Visible = false;
				}
				if (Program.m_bAutoClosePopupMenu)
					panelMenuPopup.Visible = false;
			}
			else
			{
				if (Program.m_bAutoClosePopupMenu)
					panelMenuPopup.Visible = false;
			}
		}
        private string getbarcode(string code)
        {
            return "";
        }

		private void newItemButton_Click(object sender, EventArgs e)
		{
			Button CurrentButton = (Button)sender;
			string name = CurrentButton.Name;
			string text = CurrentButton.Text;
			if (text.Trim() != "")
			{
				if (name.Length <= 4)
				{
					panelMenuPopup.Visible = false;
					barcode.Focus();
					return;
				}
				string sbarcode = name.Substring(4, name.Length - 4);
				if (sbarcode != "0" && (text !="关闭" && text.ToLower() != "close"))
				{
					this.barcode.Text = sbarcode;
					DoScanBarcode(true);
				}
				else if (text =="关闭" || text.ToLower() == "close")
				{
					panelMenuPopup.Visible = false;
				}
				if (Program.m_bAutoClosePopupMenu)
					panelMenuPopup.Visible = false;
			}

		}
		private void btnHold_Click(object sender, EventArgs e)
		{
			panelHold.Visible = true;
			lbl_showhold.Visible = false;
			ShowHoldedTables();
		}
		private void onControlLeave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}

		private void btnKeyBord_Click(object sender, EventArgs e)
		{
			Button CurrentButton = (Button)sender;
			string name = CurrentButton.Name;
			string value = CurrentButton.Text;
			Control nc;
			nc = this.barcode;
			if (m_lastFocused != null)
				nc = m_lastFocused;
			int WM_KEYDOWN = 0x0100;
			int nKey = 0x30;

			if (name == "btnBack" || name == "btnback" || name == "btnnumdel")
			{
				nKey = 0x08;
			}
			else if (name == "btnEnter" || name == "btn_penter" || name == "btnnument")
			{
				nKey = 17;
			}
			else if (name == "btnDot" || name == "btnDoc" || name == "btnnumdoc")
				nKey = 110;
			else if (name == "btnline")
				nKey = 191;
			else if (name == "btnkeyspace")
			{
				nKey = 32;
			}
			else if (name == "btnkeystar")
			{
				nKey = 106;
			}
			else if (name == "btndash")
			{
				nKey = 189;
			}
			else if (name == "btnkeyclear")
			{
				return;
			}
			else if (name == "btnkeyhash")
			{
				if (m_lastFocused.Text == "")
					m_lastFocused.Text = "#";
				else
					m_lastFocused.Text = m_lastFocused.Text + "#";
				return;
			}
			else
			{
				try
				{
					int.Parse(value);
					for (int k = 0; k < value.Length; k++)
					{
						string keyvalue = value.Substring(k, 1);
						nKey = Program.MyIntParse(keyvalue) + 48;
						Message msg = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
						PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
					}
					try
					{
					}
					catch
					{
					}
					return;
				}
				catch
				{
					char[] c = new char[0];
					c = value.ToCharArray();
					int keyvalue = (int)c[0];
					value = keyvalue.ToString();
					nKey = Program.MyIntParse(value);
				}

			}
			Message msg1 = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
			PostMessage(msg1.HWnd, msg1.Msg, msg1.WParam, msg1.LParam);
		}
		private void btnPinpad_Click(object sender, EventArgs e)
		{
			btn_charge.Enabled = true;
			charge.Enabled = true;
			Button CurrentButton = (Button)sender;
			string name = CurrentButton.Name;
			string value = CurrentButton.Text;
//Program.g_log.Log("btnPinpad_Click, name=" + name + ", value=" + value);						
			if (name == "btnPay")
			{
				if (cart.Rows.Count <= 0)
				{
					FormMSG fm = new FormMSG();
					fm.m_sYesNo = "1";
					fm.m_sMsg = "VIP Payment?";
					fm.ShowDialog();
					if (fm.m_bYes)
					{
						m_sVipAmounts = "";
						m_sVipInvoices = "";
						m_sVipPaymentTypes = "";
						m_dVipPayAmount = 0;
						FormVIP AP = new FormVIP();
						AP.ShowDialog();

						if (AP.m_bPay)
						{
							m_sVipAmounts = AP.m_sAmounts;
							m_sVipInvoices = AP.m_sInvoices;
							m_dVipPayAmount = AP.m_dPayAmount;
							MemberShipID.Text = AP.m_sVipCardId;
							m_sCardName = AP.m_sVipName;
							DoVipPayment();
							return;
						}
					}
					return;
				}
				if (Program.m_bEnableSalesLogin)
				{
					adminLogin fms = new adminLogin();
					fms.m_bChangeSales = true;
                    fms.m_sText = "Enter Passcode";
					fms.ShowDialog();
					if (!fms.m_bPass)
						return;

					m_nSalesId = Program.MyIntParse(fms.m_sSalesId);
					lblSales.Text = fms.m_sSalesName;
					SalesName.Text = fms.m_sSalesName;
				}

				if (Program.m_bEnableSelfService)
				{
					panelCheckout.Location = new Point(670, 5);
					panelCheckout.Size = new Size(350, 736);
				}
				else
				{
					panelCheckout.Location = new Point(245, 50);
					panelCheckout.Size = new Size(770, 736);
				}
				panelCheckout.Visible = true;
				panelCheckout.BringToFront();
				txtShowCashOut.Visible = false;
				txtcashouttitle.Visible = false;
				if (Program.m_bEnableSelfService)
				{
					SelfServiceModelCheckout();
					Eftpos.Focus();
				}
				else
					Cash.Focus();
				panelHold.Enabled = false;

                m_fmad.lblSurCharge.Text = "";
                m_fmads.lblSurCharge.Text = "";
				//	Cash.Text = Program.RoundCents(Program.MyMoneyParse(this.labelBalance.Text), m_sRoundingNum).ToString();
			}
			else if (name == "btnQty")
			{
                if (Program.m_bEnableQtyPassword)
                {
                    if (!AdminOK("discount"))
                        return;
                }
				if (cart.Rows.Count <= 0)
					return;
				txtChangeQty.Text = "";
				panelChangeQty.Visible = true;
				txtChangeQty.Focus();
			}
			else
			{
				Control nc = this.barcode;
				if (m_lastFocused != null)
					nc = m_lastFocused;
				int WM_KEYDOWN = 0x0100;
				int nKey = 0x30;

				if (name == "btnBack" || name == "btn_pback")
				{
					double dBalance = Math.Round(m_dTotal - m_dTotalPaid + m_dRoundingTotal, 2);
					if (dBalance == 0 && m_nOrderId != 0)
					{
						FormMSG fm = new FormMSG();
						fm.m_sMsg = "Already paid in full, cannot go back";
						fm.btnNo.Visible = false;
						fm.btnYes.Visible = false;
						fm.ShowDialog();
						return;
					}
					btnClose.Enabled = true;
					nKey = 0x08;

				}
				else if (name == "btnEnter" || name == "btn_penter")
				{
					m_dCashout = Program.MyMoneyParse(Cashout.Text);
					m_dCash = Program.MyMoneyParse(Cash.Text);
					m_dCheque = Program.MyMoneyParse(Cheque.Text);
					m_dCredit = Program.MyMoneyParse(Credit.Text);
					if (m_dCashout != 0 && (m_dCheque != 0 || m_dCredit != 0 || m_dCash != 0))
					{
						FormMSG fmsg = new FormMSG();
						fmsg.btnNo.Visible = false;
						fmsg.btnYes.Visible = false;
						fmsg.m_sMsg = "       Sorry, No CashOut In \r\n      Cash, Credit Card or Cheque Payment";
						fmsg.m_sbuttonFocus = "0";
						fmsg.ShowDialog();
						this.Cashout.Text = "";
						m_dCashout = 0;
						//						this.Credit.Select();
						//						this.Credit.Focus();
						return;
					}
					nKey = 17;
				}
				else if (name == "btnDot" || name == "btn_pdoc")
					nKey = 110;
				else if (name == "btn_100" || name == "btn_10")
				{
					nKey = 49;
					onSelectAll();
				}
				else if (name == "btn_200" || name == "btn_20")
				{
					nKey = 50;
					onSelectAll();
				}
				else if (name == "btn_50" || name == "btn_5")
				{
					nKey = 53;
					onSelectAll();
				}
				else
					nKey += Program.MyIntParse(value);
				if (nc.Handle == Cashout.Handle && name == "btn_penter")
					nKey = 30;
				Message msg = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
				PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
				if (name == "btn00" || name == "btn_10" || name == "btn_20" || name == "btn_50" || name == "btn_p00")
				{
					nKey = 48;
					Message msg2 = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
					PostMessage(msg2.HWnd, msg2.Msg, msg2.WParam, msg2.LParam);
				}
				if (name == "btn_100" || name == "btn_200")
				{
					nKey = 48;
					Message msg1 = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
					for (int i = 0; i < 2; i++)
					{
						PostMessage(msg1.HWnd, msg1.Msg, msg1.WParam, msg1.LParam);
					}
				}
			}
		}
		private void doBuideBrands()
		{
			this.BrandComboBox.Items.Clear();
			int rows = 0;
			if (dst.Tables["brand_selection"] != null)
				dst.Tables["brand_selection"].Clear();
			string sc = " SELECT  cat, s_cat FROM catalog ";
			sc += " WHERE LOWER(cat) = 'brands'";
			sc += " GROUP BY s_cat, cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "brand_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			this.BrandComboBox.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["brand_selection"].Rows[i];
				string cat = dr["s_cat"].ToString();
				if (cat.ToLower() == "zzzothers")
					continue;
				this.BrandComboBox.Items.Add(cat);
			}
		}
		private void doBuideCat()
		{
			this.catselectionCB.Items.Clear();
			int rows = 0;
			if (dst.Tables["cat_selection"] != null)
				dst.Tables["cat_selection"].Clear();
			string sc = " SELECT  cat FROM catalog ";
			sc += " WHERE 1=1 AND LOWER(cat) <> 'brands'";
			sc += " GROUP BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "cat_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			this.catselectionCB.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["cat_selection"].Rows[i];
				string cat = dr["cat"].ToString();
				this.catselectionCB.Items.Add(cat);
			}
		}

		private void doBuideSCat()
		{
			this.scatselectionCB.Items.Clear();
			int rows = 0;
			if (dst.Tables["scat_selection"] != null)
				dst.Tables["scat_selection"].Clear();
			string sc = " SELECT  s_cat FROM catalog ";
			sc += " WHERE 1=1 AND LOWER(cat) <> 'brands'";
			sc += " AND cat = '" + Program.EncodeQuote(catselectionCB.Text) + "'";
			sc += " GROUP BY s_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "scat_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			this.scatselectionCB.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["scat_selection"].Rows[i];
				string scat = dr["s_cat"].ToString();
				this.scatselectionCB.Items.Add(scat);
			}
		}

		private void doBuideSSCat()
		{
			this.sscatselectionCB.Items.Clear();
			int rows = 0;
			if (dst.Tables["sscat_selection"] != null)
				dst.Tables["sscat_selection"].Clear();
			string sc = " SELECT  ss_cat FROM catalog ";
			sc += " WHERE 1=1 AND LOWER(cat) <> 'brands'";
			sc += " AND cat = '" + Program.EncodeQuote(catselectionCB.Text) + "'";
			sc += " AND s_cat = '" + Program.EncodeQuote(scatselectionCB.Text) + "'";
			sc += " GROUP BY ss_cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "sscat_selection");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			this.catselectionCB.Items.Add("");
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["sscat_selection"].Rows[i];
				string sscat = dr["ss_cat"].ToString();
				this.sscatselectionCB.Items.Add(sscat);
			}
		}

		private void doBuideTax()
		{
			this.cbTaxCode.Items.Clear();
			int rows = 0;
			if (dst.Tables["doBuideTax"] != null)
				dst.Tables["doBuideTax"].Clear();
			string sc = " SELECT  * FROM tax_code ";
			sc += " WHERE 1=1 ";
			//			sc += " GROUP BY cat ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "doBuideTax");
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
				DataRow dr = dst.Tables["doBuideTax"].Rows[i];
				string tax_code = dr["tax_code"].ToString();
				string tax_rate = dr["tax_rate"].ToString();
				this.cbTaxCode.Items.Add(tax_code);
			}
			this.cbTaxCode.Text = dst.Tables["doBuideTax"].Rows[0]["tax_code"].ToString();
		}
		private void GstRatebygstcode()
		{
			if (dst.Tables["GstRatebygstcode"] != null)
				dst.Tables["GstRatebygstcode"].Clear();
			string sc = "";
			int rows = 0;
			sc += " SELECT TOP 1* FROM tax_code WHERE 1=1 ";
			sc += " AND tax_code = N'" + this.cbTaxCode.Text + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "GstRatebygstcode");
				if (rows <= 0)
					return;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			string tax_rate = dst.Tables["GstRatebygstcode"].Rows[0]["tax_rate"].ToString();
			this.txtTaxRate.Text = (double.Parse(tax_rate) * 100).ToString();
		}

		private void onSelectAll()
		{
			if (this.Cash.Text != "")
				this.Cash.SelectAll();
			else if (this.Eftpos.Text != "")
				this.Eftpos.SelectAll();
			else if (this.Credit.Text != "")
				this.Credit.SelectAll();
			else if (this.Cheque.Text != "")
				this.Cheque.SelectAll();
			else if (this.charge.Text != "")
				this.charge.SelectAll();
		}
		private void cart_CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			MyDataGridView dgv = (MyDataGridView)sender;
			m_lastFocused = (Control)dgv;
		}
		private void btnQtyCancel_Click(object sender, EventArgs e)
		{
			panelChangeQty.Visible = false;
		}
		private void btnQtyOK_Click(object sender, EventArgs e)
		{
			double dQty = Program.MyDoubleParse(txtChangeQty.Text);
			if (dQty > 0)
				doChangeQty(dQty, m_irow);
			m_irow = "";
			panelChangeQty.Visible = false;
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			double dBalance = Math.Round(m_dTotal - m_dTotalPaid + m_dRoundingTotal, 2);
			if (dBalance == 0 && m_nOrderId != 0)
			{
				return;
			}
			txtpaymentinfo.Text = "";
			ResetPayment();
			btnClose.Enabled = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
		}
		private void txtChangeQty_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13 || e.KeyValue == 17)
			{
				btnQtyOK_Click(sender, e);
			}
		}
		private bool AdminOK(string cmd)
		{
			adminLogin fm = new adminLogin();
            fm.m_sText = "Enter Passcode";
			fm.cmd = cmd;
			fm.ShowDialog();
			if (fm.m_bPass)
				return true;
			return false;
		}
		private void DoAdminAction()
		{
			if (m_sAdminAction == "open_draw")
			{
				if (!AdminOK("open cashdraw"))
					return;
				KickCashdraw();
				m_sPrintBuffer = "[b]Cash Draw Opened:[/b] Station:" + Station.Text + " Date/Time:" + DateTime.Now.ToString("dd-MM-yyyy") + DateTime.Now.ToString("HH:mm");
				printDoc.Print();
				Program.RecordTillData("draw_opens_no_sales", "1");
				this.barcode.Focus();
				this.barcode.Text = "";
				return;
			}
			else if (m_sAdminAction == "delete_last_item")
			{
				if (!AdminOK("delete item"))
					return;
				DeleteCartItem_old(0);
				this.barcode.Text = "";
			}
			else if (m_sAdminAction == "delete_item")
			{
				if (!AdminOK("delete item"))
					return;
				DeleteCartItem_old(-1);
				ResetPayment();
				barcode.Focus();
			}
			else if (m_sAdminAction == "delete_item_by_discount")
			{
				if (this.cart.Rows.Count.ToString() != "0")
				{
					if (this.discountswtich.Text == "1")
						//						DeleteCartItem(-1);
						DeleteCartItem_old(-1);
					else if (this.discountswtich.Text == "2")
						//						DeleteCartItem(0);
						DeleteCartItem_old(0);
				}
			}
			else if (m_sAdminAction == "refund")
			{
				//				if (!AdminOK("refund"))
				//					return;
				DoRefund();
				bIs_Refund = true;
				barcode.Focus();
			}
			else if (m_sAdminAction == "clear_payment")
			{
				ResetPayment();
				Cash.Focus();
			}
			else if (m_sAdminAction == "reprint_total")
			{
				MessageBox.Show("Reprint TOTAL");
				m_sPrintBuffer = Program.m_bReprintTotal;//Program.rePrintTotal(int.Parse(Program.m_sStationID), m_nBranchId);
				printDoc.Print();
			}
			else if (m_sAdminAction == "delete_current_order")
			{
				if (CheckLatipayGiftCardItemInCart(0, true))
				{
					MessageBox.Show("Gift card cannot be deleted, please go to the gift card interface to cancel the relevant gift card.", "prompt");
					return;
				}
				Program.RecordTillData("total_cancel_sales", m_dTotal.ToString());
				Program.RecordVoidLog(m_dTotal.ToString(), SalesName.Text);
				Program.RecordTillData("total_void", "1");
				ResetPaymentLabels();
				ResetCart();
				UpdateDtCart(m_nCurrentCart);
			}
			else if (m_sAdminAction == "print_x_total")
			{
				//				if (!AdminOK("xtotal"))
				//					return;
				string z = Program.PrintXTotal(Program.m_sStationID, "X", m_nBranchId);
				m_sPrintBuffer = z;
				printDoc.Print();
				//				if (MessageBox.Show("Please Log Off, Have A NICE DAY,BYE!.", "Confirm Printing OK", MessageBoxButtons.YesNo) != DialogResult.Yes)
				//					MessageBox.Show("Please Sign off! Have a nice day. Bye");
			}
			else if (m_sAdminAction == "print_z_total")
			{
				//				if (!AdminOK("xtotal"))
				//					return;
				string r = Program.PrintXTotal(Program.m_sStationID, "Z", m_nBranchId);
				m_sPrintBuffer = r;
				printDoc.Print();
				KickCashdraw();
				KickCashdraw();

				if (MessageBox.Show("Does the report print out successfully? click Yes to logoff!", "Confirm Printing", MessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					return;
				}
                Program.ResetTillDataAfterZTotal(Program.m_sStationID);
				this.BringToFront();

				timer1.Enabled = true;
				timer1.Interval = 1510000;
				//Thread.Sleep(1000); 
				if (Program.m_backupdata == true)
					DoBackup();
				DoSignOff();
			}
			else if (m_sAdminAction == "total_discount")
			{
				//				if (!AdminOK("discount"))
				//					return;
				ResetPayment();
				this.discountswtich.Text = "2";
				ShowDiscountPanel("", "", "");
				textBoxDisc.Focus();
				if (!sAllowDiscountRollBack)
					m_bBeDisced = true;
			}
			else if (m_sAdminAction == "item_discount")
			{
				//				if (!AdminOK("discount"))
				//					return;
				this.discountswtich.Text = "1";
				textBoxDisc.Focus();
				if (cart.Rows.Count == 0)
					return;
				int cIndex = cart.CurrentRow.Index;
				if (!cart.CurrentRow.Selected)
					cIndex = 0;
				ShowDiscountPanel(cart.Rows[cIndex].Cells["cc_supplier_code"].Value.ToString(), cart.Rows[cIndex].Cells["cc_name"].Value.ToString(), cart.Rows[cIndex].Cells["cc_price"].Value.ToString());
			}
			else if (m_sAdminAction == "setting")
			{
				if (!AdminOK("setting"))
					return;
				FormConfig fc = new FormConfig();
				if (!Program.m_formconfigshow)
				{
					Program.m_formconfigshow = true;
					fc.Show();
					this.SendToBack();
					fc.BringToFront();
					//fc.BringToFront();
					//this.SendToBack();
				}
				else
				{
					//Program.m_formconfigshow = false;
					//fc.Hide();
					//fc.Close();
				}

			}
			else if (m_sAdminAction != "total_discount" && m_sAdminAction != "item_discount" && m_sAdminAction != "setting")
			{
				barcode.Focus();
			}

		}
		private void btn_cash_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			RefreshPayment("payment_cash");
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = Program.RoundCents(dGetBanlance, m_sRoundingNum).ToString("N2");
			this.Cash.Text = this.labelBalance.Text;
			this.Cashout.Text = "";
			this.Cash.SelectAll();
			this.Cash.Focus();
		}
		private void btn_eftpos_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			RefreshPayment("payment_eftpos");
            m_dRoundingTotal = 0;
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.Eftpos.Text = dGetBanlance.ToString("N2");
			this.Eftpos.SelectAll();
			this.Eftpos.Focus();
		}
		private void btn_cashout_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			//			if (this.Cash.Text != "" || m_dCashTotal != 0)
			if (this.Cash.Text != "" || m_dCash != 0)
			{
				FormMSG errMsg = new FormMSG();
				errMsg.btnNo.Visible = false;
				errMsg.btnYes.Visible = false;
				errMsg.m_sMsg = " Sorry , No Cash Out Allow \r\n Because Cash In Detected ";
				errMsg.ShowDialog();
				return;
			}
			Cashout.Focus();
		}
		private void btn_cc_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";

			RefreshPayment("payment_cc");
            m_dRoundingTotal = 0;
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.Credit.Text = dGetBanlance.ToString("N2");
			this.Credit.SelectAll();
			this.Cashout.Text = "";
			this.Credit.Focus();
		}
		private void btn_cheque_Click(object sender, EventArgs e)
		{
			m_fmad.lblSurCharge.Text = "";
			m_fmads.lblSurCharge.Text = "";
			RefreshPayment("payment_cheque");
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.Cheque.Text = dGetBanlance.ToString("N2");
			this.Cheque.SelectAll();
			this.Cashout.Text = "";
			this.Cheque.Focus();

			//bool bSuccess = false;
			//if (Program.m_bEnableLatiPay)
			//{
			//    if (Program.m_sLatiPayApiKey == "" || Program.m_sLatiPayWalletID == "" || Program.m_sLatiPayUserID == "")
			//    {
			//        Program.MsgBox("Latipay is not set up, please contact your dealer.");
			//        return;
			//    }
			//    else
			//    {
			//        FormLatipay flp = new FormLatipay();
			//        flp.m_sInvoiceNumber = GetNextInvNumber();
			//        flp.m_dAmount = dGetBanlance;
			//        flp.m_bSuccess = false;
			//        flp.ShowDialog();
			//        bSuccess = flp.m_bSuccess;
			//    }
			//}
			//else if (Program.m_bEnableAttractPay || Program.m_bEnableWechatPayment)
			//{
			//    FormOtherPayment fm = new FormOtherPayment();
			//    fm.m_sInvoiceNumber = GetNextInvNumber();
			//    fm.m_dAmount = dGetBanlance;
			//    fm.m_bSuccess = false;
			//    fm.ShowDialog();
			//    bSuccess = fm.m_bSuccess;
			//}
			//else
			//{
			//    FormMSG fm = new FormMSG();
			//    fm.btnYes.Visible = true;
			//    fm.btnNo.Visible = true;
			//    fm.btnOK.Visible = false;
			//    fm.m_sMsg = "DIGITAL PAYMENT ACCEPTED?";
			//    fm.ShowDialog();
			//    if (!fm.m_bYes)
			//        return;
			//    else
			//        bSuccess = true;
			//}
			//if (bSuccess)
			//{
			//    m_dCheque = dGetBanlance;
			//    if (TotalPaymentOK())
			//    {
			//        doShowPaymentInfo();
			//        DoCheckout();
			//        Cash.Text = "";
			//        labelBalance.Text = "";
			//        return;
			//    }
			//}
			//Cheque.Text = "";
			//Cheque.Focus();
		
/*			if(Program.m_bEnableWechatPayment)
			{
				m_fmad.lblSurCharge.Text = "";
				m_fmads.lblSurCharge.Text = "";
				RefreshPayment("payment_cheque");
				double dGetBanlance = Program.MyDoubleParse(GetBalance());
				this.labelBalance.Text = dGetBanlance.ToString("N2");
				this.Cheque.Text = dGetBanlance.ToString("N2");
				this.Cheque.SelectAll();
				this.Cashout.Text = "";
				this.Cheque.Focus();

				FormWeChat fmw = new FormWeChat();
				if (Program.m_sDMoniter == true)
				{
					m_fmad.m_hParent = this.Handle;
				}
				string inv = GetNextInvNumber();
				fmw.m_sInvoiceNumber = Program.m_sBranchID + Program.m_sStationID + inv;
				fmw.m_dAmount = dGetBanlance;
				fmw.ShowDialog(); 
				if(fmw.m_bSuccess)
				{
					m_dCheque = dGetBanlance;
					if (TotalPaymentOK())
					{
						doShowPaymentInfo();
						DoCheckout();
						Cash.Text = "";
						labelBalance.Text = "";
						return;
					}
				}
				Cheque.Text = "";
				Cheque.Focus();
			}
			else
			{
				m_bSurcharge = false;
                m_fmad.lblSurCharge.Text = "";
                m_fmads.lblSurCharge.Text = "";
				RefreshPayment("payment_cheque");
				double dGetBanlance = Program.MyDoubleParse(GetBalance());
				this.labelBalance.Text = dGetBanlance.ToString("N2");
				this.Cheque.Text = dGetBanlance.ToString("N2");
				this.Cheque.SelectAll();
				this.Cashout.Text = "";
				this.Cheque.Focus();
			}
*/ 
		}
		private void btn_charge_Click(object sender, EventArgs e)
		{
			m_bSurcharge = false;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
			if (m_dVipPayAmount != 0)
				return;
			if ((!m_bScanMember && MemberShipID.Text == "") || m_nCardId == 0)
			{
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = " Please keyin Vip ID!";
				fm.ShowDialog();
				MemberShipID.Focus();
				return;
			}
			RefreshPayment("payment_charge");
            m_dRoundingTotal = 0;
			double dGetBanlance = Program.MyDoubleParse(GetBalance());
			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.charge.Text = dGetBanlance.ToString("N2");
			this.charge.SelectAll();
			this.charge.Focus();
		}
		private void button12_Click(object sender, EventArgs e)
		{
            CachePromotion(); //refresh promo_cache_table
			if (Program.m_bEnableSelfService)
				timer5.Stop();
			Cash.Text = "";
			Cashout.Text = "";
			Eftpos.Text = "";
			Credit.Text = "";
			Cheque.Text = "";
			charge.Text = "";

			bool bVipPaymentOK = false;
			if (m_dVipPayAmount != 0)
			{
				FormMSG fm = new FormMSG();
				fm.m_sYesNo = "1";
				fm.m_sMsg = "Finish VIP Payment?";
				fm.ShowDialog();
				if (fm.m_bYes)
				{
					DoVipCheckout();
					ResetCart();
					ResetPaymentLabels();
				}
				bVipPaymentOK = true;
			}
			double dBalance = Math.Round(m_dTotal - m_dTotalPaid + m_dRoundingTotal + m_dCashoutTotal, 2);
			bool bPaidInFull = TotalPaymentOK();
			if (bVipPaymentOK || m_dTotalPaid == 0 || bPaidInFull) //Math.Round(m_dTotalPaid, 2) == Math.Round(m_dTotal, 2) + m_dRoundingTotal)
			{
				txtpaymentinfo.Text = "";
				panelCheckout.Visible = false;
				txtShowCashOut.Visible = false;
				txtcashouttitle.Visible = false;
				Change.Visible = true;
				m_fmad.showchange.Text = "";
				m_fmad.labelChange.Text = "";
				m_fmads.showchange.Text = "";
				m_fmads.labelChange.Text = "";
				ResetPayment();
				barcode.Focus();
				this.m_iVoucherControl = 0;
				this.m_dSurcharge = 0;
				this.m_dSurchargeRate = 0;
				txtSurcharge.Text = "";
				m_bSurcharge = false;
			}
			else
			{
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = "Please clear payment first!\r\n";
                fm.m_sMsg += "OrderTotal = " + m_dTotal.ToString() + "\r\n";
				fm.m_sMsg += "TotalPaid = " + m_dTotalPaid.ToString();
				fm.m_sMsg += "TotalRounding = " + m_dRoundingTotal.ToString();
				fm.ShowDialog();
				return;
			}

			if (bVipPaymentOK || bPaidInFull) //paid in full
			{
				ResetCart();
				ResetPaymentLabels();
				this.barcode.Select();
				this.barcode.Focus();
				UpdateDtCart(m_nCurrentCart);
				m_sDiscount[m_nCurrentCart + 1] = "";
			}
			panelHold.Enabled = true;
            m_fmad.lblSurCharge.Text = "";
            m_fmads.lblSurCharge.Text = "";
		}
		private void panel3_Paint(object sender, PaintEventArgs e)
		{
		}
		private void btn_20_Click(object sender, EventArgs e)
		{
		}
		private void btn_penter_Click(object sender, EventArgs e)
		{
		}
		private void barcode_MouseClick(object sender, MouseEventArgs e)
		{
			m_bScanMember = false;
		}
		private bool calCashRedeen(double dOrderTotal)
		{
			return true;
		}
		private bool m_bEftposLog(string note, bool ok)
		{
			if (!Program.m_bEnableEftposLog)
				return true;
			if (dst.Tables["eftposlog"] != null)
				dst.Tables["eftposlog"].Clear();
			string sc1 = " BEGIN TRANSACTION ";
			sc1 += " INSERT INTO eftposlog (elog, ok) ";
			sc1 += " VALUES ('" + note + "','";
			sc1 += ok;
			sc1 += "')";
			sc1 += "SELECT IDENT_CURRENT('eftposlog') AS id";
			sc1 += " COMMIT ";
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc1, myConnection);
				if (myCommand.Fill(dst, "eftposlog") == 1)
				{
					m_iEftposLog = Program.MyIntParse(dst.Tables["eftposlog"].Rows[0]["id"].ToString());
				}
				//else
				//{
				////	MessageBox.Show("record log fail", "Error");
				//	return false;
				//}
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc1, e);
				return false;
			}
			return true;
		}
        private bool insertEftposLogError(string inv_number, string till, string elog)
        {
            if (!Program.m_bEnableEftposLog)
                return true;
            if (dst.Tables["insertEftposLogErro"] != null)
                dst.Tables["insertEftposLogErro"].Clear();
            string sc1 = " BEGIN TRANSACTION ";
            sc1 += " INSERT INTO eftposlog (inv_number, elog, till) ";
            sc1 += " VALUES ('" + inv_number + "'";
            sc1 += " ,'" +elog+ "'";
            sc1 += " ,'" +till+ "'";
            sc1 += "')";
            sc1 += "SELECT IDENT_CURRENT('eftposlog') AS id";
            sc1 += " COMMIT ";
            try
            {
                SqlDataAdapter myCommand = new SqlDataAdapter(sc1, myConnection);
                if (myCommand.Fill(dst, "insertEftposLogErro") == 1)
                {
                    m_iEftposLog = Program.MyIntParse(dst.Tables["insertEftposLogErro"].Rows[0]["id"].ToString());
                }
            }
            catch (Exception e)
            {
                myConnection.Close();
                Program.ShowExp(sc1, e);
                return false;
            }
            return true;
        }

		private bool DoUpdateEftposLog(string inv_num, string id, string till)
		{
			string sc = "UPDATE eftposlog SET inv_number =" + inv_num;
			sc += ", till =" + till;
			sc += " WHERE id =" + id;
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
		private bool DoClearUpEftplog(string till)
		{

			if (Program.m_sEftposLogDay.Trim() == "")
				return true;
			string sc = "DELETE FROM eftposlog WHERE DATEDIFF(day, date, GETDATE()) >= " + Program.m_sEftposLogDay;
			sc += " AND till =" + till;
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
				myCommand.Connection.Close();
				return false;
			}
			return true;
		}
		private bool DoCreateVoucher()
		{
			string nextcode = GetItemCode();
			string expire_date = DateTime.Now.AddDays(int.Parse(m_sVoucherValidDay)).ToString("dd-MM-yyyy");
			string expire_dates = DateTime.Now.AddDays(int.Parse(m_sVoucherValidDay)-1).ToString("dd-MM-yyyy");
			string voucher_barcode = expire_date.Replace("-", "");
			voucher_barcode += m_invoiceNumber;
			voucher_barcode += Program.m_sStationID;
			m_sVoucherBarcode = voucher_barcode;
			int r = Program.MyIntParse((m_dTotal / m_dVoucherStart).ToString());
            double voucher_amount = 0;
            if (m_bVipVoucherEnabled && m_dVipVoucherValue > 0 )
                voucher_amount = m_dVipVoucherValue;
            else
                voucher_amount = Program.MyDoubleParse(r.ToString()) * m_dVoucherRate;
			m_sVoucherAmount = voucher_amount.ToString("c");

			//Check voucher Item
			string sc = "";
			if (!ExistItemCode())
			{
				sc += " INSERT INTO code_relations (id, code, name, name_cn, brand, cat, price1, manual_cost_frd, supplier_price) ";
				sc += " VALUES ";
				sc += " ('" + DateTime.Now.ToString("ddmmyyyyhhss") + "'";
				sc += ", '";
				sc += nextcode + "'";
				sc += ", N'Voucher'";
				sc += ", N'Voucher'";
				sc += ", N''";
				sc += ", N'ServiceItem'";
				sc += ", '0'";
				sc += ", '0'";
				sc += ", '0'";
				sc += " )";
				sc += " UPDATE code_relations SET id=code, supplier_code=code WHERE code =" + nextcode;
				sc += " INSERT INTO product (code, name, name_cn, brand, cat, s_cat, ss_cat, hot, price, stock, eta, supplier, supplier_code, supplier_price, price_dropped, price_age, allocated_stock, popular, real_stock) SELECT ";
				sc += " c.code, c.name, c.name_cn,c.brand,  c.cat, c.s_cat, c.ss_cat, '0', c.price1, 0,'', c.supplier, c.supplier_code, c.supplier_price, '0', getdate(), 0, '1', '0' FROM";
				sc += " code_relations c WHERE c.code =" + nextcode;
				sc += " INSERT INTO stock_qty (code, qty, branch_id, supplier_price, allocated_stock, average_cost, qpos_price, special_price, sp_start_date, sp_end_date) SELECT ";
				sc += " p.code, 0, 1, p.supplier_price,  p.allocated_stock, 0, p.price, '0', getdate(), getdate() FROM product p WHERE p.code =" + nextcode;
				sc += " INSERT INTO catalog (seq, cat, s_cat, ss_cat) VALUES ('99', N'Voucher', N'', N'')";
				sc += " UPDATE settings SET value='" + nextcode + "' WHERE name=N'voucher_item_code' ";
			}

			sc += " INSERT INTO barcode (item_code,barcode,item_qty, carton_qty, carton_barcode, box_qty, package_price, supplier_code, invoice_number, voucher_amount)";
			sc += " SELECT c.code, '" + m_sVoucherBarcode + "', 1,0,'',0,'0',c.supplier_code,'" + m_invoiceNumber + "', '" + voucher_amount.ToString() + "'  FROM code_relations c WHERE 1=1";
			sc += " AND c.code = (SELECT TOP 1 value FROM settings WHERE name= N'voucher_item_code')";

			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception er)
			{
				myConnection.Close();
				Program.ShowExp(sc, er);
				return true;
			}
			string ssv = ReadSitePage("voucher_template");
			if (ssv == "") //not ready, create a temperate one
			{
				ssv = "*********************************\r\n";
				ssv += "* " + Program.m_sTradingName + " Voucher\r\n";
				ssv += "* Voucher Value : " + voucher_amount.ToString("c") + "\r\n";
				ssv += "* Created Date : " + DateTime.Now.ToString("dd-MM-yyyy") + "\r\n";
				ssv += "* Expire Date : " + expire_dates + "\r\n";
				// ssv += "* Inv : " + m_invoiceNumber + "\r\n";
				// ssv += "* \r\n";
				ssv += "* " + m_sVoucherBarcode + "\r\n";
				ssv += "* \r\n";
				ssv += "* Please use only in this branch \r\n";
				ssv += "* Please use with in expiration \r\n";
				ssv += "*********************************\r\n";
			}
			ssv = ssv.Replace("@@voucher_value", m_sVoucherAmount);
			ssv = ssv.Replace("@@expire_date", expire_dates);
			ssv = ssv.Replace("@@voucher_barcode", voucher_barcode);
			ssv = ssv.Replace("@@voucher_invoice", m_invoiceNumber);
			m_sVoucherText = ssv;
			return true;
		}
		private string DoUseVoucher(string barcode)
		{
			if (barcode.Trim() == "")
				return "";
			string vDays = barcode.Substring(0, 2);
			string vMonth = barcode.Substring(2, 2);
			string vYear = barcode.Substring(4, 4);
			DateTime vVolidDay;
			try
			{
				vVolidDay = DateTime.Parse(vDays + "/" + vMonth + "/" + vYear);
			}
			catch (Exception e)
			{
				string s = e.ToString();
				return "Product Not Found";
			}

			if (dst.Tables["usevoucher"] != null)
				dst.Tables["usevoucher"].Clear();
			
			string sc = " SELECT * FROM barcode WHERE item_code='" + m_sVoucherItem + "' AND barcode ='" + barcode + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "usevoucher") <= 0)
				{
					return "Vourcher has been used \r\n or Not Existing";
				}
			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return "";
			}
			if (dst.Tables["usevoucher"].Rows.Count == 1)
			{
				if (bool.Parse(dst.Tables["usevoucher"].Rows[0]["bcancelled"].ToString()))
					return "This voucher has been \r\ncancelled. ref:" + dst.Tables["usevoucher"].Rows[0]["invoice_number"].ToString();
			}
			if (vVolidDay <= DateTime.Now)
			{
				DoDeleteVoucherBarcode(barcode);
				return "    Voucher Is Expired" + "\r\n" + "     Click Ok To Delete";
			}
			this.barcode.Text = barcode;
			m_sVoucherUsedBarcode = barcode;
			return (0 - double.Parse(dst.Tables["usevoucher"].Rows[0]["voucher_amount"].ToString())).ToString();
		}
		private bool DoDeleteVoucherBarcode(string b)
		{
			if (b.Trim() == "")
				return false;
			string sc = " DELETE FROM barcode WHERE barcode ='" + b + "'";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception er)
			{
				myConnection.Close();
				Program.ShowExp(sc, er);
				return false;
			}
			return true;
		}
		private bool DoCheckVoucher(string inv_number)
		{
			if (dst.Tables["checktotal"] != null)
				dst.Tables["checktotal"].Clear();
			string sc = " SELECT total FROM invoice WHERE invoice_number=" + inv_number;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checktotal") <= 0)
				{
					FormMSG invnofound = new FormMSG();
					invnofound.btnNo.Visible = false;
					invnofound.btnYes.Visible = false;
					invnofound.m_sMsg = "     Invoice Not Found  ";
					invnofound.Show();
					return false;
				}
			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return false;
			}
			string old_total = dst.Tables["checktotal"].Rows[0]["total"].ToString();
			double old_invoice_total = Program.MyDoubleParse(old_total);
			double new_total = 0;
			if (m_dTotal < 0)
				new_total = old_invoice_total - (0 - m_dTotal);
			else
				new_total = old_invoice_total - m_dTotal;

			if (old_invoice_total < m_dTotal)
				return true;

			if (new_total < m_dVoucherStart)
			{
				sc = " UPDATE barcode SET bcancelled = 1, voucher_amount= '0',cancelled_note = 'Inv Total:" + double.Parse(old_total).ToString("c") + "\r\nRefunded Total:" + m_dTotal.ToString("c");
				sc += " \r\nVoucher Total:" + new_total.ToString("c") + "\r\nAction Date:" + DateTime.Now.ToString() + "' WHERE invoice_number=" + inv_number;
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception er)
				{
					myConnection.Close();
					Program.ShowExp(sc, er);
					return false;
				}
				return true;
			}
			else
			{
				int r = Program.MyIntParse((new_total / m_dVoucherStart).ToString());
				double new_voucher_amount = Program.MyDoubleParse(r.ToString()) * m_dVoucherRate;
				sc = " UPDATE barcode SET voucher_amount='" + new_voucher_amount + "',";
				sc += " cancelled_note = 'Inv Total:" + double.Parse(old_total).ToString("c") + "\r\nRefunded Total:" + m_dTotal.ToString("c");
				sc += " \r\nVoucher Total:" + new_total.ToString("c") + "\r\nAction Date:" + DateTime.Now.ToString() + "'";
				sc += " WHERE invoice_number=" + inv_number;
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception er)
				{
					myConnection.Close();
					Program.ShowExp(sc, er);
					return false;
				}
			}
			return true;
		}
		private void btnvouchersave_Click(object sender, EventArgs e)
		{
			if (txtoldinv.Text.Trim() == "")
				return;
			if (DoCheckVoucher(txtoldinv.Text))
			{
				FormMSG oldinvsave = new FormMSG();
				oldinvsave.btnYes.Visible = false;
				oldinvsave.btnNo.Visible = false;
				oldinvsave.m_sMsg = "    Invoice Number Saved   ";
				oldinvsave.Show();
				txtoldinv.Text = "";
				pnlkeyininv.Visible = false;
			}
		}
		private void btnvclose_Click(object sender, EventArgs e)
		{
			pnlkeyininv.Visible = false;
			txtoldinv.Text = "";
			barcode.Focus();
		}
		private bool DoCheckvoucherStatus()
		{
			if (dst.Tables["checkvoucherinfo"] != null)
				dst.Tables["checkvoucherinfo"].Clear();
			string sc = "SELECT * FROM barcode WHERE 1=1 AND";
			if (txtvoucherbarcode.Text.Length >= 10)
				sc += " barcode ='";
			else
				sc += " invoice_number ='";
			sc += txtvoucherbarcode.Text;
			sc += "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkvoucherinfo") <= 0)
				{
					rctxtvoucher.Text = " No Record ";
					return false;
				}
			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return false;
			}
			rctxtvoucher.Text = dst.Tables["checkvoucherinfo"].Rows[0]["cancelled_note"].ToString();
			txtvoucherbarcode.SelectAll();
			return true;
		}
		private void btncheckvoucher_Click(object sender, EventArgs e)
		{
			DoCheckvoucherStatus();
		}
		private void button2_Click(object sender, EventArgs e)
		{
			pnlInvVou.Visible = false;
			rctxtvoucher.Text = "";
			txtvoucherbarcode.Text = "";
			barcode.Focus();
		}
		private void txtvoucherbarcode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode.ToString() == "13" || e.KeyCode.ToString() == "17")
				DoCheckvoucherStatus();
		}
		private void txtvoucherbarcode_TextChanged(object sender, EventArgs e)
		{
			DoCheckvoucherStatus();
		}
		private void MemberShipID_Click(object sender, EventArgs e)
		{
			if (!lblsearchvip.Visible)
				lblsearchvip.Visible = true;
			else
				lblsearchvip.Visible = false;
		}
		private void lblsearchvip_Click(object sender, EventArgs e)
		{
			DoSearchVIP();
		}
		private void MemberShipName_Click(object sender, EventArgs e)
		{
			DoSearchVIP();
		}
		private void DoSearchVIP()
		{
			FormSearchVip fm = new FormSearchVip();
			this.MemberShipID.Text = "";
			fm.ShowDialog();
			this.m_sCardId = fm.m_sBarcode;
			this.MemberShipID.Text = m_sCardId;
			if (m_sCardId != "")
				ScanMembership();
			else
				this.MemberShipID.Focus();
		}
		private string GetItemCode()
		{
			if (dst.Tables["itemcode"] != null)
				dst.Tables["itemcode"].Clear();
			string sc = " SELECT TOP 1 code + 1 AS code FROM code_relations ORDER BY code  DESC";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "itemcode") <= 0)
					return "";
			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return "";
			}
			string itemcode = "";
			if (dst.Tables["itemcode"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["itemcode"].Rows[0];
				itemcode = dr["code"].ToString();
			}
			return itemcode;
		}
		private bool ExistItemCode()
		{
			if (dst.Tables["existsitem"] != null)
				dst.Tables["existsitem"].Clear();
			string sc = "SELECT * FROM code_relations WHERE code=(SELECT value FROM settings WHERE name=N'voucher_item_code')";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "existsitem") <= 0)
					return false;
			}
			catch (Exception ex)
			{
				myConnection.Close();
				Program.ShowExp(sc, ex);
				return false;
			}
			if (dst.Tables["existsitem"].Rows.Count > 0)
				return true;
			return false;
		}

		private void DoCheckLicense()
		{
			if (Program.CheckLicense() == "1")
			{
				m_bTrial = true;
				m_sTrialDateLeft = Program.m_sTrialDay;
				double dDay = (Convert.ToDouble(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sSecrectValue + "\\", "expired_day", "")));
				if (dDay > 0)
				{
					m_bTrial = false;
				}
				else
				{
					m_bTrialExpired = true;
					FormMSG fm = new FormMSG();
					fm.btnYes.Visible = false;
					fm.btnNo.Visible = false;
					fm.m_sMsg = "Sorry, the system is expired!";
					fm.ShowDialog();
					return;
				}
			}
			else if (Program.CheckLicense() == "2")
			{
				FormMSG invalidKey = new FormMSG();
				invalidKey.btnNo.Visible = false;
				invalidKey.btnYes.Visible = false;
				invalidKey.m_sMsg = " Invalided Key ";
				invalidKey.ShowDialog();
				Key_Register keyin = new Key_Register();
				keyin.m_sKeyInvalided = "1";
				keyin.ShowDialog();
				if (keyin.m_sKeyInvalided == "1")
				{
					buttonSpot.Enabled = false;
					buttonAdmin.Enabled = false;
					//                    m_bKeyInvalid = true;
				}

			}
			else
			{
				if (Program.CheckKeyDate())
				{
					if (double.Parse(Program.m_sTrialDayLife) <= -20)
					{
						FormMSG renewkey = new FormMSG();
						renewkey.btnNo.Visible = false;
						renewkey.btnYes.Visible = false;
						renewkey.m_sMsg = "Your Linece is expired,\r\nContact suppor for renew";
						renewkey.ShowDialog();
						Key_Register keyin = new Key_Register();
						keyin.ShowDialog();
					}
					else
						btnPayByPoint.Visible = false;
				}
				else
					btnPayByPoint.Visible = false;

			}
			return;
		}
		private bool doAddNewPriceToItem()
		{
			string newItemPrice = this.price.Text;
			string sc = " UPDATE code_relations SET price1 = " + newItemPrice;
			sc += " WHERE code = " + m_sNewPriceCodeFlag;
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
			m_sNewPriceCodeFlag = "0";
			return true;
		}
		private void btnPayByPoint_Click(object sender, EventArgs e)
		{
			Key_Register key = new Key_Register();
			key.ShowDialog();
		}

		private void button15_Click(object sender, EventArgs e)
		{
			//this.CreateNewItemPanel1.Visible = false;
			//doClearCreateForm();
			//this.CreateNewBarcodeTB.Text = "";
			this.barcode.Focus();
		}

		private int GetNextCode()
		{
			int next_code = -1;
			if (dst.Tables["get_code"] != null)
				dst.Tables["get_code"].Clear();
			int rows;
			string sc = "SELECT TOP 1 code FROM code_relations ORDER BY code DESC";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "get_code");
				if (rows > 0)
					next_code = int.Parse(dst.Tables["get_code"].Rows[0]["code"].ToString()) + 1;
				else
					//					next_code = 1010;
					next_code = 1020;
			}
			catch (Exception e)
			{
				myConnection.Close();
				Program.ShowExp(sc, e);
			}
			return next_code;
		}
		private void button16_Click(object sender, EventArgs e)
		{

		}
		private void btnSetting_Click(object sender, EventArgs e)
		{
			if (!AdminOK("setting"))
				return;
			m_sAdminAction = "setting";
			DoAdminAction();
		}
		private void button15_Click_1(object sender, EventArgs e)
		{
			this.CreateNewItemPanel.Visible = false;
			this.numericpad.Visible = false;
			doClearCreateForm();
			this.CreateNewBarcodeTB.Text = "";
			this.barcode.Focus();
		}
		private bool doCreateNewItem()
		{
			if (this.CreateNewNameTB.Text.Trim() == "" || this.CreateNewNameTB.Text.Trim() == null || this.CreateNewBarcodeTB.Text.Trim() == "" || this.CreateNewBarcodeTB.Text.Trim() == null)
				return false;
			string barcodeNew = this.CreateNewBarcodeTB.Text.Trim();
            string supplierCode = this.CreateNewSupplierCodeTB.Text.Trim();
			string name = this.CreateNewNameTB.Text.Trim();
			string otherName = this.CreateNewOtherNameTB.Text.Trim();
			string brand = "";
			if (this.BrandComboBox.Text.Trim() != null && this.BrandComboBox.Text.Trim() != "")
				brand = this.BrandComboBox.Text.Trim();
			else
				brand = this.CreateNewBrandTB.Text.Trim();
			string catnew = "";
			if (this.catselectionCB.Text.Trim() != null && this.catselectionCB.Text.Trim() != "")
				catnew = this.catselectionCB.Text.Trim();
			else
				catnew = this.CreateNewCatTB.Text.Trim();
			/***********************************/
			string scatnew = "";
			if (this.scatselectionCB.Text.Trim() != null && this.scatselectionCB.Text.Trim() != "")
				scatnew = this.scatselectionCB.Text.Trim();
			else
				scatnew = this.CreateNewSCatTB.Text.Trim();
			string sscatnew = "";
			if (this.sscatselectionCB.Text.Trim() != null && this.sscatselectionCB.Text.Trim() != "")
				sscatnew = this.sscatselectionCB.Text.Trim();
			else
				sscatnew = this.CreateNewSSCatTB.Text.Trim();
			/***********************************/

			string sTaxcode = "";
			if (this.cbTaxCode.Text.Trim() != null && this.cbTaxCode.Text.Trim() != "")
				sTaxcode = this.cbTaxCode.Text.Trim();
			string sTaxrate = "";
			if (this.txtTaxRate.Text.Trim() != null && this.txtTaxRate.Text.Trim() != "")
				sTaxrate = (Program.MyDoubleParse(this.txtTaxRate.Text.Trim()) / 100).ToString();

			string priceNew = "0";
			if (this.CreateNewSellPriceTB.Text.Trim() != "" && this.CreateNewSellPriceTB.Text.Trim() != null)
				priceNew = this.CreateNewSellPriceTB.Text.Trim();
			string Mcost = this.CreateNewMcostTB.Text.Trim();
			string codeNew = GetNextCode().ToString();

			string sc = "INSERT INTO code_relations (id, code, supplier_code, name, name_cn, brand, cat,s_cat,ss_cat, price1, manual_cost_frd, supplier_price, tax_code, tax_rate) ";
			sc += " VALUES ";
			//			sc += " ('" + DateTime.Now.ToOADate().ToString() + "'";
			sc += " ('" + DateTime.Now.ToString("yyyyMMddHHmmss") + "'";
			sc += ", ";
			sc += codeNew;
            sc += ", '" + supplierCode + "'";
			sc += ", N'" + name + "'";
			sc += ", N'" + otherName + "'";
			sc += ", N'" + brand + "'";
			sc += ", N'" + catnew + "'";
			/*********************/
			sc += ", N'" + scatnew + "'";
			sc += ", N'" + sscatnew + "'";
			/*********************/
			sc += ", ";
			sc += "'" + priceNew + "'";
			sc += ", '" + Mcost + "'";
			sc += ", '" + Mcost + "'";
			sc += ", '" + sTaxcode + "'";
			sc += ", '" + sTaxrate + "'";
			sc += ")";
			sc += " UPDATE code_relations SET barcode = '" + Program.EncodeQuote(barcodeNew) + "' ";
            if(supplierCode == "" || supplierCode == null)
			    sc += ", supplier_code = code ";
            sc += ", average_cost = supplier_price WHERE code =" + codeNew;
			sc += " INSERT INTO product (code, name, name_cn, brand, cat, s_cat, ss_cat, hot, price, stock, eta, supplier, supplier_code, supplier_price, price_dropped, price_age, allocated_stock, popular, real_stock) SELECT ";
			sc += " c.code, c.name, c.name_cn,c.brand,  c.cat, c.s_cat, c.ss_cat, '0', c.price1, 0,'', c.supplier, c.supplier_code, c.supplier_price, '0', getdate(), 0, '1', '0' FROM";
			sc += " code_relations c WHERE c.code =" + codeNew;

			sc += " INSERT INTO stock_qty (code, qty, branch_id, supplier_price, allocated_stock, average_cost, qpos_price, special_price, sp_start_date, sp_end_date) SELECT ";
			sc += " p.code, 0, 1, p.supplier_price,  p.allocated_stock, 0, p.price, '0', getdate(), getdate() FROM product p WHERE p.code =" + codeNew;

			if (catnew != "" && (this.catselectionCB.Text.Trim() == "" || this.catselectionCB.Text.Trim() == null))
				sc += " INSERT INTO catalog (seq, cat) VALUES ('99', N'" + catnew + "')";
			/************************/
			if (catnew != "" && (this.catselectionCB.Text.Trim() == "" || this.catselectionCB.Text.Trim() == null))
				sc += " INSERT INTO catalog (seq, cat,s_cat) VALUES ('99', N'" + catnew + "', N'" + scatnew + "')";
			if (catnew != "" && (this.sscatselectionCB.Text.Trim() == "" || this.sscatselectionCB.Text.Trim() == null))
				sc += " INSERT INTO catalog (seq, cat, s_cat, ss_cat) VALUES ('99', N'" + catnew + "', N'" + scatnew + "', N'" + sscatnew + "')";
			/*************************/
			if (brand != "" && (this.BrandComboBox.Text.Trim() == "" || this.BrandComboBox.Text.Trim() == null))
				sc += " INSERT INTO catalog (seq, cat, s_cat) VALUES ('99', N'Brands', N'" + brand + "')";
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
            
            sc = "IF NOT EXISTS(SELECT * FROM barcode WHERE barcode ='" + barcodeNew + "') ";
            sc += " BEGIN ";
            sc += " INSERT INTO barcode (item_code, barcode, item_qty, carton_barcode, box_qty, package_price, supplier_code)";
            sc += " SELECT c.code, '" + barcodeNew + "', 1, 0, 0, 0, c.supplier_code FROM code_relations c WHERE c.code='" + codeNew + "'";
            sc += " END ";
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
		private void doClearCreateForm()
		{
			this.CreateNewNameTB.Text = "";
			this.CreateNewBrandTB.Text = "";
            this.CreateNewSupplierCodeTB.Text = "";
			this.CreateNewOtherNameTB.Text = "";
			this.CreateNewCatTB.Text = "";
			this.CreateNewMcostTB.Text = "";
			this.catselectionCB.Text = "";
			this.BrandComboBox.Text = "";
			this.CreateNewSellPriceTB.Text = "";
		}
		private void button16_Click_1(object sender, EventArgs e)
		{
			if (CreateNewNameTB.Text == "" || CreateNewSellPriceTB.Text == "" || cbTaxCode.Text == "")
			{
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = "* cannot be blank!";
				fm.ShowDialog();
				return;
			}
			if (MessageBox.Show("Are you sure you want to create a new item with this barcode?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				if (!doCreateNewItem())
					MessageBox.Show("Error, New Item Creation Failed");
				else
				{
					MessageBox.Show("New Item Creation Complete!");
					this.CreateNewItemPanel.Visible = false;
					this.barcode.Text = this.CreateNewBarcodeTB.Text.Trim();
					doClearCreateForm();
					this.CreateNewBarcodeTB.Text = "";
					this.CreateNewCatTB.Text = "";
					this.CreateNewSCatTB.Text = "";
					this.CreateNewSSCatTB.Text = "";
					this.numericpad.Visible = false;
					DoScanBarcode(false);
				}
			}
		}
		private void timer2_Tick(object sender, EventArgs e)
		{
			timer2.Stop();
			m_bEftposWaiting = false;
			btn_penter.Visible = true;
		}
		private void buttonAdmin_Click(object sender, EventArgs e)
		{
			if (this.flAdminBtns.Visible == false)
				this.flAdminBtns.Visible = true;
			else
				this.flAdminBtns.Visible = false;
		}
		private void form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (m_mod != null)
				m_mod.CloseModem();
			eftPanel.DoClose();
			eftPanel.Close();
		}
		private void buttonAdmin_Click_1(object sender, EventArgs e)
		{
		}
		private void buttonSpot_Click_1(object sender, EventArgs e)
		{
		}
		private void button20_Click(object sender, EventArgs e)
		{
			this.flAdminBtns.Visible = false;
			//			if (!AdminOK("vip payment"))
			//				return;
			m_sAdminAction = "open_draw";
			DoAdminAction();
		}
		private void button17_Click(object sender, EventArgs e)
		{
			this.flAdminBtns.Visible = false;
			if (!AdminOK("adminzone"))
				return;

			m_sVipAmounts = "";
			m_sVipInvoices = "";
			m_sVipPaymentTypes = "";
			m_dVipPayAmount = 0;
			btncompanyok AP = new btncompanyok();
		//	AP.m_sStaff = this.m_nSalesId;
			AP.Owner = this;
			AP.BringToFront();
			AP.ShowDialog();
			AP.BringToFront();

			if (AP.m_bPay)
			{
				m_sVipAmounts = AP.m_sAmounts;
				m_sVipInvoices = AP.m_sInvoices;
				m_dVipPayAmount = AP.m_dPayAmount;
				m_sCardId = AP.m_sVipCardId;
				MemberShipID.Text = AP.lblVABarcode.Text;// AP.m_sVipCardId;
//				m_sCardName = AP.lblVAName.Text;
//				m_sCardBarcode = AP.lblVABarcode.Text;
				DoVipPayment();
				return;
			}
			CachePromotion();
			GetPromotion();
			RepositionPanels();
			BuildMenuButtons();
			//			barcode.Focus();
			if (Program.m_bEnableSelfService)
				SelfServiceModelMain();
		}
		private void DoVipPayment()
		{
			m_dTotal = m_dVipPayAmount;
			labelBalance.Text = Math.Round(m_dTotal, 2).ToString();
			btn_charge.Enabled = false;
			charge.Enabled = false;
			panelCheckout.Location = new Point(245, 50);
			panelCheckout.Size = new Size(770, 736);
			panelCheckout.BringToFront();
			panelCheckout.Visible = true;
			txtShowCashOut.Visible = false;
			txtcashouttitle.Visible = false;
			Cash.Focus();
		}
		private void btnInvTrace_Click(object sender, EventArgs e)
		{
			//			if (!AdminOK(""))
			//				return;
			this.flAdminBtns.Visible = false;
			invoicetrace frminv = new invoicetrace();
			frminv.ShowDialog();
		}
		private void btnSync_Click(object sender, EventArgs e)
		{
return;		
			this.flAdminBtns.Visible = false;
			FormSync fSync = new FormSync();
			fSync.ShowDialog();
			fSync.DoUploadInvoiceActivata();
			FormMSG fm = new FormMSG();
			fm.m_sMsg = "Sync Done!";
			fm.btnNo.Visible = false;
			fm.btnYes.Visible = false;
			fm.ShowDialog();

			/*	
				if (MessageBox.Show("Are you sure you want want to Sync now?", "Sync Process", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
				}
				else
				{
					Process psync = new Process();
					psync.StartInfo.FileName = "ezFSyncC.exe";
					psync.StartInfo.Arguments = "-auto";
					psync.Start();
				}
			 */
		}
		private void btnClock_Click(object sender, EventArgs e)
		{
			clockinout ck = new clockinout();
			ck.ShowDialog();
		}
		private void button19_Click(object sender, EventArgs e)
		{
			//if (cart.Rows.Count <= 0)
			//    return;
			//if (Program.m_bEnableAutoTare)
			//    DoTare(double.Parse(Program.m_sTare));
			//else 
			//{
			//    txtTare.Text = "";
			//    panelChangeTare.Visible = true;
			//    txtTare.Focus();
			//}
			panelNote.Size = new Size(378, 245);
			panelNote.Location = new Point(400, 145);
			panelNote.Visible = true;
			txtNote.Focus();
			panelNote.BringToFront();

		}
		private void textBox2_TextChanged(object sender, EventArgs e)
		{
		}
		private void catselectionCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			scatselectionCB.SelectedItem = "";
			scatselectionCB.SelectedText = "";
			scatselectionCB.SelectedValue = "";
			doBuideSCat();
			sscatselectionCB.SelectedItem = "";
			sscatselectionCB.SelectedText = "";
			sscatselectionCB.SelectedValue = "";
			doBuideSSCat();

		}

		private void scatselectionCB_SelectedIndexChanged(object sender, EventArgs e)
		{
			doBuideSSCat();
		}

		private void btnYes_Click(object sender, EventArgs e)
		{
			doAddQty(Program.MyDoubleParse(this.txtQty.Text), m_irow);
			m_AddonQty = "1";
			this.barcode.Text = "";
			m_irow = "";
			this.pnlAddOnQty.Visible = false;
		}

		private void btnNo_Click(object sender, EventArgs e)
		{
			this.pnlAddOnQty.Visible = false;
			m_irow = "";
			this.barcode.Text = "";
		}

		private void txtQty_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13 || e.KeyValue == 17)
			{
				btnQtyOK_Click(sender, e);
			}
		}

		private void txtQty_Leave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}


		private void button21_Click(object sender, EventArgs e)
		{
			this.panelChangeTare.Visible = false;
		}

		private void button22_Click(object sender, EventArgs e)
		{
			double dQty = Program.MyDoubleParse(txtTare.Text);
			if (dQty > 0)
				//				doChangeQty(dQty);
				DoTare(dQty);
			m_irow = "";
			panelChangeTare.Visible = false;
		}

		private void txtTare_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13 || e.KeyValue == 17)
			{
				button22_Click(sender, e);
			}
		}

		private void txtTare_Leave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}
/** 
 * Handle Latipay Gift Card Start
 * */
		private void btnLatipayGiftCard_Click(object sender, EventArgs e)
		{
			FormLatipayGiftCard flgc = new FormLatipayGiftCard();
			flgc.m_sTillNumber = Program.m_sStationID;
			flgc.m_sCartNumber = m_nCurrentCart.ToString();
			flgc.ShowDialog();
			CheckLatipayGiftCard(flgc.m_sVerifiedCardCode, flgc.m_sVerifiedCardFaceValue);
		}
		private void CheckLatipayGiftCard(string[] verifiedCardCode, string[] verifiedCardFaceValue)
		{
			int i = 0;
			for (i = 0; i < dsCart.Tables[m_nCurrentCart].Rows.Count; )
			{
				string sItemCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
				string sItemName = cart.Rows[i].Cells["cc_name"].Value.ToString();

				if (sItemCode == "1006" && sItemName.IndexOf("LatipayGiftCard") != -1)
					DeleteCartItem(i);
				else
				{
					i++;
					continue;
				}
			}
			double total_order = m_dTotal;
			double total_face_value = 0;
			int iExcess = 0;
			for (i = 0; i < verifiedCardCode.Length; i++)
			{
				string gc_code = verifiedCardCode[i];
				string gc_name = "LatipayGiftCard:" + gc_code.Substring(0, 2) + "***" + gc_code.Substring(gc_code.Length - 3);
				double gc_face_value = Program.MyDoubleParse(verifiedCardFaceValue[i]);
				total_face_value += gc_face_value;
				total_order -= gc_face_value;
				if (total_order < 0)
				{
					if (iExcess > 0)
						gc_face_value = 0;
					else
						gc_face_value += total_order;
					iExcess++;
				}
				AddToCart(gc_code, m_sVoucherItem, gc_name, "", -gc_face_value, 1, "0", false, m_sVoucherItem, 0, false, false);
			}
			if (iExcess > 0)
			{
				MessageBox.Show("The total face value of the gift card(" + total_face_value.ToString("c") + ") is greater than the total price of the order(" + m_dTotal.ToString("c") + "). The total face value of the gift card will be adjusted to the order price! Please remind the customer that the gift card cannot be split and the excess amount will not be refunded..", "prompt");
				MessageBox.Show("If the customer wants to add some new items, please remember to reopen the gift card interface and update the face value of the gift card in the shopping cart.", "prompt");
			}
			return;
		}
		private bool CheckLatipayGiftCardItemInCart(int iSelectedRow, bool bCheckAll)
		{
			int i = iSelectedRow;
			string sItemCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
			string sItemName = cart.Rows[i].Cells["cc_name"].Value.ToString();
			if (bCheckAll)
			{
				int nRow = dsCart.Tables[m_nCurrentCart].Rows.Count;
				if (nRow <= 0)
					return false;
				for (i = 0; i < nRow; i++)
				{
					sItemCode = cart.Rows[i].Cells["cc_code"].Value.ToString();
					sItemName = cart.Rows[i].Cells["cc_name"].Value.ToString();
					if (sItemCode == "1006" && sItemName.IndexOf("LatipayGiftCard") != -1)
						return true;
				}
				return false;
			}
			else
			{
				if (sItemCode == "1006" && sItemName.IndexOf("LatipayGiftCard") != -1)
					return true;
				else
					return false;
			}
		}
		private void DoUpdateLatipayGiftCard(string sInvoiceNumber)
		{
			string sc = " UPDATE latipay_gift_card SET invoice_number = '" + sInvoiceNumber + "'";
			sc += " WHERE 1=1 ";
			sc += " AND till_number ='" + Program.m_sStationID + "'";
			sc += " AND cart_number ='" + m_nCurrentCart.ToString() + "'";
			sc += " AND invoice_number IS NULL ";
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
				myCommand.Connection.Close();
				return;
			}
			return;
		}
/** 
 * Handle Latipay Gift Card End
 * */
		private void btnAA_Click(object sender, EventArgs e)
		{
			DoAAService(m_dOrderTotal, m_invoiceNumber, false);
		}
		private bool DoAAService(double dAmount, string sInvoiceNumber, bool bCheckAmount)
		{
			double dAmountMin = 20;
			if (Registry.CurrentUser.OpenSubKey(Program.m_sRegKey, false) != null)
			{
				dAmountMin = Program.MyDoubleParse(Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "aa_amount_min", "20")));
			}
			if (bCheckAmount && dAmount < dAmountMin)
				return true;
			FormAA fa = new FormAA();
			fa.m_sInvoiceNumber = sInvoiceNumber;
			fa.m_dAmount = dAmount;
			fa.ShowDialog();
			return true;
		}

		private void textBoxNewTotal_TextChanged(object sender, EventArgs e)
		{

		}

		private void CreateNewNameTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewNameTB;
		}

		private void CreateNewOtherNameTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewOtherNameTB;
		}

		private void CreateNewSellPriceTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewSellPriceTB;
		}

		private void CreateNewMcostTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewMcostTB;
		}

		private void btnnumclose_Click(object sender, EventArgs e)
		{
			this.numericpad.Visible = false;
		}

		private void CreateNewBrandTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewBrandTB;
		}

		private void CreateNewCatTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewCatTB;
		}

		private void CreateNewSCatTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewSCatTB;
		}

		private void CreateNewSSCatTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewSSCatTB;
		}

		private void cbTaxCode_SelectedIndexChanged(object sender, EventArgs e)
		{
			GstRatebygstcode();
		}
		private void btnReturn_Click(object sender, EventArgs e)
		{
			rollbackDiscount();
		}
		public void ShowNitroCP()
		{
			eftPanel.ShowNitroCP();
		}
		private void DisplayNotePanel()
		{
			txtNote.Text = m_sNote;
			panelNote.Location = new Point(370, 210);
			panelNote.Visible = true;
			txtNote.Focus();
		}
		private void btnNoteSave_Click(object sender, EventArgs e)
		{
			m_sNote = txtNote.Text;
			panelNote.Visible = false;
            txtNote.Text = "";
		}
		private void btnNoteClose_Click(object sender, EventArgs e)
		{
			m_sNote = txtNote.Text;
			panelNote.Visible = false;
            txtNote.Text = "";
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "FormKeyboard")
                {
                    frm.Close();
                    return;
                }
            }
		}
		private void b_Click(object sender, EventArgs e)
		{
			if (cart.Rows.Count <= 0)
				return;
			if (Program.m_bEnableAutoTare)
				DoTare(double.Parse(Program.m_sTare));
			else
			{
				txtTare.Text = "";
				panelChangeTare.Visible = true;
				txtTare.Focus();
			}
		}
		private void button18_Click(object sender, EventArgs e)
		{
			RefreshPayment("payment_cc");
			if (txtSurcharge.Text == "" || txtSurcharge.Text == "0")
			{
				this.panelSurcharge.Visible = false;
				return;
			}
			m_bSurcharge = true;
			m_dSurchargeRate = Program.MyDoubleParse(this.txtSurcharge.Text) / 100;
			m_dSurcharge = Math.Round(Program.MyDoubleParse(GetBalance()) * m_dSurchargeRate, 2);

			double dGetBanlance = Math.Round(Program.MyDoubleParse(GetBalance()) + m_dSurcharge, 2);

			this.labelBalance.Text = dGetBanlance.ToString("N2");
			this.Credit.Text = dGetBanlance.ToString();
			this.Credit.SelectAll();
			this.Credit.Focus();
			this.panelSurcharge.Visible = false;
            m_fmad.lblSurCharge.Text = m_dSurchargeRate*100 + "% Surcharge Applied!";
            m_fmad.TotalPrice.Text = dGetBanlance.ToString("c");
            m_fmads.lblSurCharge.Text = m_dSurchargeRate * 100 + "% Surcharge Applied!";
            m_fmads.lblSurCharge.Text = dGetBanlance.ToString("c");
		}
		private void button12_Click_1(object sender, EventArgs e)
		{
			this.panelSurcharge.Visible = false;
			txtSurcharge.Text = "";
		}
		private void btnSurcharge_Click(object sender, EventArgs e)
		{
			this.panelSurcharge.Visible = true;
			this.panelSurcharge.BringToFront();
			this.txtSurcharge.Focus();
		}
		private void txtSurcharge_Leave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}
		private void timer3_Tick(object sender, EventArgs e)
		{
			m_nFlashCount++;
			if (m_nFlashCount > 4)
			{
				timer3.Stop();
				m_nFlashCount = 0;
				return;
			}
			int n = Program.MyIntParse(m_irow);
			if (n < 0 || cart.Rows.Count == 0)
				return;
			cart.Rows[n].Selected = !cart.Rows[n].Selected;
		}
		private bool GetItemFromServer(string barcode)
		{
			this.price.Text = "";
			if (!Program.m_bEnableWebService)
				return false;

			CGPOSService gs = new CGPOSService();
			gs.Url = Program.m_sWebServiceUrl;
			string sc = "";
			string s = gs.GetItem(barcode, "service@gpos.co.nz", "9aysdata");
			string[] astr = s.Split(',');
			if (astr.Length < 1)
			{
				MessageBox.Show(s);
				return false;
			}
			if (astr[0] != "found")
				return false;

			string name = astr[1];
			double dPrice = Program.MyMoneyParse(astr[4]);
			string cat = astr[5];
			string s_cat = astr[6];
			string ss_cat = astr[7];

			FormNewItemWebService fm = new FormNewItemWebService();
			fm.m_sBarcode = barcode;
			fm.m_sName = name;
			fm.m_dPrice = dPrice;
			fm.ShowDialog();
			if (!fm.m_bDoImport)
				return false;

			string price1 = fm.m_dPrice.ToString();
			name = fm.m_sName;
			string code = "";
			code = GetNextCode().ToString();
			string supplier_code = astr[3];

			sc = " Begin Transaction ";
			if (Program.m_bSyncWithCloud)
			{
				code = astr[2];
				sc += " IF NOT EXISTS(SELECT code FROM code_relations WHERE code = '" + code + "')";
			}
			sc += " INSERT INTO code_relations (code, barcode, id, supplier, supplier_code, name, hot, skip, price1, cat, s_cat, ss_cat) ";
			sc += " VALUES (" + code + ", '" + Program.EncodeQuote(barcode) + "', '" + Program.EncodeQuote(barcode) + "', '', '" + Program.EncodeQuote(supplier_code) + "' ";
			sc += ", N'" + Program.EncodeQuote(name) + "', 1, 0, " + price1 + ", N'" + Program.EncodeQuote(cat) + "', N'" + Program.EncodeQuote(s_cat) + "', N'" + Program.EncodeQuote(ss_cat) + "') ";
			if (Program.m_bSyncWithCloud)
			{
				sc += " ELSE ";

				sc += " UPDATE code_relations SET barcode = '" + barcode + "', supplier_code = '" + supplier_code + "', ";
				sc += " name = N'" + Program.EncodeQuote(name) + "', price1 = '" + price1 + "' ";
				sc += " WHERE 1=1 and code = '" + code + "'";

				sc += " IF NOT EXISTS(SELECT code FROM product WHERE code = '" + code + "')";
			}
			
			sc += " INSERT INTO product (code, supplier, supplier_code, name, price, cat, s_cat, ss_cat) ";
			sc += " VALUES (" + code + ", '', '" + Program.EncodeQuote(supplier_code) + "', N'" + Program.EncodeQuote(name) + "', " + price1;
			sc += ", N'" + Program.EncodeQuote(cat) + "', N'" + Program.EncodeQuote(s_cat) + "', N'" + Program.EncodeQuote(ss_cat) + "') ";
			if (Program.m_bSyncWithCloud)
			{
				sc += " ELSE ";

				sc += " UPDATE product SET supplier_code = '" + supplier_code + "', ";
				sc += " name = N'" + Program.EncodeQuote(name) + "', price = '" + price1 + "'";
				sc += " WHERE 1=1 and code = '"+code+"'";
			}
			sc += " INSERT INTO barcode(item_code, barcode)";
			sc += " VALUES('" + code + "','" + barcode + "')";

			sc += " IF NOT EXISTS(SELECT cat FROM catalog ";
			sc += " WHERE cat = N'" + Program.EncodeQuote(cat) + "' ";
			sc += " AND s_cat = N'" + Program.EncodeQuote(s_cat) + "' ";
			sc += " AND ss_cat = N'" + Program.EncodeQuote(ss_cat) + "') ";
			sc += " INSERT INTO catalog(cat, s_cat, ss_cat) VALUES(";
			sc += " N'" + Program.EncodeQuote(cat) + "' ";
			sc += ", N'" + Program.EncodeQuote(s_cat) + "' ";
			sc += ", N'" + Program.EncodeQuote(ss_cat) + "') ";

			sc += " Commit ";

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
				myCommand.Connection.Close();
				return false;
			}
			timer4.Interval = 100;
			timer4.Start();
			return true;
		}
		private void timer4_Tick(object sender, EventArgs e)
		{
			timer4.Stop();
			if (m_sTimer4Task == "check_qty_promotion")
			{
				m_bQtyEditing = true;
				CheckQtyPromotion(m_sTimer4Code, m_dTimer4Qty, m_nTimer4Index, "");
				m_bQtyEditing = false;
				m_sTimer4Task = "";
				return;
			}
			DoScanBarcode(false);
		}
		private string GetNextInvNumber()
		{
			int nRows = 0;
			if(dst.Tables["ni"] != null)
				dst.Tables["ni"].Clear();
			string sc = " SELECT TOP 1 id FROM invoice ORDER BY id DESC ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "ni");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				myConnection.Close();
			}
			string s = "";
			if(nRows > 0)
			{
				string id = dst.Tables["ni"].Rows[0]["id"].ToString();
				int nId = Program.MyIntParse(id) + 1;
				s = nId.ToString();
			}
			return s;
		}
		private void CreateNewSupplierCodeTB_Click(object sender, EventArgs e)
		{
			if (!this.numericpad.Visible)
				this.numericpad.Visible = true;
			m_lastFocused = this.CreateNewSupplierCodeTB;
		}
		private void CreateNewOtherNameTB_TextChanged(object sender, EventArgs e)
		{
		}
		private void DoSendTextToNVR(string message)
		{
			try
			{
				Int32 port = 5150;
				TcpClient client = new TcpClient(Program.m_sNVRIP, Program.MyIntParse(Program.m_sNVRPort));
				Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
				NetworkStream stream = client.GetStream();
				stream.Write(data, 0, data.Length);
				stream.Close();
				client.Close();
			}
			catch (ArgumentNullException e)
			{
				MessageBox.Show("NVR errro");
				Program.g_log.Err("send text:" + message + " to nvr failed\r\n e=" + e.ToString());
			}
			catch (SocketException e)
			{
				MessageBox.Show("NVR errro");
				Program.g_log.Err("send text:" + message + " to nvr failed, socket error\r\n e=" + e.ToString());
			}
		}
        public void ReceiveKeyboardKey(string s)
        {
            Control c = m_lastFocused;
            if (c == null)
                c = Program.FindFocusedControl(this);
            if (c == null)
                return;

            int p = 0;
            if (c is TextBox)
                p = ((TextBox)c).SelectionStart;
            else if (c is RichTextBox)
                p = ((RichTextBox)c).SelectionStart;
            string sf = c.Text.Substring(0, p);
            string sb = c.Text.Substring(p, c.Text.Length - p);
            string sr = "";
            int pNew = p;
            if (s == "del")
            {
                sr = sf.Substring(0, sf.Length - 1) + sb;
                pNew = p - 1;
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
		private void btnAppServer_Click(object sender, EventArgs e)
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory;
			string fn = sPath + "AppServer.exe";
			if (File.Exists(fn))
			{
				try
				{
					Process app = new Process();
					app.StartInfo.FileName = fn;
//					backup.StartInfo.Arguments = "-auto";
					app.Start();
				}
				catch (Exception e1)
				{
					Program.g_log.Log(e1.ToString());
				}
			}
			else
			{
				Program.MsgBox("App Server not installed");
			}
		}
        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            FormKeyboard fm = new FormKeyboard();
            fm.m_hForm1 = this;
            fm.m_x = this.Bounds.X + 20;
            fm.m_y = this.Bounds.Y + 405;
            fm.Show();
        }
        private void txtNote_Leave(object sender, EventArgs e)
        {
            m_lastFocused = (Control)sender;
        }
		private bool DoCreateVoucherService(string vip_barcode)
		{
			if (Program.m_sVoucherServiceURL == "")
			{
				Program.MsgBox("Cannot create voucher, Please enter voucher service URL");
				return false;
			}
			csVoucher csVoucher = new csVoucher(Program.m_sVoucherServiceURL);
			string sRet = "";
			try
			{
				sRet = csVoucher.CreateVoucher(vip_barcode, "", m_invoiceNumber);
			}
			catch (Exception e)
			{
				Program.ShowExp("", e);
				return false;
			}
			if (sRet.IndexOf("auth_key") >= 0)
			{
				string sTimeStamp = sRet.Substring(9, sRet.Length - 9);
				string auth_key = "eznz_voucher_auth_" + sTimeStamp + "_darcy_090918";
				string password = FormsAuthentication.HashPasswordForStoringInConfigFile(auth_key, "md5");
				sRet = csVoucher.CreateVoucher(vip_barcode, password, m_invoiceNumber); //challenge
			}
			if (sRet.Length <= 2)
				return false;
			if (sRet.Substring(0, 2) != "ok")
			{
				Program.MsgBox(sRet);
				return false;
			}
			string[] sa = sRet.Split(',');
			if (sa[1] == "NotEnoughPoints")
			{
				this.labelPoints.Text = sa[2];
				return false;
			}
			string barcode = sa[1];
			double dAmount = Program.MyDoubleParse(sa[2]);
			int nPoints = Program.MyIntParse(sa[3]);
			string sc = " UPDATE card SET points = " + nPoints + " WHERE barcode = '" + Program.EncodeQuote(MemberShipID.Text) + "' ";
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

			this.labelPoints.Text = nPoints.ToString();

			string s = @"
**********************
**********************
[b]Voucher[/b]
[b] @@volume[/b]
Staff Name : @@sales_name
Member Name : @@name
VIP # : @@member_id
Barcode : @@barcode
Volume : @@volume

@@COMPANY_NAME
Issue Date:@@date     
Expired Date:@@expire_date
************************
************************
			";
			s = s.Replace("@@volume", dAmount.ToString("c"));
			s = s.Replace("@@barcode", barcode);
			s = s.Replace("@@name", MemberShipName.Text);
			s = s.Replace("@@COMPANY_NAME", Program.m_sCompanyName);
			s = s.Replace("@@date", DateTime.Now.ToString("dd-MM-yyyy"));
			s = s.Replace("@@sales_name", MemberShipName.Text);
			s = s.Replace("@@member_id", vip_barcode);
			s = s.Replace("@@expire_date", DateTime.Now.AddMonths(2).ToString("dd-MM-yyyy"));
			m_sReceiptVoucher = s;
			m_sVoucherBarcode = barcode;
			m_sVoucherAmount = dAmount.ToString("c");
			return true;
		}
		private bool DoUseVoucherService(string v_barcode)
		{
			if (Program.m_sVoucherServiceURL == "")
			{
				Program.MsgBox("Cannot create voucher, Please enter voucher service URL");
				return false;
			}
			csVoucher csVoucher = new csVoucher(Program.m_sVoucherServiceURL);
			string sRet = "";
			try
			{
				sRet = csVoucher.UseVoucher(v_barcode, "");
			}
			catch (Exception e)
			{
				Program.ShowExp("", e);
				return false;
			}
			if (sRet.IndexOf("auth_key") >= 0)
			{
				string sTimeStamp = sRet.Substring(9, sRet.Length - 9);
				string auth_key = "eznz_voucher_auth_" + sTimeStamp + "_darcy_090918";
				string password = FormsAuthentication.HashPasswordForStoringInConfigFile(auth_key, "md5");
				sRet = csVoucher.UseVoucher(v_barcode, password); //challenge
			}
			if (sRet.Length <= 2)
				return false;
			if (sRet.Substring(0, 2) != "ok")
			{
				Program.MsgBox(sRet);
				return false;
			}
			string[] sa = sRet.Split(',');
			double dAmount = 0 - Program.MyDoubleParse(sa[1]);
			string name = (0 - dAmount).ToString("c") + " VOUCHER:(" + v_barcode.Substring(0, 2) + "***" + v_barcode.Substring(v_barcode.Length - 4) + ")";
			AddToCart(v_barcode, "1006", name, "", dAmount, 1, "0", false, "VOUCHER", 0, false, false);
			this.barcode.Text = "";
			this.barcode.Focus();
			return true;
		}
		
		private void timer5_Tick(object sender, EventArgs e)
		{
			timer5.Stop();
			btnCheckoutClose.PerformClick();
		}

		private void BtnFC_Click(object sender, EventArgs e)
		{
			if (Cash.Text == "" || Cash.Text == null)
				return;
			FormCurrency fmc = new FormCurrency();
			fmc.m_dOrderTotal = m_dOrderTotal;
			fmc.ShowDialog();
			if (fmc.m_dPaymentTotal != 0)
			{
				Cash.Text = fmc.m_dPaymentTotal.ToString();
				m_lastFocused = Cash;
				Cash.Select();
				Cash.Focus();
				return;
			}
		}
	}
}
