using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMuzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject pivot;

    private float rotSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(pivot.transform.position, gameObject.transform.forward, rotSpeed * Time.deltaTime);
    }
}
