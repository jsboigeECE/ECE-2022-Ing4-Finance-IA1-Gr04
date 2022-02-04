using System;
using Sudoku.Shared;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.ConstraintSolver;
using Google.OrTools.LinearSolver;


namespace Sudoku.ORToolsSolvers
{

    // 1st solver : Constraint Solver
    public class ORToolsConstraintSolver : ISolverSudoku
    {

        public Shared.GridSudoku Solve(Shared.GridSudoku s)
        {
            // Declaration of the solver
            Google.OrTools.ConstraintSolver.Solver solver = new Google.OrTools.ConstraintSolver.Solver("Sudoku");

            // Definition of the variables
            int cell_size = 3;
            IEnumerable<int> CELL = Enumerable.Range(0, cell_size);
            int n = cell_size * cell_size;
            IEnumerable<int> RANGE = Enumerable.Range(0, n);


            int[][] grille = s.Cellules;

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

            // Create the constraint
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

            // Call the solver
            DecisionBuilder db = solver.MakePhase(grid_flat,
                                                  Google.OrTools.ConstraintSolver.Solver.INT_VAR_SIMPLE,
                                                  Google.OrTools.ConstraintSolver.Solver.INT_VALUE_SIMPLE);

            // Print the solution
            solver.NewSearch(db);

            while (solver.NextSolution())
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        s.Cellules[i][j] = (int)grid[i, j].Value();
                    }

                }

            }
           
            return s;

        }

    }


    // 2nd solver : Integer Optimzation Solver
    public class ORToolsIntegerOptimizationSolver : ISolverSudoku
    {

        public Shared.GridSudoku Solve(Shared.GridSudoku s)
        {
            // Declaration of the solver
            Google.OrTools.LinearSolver.Solver solver = Google.OrTools.LinearSolver.Solver.CreateSolver("Sudoku");

            // Definition of the variables
            int cell_size = 3;
            IEnumerable<int> CELL = Enumerable.Range(0, cell_size);
            int n = cell_size * cell_size;
            IEnumerable<int> RANGE = Enumerable.Range(0, n);


            int[][] grille = s.Cellules;

            int[,] initial_grid = grille.To2D();


            Variable[,] grid = solver.MakeIntVarMatrix(n, n, 1, 9, "grid");
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

            // Definition of the constraints
            foreach (int i in RANGE)
            {
                
            }


            // Definition of the objective


            // Call the solver
            Google.OrTools.LinearSolver.Solver.ResultStatus resultStatus = solver.Solve();


            // Print the solution



            return s;

        }

    }

}
