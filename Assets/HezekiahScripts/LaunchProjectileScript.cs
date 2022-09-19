using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectileScript : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 1500f;
    public float reload_time = 1;
    private bool can_shoot = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && can_shoot)
        {
            GameObject ball = Instantiate(projectile, transform.position, transform.rotation);

            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, -launchVelocity));

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

}
