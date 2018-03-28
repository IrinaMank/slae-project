using System;
using System.Collections.Generic;
using System.IO;
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
        Dictionary<int, Label> name_arr = new Dictionary<int, Label>();
        public static string str_format_matrix; //формат матрицы
        public static string str_solver; //тип решателя
        public static string str_precond; //тип предобусловлевания
        public static bool property_matr = false; //симметричность матрицы: по умолчанию несимметричная
        private bool inputModeHand = true;
        public static double accurent = 0.1;
        public static int maxiter = 1000;
        public double percent = 0;
        public int ourIter = 0;
        String[] precondTypesList;
        String[] matrixTypesList;
        String[] solverTypesList;
        Factory f;

        public static Button next, exit, back, justDoIt, icon, loadFiles, graphic;
        public GroupBox gr;
        public PictureBox picture;
        public RadioButton fileRead, myRead;
        public Label sizel, iconRule, formMatrix, solvMatrix, precondMatrix, accl, maxiterl, iterLife;
        public CheckBox propertyMatrix;
        public ComboBox solver, format, precond;
        public NumericUpDown size, acc;
        public TextBox maxit;
        public static ProgressBar bar;

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public Form1()
        {           
            InitializeComponent();
            this.Size = new Size(500, 375);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            Factory f = new Factory();
            //получаем из Фабрики самую свежую и точную инфу!
            precondTypesList = Factory.PrecondTypes.Keys.ToArray();
            matrixTypesList = Factory.MatrixTypes.Keys.ToArray();
            solverTypesList = Factory.SolverTypes.Keys.ToArray();
            /////////////////////////////////1 окно//////////////////////////////////////////

            List<string> arrays = new List<string> { "Ввод данных", "ввести данные из файла", "ввести вручную" };

            next = new Button();
            next.Text = "Решить";
            next.Size = new Size(100, 30);
            next.Location = new Point(210, 290);
            next.Click += new System.EventHandler(nextClick);
            this.Controls.Add(next);
            next.BackColor = Color.White;
            next.Enabled = false;

            format = new ComboBox();
            format.Size = new Size(210, 30);
            format.Location = new System.Drawing.Point(200, 70);
            //чтобы все было через Фабрику, как у людей
            for (int i = 0; i < matrixTypesList.Length; i++)
                format.Items.Add(matrixTypesList[i]);

            format.SelectedIndex = 0;
            format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(format);
            format.BringToFront();

            formMatrix = new Label();
            formMatrix.Text = "Формат матрицы";
            formMatrix.Size = new Size(110, 15);
            formMatrix.Location = new System.Drawing.Point(60, 70);
            name_arr.Add(60, formMatrix);
            this.Controls.Add(formMatrix);
            formMatrix.BringToFront();
            formMatrix.BackColor = Color.Transparent;

            solver = new ComboBox();
            solver.Size = new Size(210, 30);
            solver.Location = new System.Drawing.Point(200, 40);
            for (int i = 0; i < solverTypesList.Length; i++)
                solver.Items.Add(solverTypesList[i]);

            solver.SelectedValueChanged += new System.EventHandler(solverChanged);
            solver.SelectedIndex = 0;
            solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(solver);
            solver.BringToFront();
            solver.BackColor = Color.White;

            solvMatrix = new Label();
            solvMatrix.Text = "Решатель";
            solvMatrix.Size = new Size(80, 15);
            solvMatrix.Location = new System.Drawing.Point(60, 40);
            name_arr.Add(90, solvMatrix);
            this.Controls.Add(solvMatrix);
            solvMatrix.BringToFront();
            solvMatrix.BackColor = Color.Transparent;

            precond = new ComboBox();
            precond.Size = new Size(210, 30);
            precond.Location = new System.Drawing.Point(200, 100);
            for (int i = 0; i < precondTypesList.Length; i++)
            {
                precond.Items.Add(precondTypesList[i]);
            }
            //precond.Items.Add("Диагональное");
            //precond.Items.Add("Методом Зейделя");
            //precond.Items.Add("LU-разложение");
            precond.SelectedIndex = 0;
            precond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(precond);
            precond.BringToFront();
            precond.BackColor = Color.White;

            precondMatrix = new Label();
            precondMatrix.Text = "Предобусловливание";
            precondMatrix.Size = new Size(120, 15);
            precondMatrix.Location = new System.Drawing.Point(60, 100);
            name_arr.Add(120, precondMatrix);
            this.Controls.Add(precondMatrix);
            precondMatrix.BringToFront();
            precondMatrix.BackColor = Color.Transparent;

            accl = new Label();
            accl.Text = "Точность решения:               1E -";
            accl.Size = new Size(200, 15);
            accl.Location = new System.Drawing.Point(60, 140);
            name_arr.Add(108, accl);
            this.Controls.Add(accl);
            accl.BringToFront();
            accl.BackColor = Color.Transparent;

            acc = new NumericUpDown();
            acc.Size = new Size(40, 60);
            acc.Location = new System.Drawing.Point(235, 139);
            this.Controls.Add(acc);
            acc.BringToFront();
            acc.Minimum = 1;
            acc.Maximum = 16;
            acc.Value = 10;


            maxiterl = new Label();
            maxiterl.Text = "Максимальное число итераций:";
            maxiterl.Size = new Size(190, 15);
            maxiterl.Location = new System.Drawing.Point(60, 170);
            name_arr.Add(109, maxiterl);
            this.Controls.Add(maxiterl);
            maxiterl.BringToFront();
            maxiterl.BackColor = Color.Transparent;

            maxit = new TextBox();
            maxit.Size = new Size(40, 100);
            maxit.Location = new System.Drawing.Point(235, 170);
            this.Controls.Add(maxit);
            maxit.BringToFront();

            maxit.Text = "1000";
            maxit.TextChanged += new System.EventHandler(maxitTextChange);

            propertyMatrix = new CheckBox();
            propertyMatrix.Text = "Симметричная матрица";
            propertyMatrix.Size = new Size(200, 20);
            propertyMatrix.Location = new Point(60, 198);
            propertyMatrix.Checked = false;
            propertyMatrix.CheckedChanged += new System.EventHandler(propertyChange);
            this.Controls.Add(propertyMatrix);
            propertyMatrix.BringToFront();
            propertyMatrix.BackColor = Color.Transparent;

            justDoIt = new Button();
            justDoIt.Text = "Ручной ввод";
            justDoIt.Size = new Size(100, 30);
            justDoIt.Location = new Point(310, 138);
            justDoIt.Click += new System.EventHandler(justDoItClick);
            this.Controls.Add(justDoIt);
            justDoIt.BringToFront();

            loadFiles = new Button();
            loadFiles.Text = "Файловый ввод";
            loadFiles.Size = new Size(100, 30);
            loadFiles.Location = new Point(310, 173);
            loadFiles.Click += new System.EventHandler(loadFilesClick);
            this.Controls.Add(loadFiles);
            loadFiles.BringToFront();

            bar = new ProgressBar();
            bar.Size = new Size(370, 20);
            bar.Location = new System.Drawing.Point(50, 225);
            this.Controls.Add(bar);
            bar.BringToFront();

            bar.Maximum = 100;

        }

        private void graphicClick(object sender, EventArgs e)
        {
            SharpGL_limbo.SharpGL_Open();
        }

        private void exitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static void updateProgressBar(int perc)
        {
            bar.Value = perc;
        }

        public void solverChanged(object sender, EventArgs e)
        {
            var s = solver.SelectedValue;
        }

        private void readClick(object sender, EventArgs e)
        {

        }

        private void maxitTextChange(object sender, EventArgs e)
        {
            try
            {
                maxit.Text = Convert.ToUInt16(maxit.Text).ToString();
            }
            catch
            {
                maxit.Text = "0";
            }
        }

        private void threadSolver()
        {
            Factory.CreateMatrix(str_format_matrix);
            Factory.Create_Full_Matrix(str_format_matrix, property_matr);
            Factory.CreatePrecond(str_precond);
            Factory.CreateSolver(str_solver);
            //graphic.Enabled = true;
            //back.Enabled = true;
        }

        private void nextClick(object sender, EventArgs e)
        {
            accurent = Convert.ToDouble("1e-" + acc.Value.ToString());

            if (!inputModeHand)
                str_format_matrix = format.SelectedItem.ToString();
            str_solver = solver.SelectedItem.ToString();
            str_precond = precond.SelectedItem.ToString();

            threadSolver();
            
        }

        private void loadFilesClick(object sender, EventArgs e)
        {
            accurent = Convert.ToDouble("1e-" + acc.Value.ToString());
            str_format_matrix = format.SelectedItem.ToString();

            next.Enabled = true;
            FileLoadForm FileLoadForm = new FileLoadForm();
            FileLoadForm.Show();
            format.Enabled = true;
        }

        private void propertyChange(object sender, EventArgs e)
        {
            property_matr = !property_matr;
        }

        private void justDoItClick(object sender, EventArgs e)
        {
            writeSize("mymatrixSet.txt");
            next.Enabled = true;
            matrixForm form = new matrixForm();
            form.Show();
            inputModeHand = true;
            format.SelectedValue = 2;
            format.Enabled = false;
            justDoIt.Enabled = false;
        }

        private void writeSize(string name)
        {
            using (StreamWriter writer = File.CreateText(name))
            {
                writer.WriteLine("0");
                if (property_matr)
                    writer.WriteLine("1");
                else writer.WriteLine("0");
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
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
