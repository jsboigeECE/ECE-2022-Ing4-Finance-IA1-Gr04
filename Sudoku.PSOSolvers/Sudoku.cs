using System;
using System.Text;
namespace Sudoku.PSOSolvers
{
    public class Sudoku
    {
        private Sudoku(int[,] cellValues)
        {
            CellValues = cellValues;
        }

        public static Sudoku New(int[,] cellValues)
        {
            return new Sudoku(PSOSolvers1.DuplicateMatrix(cellValues));
        }

        public static Sudoku New(Sudoku sudoku)
        {
            return new Sudoku(PSOSolvers1.DuplicateMatrix(sudoku.CellValues));
        }

        public int[,] CellValues { get; }

        public int Error
        {
            get
            {
                return CountErrors(true) + CountErrors(false);

                int CountErrors(bool countByRow)
                {
                    var errors = 0;
                    for (var i = 0; i < PSOSolvers1.taille; ++i)
                    {
                        var counts = new int[PSOSolvers1.taille];
                        for (var j = 0; j < PSOSolvers1.taille; ++j)
                        {
                            var cellValue = countByRow ? CellValues[i, j] : CellValues[j, i];
                            ++counts[cellValue - 1];
                        }

                        for (var k = 0; k < PSOSolvers1.taille; ++k)
                        {
                            if (counts[k] == 0)
                                ++errors;
                        }
                    }

                    return errors;
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            for (var r = 0; r < PSOSolvers1.taille; ++r)
            {
                if (r == 3 || r == 6) stringBuilder.AppendLine();
                for (var c = 0; c < PSOSolvers1.taille; ++c)
                {
                    if (c == 3 || c == 6) stringBuilder.Append(" ");
                    if (CellValues[r, c] == 0)
                        stringBuilder.Append(" _");
                    else
                        stringBuilder.Append(" " + CellValues[r, c]);
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

    }
}
