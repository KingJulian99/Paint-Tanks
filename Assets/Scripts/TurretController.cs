using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    /*
        Controller for the Turret component of a Tank.

        All aiming calculations assume that the tank is on a plane at (0,0,0) facing upwards.
        All aiming to towards a point on the plane, and no other objects are considered (walls, players etc.).
    */
    
    public float maxRotationSpeed;
    public Color teamColor;
    public Camera viewCamera;
    public GameObject projectile;

    /*
        All private variables are fetched from this GameObject's children.
        This GameObject is the turret of the tank, and is assumed to have the structure:
        
        Turret (GameObject)
            - Barrel (GameObject)
                - Muzzle (GameObject)
                    - Splash Shot (Grouped Particle System)
    */
    private GameObject barrel;
    private GameObject muzzle;
    private GameObject effectObject;
    private ParticleSystem shootingEffect;
    
    void Start()
    {
        this.barrel = this.transform.GetChild(0).gameObject;
        this.muzzle = this.barrel.transform.GetChild(0).gameObject;
        this.effectObject = this.muzzle.transform.GetChild(0).gameObject;
        this.shootingEffect = this.effectObject.GetComponent<ParticleSystem>();
        this.viewCamera = Camera.main;
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
            
            this.LookAt(pointOnPlane);
        }

        if(Input.GetMouseButtonDown(0)) {
            this.shootingEffect.Play();
            this.Shoot();
        }
    }

    void LookAt(Vector3 point) {
        Vector3 heightCorrectedPoint = new Vector3(point.x, this.transform.position.y, point.z);

        // Debug.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 10, Color.red);

        this.transform.Rotate(new Vector3(0, this.GetAngleBetweenBarrelAndPoint(heightCorrectedPoint) * this.maxRotationSpeed * Time.deltaTime, 0));
    }

    float GetAngleBetweenBarrelAndPoint(Vector3 correctedPoint) {
        Vector3 directionToPoint = -(this.transform.position - correctedPoint).normalized;

        // Debug.DrawLine(this.transform.position, this.transform.position + directionToPoint * 10, Color.blue);

        return -Vector3.SignedAngle(directionToPoint, this.transform.forward, Vector3.up);
    }

    void Shoot() {
        // Animate
        this.barrel.GetComponent<Animator>().SetTrigger("Shoot");

        // Skiet mos
        GameObject shot = Instantiate(projectile, this.muzzle.transform.position, this.muzzle.transform.rotation);


        // Set Color for the bullet and paint
        shot.GetComponent<Renderer>().material.SetColor("_BaseColor", teamColor);
        shot.GetComponent<BulletController>().paintColor = teamColor;
    }
}
