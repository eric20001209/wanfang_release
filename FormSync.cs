using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
//using System.Data.SqlClient;

namespace QPOS2008
{
	public partial class FormSync : Form
	{
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;

		private string m_sUploadSql = "";
		private string m_sUploadSqlPre = "";
		private string m_sUploadSqlFinal = "";

		private string m_sDownloadFileName = "";
		private string m_sDownloadFormat = "";
		private string m_sDownloadSql = "";
		private string m_sDownloadSqlPre = "";
		private string m_sDownloadSqlFinal = "";

		private bool m_bSyncInvoice = false;
		private bool m_bSyncItem = false;
		private bool m_bSyncExtra = false;
		private bool m_bSyncVip = false;
		private bool m_bSyncButton = false;
		private bool m_bSyncMenu = false;
		private bool m_bSyncInvoiceActivata = false;
		private string m_sInvoiceSql = "";
		private string m_sItemFormat = "";
		private string m_sItemSql = "";
		private string m_sExtraFormat = "";
		private string m_sExtraSql = "";
		private string m_sButtonFormat = "";
		private string m_sButtonSql = "";
		private string m_sMenuFormat = "";
		private string m_sMenuSql = "";
		private string m_sVipFormat = "";
		private string m_sVipSql = "";
		private string m_sInvoiceSqlActivata = "";
		private int m_nProgress = 0;

		public FormSync()
		{
			InitializeComponent();
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
		}
		private void FormSync_Load(object sender, EventArgs e)
		{
			DoSync();
			timer1.Interval = 1;
//			timer1.Start();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.Hide();
			timer1.Stop();
		}
		public void DoSync()
		{
			GetSettings();
			m_nProgress = 0;
			lblStatus.Text = "Synchronizing, please wait..";
			backgroundWorker1.RunWorkerAsync();//this invokes the DoWork event
//			DoUploadInvoiceActivata();
		}
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
//			backgroundWorker1.ReportProgress(100);//reports a percentage , 0 - 100
			this.Close();
		}
		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
		}
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "sync.ini";
			if (!File.Exists(sPath))
				return;
			StreamReader sr = File.OpenText(sPath);
			string s = sr.ReadToEnd();
			sr.Close();

//			backgroundWorker1.ReportProgress(m_nProgress++);//reports a percentage , 0 - 100
			if (!DoUploadTask(s))
				return;
