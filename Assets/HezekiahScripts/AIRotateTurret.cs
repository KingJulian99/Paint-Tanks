using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRotateTurret : MonoBehaviour
{
    [SerializeField]
    public GameObject parent;
    
    public float angle;

    private GameObject target;
    private AITank aiScript;

    private float turnSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        aiScript = parent.GetComponent<AITank>();
        aiScript.Victory += VictoryDance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!aiScript.gotTargets) { return; }

        target = aiScript.target;

        //if(target != null) { return; }

        Vector3 pointToLook = (target.transform.position - gameObject.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(pointToLook.x, 0, pointToLook.z));
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        angle = Vector3.Angle(pointToLook, gameObject.transform.forward);
    }

    public void VictoryDance()
    {
        gameObject.transform.Rotate(0, Time.deltaTime * turnSpeed, 0);
    }
}
