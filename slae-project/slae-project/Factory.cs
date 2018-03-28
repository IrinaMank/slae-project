
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
        public static Dictionary<string, string> DictionaryOfFormats = FileLoadForm.filenames_format;//словарь путей до массивов
        static public Dictionary<string, (Func<Dictionary<string, string>, bool, IMatrix>, Dictionary<string, string>)> MatrixTypes = new Dictionary<string, (Func<Dictionary<string, string>, bool, IMatrix>, Dictionary<string, string>)>();
        public static IMatrix ObjectOfIMatrix;
        public static IVector Result;
        public static List<string> name_arr = new List<string>();
        public static IPreconditioner Prec = new NoPreconditioner();
        static public Dictionary<string, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, ILogger, IVector>> SolverTypes = new Dictionary<string, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, ILogger, IVector>>();
        static public Dictionary<string, Func<IPreconditioner>> PrecondTypes = new Dictionary<string, Func<IPreconditioner>>();

        public void RegisterPrecondClass(string Name, Func<IPreconditioner> Creator)
        {
            PrecondTypes.Add(Name, Creator);
        }
        static public void RegisterMatrixClass(string Name, Func<Dictionary<string, string>, bool, IMatrix> Creator1, Dictionary<string, string> Creator2)
        {
            MatrixTypes.Add(Name, (Creator1, Creator2));
        }

        public void RegisterSolverClass(string Name, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, ILogger, IVector> Creator1)
        {
            SolverTypes.Add(Name, Creator1);
        }

        public Factory()
        {
            //эти изменения с предобуславливателем внесла Ира и она не уверена, что все сделала верно. Но все работает. Вроде.
            RegisterPrecondClass("Диагональное", () => new DiagonalPreconditioner(ObjectOfIMatrix));
            //RegisterPrecondClass("Методом Зейделя", () => new (ObjectOfIMatrix));
            RegisterPrecondClass("LU-разложение", () => new LUPreconditioner(ObjectOfIMatrix));
            RegisterPrecondClass("Без предобуславливания", () => new NoPreconditioner());

            RegisterMatrixClass("Координатный", (Dictionary<string, string> DictionaryOfFormats, bool isSymmetric) => new CoordinateMatrix(DictionaryOfFormats, isSymmetric), CoordinateMatrix.requiredFileNames);
            RegisterMatrixClass("Плотный", (Dictionary<string, string> DictionaryOfFormats, bool isSymmetric) => new DenseMatrix(DictionaryOfFormats, isSymmetric), DenseMatrix.requiredFileNames);
            //RegisterMatrixClass("Строчный", (Dictionary<string, string> DictionaryOfFormats) => new SparseRowMatrix(DictionaryOfFormats),SparseRowMatrix.requiredFileNames);
            RegisterMatrixClass("Строчно - столбцовый", (Dictionary<string, string> DictionaryOfFormats, bool isSymmetric) => new SparseRowColumnMatrix(DictionaryOfFormats, isSymmetric), SparseRowColumnMatrix.requiredFileNames);

            ISolver Msg = new MSGSolver();
            ISolver Los = new LOSSolver();
            ISolver Bsg = new BSGStabSolve();
            ISolver Jac = new Jacobi();
            ISolver Zeid = new Seidel();


            RegisterSolverClass("Метод сопряжённых градиентов", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Msg.Solve(Prec, ObjectOfIMatrix, FileLoadForm.F, FileLoadForm.X0, Form1.accurent, Form1.maxiter, Log));
            RegisterSolverClass("Локально-оптимальная схема", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Los.Solve(Prec, ObjectOfIMatrix, FileLoadForm.F, FileLoadForm.X0, Form1.accurent, Form1.maxiter, Log));
            RegisterSolverClass("Метод Якоби", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) =>  Jac.Solve(Prec, ObjectOfIMatrix, FileLoadForm.F, FileLoadForm.X0, Form1.accurent, Form1.maxiter, Log));
            RegisterSolverClass("Метод Зейделя", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) =>  Zeid.Solve(Prec, ObjectOfIMatrix, FileLoadForm.F, FileLoadForm.X0, Form1.accurent, Form1.maxiter, Log));
            RegisterSolverClass("Метод бисопряжённых градиентов", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Bsg.Solve(Prec, ObjectOfIMatrix, FileLoadForm.F, FileLoadForm.X0, Form1.accurent, Form1.maxiter, Log));

            //RegisterSolverClass("Метод обобщённых минимальных невязок", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => new SparseRowColumnMatrix.Solver(Prec, ObjectOfIMatrix, Form2.F, Form2.X0, Form1.s_accur_number, Form1.max_iter, Log));
            //Log.Dispose();
        }

        static public void CreateMatrix(string typename)//получаем заполненную матрицу для передачи Solver
        {
            (Func<Dictionary<string, string>, bool, IMatrix>, Dictionary<string, string>) value;
            MatrixTypes.TryGetValue(typename, out value);
            Dictionary<string, string> reer;
            reer = value.Item2;
            name_arr.Clear();
            foreach (string i in reer.Keys)
                name_arr.Add(i);
        }
        static public void Create_Full_Matrix(string typename, bool isSymmetric = false)//получаем заполненную матрицу для передачи Solver
        {
            (Func<Dictionary<string, string>, bool, IMatrix>, Dictionary<string, string>) value;
            MatrixTypes.TryGetValue(typename, out value);

            ObjectOfIMatrix = value.Item1(DictionaryOfFormats, isSymmetric);
        }
        static public void CreateSolver(object typenameOb)//получаем заполненную матрицу для передачи Solver
        {

            string typename = typenameOb as string;
            Func<IPreconditioner, IMatrix, IVector, IVector, double, int, ILogger, IVector> value;
            SolverTypes.TryGetValue(typename, out value);

            FileLogger f = null;
            Result = value(Prec, ObjectOfIMatrix, FileLoadForm.F, FileLoadForm.X0, Form1.accurent, Form1.maxiter, f);
        }

        static public void CreatePrecond(object typenameOb)//получаем заполненную матрицу для передачи Solver
        {
            string typename = typenameOb as string;
            Func<IPreconditioner> value;
            PrecondTypes.TryGetValue(typename, out value);

            Prec = value();
        }

        // Мы передаем симметричность/ несимметричность
        public static bool Get_format()
        {
            return Form1.property_matr;
        }

    }
}