//			backgroundWorker1.ReportProgress(m_nProgress++);//reports a percentage , 0 - 100
			if (!DoDownloadTask(s))
				return;
		}
		public bool DoUploadTask(string s)
		{
			for (int i = 1; i < 100; i++)
			{
				string sk = i.ToString("D3");
				string ss = GetSection(s, "upload" + sk);
				if (ss == "")
					break;
				bool bEnabled = Program.MyBooleanParse(GetValue(ss, "enabled").Trim());
				if (!bEnabled)
					continue;
				if (!DoUploadFile(ss))
					return false;
			}
			return true;
		}
		public bool DoDownloadTask(string s)
		{
			for (int i = 1; i < 100; i++)
			{
				string sk = i.ToString("D3");
				string ss = GetSection(s, "download" + sk);
				if (ss == "")
					break;
				bool bEnabled = Program.MyBooleanParse(GetValue(ss, "enabled").Trim());
				if (!bEnabled)
					continue;
				if (!DoProcessDownloadedFile(ss))
					return false;
			}
			return true;
		}
		public bool DoProcessDownloadedFile(string s)
		{
			string sFileName = GetValue(s, "FileName").Trim().ToLower();
			string sFileFormat = GetValue(s, "FileFormat").Trim().ToLower();
			string sSqlPre = GetValue(s, "SqlPre");
			string sSqlRow = GetValue(s, "SqlRow");
			string sSqlFinal = GetValue(s, "SqlFinal");

			if (sFileName == "")
				return true;
			sFileName = sFileName.Replace("@@branchid", Program.m_sBranchID_sync);

			string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			sPath = sPath.ToLower().Replace("file:\\", "");
			string sd = sPath + "\\in";
			DirectoryInfo di = new DirectoryInfo(sd);
			foreach (FileInfo f in di.GetFiles("*.zip"))
			{
				if (f.Name.ToLower().IndexOf(sFileName) >= 0)
				{
					string fn_zip = f.FullName;
					string fn = sPath + "\\tmp\\" + f.Name.Replace(".zip", ".csv");
					if (!ProcessFile(fn_zip, fn, sFileFormat, sSqlPre, sSqlRow, sSqlFinal))
						return false;
				}
			}
			return true;
		}
		public bool DoUploadFile(string s)
		{
			string sFileName = GetValue(s, "FileName").Trim().ToLower();
			string sSqlPre = GetValue(s, "SqlPre");
			string sSqlRow = GetValue(s, "SqlRow");
			string sSqlFinal = GetValue(s, "SqlFinal");

			if (sFileName == "")
				return true;
			sFileName = sFileName.Replace("@@branchid", Program.m_sBranchID_sync);

			string sc = sSqlPre;
			if (sc != "")
			{
				try
				{
					SqlCommand myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
					myConnection.Close();
					return false;
				}
			}

			int nRows = 0;
			DataSet dst = new DataSet();
			sc = sSqlRow;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "data");
				if (nRows <= 0)
				{
					//					ShowMsg("no data found to upload.");
					return true;
				}
			}
			catch (Exception e)
			{
				ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				myConnection.Close();
				return false;
			}

			string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			string sPathOut = sPath.ToLower().Replace("file:\\", "") + "\\out";
			sPath = sPath.ToLower().Replace("file:\\", "") + "\\tmp";
			if (!Directory.Exists(sPathOut))
				Directory.CreateDirectory(sPathOut);
			if (!Directory.Exists(sPath))
				Directory.CreateDirectory(sPath);

			string tag = DateTime.Now.ToOADate().ToString();
			string fn = sFileName + "_" + tag + ".csv";
			string fn_zip = fn.Replace(".csv", ".zip");
			if (!WriteCsvFile(dst.Tables["data"], sPath + "\\" + fn, true))
				return false;
			if (!Program.ZipOneFile(sPath + "\\" + fn, sPath + "\\" + fn_zip, 9, 51200))
				return false;
			File.Move(sPath + "\\" + fn_zip, sPathOut + "\\" + fn_zip);
			File.Delete(sPath + "\\" + fn);
			ShowMsg("created file:" + fn_zip);

			sc = sSqlFinal.Trim();
			if (sc != "")
			{
				try
				{
					SqlCommand myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					myConnection.Close();
					ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
					return false;
				}
			}
			return true;
		}
		public bool GetSettings()
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "sync.ini";
			if (!File.Exists(sPath))
				return true;

			StreamReader sr = File.OpenText(sPath);
			string s = sr.ReadToEnd();
			m_bSyncInvoice = Program.MyBooleanParse(GetValue(s, "SyncInvoice"));
			m_bSyncItem = Program.MyBooleanParse(GetValue(s, "SyncItem"));
			m_bSyncExtra = Program.MyBooleanParse(GetValue(s, "SyncExtra"));
			m_bSyncVip = Program.MyBooleanParse(GetValue(s, "SyncVip"));
			m_sInvoiceSql = GetValue(s, "InvoiceSql");
			m_sItemFormat = GetValue(s, "ItemFormat");
			m_sExtraFormat = GetValue(s, "ItemFormat");
			m_sItemSql = GetValue(s, "ItemSql");//.Replace("\r\n", " ");
			m_sExtraSql = GetValue(s, "ExtraSql");

			m_bSyncInvoiceActivata = Program.MyBooleanParse(GetValue(s, "SyncInvoiceActivata"));
			m_sInvoiceSqlActivata = GetValue(s, "InvoiceSqlActivata");
			sr.Close();
			string sDir = AppDomain.CurrentDomain.BaseDirectory + "\\in";
			if (!Directory.Exists(sDir))
				Directory.CreateDirectory(sDir);
			sDir = AppDomain.CurrentDomain.BaseDirectory + "\\out";
			if (!Directory.Exists(sDir))
				Directory.CreateDirectory(sDir);
			sDir = AppDomain.CurrentDomain.BaseDirectory + "\\tmp";
			if (!Directory.Exists(sDir))
				Directory.CreateDirectory(sDir);
			sDir = AppDomain.CurrentDomain.BaseDirectory + "\\bad";
			if (!Directory.Exists(sDir))
				Directory.CreateDirectory(sDir);
			return true;
		}
		public string GetSection(string s, string skey)
		{
			string sl = s.ToLower();
			skey = skey.ToLower();
			string sBegin = "<" + skey + ">";
			string sEnd = "</" + skey + ">";
			string sRet = "";
			int p = sl.IndexOf(sBegin);
			if (p >= 0)
			{
				p += sBegin.Length;
				int q = sl.IndexOf(sEnd, p);
				if (q > p)
					sRet = s.Substring(p, q - p);
			}
			return sRet;
		}
		public string GetValue(string s, string skey)
		{
			string sl = s.ToLower();
			skey = skey.ToLower() + "=";
			string sRet = "";
			int p = sl.IndexOf(skey);
			if (p >= 0)
			{
				p += skey.Length;

				int q = sl.IndexOf("\r", p);
				if (sl[p] == '\"')
				{
					p++;
					q = sl.IndexOf("\"", p);
				}
				if (q < 0)
				{
					ShowMsg("missing double quote at the end, key=" + skey + ", text=" + s);
					return "";
				}
				sRet = s.Substring(p, q - p);
			}
			return sRet;
		}
		public void ShowMsg(string msg)
		{
			Program.g_log.LogSync(msg);
		}
		public bool DoUploadInvoiceActivata()
		{
			string sPath = AppDomain.CurrentDomain.BaseDirectory + "sync.ini";
			if (!File.Exists(sPath))
				return true;
			StreamReader sr = File.OpenText(sPath);
			string s = sr.ReadToEnd();
			sr.Close();
			string ss = GetSection(s, "uploadActivata");
			if (ss == "")
				return true;
			bool bEnabled = Program.MyBooleanParse(GetValue(ss, "enabled").Trim());
			if (!bEnabled)
				return true;
			if (!DoCreateInvoiceActivata(ss))
				return false;
			return true;
		}
		public bool DoCreateInvoiceActivata(string s)
		{
			string sFileName = GetValue(s, "FileName").Trim().ToLower();
			string sSqlPre = GetValue(s, "SqlPre");
			string sSqlRow = GetValue(s, "SqlRow");
			string sSqlFinal = GetValue(s, "SqlFinal");

			if (sSqlRow == "")
				return true;
			string sc = sSqlPre;
			if (sc != "")
			{
				try
				{
					SqlCommand myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					myConnection.Close();
					ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
					return false;
				}
			}

			int nRows = 0;
			DataSet dst = new DataSet();
			sc = sSqlRow;
			try
			{
				SqlDataAdapter myCommand = new SqlDataAdapter(sc, myConnection);
				nRows = myCommand.Fill(dst, "inv_activata");
				if (nRows <= 0)
				{
					ShowMsg("no invoice found to upload.");
					return true;
				}
			}
			catch (Exception e)
			{
				ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return false;
			}

			string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			string sPathOut = sPath.ToLower().Replace("file:\\", "") + "\\activata";
			sPath = sPath.ToLower().Replace("file:\\", "") + "\\tmp";
			if (!Directory.Exists(sPathOut))
				Directory.CreateDirectory(sPathOut);
			sPathOut += "\\out";
			if (!Directory.Exists(sPathOut))
				Directory.CreateDirectory(sPathOut);
			if (!Directory.Exists(sPath))
				Directory.CreateDirectory(sPath);

			string tag = DateTime.Now.ToString("ddMMyyHHmmss");
			string fn = tag + ".csv";
			if (!WriteCsvFileActivata(dst.Tables["inv_activata"], sPath + "\\" + fn, false))
				return false;
			try
			{
				File.Move(sPath + "\\" + fn, sPathOut + "\\" + fn);
			}
			catch (Exception e)
			{
				ShowMsg("failed, created activata invoice file:" + fn + ", e=" + e.ToString());
			}
			ShowMsg("created activata invoice file:" + fn);

			sc = sSqlFinal; //" UPDATE invoice SET uploaded_activata = 1 WHERE uploaded_activata = 0 ";
			if (sc != "")
			{
				try
				{
					SqlCommand myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					myConnection.Close();
					ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
					return false;
				}
			}
			return true;
		}
		private bool WriteCsvFile(DataTable dt, string strPath, bool bWithHeader)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			DataColumnCollection dc = dt.Columns;
			int cols = dc.Count;
			if (bWithHeader)
			{
				for (i = 0; i < cols; i++)
				{
					if (i > 0)
						sb.Append(",");
					sb.Append(dc[i].ColumnName);
				}
				sb.Append("\r\n");
				for (i = 0; i < cols; i++)
				{
					if (i > 0)
						sb.Append(",");
					sb.Append(dc[i].DataType.ToString().Replace("System.", ""));
				}
				sb.Append("\r\n");
			}
			DataRow dr = null;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				dr = dt.Rows[i];
				for (int j = 0; j < cols; j++)
				{
					if (j > 0)
						sb.Append(",");
					string sValue = dr[j].ToString().Replace("\r\n", "@@eznz_return"); //encode line return in site_pages, kit...
					sValue = sValue.Replace("\r", "@@eznz_return"); //encode single return
					sValue = sValue.Replace("\n", "@@eznz_return"); //encode single return
					sValue = sValue.Replace("@@eznz_return", "\\r\\n");
					sb.Append("\"" + Program.EncodeDoubleQuote(sValue) + "\"");
				}
				sb.Append("\r\n");
			}
			//	Encoding enc = Encoding.GetEncoding("iso-8859-1");
			Encoding enc = Encoding.GetEncoding("gb2312");
			byte[] Buffer = enc.GetBytes(sb.ToString());
			if (File.Exists(strPath))
				File.Delete(strPath);
			StreamWriter sw = new StreamWriter(strPath, false, System.Text.Encoding.Unicode);
			sw.Write(sb.ToString());
			sw.Close();
			return true;
		}
		private bool WriteCsvFileActivata(DataTable dt, string strPath, bool bWithHeader)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			DataColumnCollection dc = dt.Columns;
			int cols = dc.Count;
			if (bWithHeader)
			{
				for (i = 0; i < cols; i++)
				{
					if (i > 0)
						sb.Append(",");
					sb.Append(dc[i].ColumnName);
				}
				sb.Append("\r\n");
				for (i = 0; i < cols; i++)
				{
					if (i > 0)
						sb.Append(",");
					sb.Append(dc[i].DataType.ToString().Replace("System.", ""));
				}
				sb.Append("\r\n");
			}
			DataRow dr = null;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				string code = "";
				string barcode = "";
				dr = dt.Rows[i];
				for (int j = 0; j < cols; j++)
				{
					if (j > 0)
						sb.Append(",");
					string sValue = dr[j].ToString();

					if (dc[j].ColumnName == "code")
						code = dr[j].ToString();
					else if (dc[j].ColumnName == "barcode")
					{
						barcode = dr[j].ToString().Trim();
						if (barcode == "")
						{
							int nLen = Program.g_sActivataStoreCode.Length + code.Length;
							barcode = Program.g_sActivataStoreCode;
							for (int n = nLen; n < 16; n++)
								barcode += "0";
							barcode += code;
						}
						sValue = barcode;
					}

					sb.Append("\"" + Program.EncodeDoubleQuote(sValue) + "\"");
				}
				sb.Append("\r\n");
			}
			//	Encoding enc = Encoding.GetEncoding("iso-8859-1");
			Encoding enc = Encoding.GetEncoding("gb2312");
			byte[] Buffer = enc.GetBytes(sb.ToString());
			if (File.Exists(strPath))
				File.Delete(strPath);
			StreamWriter sw = new StreamWriter(strPath, false, System.Text.Encoding.Unicode);
			sw.Write(sb.ToString());
			sw.Close();
			return true;
		}
		private bool ProcessFile(string fn_zip, string fn, string sFormat, string sSqlPre, string sSqlRow, string sSqlFinal)
		{
			if (!File.Exists(fn_zip))
				return true;
			if (!Program.UnZipOneFile(fn_zip, fn))
			{
				ShowMsg("error unzip " + Path.GetFileName(fn_zip) + ", ");
				try
				{
					File.Delete(fn_zip);
					ShowMsg("file deleted " + Path.GetFileName(fn_zip));
				}
				catch (Exception ed)
				{
					ShowMsg("delete file " + Path.GetFileName(fn_zip) + " faile, e=" + ed.ToString());
				}
				return true;
			}
			if (!File.Exists(fn))
			{
				ShowMsg(fn + " not found after unzip");
				return true;
			}

			string sc = sSqlPre;
			if (sc != "")
			{
				try
				{
					SqlCommand myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					myConnection.Close();
					ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
					return false;
				}
			}

			int nLines = File.ReadAllLines(fn).Length;
			
			FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
			StreamReader r = new StreamReader(fs, Encoding.Unicode);
			r.BaseStream.Seek(0, SeekOrigin.Begin);

			string checkLine = sFormat;
			string[] aCol = checkLine.Split(',');
			string line = r.ReadLine();
			if (line != checkLine)
			{
				r.Close();
				fs.Close();
				ShowMsg("Invalid first line format of " + Path.GetFileName(fn) + ", expected:" + sFormat + " got:" + line);
//				string fn_zip = AppDomain.CurrentDomain.BaseDirectory + "in\\item_" + Program.m_sBranchID + ".zip";
				string fn_bad = AppDomain.CurrentDomain.BaseDirectory + "bad\\" + Path.GetFileName(fn_zip);
				if (File.Exists(fn_bad))
					File.Delete(fn_bad);
				File.Move(fn_zip, fn_bad);
				File.Delete(fn);
				return true;
			}
			line = r.ReadLine(); //skip 2nd line as well
			if (line == null)
			{
				r.Close();
				fs.Close();
				ShowMsg("not enough lines in " + Path.GetFileName(fn));
				return true;
			}

			bool bShowProgress =  fn.IndexOf("item_") >= 0 && nLines > 100;
//			if(bShowProgress)
//				this.Show();
			int i = 0;
			bool bRet = true;
			line = r.ReadLine();
			while (line != null)
			{
				if (!ProcessLine(line, aCol, sSqlRow))
				{
					bRet = false;
					break;
				}
				i++;
				line = r.ReadLine();
				if(bShowProgress)
				{
					m_nProgress = (int)(i * 100 / nLines);
					backgroundWorker1.ReportProgress(m_nProgress);
				}
			}
			if (bRet)
				ShowMsg("found file " + Path.GetFileName(fn) + ", " + i.ToString() + " rows updated.");
			r.Close();
			fs.Close();
			if (bRet)
				File.Delete(fn_zip);
			File.Delete(fn);

			sc = sSqlFinal;
			if (sc != "")
			{
				try
				{
					SqlCommand myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.Connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					myConnection.Close();
					ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
					return false;
				}
			}
			return true;
		}
		private bool ProcessLine(string sLine, string[] aCol, string sSql)
		{
			string sc = sSql;
			if (sc == "")
				return true;
			char[] cb = sLine.ToCharArray();
			int pos = 0;
			string[] av = new string[255];
			for (int i = 0; i < aCol.Length; i++)
			{
				av[i] = Program.EncodeQuote(Program.CSVNextColumn(cb, ref pos));
			}
			for (int i = aCol.Length - 1; i >= 0; i--)
			{
				sc = sc.Replace("@@" + i.ToString(), av[i]);
			}
			//			sc = sc.Replace("@@branchid", Program.m_sBranchID);
			try
			{
				SqlCommand myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				myConnection.Close();
				ShowMsg("SQL Error: " + e.ToString() + "\r\nsc=" + sc);
				return false;
			}
			return true;
		}
	}
}
