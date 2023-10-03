using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawnerSingle : MonoBehaviour
{
    private System.Action callback;
    private LTDescr anim;

    public void Setup(Vector3 targetPos, float travelTime, float delay, System.Action callback)
    {
        this.callback = callback;
        anim = LeanTween.move(gameObject, targetPos, travelTime).setDelay(delay).setOnComplete(OnComplete);
    }

    private void OnComplete()
    {
        Destroy(gameObject);
        callback();
    }

    public void Stop()
    {
        anim.reset();
        OnComplete();
    }
}
