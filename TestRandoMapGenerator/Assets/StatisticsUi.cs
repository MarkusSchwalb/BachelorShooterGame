using System;
using TMPro;
using UnityEngine;

public class StatisticsUi : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI ReachedLevel;
    [field: SerializeField] private TextMeshProUGUI TimeDisplay;
    [field: SerializeField] private TextMeshProUGUI Score;
    [field: SerializeField] private TextMeshProUGUI KillCount;
    [field: SerializeField] private TextMeshProUGUI AimModifier;
    [field: SerializeField] private TextMeshProUGUI HealthModifier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ReachedLevel != null) ReachedLevel.text = "Reached Level: " + GameData.CurrentLevel; 
        if (TimeDisplay != null)
        {
            string duration = GameData.GetDuration(DateTime.Now);
            TimeDisplay.text = "Survived for: " + duration;
        }
        if (Score != null) Score.text = "Score: " + GameData.Score;
        if (KillCount != null) KillCount.text = "Kills: " + GameData.KillCount;
        if (AimModifier != null) AimModifier.text = "Aim Modifier: " + GameData.AimModifier;
        if (HealthModifier != null) HealthModifier.text = "Health Modifier: " + GameData.HealthModifier;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     CurrentLevel = 1;
        KillCount = 0;
        Score = 0;
        StartTime = DateTime.Now;
        AimModifier = 1;
        HealthModifier = 1;
     
     */
}
