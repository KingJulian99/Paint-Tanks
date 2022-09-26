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

        // Reflect
        Vector3 reflectedDirection = Vector3.Reflect(this.transform.forward, other.contacts[0].normal);
        this.transform.rotation = Quaternion.LookRotation(reflectedDirection);

        // Paint
        Paintable p = other.collider.GetComponent<Paintable>();
        if(p != null){
            PaintManager.instance.paint(p, other.contacts[0].point, radius, hardness, strength, paintColor);
        }
    }
}
