using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    public Cell[,] masterLevel;
    public int gridSize;

    Vector2Int startCoordinates;

    float timeSinceStart = 0f;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] float gameStartTimeout;
    [SerializeField] CinemachineVirtualCamera topView, ballFollow;

    [SerializeField] GameObject pointingArrow;

    public bool gameStarted = false, gameEnded = false, gamePaused = false;
    bool ballSpawned = false;
    void Awake()
    {
        gameManagerInstance = this;
        gridSize = DataCarrier.gridSize;
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
                CheckReveal();
                pointingArrow.SetActive(true);
                ballFollow.Priority = 10;
                topView.Priority = 5;
            }
        }
    }

    public void StartGame(Vector2 coordinates)
    {
        startCoordinates = new Vector2Int((int)coordinates.x, (int)coordinates.y);
        ballSpawned = true;
        GameObject ball = Instantiate(ballPrefab, coordinates, Quaternion.identity);
        ballFollow.Follow = ball.transform;
        ballFollow.LookAt = ball.transform;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        FindObjectOfType<TrajectoryLine>().UpdateBallReference();
        LevelGeneration.levelGenerationInstance.PlaceFinishPoint(startCoordinates);
        LevelGeneration.levelGenerationInstance.SetTiles(masterLevel);
        HealthManager.healthManagerInstance.GameStarted();
    }

    void CheckReveal()
    {
        if (masterLevel[startCoordinates.x, startCoordinates.y].NeighbourCount == 0)
            LevelGeneration.levelGenerationInstance.CheckNeighbours(masterLevel[startCoordinates.x, startCoordinates.y]);
    }

    public void FlagTouched()
    {
        gameStarted = false;
        gameEnded = true;
        GetComponent<GameMenuManager>().WinGame();
    }
    public void OutOfLives()
    {
        gameStarted = false;
        gameEnded = true;
        GetComponent<GameMenuManager>().LoseGame();
    }
}
