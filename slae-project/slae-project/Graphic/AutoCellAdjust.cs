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
            int maxWidth = 1;
            int maxAppropriateWidth = 0;
            int tempWidth;
            int borderIndent = 10;
            int constant = 0;
            foreach (var obj in GD.List_Of_Objects)
            {

                foreach (var vect in obj.Matrix)
                {
                    foreach (var value in vect)
                    {

                        tempWidth = value.ToString(GD.font_format.ToString() + GD.FontQuanitityAfterPoint.ToString()).Length;


                        if (maxWidth < tempWidth)
                        {
                            maxWidth = tempWidth;
                        }

                    }
                }

            }
            switch (GD.font_format)
            {
                case GraphicData.FontFormat.G:
                    maxAppropriateWidth = (int)((double)(GD.FontSize) * 0.685) + 0;
                    break;
                case GraphicData.FontFormat.F:
                    maxAppropriateWidth = (int)((double)(GD.FontSize) * 0.65) + 0;
                    break;
                case GraphicData.FontFormat.E:
                    maxAppropriateWidth = (int)((double)(GD.FontSize) * 0.65) + 0;
                    break;
            }
            switch (GD.font_format)
            {
                case GraphicData.FontFormat.G:
                    constant = 0;
                    break;
                case GraphicData.FontFormat.F:
                    constant = 10;
                    if (GD.FontSize == 6) constant += 10;
                    break;
                case GraphicData.FontFormat.E:
                    constant = 5;
                    break;
            }


            GD.Grid.xCellSize = (int)(maxAppropriateWidth * maxWidth) + constant + 10;
            GD.Grid.yCellSize = (int)(GD.FontSize) + 10;
            Refresh_Window();

        }
    }
}
