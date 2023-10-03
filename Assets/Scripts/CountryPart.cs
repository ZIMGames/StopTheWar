using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class CountryPart : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animation anim;
    private CountryVisual country;
    private System.Action callback;

    private bool active = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animation>();
        GetComponent<Button_Sprite>().ClickFunc = () =>
        {
            if (active)
            {
                callback?.Invoke();
            }
        };
    }


    public void SetCountry(CountryVisual country, System.Action callback)
    {
        this.country = country;
        this.callback = callback;
    }

    public void Activate()
    {
        active = true;
        spriteRenderer.color = Color.white;
        anim.Play();
    }

    public void Inactivate()
    {
        active = false;
        spriteRenderer.color = new Color32(113, 113, 113, 255);
        anim.Stop();
    }

    public void WonTerritory()
    {
        active = false;
        spriteRenderer.color = new Color32(118, 248, 29, 255);
        anim.Stop();
    }
}
