using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEST : MonoBehaviour {

    private PlayerInput character;
    private bool spawned = false;


    void OnPlayerJoined(PlayerInput playerInput) {
        character = playerInput;
        print("working");
        //MoveToPrison();
    }

    void MoveToPrison() {
        print("moved to prison");
        character.transform.position = new Vector3(0, -10, 0);
    }
}
