using System;
using System.Text;
namespace Sudoku.PSOSolvers
{
    public class Sudoku //Création d'une classe Sudoku similaire à Sudoku.Shared.Grid 
    {
        private Sudoku(int[,] cellValues) //Fonction chargée de récupérer les valeurs des cellules de la grille 
        {
            CellValues = cellValues;
        }

        public static Sudoku New(int[,] cellValues) //Constructeur par défaut de la grille Sudoku dans notre solveur qui va dupliquer les valeurs des cellules de notre grille
        {
            return new Sudoku(PSOSolvers1.DuplicateMatrix(cellValues));
        }

        public static Sudoku New(Sudoku sudoku) //Constructeur par défaut d'une classe Sudoku qui va copier la grille et les valeurs contenues dans les cellules 
        {                                       //Il fait appel à la fonction codée au-dessus qui duplique les valeurs des cellules
            return new Sudoku(PSOSolvers1.DuplicateMatrix(sudoku.CellValues));
        }

        public int[,] CellValues { get; } //Getter pour les valeurs des cellules car CellValues est un attribut privé

        public int Error //Fonction pour compter le nombre d'erreurs
        {
            get
            {
                return CountErrors(true) + CountErrors(false); //On compte le nombre d'erreurs en fonction des valeurs booléennes true ou false (
                                                               //True permet le comptage par ligne et false par colonne 
                int CountErrors(bool countByRow) //fonction pour compter les erreurs en fonction de la valeur du bool reçu en paramètres
                {                                                                  //Les erreurs peuvent être soit des cases vides, soit des doublons 
                    var errors = 0; //Initialisation du compteur d'erreurs à 0
                    for (var i = 0; i < PSOSolvers1.taille; ++i) //On commence à parcourir la totalité de la grille (ligne par ligne ou colonne par colonne)
                    {
                        var counts = new int[PSOSolvers1.taille]; //On initialise un compteur d'occurrences pour chaque valeur de la ligne ou de la colonne 
                        for (var j = 0; j < PSOSolvers1.taille; ++j) //On commence à parcourir toute la ligne ou toute la colonne
                        {
                            var cellValue = countByRow ? CellValues[i, j] : CellValues[j, i]; // cellValue = CellValues[i, j] si countByRow = true. Sinon, cellValue = CellValues[j, i]
                            ++counts[cellValue - 1]; //On remplit le tableau counts pour qu'il stocke le nombre de fois que CHAQUE valeur de la ligne se répète
                        }

                        for (var k = 0; k < PSOSolvers1.taille; ++k) //On refait un nouveau comptage pour voir quelles sont les valeurs manquantes par ligne ou par cellule
                        {
                            if (counts[k] == 0) //Si au cours du comptage il y a une valeur qui n'apparaît pas, alors le compteur d'erreurs s'incrémente
                                ++errors;
                        }
                    }

                    return errors;
                }
            }
        }

        public override string ToString() //Cette classe nous permet de construire esthétiquement notre grille Sudoku
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