using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    int xCoordinate, yCoordinate, neighbours = 0;
    bool hasBomb;
    public Cell(int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
    }

    public Vector2 GetCoordinates()
    {
        return new Vector2(xCoordinate, yCoordinate);
    }

    public void SetCoordinates(int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
    }

    public bool HasBomb()
    {
        return hasBomb;
    }

    public void SetBomb()
    {
        hasBomb = true;
    }

    public void AddNeighbour(int n)
    {
        neighbours += n;
    }

    public int GetNeighbours()
    {
        return neighbours;
    }
}
