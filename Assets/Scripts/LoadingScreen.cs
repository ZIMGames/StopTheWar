using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    public static LoadingScreen Instance { get; private set; }

    [SerializeField] private BarUI barUI;
    [SerializeField] private Image loadingIconImage;

    private void Awake()
    {
        Instance = this;

        Hide();
    }

    public void SetBarSize(float value)
    {
        barUI.SetAnimatedValue(value);
    }

    public void SetLoadingIcon(Sprite sprite)
    {
        loadingIconImage.sprite = sprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
