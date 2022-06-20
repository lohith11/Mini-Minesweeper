using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    int xCoordinate, yCoordinate, neighbours = 0;
    bool hasBomb = false, isRevealed = false;
    public Cell(int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
    }

    public Vector2Int Coordinates
    {
        get { return new Vector2Int(xCoordinate, yCoordinate); }
        private set { }
    }
    public int NeighbourCount
    {
        get { return neighbours; }
        private set { }
    }

    public bool HasBomb
    {
        get { return hasBomb; }
        private set { }
    }

    public bool IsRevealed
    {
        get { return isRevealed; }
        private set { }
    }

    public void SetCoordinates(int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
    }

    public void SetBomb(bool b)
    {
        hasBomb = b;
    }

    public void SetRevealed(bool b)
    {
        isRevealed = b;
    }

    public void AddNeighbour(int n)
    {
        neighbours += n;
    }
}
