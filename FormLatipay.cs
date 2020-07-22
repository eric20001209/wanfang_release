using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormLatipay : Form
	{
		public double m_dAmount = 0;
		public string m_sInvoiceNumber = "";
		public bool m_bSuccess = false;
		public string m_sType = "Latipay";
		private FormLatipayQRCode m_fLatipay;

		public FormLatipay()
		{
			InitializeComponent();
			m_fLatipay = new FormLatipayQRCode();
		}
		private void FormLatipay_Load(object sender, EventArgs e)
		{
			lblAmount.Text = m_dAmount.ToString("c");
			lblStatus.Text = "Waiting";
			btnCancel.Visible = false;
			btnClose.Visible = true;
			btnPay.Visible = true;
		}
		private void btnPay_Click(object sender, EventArgs e)
		{
			btnClose.Visible = false;
			btnCancel.Visible = true;
			btnPay.Visible = false;
			DoLatipay();
			timer1.Interval = 1000;
			timer1.Start();
			lblStatus.Text = "Processing";
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_fLatipay.Close();
			btnCancel.Visible = false;
			btnPay.Visible = true;
			btnClose.Visible = true;
			lblStatus.Text = "Waiting";
			m_fLatipay = new FormLatipayQRCode();
		}
		private void DoLatipay()
		{
			m_fLatipay.m_sInvoiceNumber = m_sInvoiceNumber;
			m_fLatipay.m_bSuccess = false;
			m_fLatipay.m_dAmount = m_dAmount;
			m_fLatipay.Show();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			m_bSuccess = false;
			m_bSuccess = m_fLatipay.m_bSuccess;
			if (m_bSuccess)
			{
				timer1.Stop();
				lblStatus.Text = "Success";
				btnCancel.Visible = false;
				btnClose.Visible = true;
				m_fLatipay.Close();
			}
		}
	}
}
