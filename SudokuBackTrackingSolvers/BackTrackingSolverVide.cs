using Sudoku.Shared;
using System;

namespace SudokuBackTrackingSolvers
{
    public class BackTrackingSolverVide : Sudoku.Shared.ISolverSudoku
    {
        public GridSudoku Solve(GridSudoku s)
        {
            return s;
        }
    }
}
