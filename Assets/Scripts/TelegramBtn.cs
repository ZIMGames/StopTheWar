using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegramBtn : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("telegram"))
            gameObject.SetActive(false);
    }

    public void OpenTelegramLink()
    {
        Application.OpenURL("https://t.me/stopwargame");
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("telegram", 0);
    }
}
