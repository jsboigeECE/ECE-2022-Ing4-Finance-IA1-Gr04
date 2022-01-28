using System;
using Sudoku.Shared;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.ConstraintSolver;


namespace Sudoku.ORToolsSolvers
{

    public class ORToolsSolver1 : Sudoku.Shared.ISolverSudoku
    {

        public GridSudoku Solve(GridSudoku s)
        {

            return s;

        }

    }

}
