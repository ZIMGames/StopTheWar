using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject settingsPanelGameObject;


    private void Start()
    {
        Hide();

        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);

        musicSlider.SetValueWithoutNotify(musicVolume);
        sfxSlider.SetValueWithoutNotify(sfxVolume);

        UpdateMusicVolume(musicVolume);
        UpdateSfxVolume(sfxVolume);

        musicSlider.onValueChanged.AddListener((float volume) =>
        {
            UpdateMusicVolume(volume);
        });
        sfxSlider.onValueChanged.AddListener((float volume) =>
        {
            UpdateSfxVolume(volume);
        });
    }

    public void Show()
    {
        settingsPanelGameObject.SetActive(true);
    }

    public void Hide()
    {
        settingsPanelGameObject.SetActive(false);
    }

    private void UpdateMusicVolume(float volume)
    {
        BackgroundMusic.SetVolume(volume);
    }

    private void UpdateSfxVolume(float volume)
    {
        SFXMusic.Instance.SetVolume(volume);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }

    private void OnDisable()
    {
        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
}
