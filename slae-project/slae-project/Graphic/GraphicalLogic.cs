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
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;
using slae_project.Matrix;//files
using slae_project.Vector;
using slae_project.Properties;//files
using slae_project.Preconditioner;
using slae_project.Solver;
using slae_project.Logger;
namespace slae_project
{
    /// <summary>
    /// Данные выводимые на экран представляются последовательно отображенными названиями и матрицами
    /// Где матрица = это Матрица|Вектор|Число.
    /// </summary>
    public class GraphicData
    {
        public OpenGLControl openGLControl;
        public SharpGLForm sharpGLform;
        public Single FontSize = 14.0f;
        public enum FontFormat { G, F, E };
        public FontFormat font_format = FontFormat.G;
        public int FontQuanitityAfterPoint = 3;

    public GraphicData(OpenGLControl openGLController, SharpGLForm sharpGLformer)
        {
            openGLControl = openGLController;
            sharpGLform = sharpGLformer;
            Grid= new Net(this);
            mouse = new MouseClass(ref Grid,openGLControl);
            Add_objects();
            MoveToEndCursor();
        }
        public void MoveToEndCursor()
        {
            RealDraw();
            //mouse.ShiftedPosition.y = -Grid.cursorP.y - Grid.yCellSize;
            //mouse.ShiftedPosition.x = Math.Abs(Grid.cursorP.x);
            int temp = mouse.BorderEnd.y;
        }


        

        /// <summary>
        /// Вот пример одного выводимого объекта
        /// У него есть имя. И у него есть матрица.
        /// И конструкторы если дали вектор иль число.
        /// </summary>
        public class GraphicObject
        {
            public string Name;

            public List<string> FilesString = null;
            public GraphicObject(string _Name, string _FileName)
            {
                xCellCount = 1;
                yCellCount = 1;
                try
                {
                    using (FileStream stream = File.Open(_FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            while (!reader.EndOfStream)
                            {
                                string str = "123";
                                FilesString = new List<string>();
                                while ((str = reader.ReadLine()) != null) FilesString.Add(str);
                            }
                        }
                    }
                    
                }
                catch (Exception Exc)
                {
                }
            }
            public double max = double.MinValue;
            public double min = double.MaxValue;
            public double range = double.MaxValue;
            public List<double> GraphicalVector = null;
            public GraphicObject(string _Name, List<double> _GraphicalVector, bool NothingToWorryAbout)
            {
                GraphicalVector = _GraphicalVector;
                xCellCount = GraphicalVector.Count() - 1;
                yCellCount = 20 + 1;
                foreach (var value in GraphicalVector)
                {
                    if (value > max) max = value;
                    if (value < min) min = value;
                }
                range = max - min;
            }
            public List<List<double>> Matrix = new List<List<double>>();
            public IMatrix ReferencedMatrix = null;
            public IVector ReferencedVector = null;
            public int xCellCount = 0, yCellCount = 0;
            public GraphicObject(string _Name, ref IMatrix _ReferencedMatrix)
            {
                //for Imatrix.
                this.Name = _Name;

                ReferencedMatrix = _ReferencedMatrix;
                ReferencedVector = null;

                if (ReferencedMatrix != null)
                {
                    xCellCount = ReferencedMatrix.Size;
                    yCellCount = ReferencedMatrix.Size;
                }
                else
                {
                    xCellCount = 0;
                    yCellCount = 0;
                    this.Name = "";
                }
            }
            public GraphicObject(string _Name, ref IVector _ReferencedVector)
            {
                //for Imatrix.
                this.Name = _Name;

                ReferencedMatrix = null;
                ReferencedVector = _ReferencedVector;

                if (ReferencedVector != null)
                {
                    xCellCount = ReferencedVector.Size;
                    yCellCount = 1;
                }
                else
                {
                    xCellCount = 0;
                    yCellCount = 0;
                    this.Name = "";
                }
            }
            public GraphicObject(string _Name, List<List<double>> _Matrix)
            {
                this.Name = _Name; Matrix = _Matrix;
                xCellCount = Matrix[0].Count();
                yCellCount = Matrix.Count();
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
                xCellCount = Matrix[0].Count();
                yCellCount = Matrix.Count();
            }
            public GraphicObject(string _Name, List<double> _Vector)
            {
                this.Name = _Name; Matrix.Add(new List<double>(_Vector));
                xCellCount = Matrix[0].Count();
                yCellCount = Matrix.Count();
            }
            public GraphicObject(string _Name, double[] _Vector)
            {
                this.Name = _Name; Matrix.Add(new List<double>(_Vector.ToList()));
                xCellCount = Matrix[0].Count();
                yCellCount = Matrix.Count();
            }
            public GraphicObject(string _Name, double _Value)
            {
                this.Name = _Name; Matrix.Add(new List<double>()); Matrix[0].Add(_Value);
                xCellCount = Matrix[0].Count();
                yCellCount = Matrix.Count();
            }

