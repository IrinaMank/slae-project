using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using slae_project.Vector;
namespace slae_project
{
    public partial class Form1 : Form
    {
        public static string str_format_matrix; //формат матрицы
        public static string str_solver; //тип решателя
        public static string str_precond; //тип предобусловлевания
        public static bool property_matr = false; //симметричность матрицы: по умолчанию несимметричная
        public static int Size_Matrix=0;
        public static double s_accur_number;//точность решения
        public static int max_iter = 10000;
        public Form1()
        {
            
            InitializeComponent();
            Factory f = new Factory();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            maskedTextBox1_size.Mask = "00000";
            maskedTextBox1_accuracy.Mask = "00e-00";
            property_matrix.Text = "";
            foreach (string i in Factory.MatrixTypes.Keys)
            {
                format_matrix.Items.Add(i);
            }

            format_matrix.SelectedIndex = 0;
            format_matrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            foreach (string i in Factory.SolverTypes.Keys)
            {
                solver.Items.Add(i);
            }
            solver.SelectedIndex = 0;
            solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            foreach (string i in Factory.PrecondTypes)
            {
                precond.Items.Add(i);
            }
            precond.SelectedIndex = 0;
            precond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            property_matrix.Items.Add("Симметричная");
            property_matrix.Items.Add("Несимметричная");
            property_matrix.SelectionMode = SelectionMode.One;
            
        }
        // выбор только одного элемента в checkBox
        private void property_matrix_ItemCheck(object sender, ItemCheckEventArgs e)
        {
           var list = sender as CheckedListBox;
            if (e.NewValue == CheckState.Checked)
                foreach (int index in list.CheckedIndices)
                    if (index != e.Index)
                        list.SetItemChecked(index, false);
            if (list.CheckedIndices.Count == 0) property_matrix.Text = " ";
            else property_matrix.Text = "";

            if (maskedTextBox1_size.Text != "" && maskedTextBox1_accuracy.Text != "" && property_matrix.Text != "") start.Enabled = true;
            else start.Enabled = false;
        }

        private void start_Click(object sender, EventArgs e)
        {
            string s = maskedTextBox1_size.Text;
            string s_accur = maskedTextBox1_accuracy.Text;
            s_accur_number = Convert.ToDouble(s_accur);
           // s_accur_number = Convert.ToInt32(s_accur);
            int number = Convert.ToInt32(s);
            if (s[0] == '0')  MessageBox.Show("Ошибка в размерности матрицы"); 
            else
            {
                str_format_matrix = format_matrix.SelectedItem.ToString();
                str_solver = solver.SelectedItem.ToString();
                str_precond = precond.SelectedItem.ToString();
                property_matr = false;
                if (property_matrix.SelectedIndex == 1)
                    property_matr = true;
                else property_matr = false;
                Size_Matrix = number;
                Form2 formats = new Form2();
                formats.Show();

                button1.Visible = true;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Factory.Create_Full_Matrix(str_format_matrix);
            Factory.CreateSolver(str_solver);
        }


        private void property_matrix_Validated(object sender, EventArgs e)
        {
            if (maskedTextBox1_size.Text != "" && maskedTextBox1_accuracy.Text != "" && property_matrix.Text != "") start.Enabled = true;
            else start.Enabled = false;
        }

        
        private void maskedTextBox1_size_TextAlignChanged(object sender, EventArgs e)
        {
            if (maskedTextBox1_size.Text != "" && maskedTextBox1_accuracy.Text != "" && property_matrix.Text != "") start.Enabled = true;
            else start.Enabled = false;
        }

        private void maskedTextBox1_accuracy_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox1_size.Text != "" && maskedTextBox1_accuracy.Text != "" && property_matrix.Text != "") start.Enabled = true;
            else start.Enabled = false;
        }
    }
}
