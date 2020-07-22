using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormMyPosMate : Form
	{
		public double m_dAmount = 0;
		public string m_sInvoiceNumber = "";
		public bool m_bSuccess = false;
		public string m_sType = "";
		private FormMyPosMatePosPay m_fMPMPosPay;

		public FormMyPosMate()
		{
			InitializeComponent();
			m_fMPMPosPay = new FormMyPosMatePosPay();
		}
		private void FormMyPosMate_Load(object sender, EventArgs e)
		{
			lblAmount.Text = m_dAmount.ToString("c");
			lblStatus.Text = "Waiting";
			btnCancel.Visible = false;
			btnClose.Visible = true;
			btnPay.Visible = true;
			btnPay.Text = m_sType;
			if (m_sType == "Refund")
			{
				btnPay.Enabled = false;
				txtreference.Visible = true;
				label4.Visible = true;
				//txtreference.Focus();
			}
		}
		private void btnPay_Click(object sender, EventArgs e)
		{
			btnClose.Visible = false;
			btnCancel.Visible = false;
			btnPay.Visible = false;
			DoMyPosMate();
			timer1.Interval = 1000;
			timer1.Start();
			lblStatus.Text = "Processing";
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			m_fMPMPosPay.Close();
			this.Close();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_fMPMPosPay.Close();
			btnCancel.Visible = false;
			btnPay.Visible = true;
			btnClose.Visible = true;
			lblStatus.Text = "Waiting";
			m_fMPMPosPay = new FormMyPosMatePosPay();
		}
		private void DoMyPosMate()
		{
			string refund_password = "";
			string refund_reference_no = "";
			if (m_sType == "Refund") 
			{
				numbericpad np = new numbericpad();
				np.m_sText = "Enter Refund Password";
				np.ShowDialog();
				refund_password = np.m_sAdmount;
				refund_reference_no = txtreference.Text;
			}
			m_fMPMPosPay.m_sInvoiceNumber = m_sInvoiceNumber;
			m_fMPMPosPay.m_bSuccess = false;
			m_fMPMPosPay.m_dAmount = m_dAmount;
			m_fMPMPosPay.m_sType = m_sType;
			m_fMPMPosPay.m_sRefundPassword = refund_password;
			m_fMPMPosPay.m_sRefundReferenceNo = refund_reference_no;
			m_fMPMPosPay.Show();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			m_bSuccess = false;
			m_bSuccess = m_fMPMPosPay.m_bSuccess;
			if (m_bSuccess)
			{
				timer1.Stop();
				lblStatus.Text = "Success";
				btnCancel.Visible = false;
				btnClose.Visible = true;
				//m_fMPMPosPay.Close();
			}
			else if (!m_bSuccess && m_sType == "Refund")
			{
				timer1.Stop();
				lblStatus.Text = "Refund failed";
				btnCancel.Visible = false;
				btnClose.Visible = true;
				//m_fMPMPosPay.Close();
			}
			else if (!m_bSuccess && m_fMPMPosPay.m_sTradeMessage == "TRADE_CLOSED" && m_sType == "Pay")
			{
				timer1.Stop();
				lblStatus.Text = "Trade Closed";
				btnCancel.Visible = false;
				btnClose.Visible = true;
				//m_fMPMPosPay.Close();
			}
			else if (!m_bSuccess && m_fMPMPosPay.m_sTradeMessage == "" && m_sType == "Pay")
			{
				lblStatus.Text = "Processing";
				btnCancel.Visible = true;
				btnClose.Visible = true;
				//m_fMPMPosPay.Close();
			}
			else if (!m_bSuccess && m_fMPMPosPay.m_sTradeMessage != "" && m_sType == "Pay")
			{
				lblStatus.Text = "Processing";
				btnCancel.Visible = false;
				btnClose.Visible = false;
				//m_fMPMPosPay.Close();
			}
		}

		private void txtreference_TextChanged(object sender, EventArgs e)
		{
			if (txtreference.Text != "")
				btnPay.Enabled = true;
		}
	}
}
