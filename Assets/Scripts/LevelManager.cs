using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int gridSize;
    public static LevelManager levelManagerInstance;
    public Cell[,] level;
    void Awake()
    {
        levelManagerInstance = this;
    }
    public Cell[,] StartGeneration()
    {
        level = LevelGeneration.levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        level = LevelGeneration.levelGenerationInstance.SetNeighbours(level);
        LevelGeneration.levelGenerationInstance.SetTiles(level);
        return level;
    }

    public Cell[,] GetLevel()
    {
        return level;
    }
}
