using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    Text,
    Img,
}

public class Card : Movable
{
    public int Id;
    public Type currentType;
    public AudioClip audioClip;

    [SerializeField]
    private Level level; // Đảm bảo Level đã được tham chiếu
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Button button;
    [SerializeField] private Animator anim;

    private string animName = Constants.ANIM_IDLE;

    protected override void Start()
    {
        base.Start();

        if (button != null)
        {
            button.onClick.AddListener(FlipCard);
        }
        else
        {
            Debug.LogWarning("Button is not assigned in the Inspector");
        }
        if (image != null) image.enabled = false;
        if (text != null) text.enabled = false;
    }

    public void OnInit(int id, Type type, AudioClip audioClip, string name, Sprite sprite)
    {
        if (image != null) image.enabled = false;
        if (text != null) text.enabled = false;
        this.Id = id;
        this.currentType = type;
        this.audioClip = audioClip;
        text.text = name.ToString();
        image.sprite = sprite;

        ChangeAnim(Constants.ANIM_IDLE);
    }

    public void ResetCard()
    {
        if (image != null) image.enabled = false;
        if (text != null) text.enabled = false;
    }

    public void ChangeAnim(string newAnimName)
    {
        if (anim == null)
        {
            Debug.LogWarning("Animator is not assigned in the Inspector");
            return;
        }

        if (this.animName != newAnimName)
        {
            anim.ResetTrigger(this.animName);
            this.animName = newAnimName;
            anim.SetTrigger(this.animName);
        }
    }
    // button lật bài
    public void FlipCard()
    {
        if (!level.IsIdle) return;

        // Play Sound
        if (SoundManager.Instance != null && audioClip != null)
        {
            SoundManager.Instance.PlaySound(audioClip);
        }
        else
        {
            Debug.LogWarning("SoundManager or AudioClip is missing");
        }

        StartCoroutine(CoSelectCardType());
    }

    private IEnumerator CoSelectCardType()
    {
        ChangeAnim(Constants.ANIM_FLIP);

        // Delay for animation to complete
        yield return new WaitForSeconds(0.5f);

        // Logic to display image or text
        if (currentType == Type.Img && image != null)
        {
            image.enabled = true;
            LogicFlipCard();
        }
        else if (currentType == Type.Text && text != null)
        {
            text.enabled = true;
            LogicFlipCard();
        }
        else
        {
            Debug.LogWarning("Invalid card type or missing UI components");
        }

        yield return new WaitForSeconds(0.5f);

        ChangeAnim(Constants.ANIM_IDLE);
    }

    private void LogicFlipCard()
    {
        if (!level.cardsLogic.Contains(this))
        {
            level.cardsLogic.Add(this);
        }
        level.CheckCard();
    }
    [ContextMenu("anim")]
    public void AnimShuffle()
    {
        anim.enabled = false;

        StartCoroutine(CoMove());

    }

    IEnumerator CoMove()
    {
        yield return StartCoroutine(MoveToPostion(Vector2.zero));

        yield return StartCoroutine(MoveToPostion(positonStart));

        anim.enabled = true;

    }

    public IEnumerator CoAnimFlipStart()
    {

        level.IsIdle = false;

        yield return new WaitForSeconds(0.5f);

        ChangeAnim(Constants.ANIM_FLIP);

        yield return new WaitForSeconds(0.5f);

        ActiveTextOrImg(true);

        yield return new WaitForSeconds(0.5f);

        ChangeAnim(Constants.ANIM_IDLE);

        yield return new WaitForSeconds(0.5f);

        ChangeAnim(Constants.ANIM_FLIP);

        yield return new WaitForSeconds(0.5f);

        ActiveTextOrImg(false);

        yield return new WaitForSeconds(0.5f);

        ChangeAnim(Constants.ANIM_IDLE);
    }
    public void ActiveTextOrImg(bool isActive)
    {
        if (currentType == Type.Img && image != null)
        {
            image.enabled = isActive;
        }
        else if (currentType == Type.Text && text != null)
        {
            text.enabled = isActive;
        }
        else
        {
            Debug.LogWarning("Invalid card type or missing UI components");
        }
    }
}
