using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest Description/Description")]
public class QuestDescriptionSO : ScriptableObject
{
    public Sprite iconSprite;
    public string nameString;
    public string rulesString;
}
