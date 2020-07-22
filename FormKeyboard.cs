using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormKeyboard : Form
	{
		public btncompanyok m_hDest = null;
		public AddUser m_hAddUser = null;
        public form1 m_hForm1 = null;
		public int m_x = 0;
		public int m_y = 0;
		private bool m_bUpper = false;
		public FormKeyboard()
		{
			InitializeComponent();
		}
		private void FormKeyboard_Load(object sender, EventArgs e)
		{
			this.Location = new Point(m_x, m_y);
		}
		private void btnKey_Click(object sender, EventArgs e)
		{
			Button key = (Button)sender;
			string t = key.Text.ToLower();
			string rt = t;
			if(m_bUpper)
				rt = t.ToUpper();
			if (t == "shift")
			{
				m_bUpper = !m_bUpper;
				return;
			}
			else if (t == "del")
			{
				rt = "del";
			}
			else if (key.Text.ToLower() == "enter")
			{
				rt = "\r\n";
			}
			else if (key.Text.ToLower() == "close")
			{
				this.Close();
				return;
			}
			else if (key.Text.ToLower() == "space")
			{
				rt = " ";
			}
			if(m_hDest != null)
				m_hDest.ReceiveKeyboardKey(rt);
			else if (m_hAddUser != null)
				m_hAddUser.ReceiveKeyboardKey(rt);
            else if (m_hForm1 != null)
                m_hForm1.ReceiveKeyboardKey(rt);
		}
	}
}
