using System;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.CSPSolvers
{
    public class CSPSolverVide : Sudoku.Shared.ISolverSudoku
    {
        public GridSudoku Solve(GridSudoku s)
        {
            return s;
        }
    
         public GridSudoku Solve(GridSudoku s)
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

                // the person object may now be used in Python
                string code = Resources.sudokucsp_py;
                scope.Exec(code);
                var result = scope.Get("solvedSudoku");
                var toReturn = result.As<Shared.GridSudoku>();
                return toReturn;
                
            }
            //}

        }
        
        
    }

    
}
