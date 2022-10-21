using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    AudioSource buttonSound;
    // Start is called before the first frame update
    void Start()
    {
        float buttonVolume;
        if (PlayerPrefs.HasKey("buttonvolume"))
        {
            Debug.Log("The key buttonvolume exists");
            buttonVolume = PlayerPrefs.GetFloat("buttonvolume");
        }
        else
        {
            Debug.Log("The key buttonvolume doesnt exist");
            PlayerPrefs.SetFloat("buttonvolume", 1.0f);
            buttonVolume = 1.0f;
        }

        buttonSound = GetComponent<AudioSource>();
        buttonSound.volume = buttonVolume;
        Debug.Log("Playing audio source " + buttonSound);

    } 

    //to toggle volume
    public void ToggleUIVolume(){
        buttonSound = GetComponent<AudioSource>();
        float buttonVolume;
        if (PlayerPrefs.HasKey("buttonvolume"))
        {
            Debug.Log("The key buttonvolume exists");
            buttonVolume = PlayerPrefs.GetFloat("buttonvolume");

            if(buttonVolume == 1){
                PlayerPrefs.SetFloat("buttonvolume", 0.0f);
                buttonSound.volume = 0.0f;
            }
            else{
                PlayerPrefs.SetFloat("buttonvolume", 1.0f);
                buttonSound.volume = 1.0f;
            }
        }
    }
}
