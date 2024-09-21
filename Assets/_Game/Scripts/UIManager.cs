using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] arrayUI;
    public Button buttonPlay;
    public Button buttonRetryInLose;
    public Button buttonRetryInWin;
    public Image bGGamePlay;
    public List<Sprite> spritesBG;

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
        buttonRetryInLose.onClick.AddListener(ButtonRetry);
        buttonRetryInWin.onClick.AddListener(ButtonRetry);

        // Active UI Start
        ActiveUI(0);
    }

    public void ButtonPlay()
    {
        LevelManager.Instance.LoadLevel(0);

        // Active UI GamePlay
        ActiveUI(3);
    }
    public void ButtonRetry()
    {
        LevelManager.Instance.RetryLevel();
        
        // Active UI GamePlay        
        ActiveUI(3);
    }

    public void ActiveUI(int indexUi)
    {
        for (int i = 0; i < arrayUI.Length; i++)
        {
            arrayUI[i].SetActive(false);
            if(i==3){
                bGGamePlay.sprite = spritesBG[Random.Range(0,spritesBG.Count)];
            }
        }
        arrayUI[indexUi].SetActive(true);
    }
}
