using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace QPOS2008
{
	public partial class FormConfig : Form
	{
		private SqlConnection myConnection;
//		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet ds = new DataSet();
		private DataSet dst = new DataSet();
		//private bool bVoucher_enable = false;
		public FormConfig()
		{
			InitializeComponent();
		}

		private void FormConfig_Load(object sender, EventArgs e)
		{
			this.Server.Text = Program.m_sServer;
			this.server2.Text = Program.m_sServer2;
			this.User.Text = Program.m_sUser;
			this.Password.Text = Program.m_sPass;
			this.Catalog.Text = Program.m_sCompanyName;
			this.PrinterName.Text = Program.m_sPrinterName;
			this.PaperWidth.Text = Program.m_nPaperWidth.ToString();
			this.fontName.Text = Program.m_sFontName;
			this.fontSize.Text = Program.m_sFontSize;
			this.fontStyle.Text = Program.m_sFontStyle;
			this.ScaleName.Text = Program.m_sScaleName;
			this.ScannerName.Text = Program.m_sScannerName;
			this.textStationID.Text = Program.m_sStationID;
			this.textTimeout.Text = Program.m_nTimeout.ToString();
			this.textBoxAdUrl.Text = Program.m_sAdUrl;
			this.cbSerialDevicePort.Text = Program.m_sSerialDevicePort;

			this.chkCustomerDis.Checked = bool.Parse(Program.m_sDMoniter.ToString());

			this.txtgroceryitem.Text = Program.m_sgroceryitem;
			this.txtgroceryweight.Text = Program.m_sgroceryweight;
			this.txtvoidscale.Text = Program.m_svoidscale;
			this.txtgroceryitemaddup.Text = Program.m_sgroceryitemaddup;
			this.txtgroceryweightitemaddup.Text = Program.m_sgroceryweightitemaddup;
			this.txtvoiddisc.Text = Program.m_svoiddis;

			this.chkEnableEftpos.Checked = bool.Parse(Program.m_bEnableEftpos.ToString());

			this.eftposCom.Text = Program.m_sEftposCom;
			this.txtVoucherServiceURL.Text = Program.m_sVoucherServiceURL;
			this.txtelog.Text = Program.m_sEftposLogDay;
			this.chkelog.Checked = bool.Parse(Program.m_bEnableEftposLog.ToString());
			this.rdbmonitorr.Checked = bool.Parse(Program.m_bRegularMonitor.ToString());
			this.rdbmonitors.Checked = bool.Parse(Program.m_bSmallMonitor.ToString());
			this.chkEnableVipDis.Checked = bool.Parse(Program.m_bEnableVipDis.ToString());

			this.ckbSpecialDis.Checked = bool.Parse(Program.m_bEnableSpecialDis.ToString());

			this.ckbBackupData.Checked = bool.Parse(Program.m_backupdata.ToString());
			this.tbBackup.Text = Program.m_backuproot;
			this.ckbSync.Checked = bool.Parse(Program.m_sync.ToString());
			this.tbSyncRoot.Text = Program.m_syncroot;
		
			this.txtiis.Text = Program.m_sIISServer;
			this.cbEftposType.Text = Program.m_eftposType;
			this.cbDebugLog.Text = Program.m_debugLogType;
			this.Server.Focus();
			
			if (Program.m_bServer2)
			{
				this.Hide();
				FormMSG confirm = new FormMSG();
				confirm.m_sMsg = "Server 1 Down ";
				confirm.m_sconfirm = "1";
				confirm.m_showconf = "1";
				confirm.ShowDialog();
				if (confirm.m_sconfirm == "1")
				{
					SaveSettings();
					this.Close();
				}
				else if (confirm.m_showconf == "1")
				{
					this.Show();
				}
			}
		}
		private void buttonTest_Click(object sender, EventArgs e)
		{
			string server = this.Server.Text;
			string user = this.User.Text;
			string pass = this.Password.Text;
			string catalog = this.Catalog.Text;
			string securityString = "User id=" + user + ";Password=" + pass + ";Integrated Security=false;Connect Timeout=10";
			if (user == "")
				securityString = " Integrated Security=SSPI;";
//			SqlConnection myConnection = new SqlConnection("Initial Catalog=" + catalog + ";data source=" + server + ";" + securityString);
			this.buttonTest.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				myConnection.Open();
			}
			catch
			{
				
				MessageBox.Show("Error connectiong to SQL Server, please check your settings and try again .\r\n", "Database Error");
				this.buttonTest.Enabled = true;
//				this.server2.Text = this.Server.Text;
//				Program.m_sServerBkIp = this.server2.Text;
//				this.Server.Text = Program.m_sServerBkIp;
				Cursor.Current = Cursors.Default;
				//return;
			}
			finally
			{
				myConnection.Close();
			}
			Cursor.Current = Cursors.Default;
			this.buttonTest.Enabled = true;
			SaveSettings();
			MessageBox.Show("Database successfully connected, settings are saved", "Good News");
		}
		private bool SaveSettings()
		{
			string station_id = this.textStationID.Text;
			int nid = Program.MyIntParse(station_id);
			if(nid <= 0 || nid.ToString() != station_id)
			{
				MessageBox.Show("Incorrect Station ID, must be an integer and greater than 0");
				this.textStationID.Focus();
				return false;
			}
			int nTimeout = Program.MyIntParse(this.textTimeout.Text);
			if(nTimeout <= 0 || nTimeout > 3000)
			{
				MessageBox.Show("Incorrect timeout value, must between 0 and 3000");
				this.textTimeout.Focus();
				return false;
			}
			
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_server", this.Server.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "iis_server", this.txtiis.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_server_2", this.server2.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_user", this.User.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_pass", this.Password.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "db_catalog", this.Catalog.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "printer_name", this.PrinterName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "paper_width", Program.MyIntParse(this.PaperWidth.Text).ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "font_name", this.fontName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "font_size", this.fontSize.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "font_style", this.fontStyle.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "scale_name", this.ScaleName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "scanner_name", this.ScannerName.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "station_id", station_id);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "opos_timeout", nTimeout.ToString());
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "ad_url", this.textBoxAdUrl.Text);
//			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "ad_url", this.cbSerialDevicePort.Text);

