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

        public Boolean IsTextEnabled = true;
        public void TextDisable() { IsTextEnabled = false; }
        public void TextEnable() { IsTextEnabled = true; }
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
        public MouseClass mouse = new MouseClass();
        /// <summary>
        /// Главная рисовалка.
        /// </summary>
        /// <param name="openGLControl"></param>
        public void RealDraw(OpenGLControl openGLControl)
        {

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Load the identity matrix.
            gl.LoadIdentity();

            
            Grid.initP.y = openGLControl.Height - Grid.yCellSize;
            foreach (var obj in List_Of_Objects)
            {
                if (IsTextEnabled) gl.DrawText(Grid.cursorP.x + mouse.ShiftedPosition.x, Grid.cursorP.y + mouse.ShiftedPosition.y, 0.0f, 0.0f, 0.0f, "", 14.0f, obj.Name);
                Grid.Y_move();

                foreach (var vect in obj.Matrix)
                {
                    foreach (var value in vect)
                    {
                        if (IsTextEnabled) gl.DrawText(Grid.cursorP.x + mouse.ShiftedPosition.x, Grid.cursorP.y + mouse.ShiftedPosition.y, 0.0f, 0.0f, 0.0f, "", 14.0f, value.ToString());
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
    public class PointInt
    {
        public int x, y;
        public PointInt(int _x, int _y)
        {
            x = _x; y = _y;
        }
        public PointInt(PointInt input)
        {
            x = input.x; y = input.y;
        }
    }

    /// <summary>
    /// По сути курсор на консольном окне.
    /// </summary>
    public class Net
    {
        public PointInt initP = new PointInt(5, 5);
        public PointInt cursorP;
        public int xCellSize = 25, yCellSize = 25;

        public Net()
        {
            cursorP = new PointInt(initP.x, initP.y);
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
    public class MouseClass
    {
        public PointInt LastPosition;
        public PointInt NewPosition;
        public PointInt ShiftPosition;
        public PointInt ShiftedPosition;
        public Boolean isPressed = false;
        public Boolean isPressedBefore = false;
        public MouseClass()
        {
            ShiftPosition = new PointInt(0, 0);
            LastPosition = new PointInt(0, 0);
            ShiftedPosition = new PointInt(0, 0);
        }

        public string mousebuttons;
        public int x, y;
        public void setMouseData(string str, int _x, int _y)
        { mousebuttons = str; x = _x; y = _y; Mouse_movements(); }

        double mouse_decrease = 1;
        public void Mouse_movements()
        {
            //Если мышка ранее не была нажатой, то запомнить последними коордами текущие коорды
            if (isPressedBefore == false && isPressed == true)
            {
                isPressedBefore = true;
                LastPosition.x = x;
                LastPosition.y = y;
            }

            //Посчитать смещение мышки и прибавить к итогу.
            if (isPressed == true)
            {
                ShiftPosition.x = (int)((x - LastPosition.x) / mouse_decrease);
                ShiftPosition.y = (int)((y - LastPosition.y) / mouse_decrease);

                LastPosition.x = x;
                LastPosition.y = y;

                ShiftedPosition.x += ShiftPosition.x;
                ShiftedPosition.y -= ShiftPosition.y;
            }
            
        }
    }

}
