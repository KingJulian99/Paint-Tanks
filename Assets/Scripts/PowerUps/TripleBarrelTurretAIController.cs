using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TripleBarrelTurretAIController : MonoBehaviour
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
    private const float RELOADTIME = 1f;

    public float maxRotationSpeed;
    public Camera viewCamera;
    public GameObject projectile;

    [SerializeField]
    private GameObject barrel;

    private TankController tankController;
    private GameObject leftMuzzle, rightMuzzle, middleMuzzle;
    private GameObject leftEffectObject, rightEffectObject, middleEffectObject;
    private ParticleSystem leftShootingEffect, rightShootingEffect, middleShootingEffect;

    private int bounceNumber;
    private Color teamColor;

    private float reloadTime;
    private bool canShoot;

    private float duration;

    // AI fields
    private NavMeshAgent navAgent;
    private GameObject target;
    private float angle;
    private List<GameObject> targets;
    private GameObject playerContainer;
    private bool gotTargets = false;

    void Start()
    {
        this.middleMuzzle = this.barrel.transform.GetChild(0).gameObject;
        this.rightMuzzle = this.barrel.transform.GetChild(1).gameObject;
        this.leftMuzzle = this.barrel.transform.GetChild(2).gameObject;

        this.middleEffectObject = this.leftMuzzle.transform.GetChild(0).gameObject;
        this.leftEffectObject = this.leftMuzzle.transform.GetChild(0).gameObject;
        this.rightEffectObject = this.rightMuzzle.transform.GetChild(0).gameObject;

        this.middleShootingEffect = this.middleEffectObject.GetComponent<ParticleSystem>();
        this.leftShootingEffect = this.leftEffectObject.GetComponent<ParticleSystem>();
        this.rightShootingEffect = this.rightEffectObject.GetComponent<ParticleSystem>();

        this.viewCamera = Camera.main;

        this.tankController = this.gameObject.transform.parent.GetComponent<TankController>();
        this.teamColor = tankController.teamColor;
        this.bounceNumber = tankController.bounceNumber;
        this.maxRotationSpeed = 10f;
        this.reloadTime = RELOADTIME;
        this.canShoot = true;

        duration = 20f;

        navAgent = GetComponent<NavMeshAgent>();
        playerContainer = GameObject.Find("PlayerContainer");
        
        SetUpAI();

        SpawnScript s = GameObject.Find("SpawnManager").GetComponent<SpawnScript>();
        s.RespawnDone += AddToTargets;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            SpawnTurret(this.transform.parent.gameObject);
        }

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

    void LookAt()
    {
        Vector3 pointToLook = (target.transform.position - gameObject.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(pointToLook.x, 0, pointToLook.z));
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * maxRotationSpeed);

        angle = Vector3.Angle(pointToLook, gameObject.transform.forward);
    }

    void Shoot() {
        // Smoke 😎🚬
        this.middleShootingEffect.Play();
        this.leftShootingEffect.Play();
        this.rightShootingEffect.Play();

        // Animate
        this.barrel.GetComponent<Animator>().SetTrigger("Shoot");

        // Get number of bounces
        this.setBounceNumber(this.tankController.bounceNumber);

        // Set team color
        this.setTeamColor(this.tankController.teamColor);

        // Skiet mos
        GameObject middleShot = Instantiate(projectile, this.middleMuzzle.transform.position, this.middleMuzzle.transform.rotation);
        GameObject leftShot = Instantiate(projectile, this.leftMuzzle.transform.position, this.leftMuzzle.transform.rotation);
        GameObject rightShot = Instantiate(projectile, this.rightMuzzle.transform.position, this.rightMuzzle.transform.rotation);

        // Set Color for the bullet, paint, paint explosion and amount of bounces left.
        middleShot.GetComponent<BulletController>().Setup(this.teamColor, this.bounceNumber);
        leftShot.GetComponent<BulletController>().Setup(this.teamColor, this.bounceNumber);
        rightShot.GetComponent<BulletController>().Setup(this.teamColor, this.bounceNumber);
    }

    public void setBounceNumber(int bounceLimit) {
        this.bounceNumber = bounceLimit;
    }

    public void setTeamColor(Color teamColor) {
        this.teamColor = teamColor;

        Material tm = gameObject.GetComponent<Renderer>().material;
        tm.SetColor("_BaseColor", this.teamColor);
    }

    private void SpawnTurret(GameObject tank)
    {
        var t = Resources.Load("Turret", typeof(GameObject));

        GameObject turret = tank.transform.Find("TripleBarrel").gameObject;
        Transform t_transform = turret.transform;

        Destroy(turret);

        if (t != null)
        {
            GameObject new_t = Instantiate(t, t_transform) as GameObject;
            new_t.transform.SetParent(tank.transform);
            new_t.name = "Turret";
            new_t.GetComponent<TurretController>().setTeamColor(tank.transform.GetComponent<TankController>().teamColor);
        }
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
        List<float> distance = new List<float>();
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