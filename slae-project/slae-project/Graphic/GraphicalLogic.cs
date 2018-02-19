﻿using System;
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
        public OpenGLControl openGLControl;
        public Single FontSize = 14.0f;
        public enum FontFormat { G, F, E };
        public FontFormat font_format = FontFormat.G;
        public int FontQuanitityAfterPoint = 3;

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
            mouse.ShiftedPosition.x = Math.Abs(Grid.cursorP.x);
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
            //Пример как работать с этой формой
            //Добавление, удаление и очищение объектов.

        }
        /// <summary>
        /// Попробовать что все работает
        /// </summary>

        /// <summary>
        /// В каком то роде Grid это курсор на консольном окне.
        /// </summary>
        public Net Grid = new Net();
        public MouseClass mouse;

        public bool RealDraw_Try_To_Initialize = true;
        PointInt CurrentCell;
        PointInt xCellArea;
        PointInt yCellArea;
        private const int AreaRadius = 2;
        public bool Belongs_xCellArea()
        {
            int TempX = mouse.ShiftedPosition.x / Grid.xCellSize;
            if (Grid.X_Y_counter.x > TempX - AreaRadius && Grid.X_Y_counter.x < TempX + openGLControl.Width / Grid.xCellSize + AreaRadius)
                return true;
            else return false;
        }
        public bool Belongs_yCellArea()
        {
            int TempY = (mouse.ShiftedPosition.y + openGLControl.Height) / Grid.yCellSize;
            if (Grid.X_Y_counter.y > TempY - openGLControl.Height / Grid.yCellSize - AreaRadius && Grid.X_Y_counter.y < TempY + AreaRadius)
                return true;
            else return false;
        }
        /// <summary>
        /// Главная рисовалка.
        /// </summary>
        /// <param name="openGLControl"></param>
        public void RealDraw()
        {
            RealDraw_Try_To_Initialize = true;

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Load the identity matrix.
            gl.LoadIdentity();

            Grid.initP.y = openGLControl.Height - Grid.yCellSize;

            Grid.DeadPoint.x = 0;
            Grid.Y_nullificate();
            //draw_white_square(0, openGLControl.Height, openGLControl.Width, openGLControl.Height - 40);
            //draw_white_square(0, openGLControl.Height, 40, 0);

            int CentrX = mouse.x - openGLControl.Location.X;
            //int CentrY = openGLControl.Height - mouse.y;
            int CentrY = mouse.y;
            string str = openGLControl.Location.X.ToString();
            string str2 = openGLControl.Location.Y.ToString();
            draw_line(0, CentrY, openGLControl.Width, CentrY, false);
            draw_line(CentrX, 0, CentrX, openGLControl.Height, false);
            //List_Of_Objects.Reverse();
            //Для каждой матрицы в списке объектов
            foreach (var obj in List_Of_Objects)
            {
                //Отдели от предыдущей двумя очень длинными горизонтальными линиями
                if (Belongs_yCellArea())
                {
                    draw_line(0, Grid.cursorP.y,
                                    100000, Grid.cursorP.y);

                    draw_line(0, Grid.cursorP.y + 2,
                                    100000, Grid.cursorP.y + 2);
                }
                Grid.X_move();
                //Напиши как называется текущая матрица
                Draw_Text(Grid.cursorP.x, Grid.cursorP.y, obj.Name, true);
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

                    if (Belongs_yCellArea())
                        Draw_Text(Grid.cursorP.x + 25, Grid.cursorP.y, Count_by_Y.ToString(),true);
                    Count_by_Y++; Grid.X_move();

                        //Пиши его значения в строчку
                        foreach (var value in vect)
                        {
                            if (Belongs_yCellArea()&&Belongs_xCellArea()) Draw_Text(Grid.cursorP.x, Grid.cursorP.y, value.ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()),true);
                            Grid.X_move();
                        }

                    //Рисует горизонтальные линии матрицы
                    if (Belongs_yCellArea())
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
                if (Belongs_yCellArea())
                    draw_line(X_new, Y_new,
                                Grid.cursorP.x + Grid.xCellSize, Y_new);
                Grid.Y_move();
            }

            Grid.DeadPoint.y = -Grid.cursorP.y;
            //Возвращает курсор по Y координатами в саааамое начало.

            //List_Of_Objects.Reverse();
            RealDraw_Try_To_Initialize = false;

        }
        void Draw_Text(int in_x, int in_y, string phrase, bool autoshifted, Single r = 0, Single g = 0, Single b = 0)
        {
            OpenGL gl = openGLControl.OpenGL;

            if (autoshifted)
            {
                in_x -= mouse.ShiftedPosition.x;
                in_y += +mouse.ShiftedPosition.y;
            }
            gl.DrawText(in_x, in_y, r, g, b, "", FontSize, phrase);
        }
        void Draw_Horizontal_numbers_for_matrix(GraphicObject obj)
        {
            OpenGL gl = openGLControl.OpenGL;
            int Count_by_X = 1;
            foreach (var value in obj.Matrix[0])
            {
                if (Belongs_xCellArea())
                    Draw_Text(Grid.cursorP.x, Grid.cursorP.y, Count_by_X.ToString(), true, 0.0f, 0.0f, 0.0f);
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
                if (Belongs_xCellArea())
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
        private void draw_line(int x_from,int y_from = 0, int x_to = 0, int y_to = 0, bool autoshifter = true)
        {
            if (autoshifter)
            {
                x_from -= mouse.ShiftedPosition.x + 3;
                y_from += mouse.ShiftedPosition.y + Grid.yCellSize * 3 / 4;
                x_to -= mouse.ShiftedPosition.x + 3;
                y_to += mouse.ShiftedPosition.y + Grid.yCellSize * 3 / 4;
            }
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
        private void draw_white_square(int x_from, int y_from, int x_to, int y_to)
        {
            //x_from -= mouse.ShiftedPosition.x;
            //y_from += mouse.ShiftedPosition.y;
            //x_to -= mouse.ShiftedPosition.x;
            //y_to += mouse.ShiftedPosition.y;

            //Чтобы не прописывать постоянно
            OpenGL gl = openGLControl.OpenGL;
            //  Clear the color and depth buffer.
            //  Load the identity matrix.
            gl.LoadIdentity();
            gl.Color(0.0f, 0.0f, 0.0f, 1.0f); //Must have, weirdness!
            gl.Begin(OpenGL.GL_QUADS);

            Single Line_Height = -0.4f;

            gl.Vertex(x_to, y_to, Line_Height);
            gl.Vertex(x_to, y_from, Line_Height);
            gl.Vertex(x_from, y_from, Line_Height);
            gl.Vertex(x_from, y_to, Line_Height);
            

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
        public PointInt DeadPoint = new PointInt(0,0);//Самый нижний правый конец поля курсором!
        public PointInt X_Y_counter = new PointInt(0, 0);
        public int xCellSize = 80, yCellSize = 30;

        public Net()
        {
            cursorP = new PointInt(initP.x, initP.y);
        }
        public void X_move()
        {
            cursorP.x += xCellSize;
            X_Y_counter.x++;
        }
        public void X_nullificate()
        {
            cursorP.x = initP.x;
            X_Y_counter.x = 0;
        }

        public void Y_move()
        {
            cursorP.y -= yCellSize;
            X_Y_counter.y++;
        }
        public void Y_nullificate()
        {
            cursorP.y = initP.y;
            X_Y_counter.y = 0;
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

                BorderEndRecalculate();

                if (ShiftedPosition.x - ShiftPosition.x > BorderBegin.x && ShiftedPosition.x - ShiftPosition.x < (BorderEnd.x))
                    ShiftedPosition.x -= ShiftPosition.x;

                if (ShiftedPosition.y - ShiftPosition.y > BorderBegin.y && ShiftedPosition.y - ShiftPosition.y < (BorderEnd.y))
                ShiftedPosition.y -= ShiftPosition.y;
            }
            


            
        }
        public void BorderEndRecalculate()
        {
            BorderEnd.x = +Grid.DeadPoint.x - openGLControl.Width + Grid.xCellSize;
            BorderEnd.y = Grid.DeadPoint.y;
        }
        public PointInt BorderBegin = new PointInt(15, 10);//самый верхний левый угол для ShiftedPosition
        public PointInt BorderEnd = new PointInt(15, 10);//самый нижний правый угол для ShiftedPosition

    }

}

/*
 * ЮзерГайд пользования модулем с точки зрения Form1
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
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("listed_matrix", listed_matrix));
            SharpForm.GD.List_Of_Objects.Add(new GraphicData.GraphicObject("bigdouble", bigdouble));
            //SharpForm.GD.List_Of_Objects.RemoveAt(1); Удалить какойто конкретный
            //SharpForm.GD.List_Of_Objects.Clear(); //Удалить все.
            //SharpForm.GD.List_Of_Objects.RemoveAt(List_Of_Objects.Count() - 1); //Удалить последний

            //ВАЖНО! После добавлений или удалений вызывать вот эту функцию.
            SharpForm.Refresh_Window();
        }
    }
}
     */
