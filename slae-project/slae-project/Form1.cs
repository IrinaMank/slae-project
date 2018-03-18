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
    public partial class Form1 : Form
    {
        public string str_format_matrix; //формат матрицы
        public string str_solver; //тип решателя
        public string str_precond; //тип предобусловлевания
        public static bool property_matr = false; //симметричность матрицы: по умолчанию несимметричная
        public static int Size_Matrix=0;


        public Form1()
        {
            
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            maskedTextBox1_size.Mask = "00000";
            //foreach(string i in factory.GetFormat)
            //{
            //    format_matrix.Items.Add(i);
            //}
            //format_matrix.SelectedIndex = 0;
            //format_matrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            //foreach (string i in factory.GetSolver)
            //{
            //    solver.Items.Add(i);
            //}
            //solver.SelectedIndex = 0;
            //solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            //foreach (string i in factory.GetPrecond)
            //{
            //    precond.Items.Add(i);
            //}
            //precond.SelectedIndex = 0;
            //precond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

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
        }

        private void start_Click(object sender, EventArgs e)
        {
            string s = maskedTextBox1_size.Text;
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

        }
    }
}
