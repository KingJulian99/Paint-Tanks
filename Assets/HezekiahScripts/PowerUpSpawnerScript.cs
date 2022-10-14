using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PowerUpSpawnerScript : MonoBehaviour
{

    [SerializeField]
    private Object[] powerUps;

    [SerializeField]
    private int numPowerUps = 1;

    [SerializeField]
    private GameObject[] spawnPoints;

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
        GameObject spawnedPowerUps = new GameObject("PowerUpContainer");
        container = Instantiate(spawnedPowerUps);
    }


    private void SpawnPowerUps()
    {
        if (container.transform.childCount < numPowerUps)
        {
            if (canSpawn)
            {
                int spwn = Random.Range(0, spawnPoints.Length);
                int pwr = Random.Range(0, powerUps.Length);

                GameObject new_powerUp = Instantiate(powerUps[pwr], spawnPoints[spwn].transform) as GameObject;

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

    public void SetPowerUps(Object[] powerUps)
    {
        this.powerUps = powerUps;
    }
}
