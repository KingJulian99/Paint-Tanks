using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = 50;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // On collision despawn cannon ball
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        //gameObject.GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject, 0.3f);
    }
}
