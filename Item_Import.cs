using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
//using System.Data.SqlClient;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class Item_Import : Form
	{
		private SqlConnection myConnection;
		private SqlDataAdapter myAdapter;
		private SqlCommand myCommand;
		private DataSet dst = new DataSet();
		private string Folder = Directory.GetCurrentDirectory() + "\\" + "item\\";       
		private string fileNamePath = "";
		private string[] csvFileIndex = new string[20];
		private int viewCols = 0;
		private string[] selectColName = new string[20];
		private string[] selectColNameDB = new string[20];
		private string[] selectColIndex = new string[20];
		private string[,] m_av = new string[99999,20];
		private int m_nRows = 0;

		public Item_Import()
		{
			InitializeComponent();
		}
		private void Item_Import_Load(object sender, EventArgs e)
		{
//			myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
			doCheckFolder();
			
			progressBar.Style = ProgressBarStyle.Continuous;
			progressBar.Maximum = 100;
			progressBar.Value = 0;
			
			pnlMapping.Location = new Point(0, 49);
			pnlMapping.Size = new Size(897, 584);
			pnltest.Location = new Point(0, 49);
			pnltest.Size = new Size(897, 584);
			pnlprocess.Location = new Point(0, 49);
			pnlprocess.Size = new Size(897, 584);
			btnrestart.BringToFront();
		}
		private void doShowColumn()
		{
			if (dst.Tables["importcolumn"] != null)
				dst.Tables["importcolumn"].Clear();
			string sc = "SELECT * FROM import_item_format WHERE 1=1";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				myAdapter.Fill(dst, "importcolumn");
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return;
			}

			int i = 0;
			int e = 0;
			int n = 0;
			int d = 0;
			foreach (DataColumn col in dst.Tables["importcolumn"].Columns)
			{
				ColumnHeader CH = new ColumnHeader();
				CH.Text = col.Caption;
				if (CH.Text == "id" || CH.Text == "file_name" || CH.Text == "sub_cat_seperator" || CH.Text == "ss_cat" || CH.Text == "sub_cat_seperator"
					|| CH.Text == "price4" || CH.Text == "price5" || CH.Text == "price6" || CH.Text == "lines_to_skip" || CH.Text == "product_update" || CH.Text == "branch" || CH.Text == "spec"
					|| CH.Text == "service_item" || CH.Text == "price3" || CH.Text == "eta" || CH.Text == "average_cost" || CH.Text == "expire_date" || CH.Text == "location" || CH.Text == "stock"
					|| CH.Text == "price2" ||CH.Text == "supplier" || CH.Text.ToLower() == "m_pn" || CH.Text == "ref_code"  
					)
					continue;
				if (CH.Text == "department")
				{
					CH.Text = "Department";
					d = i;
				}
				if (CH.Text == "name_cn")
				{
					CH.Text = "button name";
					n = i;
				}
				if (CH.Text == "cat")
				{
					CH.Text = "Catalog";
				}
				if (CH.Text == "s_cat")
				{
					CH.Text = "SubCat";
				}
				if (CH.Text == "barcode")
				{
					e = i;
				}
				if (CH.Text == "last_cost")
				{
					CH.Text = "Cost (Ext GST)";
				}
				if (CH.Text == "price1")
				{
					CH.Text = "Price (Inc GST)";
				}
				/***************************/
				if (CH.Text == "level_price1")
				{
					CH.Text = "Level Price 1";
				}
				if (CH.Text == "level_price2")
				{
					CH.Text = "Level Price 2";
				}
				if (CH.Text == "level_price3")
				{
					CH.Text = "Level Price 3";
				}
				if (CH.Text == "level_price4")
				{
					CH.Text = "Level Price 4";
				}
				if (CH.Text == "level_price5")
				{
					CH.Text = "Level Price 5";
				}
				if (CH.Text == "level_price6")
				{
					CH.Text = "Level Price 6";
				}
				/***************************/
				if (CH.Text == "tax_rate")
				{
					CH.Text = "Tax Rate";
				}
				dgvimport.Rows.Add(CH.Text.ToUpper(), "", "", col.Caption);
				if (n != 0)
					dgvimport.Rows[n].Cells["ii_name"].Style.ForeColor = System.Drawing.Color.Red;
				if (d != 0)
					dgvimport.Rows[d].Cells["ii_name"].Style.ForeColor = System.Drawing.Color.Red;
				if (e == 0)
					dgvimport.Rows[e].Cells["ii_name"].Style.ForeColor = System.Drawing.Color.Red;
				i++;
			}
		}
		private void doCheckFolder()
		{
			if (!Directory.Exists(Folder))
			{
				try
				{
					Directory.CreateDirectory(Folder);
				}
				catch (Exception ex)
				{
					Program.ShowExp("", ex);
					return;
				}
			}
		}
		private void btnImport_Click(object sender, EventArgs e)
		{
			OpenFileDialog OFD = new OpenFileDialog();
			OFD.Title = "Import Item";
			OFD.InitialDirectory = @"Desktop\";
			OFD.Filter = "All Files (*.*)|*.*|CSV (Comma delimited) (*.csv)|*.csv|Text Files(*.txt)|*.txt";
			OFD.FilterIndex = 1;
			OFD.RestoreDirectory = true;
			if (OFD.ShowDialog() == DialogResult.OK)
			{
				txtfilename.Text = OFD.FileName;
				fileNamePath = @"" + txtfilename.Text + "";
				txtUploadFileName.Text = Path.GetFileName(fileNamePath);
			}
			else
			{
				return;
			}
			dgvimport.Rows.Clear();
			doShowColumn();
			ii_mapto.Items.Clear();
			btncopy_Click(null, null);
			pnlMapping.Visible = true;
			btnrestart.Visible = true;
		}
		private bool DoCopyFile()
		{
				try
				{
					if (Directory.Exists(Folder))
					{
						if (File.Exists(Folder + txtUploadFileName.Text))
						{
						   File.Delete(Folder + txtUploadFileName.Text);
						}
						
						File.Copy(@"" + txtfilename.Text + "", @"" + Folder + txtUploadFileName.Text + "");
					}
					else
					{
						MessageBox.Show(" Cannot found '" + Folder + "'. Please check device ");
						return false;
					}

				}
				catch (Exception ex)
				{
					string sex = ex.ToString();
					MessageBox.Show("Cannot found '" + Folder + "'. Please check device ");
					return true;
				}
				txtUploadFileName.Visible = true;
				txtfilename.Text = "";
				return true;
		}
		private void btncopy_Click(object sender, EventArgs e)
		{
			string path = Folder + txtUploadFileName.Text;
			if (DoCopyFile())
			{
				MessageBox.Show("Upload File Done, Next is mapping import column");
				loadCSV(path);
			}
		}
		public List<string[]> loadCSV(string path)
		{
			List<string[]> csvData = new List<string[]>();
			int rows = 1;
			try
			{
				using (StreamReader readFile = new StreamReader(path))
				{
					string line;
					string[] row;
					while ((line = readFile.ReadLine()) != null && rows == 1)
					{
						if (rows > 1)
							break;
						rows++;
						
						row = Program.MySplitCSV(line);
						csvFileIndex = row;
						int fileHeader = row.Length;
						for (int i = 0; i < fileHeader; i++)
						{
							ii_mapto.Items.Add(row[i]);
						}
					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			return csvData;
		}
		private void btnTest_Click(object sender, EventArgs e)
		{
			if (lv_list.Columns.Count > 0)
			{
				lv_list.Columns.Clear();
				lv_list.Items.Clear();
				Array.Clear(selectColName, 0, 20);
				Array.Clear(selectColNameDB, 0, 20);
				Array.Clear(selectColIndex, 0, 20);
				viewCols = 0;
			}
			doTest();
			btntestnext.Enabled = true;
		}
		private void doTest()
		{
			int rows = dgvimport.Rows.Count;
			string path = Folder + txtUploadFileName.Text;
			
			for (int i = 0; i < rows; i++)
			{
				string col = dgvimport.Rows[i].Cells["ii_name"].Value.ToString();
				string colv = "";

				if (dgvimport.Rows[i].Cells["ii_mapto"].Value == null)
					continue;
				else
					colv = dgvimport.Rows[i].Cells["ii_mapto"].Value.ToString();
				 
				if (colv.Trim() != "")
				{
					ColumnHeader header = new ColumnHeader();
					header.Width = col.Length*18;
					header.Text = col;
					header.Name = col;
					lv_list.Columns.Add(header);
					string sName = dgvimport.Rows[i].Cells["ii_mapto"].Value.ToString();
					string sNameDB = dgvimport.Rows[i].Cells["ii_name_db"].Value.ToString();
					if(sNameDB == "description")
						sNameDB = "name";
					else if(sNameDB == "avg_cost")
						sNameDB = "manual_cost_frd";
					
					selectColName[viewCols] += sName;
					selectColNameDB[viewCols] += sNameDB;
					
					for(int m=0; m<ii_mapto.Items.Count; m++)
					{
						if(ii_mapto.Items[m].ToString() == sName)
						{
							selectColIndex[viewCols] = m.ToString();
							break;
						}
					}
					viewCols++;
				}
			}
			testCSV();
		}
//		public List<string[]> testCSV()
		public void testCSV()
		{		
			string path = Folder + txtUploadFileName.Text;
			int lineCount = 0;
			using (var reader = File.OpenText(path))
			{
				while (reader.ReadLine() != null)
				{
					lineCount++;
				}
			}
			if(lineCount >= 99999)
			{
				MessageBox.Show("Too many rows in file, please split it, 99998 lines max per file.");
				return;
			}			
			int rows = 0;
			int nCols = 0;
			string skipedItem = "";
			try
			{
				using (StreamReader readFile = new StreamReader(path))
				{
					string line;
					string[] row;
				
					while ((line = readFile.ReadLine()) != null )
					{
						rows++;
//						if(rows > 10000)
//							break;
						if(rows % 100 == 0 || rows >= lineCount)
						{
							lblRows.Text = rows.ToString();
							double d = (double)rows / ((double)lineCount / 100);
							int n = (int)d;
							if(n > 100)
								n = 100;
							progressBar.Value = n;
							lblRows.Refresh();
							progressBar.Refresh();
						}
						if (rows == 1)
						{
							string[] aCol = Program.MySplitCSV(line);
							nCols = aCol.Length;
							continue;
						}
/*							
						if(line.IndexOf("\"")>0)
						{
							skipedItem += line + "\r\n";
							continue;
						}
*/					   
//						row = line.Split(',');
						row = Program.MySplitCSV(line);
						if(row.Length != nCols)
						{
							skipedItem += line + "\r\n";
							break;
						}
						
						//store data in memory
						for (int r = 0; r < viewCols; r++)
						{
							for (int i = 0; i < nCols; i++)
							{
								if (csvFileIndex[i] == selectColName[r])
								{
									m_av[m_nRows, r] = row[i];
								}
							}
						}
						m_nRows++;
						
						//display example
						if(rows <= 100)
						{
							ListViewItem item = new ListViewItem();
							for (int r = 0; r < viewCols; r++)
							{
								for (int i = 0; i < nCols; i++)
								{
									if (csvFileIndex[i] == selectColName[r])
									{
										if (r != 0)
										{
											item.SubItems.Add(row[i]);
										}
										else
										{
											item.Text = row[i];
										}
									}
								}
							}
							lv_list.Items.Add(item);
//							lv_list.Refresh();
						}
					}
					if (skipedItem.Trim() != "")
					{
						MessageBox.Show("Incorrect data found on imported file, click ok to view the error");
						MessageBox.Show("The error on cell which might be including illegal characters. Such as comma or item row column does "+ "\r\n"+ "not match heading" + "\r\n\r\n"  + 
							skipedItem + "\r\n\r\n"+
							"Those item(s) will be skip if you process the this file. Alternately, edit file then import again");
					}
					else
					{
						string msg = "File is good, " + rows + " rows successfully processed.";
						if(rows > 100)
							msg += "\r\nTop 100 rows displayed. ";
						MessageBox.Show(msg);
					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			return;
		}
		private bool barcodeExists(string barcode)
		{
			if (dst.Tables["barcodeExists"] != null)
				dst.Tables["barcodeExists"].Clear();
			string sc = "";
			if (barcode == "")
				return true;
			barcode = Program.EncodeQuote(barcode);
			sc = " SELECT * FROM barcode WHERE barcode = N'" + barcode + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "barcodeExists") > 0)
					return true;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			sc = " SELECT * FROM code_relations WHERE barcode = N'" + barcode + "' ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if(myAdapter.Fill(dst, "barcodeExists") > 0)
					return true;
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return false;
			}
			return false;
		}
		private int GetNewCode()
		{
			if (dst.Tables["selectcode"] != null)
				dst.Tables["selectcode"].Clear();
			int nCode = 1020;
			string sc = " SELECT TOP 1 code FROM code_relations WHERE code > " + nCode + " ORDER BY code DESC ";
			try
			{
				myAdapter = new SqlDataAdapter(sc, myConnection);
				if (myAdapter.Fill(dst, "selectcode") == 1)
				{
					nCode = Program.MyIntParse(dst.Tables["selectcode"].Rows[0]["code"].ToString());
				}
			}
			catch (Exception ex)
			{
				Program.ShowExp(sc, ex);
				myConnection.Close();
				return 0;
			}
			nCode++;
			return nCode;
		}
		private bool doImport()
		{
			if (m_nRows <= 0)
			{
				MessageBox.Show("Please check imported file");
				return false;
			}
			int nCode = GetNewCode();
			if(nCode <= 0)
				return false;
			string sMsg = "";
			int nSkip = 0;
			int nExists = 0;
			int nNew = 0;
			string sc = "";

			string code = "";
			string mpn = "";
			string barcode = "";
			string name_cn = "";
			string name = "";
			string is_addup = "";
			string cost = "";
			string cat = "";
			string s_cat = "";
			string brand = "";
			string price1 = "";
			string stock = "";
			string has_scale = "";
			string level_price1 = "";
			string level_price2 = "";
			string level_price3 = "";
			string level_price4 = "";
			string level_price5 = "";
			string level_price6 = "";
			string tax_rate = "";

			for (int i = 0; i < m_nRows; i++)
			{
				code = "";
				mpn = "";
				barcode = "";
				name_cn = "";
				name = "";
				is_addup = "0";
				cost = "0";
				cat = "";
				s_cat = "";
				brand = "";
				price1 = "0";
				stock = "0";
				has_scale = "0";
				level_price1 = "0";
				level_price2 = "0";
				level_price3 = "0";
				level_price4 = "0";
				level_price5 = "0";
				level_price6 = "0";
				tax_rate = "0.15";

				for(int j=0; j<viewCols; j++)
				{
					string v = Program.EncodeQuote(m_av[i, j]);
					if(selectColNameDB[j] == "barcode")
						barcode = v;
					else if (selectColNameDB[j] == "supplier_id")
						mpn = v;
					else if (selectColNameDB[j] == "name")
						name = v;
					else if (selectColNameDB[j] == "other_description")
						name_cn = v;
					else if (selectColNameDB[j] == "brand")
						brand = v;
					else if (selectColNameDB[j] == "cat")
						cat = v;
					else if (selectColNameDB[j] == "s_cat")
						s_cat = v;
                    else if (selectColNameDB[j] == "auto_weight")
                        has_scale = v;
					else if (selectColNameDB[j] == "stock_qty")
						stock = Program.MyDoubleParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "last_cost")
						cost = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "price1")
						price1 = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "level_price1")
						level_price1 = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "level_price2")
						level_price2 = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "level_price3")
						level_price3 = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "level_price4")
						level_price4 = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "level_price5")
						level_price5 = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "level_price6")
						level_price6 = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "manual_cost_frd")
						cost = Program.MyMoneyParse(m_av[i, j]).ToString();
					else if (selectColNameDB[j] == "tax_rate")
						tax_rate = Program.MyDoubleParse(m_av[i, j]).ToString();
				}
				if(mpn == "" && barcode == "")
				{
					nSkip++;
					continue;
				}
				if(mpn.Length > 49 || barcode.Length > 15)
				{
					nSkip++;
					continue;
				}

				code = "";
				sc = "";
				if (barcode != null && barcode != "")
					sc = " SELECT code FROM code_relations WHERE barcode = '" + barcode + "' ";
				else if (mpn != "")
					sc = " SELECT code FROM code_relations WHERE supplier_code = '" + mpn + "' ";

				if (sc == "")
				{
					nSkip++;
					sMsg += "Supplier Code or Barcode needed\r\n";
					continue;
				}
				if (dst.Tables["checkcode"] != null)
					dst.Tables["checkcode"].Clear();
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					if (myAdapter.Fill(dst, "checkcode") == 1)
					{
						code = dst.Tables["checkcode"].Rows[0]["code"].ToString();
						nExists++;
					}
				}
				catch (Exception ex)
				{
					Program.ShowExp(sc, ex);
					myConnection.Close();
					return false;
				}
				if(code == "")
				{
					if (dst.Tables["checkcode"] != null)
						dst.Tables["checkcode"].Clear();
					sc = " SELECT code FROM code_relations WHERE id = '" + nCode.ToString() + "' ";
					try
					{
						myAdapter = new SqlDataAdapter(sc, myConnection);
						if (myAdapter.Fill(dst, "checkcode") == 1)
						{
							code = dst.Tables["checkcode"].Rows[0]["code"].ToString();
							// force delete exists item with this id (id cannot bigger than the biggest code)
							sc = " DELETE FROM code_relations WHERE code = " + code;
							try
							{
								myCommand = new SqlCommand(sc);
								myCommand.Connection = myConnection;
								myCommand.CommandTimeout = 300;
								myConnection.Open();
								myCommand.ExecuteNonQuery();
								myCommand.Connection.Close();
							}
							catch (Exception e)
							{
								Program.ShowExp(sc, e);
								myConnection.Close();
								return false;
							}
							code = ""; //insert as new
						}
					}
					catch (Exception ex)
					{
						Program.ShowExp(sc, ex);
						myConnection.Close();
						return false;
					}
				}
				if (code == "")
				{
					string newCode = nCode.ToString();
					code = newCode;
					nCode++;
					sc = " INSERT INTO code_relations (code, id, supplier_code, barcode, name ";
					if(name_cn != "")
						sc += ", name_cn";
					if(brand != "")
						sc += ", brand";
					if(cat != "")
						sc += ", cat";
					if (s_cat != "")
						sc += ", s_cat";
					if (price1 != "0")
						sc += ", price1";
                    if (has_scale != "0")
                        sc += ", has_scale";
                    if (cost != "0")
                    {
                        sc += ", manual_cost_frd";
                        sc += ", average_cost ";
                    }
					if(level_price1 != "0")
						sc += ", level_price1, level_price2, level_price3, level_price4, level_price5, level_price6";
					if(tax_rate != "0.15")
						sc += ", tax_rate ";
					sc += ") VALUES(" + newCode + ", '" + newCode + "', '" + mpn + "', '" + barcode + "', N'" + name + "' ";
					if (name_cn != "")
						sc += ", N'" + name_cn + "'";
					if (brand != "")
						sc += ", N'" + brand + "'";
					if (cat != "")
						sc += ", N'" + cat + "'";
					if (s_cat != "")
						sc += ", N'" + s_cat + "'";
					if (price1 != "0")
						sc += ", " + price1;
                    if (has_scale != "0")
                        sc += ", " + has_scale;
                    if (cost != "0")
                    {
                        sc += ", " + cost;
                        sc += ", " + cost;
                    }
					if (level_price1 != "0")
						sc += ", " + level_price1 + ", " + level_price2 + ", " + level_price3 + ", " + level_price4 + ", " + level_price5 + ", " + level_price6;
					if (tax_rate != "0.15")
						sc += ", " + tax_rate;
					sc += ") ";
					sc += " INSERT INTO product (code, supplier_code, name";
					if (name_cn != "")
						sc += ", name_cn";
					if (brand != "")
						sc += ", brand";
					if (cat != "")
						sc += ", cat";
					if (s_cat != "")
						sc += ", s_cat";
					if (price1 != "0")
						sc += ", price";
					sc += ") VALUES(" + newCode + ", '" + mpn + "', N'" + name + "'";
					if (name_cn != "")
						sc += ", N'" + name_cn + "'";
					if (brand != "")
						sc += ", N'" + brand + "'";
					if (cat != "")
						sc += ", N'" + cat + "'";
					if (s_cat != "")
						sc += ", N'" + s_cat + "'";
					if (price1 != "0")
						sc +=",'"+ price1 + "'";
					sc += ")";
					sc += " INSERT INTO code_branch (inactive, code, branch_id, price1)VALUES(0, " + newCode + ", 1, " + price1 + ") ";
					if(stock != "0")
						sc += " IF NOT EXISTS(SELECT id FROM stock_qty WHERE branch_id = 1 AND code = " + newCode + ") INSERT INTO stock_qty (code, qty) VALUES(" + newCode + ", " + stock + ") ";
					nNew++;
				}
				else
				{
					sc = " UPDATE code_relations SET supplier_code = '" + mpn + "', barcode = '" + barcode + "', name = N'" + name + "' ";
					if (name_cn != "")
						sc += ", name_cn = N'" + name_cn + "' ";
					if (brand != "")
						sc += ", brand = N'" + brand + "' ";
					if (cat != "")
						sc += ", cat = N'" + cat + "' ";
					if (s_cat != "")
						sc += ", s_cat = N'" + s_cat + "' ";
					if (price1 != "0")
						sc += ", price1 = " + price1;
                    if (has_scale != "0")
                        sc += ", has_scale = " + has_scale;
                    if (cost != "0")
                    {
                        sc += ", manual_cost_frd = " + cost;
                        sc += ", average_cost = " + cost;
                    }
					if(level_price1 != "0")
					{
						sc += ", level_price1 = " + level_price1 + ", level_price2 = " + level_price2 + ", level_price3 = " + level_price3;
						sc += ", level_price4 = " + level_price4 + ", level_price5 = " + level_price5 + ", level_price6 = " + level_price6;
					}
					if(tax_rate != "0.15")
						sc += ", tax_rate = " + tax_rate;
					sc += " WHERE code = " + code;
					sc += " UPDATE product SET supplier_code = '" + mpn + "', name = N'" + name + "' ";
					if(name_cn != "")
						sc += ", name_cn = N'" + name_cn + "' ";
					if(brand != "")
						sc += ", brand = N'" + brand + "' ";
					if (cat != "")
						sc += ", cat = N'" + cat + "' ";
					if (s_cat != "")
						sc += ", s_cat = N'" + s_cat + "' ";
					if (price1 != "0")
						sc += ", price = " + price1;
					sc += " WHERE code = " + code;
					if(stock != "0")
						sc += " UPDATE stock_qty SET qty = " + stock + " WHERE branch_id = 1 AND code = " + code;
					nExists++;
				}
				if (barcode != "")
					sc += " IF NOT EXISTS(SELECT id FROM barcode WHERE item_code = " + code + " AND barcode = '" + barcode + "') INSERT INTO barcode(item_code, barcode) VALUES(" + code + ", '" + barcode + "') ";
				try
				{
					myCommand = new SqlCommand(sc);
					myCommand.Connection = myConnection;
					myCommand.CommandTimeout = 300;
                    myCommand.connection.Open();
					myCommand.ExecuteNonQuery();
					myCommand.Connection.Close();
				}
				catch (Exception e)
				{
					Program.ShowExp(sc, e);
					myConnection.Close();
					return false;
				}
				if (i % 100 == 0 || i >= m_nRows)
				{
					lblRows.Text = i.ToString();
					double d = (double)i / ((double)m_nRows / 100);
					int n = (int)d;
					if (n > 100)
						n = 100;
//					int n = i / (m_nRows / 100);
					progressBar.Value = n;
					lblRows.Refresh();
					progressBar.Refresh();
				}
			}
			if(sMsg != "")
				MessageBox.Show(sMsg);
			else
				MessageBox.Show("Done, " + nNew + " new items added, " + nExists + " existing items updated, " + nSkip + " invalid supplier_code OR barcode skipped.");
			return true;
		}
		private void btnprocess_Click(object sender, EventArgs e)
		{
		  //  lblpleasewait.Visible = true;
			if (MessageBox.Show("Data import might take a few minutes.Please click ok to start the data import", "Data Import Message", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
			{
				MessageBox.Show("Data import has been ternimated");
				return;
			}
			if (!doImport())
			{
				MessageBox.Show("Error in importing product");
				lblpleasewait.Text = "";
				lblpleasewait.Visible = false;
				return;
			}

			if (!doUpdateCatalog())
			{
				MessageBox.Show("Error in updating catalog and department");
				lblpleasewait.Text = "";
				lblpleasewait.Visible = false;
				return;
			}
			MessageBox.Show("Item Import Sussccefully !");
			this.Close();
		}
		private bool doUpdateCatalog()
		{
			int rows = 0;
			if (dst.Tables["selectdepart"] != null)
					dst.Tables["selectdepart"].Clear();
				string sc = " SELECT brand FROM code_relations GROUP BY  brand ";
				try
				{
					myAdapter = new SqlDataAdapter(sc, myConnection);
					rows = myAdapter.Fill(dst, "selectdepart");
					if(rows <=0)
						return false;
					 
				}
				catch (Exception ex)
				{
					Program.ShowExp(sc, ex);
					myConnection.Close();
					return false;
				}

				for (int i = 0; i < rows; i++)
				{
					DataRow dr = dst.Tables["selectdepart"].Rows[i];
					string depart = dr["brand"].ToString();
					string scd = "IF NOT EXISTS(SELECT * FROM catalog WHERE cat = 'Brands' AND LOWER(s_cat) = N'" + depart.ToLower() + "')";
					scd += "INSERT INTO catalog (cat, s_cat, ss_cat)";
					scd += " VALUES ('Brands', N'" + depart + "','')";
					try
					{
						myCommand = new SqlCommand(scd);
						myCommand.Connection = myConnection;
                        myCommand.connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception e)
					{
						Program.ShowExp(sc, e);
						myConnection.Close();
						return false;
					}
				}
				int rowsc = 0;
				if (dst.Tables["selectcat"] != null)
					dst.Tables["selectcat"].Clear();
				string scc = " SELECT DISTINCT cat, s_cat FROM code_relations ";
				try
				{
					myAdapter = new SqlDataAdapter(scc, myConnection);
					rowsc = myAdapter.Fill(dst, "selectcat");
					if (rowsc <= 0)
						return false;

				}
				catch (Exception ex)
				{
					Program.ShowExp(scc, ex);
					myConnection.Close();
					return false;
				}

				for (int i = 0; i < rowsc; i++)
				{
					DataRow dr = dst.Tables["selectcat"].Rows[i];
					string cat = dr["cat"].ToString();
					string s_cat = dr["s_cat"].ToString();
					string scsc = "IF NOT EXISTS(SELECT * FROM catalog WHERE LOWER(cat) = N'" + cat.ToLower() + "' AND LOWER(s_cat) = N'" + s_cat.ToLower() + "')";
					scsc += "INSERT INTO catalog (cat, s_cat, ss_cat)";
					scsc += " VALUES (N'" + cat + "', N'" + s_cat + "', '')";
					try
					{
						myCommand = new SqlCommand(scsc);
						myCommand.Connection = myConnection;
                        myCommand.connection.Open();
						myCommand.ExecuteNonQuery();
						myCommand.Connection.Close();
					}
					catch (Exception e)
					{
						Program.ShowExp(sc, e);
						myConnection.Close();
						return false;
					}
				}
				return true;
		}
		private void dgvimport_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			btnTest.Enabled = true;
		}
		private void dgvimport_CellClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void btnmappingnext_Click(object sender, EventArgs e)
		{
			int rows = dgvimport.Rows.Count;
			for (int i = 0; i < rows; i++)
			{
				string col = dgvimport.Rows[i].Cells["ii_name"].Value.ToString();
				string colv = "";
				if (dgvimport.Rows[i].Cells["ii_mapto"].Value == null)
					continue;
				else
					colv = dgvimport.Rows[i].Cells["ii_mapto"].Value.ToString();
		/**********************************/
				if (col.ToLower().Trim() == "supplier_id" && colv.ToLower().Trim() == "")
				{
					MessageBox.Show("Please select a value for supplier_id");
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.Red;
					return;
				}
		/***********************************/
				if (col.ToLower().Trim() == "customer code" && colv.ToLower().Trim() == "")
				{
					MessageBox.Show("Please select a value for customer code");
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.Red;
					return;
				}
				else
				{
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.White;
				}
				
				if (col.ToLower().Trim() == "department" && colv.ToLower().Trim() == "")
				{
					MessageBox.Show("Please select a value for department");
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.Red;
					return;
				}
				else
				{
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.White;
				}
				
				if (col.ToLower().Trim() == "button name" && colv.ToLower().Trim() == "")
				{
					MessageBox.Show("Please select a value for button name");
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.Red;
					return;
				}
				else
				{
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.White;
				}
					
					
				if (col.ToLower().Trim() == "ref code" && colv.ToLower().Trim() == "")
				{
					MessageBox.Show("Please select a value for ref code");
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.Red;
					return;
				}
				else
				{
					dgvimport.Rows[i].Cells["ii_mapto"].Style.BackColor = System.Drawing.Color.White;
				}
			}
			lv_list.Items.Clear();
			pnlMapping.Visible = false;
			pnltest.Visible = true;
		}
		private void btntestnext_Click(object sender, EventArgs e)
		{
			pnltest.Visible = false;
			pnlprocess.Visible = true;
		}
		private void dgvimport_CellClick_1(object sender, DataGridViewCellEventArgs e)
		{
			btnmappingnext.Enabled = true;
		}
		private void btnmappinglast_Click(object sender, EventArgs e)
		{
			pnlMapping.Visible = false;
			btnmappingnext.Enabled = false;
			btnrestart.Visible = false;
		}
		private void btntestlast_Click(object sender, EventArgs e)
		{
			pnltest.Visible = false;
			btntestnext.Enabled = false;
			pnlMapping.Visible = true;
		}
		private void btnrestart_Click(object sender, EventArgs e)
		{
			pnltest.Visible = false;
			btntestnext.Enabled = false;
			pnlMapping.Visible = false;
			btnmappingnext.Enabled = false;
			btnrestart.Visible = false;
			pnlprocess.Visible = false;
			txtUploadFileName.Text = "";
			lblpleasewait.Visible = false;
		}
	}
}
