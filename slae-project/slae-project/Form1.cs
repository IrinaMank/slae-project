using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace slae_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SharpGL_limbo sharpGL_limbo = new SharpGL_limbo(true); //рядом с кнопочкой
        //sharpGL_limbo.SharpGLCallTheWindow_for_The_Button(); //в кнопочке
    }
}
