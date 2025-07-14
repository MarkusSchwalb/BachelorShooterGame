using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadGameUiScript : MonoBehaviour
{
    public TMP_InputField EigabeFeld;

    public void LoadGame()
    {

        GameData.SetSeed(EigabeFeld.text);

        Debug.Log("Current Seed " + GameData.Seed + " Eingabe war " + GameData.Eingabe);

        SceneManager.LoadScene(1); // muss nachher auf 1 geändert werden

        GameData.CurrentLevel = 1;
        GameData.ResetGameData();
    }

    public void QuickPlay()
    {
        DateTime now = DateTime.Now;


        string dateTime = now.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");

        GameData.Eingabe = dateTime;
        GameData.SetSeed(dateTime);

        Debug.Log("Current Seed " + GameData.Seed + " Eingabe war " + GameData.Eingabe);

        SceneManager.LoadScene(1); // muss nachher auf 1 geändert werden

        GameData.CurrentLevel = 1;
    }

    public void DailyChallenge()
    {

        DateTime now = DateTime.Now;
        string date = now.ToString("yyyy-MM-dd");

        GameData.Eingabe = date;
        Debug.Log("Current Seed " + GameData.Seed + " Eingabe war " + GameData.Eingabe);

        GameData.SetSeed(date);

        SceneManager.LoadScene(1); // muss nachher auf 1 geändert werden

        GameData.CurrentLevel = 1;
    }
}
