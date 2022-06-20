using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    int xCoordinate, yCoordinate, neighbours = 0;
    bool hasBomb = false, isRevealed = false, isMarked = false;
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
        if (isMarked)
        {
            Debug.Log("Tile marked");
            return;
        }
        isRevealed = b;
        LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[this.xCoordinate, this.yCoordinate]);
        if (this.hasBomb)
        {
            Debug.Log("Bomb hit!");
            return;
        }
        CheckNeighbours();
    }

    public void SetMarked(bool b)
    {
        if (isRevealed)
        {
            Debug.Log("Tile already revealed");
            return;
        }
        isMarked = b;
    }

    public void AddNeighbour(int n)
    {
        neighbours += n;
    }

    public void CheckNeighbours()
    {
        if (neighbours == 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    else if (xCoordinate + x < 0 || xCoordinate + x >= GameManager.gameManagerInstance.masterLevel.GetLength(0) || yCoordinate + y < 0 || yCoordinate + y >= GameManager.gameManagerInstance.masterLevel.GetLength(1))
                        continue;

                    else if (!GameManager.gameManagerInstance.masterLevel[xCoordinate + x, yCoordinate + y].IsRevealed)
                    {
                        GameManager.gameManagerInstance.masterLevel[xCoordinate + x, yCoordinate + y].SetRevealed(true);
                        GameManager.gameManagerInstance.masterLevel[xCoordinate + x, yCoordinate + y].CheckNeighbours();
                    }
                }
            }
        }
    }
}