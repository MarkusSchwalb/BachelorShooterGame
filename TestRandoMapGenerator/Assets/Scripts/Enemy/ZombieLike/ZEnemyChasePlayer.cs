using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZEnemyChasePlayer : ZEnemyBaseState
{
    private float previousFrameTime;
    private readonly int IdleHash = Animator.StringToHash("Standard");

    private readonly int Forward = Animator.StringToHash("Forward");
    private readonly int Sideward = Animator.StringToHash("Sideward");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;
    public ZEnemyChasePlayer(ZEnemyStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("ZEnemyChasePlayerEnterState");
        if (stateMashine.Animator != null)
            stateMashine.Animator?.CrossFadeInFixedTime(IdleHash, crossFadeDuration);

        stateMashine.Agent.speed = stateMashine.ChasingSpeed;
        stateMashine.Agent.SetDestination(
            OffsetTarget(
                stateMashine.transform.position,
                stateMashine.Player.transform.position,
                stateMashine.AttackRange));

    }

    public override void ExitState()
    {
        Debug.Log("ZEnemyChasePlayerExitState");
    }

    public override void OnAnimatorMoveState()
    {

    }

    public override void UpdateState(float DeltaTime)
    {
        Debug.Log("ZEnemyChasePlayerUpdateState");

        float currentForward = stateMashine.Agent.velocity.magnitude;
        //float currentSideward = stateMashine.Agent.velocity.x;

        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.SetFloat(Forward, currentForward, animationDampTime, DeltaTime);
            //stateMashine.Animator?.SetFloat(Sideward, currentSideward, animationDampTime, DeltaTime);
        }

        //move
        stateMashine.Agent.SetDestination(
            OffsetTarget(
                stateMashine.transform.position, 
                stateMashine.Player.transform.position, 
                stateMashine.AttackRange));

        float distanceToPlayer = (stateMashine.transform.position - stateMashine.transform.position).sqrMagnitude;
        if (distanceToPlayer < stateMashine.AttackRange * stateMashine.AttackRange)
        {
            stateMashine.RequestAttack();
        }
    }

   
    
}
