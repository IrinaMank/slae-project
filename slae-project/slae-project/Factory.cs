using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slae_project.Matrix;
using slae_project.Solver;
using slae_project.Vector;
using slae_project.Preconditioner;

namespace slae_project
{
    class Factory
    {
        Form1 main_form;

        public static Dictionary<string, string> DictionaryOfFormats = Form2.filenames_format;//словарь путей до массивов
        static public Dictionary<string, Func <Dictionary<string, string>, IMatrix>> MatrixTypes = new Dictionary<string, Func<Dictionary<string, string>, IMatrix>>();
        public static IMatrix ObjectOfIMatrix; // ?????????
        static public Dictionary<string, Func <IMatrix, double, string, IVector, IVector>> SolverTypes = new Dictionary<string, Func<IMatrix, double, string, IVector, IVector>>();
        static public List <string> PrecondTypes = new List<string>();
        
        public void RegisterPrecondClass(string Name)
         {
             PrecondTypes.Add(Name);
         }
        static public void RegisterMatrixClass(string Name, Func<Dictionary<string, string>, IMatrix> Creator1)
        {
            MatrixTypes.Add(Name, Creator1);
        }

        public void RegisterSolverClass(string Name,  Func<IMatrix, double, string, IVector, IVector> Creator1)
        {
            SolverTypes.Add(Name, Creator1);
        }

        public Factory()
        {
            RegisterPrecondClass("Диагональное");
            RegisterPrecondClass("Методом Зейделя");
            RegisterPrecondClass("LU-разложение");

            RegisterMatrixClass("Координатный", (Dictionary<string, string> DictionaryOfFormats) => new CoordinateMatrix.requiredFileNames(DictionaryOfFormats));
            RegisterMatrixClass("Плотный", (Dictionary<string, string> DictionaryOfFormats) => new DenseMatrix.requiredFileNames(DictionaryOfFormats));
           // RegisterMatrixClass("Строчный", (Dictionary<string, string> DictionaryOfFormats) => new SparseRowMatrix(DictionaryOfFormats));
            RegisterMatrixClass("Строчно - столбцовый", (Dictionary<string, string> DictionaryOfFormats) => new SparseRowColumnMatrix.requiredFileNames(DictionaryOfFormats));

            RegisterSolverClass("Метод сопряжённых градиентов", (IMatrix Object, double accur, string p, IVector r) => new MSGSolver..Solver(Form1.str_precond, ObjectOfIMatrix, Form2.F, Form1.s_accur_number, Form1.max_iter));
            RegisterSolverClass("Локально-оптимальная схема", (IMatrix Object, double accur, string p, IVector r) => new LOSSolver.Solver(Form1.str_precond, ObjectOfIMatrix, Form2.F, Form1.s_accur_number, Form1.max_iter));
            //RegisterSolverClass("Метод Якоби", (IMatrix Object, double accur, string p, IVector r) => new Jacoby.Solver(Form1.str_precond, ObjectOfIMatrix, Form2.F, Form1.s_accur_number, Form1.max_iter));
            //RegisterSolverClass("Метод Зейделя", (IMatrix Object, double accur, string p, IVector r) => new Zeid.Solver(Form1.str_precond, ObjectOfIMatrix, Form2.F, Form1.s_accur_number, Form1.max_iter));
            RegisterSolverClass("Метод бисопряжённых градиентов", (IMatrix Object, double accur, string p, IVector r) => new BSGStabSolve.Solver(Form1.str_precond, ObjectOfIMatrix, Form2.F, Form1.s_accur_number, Form1.max_iter));
            //RegisterSolverClass("Метод обобщённых минимальных невязок", (IMatrix Object, double accur, string p, IVector r) => new SparseRowColumnMatrix(ObjectOfIMatrix, Form1.s_accur_number, Form1.str_precond, Form1.F));

        }
        static public void CreateMatrix(string typename)//конструкторы пустых матриц для каждого формата - результат в arr_format
        {
            Func<Dictionary<string, string>, IMatrix> value;
            MatrixTypes.TryGetValue(typename, out value);
            ObjectOfIMatrix = value(DictionaryOfFormats);

        }
        static public IMatrix Solver_Matrix()
        {
            return ObjectOfIMatrix;
        }

       /* static public List<string> GetArrays(string typename)//передача нам массивов от форматов матричек
        {
            return Matrix.Format_array(typename);
        }*/

       // Мы передаем симметричность/ несимметричность
       public static bool Get_format()
        {
            return Form1.property_matr;
        }
        
    }
}
