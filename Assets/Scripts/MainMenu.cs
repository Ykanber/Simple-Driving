using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text energyText;
    [SerializeField] Button playButton;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;

    private int energy;

    private const string energyKey = "Energy";
    private const string energyReadyKey = "EnergyReady";


    void Start()
    {
        OnApplicationFocus(true);
    }

    void OnApplicationFocus( bool focus)
    {
        if (!focus) { return; }

        CancelInvoke();


        int highScore  = PlayerPrefs.GetInt(ScoreScript.HighScoreKey, 0);
        highScoreText.text = $"High Score: {highScore}";

        energy = PlayerPrefs.GetInt(energyKey, maxEnergy);

        if(energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(energyReadyKey, string.Empty);

            if(energyReadyString == string.Empty) { return; }

            DateTime energyReady = DateTime.Parse(energyReadyString);

            if(DateTime.Now > energyReady)
            {
                playButton.interactable = true;
                energy = maxEnergy;
                PlayerPrefs.SetInt(energyKey, maxEnergy); 
            }
            else
            {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        
        }

        energyText.text = $"Play ({energy})";
    }

    private void EnergyRecharged() {
        energy = maxEnergy;
        PlayerPrefs.SetInt(energyKey, energy);
        energyText.text = $"Play ({energy})";
        playButton.interactable = true;
    }


    public void PlayGame()
    {
        if(energy < 1)
        {
            return;
        }

        energy--;

        PlayerPrefs.SetInt(energyKey, energy);
        if(energy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(energyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(energyReady);
#endif
        }
        SceneManager.LoadScene(1);
    }
}
