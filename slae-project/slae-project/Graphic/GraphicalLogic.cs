using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;

namespace slae_project
{
    /// <summary>
    /// Данные выводимые на экран представляются последовательно отображенными названиями и матрицами
    /// Где матрица = это Матрица|Вектор|Число.
    /// </summary>
    public class GraphicData
    {
        /// <summary>
        /// Вот пример одного выводимого объекта
        /// У него есть имя. И у него есть матрица.
        /// И конструкторы если дали вектор иль число.
        /// </summary>
        public class GraphicObject
        {
            public string Name;

            public List<List<double>> Matrix = new List<List<double>>();

            public GraphicObject(string _Name, List<List<double>> _Matrix)
            {
                this.Name = _Name; Matrix = _Matrix;
            }
            public GraphicObject(string _Name, List<double> _Vector)
            {
                this.Name = _Name; Matrix.Add(new List<double>(_Vector));
            }
            public GraphicObject(string _Name, double _Value)
            {
                this.Name = _Name; Matrix.Add(new List<double>()); Matrix[0].Add(_Value);
            }
        }

        public List<GraphicObject> List_Of_Objects = new List<GraphicObject>();

        /// <summary>
        /// Попробовать что все работает
        /// </summary>
        public void ItisATest()
        {
            double[] vector4ik = new double[] { 1, 2, 3, 4, 5 };
            List_Of_Objects.Add(new GraphicObject("FirstVector", vector4ik.ToList()));
            List_Of_Objects.Add(new GraphicObject("SecondVector", vector4ik.ToList()));
            List_Of_Objects.Add(new GraphicObject("ThirdVector", 5));
            List_Of_Objects.Add(new GraphicObject("FourthVector", vector4ik.ToList()));
        }

        /// <summary>
        /// В каком то роде Grid это курсор на консольном окне.
        /// </summary>
        Net Grid = new Net();

        /// <summary>
        /// Главная рисовалка.
        /// </summary>
        /// <param name="openGLControl"></param>
        public void RealDraw(OpenGLControl openGLControl)
        {
            OpenGL gl = openGLControl.OpenGL;

            Grid.initP.y = openGLControl.Height - Grid.yCellSize;
            foreach (var obj in List_Of_Objects)
            {
                gl.DrawText((int)Grid.cursorP.x, (int)Grid.cursorP.y, 0.0f, 0.0f, 0.0f, "", 14.0f, obj.Name);
                Grid.Y_move();

                foreach (var vect in obj.Matrix)
                {
                    foreach (var value in vect)
                    {
                        gl.DrawText((int)Grid.cursorP.x, (int)Grid.cursorP.y, 0.0f, 0.0f, 0.0f, "", 14.0f, value.ToString());
                        Grid.X_move();
                    }

                    Grid.X_nullificate(); Grid.Y_move();
                }
            }
            Grid.Y_nullificate();
        }

    }

    /// <summary>
    /// Значит PointF из флоатов есть, а из даблов нету?!
    /// </summary>
    public class PointDouble
    {
        public double x, y;
        public PointDouble(double _x, double _y)
        {
            x = _x; y = _y;
        }
    }

    /// <summary>
    /// По сути курсор на консольном окне.
    /// </summary>
    public class Net
    {
        public PointDouble initP = new PointDouble(5.0, 5.0);
        public PointDouble cursorP;
        public double xCellSize = 25, yCellSize = 25;

        public Net()
        {
            cursorP = new PointDouble(initP.x, initP.y);
        }
        public void X_move()
        {
            cursorP.x += xCellSize;
        }
        public void X_nullificate()
        {
            cursorP.x = initP.x;
        }

        public void Y_move()
        {
            cursorP.y -= yCellSize;
        }
        public void Y_nullificate()
        {
            cursorP.y = initP.y;
        }
    }

}
