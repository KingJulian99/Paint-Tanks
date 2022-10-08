using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    public float driveSpeed;
    public float rotateSpeed;
    public int health;
    public Color teamColor;
    public int bounceNumber;

    private CharacterController characterController;
    private float randomRotationSpeed; 
    private float gravity;
    private bool rotatingUncontrollably;


    void Start()
    {
        this.driveSpeed = 5.0f;
        this.rotateSpeed = 55.0f;
        this.health = 100;
        this.characterController = this.GetComponent<CharacterController>();
        this.bounceNumber = 0;
        this.randomRotationSpeed = Random.Range(10.0f, 20.0f);
        this.gravity = -9.0f;
        this.rotatingUncontrollably = false;

        ChangeTankColor();
    }

    void Update()
    {

        ChangeTankColor();

        if ( !this.characterController.isGrounded ) {
            this.characterController.Move( new Vector3(0.0f, this.gravity * Time.deltaTime, 0.0f) ); // applies "gravity"

            if(this.rotatingUncontrollably) {
                this.transform.Rotate(this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime, this.randomRotationSpeed * Time.deltaTime);
            }

        } else {
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
        
        if(this.health <= 0) {
            Destroy(this.gameObject);
        }

    }

    public void RotateUncontrollably() {
        this.rotatingUncontrollably = true;
    }

    public void SetGravity(float gravity) {
        this.gravity = gravity;
    }

    void ChangeTankColor() {
        /*
            This method sets both the tank and turret prefab colors to the current teamColor.
        */
        
        Material tankMaterial = GetComponent<Renderer>().material;
        tankMaterial.SetColor("_BaseColor", this.teamColor);
        Material turretMaterial = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        turretMaterial.SetColor("_BaseColor", this.teamColor);
    }
}