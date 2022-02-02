using System;
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