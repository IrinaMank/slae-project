using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using System.IO;
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



        private void WriteMatrix(string path)
        {
            using (StreamWriter reader = new StreamWriter(path, false, System.Text.Encoding.Default)) ;
            {


            }

        }

        private void ReadMatrix(string path)
        {
            using (StreamReader reader = new StreamReader(path, System.Text.Encoding.Default)) ;
            {
                

            }

        }


        private void WriteSettings(string path)
        {
            using (StreamWriter reader = new StreamWriter(path, false, System.Text.Encoding.Default)) ;
            {


            }

        }

        private void ReadSettings(string path)
        {
            using (StreamReader reader = new StreamReader(path, System.Text.Encoding.Default));
            {


            }

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
        private bool Belongs_xCellArea()
        {
            int TempX = mouse.ShiftedPosition.x / Grid.xCellSize;
            if (Grid.X_Y_counter.x > TempX - AreaRadius && Grid.X_Y_counter.x < TempX + openGLControl.Width / Grid.xCellSize + AreaRadius)
                return true;
            else return false;
        }
        private bool Belongs_yCellArea()
        {
            int TempY = (mouse.ShiftedPosition.y + openGLControl.Height) / Grid.yCellSize;
            if (Grid.X_Y_counter.y > TempY - openGLControl.Height / Grid.yCellSize - AreaRadius && Grid.X_Y_counter.y < TempY + AreaRadius)
                return true;
            else return false;
        }

        public bool TargetPlus = true;
        public bool TargetNumber = true;
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

            //Целеуказатель плюсиком зеленый
            if (TargetPlus)
            {
                draw_line(0, mouse.true_y, openGLControl.Width, mouse.true_y, false, 0, 1, 0);
                draw_line(mouse.true_x, 0, mouse.true_x, openGLControl.Height, false, 0, 1, 0);
            }
            //Draw_Text(mouse.true_x, mouse.true_y, obj.Name, false);
            //


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


                if (Belongs_yCellArea()) Draw_Horizontal_numbers_for_matrix(obj);
                Grid.Y_move();

                int X_start = Grid.cursorP.x;
                int Y_start = Grid.cursorP.y;

                int X_old = Grid.cursorP.x;
                int Y_old = Grid.cursorP.y;

                int X_new = Grid.cursorP.x;
                int Y_new = Grid.cursorP.y;


                int Target_Y_value = mouse.ShiftedPosition.y + Grid.yCellSize / 4 - mouse.true_y;
                int Target_Y_radius = Grid.yCellSize / 2;

                //int Target_X_value = mouse.ShiftedPosition.x + Grid.xCellSize / 2 - 12 - mouse.true_x;
                //int Target_X_radius = Grid.xCellSize / 2;

                if (TargetNumber)
                Draw_Text(mouse.true_x + 20, mouse.true_y - 20, "| " + (((int)(mouse.ShiftedPosition.x + mouse.true_x) / Grid.xCellSize)).ToString(), false);
                //Для каждого вектора текущей матрицы
                foreach (var vect in obj.Matrix)
                {

                    X_old = Grid.cursorP.x;
                    Y_old = Grid.cursorP.y;

                    if (Belongs_yCellArea())
                    {
                        Draw_Text(Grid.cursorP.x + 25, Grid.cursorP.y, Count_by_Y.ToString(), true);

                        if (TargetNumber)
                        if (Math.Abs(Grid.cursorP.y + Target_Y_value) < Target_Y_radius)
                        Draw_Text(mouse.true_x + 20, mouse.true_y, "- " + Count_by_Y.ToString(), false);
                    }
                    Count_by_Y++; Grid.X_move();

                    //Пиши его значения в строчку
                    foreach (var value in vect)
                    {
                        if (Belongs_yCellArea())
                            if (Belongs_xCellArea())
                            {
                                Draw_Text(Grid.cursorP.x, Grid.cursorP.y, value.ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()), true);
                            }
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
                {
                    Draw_Text(Grid.cursorP.x, Grid.cursorP.y, Count_by_X.ToString(), true, 0.0f, 0.0f, 0.0f);
                }

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
        private void draw_line(int x_from,int y_from = 0, int x_to = 0, int y_to = 0, bool autoshifter = true, Single r = 0, Single g = 0, Single b = 0)
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
            gl.Color(r, g, b, 1.0f); //Must have, weirdness!
            gl.LineWidth(1.0f);
            gl.Begin(OpenGL.GL_LINES);

            Single Line_Height = 0.5f;

            //gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(x_from, y_from, Line_Height);
            //gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(x_to, y_to, Line_Height);
            gl.End();
        }
        /*private void draw_white_square(int x_from, int y_from, int x_to, int y_to)
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
        }*/
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
        public int true_x = 1, true_y = 1;
        public void setMouseData(string str, int _x, int _y, int _true_x, int _true_y)
        { mousebuttons = str; x = _x; y = _y; Mouse_movements(); true_x = _true_x; true_y = _true_y; }

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