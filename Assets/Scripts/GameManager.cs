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

            float minutes = Mathf.Floor(gameTime/60);
            float seconds = Mathf.Floor(gameTime % 60);

            timer.transform.Find("Time").GetComponent<TextMeshProUGUI>().SetText(string.Format("{0:00}", minutes) + ":" + string.Format("{0:00}", seconds));
        }
        else
        {
            GameOver();
        }
    }

    public void SetupGame(float gameTime)
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

    // What happens when the game ends after the timer hits 0
    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    public virtual void OnGameSetup()
    {
        gameStart = true;
        GameSetup?.Invoke();
    }
}
