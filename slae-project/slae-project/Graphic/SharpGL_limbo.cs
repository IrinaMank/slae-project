using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project
{
    //Класс взаимодействия с внешним миром.
    public class SharpGL_limbo
    {
        //SharpGL_limbo SharpGL = new SharpGL_limbo();

        private SharpGLForm SharpForm = null;

        public List<GraphicData.GraphicObject> List_Of_Objects;
        public bool List_of_Objects_is_Available()
        {
            if (SharpForm != null)
                if (SharpForm.Enabled)
                    return true;
            return false;
        }
        public void Refresh_Window()
        {
            SharpForm.Refresh_Window();
        }
        public SharpGL_limbo(bool SelfCallingThingWhenFalse = false)
        {
            //Убрать эту строку когда появится наша кнопочка. А можно и оставить.
            if (SelfCallingThingWhenFalse == false) SharpGLCallTheWindow_for_The_Button();
        }

        //Образец кнопочки. ,будущей. потом.
        //SharpGL_limbo sharpGL_limbo = new SharpGL_limbo(true); //рядом с кнопочкой
        //sharpGL_limbo.SharpGLCallTheWindow_for_Button(); //в кнопочку

        public void SharpGLCallTheWindow_for_The_Button()
        {
            if (SharpForm != null)
                if (SharpForm.Enabled)
                    SharpForm.Close();
            SharpForm = new SharpGLForm(true);
            SharpForm.Visible = true;

            this.List_Of_Objects = SharpForm.GD.List_Of_Objects;

            if (ShowExample)
            {
                User_Guide_To_Graphic();
            }
        }
        private bool ShowExample = true;
        private void User_Guide_To_Graphic()
        {
            //Примеры добавляемых объектов
            double single_value = 5;

            double[] vector4ik = new double[40]; for (int i = 0; i < 40; i++) vector4ik[i] = i;
            double[,] randomMatrix = new double[,] { { 123, 2, 3, 4 }, { -12345678912345, 4, 1, 1 }, { 5, 6, 1, 1 } };

            List<double> listed_vectorik = new List<double>() { 1, 2, 3, 4, 5 };
            List<List<double>> listed_matrix = new List<List<double>>() { new List<double> { 1, 2 }, new List<double> { 3, 4 }, new List<double> { 5, 6 } };

            double[,] bigdouble = new double[100, 100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++) bigdouble[i, j] = i + j;
            //Добавление объектов на отображение.
            //Имя и Число/Вектор/Матрица в формате (double, double[], double[,], List<double>, List<List<double>>) на выбор.
            this.List_Of_Objects.Add(new GraphicData.GraphicObject("vector4ik", vector4ik));
            this.List_Of_Objects.Add(new GraphicData.GraphicObject("single_value", single_value));
            this.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_vectorik", listed_vectorik));
            this.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            this.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            this.List_Of_Objects.Add(new GraphicData.GraphicObject("bigdouble", bigdouble));
            this.List_Of_Objects.Add(new GraphicData.GraphicObject("Matrix", randomMatrix));
            //SharpForm.GD.List_Of_Objects.RemoveAt(1); Удалить какойто конкретный
            //SharpForm.GD.List_Of_Objects.Clear(); //Удалить все.
            //SharpForm.GD.List_Of_Objects.RemoveAt(List_Of_Objects.Count() - 1); //Удалить последний

            //ВАЖНО! После добавлений или удалений вызывать вот эту функцию.
            this.Refresh_Window();
        }
    }
}
