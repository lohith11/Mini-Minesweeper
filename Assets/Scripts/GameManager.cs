using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    public Cell[,] masterLevel;
    public int gridSize;
    void Awake()
    {
        gameManagerInstance = this;
    }
    void Start()
    {
        masterLevel = LevelGeneration.levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        masterLevel = LevelGeneration.levelGenerationInstance.SetNeighbours(masterLevel);
        LevelGeneration.levelGenerationInstance.SetTiles(masterLevel);
    }
}
