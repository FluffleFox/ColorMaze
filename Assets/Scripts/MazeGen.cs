using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    //public int GridSize = 50;
    //public float Wyp = 0.9f;
    public GameObject Player;
    public GameObject Target;

    MazeGrid Grid = new MazeGrid();
    public PhysicsMaterial2D PhysicsMaterial;
    public void Generate(int GridSize,float Wyp)
    {
        //int Width = Screen.width+ Screen.width%2; //Camera.main.pixelWidth + Camera.main.pixelWidth%2; 
        //int Heihgt = Screen.height + Screen.height % 2;//Camera.main.pixelHeight + Camera.main.pixelHeight%2;
        int Heihgt = Mathf.CeilToInt(Camera.main.orthographicSize * 200.0f);
        int Width = Mathf.CeilToInt(Heihgt * Camera.main.aspect);

        //Debug.Log(Width + " X " + Heihgt);

        Texture2D Tex = new Texture2D(Width, Heihgt);
        Tex.filterMode = FilterMode.Point;

        int BoardY = Heihgt % GridSize / 2;
        int BoardX = Width  % GridSize / 2;

        int MazeRow = Heihgt / GridSize;
        int MazeColl = Width / GridSize;

        Player.transform.position = new Vector3((1-MazeColl%2)*GridSize*0.005f +Random.Range((int)-MazeColl / 2, (int)MazeColl / 2)* GridSize * 0.01f, (1 - MazeRow % 2) * GridSize * 0.005f + Random.Range((int)-MazeRow / 2, (int)MazeRow / 2)* GridSize * 0.01f, -1.0f);
        Target.transform.position = new Vector3((1 - MazeColl % 2) * GridSize * 0.005f + Random.Range((int)-MazeColl / 2, (int)MazeColl / 2)* GridSize * 0.01f, (1 - MazeRow % 2) * GridSize * 0.005f+Random.Range((int)-MazeRow / 2, (int)MazeRow / 2)* GridSize * 0.01f, -1.0f);

        Grid.SetGrid(MazeColl, MazeRow);
        Node[,] Map = Grid.GenerateMaze();

        GenerateCollider(Map, BoardX, BoardY, Width/2, Heihgt/2, GridSize, Wyp);

        float RSCale = Random.Range(0.005f, 0.1f);
        float GSCale = Random.Range(0.005f, 0.1f);
        float BSCale = Random.Range(0.005f, 0.1f);

        for (int x=0; x<Width; x++)
        {
            for (int y = 0; y < Heihgt; y++)
            {
                //Wolne pola
                Tex.SetPixel(x, y, new Color(0, 0, 0, 0));

                
                //Ściany
                int X = (x - BoardX) / GridSize;
                if (X >= Map.GetLength(0)) { X = Map.GetLength(0) - 1; }
                int Y = (y - BoardY) / GridSize;
                if (Y >= Map.GetLength(1)) { Y = Map.GetLength(1) - 1; }


                //if ((y - BoardY) % GridSize > GridSize * Wyp && (Map[X, Y] == 1 || Map[X, Y] == 3))
                //{ Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * 0.02f, y * 0.02f), Mathf.PerlinNoise(x * 0.05f + 50, y * 0.05f + 50), Mathf.PerlinNoise(x * 0.01f + 30, y * 0.01f + 30), 1.0f)); }

                if ((x - BoardX) % GridSize < GridSize * Wyp * 0.5f && Map[X, Y].Wall[1])
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }

                if ((x - BoardX) % GridSize > GridSize * (1.0f - (Wyp * 0.5f)) && Map[X, Y].Wall[3])
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }

                if ((y - BoardY) % GridSize < GridSize * Wyp * 0.5f && Map[X, Y].Wall[0])
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }

                if ((y - BoardY) % GridSize > GridSize * (1.0f - (Wyp * 0.5f)) && Map[X, Y].Wall[2])
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }

                //Ramka
                if (x < (BoardX + GridSize * Wyp * 0.5f) || x > Width - (BoardX + GridSize * Wyp * 0.5f) || y < (BoardY + GridSize * Wyp * 0.5f) || y > Heihgt - (BoardY + GridSize * Wyp * 0.5f))
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }

                // Kostki skrzyrzowań
                if ((x - BoardX) % GridSize > GridSize * (1.0f - (Wyp * 0.5f)) && (y - BoardY) % GridSize < GridSize * Wyp * 0.5f)
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }
                if ((x - BoardX) % GridSize > GridSize * (1.0f - (Wyp * 0.5f)) && (y - BoardY) % GridSize > GridSize * (1.0f - (Wyp * 0.5f)))
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }
                if ((x - BoardX) % GridSize < GridSize * Wyp * 0.5f && (y - BoardY) % GridSize < GridSize * Wyp * 0.5f)
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }
                if ((x - BoardX) % GridSize < GridSize * Wyp * 0.5f && (y - BoardY) % GridSize > GridSize * (1.0f - (Wyp * 0.5f)))
                { Tex.SetPixel(x, y, new Color(Mathf.PerlinNoise(x * RSCale, y * RSCale), Mathf.PerlinNoise(x * GSCale, y * GSCale), Mathf.PerlinNoise(x * BSCale, y * BSCale), 1.0f)); }
            }
        }
        Tex.Apply();
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(Tex, new Rect(0,0,Width,Heihgt), new Vector2(0.5f,0.5f));
        Player.GetComponent<PlayerCollider>().Map = Tex;

        GetComponent<CompositeCollider2D>().GenerateGeometry(); //Nice

        foreach(Transform k in transform)
        {
            Destroy(k.gameObject);
        }
        //Destroy(this, 1.0f);
    } 

    void GenerateCollider(Node[,] Map, float BoardX, float BoardY, int XOffset, int YOffset, int GridSize, float Wyp)
    {
        float ShortCollSize = (Mathf.Ceil(GridSize * Wyp * 0.5f) + Mathf.Ceil(GridSize * Wyp * 0.5f) % 2) * 0.01f;
        float CollSize = Mathf.Floor(GridSize*(1.0f+Wyp)) * 0.01f;

        float ShortX = (-XOffset + BoardX)*0.01f + ShortCollSize*0.5f;
        float ShortY = (-YOffset + BoardY)*0.01f + ShortCollSize*0.5f;
        float LongX = (-XOffset + BoardX) * 0.01f + GridSize * 0.005f + (1 - (int)(CollSize * 100.0f) % 2) * 0.005f;
        float LongY = (-YOffset + BoardY) * 0.01f + GridSize * 0.005f + (1 - (int)(CollSize * 100.0f) % 2) * 0.005f;

        foreach (Node k in Map)
         {
            if (k.Wall[0])
            {
                GameObject GO = new GameObject();
                BoxCollider2D Coll = GO.AddComponent<BoxCollider2D>();
                Coll.size = new Vector2(CollSize, ShortCollSize);
                Coll.offset = new Vector2(LongX + k.x * GridSize * 0.01f, ShortY + k.y * GridSize * 0.01f);
                GO.transform.parent = this.transform;
                Coll.usedByComposite = true;
                if (PhysicsMaterial != null)
                {
                    Coll.sharedMaterial = PhysicsMaterial;
                }
            }

            if (k.Wall[1])
            {
                GameObject GO = new GameObject();
                BoxCollider2D Coll = GO.AddComponent<BoxCollider2D>();
                Coll.size = new Vector2(ShortCollSize, CollSize);
                Coll.offset = new Vector2(ShortX + k.x * GridSize * 0.01f, LongY + k.y * GridSize * 0.01f);
                GO.transform.parent = this.transform;
                Coll.usedByComposite = true;
                if (PhysicsMaterial != null)
                {
                    Coll.sharedMaterial = PhysicsMaterial;
                }
            }

            if (k.Wall[2])
            {
                GameObject GO = new GameObject();
                BoxCollider2D Coll = GO.AddComponent<BoxCollider2D>();
                Coll.size = new Vector2(CollSize, ShortCollSize);
                Coll.offset = new Vector2(LongX + k.x * GridSize * 0.01f, ShortY + Mathf.Ceil(k.y * GridSize + GridSize*(1.0f-Wyp*0.5f)) * 0.01f);
                GO.transform.parent = this.transform;
                Coll.usedByComposite = true;
                if (PhysicsMaterial != null)
                {
                    Coll.sharedMaterial = PhysicsMaterial;
                }
            }

            if (k.Wall[3])
            {
                GameObject GO = new GameObject();
                BoxCollider2D Coll = GO.AddComponent<BoxCollider2D>();
                Coll.size = new Vector2(ShortCollSize, CollSize);
                Coll.offset = new Vector2(ShortX + Mathf.Ceil(k.x * GridSize + GridSize * (1.0f - Wyp * 0.5f)) * 0.01f, LongY + k.y * GridSize * 0.01f);
                GO.transform.parent = this.transform;
                Coll.usedByComposite = true;
                if (PhysicsMaterial != null)
                {
                    Coll.sharedMaterial = PhysicsMaterial;
                }
            }
        }
    }
}
