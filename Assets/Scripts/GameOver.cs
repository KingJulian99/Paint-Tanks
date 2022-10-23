using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject background;

    [SerializeField]
    private List<Texture> winners;

    private void Awake()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        string winner = gm.GetWinner();

        if (winner == "red")
        {
            background.GetComponent<RawImage>().texture = winners[0];
        }
        else if (winner == "green")
        {
            background.GetComponent<RawImage>().texture = winners[1];
        }
        else
        {
            background.GetComponent<RawImage>().texture = winners[2];
        }
    }
    
}
