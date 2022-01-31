﻿using Sudoku.Shared;
using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Sudoku.LaNorvigSolveur
{
    class LaNorvigSolveur : Sudoku.Shared.ISolverSudoku
    {
        public GridSudoku Solve(GridSudoku s)
        {
            var sAuBonFormat = s.Cellules.Select(r => r.Select(c => c == 0 ? "." : c.ToString())
            .Aggregate("", (s1, s2) => s1 + s2, s => s))
                .Aggregate("", (s1, s2) => s1 + s2, s => s);
            var sudokuDICO = LinqSudokuSolver.parse_grid(sAuBonFormat);

            var solution = LinqSudokuSolver.search(sudokuDICO);
            List<String> lstval = new List<string>();

            //Affichage de toute les valeurs et des clés du dictionnaires

            /*foreach (KeyValuePair<string, string> kvp in solution)
            {
                string valeur = Convert.ToString(kvp.Value);
                lstval.Add(valeur);
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            Console.Write(lstval);*/


            //Affichage d'une valeur en fonction de sa clé 
            /*for(int i = 0; i < 9; i++) 
            { 
                Console.WriteLine(solution["A1"]);
                
            }*/

            
            foreach (string val in solution.Values)
            {             
                Console.Write(val + "\n");       
            }
            Console.WriteLine("\n");

            

            return s;
        }


    }
}
