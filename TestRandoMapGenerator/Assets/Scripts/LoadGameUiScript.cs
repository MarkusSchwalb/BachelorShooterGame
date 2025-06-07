using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameUiScript : MonoBehaviour
{
    public TMP_InputField EigabeFeld;

    public void LoadGame()
    {

        GameData.SetSeed(EigabeFeld.text);

        Debug.Log("Current Seed " + GameData.Seed + " Eingabe war " + GameData.Eingabe);

        SceneManager.LoadScene(0); // muss nachher auf 1 geändert werden
    }
}
