using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectClient : NetworkBehaviour
{

    [SerializeField]
    private NetworkManager netMan;

    [SerializeField]
    private Button hostBtn, joinBtn;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            netMan.StartHost();
        });

        joinBtn.onClick.AddListener(() =>
        {
            netMan.StartClient();
        });
    }
}