//          Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "dMonitor", this.textBoxDmoniter.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "dMonitor", this.chkCustomerDis.Checked);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryitem", this.txtgroceryitem.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryweight", this.txtgroceryweight.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryitemaddup", this.txtgroceryitemaddup.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "groceryweightitemaddup", this.txtgroceryweightitemaddup.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "voidscale", this.txtvoidscale.Text);
 //           Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "barcodewithprice1", this.BarcodeStarttxt.Text);
	//        Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "barcodewithprice2", this.txtbarcodewithprice2.Text);
	//        Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "barcodewithprice3", this.txtbarcodewithprice3.Text);
	//        Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "barcodelength1", this.txtbarcodelength1.Text);
  //          Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "barcodelength2", this.pricestarttxt.Text);
  //          Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "barcodelength3", this.dicimaltxt.Text);
	 //       Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "pricelength1", this.txtpricelength1.Text);
	 //       Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "pricelength2", this.txtpricelength2.Text);
	 //       Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "pricelength3", this.txtpricelength3.Text);
	  //      Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "point1", this.txtpoint1.Text);
	 //       Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "point2", this.txtpoint2.Text);
	 //       Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "point3", this.txtpoint3.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "voiddis", this.txtvoiddisc.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enableeftpos", this.chkEnableEftpos.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "eftposCom", this.eftposCom.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "voucher_service_url", this.txtVoucherServiceURL.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "enable_eftpos_log", this.chkelog.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "eftpos_log_day", this.txtelog.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "regular_monitor", this.rdbmonitorr.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "small_monitor", this.rdbmonitors.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "vip_discount", this.chkEnableVipDis.Checked);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "special_discount", this.ckbSpecialDis.Checked);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "backup_Database", this.ckbBackupData.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "backup_root", this.tbBackup.Text);


			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "sync", this.ckbSync.Checked);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "sync_root", this.tbSyncRoot.Text);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "eftpos_type", this.cbEftposType.Text);
			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "debug_log_type", this.cbDebugLog.Text);

			Registry.SetValue("HKEY_CURRENT_USER\\" + Program.m_sRegKey + "\\", "serialDevicePort", this.cbSerialDevicePort.Text);

			if (this.User.Text == "" && this.Catalog.Text == "")
			{
				this.Close();
				return false;
			}
			Program.GetSettings(); //apply new settings
		  
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			string sc = " IF NOT EXISTS (SELECT id FROM till WHERE station_id = " + station_id + ") INSERT INTO till (station_id) VALUES(" + station_id + ") ";
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
		private void buttonSave_Click(object sender, EventArgs e)
		{
			//lbl_holded lh = new lbl_holded();
			if (SaveSettings())
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = " Setting Saved ";
				fm.ShowDialog();
				Program.m_formconfigshow = false;
				this.Close();
			}
		}
		private void buttonHotkey_Click(object sender, EventArgs e)
		{
			FormKey fm = new FormKey();
			fm.Show();
			//lbl_holded lh = new lbl_holded();
			Program.m_formconfigshow = false;
			this.Close();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			string server = this.Server.Text;
			string user = this.User.Text;
			string pass = this.Password.Text;
			string catalog = this.Catalog.Text;
			string securityString = "User id=" + user + ";Password=" + pass + ";Integrated Security=false;Connect Timeout=10";
			if (user == "")
				securityString = " Integrated Security=SSPI;";
//			SqlConnection myConnection = new SqlConnection("Initial Catalog=" + catalog + ";data source=" + server + ";" + securityString);
			this.buttonTest.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				myConnection.Open();
			}
			catch
			{
				MessageBox.Show("Error connectiong to SQL Server, please check your settings and try again.\r\n", "Database Error");
				this.buttonTest.Enabled = true;
				Cursor.Current = Cursors.Default;
				return;
			}
			finally
			{
				myConnection.Close();
			}
			Cursor.Current = Cursors.Default;
			this.buttonTest.Enabled = true;
			SaveSettings();
			MessageBox.Show("Database successfully connected, settings are saved", "Good News");
		}
		private void buttonTest_Click_1(object sender, EventArgs e)
		{
			string server = this.Server.Text;
			string user = this.User.Text;
			string pass = this.Password.Text;
			string catalog = this.Catalog.Text;
			string securityString = "User id=" + user + ";Password=" + pass + ";Integrated Security=false;Connect Timeout=10";
			if (user == "")
				securityString = " Integrated Security=SSPI;";
//			SqlConnection myConnection = new SqlConnection("Initial Catalog=" + catalog + ";data source=" + server + ";" + securityString);
			this.buttonTest.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
//            try
//            {
//                myConnection.Open();
//            }
//            catch
//            {
//                MessageBox.Show("Error connectiong to SQL Server, please check your settings and try again.\r\n", "Database Error");
//                this.buttonTest.Enabled = true;
//                Program.m_sServerBkIp = this.Server.Text;
////				this.Server.Text =this.server2.Text;
////				this.server2.Text = Program.m_sServerBkIp;
////				SaveSettings();
//                Cursor.Current = Cursors.Default;
//                return;
//            }
//            finally
//            {
//                myConnection.Close();
//            }
			Cursor.Current = Cursors.Default;
			this.buttonTest.Enabled = true;
			SaveSettings();
			MessageBox.Show("Database successfully connected, settings are saved", "Good News");
		}
		private void button1_Click_1(object sender, EventArgs e)
		{
			//lbl_holded lh = new lbl_holded();
			if (SaveSettings())
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = " Setting Saved ";
				fm.ShowDialog();
				Program.m_formconfigshow = false;
				this.Close();
			}
		}
		private void button2_Click(object sender, EventArgs e)
		{
			//lbl_holded lh = new lbl_holded();
			if (SaveSettings())
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = " Setting Saved ";
				fm.ShowDialog();
				Program.m_formconfigshow = false;
				this.Close();
			}
		}
		private void button3_Click(object sender, EventArgs e)
		{
			//lbl_holded lh = new lbl_holded();
			if (SaveSettings())
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = " Setting Saved ";
				fm.ShowDialog();
				Program.m_formconfigshow = false;
				this.Close();
			}
		}
		private void button4_Click(object sender, EventArgs e)
		{
			if (SaveSettings())
			{
				FormMSG fm = new FormMSG();
				fm.btnYes.Visible = false;
				fm.btnNo.Visible = false;
				fm.m_sMsg = " Setting Saved ";
				fm.ShowDialog();
				Program.m_formconfigshow = false;
				this.Close();
			}
		}
		private void richTextBox2_TextChanged(object sender, EventArgs e)
		{
		}
		private void textBox2_TextChanged(object sender, EventArgs e)
		{
		}
		private void label19_Click(object sender, EventArgs e)
		{
		}
		private void groupBox5_Enter(object sender, EventArgs e)
		{
		}
		private void label20_Click(object sender, EventArgs e)
		{
		}
		private void label18_Click(object sender, EventArgs e)
		{
		}
		private void btnDebug_Click(object sender, EventArgs e)
		{
			FormDebug fm = new FormDebug();
			fm.Show();
		}
		private void btnConfigSql_Click(object sender, EventArgs e)
		{
			FormSQL fm = new FormSQL();
			fm.ShowDialog();
		}
	}
}
