using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class numbericpad : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
	  
		private Control m_lastFocused = null;
		public string m_sAdmount = "";
		public int m_iLocation = 0;
		public string m_pw = "0";
		public string m_sText = "";
		private string m_btncheck = "1";
        public string m_sTitle = "";

		public numbericpad()
		{
			InitializeComponent();
		}
		private void onControlLeave(object sender, EventArgs e)
		{
			m_lastFocused = (Control)sender;
		}
		private void numbericpad_Load(object sender, EventArgs e)
		{
			txtdsp.Select();
			txtdsp.Focus();
			txtdsp.Text = "";
            label1.Text = m_sTitle;
			if (m_pw == "0")
				txtdsp.ForeColor = System.Drawing.Color.Red;
			lblTitle.Text = m_sText;
			//txtdsp.Text = m_sText;
			switch (m_iLocation)
			{
				case 0:
					this.Location = new Point(600,0);
					break;
				case 1:
					this.Location = new Point(520, 10);
					break;
				case 2:
					this.Location = new Point(540, 310);
					this.Size = new Size(480, 450);
					txtdsp.Location = new Point(37, 58);
					panel1.Location = new Point(37,107 );
					break;
				default:
					break;
			}
			this.BringToFront();
//			MessageBox.Show("location:" + this.Location.X + ", " + this.Location.Y);
		}
		private void btnPinpad_Click(object sender, EventArgs e)
		{
			Button CurrentButton = (Button)sender;
			string name = CurrentButton.Name;
			string value = CurrentButton.Text;
			Control nc = txtdsp;
			if (m_btncheck == "1")
			{
				this.txtdsp.Text = "";
				m_btncheck = "0";
			}
			if (m_pw == "1")
				this.txtdsp.UseSystemPasswordChar = true;
			if (m_lastFocused != null)
				nc = m_lastFocused;
			int WM_KEYDOWN = 0x0100;
			int nKey = 0x30;

			if (name == "btndel")
			{
				nKey = 0x08;
			}
			else if (name == "btnent" )
				nKey = 17;
			else if (name == "btndot")
				nKey = 110;
			else if (name == "btnclear")
			{
				txtdsp.Text = "";
				return;
			}
			else
				nKey += Program.MyIntParse(value);

			Message msg = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
			PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
			if (name == "btn00" )
			{
				nKey = 48;
				Message msg2 = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
				PostMessage(msg2.HWnd, msg2.Msg, msg2.WParam, msg2.LParam);
			}

		}

		private void btnclose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void numbericpad_KeyDown(object sender, KeyEventArgs e)
		{
			//this.txtdsp.UseSystemPasswordChar = true;
			
			if (e.Control && e.KeyValue == 77)
			{
				m_sAdmount = txtdsp.Text;
				this.Close();
			}
			else if (e.KeyValue == 17)
			{
				m_sAdmount = txtdsp.Text;
				this.Close();
			}
			else if (e.KeyValue == 13)
			{
				m_sAdmount = txtdsp.Text;
				this.Close();
			}


			switch (e.KeyCode)
			{
				case Keys.Return:
					m_sAdmount = txtdsp.Text;
			//		btnPinpad_Click(sender, e);
					break;
				default:
					break;
					//this.Close();
			}
			
		   
			
		}

		private void txtdsp_KeyDown(object sender, KeyEventArgs e)
		{
			
			numbericpad_KeyDown(sender, e);
			
		}

		private void txtdsp_Click(object sender, EventArgs e)
		{
			this.txtdsp.Text = "";
			if (m_pw == "1")
				this.txtdsp.UseSystemPasswordChar = true;
		}

		private void txtdsp_TextChanged(object sender, EventArgs e)
		{

		}
 
	}
}
