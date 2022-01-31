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
       /* public GridSudoku Solve(GridSudoku s)
        {
            return s;
        }*/
    
         public override Shared.GridSudoku Solve(Shared.GridSudoku s)
        { 
            
            // create a Python scope
            
            using (PyModule scope = Py.CreateScope())
            {
                // convert the Person object to a PyObject
                PyObject pySudoku = s.ToPython();
               
                // create a Python variable "person"
                scope.Set("sudoku", pySudoku);

               Console.WriteLine(" Ca bloque après !!");
                // the person object may now be used in Python
                string code = Resources.MainGui_py;
                scope.Exec(code);

                Console.WriteLine("On a executer le fichier MainGui_py!");

                //!!!!!!!!!!!!!! .Get()-> notre nom de variable à recup !!!!!!!!!!!!!
                var result = scope.Get("self");
                var toReturn = result.As<Shared.GridSudoku>();

                Console.WriteLine("resulat!");
                return toReturn;
                
            }
            //}

        }
        protected override void InitializePythonComponents()
        {
            //InstallPipModule("z3-solver"); A adapter au modèle PSO
            base.InitializePythonComponents();
        }

/*
        public GridSudoku test(GridSudoku s)
        {
            
            sudooo = SudokuCSP(s);


        }
        */

       
        
    }
    
     
}
