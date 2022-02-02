using Sudoku.Shared;
using SudokuSolver;

namespace Sudoku.GraphColoringSolvers.GraphColoringSolvers
{
    /// <summary>
    /// Ce solver utilise le code du dépôt suivant: https://github.com/MostafaEissa/SudokuSolver
    /// Ce code est présenté dans l'article suivant: https://www.codeproject.com/Articles/801268/A-Sudoku-Solver-using-Graph-Coloring
    /// </summary>
    public class GraphColoringCodeProjectSolver : ISolverSudoku
    {
        public GridSudoku Solve(GridSudoku s)
        {
            var puzzle = new SudokuPuzzle();
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 9; colIndex++)
                {
                    var cellValue = s.Cellules[rowIndex][colIndex];
                    if (cellValue!=0)
                    {
                        puzzle[rowIndex+1, colIndex+1] = cellValue;
                    }
                }
            }

            if (puzzle.Solve())
            {
                for (int rowIndex = 0; rowIndex < 9; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < 9; colIndex++)
                    {
                        s.Cellules[rowIndex][colIndex] = puzzle[rowIndex+1, colIndex+1]??0;
                    }
                }

               
            }
            return s;
        }
    }
}