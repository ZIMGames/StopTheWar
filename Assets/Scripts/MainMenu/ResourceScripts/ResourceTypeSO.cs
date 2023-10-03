using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Gold,
    Silver
}

[CreateAssetMenu(menuName = "Resources/Type")]
public class ResourceTypeSO : ScriptableObject
{
    public Sprite sprite;
    public Color color;
    public string shortName;
    public ResourceType type;
}
