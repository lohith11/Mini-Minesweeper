using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas, pauseCanvas, winCanvas, deathCanvas;
    Vector2 ballVelocity;
    public void PauseGame()
    {
        if (GameManager.gameManagerInstance.gameStarted)
            ballVelocity = FindObjectOfType<DragNShoot>().gameObject.GetComponent<Rigidbody2D>().velocity;
        GameManager.gameManagerInstance.gamePaused = true;
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        GameManager.gameManagerInstance.gamePaused = false;
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
        if (GameManager.gameManagerInstance.gameStarted)
        {
            FindObjectOfType<DragNShoot>().gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            FindObjectOfType<DragNShoot>().gameObject.GetComponent<Rigidbody2D>().velocity = ballVelocity;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void WinGame()
    {
        Time.timeScale = 1;
        gameCanvas.SetActive(false);
        winCanvas.SetActive(true);
    }
    public void LoseGame()
    {
        Time.timeScale = 1;
        gameCanvas.SetActive(false);
        deathCanvas.SetActive(true);
    }
    public void MainMenu()
    {
        DataCarrier.difficulty = "Medium";
        DataCarrier.hearts = "3";
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
