using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void GameSetupNotify();

public class GameManager : MonoBehaviour
{
    private float gameTime;

    [SerializeField]
    private GameObject map;

    private GameObject currentMap;
    private Object[] powerUps;

    private List<Color> teamColors;

    public event GameSetupNotify GameSetup;

    // Start is called before the first frame update
    void Start()
    {
        // Create map
        currentMap = Instantiate(map, new Vector3(0, 0, 0), new Quaternion());

        teamColors = new List<Color>();

        teamColors.Add(Color.red);
        teamColors.Add(Color.green);
        teamColors.Add(Color.blue);
        
        // Get team colors
        //for (int i = 0; i < 4; i++)
        //{
        //    teamColors.Add(new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));
        //}

        currentMap.transform.Find("SpawnManager").GetComponent<SpawnScript>().SetTeamColors(teamColors);

        // Load Powerups
        powerUps = Resources.LoadAll("PowerUps", typeof(GameObject));
        currentMap.transform.Find("PowerUpSpawner").GetComponent<PowerUpSpawnerScript>().SetPowerUps(powerUps);

        gameTime = 180f;

        OnGameSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameTime > 0)
        {
            gameTime -= Time.deltaTime;
        }
        else
        {
            GameOver();
        }
    }

    // What happens when the game ends after the timer hits 0
    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    protected virtual void OnGameSetup()
    {
        GameSetup?.Invoke();
    }
}
