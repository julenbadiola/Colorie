using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RDG;

public class VibrationManager : MonoBehaviour
{
    public const string DefaultVibration = "on";
    public string vibration;
    
    void Start()
    {
        if(PlayerPrefs.HasKey("Vibration")){
            vibration = PlayerPrefs.GetString("Vibration");
        }else{
            vibration = DefaultVibration;
        }
        PlayerPrefs.SetString("Vibration", vibration);
    }

    public void change(){
        if(vibration == "on"){
            PlayerPrefs.SetString("Vibration", "off");
        }else{
            PlayerPrefs.SetString("Vibration", "on");
            longVibration();
        }  
        vibration = PlayerPrefs.GetString("Vibration");
    }

    public void shortVibration(){
        if(PlayerPrefs.GetString("Vibration")=="on"){
            print("short vibrating");
            Vibration.Vibrate (200);
        }
    }

    public void longVibration(){
        if(PlayerPrefs.GetString("Vibration")=="on"){
            print("long vibrating");
            Vibration.Vibrate (400);
        }
        
    }

    public void vibrateSeconds(int sec){
        if(PlayerPrefs.GetString("Vibration")=="on"){
            Vibration.Vibrate (sec * 1000);
        }
        
    }

    public void vibrateMilliseconds(int milli){
        if(PlayerPrefs.GetString("Vibration")=="on"){
            Vibration.Vibrate (milli);
        }
        
    }
}
