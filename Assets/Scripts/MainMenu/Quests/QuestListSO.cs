using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest List/List")]
public class QuestListSO : ScriptableObject
{
    public List<QuestSO> list;

    public List<QuestSO> openPetitionList;
    public List<QuestSO> minesweeperList;
    public List<QuestSO> findTraitorList;
    public List<QuestSO> crashPlaneList;
    public List<QuestSO> decoderGameList;
    public List<QuestSO> wolfAndEggsList;

    private void OnEnable()
    {
        if (list.Count > 0)
            return;
        for (int i = 0; i < openPetitionList.Count; i++)
        {
            list.Add(openPetitionList[i]);
            list.Add(minesweeperList[i]);
            list.Add(findTraitorList[i]);
            list.Add(crashPlaneList[i]);
            list.Add(decoderGameList[i]);
            list.Add(wolfAndEggsList[i]);
        }
    }
}

