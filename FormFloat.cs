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
	public partial class FormFloat : Form
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
		private Control m_lastFocused = null;
		public string m_sAdmount = "";
		private double floating = 0;
		
		public FormFloat()
		{
			InitializeComponent();
		}

		private void btnPinpad_Click(object sender, EventArgs e)
		{
			Button CurrentButton = (Button)sender;
			string name = CurrentButton.Name;
			string value = CurrentButton.Text;
			Control nc = txt100;
//			Control nc = m_lastFocused;

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
			{
	//			nKey = 17;
				Calculate();
				return;
			}
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
			
/*			if (name == "btn00")
			{
				nKey = 48;
				Message msg2 = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
				PostMessage(msg2.HWnd, msg2.Msg, msg2.WParam, msg2.LParam);
			}
 */ 

		}
		
		private void Calculate()
		{
			floating = Program.MyDoubleParse(txt100.Text) * 100 + Program.MyDoubleParse(txt50.Text) * 50 + Program.MyDoubleParse(txt20.Text) * 20 + Program.MyDoubleParse(txt10.Text) * 10 + Program.MyDoubleParse(txt5.Text) * 5 + Program.MyDoubleParse(txt2.Text) * 2 + Program.MyDoubleParse(txt1.Text) * 1 + Program.MyDoubleParse(txt50c.Text) * 0.5 + Program.MyDoubleParse(txt20c.Text) * 0.2 + Program.MyDoubleParse(txt10c.Text) * 0.1;
			this.txtQty.Text = floating.ToString("c");
			
//			Program.RecordTillData1("floating", floating.ToString());
//			FormMSG fm = new FormMSG();
//			fm.m_sMsg = "Floating input!";
//			fm.btnNo.Visible = false;
//			fm.btnYes.Visible = false;
//			fm.ShowDialog();
			return;
		}
		
		private void Input()
		{
//			if (MessageBox.Show("Input float? click Yes to input!", "Confirm Input", MessageBoxButtons.YesNo) != DialogResult.Yes)
//				return;
//			else	
//				Program.RecordTillData1("floating", floating.ToString());
			FormConfirm fc = new FormConfirm();
			fc.m_sMSG = floating.ToString("c") + " Floating Input!" ;
			fc.ShowDialog();
			if(fc.m_sConfirm == "0")
				return;
			else if(fc.m_sConfirm == "1")
			{
				Program.RecordTillData1("floating", floating.ToString());
				this.Close();
			}	
			
		}
		
		private void reset()
		{
			txt100.Text="0";
			txt50.Text = "0";
			txt20.Text = "0";
			txt10.Text = "0";
			txt5.Text = "0";
			txt2.Text = "0";
			txt1.Text = "0";
			txt50c.Text = "0";
			txt20c.Text = "0";
			txt10c.Text = "0";
//			txtQty.Text = "0";
		}

		private void btnclose_Click(object sender, EventArgs e)
		{
				Input();
		}

		private void txt100_Click(object sender, EventArgs e)
		{
//			this.txt100.Focus();
			this.txt100.SelectAll();
			m_lastFocused = this.txt100;
		}

		private void txt50_Click(object sender, EventArgs e)
		{
			this.txt50.SelectAll();
			m_lastFocused = this.txt50;
		}

		private void txt20_Click(object sender, EventArgs e)
		{
			this.txt20.SelectAll();
			m_lastFocused = this.txt20;
		}

		private void txt10_Click(object sender, EventArgs e)
		{
			this.txt10.SelectAll();
			m_lastFocused = this.txt10;
		}

		private void txt5_Click(object sender, EventArgs e)
		{
			this.txt5.SelectAll();
			m_lastFocused = this.txt5;
		}

		private void txt2_Click(object sender, EventArgs e)
		{
			this.txt2.SelectAll();
			m_lastFocused = this.txt2;
		}

		private void txt1_Click(object sender, EventArgs e)
		{
			this.txt1.SelectAll();
			m_lastFocused = this.txt1;
		}

		private void txt50c_Click(object sender, EventArgs e)
		{
			this.txt50c.SelectAll();
			m_lastFocused = this.txt50c;
		}

		private void txt20c_Click(object sender, EventArgs e)
		{
			this.txt20c.SelectAll();
			m_lastFocused = this.txt20c;
		}

		private void txt10c_Click(object sender, EventArgs e)
		{
			this.txt10c.SelectAll();
			m_lastFocused = this.txt10c;
		}

		private void FormFloat_Load(object sender, EventArgs e)
		{
			this.txtQty.Text = "";
		}

		private void txt50_TextChanged(object sender, EventArgs e)
		{

		}




	}
}
