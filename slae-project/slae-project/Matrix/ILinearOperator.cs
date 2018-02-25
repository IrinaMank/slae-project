using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Vector;

namespace slae_project.Matrix
{
    public interface ILinearOperator
    {
        /// <summary>
        /// Опреация умножения матрицы на вектор: Ax = y
        /// </summary>
        /// <returns>Результатом является вектор (y)</returns>
        IVector Mult(IVector x);

        /// <summary>
        /// Решение СЛАУ относительно матрицы L LU-разложения матрицы: Lx = y
        /// </summary>
        /// <param name="y"></param>
        /// <param name="UseDiagonal"> =true, если предполагается наличие ненулевой диагонали в матрице L;
        /// =false иначе</param>
        /// <returns>Результатом является вектор (x)</returns>
        IVector SolveL(IVector y, bool UseDiagonal = true);

        /// <summary>
        /// Решение СЛАУ относительно матрицы U LU-разложения матрицы: Ux = y
        /// </summary>
        /// <param name="y"></param>
        /// <param name="UseDiagonal"> =true, если предполагается наличие ненулевой диагонали в матрице U;
        /// =false иначе</param>
        /// <returns>Результатом является вектор (x)</returns>
        IVector SolveU(IVector y, bool UseDiagonal = true);

        /// <summary>
        /// Умножение матрицы L LU-разложения матрицы на вектор: Lx = y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="UseDiagonal"> =true, если предполагается наличие ненулевой диагонали в матрице L;
        /// =false иначе</param>
        /// <returns>Результатом является вектор (y)</returns>
        IVector MultL(IVector x, bool UseDiagonal = true);

        /// <summary>
        /// Умножение матрицы U LU-разложения матрицы на вектор: Ux = y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="UseDiagonal"> =true, если предполагается наличие ненулевой диагонали в матрице U;
        /// =false иначе</param>
        /// <returns>Результатом является вектор (y)</returns>
        IVector MultU(IVector x, bool UseDiagonal = true);

        // Примечание: методы Transpose и T должны делать одну и ту же работу
        // Такое дублирование необходимо для того, чтобы дать пользователю возможность 
        // Писать A.Transpose для ясности кода
        // Или A.T для краткости в зависимости от предпочтений
        // Приятные удобности :3
        /// <summary>
        /// Транспонированная матрица
        /// </summary>
        ILinearOperator Transpose { get; }
        /// <summary>
        /// Транспонированная матрица
        /// </summary>
        ILinearOperator T { get; }

        /// <summary>
        /// Диагональ матрицы
        /// </summary>
        IVector Diagonal { get; }

        /// <summary>
        /// Длина стороны квадратной матрицы
        /// </summary>
        int Size { get; }
    }
}
