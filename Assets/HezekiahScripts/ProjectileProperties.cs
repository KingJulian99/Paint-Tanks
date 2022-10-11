using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public int damage;

    // Start is called before the first frame update
    private void Start()
    {
        damage = 50;
    }

    // On collision despawn cannon ball
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "AI")
        {
            if (gameObject.GetComponent<TankProperties>())
            {
                gameObject.GetComponent<TankProperties>().TakeDamage(damage);
            }
        }

        gameObject.GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject, 0.3f);
    }
}
