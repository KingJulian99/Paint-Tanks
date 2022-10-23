using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoAimTurretController : MonoBehaviour
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
    private TankController tankController;
    private int bounceNumber;
    private Color teamColor;

    private float reloadTime;
    private bool canShoot;

    // AI fields
    private NavMeshAgent navAgent;
    private GameObject target;
    private float angle;
    private List<GameObject> targets;
    private GameObject playerContainer;
    private bool gotTargets = false;

    void Start()
    {
        Debug.Log("Setup ai turret");

        this.barrel = this.gameObject.transform.GetChild(0).gameObject;
        this.muzzle = this.barrel.transform.GetChild(0).gameObject;
        this.effectObject = this.muzzle.transform.GetChild(0).gameObject;
        this.shootingEffect = this.effectObject.GetComponent<ParticleSystem>();
        this.viewCamera = Camera.main;

        this.tankController = this.gameObject.transform.parent.GetComponent<TankController>();
        this.teamColor = tankController.teamColor;
        this.bounceNumber = tankController.bounceNumber;
        this.maxRotationSpeed = 10f;
        this.reloadTime = 1;
        this.canShoot = true;

        navAgent = GetComponent<NavMeshAgent>();
        playerContainer = GameObject.Find("PlayerContainer");

        SetUpAI();

        SpawnScript s = GameObject.Find("SpawnManager").GetComponent<SpawnScript>();
        s.RespawnDone += AddToTargets;
    }

    void Update()
    {
        if (!this.gotTargets) { return; }

        UpdateTargets();

        target = GetTarget();

        if (target == null) { return; }

        this.LookAt();

        // Check if angle between this and target is less than 10
        // and there is line of sight target
        if (this.angle < 10 && this.canShoot && LineOfSight())
        {
            this.Shoot();
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

    public void VictoryDance()
    {
        this.gameObject.transform.Rotate(0, Time.deltaTime * this.maxRotationSpeed * 100f, 0);
    }

    private void SetUpAI()
    {
        targets = new List<GameObject>();

        foreach (Transform child in playerContainer.transform)
        {
            if (child.gameObject != gameObject.transform.parent.gameObject)
            {
                targets.Add(child.gameObject);
            }
        }

        gotTargets = true;
    }

    // Checks whether the AI has line of sight of its target
    private bool LineOfSight()
    {
        NavMeshHit hit;

        if (!navAgent.Raycast(target.transform.position, out hit))
        {
            return true;
        }

        return false;
    }

    // Randomly get target from list of spawned players and AI
    private GameObject GetTarget()
    {
        // Assign new target
        if (targets.Count > 0)
        {
            GameObject t = GetClosest();

            return t;
        }

        return null;
    }

    private GameObject GetClosest()
    {
        List<float> distance = new();
        foreach (GameObject t in targets)
        {
            distance.Add(Vector3.Distance(gameObject.transform.position, t.transform.position));
        }

        int min = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            if (distance[i] <= distance[min])
            {
                min = i;
            }
        }

        return targets[min];
    }

    // Updates the list of targets by removing players that aren't
    // in the scene anymore
    private void UpdateTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
        }
    }

    // When a player respawns add them to the list of targets
    private void AddToTargets(GameObject go)
    {
        targets.Add(go);
    }
}