using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    public Cell[,] masterLevel;
    public int gridSize;

    [SerializeField] GameObject ballPrefab;

    public bool gameStarted = false;
    void Awake()
    {
        gameManagerInstance = this;
    }
    void Start()
    {
        masterLevel = LevelGeneration.levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        LevelGeneration.levelGenerationInstance.SetTiles(masterLevel);
    }

    public void StartGame()
    {
        masterLevel = LevelGeneration.levelGenerationInstance.SetNeighbours(masterLevel);
    }
}
