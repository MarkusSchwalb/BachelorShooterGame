using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData 
{
    public static int Seed;
    public static string Eingabe;
    public static int CurrentLevel;

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

    


}
