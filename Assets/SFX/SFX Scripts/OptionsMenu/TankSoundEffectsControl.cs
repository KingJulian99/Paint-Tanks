using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSoundEffectsControl : MonoBehaviour
{
    public void ToggleEffectsVolume(){
        float effectsVolume;
        if (PlayerPrefs.HasKey("effectsvolume"))
        {
            Debug.Log("The key effectsvolume exists");
            effectsVolume = PlayerPrefs.GetFloat("effectsvolume");

            if(effectsVolume == 1){ 
                //mute music
                PlayerPrefs.SetFloat("effectsvolume", 0.0f);
            }
            else{ 
                //unmute music
                PlayerPrefs.SetFloat("effectsvolume", 1.0f);
            }
        }
    }
}
