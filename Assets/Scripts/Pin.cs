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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(remove)
            Destroy(gameObject);
    }

    public void OnTouchFloor()
    {
        Debug.Log("Touched floor");
        if(gameObject != null)
            Destroy(gameObject);
    }
}
