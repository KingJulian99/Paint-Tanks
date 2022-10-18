using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetController : MonoBehaviour
{

    private float radiusScale;

    private Vector2 targetInput;

    private Vector2 lastKnownInput;

    private GameObject parent; 

    private PlayerInput m_PlayerInput;

    private InputAction m_RotateTarget;


    void Awake() {
        this.parent = this.transform.parent.gameObject;
        this.radiusScale = 2.0f;
        this.lastKnownInput = new Vector2(0.0f, 1.0f) * this.radiusScale;
    }

    void Update() {

        if (m_PlayerInput == null)
        {
            m_PlayerInput = this.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PlayerInput>();
            m_RotateTarget = m_PlayerInput.actions["RotateTarget"];
        }

        m_RotateTarget.performed += context => {
            this.targetInput = context.ReadValue<Vector2>();
            this.lastKnownInput = context.ReadValue<Vector2>();
        };

        m_RotateTarget.canceled += context => {
            this.targetInput = Vector2.zero;
        };

        // x & z
        Vector3 addition = new Vector3(this.targetInput.x, 0, this.targetInput.y) * this.radiusScale;
        //print(addition);

        //Vector3 subtraction = new Vector3(-this.lastKnownInput.x, 0, -this.lastKnownInput.y) * this.radiusScale * 0.75f;

        this.transform.position = this.parent.transform.position + addition;
    }

}
