using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;

namespace slae_project
{
    class factory
    {
        List<string> arr_format;
        Form1 main_form;

        Dictionary<string, string> S = Form2.filenames_format;//словарь путей до массивов
        static IMatrix CreateMatrix(string typename)//конструкторы пустых матриц для каждого формата - результат в arr_format
        {
            slae_project.Matrix.CoordinateMatrix coordinateMatrix ;

            switch (typename)
            {
                case "Координатный": return coordinateMatrix(Form1.Size_Matrix); // public CoordinateMatrix(int size) как вызвать?
                case "Плотный": return Dense_matrix(Form1.Size_Matrix);//
                case "Строчный": return SparseRowMatrix(Form1.Size_Matrix);
                case "Строчно - столбцовый": return SparseRowColumnMatrix(Form1.Size_Matrix);
                default: break;
            }

        }
        static List<string> GetArrays(string typename)//передача нам массивов от форматов матричек
        {
            return Format_array();
        }
        static IMatrix FullMatrix(string typename)//заполнение матриц по форматам(передаются пути к файлам)
        {
            switch (typename)
            {
                case "Координатный": return CoordinateMatrix_full(S);
                case "Плотный": return Dense_matrix_full(S);
                case "Строчный": return SparseRowMatrix_full(S);
                case "Строчно - столбцовый": return SparseRowColumnMatrix_full(S);// public SparseRowColumnMatrix(int[] ig, int[] jg, double[] di, double[] al, double[] au) Мы передаем пути, а не массивы
            }

        }
        public static IEnumerable<string> GetFormat
        {
              get
                {
                yield return "Плотный";
                yield return "Строчный";
                yield return "Строчно - столбцовый";
                yield return "Координатный";
            }
         
        }
        public static IEnumerable<string> GetSolver
        {
            get
            {
                yield return "Метод сопряжённых градиентов";
                yield return "Локально-оптимальная схема";
                yield return "Метод Якоби";
                yield return "Метод Зейделя";
                yield return "Метод бисопряжённых градиентов";
                yield return "Метод обобщённых минимальных невязок";
            }
        }
        public static IEnumerable<string> GetPrecond
        {
            get
            {
                yield return "Диагональное";
                yield return "Методом Зейделя";
                yield return "LU-разложение";
            }

        }
        // Мы передаем симметричность/ несимметричность
        public static bool Get_format()
        {
            return Form1.property_matr;
        }
        
    }
}
