using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/CrashPlane")]
public class QuestCrashPlaneSO : QuestSO
{
    public int totalPlanes;
    public int enemyPlanes;
    public float timeSpawnRate;
    public float planeSpeed;
    public float bulletSpeed;
}