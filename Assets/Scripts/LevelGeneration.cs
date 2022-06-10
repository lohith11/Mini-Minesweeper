using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] float bombProbability;
    [SerializeField] GameObject cellPrefab;
    int bombCount = 0;

    public Cell[,] GenerateLevel(int rows, int cols)
    {
        Cell[,] level = new Cell[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                level[i, j] = new Cell(i, j);
                if (Random.Range(0f, 1f) < bombProbability)
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
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                if (level[i, j].HasBomb())
                    continue;
                else
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0)
                                continue;
                            else if (i + x < 0 || i + x >= level.GetLength(0) || j + y < 0 || j + y >= level.GetLength(1))
                                continue;
                            else if (level[i + x, j + y].HasBomb())
                                level[i, j].AddNeighbour(1);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                Debug.Log("Cell at " + i + ", " + j + " has " + level[i, j].GetNeighbours() + " neighbours");
            }
        }
        return level;
    }

    public void SetTiles(Cell[,] level)
    {
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                GameObject tile = Instantiate(cellPrefab, new Vector3(i, j, 0), Quaternion.identity);
                //tile.GetComponent<Cell>().SetCoordinates(i, j);
                if (level[i, j].HasBomb())
                {
                    tile.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                    tile.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                }
                else
                {
                    tile.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    tile.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                    tile.transform.GetChild(1).GetComponent<TextMesh>().text = level[i, j].GetNeighbours().ToString();
                }
            }
        }
    }
}
