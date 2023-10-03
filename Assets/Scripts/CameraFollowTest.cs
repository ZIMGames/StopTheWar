using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.MonoBehaviours;

public class CameraFollowTest : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraFollow;

    private void Start()
    {
        cameraFollow.SetCameraFollowPosition(cameraFollow.transform.position);
    }
}
