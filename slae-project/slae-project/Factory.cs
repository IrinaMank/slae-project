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
using slae_project.Logger;

namespace slae_project
{
    class Factory
    {
        Form1 main_form;
        FileLogger Log = new FileLogger();

        public static Dictionary<string, string> DictionaryOfFormats = Form2.filenames_format;//словарь путей до массивов
        static public Dictionary<string, Func<Dictionary<string, string>, IMatrix>> MatrixTypes = new Dictionary<string, Func<Dictionary<string, string>, IMatrix>>();
        public static IMatrix ObjectOfIMatrix;
        public static IVector Result;
        public static IPreconditioner Prec = new NoPreconditioner();
        static public Dictionary<string, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, FileLogger, IVector>> SolverTypes = new Dictionary<string, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, FileLogger, IVector>>();
        static public List<string> PrecondTypes = new List<string>();

        public void RegisterPrecondClass(string Name)
        {
            PrecondTypes.Add(Name);
        }
        static public void RegisterMatrixClass(string Name, Func<Dictionary<string, string>, IMatrix> Creator1)
        {
            MatrixTypes.Add(Name, Creator1);
        }

        public void RegisterSolverClass(string Name, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, FileLogger, IVector> Creator1)
        {
            SolverTypes.Add(Name, Creator1);
        }

        public Factory()
        {
            RegisterPrecondClass("Диагональное");
            RegisterPrecondClass("Методом Зейделя");
            RegisterPrecondClass("LU-разложение");

            //RegisterMatrixClass("Координатный", (Dictionary<string, string> DictionaryOfFormats) => CoordinateMatrix.requiredFileNames(DictionaryOfFormats));
            //RegisterMatrixClass("Плотный", (Dictionary<string, string> DictionaryOfFormats) => new DenseMatrix.requiredFileNames(DictionaryOfFormats));
            //RegisterMatrixClass("Строчный", (Dictionary<string, string> DictionaryOfFormats) => new SparseRowMatrix(DictionaryOfFormats));
            //RegisterMatrixClass("Строчно - столбцовый", (Dictionary<string, string> DictionaryOfFormats) => new SparseRowColumnMatrix.requiredFileNames(DictionaryOfFormats));

            ISolver Msg = new MSGSolver();
            ISolver Los = new LOSSolver();
            ISolver Bsg = new BSGStabSolve();

            RegisterSolverClass("Метод сопряжённых градиентов", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => Msg.Solve(Prec, ObjectOfIMatrix, Form2.F, Form2.X0, Form1.s_accur_number, Form1.max_iter, Log));
            RegisterSolverClass("Локально-оптимальная схема", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => Los.Solve(Prec, ObjectOfIMatrix, Form2.F, Form2.X0, Form1.s_accur_number, Form1.max_iter, Log));
            //RegisterSolverClass("Метод Якоби", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => new Jacoby.Solver(Prec, ObjectOfIMatrix, Form2.F, Form2.X0, Form1.s_accur_number, Form1.max_iter, Log));
            //RegisterSolverClass("Метод Зейделя", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => new Zeid.Solver(Prec, ObjectOfIMatrix, Form2.F,  Form2.X0,Form1.s_accur_number, Form1.max_iter, Log));
            RegisterSolverClass("Метод бисопряжённых градиентов", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => Bsg.Solve(Prec, ObjectOfIMatrix, Form2.F, Form2.X0, Form1.s_accur_number, Form1.max_iter, Log));
            //RegisterSolverClass("Метод обобщённых минимальных невязок", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => new SparseRowColumnMatrix.Solver(Prec, ObjectOfIMatrix, Form2.F, Form2.X0, Form1.s_accur_number, Form1.max_iter, Log));

        }

        static public void CreateMatrix(string typename)//получаем заполненную матрицу для передачи Solver
        {
            Func<Dictionary<string, string>, IMatrix> value;
            MatrixTypes.TryGetValue(typename, out value);
            ObjectOfIMatrix = value(DictionaryOfFormats);

        }
        static public void CreateSolver(string typename)//получаем заполненную матрицу для передачи Solver
        {
            Func<IPreconditioner, IMatrix, IVector, IVector, double, int, FileLogger, IVector> value;
            SolverTypes.TryGetValue(typename, out value);
            Result = value(Prec, ObjectOfIMatrix, Form2.F, Form2.X0, Form1.s_accur_number, Form1.max_iter, new FileLogger());

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
