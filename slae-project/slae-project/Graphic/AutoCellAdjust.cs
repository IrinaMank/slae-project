using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace slae_project
{
    public partial class SharpGLForm
    {
        
        public void setAutoCell()
        {
            int borderIndent = 10;
            foreach (var obj in GD.List_Of_Objects)
            {
                //Отдели от предыдущей двумя очень длинными горизонтальными линиями
                /* if (Belongs_yCellArea())
                 {
                     draw_line(0, Grid.cursorP.y,
                                     100000, Grid.cursorP.y);

                     draw_line(0, Grid.cursorP.y + 2,
                                     100000, Grid.cursorP.y + 2);
                 }
                 Grid.X_move();
                 //Напиши как называется текущая матрица
                 Draw_Text(Grid.cursorP.x, Grid.cursorP.y, obj.Name);
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

     */
                //Для каждого вектора текущей матрицы
                int maxWidth = 1;
                int tempWidth;
                foreach (var vect in obj.Matrix)
                {
                    foreach (var value in vect)
                    {
                        tempWidth = value.ToString().Length;
                        if (maxWidth < tempWidth)
                        {
                            maxWidth = tempWidth;
                        }

                    }
                }
                GD.Grid.xCellSize = (int)(maxWidth * GD.FontSize);
                GD.Grid.yCellSize = (int)(GD.FontSize);
                //openGLControl.Refresh();
            }
            
        }
    }
}
