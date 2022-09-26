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
    private TurretController turretController;

    void Start()
    {
        // Setting movement variables and other defaults
        this.driveSpeed = 5.0f;
        this.rotateSpeed = 55.0f;
        this.health = 100;
        this.characterController = this.GetComponent<CharacterController>();
        this.bounceNumber = 2;

        // Changing to team Colors
        Material tankMaterial = GetComponent<Renderer>().material;
        tankMaterial.SetColor("_BaseColor", teamColor);
        Material turretMaterial = this.transform.GetChild(0).GetComponent<Renderer>().material;
        turretMaterial.SetColor("_BaseColor", teamColor);


        // Setting bullet color in turret controller
        turretController = this.transform.GetChild(0).GetComponent<TurretController>();
        turretController.setTeamColor(this.teamColor);

        // Setting bullet bounce limit in turret controller
        turretController.setBounceNumber(this.bounceNumber);
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

        if(this.health <= 0) {
            Destroy(this.gameObject);
        }

    }
}
