using UnityEngine;
using UnityEngine.SceneManagement;

public class Abstieg : Interactable
{
    FadeScript fadeScript;
    private void Start()
    {
        fadeScript = FindFirstObjectByType<FadeScript>();
        
    }
    public override void Interact()
    {
        //save weapon 
        Player player = FindFirstObjectByType<Player>();
        player.SaveGuns();

        if (GameData.CurrentLevel == 3)
        {
            SceneManager.LoadScene(3);
            return;
        }
        GameData.CurrentLevel++;

        SceneManager.LoadScene(1);
        /*
        fadeScript.FadeOutComplete += HandleFadeOut;
        fadeScript.StartFade();*/
    }

    public void HandleFadeOut()
    {
        Debug.Log("HandleFadeOut");
        if (GameData.CurrentLevel == 3)
        {
            SceneManager.LoadScene(3);
            return;
        }
        GameData.CurrentLevel++;
        
        SceneManager.LoadScene(1);
    }
}
