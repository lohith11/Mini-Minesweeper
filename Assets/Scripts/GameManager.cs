using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    public Cell[,] masterLevel;
    public int gridSize;

    float timeSinceStart = 0f;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] float gameStartTimeout;
    [SerializeField] CinemachineVirtualCamera topView, ballFollow;

    public bool gameStarted = false;
    bool ballSpawned = false;
    void Awake()
    {
        gameManagerInstance = this;
    }
    void Start()
    {
        masterLevel = LevelGeneration.levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        topView.transform.position = new Vector3(gridSize / 2, gridSize / 2, -10);
        topView.m_Lens.OrthographicSize = (gridSize / 2) + 1;
        LevelGeneration.levelGenerationInstance.SetTiles(masterLevel);
    }

    void Update()
    {
        if (ballSpawned)
            timeSinceStart += Time.deltaTime;

        if (timeSinceStart > gameStartTimeout)
        {
            if (!gameStarted)
            {
                gameStarted = true;
                ballFollow.Priority = 10;
                topView.Priority = 5;
            }
        }
    }

    public void StartGame(Vector2 coordinates)
    {
        ballSpawned = true;
        GameObject ball = Instantiate(ballPrefab, coordinates, Quaternion.identity);
        ballFollow.Follow = ball.transform;
        ballFollow.LookAt = ball.transform;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        FindObjectOfType<TrajectoryLine>().UpdateBallReference();
        masterLevel = LevelGeneration.levelGenerationInstance.SetNeighbours(masterLevel);
        LevelGeneration.levelGenerationInstance.SetTiles(masterLevel);
    }
}
