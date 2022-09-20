using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class PlayerTank : NetworkBehaviour
{
    [SerializeField]
    private float move_speed = 5f, rotate_speed = 110f;

    private CharacterController characterController;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        characterController.SimpleMove(vertical * transform.forward * move_speed);
        transform.Rotate(new Vector3(0, horizontal * Time.deltaTime * rotate_speed, 0));
    }
}
