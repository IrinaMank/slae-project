using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace slae_project
{
    public partial class loadWindow : Form
    {
        public loadWindow()
        {
            InitializeComponent();
        }

        private void loadWindow_Load(object sender, EventArgs e)
        {
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            timer1.Start();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
           // this.Opacity += 0.5;
            //if (this.Opacity == 1)
             //   timer1.Stop();
        }
    }
}
