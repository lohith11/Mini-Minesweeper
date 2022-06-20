using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProperties : MonoBehaviour
{
    public int xCoordinate, yCoordinate, neighbours;
    public bool hasBomb, isRevealed;

    public void SetProperties(Cell cell)
    {
        xCoordinate = cell.Coordinates.x;
        yCoordinate = cell.Coordinates.y;
        neighbours = cell.NeighbourCount;
        hasBomb = cell.HasBomb;
        isRevealed = cell.IsRevealed;
    }
}
