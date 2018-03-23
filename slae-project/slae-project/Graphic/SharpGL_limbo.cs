using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;//files
using slae_project.Matrix;//files
using slae_project.Vector;
using slae_project.Properties;//files
using slae_project.Preconditioner;
using slae_project.Solver;
using slae_project.Logger;
namespace slae_project
{
    //Класс взаимодействия с внешним миром.
    public static class SharpGL_limbo
    {
        //SharpGL_limbo SharpGL = new SharpGL_limbo();

        static SharpGLForm SharpForm = null;

        //Сюда добавлять матрицы на отображения. Примеры в функции UserGuide_To_Graphic
        public static List<GraphicData.GraphicObject> List_Of_Objects;

        //Желательно проверять доступность List_of_Objects перед его вызовом.
        public static bool SharpGL_is_opened()
        {
            if (SharpForm != null)
                if (!SharpForm.IsDisposed)
                    return true;
            return false;
        }

        //Обновления окна. Вызывать после добавления или удалений матриц из List_of_objects
        public static void Refresh_Window()
        {
            if (SharpGL_is_opened()) SharpForm.Refresh_Window();
        }

        
        //Конструктор. Параметр самовызова для ленивости.
        /*public static SharpGL_limbo(bool SelfInit = false)
        {
            //if (SelfInit) SharpGL_Open_hidden();


            //Убери восклицательный знак для открытия
            if (SelfInit)
            {
                SharpGL_Open();
                UR.UserGuide_access(ref List_Of_Objects);
                Refresh_Window();
            }

    }*/

        //Образец кнопочки. ,будущей. потом.
        //SharpGL_limbo sharpGL_limbo = new SharpGL_limbo(true); //рядом с кнопочкой
        //sharpGL_limbo.SharpGL_Open(); //в кнопочку

        //Скрывает, если окно существует
        static public void SharpGL_Hide()
        {
            if (SharpGL_is_opened())
            {
                SharpForm.Visible = false;
                List_Of_Objects = SharpForm.GD.List_Of_Objects;
            }
        }
        /// <summary>
        /// Записать матрицу в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="numObject">Номер матрицы в массиве объектов</param>
        static public void WriteMatrix(string path, int numObject)
        {
            SharpForm.WriteMatrix(path, numObject);

        }

        /// <summary>
        /// Считать матрицу из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="numObject">Номер матрицы в массиве объектов</param>
        static public void ReadMatrix(string path, int numObject, bool BoolMessage = true)
        {
            SharpForm.ReadMatrix(path,numObject, BoolMessage);

        }

        //Открывает в скрытом режиме. Можно добавлять матрицы.
        static public void SharpGL_Open_hidden()
        {
            if (!SharpGL_is_opened()) SharpForm = new SharpGLForm(false);
            List_Of_Objects = SharpForm.GD.List_Of_Objects;
        }

        //Открывает сразу видимым. Можно добавлять матрицы.
        static public void SharpGL_Open()
        {
            if (!SharpGL_is_opened()) SharpForm = new SharpGLForm(true);
            List_Of_Objects = SharpForm.GD.List_Of_Objects;
            SharpForm.Show();
        }
        static public void SharpGL_add_Factory_things()
        {
            
        }
        //Запуск в тестовом режиме.
        static public void SharpGL_Open_Test()
        {
            //SharpGL_Open();
            //UR.UserGuide_access(ref List_Of_Objects);
            //Refresh_Window();

            
    }

        //закрывает окно графики если оно существует. Совсем закрыть
        //По умолчанию открыто скрытым.
        static public void SharpGL_Close()
        {
            if (SharpGL_is_opened()) SharpForm.Close();
            SharpForm = null;
        }

