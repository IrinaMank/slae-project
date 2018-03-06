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
        public bool property_matr = false; //симметричность матрицы: по умолчанию несимметричная


     
        public Form1()
        {
            
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            format_matrix.Items.Add("Плотный");
            format_matrix.Items.Add("Строчный");
            format_matrix.Items.Add("Строчно - столбцовый");
            format_matrix.Items.Add("Координатный");
            format_matrix.SelectedIndex = 0;
            format_matrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            solver.Items.Add("Метод сопряжённых градиентов");
            solver.Items.Add("Локально-оптимальная схема");
            solver.Items.Add("Метод Якоби");
            solver.Items.Add("Метод Зейделя");
            solver.Items.Add("Метод бисопряжённых градиентов");
            solver.Items.Add("Метод обобщённых минимальных невязок");
            solver.SelectedIndex = 0;
            solver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            precond.Items.Add("Диагональное");
            precond.Items.Add("Методом Зейделя");
            precond.Items.Add("LU-разложение");
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
        }

        private void start_Click(object sender, EventArgs e)
        {
             str_format_matrix = format_matrix.SelectedItem.ToString();
             str_solver = solver.SelectedItem.ToString();
             str_precond = precond.SelectedItem.ToString();
             property_matr = false;
            if (property_matrix.SelectedIndex == 1)
                property_matr = true;
            else property_matr = false;

            Form2 formats = new Form2();
            formats.Show();

            button1.Visible = true;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В основном окне выбираются формат матрицы, решатель и предобусловливание. \nДля начала, выберите формат матрицы, решатель и предобусловливатель. \nПосле чего нужно выбрать тип матрицы (симметричная/несимметричная) и далее нажать кнопку Старт ", "Помощь", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
        "Написать разработчикам?", "Visit", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk
    ) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("http://pm43@mail.ru");
            }
            //MessageBox.Show("BLABLABLA версия 0.1. от 25 марта 2018. Специальное издание для Windows \nНаписать разработчикам: " , "О программе", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }




        //private void linkLabel1_Click(object sender, EventArgs e)
        //{

        //    linkLabel1.Links[0].Visited = true;
        //    string target = linkLabel1.Links[0].LinkData as string;
        //    if (null != target && (target.StartsWith("http") || target.StartsWith("www")))
        //        System.Diagnostics.Process.Start(target);
        //    else
        //        MessageBox.Show("Item clicked: " + target);
        //}
    }
}
