using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Hero List")]
public class HeroListSO : ScriptableObject
{
    public List<HeroSO> list;
}
