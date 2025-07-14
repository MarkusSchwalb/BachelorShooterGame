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
        fadeScript.FadeOutComplete += HandleFadeOut;
        fadeScript.StartFade();
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
