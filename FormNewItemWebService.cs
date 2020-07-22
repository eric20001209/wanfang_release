using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormNewItemWebService : Form
	{
		public bool m_bDoImport = false;
		public string m_sBarcode = "";
		public string m_sName = "";
		public double m_dPrice = 0;
		public FormNewItemWebService()
		{
			InitializeComponent();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void btnImport_Click(object sender, EventArgs e)
		{
			m_dPrice = Program.MyMoneyParse(txtPrice.Text);
			if(m_dPrice == 0)
			{
				Program.MsgBox("Please enter selling price");
				txtPrice.Focus();
				return;
			}
			m_sName = txtName.Text.Trim();
			m_bDoImport = true;
			this.Close();
		}
		private void FormNewItemWebService_Load(object sender, EventArgs e)
		{
			txtBarcode.Text = m_sBarcode;
			txtName.Text = m_sName;
			txtPrice.Text = m_dPrice.ToString("c");
//			txtPrice.Focus();
			this.ActiveControl = txtPrice;
		}
		private void txtPrice_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("osk.exe");
		}
		private void btnKeyboard_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("osk.exe");
		}
	}
}
