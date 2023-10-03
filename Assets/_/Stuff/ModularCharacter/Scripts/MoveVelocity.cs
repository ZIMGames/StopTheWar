﻿/* 
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

public class MoveVelocity : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed;

    private Vector3 velocityVector;
    private Rigidbody2D rigidbody2D;
    private Character_Base characterBase;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        characterBase = GetComponent<Character_Base>();
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody2D.velocity = velocityVector * moveSpeed;

        characterBase.PlayMoveAnim(velocityVector);
    }

    public void Disable() {
        this.enabled = false;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }
}
