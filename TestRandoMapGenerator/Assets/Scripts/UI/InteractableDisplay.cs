using TMPro;
using UnityEngine;

public class InteractableDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText(Interactable interactable)
    {
        Debug.Log("Change Text" + interactable.interactableText);
        if (displayText == null || interactable == null) return; 
        displayText.text = interactable.interactableText;
        //maybe at some point comparing weapons


    }
}
