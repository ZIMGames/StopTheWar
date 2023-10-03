using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPetition_BarUI : MonoBehaviour
{
    private OpenPetition_Bar bar;

    [SerializeField] private RectTransform barMaskRectTransform;
    [SerializeField] private Text targetText, progressText, progressPercentText;
    private float maxValueWidth;

    private void Awake()
    {
        maxValueWidth = barMaskRectTransform.sizeDelta.x;
    }

    public void Setup(OpenPetition_Bar bar, string petitionString)
    {
        this.bar = bar;

        targetText.text = "<b>" + bar.GetMaxValue() + "</b> target";

        bar.OnValueChanged += Bar_OnValueChanged;
        UpdateVisuals();
    }

    private void Bar_OnValueChanged()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        float valueNormalized = bar.GetValueNormalized();
        barMaskRectTransform.sizeDelta = new Vector2(valueNormalized * maxValueWidth, barMaskRectTransform.sizeDelta.y);
        progressText.text = "<b>" + bar.GetValue() + "</b> signed";
        progressPercentText.text = "<b>" + Mathf.RoundToInt(valueNormalized * 100) + "%</b> reached";
    }
}
