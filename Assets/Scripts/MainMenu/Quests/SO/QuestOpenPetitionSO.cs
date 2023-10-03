using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/OpenPetition")]
public class QuestOpenPetitionSO : QuestSO
{
    public int value;
    public int maxValue;
    public int valueStepPerClick;
    public string petitionString;
}
