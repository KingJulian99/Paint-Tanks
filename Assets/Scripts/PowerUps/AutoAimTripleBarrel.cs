using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimTripleBarrel : MonoBehaviour
{
    private bool autoAim;

    private void Start()
    {
        GetComponent<TripleBarrelTurretAIController>().enabled = false;
        GetComponent<TripleBarrelTurretController>().enabled = true;
        autoAim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ActivateAutoAim();
        }
    }

    private void ActivateAutoAim()
    {
        if (autoAim)
        {
            GetComponent<TripleBarrelTurretAIController>().enabled = false;
            GetComponent<TripleBarrelTurretController>().enabled = true;
            autoAim = false;
        }
        else
        {
            GetComponent<TripleBarrelTurretAIController>().enabled = true;
            GetComponent<TripleBarrelTurretController>().enabled = false;
            autoAim = true;
        }
    }
}
