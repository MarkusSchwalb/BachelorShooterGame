using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class DeckungSpawner : MonoBehaviour
{
    [field: SerializeField] public Deckung[] Deckungen;
    [field: SerializeField] private int minCoverCount = 1;
    [field: SerializeField] private int maxCoverCount = 10;

    List<int> spawnedDeckung = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnDeckung(); //For Testing
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDeckung()
    {
        ResetCovers(); //reset
        if (Deckungen.Length == 0) return; //wenn keine Möglichen deckungen vorhanden sind dann nochmal kann nichts spawnen
        // mögliche Ideen 

        int length = maxCoverCount;
        if (maxCoverCount > Deckungen.Length) { length = Deckungen.Length; maxCoverCount = Deckungen.Length; }

        spawnedDeckung.Clear(); 
        //generiere wie viele möglichen deckungen
        int anzlCover = UnityEngine.Random.Range(minCoverCount, length + 1); //+ 1 weil ist die max anzahl und ist nicht im array

        for (int i = 0; i < anzlCover; i++)
        {
            int randomCoverNr = UnityEngine.Random.Range(0, Deckungen.Length); //one specific
            while (spawnedDeckung.Contains(randomCoverNr))  //make sure we dont have Multiples
            {
                randomCoverNr++;
                randomCoverNr = randomCoverNr % Deckungen.Length;
            }

            if (Deckungen[randomCoverNr] != null)
            {
                Deckungen[randomCoverNr].SpawnDeckung(); //Spawn
            }
            spawnedDeckung.Add(randomCoverNr);
        }

        /*
        int i = 0;
        foreach (Deckung deck in Deckungen) // Geh alle Deckungen durch
        {
            if (spawnedDeckung.Contains(i)) { i++; continue; } 
            int randomInt = UnityEngine.Random.Range(0, 2);
            bool spawning = randomInt > 0;

            i++;
            if (!spawning) continue;

            deck.SpawnDeckung();
            spawnedDeckung.Add(i);
        }*/
    }

    public void ResetCovers()
    {
        

        foreach (Deckung deck in Deckungen)
        {
            if (deck != null)
            {
                deck.DeletePrevious();
            } else
            {
                Debug.LogWarning(name + "Has a cover that has a null value at position" + transform.position);
            }
            
        }

        spawnedDeckung.Clear();
    }

    public void GetDeckungen()
    {
        List<Deckung> deckungen = new List<Deckung>();

        Transform parent = gameObject.transform;
        foreach (Transform child in parent)
        {
            if (child.gameObject.TryGetComponent<Deckung>(out Deckung deck))
            {
                deckungen.Add(deck);
            }
        }

        Deckungen = deckungen.ToArray();
    }
}
