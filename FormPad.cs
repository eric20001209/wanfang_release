using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace QPOS2008
{
	public partial class FormPad : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
		private Control m_lastFocused = null;
		public string m_sAdmount = "";

		public string m_sChangeQty = "";
		public string m_sPasswordControl = "1";
		
		public FormPad()
		{
			InitializeComponent();
		}

		private void onControlLeave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}

		private void FormPad_Load(object sender, EventArgs e)
		{
			this.txtQty.Text = m_sChangeQty;
			this.txtQty.Select();
			this.txtQty.Focus();   
		}

		private void btnPinpad_Click(object sender, EventArgs e)
		{
			//	this.textBoxBarcode.UseSystemPasswordChar = true;
			Button CurrentButton = (Button)sender;
			string name = CurrentButton.Name;
			string value = CurrentButton.Text;
			Control nc = txtQty;
			//	if (m_btncheck == "1")
			//	{
			//		this.txtdsp.Text = "";
			//		m_btncheck = "0";
			//	}
			//	if (m_pw == "1")
			this.txtQty.UseSystemPasswordChar = true;
			if (m_lastFocused != null)
				nc = m_lastFocused;
			int WM_KEYDOWN = 0x0100;
			int nKey = 0x30;

			if (name == "btndel")
			{
				nKey = 0x08;
			}
			else if (name == "btnent")
				nKey = 17;
			else if (name == "btndot")
				nKey = 110;
			else if (name == "btnclear")
			{
				txtQty.Text = "";
				return;
			}
			else
				nKey += Program.MyIntParse(value);

			Message msg = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
			PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
			if (name == "btn00")
			{
				nKey = 48;
				Message msg2 = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
				PostMessage(msg2.HWnd, msg2.Msg, msg2.WParam, msg2.LParam);
			}

		}

		private void numbericpad_KeyDown(object sender, KeyEventArgs e)
		{
			//	this.textBoxBarcode.UseSystemPasswordChar = true;

			if (e.Control && e.KeyValue == 77)
			{
				//	m_sAdmount = this.txtQty.Text;
				m_sChangeQty = this.txtQty.Text;
				this.Close();
			}
			else if (e.KeyValue == 17)
			{
				//	m_sAdmount = txtQty.Text;
				m_sChangeQty = this.txtQty.Text;
				this.Close();
			}
			else if (e.KeyValue == 13)
			{
				//	m_sAdmount = txtQty.Text;
				m_sChangeQty = this.txtQty.Text;
				this.Close();
			}
			switch (e.KeyCode)
			{
				case Keys.Return:
					m_sAdmount = txtQty.Text;
					//		btnPinpad_Click(sender, e);
					break;
				default:
					break;
				//this.Close();
			}
		}

		private void txtQty_KeyDown(object sender, KeyEventArgs e)
		{
			numbericpad_KeyDown(sender, e);

			this.txtQty.Select();
			this.txtQty.Focus();
			if (e.KeyCode == Keys.Return)
			{
				m_sChangeQty = this.txtQty.Text;
				this.Close();
			}
		}

		private void FormQty_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				m_sChangeQty = this.txtQty.Text;
				this.Close();
			}
		}

		private void btnclose_Click(object sender, EventArgs e)
		{
            this.Close();
		}


	}
}
