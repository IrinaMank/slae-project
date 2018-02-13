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
        public Form1()
        {
            InitializeComponent();
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

    }
}
