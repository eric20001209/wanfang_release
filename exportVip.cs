using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QPOS2008
{
	public partial class exportVip : Form
	{
		public int exportViptimetype = 2;
		
		public exportVip()
		{
			InitializeComponent();
		}

		private void btnexportsum_Click(object sender, EventArgs e)
		{
			exportViptimetype = 2;
			this.Close();

		}

		private void btnexporttimedetail_Click(object sender, EventArgs e)
		{
			exportViptimetype = 6;
			this.Close();
		}
	}
}
