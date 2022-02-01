using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.PSOSolvers  //Ceci est un test
{
    public class PSOSolver1 : Sudoku.Shared.ISolverSudoku //Nous commencerons par coder une classe PSOSolver1 qui fera parti de l'un de nos solvers 
    {
       public Shared.GridSudoku Solve(Shared.GridSudoku s) //Le solveur reçoit la grille Sudoku que la console nous donne au départ
       {


            return s;
       }
    }
}
 
    //public class PSOSolvers1 : PythonSolverBase
    //{


    //    public override Shared.GridSudoku Solve(Shared.GridSudoku s)
    //    {

    //        //using (Py.GIL())
    //        //{
    //        // create a Python scope
    //        using (PyModule scope = Py.CreateScope())
    //        {
    //            // convert the Person object to a PyObject
    //            PyObject pyCells = s.Cellules.ToPython();

    //            // create a Python variable "person"
    //            scope.Set("instance", pyCells);

    //            // the person object may now be used in Python
    //            string code = Resources.PSOSolvers1_py;
    //            scope.Exec(code);
    //            var result = scope.Get("r");
    //            var managedResult = result.As<int[][]>();
    //            //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
    //            return new Shared.GridSudoku() { Cellules = managedResult };
    //        }
    //        //}

    //    }

    //    protected override void InitializePythonComponents()
    //    {
    //        //InstallPipModule("z3-solver"); A adapter au modèle PSO
    //        base.InitializePythonComponents();
    //    }
    //}


