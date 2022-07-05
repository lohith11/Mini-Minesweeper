using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth, tilesPerHealthPoint;
    [SerializeField] List<Image> hearts;
    int health, tilesHit = 0;
    public static HealthManager healthManagerInstance;
    void Awake()
    {
        healthManagerInstance = this;
        health = maxHealth;
    }

    public void GameStarted()
    {
        tilesHit = 0;
        //Debug.Log("Game started");
        for (int i = 0; i < maxHealth; i++)
        {
            hearts[i].enabled = true;
        }
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
            hearts[health - 1].enabled = true;
        }
    }
    public void BombHit()
    {
        health--;
        tilesHit = 0;
        hearts[health].enabled = false;
        if (health <= 0)
        {
            GameManager.gameManagerInstance.OutOfLives();
        }
    }
}
