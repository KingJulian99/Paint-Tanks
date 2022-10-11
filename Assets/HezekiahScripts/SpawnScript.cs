using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SpawnNotify();

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ai, player, playerContainer;

    [SerializeField]
    private GameObject[] spawn_points;

    public event SpawnNotify SpawnDone;

    // Start is called before the first frame update
    private void Awake()
    {
        GameObject p = Instantiate(player, spawn_points[0].transform.position, spawn_points[0].transform.rotation);
        p.transform.SetParent(playerContainer.transform);

        for (int i = 1; i < spawn_points.Length; i++)
        {
            p = Instantiate(ai, spawn_points[i].transform.position, spawn_points[i].transform.rotation);
            p.transform.SetParent(playerContainer.transform);
        }

        OnSpawnDone();
    }

    protected virtual void OnSpawnDone()
    {
        SpawnDone?.Invoke();
    }
}
