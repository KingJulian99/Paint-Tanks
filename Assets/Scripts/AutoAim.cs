using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAim : MonoBehaviour
{
    private bool autoAim;

    private void Awake()
    {
        GetComponent<AutoAimTurretController>().enabled = false;
        GetComponent<TurretController>().enabled = true;
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
        Debug.Log("Toggle Auto Aim");

        if (autoAim)
        {
            GetComponent<AutoAimTurretController>().enabled = false;
            GetComponent<TurretController>().enabled = true;
            autoAim = false;
        }
        else
        {
            GetComponent<AutoAimTurretController>().enabled = true;
            GetComponent<TurretController>().enabled = false;
            autoAim = true;
        }
    }
}
