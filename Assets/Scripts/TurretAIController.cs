using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAIController : MonoBehaviour
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

    private int bounceNumber;
    private Color teamColor;

    private float reloadTime;
    private bool canShoot;

    // AI fields
    private TankAIController tankAIController;
    private GameObject target;
    private bool lineOfSight;
    private float angle;

    void Start()
    {
        this.barrel = this.gameObject.transform.GetChild(0).gameObject;
        this.muzzle = this.barrel.transform.GetChild(0).gameObject;
        this.effectObject = this.muzzle.transform.GetChild(0).gameObject;
        this.shootingEffect = this.effectObject.GetComponent<ParticleSystem>();
        this.viewCamera = Camera.main;

        this.tankAIController = this.gameObject.transform.parent.GetComponent<TankAIController>();
        this.teamColor = tankAIController.teamColor;
        this.bounceNumber = tankAIController.bounceNumber;
        this.maxRotationSpeed = 10f;
        this.reloadTime = 1;
        this.canShoot = true;

        tankAIController.Victory += VictoryDance;
        
    }

    void Update()
    {
        if(!this.tankAIController.gotTargets) { return; }

        this.target = this.tankAIController.target;
        this.lineOfSight = this.tankAIController.lineOfSight;

        if(target == null) { return; }

        this.LookAt();

        // Check if angle between this and target is less than 10
        // and there is line of sight target
        if(this.angle < 10 && this.canShoot && this.lineOfSight) {
            this.Shoot();
            TankSounds.PlayShoot();
            this.canShoot = false;
        }

        if (!this.canShoot)
        {
            if (this.reloadTime > 0)
            {
                this.reloadTime -= Time.deltaTime;
            }
            else
            {
                this.reloadTime = 1;
                this.canShoot = true;
            }
        }
    }

    void LookAt() {
        Vector3 pointToLook = (target.transform.position - gameObject.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(pointToLook.x, 0, pointToLook.z));
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * maxRotationSpeed);

        angle = Vector3.Angle(pointToLook, gameObject.transform.forward);
    }

    void Shoot() {
        // Smoke 😎🚬
        this.shootingEffect.Play();

        // Animate
        this.barrel.GetComponent<Animator>().SetTrigger("Shoot");

        // Get number of bounces
        this.setBounceNumber(this.tankAIController.bounceNumber);

        // Set team color
        this.setTeamColor(this.tankAIController.teamColor);

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

    public void VictoryDance()
    {
        this.gameObject.transform.Rotate(0, Time.deltaTime * this.maxRotationSpeed * 100f, 0);
    }
}