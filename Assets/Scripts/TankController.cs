using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    private float driveSpeed;

    void Start()
    {
        this.driveSpeed = 5.0f;
    }

    void Update()   // Refactor to use fixed update
    {

        // Tank movements
        if(Input.GetKey("w")) {
            this.transform.Translate(Vector3.forward * Time.deltaTime * driveSpeed);
        }

        if(Input.GetKey("s")) {
            this.transform.Translate(-Vector3.forward * Time.deltaTime * driveSpeed);
        }

        if(Input.GetKey("d")) {
            this.transform.Rotate(0.0f, 55.0f * Time.deltaTime, 0.0f);
        }

        if(Input.GetKey("a")) {
            this.transform.Rotate(0.0f, -55.0f * Time.deltaTime, 0.0f);
        }


    }
}
