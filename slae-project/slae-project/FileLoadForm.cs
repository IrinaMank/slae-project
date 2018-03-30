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
        int unFiledFields;
        List<string> name_arr = new List<string>();//имена лейблов
        List<Button> input_buttons = new List<Button>();
        List<TextBox> puths = new List<TextBox>();

        public static Dictionary<string, string> filenames_format = new Dictionary<string, string>(); // словарь: ключ - название массива, значение - путь к файлу
        public static List<string> arrays = new List<string>();
        public static Dictionary<Label, TextBox> sootvet = new Dictionary<Label, TextBox>();
        //public static IVector F = new SimpleVector();//правая часть
        //public static IVector X0 = new SimpleVector();//Начально приближение 
        string filename_b = null, filename_X0 = null;
        bool multireadFlag = false;

        Button button_load, button_cancel;

        public FileLoadForm()
        {
            InitializeComponent();

        }
        List<Button> obzors = new List<Button>();//лист кнопок обзоров
        private void FileLoadForm_Load(object sender, EventArgs e)
        {
            obzors.Clear();
            Factory.CreateMatrix(Form1.str_format_matrix);

            arrays = Factory.name_arr; //<==========================================================LOOK AT ME
            if (Form1.property_matr && Form1.format.SelectedIndex == 2)
                arrays.RemoveAt(arrays.IndexOf("au"));

            int count_arr = arrays.Count();

            string mess = "";
            mess += "Выберите массивы со следующими названиями: ";
            mess += "F.txt, X0.txt,";
            for (int i = 0; i < arrays.Count(); i++)
                mess += " " + arrays[i] + ".txt,";
            mess = mess.Remove(mess.Length - 1);

            int x_l = 45, y = 25, x_p = 100, x_b = 315;
            Label arr_label = new Label
            {
                Text = mess.ToString(),
                Size = new Size(200, 40),
                Location = new System.Drawing.Point(x_l, y)
            };
            this.Controls.Add(arr_label);
            arr_label.BackColor = Color.Transparent;

            Button button_all = new Button
            {
                Text = "Обзор",
                Size = new Size(75, 23),
                Location = new Point(x_b, y)
            };
            button_all.Click += new System.EventHandler(MultireadingButton_Click);
            this.Controls.Add(button_all);

            y += 50;
            puths.Clear();
            name_arr.Clear();
            filenames_format.Clear();
            input_buttons.Clear();

            for (int i = 0; i < count_arr; i++)
            {
                Label name = new Label
                {
                    Text = arrays[i],
                    Size = new Size(50, 15),
                    Location = new System.Drawing.Point(x_l, y)
                };
                filenames_format.Add(name.Text, "");
                this.Controls.Add(name);
                name.BackColor = Color.Transparent;
                name_arr.Add(name.Text);

                TextBox puth = new TextBox
                {
                    Size = new Size(185, 20),
                    Name = i.ToString(),
                    Location = new Point(x_p, y)
                };
                puths.Add(puth);
                this.Controls.Add(puth);

                Button button = new Button();
                obzors.Add(button);
                button.Text = "Обзор";
                button.Size = new Size(75, 23);
                button.Location = new Point(x_b, y-2);
                button.Click += new System.EventHandler(button_Click);
                this.Controls.Add(button);
                input_buttons.Add(button);

                y += 33;
            }
            //правая часть
            Label name_b = new Label
            {
                Text = "f",
                Size = new Size(25, 15),
                Location = new System.Drawing.Point(x_l, y)
            };
            this.Controls.Add(name_b);
            name_b.BackColor = Color.Transparent;
            name_arr.Add(name_b.Text);

            TextBox puth_b = new TextBox
            {
                Size = new Size(185, 20),
                Name = "f",
                Location = new Point(x_p, y)
            };
            puths.Add(puth_b);
            this.Controls.Add(puth_b);

            Button button_b = new Button();
            obzors.Add(button_b);
            button_b.Text = "Обзор";
            button_b.Size = new Size(75, 23);
            button_b.Location = new Point(x_b, y);
            button_b.Click += new System.EventHandler(button_Click);
            this.Controls.Add(button_b);
            input_buttons.Add(button_b);

            y += 33;
            ///начальное приближение
            Label name_x0 = new Label
            {
                Text = "x0",
                Size = new Size(25, 15),
                Location = new System.Drawing.Point(x_l, y)
            };
            //name_arr.Add(name_x0.Text, name_x0);
            this.Controls.Add(name_x0);
            name_x0.BackColor = Color.Transparent;
            name_arr.Add(name_x0.Text);

            TextBox puth_x0 = new TextBox
            {
                Size = new Size(185, 20),
                Name = "x0",
                Location = new Point(x_p, y)
            };
            puths.Add(puth_x0);
            this.Controls.Add(puth_x0);

            Button button_x0 = new Button();
            obzors.Add(button_x0);
            button_x0.Text = "Обзор";
            button_x0.Size = new Size(75, 23);
            button_x0.Location = new Point(x_b, y);
            button_x0.Click += new System.EventHandler(button_Click);
            this.Controls.Add(button_x0);
            input_buttons.Add(button_x0);

            y += 50;
            //кнопка загрузки
            button_load = new Button
            {
                Text = "ОК",
                Size = new Size(85, 23),
                Location = new Point(100, y)
            };
            button_load.Click += new System.EventHandler(this.button_load_Click);
            this.Controls.Add(button_load);
            button_load.Enabled = false;

            button_cancel = new Button
            {
                Text = "Отмена",
                Size = new Size(85, 23),
                Location = new Point(200, y)
            };
            button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            this.Controls.Add(button_cancel);
            button_cancel.Enabled = true;

            unFiledFields = name_arr.Count;
            arrays.Add("F"); arrays.Add("X0");

            this.Size = new Size(450, y+100);
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Form1.format.Enabled = true;
            this.Visible = false;
            Form1.next.Enabled = false;
        }

        private void MultireadingButton_Click(object sender, EventArgs e)
        {
            filename_b = null;
            filename_X0 = null;
            openFileDialog1.Multiselect = true;

            foreach (var i in puths)
                i.Enabled = false;

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            if (openFileDialog1.FileNames.Length != arrays.Count)
            {
                MessageBox.Show("Неверное число файлов");
                return;
            }

            foreach (string file in openFileDialog1.FileNames)
            {
                string buf = file;
                buf = buf.Remove(0, buf.LastIndexOf('\\') + 1);
                buf = buf.Remove(buf.LastIndexOf('.')).ToLower();

                switch (buf)
                {
                    case "x0":
                        filename_X0 = file;
                        break;
                    case "f":
                        filename_b = file;
                        break;
                    default:
                        if (filenames_format.ContainsKey(buf))
                            filenames_format[buf] = file;
                        else
                        {
                            MessageBox.Show("Файл "+ buf + " имеет неверное название");
                            return;
                        }
                        break;
                }
                puths[name_arr.IndexOf(buf)].Text = file;
            }
            button_load.Enabled = true;
            multireadFlag = true;
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            FileStream file = new FileStream(filename_b, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            var k = reader.ReadLine().Split();
            Factory.RightVector = new SimpleVector(k.Length);

            for (int i=0; i<k.Length; i++)
                Factory.RightVector[i] = Convert.ToDouble(k[i]);

            reader.Close();
            file.Close();

            file = new FileStream(filename_X0, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(file);
            k = reader.ReadLine().Split();
            Factory.X0 = new SimpleVector(k.Length);
            for (int i = 0; i < k.Length; i++)
                Factory.X0[i] = Convert.ToDouble(k[i]);

            reader.Close();
            file.Close();

            Form1.format.Enabled = true;
            this.Visible = false;
        }

        private void FileLoadForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                Form1.justDoIt.Enabled = true;
                Form1.loadFiles.Enabled = true;
                Form1.next.Enabled = true;
                Form1.format.Enabled = true;
            }
        }

        private void FileLoadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.justDoIt.Enabled = true;
            Form1.loadFiles.Enabled = true;
            Form1.format.Enabled = false;
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (multireadFlag)
            {
                unFiledFields = name_arr.Count;
                button_load.Enabled = false;
                filename_b = null;
                filename_X0 = null;

                foreach (var i in puths)
                {
                    i.Enabled = true;
                    i.Text = "";
                }
            }

            multireadFlag = false;
            Button btn = (Button)sender;
            openFileDialog1.Multiselect = false;

            int pressedButton = input_buttons.IndexOf(btn);

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            unFiledFields--;
            if (unFiledFields == 0)
                button_load.Enabled = true;

            puths[pressedButton].Text = openFileDialog1.FileName;
            if (pressedButton == input_buttons.Count - 1)
            {
                filename_X0 = openFileDialog1.FileName;
                return;
            }
            if (pressedButton == input_buttons.Count - 2)
            {
                filename_b = openFileDialog1.FileName;
                return;
            }
            filenames_format[name_arr[pressedButton]] = openFileDialog1.FileName;
        }
    }
}
