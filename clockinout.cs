using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QPOS2008
{
    public partial class clockinout : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private SqlDataAdapter myAdapter;
        private SqlConnection myConnection;
        private SqlCommand myCommand;
        private Control m_lastFocused = null;
        private DataSet dst = new DataSet();
        private string m_sStaffId = "";
		private string m_sStaffBarcode = "";
        private string m_sStaffName = "";


        private bool bCheckIn = true;
        private bool bLastIsCheckin = false;
        private string lastChckeInTime = "";
        private string lastRecordId = "";

        public clockinout()
        {
            InitializeComponent();
        }

        private void clockinout_Load(object sender, EventArgs e)
        {
//            myConnection = new SqlConnection("Initial Catalog=" + Program.m_sCompanyName + Program.m_sDataSource + Program.m_sSecurityString);
            this.Location = new Point(400, 100);
        }
        private void onControlLeave(object sender, EventArgs e)
        {

        }

        private bool DoSearchStaff()
        {
            if (dst.Tables["staff"] != null)
                dst.Tables["staff"].Clear();
            string sc = " SELECT * FROM card WHERE barcode = '" + this.txtbarcode.Text + "'";
            sc += " AND type= 4 ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                if (myAdapter.Fill(dst, "staff") <= 0)
                    return false;
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return false;
            }
            if (dst.Tables["staff"].Rows.Count == 1)
            {
				m_sStaffId = dst.Tables["staff"].Rows[0]["id"].ToString();
				m_sStaffBarcode = dst.Tables["staff"].Rows[0]["barcode"].ToString();
                m_sStaffName = dst.Tables["staff"].Rows[0]["name"].ToString();
            }
            return true;
        }
        private bool DoClockInOut()
        {

            if (dst.Tables["clockin"] != null)
                dst.Tables["clockin"].Clear();
            string sc = " SELECT TOP 1 * FROM work_time WHERE card_id ='" + m_sStaffId + "' AND DATEDIFF(day, record_time, GETDATE())= 0";
            sc += " ORDER BY record_time DESC ";
            try
            {
                myAdapter = new SqlDataAdapter(sc, myConnection);
                if (myAdapter.Fill(dst, "clockin") == 1)
                {
                    DataRow dr = dst.Tables["clockin"].Rows[0];
                    bLastIsCheckin = Program.MyBooleanParse(dr["is_checkin"].ToString());
                    if (bLastIsCheckin)
                    {
                        bCheckIn = false;
                        lastChckeInTime = dr["record_time"].ToString();
                        lastRecordId = dr["id"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ShowExp(sc, ex);
                myConnection.Close();
                return false;
            }

            if (!bLastIsCheckin)
            {
				sc = " INSERT INTO work_time (card_id, barcode, is_checkin) VALUES('" + m_sStaffId + "','" + m_sStaffBarcode + "', '1')";
            }
            else
            {
                double dMin = 0;
                double dHour = 0;
                if (dst.Tables["time"] != null)
                    dst.Tables["time"].Clear();
                sc = " SELECT DATEDIFF(minute, record_time, GETDATE()) AS minutes FROM work_time WHERE id ='" + lastRecordId + "'";
                try
                {
                    myAdapter = new SqlDataAdapter(sc, myConnection);
                    if (myAdapter.Fill(dst, "time") == 1)
                    {
                        DataRow drt = dst.Tables["time"].Rows[0];
                        dMin = Program.MyDoubleParse(drt["minutes"].ToString());
                        dHour = Math.Round(dMin / 60, 2);
                    }

                }
                catch (Exception ex)
                {
                    Program.ShowExp(sc, ex);
                    myConnection.Close();
                    return false;
                }
				sc = " INSERT INTO work_time (card_id, barcode, hours, is_checkin) VALUES ('" + m_sStaffId + "','" + m_sStaffBarcode + "', '" + dHour + "', '0')";
            }
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
                myConnection.Close();
                return false;
            }
            return true;
        }

        private void btnkey_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string name = btn.Name;
            string text = btn.Text;

            this.txtbarcode.Focus();

            Control nc = txtbarcode;
            if (m_lastFocused != null)
                nc = m_lastFocused; ;

            int WM_KEYDOWN = 0x0100;
            int nKey = 0x30;

            if (name.Trim() == "btnclear")
            {
                this.txtbarcode.Text = "";
                return;
            }
            else if (name.Trim() == "btnbackspace")
            {
                nKey = 0x08;
            }
            else if (name.Trim() == "btnenter")
            {
                nKey = 17;
            }
            else
                nKey += Program.MyIntParse(text);

            Message msg = Message.Create(nc.Handle, WM_KEYDOWN, new IntPtr(nKey), new IntPtr(0));
            PostMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
        }

        private void clockinout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 17)
            {
                if (!DoSearchStaff())
                {
                    FormMSG msg = new FormMSG();
                    msg.btnNo.Visible = false;
                    msg.btnYes.Visible = false;
                    msg.m_sMsg = "Sorry, Barcode Incorrect or Sales Not Exist";
                    msg.ShowDialog();
                    return;
                }
                else
                {
                    if (DoClockInOut())
                    {
                        FormMSG msg = new FormMSG();
                        msg.btnNo.Visible = false;
                        msg.btnYes.Visible = false;
                        if (bCheckIn)
                            msg.m_sMsg = m_sStaffName + " You have Clocked In At :" + System.DateTime.Now.ToString();
                        else
                            msg.m_sMsg = m_sStaffName + " You have Clocked Out At :" + System.DateTime.Now.ToString();
                        msg.ShowDialog();
                        this.txtbarcode.Text = "";
                        m_sStaffName = "";
                        m_sStaffBarcode = "";
                        m_sStaffId = "";
                        this.Close();
                        return;
                    }
                }
            }
        }

 
    }
}
