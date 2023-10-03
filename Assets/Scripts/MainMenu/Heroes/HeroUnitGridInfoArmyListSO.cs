using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HeroUnitGridInfo/ArmyList")]
public class HeroUnitGridInfoArmyListSO : ScriptableObject
{
    public List<HeroUnitGridInfoArmySO> list;
}
