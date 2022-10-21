using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{

    // public PlayerControls controls;

    public float RELOADTIME = 0.1f;
    private float reloadTime;

    private GamepadTurretController turretController;

    private bool forward;
    private bool backward;

    private float shpeeeeed = 0.0f; // speed
    public float slowDown = 0.0f;

    private float rotateSpeed = 10.0f;

    private Vector2 rotateBodyDirection;

    private PlayerInput m_PlayerInput;
    private InputAction m_ForwardAction;
    private InputAction m_BackwardAction;
    private InputAction m_RotateBody;
    private InputAction m_Shoot;

    private Vector3 driveTarget;
    private bool drive;

    private bool canShoot;

    void Awake() {
        turretController = this.transform.GetChild(0).GetComponent<GamepadTurretController>();
        canShoot = true;
        reloadTime = RELOADTIME;
    }


    void Update() {

        if (!canShoot)
        {
            if (reloadTime > 0)
            {
                reloadTime -= Time.deltaTime;
            }
            else
            {
                reloadTime = RELOADTIME;
                canShoot = true;
            }
        }

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
            drive = true;
            this.rotateBodyDirection = context.ReadValue<Vector2>();
        };

        m_RotateBody.canceled += context => {
            drive = false;
            this.rotateBodyDirection = Vector2.zero;
        };

        m_Shoot.performed += context => {
            if(canShoot) {
                this.turretController.Shoot();
                canShoot = false;
            }
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

            // Single stick rotation
            if(this.drive){

                // Rotate in direction
                this.driveTarget = this.transform.position + new Vector3(this.rotateBodyDirection.x, 0, this.rotateBodyDirection.y);
                this.transform.Rotate(new Vector3(0, GetAngleBetweenThisAndPoint(this.driveTarget)* this.rotateSpeed * Time.deltaTime, 0));

                // Move in that desired way
                Vector3 moveDirection = -(this.transform.position - this.driveTarget).normalized;
                this.GetComponent<CharacterController>().SimpleMove(moveDirection * (5.0f - this.slowDown));
            }
            
        }
        
    }

    float GetAngleBetweenThisAndPoint(Vector3 correctedPoint) {
        Vector3 directionToPoint = -(this.transform.position - correctedPoint).normalized;
        return -Vector3.SignedAngle(directionToPoint, this.transform.forward, Vector3.up);
    }
   
}
