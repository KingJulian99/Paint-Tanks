using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // My variable instantiation is whack
    public float bulletSpeed;
    public int bouncesLeft;
    public Color paintColor;
    public float radius = 3;
    public float strength = 1;
    public float hardness = 0.3f;
    public Material splashMaterial;

    private int paintExplodeQuantity = 9;
    
    void Start()
    {
        this.bulletSpeed = 10.0f;
    }

    public void Setup(Color teamColor, int bounceLimit) {

        this.bouncesLeft = bounceLimit;

        this.paintColor = teamColor;

        this.SetColors();

    }

    void SetColors() {
        this.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", this.paintColor);

        Material newMaterial = Instantiate(splashMaterial);
        newMaterial.SetColor("_Color", this.paintColor);
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().trailMaterial = newMaterial;
    }

    void FixedUpdate()
    {
        this.transform.Translate(Vector3.forward * bulletSpeed * Time.fixedDeltaTime);
        //Debug.DrawRay(this.transform.position, -this.transform.forward * 10, Color.green, 10f);

        RaycastHit hit;

        if(Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity)) {
            Debug.DrawRay(transform.position, -Vector3.up * hit.distance, Color.red);

            Paintable p = hit.collider.GetComponent<Paintable>();
            if(p != null){
                PaintManager.instance.paint(p, hit.point, radius, hardness, 0.1f, paintColor);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {

        // Get other paintable object if available
        Paintable p = other.collider.GetComponent<Paintable>();

        this.bouncesLeft--;

        if(this.bouncesLeft < 0) {
            // Paint largely on wall
            if(p != null){
                PaintManager.instance.paint(p, other.contacts[0].point, radius * 3, hardness, strength, paintColor);
            }

            // Destroy the ball and paint on floor
            Explode(other);

            // End prematurely
            return;
        }

        // Reflect
        Vector3 reflectedDirection = Vector3.Reflect(this.transform.forward, other.contacts[0].normal);
        this.transform.rotation = Quaternion.LookRotation(reflectedDirection);

        // Paint normally
        if(p != null){
            PaintManager.instance.paint(p, other.contacts[0].point, radius, hardness, strength, paintColor);
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Tank") {
            // Take health from the other tank 🔪 (if it's on a different team!)
            if( !(other.gameObject.GetComponent<TankController>().teamColor == this.paintColor) ) {
                other.gameObject.GetComponent<TankController>().health -= 20;
            }

            // Make this paintball explode
            this.bouncesLeft = 0;
            Explode(other);
        }

        if(other.gameObject.tag == "GamepadTank") {
            // Take health from the other tank 🔪 (if it's on a different team!)
            if( !(other.gameObject.GetComponent<GamepadTankController>().teamColor == this.paintColor) ) {
                other.gameObject.GetComponent<GamepadTankController>().health -= 50;
            }

            // Make this paintball explode
            this.bouncesLeft = 0;
            Explode(other);
        }
        
        if(other.gameObject.tag == "AI")
        {
            if (!(other.gameObject.GetComponent<TankAIController>().teamColor == this.paintColor))
            {
                other.gameObject.GetComponent<TankAIController>().health -= 20;
            }

            this.bouncesLeft = 0;
            Explode(other);
        }

    }

    void Explode<T>(T other) { // other is either collider or collision
        
        // Raycast and spread paint on the floor

        float yDrift = 30f;
        float xDrift = 20f;
        
        Vector3 direction = -Vector3.up;
        direction = Quaternion.AngleAxis(-yDrift, Vector3.forward) * direction;
        RaycastHit hit;

        for(int i = 0; i < paintExplodeQuantity; i++) {

            direction = Quaternion.AngleAxis(xDrift, Vector3.right) * direction;

            if(i % 3 == 0 && i != 0) {
                direction = Quaternion.AngleAxis(-xDrift*3, Vector3.right) * direction;
                direction = Quaternion.AngleAxis(yDrift, Vector3.forward) * direction;
            }

            //Debug.DrawRay(transform.position, direction * 20f, Color.green, 100f);

            if(Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity)) {

                Paintable p = hit.collider.GetComponent<Paintable>();
                if(p != null){
                    PaintManager.instance.paint(p, hit.point, radius, hardness, 0.8f, paintColor);
                }
            }
        }

        // "Destroy" the bullet
        this.bulletSpeed = 0f;
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<SphereCollider>());
        Destroy(this.GetComponent<MeshRenderer>());
        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        // Actually destroy the bullet after the ParticleSystem is done.
        Destroy(this.gameObject, 5.0f);
    }
}