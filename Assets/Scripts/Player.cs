using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bowlingBall;
    public float ballDistance = 2.5f;
    public float ballThrowingForce = 200f;

    private bool holdingBowlingBall = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (holdingBowlingBall)
        {
            bowlingBall.transform.position = transform.position + transform.forward * ballDistance;

            if (Input.GetKeyDown("space"))
            {
                ThrowBowlingBall();
            }
        }
    }

    private void ThrowBowlingBall()
    {
        holdingBowlingBall = false;

        var bowlingBallRigidBody = bowlingBall.GetComponent<Rigidbody>();

        bowlingBallRigidBody.useGravity = true;
        bowlingBallRigidBody.AddForce(transform.forward * ballThrowingForce);
    }
}
