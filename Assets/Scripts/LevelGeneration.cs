using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] float bombProbability;
    int bombCount=0;

    public Cell[,] GenerateLevel(int rows, int cols)
    {
        Cell[,] level = new Cell[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                level[i, j] = new Cell(i,j);
                if(Random.Range(0f,1f) < bombProbability)
                {
                    Debug.Log("Bomb placed at " + i + ", " + j);
                    level[i, j].SetBomb();
                    bombCount++;
                }
            }
        }
        Debug.Log("Bomb count: " + bombCount);
        return level;
    }

    public Cell[,] SetNeighbours(Cell[,] level)
    {
        for (int i = 0; i < level.GetLength(0); i++) {
            for (int j = 0; j < level.GetLength(1); j++) {
                if(level[i,j].HasBomb()) {
                    for(int x=-1; x<=1; x++) {
                        for(int y=-1; y<=1; y++) {
                            if(x!=0 || y!=0 && !level[i+x,j+y].HasBomb()) {
                                if(i+x<level.GetLength(0) && j+y<level.GetLength(1)) {
                                    level[i+x, j+y].AddNeighbour();
                                }
                            }
                        }
                    }
                }
            }
        }
        return level;
    }
}
