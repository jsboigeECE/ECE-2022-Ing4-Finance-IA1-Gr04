using Sudoku.Shared;
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

            foreach (string val in solution.Values)
            {             
                lstval.Add(val);
            }
            int compter = 0;
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    s.Cellules[i][j] = Convert.ToInt32(lstval[compter]);
                    compter++;
                }
            }         

            return s;
        }


    }
}

