using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public List<Card> cards;

    public List<Card> cardsLogic;

    public bool IsIdle { get; set; }

    private float timeCount;
    private float time;
    public int NumberOfPlays { get; set; }

    [SerializeField] private GameObject iconHand;
    private void Start()
    {
        NumberOfPlays = 0;
        timeCount = 0f;
        time = 5f;
        iconHand.SetActive(false);
        IsIdle = true;
        StartCoroutine(AnimShuffle());
    }
    public IEnumerator AnimShuffle()
    {
        IsIdle = false;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].ChangeAnim(Constants.ANIM_MOVE);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].ChangeAnim(Constants.ANIM_IDLE);
        }
        IsIdle = true;

    }

    // logic
    public void CheckCard()
    {
        timeCount = 0f;
        iconHand.SetActive(false);

        if (cardsLogic.Count <= 1) return;
        NumberOfPlays++;
        LevelManager.Instance.UITextNumberOfPlays();
        IsIdle = false;

        if (cardsLogic[0].Id == cardsLogic[1].Id)
        {
            StartCoroutine(CODestroyCard(cardsLogic[0], cardsLogic[1]));
            return;
        }
        StartCoroutine(COResetCard());

    }
    private void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > time)
        {
            ActiveIconHand();
        }
        if (cards.Count == 0) StartCoroutine(CoNextLevel());
    }
    IEnumerator CoNextLevel(){

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ActiveUI(4);
        LevelManager.Instance.UITextNextLevel();

        yield return new WaitForSeconds(1f);
        LevelManager.Instance.NextLevel();

    }
    private void ActiveIconHand()
    {
        if (cards.Count == 0) return;
        iconHand.SetActive(true);
        iconHand.transform.position = cards[0].transform.position - new Vector3(80, 80, 0);
    }

    private IEnumerator CODestroyCard(Card card1, Card card2)
    {
        SoundManager.Instance.PlaySoundTrue(card1.audioClip);

        yield return new WaitForSeconds(1f);

        cardsLogic.Remove(card1);
        cardsLogic.Remove(card2);
        cards.Remove(card1);
        cards.Remove(card2);
        Destroy(card1.gameObject);  
        Destroy(card2.gameObject);
        IsIdle = true;
    }

    private IEnumerator COResetCard()
    {
        yield return new WaitForSeconds(1f);
        int indexRandom = Random.Range(1, 3);

        switch (indexRandom)
        {
            case 1:
                SoundManager.Instance.PlaySoundError();

                cardsLogic[1].ChangeAnim(Constants.ANIM_VIBRATE);
                cardsLogic[0].ChangeAnim(Constants.ANIM_VIBRATE);

                break;
            case 2:
                SoundManager.Instance.PlayRandomSoundFalse();
                break;
        }

        yield return new WaitForSeconds(1f);
        cardsLogic[1].ChangeAnim(Constants.ANIM_FLIP);
        cardsLogic[0].ChangeAnim(Constants.ANIM_FLIP);

        yield return new WaitForSeconds(0.5f);
        cardsLogic[0].ResetCard();
        cardsLogic[1].ResetCard();

        yield return new WaitForSeconds(0.5f);
        cardsLogic[1].ChangeAnim(Constants.ANIM_IDLE);
        cardsLogic[0].ChangeAnim(Constants.ANIM_IDLE);
        cardsLogic.Clear();

        IsIdle = true;

        //Active UI lose and logic lose
        if (NumberOfPlays == 3)
        {
            UIManager.Instance.ActiveUI(1);
            LevelManager.Instance.Lose();
        }
    }
}
