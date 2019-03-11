using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip moveSfx;
    public AudioClip moveSpecialSfx;
    public AudioClip gameEndSfx;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMoveSfx()
    {
        audioSource.PlayOneShot(moveSfx);
    }

    public void PlayMoveSpecialSfx()
    {
        audioSource.PlayOneShot(moveSpecialSfx);
    }

    public void PlayGameEndSfx()
    {
        audioSource.PlayOneShot(gameEndSfx);
    }

}
