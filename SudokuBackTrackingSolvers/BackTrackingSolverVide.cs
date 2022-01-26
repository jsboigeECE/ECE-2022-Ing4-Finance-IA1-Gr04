using Sudoku.Shared;
using System;

namespace SudokuBackTrackingSolvers
{
    public class BackTrackingSolverVide : Sudoku.Shared.ISolverSudoku
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
		public GridSudoku Solve(GridSudoku s)
        {
            return s;
        }
    }
}
