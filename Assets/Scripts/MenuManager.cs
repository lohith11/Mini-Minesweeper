using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject menuCanvas, settingsCanvas;

    void Start()
    {
        DataCarrier.difficulty = "Medium";
    }

    public void StartGame(string gameMode = "Custom Game")
    {
        DataCarrier.gameMode = gameMode;
        DataCarrier.gridSize = (int)slider.value;
        SceneManager.LoadScene("Level");
    }

    public void OpenSettings()
    {
        menuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void BackToMenu()
    {
        menuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DifficultyChanged(string difficulty)
    {
        DataCarrier.difficulty = difficulty;
        Debug.Log("Difficulty: " + DataCarrier.difficulty);
    }

    public void HeartsChanged(string hearts)
    {
        DataCarrier.hearts = hearts;
        Debug.Log("Hearts: " + DataCarrier.hearts);
    }
}
