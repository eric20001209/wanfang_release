using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
    public partial class FormMSG : Form
    {
        public string m_sMsg = "";
        public string m_sbuttonFocus = "";
        public string m_PaymentAccepted = "";
        public string m_sconfirm = "0";
        public string m_showconf = "0";
		public string m_sCreateNewItem = "";
		public string m_sYesNo = "";
		public bool m_bYes = false;

        public FormMSG()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ePaymentAccept();
            if(m_sconfirm == "1")
				m_sconfirm = "1";
        }

        private void FormMSG_Load(object sender, EventArgs e)
        {
			if (m_sYesNo == "1")
			{
				this.btnYes.Visible = true;
				this.btnYes.Visible = true;
				this.btnOK.Visible = false;
				m_sYesNo = "0";
			}

            this.FormBorderStyle = FormBorderStyle.None;
           // if(m_sconfirm == "1")
			//	this.button2.Visible = true;
            this.txtmsg.Text = m_sMsg;
            if (m_sbuttonFocus == "2")
            {
                this.btnOK.Visible = false;
            }
            else
            {
                if (m_sbuttonFocus == "")
                    this.txt.Focus();
                else
                    this.btnOK.Focus();
            }
            if (m_sMsg == "ACCEPTED")
                m_PaymentAccepted = "1";
        }
        private void ePaymentAccept()
        {
            if (m_sMsg == "ACCEPTED")
                m_PaymentAccepted = "1";
            this.Close();
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
		private void button2_Click(object sender, EventArgs e)
		{
			m_showconf = "1";
			this.Close();
		}
		private void btnYes_Click(object sender, EventArgs e)
		{
			m_sCreateNewItem = "1";
			m_bYes = true;
			this.Close();
		}
		private void btnNo_Click(object sender, EventArgs e)
		{
			this.Close();
		}
    }
}
