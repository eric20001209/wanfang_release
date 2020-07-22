using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormConfirm : Form
	{
		public  string m_sMSG = "";
		public  string m_sConfirm ="0";
		
		public FormConfirm()
		{
			InitializeComponent();
		}

		private void FormConfirm_Load(object sender, EventArgs e)
		{
			lblMsg.Text = m_sMSG;
		}

		private void btnYes_Click(object sender, EventArgs e)
		{
			m_sConfirm = "1";
			this.Close();
		}

		private void btnNo_Click(object sender, EventArgs e)
		{
			m_sConfirm = "0";
			this.Close();
			
		}
		
	}
}
