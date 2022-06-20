using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector] List<Vector2Int> notRevealed;
    Cell[,] levelCopy;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                CellProperties cellProps = hit.collider.gameObject.transform.GetComponentInParent<CellProperties>();
                Debug.Log("Clicked on " + cellProps.xCoordinate + ", " + cellProps.yCoordinate);
                GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetRevealed(true);
                LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                CellProperties cellProps = hit.collider.gameObject.transform.GetComponentInParent<CellProperties>();
                Debug.Log("Clicked on " + cellProps.xCoordinate + ", " + cellProps.yCoordinate);
                GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(true);

                if (cellProps.isMarked)
                {
                    cellProps.isMarked = false;
                    GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(false);
                    cellProps.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    cellProps.isMarked = true;
                    GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(true);
                    cellProps.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            levelCopy = GameManager.gameManagerInstance.masterLevel;
            notRevealed = new List<Vector2Int>();

            for (int i = 0; i < levelCopy.GetLength(0); i++)
            {
                for (int j = 0; j < levelCopy.GetLength(1); j++)
                {
                    if (!levelCopy[i, j].IsRevealed)
                    {
                        notRevealed.Add(new Vector2Int(i, j));
                        levelCopy[i, j].SetRevealed(true);
                    }
                }
            }
            LevelGeneration.levelGenerationInstance.SetTiles(levelCopy);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            for (int i = 0; i < levelCopy.GetLength(0); i++)
            {
                for (int j = 0; j < levelCopy.GetLength(1); j++)
                {
                    if (notRevealed[0].Equals(new Vector2Int(i, j)))
                    {
                        notRevealed.RemoveAt(0);
                        levelCopy[i, j].SetRevealed(false);
                    }
                }
            }
            LevelGeneration.levelGenerationInstance.SetTiles(levelCopy);
        }
    }
}
