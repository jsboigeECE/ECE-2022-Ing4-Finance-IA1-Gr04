using Sudoku.Shared;
using System.Text;
using System.Linq;

namespace Sudoku.LaNorvigSolveur
{
    class LaNorvigSolveur : Sudoku.Shared.ISolverSudoku
    {
        public GridSudoku Solve(GridSudoku s)
        {
            var sAuBonFormat = s.Cellules.Select(r => r.Select(c => c == 0 ? "." : c.ToString())
            .Aggregate("", (s1, s2) => s1 + s2, s => s))
                .Aggregate("", (s1, s2) => s1 + s2, s => s);
           var sudokuDICO   = LinqSudokuSolver.parse_grid(sAuBonFormat);
           var solution= LinqSudokuSolver.search(sudokuDICO);
            return s;
        }


    }
}

