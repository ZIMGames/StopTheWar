using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSO : ScriptableObject
{
    [Range(0, 5)]
    public int difficulty;
    public int coins;
    public string uniqueCode;
    public QuestDescriptionSO questDescription;
}