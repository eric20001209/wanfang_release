using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormBOMItem : Form
	{
		public string m_sName = "";
		public double m_dQty = -1;
		public FormBOMItem()
		{
			InitializeComponent();
		}
		private void btnOK_Click(object sender, EventArgs e)
		{
			m_dQty = Program.MyDoubleParse(txtQty.Text);
			Close();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void FormBOMItem_Load(object sender, EventArgs e)
		{
			lblName.Text = m_sName;
			txtQty.Text = m_dQty.ToString();
		}
	}
}
