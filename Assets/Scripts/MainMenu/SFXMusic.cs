using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXMusic : MonoBehaviour
{
    public static SFXMusic Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip applauseClip, disappointedCrowdClip;
    public AudioClip[] audioClipArray;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBoxDrop()
    {
        audioSource.PlayOneShot(audioClipArray[0]);
    }

    public void PlayCoinsDrop()
    {
        audioSource.PlayOneShot(audioClipArray[1]);
    }

    public void PlayPowerPointsDrop()
    {
        audioSource.PlayOneShot(audioClipArray[2]);
    }

    public void PlayGemsDrop()
    {
        audioSource.PlayOneShot(audioClipArray[3]);
    }

    public void PlayUpgrade()
    {
        audioSource.PlayOneShot(audioClipArray[4]);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(audioClipArray[5]);
    }

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(audioClipArray[6]);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(audioClipArray[7]);
    }

    public void PlayRollSound()
    {
        audioSource.PlayOneShot(audioClipArray[8]);
    }

    public void PlayNewHeroSound()
    {
        audioSource.PlayOneShot(audioClipArray[9]);
    }

    public void PlayHeavyShoot()
    {
        audioSource.PlayOneShot(audioClipArray[10]);
    }

    public void PlayCoinImpact()
    {
        audioSource.PlayOneShot(audioClipArray[11]);
    }

    public void PlayBombSwepped()
    {
        audioSource.PlayOneShot(audioClipArray[12]);
    }

    public void PlayBomb()
    {
        audioSource.PlayOneShot(audioClipArray[13]);
    }

    public void PlayExplosion()
    {
        audioSource.PlayOneShot(audioClipArray[14]);
    }

    public void PlayRocketShoot()
    {
        audioSource.PlayOneShot(audioClipArray[15]);
    }

    public void PlayGoodAfternoon()
    {
        audioSource.PlayOneShot(audioClipArray[16]);
    }

    public void PlayRocketBarrage()
    {
        audioSource.PlayOneShot(audioClipArray[17]);
    }

    public void PlayCorrectAnswerSound()
    {
        PlayCoinImpact();
    }

    public void PlayApplauses()
    {
        audioSource.PlayOneShot(applauseClip);
    }

    public void PlayDisappointedCrowd()
    {
        audioSource.PlayOneShot(disappointedCrowdClip);
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void StopPlayingAllClips()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
