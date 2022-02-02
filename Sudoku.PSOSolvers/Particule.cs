using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku.PSOSolvers
{
    public class Particule  //On va commencer par coder ce qui compose la classe particule 
    {
        public int currentPos_X;  //D'abord, elle se compose d'une position, en abscissse et ordonnée, qu'elle mettra à jour au fur et à mesure du déplacement
        public int currentPos_Y;

        public int bestPos_X; //La meilleure position sera conditionnée en fonction de la fonction fitness que l'on détaillera plus tard
        public int bestPos_Y;

        public int vitesse_X; //Enfin, il convient de dire que cette particule possèdera une vitesse qui lui permettra de se déplacer 
        public int vitesse_Y;

        public int representation; //N'oublions pas que chaque particule est principalement caractérisé par un chiffre
                                   //En l'occurrence, il s'agit du nombre qui sera placé dans la case 

        //Ici on a le constructeur par défaut qui nous permettra d'initialiser chaque particule
        public Particule(int PosX, int PosY, int BPos_X, int Bpos_Y, int v_X, int v_Y, int rep) 
        {
            currentPos_X = PosX;
            currentPos_Y = PosY;
            bestPos_X = BPos_X;
            bestPos_Y = Bpos_Y;
            vitesse_X = v_X;
            vitesse_Y = v_Y;
            representation = rep;
        }
    }
}
