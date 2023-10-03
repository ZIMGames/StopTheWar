using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.MonoBehaviours;

public class BattleTester : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private void Start()
    {
        Setup();
    }
    private void Setup()
    {
        float cameraYOffset = 0f;
        var grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        cameraTransform.position = new Vector3(grid.GetWidth() * grid.GetCellSize() * .5f, grid.GetHeight() * grid.GetCellSize() * .5f + cameraYOffset, cameraTransform.position.z);
        CameraFollow cameraFollow = cameraTransform.GetComponent<CameraFollow>();
        cameraFollow.SetCameraFollowPosition(cameraTransform.position);
    }
}