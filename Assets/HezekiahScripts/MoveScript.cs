using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveScript : MonoBehaviour
{
    [SerializeField]
    private float move_speed = 5f, rotate_speed = 100f;

    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        characterController.SimpleMove(vertical * transform.forward * move_speed);
        transform.Rotate(new Vector3(0, horizontal * Time.deltaTime * rotate_speed, 0));
    }
}
