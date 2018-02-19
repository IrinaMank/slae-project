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

            //Чтобы не нажимать при отладке на кнопку вызова постоянно, раскоментируй это.
            button1_Graphic.PerformClick();
        }

        public SharpGLForm SharpForm = null;
        private void button1_Graphic_Click(object sender, EventArgs e)
        {
            if (SharpForm != null)
                if (SharpForm.Enabled)
                    SharpForm.Close();
            SharpForm = new SharpGLForm(true);
            SharpForm.Visible = true;

            bool ShowExample = true; ;
            if (ShowExample)
            {
                User_Guide_To_Graphic();
            }
        }
        public void User_Guide_To_Graphic()
        {
            //Примеры добавляемых объектов
            double single_value = 5;

            double[] vector4ik = new double[40]; for (int i = 0; i < 40; i++) vector4ik[i] = i;
            double[,] randomMatrix = new double[,] { { 12345678912345, 2, 3, 4 }, { -12345678912345, 4, 1, 1 }, { 5, 6, 1, 1 } };

            List<double> listed_vectorik = new List<double>() { 1, 2, 3, 4, 5 };
            List<List<double>> listed_matrix = new List<List<double>>() { new List<double> { 1, 2 }, new List<double> { 3, 4 }, new List<double> { 5, 6 } };

            double[,] bigdouble = new double[100,100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++) bigdouble[i, j] = i + j;

            //Добавление объектов на отображение.
            //Имя и Число/Вектор/Матрица в формате (double, double[], double[,], List<double>, List<List<double>>) на выбор.
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("vector4ik", vector4ik));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("single_value", single_value));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("Matrix", randomMatrix));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_vectorik", listed_vectorik));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("bigdouble", bigdouble));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            //SharpForm.GD.List_Of_Objects.RemoveAt(1); Удалить какойто конкретный
            //SharpForm.GD.List_Of_Objects.Clear(); //Удалить все.
            //SharpForm.GD.List_Of_Objects.RemoveAt(List_Of_Objects.Count() - 1); //Удалить последний

            //ВАЖНО! После добавлений или удалений вызывать вот эту функцию.
            SharpForm.Refresh_Window();
        }
    }
}
