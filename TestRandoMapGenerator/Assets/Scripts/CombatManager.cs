using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [field: SerializeField] private int MaxAttackers = 5;
    [field: SerializeField] private int MaxMeleeAtck = 3;
    public static CombatManager Instance;

    public List<BaseEnemyStateMashine> Attackers = new List<BaseEnemyStateMashine>();
    public List<BaseEnemyStateMashine> WaitingList = new List<BaseEnemyStateMashine>();
    public Player player {  get; private set; }
    #region Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple Gamemanagers");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
        }

    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("UpdateLists", 1, 1);

        
    }

    // Update is called once per frame
    void Update()
    {
        
            
    }

    private void UpdateLists()
    {
        Debug.Log("TickUpdateList");
        Attackers.RemoveAll(e => e == null);
        WaitingList.RemoveAll(e => e == null);

        if (Attackers.Count < MaxAttackers)
        {
            CheckForAttackers();
        }
    }

    private void CheckForAttackers()
    {
        Debug.Log("Check for Attackers");   
        if (Attackers == null || WaitingList == null || WaitingList.Count == 0) return;

        Attackers.RemoveAll(e => e == null);
        WaitingList.RemoveAll(e => e == null);

        int openSeats = MaxAttackers - Attackers.Count;
        Debug.Log("Openseats: " + openSeats);
        if (openSeats >= WaitingList.Count)
        {
            WaitingList.RemoveAll(e => e == null);
            
            List<BaseEnemyStateMashine> list = new List<BaseEnemyStateMashine>();
            
            foreach (BaseEnemyStateMashine besm in WaitingList)
            {
                if (besm == null) continue;
                list.Add(besm);
            }

            foreach (BaseEnemyStateMashine besm in list)
            {
                AddAttacker(besm);
            }
            return;
        }

        if (openSeats < WaitingList.Count)
        {
            for (int i = 1; i < openSeats; i++)
            {
                Debug.Log("Ich bin eine dumme for schleife in CheckforAttackers" + i);
                BaseEnemyStateMashine nextAttacker = GetNextAttacker();
                if (nextAttacker == null) continue;
                AddAttacker(nextAttacker);
                
            }
            return;
        }
        if (WaitingList.Count > 0)
        {
            AddAttacker(WaitingList[0]);
        }
        
    }

    private void AddAttacker(BaseEnemyStateMashine besm)
    {
        Debug.Log("AddAttackers");
        Attackers.Add(besm);
        WaitingList.Remove(besm);
        besm.AttackGranted();
    }
    private BaseEnemyStateMashine GetNextAttacker()
    {   //test
        Debug.Log("getNextAttacker");
        //prio erst Bosse dann melee bis zum gewissen Punkt und dann auffüllen mit den Fernkampf
        if (WaitingList.Any(e => e.EType == EnemyType.Boss))
            return WaitingList.Find(e => e != null && e.EType == EnemyType.Boss);

        int MeleeCount = Attackers.Count(e => e.EType == EnemyType.Melee);
        Debug.Log("Melee Count" + MeleeCount + "Has place for melee" + (MeleeCount <= MaxMeleeAtck));
        if (MeleeCount <= MaxMeleeAtck)
        {
            return GetClosestAtckOfType(EnemyType.Melee);
        }

        int DistanceAtck = Attackers.Count(e => e.EType == EnemyType.Distance);

        if (DistanceAtck != 0)
        { //a
            return GetClosestAtckOfType(EnemyType.Distance);
        }

        return WaitingList[0];
    }

    private BaseEnemyStateMashine GetClosestAtckOfType(EnemyType enemyType)
    {
        Debug.Log("GetClosestEnenmyOfType " + enemyType.ToString()) ;
        List<BaseEnemyStateMashine> list = WaitingList.Where(e => e != null && e.EType == enemyType).ToList();
        if (list.Count == 0) { return WaitingList[0]; }
        Debug.Log("GetClosestEnenmyOfType List: " + string.Join(" ,", list));
        BaseEnemyStateMashine closest = list[0]; 
        float distance = Mathf.Infinity;

        foreach (BaseEnemyStateMashine baseEnemyState in list)
        {
            float dist = (player.transform.position - baseEnemyState.transform.position).sqrMagnitude;
            if (distance > dist)
            {
                closest = baseEnemyState;
                distance = dist;
                
            }
        }
        Debug.Log("GetClosestEnenmyOfType chosen was " + closest.gameObject.name);
        return closest;
    }

    

    public void WantsToAttack(BaseEnemyStateMashine requester)
    {
        if (requester == null) return;
        WaitingList.Add(requester);
    }

    public void finishedAttack(BaseEnemyStateMashine finishedAttacker)
    {
        if (Attackers.Contains(finishedAttacker))
        {
            Attackers.Remove(finishedAttacker);
        }
    }

    internal void DeleteAttacker(BaseEnemyStateMashine baseEnemyStateMashine)
    {
        if (Attackers.Contains(baseEnemyStateMashine))
        {
            Attackers.Remove(baseEnemyStateMashine);
        }
        if (WaitingList.Contains(baseEnemyStateMashine))
        {
            WaitingList.Remove(baseEnemyStateMashine);
        }
    }
}
