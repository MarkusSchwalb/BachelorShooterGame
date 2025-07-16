using UnityEngine;
using UnityEngine.SceneManagement;

public class Abstieg : Interactable
{
    FadeScript fadeScript;
    bool interacted = false;
    private void Start()
    {
        fadeScript = FindFirstObjectByType<FadeScript>();
        interacted = false;
    }
    public override void Interact()
    {
        if ( interacted ) { return; }
        interacted = true;

        //save weapon 
        Player player = FindFirstObjectByType<Player>();
        player.SaveGuns();

        //XPManager.Instance?.SaveXP();

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
