using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
    public partial class export_time_sheet : Form
    {
        public int extimetype = 4;
        
        public export_time_sheet()
        {
            InitializeComponent();
        }

        private void btnexportsum_Click(object sender, EventArgs e)
        {
            extimetype = 4;
            this.Close();
        }

        private void btnexporttimedetail_Click(object sender, EventArgs e)
        {
            extimetype = 5;
            this.Close();
        }

        private void export_time_sheet_Load(object sender, EventArgs e)
        {

        }
    }
}
