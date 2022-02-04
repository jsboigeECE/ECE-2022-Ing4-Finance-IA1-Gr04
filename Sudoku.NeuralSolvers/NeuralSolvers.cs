using Sudoku.Shared;

using System;
using Python.Runtime;
using System.IO;
using System.Linq;
using System.Globalization;

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

                var modelPath = Path.Combine(Environment.CurrentDirectory, @"Resources\train_model.h5");
                //Transformation du chemin du modèle en python
                PyObject pyModelPath = modelPath.ToPython();
                //assignation à une variable pour le script
                scope.Set("modelPath", pyModelPath);

                //Execution du script
                //string code = Resources.neuralnets_py;
                string code = Resources.ModelLoad_py;
                scope.Exec(code);

                //PyObject resultat = pyModelPath.ToPython();
                //assignation à une variable pour le script
                //scope.Set("resultat", pyModelPath);
                //Récupération du sudoku résolu
                var result = scope.Get("solvedsudoku");
                //var result = scope.Get("sudoku");
                var managedResult = result.As<object[][]>()
                    .Select(row => row.Select(cell => int.Parse(cell.ToString(), CultureInfo.InvariantCulture)).ToArray()).ToArray();
                //var toReturn = result.As<Shared.GridSudoku>();
                s.Cellules=managedResult;
                return s;
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
