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
    public partial class FAQ : Form
    {
        public FAQ()
        {
            InitializeComponent();
            Show();
            pictureBox1.Size = new Size(300,300);
            label1.Text = "Important \r\n Thing \r\n To say \r\n You \r\n I have!";
        }

        void Wrapped_Changer()
        {

        }
    }
}
