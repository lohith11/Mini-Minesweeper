using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    int xCoordinate, yCoordinate, neighbours=0;
    bool hasBomb;
    public Cell(int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
        Debug.Log("Cell created");
    }

    public Vector2 GetCoordinates()
    {
        return new Vector2(xCoordinate, yCoordinate);
    }

    public bool HasBomb()
    {
        return hasBomb;
    }

    public void SetBomb()
    {
        hasBomb = true;
    }

    public void AddNeighbour()
    {
        neighbours++;
    }
}
