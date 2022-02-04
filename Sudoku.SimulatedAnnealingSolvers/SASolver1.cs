using System;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.SimulatedAnnealingSolvers
{
    public class SolversSimulatedAnnealing2 : PythonSolverBase
    {


        public override Shared.GridSudoku Solve(Shared.GridSudoku s)
        {

            //using (Py.GIL())
            //{
            // create a Python scope
            using (PyModule scope = Py.CreateScope())
            {
                // convert the Person object to a PyObject
                PyObject pyCells = s.Cellules.ToPython();

                // create a Python variable "person"
                scope.Set("instance", pyCells);

                // the person object may now be used in Python
                string code = Resources.SASolvers2_py;
                scope.Exec(code);
                var result = scope.Get("solution");
                var managedResult = result.As<int[][]>();
                //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
                return new Shared.GridSudoku() { Cellules = managedResult };
            }
            //}

        }

        protected override void InitializePythonComponents()
        {
            InstallPipModule("numpy");
            base.InitializePythonComponents();
        }



    }

}
