using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class AddUser : Form
	{
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		private string dCustomerGst = "0";
		private double m_dCustomerGST = 0;
		public bool m_bUpdate = false;
		public string m_sBarcode = "";
		public string m_sCard_id = "";
		public string m_sTitle = "Add New User ";
		public bool m_bVIP = false;
		private string m_oldBarcode = "";
		private double m_dVipDiscount = 0;
		Control m_hLastFocusedControl;

		public AddUser()
		{
			InitializeComponent();
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			foreach (Control pnl in Controls)
			{
				if (pnl is Panel || pnl is GroupBox || pnl is TabControl)
				{
					foreach (Control ctrl in pnl.Controls)
					{
						if (ctrl is TextBox || ctrl is RichTextBox)
						{
							ctrl.MouseUp += delegate(object sender, MouseEventArgs e)
							{
								m_hLastFocusedControl = (Control)sender;
							};
						}
						else if (ctrl is Panel || ctrl is GroupBox || ctrl is TabPage)
						{
							foreach (Control cctr in ctrl.Controls)
							{
								if (cctr is TextBox || cctr is RichTextBox)
								{
									cctr.MouseUp += delegate(object sender, MouseEventArgs e)
									{
										m_hLastFocusedControl = (Control)sender;
									};
								}
								else if (cctr is Panel || cctr is GroupBox)
								{
									foreach (Control ccct in cctr.Controls)
									{
										if (ccct is TextBox || ccct is RichTextBox)
										{
											ccct.MouseUp += delegate(object sender, MouseEventArgs e)
											{
												m_hLastFocusedControl = (Control)sender;
											};
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private void AddUser_Load(object sender, EventArgs e)
		{
			label2.Text = m_sTitle;
			if (m_bVIP)
			{
				if(!DoGetCat())
					return;
				bindlistdis();
				if (Program.m_bEnableLevelPrice)
				{
					lbPriceLevel.Location = new Point(3, 284);
					lbPriceLevel.Visible = true;
					cbPriceLevel.Location = new Point(149, 284);
					cbPriceLevel.Visible = true;
					lbldiscount.Visible = false;
					txtvipdiscountrate.Visible = false;
				}
				else
				{
					lbldiscount.Location = new Point(3, 284);
					lbldiscount.Visible = true;
					txtvipdiscountrate.Location = new Point(149, 284);
					txtvipdiscountrate.Visible = true;
					lbPriceLevel.Visible = false;
					cbPriceLevel.Visible = false;
				}
				cbLanguage.Visible = false;
				lblLanguage.Visible = false;
				pnlAccess.Visible = false;
                txtPoints.Location = new Point(149, 320);
                txtPoints.Visible = true;
                label6.Location = new Point(15, 320);
                label6.Visible = true;
			}
			else if(Program.m_bAccessAdmin)
			{
				pnlAccess.Visible = true;
			}
			if (m_bUpdate)
			{
				ShowUserInformation();
				btnSave.Text = "Update";
			}
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_bUpdate = false;
			btnSave.Text ="Save";
			Close();
		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			if (txtName.Text == "")
			{
				MessageBox.Show("Please enter Customer Name");
				txtName.Focus();
				return;
			}
			string viprate = txtvipdiscountrate.Text;
			viprate = viprate.Replace("%", "");
			if (viprate.Trim() == "")
				viprate = "0";
			try
			{
				double.Parse(viprate);
				if (Program.MyDoubleParse(viprate) > 1)
				{
					m_dVipDiscount = Program.MyDoubleParse(viprate) / 100;
				}
				else
				{
					m_dVipDiscount = Program.MyDoubleParse(viprate);
				}
			}
			catch (Exception ex)
			{
				string sex = ex.ToString();
				MessageBox.Show("Please type in number only for discount");
				txtvipdiscountrate.Text = "";
				return;
			}
		   
			string sAccessLevel = "8";
			if (this.ckbAdmin.Checked)
				sAccessLevel = "10";
			if(btnSave.Text == "Save")
			{
				if (bBarcodeExist(txtBarcode.Text))
				{
					MessageBox.Show("Error, Duplicate barcode");
					return;
				}

				string cardID = "";
				if (dst.Tables["addnewcustomer"] != null)
					dst.Tables["addnewcustomer"].Clear();
				string sc = " INSERT INTO card (name, email, phone, mobile, address1, address2, address3, barcode ";
				sc += ", type, gst_rate, access_level, password, m_discount_rate, discount, price_level ";
				sc += ", access_delete_item, access_refund, access_discount, access_cashdraw, access_x_total, access_report ";
				sc += ", access_vip_payment, access_product, access_stock, access_setting, access_database, access_admin_zone ";
				sc += ", trading_name, language) VALUES(";
				sc += " N'" + Program.EncodeQuote(txtName.Text) + "' ";
				sc += ", N'" + Program.EncodeQuote(txtBarcode.Text) + "' ";
				sc += ", N'" + Program.EncodeQuote(txtPhone.Text) + "' ";
				sc += ", N'" + Program.EncodeQuote(txtMobile.Text) + "' ";
				sc += ", N'" + Program.EncodeQuote(txtAddress1.Text) + "' ";
				sc += ", N'" + Program.EncodeQuote(txtAddress2.Text) + "' ";
				sc += ", N'" + Program.EncodeQuote(txtAddress3.Text) + "' ";
				sc += ", N'" + Program.EncodeQuote(txtBarcode.Text) + "' ";
				if (m_bVIP)
				{
					sc += ", 6 ";
					sc += ", " + GetSysGstRate() + " ";
					sc += ", 0 ";
					sc += ", '" + Program.md5(System.DateTime.Now.ToOADate().ToString()) + "'";
					sc += ", '" + m_dVipDiscount.ToString() + "'";
					sc += ", '" + m_dVipDiscount.ToString() + "'";
					if (cbPriceLevel.Text == "")
						cbPriceLevel.Text = "0";
					sc += ", '" + cbPriceLevel.Text + "'";
					sc += ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ";
				}
				else
				{
					sc += ", 4";
					sc += ", " + GetSysGstRate() + " ";
					sc += ", " + sAccessLevel + " ";
					sc += ", '" + Program.md5(System.DateTime.Now.ToOADate().ToString()) + "'";
					sc += ", ''";
					sc += ", ''";
					sc += ", 0";
					
					sc += ", '" + this.ckbDeleteItem.Checked + "'";
					sc += ", '" + this.ckbRefund.Checked + "'";
					sc += ", '" + this.ckbDiscount.Checked + "'";
					sc += ", '" + this.ckbCashdraw.Checked + "'";
					sc += ", '" + this.ckbXTotal.Checked + "'";
					sc += ", '" + this.ckbReport.Checked + "'";
					sc += ", '" + this.ckbVipPayment.Checked + "'";
					sc += ", '" + this.ckbProduct.Checked + "'";
					sc += ", '" + this.ckbStock.Checked + "'";
					sc += ", '" + this.ckbSetting.Checked + "'";
					sc += ", '" + this.ckbDatabase.Checked + "'";
					sc += ", '" + this.ckbAdminZone.Checked + "'";
				}
				sc += ",'" + Program.EncodeQuote(txtcompany.Text) + "'";
				if (this.cbLanguage.Text == "English")
					sc += ", 2";
				else if (this.cbLanguage.Text == "Others")
					sc += ", 1";
				else
					sc += ", 0";
				sc += ") ";
				sc += " SELECT IDENT_CURRENT('card') AS id";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "addnewcustomer") <= 0)
						return;
					else
						cardID = dst.Tables["addnewcustomer"].Rows[0]["id"].ToString();
				}
				catch (Exception e1)
				{
					Program.ShowExp(sc, e1);
					return;
				}
				m_sBarcode = cardID;
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = " User has been added";
				fm.ShowDialog();
				Close();
			}
			else if(btnSave.Text == "Update")
			{
				if (m_oldBarcode != Program.EncodeQuote(txtBarcode.Text))
				{
					if (bBarcodeExist(txtBarcode.Text))
					{
						MessageBox.Show("Error, Duplicate barcode");
						
						return;
					}   
				}

				string type = "";
				string sc1 = " Select type from card where id = '" + txtid.Text + "'";
				try
				{
					myAdapter = new SqlDataAdapter(sc1, myConnection);
					if (myAdapter.Fill(dst, "gettype") <= 0)
						return;
					else
						type = dst.Tables["gettype"].Rows[0]["type"].ToString();
				}
				catch (Exception e1)
				{
					Program.ShowExp(sc1, e1);
					return;
				}
				
				string sc = " UPDATE card SET address1 = N'"+Program.EncodeQuote(txtAddress1.Text)+"'";
				sc += " , address2 = N'"+Program.EncodeQuote(txtAddress2.Text)+"'";
				sc += " , name = N'" + txtName.Text + "'";
				sc += " , address3 = N'"+Program.EncodeQuote(txtAddress3.Text)+"'";
				sc += " , phone = '"+Program.EncodeQuote(txtPhone.Text)+"'";
				sc += " , mobile = '"+Program.EncodeQuote(txtMobile.Text)+"'";
				sc += " , barcode ='"+Program.EncodeQuote(txtBarcode.Text)+"'";
				sc += " , discount ='" + m_dVipDiscount.ToString() + "'";
				sc += " , m_discount_rate ='" + m_dVipDiscount.ToString() + "'";

				sc += " , price_level = '" + Program.EncodeQuote(this.cbPriceLevel.Text) + "'";
				
				sc += " , trading_name  ='" + Program.EncodeQuote(txtcompany.Text) + "'";
				if(cbLanguage.Text == "English")
					sc += " , language  ='2'";
				else if (cbLanguage.Text == "Others")
					sc += " , language  ='1'";
				else
					sc += " , language  ='0'";
				if(type=="4")
				{
					sc += ", access_level = " + sAccessLevel;
					sc += ", access_delete_item = '" + this.ckbDeleteItem.Checked + "'";
					sc += ", access_refund = '" + this.ckbRefund.Checked + "'";
					sc += ", access_discount = '" + this.ckbDiscount.Checked + "'";
					sc += ", access_cashdraw = '" + this.ckbCashdraw.Checked + "'";
					sc += ", access_x_total = '" + this.ckbXTotal.Checked + "'";
					sc += ", access_report = '" + this.ckbReport.Checked + "'";
					sc += ", access_vip_payment = '" + this.ckbVipPayment.Checked + "'";
					sc += ", access_product = '" + this.ckbProduct.Checked + "'";
					sc += ", access_stock = '" + this.ckbStock.Checked + "'";
					sc += ", access_setting = '" + this.ckbSetting.Checked + "'";
					sc += ", access_database = '" + this.ckbDatabase.Checked + "'";
					sc += ", access_admin_zone = '" + this.ckbAdminZone.Checked + "'";
				}
				sc += " WHERE id ='" + txtid.Text + "'";
				try
				{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
				}
				catch (Exception ex)
				{
					Program.ShowExp(sc, ex);
					return;
				}
				FormMSG fm = new FormMSG();
				fm.btnNo.Visible = false;
				fm.btnYes.Visible = false;
				fm.m_sMsg = " User detail updated ";
				fm.ShowDialog();
				Close();
			}
		}
		private double GetSysGstRate()
		{
			if (dst.Tables["gstsetting"] != null)
				dst.Tables["gstsetting"].Clear();
			string sc = " SELECT value FROM settings WHERE name='gst_rate_percent'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "getsetting") <= 0)
					return 0;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				return 0;
			}
			if (dst.Tables["getsetting"].Rows.Count > 0)
			{
				DataRow dr = dst.Tables["getsetting"].Rows[0];
				dCustomerGst = dr["value"].ToString();
			}

			if (dCustomerGst != "")
			{
				if (double.Parse(dCustomerGst) > 1)
				{
					m_dCustomerGST = Program.MyDoubleParse(dCustomerGst);
					m_dCustomerGST = m_dCustomerGST / 100;
				}
				dCustomerGst = m_dCustomerGST.ToString();
			}
			return Program.MyDoubleParse(dCustomerGst);
		}
		private void ShowUserInformation()
		{
			if (dst.Tables["user"] != null)
				dst.Tables["user"].Clear();
			string sc = " SELECT  * FROM card WHERE 1 = 1 ";
			if (m_sBarcode != "")
				sc += " AND barcode = '" + m_sBarcode + "' ";
			else if (m_sCard_id != "")
				sc += " AND id = '" + m_sCard_id + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "user") <= 0)
				{
					FormMSG fm = new FormMSG();
					fm.btnYes.Visible = false;
					fm.btnNo.Visible = false;
					fm.m_sMsg = " User Not Found ";
					fm.ShowDialog();
					return;
				}
			   
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}
			DataRow dr = dst.Tables["user"].Rows[0];
			int nAccessLevel = Program.MyIntParse(dr["access_level"].ToString());
			if (nAccessLevel >= 10)
			{
				ckbAdmin.Checked = true;
			}
			else
			{
				ckbAdmin.Checked = false;
				ckbDeleteItem.Checked = Program.MyBooleanParse(dr["access_delete_item"].ToString());
				ckbRefund.Checked = Program.MyBooleanParse(dr["access_refund"].ToString());
				ckbDiscount.Checked = Program.MyBooleanParse(dr["access_discount"].ToString());
				ckbCashdraw.Checked = Program.MyBooleanParse(dr["access_cashdraw"].ToString());
				ckbXTotal.Checked = Program.MyBooleanParse(dr["access_x_total"].ToString());
				ckbReport.Checked = Program.MyBooleanParse(dr["access_report"].ToString());
				ckbVipPayment.Checked = Program.MyBooleanParse(dr["access_vip_payment"].ToString());
				ckbProduct.Checked = Program.MyBooleanParse(dr["access_product"].ToString());
				ckbStock.Checked = Program.MyBooleanParse(dr["access_stock"].ToString());
				ckbSetting.Checked = Program.MyBooleanParse(dr["access_setting"].ToString());
				ckbDatabase.Checked = Program.MyBooleanParse(dr["access_database"].ToString());
				ckbAdminZone.Checked = Program.MyBooleanParse(dr["access_admin_zone"].ToString());
			}			

			if (dr["name"].ToString().ToLower() == "admin")
				txtName.ReadOnly = true;
			txtAddress1.Text = dr["address1"].ToString();
			txtAddress2.Text = dr["address2"].ToString();
			txtAddress3.Text = dr["address3"].ToString();
			txtName.Text = dr["name"].ToString();
			txtBarcode.Text = dr["barcode"].ToString();
			txtMobile.Text = dr["mobile"].ToString();
			txtPhone.Text = dr["phone"].ToString();
			txtid.Text = dr["id"].ToString();
			m_oldBarcode = dr["barcode"].ToString();
			cbPriceLevel.Text = dr["price_level"].ToString();
            txtPoints.Text = dr["points"].ToString();
			txtvipdiscountrate.Text = Program.MyDoubleParse(dr["discount"].ToString()).ToString("p");
			string lang = dr["language"].ToString();
			if (lang == "2")
				cbLanguage.Text = "English";
			else if (lang == "1")
				cbLanguage.Text = "Others";
			else
				cbLanguage.Text = "";
			if (!m_bVIP)
			{
				txtcompany.Text = GetCompanyInfo("trading_name");
				txtcompany.Enabled = false;
			}
			else
			{
				txtcompany.Enabled = true;
				txtcompany.Text = dr["trading_name"].ToString();
			}
		}
		private bool DoGetCat()
		{
			if (dst.Tables["getCat"] != null)
				dst.Tables["getCat"].Clear();
			string sc = "";
			sc += " SELECT c.cat FROM code_relations c JOIN catalog ca ON c.cat = ca.cat GROUP BY c.cat, ca.seq ORDER BY ABS(ca.seq)";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "getCat")<=0)
					return true;
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			this.cbcat.Items.Clear();
			for (int i = 0; i < dst.Tables["getCat"].Rows.Count; i++)
			{
				DataRow dr = dst.Tables["getCat"].Rows[i];
				string cat = dr["cat"].ToString();
				cbcat.Items.Add(cat);
			}
			return true;
		}
		private bool bBarcodeExist(string barcode)
		{
			if (dst.Tables["checkbarcode"] != null)
				dst.Tables["checkbarcode"].Clear();
			string sc = " SELECT * FROM card WHERE barcode ='" + barcode + "'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "checkbarcode") <= 0)
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}

			txtBarcode.Text = "";
			return true;
		}
		private string GetCompanyInfo(string field)
		{
			if (dst.Tables["company"] != null)
				dst.Tables["company"].Clear();
			string sc = "SELECT  " + field + " FROM card WHERE id = 1";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "company") <= 0)
					return "";
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return "";
			}
			return dst.Tables["company"].Rows[0][field].ToString();
		}
		private void bindlistdis()
		{
			int rows = 0;
			if (dst.Tables["bindlistdis"] != null)
				dst.Tables["bindlistdis"].Clear();
			string sc = "SELECT * FROM cat_dis WHERE card_id = '" + m_sCard_id +"'";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				rows = myAdapter.Fill(dst, "bindlistdis");
				if (myAdapter.Fill(dst, "bindlistdis") < 0)
					return ;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return ;
			}

			this.lvCatDis.Items.Clear();
			for (int i = 0; i < rows; i++)
			{
				DataRow dr = dst.Tables["bindlistdis"].Rows[i];
				string id = dr["id"].ToString();
				string cat = dr["cat"].ToString();
				string dis_rate = dr["dis_rate"].ToString();
				dis_rate = (Program.MyDoubleParse(dis_rate)*100).ToString() + "%";

				ListViewItem item = new ListViewItem(id);
				item.SubItems.Add(cat);
				item.SubItems.Add(dis_rate);
				item.SubItems.Add("del");
				lvCatDis.Items.Add(item);
			}
			this.cbcat.Text = "";
			this.txtDis.Text = "";
		}
		bool DoAddCatDis()
		{
			string cat = this.cbcat.Text;

			string sc = " IF NOT EXISTS ";
			sc += " (SELECT * FROM cat_dis WHERE card_id = '" + txtid.Text + "' AND cat='" + Program.EncodeQuote(cat) + "') ";
			sc += " INSERT INTO cat_dis (card_id, cat, dis_rate) VALUES( ";
			sc += "'" + txtid.Text + "'";
			sc += ", N'" + Program.EncodeQuote(cat) + "' ";
			sc += ", " + Program.MyDoubleParse(this.txtDis.Text)/100;
			sc += ") ";
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return false;
			}
			return true;
		}
		private void button1_Click(object sender, EventArgs e)
		{
			DoAddCatDis();
			bindlistdis();
		}
		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
		private void lvCatDis_MouseClick(object sender, MouseEventArgs e)
		{
			ListView.SelectedListViewItemCollection items = this.lvCatDis.SelectedItems;
			if (items.Count <= 0)
				return;
			if (items[0].GetSubItemAt(e.X, e.Y).Text != "del")
				return;
			string id = items[0].SubItems[0].Text;
			//			if (barcode.Trim() == "")
			//				return;
			if (MessageBox.Show("Do you want to delete this record? click Yes to delete", "Confirm deleting", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return;
			}
			DoDeleteDis(id);
		}
		private void DoDeleteDis(string id)
		{
			string sc = " DELETE FROM cat_dis WHERE id = " + id ;
			try
			{
				myCommand = new SqlCommand(sc);
				myCommand.Connection = myConnection;
				myCommand.Connection.Open();
				myCommand.ExecuteNonQuery();
				myCommand.Connection.Close();
			}
			catch (Exception e)
			{
				Program.ShowExp(sc, e);
				return;
			}
			bindlistdis();
		}
		public void ReceiveKeyboardKey(string s)
		{
			Control c = m_hLastFocusedControl;
			if (c == null)
				c = Program.FindFocusedControl(this);
			if (c == null)
				return;

			int p = 0;
			if (c is TextBox)
				p = ((TextBox)c).SelectionStart;
			else if (c is RichTextBox)
				p = ((RichTextBox)c).SelectionStart;
			string sf = c.Text.Substring(0, p);
			string sb = c.Text.Substring(p, c.Text.Length - p);
			string sr = "";
			int pNew = p;
			if (s == "del")
			{
				sr = sf.Substring(0, sf.Length - 1) + sb;
				pNew = p - 1;
			}
			else
			{
				sr = sf + s + sb;
				pNew = p + s.Length;
			}
			c.Text = sr;
			if (c is TextBox)
				((TextBox)c).SelectionStart = pNew;
			else if (c is RichTextBox)
				((RichTextBox)c).SelectionStart = pNew;
		}
		private void btnKeyboard_Click(object sender, EventArgs e)
		{
			FormKeyboard fm = new FormKeyboard();
			fm.m_hAddUser = this;
			fm.m_x = this.Bounds.X + 20;
			fm.m_y = this.Bounds.Y + 370;
			fm.Show();
		}
	}
}
