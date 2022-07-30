using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    int xCoordinate, yCoordinate, neighbours = 0;
    bool hasBomb = false, isRevealed = false, isMarked = false, isEndPoint = false;
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

    public bool IsMarked
    {
        get { return isMarked; }
        private set { }
    }

    public bool IsEndPoint
    {
        get { return isEndPoint; }
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
        if (!hasBomb)
            HealthManager.healthManagerInstance.TileRevealed();

        isRevealed = b;
        AudioManager.audioManagerInstance.Play("Break");
    }

    public void SetMarked(bool b)
    {
        isMarked = b;
    }

    public void SetEndPoint(bool b)
    {
        isEndPoint = b;
    }

    public void AddNeighbour(int n)
    {
        neighbours += n;
    }
    public void SetNeighbours(int n)
    {
        neighbours = n;
    }
}