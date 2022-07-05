using UnityEngine;

public class CellProperties : MonoBehaviour
{
    public int xCoordinate, yCoordinate, neighbours;
    public bool hasBomb, isRevealed, isMarked, isEndPoint;

    public void SetProperties(Cell cell)
    {
        xCoordinate = cell.Coordinates.x;
        yCoordinate = cell.Coordinates.y;
        neighbours = cell.NeighbourCount;
        hasBomb = cell.HasBomb;
        isRevealed = cell.IsRevealed;
        isMarked = cell.IsMarked;
        isEndPoint = cell.IsEndPoint;
    }
}
