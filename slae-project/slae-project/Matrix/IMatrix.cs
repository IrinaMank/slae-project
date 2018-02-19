using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    public interface IMatrix : ILinearOperator, IEnumerable<(double value, int row, int col)>
    {
        // Если в матрице нет i,j-го элемента, то метод должен возвращать 0
        /// <summary>
        /// Доступ к i,j-му элементу матрицы
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        double this[int i, int j] { get; set; }
    }
}
