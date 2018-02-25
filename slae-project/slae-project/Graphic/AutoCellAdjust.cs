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
            int maxWidth = GD.ViktorsMaxWidth;
            int maxAppropriateWidth = 0;
            int tempWidth;
            int borderIndent = 10;

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


            GD.Grid.xCellSize = (int)(maxAppropriateWidth * GD.FontSize * 2.0 / 3.0) + 10;
            GD.Grid.yCellSize = (int)(GD.FontSize) + 10;
            Refresh_Window(false);

        }
    }
}