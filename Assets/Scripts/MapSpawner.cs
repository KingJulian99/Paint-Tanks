using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject paintManager;

    private GameManager manager;

    private void Awake()
    {
        GameObject gm = GameObject.Find("GameManager");
        manager = gm.GetComponent<GameManager>();

        Instantiate(paintManager);

        // Create map
        manager.currentMap = Instantiate(manager.map, new Vector3(0, 0, 0), new Quaternion());

        manager.teamColors = new List<Color>();

        manager.teamColors.Add(Color.red);
        manager.teamColors.Add(Color.green);
        manager.teamColors.Add(Color.blue);

        // Get team colors
        //for (int i = 0; i < 4; i++)
        //{
        //    teamColors.Add(new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
        //}

        manager.currentMap.transform.Find("SpawnManager").GetComponent<SpawnScript>().SetTeamColors(manager.teamColors);

        // Load Powerups
        manager.powerUps = Resources.LoadAll("PowerUps", typeof(GameObject));
        manager.currentMap.transform.Find("PowerUpSpawner").GetComponent<PowerUpSpawnerScript>().SetPowerUps(manager.powerUps);

        manager.gameTime = 180f;

        manager.OnGameSetup();
    }
}
