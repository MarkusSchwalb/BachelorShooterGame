using TMPro;
using UnityEngine;

public class DiedInRoomUiScript : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        if (textMeshProUGUI == null) return;

        textMeshProUGUI.text = "Died in Room: " + GameData.LastRoom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
