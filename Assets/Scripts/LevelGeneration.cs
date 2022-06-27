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
                SpriteRenderer tileRenderer = tile.transform.GetChild(2).GetComponent<SpriteRenderer>();
                BoxCollider2D boxCollider = tile.transform.GetChild(2).GetComponent<BoxCollider2D>();
                SpriteRenderer flagRenderer = tile.transform.GetChild(3).GetComponent<SpriteRenderer>();
                MeshRenderer textRenderer = tile.transform.GetChild(1).GetComponent<MeshRenderer>();
                TextMesh textMesh = tile.transform.GetChild(1).GetComponent<TextMesh>();

                cellProperties.SetProperties(cell);
                tileRenderer.enabled = true;
                boxCollider.enabled = true;
                textRenderer.enabled = false;
                bombRenderer.enabled = false;
                flagRenderer.enabled = false;

                textMesh.text = cellProperties.neighbours.ToString();
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
                        flagRenderer.enabled = false;
                        textRenderer.enabled = false;
                    }
                    else
                    {
                        if (cell.NeighbourCount != 0)
                        {
                            textRenderer.enabled = true;
                            bombRenderer.enabled = false;
                            flagRenderer.enabled = false;
                            textMesh.text = cell.NeighbourCount.ToString();
                        }
                        else
                        {
                            bombRenderer.enabled = false;
                            flagRenderer.enabled = false;
                            textRenderer.enabled = false;
                        }
                    }
                }
                else
                {
                    if (cellProperties.isMarked)
                    {
                        tileRenderer.enabled = true;
                        boxCollider.enabled = true;
                        flagRenderer.enabled = true;
                        textRenderer.enabled = false;
                        bombRenderer.enabled = false;
                    }
                    else
                    {
                        tileRenderer.enabled = true;
                        boxCollider.enabled = true;
                        flagRenderer.enabled = false;
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
