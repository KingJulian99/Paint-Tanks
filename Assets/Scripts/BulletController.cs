using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletSpeed;
    public int bouncesLeft;
    public Color paintColor;
    public float radius = 3;
    public float strength = 1;
    public float hardness = 0.3f;
    
    void Start()
    {
        this.bulletSpeed = 10f;
        Material test = this.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().trailMaterial;
        test.SetColor("_Color", paintColor);
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
            Explode(other);
        }

        // Reflect
        Vector3 reflectedDirection = Vector3.Reflect(this.transform.forward, other.contacts[0].normal);
        this.transform.rotation = Quaternion.LookRotation(reflectedDirection);

        // Paint
        Paintable p = other.collider.GetComponent<Paintable>();
        if(p != null){
            PaintManager.instance.paint(p, other.contacts[0].point, radius, hardness, strength, paintColor);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Tank") {
            // Take health from the other tank 🔪 (if it's on a different team!)
            if( !(other.gameObject.GetComponent<TankController>().teamColor == this.paintColor) ) {
                other.gameObject.GetComponent<TankController>().health -= 50;
            }

            // Make this paintball explode
            this.bouncesLeft = 0;
            Explode(other);
        }
    }

    void Explode<T>(T other) { // other is either collider or collision
        // "Destroy" the bullet
        this.bulletSpeed = 0f;
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<SphereCollider>());
        Destroy(this.GetComponent<MeshRenderer>());
        this.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        // Actually destroy the bullet after the ParticleSystem is done.
        Destroy(this.gameObject, 5.0f);
    }
}
