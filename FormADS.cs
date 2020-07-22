using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace QPOS2008
{
    public partial class FormADS : Form
    {
        public IntPtr m_hParent;

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public FormADS()
        {
            InitializeComponent();
        }

        private void FormADS_Load(object sender, EventArgs e)
        {
            this.TotalPrice.BorderStyle = BorderStyle.None;
            this.TotalPrice.Width = 200;
            this.labelChange.Text = "";
            this.showchange.Text = "";
            this.citem1.Text = "";
            this.citem2.Text = "";

            this.webBrowser1.Visible = false;

            Uri uri;
            bool bRet = Uri.TryCreate(Program.m_sAdUrl, UriKind.Absolute, out uri);
            if (bRet)
                webBrowser1.Url = uri;
            //			MessageBox.Show(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width);

            Screen[] screens = Screen.AllScreens;
            if (screens.Length == 2)
            {
                List<Screen> lstScreen = new List<Screen>();
                foreach (Screen screen in Screen.AllScreens)
                {
                    if (screen.Primary == false)
                        lstScreen.Add(screen);
                }

                this.Location = lstScreen[0].WorkingArea.Location;
                //this.Width = lstScreen[0].WorkingArea.Width;
                //this.Height = lstScreen[0].WorkingArea.Height;
                this.FormBorderStyle = FormBorderStyle.None; //no title, no border   

            }
            else
            {
                //				this.Width = Screen.PrimaryScreen.Bounds.Width - 1024;
                //				this.Height = this.Width * 3 / 4;

                int nScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                int nScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                this.Location = new Point(800, 0);



                //				this.Location = new Point(0, 0);
            }
            //			SetForegroundWindow(m_hParent);
        }

        private void citem1_Click(object sender, EventArgs e)
        {

        }


    }
}
