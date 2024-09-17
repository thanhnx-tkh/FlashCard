using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] arrayUI;
    public Button buttonPlay;
    public Button buttonRetry;
    public Button buttonRetry1;
    public Button buttonNextLevel;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
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

    private void Start()
    {
        buttonPlay.onClick.AddListener(ButtonPlay);
        buttonRetry.onClick.AddListener(ButtonRetry);
        buttonRetry1.onClick.AddListener(ButtonRetry);
        buttonNextLevel.onClick.AddListener(ButtonNext);
        ActiveUI(0);
    }
    public void ActiveUI(int indexUi)
    {
        for (int i = 0; i < arrayUI.Length; i++)
        {
            arrayUI[i].SetActive(false);
        }
        arrayUI[indexUi].SetActive(true);
    }
    public void ButtonPlay()
    {
        LevelManager.Instance.LoadLevel(0);
        ActiveUI(4);
    }
    public void ButtonRetry()
    {
        LevelManager.Instance.RetryLevel();
        ActiveUI(4);

    }
    public void ButtonNext()
    {
        LevelManager.Instance.NextLevel();
        ActiveUI(4);

    }
}
