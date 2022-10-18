using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{

    // public PlayerControls controls;

    private GamepadTurretController turretController;

    private bool forward;
    private bool backward;

    private float shpeeeeed = 0.0f; // speed
    public float slowDown = 0.0f;

    private float rotateSpeed = 55.0f;

    private Vector2 rotateBodyDirection;

    private PlayerInput m_PlayerInput;
    private InputAction m_ForwardAction;
    private InputAction m_BackwardAction;
    private InputAction m_RotateBody;
    private InputAction m_Shoot;

    void Awake() {
        // controls = new PlayerControls();

        turretController = this.transform.GetChild(0).GetComponent<GamepadTurretController>();

        // controls.Gameplay.Forward.performed += context => {
        //     this.forward = true;
        // };

        // controls.Gameplay.Forward.canceled += context => {
        //     this.forward = false;
        // };

        // controls.Gameplay.Backward.performed += context => {
        //     this.backward = true;
        // };

        // controls.Gameplay.Backward.canceled += context => {
        //     this.backward = false;
        // };

        // controls.Gameplay.RotateBody.performed += context => {
        //     this.rotateBodyDirection = context.ReadValue<Vector2>();
        // };

        // controls.Gameplay.RotateBody.canceled += context => {
        //     this.rotateBodyDirection = Vector2.zero;
        // };

        // controls.Gameplay.Shoot.performed += context => {
        //     this.turretController.Shoot();
        // };
    }

    // private void OnForward() {
    //     print("forward!");
    //     print("isgrounded: " + this.GetComponent<CharacterController>().isGrounded);
    //     // if (this.GetComponent<CharacterController>().isGrounded) {
    //     //     this.GetComponent<CharacterController>().SimpleMove(this.transform.forward * (this.shpeeeeed - this.slowDown));
    //     // }
    //     this.shpeeeeed = 5.0f;
        
    // }

    // private void OnBackward() {
    //     print("backward!");
    //     // if (this.GetComponent<CharacterController>().isGrounded) {
    //     //     this.GetComponent<CharacterController>().SimpleMove(-this.transform.forward * (this.shpeeeeed + this.slowDown));
    //     // }
    //     this.shpeeeeed = -5.0f;
        
    // }

    // private void OnRotateBody(InputValue value) {
    //     this.rotateBodyDirection = value.Get<Vector2>();
    //     this.transform.Rotate(0.0f, this.rotateSpeed * Time.deltaTime * this.rotateBodyDirection.x, 0.0f);
    // }


    void Update() {

        if (m_PlayerInput == null)
        {
            m_PlayerInput = GetComponent<PlayerInput>();
            m_ForwardAction = m_PlayerInput.actions["Forward"];
            m_BackwardAction = m_PlayerInput.actions["Backward"];
            m_RotateBody = m_PlayerInput.actions["RotateBody"];
            m_Shoot = m_PlayerInput.actions["Shoot"];
        }

        m_ForwardAction.performed += context => {
            this.forward = true;
        };

        m_ForwardAction.canceled += context => {
            this.forward = false;
        };

        m_BackwardAction.performed += context => {
            this.backward = true;
        };

        m_BackwardAction.canceled += context => {
            this.backward = false;
        };

        m_RotateBody.performed += context => {
            this.rotateBodyDirection = context.ReadValue<Vector2>();
        };

        m_RotateBody.canceled += context => {
            this.rotateBodyDirection = Vector2.zero;
        };

        m_Shoot.performed += context => {
            this.turretController.Shoot();
        };

        
        //Tank movements
        if(this.forward && this.backward) {
            this.shpeeeeed = 0.0f;
        } else if(this.forward) {
            this.shpeeeeed = 5.0f - this.slowDown;
        } else if(this.backward) {
            this.shpeeeeed = -5.0f + this.slowDown;
        } else {
            this.shpeeeeed = 0.0f;
        }

        if (this.GetComponent<CharacterController>().isGrounded) {
            
            // Forward and back movement
            this.GetComponent<CharacterController>().SimpleMove(this.transform.forward * this.shpeeeeed);

            // Rotation
            this.transform.Rotate(0.0f, this.rotateSpeed * Time.deltaTime * this.rotateBodyDirection.x, 0.0f);
        }
        
    }
   
}
