using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrelTurretController : MonoBehaviour
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

    [SerializeField]
    private GameObject leftBarrel;
    [SerializeField]
    private GameObject rightBarrel;

    private GameObject leftMuzzle, rightMuzzle;
    private GameObject leftEffectObject, rightEffectObject;
    private ParticleSystem leftShootingEffect, rightShootingEffect;

    private TankController tankController;
    private int bounceNumber;
    private Color teamColor;

    private float reloadTime;
    private bool canShoot;

    private float duration;

    void Start()
    {
        this.leftMuzzle = this.leftBarrel.transform.GetChild(0).gameObject;
        this.rightMuzzle = this.rightBarrel.transform.GetChild(0).gameObject;
        
        this.leftEffectObject = this.leftMuzzle.transform.GetChild(0).gameObject;
        this.rightEffectObject = this.rightMuzzle.transform.GetChild(0).gameObject;

        this.leftShootingEffect = this.leftEffectObject.GetComponent<ParticleSystem>();
        this.rightShootingEffect = this.rightEffectObject.GetComponent<ParticleSystem>();

        this.viewCamera = Camera.main;

        this.tankController = this.gameObject.transform.parent.gameObject.GetComponent<TankController>();
        this.teamColor = tankController.teamColor;
        this.bounceNumber = tankController.bounceNumber;
        this.maxRotationSpeed = 10f;
        this.reloadTime = 1;
        this.canShoot = true;

        duration = 30f;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            SpawnTurret(this.transform.parent.gameObject);
        }

        // Debug.DrawLine(this.muzzle.transform.position, this.muzzle.transform.position + this.muzzle.transform.forward * 10, Color.red);

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if(groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 pointOnPlane = ray.GetPoint(rayDistance);

            //Debug.DrawLine(ray.origin, pointOnPlane, Color.green);
            
            this.LookAt(pointOnPlane);
        }

        // Check if reload is complete 
        if(Input.GetMouseButtonDown(0) && canShoot) {
            this.Shoot();
            canShoot = false;
        }

        if (!canShoot)
        {
            if (reloadTime > 0)
            {
                reloadTime -= Time.deltaTime;
            }
            else
            {
                reloadTime = 1;
                canShoot = true;
            }
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
        // Smoke 😎🚬
        this.leftShootingEffect.Play();
        this.rightShootingEffect.Play();

        // Animate
        this.leftBarrel.GetComponent<Animator>().SetTrigger("Shoot");
        this.rightBarrel.GetComponent<Animator>().SetTrigger("Shoot");

        // Get number of bounces
        this.setBounceNumber(this.tankController.bounceNumber);

        // Set team color
        this.setTeamColor(this.tankController.teamColor);

        // Skiet mos
        GameObject leftShot = Instantiate(projectile, this.leftMuzzle.transform.position, this.leftMuzzle.transform.rotation);
        GameObject rightShot = Instantiate(projectile, this.rightMuzzle.transform.position, this.rightMuzzle.transform.rotation);

        // Set Color for the bullet, paint, paint explosion and amount of bounces left.
        leftShot.GetComponent<BulletController>().Setup(this.teamColor, this.bounceNumber);
        rightShot.GetComponent<BulletController>().Setup(this.teamColor, this.bounceNumber);

    }

    public void setBounceNumber(int bounceLimit) {
        this.bounceNumber = bounceLimit;
    }

    public void setTeamColor(Color teamColor) {
        this.teamColor = teamColor;
    }

    private void SpawnTurret(GameObject tank)
    {
        var t = Resources.Load("Turret", typeof(GameObject));

        GameObject turret = tank.transform.Find("DoubleBarrel").gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
            new_t.GetComponent<TurretController>().setTeamColor(tank.transform.GetComponent<TankController>().teamColor);
        }
    }
}