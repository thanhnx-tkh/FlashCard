using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int Id;
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

        // thực hiện lập bài, tráo bài
        StartCoroutine(CoAnimStart());
    }

    public IEnumerator CoAnimStart()
    {
        IsIdle = false;
        StartCoroutine(AnimFlipStart());
        yield return new WaitForSeconds(4f);
        StartCoroutine(AnimShuffle());
    }
    public IEnumerator AnimFlipStart()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < cards.Count; i++)
        {
            StartCoroutine(cards[i].CoAnimFlipStart());
        }
    }

    public IEnumerator AnimShuffle()
    {
        yield return new WaitForSeconds(0.5f);
        ShuffleCard();
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].AnimShuffle();
        }
        timeCount = 0f;
        yield return new WaitForSeconds(0.5f);
        IsIdle = true;


    }

    // logic
    public void CheckCard()
    {
        timeCount = 0f;
        iconHand.SetActive(false);

        if (cardsLogic.Count <= 1) return;
        LevelManager.Instance.UITextNumberOfPlays();
        IsIdle = false;

        if (cardsLogic[0].Id == cardsLogic[1].Id)
        {
            StartCoroutine(CODestroyCard(cardsLogic[0], cardsLogic[1]));
            return;
        }
        NumberOfPlays++;
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
    IEnumerator CoNextLevel()
    {

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

    public void ShuffleCard()
    {
        LevelData levelData = LevelManager.Instance.soData.GetLevelData(Id);
        List<ItemCard> cardsData = levelData.cards;
        Shuffle(cardsData);
        for (int i = 0; i < cardsData.Count; i++)
        {
            ItemCard itemCard = cardsData[i];
            cards[i].OnInit(itemCard.id, itemCard.type, itemCard.audioClip, itemCard.name, itemCard.sprite);
        }
    }
    public void Shuffle(List<ItemCard> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            ItemCard temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}
