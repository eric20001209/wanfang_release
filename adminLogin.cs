using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using System.Data.SqlClient;

namespace QPOS2008
{
	public partial class adminLogin : Form
	{
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private DataSet dst = new DataSet();
		public double dOldTotal = 0;
		public double dNewTotal = 0;
		private string sBarcode = "";
		public bool m_bPass= false;
		public bool m_bChangeSales = false;
//		public bool m_bAdmin = false;
		public string cmd = "";
		public string m_sSalesId = "";
		public string m_sSalesName = "";
        public string m_sText = "";

		public adminLogin()
		{
			InitializeComponent();
		}
		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 17)
			{
				if (CheckAccess())
				{
					m_bPass = true;
					this.Close();

				}
			}
			switch (e.KeyCode)
			{
				case Keys.Return:
					if (CheckAccess())
					{
						m_bPass = true;
						this.Close();

					}
					break;
				case Keys.Escape:
					this.Close();
					break;
				default:
					break;
			}
		}
		private bool CheckAccess()
		{
			sBarcode = this.textBoxBarcode.Text.ToString();
			if(sBarcode.Trim() == "")
				return false;
			string sc = " SELECT * ";
			sc += " FROM card WHERE barcode = '" + sBarcode + "' ";
			sc += " AND type = 4";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "adjustLevel");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return false;
			}
			if (dst.Tables["adjustLevel"].Rows.Count <= 0)
			{
				MessageBox.Show("Sorry, You are unauthorised !");
				this.textBoxBarcode.Text = "";
				this.textBoxBarcode.Focus();
				return false;
			}
			if(m_bChangeSales)
			{
				m_bPass = true;
	//			return true;
			}
			DataRow dr = dst.Tables["adjustLevel"].Rows[0];
			string names = dr["name"].ToString();
			m_sSalesId = dr["id"].ToString();
			m_sSalesName = dr["name"].ToString();
			int iAccessLevel = Program.MyIntParse(dr["access_level"].ToString());
			if(iAccessLevel >= 10)
			{
				m_bPass = true;
				Program.m_bAccessAdmin = true;
				return true;
			}
			else
			{
				Program.m_bAccessAdmin = false;
			}
			
			Program.m_bAccessDeleteItem = Program.MyBooleanParse(dr["access_delete_item"].ToString());
			Program.m_bAccessRefund = Program.MyBooleanParse(dr["access_refund"].ToString());
			Program.m_bAccessDiscount = Program.MyBooleanParse(dr["access_discount"].ToString());
			Program.m_bAccessCashdraw = Program.MyBooleanParse(dr["access_cashdraw"].ToString());
			Program.m_bAccessXTotal = Program.MyBooleanParse(dr["access_x_total"].ToString());
			Program.m_bAccessReport = Program.MyBooleanParse(dr["access_report"].ToString());
			Program.m_bAccessVipPayment = Program.MyBooleanParse(dr["access_vip_payment"].ToString());
			Program.m_bAccessProduct = Program.MyBooleanParse(dr["access_product"].ToString());
			Program.m_bAccessStock = Program.MyBooleanParse(dr["access_stock"].ToString());
			Program.m_bAccessSetting = Program.MyBooleanParse(dr["access_setting"].ToString());
			Program.m_bAccessDatabase = Program.MyBooleanParse(dr["access_database"].ToString());
			Program.m_bAccessAdminZone = Program.MyBooleanParse(dr["access_admin_zone"].ToString());
				
			bool bDeleteItem = Program.MyBooleanParse(dr["access_delete_item"].ToString());
			bool bRefund = Program.MyBooleanParse(dr["access_refund"].ToString());
			bool bDiscount = Program.MyBooleanParse(dr["access_discount"].ToString());
			bool bCashdraw = Program.MyBooleanParse(dr["access_cashdraw"].ToString());
			bool bXTotal = Program.MyBooleanParse(dr["access_x_total"].ToString());
			bool bReport = Program.MyBooleanParse(dr["access_report"].ToString());
			bool bVipPayment = Program.MyBooleanParse(dr["access_vip_payment"].ToString());
			bool bProduct = Program.MyBooleanParse(dr["access_product"].ToString());
			bool bStock = Program.MyBooleanParse(dr["access_stock"].ToString());
			bool bSetting = Program.MyBooleanParse(dr["access_setting"].ToString());
			bool bDatabase = Program.MyBooleanParse(dr["access_database"].ToString());
			bool bAccessAdminZone = Program.MyBooleanParse(dr["access_admin_zone"].ToString());

			if (cmd == "id_check" && bProduct)
				m_bPass = true;
			if (cmd == "delete item" && bDeleteItem)
				m_bPass = true;
			else if (cmd == "refund" && bRefund)
				m_bPass = true;
			else if (cmd == "discount" && bDiscount)
				m_bPass = true;
			else if (cmd == "open cashdraw" && bCashdraw)
				m_bPass = true;
			else if (cmd == "xtotal" && bXTotal)
				m_bPass = true;
			else if (cmd == "report" && bReport)
				m_bPass = true;
			else if (cmd == "vip payment" && bVipPayment)
				m_bPass = true;
			else if (cmd == "product edit" && bProduct)
				m_bPass = true;
			else if (cmd == "stock control" && bStock)
				m_bPass = true;
			else if (cmd == "setting" && bSetting)
				m_bPass = true;
			else if (cmd == "database" && bDatabase)
				m_bPass = true;
			else if (cmd == "adminzone" && bAccessAdminZone)
				m_bPass = true;	
			
			else if( cmd == "" && iAccessLevel >= 10)
				m_bPass = true;
					
			if(!m_bPass)
			{
				MessageBox.Show("Sorry, You are unauthorised !");
				this.textBoxBarcode.Text = "";
				this.textBoxBarcode.Focus();
				return false;
			}
			return true;
		}
		private void adminLogin_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			this.Location = new Point(0, 0);
			this.BringToFront();
			ShowPinpad();
		}
		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (CheckAccess())
			{
				m_bPass = true;
				Close();
			}
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			m_bPass = false;
			this.Close();
		}
		private void ShowPinpad()
		{
			//Keyboard frmpinpad = new Keyboard();
			numbericpad frmpinpad = new numbericpad();
			string pw = "1";
			frmpinpad.m_pw = pw;
            frmpinpad.m_sTitle = m_sText;
//			frmpinpad.Location = new Point(0, 0);
//			frmpinpad.m_sText = "Key in the Password";
			frmpinpad.ShowDialog();
			this.textBoxBarcode.Text = frmpinpad.m_sAdmount;
			if (CheckAccess())
			{
				m_bPass = true;
				this.Close();
			}
			this.Close();
		}
		private void textBoxBarcode_Click(object sender, EventArgs e)
		{
			ShowPinpad();
		}
	}
}
