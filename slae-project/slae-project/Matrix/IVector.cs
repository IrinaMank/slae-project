using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project.Matrix
{
    interface IVector
    {
        // Операция простого сложения векторов с коэффициентами
        // result = coef1 * this + coef2 * b
        // Удобно тем, что отсутвуют ненужные промежуточные результаты
        // Если _override == false, то результат НЕ записывается в вызывающую структуру, а возвращается как результат (return result)
        // Если _override == true, то результат записывается в вызывающую структуру, И возвращается как результат (return this)
        IVector Add(IVector b, double coef1 = 1.0, double coef2 = 1.0, bool _override = false);

        // Скалярное произведение вектора на b
        double ScalarMult(IVector b);
        
        // Норма разности векторов
        // ||a - b||
        double Norm(IVector b);

        // Норма вектора
        // ||a||
        double Norm();
    }
}
