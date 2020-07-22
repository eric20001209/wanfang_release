using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.SqlClient;

namespace QPOS2008
{
	class SqlConnection
	{
		private System.Data.SqlClient.SqlConnection myConnection;
		public SqlConnection(string s)
		{
//			myConnection = new System.Data.SqlClient.SqlConnection(s);
		}
		public void Open()
		{
//			myConnection.Open();
		}
		public void Close()
		{
//			myConnection.Close();
		}
	}
	class SqlDataAdapter
	{
		public System.Data.SqlClient.SqlCommand SelectCommand;
		private bool m_bSelectOnly = true;
		private System.Data.SqlClient.SqlDataAdapter myAdapter1;
		private System.Data.SqlClient.SqlDataAdapter myAdapter2;
		public SqlDataAdapter(string sc, SqlConnection myConnection)
		{
			myAdapter1 = new System.Data.SqlClient.SqlDataAdapter(sc, Program.myConnection1);
			myAdapter2 = new System.Data.SqlClient.SqlDataAdapter(sc, Program.myConnection2);
			if(Program.m_bSql1Good)
				SelectCommand = myAdapter1.SelectCommand;
			else if(Program.m_bSql2Good)
				SelectCommand = myAdapter2.SelectCommand;
			string s = sc.ToLower();
			if(s.IndexOf("update ") >= 0 || s.IndexOf("insert ") >= 0 || s.IndexOf("delete ") >= 0)
				m_bSelectOnly = false;
		}
		public int Fill(DataSet ds)
		{
			return Fill(ds, "noname");
		}
		public int Fill(DataSet ds, string sTableName)
		{
			int nRows = 0;
			if(Program.m_bSql1Good)
			{
				SelectCommand = myAdapter1.SelectCommand;
				try
				{
					nRows = myAdapter1.Fill(ds, sTableName);
				}
				catch(Exception e)
				{
					if(Program.IsDatabaseError(e.ToString()))
					{
						Program.m_bSql1Good = false;
						if(Program.m_bServer2AsBackup)
							Program.MsgBox("Server 1 is down, switch to Server 2, please advice manager, you can continue checkout !!");
						else
							Program.MsgBox("Server 1 is down, please contact manager immediatly !!");
					}
					else if(m_bSelectOnly)
					{
						throw(e);
						return 0;
					}
				}
				if(Program.m_bSql1Good && m_bSelectOnly)
					return nRows;
			}
			if(Program.m_bServer2AsBackup && Program.m_bSql1Good)
				return nRows;
			if(Program.m_bSql2Good)
			{
				SelectCommand = myAdapter2.SelectCommand;
				try
				{
					nRows = myAdapter2.Fill(ds, sTableName);
				}
				catch (Exception e)
				{
					if (Program.IsDatabaseError(e.ToString()))
					{
						Program.m_bSql2Good = false;
						Program.MsgBox("Server 2 is down, please contact manager immediatly !!");
					}
					else if (m_bSelectOnly)
					{
						throw (e);
						return 0;
					}
				}
			}
			return nRows;
		}
	}
	class SqlCommand
	{
		public SqlConnection connection = new SqlConnection("");
		public System.Data.CommandType commandType;
		public System.Int32 CommandTimeout;
		public System.Data.SqlClient.SqlParameterCollection Parameters;
		private System.Data.SqlClient.SqlCommand myCommand1;
		private System.Data.SqlClient.SqlCommand myCommand2;
		public SqlCommand(string sc, SqlConnection myConnection)
		{
			myCommand1 = new System.Data.SqlClient.SqlCommand(sc, Program.myConnection1);
			myCommand2 = new System.Data.SqlClient.SqlCommand(sc, Program.myConnection2);
			if(Program.m_bSql1Good)
			{
//				connection = Program.myConnection1;
				Parameters = myCommand1.Parameters;
			}
			else if(Program.m_bSql2Good)
			{
//				connection = Program.myConnection2;
				Parameters = myCommand2.Parameters;
			}
		}
		public SqlCommand(string sc)
		{
			myCommand1 = new System.Data.SqlClient.SqlCommand(sc, Program.myConnection1);
			myCommand2 = new System.Data.SqlClient.SqlCommand(sc, Program.myConnection2);
/*			if(Program.m_bSql1Good)
				connection = Program.myConnection1;
			else if(Program.m_bSql2Good)
				connection = Program.myConnection2;
*/		}
		public SqlConnection Connection
		{
			get
			{
				return this.connection;
			}
			set
			{
//				if (Program.m_bSql1Good)
//					connection = Program.myConnection1;
//				else if (Program.m_bSql2Good)
//					connection = Program.myConnection2;
			}
		}
		public System.Data.CommandType CommandType
		{
			get
			{
				return this.commandType;
			}
			set
			{
				this.commandType = value;
				if (Program.m_bSql1Good)
					myCommand1.CommandType = value;
				if (Program.m_bSql2Good)
					myCommand2.CommandType = value;
			}
		}
/*		public void Open()
		{
			if (Program.m_bSql1Good)
				connection = Program.myConnection1;
			else if (Program.m_bSql2Good)
				connection = Program.myConnection2;
		}
		public void Close()
		{
			if (Program.m_bSql1Good)
				UpdateAccessIndex(Program.myConnection1);
			else if (Program.m_bSql2Good)
				UpdateAccessIndex(Program.myConnection2);
		}
*/		private bool UpdateAccessIndex(System.Data.SqlClient.SqlConnection myConnection)
		{
			string sc = " IF NOT EXISTS(SELECT id FROM settings WHERE name = 'server_last_update_index') ";
			sc += " INSERT INTO settings (name, access) VALUES('server_last_update_index', 1001) ";
			sc += " UPDATE settings SET access = ISNULL(access, 0) + 1 WHERE name = 'server_last_update_index' ";	
			System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(sc, myConnection);
			try
			{
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
		public int ExecuteNonQuery()
		{
			int nRows = 0;
			if(Program.m_bSql1Good)
			{
				try
				{
//					myCommand1.CommandType = commandType;
					if(myCommand1.Connection.State == ConnectionState.Closed)
						myCommand1.Connection.Open();
					nRows = myCommand1.ExecuteNonQuery();
				}
				catch(Exception e)
				{
					myCommand1.Connection.Close();
					if (Program.IsDatabaseError(e.ToString()))
					{
						Program.m_bSql1Good = false;
						if (Program.m_bServer2AsBackup)
							Program.MsgBox("Server 1 is down, switch to Server 2, please advice manager, you can continue checkout !!");
						else
							Program.MsgBox("Server 1 is down, please contact manager immediatly !!");
					}
					else if (!Program.m_bSql2Good)
					{
						throw(e);
						return nRows;
					}
				}
				if(Program.m_bSql1Good)
				{
					myCommand1.Connection.Close();
//					UpdateAccessIndex(Program.myConnection1);
				}
			}
			if (Program.m_bServer2AsBackup && Program.m_bSql1Good)
				return nRows;
			if (Program.m_bSql2Good)
			{
				if(Parameters != null && Parameters.Count > 0)
				{
					if(myCommand2.Parameters.Count <= 0)
					{
						foreach(System.Data.SqlClient.SqlParameter p in Parameters)
						{
							string name = p.ParameterName;
							System.Data.SqlDbType dbType = p.SqlDbType;
							object value = p.Value;
							if(name == "@return_tran_id")
								myCommand2.Parameters.Add(name, dbType).Direction = ParameterDirection.Output;
							else if (dbType == SqlDbType.Bit || dbType == SqlDbType.Int)
								myCommand2.Parameters.Add(name, dbType).Value = Program.MyIntParse(value.ToString());
							else if (dbType == SqlDbType.Money)
								myCommand2.Parameters.Add(name, dbType).Value = Program.MyMoneyParse(value.ToString());
							else
								myCommand2.Parameters.Add(name, dbType).Value = value.ToString();
						}
					}
				}
				try
				{
//					myCommand2.CommandType = commandType;
					if (myCommand2.Connection.State == ConnectionState.Closed)
						myCommand2.Connection.Open();
					nRows = myCommand2.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					myCommand2.Connection.Close();
					if (Program.IsDatabaseError(e.ToString()))
					{
						Program.m_bSql2Good = false;
						Program.MsgBox("Server 2 is down, please contact manager immediatly !!");
						return 0;
					}
					else
					{
						throw (e);
						return 0;
					}
				}
				myCommand2.Connection.Close();
//				UpdateAccessIndex(Program.myConnection2);
			}
			return nRows;
		}
	}
}
