using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Vector
{
    public interface IVector : IEnumerable<(double value, int index)>, ICloneable
    {
        /// <summary>
        /// Длина вектора
        /// </summary>
        int Size { get; }

        /// <summary>
        /// i-й элемента вектора
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        double this[int i] { get; set; }

        /// <summary>
        /// Норма вектора
        /// </summary>
        double Norm { get; }

        /// <summary>
        /// Скалярное произведение вектора на вектор vec
        /// </summary>
        /// <param name="vec">Вектор, на который происходит скалярное умножение</param>
        /// <returns></returns>
        double ScalarMult(IVector vec);

        /// <summary>
        /// Взвешенная сумма векторов: result = coef1 * this + coef2 * b
        /// </summary>
        /// <param name="b">Вектор, который необходимо добавить.</param>
        /// <param name="coef1">Коэффициент, на который домнажается исходный вектор.</param>
        /// <param name="coef2">Коэффициент, на который домнажается добавляемый вектор.</param>
        /// <param name="_override">=true, если необходимо записать результат процедуры в вызывающую переменную;
        /// =false, если необходимо вернуть результат без перезаписывания.</param>
        /// <returns>Если _override == true, то в качестве результата выступает this;
        /// если _override = false, то новый объект..</returns>
        IVector Add(IVector b, double coef1, double coef2, bool _override = false);

        /// <summary>
        /// Задать каждый элемент вектора одним и тем же числом
        /// </summary>
        /// <param name="v">Задаваемое значение</param>
        void SetConst(double v = 0);
    }
}
