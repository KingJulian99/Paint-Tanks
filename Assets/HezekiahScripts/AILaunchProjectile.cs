using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILaunchProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    private GameObject projectile;
    private float angle;

    private float launchVelocity;
    private float reload_time = 1;
    private bool can_shoot = true;
    private bool lineOfSight;

    // Start is called before the first frame update
    void Start()
    {
        launchVelocity = 1000f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!parent.GetComponent<AITank>().gotTargets) { return; }

        lineOfSight = parent.GetComponent<AITank>().lineOfSight;
        angle = parent.transform.GetChild(0).GetComponent<AIRotateTurret>().angle;

        if (angle<10 && can_shoot && lineOfSight)
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
