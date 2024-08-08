using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilegrid : MonoBehaviour
{
    public Tilerow[] rows { get; private set; }
    public Tilecell[] cells { get; private set; }

    public int Size => cells.Length;
    public int Height => rows.Length;
    public int Width => Size / Height;

    private void Awake()
    {
        rows = GetComponentsInChildren<Tilerow>();
        cells = GetComponentsInChildren<Tilecell>();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].coordinates = new Vector2Int(i % Width, i / Width);
        }
    }
    public void Start()
    {
        for(int y = 0; y< rows.Length; y ++)
        {
            for (int x =0;x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int (x,y);
            }
        }
    }
    public Tilecell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }
    public Tilecell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length)
            {
                index = 0;
            }

            // all cells are occupied
            if (index == startingIndex)
            {
                return null;
            }
        }

        return cells[index];
    }

    public Tilecell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }
    public Tilecell GetAdjacentCell(Tilecell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }



}
