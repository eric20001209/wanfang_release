using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace QPOS2008
{
	public class MySerialPort
	{
		public byte[] m_buffer = new byte[409600];
		public int m_nIndex = 0;
		public int m_nBegin = 0;
		public string m_sRet = "";
		private SerialPort _port;
		private string _pname = "";
		private int _boudrate = 9600;
		private StopBits _stopbits = StopBits.One;
		private int _databits = 8;
		private Parity _parity = Parity.None;
		
//		private string _answer;

		// COM port name
		public string PortNumber
		{
			get { return _pname; }
			set { _pname = value; }
		}

		// Port Boudrate
		public string PortBoudrate
		{
			get { return _boudrate.ToString(); }
			set { _boudrate = int.Parse(value); }
		}

		// Default boudrate read-only
		public virtual int DefaultBoudrate
		{
			get { return 9600; }
		}

		// First constructor
		public MySerialPort(SerialPort prt, string prtnum)
		{
			_port = prt;
			_port.DataReceived += new SerialDataReceivedEventHandler(this.DataReceived);
			//			_port.ReceivedBytesThreshold = 4;
			this.PortNumber = prtnum;
			this.PortBoudrate = this.DefaultBoudrate.ToString();
		}
		/*
		Second constructor parameterless - this means that there will be no
		answer delegate (null). This in turn results that UI will not 
		see initialization and answer events.
		*/
		public MySerialPort()
			: this(new SerialPort(), null)
		{
			_port.DataReceived += new SerialDataReceivedEventHandler(this.DataReceived);
			this.PortBoudrate = this.DefaultBoudrate.ToString();
		}
		public MySerialPort(string prtnum, int nBoudrate, int nDataBits, Parity parity, StopBits stopBits)
		{
			_port = new SerialPort(prtnum, nBoudrate, parity, nDataBits, stopBits);
			_port.DataReceived += new SerialDataReceivedEventHandler(this.DataReceived);
			//			_port.ReceivedBytesThreshold = 4;
			this.PortNumber = prtnum;
			this._boudrate = nBoudrate;
			this._parity = parity;
			this._databits = nDataBits;
			this._stopbits = stopBits;
		}

		// Opening port
		public bool Open()
		{
			if (_port.IsOpen)
				return true;
//			this.Close();
			if (_pname == null)
				return false;
			_port.PortName = _pname;
			_port.BaudRate = _boudrate;
//			_port.DataBits = _databits;
//			_port.Parity = _parity;
//			_port.StopBits = _stopbits;
			try
			{
				_port.Open();
			}
			catch (Exception e)
			{
				string s = e.ToString();
				return false;
			}
			return true;
		}
		// Closing port
		public void Close()
		{
			if (_port.IsOpen)
			{
				try
				{
					_port.Close();
					_port.Dispose();
				}
				catch (Exception e)
				{
					string es = e.ToString();
				}
			}
		}
		public void ResetBuffer()
		{
			m_nBegin = 0;
			m_nIndex = 0;
		}
		protected void SendCommand(string command)
		{
			//			_answer = "";
			if (!_port.IsOpen) this.Open();
			//			_port.Write(command);
			byte[] data = System.Text.Encoding.UTF8.GetBytes(command);
			try
			{
				_port.Write(data, 0, data.Length);
			}
			catch (Exception e)
			{
				string se = e.ToString();
				return;
			}
			//			Thread.Sleep(500);
			//			m_nIndex = 0;
		}
		public virtual void SendCmd(string sCmd)
		{
			//			m_nIndex = 0;
			this.SendCommand(sCmd);
		}
		public virtual void SendCmd(byte[] data, int nStart, int nLen)
		{
			if (!_port.IsOpen) this.Open();
			try
			{
				_port.Write(data, nStart, nLen);
			}
			catch (Exception e)
			{
				string se = e.ToString();
				return;
			}
			//			Thread.Sleep(500);
			//			m_nIndex = 0;
		}

		// Event when signal is received on COM port data pins. Usually 8 pins.
		private void DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			byte[] data = new byte[4096];
			int nLen = _port.Read(data, 0, 4096);
			Program.g_log.Info("serial port read " + nLen + " bytes data, m_nIndex=" + m_nIndex.ToString());
			//			data[nLen] = 0;
			//			int nLen = _port.Read(m_buffer, m_nIndex, 4096);
			for (int i = 0; i < nLen; i++)
			{
				if (m_nIndex >= 409600)
				{
					Program.g_log.Info("serial port buffer overflow, index=" + m_nIndex.ToString());
					break;
				}
				m_buffer[m_nIndex++] = data[i];
			}
			if (nLen <= 1)
				return;
			m_buffer[m_nIndex] = 0x0;
			//			m_buffer[m_nIndex + 1] = 0x0;
			//			string result = System.Text.Encoding.UTF8.GetString(data);
			//			string result = System.Text.Encoding.UTF8.GetString(m_buffer);
			ASCIIEncoding ascii = new ASCIIEncoding();
			//			string result = ascii.GetString(m_buffer, 0, m_nIndex);
			string result = ascii.GetString(m_buffer, m_nBegin, m_nIndex - m_nBegin);
			m_sRet = result.Replace("\0", "").Trim().ToLower();
			m_sRet = m_sRet.Replace("\r", "").Replace("\n", "");

			string sh = "";
//			for (int m = 0; m < result.Length; m++)
//			{
//				sh += "0x" + ((int)result[m]).ToString("X") + " ";
//			}
			for(int m = m_nBegin; m < m_nIndex; m++)
			{
				sh += "0x" + m_buffer[m].ToString("X") + " ";
			}
			Program.g_log.Info("serial port received " + nLen.ToString() + " bytes data, m_nBegin:" + m_nBegin.ToString() + ", m_nIndex:" + m_nIndex.ToString() + ", string in buffer:" + result + " Hex:" + sh);
			//			string result = System.Text.Encoding.UTF8.GetString(data, 0, nLen);
			//			result = result.Replace("\r", "").Replace("\n", "");
			//			_answer = result.Replace("\0", "").Trim().ToLower();
		}
	}
}
