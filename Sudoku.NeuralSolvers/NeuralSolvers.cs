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
                //Transformation du sudoku en python
                PyObject pySudoku = s.ToPython();
                //assignation à une variable pour le script
                scope.Set("sudoku", pySudoku);

                var modelPath = Path.Combine(Environment.CurrentDirectory, @"Resources\train_model_test2.h5");
                //Transformation du chemin du modèle en python
                PyObject pyModelPath = modelPath.ToPython();
                //assignation à une variable pour le script
                scope.Set("modelPath", pyModelPath);

                //Execution du script
                //string code = Resources.neuralnets_py;
                string code = Resources.ModelLoad_py;
                scope.Exec(code);
                
                //Récupération du sudoku résolu
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
