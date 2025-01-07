using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hyb.Utils;
public class AudioManager : ManualSingletonMono<AudioManager>
{
    [Header("------------Audio------------")]
    public AudioSource audioSource;
    public AudioClip soundBackground;
    public AudioClip drag;
    public AudioClip failGame;
    public AudioClip star;

    public override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundBackground;
        audioSource.Play();
    }
    public void PlayAudioBackground()
    {
        audioSource.clip = soundBackground;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayAudioFailGame()
    {
        audioSource.clip = failGame;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void PlayAudioCollect()
    {
        audioSource.PlayOneShot(drag, 0.8f);
    }
    public void PlayAudioStart()
    {
        audioSource.PlayOneShot(star);
    }


}
