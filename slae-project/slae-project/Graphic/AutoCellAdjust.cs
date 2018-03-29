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

        /*
        * Авторазмерщик ширины/высоты ячейки в зависимости от размера шрифта

        Switch(В зависимости от формата записи, Основной, Дробный или Экспоненциальный)
        {
        Ширина ячейки = Колво_символов_числа_в_обычном_дабл(основном) + Колво_знаков_после_запятой + Константа(варьируемая от формата записи), эти три числа по разному комбинируются для этих трех форматов.
        }

        На вход просит Колво_символов_числа_в_обычном_дабл(основном) = double.ToString().Length
        и Колво_знаков_после_запятой
        И тип формата записи чисел.
        И в конце умножаются на Размер шрифта

        И дает ширину и высоту ячейки
        */
        public void setAutoCell()
        {
            GD.Grid.xCellSize_old = (GD.Grid.xCellSize);
            GD.Grid.yCellSize_old = (GD.Grid.yCellSize);

            int maxWidth = GD.ViktorsMaxWidth;
            int maxAppropriateWidth = 0;
            int tempWidth;
            int borderIndent = 10;

            if (GD.font_format == GraphicData.FontFormat.G && GD.FontQuanitityAfterPoint == 0)
                maxAppropriateWidth = maxWidth + GD.FontQuanitityAfterPoint + 2;
            else
            switch (GD.font_format)
            {
                case GraphicData.FontFormat.G:
                    maxAppropriateWidth = System.Math.Min(GD.FontQuanitityAfterPoint, maxWidth);
                    if (maxWidth > GD.FontQuanitityAfterPoint)
                    {
                        maxAppropriateWidth = GD.FontQuanitityAfterPoint + 7;
                    }
                    break;
                case GraphicData.FontFormat.F:
                    maxAppropriateWidth = maxWidth + GD.FontQuanitityAfterPoint + 2;
                    break;
                case GraphicData.FontFormat.E:
                    maxAppropriateWidth = GD.FontQuanitityAfterPoint + 7;
                    break;
            }


            GD.Grid.xCellSize = (int)(maxAppropriateWidth * GD.FontSize * 8.0 / 10.0) + 10;
            GD.Grid.yCellSize = (int)(GD.FontSize) + 14;
            Refresh_Window(false);

        }
    }
}