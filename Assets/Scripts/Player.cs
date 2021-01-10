using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bowlingBall;
    public float ballDistance = 2.5f;
    public float ballThrowingForce = 200f;


    public bool holdingBowlingBall = true;
    //public bool holdingBowlingBall = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (holdingBowlingBall)
        {
            //  bowlingBall.transform.position = new Vector3(-1f, 1f, 0.2f);
            //  bowlingBall.transform.position = new Vector3(0.382f, 0.763f, 0.2f);

            bowlingBall.transform.position = new Vector3(1.809f, 0.875f, -0.533f);
            var bowlingBallRigidBody = bowlingBall.GetComponent<Rigidbody>();
            bowlingBallRigidBody.velocity = new Vector3(0f, 0f, 0f);
            bowlingBallRigidBody.angularVelocity = new Vector3(0f, 0f, 0f);
            bowlingBall.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

            if (Input.GetKeyDown("space"))
            {
                //bowlingBall.transform.SetPositionAndRotation(new Vector3(0.382f, 0.763f, 0.2f), Quaternion.Euler(new Vector3(0, 0, 0)));
               
                // ThrowBowlingBall();
            }

            //if (OVRInput.GetDown(OVRInput.Button.Two))
            if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
            {
                holdingBowlingBall = false;
            }
        }
    }

    public void GetBowlingBallToHand()
    {
        var bowlingBallRigidBody = bowlingBall.GetComponent<Rigidbody>();
        //bowlingBallRigidBody.useGravity = false;

        
        //tutaj ogólnie zrestartuj wszystko co możliwe z fizyką bo inaczej kula będzie leciała niewiadomo gdzie xD
        //  bowlingBall.transform.position = new Vector3(2, 2, 2);
        // bowlingBallRigidBody.velocity = new Vector3(0f, 0f, 0f);
        //   bowlingBallRigidBody.angularVelocity = new Vector3(0f, 0f, 0f);
        //   bowlingBall.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

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
