using Sudoku.Shared;
using System;

public class Function //Cette classe contiendra essentiellement des fonctions que nous avons codées pour réaliser notre solveur
{
    public int Comptage_Ligne(Sudoku.Shared.GridSudoku s, int row_Nb) //Cette fonction va être utilisée pour compter le nombre de cases vides qu'il y a dans une ligne 
    {
        int i;     
        int c = 0;  //On initialise le compteur des cases vides à 0 

        for(i = 0; i < 9; i++) //On commence à parcourir toute la ligne 
        {
            if(s.Cellules[row_Nb][i]==0) //Si une cellule est vide, c'est-à-dire qu'elle est égale à 0, alors le compteur s'incrémente
            {
                c++;
            }
        }
        return c; //La fonction nous retourne à la fin le nombre de cases vides qu'il y a dans chaque ligne 
    }

    //public int Swarm_Generator(int nbManquant); // En paramètre, il faudra mettre la classe Swarm également 


}
