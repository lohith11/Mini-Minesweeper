using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    public Cell[,] masterLevel;
    public int gridSize;

    float timeSinceStart = 0f;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] float gameStartTimeout;

    public bool gameStarted = false;
    bool ballSpawned = false;
    void Awake()
    {
        gameManagerInstance = this;
    }
    void Start()
    {
        masterLevel = LevelGeneration.levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        //masterLevel = LevelGeneration.levelGenerationInstance.SetNeighbours(masterLevel);
        LevelGeneration.levelGenerationInstance.SetTiles(masterLevel);
    }

    void Update()
    {
        if (ballSpawned)
            timeSinceStart += Time.deltaTime;
    }

    public void StartGame(Vector2 coordinates)
    {
        ballSpawned = true;
        GameObject ball = Instantiate(ballPrefab, coordinates, Quaternion.identity);
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //FindObjectOfType<TrajectoryLine>().UpdateBallReference();
        masterLevel = LevelGeneration.levelGenerationInstance.SetNeighbours(masterLevel);
        LevelGeneration.levelGenerationInstance.SetTiles(masterLevel);
    }
}
