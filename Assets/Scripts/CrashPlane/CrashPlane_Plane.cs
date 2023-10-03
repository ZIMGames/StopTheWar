using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashPlane_Plane : MonoBehaviour
{
    public static CrashPlane_Plane Create(Transform prefab, Vector3 spawnPos, Vector3 targetPos, float speed, bool isEnemy,
        System.Action onDestroyedByBullet, System.Action onReachedEndPointCallback)
    {
        Transform planeTransform = Instantiate(prefab, spawnPos, Quaternion.identity);
        CrashPlane_Plane plane = planeTransform.GetComponent<CrashPlane_Plane>();
        plane.Setup(spawnPos, targetPos, speed, isEnemy, onDestroyedByBullet, onReachedEndPointCallback);
        return plane;
    }

    private Vector3 dir;
    private Vector3 targetPos;
    private float speed;
    private bool isEnemy;
    private System.Action onDestroyedByBullet;
    private System.Action onReachedEndPointCallback;

    private void Setup(Vector3 spawnPos, Vector3 targetPos, float speed, bool isEnemy,
        System.Action onDestroyedByBullet, System.Action onReachedEndPointCallback)
    {
        this.dir = (targetPos - spawnPos).normalized;
        this.targetPos = targetPos;
        this.speed = speed;
        this.isEnemy = isEnemy;
        this.onDestroyedByBullet = onDestroyedByBullet;
        this.onReachedEndPointCallback = onReachedEndPointCallback;

        transform.eulerAngles = new Vector3(0, 0, CodeMonkey.Utils.UtilsClass.GetAngleFromVectorFloat(dir));
    }

    public bool IsEnemy => isEnemy;


    private void Update()
    {
        if (CrashPlane.isEnded)
            return;

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            onReachedEndPointCallback?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            transform.position = transform.position + dir * speed * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CrashPlane_Bullet>() != null)
        {
            SFXMusic.Instance.PlayExplosion();

            var bullet = collision.gameObject.GetComponent<CrashPlane_Bullet>();
            bullet.DestroyObject();
            onDestroyedByBullet?.Invoke();
            Destroy(gameObject);
        }
    }
}
