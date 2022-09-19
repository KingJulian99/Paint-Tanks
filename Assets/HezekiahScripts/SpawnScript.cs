using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ai, player;

    [SerializeField]
    private GameObject[] spawn_points;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, spawn_points[0].transform.position, spawn_points[0].transform.rotation);

        for (int i = 1; i < spawn_points.Length; i++)
        {
            Instantiate(ai, spawn_points[i].transform.position, spawn_points[i].transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
