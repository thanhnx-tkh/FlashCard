using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clipTing;
    public AudioClip error;
    public AudioClip[] clipFalse;
    public static SoundManager Instance { get; private set; }

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
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlaySoundTrue(AudioClip audioClip)
    {
        StartCoroutine(CoPlaySound(audioClip));
    }

    IEnumerator CoPlaySound(AudioClip audioClip)
    {
        PlaySound(clipTing);
        yield return new WaitForSeconds(clipTing.length);
        PlaySound(audioClip);
    }

    public void PlayRandomSoundFalse()
    {
        int indexRandom = Random.Range(0, clipFalse.Length);
        PlaySound(clipFalse[indexRandom]);
    }

    public void PlaySoundError()
    {
        PlaySound(error);
    }
}
