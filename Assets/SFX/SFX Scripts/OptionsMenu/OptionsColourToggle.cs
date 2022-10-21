using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsColourToggle : MonoBehaviour
{
    [SerializeField] GameObject music;
    [SerializeField] GameObject musicred;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject uired;
    [SerializeField] GameObject effects;
    [SerializeField] GameObject effectsred;
    [SerializeField] GameObject autoaim;
    [SerializeField] GameObject autoaimred;

    // Update is called once per frame
    void Update()
    {
        float bmgvolume;
        float effectsvolume;
        float uivolume;
        float autoAimOn;

        if (PlayerPrefs.HasKey("bgmvolume"))
        {
            bmgvolume = PlayerPrefs.GetFloat("bgmvolume");
            if (bmgvolume == 1.0f){ //music is ON
                musicred.SetActive(false); //show red button
                music.SetActive(true);

            }
            else{//music is OFF
                musicred.SetActive(true); //show red button
                music.SetActive(false);
            }
        }
        else{
            Debug.Log("The key bgmvolume doesnt exist");
        }

        if (PlayerPrefs.HasKey("buttonvolume"))
        {
            uivolume = PlayerPrefs.GetFloat("buttonvolume");
            if (uivolume == 1.0f){ //ui squish sounds is ON
                uired.SetActive(false); //show red button
                ui.SetActive(true);

            }
            else{//music is OFF
                uired.SetActive(true); //show red button
                ui.SetActive(false);
            }
        }
        else{
            Debug.Log("The key buttonvolume doesnt exist");
        }

        if (PlayerPrefs.HasKey("effectsvolume"))
        {
            effectsvolume = PlayerPrefs.GetFloat("effectsvolume");
            if (effectsvolume == 1.0f){ //effects is ON
                effectsred.SetActive(false); //show red button
                effects.SetActive(true);

            }
            else{//music is OFF
                effectsred.SetActive(true); //show red button
                effects.SetActive(false);
            }
        }
        else{
            Debug.Log("The key effectsvolume doesnt exist");
        }

        if (PlayerPrefs.HasKey("autoaim"))
        {
            autoAimOn = PlayerPrefs.GetFloat("autoaim");
            if (autoAimOn == 1.0f){ //autoaim is ON
                autoaimred.SetActive(false); //show red button
                autoaim.SetActive(true);

            }
            else{//music is OFF
                autoaimred.SetActive(true); //show red button
                autoaim.SetActive(false);
            }
        }
        else{
            Debug.Log("The key autoaim doesnt exist");
        }
        
    }
}
