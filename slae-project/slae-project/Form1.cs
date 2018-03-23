using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using slae_project.Logger;

namespace slae_project
{
    public partial class Form1 : Form
    {
        //FileLogger fileLogger = new FileLogger();
        Dictionary<int, Label> name_arr = new Dictionary<int, Label>();
        public static string str_format_matrix; //формат матрицы
        public static string str_solver; //тип решателя
        public static string str_precond; //тип предобусловлевания
        public static bool property_matr = false; //симметричность матрицы: по умолчанию несимметричная
        private int numbForm;
        private bool hand;
        private bool writeMyMatrix = false;
        private bool loadData = false;
        public static double accurent = 0.1;
        public static int maxiter = 1000;
        public double percent = 0;
        public int ourIter = 0;


        public Button next, exit, back, justDoIt, icon, loadFiles;
        public GroupBox gr;
        public PictureBox picture;
        public RadioButton fileRead, myRead;
        public Label sizel, iconRule, formMatrix, solvMatrix, precondMatrix, accl, maxiterl, iterLife;
        public CheckBox propertyMatrix;
        public ComboBox solver, format, precond;
        public NumericUpDown size, acc;
        public TextBox maxit;
        public ProgressBar bar;
        

        public Form1()
        {
            numbForm = 1;
            hand = false;

            InitializeComponent();
            Factory f = new Factory();
            this.Size = new Size(500, 375);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            /////////////////////////////////1 окно//////////////////////////////////////////

            List<string> arrays = new List<string> { "Ввод данных", "ввести данные из файла", "ввести вручную" };

            gr = new GroupBox();
            gr.Text = arrays[0];
            gr.Size = new Size(430, 225);
            gr.Location = new System.Drawing.Point(17, 40);
            this.Controls.Add(gr);
            gr.BackColor = Color.Transparent;

            fileRead = new RadioButton();
            fileRead.Text = arrays[1];
            fileRead.Size = new Size(170, 20);
            fileRead.Location = new Point(50, 100);
            fileRead.Checked = true;
            fileRead.CheckedChanged += new System.EventHandler(readClick);
            this.Controls.Add(fileRead);
            fileRead.BringToFront();
            fileRead.BackColor = Color.Transparent;

            myRead = new RadioButton();
            myRead.Text = arrays[2];
            myRead.Size = new Size(150, 20);
            myRead.Location = new Point(50, 150);
            myRead.Checked = false;
            myRead.CheckedChanged += new System.EventHandler(readClick);
            this.Controls.Add(myRead);
            myRead.BringToFront();
            myRead.BackColor = Color.Transparent;

            //picture = new PictureBox();
            //picture.Size = new Size(100, 56);
            //picture.Image = Properties.Resources.matrix;
            //picture.Location = new Point(300, 110);
            //this.Controls.Add(picture);
            //picture.BringToFront();

            next = new Button();
            next.Text = "Далее";
            next.Size = new Size(100, 30);
            next.Location = new Point(210, 290);
            next.Click += new System.EventHandler(nextClick);
            this.Controls.Add(next);
            next.BackColor = Color.White;

            back = new Button();
            back.Text = "Назад";
            back.Size = new Size(100, 30);
            back.Location = new Point(75, 290);
            back.Click += new System.EventHandler(backClick);
            this.Controls.Add(back);
            back.Visible = false;
            back.BackColor = Color.White;

            exit = new Button();
            exit.Text = "Выход";
            exit.Size = new Size(100, 30);
            exit.Location = new Point(345, 290);
            exit.Click += new System.EventHandler(exitClick);
            this.Controls.Add(exit);
            exit.BackColor = Color.White;

            /////////////////////////////////2 окно - ввести данные из файла//////////////////////////////////////////

            format = new ComboBox();
            format.Size = new Size(210, 30);
            format.Location = new System.Drawing.Point(200, 70);
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
            formMatrix.Size = new Size(110, 15);
            formMatrix.Location = new System.Drawing.Point(60, 70);
            name_arr.Add(60, formMatrix);
            this.Controls.Add(formMatrix);
            formMatrix.Visible = false;
            formMatrix.BringToFront();
            formMatrix.BackColor = Color.Transparent;

            solver = new ComboBox();
            solver.Size = new Size(210, 30);
            solver.Location = new System.Drawing.Point(200, 100);
            solver.Items.Add("Метод сопряжённых градиентов");
            solver.Items.Add("Локально-оптимальная схема");
            //solver.Items.Add("Метод Якоби");
           // solver.Items.Add("Метод Зейделя");
            solver.Items.Add("Метод бисопряжённых градиентов");
           // solver.Items.Add("Метод обобщённых минимальных невязок");
            solver.SelectedIndex = 0;
            solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(solver);
            solver.Visible = false;
            solver.BringToFront();
            solver.BackColor = Color.White;

            solvMatrix = new Label();
            solvMatrix.Text = "Решатель";
            solvMatrix.Size = new Size(80, 15);
            solvMatrix.Location = new System.Drawing.Point(60, 100);
            name_arr.Add(90, solvMatrix);
            this.Controls.Add(solvMatrix);
            solvMatrix.Visible = false;
            solvMatrix.BringToFront();
            solvMatrix.BackColor = Color.Transparent;
            
            precond = new ComboBox();
            precond.Size = new Size(210, 30);
            precond.Location = new System.Drawing.Point(200, 130);
            precond.Items.Add("Диагональное");
            precond.Items.Add("Методом Зейделя");
            precond.Items.Add("LU-разложение");
            precond.SelectedIndex = 0;
            precond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(precond);
            precond.Visible = false;
            precond.BringToFront();
            precond.BackColor = Color.White;

            precondMatrix = new Label();
            precondMatrix.Text = "Предобусловливание";
            precondMatrix.Size = new Size(120, 15);
            precondMatrix.Location = new System.Drawing.Point(60, 130);
            name_arr.Add(120, precondMatrix);
            this.Controls.Add(precondMatrix);
            precondMatrix.Visible = false;
            precondMatrix.BringToFront();
            precondMatrix.BackColor = Color.Transparent;

            accl = new Label();
            accl.Text = "Точность решения:               10E -";
            accl.Size = new Size(200, 15);
            accl.Location = new System.Drawing.Point(60, 170);
            name_arr.Add(108, accl);
            this.Controls.Add(accl);
            accl.BringToFront();
            accl.Visible = false;
            accl.BackColor = Color.Transparent;
            
            acc = new NumericUpDown();
            acc.Size = new Size(40, 60);
            acc.Location = new System.Drawing.Point(235, 169);
            this.Controls.Add(acc);
            acc.BringToFront();
            acc.Visible = false;
            acc.Minimum = 1;
            acc.Maximum = 16;
            acc.ValueChanged += new System.EventHandler(accValueChanged);

            maxiterl = new Label();
            maxiterl.Text = "Максимальное число итераций:";
            maxiterl.Size = new Size(190, 15);
            maxiterl.Location = new System.Drawing.Point(60, 200);
            name_arr.Add(109, maxiterl);
            this.Controls.Add(maxiterl);
            maxiterl.BringToFront();
            maxiterl.Visible = false;
            maxiterl.BackColor = Color.Transparent;

            maxit = new TextBox();
            maxit.Size = new Size(40, 100);
            maxit.Location = new System.Drawing.Point(235, 200);
            this.Controls.Add(maxit);
            maxit.BringToFront();
            maxit.Visible = false;
            maxit.Text = "1000";
            maxit.TextChanged += new System.EventHandler(maxitTextChange);

            propertyMatrix = new CheckBox();
            propertyMatrix.Text = "Симметричная матрица";
            propertyMatrix.Size = new Size(200, 20);
            propertyMatrix.Location = new Point(60, 228);
            propertyMatrix.Checked = false;
            propertyMatrix.CheckedChanged += new System.EventHandler(propertyChange);
            this.Controls.Add(propertyMatrix);
            propertyMatrix.Visible = false;
            propertyMatrix.BringToFront();
            propertyMatrix.BackColor = Color.Transparent;

            loadFiles = new Button();
            loadFiles.Text = "Загрузить\nданные";
            loadFiles.Size = new Size(100, 60);
            loadFiles.Location = new Point(310, 179);
            loadFiles.Click += new System.EventHandler(loadFilesClick);
            this.Controls.Add(loadFiles);
            loadFiles.BringToFront();
            loadFiles.Visible = false;

            /////////////////////////////////2 окно - ввести данные вручную//////////////////////////////////////////

            sizel = new Label();
            sizel.Text = "Введите размер:";
            sizel.Size = new Size(120, 15);
            sizel.Location = new System.Drawing.Point(50, 87);
            name_arr.Add(107, sizel);
            this.Controls.Add(sizel);
            sizel.BringToFront();
            sizel.Visible = false;
            sizel.BackColor = Color.Transparent;

            size = new NumericUpDown();
            size.Size = new Size(40, 60);
            size.Location = new System.Drawing.Point(150, 87);
            this.Controls.Add(size);
            size.BringToFront();
            size.Visible = false;
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
            icon.BackColor = Color.White;

            iconRule = new Label();
            iconRule.Text = "НИИИЗЯЯЯЯЯЯ\nМинимальный размер матрицы 2х2;\nМаксимальный размер 10х10.";
            iconRule.Size = new Size(195, 50);
            iconRule.Font = new Font("Times New Roman", 8, FontStyle.Italic);
            iconRule.ForeColor = Color.Red;
            iconRule.Location = new Point(250, 77);
            iconRule.Visible = false;
            this.Controls.Add(iconRule);
            iconRule.BringToFront();
            iconRule.BackColor = Color.Transparent;

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
            justDoIt.BackColor = Color.White;

            ///////////////////////////////////////////////////////////////////////////////////

            iterLife = new Label();
            iterLife.Text = "";
            iterLife.Size = new Size(50, 15);
            iterLife.Location = new System.Drawing.Point(50, 170);
            name_arr.Add(147, iterLife);
            this.Controls.Add(iterLife);
            iterLife.BringToFront();
            iterLife.Visible = false;
            iterLife.BackColor = Color.Transparent;

            bar = new ProgressBar();
            bar.Size = new Size(300, 40);
            bar.Location = new System.Drawing.Point(50, 120);
            this.Controls.Add(bar);
            bar.BringToFront();
            bar.Visible = false;
            bar.Maximum = 100;

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

        private void maxitTextChange(object sender, EventArgs e)
        {
            string iter;
            iter = maxit.Text;
            textToOnlyNumbers(ref iter);
            double iterat;
            iterat = Convert.ToDouble(iter);
            if (iterat > 60000)
                iterat = 60000;
            maxit.Text = iter;
            maxiter = Convert.ToInt32(iterat);
        }

        private void accValueChanged(object sender, EventArgs e)
        {
            
        }

        public void textToOnlyNumbers(ref string text) // Проверка, на цифру в поле
        {                                               //если чо-то левое то обнуление полей
            string buff = text;
            string result = "";
            int lettersCount = text.Count();
            
            for (int i = 0; i < lettersCount; i++)
                if (buff[i] >= '0' && buff[i] <= '9')
                {
                    result += buff[i];
                }
            if (result == "")
                result = "0";
            text = result;
        }

        private void threadSolver()
        {
            Factory.CreateMatrix(str_format_matrix);
            Factory.Create_Full_Matrix(str_format_matrix);

            Factory.CreateSolver(str_solver);
            
          //  thread.Start(str_solver);
            //var reader = new FileStream("./log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            //StreamReader reader = new StreamReader("./log.txt");
           // while (ourIter < maxiter || percent < 100)
            //{
                //string s = writer.ReadLine();

                //string[] ss;
                //while (s.Length != 0)
                //{
                //    ss = s.Split(' ');
                //    s = writer.ReadLine();
                //}

                //ourIter = reader.Read();
                //percent = reader.Read();
                //if (percent != -1)
                //    bar.Value = Convert.ToInt32(percent);
                //if (ourIter != -1)
                //    iterLife.Text = ourIter.ToString();

                //ourIter = fileLogger.iter;
                //percent = fileLogger.per;
             //   bar.Value = Convert.ToInt32(percent);
             //   iterLife.Text = ourIter.ToString();
             //   Update();
           // }
            
            //reader.Close();
            //writer.Close();
        }

        private void nextClick(object sender, EventArgs e)
        {
            numbForm++;
            accurent = Math.Pow(10, -Convert.ToDouble(acc.Value));
            if (!hand && numbForm == 3)
                threadSolver();
            this.Update();
        }

        private void loadFilesClick(object sender, EventArgs e)
        {

            if (!hand && numbForm == 2)
            {
                str_format_matrix = format.SelectedItem.ToString();
                str_solver = solver.SelectedItem.ToString(); 
                str_precond = precond.SelectedItem.ToString(); 
            }
            if (hand && numbForm == 3)
            {
                str_solver = solver.SelectedItem.ToString();
                str_precond = precond.SelectedItem.ToString();
            }
            accurent = Math.Pow(10, -Convert.ToDouble(acc.Value));

            loadData = true;
            next.Enabled = true;
            Form2 form2 = new Form2();
            form2.Show();
        }
        

        private void backClick(object sender, EventArgs e)
        {
            numbForm--;
            this.Update();
        }

        private void propertyChange(object sender, EventArgs e)
        {
            property_matr = !property_matr;
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
                if(property_matr)
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
                loadFiles.Visible = false;
                next.Enabled = true;
                accl.Visible = false;
                acc.Visible = false;
                maxit.Visible = false;
                maxiterl.Visible = false;
            }
            
            if (numbForm == 2) //
            {
                fileRead.Visible = false;
                myRead.Visible = false;
                back.Visible = true;
                //picture.Visible = false;

                if (hand == false) 
                {
                    format.Visible = true;
                    formMatrix.Visible = true;
                    solver.Visible = true;
                    precond.Visible = true;
                    precondMatrix.Visible = true;
                    solvMatrix.Visible = true;
                    propertyMatrix.Visible = true;
                    loadFiles.Visible = true;
                    accl.Visible = true;
                    acc.Visible = true;
                    maxiterl.Visible = true;
                    maxit.Visible = true;
                    if (!loadData) next.Enabled = false;
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
                    loadFiles.Visible = false;
                    maxit.Visible = false;
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
                    loadFiles.Visible = false;
                    accl.Visible = false;
                    acc.Visible = false;
                    maxiterl.Visible = false;
                    maxit.Visible = false;
                    gr.Visible = false;
                    next.Visible = false;
                    bar.Visible = true;
                    iterLife.Visible = true;
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
                    maxit.Visible = false;
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
