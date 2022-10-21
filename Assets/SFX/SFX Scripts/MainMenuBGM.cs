using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBGM : MonoBehaviour
{
    AudioSource mainMenuBGM;
    // Start is called before the first frame update
    void Start()
    {
        float bmgvolume;
        if (PlayerPrefs.HasKey("bgmvolume"))
        {
            Debug.Log("The key bgmvolume exists");
            bmgvolume = PlayerPrefs.GetFloat("bgmvolume");
        }
        else
        {
            Debug.Log("The key bgmvolume doesnt exist");
            PlayerPrefs.SetFloat("bgmvolume", 1.0f);
            bmgvolume = 1.0f;
        }
        Debug.Log("Playing audio source " + mainMenuBGM);

        mainMenuBGM = GetComponent<AudioSource>();
        mainMenuBGM.volume = bmgvolume;
        mainMenuBGM.Play(); //startplaying the BGM on a loop

    }

    public void MuteBGM(){
        mainMenuBGM.volume = 0.0f;
    }

    void Update(){
        float musicVolume;
        if (PlayerPrefs.HasKey("bgmvolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("bgmvolume");
            mainMenuBGM.volume = musicVolume;
        }
    }
}
