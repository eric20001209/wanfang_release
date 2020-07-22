using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;
using Microsoft.Win32;
using System.Web.Security;
using Sektor.Vault.Common;
using Sektor.Vault.POSInterface;
using ICSharpCode.SharpZipLib.Zip;
using System.Net;

namespace QPOS2008
{
	public partial class FormLevelPrice : Form
	{

		private SqlDataAdapter myAdapter;
		private SqlConnection myConnection;
		private SqlCommand myCommand;
		DataSet dst = new DataSet();
		private string m_code = "";
		private double d_taxrate = 0.15;
		private double d_retail_price_with_gst = 0;
		private double d_retail_price_no_gst = 0;
		
		public FormLevelPrice()
		{
			InitializeComponent();
			
		}

		private void FormLevelPrice_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			m_code	= Program.m_sCode;
			this.checkBox1.Checked = true;
			tblevel1.Focus();
			getlevelprice(m_code);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			updatelevelprice(m_code);
		}

		private bool updatelevelprice(string code)
		{
			string sc = " UPDATE code_relations SET ";
			if(!this.checkBox1.Checked)
			{
				sc += "  level_price1 = N'" + Program.MyMoneyParse(this.tblevel1.Text) + "' ";
				sc += ", level_price2 = N'" + Program.MyMoneyParse(this.tblevel2.Text) + "' ";
				sc += ", level_price3 = N'" + Program.MyMoneyParse(this.tblevel3.Text) + "' ";
				sc += ", level_price4 = N'" + Program.MyMoneyParse(this.tblevel4.Text) + "' ";
				sc += ", level_price5 = N'" + Program.MyMoneyParse(this.tblevel5.Text) + "' ";
				sc += ", level_price6 = N'" + Program.MyMoneyParse(this.tblevel6.Text) + "' ";
			}
			else if (this.checkBox1.Checked)
			{
				sc += "  level_price1 = N'" + Program.MyMoneyParse(this.tblevel1.Text) / (1 + d_taxrate) + "' ";
				sc += ", level_price2 = N'" + Program.MyMoneyParse(this.tblevel2.Text) / (1 + d_taxrate) + "' ";
				sc += ", level_price3 = N'" + Program.MyMoneyParse(this.tblevel3.Text) / (1 + d_taxrate) + "' ";
				sc += ", level_price4 = N'" + Program.MyMoneyParse(this.tblevel4.Text) / (1 + d_taxrate) + "' ";
				sc += ", level_price5 = N'" + Program.MyMoneyParse(this.tblevel5.Text) / (1 + d_taxrate) + "' ";
				sc += ", level_price6 = N'" + Program.MyMoneyParse(this.tblevel6.Text) / (1 + d_taxrate) + "' ";
			}
			sc += " WHERE code = " + code;
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
				Program.ShowExp(sc, e1);
				return false;
			}
			