            public double this[int column, int row]
            {
                get
                {
                    try
                    {
                        if (ReferencedMatrix != null) return ReferencedMatrix[row - 1,column - 1];
                        else if (ReferencedVector != null && row == 1) return ReferencedVector[column - 1];
                        else return Matrix[row - 1][column - 1];
                    }
                    catch (Exception ex)
                    {
                        return double.NaN;
                    }
                }

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
        public Net Grid;
        public MouseClass mouse;

        public bool RealDraw_Try_To_Initialize = true;
        PointInt CurrentCell;
        PointInt xCellArea;
        PointInt yCellArea;

        private const int AreaRadius = 3;

        //Если вести счёт ячеек в обычных цифрах(как X_Y_counter), данная функция выдает
        //По обоим осям рабочую зону, которую надо отобразить на экран
        private void AreaCalculator(ref int X_high, ref int X_low, ref int Y_high, ref int Y_low)
        {
            int TempX = mouse.ShiftedPosition.x / Grid.xCellSize;
            X_high = TempX + openGLControl.Width / Grid.xCellSize + AreaRadius;
            X_low = TempX - AreaRadius;

            int TempY = (mouse.ShiftedPosition.y + openGLControl.Height) / Grid.yCellSize;
            Y_high = TempY + AreaRadius;
            Y_low = TempY - openGLControl.Height / Grid.yCellSize - AreaRadius;

            if (X_low < 0) X_low = 0;
            if (X_high < 0) X_high = 0;
            if (Y_low < 0) Y_low = 0;
            if (Y_high < 0) Y_high = 0;

            if (Grid.NetWorkOS_Y.Count() < Y_high) Y_high = Grid.NetWorkOS_Y.Count() - 1;
        }
        public List<Point> LeftTopCellOfEachMatrix = new List<Point>();
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

        public void DrawingInitializer()
        {
            LeftTopCellOfEachMatrix.Clear();

            foreach (var item in Grid.NetWorkOS_X) item.List_of_func.Clear();
            Grid.NetWorkOS_X.Clear();
            
            //Grid = new Net(this);
        }
        ComputerInfo Comp = new ComputerInfo();
        bool MemoryChecker()
        {
            if (Comp.AvailablePhysicalMemory / 1048576 < 100)
            {
                sharpGLform.Clearer();
                MessageBox.Show("Извините, но у вас закончилась оперативная память. Порог 100мб достигнут.", "Ошибка"); 
                return true;
            }
            return false;
        }
        public bool TargetPlus = true;
        public bool TargetNumber = true;
        /// <summary>
        /// Главная рисовалка.
        /// </summary>
        /// <param name="openGLControl"></param>
        public void RealDraw()
        {
            Grid.initP.y = openGLControl.Height - Grid.yCellSize;

            if (MemoryChecker()) return;
            if (RealDraw_Try_To_Initialize)
            {
                DrawingInitializer();

                RealDraw_Try_To_Initialize = false;

                

                Grid.DeadPoint.x = 0;
                Grid.Y_nullificate();

                int Matrix_Counter = 0;
                
                //List_Of_Objects.Reverse();
                //Для каждой матрицы в списке объектов
                foreach (var obj in List_Of_Objects)
                {
                    //Отдели от предыдущей двумя очень длинными горизонтальными линиями
                    //if (Belongs_yCellArea()) {
                    Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawLine, "", 0, Grid.X_Y_counter.y, 1000, Grid.X_Y_counter.y));
                    Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawLine, "", 0, Grid.X_Y_counter.y, 1000, Grid.X_Y_counter.y));
                    //}

                    //draw_line(0, Grid.cursorP.y,100000, Grid.cursorP.y);
                    //draw_line(0, Grid.cursorP.y + 2,100000, Grid.cursorP.y + 2);
                    
                    Grid.X_move();

                    //Напиши как называется текущая матрица
                    Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawText, "#" + (Matrix_Counter).ToString() + " - " + obj.Name, Grid.X_Y_counter.x, Grid.X_Y_counter.y));
                    Matrix_Counter++;
                    //Draw_Text(Grid.cursorP.x, Grid.cursorP.y, "#" + Matrix_Counter.ToString() + " - " + obj.Name); 
                    Grid.Y_move(); Grid.X_nullificate();

                    Grid.X_move();
                    int Count_by_Y = 0;

                    //Запомнить левый верхний уголо матрицы для определения текущей строке и столбца.
                    //Мысля. Достаточно будет найти число с игреком чуть меньшим текущего
                    //И значит эту ячейку использовать для вычисления текущих координат. И выйти 1,1
                    LeftTopCellOfEachMatrix.Add(new Point(Grid.X_Y_counter.x, Grid.X_Y_counter.y));

                    //if (Belongs_yCellArea()) 
                    Draw_Horizontal_numbers_for_matrix(obj);
                    Grid.Y_move();

                    int X_start = Grid.X_Y_counter.x;
                    int Y_start = Grid.X_Y_counter.y;

                    int X_old = Grid.X_Y_counter.x;
                    int Y_old = Grid.X_Y_counter.y;

                    int X_new = Grid.X_Y_counter.x;
                    int Y_new = Grid.X_Y_counter.y;

                    /* Change location for it
                     * if (TargetNumber)
                        Draw_Text(mouse.true_x + 20, mouse.true_y - 20, "| " + (((int)(mouse.ShiftedPosition.x + mouse.true_x) / Grid.xCellSize)).ToString(),0,0,0);*/

