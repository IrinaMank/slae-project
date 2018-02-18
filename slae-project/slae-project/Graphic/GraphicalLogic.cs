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
        OpenGLControl openGLControl;
        public Single FontSize = 14.0f;
        public GraphicData(OpenGLControl openGLController)
        {
            openGLControl = openGLController;
            mouse = new MouseClass(ref Grid,openGLControl);
            Add_objects();
            MoveToEndCursor();
        }
        public void MoveToEndCursor()
        {
            RealDraw();
            mouse.ShiftedPosition.y = -Grid.cursorP.y - Grid.yCellSize;
            mouse.ShiftedPosition.x = Grid.cursorP.x;
        }

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
            public GraphicObject(string _Name, double[,] _Matrix)
            {
                //_Matrix.
                this.Name = _Name; //Matrix = _Matrix;

                for (int i = 0; i < _Matrix.GetLength(0); i++)
                {
                    Matrix.Add(new List<double>(_Matrix.GetLength(1)));
                    for (int j = 0; j < _Matrix.GetLength(1); j++)
                    {
                        Matrix[i].Add(_Matrix[i, j]);
                    }
                    
                }
            }
            public GraphicObject(string _Name, List<double> _Vector)
            {
                this.Name = _Name; Matrix.Add(new List<double>(_Vector));
            }
            public GraphicObject(string _Name, double[] _Vector)
            {
                this.Name = _Name; Matrix.Add(new List<double>(_Vector.ToList()));
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

        public void Add_objects()
        {
            Example();
        }

        /// <summary>
        /// Попробовать что все работает
        /// </summary>
        public void Example()
        {
            //Примеры добавляемых объектов
            double single_value = 5;

            double[] vector4ik = new double[40]; for (int i = 0; i < 40; i++) vector4ik[i] = i;
            double[,] randomMatrix = new double[,] { { 1, 2, 3, 4 },{ 3, 4, 1, 1 },{ 5, 6, 1, 1 } };

            List<double> listed_vectorik = new List<double>() { 1, 2, 3, 4, 5};
            List<List<double>> listed_matrix = new List<List<double>>() { new List<double>{ 1, 2}, new List<double> { 3, 4}, new List<double> { 5, 6} };

            //Добавление объектов на отображение.
            //Имя и Число/Вектор/Матрица в формате (double, double[], double[,], List<double>, List<List<double>>) на выбор.
            List_Of_Objects.Add(new GraphicObject("vector4ik", vector4ik));
            List_Of_Objects.Add(new GraphicObject("single_value", single_value));
            List_Of_Objects.Add(new GraphicObject("Matrix", randomMatrix));
            List_Of_Objects.Add(new GraphicObject("listed_vectorik", listed_vectorik));
            List_Of_Objects.Add(new GraphicObject("listed_matrix", listed_matrix));
            List_Of_Objects.Add(new GraphicObject("listed_matrix", listed_matrix));
            List_Of_Objects.Add(new GraphicObject("listed_matrix", listed_matrix));
            List_Of_Objects.Add(new GraphicObject("listed_matrix", listed_matrix));
            //List_Of_Objects.RemoveAt(1); Удалить какойто конкретный
            //List_Of_Objects.Clear(); //Удалить все.
            //List_Of_Objects.RemoveAt(List_Of_Objects.Count() - 1); //Удалить последний
        }

        /// <summary>
        /// В каком то роде Grid это курсор на консольном окне.
        /// </summary>
        public Net Grid = new Net();
        public MouseClass mouse;
        /// <summary>
        /// Главная рисовалка.
        /// </summary>
        /// <param name="openGLControl"></param>
        public void RealDraw()
        {
            
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Load the identity matrix.
            gl.LoadIdentity();

            Grid.initP.y = openGLControl.Height - Grid.yCellSize;

            Grid.Y_nullificate();
            //List_Of_Objects.Reverse();
            //Для каждой матрицы в списке объектов
            foreach (var obj in List_Of_Objects)
            {
                //Отдели от предыдущей двумя очень длинными горизонтальными линиями
                draw_line(-100000, Grid.cursorP.y,
                                100000, Grid.cursorP.y);

                draw_line(-100000, Grid.cursorP.y + 1,
                                100000, Grid.cursorP.y + 1);

                Grid.X_move();
                //Напиши как называется текущая матрица
                if (IsTextEnabled) gl.DrawText(Grid.cursorP.x + mouse.ShiftedPosition.x, Grid.cursorP.y + mouse.ShiftedPosition.y, 0.0f, 0.0f, 0.0f, "", FontSize, obj.Name);
                Grid.Y_move(); Grid.X_nullificate();

                Grid.X_move();
                int Count_by_Y = 1;
                Draw_Horizontal_numbers_for_matrix(obj);
                Grid.Y_move();

                int X_start = Grid.cursorP.x;
                int Y_start = Grid.cursorP.y;

                int X_old = Grid.cursorP.x;
                int Y_old = Grid.cursorP.y;

                int X_new = Grid.cursorP.x;
                int Y_new = Grid.cursorP.y;

                
                //Для каждого вектора текущей матрицы
                foreach (var vect in obj.Matrix)
                {

                    X_old = Grid.cursorP.x;
                    Y_old = Grid.cursorP.y;

                    if (IsTextEnabled) gl.DrawText(Grid.cursorP.x + mouse.ShiftedPosition.x, Grid.cursorP.y + mouse.ShiftedPosition.y, 0.0f, 0.0f, 0.0f, "", FontSize, Count_by_Y.ToString());
                    Count_by_Y++; Grid.X_move();

                    //Пиши его значения в строчку
                    foreach (var value in vect)
                    {
                        if (IsTextEnabled) gl.DrawText(Grid.cursorP.x + mouse.ShiftedPosition.x, Grid.cursorP.y + mouse.ShiftedPosition.y, 0.0f, 0.0f, 0.0f, "", FontSize, value.ToString());
                        Grid.X_move();
                    }
                    
                    //Рисует горизонтальные линии матрицы
                    draw_line(X_old + Grid.xCellSize, Y_old,
                                Grid.cursorP.x, Grid.cursorP.y);

                    Grid.Y_move();
                    X_new = Grid.cursorP.x;
                    Y_new = Grid.cursorP.y;

                    if (Grid.cursorP.x > Grid.DeadPoint.x) Grid.DeadPoint.x = Grid.cursorP.x;
                    //Верни курсор в начало строки.
                    Grid.X_nullificate();
                }
                //Рисует вертикальные линии матрицы
                Draw_Vertical_net_for_matrix(obj, Y_start);

                //Рисует последнюю горизонтальную линию матрицы
                draw_line(X_new, Y_new,
                                Grid.cursorP.x + Grid.xCellSize, Y_new);
                Grid.Y_move();
            }

            if (Grid.cursorP.y < -Grid.DeadPoint.y) Grid.DeadPoint.y = -Grid.cursorP.y;
            //Возвращает курсор по Y координатами в саааамое начало.

            //List_Of_Objects.Reverse();

        }
        void Draw_Horizontal_numbers_for_matrix(GraphicObject obj)
        {
            OpenGL gl = openGLControl.OpenGL;
            int Count_by_X = 1;
            foreach (var value in obj.Matrix[0])
            {
                gl.DrawText(Grid.cursorP.x + mouse.ShiftedPosition.x, Grid.cursorP.y + mouse.ShiftedPosition.y, 0.0f, 0.0f, 0.0f, "", FontSize, Count_by_X.ToString());
                Grid.X_move();
                Count_by_X++;
            }

            Grid.X_move();
            Grid.X_nullificate();
        }
        void Draw_Vertical_net_for_matrix(GraphicObject obj, int Y_start)
        {
            Grid.X_move();
            foreach (var value in obj.Matrix[0])
            {
                draw_line(Grid.cursorP.x, Y_start,
                            Grid.cursorP.x, Grid.cursorP.y);
                Grid.X_move();
            }
            draw_line(Grid.cursorP.x, Y_start,
                            Grid.cursorP.x, Grid.cursorP.y);
            Grid.X_move();
            Grid.X_nullificate();
        }
        /// <summary>
        /// Draw Grid
        /// </summary>
        private void draw_line(int x_from,int y_from = 0, int x_to = 0, int y_to = 0)
        {
            x_from += mouse.ShiftedPosition.x - 3;
            y_from += mouse.ShiftedPosition.y + Grid.yCellSize * 3 / 4;
            x_to += mouse.ShiftedPosition.x - 3;
            y_to += mouse.ShiftedPosition.y + Grid.yCellSize * 3 / 4;

            //Чтобы не прописывать постоянно
            OpenGL gl = openGLControl.OpenGL;
            //  Clear the color and depth buffer.
            //  Load the identity matrix.
            gl.LoadIdentity();
            gl.Color(0.0f, 0.0f, 0.0f, 1.0f); //Must have, weirdness!
            gl.LineWidth(1.0f);
            gl.Begin(OpenGL.GL_LINES);

            Single Line_Height = 0.5f;

            //gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(x_from, y_from, Line_Height);
            //gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(x_to, y_to, Line_Height);
            gl.End();
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
        public PointInt DeadPoint = new PointInt(0,0);//Самый правый нижний край поля.
        public int xCellSize = 30, yCellSize = 30;

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

        private Net Grid;
        private OpenGLControl openGLControl;
        public MouseClass(ref Net Grider, OpenGLControl openGLcontroller)
        {
            openGLControl = openGLcontroller;
            Grid = Grider;
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

                if (ShiftedPosition.x + ShiftPosition.x < 15 && ShiftedPosition.x + ShiftPosition.x > -Grid.DeadPoint.x + openGLControl.Width - Grid.xCellSize)
                    ShiftedPosition.x += ShiftPosition.x;

                if (ShiftedPosition.y - ShiftPosition.y > 10 && ShiftedPosition.y - ShiftPosition.y < Grid.DeadPoint.y)
                ShiftedPosition.y -= ShiftPosition.y;
            }
            
        }
    }

}
