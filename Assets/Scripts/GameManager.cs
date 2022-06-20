using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    public Cell[,] masterLevel;

    void Awake()
    {
        gameManagerInstance = this;
    }
    void Start()
    {
        masterLevel = LevelManager.levelManagerInstance.StartGeneration();
    }
}
