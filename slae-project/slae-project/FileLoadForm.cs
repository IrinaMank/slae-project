using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using slae_project.Matrix;
using slae_project.Vector;
using System.IO;
namespace slae_project
{


    public partial class FileLoadForm : Form
    {
        int Y = 0;
        public static Dictionary<string, Label> name_arr = new Dictionary<string, Label>();//имена массивов
        public static Dictionary<int, TextBox> puths = new Dictionary<int, TextBox>();//пути до массивов
        public static Dictionary<string, string> filenames_format = new Dictionary<string, string>(); // словарь: ключ - название массива, значение - путь к файлу
        public static List<string> arrays = new List<string>();
        public static Dictionary<Label, TextBox> sootvet = new Dictionary<Label, TextBox>();
        //public static IVector F = new SimpleVector();//правая часть
        //public static IVector X0 = new SimpleVector();//Начально приближение 
        public FileLoadForm()
        {
            InitializeComponent();
            this.Size = new Size(500, 400);
        }
        List<Button> obzors = new List<Button>();//лист кнопок обзоров
        private void FileLoadForm_Load(object sender, EventArgs e)
        {
            obzors.Clear();
            Factory.CreateMatrix(Form1.str_format_matrix);

            arrays = Factory.name_arr;
            int count_arr = arrays.Count();

            string mess = "";
            mess += "Выбери массивы со следующими названиями: ";
            mess += "F, X0, ";
            for (int i = 0; i < arrays.Count(); i++)
            {

                mess += arrays[i];
                mess += ", ";
            }

            int x_l = 45, y = 25, x_p = 100, x_b = 315;
            Label arr_label = new Label();
            arr_label.Text = mess.ToString();
            arr_label.Size = new Size(200, 40);
            arr_label.Location = new System.Drawing.Point(x_l, y);
            this.Controls.Add(arr_label);
            arr_label.BackColor = Color.Transparent;

            Button button_all = new Button();
            button_all.Text = "Обзор";
            button_all.Size = new Size(75, 23);
            button_all.Location = new Point(x_b, y);
            button_all.Click += new System.EventHandler(button_Click);
            this.Controls.Add(button_all);

            y += 50;
            puths.Clear(); name_arr.Clear();
            //все массивы
            for (int i = 0; i < count_arr; i++)
            {

                Label name = new Label();
                name.Text = arrays[i];
                name.Size = new Size(50, 15);
                name.Location = new System.Drawing.Point(x_l, y);
                name_arr.Add(name.Text, name);
                this.Controls.Add(name);
                name.BackColor = Color.Transparent;

                TextBox puth = new TextBox();
                puth.Size = new Size(185, 20);
                puth.Name = i.ToString();
                puth.Location = new Point(x_p, y);
                puths.Add(y, puth);
                sootvet.Add(name, puth);
                this.Controls.Add(puth);

                Button button = new Button();
                obzors.Add(button);
                button.Text = "Обзор";
                button.Size = new Size(75, 23);
                button.Location = new Point(x_b, y);
                button.Click += new System.EventHandler(button_Click);
                this.Controls.Add(button);


                y += 33;
            }
            //правая часть
            Label name_b = new Label();
            name_b.Text = "F";
            name_b.Size = new Size(25, 15);
            name_b.Location = new System.Drawing.Point(x_l, y);
            name_arr.Add(name_b.Text, name_b);
            this.Controls.Add(name_b);
            name_b.BackColor = Color.Transparent;

            TextBox puth_b = new TextBox();
            puth_b.Size = new Size(185, 20);
            puth_b.Name = "F";
            puth_b.Location = new Point(x_p, y);
            puths.Add(y, puth_b);
            this.Controls.Add(puth_b);
            sootvet.Add(name_b, puth_b);

            Button button_b = new Button();
            obzors.Add(button_b);
            button_b.Text = "Обзор";
            button_b.Size = new Size(75, 23);
            button_b.Location = new Point(x_b, y);
            button_b.Click += new System.EventHandler(button_Click);
            this.Controls.Add(button_b);

            y += 33;
            ///начальное приближение
            Label name_x0 = new Label();
            name_x0.Text = "X0";
            name_x0.Size = new Size(25, 15);
            name_x0.Location = new System.Drawing.Point(x_l, y);
            name_arr.Add(name_x0.Text, name_x0);
            this.Controls.Add(name_x0);
            name_x0.BackColor = Color.Transparent;

            TextBox puth_x0 = new TextBox();
            puth_x0.Size = new Size(185, 20);
            puth_x0.Name = "x0";
            puth_x0.Location = new Point(x_p, y);
            puths.Add(y, puth_x0);
            this.Controls.Add(puth_x0);
            sootvet.Add(name_x0, puth_x0);

            Button button_x0 = new Button();
            obzors.Add(button_x0);
            button_x0.Text = "Обзор";
            button_x0.Size = new Size(75, 23);
            button_x0.Location = new Point(x_b, y);
            button_x0.Click += new System.EventHandler(button_Click);
            this.Controls.Add(button_x0);

            y += 50;
            //кнопка загрузки
            Button button_load = new Button();
            button_load.Text = "ОК";
            button_load.Size = new Size(183, 23);
            button_load.Location = new Point(x_p, y);
            button_load.Click += new System.EventHandler(this.button_load_Click);
            this.Controls.Add(button_load);

            Y = y;
        }
        private void button_load_Click(object sender, EventArgs e)
        {
            TextBox value = new TextBox();
            int y = 55;
            int i = 0;
            for (i = 0; i < puths.Count(); i++, y += 33)
            {
                puths.TryGetValue(y, out value);
                if (value.Text.ToString() == "")
                {
                    MessageBox.Show("Заполнены не все поля!");
                    break;
                }
            }
            if (i == puths.Count())
            {
                this.Close();
            }


        }
        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string filename_b = null, filename_X0 = null;
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            List<string> filenames = new List<string>();
            foreach (string file in openFileDialog1.FileNames)
            {
                //получаем выбранный файл 
                filenames.Add(file);
            }
            arrays.Add("F"); arrays.Add("X0");
            for (int i = 0; i < filenames.Count(); i++)
            {

                int j = 0;
                for (; j < arrays.Count(); j++)
                {
                    if (filenames[i].IndexOf(arrays[j]) != -1)
                    {
                        filenames_format.Remove(arrays[j]);
                        filenames_format.Add(arrays[j], filenames[i]);
                        foreach (string iy in name_arr.Keys)
                        {
                            if (arrays[j] == iy)
                            {
                                Label value = new Label();
                                TextBox value1 = new TextBox();
                                name_arr.TryGetValue(iy, out value);
                                sootvet.TryGetValue(value, out value1);
                                value1.Text = filenames[i];

                                if (arrays[j] == "F") filename_b = filenames[i];
                                if (arrays[j] == "X0") filename_X0 = filenames[i];
                            }
                        }
                        break;
                    }
                }

            }

            if (filename_b == null || filename_X0 == null) { MessageBox.Show("Выбраны не все файлы"); }

            if (btn == obzors[obzors.Count - 2])
            {

                FileStream file = new FileStream(filename_b, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(file);

                List<int> z = new List<int>();
                int r = 0;
                int size = Convert.ToInt32(reader.ReadLine());
                var k = reader.ReadLine().Split();
                Factory.RightVector = new SimpleVector(size);
                //F = new SimpleVector(size);
                while (r < size)
                {
                    Factory.RightVector[r] = Convert.ToDouble(k[r]);
                    r++;

                }
            }
            else if (btn == obzors[obzors.Count - 1])
            {
                FileStream file = new FileStream(filename_X0, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(file);
                List<int> z = new List<int>();
                int r = 0;

                int size = Convert.ToInt32(reader.ReadLine());
                var k = reader.ReadLine().Split();
                Factory.X0 = new SimpleVector(size);
               // X0 = new SimpleVector(size);
                while (r < size)
                {
                    Factory.X0[r] = Convert.ToDouble(k[r]);
                    r++;

                }
            }
        }


    }
}
