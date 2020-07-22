using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace QPOS2008
{
	public partial class FormCasScale : Form
	{
		private MySerialPort m_sp = null;
		private System.Timers.Timer aTimer;
		private int m_nTimeout = 0;
		public double m_dQty = 0;
		public string m_sErr = "";

		public FormCasScale()
		{
			m_sErr = "";
			InitializeComponent();
			if (Program.m_sCasScalePort == "" || Program.m_sCasScalePort.IndexOf("COM") < 0)
			{
				m_sErr = "Error Cas Scale Port not properly configured";
				return;
			}
			m_sp = new MySerialPort(Program.m_sCasScalePort, 9600, 7, Parity.Even, StopBits.One);
			m_sp.PortNumber = Program.m_sCasScalePort;
			timer1.Interval = 100;
		}
		private void FormCasScale_Load(object sender, EventArgs e)
		{
			this.Left = -16348;
			if (!m_sp.Open())
			{
				m_sErr = "Error, open port " + Program.m_sCasScalePort + " failed";
				return;
			}
			byte[] buf = new byte[16];
			buf[0] = 87;//'W';
			buf[1] = 13;//
			m_sp.SendCmd(buf, 0, 2);
			m_nTimeout = 0;
			m_dQty = 0;
			timer1.Start();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			m_nTimeout++;
			if (m_nTimeout > 10)
			{
				timer1.Stop();
				m_sp.Close();
				m_sErr = "Error, read weight timed out";
				m_dQty = 0;
				this.Close();
				return;
			}
			if (m_sp.m_nIndex <= m_sp.m_nBegin)
				return;
				
//			string s = m_sp.m_sRet.Replace("KG", "");
			string s = System.Text.Encoding.GetEncoding(1251).GetString(m_sp.m_buffer, m_sp.m_nBegin, m_sp.m_nIndex - m_sp.m_nBegin).ToLower();
			int m = s.IndexOf("\n");
			if(m < 0)
				return;
			m++;
			int n = s.IndexOf("\r", m);
			if(n <= m)
				return;
			s = s.Substring(m, n - m);
			s = s.Replace("kg", "");
			try
			{
				m_dQty = double.Parse(s);
			}
			catch(Exception e1)
			{
				m_dQty = 0;
			}
			m_sp.Close();
			timer1.Stop();
			this.Close();
		}
	}
}
