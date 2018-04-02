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
            Wrapped_Changer();
        }
        private void FAQ_Resize(object sender, EventArgs e)
        {
            Wrapped_Changer();
        }
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            Wrapped_Changer();
        }
        void Wrapped_Changer()
        {
            int Curs = 10 - vScrollBar1.Value;
            int MinusWidth = 30 + vScrollBar1.Size.Width;

            label1.Location = new Point(10, Curs + 10);
            label1.Text = "Добро пожаловать в справку!\r\n" +
                "Перемещайтесь пожалуйста с помощью ползунка с правой части окна.";
            Curs += label1.Size.Height;

            pictureBox1.Location = new Point(10, Curs + 10);
            pictureBox1.Size = new Size(this.Width - MinusWidth, (int)((double)this.Width / pictureBox1.PreferredSize.Width * pictureBox1.PreferredSize.Height));
            Curs += pictureBox1.Size.Height;

            
        }

        
    }
}
