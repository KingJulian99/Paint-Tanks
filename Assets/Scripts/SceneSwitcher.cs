using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // --- Game Maps
    //map switcherinator
    public void LoadSmallMed(){
        SceneManager.LoadScene("SmallMed");
    }
    public void LoadLargeMed(){
        SceneManager.LoadScene("LargeMed");
    }
    public void LoadSmallJapan(){
        SceneManager.LoadScene("SmallJapan");
    }
    public void LoadLargeJapan(){
        SceneManager.LoadScene("LargeJapan");
    }

    // --- Menus
    //to go back to main menu
    public void LoadMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    //to go to the options menu
    public void LoadOptionsMenu(){
        SceneManager.LoadScene("OptionsMenu");
    }
    //to go to the tips section
    public void LoadTipsMenu(){
        SceneManager.LoadScene("TipsMenu");
    }
    //to go to the powerups menu
    public void LoadPowerupsMenu(){
        SceneManager.LoadScene("Powerups");
    }
    //to go back to main menu
    public void LoadSettings(){
        SceneManager.LoadScene("GameSettings");
    }
}
