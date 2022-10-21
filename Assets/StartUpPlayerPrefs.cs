using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpPlayerPrefs : MonoBehaviour
{
    //initalize all userprefs first time load. FOR MAIN MENU ONLY
    void Start(){

        //default bgm volume
        if (!PlayerPrefs.HasKey("bgmvolume"))
        {
            PlayerPrefs.SetFloat("bgmvolume", 1.0f);
        }

        //default ui
        if (!PlayerPrefs.HasKey("buttonvolume"))
        {
            PlayerPrefs.SetFloat("buttonvolume", 1.0f);
        }

        //default effects
        if (!PlayerPrefs.HasKey("effectsvolume"))
        {
            PlayerPrefs.SetFloat("effectsvolume", 1.0f);
        }

        //default autoaim
        if (!PlayerPrefs.HasKey("autoaim"))
        {
            PlayerPrefs.SetFloat("autoaim", 0.0f); //autoaim default off
        }
    }
}
