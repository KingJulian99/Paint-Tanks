using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{

    public PlayerControls controls;

    private GamepadTurretController turretController;

    private bool forward;
    private bool backward;

    private float shpeeeeed = 0.0f; // speed
    public float slowDown = 0.0f;

    private float rotateSpeed = 55.0f;

    private Vector2 rotateBodyDirection;


    void Awake() {
        controls = new PlayerControls();

        turretController = this.transform.GetChild(0).GetComponent<GamepadTurretController>();

        controls.Gameplay.Forward.performed += context => {
            this.forward = true;
        };

        controls.Gameplay.Forward.canceled += context => {
            this.forward = false;
        };

        controls.Gameplay.Backward.performed += context => {
            this.backward = true;
        };

        controls.Gameplay.Backward.canceled += context => {
            this.backward = false;
        };

        controls.Gameplay.RotateBody.performed += context => {
            this.rotateBodyDirection = context.ReadValue<Vector2>();
        };

        controls.Gameplay.RotateBody.canceled += context => {
            this.rotateBodyDirection = Vector2.zero;
        };

        controls.Gameplay.Shoot.performed += context => {
            this.turretController.Shoot();
        };
    }

    void Update() {
        
            // Tank movements
            if(this.forward && this.backward) {
                this.shpeeeeed = 0.0f;
            } else if(this.forward) {
                this.shpeeeeed = 5.0f - this.slowDown;
            } else if(this.backward) {
                this.shpeeeeed = -5.0f + this.slowDown;
            } else {
                this.shpeeeeed = 0.0f;
            }

            // Forward and back movement
            this.GetComponent<CharacterController>().SimpleMove(this.transform.forward * (this.shpeeeeed - this.slowDown));

            // Rotation
            this.transform.Rotate(0.0f, this.rotateSpeed * Time.deltaTime * this.rotateBodyDirection.x, 0.0f);
        
    }

    void OnEnable() {
        controls.Gameplay.Enable();
    }

    void OnDisable() {
        controls.Gameplay.Disable();
    }
   
}
