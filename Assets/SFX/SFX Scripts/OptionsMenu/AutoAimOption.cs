using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimOption : MonoBehaviour
{
    public void ToggleAutoAimOption(){
        float autoaim;
        if (PlayerPrefs.HasKey("autoaim"))
        {
            Debug.Log("The key autoaim exists");
            autoaim = PlayerPrefs.GetFloat("autoaim");

            if(autoaim == 1){ 
                //mute music
                PlayerPrefs.SetFloat("autoaim", 0.0f);
            }
            else{ 
                //unmute music
                PlayerPrefs.SetFloat("autoaim", 1.0f);
            }
        }
    }
}
