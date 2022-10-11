using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public delegate void VictoryNotify();

public class AITank : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private List<GameObject> targets;

    public GameObject playerContainer;
    public GameObject target;
    public bool lineOfSight;
    public bool gotTargets = false;

    public event VictoryNotify Victory;

    // Start is called before the first frame update
    public void Spawned()
    {
        targets = new List<GameObject>();

        foreach (Transform child in playerContainer.transform)
        {
            if (child.gameObject != gameObject)
            {
                targets.Add(child.gameObject);
            }
        }

        gotTargets = true;
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerContainer = GameObject.Find("PlayerContainer");
        lineOfSight = false;

        SpawnScript s = GameObject.Find("Spawner").GetComponent<SpawnScript>();
        s.SpawnDone += Spawned;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gotTargets) { return; }

        UpdateTargets();

        target = GetTarget();

        if (target != null && !LineOfSight())
        {
            // Move toward target
            navAgent.SetDestination(target.transform.position);
        }
        else
        {
            // Stop at current destination
            navAgent.SetDestination(gameObject.transform.position);
        }
    }

    private bool LineOfSight()
    {
        NavMeshHit hit; 
        
        if(!navAgent.Raycast(target.transform.position, out hit))
        {
            lineOfSight = true;
            return true;
        }

        lineOfSight = false;
        return false;
    }

    private GameObject GetTarget()
    {
        // Check if target is not dead
        if(target != null) { return target; }

        // Assign new target
        if (targets.Count > 0)
        {
            int spwn = Random.Range(0, targets.Count);
            Debug.Log("Get target: " + spwn);
            GameObject t = targets[spwn];

            return t;
        }

        OnVictory();
        return null;
    }

    private void UpdateTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
            }
        }
    }

    protected virtual void OnVictory()
    {
        Debug.Log("Victory");
        Victory?.Invoke();
    }
}
