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
        if (DataCarrier.gameMode == "Quick Game")
        {
            switch (DataCarrier.difficulty)
            {
                case "Easy": maxHealth = 5; break;
                case "Medium": maxHealth = 3; break;
                case "Hard": maxHealth = 3; break;
            }
        }
        else
        {
            switch (DataCarrier.hearts)
            {
                case "1": maxHealth = 1; break;
                case "3": maxHealth = 3; break;
                case "5": maxHealth = 5; break;
                case "-1": maxHealth = 9; godMode = true; break;
                default: maxHealth = 3; break;
            }
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
        {
            GameManager.gameManagerInstance.OutOfLives();
        }
    }

    public void UpdateHealth()
    {
        healthText.text = "X" + health.ToString();
    }
}
