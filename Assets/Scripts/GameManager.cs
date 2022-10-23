using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void GameSetupNotify();
public delegate void GameOverNotify(out string winner);

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private GameObject mapPanel;

    private bool gameStart;
    public float gameTime;
    public GameObject timer;

    public GameObject map;
    public string mapName;
    public GameObject currentMap;
    public Object[] powerUps;

    public List<Color> teamColors;

    private string winner;

    public event GameSetupNotify GameSetup;
    public event GameOverNotify GameOver;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;
        gameTime = 180f;

        if (playButton != null)
        {
            playButton.onClick.AddListener(() =>
            {
                this.SetupGame();
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

            float minutes = Mathf.Floor(gameTime/60);
            float seconds = Mathf.Floor(gameTime % 60);

            timer.transform.Find("Time").GetComponent<TextMeshProUGUI>().SetText(string.Format("{0:00}", minutes) + ":" + string.Format("{0:00}", seconds));
        }
        else
        {
            // What happens when the game ends after the timer hits 0
            OnGameOver();

            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);

            gameStart = false;
        }
    }

    public void SetupGame()
    {
        map = mapPanel.GetComponent<MapSelector>().GetSelectedMap();
        mapName = mapPanel.GetComponent<MapSelector>().mapName;

        if (map == null)
        {
            Debug.Log("Select map.");
            return;
        }

        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    private void OnGameOver()
    {
        GameOver?.Invoke(out winner);
    }

    public string GetWinner()
    {
        return winner;
    }

    public virtual void OnGameSetup()
    {
        gameStart = true;
        GameSetup?.Invoke();
    }
}
