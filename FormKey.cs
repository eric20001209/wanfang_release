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
	public partial class FormKey : Form
	{
		private SqlConnection myConnection;
//		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		
		public FormKey()
		{
			InitializeComponent();
		}

		private void FormKey_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			if(Program.m_sSalesId == "")
			{
				vKeys.Columns["cc_code_name"].Visible = false;
				vKeys.Columns["cc_name"].Width += 100;
				vKeys.AllowUserToAddRows = false;
			}
			RefreshHotKey();
		}
		private void RefreshHotKey()
		{
			Program.GetHotkey();
			this.vKeys.Rows.Clear();
			for (int i = 0; i < Program.m_dtHotkey.Rows.Count; i++)
			{
				DataRow dr = Program.m_dtHotkey.Rows[i];
				string id = dr["id"].ToString();
				string name = dr["name"].ToString();
				string code_name = dr["code_name"].ToString();
				string key_code = dr["key_code"].ToString();
				string key_desc = dr["key_desc"].ToString();
				bool bCtrl = Program.MyBooleanParse(dr["ctrl"].ToString());
				bool bAlt = Program.MyBooleanParse(dr["alt"].ToString());
				bool bShift = Program.MyBooleanParse(dr["shift"].ToString());

				string[] sdr = { id, name, code_name, key_code, key_desc, "Reset" };
				this.vKeys.Rows.Add(sdr);
			}
		}
		private bool DoSaveHotKey(int nIndex)
		{
			if(vKeys.Rows[nIndex].Cells["cc_id"].Value == null)
			{
				if(DoAddNewHotKey(nIndex))
				{
					RefreshHotKey();
					return true;
				}
				return false;
			}
			string id = vKeys.Rows[nIndex].Cells["cc_id"].Value.ToString();
			if (vKeys.Rows[nIndex].Cells["cc_name"].Value == null)
			{
				MessageBox.Show("Name cannot be blank");
				return false;
			}
			if (vKeys.Rows[nIndex].Cells["cc_code_name"].Value == null)
			{
				MessageBox.Show("CodeName cannot be blank");
				return false;
			}
			string name = vKeys.Rows[nIndex].Cells["cc_name"].Value.ToString();
			string code_name = vKeys.Rows[nIndex].Cells["cc_code_name"].Value.ToString();
			string sc = " UPDATE hotkey SET ";
			sc += " name = '" + Program.EncodeQuote(name) + "' ";
			sc += ", code_name = '" + Program.EncodeQuote(code_name) + "' ";
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
				Program.ShowExp(sc, e);
				return false;
			}
			RefreshHotKey();
			return true;
		}
		private bool DoAddNewHotKey(int nIndex)
		{
			if(vKeys.Rows[nIndex].Cells["cc_name"].Value == null)
			{
				MessageBox.Show("Name cannot be blank");
				return false;
			}
			if (vKeys.Rows[nIndex].Cells["cc_code_name"].Value == null)
			{
				MessageBox.Show("CodeName cannot be blank");
				return false;
			}
			string name = vKeys.Rows[nIndex].Cells["cc_name"].Value.ToString();
			string code_name = vKeys.Rows[nIndex].Cells["cc_code_name"].Value.ToString();
			string sc = " INSERT INTO hotkey (name, code_name) VALUES('" + Program.EncodeQuote(name) + "', '" + Program.EncodeQuote(code_name) + "') ";
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
		private void vKeys_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if(vKeys.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Save")
			{
				DoSaveHotKey(e.RowIndex);
			}
		}

		private void vKeys_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (vKeys.Rows[vKeys.CurrentRow.Index].Cells["cc_id"].Value == null)
				return; //donot assign before new record added
			if (vKeys.Columns[e.ColumnIndex].Name == "cc_remove")
			{
			}
			else
			{
				vKeys.Rows[e.RowIndex].Cells["cc_remove"].Value = "Save";
				vKeys.Rows[e.RowIndex].Cells["cc_key_code"].Value = "press a key..";
			}
		}

		private void vKeys_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			e.Row.Cells["cc_remove"].Value = "Save";
		}

		private void vKeys_KeyDown(object sender, KeyEventArgs e)
		{
			if (vKeys.CurrentRow == null)
				return;
			if(vKeys.Rows[vKeys.CurrentRow.Index].Cells["cc_id"].Value == null)
				return; //donot assign before new record added
			string id = vKeys.Rows[vKeys.CurrentRow.Index].Cells["cc_id"].Value.ToString();
			if (id == "")
				return; //donot assign before new record added
			if (e.KeyCode == Keys.Enter)
			{
				vKeys.Rows[vKeys.CurrentRow.Index].Cells["cc_key_code"].Value = "press a key..";
				return; //ready to assign new hotkey
			}
			if (e.KeyCode == Keys.Escape)
			{
				vKeys.Rows[vKeys.CurrentRow.Index].Cells["cc_key_code"].Value = "";
				return;
			}
			if(e.KeyValue == 16 || e.KeyValue == 17 || e.KeyValue == 18) //ignore only ctrl, alt, shift keypress
				return;
			int key_code = e.KeyValue;
			string sCtrl = "0";
			string sAlt = "0";
			string sShift = "0";
			string desc = "";
			if(e.Control)
			{
				sCtrl = "1";
				desc += "[CTRL]";
			}
			if(e.Alt)
			{
				sAlt = "1";
				desc += "[ALT]";
			}
			if(e.Shift)
			{
				sShift = "1";
				desc += "[SHIFT]";
			}
			string key_name = e.KeyCode.ToString();
			desc += key_name;
	
			string sc = " UPDATE hotkey SET key_code = " + key_code + " ";
			sc += ", ctrl = " + sCtrl + ", alt = " + sAlt + ", shift = " + sShift + ", key_desc = '" + desc + "' ";
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
				Program.ShowExp(sc, e1);
				return;
			}
			RefreshHotKey();
			return;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
