using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Unity.Netcode;

namespace NetworkUI
{
    public class NetworkManagerUI : MonoBehaviour
    {
        public Button serverBtn;
        public Button hostBtn;
        public Button clientBtn;


        //private void Awake() {
        //    serverBtn.onClick.AddListener(() => {
        //        NetworkManager.Singleton.StartServer();
        //    });
        //    hostBtn.onClick.AddListener(() => {
        //        print("host clicked");
        //        NetworkManager.Singleton.StartHost();
        //    });
        //    clientBtn.onClick.AddListener(() => {
        //        print("client clicked");
        //        NetworkManager.Singleton.StartClient();
        //    });
        //}

    }
}
