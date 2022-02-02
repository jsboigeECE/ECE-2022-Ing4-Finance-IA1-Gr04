using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.CSPSolvers
{
    public class CSPSolverVide : PythonSolverBase
    {
        public override GridSudoku Solve(GridSudoku s)
        {
            using (Py.GIL())
            {
                using (PyModule scope = Py.CreateScope())
                {
                    PyObject pySudoku = s.ToPython();

                    var inf = 0;
                    Console.WriteLine("\nChoose your method: \n");
                    Console.WriteLine("1 - No inference \n2 - forward_checking \n3 - MAC");
                    inf = int.Parse(Console.ReadLine());
                    
                    //on recupere le choix de l inference de l utilisateur
                    scope.Set("inference", inf);
                    scope.Set("sudoku", pySudoku);

                    string code = Resources.MainGui_py;
                    scope.Exec(code);

                    Console.WriteLine("On a executer le fichier MainGui_py!");

                    var result = scope.Get("sudoku");
                    Console.WriteLine("Sudoku: " + result);
                    var toReturn = result.As<Shared.GridSudoku>();

                    Console.WriteLine("resulat!");
                    return toReturn;
                }
            }
        }

        protected override void InitializePythonComponents()
        {
            //InstallPipModule("z3-solver"); A adapter au modèle PSO
            base.InitializePythonComponents();
        }        
    }
}
