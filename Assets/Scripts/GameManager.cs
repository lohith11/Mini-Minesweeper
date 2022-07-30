using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Cell[,] masterLevel;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] float gameStartTimeout;
    [SerializeField] CinemachineVirtualCamera topView, ballFollow;
    [SerializeField] GameObject pointingArrow, healthImage, timer;
    [SerializeField] TMP_Text timerText;
    GameObject ball;
    public int gridSize;
    public static GameManager gameManagerInstance;
    [HideInInspector] public bool gameStarted = false, gameEnded = false, gamePaused = false, ballSpawned = false, sneakpeek = false;
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

        if (Input.GetKeyDown(KeyCode.Space))
            SneakPeak();
    }

    public void StartGame(Vector2 coordinates)
    {
        startCoordinates = new Vector2Int((int)coordinates.x, (int)coordinates.y);
        ballSpawned = true;
        ball = Instantiate(ballPrefab, coordinates, Quaternion.identity);
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
            string time = string.Format("{0:00}:{1:00}", (timeCounter / 60) % 60, timeCounter % 60);
            timerText.text = time;
        }
    }

    void SneakPeak()
    {
        sneakpeek = !sneakpeek;
        GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
        for (int i = 0; i < cells.Length; i++)
        {
            float a;
            if (sneakpeek)
                a = 0.0f;
            else
                a = 1.0f;
            Color tempColor = cells[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color;
            tempColor.a = a;
            cells[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color = tempColor;
        }
    }
}
