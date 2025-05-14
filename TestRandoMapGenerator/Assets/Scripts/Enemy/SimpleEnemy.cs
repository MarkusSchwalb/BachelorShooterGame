using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : BaseStateMashine
{
    [field: SerializeField] public Eyes Eyes { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    public NavMeshAgent Agent {  get; private set; }

    public GameObject Player;

    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float WalkingSpeed { get; private set; }
    [field: SerializeField] public float ChasingSpeed { get; private set; }

    [field: SerializeField] public float Damage { get; private set; } = 10;
    private void Start()
    {
        CheckComponents();
        GoBackToStandardState();
    }

    private void CheckComponents()
    {
        if (Eyes == null) { Eyes = GetComponent<Eyes>(); }
        if (Animator == null) { Animator = GetComponent<Animator>(); }
        if (Agent == null) { Agent = GetComponent<NavMeshAgent>();}
        if (Player == null) { Player = GameObject.FindWithTag("Player"); }
    }

    public virtual void GoBackToStandardState()
    {
        //Debug.Log("gobacktostandardState");
        SwitchState(new SimpleRandoPatrol(this));
    }
}
