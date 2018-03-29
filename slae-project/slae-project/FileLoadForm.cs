﻿using System;
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
        public static IVector F = new SimpleVector();//правая часть
        public static IVector X0 = new SimpleVector();//Начально приближение 
        string filename_b = null, filename_X0 = null;
        bool multireadFlag = false;

        Button button_load;

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

            arrays = Factory.name_arr; //<==========================================================LOOK AT ME
            int count_arr = arrays.Count();

            string mess = "";
            mess += "Выберите массивы со следующими названиями: ";
            mess += "F.txt, X0.txt,";
            for (int i = 0; i < arrays.Count(); i++)
                mess += " " + arrays[i] + ".txt,";
            mess = mess.Remove(mess.Length - 1);

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
            button_all.Click += new System.EventHandler(MultireadingButton_Click);
            this.Controls.Add(button_all);

            y += 50;
            puths.Clear();
            name_arr.Clear();
            filenames_format.Clear();
            input_buttons.Clear();

            for (int i = 0; i < count_arr; i++)
            {

                Label name = new Label();
                name.Text = arrays[i];
                name.Size = new Size(50, 15);
                name.Location = new System.Drawing.Point(x_l, y);
                filenames_format.Add(name.Text, "");
                this.Controls.Add(name);
                name.BackColor = Color.Transparent;
                name_arr.Add(name.Text);

                TextBox puth = new TextBox();
                puth.Size = new Size(185, 20);
                puth.Name = i.ToString();
                puth.Location = new Point(x_p, y);
                puths.Add(puth);
                sootvet.Add(name, puth);
                this.Controls.Add(puth);

                Button button = new Button();
                obzors.Add(button);
                button.Text = "Обзор";
                button.Size = new Size(75, 23);
                button.Location = new Point(x_b, y);
                button.Click += new System.EventHandler(button_Click);
                this.Controls.Add(button);
                input_buttons.Add(button);

                y += 33;
            }
            //правая часть
            Label name_b = new Label();
            name_b.Text = "f";
            name_b.Size = new Size(25, 15);
            name_b.Location = new System.Drawing.Point(x_l, y);
            //name_arr.Add(name_b.Text, name_b);
            this.Controls.Add(name_b);
            name_b.BackColor = Color.Transparent;
            name_arr.Add(name_b.Text);

            TextBox puth_b = new TextBox();
            puth_b.Size = new Size(185, 20);
            puth_b.Name = "f";
            puth_b.Location = new Point(x_p, y);
            puths.Add(puth_b);
            this.Controls.Add(puth_b);
            sootvet.Add(name_b, puth_b);

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
            Label name_x0 = new Label();
            name_x0.Text = "x0";
            name_x0.Size = new Size(25, 15);
            name_x0.Location = new System.Drawing.Point(x_l, y);
            //name_arr.Add(name_x0.Text, name_x0);
            this.Controls.Add(name_x0);
            name_x0.BackColor = Color.Transparent;
            name_arr.Add(name_x0.Text);

            TextBox puth_x0 = new TextBox();
            puth_x0.Size = new Size(185, 20);
            puth_x0.Name = "x0";
            puth_x0.Location = new Point(x_p, y);
            puths.Add(puth_x0);
            this.Controls.Add(puth_x0);
            sootvet.Add(name_x0, puth_x0);

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
            button_load = new Button();
            button_load.Text = "ОК";
            button_load.Size = new Size(183, 23);
            button_load.Location = new Point(x_p, y);
            button_load.Click += new System.EventHandler(this.button_load_Click);
            this.Controls.Add(button_load);
            button_load.Enabled = false;

            unFiledFields = name_arr.Count;
            arrays.Add("F"); arrays.Add("X0");
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
                            MessageBox.Show("Файл "+ buf + " имеет неверное наименование");
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

            int size = Convert.ToInt32(reader.ReadLine());
            var k = reader.ReadLine().Split();

            F = new SimpleVector(size);
            for (int r = 0; r < size; r++)
            {
                F[r] = Convert.ToDouble(k[r]);
            }
            reader.Close();
            file.Close();

            file = new FileStream(filename_X0, FileMode.Open, FileAccess.Read);
            reader = new StreamReader(file);

            size = Convert.ToInt32(reader.ReadLine());
            k = reader.ReadLine().Split();

            X0 = new SimpleVector(size);
            for (int r = 0; r < size; r++)
            {
                X0[r] = Convert.ToDouble(k[r]);
            }

            this.Close();
            return;
        }

        private void FileLoadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.justDoIt.Enabled = true;
            Form1.loadFiles.Enabled = true;
            Form1.next.Enabled = true;
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
