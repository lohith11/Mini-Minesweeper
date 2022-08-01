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
        gridSize = PlayerPrefs.GetInt(PlayerPrefs.GetString("GameMode") + ".GridSize");
        Debug.Log("Grid size: " + gridSize);
    }
    void Start()
    {
        topView.transform.position = new Vector3(gridSize / 2f, (gridSize / 2f) - 0.5f, -10);
        topView.m_Lens.OrthographicSize = (gridSize / 2) + 2;
        masterLevel = LevelGeneration.levelGenerationInstance.GenerateLevel(gridSize, gridSize);
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
                ballFollow.enabled = true;
                topView.enabled = false;
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

    public IEnumerator FlagTouched()
    {
        AudioManager.audioManagerInstance.Play("Win");
        gameStarted = false;
        gameEnded = true;
        ballFollow.enabled = false;
        topView.enabled = true;

        yield return null;
        GetComponent<GameMenuManager>().WinGame();
    }
    public IEnumerator OutOfLives()
    {
        AudioManager.audioManagerInstance.Play("Lose");
        gameStarted = false;
        gameEnded = true;
        ballFollow.enabled = false;
        topView.enabled = true;

        yield return null;
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
                a = 1.0f;
            else
                a = 1.0f;
            Color tempColor = cells[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color;
            tempColor.a = a;
            cells[i].transform.GetChild(2).GetComponent<SpriteRenderer>().color = tempColor;
        }
    }
}
