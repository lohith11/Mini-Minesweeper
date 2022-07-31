using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject quickMask, customMask, zenMask;

    void Start()
    {
        if (PlayerPrefs.GetInt("FirstLaunch") == 0)
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);

            PlayerPrefs.SetInt("Quick.GridSize", 20);
            PlayerPrefs.SetString("Quick.Difficulty", "Medium");

            PlayerPrefs.SetInt("Zen.GridSize", 50);
            PlayerPrefs.SetString("Zen.Difficulty", "Medium");
            PlayerPrefs.SetInt("Zen.Hearts", -1);

            PlayerPrefs.SetInt("Custom.GridSize", 50);
            PlayerPrefs.SetString("Custom.Difficulty", "Medium");
            PlayerPrefs.SetInt("Custom.Hearts", 3);

            PlayerPrefs.SetString("GameMode", "Quick");
        }
        else
        {

        }
    }

    public void GameModeSelected(string gameMode = "Quick")
    {
        PlayerPrefs.SetString("GameMode", gameMode);

        if (gameMode == "Quick")
        {
            PlayerPrefs.SetInt("Quick.GridSize", Random.Range(20, 51));
            quickMask.SetActive(true);

            int index;
            switch (PlayerPrefs.GetString("Quick.Difficulty"))
            {
                case "Easy": index = 0; break;
                case "Medium": index = 1; break;
                case "Hard": index = 2; break;
                default: index = 1; break;
            }
            quickMask.transform.GetChild(0).GetComponent<TMP_Text>().text = "Grid Size: " + PlayerPrefs.GetInt("Quick.GridSize");
            quickMask.transform.GetChild(1).transform.GetChild(0).GetComponent<HorizontalSelector>().index = index;

            customMask.SetActive(false);
            zenMask.SetActive(false);
        }
        else if (gameMode == "Custom")
        {
            customMask.SetActive(true);

            int index;
            switch (PlayerPrefs.GetString("Custom.Difficulty"))
            {
                case "Easy": index = 0; break;
                case "Medium": index = 1; break;
                case "Hard": index = 2; break;
                default: index = 1; break;
            }
            quickMask.transform.GetChild(0).transform.GetChild(0).GetComponent<HorizontalSelector>().index = index;
            //quickMask.transform.GetChild(0).GetComponent<TMP_Text>().text = "Grid Size: " + PlayerPrefs.GetInt("Quick.GridSize");

            quickMask.SetActive(false);
            zenMask.SetActive(false);
        }
        else if (gameMode == "Zen")
        {
            zenMask.SetActive(true);
            quickMask.SetActive(false);
            customMask.SetActive(false);
        }
    }

    public void DifficultyChanged(string difficulty = "Medium")
    {
        string gameMode = PlayerPrefs.GetString("GameMode");
        PlayerPrefs.SetString(gameMode + ".Difficulty", difficulty);
    }
}
