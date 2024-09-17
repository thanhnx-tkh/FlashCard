using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> PrefabLevels;
    public Level curLevel;
    public int indexLevel;
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        indexLevel = 0; 
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void LoadLevel(int indexLevel)
    {
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[indexLevel], transform);
        this.indexLevel = indexLevel;
    }

    public void NextLevel()
    {
        indexLevel++;
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[indexLevel], transform);
    }

    public void RetryLevel()
    {
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[indexLevel], transform);
    }
}
