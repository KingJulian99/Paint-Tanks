using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public delegate void SpawnNotify();
public delegate void RespawnNotify(GameObject go);

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ai, player, playerContainer;

    [SerializeField]
    private GameObject[] spawn_points;

    private List<Color> teamColors;

    private List<GameObject> spawnedPlayers;

    public event SpawnNotify SpawnDone;
    public event RespawnNotify RespawnDone;

    // Start is called before the first frame update
    private void Awake()
    {
        GameObject gm = GameObject.Find("GameManager");
        GameManager gameManager = gm.GetComponent<GameManager>();
        spawnedPlayers = new List<GameObject>();

        gameManager.GameSetup += SpawnPlayers;
    }

    private void SpawnPlayers()
    {
        SpawnPlayer(0, teamColors[0]);

        // Spawn AI at subsequent spawn points
        for (int i = 1; i < spawn_points.Length; i++)
        {
            SpawnAI(i, teamColors[i]);
        }

        OnSpawnDone();
    }

    private GameObject SpawnPlayer(int spawnPoint, Color teamColor)
    {
        // Spawn Player at first spawn point
        GameObject p = Instantiate(player, spawn_points[spawnPoint].transform.position, spawn_points[spawnPoint].transform.rotation);

        // Put player in container
        p.transform.SetParent(playerContainer.transform);

        // Set players team color
        p.transform.GetComponent<TankController>().SetTeamColor(teamColor);

        // When Tank is Destroyed Respawn the player
        p.transform.GetComponent<TankController>().TankDestroyed += RespawnPlayer;

        // Add player to list of spawned players
        spawnedPlayers.Add(p);

        return p;
    }

    private GameObject SpawnAI(int spawnPoint, Color teamColor)
    {
        // Spawn Player at first spawn point
        GameObject p = Instantiate(ai, spawn_points[spawnPoint].transform.position, spawn_points[spawnPoint].transform.rotation);

        // Put player in container
        p.transform.SetParent(playerContainer.transform);

        // Set players team color
        p.transform.GetComponent<TankAIController>().SetTeamColor(teamColor);

        // When Tank is Destroyed Respawn the player
        //p.transform.GetComponent<TankAIController>().TankDestroyed += RespawnAI;

        // Add player to list of spawned players
        spawnedPlayers.Add(p);

        return p;
    }

    protected virtual void OnSpawnDone()
    {
        SpawnDone?.Invoke();
    }

    protected virtual void OnRespawnDone(GameObject go)
    {
        Debug.Log("Done Respawning");
        RespawnDone?.Invoke(go);
    }

    public void SetTeamColors(List<Color> teamColors)
    {
        this.teamColors = teamColors;
    }

    private void RespawnPlayer(GameObject go)
    {
        int spwn = Random.Range(0, spawn_points.Length);
        GameObject p = SpawnPlayer(spwn, go.transform.GetComponent<TankController>().teamColor);

        OnRespawnDone(p);
    }

    private void RespawnAI(GameObject go)
    {
        int spwn = Random.Range(0, spawn_points.Length);
        GameObject p = SpawnPlayer(spwn, go.transform.GetComponent<TankAIController>().teamColor);

        OnRespawnDone(p);
    }
}
