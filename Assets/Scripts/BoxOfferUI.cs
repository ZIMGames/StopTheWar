using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxOfferUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private int boxIndex;
    [SerializeField] private BoxSystem boxSystem;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            AdManager.ShowRewardedVideo(() =>
            {
                boxSystem.OpenBox(boxIndex);
            }, true);
        });
    }
}
