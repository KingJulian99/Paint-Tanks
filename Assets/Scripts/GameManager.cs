using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void GameSetupNotify();

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private GameObject mapPanel;

    private bool gameStart;
    private float gameTime;

    private GameObject map;
    private GameObject currentMap;
    private Object[] powerUps;

    private List<Color> teamColors;

    public event GameSetupNotify GameSetup;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;

        if (playButton != null)
        {
            playButton.onClick.AddListener(() =>
            {
                this.SetupGame(180f);
            });
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart) { return; }

        if(gameTime > 0)
        {
            gameTime -= Time.deltaTime;
        }
        else
        {
            GameOver();
        }
    }

    private void test()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void SetupGame(float gameTime)
    {
        map = mapPanel.GetComponent<MapSelector>().GetSelectedMap();

        if(map == null)
        {
            Debug.Log("Select map.");
            return;
        }

        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);

        Debug.Log("Map: " + map);

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

        this.gameTime = gameTime;

        Debug.Log("Here");

        OnGameSetup();
    }

    // What happens when the game ends after the timer hits 0
    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    protected virtual void OnGameSetup()
    {
        gameStart = true;
        GameSetup?.Invoke();
    }
}