        //Сбрасывает все данные
        static public void SharpGL_Reset_Full()
        {
            bool visible_status = true;
            if (SharpGL_is_opened()) 
            {
                visible_status = SharpForm.Visible;
                SharpForm.Close();
            }
            if (visible_status) SharpGL_Open(); else if (visible_status) SharpGL_Open_hidden();
        }
        static public void SharpGL_Reset()
        {
            if (SharpGL_is_opened())
            {
                List_Of_Objects.Clear();
            }
        }

        
        
    }
    public class UserGuide
    {
        //Инструкция к классу SharpGL_limbo, создан в Form1
        protected void User_Guide_To_Graphic(ref List<GraphicData.GraphicObject> List_Of_Objects)
        {
            //SharpGL_Open_hidden() открывает окно графики и 
            //Делает доступным для записи в List_of_Objects

            //SharpGL_Open() открывает окно видимым и дает доступ к записи, показывает если оно существует
            //SharpGL_Open_hidden() открывает окно скрытым и дает доступ к записи
            //SharpGL_Hide() скрывает окно если оно существует
            //SharpGL_Reset_Full() сбрасывает данные через переоткрытие, если окно открыто. не меняет статус видимости
            //SharpGL_Reset() сбрасывает данные введенных матриц если окно открыто
            //SharpGL_Close() закрывает окно и лишает доступности для записи

            //SharpGL_is_opened() Проверка доступности для записи

            //Примеры добавляемых объектов
            double single_value = 5;

            double[] vector4ik = new double[40]; for (int i = 0; i < 40; i++) vector4ik[i] = i;
            double[,] randomMatrix = new double[,] { { 123, 2, 3, 4 }, { -1, 4, 1, 1 }, { 5, 6, 1, 1 } };

            List<double> listed_vectorik = new List<double>() { 1, 2, 3, 4, 5 };
            List<List<double>> listed_matrix = new List<List<double>>() { new List<double> { 1, 2 }, new List<double> { 3, 4 }, new List<double> { 5, 6 } };

            double[,] bigdouble = new double[1000, 1000];
            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++) bigdouble[i, j] = i + j;
            //Добавление объектов на отображение.
            //Имя и Число/Вектор/Матрица в формате (double, double[], double[,], List<double>, List<List<double>>) на выбор.
            List_Of_Objects.Add(new GraphicData.GraphicObject("vector4ik", vector4ik));
            List_Of_Objects.Add(new GraphicData.GraphicObject("single_value", single_value));
            List_Of_Objects.Add(new GraphicData.GraphicObject("listed_vectorik", listed_vectorik));
            List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            SharpGL_limbo.ReadMatrix(ProjectPath + "\\Graphic\\GraphicData_Magneto.txt", List_Of_Objects.Count(),false);
            //List_Of_Objects.Add(new GraphicData.GraphicObject("bigdouble", bigdouble));
            //List_Of_Objects.Add(new GraphicData.GraphicObject("Imatrix", 5,5));
            List_Of_Objects.Add(new GraphicData.GraphicObject("Matrix", randomMatrix));
            //this.List_Of_Objects.RemoveAt(1); Удалить какойто конкретный
            //this.List_Of_Objects.Clear(); //Удалить все.
            //this.List_Of_Objects.RemoveAt(List_Of_Objects.Count() - 1); //Удалить последний

            //ВАЖНО! После добавлений или удалений вызывать вот эту функцию.
            //SharpGL_limbo.Refresh_Window();

            //CoordinateMatrix.localtest();
            (int, int)[] coord = new(int, int)[100];
            //   double[] valMatrix = new double[25] { 1, 5, 1, 2, 1, 8, 2, 1, 3, 2, 2, 9, 3, 7, 3, 1, 3, 10, 4, 6, 3, 1, 2, 11, 5 };	
            // double[] valB = new double[] { 27, 37, 72, 83, 80 };	
            double[] valX = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            double[] valMatrix = new double[100] { 7, 2, 0, 0, 0, 1, 3, 0, 0, 0, 1, 8, 4, 0, 0, 0, 1, 2, 0, 0, 0, 3, 14, 3, 0, 0, 0, 4, 4, 0, 0, 0, 2, 9, 1, 0, 0, 0, 2, 4, 0, 0, 0, 4, 6, 1, 0, 0, 0, 1, 2, 0, 0, 0, 1, 5, 2, 0, 0, 0, 2, 3, 0, 0, 0, 3, 11, 3, 0, 0, 0, 4, 1, 0, 0, 0, 3, 12, 4, 0, 0, 0, 1, 4, 0, 0, 0, 2, 8, 1, 0, 0, 0, 2, 1, 0, 0, 0, 1, 4 };
            double[] valB = new double[] { 38.0000, 52.0000, 128.0000, 105.0000, 62.0000, 51.0000, 127.0000, 164.0000, 117.0000, 62.0000 };


            for (int i = 0; i < 100; i++)
            {
                coord[i] = (i / 10, i % 10);
            }

            IMatrix mar = new CoordinateMatrix(coord, valMatrix);
            //NoPreconditioner preco = new NoPreconditioner();
            LUPreconditioner preco = new LUPreconditioner(mar);
            //DiagonalPreconditioner preco = new DiagonalPreconditioner(mar);	

            IVector b = new SimpleVector(valB);
            IVector x0 = new SimpleVector(10);
            IVector rigthX = new SimpleVector(valX);

            List_Of_Objects.Add(new GraphicData.GraphicObject("Imatrix", ref mar));
            List_Of_Objects.Add(new GraphicData.GraphicObject("Ivector", ref b));
            List_Of_Objects.Add(new GraphicData.GraphicObject("Ivector", ref x0));
            List_Of_Objects.Add(new GraphicData.GraphicObject("Ivector", ref rigthX));
        }
        public string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
    }
}
