using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;
    //public Node ParentNode;
    public bool[] Wall;
    public bool Ready;

    public Node()
    {
        this.Wall = new bool[4];
        Wall[0] = true; // Prawa
        Wall[1] = true; // Góra
        Wall[2] = true; // Lewo
        Wall[3] = true; // Dół
        Ready = false;
    }
}
