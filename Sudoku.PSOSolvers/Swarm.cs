using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.PSOSolvers
{
    public class Swarm //Ici on va coder toute notre classe Swarm qui représente l'essaim contenant les particules en question 
    {
        public int gBestPosition_X; //Elle se caractérise principalement par la meilleure position globale
        public int gBestPosition_Y;

        public int nb_Particules;  //Elle aura également le nombre de particules qu'elle contiendra 

        public Particule[] ens_Particules; //Enfin, elle contiendra un tableau de particule dont la taille sera celle du nombre de particules 

    }
}
