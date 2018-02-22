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

   
    public partial class Form2 : Form
    {
        List<Label> name_arr = new List<Label>();
        Dictionary<int, TextBox> puths = new Dictionary<int, TextBox>();
        public Form2()
        {
            InitializeComponent();
            this.Size = new Size(500, 375);
    }

        private void Form2_Load(object sender, EventArgs e)
        {
            //  List<string> arrays = factory.Set_arrays();
            List<string> arrays = new List<string> { "ia", "ig", "ggu", "ggl" };
            int count_arr = arrays.Count();     
            int x_l = 36, y = 50, x_p = 100, x_b = 315;
            
            for (int i = 0; i < count_arr; i++)
            {

            Label name = new Label();
            name.Text = arrays[0];
            name.Size = new Size(15, 15);
            name.Location = new System.Drawing.Point(x_l, y);
            name_arr.Add(name);
            this.Controls.Add(name);

            TextBox puth = new TextBox();
            puth.Size = new Size(185, 20);
            puth.Name = i.ToString();
            puth.Location = new Point(x_p, y);
            puths.Add(y, puth);
            this.Controls.Add(puth);

            Button button = new Button();
            button.Text = "Обзор";
            button.Size = new Size(75, 23);
            button.Location = new Point(x_b, y);
            button.Click += new System.EventHandler(button_Click);
            this.Controls.Add(button);


                y += 50;
            }
            Button button_load = new Button();
            button_load.Text = "ОК";
            button_load.Size = new Size(183, 23);
            button_load.Location = new Point(x_p, y);
            this.Controls.Add(button_load);
        }
        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Point coord = btn.Location;

            TextBox value = new TextBox();
            puths.TryGetValue(coord.Y, out value);

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            //получаем выбранный файл
            string filename_ia = openFileDialog1.FileName;
            // читаем файл в строку
            value.Text = filename_ia;
        }
    }
}
