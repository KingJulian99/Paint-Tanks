using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadTurretController : MonoBehaviour
{
    /*
        This GameObject is the turret of the tank, and is assumed to have the structure:
            
            Turret (GameObject)
                - Barrel (GameObject)
                    - Muzzle (GameObject)
                        - Splash Shot (Grouped Particle System)


        Controller for the Turret component of a Tank.

        All aiming calculations assume that the tank is on a plane at (0,0,0) facing upwards.
        All aiming to towards a point on the plane, and no other objects are considered (walls, players etc.).
    */
    
    public float maxRotationSpeed;
    public Camera viewCamera;
    public GameObject projectile;


    private GameObject barrel;
    private GameObject muzzle;
    private GameObject effectObject;
    private ParticleSystem shootingEffect;
    private GameObject target;
    private float distanceToTarget;

    private GamepadTankController tankController;
    private int bounceNumber;
    private Color teamColor;
    
    
    void Start()
    {
        this.barrel = this.gameObject.transform.GetChild(0).gameObject;
        this.muzzle = this.barrel.transform.GetChild(0).gameObject;
        this.effectObject = this.muzzle.transform.GetChild(0).gameObject;
        this.shootingEffect = this.effectObject.GetComponent<ParticleSystem>();
        this.viewCamera = Camera.main;
        this.target = this.transform.GetChild(1).gameObject;

        this.tankController = this.gameObject.transform.parent.gameObject.GetComponent<GamepadTankController>();
        this.teamColor = tankController.teamColor;
        this.bounceNumber = tankController.bounceNumber;
        this.maxRotationSpeed = 10f;
    }

    void Update()
    {
        // Debug.DrawLine(this.muzzle.transform.position, this.muzzle.transform.position + this.muzzle.transform.forward * 10, Color.red);

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 pointOnPlane = ray.GetPoint(rayDistance);

            //Debug.DrawLine(ray.origin, pointOnPlane, Color.green);

            this.distanceToTarget = Vector3.Distance(this.target.transform.position, this.transform.position);
            
            this.LookAt(pointOnPlane);
        }

    }

    void LookAt(Vector3 point) {
        point = this.transform.GetChild(1).gameObject.transform.position; //NEW addition

        Vector3 heightCorrectedPoint = new Vector3(point.x, this.transform.position.y, point.z);

        // Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 10, Color.red);

        this.transform.Rotate(new Vector3(0, this.GetAngleBetweenBarrelAndPoint(heightCorrectedPoint) * this.maxRotationSpeed * Time.deltaTime * (distanceToTarget * 0.5f), 0));
    }

    float GetAngleBetweenBarrelAndPoint(Vector3 correctedPoint) {
        Vector3 directionToPoint = -(this.transform.position - correctedPoint).normalized;

        // Debug.DrawLine(this.transform.position, this.transform.position + directionToPoint * 10, Color.blue);

        return -Vector3.SignedAngle(directionToPoint, this.transform.forward, Vector3.up);
    }

    public void Shoot() {
        // Smoke 😎🚬
        this.shootingEffect.Play();

        // Animate
        this.barrel.GetComponent<Animator>().SetTrigger("Shoot");

        // Get number of bounces
        this.setBounceNumber(this.tankController.bounceNumber);

        // Set team color
        this.setTeamColor(this.tankController.teamColor);

        // Skiet mos
        GameObject shot = Instantiate(projectile, this.muzzle.transform.position, this.muzzle.transform.rotation);

        // Set Color for the bullet, paint, paint explosion and amount of bounces left.
        shot.GetComponent<BulletController>().Setup(this.teamColor, this.bounceNumber);
       
    }

    public void setBounceNumber(int bounceLimit) {
        this.bounceNumber = bounceLimit;
    }

    public void setTeamColor(Color teamColor) {
        this.teamColor = teamColor;
    }
}