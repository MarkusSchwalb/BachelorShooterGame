using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRandoPatrol : BaseSimpleEnemyState
{
    private readonly int StandardBlendTree = Animator.StringToHash("Standard");
    private readonly int ForwardSpeedHash = Animator.StringToHash("ForwardSpeed");
    private readonly int SideSpeed = Animator.StringToHash("SideSpeed");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    public SimpleRandoPatrol(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    public override void UpdateState(float DeltaTime)
    {
        Debug.Log("UpdateSimpleRandoPatrolState");
        if (stateMashine.Player == null) stateMashine.Player = GameObject.Find("Player");
        if (stateMashine.Agent == null) Debug.LogWarning("NoAgentFound");


        stateMashine.Agent.SetDestination(stateMashine.Player.transform.position);

        float currentSpeed = stateMashine.Agent.velocity.magnitude;
        if (stateMashine.Animator != null)
            stateMashine.Animator.SetFloat(ForwardSpeedHash, currentSpeed, animationDampTime, DeltaTime);
    }
}
