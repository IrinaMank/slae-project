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
        public static string str_format_matrix; //формат матрицы
        public static string str_solver; //тип решателя
        public static string str_precond; //тип предобусловлевания
        public static bool property_matr = false; //симметричность матрицы: по умолчанию несимметричная
        public static double accurent = 0.1;
        public static int maxiter = 1000;
        public double percent = 0;
        public int ourIter = 0;
        String[] precondTypesList;
        String[] matrixTypesList;
        String[] solverTypesList;

        public static Button next, exit, back, justDoIt, icon, loadFiles, graphics;
        public GroupBox gr;
        public PictureBox picture;
        public RadioButton fileRead, myRead;
        public Label sizel, iconRule, formMatrix, solvMatrix, precondMatrix, accl, maxiterl, iterLife;
        public CheckBox propertyMatrix;
        public ComboBox solver, format, precond;
        public NumericUpDown size, acc;
        public TextBox maxit;
        public static ProgressBar bar;
        public matrixForm form = new matrixForm();

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(430, 350);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            Factory f = new Factory();
            //получаем из Фабрики самую свежую и точную инфу!
            precondTypesList = Factory.PrecondTypes.Keys.ToArray();
            matrixTypesList = Factory.MatrixTypes.Keys.ToArray();
            solverTypesList = Factory.SolverTypes.Keys.ToArray();

            format = new ComboBox();
            format.Size = new Size(210, 30);
            format.Location = new System.Drawing.Point(175, 70);
            for (int i = 0; i < matrixTypesList.Length; i++)
                format.Items.Add(matrixTypesList[i]);

            format.SelectedIndex = 0;
            format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(format);
            format.BringToFront();

            formMatrix = new Label();
            formMatrix.Text = "Формат матрицы";
            formMatrix.Size = new Size(110, 15);
            formMatrix.Location = new System.Drawing.Point(35, 73);
            this.Controls.Add(formMatrix);
            formMatrix.BringToFront();
            formMatrix.BackColor = Color.Transparent;

            solver = new ComboBox();
            solver.Size = new Size(210, 30);
            solver.Location = new System.Drawing.Point(175, 40);
            for (int i = 0; i < solverTypesList.Length; i++)
                solver.Items.Add(solverTypesList[i]);

            solver.SelectedIndex = 0;
            solver.SelectionChangeCommitted += new System.EventHandler(comboboxSelectionChangeCommitted);
            solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(solver);
            solver.BringToFront();
            solver.BackColor = Color.White;

            solvMatrix = new Label();
            solvMatrix.Text = "Решатель";
            solvMatrix.Size = new Size(80, 15);
            solvMatrix.Location = new System.Drawing.Point(35, 43);
            this.Controls.Add(solvMatrix);
            solvMatrix.BringToFront();
            solvMatrix.BackColor = Color.Transparent;

            precond = new ComboBox();
            precond.Size = new Size(210, 30);
            precond.Location = new System.Drawing.Point(175, 100);
            for (int i = 0; i < precondTypesList.Length; i++)
                precond.Items.Add(precondTypesList[i]);

            precond.SelectedIndex = 0;
            precond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(precond);
            precond.BringToFront();
            precond.BackColor = Color.White;

            precondMatrix = new Label();
            precondMatrix.Text = "Предобусловливание";
            precondMatrix.Size = new Size(120, 15);
            precondMatrix.Location = new System.Drawing.Point(35, 103);
            this.Controls.Add(precondMatrix);
            precondMatrix.BringToFront();
            precondMatrix.BackColor = Color.Transparent;

            accl = new Label();
            accl.Text = "Точность решения:                   1E-";
            accl.Size = new Size(200, 15);
            accl.Location = new System.Drawing.Point(35, 142);
            this.Controls.Add(accl);
            accl.BringToFront();
            accl.BackColor = Color.Transparent;

            acc = new NumericUpDown();
            acc.Size = new Size(40, 60);
            acc.Location = new System.Drawing.Point(210, 139);
            this.Controls.Add(acc);
            acc.BringToFront();
            acc.Minimum = 1;
            acc.Maximum = 16;
            acc.Value = 10;

            maxiterl = new Label();
            maxiterl.Text = "Максимальное число итераций";
            maxiterl.Size = new Size(190, 15);
            maxiterl.Location = new System.Drawing.Point(35, 173);
            this.Controls.Add(maxiterl);
            maxiterl.BringToFront();
            maxiterl.BackColor = Color.Transparent;

            maxit = new TextBox();
            maxit.Size = new Size(40, 100);
            maxit.Location = new System.Drawing.Point(210, 170);
            this.Controls.Add(maxit);
            maxit.BringToFront();

            maxit.Text = "1000";
            maxit.TextChanged += new System.EventHandler(maxitTextChange);

            propertyMatrix = new CheckBox();
            propertyMatrix.Text = "Симметричная матрица";
            propertyMatrix.Size = new Size(200, 20);
            propertyMatrix.Location = new Point(38, 198);
            propertyMatrix.CheckedChanged += new System.EventHandler(propertyChange);
            this.Controls.Add(propertyMatrix);
            propertyMatrix.BringToFront();
            propertyMatrix.BackColor = Color.Transparent;

            justDoIt = new Button();
            justDoIt.Text = "Ручной ввод";
            justDoIt.Size = new Size(100, 30);
            justDoIt.Location = new Point(285, 138);
            justDoIt.Click += new System.EventHandler(justDoItClick);
            this.Controls.Add(justDoIt);
            justDoIt.BringToFront();

            loadFiles = new Button();
            loadFiles.Text = "Файловый ввод";
            loadFiles.Size = new Size(100, 30);
            loadFiles.Location = new Point(285, 173);
            loadFiles.Click += new System.EventHandler(loadFilesClick);
            this.Controls.Add(loadFiles);
            loadFiles.BringToFront();

            graphics = new Button();
            graphics.Text = "Графика";
            graphics.Size = new Size(100, 30);
            graphics.Location = new Point(165, 260);
            graphics.Click += new System.EventHandler(graphicsClick);
            this.Controls.Add(graphics);
            graphics.BringToFront();
            graphics.Enabled = false;

            next = new Button();
            next.Text = "Решить";
            next.Size = new Size(100, 30);
            next.Location = new Point(285, 260);
            next.Click += new System.EventHandler(nextClick);
            this.Controls.Add(next);
            next.Enabled = false;


            bar = new ProgressBar();
            bar.Size = new Size(350, 20);
            bar.Location = new System.Drawing.Point(35, 225);
            this.Controls.Add(bar);
            bar.BringToFront();
        }

        private void graphicClick(object sender, EventArgs e)
        {
            SharpGL_limbo.SharpGL_Open();
        }

        public static void updateProgressBar(int perc)
        {
            bar.Value = perc;
        }

        private void comboboxSelectionChangeCommitted(object sender, EventArgs e)
        {
            if (solver.SelectedIndex == 2 || solver.SelectedIndex == 3)
            {
                precond.SelectedIndex = 0;
                precond.Enabled = false;
            }
            else
                precond.Enabled = true;
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
            maxiter = Convert.ToUInt16(maxit.Text);
            bar.Maximum = Convert.ToUInt16(maxit.Text);
            accurent = Convert.ToDouble("1e-" + acc.Value.ToString());

            str_format_matrix = format.SelectedItem.ToString();
            str_solver = solver.SelectedItem.ToString();
            str_precond = precond.SelectedItem.ToString();

            threadSolver();
            graphics.Enabled = true;
        }

        private void loadFilesClick(object sender, EventArgs e)
        {
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
            form.Show();
            format.SelectedValue = 2;
            format.Enabled = false;
            justDoIt.Enabled = false;
            loadFiles.Enabled = false;
        }

        public void graphicsClick(object sender, EventArgs e)
        {
            
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
    }
}
