using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class DecoderGame_ActivatorSingle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private System.Action callback;

    private Light2D light2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        light2D = GetComponentInChildren<Light2D>();
    }

    public void UpdateState(bool state)
    {
        spriteRenderer.color = state ? Color.green : Color.red;
        light2D.color = state ? Color.green : Color.red;
    }

    public void OnMouseDown()
    {
        SFXMusic.Instance.PlayCorrectAnswerSound();
        callback?.Invoke();
    }

    public void SetOnClickCallback(System.Action callback)
    {
        this.callback = callback;
    }
}
