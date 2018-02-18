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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Чтобы не нажимать при отладке на кнопку вызова постоянно
            SharpForm = new SharpGLForm(true);
            SharpForm.Visible = true;
            this.WindowState = FormWindowState.Minimized;
        }

        SharpGLForm SharpForm = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (SharpForm != null)
                if (SharpForm.Enabled)
                    SharpForm.Close();
            SharpForm = new SharpGLForm(true);
            SharpForm.Visible = true;
        }
    }
}
