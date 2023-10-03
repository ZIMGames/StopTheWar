using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/List")]
public class ResourceTypeListSO: ScriptableObject
{
    public List<ResourceTypeSO> list;
}
