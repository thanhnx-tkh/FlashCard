using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Type
{
    Text,
    Img,
}
public class Card : MonoBehaviour
{
    public int Id;
    public Type currentType;
    public AudioClip audioClip;
    
    [SerializeField]
    private Level level;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Button button;
    [SerializeField] Animator anim;
    private string animName = Constants.ANIM_IDLE;

    private void Start()
    {
        button.onClick.AddListener(FlipCard);
        OnInit();
    }
    public void OnInit()
    {
        image.enabled = false;
        text.enabled = false;
        ChangeAnim(Constants.ANIM_IDLE);
    }
     public void ResetCard()
    {
        image.enabled = false;
        text.enabled = false;
    }

    public void ChangeAnim(string animName)
    {
        if (this.animName != animName)
        {
            anim.ResetTrigger(this.animName);
            this.animName = animName;
            anim.SetTrigger(this.animName);
        }
    }
    public void FlipCard()
    {
        if (!level.IsIdle) return;

        //Play Sound
        SoundManager.Instance.PlaySound(audioClip);

        StartCoroutine(CoSelectCardType());
        
    }
    private IEnumerator CoSelectCardType(){

        ChangeAnim(Constants.ANIM_FLIP);

        yield return new WaitForSeconds(0.5f);

        
        if (currentType == Type.Img)
        {
            image.enabled = true;
            LogicFlipCard();
        }
        else if (currentType == Type.Text)
        {
            text.enabled = true;
            LogicFlipCard();
        }
        else
        {
            Debug.LogWarning("Error");
        }

        yield return new WaitForSeconds(0.5f);

        ChangeAnim(Constants.ANIM_IDLE);

    }
    private void LogicFlipCard()
    {
        // to do : play sound  
        if(!level.cardsLogic.Contains(this)) level.cardsLogic.Add(this);
        level.CheckCard();
    }
}
