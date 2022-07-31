using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas, pauseCanvas, winCanvas, deathCanvas;
    Vector2 ballVelocity;
    public void PauseGame()
    {
        AudioManager.audioManagerInstance.Play("Click");
        if (GameManager.gameManagerInstance.gameStarted)
            ballVelocity = FindObjectOfType<DragNShoot>().gameObject.GetComponent<Rigidbody2D>().velocity;
        GameManager.gameManagerInstance.gamePaused = true;
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        AudioManager.audioManagerInstance.Play("Click");
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
        AudioManager.audioManagerInstance.Play("Click");
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
        AudioManager.audioManagerInstance.Play("Click");
        Time.timeScale = 1;
        SceneManager.LoadScene("New Menu");
    }
}
