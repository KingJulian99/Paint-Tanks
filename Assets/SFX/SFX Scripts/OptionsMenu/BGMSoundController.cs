using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSoundController : MonoBehaviour
{
    public void ToggleBGMVolume(){
        float buttonVolume;
        if (PlayerPrefs.HasKey("bgmvolume"))
        {
            Debug.Log("The key bgmvolume exists");
            buttonVolume = PlayerPrefs.GetFloat("bgmvolume");

            if(buttonVolume == 1){ 
                //mute music
                PlayerPrefs.SetFloat("bgmvolume", 0.0f);
            }
            else{ 
                //unmute music
                PlayerPrefs.SetFloat("bgmvolume", 1.0f);
            }
        }
    }
}
