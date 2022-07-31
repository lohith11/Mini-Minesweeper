using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager healthManagerInstance;
    [SerializeField] int tilesPerHealthPoint;
    [SerializeField] TMP_Text healthText;
    int maxHealth, health, tilesHit = 0;
    bool godMode = false;
    void Awake()
    {
        healthManagerInstance = this;

        switch (PlayerPrefs.GetString("GameMode"))
        {
            case "Quick":
                switch (PlayerPrefs.GetString("Quick.Difficulty"))
                {
                    case "Easy": maxHealth = 5; break;
                    case "Medium": maxHealth = 3; break;
                    case "Hard": maxHealth = 2; break;
                    default: maxHealth = 3; break;
                }
                break;
            case "Zen": maxHealth = PlayerPrefs.GetInt("Zen.Hearts"); break;
            case "Custom": maxHealth = PlayerPrefs.GetInt("Custom.Hearts"); break;
        }
        Debug.Log("Max Health: " + maxHealth);
        if (maxHealth == -1)
        {
            godMode = true;
            maxHealth = 9;
        }
        health = maxHealth;
    }

    public void GameStarted()
    {
        tilesHit = 0;
        UpdateHealth();
    }

    public void TileRevealed()
    {
        tilesHit++;
        if (tilesHit == tilesPerHealthPoint)
        {
            tilesHit = 0;
            health++;
            if (health >= maxHealth)
                health = maxHealth;
        }
        UpdateHealth();
    }
    public void BombHit()
    {
        if (godMode)
            return;
        health--;
        tilesHit = 0;
        UpdateHealth();
        if (health <= 0)
            StartCoroutine(GameManager.gameManagerInstance.OutOfLives());
    }

    public void UpdateHealth()
    {
        healthText.text = "X" + health.ToString();
    }
}
