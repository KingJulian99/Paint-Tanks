using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Global Sound Player
//Use by calling static functions anywhere. object instance not need as its carried over from main menu to all other scenes.

// TankSounds.PlayShoot();


public class TankSounds : MonoBehaviour
{
    // Start is called before the first frame update
    static float effectsvolume;
    //list of all audio sources to play
    [SerializeField] AudioSource shoot;
    [SerializeField] AudioSource death;
    [SerializeField] AudioSource electric;
    [SerializeField] AudioSource electricHit;
    [SerializeField] AudioSource bounce;
    [SerializeField] AudioSource heal;
    [SerializeField] AudioSource mggun;
    [SerializeField] AudioSource gettinghit;
    
    private static AudioSource _shoot;
    private static AudioSource _death;
    private static AudioSource _electric;
    private static AudioSource _electricHit;
    private static AudioSource _bounce;
    private static AudioSource _heal;
    private static AudioSource _mggun;
    private static AudioSource _gettinghit;

    void Start()
    {//double check that there is a sound option chosen
        if (PlayerPrefs.HasKey("effectsvolume")){
            effectsvolume = PlayerPrefs.GetFloat("effectsvolume");
        }
        else{
            PlayerPrefs.SetFloat("effectsvolume", 1.0f);
            effectsvolume = 1.0f;
        }
        _shoot = shoot;
        _death = death;
        _electric = electric;
        _electricHit = electricHit;
        _bounce = bounce;
        _heal = heal;
        _mggun = mggun;
        _gettinghit = gettinghit;
    }
 
 
    public static void PlayShoot(){
        _shoot.volume = effectsvolume;
        _shoot.Play();
    }
    public static void PlayDeath(){
        _death.volume = effectsvolume;
        _death.Play();
    }
    public static void PlayElectric(){
        _electric.volume = effectsvolume;
        _electric.Play();
    }
    public static void PlayBounce(){
        _bounce.volume = effectsvolume;
        _bounce.Play();
    }
    public static void PlayHeal(){
        _heal.volume = effectsvolume;
        _heal.Play();
    }
    public static void PlayMggun(){
        _mggun.volume = effectsvolume;
        _mggun.Play();
    }
    public static void PlayGettinghit(){
        _gettinghit.volume = effectsvolume;
        _gettinghit.Play();
    }
 
    private void Awake()
    {
        if (PlayerPrefs.HasKey("effectsvolume")){
            effectsvolume = PlayerPrefs.GetFloat("effectsvolume");
        }
        else{
            PlayerPrefs.SetFloat("effectsvolume", 1.0f);
            effectsvolume = 1.0f;
        }

    }

}
