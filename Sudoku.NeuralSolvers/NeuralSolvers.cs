using Sudoku.Shared;

using System;
using Python.Runtime;
using System.IO;


namespace Sudoku.NeuralSolvers
{
    public class NeuralSolvers : Sudoku.Shared.PythonSolverBase
    {
        public override GridSudoku Solve(GridSudoku s)
        {
            //System.Diagnostics.Debugger.Break();

            //For some reason, the Benchmark runner won't manage to get the mutex whereas individual execution doesn't cause issues
            //using (Py.GIL())
            //{
            // create a Python scope
            using (PyModule scope = Py.CreateScope())
           {
                // convert the Person object to a PyObject
                PyObject pySudoku = s.ToPython();
                // create a Python variable "person"
                scope.Set("sudoku", pySudoku);

                var modelPath = Path.Combine(Environment.CurrentDirectory, @"Resources\train_model_test2.h5");
                PyObject pyModelPath = modelPath.ToPython();
                scope.Set("modelPath", pyModelPath);

                // the person object may now be used in Python
                string code = Resources.neuralnets_py;
                scope.Exec(code);
                var result = scope.Get("solvedSudoku");
                var toReturn = result.As<Shared.GridSudoku>();
                return toReturn;
            }
        }

        

        protected override void InitializePythonComponents()
        {
            InstallPipModule("tensorflow");
            InstallPipModule("pandas");
            InstallPipModule("numpy");

            base.InitializePythonComponents();
        }

    }




}
