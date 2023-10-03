/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionDirect : MonoBehaviour, IMovePosition {

    public Vector3 movePosition;
    private System.Action onReachedPosition;
    private IMoveVelocity moveVelocity;

    private void Awake() {
        movePosition = transform.position;
        moveVelocity = GetComponent<IMoveVelocity>();
    }

    public void SetMovePosition(Vector3 movePosition, System.Action onReachedPosition) {
        this.movePosition = movePosition;
        this.onReachedPosition = onReachedPosition;
    }

    private void Update() {
        Vector3 moveDir = (movePosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, movePosition) < Vector3.Distance(transform.position, transform.position + moveDir * moveVelocity.GetSpeed() * Time.deltaTime))
        {
            transform.position = movePosition;
            moveDir = Vector3.zero; // Stop moving when near
            if (onReachedPosition != null)
            {
                onReachedPosition();
                onReachedPosition = null;
            }
        }
        moveVelocity.SetVelocity(moveDir);
    }

}
