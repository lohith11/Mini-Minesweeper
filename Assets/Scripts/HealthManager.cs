using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int tilesPerHealthPoint;
    [SerializeField] List<Sprite> digits;
    [SerializeField] Image digit1, digit2;
    int maxHealth = 3, health, tilesHit = 0;
    bool godMode = false;
    public static HealthManager healthManagerInstance;
    void Awake()
    {
        healthManagerInstance = this;
        switch (DataCarrier.hearts)
        {
            case "1": maxHealth = 1; break;
            case "3": maxHealth = 3; break;
            case "5": maxHealth = 5; break;
            case "-1": maxHealth = 99; godMode = true; break;
            default: maxHealth = 3; break;
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
        int two = health % 10;
        int one = (health / 10) % 10;
        digit1.sprite = digits[one];
        digit2.sprite = digits[two];
    }
}
