using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] List<Sprite> numbers;
    [SerializeField] Sprite marked, unmarked;
    [SerializeField] GameObject cellPrefab, blockerTop, blockerSides, blockerTopLeft, blockerTopRight, blockerBottomLeft, blockerBottomRight;
    int bombCount = 0;
    bool locationFound = false;
    float bombProbability = 0.15f;
    public static LevelGeneration levelGenerationInstance;

    public bool peeking = false;

    void Awake()
    {
        levelGenerationInstance = this;
        switch (DataCarrier.difficulty)
        {
            case "Easy": bombProbability = 0.05f; break;
            case "Medium": bombProbability = 0.15f; break;
            case "Hard": bombProbability = 0.2f; break;
            default: bombProbability = 0.15f; break;
        }
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
                BoxCollider2D endTrigger = tile.transform.GetChild(3).GetComponent<BoxCollider2D>();
                SpriteRenderer flagRenderer = tile.transform.GetChild(3).GetComponent<SpriteRenderer>();

                cellProperties.SetProperties(cell);
                tileRenderer.enabled = true;
                boxCollider.enabled = true;
                textRenderer.enabled = false;
                bombRenderer.enabled = false;
                flagRenderer.enabled = false;
                endTrigger.enabled = false;

                if (cellProperties.isEndPoint)
                {
                    textRenderer.enabled = false;
                    flagRenderer.enabled = true;
                    endTrigger.enabled = true;
                }

                if (cellProperties.hasBomb)
                    bombRenderer.enabled = true;

                if (cellProperties.neighbours != 0)
                {
                    textRenderer.enabled = true;
                    textRenderer.sprite = numbers[cellProperties.neighbours - 1];
                }

                if (cellProperties.isMarked)
                    tileRenderer.sprite = marked;
                else
                    tileRenderer.sprite = unmarked;

                if (cellProperties.isRevealed)
                {
                    tileRenderer.enabled = false;
                    boxCollider.enabled = false;
                }
                else
                {
                    tileRenderer.enabled = true;
                    boxCollider.enabled = true;
                }

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

    public void PlaceFinishPoint(Vector2Int ballCoordinates)
    {
        while (!locationFound)
        {
            int x = UnityEngine.Random.Range(0, GameManager.gameManagerInstance.masterLevel.GetLength(0));
            int y = UnityEngine.Random.Range(0, GameManager.gameManagerInstance.masterLevel.GetLength(1));
            Vector2Int finishCoordinates = new Vector2Int(x, y);
            if ((Vector2Int.Distance(finishCoordinates, ballCoordinates) > (GameManager.gameManagerInstance.gridSize / 2)) && !GameManager.gameManagerInstance.masterLevel[x, y].HasBomb)
            {
                locationFound = true;
                GameManager.gameManagerInstance.masterLevel[x, y].SetEndPoint(true);
                SetTile(GameManager.gameManagerInstance.masterLevel[x, y]);
                Debug.Log("Finish point placed at " + finishCoordinates);
            }
        }
    }
}
