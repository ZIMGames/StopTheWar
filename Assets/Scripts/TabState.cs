using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabState : MonoBehaviour
{
    public System.Action onEnableCallback;
    public System.Action onDisableCallback;

    public void OnDisable()
    {
        onDisableCallback?.Invoke();
    }

    public void OnEnable()
    {
        onEnableCallback?.Invoke();
    }
}
