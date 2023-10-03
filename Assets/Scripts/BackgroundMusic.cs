using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    public static void Mute()
    {
        Instance.audioSource.mute = true;
    }

    public static void Unmute()
    {
        Instance.audioSource.mute = false;
    }

    public static void SetVolume(float volume)
    {
        Instance.audioSource.volume = volume;
    }
}
