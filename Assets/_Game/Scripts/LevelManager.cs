using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<Level> PrefabLevels;
    public Level curLevel;
    public int indexLevel;
    public Text textLevel;
    public Text textNextLevel;

    public Text textNumberOfPlays;
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
        UITextLevel();
        UITextNumberOfPlays();
    }

    public void NextLevel()
    {
        indexLevel++;
        if (curLevel != null) Destroy(curLevel.gameObject);

        // Active UI Win 
        if (indexLevel == PrefabLevels.Count)
        {
            UIManager.Instance.ActiveUI(2);
            return;
        }

        curLevel = Instantiate(PrefabLevels[indexLevel], transform);

        // Active UI Game Play
        UIManager.Instance.ActiveUI(3);
        UITextLevel();
        UITextNumberOfPlays();
    }

    public void RetryLevel()
    {
        if (indexLevel == PrefabLevels.Count)
        {
            indexLevel = 0;
        }
        if (curLevel != null) Destroy(curLevel.gameObject);
        curLevel = Instantiate(PrefabLevels[indexLevel], transform);
        UITextLevel();
        UITextNumberOfPlays();
    }

    public void Lose()
    {
        if (curLevel != null) Destroy(curLevel.gameObject);
    }

    public void UITextLevel()
    {
        textLevel.text = "Level: " + (indexLevel + 1).ToString();
    }
    public void UITextNextLevel()
    {
        textNextLevel.text = "Level: " + (indexLevel + 2).ToString();
    }
    public void UITextNumberOfPlays()
    {
        textNumberOfPlays.text = "Number Of Plays: " + (3 - curLevel.NumberOfPlays).ToString();
    }
}
