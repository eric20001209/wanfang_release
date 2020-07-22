using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class MyMessageBox : Form
	{
		public string m_title = "";
		public string m_msg = "";
		
		public MyMessageBox()
		{
			InitializeComponent();
		}
		private void MyMessageBox_Load(object sender, EventArgs e)
		{
			int boundWidth = Screen.PrimaryScreen.Bounds.Width;
			int boundHeight = Screen.PrimaryScreen.Bounds.Height;
			int x = boundWidth - this.Width;
			int y = boundHeight - this.Height;
			this.Location = new Point(x / 2, y / 2);

			if (m_title == "")
				this.ControlBox = false;
			else
				this.Text = m_title;
			txtMsg.Text = m_msg;
			txtMsg.Select(0, 0);
			btnOK.Focus();
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
