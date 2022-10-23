using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject paintManager;
    [SerializeField]
    private GameObject timer;
    [SerializeField]
    private GameObject respawnTimer;

    private GameManager manager;

    private void Awake()
    {
        GameObject gm = GameObject.Find("GameManager");
        manager = gm.GetComponent<GameManager>();

        Instantiate(paintManager);

        // Create map
        manager.currentMap = Instantiate(manager.map, manager.map.transform.position, manager.map.transform.rotation);

        if(manager.mapName == "Japanese Large")
        {
            Camera.main.transform.position = new Vector3(-28, 60, -5);
            Camera.main.transform.rotation = Quaternion.Euler(80,0,0);
        }

        manager.teamColors = new List<Color>
        {
            Color.red,
            Color.green,
            Color.blue
        };

        SpawnScript ss = manager.currentMap.transform.Find("SpawnManager").GetComponent<SpawnScript>();

        ss.SetTeamColors(manager.teamColors);
        ss.SetTimer(timer);
        ss.SetRespawnTimer(respawnTimer);

        // Load Powerups
        manager.powerUps = Resources.LoadAll("PowerUps", typeof(GameObject));
        manager.currentMap.transform.Find("PowerUpSpawner").GetComponent<PowerUpSpawnerScript>().SetPowerUps(manager.powerUps);

        manager.gameTime = 180f;
        manager.timer = timer;

        manager.OnGameSetup();
    }
}
