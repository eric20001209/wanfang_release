using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace QPOS2008
{
	public partial class FormPhoneCard : Form
	{
		public FormPhoneCard()
		{
			InitializeComponent();
		}
		private void FormPhoneCard_Load(object sender, EventArgs e)
		{
			txtTrack2.Text = "8964023616468400=2308";
			txtAmount.Text = "10";
			txtTrack2.Focus();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private byte[] GetBytesFromPEM(string pemString, string section)
		{
			var header = String.Format("-----BEGIN {0}-----", section);
			var footer = String.Format("-----END {0}-----", section);

			var start = pemString.IndexOf(header, StringComparison.Ordinal);
			if (start < 0)
				return null;

			start += header.Length;
			var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;

			if (end < 0)
				return null;

			return Convert.FromBase64String(pemString.Substring(start, end));
		}
		public void ATSRequestResponse()
		{
			string host = "https://ws.activata.net.nz:52724";
			string certName = AppDomain.CurrentDomain.BaseDirectory + "12831474.p12";
			string certPEM = AppDomain.CurrentDomain.BaseDirectory + "Activata_POS_CA.pem";
			string password = "$pO5_74";
			string XML_Logon =
				"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
				"<soapenv:Body>" +
				"<Transaction xmlns=\"http://ws.activata.net.nz/POSServ\">" +
				"<TxnType>Logon</TxnType>" +
				"<TermNum>12831474</TermNum>" +
				"<TraceNum>1</TraceNum>" +
				"<POSTimeStamp>" + DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss", DateTimeFormatInfo.InvariantInfo) + "</POSTimeStamp>" +
				"<LastSuccFinTraceNum>0</LastSuccFinTraceNum>" +
				"<POSVersion>GPOS3.74</POSVersion>" +
				"</Transaction>" +
				"</soapenv:Body>" +
				"</soapenv:Envelope>";

			try
			{
				X509Certificate2Collection certificates = new X509Certificate2Collection();
				certificates.Import(certName, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
				
//				X509Certificate2 cert = new X509Certificate2(certPEM);
//				certificates.Add(cert);

				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
				req.AllowAutoRedirect = true;
				req.ClientCertificates = certificates;
				req.Method = "POST";
				string postData = XML_Logon;
				byte[] postBytes = Encoding.UTF8.GetBytes(postData);
				req.ContentLength = postBytes.Length;

//				Console.WriteLine("SENDING:\n" + postData + "\n---------------------");

				Stream postStream = req.GetRequestStream();
				postStream.Write(postBytes, 0, postBytes.Length);
				postStream.Flush();
				postStream.Close();
				WebResponse resp = req.GetResponse();

				Stream stream = resp.GetResponseStream();
				string xmlstring = "";

				string sRet = "";
				using (StreamReader reader = new StreamReader(stream))
				{
					string line = reader.ReadLine();
					sRet += "RESPONSE:\n--------------";
					while (line != null)
					{
						xmlstring += line;
						sRet += line + "\r\n";
						line = reader.ReadLine();
					}
				}
				sRet += "XML OUTPUT:\n--------------";
				sRet += xmlstring;
				sRet += "TABULAR OUTPUT:\n";
				int x = 0;
				foreach (DataTable dt in stam(xmlstring).Tables)
				{
					sRet += "TABLE-" + x.ToString() + ":\n--------------";
					foreach (DataRow dataRow in dt.Rows)
					{
						foreach (var item in dataRow.ItemArray)
						{
							sRet += item;
						}
					}
					x++;
				}
				stream.Close();
			}
			catch (Exception e)
			{
				Program.ShowExp("", e);
			}
		}
		public DataSet stam(string xmlData)
		{
			StringReader theReader = new StringReader(xmlData);
			DataSet theDataSet = new DataSet();
			theDataSet.ReadXml(theReader);
			return theDataSet;
		}
		private void btnRecharge_Click(object sender, EventArgs e)
		{
			ATSRequestResponse();
		}
	}
}
