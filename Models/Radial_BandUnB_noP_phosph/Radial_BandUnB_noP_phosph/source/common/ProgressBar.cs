using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Radial_BandUnB_noP_phosph.source.common
{
    public partial class ProgressBar : Form
    {
        private bool _exit = false;
        public bool Exit
        {
            set
            {
                this._exit = value;
            }
            get
            {
                return this._exit;
            }
        }
        public ProgressBar()
        {
            InitializeComponent();
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 0;
        }      
        public void SetProgress(int value, int MAX)
        {
            if (progressBar1.Maximum != MAX)
            {
                progressBar1.Maximum = MAX;
            }
            progressBar1.Value = value;

            lab_index.Text = value.ToString() + " (" + MAX.ToString() + ")";
            /*
            this.Invalidate();
            this.Update();
            this.Refresh();

            progressBar1.Invalidate();
            progressBar1.Update();
            progressBar1.Refresh();

            lab_index.Invalidate();
            lab_index.Update();
            lab_index.Refresh();
            */
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Exit = true;
        }
    }
}
