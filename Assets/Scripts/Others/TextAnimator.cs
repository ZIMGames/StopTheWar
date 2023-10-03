using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimator : MonoBehaviour
{
    public string[] textAnimationArray;
    public float animationRate;

    private int currentIndex = 0;

    private Text text;


    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        InvokeRepeating("Process", 0f, animationRate);
    }

    private void Process()
    {
        text.text = GetNextTextAnimation();
    }

    private string GetNextTextAnimation()
    {
        currentIndex = (currentIndex + 1) % textAnimationArray.Length;
        return textAnimationArray[currentIndex];
    }
}
