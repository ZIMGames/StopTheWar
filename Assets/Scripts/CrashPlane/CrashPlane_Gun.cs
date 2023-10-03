using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CrashPlane_Gun : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPointTransform;
    [SerializeField] private Transform pfBullet;

    private void Update()
    {
        if (CrashPlane.isEnded)
            return;

        if (Input.GetMouseButton(0))
        {
            //follow mouse
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
            Vector3 dir = (mousePos - transform.position).normalized;
            transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
            Vector3 dir = (mousePos - transform.position).normalized;

            CrashPlane_Bullet.Create(pfBullet, bulletSpawnPointTransform.position, dir, CrashPlane.bulletSpeed, 10f);
        }
    }
}
