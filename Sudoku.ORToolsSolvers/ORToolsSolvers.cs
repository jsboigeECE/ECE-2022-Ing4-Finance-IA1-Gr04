using System;
using Sudoku.Shared;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.ConstraintSolver;
using Google.OrTools.LinearSolver;


namespace Sudoku.ORToolsSolvers
{

    public class ORToolsConstraintSolver : ISolverSudoku
    {

        public Shared.GridSudoku Solve(Shared.GridSudoku s1)
        {

            Google.OrTools.ConstraintSolver.Solver solver = new Google.OrTools.ConstraintSolver.Solver("Sudoku");


            int cell_size = 3;
            IEnumerable<int> CELL = Enumerable.Range(0, cell_size);
            int n = cell_size * cell_size;
            IEnumerable<int> RANGE = Enumerable.Range(0, n);


            int[][] grille = s1.Cellules;

            int[,] initial_grid = grille.To2D();

            IntVar[,] grid = solver.MakeIntVarMatrix(n, n, 1, 9, "grid");
            IntVar[] grid_flat = grid.Flatten();

            foreach (int i in RANGE)
            {
                foreach (int j in RANGE)
                {
                    if (initial_grid[i, j] > 0)
                    {
                        solver.Add(grid[i, j] == initial_grid[i, j]);
                    }
                }
            }
            foreach (int i in RANGE)
            {

                // rows
                solver.Add((from j in RANGE
                            select grid[i, j]).ToArray().AllDifferent());

                // cols
                solver.Add((from j in RANGE
                            select grid[j, i]).ToArray().AllDifferent());

            }

            // cells
            foreach (int i in CELL)
            {
                foreach (int j in CELL)
                {
                    solver.Add((from di in CELL
                                from dj in CELL
                                select grid[i * cell_size + di, j * cell_size + dj]
                                 ).ToArray().AllDifferent());
                }
            }


            DecisionBuilder db = solver.MakePhase(grid_flat,
                                                  Google.OrTools.ConstraintSolver.Solver.INT_VAR_SIMPLE,
                                                  Google.OrTools.ConstraintSolver.Solver.INT_VALUE_SIMPLE);

            solver.NewSearch(db);

            while (solver.NextSolution())
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        //Console.Write("{0} ", grid[i, j].Value());
                        s1.Cellules[i][j] = (int)grid[i, j].Value();
                    }
                    //Console.WriteLine();
                }

                //Console.WriteLine();
            }

            //Console.WriteLine("\nSolutions: {0}", solver.Solutions());
            //Console.WriteLine("WallTime: {0}ms", solver.WallTime());
            //Console.WriteLine("Failures: {0}", solver.Failures());
            //Console.WriteLine("Branches: {0} ", solver.Branches());

            //s.Cellules = grid.ToJaggedArray().Select(varrow => varrow.Select(varcells => (int)varcells.Value()).ToArray()).ToArray();
            //solver.EndSearch();
             


            return s1;

        }

    }



    public class ORToolsIntegerOptimizationSolver : ISolverSudoku
    {

        public Shared.GridSudoku Solve(Shared.GridSudoku s2)
        {

            Google.OrTools.LinearSolver.Solver solver = Google.OrTools.LinearSolver.Solver.CreateSolver("Sudoku");


            int cell_size = 3;
            IEnumerable<int> CELL = Enumerable.Range(0, cell_size);
            int n = cell_size * cell_size;
            IEnumerable<int> RANGE = Enumerable.Range(0, n);


            int[][] grille = s2.Cellules;

            int[,] initial_grid = grille.To2D();





            return s2;

        }

    }

}
