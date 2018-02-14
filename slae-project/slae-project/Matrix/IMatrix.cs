using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    public interface IMatrix
    {
        // Стандартная опреация умножения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        Vector Mult(Vector vec, bool inverse = false);

        // Операция умножения L-компоненты LU-разложения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        Vector MultL(Vector vec, bool inverse = false);

        // Операция умножения U-компоненты LU-разложения матрицы на вектор
        // inverse = true предполагает умножение обратной матрицы вместо исходной
        Vector MultU(Vector vec, bool inverse = false);

        double this[int i, int j] { get;set; }
        // Пример использования:
        // a = A.mult(b);
        // c = A.mult(A.mult(a), true);

        /* Альтернатива 1:
        // Стандартная опреация умножения матрицы на вектор
        IVector Mult(IVector vec);
        IVector MultInv(IVector vec);

        // Операция умножения L-компоненты LU-разложения матрицы на вектор
        IVector MultL(IVector vec);
        IVector MultLInv(IVector vec);

        // Операция умножения U-компоненты LU-разложения матрицы на вектор
        IVector MultU(IVector vec);         
        IVector MultUInv(IVector vec);  
    
        //Пример использования:
        //a = A.mult(b);
        //c = A.multInv(A.mult(a));
        */

        /* Альтернатива 2:
        class MatrixInv
        {
            # Описание обратной матрицы
        };
        MatrixInv Inv;
        
        // Стандартная опреация умножения матрицы на вектор
        IVector Mult(IVector vec);

        // Операция умножения L-компоненты LU-разложения матрицы на вектор
        IVector MultL(IVector vec);

        // Операция умножения U-компоненты LU-разложения матрицы на вектор
        IVector MultU(IVector vec);        
        
        //Пример использования:
        //a = A.mult(b);
        //c = A.Inv.mult(A.mult(a));
         */
    }
}
