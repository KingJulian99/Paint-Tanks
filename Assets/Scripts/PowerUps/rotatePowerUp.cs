using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatePowerUp : MonoBehaviour
{
    private float rotSpeed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(0, Time.deltaTime * rotSpeed * 100f, 0);
    }
}
