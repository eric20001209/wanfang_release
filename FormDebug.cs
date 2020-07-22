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
	public partial class FormDebug : Form
	{
		private SqlDataAdapter myAdapter;
		private SqlConnection myConnection;
		private SqlCommand myCommand;
		DataSet dst = new DataSet();

		public FormDebug()
		{
			InitializeComponent();
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
		}
		private void btnLanuch_Click(object sender, EventArgs e)
		{
			form1 fm = new form1();
			fm.ShowDialog();
		}
		private void FormDebug_Load(object sender, EventArgs e)
		{
			this.ActiveControl = txtSql;
		}
		private void btnQuery_Click(object sender, EventArgs e)
		{
			string sc = txtSql.SelectedText.Trim();
			if(sc == "")
				sc = txtSql.Text.Trim();
			if (sc == "")
			{
				MessageBox.Show("Please enter sql query first");
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			if(sc.ToLower().IndexOf("select ") != 0 && sc.ToLower().IndexOf("set dateformat") != 0)
			{
				if(sc.ToLower().IndexOf("update") >= 0 && sc.ToLower().IndexOf("where") <= 0)
				{
					MessageBox.Show("UPDATE without WHERE is dangerous and prohibit");
					Cursor.Current = Cursors.Default;
					return;
				}
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.CommandTimeout = 30000;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception ex)
				{
					myConnection.Close();
					Program.ShowExp(sc, ex);
					Cursor.Current = Cursors.Default;
					return;
				}
				MessageBox.Show("done");
				Cursor.Current = Cursors.Default;
				return;
			}
			dst.Tables.Clear();
			int nRows = 0;
			DateTime dtStart = DateTime.Now;
			lblTime.Text = "";
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.SelectCommand.CommandTimeout = 30000;
				nRows = myAdapter.Fill(dst, "data");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				Cursor.Current = Cursors.Default;
				return;
			}
			DateTime dtEnd = DateTime.Now;
			TimeSpan ts = dtEnd - dtStart;
			lblTime.Text = " query time:" + ts.TotalSeconds.ToString() + " seconds, rows:" + nRows.ToString();
			dgv.DataSource = dst.Tables["data"];
			Cursor.Current = Cursors.Default;
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void FormDebug_Resize(object sender, EventArgs e)
		{
			int nWidth = this.Width - 40;
			txtSql.Width = nWidth;
			dgv.Width = nWidth;
			dgv.Height = this.Height - txtSql.Height - btnQuery.Height - 80;
		}
		private void FormDebug_KeyPress(object sender, KeyPressEventArgs e)
		{
		}
		private void txtSql_KeyPress(object sender, KeyPressEventArgs e)
		{
//			if (e.KeyChar == (char)Keys.F5)
//				btnQuery_Click(null, null);
		}
		private void txtSql_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.F5)
				btnQuery_Click(null, null);
		}
	}
}
