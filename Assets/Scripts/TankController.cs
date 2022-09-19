using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    private float driveSpeed;
    private CharacterController characterController;

    void Start()
    {
        this.driveSpeed = 5.0f;
        this.characterController = this.GetComponent<CharacterController>();
    }

    void Update()   // Refactor to use fixed update
    {

        // Tank movements
        if(Input.GetKey("w")) {
            this.characterController.SimpleMove(this.transform.forward * driveSpeed);
            //this.transform.Translate(Vector3.forward * Time.deltaTime * driveSpeed);
        }

        if(Input.GetKey("s")) {
            this.characterController.SimpleMove(-this.transform.forward * driveSpeed);
            //this.transform.Translate(-Vector3.forward * Time.deltaTime * driveSpeed);
        }

        if(Input.GetKey("d")) {
            this.transform.Rotate(0.0f, 55.0f * Time.deltaTime, 0.0f);
        }

        if(Input.GetKey("a")) {
            this.transform.Rotate(0.0f, -55.0f * Time.deltaTime, 0.0f);
        }


    }
}
