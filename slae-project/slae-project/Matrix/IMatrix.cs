using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Vector;

namespace slae_project.Matrix
{
    public interface IMatrix : ILinearOperator, IEnumerable<(double value, int row, int col)>, ICloneable
    {
        // Если в матрице нет i,j-го элемента, то метод должен возвращать 0
        /// <summary>
        /// Доступ к i,j-му элементу матрицы
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        double this[int i, int j] { get; set; }

        /// <summary>
        /// Проверяет СЛАУ с исходной матрицей и b в качесте правой части на совместимость
        /// </summary>
        /// <param name="b">Правая часть СЛАУ</param>
        /// <returns>Совместима/несовместима - true/false</returns>
        bool CheckCompatibility(IVector b);
    }
}
