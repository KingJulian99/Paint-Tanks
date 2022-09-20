using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TankProperties : NetworkBehaviour
{

    [SerializeField]
    private int health = 100;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            int damage = collision.gameObject.GetComponent<ProjectileProperties>().damage;

            health -= damage;

            Debug.Log(health);

            if(health <= 0)
            {
                DestroyTank();
            }
        }
    }

    private void DestroyTank()
    {
        GameObject turret = gameObject.transform.GetChild(0).gameObject;
        turret.AddComponent<Rigidbody>();

        turret.GetComponent<Rigidbody>().AddForce(new Vector3(0,10f,0), mode: ForceMode.Impulse);
        Destroy(gameObject, 2);
    }

}
