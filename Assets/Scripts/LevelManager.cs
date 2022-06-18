using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int gridSize;
    LevelGeneration levelGenerationInstance;
    public static LevelManager levelManagerInstance;
    public Cell[,] level;
    void Start()
    {
        levelManagerInstance = this;
        levelGenerationInstance = LevelGeneration.levelGenerationInstance;
        StartGeneration();
    }
    void StartGeneration()
    {
        level = levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        level = levelGenerationInstance.SetNeighbours(level);
        levelGenerationInstance.SetTiles(level);
    }

    public Cell[,] GetLevel()
    {
        return level;
    }
}
