using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Sudoku.Shared;
using System.Linq;

namespace Sudoku.PSOSolvers  //Ceci est un test
{
    public class PSOSolvers1 : ISolverSudoku //Nous commencerons par coder une classe PSOSolver1 qui fera parti de l'un de nos solvers 
    {
        public const int taille = 9; //Taille de la grille Sudoku
        public const int taille_block = 3;  //Taille de la sous-grille 3x3
        static Random rnd = new Random(0);  // Initialisation d'une variable aléatoire qui nous permettra d'en initialiser d'autres dans la suite du code
        
        public enum OrganismType //La classe OrganismType est la classe qui représentera le type de particule qu'on aura dans l'algorithme PSO 
        {
            Worker,
            Explorer
        }

        public class Organism //Cette classe Organism représente la particule de notre algorithme PSO
        {
            public OrganismType Type { get; } //Il faut faire un getter pour accéder et/ou modifier ces 4 variables à gauche car leurs attributs sont privés
            public int[,] Matrix { get; set; }
            public int Error { get; set; }
            public int Age { get; set; }

            public Organism(OrganismType type, int[,] m, int error, int age) //Constructeur par défaut de notre classe organisme 
            {                                                                //Ce constructeur nous permettra d'initialiser l'instance de la classe Organisme
                Type = type;
                Error = error;
                Age = age;
                Matrix = PSOSolvers1.DuplicateMatrix(m);
            }
        }

        private Sudoku SolveInternal(Sudoku sudoku, int numOrganisms, int maxEpochs) //Cette méthode représentera le coeur de notre Solver 
        {                                                                        //Elle recevra en paramètre la grille Sudoku, le nombre d'Organismes et maxEpochs
            var numberOfWorkers = (int)(numOrganisms * 0.90); //Le nombre de particules de type Workers sera initialisé à 90 % du nombre total de particules 
            var hive = new Organism[numOrganisms]; //Initialisation de notre Swarm en fonction du nombre d'organismes (Particules)

            var bestError = int.MaxValue; //On initialise l'entier de la meilleure erreur à une valeur maximale 
            Sudoku bestSolution = null; //La meilleure solution sera initialisée à null

            for (var i = 0; i < numOrganisms; ++i) //On va parcourir toutes nos Organismes (particules)
            {
                var organismType = i < numberOfWorkers
                 ? OrganismType.Worker
                  : OrganismType.Explorer;

                var randomSudoku = Sudoku.New(PSOSolvers1.RandomMatrix(rnd, sudoku.CellValues));
                var err = randomSudoku.Error;

                hive[i] = new Organism(organismType, randomSudoku.CellValues, err, 0);

                if (err >= bestError) continue;
                bestError = err;
                bestSolution = Sudoku.New(randomSudoku);
            } //jusqu'ici

            var epoch = 0;
            while (epoch < maxEpochs)
            {
                if (epoch % 1000 == 0)
                    Console.WriteLine($"Epoch: {epoch}, Best error: {bestError}");

                if (bestError == 0)
                    break;

                for (var i = 0; i < numOrganisms; ++i)
                {
                    if (hive[i].Type == OrganismType.Worker)
                    {
                        var neighbor = PSOSolvers1.NeighborMatrix(rnd, sudoku.CellValues, hive[i].Matrix);
                        var neighborSudoku = Sudoku.New(neighbor);
                        var neighborError = neighborSudoku.Error;

                        var p = rnd.NextDouble();
                        if (neighborError < hive[i].Error || p < 0.001)
                        {
                            hive[i].Matrix = PSOSolvers1.DuplicateMatrix(neighbor);
                            hive[i].Error = neighborError;
                            if (neighborError < hive[i].Error) hive[i].Age = 0;

                            if (neighborError >= bestError) continue;
                            bestError = neighborError;
                            bestSolution = neighborSudoku;
                        }
                        else
                        {
                            hive[i].Age++;
                            if (hive[i].Age <= 1000) continue;
                            var randomSudoku = Sudoku.New(PSOSolvers1.RandomMatrix(rnd, sudoku.CellValues));
                            hive[i] = new Organism(0, randomSudoku.CellValues, randomSudoku.Error, 0);
                        }
                    }
                    else
                    {
                        var randomSudoku = Sudoku.New(PSOSolvers1.RandomMatrix(rnd, sudoku.CellValues));
                        hive[i].Matrix = PSOSolvers1.DuplicateMatrix(randomSudoku.CellValues);
                        hive[i].Error = randomSudoku.Error;

                        if (hive[i].Error >= bestError) continue;
                        bestError = hive[i].Error;
                        bestSolution = randomSudoku;
                    }
                }




                // merge best worker with best explorer into worst worker
                var bestWorkerIndex = 0;
                var smallestWorkerError = hive[0].Error;
                for (var i = 0; i < numberOfWorkers; ++i)
                {
                    if (hive[i].Error >= smallestWorkerError) continue;
                    smallestWorkerError = hive[i].Error;
                    bestWorkerIndex = i;
                }

                var bestExplorerIndex = numberOfWorkers;
                var smallestExplorerError = hive[numberOfWorkers].Error;
                for (var i = numberOfWorkers; i < numOrganisms; ++i)
                {
                    if (hive[i].Error >= smallestExplorerError) continue;
                    smallestExplorerError = hive[i].Error;
                    bestExplorerIndex = i;
                }

                var worstWorkerIndex = 0;
                var largestWorkerError = hive[0].Error;
                for (var i = 0; i < numberOfWorkers; ++i)
                {
                    if (hive[i].Error <= largestWorkerError) continue;
                    largestWorkerError = hive[i].Error;
                    worstWorkerIndex = i;
                }

                var merged = PSOSolvers1.MergeMatrices(rnd, hive[bestWorkerIndex].Matrix, hive[bestExplorerIndex].Matrix);
                var mergedSudoku = Sudoku.New(merged);

                hive[worstWorkerIndex] = new Organism(0, merged, mergedSudoku.Error, 0);
                if (hive[worstWorkerIndex].Error < bestError)
                {
                    bestError = hive[worstWorkerIndex].Error;
                    bestSolution = mergedSudoku;
                }

                ++epoch;
            }

            return bestSolution;
        }



