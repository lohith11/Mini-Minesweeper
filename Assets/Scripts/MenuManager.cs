using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    public void StartGame()
    {
        DataCarrier.gridSize = (int)slider.value;
        SceneManager.LoadScene("Level");
    }
}
