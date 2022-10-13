using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public delegate void SpawnNotify();

public class SpawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ai, player, playerContainer;

    [SerializeField]
    private GameObject[] spawn_points;

    private List<Color> teamColors;

    public event SpawnNotify SpawnDone;

    // Start is called before the first frame update
    private void Awake()
    {
        GameObject gm = GameObject.Find("GameManager");
        GameManager gameManager = gm.GetComponent<GameManager>();
        gameManager.GameSetup += SpawnPlayers;

        OnSpawnDone();
    }

    private void SpawnPlayers()
    {
        // Spawn Player at first spawn point
        GameObject p = Instantiate(player, spawn_points[0].transform.position, spawn_points[0].transform.rotation);

        // Put player in container
        p.transform.SetParent(playerContainer.transform);

        // Set players team color
        p.transform.GetComponent<TankController>().SetTeamColor(teamColors[0]);

        Debug.Log(teamColors.Count);
        // Spawn AI at subsequent spawn points
        for (int i = 1; i < spawn_points.Length; i++)
        {
            p = Instantiate(ai, spawn_points[i].transform.position, spawn_points[i].transform.rotation);
            p.transform.SetParent(playerContainer.transform);
            p.transform.GetComponent<TankController>().SetTeamColor(teamColors[i]);
        }
    }

    protected virtual void OnSpawnDone()
    {
        SpawnDone?.Invoke();
    }

    public void SetTeamColors(List<Color> teamColors)
    {
        this.teamColors = teamColors;
    }
}
