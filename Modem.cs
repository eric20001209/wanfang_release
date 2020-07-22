using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace QPOS2008
{
	/// <summary>
	/// Base class for communication with modem. 
	/// All concrete model modem sub-classes should inherit this base class.
	/// </summary>
	public class Modem
	{
		#region structs

		public struct AttachedModem
		{
			public string Port;
			public string ModemModel;
		}

		#endregion

		#region private fields

		public byte[] m_buffer = new byte[40960];
		public int m_nIndex = 0;
		private SerialPort _port;
		private string _pname = "";
		private int _boudrate = 0;
		private string _answer;
		private SetCallback _call = null;
	
		#endregion

		#region delegates

		public delegate void SetCallback(Modem modcall);

		#endregion

		#region properties

		// COM port name
		public string PortNumber
		{
			get {return _pname ;}
			set {_pname=value ;}
		}

		// Port Boudrate
		public string PortBoudrate
		{
			get {return _boudrate.ToString() ;}
			set {_boudrate=int.Parse(value) ;}
		}

		// Default boudrate read-only
		public virtual int DefaultBoudrate
		{
			get {return 9600;}
		}

		// Modem answer to command, field only read-only
		public string ModemAnswer
		{
			get {return _answer ;}
		}

		#endregion

		#region constructors

		// First constructor
		public Modem(SerialPort prt, SetCallback c)
		{
			_call = c;
			_port = prt;
			_port.DataReceived+=new SerialDataReceivedEventHandler(this.DataReceived);
//			_port.ReceivedBytesThreshold = 4;
			if (_pname == "") _pname = Modem.FirstAttachedModem().Port ;
			if (_boudrate == 0) _boudrate = this.DefaultBoudrate ;
//			_port.Encoding = Encoding.UTF8;
			OpenModem();
		}
		// Second constructor
		public Modem(SerialPort prt, string prtnum, SetCallback c)
		{
			_call = c;
			_port = prt;
			_port.DataReceived += new SerialDataReceivedEventHandler(this.DataReceived);
//			_port.ReceivedBytesThreshold = 4;
			this.PortNumber = prtnum;
			this.PortBoudrate = this.DefaultBoudrate.ToString();
			OpenModem();
		}
		/*
		Third constructor parameterless - this means that there will be no
		modem answer delegate (null). This in turn results that UI will not 
		see modem initialization and answer events.
		*/
		public Modem():this(new SerialPort(), null) { }

		#endregion

		#region non-virtual methods

		// Method which will call delegate. This delegate 
		// can be fired not on every DataReceived event.
		private void AnalyzeAnswer()
		{
			if (_answer.Length > 0 && _call!=null)
			{
				_call(this);
			}
		}

		// Opening modem port
		private void OpenModem()
		{
			this.CloseModem();
			if(_pname == null)
				return;
			_port.PortName = _pname;
			_port.BaudRate = _boudrate;
            if (_port.IsOpen)
                _port.Close();    
		    _port.Open();
			this.InitializeModem();
		}
		
		// Closing modem port
		public void CloseModem() 
		{
		   if (_port.IsOpen) _port.Close();
		}

		// Sending command to modem port, needs to be marked as 'protected'
		// for being seen in sub-classes
		protected void SendCommandToModem(string command)
		{
			_answer = "";
			if (!_port.IsOpen) this.OpenModem();
//			_port.Write(command);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(command);
                _port.Write(data, 0, data.Length);

		}

		#endregion

		#region virtual methods

		// Modem initialization, basic AT commands. E0 means echo off.
		protected virtual void InitializeModem()
		{
			this.SendCommandToModem("ATE0\r");
		}

		// Finding modem manufacturer, AT command I4.
		public virtual void GetManufacturer()
		{
			this.SendCommandToModem("ATI4\r");
		}

		// Finding modem product Id, AT command I0.
		public virtual void GetProductId()
		{
			this.SendCommandToModem("ATI0\r");
		}

		public virtual void SendCmd(string sCmd)
		{
			m_nIndex = 0;
			this.SendCommandToModem(sCmd);
		}

		#endregion

		#region static methods

		// Method for searching connected modem on COM ports.
		// Returns modem manufacturer, modem chipset version
		// (with AT commands I4I3) and COM port on which modem exists.
		public static AttachedModem FirstAttachedModem()
		{
			AttachedModem am = new AttachedModem();
			SerialPort sp = new SerialPort();
			string port, answer;

			for (int i = 1; i <= 8; i++)
			{
				port = "COM" + i.ToString();
				sp.PortName = port;
				sp.BaudRate = 9600;

				try
				{
					sp.Open();
					if (sp.IsOpen)
					{
						sp.Write("ATE0\r");
						Thread.Sleep(200);
						answer = sp.ReadExisting().Replace("ATE0","").Trim().ToUpper();
						if (answer == "OK")
						{
							sp.Write("ATI4I3\r");
							Thread.Sleep(200);
							answer = sp.ReadExisting().Trim().ToUpper().Replace("\r\nOK","").Trim().Replace("\r\n","");
							am.Port = port;
							am.ModemModel = answer;
							sp.Close();
							break;
						}
					}
					if (sp.IsOpen) sp.Close();
				}
				catch (Exception) {}
			}

			return am;
		}

		#endregion

		#region events

		// Event when signal is received on COM port data pins. Usually 8 pins.
		private void DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			byte[] data = new byte[4096];
			int nLen = _port.Read(data, 0, 4096);
//			data[nLen] = 0;
//			int nLen = _port.Read(m_buffer, m_nIndex, 4096);
			for(int i=0; i<nLen; i++)
			{
				m_buffer[m_nIndex++] = data[i];
			}
//			m_buffer[m_nIndex] = 0;
//			m_buffer[m_nIndex + 1] = 0;
			string result = System.Text.Encoding.UTF8.GetString(data, 0, nLen);
//			result = result.Replace("\r", "").Replace("\n", "");
			_answer = result.Replace("\0", "").Trim().ToLower();
			this.AnalyzeAnswer();
		}

		#endregion
	}
}
