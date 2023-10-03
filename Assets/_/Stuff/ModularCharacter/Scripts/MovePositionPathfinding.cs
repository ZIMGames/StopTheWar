/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;

public class MovePositionPathfinding : MonoBehaviour, IMovePosition {

    private Action onReachedTargetPosition;
    private List<Vector3> pathVectorList;
    private int pathIndex = -1;
    private IMoveVelocity iMoveVelocity;

    private void Awake()
    {
        iMoveVelocity = GetComponent<IMoveVelocity>();
    }

    public void SetMovePosition(Vector3 movePosition, Action onReachedTargetPosition) {
        this.onReachedTargetPosition = onReachedTargetPosition;
        pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, movePosition).pathVectorList;
        /*Debug.Log("##########");
        foreach (Vector3 vec in pathVectorList) {
            Debug.Log(vec);
        }*/
        if (pathVectorList.Count > 0) {
            // Remove first position so he doesn't go backwards
            //pathVectorList.RemoveAt(0);
        }
        if (pathVectorList.Count > 0) {
            pathIndex = 0;
        } else {
            pathIndex = -1;
        }
    }

    private void Update() {
        if (pathIndex != -1) {
            // Move to next path position
            Vector3 nextPathPosition = pathVectorList[pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            Debug.Log(moveVelocity);

            if (Vector3.Distance(transform.position, nextPathPosition) < 1f || Vector3.Distance(transform.position, nextPathPosition) < Vector3.Distance(transform.position, transform.position + moveVelocity * iMoveVelocity.GetSpeed() * Time.deltaTime))
            {
                Debug.Log("pass");
                //if (Vector3.Distance(transform.position, nextPathPosition) < 1f) { 
                moveVelocity = Vector3.zero;
                transform.position = nextPathPosition;
                pathIndex++;
                if (pathIndex >= pathVectorList.Count) {
                    // End of path
                    pathIndex = -1;
                    onReachedTargetPosition();
                }
            }
            iMoveVelocity.SetVelocity(moveVelocity);
        } else {
            // Idle
            iMoveVelocity.SetVelocity(Vector3.zero);
        }
    }

}