			FormMSG fm = new FormMSG();
			fm.m_sMsg = "Level Price Updated!";
			fm.btnNo.Visible = false;
			fm.btnYes.Visible = false;
			fm.ShowDialog();
			if (!this.checkBox1.Checked)
			{
				this.lblRetailPrice1.Text = d_retail_price_no_gst.ToString(".00");
				this.lblRetailPrice2.Text = d_retail_price_no_gst.ToString(".00");
				this.lblRetailPrice3.Text = d_retail_price_no_gst.ToString(".00");
				this.lblRetailPrice4.Text = d_retail_price_no_gst.ToString(".00");
				this.lblRetailPrice5.Text = d_retail_price_no_gst.ToString(".00");
				this.lblRetailPrice6.Text = d_retail_price_no_gst.ToString(".00");
				this.lblLevelRate1.Text = (Math.Round(Program.MyMoneyParse(this.tblevel1.Text) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate2.Text = (Math.Round(Program.MyMoneyParse(this.tblevel2.Text) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate3.Text = (Math.Round(Program.MyMoneyParse(this.tblevel3.Text) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate4.Text = (Math.Round(Program.MyMoneyParse(this.tblevel4.Text) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate5.Text = (Math.Round(Program.MyMoneyParse(this.tblevel5.Text) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate6.Text = (Math.Round(Program.MyMoneyParse(this.tblevel6.Text) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
			}
			else if (this.checkBox1.Checked)
			{
				this.lblRetailPrice1.Text = d_retail_price_with_gst.ToString(".00");
				this.lblRetailPrice2.Text = d_retail_price_with_gst.ToString(".00");
				this.lblRetailPrice3.Text = d_retail_price_with_gst.ToString(".00");
				this.lblRetailPrice4.Text = d_retail_price_with_gst.ToString(".00");
				this.lblRetailPrice5.Text = d_retail_price_with_gst.ToString(".00");
				this.lblRetailPrice6.Text = d_retail_price_with_gst.ToString(".00");
				this.lblLevelRate1.Text = (Math.Round(Program.MyMoneyParse(this.tblevel1.Text) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate2.Text = (Math.Round(Program.MyMoneyParse(this.tblevel2.Text) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate3.Text = (Math.Round(Program.MyMoneyParse(this.tblevel3.Text) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate4.Text = (Math.Round(Program.MyMoneyParse(this.tblevel4.Text) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate5.Text = (Math.Round(Program.MyMoneyParse(this.tblevel5.Text) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
				this.lblLevelRate6.Text = (Math.Round(Program.MyMoneyParse(this.tblevel6.Text) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
			}
			return true;
		}

		private void getlevelprice(string code)
		{
			if (dst.Tables["getlevelprice"] != null)
				dst.Tables["getlevelprice"].Clear();
			string sc = "";
			int nRows = 0;
			sc = " SELECT top 1 * FROM code_relations WHERE code = " + code;
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				nRows = myAdapter.Fill(dst, "getlevelprice");
				if(nRows <= 0)
					return;	
				else
				{	
					DataRow dr = dst.Tables["getlevelprice"].Rows[0];
					string level_price1 = dr["level_price1"].ToString();
					string level_price2 = dr["level_price2"].ToString();
					string level_price3 = dr["level_price3"].ToString();
					string level_price4 = dr["level_price4"].ToString();
					string level_price5 = dr["level_price5"].ToString();
					string level_price6 = dr["level_price6"].ToString();
					string tax_rate = dr["tax_rate"].ToString();
					double dtax_rate = Program.MyDoubleParse(tax_rate);
					d_taxrate = dtax_rate;
					d_retail_price_with_gst = Program.MyMoneyParse(dr["price1"].ToString());
					d_retail_price_no_gst = Math.Round(d_retail_price_with_gst / (1 + dtax_rate), 2);
						
					if (!this.checkBox1.Checked)
					{
						this.tblevel1.Text = Program.MyMoneyParse(level_price1).ToString();
						this.tblevel2.Text = Program.MyMoneyParse(level_price2).ToString();
						this.tblevel3.Text = Program.MyMoneyParse(level_price3).ToString();
						this.tblevel4.Text = Program.MyMoneyParse(level_price4).ToString();
						this.tblevel5.Text = Program.MyMoneyParse(level_price5).ToString();
						this.tblevel6.Text = Program.MyMoneyParse(level_price6).ToString();
						this.lblRetailPrice1.Text = d_retail_price_no_gst.ToString(".00");
						this.lblRetailPrice2.Text = d_retail_price_no_gst.ToString(".00");
						this.lblRetailPrice3.Text = d_retail_price_no_gst.ToString(".00");
						this.lblRetailPrice4.Text = d_retail_price_no_gst.ToString(".00");
						this.lblRetailPrice5.Text = d_retail_price_no_gst.ToString(".00");
						this.lblRetailPrice6.Text = d_retail_price_no_gst.ToString(".00");
						this.lblLevelRate1.Text = (Math.Round(Program.MyMoneyParse(level_price1) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate2.Text = (Math.Round(Program.MyMoneyParse(level_price2) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate3.Text = (Math.Round(Program.MyMoneyParse(level_price3) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate4.Text = (Math.Round(Program.MyMoneyParse(level_price4) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate5.Text = (Math.Round(Program.MyMoneyParse(level_price5) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate6.Text = (Math.Round(Program.MyMoneyParse(level_price6) / d_retail_price_no_gst, 2) * 100).ToString() + "%";
					}
					else
					{
						this.tblevel1.Text = Math.Round(Program.MyMoneyParse(level_price1) * (1 + dtax_rate), 2).ToString();
						this.tblevel2.Text = Math.Round(Program.MyMoneyParse(level_price2) * (1 + dtax_rate), 2).ToString();
						this.tblevel3.Text = Math.Round(Program.MyMoneyParse(level_price3) * (1 + dtax_rate), 2).ToString();
						this.tblevel4.Text = Math.Round(Program.MyMoneyParse(level_price4) * (1 + dtax_rate), 2).ToString();
						this.tblevel5.Text = Math.Round(Program.MyMoneyParse(level_price5) * (1 + dtax_rate), 2).ToString();
						this.tblevel6.Text = Math.Round(Program.MyMoneyParse(level_price6) * (1 + dtax_rate), 2).ToString();
						this.lblRetailPrice1.Text = d_retail_price_with_gst.ToString(".00");
						this.lblRetailPrice2.Text = d_retail_price_with_gst.ToString(".00");
						this.lblRetailPrice3.Text = d_retail_price_with_gst.ToString(".00");
						this.lblRetailPrice4.Text = d_retail_price_with_gst.ToString(".00");
						this.lblRetailPrice5.Text = d_retail_price_with_gst.ToString(".00");
						this.lblRetailPrice6.Text = d_retail_price_with_gst.ToString(".00");
						this.lblLevelRate1.Text = (Math.Round(Program.MyMoneyParse(level_price1) * (1 + dtax_rate) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate2.Text = (Math.Round(Program.MyMoneyParse(level_price2) * (1 + dtax_rate) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate3.Text = (Math.Round(Program.MyMoneyParse(level_price3) * (1 + dtax_rate) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate4.Text = (Math.Round(Program.MyMoneyParse(level_price4) * (1 + dtax_rate) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate5.Text = (Math.Round(Program.MyMoneyParse(level_price5) * (1 + dtax_rate) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
						this.lblLevelRate6.Text = (Math.Round(Program.MyMoneyParse(level_price6) * (1 + dtax_rate) / d_retail_price_with_gst, 2) * 100).ToString() + "%";
					}
				}
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return ;
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			getlevelprice(m_code);
		}
	}
}
