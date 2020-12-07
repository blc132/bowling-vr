using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{

    /// <summary>
    /// Jak ustawimy flagę na true to zniszcz obiekt
    /// </summary>
    public bool remove;
    // Start is called before the first frame update

    public AudioSource bowlingBallHitPinSource;
    void Start()
    {
        bowlingBallHitPinSource = GetComponent<AudioSource>();
        bowlingBallHitPinSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (remove)
            Destroy(gameObject);
    }

    public void OnTouchFloor()
    {
        Debug.Log("Touched floor");
        if (gameObject != null)
            Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        var colliderTag = collision.collider.tag;
        if (colliderTag == "BowlingBall" || colliderTag == "Pin" && !bowlingBallHitPinSource.isPlaying)
        {

            Debug.Log("Ball hit the pin");
            bowlingBallHitPinSource.Play();
        }
    }
}
