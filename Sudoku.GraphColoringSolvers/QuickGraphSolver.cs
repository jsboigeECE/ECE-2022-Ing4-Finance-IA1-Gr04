using System.Collections.Generic;
using System.Linq;
using QuikGraph;
using QuikGraph.Algorithms.VertexColoring;
using Sudoku.Shared;

namespace Sudoku.GraphColoringSolvers.GraphColoringSolvers
{
    public class QuickGraphSolver:Sudoku.Shared.ISolverSudoku
    {
        public GridSudoku Solve(GridSudoku s)
        {
            UndirectedGraph<int, Edge<int>> graph = CreateSudokuGraph(s);
            var algorithm = new VertexColoringAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();
            IDictionary<int, int?> coloredVertices = algorithm.Colors;

            // Il faudrait réconcilier les couleurs avec les chiffres mais l'algorithme de coloration glouton naïf ne donne pas satisfaction (beaucoup trop de couleurs)
            //Voir plutôt la solution codeproject
            

            return s;

        }

        private UndirectedGraph<int, Edge<int>> CreateSudokuGraph(GridSudoku gridSudoku)
        {
            var toReturn = new UndirectedGraph<int, Edge<int>>();
            // Création des noeuds de cellules
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 9; colIndex++)
                {
                    var cellIndex = rowIndex * 9 + colIndex;
                    toReturn.AddVertex(cellIndex);

                }
            }
            // Création des arrêtes de voisinage
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 9; colIndex++)
                {
                    var cellIndex = rowIndex * 9 + colIndex;
                    var neighbors = GridSudoku.CellNeighbours[rowIndex][colIndex];
                    foreach (var neighbor in neighbors)
                    {
                        var neighborIndex = neighbor.row * 9 + neighbor.column;
                        toReturn.AddEdge(new Edge<int>(cellIndex, neighborIndex));
                    }
                }
            }
            //Création des arrêtes des chiffres du puzzle
            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 9; colIndex++)
                {
                    var currentCellValue = gridSudoku.Cellules[rowIndex][colIndex];
                    var cellIndex = rowIndex * 9 + colIndex;
                    if (currentCellValue != 0)
                    {
                        //Chiffres différents
                        var diffCells = gridSudoku.Cellules.Select((row, rowIndex) => (rowIndex,
                            row.Select((cell, colIndex) => (colIndex, cell))
                                .Where(cell => cell.cell != 0 && cell.cell != currentCellValue)));
                        foreach (var diffRow in diffCells)
                        {
                            foreach (var diffCell in diffRow.Item2)
                            {
                                var diffCellIndex = diffRow.rowIndex * 9 + diffCell.colIndex;
                                toReturn.AddEdge(new Edge<int>(cellIndex, diffCellIndex));
                            }
                        }

                        //Chiffres identiques = voisinages différents

                        var sameCells = gridSudoku.Cellules.Select((row, rowIndex) => (rowIndex,
                            row.Select((cell, colIndex) => (colIndex, cell))
                                .Where(cell => cell.cell == currentCellValue)));
                        foreach (var sameRow in diffCells)
                        {
                            foreach (var sameCell in sameRow.Item2)
                            {
                                var sameCellIndex = sameRow.rowIndex * 9 + sameCell.colIndex;
                                var sameCellNeighborhood =
                                    GridSudoku.CellNeighbours[sameRow.rowIndex][sameCell.colIndex];
                                foreach (var neighborCell in sameCellNeighborhood)
                                {
                                    var neighborCellIndex = neighborCell.row * 9 + neighborCell.column;
                                    toReturn.AddEdge(new Edge<int>(cellIndex, neighborCellIndex));
                                }
                            }
                        }
                    }
                }
            }


            return toReturn;
        }
    }
}
