using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<LevelGeneration>().GenerateLevel(5,5);
    }
}
