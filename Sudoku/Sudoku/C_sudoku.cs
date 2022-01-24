using System;
using System.Collections;
using System.Collections.Generic;

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
        v = 0;

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



        for (int i = 0; i< 9; i++)
        {
            for (int j = 0; j< 9; j++)
            {


                // algo adj matt
                //Meme x et y
                for (int a = 0; a< 9; a++)
                {
                    if (a != i) s[i,j].addadj(s[i,a]);
                    if (a != j) s[i,j].addadj(s[a,j]);
                }
                //Meme case
                for (int a = 0; a< 9; a++)
                {
                    for (int b = 0; b< 9; b++)
                    {
                        
                        int k1 = s[a, b].getM();
                        int k2 = s[i, j].getM();
                        if (k1 == k2 && (a != i || b != j))
                        {
                            s[i, j].addadj(s[a, b]);
                        }
                        
                    }
                }

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

    public int[] greedyColoring(int[] result)
    {

        // A temporary array to store the available colors. True
        // value of available[cr] would mean that the color cr is
        // assigned to one of its adjacent vertices
        bool[] available = StoreColors();

        // Assign colors to remaining V-1 vertices
        result= AsignColors(result, available);

        return result;
    }

    private int[] AsignColors(int[] result, bool[] available)
    {
        for (int u = 1; u < 9; u++)
        {
            for (int j = 1; j < 9; j++)
            {
                // Process all adjacent vertices and flag their colors
                // as unavailable
                IEnumerator<Case> it = s[u,j].GetEnum();
                while (it.MoveNext())
                {
                    int i = it.Current.getV();
                    if (result[i] != -1)
                    {
                        available[result[i]] = true;
                    }
                }

                // Find the first available color
                int cr;
                for (cr = 0; cr < 9; cr++)
                {
                    if (available[cr] == false)
                    {
                        break;
                    }
                }

                result[u] = cr; // Assign the found color

                // Reset the values back to false for the next iteration
                it = s[u,j].GetEnum();
                while (it.MoveNext())
                {
                    int i = it.Current.getV();
                    if (result[i] != -1)
                    {
                        available[result[i]] = false;
                    }
                }
            }
        }
        return result;
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