using System;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.PSOSolvers
{
    //public class PSOSolver1 : Sudoku.Shared.ISolverSudoku

    //{
    //    public GridSudoku Solve(GridSudoku s)
    //    {
    //        return s;
    //    }
    //}

    public class PSOSolvers1 : PythonSolverBase
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
                string code = Resources.PSOSolvers1_py;
                scope.Exec(code);
                var result = scope.Get("r");
                var managedResult = result.As<int[][]>();
                //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
                return new Shared.GridSudoku() { Cellules = managedResult };
            }
            //}

        }

        protected override void InitializePythonComponents()
        {
            //InstallPipModule("z3-solver"); A adapter au modèle PSO
            base.InitializePythonComponents();
        }



    }
}

