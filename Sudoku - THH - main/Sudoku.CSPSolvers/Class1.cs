using System;
using Sudoku.Shared;

namespace Sudoku.CSPSolvers
{
    public class CSPSolverVide : Sudoku.Shared.ISolverSudoku
    {
        public GridSudoku Solve(GridSudoku s)
        {
            return s;
        }
    }
}
