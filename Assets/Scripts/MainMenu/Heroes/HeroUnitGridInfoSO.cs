using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HeroUnitGridInfo/Hero")]
public class HeroUnitGridInfoSO : ScriptableObject
{
    public int healthAmount;
    public int attackDamage;
    public int attackDistance;
    public int moveDistanceMax;
    public Hero.HeroType heroType;
}
