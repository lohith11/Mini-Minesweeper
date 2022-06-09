using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int gridSize;
    LevelGeneration levelGenerationInstance;
    void Awake()
    {
        levelGenerationInstance = FindObjectOfType<LevelGeneration>();
    }
    void Start()
    {
        Cell[,] level = levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        level = levelGenerationInstance.SetNeighbours(level);
        levelGenerationInstance.SetTiles(level);
    }
}
