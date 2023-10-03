using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OpenPetition_Bar
{
    private int value;
    private int maxValue;

    public event System.Action OnValueChanged;
    public event System.Action OnMaxValueReached;

    public OpenPetition_Bar(int value, int maxValue)
    {
        this.value = value;
        this.maxValue = maxValue;
    }

    public void AddValue(int _value)
    {
        value += _value;
        if (value >= maxValue)
        {
            value = maxValue;
            OnMaxValueReached?.Invoke();
        }
        OnValueChanged?.Invoke();
    }

    public int GetValue()
    {
        return value;
    }

    public int GetMaxValue()
    {
        return maxValue;
    }

    public float GetValueNormalized()
    {
        return (float)value / maxValue;
    }
}
