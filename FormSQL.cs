using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace QPOS2008
{
	public partial class FormSQL : Form
	{
		private DataSet ds = new DataSet();
		private SqlDataAdapter myAdapter;
		
		public FormSQL()
		{
			InitializeComponent();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			if(DoSaveConfig())
				this.Close();
		}
		private void FormSQL_Load(object sender, EventArgs e)
		{
			if(Program.GetSqlConfig())
			{
				txtServer1.Text = Program.m_sServer1;
				txtDB1.Text = Program.m_sDBName1;
				txtUser1.Text = Program.m_sUser1;
				txtPwd1.Text = Program.m_sPass1;
				txtServer2.Text = Program.m_sServer2;
				txtDB2.Text = Program.m_sDBName2;
				txtUser2.Text = Program.m_sUser2;
				txtPwd2.Text = Program.m_sPass2;
				if(Program.m_bServer2AsBackup)
					rbAsBackup.Checked = true;
				else
					rbSimultaneously.Checked = true;
			}
			DoUpdateStatus();
		}
		private bool TestConnection(System.Data.SqlClient.SqlConnection myConnection)
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
//		public static string m_sDataSource2 = ";data source=192.168.1.66;";
//		public static string m_sSecurityString = "User id=eznz;Password=9seqxtf7;Integrated Security=false;Connect Timeout=30";
		private void btnTest1_Click(object sender, EventArgs e)
		{
			lblLastAccess1.Text = "";
			lblLastIndex1.Text = "";
			lblInvNo1.Text = "";
			lblItemCode1.Text = "";
			Program.m_sServer1 = txtServer1.Text;
			Program.m_sDBName1 = txtDB1.Text;
			Program.m_sUser1 = txtUser1.Text;
			Program.m_sPass1 = txtPwd1.Text;
			if (Program.m_sServer1 == "")
			{
				DoSaveConfig();
				return;
			}
			string sSecurityString = "User id=" + Program.m_sUser1 + ";Password=" + Program.m_sPass1 + ";Integrated Security=false;Connect Timeout=15";
			if(Program.m_sUser1 == "")
				sSecurityString = "Integrated Security=SSPI;";
			System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection("Initial Catalog=" + Program.m_sDBName1 + ";data source=" + Program.m_sServer1 + ";" + sSecurityString);
			if(TestConnection(myConnection))
			{
				Program.myConnection1 = myConnection;
				Program.m_bSql1Good = true;
				DoSaveConfig();
				Program.MsgBox("Database 1 successfully connected, settings saved");
			}
			else
			{
				Program.m_bSql1Good = false;
//Program.myConnection1 = myConnection; //test
				MessageBox.Show("Error connectiong to SQL Server 1, please check your settings and try again 1.\r\n", "Database Error");
			}
			DoUpdateStatus();
		}
		private void btnTest2_Click(object sender, EventArgs e)
		{
			lblLastAccess2.Text = "";
			lblLastIndex2.Text = "";
			lblInvNo2.Text = "";
			lblItemCode2.Text = "";
			Program.m_sServer2 = txtServer2.Text;
			Program.m_sDBName2 = txtDB2.Text;
			Program.m_sUser2 = txtUser2.Text;
			Program.m_sPass2 = txtPwd2.Text;
			if(Program.m_sServer2 == "")
			{
				DoSaveConfig();
				return;
			}
			string sSecurityString = "User id=" + Program.m_sUser2 + ";Password=" + Program.m_sPass2 + ";Integrated Security=false;Connect Timeout=15";
			if (Program.m_sUser2 == "")
				sSecurityString = "Integrated Security=SSPI;";
			System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection("Initial Catalog=" + Program.m_sDBName2 + ";data source=" + Program.m_sServer2 + ";" + sSecurityString);
			if (TestConnection(myConnection))
			{
				Program.myConnection2 = myConnection;
				Program.m_bSql2Good = true;
				DoSaveConfig();
				Program.MsgBox("Database 2 successfully connected, settings saved");
			}
			else
			{
				Program.m_bSql2Good = false;
//Program.myConnection2 = myConnection; //test
				MessageBox.Show("Error connectiong to SQL Server 2, please check your settings and try again 1.\r\n", "Database Error");
			}			
			DoUpdateStatus();
		}
		private bool DoSaveConfig()
		{
			string s = "";
			s += "server1=" + txtServer1.Text + "\r\n";
			s += "name1=" + txtDB1.Text + "\r\n";
			s += "user1=" + txtUser1.Text + "\r\n";
			s += "pass1=" + txtPwd1.Text + "\r\n";
			s += "server2=" + txtServer2.Text + "\r\n";
			s += "name2=" + txtDB2.Text + "\r\n";
			s += "user2=" + txtUser2.Text + "\r\n";
			s += "pass2=" + txtPwd2.Text + "\r\n";
			if(rbAsBackup.Checked)
				s += "server2_as_backup=1\r\n";
			else
				s += "server2_as_backcp=0\r\n";
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "sql.ini";
			if(File.Exists(sPath))
				File.Delete(sPath);
			try
			{
				File.AppendAllText(sPath, s);
			}
			catch (Exception e)
			{
				Program.MsgBox(e.ToString());
				return false;
			}
			return true;
		}
		private bool DoUpdateStatus()
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
			if(Program.m_sServer1 == "")
			{
				btnStatus1.BackColor = Color.Black;
			}
			else
			{
				System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection("Initial Catalog=" + Program.m_sDBName1 + ";data source=" + Program.m_sServer1 + ";" + sSecurityString);
				if (TestConnection(myConnection))
				{
					btnStatus1.BackColor = Color.Green;
					sAccess1 = Program.GetSiteSettingWithConnection(myConnection, "server_last_access_time", false);
					sIndex1 = Program.GetSiteSettingWithConnection(myConnection, "server_last_update_index", true);
					sInv1 = Program.GetSqlInvNum(myConnection);
					sCode1 = Program.GetSqlLastCode(myConnection);
					Program.myConnection1 = myConnection;
				}
				else
				{
					btnStatus1.BackColor = Color.Red;
				}
			}			

			sSecurityString = "User id=" + Program.m_sUser2 + ";Password=" + Program.m_sPass2 + ";Integrated Security=false;Connect Timeout=3";
			if (Program.m_sUser2 == "")
				sSecurityString = "Integrated Security=SSPI;";
			if (Program.m_sServer2 == "")
			{
				btnStatus2.BackColor = Color.Black;
			}
			else
			{
				System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection("Initial Catalog=" + Program.m_sDBName2 + ";data source=" + Program.m_sServer2 + ";" + sSecurityString);
				if (TestConnection(myConnection))
				{
					btnStatus2.BackColor = Color.Green;
					sAccess2 = Program.GetSiteSettingWithConnection(myConnection, "server_last_access_time", false);
					sIndex2 = Program.GetSiteSettingWithConnection(myConnection, "server_last_update_index", true);
					sInv2 = Program.GetSqlInvNum(myConnection);
					sCode2 = Program.GetSqlLastCode(myConnection);
					Program.myConnection2 = myConnection;
				}
				else
				{
					btnStatus2.BackColor = Color.Red;
				}
			}			
			
			lblLastAccess1.Text = sAccess1;
//			lblLastIndex1.Text = sIndex1;
			lblInvNo1.Text = sInv1;
			lblItemCode1.Text = sCode1;
			lblLastAccess2.Text = sAccess2;
//			lblLastIndex2.Text = sIndex2;
			lblInvNo2.Text = sInv2;
			lblItemCode2.Text = sCode2;
/*			
			if(sIndex1 != sIndex2)
				btnIndex.BackColor = Color.Red;
			else
				btnIndex.BackColor = Color.Green;
*/			
			if(sInv1 != sInv2)
				btnInv.BackColor = Color.Red;
			else
				btnInv.BackColor = Color.Green;
			
			if(sCode1 != sCode2)
				btnCode.BackColor = Color.Red;
			else
				btnCode.BackColor = Color.Green;	
/*			
			if(sIndex1 != sIndex2)
			{
				if(Program.MyIntParse(sIndex1) >= Program.MyIntParse(sIndex2))
				{
					btnStatus1.BackColor = Color.Green;
					if(btnStatus2.BackColor == Color.Green)
						btnStatus2.BackColor = Color.Orange;
				}
				else
				{
					if (btnStatus1.BackColor == Color.Green)
						btnStatus1.BackColor = Color.Orange;
					btnStatus2.BackColor = Color.Green;
				}
			}
*/ 
			if(sInv1 != sInv2)
			{
				if(Program.MyIntParse(sInv1) >= Program.MyIntParse(sInv2))
				{
					btnStatus1.BackColor = Color.Green;
					if (btnStatus2.BackColor == Color.Green)
						btnStatus2.BackColor = Color.Orange;
				}
				else
				{
					if (btnStatus1.BackColor == Color.Green)
						btnStatus1.BackColor = Color.Orange;
					btnStatus2.BackColor = Color.Green;
				}
			}
			else if(sCode1 != sCode2)
			{
				if(Program.MyIntParse(sCode1) >= Program.MyIntParse(sCode2))
				{
					btnStatus1.BackColor = Color.Green;
					if (btnStatus2.BackColor == Color.Green)
						btnStatus2.BackColor = Color.Orange;
				}
				else
				{
					if (btnStatus1.BackColor == Color.Green)
						btnStatus1.BackColor = Color.Orange;
					btnStatus2.BackColor = Color.Green;
				}
			}	
			return true;
		}
	}
}
