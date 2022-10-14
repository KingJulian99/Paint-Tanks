using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProperties : MonoBehaviour
{
    private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Projectile")
    //    {
    //        int damage = collision.gameObject.GetComponent<ProjectileProperties>().damage;

    //        TakeDamage(damage);

    //        if(health <= 0)
    //        {
    //            DestroyTank();
    //        }
    //    }

    //}

    private void Update()
    {
        if(health <= 0)
        {
            DestroyTank();
        }
    }

    private void DestroyTank()
    {
        GameObject turret = gameObject.transform.GetChild(0).gameObject;

        turret.AddComponent<Rigidbody>();

        turret.GetComponent<Rigidbody>().AddForce(new Vector3(0,10f,0), mode: ForceMode.Impulse);
        
        Destroy(gameObject, 1f);
    }

}
