using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPetition : MonoBehaviour
{
    [SerializeField] private OpenPetition_BarUI barUI;
    [SerializeField] private Text petitionText;

    public static int value = 0;
    public static int maxValue = 5;
    public static int valueStepPerClick = 1;
    public static string petitionString = "sas";
    public static System.Action onPetitionSignedCompletelyCallback;

    public static void SetStaticParams(int _value, int _maxValue, int _valueStepPerClick,
        string _petitionString)

    {
        value = _value;
        maxValue = _maxValue;
        valueStepPerClick = _valueStepPerClick;
        petitionString = _petitionString;
    }

    private OpenPetition_Bar bar;

    private void Start()
    {
        bar = new OpenPetition_Bar(value, maxValue);
        bar.OnMaxValueReached += Bar_OnMaxValueReached;

        petitionText.text = petitionString;

        barUI.Setup(bar, petitionString);
    }

    private void Bar_OnMaxValueReached()
    {
        onPetitionSignedCompletelyCallback?.Invoke();
    }

    public void Click()
    {
        bar.AddValue(valueStepPerClick);
        SFXMusic.Instance.PlayCorrectAnswerSound();
    }
}
