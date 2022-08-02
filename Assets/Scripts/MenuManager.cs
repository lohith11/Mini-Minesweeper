using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] GameObject quickMask, customMask, zenMask;

    [SerializeField] RectTransform content;
    [SerializeField] float animationDelay;

    void Start()
    {
        if (PlayerPrefs.GetInt("FirstLaunch") == 0)
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);

            PlayerPrefs.SetInt("Quick.GridSize", 30);
            PlayerPrefs.SetString("Quick.Difficulty", "Medium");

            PlayerPrefs.SetInt("Zen.GridSize", 50);
            PlayerPrefs.SetString("Zen.Difficulty", "Medium");
            PlayerPrefs.SetInt("Zen.Hearts", -1);

            PlayerPrefs.SetInt("Custom.GridSize", 30);
            PlayerPrefs.SetString("Custom.Difficulty", "Medium");
            PlayerPrefs.SetInt("Custom.Hearts", 3);

            PlayerPrefs.SetString("GameMode", "Quick");
        }

        PlayerPrefs.SetString("GameMode", "Unselected");
    }

    void Update()
    {
        float x = Mathf.Clamp(content.localPosition.x, -450f, 0f);
        content.localPosition = new Vector3(x, content.localPosition.y, content.localPosition.z);
    }

    public void GameModeSelected(string gameMode)
    {

        if (gameMode == PlayerPrefs.GetString("GameMode") && gameMode == "Quick")
        {
            AudioManager.audioManagerInstance.Play("Click");
            PlayerPrefs.SetInt("Quick.GridSize", Random.Range(20, 51));
            quickMask.transform.GetChild(0).GetComponent<TMP_Text>().text = "Grid Size: " + PlayerPrefs.GetInt("Quick.GridSize");
            quickMask.transform.GetChild(0).GetComponent<Animator>().SetTrigger("SlideIn");
        }

        if (gameMode == PlayerPrefs.GetString("GameMode"))
            return;

        ResePositions();
        PlayerPrefs.SetString("GameMode", gameMode);
        AudioManager.audioManagerInstance.Play("Click");
        if (gameMode == "Quick")
        {
            PlayerPrefs.SetInt("Quick.GridSize", Random.Range(20, 51));
            customMask.SetActive(false);
            zenMask.SetActive(false);
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
            quickMask.transform.GetChild(1).transform.GetChild(0).GetComponent<HorizontalSelector>().defaultIndex = index;
        }

        else if (gameMode == "Custom")
        {
            quickMask.SetActive(false);
            zenMask.SetActive(false);
            customMask.SetActive(true);

            int difficultyIndex, healthIndex;
            switch (PlayerPrefs.GetString("Custom.Difficulty"))
            {
                case "Easy": difficultyIndex = 0; break;
                case "Medium": difficultyIndex = 1; break;
                case "Hard": difficultyIndex = 2; break;
                default: difficultyIndex = 1; break;
            }
            switch (PlayerPrefs.GetInt("Custom.Hearts"))
            {
                case 1: healthIndex = 0; break;
                case 3: healthIndex = 1; break;
                case 5: healthIndex = 2; break;
                case -1: healthIndex = 3; break;
                default: healthIndex = 1; break;
            }
            customMask.transform.GetChild(0).transform.GetChild(0).GetComponent<SliderManager>().mainSlider.value = PlayerPrefs.GetInt("Custom.GridSize");
            customMask.transform.GetChild(1).transform.GetChild(0).GetComponent<HorizontalSelector>().defaultIndex = difficultyIndex;
            customMask.transform.GetChild(2).transform.GetChild(0).GetComponent<HorizontalSelector>().defaultIndex = healthIndex;
        }

        else if (gameMode == "Zen")
        {
            quickMask.SetActive(false);
            customMask.SetActive(false);
            zenMask.SetActive(true);
        }
        StartCoroutine(AnimateStuff());
    }

    public void DifficultyChanged(string difficulty)
    {
        AudioManager.audioManagerInstance.Play("Click");
        string gameMode = PlayerPrefs.GetString("GameMode");
        PlayerPrefs.SetString(gameMode + ".Difficulty", difficulty);
    }
    public void HeartsChanged(string hearts)
    {
        AudioManager.audioManagerInstance.Play("Click");
        string gameMode = PlayerPrefs.GetString("GameMode");
        PlayerPrefs.SetInt(gameMode + ".Hearts", int.Parse(hearts));
    }
    public void GridSizeChanged()
    {
        AudioManager.audioManagerInstance.Play("Click");
        string gameMode = PlayerPrefs.GetString("GameMode");
        PlayerPrefs.SetInt(gameMode + ".GridSize", (int)slider.value);
    }

    public void StartGame()
    {
        AudioManager.audioManagerInstance.Play("Click");
        SceneManager.LoadScene("Level");
    }

    public void ResePositions()
    {
        RectTransform rt;
        for (int i = 0; i < quickMask.transform.childCount; i++)
        {
            rt = quickMask.transform.GetChild(i).GetComponent<RectTransform>();
            rt.localPosition = new Vector3(-500, rt.localPosition.y, rt.localPosition.z);
        }
        for (int i = 0; i < customMask.transform.childCount; i++)
        {
            rt = customMask.transform.GetChild(i).GetComponent<RectTransform>();
            rt.localPosition = new Vector3(-500, rt.localPosition.y, rt.localPosition.z);
        }
        for (int i = 0; i < zenMask.transform.GetChild(0).childCount; i++)
        {
            rt = zenMask.transform.GetChild(0).GetChild(i).GetComponent<RectTransform>();
            rt.localPosition = new Vector3(-500, rt.localPosition.y, rt.localPosition.z);
        }
        rt = zenMask.transform.GetChild(1).GetComponent<RectTransform>();
        rt.localPosition = new Vector3(-500, rt.localPosition.y, rt.localPosition.z);
    }

    public IEnumerator AnimateStuff()
    {
        Animator animator;
        switch (PlayerPrefs.GetString("GameMode"))
        {
            case "Quick":
                for (int i = 0; i < quickMask.transform.childCount; i++)
                {
                    animator = quickMask.transform.GetChild(i).GetComponent<Animator>();
                    animator.SetTrigger("SlideIn");
                    yield return new WaitForSeconds(animationDelay);
                }
                break;
            case "Zen":
                for (int i = 0; i < zenMask.transform.GetChild(0).childCount; i++)
                {
                    animator = zenMask.transform.GetChild(0).GetChild(i).GetComponent<Animator>();
                    animator.SetTrigger("SlideIn");
                    yield return new WaitForSeconds(animationDelay);
                }
                animator = zenMask.transform.GetChild(1).GetComponent<Animator>();
                animator.SetTrigger("SlideIn");
                break;
            case "Custom":
                for (int i = 0; i < customMask.transform.childCount; i++)
                {
                    animator = customMask.transform.GetChild(i).GetComponent<Animator>();
                    animator.SetTrigger("SlideIn");
                    yield return new WaitForSeconds(animationDelay);
                }
                break;
        }
        yield return null;
    }
}
