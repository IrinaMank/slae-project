﻿using System;
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
        public static Dictionary<string, string> DictionaryOfFormats = FileLoadForm.filenames_format;//словарь путей до массивов
        static public Dictionary<string, (Func<Dictionary<string, string>, bool, IMatrix>, Dictionary<string, string>)> MatrixTypes = new Dictionary<string, (Func<Dictionary<string, string>, bool, IMatrix>, Dictionary<string, string>)>();
        public static Dictionary<string, string> reer;
        public static IMatrix ObjectOfIMatrix;
        public static IVector Result;
        public static IVector RightVector;
        public static IVector X0;
        public static ILogger Log;
        public static List<double> Residual = new List<double>();//Невязка
        public static int MaxIter;
        public static double Accuracy;
        public static List<string> name_arr = new List<string>();
        public static IPreconditioner Prec = new NoPreconditioner();
        static public Dictionary<string, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, ILogger, IVector>> SolverTypes = new Dictionary<string, Func<IPreconditioner, IMatrix, IVector, IVector, double, int, ILogger, IVector>>();
        static public Dictionary<string, Func<IPreconditioner>> PrecondTypes = new Dictionary<string, Func<IPreconditioner>>();
        static public Dictionary<string, Func<ILogger>> LoggerTypes = new Dictionary<string, Func<ILogger>>();

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

        static public void RegisterLoggerClass(string Name, Func<ILogger> Creator1)
        {
                LoggerTypes.Add(Name, Creator1);  
        }

        public Factory()
        {
            //эти изменения с предобуславливателем внесла Ира и она не уверена, что все сделала верно. Но все работает. Вроде.
            RegisterPrecondClass("Без предобуславливания", () => new NoPreconditioner());
            RegisterPrecondClass("Диагональное", () => new DiagonalPreconditioner(ObjectOfIMatrix));
            //RegisterPrecondClass("Методом Зейделя", () => new (ObjectOfIMatrix));
            RegisterPrecondClass("LU-разложение", () => new LUPreconditioner(ObjectOfIMatrix));

            RegisterMatrixClass("Плотный", (Dictionary<string, string> DictionaryOfFormats, bool isSymmetric) => new DenseMatrix(DictionaryOfFormats, isSymmetric), DenseMatrix.requiredFileNames);
            RegisterMatrixClass("Координатный", (Dictionary<string, string> DictionaryOfFormats, bool isSymmetric) => new CoordinateMatrix(DictionaryOfFormats, isSymmetric), CoordinateMatrix.requiredFileNames);
            RegisterMatrixClass("Строчный", (Dictionary<string, string> DictionaryOfFormats, bool isSymmetric) => new SparseRowMatrix(DictionaryOfFormats, isSymmetric),SparseRowMatrix.requiredFileNames);
            RegisterMatrixClass("Строчно - столбцовый", (Dictionary<string, string> DictionaryOfFormats, bool isSymmetric) => new SparseRowColumnMatrix(DictionaryOfFormats, isSymmetric), SparseRowColumnMatrix.requiredFileNames);

            ISolver Msg = new MSGSolver();
            ISolver Los = new LOSSolver();
            ISolver Bsg = new BSGStabSolve();
            ISolver Jacoby = new Jacobi();
            ISolver Zeid = new Seidel();

            RegisterSolverClass("Метод сопряжённых градиентов", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Msg.Solve(Prec, ObjectOfIMatrix, RightVector, X0, Accuracy, MaxIter, Log));
            RegisterSolverClass("Локально-оптимальная схема", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Los.Solve(Prec, ObjectOfIMatrix, RightVector, X0, Accuracy, MaxIter, Log));
            RegisterSolverClass("Метод Якоби", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Jacoby.Solve(Prec, ObjectOfIMatrix, RightVector, X0, Accuracy, MaxIter, Log));
            RegisterSolverClass("Метод Зейделя", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Zeid.Solve(Prec, ObjectOfIMatrix, RightVector, X0, Accuracy, MaxIter, Log));
            RegisterSolverClass("Метод бисопряжённых градиентов", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, ILogger g) => Bsg.Solve(Prec, ObjectOfIMatrix, RightVector, X0, Accuracy, MaxIter, Log));

            RegisterLoggerClass("Консоль", () => new ConsoleLogger());
            FileLogger fl = new FileLogger();
            RegisterLoggerClass("Файл log.txt", () => fl.returnThis());
            //RegisterSolverClass("Метод обобщённых минимальных невязок", (IPreconditioner a, IMatrix b, IVector c, IVector d, double e, int f, FileLogger g) => new SparseRowColumnMatrix.Solver(Prec, ObjectOfIMatrix, FileLoadForm.F, FileLoadForm.X0, Form1.s_accur_number, Form1.max_iter, Log));
            //Log.Dispose();
        }

        static public void CreateMatrix(string typename)//получаем заполненную матрицу для передачи Solver
        {
            (Func<Dictionary<string, string>, bool, IMatrix>, Dictionary<string, string>) value;
            MatrixTypes.TryGetValue(typename, out value);           
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

            try
            {
                switch (ObjectOfIMatrix.CheckCompatibility(Factory.RightVector))
                {
                    case MatrixConstants.SLAE_INCOMPATIBLE:
                        {
                            throw new Matrix.MatrixExceptions.SlaeNotCompatipableException("СЛАУ несовместна. Решения не существует. ");
                        };
                    case MatrixConstants.SLAE_MORE_ONE_SOLUTION:
                        {
                            System.Windows.Forms.MessageBox.Show("СЛАУ имеет более одного решения. Будет найдено одно из них. ",
                                    "Предупреждение",
                                    System.Windows.Forms.MessageBoxButtons.OK,
                                    System.Windows.Forms.MessageBoxIcon.Asterisk);
                            break;
                        }
                }
                Result = value(Prec, ObjectOfIMatrix, RightVector, X0, Accuracy, MaxIter, Log);
                System.Media.SoundPlayer sp = new System.Media.SoundPlayer(Properties.Resources.ya);
                sp.Play();
            }
            catch (Solver.CantSolveException a)
            {
                var result = System.Windows.Forms.MessageBox.Show("Вы женского пола?", "Важный вопрос", System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question);
                string mesg = "Увы, месье, ";
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    mesg = "Увы, мадам, ";
                }
                System.Windows.Forms.MessageBox.Show(mesg + a.Message,
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Stop);
            }
            catch (Matrix.MatrixExceptions.SlaeNotCompatipableException a)
            {
                System.Windows.Forms.MessageBox.Show(a.Message,
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Stop);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Решение СЛАУ не может быть получено с помощью данного метода.",
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Stop);
            }
            finally
            {
                Log.Dispose();
            }
        }

        static public void CreatePrecond(object typenameOb)//получаем заполненную матрицу для передачи Solver
        {
            string typename = typenameOb as string;
            Func<IPreconditioner> value;
            try
            {
                PrecondTypes.TryGetValue(typename, out value);
                Prec = value();
            }
            catch (slae_project.Matrix.MatrixExceptions.LUFailException a)
            {
                System.Windows.Forms.MessageBox.Show(a.Message + "\nРешение будет производиться без предобуславливания.",
                    "Предупреждение",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Asterisk);
                PrecondTypes.TryGetValue("Без предобуславливания", out value);
                Prec = value();
            }

        }

        static public void CreateLogger(object typenameOb)
        {
            string typename = typenameOb as string;
            Func<ILogger> value;
            LoggerTypes.TryGetValue(typename, out value);
            Log = value();
        }


        // Мы передаем симметричность/ несимметричность
        public static bool Get_format()
        {
            return Form1.property_matr;
        }

    }
}