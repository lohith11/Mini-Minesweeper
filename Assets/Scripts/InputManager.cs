using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManagerInstance;
    public CellProperties StartProperties;
    void Awake()
    {
        inputManagerInstance = this;
    }
    void Update()
    {
        if (!GameManager.gameManagerInstance.gameStarted && !GameManager.gameManagerInstance.gameEnded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    StartGameAt(hit);
                }
            }
        }

        if (GameManager.gameManagerInstance.gameStarted)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    MarkedTile(hit);
                }
            }
        }

        #region SneakPeakCode

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     levelCopy = GameManager.gameManagerInstance.masterLevel;
        //     notRevealed = new List<Vector2Int>();

        //     for (int i = 0; i < levelCopy.GetLength(0); i++)
        //     {
        //         for (int j = 0; j < levelCopy.GetLength(1); j++)
        //         {
        //             if (!levelCopy[i, j].IsRevealed)
        //             {
        //                 notRevealed.Add(new Vector2Int(i, j));
        //                 levelCopy[i, j].SetRevealed(true);
        //             }
        //         }
        //     }
        //     LevelGeneration.levelGenerationInstance.SetTiles(levelCopy);
        // }

        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     for (int i = 0; i < levelCopy.GetLength(0); i++)
        //     {
        //         for (int j = 0; j < levelCopy.GetLength(1); j++)
        //         {
        //             if (notRevealed[0].Equals(new Vector2Int(i, j)))
        //             {
        //                 notRevealed.RemoveAt(0);
        //                 levelCopy[i, j].SetRevealed(false);
        //             }
        //         }
        //     }
        //     LevelGeneration.levelGenerationInstance.SetTiles(levelCopy);
        // }

        #endregion
    }

    public void ClickedOnTile(CellProperties cellProps)
    {
        //CellProperties cellProps = hit.collider.gameObject.transform.GetComponentInParent<CellProperties>();
        //Debug.Log("Hit cell: " + cellProps.xCoordinate + ", " + cellProps.yCoordinate);

        if (cellProps.isMarked)
        {
            Debug.Log("Tile marked");
            return;
        }
        else if (cellProps.isRevealed)
        {
            Debug.Log("Tile already revealed");
            return;
        }

        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetRevealed(true);

        if (cellProps.hasBomb)
        {
            Debug.Log("Bomb hit!");
            HealthManager.healthManagerInstance.BombHit();
            LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
            return;
        }
        else if (cellProps.neighbours == 0)
            LevelGeneration.levelGenerationInstance.CheckNeighbours(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);

        LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
    }

    void MarkedTile(RaycastHit2D hit)
    {
        if (!GameManager.gameManagerInstance.gameStarted)
            return;

        CellProperties cellProps = hit.collider.gameObject.transform.GetComponentInParent<CellProperties>();
        Debug.Log("Clicked on " + cellProps.xCoordinate + ", " + cellProps.yCoordinate);
        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(true);

        if (cellProps.isRevealed)
        {
            Debug.Log("Tile already revealed");
            return;
        }

        if (cellProps.isMarked)
        {
            cellProps.isMarked = false;
            GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(false);
            LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
            //cellProps.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            cellProps.isMarked = true;
            GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(true);
            LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
            //cellProps.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void StartGameAt(RaycastHit2D hit)
    {
        CellProperties cellProps = hit.collider.gameObject.transform.GetComponentInParent<CellProperties>();
        StartProperties = cellProps;
        //Debug.Log("Clicked on " + cellProps.xCoordinate + ", " + cellProps.yCoordinate);

        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetBomb(false);
        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetRevealed(true);
        LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
        GameManager.gameManagerInstance.masterLevel = LevelGeneration.levelGenerationInstance.SetNeighbours(GameManager.gameManagerInstance.masterLevel);

        GameManager.gameManagerInstance.StartGame(new Vector2(cellProps.xCoordinate, cellProps.yCoordinate));
    }
}
