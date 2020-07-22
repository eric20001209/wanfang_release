using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormLogin : Form
	{
		public bool m_bLoggedin = false;
		public string m_sBranchId = "1";
		public string m_sSalesBarcode = "";
		public string m_sSalesId = "";
		public string m_sSalesName = "";
		private string m_sPassword = "";
		public bool m_bLanguage_en = false;
		public string m_sStationID = "0";
 
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private DataSet dst = new DataSet();
		public FormLogin()
		{
			InitializeComponent();
		}
		private void FormLogin_Load(object sender, EventArgs e)
		{
//			Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
//			version.Text = "v"+v.ToString();
			version.Text = "v" + "3.74";
			int boundWidth = Screen.PrimaryScreen.Bounds.Width;
			int boundHeight = Screen.PrimaryScreen.Bounds.Height;
			int x = boundWidth - this.Width;
			int y = boundHeight - this.Height;
			this.Location = new Point(x / 2, y / 2);
			barcode.Focus();
			this.barcode.UseSystemPasswordChar = true;
		 
			for (int n = 0; n <= 2; n++)
			{
				int s = 0;
				int en = 0;
				if (n == 0)
				{
					s = 7;
					en = 9;
				}
				if (n == 1)
				{
					s = 4;
					en = 6;
				}
				if (n == 2)
				{
					s = 1;
					en = 3;
				}
				for (int i = s; i <= en; i++)
				{
					Button NewTableButton = new Button();
					NewTableButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
					NewTableButton.BackgroundImageLayout = ImageLayout.Stretch;
					NewTableButton.Width = 85;
					NewTableButton.Height = 85;
					NewTableButton.AutoSize = false;
					NewTableButton.ForeColor = System.Drawing.Color.Black;
					NewTableButton.Text = i.ToString();
					NewTableButton.Font = new Font("Arial", 18, FontStyle.Bold);
					NewTableButton.Name = "contRightInsertOKButton";
					NewTableButton.Click += new EventHandler(contRightInsertOKButton_Click);
					flowLayoutPanel1.Controls.Add(NewTableButton);
				}
			}

			Button ZButton = new Button();
			ZButton.Text = "0";
			ZButton.Font = new Font("Arial", 18, FontStyle.Bold);
			ZButton.BackColor = System.Drawing.Color.Transparent;
			ZButton.ForeColor = System.Drawing.Color.Black;
			ZButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
			ZButton.BackgroundImageLayout = ImageLayout.Stretch;
			ZButton.AutoSize = false;
			ZButton.Width = 85;
			ZButton.Height = 85;
			ZButton.Name = "Doc";
			ZButton.Click += new EventHandler(Z_Click);
			flowLayoutPanel1.Controls.Add(ZButton);

			Button NewContButton = new Button();
			NewContButton.Text = "Back";
			NewContButton.Font = new Font("Arial", 18, FontStyle.Bold);
			NewContButton.BackColor = System.Drawing.Color.Transparent;
			NewContButton.ForeColor = System.Drawing.Color.Black;
			NewContButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
			NewContButton.BackgroundImageLayout = ImageLayout.Stretch;
			NewContButton.AutoSize = false;
			NewContButton.Width = 85;
			NewContButton.Height = 85;
			NewContButton.Name = "backspace";
			NewContButton.Click += new EventHandler(backspace_Click);
			flowLayoutPanel1.Controls.Add(NewContButton);

			Button EnterButton = new Button();
			EnterButton.Text = "Enter";
			EnterButton.Font = new Font("Arial", 18, FontStyle.Bold);
			EnterButton.BackColor = System.Drawing.Color.Transparent;
			EnterButton.ForeColor = System.Drawing.Color.Black;
			EnterButton.BackgroundImageLayout = ImageLayout.Stretch;
			EnterButton.BackgroundImage = QPOS2008.Properties.Resources.blue;
			EnterButton.AutoSize = false;
			EnterButton.Width = 85;
			EnterButton.Height = 85;
			EnterButton.Name = "Enter";
			EnterButton.Click += new EventHandler(btnLogin_Click);
			flowLayoutPanel1.Controls.Add(EnterButton);
		}
		private void backspace_Click(object sender, EventArgs e)
		{
			Button CurrentBackSpace = (Button)sender;
			string currentDigit = this.barcode.Text;
			if (currentDigit != "")
			{
				currentDigit = currentDigit.Substring(0, currentDigit.Length - 1);
				this.barcode.Text = currentDigit;
			}
		}
		private void contRightInsertOKButton_Click(object sender, EventArgs e)
		{
			Button CurrentNumber = (Button)sender;
			this.barcode.Text = this.barcode.Text + CurrentNumber.Text;
		}
		private void Z_Click(object sender, EventArgs e)
		{
			Button CurrentZ = (Button)sender;
			this.barcode.Text = this.barcode.Text + CurrentZ.Text;
		}
		private void barcode_KeyDown(object sender, KeyEventArgs e)
		{
			pass_KeyDown(sender, e);
		}
		private void pass_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 77)
			{
				FormLogin_KeyDown(sender, e);
				return;
			}
			switch (e.KeyCode)
			{
				case Keys.Return:
					FormLogin_KeyDown(sender, e);
					return;
				default:
					break;

			}
			if (m_sSalesId == "")
			{
				barcode.Focus();
				return;
			}
			Program.m_sSalesId = m_sBranchId;
			m_bLoggedin = true;
			Close();
		}
		private bool ScanSalesBarcode()
		{
			if (barcode.Text == "")
				return false;
			if (barcode.Text == "1769394")
			{
				this.Close();
				return false;
			}
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			string sc = " SELECT *, b.name AS branch_name ";
			sc += " FROM card c ";
			sc += " LEFT OUTER JOIN branch b ON b.id = c.our_branch ";
			sc += " WHERE c.barcode = '" + Program.EncodeQuote(barcode.Text) + "' ";
			sc += " AND c.type = 4 ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "card");
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return false;
			}
			if (dst.Tables["card"].Rows.Count <= 0)
			{
				MessageBox.Show("Sales not found", "Login Error");
				barcode.Focus();
				return false;
			}
			
			DataRow dr = dst.Tables["card"].Rows[0];
			int nAccessLevel = Program.MyIntParse(dr["access_level"].ToString());
			if(nAccessLevel >= 10)
			{
				Program.m_bAccessAdmin = true;
			}
			else
			{
				Program.m_bAccessAdmin = false;
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
			}			
			m_sSalesBarcode = barcode.Text;
			m_sBranchId = dr["our_branch"].ToString();
			m_sSalesId = dr["id"].ToString();
			m_sSalesName = dr["name"].ToString();
			m_sPassword = dr["password"].ToString();
			if(dr["language"].ToString() == "2")
				m_bLanguage_en = true;
			Program.m_sSalesId = m_sSalesId;
			Program.OpenTillDate(int.Parse(m_sStationID));
			Program.m_sBranchName = dr["branch_name"].ToString();
			return true;
		}
		private void buttonLogin_Click(object sender, EventArgs e)
		{
			Close();
		}
		private bool DoLogin()
		{
			if (Program.md5(pass.Text).ToUpper() != m_sPassword.ToUpper())
			{
				MessageBox.Show("Incorrect password", "Login Error");
				pass.Select();
				pass.Focus();
				return false;
			}
			else
			{
				m_bLoggedin = true;
				Close();
			}
		
			return true;
		}
		private void FormLogin_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 17 || e.KeyValue == 77)
			{
				if (ScanSalesBarcode())
				{
					m_bLoggedin = true;
					Close();
				}
				else
				{
					barcode.Text = "";
					barcode.Focus();
				}
				return;
			}
			switch (e.KeyCode)
			{
				case Keys.F12:
					if (e.Control && e.Shift && e.Alt)
					{
						FormConfig fm = new FormConfig();
						fm.ShowDialog();
//						myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
					}
					break;
				case Keys.Return:
					if (ScanSalesBarcode())
					{
						m_bLoggedin = true;
						Close();
					}
					else
					{
						barcode.Text = "";
						barcode.Focus();
					}
					break;
				default:
					break;
			}
		}
		private void btnLogin_Click(object sender, EventArgs e)
		{
			if (ScanSalesBarcode())
			{
				m_bLoggedin = true;
				Close();
			}
			else
			{
				barcode.Text = "";
				barcode.Focus();
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void label4_Click(object sender, EventArgs e)
		{

		}
	}
}
