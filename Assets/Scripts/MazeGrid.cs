using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGrid
{
    public MazeGrid() { }

    Node[,] Grid;

    public void SetGrid(int x, int y)
    {
        Grid = new Node[x, y];
        for(int i=0; i<x; i++)
        {
            for(int j=0; j<y; j++)
            {
                Grid[i,j] = new Node();
                Grid[i, j].x = i;
                Grid[i, j].y = j;
            }
        }
    }

    public Node[,] GenerateMaze()
    {
        int x = Random.Range(0, Grid.GetLength(0));
        int y = Random.Range(0, Grid.GetLength(1));

        Node CurrentNode = Grid[x, y];
        //Node LastNode;

        List<Node> Path = new List<Node>();
        Path.Add(CurrentNode);

        for(int i=0; i<Grid.Length; i++)
        {
            int Dir = GetDir(x, y);

            if (Dir == 0) {CurrentNode.Ready = true; Path.Remove(CurrentNode);}

            while (Dir == 0) //Cofanie się
            {
                if (Path.Count > 0)
                {
                    CurrentNode = Path[Path.Count - 1];
                    x = CurrentNode.x;
                    y = CurrentNode.y;
                    Dir = GetDir(x, y);
                    Path.Remove(CurrentNode);
                }
                else break;
            }

            switch (Dir)
            {
                case 1: //Lewo
                    {
                        CurrentNode.Ready = true;
                        CurrentNode.Wall[1] = false;
                        x -= 1;
                        CurrentNode = Grid[x, y];
                        CurrentNode.Wall[3] = false;
                        break;
                    }
                case 2: //Góra
                    {
                        CurrentNode.Ready = true;
                        CurrentNode.Wall[0] = false;
                        y -= 1;
                        CurrentNode = Grid[x, y];
                        CurrentNode.Wall[2] = false;
                        break;
                    }
                case 3: //Prawo
                    {
                        CurrentNode.Ready = true;
                        CurrentNode.Wall[3] = false;
                        x += 1;
                        CurrentNode = Grid[x, y];
                        CurrentNode.Wall[1] = false;
                        break;
                    }
                case 4: //Dół
                    {
                        CurrentNode.Ready = true;
                        CurrentNode.Wall[2] = false;
                        y += 1;
                        CurrentNode = Grid[x, y];
                        CurrentNode.Wall[0] = false;
                        break;
                    }
            }
            Path.Add(CurrentNode);

        }

        return Grid;
    }

    int GetDir(int x, int y)
    {
        //1 - Lewo
        //2 - Góra
        //3 - Prawo
        //4 - Dół

        int control = 0;
        if (x - 1 >= 0 && !Grid[x - 1, y].Ready) control = 1;
        if (x + 1 < Grid.GetLength(0) && !Grid[x + 1, y].Ready) control += 10;
        if (y - 1 >= 0 && !Grid[x, y - 1].Ready) control += 100;
        if (y + 1 < Grid.GetLength(1) && !Grid[x, y+1].Ready) control += 1000;

        switch (control)
        {
            case 0:
                {
                    return 0; //No way comunicat
                }
            case 1:
                {
                    return 1; //Lewo
                }
            case 10:
                {
                    return 3; //Prawo
                }
            case 11:
                {
                    int[] ret = { 1, 3 };
                    return ret[Random.Range(0, 2)];
                }
            case 100:
                {
                    return 2; //Góra
                }
            case 101:
                {
                    int[] ret = { 1, 2 };
                    return ret[Random.Range(0, 2)];
                }
            case 110:
                {
                    int[] ret = { 3, 2 };
                    return ret[Random.Range(0, 2)];
                }
            case 111:
                {
                    int[] ret = { 1, 2, 3 };
                    return ret[Random.Range(0, 3)];
                }
            case 1000:
                {
                    return 4; //Dół
                }
            case 1001:
                {
                    int[] ret = { 1, 4 };
                    return ret[Random.Range(0, 2)];
                }
            case 1010:
                {
                    int[] ret = { 3, 4 };
                    return ret[Random.Range(0, 2)];
                }
            case 1011:
                {
                    int[] ret = { 1, 3, 4 };
                    return ret[Random.Range(0, 3)];
                }
            case 1100:
                {
                    int[] ret = { 2, 4 };
                    return ret[Random.Range(0, 2)];
                }
            case 1101:
                {
                    int[] ret = { 1, 2, 4 };
                    return ret[Random.Range(0, 3)];
                }
            case 1110:
                {
                    int[] ret = { 2, 3, 4 };
                    return ret[Random.Range(0, 3)];
                }
            case 1111:
                {
                    int[] ret = { 1, 2, 3, 4 };
                    return ret[Random.Range(0, 4)];
                }
        }
        return 0;
    }
}
