using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Case
{
    private int m;
    private int x;
    private int y;
    private List<Case> adj = new List<Case>();
    private int v;

    public Case(int x, int y)
    {

        this.x = x;
        this.y = y;
        v = -1;

        if (x < 3 && y < 3) m = 0;
        else if (x < 6 && y < 3) m = 1;
        else if (x < 9 && y < 3) m = 2;
        else if (x < 3 && y < 6) m = 3;
        else if (x < 6 && y < 6) m = 4;
        else if (x < 9 && y < 6) m = 5;
        else if (x < 3 && y < 9) m = 6;
        else if (x < 6 && y < 9) m = 7;
        else if (x < 9 && y < 9) m = 8;

    }

    public bool verif_coloradj(int clr)
    {
        foreach ( Case c in this.adj)
        {
            if (c.getV() == clr)
            {
                return true;
            }
        }

        return false;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public int getV()
    {
        return v;
    }

    public List<Case> getAdj()
    {
        return adj;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public void setY(int y)
    {
        this.y = y;
    }

    public void setV(int v)
    {
        this.v = v;
    }

    public void setadj(List<Case> adj)
    {
         this.adj= adj;
    }

    public void displayadj()
    {

        IEnumerator<Case> it = this.GetEnum();
        while (it.MoveNext())
        {
            Console.WriteLine(it.Current.getV());
        }
        Console.WriteLine(" ");
    }

    public void addadj(Case adjv)
    {
        this.adj.Add(adjv);
    }

    public IEnumerator<Case> GetEnum()
    {
        return this.adj.GetEnumerator();
    }

    public List<Case> getadj()
    {
        return this.adj;
    }

    public int getM()
    {
        return m;
    }

    public void setM(int m)
    {
        this.m = m;
    }
}
public class C_Sudoku
{
    private Case[,] s;

    public C_Sudoku()
    {
        s = new Case[9,9];

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                s[i, j] = new Case(i, j);
            }
        }



        for (int j = 0; j< 9; j++)
        {
            for (int i = 0; i< 9; i++)
            {


                // algo adj matt
                //Meme x et y
                for (int a = 0; a< 9; a++)
                {
                    if (a != i) s[i,j].addadj(s[a,j]);
 
                }

                for (int a = 0; a < 9; a++)
                {
                    if (a != j) s[i, j].addadj(s[i, a]);
                }
                //Meme case
                for (int b = 0; b< 9; b++)
                {
                    for (int a = 0; a< 9; a++)
                    {
                        
                        int k1 = s[a, b].getM();
                        int k2 = s[i, j].getM();
                        if (k1 == k2 && (a != i && b != j))
                        {
                            s[i, j].addadj(s[a, b]);
                        }
                        
                    }
                }

            }

        }
    }

    public void display_matrice_adj()
    {
        for (int j = 0; j < 9; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine(s[i, j].getV());
                s[i, j].displayadj();
            }
        }
    }
    public Case[,] getS()
    {
        return s;
    }

    public void setS(Case[,] s)
    {
       this.s = s;
    }

    public void fill_sudoku(int[] result)
    {
        int j = -1;

        for (int i = 0; i < 81; i++)
        {
            if (i % 9 == 0)
            {
                j++;
            }

            if (result[i] != -1)
            {
                s[i % 9, j].setV(result[i]);
            }

        }
    }

    public void display()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.WriteLine(s[j, i].getV());
            }
        }
    }


    public void reset()
    {

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                s[j, i].setV(0);
            }
        }
    }

    public int[] greedyColoring(int[] result)
    {

        // A temporary array to store the available colors. True
        // value of available[cr] would mean that the color cr is
        // assigned to one of its adjacent vertices

        bool[] available = StoreColors();

        s[0, 0].setV(0);

        // Assign colors to remaining V-1 vertices
        result = AsignColors(result, available);

        this.fill_sudoku(result);
/*
        int j = -1;

        for (int i = 0; i < 81; i++)
        {
            if (i % 9 == 0)
            {
                j++;
            }
            result[i] = s[i % 9, j].getV();

        }
*/
            return result;
    }

    private int[] AsignColors(int[] result, bool[] available)
    {

        int j = -1;

        for (int u = 0; u < 81; u++)
        {
            if (u % 9 == 0)
            {
                j++;
            }

            if (s[u % 9, j].getV() == 0)
            {
                // Process all adjacent vertices and flag their colors
                // as unavailable
                IEnumerator<Case> it = s[u % 9, j].GetEnum();
                while (it.MoveNext())
                {
                    int i = it.Current.getV();

                    if (i>=0) 
                    {
                        if (result[i] != -1 && result[i] < 9)
                        {
                            available[result[i]] = true;
                        }
                    }

                }

                // Find the first available color
                int cr;
                for (cr = 0; cr < 8; cr++)
                {
                    if (available[cr] == false)
                    {
                        break;
                    }
                }

                result[u] = cr; // Assign the found color
                this.fill_sudoku(result);
                // Reset the values back to false for the next iteration
                it = s[u % 9, j].GetEnum();
                while (it.MoveNext())
                {
                    int i = it.Current.getV();

                    if (i >= 0)
                    {
                        if (result[i] != -1 && result[i] < 9)
                        {

                            available[result[i]] = false;
                        }
                    }

                }
            }
        }
        return result;
    }

 

    private bool verif_color(int clr)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (s[j, i].getV() == clr)
                {
                    return true;
                }
            }
        }

        return false;
    }


    private void AsignColors2(int[] result,int x,int y)
    {

                for (int clr = 1; clr < 10; clr++)
                {

                    for (int j = y; j < 9; j++)
                    {
                        for (int i = x; i < 9; i++)
                        {

                            if ((s[i, j].getV() != clr) && (s[i, j].verif_coloradj(clr) == false) && (s[i, j].getV() == 0))
                            {
                                s[i, j].setV(clr);
                            }
                        }

                    }
                    
                    for (int j = 0; j < y; j++)
                    {
                        for (int i = 0; i < x; i++)
                        {

                            if ((s[i, j].getV() != clr) && (s[i, j].verif_coloradj(clr) == false) && (s[i, j].getV() == 0))
                            {
                                s[i, j].setV(clr);
                            }
                        }

                    }
                    
                }
         


    }

            private bool[] StoreColors()
    {
        bool[] available = new bool[9];
        for (int cr = 0; cr < 9; cr++)
        {
            available[cr] = false;
        }

        return available;
    }
}