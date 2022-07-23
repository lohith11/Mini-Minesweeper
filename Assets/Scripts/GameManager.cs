using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] float gameStartTimeout;
    [SerializeField] CinemachineVirtualCamera topView, ballFollow;
    [SerializeField] GameObject pointingArrow, healthImage, timer;
    [SerializeField] List<Sprite> digits;
    [SerializeField] Image secondDigit1, secondDigit2, minuteDigit1, minuteDigit2;
    public int gridSize;
    public Cell[,] masterLevel;
    public static GameManager gameManagerInstance;
    public bool gameStarted = false, gameEnded = false, gamePaused = false, ballSpawned = false;
    Vector2Int startCoordinates;
    float timeSinceStart = 0f;
    int timeCounter = 0;
    void Awake()
    {
        gameManagerInstance = this;
        gridSize = DataCarrier.gridSize;
    }
    void Start()
    {
        masterLevel = LevelGeneration.levelGenerationInstance.GenerateLevel(gridSize, gridSize);
        topView.transform.position = new Vector3(gridSize / 2, (gridSize / 2) - 0.5f, -10);
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
                StartCoroutine(Timer());
                pointingArrow.SetActive(true);
                healthImage.SetActive(true);
                timer.SetActive(true);
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

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timeCounter++;
            int m = (timeCounter / 60) % 60;
            int m1 = (m / 10) % 10;
            int m2 = m % 10;

            int s = timeCounter % 60;
            int s1 = (s / 10) % 10;
            int s2 = s % 10;
            minuteDigit1.sprite = digits[m1];
            minuteDigit2.sprite = digits[m2];
            secondDigit1.sprite = digits[s1];
            secondDigit2.sprite = digits[s2];
        }
    }
}
