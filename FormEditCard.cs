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
	public partial class FormEditCard : Form
	{
		public string m_sCardId = "";
		public string m_sBarcode = "";
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		public FormEditCard()
		{
			InitializeComponent();
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
		}
		private void FormEditCard_Load(object sender, EventArgs e)
		{
			LoadCardData();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			m_sCardId = "";
			Close();
		}
		private bool LoadCardData()
		{
			if(Program.MyIntParse(m_sCardId) == 0)
				return true;
			string sc = " SELECT * FROM card WHERE id = " + m_sCardId;
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "vip") <= 0)
				{
					MessageBox.Show("vip not found");
				}
			}
			catch (Exception e1)
			{
				Program.ShowExp(sc, e1);
				return false;
			}
			DataRow dr = dst.Tables["vip"].Rows[0];
			txtName.Text = dr["name"].ToString();
			txtBarcode.Text = dr["barcode"].ToString();
			txtEmail.Text = dr["email"].ToString();
			txtPhone.Text = dr["phone"].ToString();
			txtAddress1.Text = dr["address1"].ToString();
			txtAddress2.Text = dr["address2"].ToString();
			txtAddress3.Text = dr["address3"].ToString();
			return true;
		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			DoUpdateCard();
		}
		private bool DoUpdateCard()
		{
			if(txtBarcode.Text.Trim() == "")
			{
				MessageBox.Show("Please enter VIP barcode");
				txtBarcode.Focus();
				return false;
			}
			string sc = "";
			if(Program.MyIntParse(m_sCardId) == 0)
			{
				sc = " SELECT id FROM card WHERE barcode = '" + Program.EncodeQuote(txtBarcode.Text) + "' ";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if(myAdapter.Fill(dst, "barcode") > 0)
					{
						DialogResult ret = MessageBox.Show("This barcode already exists, do you want to select this customer?", "Baroce Exists", MessageBoxButtons.YesNo);
						if(ret == DialogResult.Yes)
						{
							m_sCardId = dst.Tables["barcode"].Rows[0]["id"].ToString();
							m_sBarcode = txtBarcode.Text;
							Close();
							return true;
						}
					}
				}
				catch (Exception e1)
				{
					Program.ShowExp(sc, e1);
					return false;
				}
				sc = " BEGIN TRANSACTION ";
				sc += " INSERT INTO card(email, type) VALUES('" + Program.EncodeQuote(txtEmail.Text) + "', 6) ";
				sc += " SELECT IDENT_CURRENT('card') AS id ";
				sc += " COMMIT ";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					myAdapter.Fill(dst, "card");
				}
				catch (Exception e1)
				{
					Program.ShowExp(sc, e1);
					return false;
				}
				m_sCardId = dst.Tables["card"].Rows[0]["id"].ToString();
			}
			
			sc = " UPDATE card SET barcode = '" + Program.EncodeQuote(txtBarcode.Text) + "' ";
			sc += ", name = N'" + Program.EncodeQuote(txtName.Text) + "' ";
			sc += ", email = '" + Program.EncodeQuote(txtEmail.Text) + "' ";
			sc += ", phone = N'" + Program.EncodeQuote(txtPhone.Text) + "' ";
			sc += ", address1 = N'" + Program.EncodeQuote(txtAddress1.Text) + "' ";			
			sc += ", address2 = N'" + Program.EncodeQuote(txtAddress2.Text) + "' ";			
			sc += ", address3 = N'" + Program.EncodeQuote(txtAddress3.Text) + "' ";			
			sc += " WHERE id = " + m_sCardId;
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
			m_sBarcode = txtBarcode.Text;
			MessageBox.Show("VIP data successfully saved");
			Close();
			return true;
		}
	}
}
