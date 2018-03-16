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
        Dictionary <string, string> requiredFileNames { get; }
        // Если в матрице нет i,j-го элемента, то метод должен возвращать 0
        /// <summary>
        /// Доступ к i,j-му элементу матрицы
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        double this[int i, int j] { get; set; }

        /// <summary>
        /// Заполнение матрицы значениями из файлов. 
        /// </summary>
        /// <param name="paths">Словарь в формате "Название требуемого файла" - "Путь до файла". Необходимые файлы можно получить из параметра requiredFileNames.</param>
        void FillByFiles(Dictionary<string, string> paths);
    }
}
