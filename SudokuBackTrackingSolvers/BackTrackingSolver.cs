using Sudoku.Shared;
using System;

namespace SudokuBackTrackingSolvers
{
	public class BackTrackingSolver : Sudoku.Shared.ISolverSudoku
	{

		public static bool isSafe(GridSudoku s,
						int row, int col,
						int num)
		{

			// Row has the unique (row-clash)
			for (int d = 0; d < s.Cellules.GetLength(0); d++)
			{

				// Check if the number
				// we are trying to
				// place is already present in
				// that row, return false;
				if (s.Cellules[row][d] == num)
				{
					return false;
				}
			}

			// Column has the unique numbers (column-clash)
			for (int r = 0; r < s.Cellules.GetLength(0); r++)
			{

				// Check if the number
				// we are trying to
				// place is already present in
				// that column, return false;
				if (s.Cellules[r][col] == num)
				{
					return false;
				}
			}

			// corresponding square has
			// unique number (box-clash)
			int sqrt = (int)Math.Sqrt(s.Cellules.GetLength(0));
			int boxRowStart = row - row % sqrt;
			int boxColStart = col - col % sqrt;

			for (int r = boxRowStart;
				r < boxRowStart + sqrt; r++)
			{
				for (int d = boxColStart;
					d < boxColStart + sqrt; d++)
				{
					if (s.Cellules[r][d] == num)
					{
						return false;
					}
				}
			}

			// if there is no clash, it's safe
			return true;
		}

		public bool Solve(GridSudoku s)
		{
			int row = -1;
			int col = -1;
			bool isEmpty = true;
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (s.Cellules[i][j] == 0)
					{
						row = i;
						col = j;

						// We still have some remaining
						// missing values in Sudoku
						isEmpty = false;
						break;
					}
				}
				if (!isEmpty)
				{
					break;
				}
			}

			// no empty space left
			if (isEmpty)
			{
				return true;
			}

			// else for each-row backtrack
			for (int num = 1; num <= 9; num++)
			{
				if (isSafe(s, row, col, num))
				{
					s.Cellules[row][col] = num;
					if (Solve(s))
					{

						// Print(board, n);
						return true;

					}
					else
					{

						// Replace it
						s.Cellules[row][col] = 0;
					}
				}
			}
			return false;

		}

            GridSudoku ISolverSudoku.Solve(GridSudoku s)
        {
 
                     if (Solve(s))
                     {
 
                            return s;
                     }
                     else
                     {
                            Console.Write("No solution");
                            return s;
							
 
                     }
              }
    }
}
