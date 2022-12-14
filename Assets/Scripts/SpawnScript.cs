using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;
using System.Linq;

public delegate void SpawnNotify();
public delegate void RespawnNotify(GameObject go);

public class SpawnScript : MonoBehaviour
{
    private const float RESPAWNTIME = 10f;

    [SerializeField]
    private GameObject ai, player, playerContainer;

    [SerializeField]
    private GameObject[] spawn_points;

    private List<Color> teamColors;

    private List<GameObject> spawnedPlayers;
    private GameObject timer;
    private GameObject respawnTimer;
    private float respawn;

    private List<Color> respawnQueueKeyboard;
    private List<Color> respawnQueueAI;
    private List<GameObject> respawnQueueGamePad;

    private float initialPhaseTime;
    private bool initialPhase;
    private bool keyboardSpawned;

    private int playerCount = 0;

    public event SpawnNotify SpawnDone;
    public event RespawnNotify RespawnDone;

    // Start is called before the first frame update
    private void Awake()
    {
        spawnedPlayers = new List<GameObject>();
        respawnQueueKeyboard = new List<Color>();
        respawnQueueAI = new List<Color>();
        respawnQueueGamePad = new List<GameObject>();

        respawn = RESPAWNTIME;

        initialPhaseTime = 10f;
        initialPhase = true;
        keyboardSpawned = false;
    }

    
    void OnPlayerJoined(PlayerInput playerInput) {
        print("Controller added");

        DeactivateGamepadTankPlayer(playerInput.gameObject);

        SendToPrison(playerInput.gameObject);
        
        if (playerCount >= spawn_points.Length) { return; }
    
        VirtualSpawnGamepadTankPlayer(playerInput.gameObject, playerCount, teamColors[playerCount]);

        playerCount++;
    }

    
    private void SendToPrison(GameObject gamepadTankPlayer) {
        gamepadTankPlayer.transform.position = new Vector3(0,-10,0);
    }

    
    private GameObject VirtualSpawnGamepadTankPlayer(GameObject playerInput, int spawnPoint, Color teamColor) {
        // Sets-up the gamepad tank player as in SpawnKeyboardPlayer.
        // "Virtually" since it is automatically instantiated beforehand by the player input controller.

        // Move to spawn position
        TeleportGamepadTankPlayer(playerInput, spawnPoint);

        // Put player in container
        playerInput.transform.SetParent(playerContainer.transform);

        // Activate the player (This needs to be done in order to change the teamcolor!)
        ActivateGamepadTankPlayer(playerInput);

        // // Set players team color
        playerInput.transform.GetComponent<GamepadTankController>().teamColor = teamColor; // This is not OOP and I'm not sorry!

        // Haven't set healthbar yet..

        // When Tank is Destroyed Respawn the player
        playerInput.transform.GetComponent<GamepadTankController>().TankDestroyed += RespawnQueue;

        // Add player to list of spawned players
        spawnedPlayers.Add(playerInput);

        return playerInput;
    }

    
    private void TeleportGamepadTankPlayer(GameObject gamepadTankPlayer, int spawnPoint) {
        gamepadTankPlayer.transform.position = spawn_points[spawnPoint].transform.position;
        gamepadTankPlayer.transform.rotation = spawn_points[spawnPoint].transform.rotation;
    }

    
    private void ActivateGamepadTankPlayer(GameObject playerInput) {
        playerInput.transform.GetComponent<CharacterController>().enabled = true;
        playerInput.transform.GetComponent<TankMovement>().enabled = true;
        playerInput.transform.GetComponent<GamepadTankController>().enabled = true;
    }

    
    private void DeactivateGamepadTankPlayer(GameObject playerInput) {
        playerInput.transform.GetComponent<CharacterController>().enabled = false;
        playerInput.transform.GetComponent<GamepadTankController>().enabled = false;
        playerInput.transform.GetComponent<TankMovement>().enabled = false;
    }

    private void Update()
    {
        if (!initialPhase)
        {
            respawn -= Time.deltaTime;
            respawnTimer.transform.Find("Time").GetComponent<TextMeshProUGUI>().SetText("00:" + string.Format("{0:00}", respawn));

            if (respawn <= 0)
            {
                RespawnPlayers();
                Debug.Log("Respawn Players");
                respawn = RESPAWNTIME;
            }
        }

        // If not in initial phase, end. 
        if (!initialPhase) { return; }

        if(initialPhaseTime <= 0)
        {
            //End phase
            initialPhase = false;

            timer.transform.Find("Label").GetComponent<TextMeshProUGUI>().SetText("Time:");

            SpawnAIs();
        }

        initialPhaseTime -= Time.deltaTime;
        timer.transform.Find("Time").GetComponent<TextMeshProUGUI>().SetText("00:" + string.Format("{0:00}", initialPhaseTime));

        // Check if a keyboard player has joined, if so, end (because only one keyboard player can join)
        if (keyboardSpawned) { return; }

        if (Input.GetKey(KeyCode.Space))
        {
            keyboardSpawned = true;

            // Check if max players have joined
            if (playerCount >= spawn_points.Length) { return; }

            SpawnKeyboardPlayer(playerCount, teamColors[playerCount]);

            playerCount++;
        }
    }

