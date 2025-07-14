using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData 
{
    public static int Seed;
    public static string Eingabe;
    public static int CurrentLevel;
    public static int KillCount;
    public static int Score;
    public static DateTime StartTime;

    public static float AimModifier;
    public static float HealthModifier;
    public static float GameObjectDiedFrom;

    public static void SetSeed(string sSeed)
    {
        Eingabe = sSeed;
        Seed = 0;
        char[] letters = sSeed.ToCharArray();

        foreach (char c in letters)
        {
            Seed += (int)c;
        }

    }

    public static string GetDuration(DateTime now)
    {
        TimeSpan delta = now - StartTime;

        return delta.ToString();
    }

    public static void ResetGameData()
    {
        CurrentLevel = 1;
        KillCount = 0;
        Score = 0;
        StartTime = DateTime.Now;
        AimModifier = 0;
        HealthModifier = 0;
    }

}
