using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    public float driveSpeed;
    public float rotateSpeed;
    private CharacterController characterController;

    void Start()
    {
        this.driveSpeed = 5.0f;
        this.rotateSpeed = 55.0f;
        this.characterController = this.GetComponent<CharacterController>();
    }

    void Update()
    {

        // Tank movements
        if(Input.GetKey("w")) {
            this.characterController.SimpleMove(this.transform.forward * driveSpeed);
        }

        if(Input.GetKey("s")) {
            this.characterController.SimpleMove(-this.transform.forward * driveSpeed);
        }

        if(Input.GetKey("d")) {
            this.transform.Rotate(0.0f, this.rotateSpeed * Time.deltaTime, 0.0f);
        }

        if(Input.GetKey("a")) {
            this.transform.Rotate(0.0f, -this.rotateSpeed * Time.deltaTime, 0.0f);
        }

    }
}
