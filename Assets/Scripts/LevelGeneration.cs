using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] float bombProbability;
    [SerializeField] GameObject cellPrefab, blockerTop, blockerSides, blockerTopLeft, blockerTopRight, blockerBottomLeft, blockerBottomRight;
    [SerializeField] Sprite marked, unmarked;
    [SerializeField] List<Sprite> numbers;
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
        return level;
    }

    public void SetTiles(Cell[,] level)
    {
        CellProperties[] existingTiles = FindObjectsOfType<CellProperties>();

        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(i, j, 0), Quaternion.identity);
                cell.GetComponent<CellProperties>().SetProperties(level[i, j]);
                SetTile(level[i, j]);
            }
        }

        foreach (CellProperties tile in existingTiles)
        {
            Destroy(tile.gameObject);
        }

        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                if (i == 0)
                    Instantiate(blockerSides, new Vector3(i - 1, j, 0), Quaternion.identity);
                if (i == level.GetLength(0) - 1)
                    Instantiate(blockerSides, new Vector3(i + 1, j, 0), Quaternion.identity);

                if (j == 0)
                    Instantiate(blockerTop, new Vector3(i, j - 1, 0), Quaternion.identity);
                if (j == level.GetLength(1) - 1)
                    Instantiate(blockerTop, new Vector3(i, j + 1, 0), Quaternion.identity);


                if (i == 0 && j == 0)
                    Instantiate(blockerBottomLeft, new Vector3(i - 1, j - 1, 0), Quaternion.identity);
                if (i == 0 && j == level.GetLength(1) - 1)
                    Instantiate(blockerTopLeft, new Vector3(i - 1, j + 1, 0), Quaternion.identity);

                if (i == level.GetLength(0) - 1 && j == 0)
                    Instantiate(blockerBottomRight, new Vector3(i + 1, j - 1, 0), Quaternion.identity);
                if (i == level.GetLength(0) - 1 && j == level.GetLength(1) - 1)
                    Instantiate(blockerTopRight, new Vector3(i + 1, j + 1, 0), Quaternion.identity);
            }
        }
    }

    public void SetTile(Cell cell)
    {
        CellProperties[] existingLevel = FindObjectsOfType<CellProperties>();
        for (int i = 0; i < existingLevel.Length; i++)
        {
            if (existingLevel[i].xCoordinate == cell.Coordinates.x && existingLevel[i].yCoordinate == cell.Coordinates.y)
            {
                GameObject tile = existingLevel[i].gameObject;
                CellProperties cellProperties = tile.GetComponent<CellProperties>();
                SpriteRenderer bombRenderer = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();
                SpriteRenderer textRenderer = tile.transform.GetChild(1).GetComponent<SpriteRenderer>();
                SpriteRenderer tileRenderer = tile.transform.GetChild(2).GetComponent<SpriteRenderer>();
                BoxCollider2D boxCollider = tile.transform.GetChild(2).GetComponent<BoxCollider2D>();
                //MeshRenderer textRenderer = tile.transform.GetChild(1).GetComponent<MeshRenderer>();
                //TextMesh textMesh = tile.transform.GetChild(1).GetComponent<TextMesh>();

                cellProperties.SetProperties(cell);
                tileRenderer.enabled = true;
                boxCollider.enabled = true;
                textRenderer.enabled = false;
                bombRenderer.enabled = false;

                //textMesh.text = cellProperties.neighbours.ToString();
                // if (cellProperties.isRevealed)
                // {
                //     tileRenderer.enabled = false;
                // }
                // if (cellProperties.neighbours != 0)
                // {
                //     textRenderer.enabled = true;
                // }
                // if (cellProperties.hasBomb)
                // {
                //     bombRenderer.enabled = true;
                //     textRenderer.enabled = false;
                // }
                // if (cellProperties.isMarked)
                // {
                //     flagRenderer.enabled = true;
                // }

                if (cellProperties.isRevealed)
                {
                    tileRenderer.enabled = false;
                    boxCollider.enabled = false;

                    if (cellProperties.hasBomb)
                    {
                        bombRenderer.enabled = true;
                        //flagRenderer.enabled = false;
                        textRenderer.enabled = false;
                    }
                    else
                    {
                        if (cell.NeighbourCount != 0)
                        {
                            textRenderer.enabled = true;
                            textRenderer.sprite = numbers[cell.NeighbourCount - 1];
                            bombRenderer.enabled = false;
                            //flagRenderer.enabled = false;
                            //textMesh.text = cell.NeighbourCount.ToString();
                        }
                        else
                        {
                            bombRenderer.enabled = false;
                            //flagRenderer.enabled = false;
                            textRenderer.enabled = false;
                        }
                    }
                }
                else
                {
                    if (cellProperties.isMarked)
                    {
                        tileRenderer.enabled = true;
                        tileRenderer.sprite = marked;
                        boxCollider.enabled = true;
                        //flagRenderer.enabled = true;
                        textRenderer.enabled = false;
                        bombRenderer.enabled = false;
                    }
                    else
                    {
                        tileRenderer.enabled = true;
                        tileRenderer.sprite = unmarked;
                        boxCollider.enabled = true;
                        //flagRenderer.enabled = false;
                        textRenderer.enabled = false;
                        bombRenderer.enabled = false;
                    }
                }
                //Destroy(existingLevel[i].gameObject);
                return;
            }
        }
        Debug.Log("Tile " + cell.Coordinates + " not found");
    }

    public void CheckNeighbours(Cell cell)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                else if (cell.Coordinates.x + x < 0 || cell.Coordinates.x + x >= GameManager.gameManagerInstance.masterLevel.GetLength(0) || cell.Coordinates.y + y < 0 || cell.Coordinates.y + y >= GameManager.gameManagerInstance.masterLevel.GetLength(1))
                    continue;

                else if (!GameManager.gameManagerInstance.masterLevel[cell.Coordinates.x + x, cell.Coordinates.y + y].IsRevealed)
                {
                    GameManager.gameManagerInstance.masterLevel[cell.Coordinates.x + x, cell.Coordinates.y + y].SetRevealed(true);
                    SetTile(GameManager.gameManagerInstance.masterLevel[cell.Coordinates.x + x, cell.Coordinates.y + y]);
                    if (GameManager.gameManagerInstance.masterLevel[cell.Coordinates.x + x, cell.Coordinates.y + y].NeighbourCount == 0)
                        CheckNeighbours(GameManager.gameManagerInstance.masterLevel[cell.Coordinates.x + x, cell.Coordinates.y + y]);
                }
            }
        }
    }
}
