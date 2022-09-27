using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Tank") {
            other.gameObject.GetComponent<TankController>().SetGravity(-1.25f);
            other.gameObject.GetComponent<TankController>().RotateUncontrollably();
        }
    }
}