        /// This creates a new random solution and maps that solution onto provided problem
        /// </summary>
        /// <param name="problem">int[][]</param>
        /// <param name="rand">pass in your random object if you got one</param>
        /// <returns></returns>
        public static int[,] RandomMatrix(Random rnd, int[,] problem)
        {
            var result = DuplicateMatrix(problem);

            for (var block = 0; block < taille; ++block)
            {
                var corner = Corner(block);
                var values = Enumerable.Range(1, taille).ToList();

                for (var k = 0; k < values.Count; ++k)
                {
                    var ri = rnd.Next(k, values.Count);
                    var tmp = values[k];
                    values[k] = values[ri];
                    values[ri] = tmp;
                }

                var r = corner.row;
                var c = corner.column;
                for (var i = r; i < r + taille_block; ++i)
                {
                    for (var j = c; j < c + taille_block; ++j)
                    {
                        var value = problem[i, j];
                        if (value != 0)
                            values.Remove(value);
                    }
                }

                var pointer = 0;
                for (var i = r; i < r + taille_block; ++i)
                {
                    for (var j = c; j < c + taille_block; ++j)
                    {
                        if (result[i, j] != 0) continue;
                        var value = values[pointer];
                        result[i, j] = value;
                        ++pointer;
                    }
                }
            }

            return result;
        }


        /// returns the row/column coordinate of the top-left cell in the block identified

        /// <param name="block">index of a 3x3 block</param>
        /// <returns>returns the (row,column) coordinate of the top-left cell</returns>
        public static (int row, int column) Corner(int block)
        {
            int r = -1, c = -1;

            if (block == 0 || block == 1 || block == 2)
                r = 0;
            else if (block == 3 || block == 4 || block == 5)
                r = 3;
            else if (block == 6 || block == 7 || block == 8)
                r = 6;

            if (block == 0 || block == 3 || block == 6)
                c = 0;
            else if (block == 1 || block == 4 || block == 7)
                c = 3;
            else if (block == 2 || block == 5 || block == 8)
                c = 6;

            return (r, c);
        }


        public static int Block(int r, int c)
        {
            if (r >= 0 && r <= 2 && c >= 0 && c <= 2)
                return 0;
            if (r >= 0 && r <= 2 && c >= 3 && c <= 5)
                return 1;
            if (r >= 0 && r <= 2 && c >= 6 && c <= 8)
                return 2;
            if (r >= 3 && r <= 5 && c >= 0 && c <= 2)
                return 3;
            if (r >= 3 && r <= 5 && c >= 3 && c <= 5)
                return 4;
            if (r >= 3 && r <= 5 && c >= 6 && c <= 8)
                return 5;
            if (r >= 6 && r <= 8 && c >= 0 && c <= 2)
                return 6;
            if (r >= 6 && r <= 8 && c >= 3 && c <= 5)
                return 7;
            if (r >= 6 && r <= 8 && c >= 6 && c <= 8)
                return 8;

            throw new Exception("Unable to find Block()");
        }