                    //if (obj.GraphicalVector == null)
                        for (int i = 0; i < obj.yCellCount; i++)
                        {
                            Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawText, Count_by_Y.ToString(), Grid.X_Y_counter.x, Grid.X_Y_counter.y));
                            Grid.Y_move();
                            Count_by_Y++;
                        }
                    //else
                    //    for (int i = 0; i < obj.yCellCount; i++)
                    //    {
                    //        Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawText, (obj.min + (double)obj.range*((double)((double)obj.yCellCount - Count_by_Y - 0.5)/ obj.yCellCount)).ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()), Grid.X_Y_counter.x, Grid.X_Y_counter.y));
                    //        Grid.Y_move();
                    //        Count_by_Y++;
                    //    }
                    Grid.X_Y_counter.x = X_new;
                    Grid.X_Y_counter.y = Y_new;

                    Count_by_Y = 0;
                    //Для каждого вектора текущей матрицы
                    foreach (var vect in obj.Matrix)
                    {
                        if (MemoryChecker()) return;
                        X_old = Grid.X_Y_counter.x;
                        Y_old = Grid.X_Y_counter.y;

                        //Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawText, Count_by_Y.ToString(), Grid.X_Y_counter.x, Grid.X_Y_counter.y));

                        Count_by_Y++; Grid.X_move();

                        //Пиши его значения в строчку
                        foreach (var value in vect)
                        {
                            if (Grid.X_Y_counter.x % 1000 == 0) if (MemoryChecker()) return;

                            Grid.NetWorkValue[Grid.X_Y_counter.y][Grid.X_Y_counter.x] = value;

                            MaxIdentifiyer(value);

                            Grid.X_move();
                        }

                        //Рисует горизонтальные линии матрицы

                              

                        Grid.Y_move();
                        X_new = Grid.X_Y_counter.x;
                        Y_new = Grid.X_Y_counter.y;

                        //Верни курсор в начало строки.
                        Grid.X_nullificate();
                    }
                    if (obj.ReferencedMatrix != null)
                    {
                        foreach (var item in obj.ReferencedMatrix)
                        {
                            MaxIdentifiyer(item.value); //Grid.X_move();
                        }
                        //for (int i = 0; i < obj.yCellCount; i++) Grid.Y_move();
                    }
                    else if (obj.ReferencedVector != null)
                    {
                        foreach (var item in obj.ReferencedVector)
                        {
                            MaxIdentifiyer(item.value); //Grid.X_move();
                        }
                        //for (int i = 0; i < obj.yCellCount; i++) Grid.Y_move();
                    }
                    else if (obj.GraphicalVector != null)
                    {
                        foreach (var item in obj.GraphicalVector)
                        {
                            MaxIdentifiyer(item); //Grid.X_move();
                        }
                        // for (int i = 0; i < obj.yCellCount; i++) Grid.Y_move();
                    }
                    else if (obj.FilesString != null)
                    {
                        foreach (var str in obj.FilesString)
                        {
                            Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawText, str, Grid.X_Y_counter.x, Grid.X_Y_counter.y));
                            Grid.Y_move();
                        }
                    }
                    //Рисует вертикальные линии матрицы
                    Draw_line_net_for_matrix(obj, Y_start);

                    Grid.Y_move();

                    int NewY = Y_start + obj.yCellCount + 1;
                    //Grid.X_Y_counter.y = NewY;
                    while (Grid.X_Y_counter.y < NewY) Grid.Y_move();
                }
                
                        //Возвращает курсор по Y координатами в саааамое начало.
                        //sharpGLform.SetScrollBars();
                        sharpGLform.SetScrollBars_to_the_end();
            }
            else PartialDrawer();

        }
        void MaxIdentifiyer(double value)
        {
            int TempMaxWidth = value.ToString().Length;
            if (TempMaxWidth > ViktorsMaxWidth) ViktorsMaxWidth = TempMaxWidth;

            if (value < double_min) double_min = value;
            if (value > double_max) double_max = value;
        }
        public int ViktorsMaxWidth = 0;
        private void PartialDrawer()
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;
            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //  Load the identity matrix.
            gl.LoadIdentity();

            int OS_x_begin = 0, OS_x_end = 0, OS_y_begin = 0, OS_y_end = 0;
            AreaCalculator(ref OS_x_end, ref OS_x_begin, ref OS_y_end, ref OS_y_begin);

            if (Grid.NetWorkOS_X.Count != 0 && true)
                for (int x = OS_x_begin; (x < OS_x_end)&&(x < Grid.NetWorkOS_X.Count()); x++)
                {
                    if (x > 0 && x < Grid.NetWorkOS_X.Count())
                        foreach (var func in Grid.NetWorkOS_X[x].List_of_func)
                        if (func.func_type == Net.FunctionType.DrawLine)
                        {
                            if (BoolLinesAreEnabled) draw_line(cursor_X(func.value1), cursor_Y(func.value2), cursor_X(func.value3), cursor_Y(func.value4));
                        }
                        else if (func.func_type == Net.FunctionType.DrawText)
                            Draw_Text(cursor_X(func.value1), cursor_Y(func.value2), func.str);
                }

            for (int y = OS_y_begin; y < OS_y_end; y++)
            {
                if (y > 0 && y < Grid.NetWorkOS_Y.Count())
                foreach (var func in Grid.NetWorkOS_Y[y].List_of_func)
                    if (func.func_type == Net.FunctionType.DrawLine)
                    {
                        if (BoolLinesAreEnabled) draw_line(cursor_X(func.value1), cursor_Y(func.value2), cursor_X(func.value3), cursor_Y(func.value4));
                    }
                    else if (func.func_type == Net.FunctionType.DrawText)
                        Draw_Text(cursor_X(func.value1), cursor_Y(func.value2), func.str);

                //if (y >= 0 && y < Grid.NetWorkValue.Count())
                //Draw_Text(Grid.NetWorkValue[y][0].CellCursorP.X+20, Grid.NetWorkValue[y][0].CellCursorP.Y, y.ToString());
                for (int x = OS_x_begin; x < OS_x_end; x++)
                {
                    if (y < Grid.NetWorkValue.Count())
                    if (x < Grid.NetWorkValue[y].Count())
                        if (!double.IsNaN(Grid.NetWorkValue[y][x]))
                        {
                            if (BoolTextIsEnabledOtherwiseQuads) Draw_Text(cursor_X(x), cursor_Y(y), Grid.NetWorkValue[y][x].ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()));
                            else draw_white_square(cursor_X(x), cursor_Y(y), Grid.NetWorkValue[y][x]);
                        }
                }
            }

            //Вставить сюда.
            
            for (int i = 0; i < List_Of_Objects.Count(); i++)
            {
                var GraphicalObject = List_Of_Objects[i];

                if (GraphicalObject.ReferencedMatrix != null)
                {
                    foreach (var item in GraphicalObject.ReferencedMatrix)
                    {
                        int x = LeftTopCellOfEachMatrix[i].X + item.col;
                        int y = LeftTopCellOfEachMatrix[i].Y + item.row + 1;
                        if (x > OS_x_begin && x < OS_x_end && y > OS_y_begin && y < OS_y_end)
                        {
                            if (BoolTextIsEnabledOtherwiseQuads) Draw_Text(cursor_X(x), cursor_Y(y), item.value.ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()));
                            else draw_white_square(cursor_X(x), cursor_Y(y), item.value);
                        }
                    }
                            
                }
                else if (GraphicalObject.ReferencedVector != null)
                {
                    foreach (var item in GraphicalObject.ReferencedVector)
                    {
                        int x = LeftTopCellOfEachMatrix[i].X + item.index;
                        int y = LeftTopCellOfEachMatrix[i].Y + 1;
                        if (x > OS_x_begin && x < OS_x_end && y > OS_y_begin && y < OS_y_end)
                        {
                            if (BoolTextIsEnabledOtherwiseQuads) Draw_Text(cursor_X(x), cursor_Y(y), item.value.ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()));
                            else draw_white_square(cursor_X(x), cursor_Y(y), item.value);
                        }
                    }
                }

                else if (GraphicalObject.GraphicalVector != null)
                {
                    for (int X_counter = OS_x_begin; X_counter < OS_x_end; X_counter++)
                    {
                        int x = LeftTopCellOfEachMatrix[i].X + X_counter;
                        int y = LeftTopCellOfEachMatrix[i].Y + (GraphicalObject.yCellCount);
                        if (x > OS_x_begin && x < OS_x_end)
                        if (X_counter >= 0 && X_counter < GraphicalObject.GraphicalVector.Count()-1)
                        {
                            int Y0 = (int)((double)cursor_Y(y) + ((double)(GraphicalObject.GraphicalVector[X_counter] - GraphicalObject.min) * Grid.yCellSize * (GraphicalObject.yCellCount - 1) / GraphicalObject.range));
                            int Y1 = (int)((double)cursor_Y(y) + ((double)(GraphicalObject.GraphicalVector[X_counter + 1] - GraphicalObject.min) * Grid.yCellSize * (GraphicalObject.yCellCount - 1) / GraphicalObject.range));

                            if (Math.Abs((Y0 - openGLControl.Height) / Grid.yCellSize) > OS_y_begin &&
                                Math.Abs((Y1) / Grid.yCellSize) < OS_y_end)
                            draw_line(cursor_X(x), Y0, cursor_X(x+1), Y1, true,(Single)153/255, (Single)51 /255, (Single)1, 3.0f);

                            if (BoolTextIsEnabledOtherwiseQuads && y > OS_y_begin && y < OS_y_end)
                                Draw_Text(cursor_X(x), cursor_Y(y), GraphicalObject.GraphicalVector[X_counter].ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()));
                        }
                    }
                }
            }
            //LeftTopCellOfEachMatrix[]
            //OS_y_end




            //It's real draw now
            //Grid.DeadPoint.y = Grid.NetWorkValue.Count()* Grid.yCellSize;
            LaserCrossroad();
            NumberCrossroad();
        }
        Stack<int> MatrixToView = new Stack<int>();
        public bool BoolTextIsEnabledOtherwiseQuads = true;
        public bool BoolLinesAreEnabled = true;
        private int cursor_X(int value)
        {
            return value * Grid.xCellSize + Grid.initP.x;
        }
        private int cursor_Y(int value)
        {
            return -value * Grid.yCellSize + Grid.initP.y;
        }

        public int Number_of_current_matrix = 0;
        public int Number_of_current_row = 0;
        public int Number_of_current_column = 0;
        double double_trash;
        private void NumberCrossroad()
        {
            if (TargetNumber)
            {
                bool ShowTheTrueTruth = false;
                //try
                //{
                //    double_trash = List_Of_Objects[Number_of_current_matrix][Number_of_current_column, Number_of_current_row];
                //}
                //catch (Exception Trashnyak)
                //{ }
                
                    int Color = 0;
                    if (!BoolTextIsEnabledOtherwiseQuads)
                    { Color = 255; }

                    Number_of_current_column = ((int)(mouse.ShiftedPosition.x + mouse.true_x) / Grid.xCellSize);
                

                    //Щас используется абсолютное значение y, нам надо узнать текущую матрицу и вычесть
                    //LeftTopCellOfEachMatrix

                    if (LeftTopCellOfEachMatrix.Count() != 0)
                    {
                        Point CurrentMatrix = LeftTopCellOfEachMatrix[0];
                        int y_pointed = mouse.ShiftedPosition.y + openGLControl.Height - mouse.true_y - Grid.yCellSize / 4;
                        for (int i = 0; i < LeftTopCellOfEachMatrix.Count(); i++)
                        {
                            if (LeftTopCellOfEachMatrix[i].Y * Grid.yCellSize < y_pointed)
                            {
                                CurrentMatrix = LeftTopCellOfEachMatrix[i];
                                Number_of_current_matrix = i;
                            }
                            else break;
                        }

                        int y = ((y_pointed - CurrentMatrix.Y * Grid.yCellSize) / Grid.yCellSize);
                    //int y = (mouse.ShiftedPosition.y + openGLControl.Height - mouse.true_y) / Grid.yCellSize;

                    Number_of_current_row = y;

                    if (Number_of_current_column > 0 && Number_of_current_row > 0
                     && Number_of_current_row <= List_Of_Objects[Number_of_current_matrix].yCellCount
                        && Number_of_current_column <= List_Of_Objects[Number_of_current_matrix].xCellCount)
                        ShowTheTrueTruth = true;

                    if (ShowTheTrueTruth)
                        Draw_Text(mouse.true_x + 20, mouse.true_y - 20, "| " + (Number_of_current_column - 1).ToString(), Color, Color, Color);

                    if (ShowTheTrueTruth)
                    Draw_Text(mouse.true_x + 20, mouse.true_y + 10, "- " + ((Number_of_current_row) - 1).ToString(), Color, Color, Color);

                        if (!BoolTextIsEnabledOtherwiseQuads && !double.IsNaN(double_trash)) Draw_Text(mouse.true_x + 20, mouse.true_y - 50, "x: " + List_Of_Objects[Number_of_current_matrix][Number_of_current_column, Number_of_current_row].ToString(font_format.ToString() + FontQuanitityAfterPoint.ToString()), Color, Color, Color);

                    }
                
            }
        }
        private void LaserCrossroad()
        {
            //Целеуказатель плюсиком зеленый
            if (TargetPlus)
            {
                draw_line(0, mouse.true_y, openGLControl.Width, mouse.true_y, false, 0, 1, 0, 3.0f);
                draw_line(mouse.true_x, 0, mouse.true_x, openGLControl.Height, false, 0, 1, 0, 3.0f);
            }
        }
        void Draw_Text(int in_x, int in_y, string phrase)
        {
            OpenGL gl = openGLControl.OpenGL;

            if (true)
            {
                in_x -= mouse.ShiftedPosition.x;
                in_y += +mouse.ShiftedPosition.y;
            }
            Ultimate_DrawText(in_x, in_y, 0, 0, 0, "Calibri", FontSize, phrase);
        }
        void Draw_Text(int in_x, int in_y, string phrase, Single r, Single g, Single b)
        {
            OpenGL gl = openGLControl.OpenGL;

            if (false)
            {
                in_x -= mouse.ShiftedPosition.x;
                in_y += +mouse.ShiftedPosition.y;
            }
            Ultimate_DrawText(in_x, in_y, r, g, b, "Arial", 14, phrase);
        }
        static Single Line_Height = 0.5f;
        static float fontsize;
        static OpenGL gl;
        static int x, y;
        static float static_step;
        
        void X_draw_move()
        {
            x += (int)static_step;
        }
        enum Actions { Верхняя,Нижняя,Средняя, Левая_верт_полная, Левая_верт_нижняя, Левая_верт_верхняя,
            Правая_верт_полная, Правая_верт_нижняя, Правая_верт_верхняя, Запятая, Наискосок_семерки, Х_верхне_лев, Х_верхне_прав, Х_нижн_лев, Х_нижн_прав, Б_верхняя, Б_нижняя
        }
        void Enum_act(Actions action)
        {
            switch (action)
            {
                case Actions.Левая_верт_полная:
                    fa.Левая_верт_полная();
                    break;
                case Actions.Левая_верт_нижняя:
                    fa.Левая_верт_нижняя();
                    break;
                case Actions.Левая_верт_верхняя:
                    fa.Левая_верт_верхняя();
                    break;
                case Actions.Правая_верт_верхняя:
                    fa.Правая_верт_верхняя();
                    break;
                case Actions.Правая_верт_нижняя:
                    fa.Правая_верт_нижняя();
                    break;
                case Actions.Правая_верт_полная:
                    fa.Правая_верт_полная();
                    break;
                case Actions.Средняя:
                    fa.Средняя();
                    break;
                case Actions.Нижняя:
                    fa.Нижняя();
                    break;
                case Actions.Верхняя:
                    fa.Верхняя();
                    break;
                case Actions.Запятая:
                    fa.Запятая();
                    break;
                case Actions.Наискосок_семерки:
                    fa.Наискосок_семерки();
                    break;
                case Actions.Х_верхне_лев:
                    fa.Х_верхне_лев();
                    break;
                case Actions.Х_верхне_прав:
                    fa.Х_верхне_прав();
                    break;
                case Actions.Х_нижн_лев:
                    fa.Х_нижн_лев();
                    break;
                case Actions.Х_нижн_прав:
                    fa.Х_нижн_прав();
                    break;
                case Actions.Б_верхняя:
                    fa.Б_верхняя();
                    break;
                case Actions.Б_нижняя:
                    fa.Б_нижняя();
                    break;

            }
        }
        //Мысленно здесь класс функций калькуляторного шрифта начинается
        public class FA
        {
            public void Левая_верт_полная()
            {
                //Левая полная черта
                gl.Vertex(x, y, Line_Height);
                gl.Vertex(x, y + fontsize, Line_Height);
            }
            public void Верхняя()
            {
                //Верхняя черта
                gl.Vertex(x, y + fontsize, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
            }
            public void Нижняя()
            {
                //Нижняя линия
                gl.Vertex(x, y, Line_Height);
                gl.Vertex(x + fontsize / 2, y, Line_Height);
            }
            public void Средняя()
            {
                //Средняя черта
                gl.Vertex(x, y + fontsize / 2, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize / 2, Line_Height);
            }
            public void Левая_верт_нижняя()
            {
                //Левая нижняя черта
                gl.Vertex(x, y, Line_Height);
                gl.Vertex(x, y + fontsize / 2, Line_Height);
            }
            public void Левая_верт_верхняя()
            {
                //Левая верхняя черта
                gl.Vertex(x, y + fontsize, Line_Height);
                gl.Vertex(x, y + fontsize / 2, Line_Height);
            }
            public void Правая_верт_полная()
            {
                //Правая полная черта
                gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                gl.Vertex(x + fontsize / 2, y, Line_Height);
            }
            public void Правая_верт_нижняя()
            {
                //Правая нижняя черта
                gl.Vertex(x + fontsize / 2, y, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize / 2, Line_Height);
            }
            public void Правая_верт_верхняя()
            {
                //Правая верхняя черта
                gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize / 2, Line_Height);
            }
            public void Запятая()
            {
                gl.Vertex(x + fontsize / 4, y, Line_Height);
                gl.Vertex(x, y - fontsize / 4, Line_Height);
            }
            public void Наискосок_семерки()
            {
                //Наискосок семерки
                gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                        gl.Vertex(x, y, Line_Height);
            }
            public void Х_верхне_лев()
            {
                gl.Vertex(x, y + fontsize, Line_Height);
                gl.Vertex(x + fontsize / 4, y + fontsize / 2, Line_Height);
            }
            public void Х_верхне_прав()
            {
                gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                gl.Vertex(x + fontsize / 4, y + fontsize/2, Line_Height);
            }
            public void Х_нижн_лев()
            {
                gl.Vertex(x + fontsize / 4, y + fontsize/2, Line_Height);
                gl.Vertex(x, y, Line_Height);
            }
            public void Х_нижн_прав()
            {
                gl.Vertex(x + fontsize / 4, y + fontsize / 2, Line_Height);
                gl.Vertex(x + fontsize/2, y, Line_Height);
            }
            public void Б_верхняя()
            {
                gl.Vertex(x, y + fontsize, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize *3 / 4, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize * 3 / 4, Line_Height);
                gl.Vertex(x, y + fontsize/2, Line_Height);
            }
            public void Б_нижняя()
            {
                gl.Vertex(x, y + fontsize/2, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize / 4, Line_Height);
                gl.Vertex(x + fontsize / 2, y + fontsize / 4, Line_Height);
                gl.Vertex(x, y, Line_Height);
            }
        }
        FA fa = new FA();
        bool Its_number(char symbol)
        {
            switch (symbol)
            {
                case '0': return true;
                case '1': return true;
                case '2': return true;
                case '3': return true;
                case '4': return true;
                case '5': return true;
                case '6': return true;
                case '7': return true;
                case '8': return true;
                case '9': return true;
                case '-': return true;
                case '+': return true;
                default: return false;
            }
            }
        //Мысленно здесь класс функций калькуляторного шрифта заканчивается
        void Ultimate_DrawText(int _x, int _y, Single r, Single g, Single b, string Font, float _fontsize, string phrase)
        {
            //openGLControl.OpenGL.DrawText(x, y, r, g, b, "TimesNewRoman", fontsize, phrase);

            bool Bool_its_number = Its_number(phrase[0]);

            x = _x; y = _y;
            gl = openGLControl.OpenGL;
            //  Clear the color and depth buffer.
            //  Load the identity matrix.
            gl.LoadIdentity();
            gl.Color(r, g, b, 1.0f); //Must have, weirdness!
            gl.LineWidth(1.0f);
            gl.Begin(OpenGL.GL_LINES);

            fontsize = _fontsize;

            phrase = phrase.ToLower();
            static_step = fontsize * 8 / 10;
            //fontsize
            foreach (var symbol in phrase)
            {
                switch (symbol)
                {
                    case ',':
                        Enum_act(Actions.Запятая);
                        X_draw_move();
                        break;
                    case '.':
                        Enum_act(Actions.Запятая);
                        X_draw_move();
                        break;
                    case '0':
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case '1':
                        Enum_act(Actions.Правая_верт_полная);
                        //Палочка однерки
                        gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 4, y + fontsize/2, Line_Height);
                        X_draw_move();
                        break;
                    case '2':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Левая_верт_нижняя);
                        Enum_act(Actions.Правая_верт_верхняя);

                       X_draw_move();
                        break;
                    case '3':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case '4':
                        Enum_act(Actions.Правая_верт_полная);
                        Enum_act(Actions.Средняя);
                        //Наискосок чертверки
                        gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                        gl.Vertex(x, y + fontsize / 2, Line_Height);
                        X_draw_move();
                        break;
                    case '5':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_верхняя);
                        Enum_act(Actions.Правая_верт_нижняя);
                        X_draw_move();
                        break;
                    case '6':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Правая_верт_нижняя);
                        Enum_act(Actions.Левая_верт_полная);
                        X_draw_move();
                        break;
                    case '7':
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Наискосок_семерки);
                        X_draw_move();
                        break;
                    case '8':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);

                        X_draw_move();
                        break;
                    case '9':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Правая_верт_полная);
                        Enum_act(Actions.Левая_верт_верхняя);
                        X_draw_move();
                        break;
                    case '-':
                        Enum_act(Actions.Средняя);
                        X_draw_move();
                        break;
                    case '|':
                        //Средняя вертикально нижняя черта
                        gl.Vertex(x + fontsize / 4, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 4, y, Line_Height);
                        X_draw_move();
                        break;
                    case '+':
                        Enum_act(Actions.Средняя);
                        //Средняя вертикальная черта
                        gl.Vertex(x + fontsize / 4, y + fontsize / 4, Line_Height);
                        gl.Vertex(x + fontsize / 4, y + fontsize *3 / 4, Line_Height);
                        X_draw_move();
                        break;
                    case 'e':
                        if (Bool_its_number)
                        {
                            //Левая полная черта
                            gl.Vertex(x, y, Line_Height);
                            gl.Vertex(x, y + fontsize / 2, Line_Height);
                            //Нижняя линия
                            gl.Vertex(x, y, Line_Height);
                            gl.Vertex(x + fontsize / 2, y, Line_Height);
                            //Верхняя черта
                            gl.Vertex(x, y + fontsize / 2, Line_Height);
                            gl.Vertex(x + fontsize / 2, y + fontsize / 2, Line_Height);
                            //Правая верхняя черта
                            gl.Vertex(x + fontsize / 2, y + fontsize / 2, Line_Height);
                            gl.Vertex(x + fontsize / 2, y + fontsize / 4, Line_Height);
                            //Средняя черта
                            gl.Vertex(x, y + fontsize / 4, Line_Height);
                            gl.Vertex(x + fontsize / 2, y + fontsize / 4, Line_Height);
                        }
                        else
                        {
                            Enum_act(Actions.Средняя);
                            Enum_act(Actions.Верхняя);
                            Enum_act(Actions.Нижняя);
                            Enum_act(Actions.Левая_верт_полная);
                        }
                        X_draw_move();
                        break;
                    case '#':
                        X_draw_move();
                        break;
                    case 'a':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case 'b':
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Б_верхняя);
                        Enum_act(Actions.Б_нижняя);
                        X_draw_move();
                        break;
                    case 'c':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_полная);
                        X_draw_move();
                        break;
                    case 'd':
                        Enum_act(Actions.Левая_верт_полная);
                        //Enum_act(Actions.Х_верхне_лев);
                        //Enum_act(Actions.Х_нижн_лев);
                        gl.Vertex(x + fontsize / 2, y + fontsize/2, Line_Height);
                        gl.Vertex(x, y, Line_Height);
                        gl.Vertex(x, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 2, y + fontsize / 2, Line_Height);
                        X_draw_move();
                        break;
                    case 'f':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_полная);
                        X_draw_move();
                        break;
                    case 'g':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Правая_верт_нижняя);
                        Enum_act(Actions.Левая_верт_полная);
                        gl.Vertex(x + fontsize / 4, y + fontsize / 2, Line_Height);
                        gl.Vertex(x + fontsize / 2, y + fontsize / 2, Line_Height);
                        X_draw_move();
                        break;
                    case 'h':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case 'i':
                        //Средняя вертикально нижняя черта
                        gl.Vertex(x + fontsize / 4, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 4, y, Line_Height);
                        //Снизу и сверху черточка
                        gl.Vertex(x + fontsize * 1 / 8, y, Line_Height);
                        gl.Vertex(x + fontsize * 3 / 8, y, Line_Height);
                        gl.Vertex(x + fontsize * 1 / 8, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize * 3 / 8, y + fontsize, Line_Height);
                        X_draw_move();
                        break;
                    case 'j':
                        //Верхняя черта
                        gl.Vertex(x + fontsize / 4, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case 'k':
                        Enum_act(Actions.Левая_верт_полная);
                        gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                        gl.Vertex(x, y + fontsize / 2, Line_Height);
                        gl.Vertex(x, y + fontsize / 2, Line_Height);
                        gl.Vertex(x + fontsize / 2, y, Line_Height);
                        X_draw_move();
                        break;
                    case 'l':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Левая_верт_полная);
                        X_draw_move();
                        break;
                    case 'm':
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        Enum_act(Actions.Х_верхне_лев);
                        Enum_act(Actions.Х_верхне_прав);
                        X_draw_move();
                        break;
                    case 'n':
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        gl.Vertex(x, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 2, y , Line_Height);
                        X_draw_move();
                        break;
                    case 'o':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case 'p':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_верхняя);
                        X_draw_move();
                        break;
                    case 'q':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        Enum_act(Actions.Запятая);
                        //gl.Vertex(x + fontsize / 3, y - fontsize / 3, Line_Height);
                        //gl.Vertex(x + fontsize / 5, y, Line_Height);
                        X_draw_move();
                        break;
                    case 'r':
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Б_верхняя);
                        gl.Vertex(x, y + fontsize / 2, Line_Height);
                        gl.Vertex(x + fontsize / 2, y, Line_Height);
                        X_draw_move();
                        break;
                    case 's':
                        Enum_act(Actions.Средняя);
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Левая_верт_верхняя);
                        Enum_act(Actions.Правая_верт_нижняя);
                        X_draw_move();
                        break;
                    case 't':
                        Enum_act(Actions.Верхняя);
                        //Средняя вертикально нижняя черта
                        gl.Vertex(x + fontsize / 4, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 4, y, Line_Height);
                        X_draw_move();
                        break;
                    case 'u':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case 'v':
                        //Средняя вертикально нижняя черта
                        gl.Vertex(x, y + fontsize, Line_Height);
                        gl.Vertex(x + fontsize / 4, y, Line_Height);
                        //Средняя вертикально нижняя черта
                        gl.Vertex(x + fontsize / 4, y, Line_Height);
                        gl.Vertex(x + fontsize / 2, y + fontsize, Line_Height);
                        X_draw_move();
                        break;
                    case 'w':
                        Enum_act(Actions.Х_нижн_лев);
                        Enum_act(Actions.Х_нижн_прав);
                        Enum_act(Actions.Левая_верт_полная);
                        Enum_act(Actions.Правая_верт_полная);
                        X_draw_move();
                        break;
                    case 'x':
                        Enum_act(Actions.Х_верхне_лев);
                        Enum_act(Actions.Х_верхне_прав);
                        Enum_act(Actions.Х_нижн_лев);
                        Enum_act(Actions.Х_нижн_прав);
                        X_draw_move();
                        break;
                    case 'y':
                        Enum_act(Actions.Х_верхне_лев);
                        Enum_act(Actions.Х_верхне_прав);
                        //Средняя вертикально нижняя черта
                        gl.Vertex(x + fontsize / 4, y + fontsize / 2, Line_Height);
                        gl.Vertex(x + fontsize / 4, y, Line_Height);
                        X_draw_move();
                        break;
                    case 'z':
                        Enum_act(Actions.Нижняя);
                        Enum_act(Actions.Верхняя);
                        Enum_act(Actions.Наискосок_семерки);
                        X_draw_move();
                        break;
                    case ' ':
                        X_draw_move();
                        break;
                    case '_':
                        Enum_act(Actions.Нижняя);
                        X_draw_move();
                        break;

                    default:
                    //gl.Vertex(x, y, Line_Height);
                        //gl.Vertex(x+ (fontsize / 2), y+ fontsize, Line_Height);
                        X_draw_move();
                        break;
                }
                //gl.Color(0.0f, 0.0f, 1.0f);
                //gl.Vertex(x_from, y_from, Line_Height);
                //gl.Color(0.0f, 0.0f, 1.0f);
                //gl.Vertex(x_to, y_to, Line_Height);
            }
            gl.End();
        }
        void Draw_Horizontal_numbers_for_matrix(GraphicObject obj)
        {
            OpenGL gl = openGLControl.OpenGL;
            int Count_by_X = 0;

            for (int i = 0; i < obj.xCellCount; i++)
            {
                //На заметку. Тут не смотря их наличие по ОсиХ, их надо отображать лишь на
                //определенной оси Y, хмм, мы можем воспользоваться старой доброй yCellBelong функций. точняк.
                while (Grid.X_Y_counter.x >= Grid.NetWorkOS_X.Count())
                    Grid.NetWorkOS_X.Add(new Net.NetWorkOSCell());
                Grid.NetWorkOS_X[Grid.X_Y_counter.x].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawText, Count_by_X.ToString(), Grid.X_Y_counter.x, Grid.X_Y_counter.y));
                //Draw_Text(Grid.cursorP.x, Grid.cursorP.y, Count_by_X.ToString());
                
                Grid.X_move();
                Count_by_X++;
            }

            Grid.X_move();
            Grid.X_nullificate();
        }
        void Draw_line_net_for_matrix(GraphicObject obj, int Y_start)
        {
            Grid.X_move();
            for(int i = 0; i < obj.xCellCount; i++)
            {
                //if (Belongs_xCellArea())
                while (Grid.X_Y_counter.x >= Grid.NetWorkOS_X.Count())
                    Grid.NetWorkOS_X.Add(new Net.NetWorkOSCell());
                Grid.NetWorkOS_X[Grid.X_Y_counter.x].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawLine, "", Grid.X_Y_counter.x, Y_start, Grid.X_Y_counter.x, Y_start + obj.yCellCount));
                //draw_line(Grid.cursorP.x, Y_start,Grid.cursorP.x, Grid.cursorP.y);
                Grid.X_move();
            }
            while (Grid.X_Y_counter.x >= Grid.NetWorkOS_X.Count())
                Grid.NetWorkOS_X.Add(new Net.NetWorkOSCell());
            Grid.NetWorkOS_X[Grid.X_Y_counter.x].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawLine, "", Grid.X_Y_counter.x, Y_start, Grid.X_Y_counter.x, Y_start + obj.yCellCount));
            //draw_line(Grid.cursorP.x, Y_start,Grid.cursorP.x, Grid.cursorP.y);

            Grid.X_nullificate();
            Grid.X_move();
            Grid.X_Y_counter.y = Y_start;
            for (int i = 0; i <= obj.yCellCount; i++)
            { 
                Grid.NetWorkOS_Y[Grid.X_Y_counter.y].List_of_func.Add(new Net.OSCell(Net.FunctionType.DrawLine, "", Grid.X_Y_counter.x, Grid.X_Y_counter.y, Grid.X_Y_counter.x + obj.xCellCount, Grid.X_Y_counter.y));
                Grid.Y_move();
            }   
            Grid.X_nullificate();
        }
        /// <summary>
        /// Draw Grid
        /// </summary>
        private void draw_line(int x_from,int y_from = 0, int x_to = 0, int y_to = 0, bool autoshifter = true, Single r = 0, Single g = 0, Single b = 0, float linewidth = 1.0f, float lineheight = 0.5f)
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
            gl.LineWidth(linewidth);
            gl.Begin(OpenGL.GL_LINES);

            //Single Line_Height = 1.0f;

            //gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(x_from, y_from, lineheight);
            //gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(x_to, y_to, lineheight);
            gl.End();
        }

        private double double_min = double.MaxValue;
        private double double_max = double.MinValue;
        private void draw_white_square(int x_from, int y_from, double value)
        {
            x_from -= mouse.ShiftedPosition.x;
            y_from += mouse.ShiftedPosition.y;
            int x_to = x_from;
            int y_to = y_from;

            //x_from -=  + 3;
            //y_from +=  + Grid.yCellSize * 3 / 4;
            //x_to -=  + 3;
            //y_to +=  + Grid.yCellSize * 3 / 4;

            x_from += - 3 - 1;
            y_from += - Grid.yCellSize / 4 - 1;
            x_to +=  + Grid.yCellSize * 3 / 4 + 1;
            y_to +=  + Grid.yCellSize * 3 / 4 + 1;

            //x_from += -5;
            //y_from += -5;
            //x_to += 0;
            //y_to += 0;

            //Чтобы не прописывать постоянно
            OpenGL gl = openGLControl.OpenGL;
            //  Clear the color and depth buffer.
            //  Load the identity matrix.
            gl.LoadIdentity();

            float temp_procent_color = (float)((value - double_min) / (double_max - double_min));

            gl.Color(temp_procent_color, 0.0f, 1.0f - temp_procent_color, 1.0f); //Must have, weirdness!
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

        public int xCellSize_old = 80, yCellSize_old = 30;

        public enum FunctionType { DrawText, DrawLine, DrawQuad };
        public class NetWorkOSCell
        {
            public List<OSCell> List_of_func = new List<OSCell>();
        }
        public class OSCell
        {
            public OSCell(FunctionType in_func, string in_str, int _value1, int _value2, int _value3 = 0, int _value4 = 0)
            {
                func_type = in_func; str = in_str;
                value1 = _value1; value2 = _value2; value3 = _value3; value4 = _value4;
            }
            public FunctionType func_type;

            //z = NumberMatrix, y = NumberRow, x = NumberColumn;
            public int value1, value2, value3, value4;
            public string str;
        }
        public class NetWorkValueableCell
        {
            public NetWorkValueableCell(double in_value = Double.NaN)
            {
                Cellvalue = in_value;
            }
            public double Cellvalue;
        }
        //The Network has been established
        public List<List<double>> NetWorkValue = new List<List<double>>();
        public List<NetWorkOSCell> NetWorkOS_X = new List<NetWorkOSCell>();
        public List<NetWorkOSCell> NetWorkOS_Y = new List<NetWorkOSCell>();
        public Net(GraphicData _GD_link)
        {
            GD_link = _GD_link;
            cursorP = new PointInt(initP.x, initP.y);
        }
        GraphicData GD_link;
        public void X_move()
        {
            cursorP.x += xCellSize;
            X_Y_counter.x++;

            if (YourRequireNewMemory)
                NetWorkValue[X_Y_counter.y].Add(double.NaN);
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

            while (X_Y_counter.y >= NetWorkOS_Y.Count())
            {
                if (YourRequireNewMemory)
                {
                    NetWorkValue.Add(new List<double>());
                    NetWorkValue[X_Y_counter.y].Add(double.NaN);
                }

                NetWorkOS_Y.Add(new NetWorkOSCell());
            }
        }
        List<int> MemorySizeChangeChecker = new List<int>();
        bool YourRequireNewMemory = true;
        bool InitiaiteMemoryChecking()
        {
            int counter = 0;
            foreach (var Object in GD_link.List_Of_Objects)
                foreach (var vector in Object.Matrix)
                {
                    if (MemorySizeChangeChecker.Count() > counter)
                    {
                        if (MemorySizeChangeChecker[counter] != vector.Count())
                            return false;
                    }
                    else return false;

                    counter++;
                }
            return true;
        }
        void InitiaiteMemoryRewriter()
        {
            MemorySizeChangeChecker.Clear();
            int counter = 0;
            foreach (var Object in GD_link.List_Of_Objects)
                foreach (var vector in Object.Matrix)
                {
                    MemorySizeChangeChecker.Add(vector.Count());
                }
        }
        public void Y_nullificate()
        {
            cursorP.y = initP.y;
            X_Y_counter.y = 0;

            if (GD_link.List_Of_Objects.Count() != 0)
            {
                if (InitiaiteMemoryChecking())
                {
                    YourRequireNewMemory = false;
                }
                else
                {
                    YourRequireNewMemory = true;
                    InitiaiteMemoryRewriter();
                }
            }
            else YourRequireNewMemory = true;
            //else NetWorkValue[X_Y_counter.y][X_Y_counter.x].CellCursorP.Y = cursorP.y;
            if (YourRequireNewMemory)
            {
                NetWorkValue.Clear();
                NetWorkValue.Add(new List<double>());
                NetWorkValue[X_Y_counter.y].Add(double.NaN);
            }
            foreach (var item in NetWorkOS_Y) item.List_of_func.Clear();
            NetWorkOS_Y.Clear();
            NetWorkOS_Y.Add(new NetWorkOSCell());

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
        bool IsPressedPressedBefore = false;
        public void Mouse_movements()
        {
            IsPressedPressedBefore = false;
            //Если мышка ранее не была нажатой, то запомнить последними коордами текущие коорды
            if (isPressedBefore == false && isPressed == true)
            {
                IsPressedPressedBefore = true;
                isPressedBefore = true;
                LastPosition.x = x;
                LastPosition.y = y;
            }

            //Посчитать смещение мышки и прибавить к итогу.
            if (isPressed == true)
            {
                //if (!IsPressedPressedBefore)
                //{
                    //Console.Write("");
                //}
                ShiftPosition.x = (int)((x - LastPosition.x) / mouse_decrease);
                ShiftPosition.y = (int)((y - LastPosition.y) / mouse_decrease);

                LastPosition.x = x;
                LastPosition.y = y;

                BorderEndRecalculate();

                if (ShiftedPosition.x - ShiftPosition.x > BorderBegin.x && ShiftedPosition.x - ShiftPosition.x < (Math.Abs(BorderEnd.x)))
                    ShiftedPosition.x -= ShiftPosition.x;

                if (ShiftedPosition.y - ShiftPosition.y > BorderBegin.y && ShiftedPosition.y - ShiftPosition.y < (Math.Abs(BorderEnd.y)))
                ShiftedPosition.y -= ShiftPosition.y;
            }
            


            
        }
        public void BorderEndRecalculate()
        {
            if (Grid != null)
            {
                if (Grid.NetWorkOS_X != null && Grid.NetWorkOS_Y != null)
                {
                    BorderEnd.x = Grid.NetWorkOS_X.Count() * Grid.xCellSize;//+Grid.DeadPoint.x - openGLControl.Width + Grid.xCellSize;
                    BorderEnd.y = Grid.NetWorkOS_Y.Count() * Grid.yCellSize;//Grid.DeadPoint.y;

                    if (BorderEnd.x - openGLControl.Width > 0 &&
                        BorderEnd.y - openGLControl.Height > 0)
                    {
                        BorderEnd.x -= openGLControl.Width;
                        BorderEnd.y -= openGLControl.Height;
                    }
                }
            }
            else
            {
                BorderEnd.x = 50;
                BorderEnd.y = 50;
            }
        }
        public PointInt BorderBegin = new PointInt(0, 0);//самый верхний левый угол для ShiftedPosition
        public PointInt BorderEnd = new PointInt(15, 10);//самый нижний правый угол для ShiftedPosition

    }

}
