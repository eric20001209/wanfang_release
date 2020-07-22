using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QPOS2008
{
	class CEzLog
	{
		private string m_fn = "gpos.log";
		private string m_fn_sync = "sync.log";
		private string m_path = "";
		private string m_path_sync = "";
		public CEzLog()
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory;
			m_path = sPath + m_fn;
			m_path_sync = sPath + m_fn_sync;
		}
		private void DoCheckFile(string sPath)
		{
			if (File.Exists(sPath))
			{
				long length = new System.IO.FileInfo(sPath).Length;
				if (length > 1024000) //100k
					File.Delete(sPath);
			}
		}
		public void LogSqlErr(string msg)
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "sqlerr.log";
			DoCheckFile(sPath);
			string s = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "INFO:" + msg + "\r\n";
			try
			{
				File.AppendAllText(m_path, s);
			}
			catch (Exception e)
			{
			}
		}
		public void LogSync(string msg)
		{
			DoCheckFile(m_path_sync);
			string s = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + msg + "\r\n";
			try
			{
				File.AppendAllText(m_path_sync, s);
			}
			catch(Exception e)
			{
			}
		}
		public void Log(string msg)
		{
			DoCheckFile(m_path);
			string s = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "INFO:" + msg + "\r\n";
			try
			{
				File.AppendAllText(m_path, s);
			}
			catch (Exception e)
			{
			}
		}
		public void Info(string msg)
		{
			if (Program.m_debugLogType == "None" || Program.m_debugLogType == "Warn" || Program.m_debugLogType == "Error")
				return;
			DoCheckFile(m_path);
			string s = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "INFO:" + msg + "\r\n";
			try
			{
				File.AppendAllText(m_path, s);
			}
			catch (Exception e)
			{
			}
		}
		public void Warn(string msg)
		{
			if (Program.m_debugLogType == "None" || Program.m_debugLogType == "Error")
				return;
			DoCheckFile(m_path);
			string s = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "WARN:" + msg + "\r\n";
			try
			{
				File.AppendAllText(m_path, s);
			}
			catch (Exception e)
			{
			}
		}
		public void Err(string msg)
		{
			DoCheckFile(m_path);
			string s = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "ERROR:" + msg + "\r\n";
			try
			{
				File.AppendAllText(m_path, s);
			}
			catch (Exception e)
			{
			}
		}
	}
}
