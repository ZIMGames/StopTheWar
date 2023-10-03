using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/DecoderGame")]
public class QuestDecoderGameSO : QuestSO
{
    public int width;
    public int height;
    public List<int> activatedIndexList;
    public bool crossActivation;
}
