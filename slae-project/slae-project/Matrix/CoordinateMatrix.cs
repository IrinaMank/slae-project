using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    class CoordinateMatrix : IMatrix
    {
        // Стандартная опреация умножения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        IVector Mult(IVector vec, bool inverse = false)
        {
            return;
        }

        // Операция умножения L-компоненты LU-разложения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        IVector MultL(IVector vec, bool inverse = false)
        {

        }

        // Операция умножения U-компоненты LU-разложения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        IVector MultU(IVector vec, bool inverse = false)
        {

        }
    }
}
