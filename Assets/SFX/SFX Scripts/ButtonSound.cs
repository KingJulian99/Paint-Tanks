using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    AudioSource buttonSound;
    //Just in case the user starts the game for the first time.
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
    } 
    // Update is called once per frame
    public void ButtonSoundClick()
    {
        buttonSound = GetComponent<AudioSource>();
        buttonSound.Play();
        Debug.Log("Playing audio source " + buttonSound);
    }
}
