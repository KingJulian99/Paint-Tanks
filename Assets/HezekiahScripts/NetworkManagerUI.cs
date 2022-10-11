using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField]
    private NetworkManager netMan;

    [SerializeField]
    private Button hostButton, joinButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            NetworkManager.SceneManager.LoadScene("Hezekiah's Test Scene", LoadSceneMode.Single);
            
        });

        joinButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
