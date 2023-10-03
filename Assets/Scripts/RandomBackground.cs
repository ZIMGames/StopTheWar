using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBackground : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteArray;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer.sprite = spriteArray[Random.Range(0, spriteArray.Length)];
    }
}
