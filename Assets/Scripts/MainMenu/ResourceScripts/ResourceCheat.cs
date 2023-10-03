using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCheat : MonoBehaviour
{
    [SerializeField] private ResourceTypeSO resourceTypeByClickingG;
    [SerializeField] private ResourceTypeSO resourceTypeByClickingH;
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            ResourceManager.Instance.AddResources(resourceTypeByClickingG, 200);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ResourceManager.Instance.AddResources(resourceTypeByClickingH, 200);
        }
#endif
    }
}
