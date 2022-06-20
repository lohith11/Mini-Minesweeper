using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] float bombProbability;
    [SerializeField] GameObject cellPrefab;
    int bombCount = 0;
    public static LevelGeneration levelGenerationInstance;

    void Awake()
    {
        levelGenerationInstance = this;
    }

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
                    level[i, j].SetBomb(true);
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
                if (level[i, j].HasBomb)
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
                            else if (level[i + x, j + y].HasBomb)
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
                Debug.Log("Cell at " + i + ", " + j + " has " + level[i, j].NeighbourCount + " neighbours");
            }
        }
        return level;
    }

    public void SetTiles(Cell[,] level)
    {
        CellProperties[] existingLevel = FindObjectsOfType<CellProperties>();
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                GameObject tile = Instantiate(cellPrefab, new Vector3(i, j, 0), Quaternion.identity);

                CellProperties cellProperties = tile.GetComponent<CellProperties>();
                SpriteRenderer bombRenderer = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();
                MeshRenderer textRenderer = tile.transform.GetChild(1).GetComponent<MeshRenderer>();
                SpriteRenderer tileRenderer = tile.transform.GetChild(2).GetComponent<SpriteRenderer>();
                TextMesh textMesh = tile.transform.GetChild(1).GetComponent<TextMesh>();

                cellProperties.SetProperties(level[i, j]);
                tileRenderer.enabled = true;
                bombRenderer.enabled = false;
                textRenderer.enabled = false;

                if (cellProperties.isRevealed)
                {
                    tileRenderer.enabled = false;

                    if (cellProperties.hasBomb)
                    {
                        bombRenderer.enabled = true;
                        textRenderer.enabled = false;
                    }
                    else
                    {
                        bombRenderer.enabled = false;
                        textRenderer.enabled = true;
                        textMesh.text = level[i, j].NeighbourCount.ToString();
                    }
                }
            }
        }

        for(int i=0;i<existingLevel.Length;i++)
            Destroy(existingLevel[i].gameObject);
    }
}
