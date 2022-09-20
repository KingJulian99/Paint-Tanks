using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed;
    public int bouncesLeft;
    
    void Start()
    {
        this.bulletSpeed = 10f;
        this.bouncesLeft = 2;
    }

    void FixedUpdate()
    {
        this.transform.Translate(Vector3.forward * bulletSpeed * Time.fixedDeltaTime);
        //Debug.DrawRay(this.transform.position, -this.transform.forward * 10, Color.green, 10f);
    }

    void OnCollisionEnter(Collision other)
    {
        this.bouncesLeft--;

        if(this.bouncesLeft < 0) {
            //Destroy(this.gameObject);
            this.GetComponent<Renderer>().material.color = Color.red;
            this.bulletSpeed = 0f;
            print("Paintball destroyed!");
        }

        Vector3 reflectedDirection = Vector3.Reflect(this.transform.forward, other.contacts[0].normal);
        this.transform.rotation = Quaternion.LookRotation(reflectedDirection);
    }
}
