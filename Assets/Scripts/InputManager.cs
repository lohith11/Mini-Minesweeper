using UnityEngine;
using Cinemachine;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManagerInstance;
    [HideInInspector] public CellProperties StartProperties;
    void Awake()
    {
        inputManagerInstance = this;
    }
    void Update()
    {
        if (GameManager.gameManagerInstance.gameEnded || GameManager.gameManagerInstance.gamePaused)
            return;
        if (!GameManager.gameManagerInstance.ballSpawned)
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
    }

    public void ClickedOnTile(CellProperties cellProps)
    {
        if (cellProps.isMarked)
            return;
        else if (cellProps.isRevealed)
            return;

        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetRevealed(true);
        AudioManager.audioManagerInstance.Play("Break");

        if (cellProps.hasBomb)
        {
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
        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(true);

        if (cellProps.isRevealed)
            return;

        AudioManager.audioManagerInstance.Play("Mark");
        if (cellProps.isMarked)
        {
            cellProps.isMarked = false;
            GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(false);
            LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
        }
        else
        {
            cellProps.isMarked = true;
            GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetMarked(true);
            LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
        }
    }

    void StartGameAt(RaycastHit2D hit)
    {
        CellProperties cellProps = hit.collider.gameObject.transform.GetComponentInParent<CellProperties>();
        StartProperties = cellProps;

        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetBomb(false);
        GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate].SetRevealed(true);
        LevelGeneration.levelGenerationInstance.SetTile(GameManager.gameManagerInstance.masterLevel[cellProps.xCoordinate, cellProps.yCoordinate]);
        GameManager.gameManagerInstance.masterLevel = LevelGeneration.levelGenerationInstance.SetNeighbours(GameManager.gameManagerInstance.masterLevel);

        GameManager.gameManagerInstance.StartGame(new Vector2(cellProps.xCoordinate, cellProps.yCoordinate));
    }
}
