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
        //public static double accurent = 0.1;
       // public static int maxiter = 1000;
        public double percent = 0;
        public int ourIter = 0;
        String[] precondTypesList;
        String[] matrixTypesList;
        String[] solverTypesList;

        public static Button next, exit, back, justDoIt, icon, loadFiles, graphics, fileResult;
        public GroupBox gr;
        public PictureBox picture;
        public RadioButton fileRead, myRead;
        public Label sizel, iconRule, formMatrix, solvMatrix, precondMatrix, accl, maxiterl, iterLife;
        public CheckBox propertyMatrix;
        public static ComboBox solver, format, precond;
        public NumericUpDown size, acc;
        public TextBox maxit;

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public static ProgressBar bar;
        public matrixForm form = new matrixForm();

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

            formMatrix = new Label
            {
                Text = "Формат матрицы",
                Size = new Size(110, 15),
                Location = new System.Drawing.Point(35, 73)
            };
            this.Controls.Add(formMatrix);
            formMatrix.BringToFront();
            formMatrix.BackColor = Color.Transparent;

            solver = new ComboBox
            {
                Size = new Size(210, 30),
                Location = new System.Drawing.Point(175, 40)
            };
            for (int i = 0; i < solverTypesList.Length; i++)
                solver.Items.Add(solverTypesList[i]);

            solver.SelectedIndex = 0;
            solver.SelectionChangeCommitted += new System.EventHandler(comboboxSelectionChangeCommitted);
            solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(solver);
            solver.BringToFront();
            solver.BackColor = Color.White;

            solvMatrix = new Label
            {
                Text = "Решатель",
                Size = new Size(80, 15),
                Location = new System.Drawing.Point(35, 43),
                BackColor = Color.Transparent
            };
            this.Controls.Add(solvMatrix);
            solvMatrix.BringToFront();

            precond = new ComboBox
            {
                Size = new Size(210, 30),
                Location = new System.Drawing.Point(175, 100)
            };
            for (int i = 0; i < precondTypesList.Length; i++)
                precond.Items.Add(precondTypesList[i]);

            precond.SelectedIndex = 0;
            precond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Controls.Add(precond);
            precond.BringToFront();
            precond.BackColor = Color.White;


            precondMatrix = new Label
            {
                Text = "Предобусловливание",
                Size = new Size(120, 15),
                Location = new System.Drawing.Point(35, 103)
            };

            this.Controls.Add(precondMatrix);
            precondMatrix.BringToFront();
            precondMatrix.BackColor = Color.Transparent;

            accl = new Label
            {
                Text = "Точность решения                    1E-",
                Size = new Size(200, 15),
                Location = new System.Drawing.Point(35, 142),
                BackColor = Color.Transparent
            };
            this.Controls.Add(accl);
            accl.BringToFront();

            acc = new NumericUpDown
            {
                Size = new Size(40, 60),
                Location = new System.Drawing.Point(210, 139),
                Minimum = 0,
                Maximum = 16,
                Value = 10
            };
            this.Controls.Add(acc);
            acc.BringToFront();


            maxiterl = new Label
            {
                Text = "Максимальное число итераций",
                Size = new Size(190, 15),
                Location = new System.Drawing.Point(35, 173),
                BackColor = Color.Transparent
            };
            this.Controls.Add(maxiterl);
            maxiterl.BringToFront();

            maxit = new TextBox
            {
                Size = new Size(40, 100),
                Location = new System.Drawing.Point(210, 170),
                Text = "1000"                
            };
            this.Controls.Add(maxit);
            maxit.BringToFront();
            maxit.TextChanged += new System.EventHandler(maxitTextChange);

            propertyMatrix = new CheckBox
            {
                Text = "Симметричная матрица",
                Size = new Size(200, 20),
                Location = new Point(38, 198),
                BackColor = Color.Transparent
            };
            propertyMatrix.CheckedChanged += new System.EventHandler(propertyChange);
            this.Controls.Add(propertyMatrix);
            propertyMatrix.BringToFront();

            justDoIt = new Button
            {
                Text = "Ручной ввод",
                Size = new Size(100, 30),
                Location = new Point(285, 138)
            };
            justDoIt.Click += new System.EventHandler(justDoItClick);
            this.Controls.Add(justDoIt);
            justDoIt.BringToFront();

            loadFiles = new Button
            {
                Text = "Файловый ввод",
                Size = new Size(100, 30),
                Location = new Point(285, 173)
            };
            loadFiles.Click += new System.EventHandler(loadFilesClick);
            this.Controls.Add(loadFiles);
            loadFiles.BringToFront();

            graphics = new Button
            {
                Text = "Графика",
                Size = new Size(100, 30),
                Location = new Point(175, 260),
                Enabled = false
            };
            graphics.Click += new System.EventHandler(graphicsClick);
            this.Controls.Add(graphics);
            graphics.BringToFront();

            next = new Button
            {
                Text = "Решить",
                Size = new Size(100, 30),
                Location = new Point(285, 260),
                Enabled = false
            };
            next.Click += new System.EventHandler(nextClick);
            this.Controls.Add(next);


            fileResult = new Button
            {
                Text = "Файл с результатом",
                Size = new Size(130, 30),
                Location = new Point(35, 260),
                Enabled = false
            };
            fileResult.Click += new System.EventHandler(fileResultClick);
            this.Controls.Add(fileResult);
            fileResult.BringToFront();

            format = new ComboBox
            {
                Size = new Size(210, 30),
                Location = new System.Drawing.Point(175, 70),
                
            };

            format.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            for (int i = 0; i < matrixTypesList.Length; i++)
                format.Items.Add(matrixTypesList[i]);
            format.SelectedIndexChanged += new System.EventHandler(format_SelectedIndexChanged);
            this.Controls.Add(format);
            format.BringToFront();
            format.SelectedIndex = 0;

            bar = new ProgressBar
            {
                Size = new Size(350, 20),
                Location = new System.Drawing.Point(35, 225)
            };
            this.Controls.Add(bar);
            bar.BringToFront();
        }

        private void format_SelectedIndexChanged(object sender, EventArgs e)
        {
            next.Enabled = false;
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
                if (maxit.Text == "0")
                    maxit.Text = "1";

            }
            catch
            {
                maxit.Text = "1";
            }
        }

        private void threadSolver()
        {
            Factory.Residual.Clear();// очистим вектор для нового решения
            Factory.CreateMatrix(str_format_matrix);
            Factory.Create_Full_Matrix(str_format_matrix, property_matr);
            Factory.CreatePrecond(str_precond);
            Factory.CreateSolver(str_solver);
        }

        private void nextClick(object sender, EventArgs e)
        {
            bar.Value = 0;
            Factory.MaxIter = Convert.ToUInt16(maxit.Text);
            bar.Maximum = Convert.ToUInt16(maxit.Text);
            Factory.Accuracy = Convert.ToDouble("1e-" + acc.Value.ToString());

            str_format_matrix = format.SelectedItem.ToString();
            str_solver = solver.SelectedItem.ToString();
            str_precond = precond.SelectedItem.ToString();

            threadSolver();
            graphics.Enabled = true;
            fileResult.Enabled = true;
        }

        private void fileResultClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("log.txt");
        }

        private void loadFilesClick(object sender, EventArgs e)
        {
            str_format_matrix = format.SelectedItem.ToString();

            FileLoadForm FileLoadForm = new FileLoadForm();
            FileLoadForm.Show();
            format.Enabled = false;
            justDoIt.Enabled = false;
            loadFiles.Enabled = false;
            next.Enabled = false;
            bar.Value = 0;
        }

        private void propertyChange(object sender, EventArgs e)
        {
            property_matr = !property_matr;
            next.Enabled = false;
            if (property_matr == true)
            {
                form.clearMatrix();
            }
        }

        private void justDoItClick(object sender, EventArgs e)
        {
            form.Show();
            format.SelectedIndex = 0;
            format.Enabled = false;
            justDoIt.Enabled = false;
            loadFiles.Enabled = false;
            bar.Value = 0;
        }

        public void graphicsClick(object sender, EventArgs e)
        {
            SharpGL_limbo.SharpGL_Open();
        }

        private void helpToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            infoForm form = new infoForm();
            form.Show();
        }

        private void aboutProgramToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            aboutProgramForm form = new aboutProgramForm();
            form.Show();
        }
    }
}
