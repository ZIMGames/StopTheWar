using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Hero")]
public class HeroSO : ScriptableObject
{
    public string heroName;
    public Sprite sprite;
    public Hero.HeroType heroType;
    public Hero.Rarity heroRarity;
    //public Transform prefab;
}