    private void SpawnPlayers()
    {
        SpawnKeyboardPlayer(0, teamColors[0]);

        // Spawn AI at subsequent spawn points
        for (int i = 1; i < spawn_points.Length; i++)
        {
            TankSounds.PlayHeal();
            SpawnAI(i, teamColors[i]);
        }

        OnSpawnDone();
    }

    private void SpawnAIs()
    {
        // Spawn AI at subsequent spawn points
        for (int i = playerCount; i < spawn_points.Length; i++)
        {
            SpawnAI(i, teamColors[i]);
        }

        OnSpawnDone();
    }

    //public void SetHealthBars(List<GameObject> hBars)
    //{
    //    this.heathBars = hBars;
    //}

    private GameObject SpawnKeyboardPlayer(int spawnPoint, Color teamColor)
    {
        // Spawn Player at first spawn point
        GameObject p = Instantiate(player, spawn_points[spawnPoint].transform.position, spawn_points[spawnPoint].transform.rotation);

        // Put player in container
        p.transform.SetParent(playerContainer.transform);

        // Set players team color
        p.transform.GetComponent<TankController>().SetTeamColor(teamColor);

        // Set UI health bar
        //p.transform.GetComponent<TankController>().SetHealthBar(heathBars[hBarNumber], hBarNumber);

        // When Tank is Destroyed Respawn the player
        p.transform.GetComponent<TankController>().TankDestroyed += RespawnQueue;

        // Add player to list of spawned players
        spawnedPlayers.Add(p);
        TankSounds.PlayHeal();

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
        p.transform.GetComponent<TankAIController>().TankAIDestroyed += RespawnQueue;

        // Add player to list of spawned players
        spawnedPlayers.Add(p);
        TankSounds.PlayHeal();

        return p;
    }

    protected virtual void OnSpawnDone()
    {
        SpawnDone?.Invoke();
    }

    protected virtual void OnRespawnDone(GameObject go)
    {
        RespawnDone?.Invoke(go);
    }


    private void RespawnPlayers()
    {
        foreach(Color keyboard in respawnQueueKeyboard.ToList())
        {
            TankSounds.PlayHeal();
            int spwn = Random.Range(0, spawn_points.Length);
            GameObject p = SpawnKeyboardPlayer(spwn, keyboard);
            respawnQueueKeyboard.Remove(keyboard);

            OnRespawnDone(p);
        }

        foreach (GameObject gamepad in respawnQueueGamePad.ToList())
        {
            TankSounds.PlayHeal();
            int spwn = Random.Range(0, spawn_points.Length);
            VirtualSpawnGamepadTankPlayer(gamepad, spwn, gamepad.transform.GetComponent<GamepadTankController>().teamColor);
            respawnQueueGamePad.Remove(gamepad);

            OnRespawnDone(gamepad);
        }

        foreach (Color aiCol in respawnQueueAI.ToList())
        {
            TankSounds.PlayHeal();
            int spwn = Random.Range(0, spawn_points.Length);
            GameObject p = SpawnAI(spwn, aiCol);
            respawnQueueAI.Remove(aiCol);

            p.GetComponent<TankAIController>().SetUpAI();
            OnRespawnDone(p);
        }
    }

    private void RespawnQueue(GameObject go)
    {
        if (go.CompareTag("Tank"))
        {
            respawnQueueKeyboard.Add(go.transform.GetComponent<TankController>().teamColor);
        }
        else if (go.CompareTag("GamepadTank"))
        {
            respawnQueueGamePad.Add(go);
        }
        else
        {
            respawnQueueAI.Add(go.transform.GetComponent<TankAIController>().teamColor);
        }
    }
    public void SetTeamColors(List<Color> teamColors)
    {
        this.teamColors = teamColors;
    }

    public void SetTimer(GameObject timer)
    {
        this.timer = timer;
    }

    public void SetRespawnTimer(GameObject respawnTimer)
    {
        this.respawnTimer = respawnTimer;
    }
}
