using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace QPOS2008
{
	class Http
	{
		public static string post(string url, string jsonParams)
		{
			string result = null;
			StreamWriter streamWriter;
			StreamReader streamReader;
			try
			{
				/*
				 * ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				 * ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072
				 * Ssl3 = 48
				 * Tls = 192
				 * Tls11 = 768
				 * Tls12 = 3072
				 */
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
					 | (SecurityProtocolType)768
					 | (SecurityProtocolType)3072;

				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				req.ContentType = "application/json; charset=UTF-8";
				req.Timeout = 30000;
				req.AllowAutoRedirect = true;
				req.Method = "POST";

				using (streamWriter = new StreamWriter(req.GetRequestStream()))
				{
					streamWriter.Write(jsonParams);
				}
				HttpWebResponse httpResponse = (HttpWebResponse)req.GetResponse();
				using (streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			return result;
		}
		public static string get(string url)
		{
			string result = null;
			Stream stream;
			StreamReader streamReader;
			try
			{
				/*
				 * ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				 * ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072
				 * Ssl3 = 48
				 * Tls = 192
				 * Tls11 = 768
				 * Tls12 = 3072
				 */
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
					 | (SecurityProtocolType)768
					 | (SecurityProtocolType)3072;

				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				req.Timeout = 30000;
				req.AllowAutoRedirect = true;
				req.Method = "GET";

				WebResponse resp = req.GetResponse();
				stream = resp.GetResponseStream();
				streamReader = new StreamReader(stream, Encoding.UTF8, true);
				result = streamReader.ReadToEnd();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			return result;
		}
	}
}

