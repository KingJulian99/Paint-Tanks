using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PowerUpSpawnerScript : MonoBehaviour
{

    [SerializeField]
    private GameObject powerUp;

    [SerializeField]
    private int numPowerUps = 1;

    [SerializeField]
    private GameObject[] spawnPoints;

    [SerializeField]
    private GameObject spawnedPowerUps;

    private GameObject container;

    private float respawnTime;
    private bool canSpawn;


    private void Start()
    {
        InitialiseSpawnContainer();
        respawnTime = 5f;
        canSpawn = true;
    }

    private void Update()
    {
        SpawnPowerUps();
    }

    private void InitialiseSpawnContainer()
    {
        container = Instantiate(spawnedPowerUps);
    }


    private void SpawnPowerUps()
    {
        if (container.transform.childCount < numPowerUps)
        {
            if (canSpawn)
            {
                int spwn = Random.Range(0, spawnPoints.Length);

                GameObject new_powerUp = Instantiate(powerUp, spawnPoints[spwn].transform);

                new_powerUp.transform.SetParent(container.transform);

                canSpawn = false;
            }
            else
            {
                if(respawnTime > 0)
                    respawnTime -= Time.deltaTime;
                else
                {
                    respawnTime = 5;
                    canSpawn=true;
                }      
            }
        }

    }
}
