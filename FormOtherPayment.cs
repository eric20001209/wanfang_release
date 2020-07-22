using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormOtherPayment : Form
	{
		public double m_dAmount = 0;
		public string m_sInvoiceNumber = "";
		public bool m_bSuccess = false;
		private string m_sType = "";
		private FormAliPay m_fAlipay;
		private FormWeChat m_fPayPlus;
		private FormAlipayDirect m_fAlipayDirect;
		
		public FormOtherPayment()
		{
			InitializeComponent();
//			m_fAlipay = new FormAliPay();
			m_fPayPlus = new FormWeChat();
//			m_fAlipayDirect = new FormAlipayDirect();
		}
		private void FormOtherPayment_Load(object sender, EventArgs e)
		{
			lblAmount.Text = m_dAmount.ToString("c");
			lblStatus.Text = "";
			btnCancel.Visible = false;
			btnClose.Visible = true;
			btnPay.Visible = true;

			if (Program.m_bEnableAttractPay && Program.m_sAttractPayUriRequest != "")
			{
				pbAlipay.Visible = true;
				rbAlipay.Visible = true;
				pbWechat.Visible = true;
				rbWechat.Visible = true;
				m_sType = "WechatPay";
				rbWechat.Checked = true;
				SelectScanningMethodVisible(true);
			}
			if(Program.m_bEnableWechatPayment && Program.m_sWeChatUri != "")
			{
				pbWechatPayPlus.Visible = true;
				rbWechatPayPlus.Visible = true;
				m_sType = "PayPlus";
				rbWechatPayPlus.Checked = true;
				SelectScanningMethodVisible(false);
			}
			if (Program.m_bEnableAlipayDirect && Program.m_sAlipayDirectPartnerID.Trim() != "")
			{
				pbAlipayDirect.Visible = true;
				rbAlipayDirect.Visible = true;
				m_sType = "AlipayDirect";
				rbAlipayDirect.Checked = true;
				SelectScanningMethodVisible(false);
			}
		}
		private void rbWechat_Click(object sender, EventArgs e)
		{
			m_sType = "WechatPay";
			SelectScanningMethodVisible(true);
		}
		private void pbWechat_Click(object sender, EventArgs e)
		{
			m_sType = "WechatPay";
			rbWechat.Checked = true;
			SelectScanningMethodVisible(true);
		}
		private void rbAlipay_Click(object sender, EventArgs e)
		{
			m_sType = "Alipay";
			SelectScanningMethodVisible(true);
		}
		private void pbAlipay_Click(object sender, EventArgs e)
		{
			m_sType = "Alipay";
			rbAlipay.Checked = true;
			SelectScanningMethodVisible(true);
		}
		private void rbWechatPayPlus_Click(object sender, EventArgs e)
		{
			m_sType = "PayPlus";
			SelectScanningMethodVisible(false);
		}
		private void pbWechatPayPlus_Click(object sender, EventArgs e)
		{
			m_sType = "PayPlus";
			rbWechatPayPlus.Checked = true;
			SelectScanningMethodVisible(false);
		}
		private void rbAlipayDirect_CheckedChanged(object sender, EventArgs e)
		{
			m_sType = "AlipayDirect";
			SelectScanningMethodVisible(false);
		}
		private void pbAlipayDirect_Click(object sender, EventArgs e)
		{
			m_sType = "AlipayDirect";
			rbAlipayDirect.Checked = true;
			SelectScanningMethodVisible(false);
		}
		private void btnPay_Click(object sender, EventArgs e)
		{
			if(m_sType == "")
			{
				Program.MsgBox("Please select payment method");
				return;
			}
			if (m_sType == "PayPlus")
			{
				DoPayPlus();
			}
			else if (m_sType == "AlipayDirect")
			{
				DoAlipayDirect();
				return;
			}
			else
			{
				if (lblScanningMethod.Text == "Scan Customer Barocde" && txtBarcode.Text == "")
				{
					Program.MsgBox("Please scan customer barcode!");
					return;
				}
				DoAlipay();
			}
			btnClose.Visible = false;
			btnCancel.Visible = true;
			btnPay.Visible = false;
			timer1.Interval = 1000;
			timer1.Start();
		}
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (m_sType == "PayPlus")
				m_fPayPlus.Close();
			else if (m_sType == "AlipayDirect")
				m_fAlipayDirect.Close();
			else
				m_fAlipay.Close();
			btnCancel.Visible = false;
			btnPay.Visible = true;
			btnClose.Visible = true;
		}
		private void DoAlipay()
		{
			m_fAlipay = new FormAliPay();
			m_fAlipay.m_sPaymentType = m_sType;
			m_fAlipay.m_sScanningMethod = lblScanningMethod.Text;
			m_fAlipay.m_sBarcode = txtBarcode.Text;
			m_fAlipay.m_bSuccess = false;
			m_fAlipay.m_dAmount = m_dAmount;
			m_fAlipay.m_sTradeNumber = Program.m_sBranchID + "." + Program.m_sStationID + "." + m_sInvoiceNumber + "." + DateTime.Now.ToOADate().ToString();
			m_fAlipay.Show();
		}
		private void DoAlipayDirect()
		{
			btnClose.Visible = false;
			btnCancel.Visible = true;
			btnPay.Visible = false;
			m_fAlipayDirect = new FormAlipayDirect();
			m_fAlipayDirect.m_sPaymentType = m_sType;
			m_fAlipayDirect.m_bSuccess = false;
			m_fAlipayDirect.m_dAmount = m_dAmount;
			m_fAlipayDirect.m_sTradeNumber = Program.m_sBranchID + "." + Program.m_sStationID + "." + m_sInvoiceNumber + "." + DateTime.Now.ToOADate().ToString();
			m_fAlipayDirect.ShowDialog();
			m_bSuccess = m_fAlipayDirect.m_bSuccess;
			if (m_bSuccess)
				lblStatus.Text = "Success";
			else
				lblStatus.Text = "Failed";
			btnCancel.Visible = false;
			btnClose.Visible = true;
			m_fAlipayDirect.Dispose();
		}
		private void DoPayPlus()
		{
			m_fPayPlus.m_sInvoiceNumber = Program.m_sBranchID + Program.m_sStationID + m_sInvoiceNumber;
			m_fPayPlus.m_dAmount = m_dAmount;
			m_fPayPlus.Show();
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			m_bSuccess = false;
			if(m_sType == "PayPlus")
				m_bSuccess = m_fPayPlus.m_bSuccess;
			else
				m_bSuccess = m_fAlipay.m_bSuccess;
			if(m_bSuccess)
			{
				timer1.Stop();
				lblStatus.Text = "Success";
				btnCancel.Visible = false;
				btnClose.Visible = true;
			}
		}

		private void btnSwitch_Click(object sender, EventArgs e)
		{
			if (lblScanningMethod.Text == "Generate QR Code")
			{
				lblScanningMethod.Text = "Scan Customer Barocde";
				txtBarcode.Visible = true;
				txtBarcode.Focus();
				txtBarcode.SelectAll();
				return;
			}
			if (lblScanningMethod.Text == "Scan Customer Barocde")
			{
				lblScanningMethod.Text = "Generate QR Code";
				txtBarcode.Visible = false;
				return;
			}
		}

		private void txtBarcode_Click(object sender, EventArgs e)
		{
			txtBarcode.SelectAll();
		}

		private void SelectScanningMethodVisible(bool visible)
		{
			label3.Visible = visible;
			lblScanningMethod.Visible = visible;
			btnSwitch.Visible = visible;
			if (lblScanningMethod.Text == "Scan Customer Barocde")
			{
				txtBarcode.Visible = visible;
			}
		}
	}
}
