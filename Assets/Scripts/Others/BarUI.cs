using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    private int value;
    private int maxValue;
    private float animatedValueNormalized;

    public event System.Action OnValueChanged;

    [SerializeField] private Image fillImage;

    public void Setup(int value, int maxValue)
    {
        this.maxValue = maxValue;
        fillImage.fillAmount = (float)value / maxValue;

        SetValue(value);
    }

    public void SetFillImageColor(Color32 color)
    {
        fillImage.color = color;
    }

    public void SetAnimatedValue(float animatedValue)
    {
        animatedValueNormalized = animatedValue;
    }


    public int GetValue()
    {
        return value;
    }

    public int GetMaxValue()
    {
        return maxValue;
    }

    public void AddValue(int _value)
    {
        SetValue(value + _value);
    }

    public void SetValue(int newValue)
    {
        value = newValue;
        if (value >= maxValue)
        {
            value = maxValue;
        }
        animatedValueNormalized = (float)value / maxValue;
        OnValueChanged?.Invoke();
    }

    private void LateUpdate()
    {
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, animatedValueNormalized, .5f);
    }
}
