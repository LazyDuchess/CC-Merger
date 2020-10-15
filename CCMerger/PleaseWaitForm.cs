using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMerger
{
    public partial class PleaseWaitForm : Form
    {
        public PleaseWaitForm()
        {
            InitializeComponent();
        }

        public void setProgress(string prog)
        {
            label1.Text = "Merging, Please Wait... (" + prog + ")";
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
