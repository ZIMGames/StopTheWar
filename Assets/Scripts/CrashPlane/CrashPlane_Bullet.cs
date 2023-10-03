using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashPlane_Bullet : MonoBehaviour
{
    public static CrashPlane_Bullet Create(Transform prefab, Vector3 spawnPos, Vector3 dir, float speed, float timeToDestroy)
    {
        SFXMusic.Instance.PlayRocketShoot();
        Transform planeTransform = Instantiate(prefab, spawnPos, Quaternion.identity);
        CrashPlane_Bullet bullet = planeTransform.GetComponent<CrashPlane_Bullet>();
        bullet.Setup(dir, speed, timeToDestroy);
        return bullet;
    }

    private Vector3 dir;
    private float speed;

    private void Setup(Vector3 dir, float speed, float timeToDestroy)
    {
        this.dir = dir;
        this.speed = speed;

        Destroy(gameObject, timeToDestroy);

        transform.eulerAngles = new Vector3(0, 0, CodeMonkey.Utils.UtilsClass.GetAngleFromVectorFloat(dir));
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }


    private void Update()
    {
        if (CrashPlane.isEnded)
            return;

        transform.position = transform.position + dir * speed * Time.deltaTime;
    }
}
