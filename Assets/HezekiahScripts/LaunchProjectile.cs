using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaunchProjectile : MonoBehaviour
{

    [SerializeField]
    private GameObject projectile;

    private float launchVelocity;
    private float reload_time = 1;
    private bool can_shoot = true;
    private bool lineOfSight;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        launchVelocity = 1000f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.root.GetChild(0).tag == "Player")
        {
            PlayerShoot();
        }
        else
        {
            AIShoot();
        }
    }

    private void AIShoot()
    {
        lineOfSight = gameObject.transform.root.GetComponent<AITank>().lineOfSight;
        angle = gameObject.transform.parent.transform.parent.GetComponent<AIRotateTurret>().angle;

        if (angle < 10 && can_shoot && lineOfSight)
        {
            SpawnProjectile();

            can_shoot = false;
        }

        if (!can_shoot)
        {
            if (reload_time > 0)
            {
                reload_time -= Time.deltaTime;
            }
            else
            {
                reload_time = 1;
                can_shoot = true;
            }
        }
    }

    private void PlayerShoot()
    {
        if (Input.GetButtonDown("Fire1") && can_shoot)
        {
            SpawnProjectile();

            can_shoot = false;
        }

        if (!can_shoot)
        {
            if (reload_time > 0)
            {
                reload_time -= Time.deltaTime;
            }
            else
            {
                reload_time = 1;
                can_shoot = true;
            }
        }
    }

    private void SpawnProjectile()
    {
        GameObject ball = Instantiate(projectile, transform.position, transform.rotation);
        ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, launchVelocity));

        ball.transform.SetParent(gameObject.transform.root);
    }
}
