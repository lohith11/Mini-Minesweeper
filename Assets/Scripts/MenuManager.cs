using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject menuCanvas, settingsCanvas;

    public void StartGame()
    {
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

    public void AudioToggleChanged()
    {
        DataCarrier.musicEnabled = !DataCarrier.musicEnabled;
        Debug.Log("Music enabled: " + DataCarrier.musicEnabled);
        if (DataCarrier.musicEnabled)
            AudioManager.audioManagerInstance.ChangeTrack();
        else
            AudioManager.audioManagerInstance.StopAll();
    }

    public void DifficultyChanged(string difficulty)
    {
        DataCarrier.difficulty = difficulty;
        Debug.Log("Difficulty: " + DataCarrier.difficulty);
    }
}
