using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace slae_project
{
    public partial class Form1 : Form
    {
        Dictionary<int, Label> name_arr = new Dictionary<int, Label>();
        public string str_format_matrix; //формат матрицы
        public string str_solver; //тип решателя
        public string str_precond; //тип предобусловлевания
        public bool property_matr = false; //симметричность матрицы: по умолчанию несимметричная
        private int numbForm;
        private bool hand;
        private bool property;
        private bool writeMyMatrix = false;

        public Button next, exit, back, justDoIt, icon;
        public GroupBox gr;
        public PictureBox picture;
        public RadioButton fileRead, myRead;
        public Label sizel, iconRule, formMatrix, solvMatrix, precondMatrix;
        public CheckBox propertyMatrix;
        public ComboBox solver, format, precond;
        public NumericUpDown size;

        public Form1()
        {
            numbForm = 1;
            hand = false;
            property = false;

            InitializeComponent();

            this.Size = new Size(500, 375);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

            /////////////////////////////////1 окно//////////////////////////////////////////

            List<string> arrays = new List<string> { "Ввод данных", "ввести данные из файла", "ввести вручную" };

            gr = new GroupBox();
            gr.Text = arrays[0];
            gr.Size = new Size(430, 200);
            gr.Location = new System.Drawing.Point(17, 40);
            this.Controls.Add(gr);

            fileRead = new RadioButton();
            fileRead.Text = arrays[1];
            fileRead.Size = new Size(170, 20);
            fileRead.Location = new Point(50, 100);
            fileRead.Checked = true;
            fileRead.CheckedChanged += new System.EventHandler(readClick);
            this.Controls.Add(fileRead);
            fileRead.BringToFront();

            myRead = new RadioButton();
            myRead.Text = arrays[2];
            myRead.Size = new Size(150, 20);
            myRead.Location = new Point(50, 150);
            myRead.Checked = false;
            myRead.CheckedChanged += new System.EventHandler(readClick);
            this.Controls.Add(myRead);
            myRead.BringToFront();

            picture = new PictureBox();
            picture.Size = new Size(100, 56);
            picture.Image = Properties.Resources.matrix;
            picture.Location = new Point(300, 110);
            this.Controls.Add(picture);
            picture.BringToFront();

            next = new Button();
            next.Text = "Далее";
            next.Size = new Size(100, 30);
            next.Location = new Point(210, 270);
            next.Click += new System.EventHandler(nextClick);
            this.Controls.Add(next);

            back = new Button();
            back.Text = "Назад";
            back.Size = new Size(100, 30);
            back.Location = new Point(75, 270);
            back.Click += new System.EventHandler(backClick);
            this.Controls.Add(back);
            back.Visible = false;

            exit = new Button();
            exit.Text = "Выход";
            exit.Size = new Size(100, 30);
            exit.Location = new Point(345, 270);
            exit.Click += new System.EventHandler(exitClick);
            this.Controls.Add(exit);

            /////////////////////////////////2 окно - ввести данные из файла//////////////////////////////////////////

            propertyMatrix = new CheckBox();
            propertyMatrix.Text = "Симметричная матрица";
            propertyMatrix.Size = new Size(200, 20);
            propertyMatrix.Location = new Point(75, 189);
            propertyMatrix.Checked = false;
            propertyMatrix.CheckedChanged += new System.EventHandler(propertyChange);
            this.Controls.Add(propertyMatrix);
            propertyMatrix.Visible = false;
            propertyMatrix.BringToFront();
            
            format = new ComboBox();
            format.Size = new Size(200, 30);
            format.Location = new System.Drawing.Point(225, 70);
            format.Items.Add("Плотный");
            format.Items.Add("Строчный");
            format.Items.Add("Строчно - столбцовый");
            format.Items.Add("Координатный");
            format.SelectedIndex = 0;
            format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(format);
            format.Visible = false;
            format.BringToFront();

            formMatrix = new Label();
            formMatrix.Text = "Формат матрицы";
            formMatrix.Size = new Size(100, 15);
            formMatrix.Location = new System.Drawing.Point(70, 70);
            name_arr.Add(60, formMatrix);
            this.Controls.Add(formMatrix);
            formMatrix.Visible = false;
            formMatrix.BringToFront();

            solver = new ComboBox();
            solver.Size = new Size(200, 30);
            solver.Location = new System.Drawing.Point(225, 110);
            solver.Items.Add("Метод сопряжённых градиентов");
            solver.Items.Add("Локально-оптимальная схема");
            solver.Items.Add("Метод Якоби");
            solver.Items.Add("Метод Зейделя");
            solver.Items.Add("Метод бисопряжённых градиентов");
            solver.Items.Add("Метод обобщённых минимальных невязок");
            solver.SelectedIndex = 0;
            solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(solver);
            solver.Visible = false;
            solver.BringToFront();

            solvMatrix = new Label();
            solvMatrix.Text = "Решатель";
            solvMatrix.Size = new Size(150, 15);
            solvMatrix.Location = new System.Drawing.Point(70, 110);
            name_arr.Add(90, solvMatrix);
            this.Controls.Add(solvMatrix);
            solvMatrix.Visible = false;
            solvMatrix.BringToFront();


            precond = new ComboBox();
            precond.Size = new Size(200, 30);
            precond.Location = new System.Drawing.Point(225, 150);
            precond.Items.Add("Диагональное");
            precond.Items.Add("Методом Зейделя");
            precond.Items.Add("LU-разложение");
            precond.SelectedIndex = 0;
            precond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(precond);
            precond.Visible = false;
            precond.BringToFront();


            precondMatrix = new Label();
            precondMatrix.Text = "Предобусловливатель";
            precondMatrix.Size = new Size(150, 15);
            precondMatrix.Location = new System.Drawing.Point(70, 150);
            name_arr.Add(120, precondMatrix);
            this.Controls.Add(precondMatrix);
            precondMatrix.Visible = false;
            precondMatrix.BringToFront();

            /////////////////////////////////2 окно - ввести данные вручную//////////////////////////////////////////

            sizel = new Label();
            sizel.Text = "Введите размер:";
            sizel.Size = new Size(200, 15);
            sizel.Location = new System.Drawing.Point(50, 87);
            name_arr.Add(107, sizel);
            this.Controls.Add(sizel);
            sizel.BringToFront();
            sizel.Visible = false;

            size = new NumericUpDown();
            size.Size = new Size(40, 60);
            size.Location = new System.Drawing.Point(150, 87);
            this.Controls.Add(size);
            size.BringToFront();
            size.Visible = false;
            int[] ItemOb = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            size.Minimum = 2;
            size.Maximum = 10;

            icon = new Button();
            icon.Image = Properties.Resources.questionImage;
            icon.Size = new Size(42, 42);
            icon.Location = new Point(205, 77);
            this.Controls.Add(icon);
            icon.Visible = false;
            icon.BringToFront();
            icon.MouseEnter += new System.EventHandler(iconMouseEnter);
            icon.MouseLeave += new System.EventHandler(iconMouseLeave);

            iconRule = new Label();
            iconRule.Text = "НИИИЗЯЯЯЯЯЯ\nМинимальный размер матрицы 2х2;\nМаксимальный размер 10х10.";
            iconRule.Size = new Size(195, 50);
            iconRule.Font = new Font("Times New Roman", 8, FontStyle.Italic);
            iconRule.ForeColor = Color.Red;
            iconRule.Location = new Point(250, 77);
            iconRule.Visible = false;
            this.Controls.Add(iconRule);
            iconRule.BringToFront();

            System.Drawing.Drawing2D.GraphicsPath Button_Path = new System.Drawing.Drawing2D.GraphicsPath();
            Button_Path.AddEllipse(2, 2, this.icon.Width - 4, this.icon.Height - 4);
            Region Button_Region = new Region(Button_Path);
            this.icon.Region = Button_Region;

            justDoIt = new Button();
            justDoIt.Text = "Построить матрицу";
            justDoIt.Size = new Size(200, 30);
            justDoIt.Location = new Point(50, 135);
            justDoIt.Click += new System.EventHandler(justDoItClick);
            this.Controls.Add(justDoIt);
            justDoIt.BringToFront();
            justDoIt.Visible = false;
        }

        private void exitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        

        private void readClick(object sender, EventArgs e)
        {
            if (fileRead.Checked == true) hand = false;
            else hand = true;
        }

        private void nextClick(object sender, EventArgs e)
        {
            numbForm++;
            this.Update();
        }

        private void backClick(object sender, EventArgs e)
        {
            numbForm--;
            this.Update();
        }

        private void propertyChange(object sender, EventArgs e)
        {
            property = !property;
        }

        private void justDoItClick(object sender, EventArgs e)
        {
            writeSize("mymatrixSet.txt");
            writeMyMatrix = true;
            next.Enabled = true;
            matrixForm form = new matrixForm();
            form.Show();
        }

        private void writeSize(string name)
        {
            using (StreamWriter writer = File.CreateText(name))
            {
                writer.WriteLine((int)size.Value);
                if(property)
                    writer.WriteLine("1");
                else writer.WriteLine("0");
            }
            
        }

        private void iconMouseEnter(object sender, EventArgs e)
        {
            iconRule.Visible = true;
        }

        private void iconMouseLeave(object sender, EventArgs e)
        {
            iconRule.Visible = false;
        }

        private  new void Update()
        {
            if (numbForm == 1) //
            {
                picture.Visible = true;
                gr.Visible = true;
                fileRead.Visible = true;
                myRead.Visible = true;
                back.Visible = false;
                propertyMatrix.Visible = false;
                format.Visible = false;
                solver.Visible = false;
                precond.Visible = false;
                precondMatrix.Visible = false;
                solvMatrix.Visible = false;
                formMatrix.Visible = false;
                size.Visible = false;
                sizel.Visible = false;
                justDoIt.Visible = false;
                icon.Visible = false;
            }
            
            if (numbForm == 2) //
            {
                fileRead.Visible = false;
                myRead.Visible = false;
                back.Visible = true;
                picture.Visible = false;

                if (hand == false) 
                {
                    format.Visible = true;
                    formMatrix.Visible = true;
                    solver.Visible = true;
                    precond.Visible = true;
                    precondMatrix.Visible = true;
                    solvMatrix.Visible = true;
                    propertyMatrix.Visible = true;
                    
                }
                else
                {
                    sizel.Visible = true;
                    size.Visible = true;
                    justDoIt.Visible = true;
                    icon.Visible = true;
                    propertyMatrix.Visible = true;
                    solver.Visible = false;
                    precond.Visible = false;
                    precondMatrix.Visible = false;
                    solvMatrix.Visible = false;
                    if(!writeMyMatrix) next.Enabled = false;
                    
                }  
            }

            if (numbForm == 3) // третья страница, надо вставить в (*) чтение файлов и 
                               // запись их в необходимые массивы по нажатию на "далее" (это button next)
            {
                if (hand == false) // (*)
                {
                    format.Visible = false;
                    formMatrix.Visible = false;
                    solver.Visible = false;
                    precond.Visible = false;
                    precondMatrix.Visible = false;
                    solvMatrix.Visible = false;
                    propertyMatrix.Visible = false;
                }
                else
                {
                    sizel.Visible = false;
                    size.Visible = false;
                    justDoIt.Visible = false;
                    icon.Visible = false;
                    propertyMatrix.Visible = false;
                    solver.Visible = true;
                    precond.Visible = true;
                    solvMatrix.Visible = true;
                    precondMatrix.Visible = true;
                }
            }

            if (numbForm == 4) 
            {
                if (hand == false) 
                {

                }
                else
                {

                }
            }
        }


        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            infoForm form = new infoForm();
            form.Show();
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutProgramForm form = new aboutProgramForm();
            form.Show();
    //        if (MessageBox.Show(
    //    "Написать разработчикам?", "Visit", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk
    //) == DialogResult.Yes)
    //        {
    //            System.Diagnostics.Process.Start("http://pm43@mail.ru");
    //        }
            //MessageBox.Show("BLABLABLA версия 0.1. от 25 марта 2018. Специальное издание для Windows \nНаписать разработчикам: " , "О программе", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }
        

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
     }
}
