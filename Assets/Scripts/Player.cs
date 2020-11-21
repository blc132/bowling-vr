using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bowlingBall;
    public float ballDistance = 2.5f;
    public float ballThrowingForce = 200f;

    public bool holdingBowlingBall = true;

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

    public void GetBowlingBallToHand()
    {
        var bowlingBallRigidBody = bowlingBall.GetComponent<Rigidbody>();
        bowlingBallRigidBody.useGravity = false;

        //tutaj ogólnie zrestartuj wszystko co możliwe z fizyką bo inaczej kula będzie leciała niewiadomo gdzie xD
        bowlingBall.transform.position = new Vector3(0, 0, 0);
        bowlingBallRigidBody.velocity = new Vector3(0f, 0f, 0f);
        bowlingBallRigidBody.angularVelocity = new Vector3(0f, 0f, 0f);
        bowlingBall.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        holdingBowlingBall = true;
    }

    private void ThrowBowlingBall()
    {
        holdingBowlingBall = false;

        var bowlingBallRigidBody = bowlingBall.GetComponent<Rigidbody>();

        bowlingBallRigidBody.useGravity = true;
        bowlingBallRigidBody.AddForce(transform.forward * ballThrowingForce);
    }
}