        public static int[,] NeighborMatrix(Random rnd, int[,] problem, int[,] matrix)
        {
            // pick a random 3x3 block,
            // pick two random cells in block
            // swap values
            var result = DuplicateMatrix(matrix);

            var block = rnd.Next(0, taille); // [0,8]
            var corner = Corner(block);
            var cells = new List<int[]>();
            for (var i = corner.row; i < corner.row + taille_block; ++i)
            {
                for (var j = corner.column; j < corner.column + taille_block; ++j)
                {
                    if (problem[i, j] == 0)
                        cells.Add(new[] { i, j });
                }
            }

            if (cells.Count < 2)
                throw new Exception($"Block {block} doesn't have two values to swap!");

            // pick two. suppose there are 4 possible cells 0,1,2,3
            var k1 = rnd.Next(0, cells.Count); // 0,1,2,3
            var inc = rnd.Next(1, cells.Count); // 1,2,3
            var k2 = (k1 + inc) % cells.Count;

            var r1 = cells[k1][0];
            var c1 = cells[k1][1];
            var r2 = cells[k2][0];
            var c2 = cells[k2][1];

            var tmp = result[r1, c1];
            result[r1, c1] = result[r2, c2];
            result[r2, c2] = tmp;

            return result;
        }

        public int[,] CellValues { get; }



        /// Creates a copy of the provided matrix
        /// <returns>another int[][] with the same values as passed in</returns>
        public static int[,] DuplicateMatrix(int[,] matrix)
        {
            var m = matrix.GetLength(0);
            var n = matrix.GetLength(1);
            var result = CreateMatrix(m, n);
            for (var i = 0; i < m; ++i)
                for (var j = 0; j < n; ++j)
                    result[i, j] = matrix[i, j];

            return result;
        }



        /// Generates a matrix (grid, or array of arrays) representing the sudoku puzzle

        /// <param name="rows">number of rows</param>
        /// <param name="columns">number of columns per row</param>
        /// <returns>an array of int[][]</returns>
        public static int[,] CreateMatrix(int m, int n)
        {
            var result = new int[m, n];
            return result;
        }
        public static int[,] MergeMatrices(Random rnd, int[,] m1, int[,] m2)
        {
            var result = DuplicateMatrix(m1);

            for (var block = 0; block < 9; ++block)
            {
                var pr = rnd.NextDouble();
                if (!(pr < 0.50)) continue;
                var corner = Corner(block);
                for (var i = corner.row; i < corner.row + taille_block; ++i)
                    for (var j = corner.column; j < corner.column + taille_block; ++j)
                        result[i, j] = m2[i, j];
            }

            return result;
        }

        public GridSudoku Solve(GridSudoku s)
        {
            var converted = s.Cellules.To2D();
            var sudoku = Sudoku.New(converted);

            Sudoku solvedSudoku ;

            var numOrganisms = 200;
            //On augmente le nombre d'organismes jusqu'à ce qu'une solution soit trouvée
            do
            {
                solvedSudoku = SolveInternal(sudoku, numOrganisms, 5000);
                numOrganisms *= 2;
            } while (solvedSudoku.Error > 0);

            var solvedCells = solvedSudoku.CellValues.ToJaggedArray();
            s.Cellules = solvedCells;
            return s;
        }

        //GridSudoku ISolverSudoku.Solve(GridSudoku s)
        //{
        //throw new NotImplementedException();
        //}
    } // Program



}




    //public class PSOSolvers1 : PythonSolverBase
    //{


    //    public override Shared.GridSudoku Solve(Shared.GridSudoku s)
    //    {

    //        //using (Py.GIL())
    //        //{
    //        // create a Python scope
    //        using (PyModule scope = Py.CreateScope())
    //        {
    //            // convert the Person object to a PyObject
    //            PyObject pyCells = s.Cellules.ToPython();

    //            // create a Python variable "person"
    //            scope.Set("instance", pyCells);

    //            // the person object may now be used in Python
    //            string code = Resources.PSOSolvers1_py;
    //            scope.Exec(code);
    //            var result = scope.Get("r");
    //            var managedResult = result.As<int[][]>();
    //            //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
    //            return new Shared.GridSudoku() { Cellules = managedResult };
    //        }
    //        //}

    //    }

    //    protected override void InitializePythonComponents()
    //    {
    //        //InstallPipModule("z3-solver"); A adapter au modèle PSO
    //        base.InitializePythonComponents();
    //    }



    //}

    //public class PSOSolvers1 : PythonSolverBase
    //{


    //    public override Shared.GridSudoku Solve(Shared.GridSudoku s)
    //    {

    //        //using (Py.GIL())
    //        //{
    //        // create a Python scope
    //        using (PyModule scope = Py.CreateScope())
    //        {
    //            // convert the Person object to a PyObject
    //            PyObject pyCells = s.Cellules.ToPython();

    //            // create a Python variable "person"
    //            scope.Set("instance", pyCells);

    //            // the person object may now be used in Python
    //            string code = Resources.PSOSolvers1_py;
    //            scope.Exec(code);
    //            var result = scope.Get("r");
    //            var managedResult = result.As<int[][]>();
    //            //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
    //            return new Shared.GridSudoku() { Cellules = managedResult };
    //        }
    //        //}

    //    }

    //    protected override void InitializePythonComponents()
    //    {
    //        //InstallPipModule("z3-solver"); A adapter au modèle PSO
    //        base.InitializePythonComponents();
    //    }
    //}


