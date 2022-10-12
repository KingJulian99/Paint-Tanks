using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{

    public PlayerControls controls;

    private float radiusScale;

    private Vector2 targetInput;

    private Vector2 lastKnownInput;

    private GameObject parent; 


    void Awake() {
        this.controls = new PlayerControls();
        this.parent = this.transform.parent.gameObject;
        this.radiusScale = 2.0f;
        this.lastKnownInput = new Vector2(0.0f, 1.0f) * this.radiusScale;

        controls.Gameplay.RotateTarget.performed += context => {
            this.targetInput = context.ReadValue<Vector2>();
            this.lastKnownInput = context.ReadValue<Vector2>();
        };

        controls.Gameplay.RotateTarget.canceled += context => {
            this.targetInput = Vector2.zero;
        };
    }

    void Update() {
        // x & z
        Vector3 addition = new Vector3(this.targetInput.x, 0, this.targetInput.y) * this.radiusScale;
        //print(addition);

        //Vector3 subtraction = new Vector3(-this.lastKnownInput.x, 0, -this.lastKnownInput.y) * this.radiusScale * 0.75f;

        this.transform.position = this.parent.transform.position + addition;
    }

    void OnEnable() {
        controls.Gameplay.Enable();
    }

    void OnDisable() {
        controls.Gameplay.Disable();
    }
}
