using System;


public class Case
    {
        public int m
        public int x
        public int y
        public case[] adj
        public int v

        public Case(int x, int y){

            this.x = x
            this.y = y
            v = 0

                if (x < 3 && y < 3)
        {
            m = 0
                }
        else if (x < 6 && y < 3)
        {
            m = 1
                }
        else if (x < 9 && y < 3)
        {
            m = 2
                }
        else if (x < 3 && y < 6)
        {
            m = 3
                }
        else if (x < 6 && y < 6)
        {
            m = 4
                }
        else if (x < 9 && y < 6)
        {
            m = 5
                }
        else if (x < 3 && y < 9)
        {
            m = 6
                }
        else if (x < 6 && y < 9)
        {
            m = 7
                }
        else if (x < 9 && y < 9)
        {
            m = 8
                }
    }
    }



 public class Sudoku
{
    public Case[] [] s

    public Sudoku()
    {
        s = new Case[9][9]
        for (int i = 0, i< 9, i++)
        {
            for (int j = 0, j< 9, j++)
            {

                s[i][j] = new Case(i, j)
                // algo adj matt
                //Meme x et y
                for (int a = 0, a< 9, a++)
                {
                    if (a != i)
                    {
                        s[i][j].adj.append(s[i][a])
                    }
                    if (a != j)
                    {
                        s[i][j].adj.append(s[a][j])
                    }
                }
                //Meme case
                for (int a = 0, a< 9, a++)
                {
                    for (int b = 0, b< 9, b++)
                    {
                        if (s[a][b].m == s[i][j].m && (a != i && b != j))
                        {
                            s[i][j].adj.append(s[a][b])
                        }
                    }
                }

            }

        }
    }
