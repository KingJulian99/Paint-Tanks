using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private List<GameObject> heathBars;
    private GameObject timer;


    private float initialPhaseTime;
    private bool initialPhase;
    private bool keyboardSpawned;

    private int playerCount = 0;

    public event SpawnNotify SpawnDone;
    public event RespawnNotify RespawnDone;

    // Start is called before the first frame update
    private void Awake()
    {
        //GameObject gm = GameObject.Find("GameManager");
        //GameManager gameManager = gm.GetComponent<GameManager>();

        spawnedPlayers = new List<GameObject>();
        heathBars = new List<GameObject>();

        initialPhaseTime = 10f;
        initialPhase = true;
        keyboardSpawned = false;

        //gameManager.GameSetup += SpawnPlayers;
    }

    void OnPlayerJoined()
    {
        //SpawnPlayer(0, Color.red, 0);
    }

    private void Update()
    {
        // Check if we're in the initial phase
        if (!initialPhase) { return; }
        if(initialPhaseTime <= 0)
        {
            //End phase
            initialPhase = false;
            SpawnAIs();
        }

        initialPhaseTime -= Time.deltaTime;
        timer.transform.Find("Time").GetComponent<TextMeshProUGUI>().SetText("00:" + string.Format("{0:00}", initialPhaseTime));

        // Check if a keyboard player has joined
        if (keyboardSpawned) { return; }
        if (Input.GetKey(KeyCode.Space))
        {
            keyboardSpawned = true;

            // Check if max players have joined
            if (playerCount >= spawn_points.Length) { return; }

            SpawnKeyboardPlayer(playerCount, teamColors[playerCount], playerCount);

            playerCount++;
        }
    }

    private void SpawnPlayers()
    {
        SpawnKeyboardPlayer(0, teamColors[0], 0);

        // Spawn AI at subsequent spawn points
        for (int i = 1; i < spawn_points.Length; i++)
        {
            SpawnAI(i, teamColors[i], i);
        }

        OnSpawnDone();
    }

    private void SpawnAIs()
    {
        // Spawn AI at subsequent spawn points
        for (int i = playerCount; i < spawn_points.Length; i++)
        {
            SpawnAI(i, teamColors[i], i);
        }

        OnSpawnDone();
    }

    public void SetHealthBars(List<GameObject> hBars)
    {
        this.heathBars = hBars;
    }

    private GameObject SpawnKeyboardPlayer(int spawnPoint, Color teamColor, int hBarNumber)
    {
        // Spawn Player at first spawn point
        GameObject p = Instantiate(player, spawn_points[spawnPoint].transform.position, spawn_points[spawnPoint].transform.rotation);

        // Put player in container
        p.transform.SetParent(playerContainer.transform);

        // Set players team color
        p.transform.GetComponent<TankController>().SetTeamColor(teamColor);

        // Set UI health bar
        p.transform.GetComponent<TankController>().SetHealthBar(heathBars[hBarNumber], hBarNumber);

        // When Tank is Destroyed Respawn the player
        p.transform.GetComponent<TankController>().TankDestroyed += RespawnPlayer;

        // Add player to list of spawned players
        spawnedPlayers.Add(p);

        return p;
    }

    private GameObject SpawnAI(int spawnPoint, Color teamColor, int hBarNumber)
    {
        // Spawn Player at first spawn point
        GameObject p = Instantiate(ai, spawn_points[spawnPoint].transform.position, spawn_points[spawnPoint].transform.rotation);

        // Put player in container
        p.transform.SetParent(playerContainer.transform);

        // Set players team color
        p.transform.GetComponent<TankAIController>().SetTeamColor(teamColor);

        // Set UI health bar
        p.transform.GetComponent<TankAIController>().SetHealthBar(heathBars[hBarNumber], hBarNumber);

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


    private void RespawnPlayer(GameObject go)
    {
        int spwn = Random.Range(0, spawn_points.Length);
        GameObject p = SpawnKeyboardPlayer(spwn, go.transform.GetComponent<TankController>().teamColor, go.transform.GetComponent<TankController>().hBarNumber);

        OnRespawnDone(p);
    }

    private void RespawnAI(GameObject go)
    {
        int spwn = Random.Range(0, spawn_points.Length);
        GameObject p = SpawnKeyboardPlayer(spwn, go.transform.GetComponent<TankAIController>().teamColor, go.transform.GetComponent<TankController>().hBarNumber);

        OnRespawnDone(p);
    }
    public void SetTeamColors(List<Color> teamColors)
    {
        this.teamColors = teamColors;
    }

    public void SetTimer(GameObject timer)
    {
        this.timer = timer;
    }
}
