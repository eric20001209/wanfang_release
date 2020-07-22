using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class FormCurrency : Form
	{
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		private Control m_lastFocused = null;
		
		private double[] m_dRates = new double[64];
		public double m_dOrderTotal = 0;
		public double m_dPaymentTotal = 0;

		public FormCurrency()
		{
			InitializeComponent();
		}
		private void FormCurrency_Load(object sender, EventArgs e)
		{
			int nRows = 0;
			string sc = " SELECT * FROM currency ORDER BY id ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "c");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			
			lblOrderTotal.Text = m_dOrderTotal.ToString("c");
			
			for(int i=0; i<nRows; i++)
			{
				DataRow dr = dst.Tables["c"].Rows[i];
				string name = dr["currency_name"].ToString();
				double dRate = Program.MyDoubleParse(dr["rates"].ToString());

				cb1.Items.Add(name);
				cb2.Items.Add(name);
				cb3.Items.Add(name);
				m_dRates[i] = dRate;
				if(i == 0)
				{
					cb1.Text = name;
					txtRate1.Text = dRate.ToString();
					cb2.Text = name;
					txtRate2.Text = dRate.ToString();
					cb3.Text = name;
					txtRate3.Text = dRate.ToString();
				}
			}
			txtF1.Focus();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_dPaymentTotal = 0;
			this.Close();
		}
		private void CalcAmount()
		{
			m_dPaymentTotal = 0;

			double dRate = Program.MyDoubleParse(txtRate1.Text.Trim());
			double dF = Program.MyDoubleParse(txtF1.Text.Trim());
			double dN = Math.Round(dF / dRate, 2);
			txtNZ1.Text = dN.ToString();
			m_dPaymentTotal += dN;

			dRate = Program.MyDoubleParse(txtRate2.Text.Trim());
			dF = Program.MyDoubleParse(txtF2.Text.Trim());
			dN = Math.Round(dF / dRate, 2);
			txtNZ2.Text = dN.ToString();
			m_dPaymentTotal += dN;

			dRate = Program.MyDoubleParse(txtRate3.Text.Trim());
			dF = Program.MyDoubleParse(txtF3.Text.Trim());
			dN = Math.Round(dF / dRate, 2);
			txtNZ3.Text = dN.ToString();
			m_dPaymentTotal += dN;
			
			double dChange = m_dPaymentTotal - m_dOrderTotal;
			lblPaymentTotal.Text = m_dPaymentTotal.ToString("c");
			lblChange.Text = dChange.ToString("c");
		}
		private void cb1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int n = cb1.SelectedIndex;
			double dRate = m_dRates[n];
			txtRate1.Text = dRate.ToString();
			txtF1.Text = "";

			dRate = Program.MyDoubleParse(txtRate1.Text.Trim());
			double dF = 0;
			dF = Math.Round(m_dOrderTotal * dRate, 2);
			double dN = 0;
			dN = Math.Round(dF / dRate, 2);
			txtF1.Text = dF.ToString();
			CalcAmount();
		}
		private void cb2_SelectedIndexChanged(object sender, EventArgs e)
		{
			int n = cb2.SelectedIndex;
			double dRate = m_dRates[n];
			txtRate2.Text = dRate.ToString();
			CalcAmount();
		}
		private void cb3_SelectedIndexChanged(object sender, EventArgs e)
		{
			int n = cb3.SelectedIndex;
			double dRate = m_dRates[n];
			txtRate3.Text = dRate.ToString();
			CalcAmount();
		}
		private void txtF1_TextChanged(object sender, EventArgs e)
		{
			CalcAmount();
		}
		private void txtRate1_TextChanged(object sender, EventArgs e)
		{
			CalcAmount();
		}
		private void txtRate2_TextChanged(object sender, EventArgs e)
		{
			CalcAmount();
		}
		private void txtF2_TextChanged(object sender, EventArgs e)
		{
			CalcAmount();
		}
		private void txtF3_TextChanged(object sender, EventArgs e)
		{
			CalcAmount();
		}
		private void txtRate3_TextChanged(object sender, EventArgs e)
		{
			CalcAmount();
		}
		private void btnFinish_Click(object sender, EventArgs e)
		{
			string sc = "";
			string name = "";
			double dRate = 0;
			
			name = cb1.Text;
			if(txtNew1.Text.Trim() != "")
				name = txtNew1.Text;
			dRate = Program.MyDoubleParse(txtRate1.Text.Trim());
			
			sc += " IF NOT EXISTS(SELECT rates FROM currency WHERE currency_name = '" + name + "') ";
			sc += " INSERT INTO currency(currency_name, rates, insert_by) VALUES('" + Program.EncodeQuote(name) + "', " + dRate + ", " + Program.m_sSalesId + ") ";
			sc += " ELSE ";
			sc += " UPDATE currency SET rates = " + dRate + " WHERE currency_name = '" + Program.EncodeQuote(name) + "' ";

			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e1)
			{
				myConnection.Close();
				Program.ShowExp(sc, e1);
				return;
			}
			this.Close();
		}
		private void pinpad_Click(object sender, MouseEventArgs e)
		{
			Control kc = txtF1;
			if (m_lastFocused != null)
				kc = m_lastFocused;

			string s = kc.Text.Trim();
			if(((TextBox)kc).SelectedText != "")
				s = "";
			string sn = ((Button)sender).Text;
			if (sn == "." && s.IndexOf(".") >= 0)
				return;
			kc.Text = s + sn;
		}
		private void textBox_Click(object sender, MouseEventArgs e)
		{
			m_lastFocused = (Control)sender;
			((TextBox)sender).SelectAll();
			if (txtF1.Text.Trim() == null || txtF1.Text.Trim() == "")
			{
				double dRate = 0;
				dRate = Program.MyDoubleParse(txtRate1.Text.Trim());
				double dF = 0;
				dF = Math.Round(m_dOrderTotal * dRate,2);
				double dN = 0;
				dN = Math.Round(dF / dRate, 2);
				txtF1.Text = dF.ToString();
			}
			//CalcAmount();
		}
	}
}
