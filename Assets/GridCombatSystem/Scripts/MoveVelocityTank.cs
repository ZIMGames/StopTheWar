
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class MoveVelocityTank : MonoBehaviour, IMoveVelocity
{

    [SerializeField] private float moveSpeed;

    private Vector3 velocityVector;
    private Rigidbody2D rigidbody2D;
    [SerializeField] private Transform baseTankTransform;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        baseTankTransform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = velocityVector * moveSpeed;
        if (velocityVector.magnitude >= 0.3f)
            baseTankTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(velocityVector));
    }

    public void Disable()
    {
        this.enabled = false;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable()
    {
        this.enabled = true;
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }
}
