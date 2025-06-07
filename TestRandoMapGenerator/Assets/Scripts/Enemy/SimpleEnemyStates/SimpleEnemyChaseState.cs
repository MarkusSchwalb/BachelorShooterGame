using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyChaseState : BaseSimpleEnemyState
{
    private readonly int StandardBlendTree = Animator.StringToHash("Standard");
    private readonly int ForwardSpeedHash = Animator.StringToHash("ForwardSpeed");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    public SimpleEnemyChaseState(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    public override void EnterState()
    {
        Debug.Log("EnterChaseState");
        //Debug.Log("EnterChaseState");
        if (stateMashine.Player == null)
        {
            //stateMashine.SwitchState(new EnemyIdleState(stateMashine)); //
            stateMashine.GoBackToStandardState();
            Debug.LogWarning("Is In ChaseState and no player found");
            return;
        }

        stateMashine.Agent.enabled = true;
        stateMashine.Agent.speed = stateMashine.ChasingSpeed;
        stateMashine.Agent.destination = stateMashine.Player.transform.position;

        if (stateMashine.Animator != null)
            stateMashine.Animator?.CrossFadeInFixedTime(StandardBlendTree, crossFadeDuration);    //?is the safety operator that checks if it is null
    }

    public override void ExitState()
    {
        Debug.Log("ExitChaseState");
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    public override void UpdateState(float DeltaTime)
    {
        Debug.Log("Update Chase State");
        
        float currentSpeed = stateMashine.Agent.velocity.magnitude;
        if (stateMashine.Animator != null)
            stateMashine.Animator?.SetFloat(ForwardSpeedHash, currentSpeed, animationDampTime, DeltaTime);

        //Debug.Log("updateChaseState");
        if (stateMashine.Eyes.CheckIsInView(stateMashine.Player))
        {
            
            stateMashine.Agent.destination = stateMashine.Player.transform.position;
            float distance = (stateMashine.Player.transform.position - stateMashine.transform.position).sqrMagnitude;
            if (stateMashine.Agent.remainingDistance <= 1 || distance < 2)
            {
                stateMashine.SwitchState(new SimpleEnemyAttackState(stateMashine));
            }

            return;
        } 

        if (!stateMashine.Agent.pathPending && stateMashine.Agent.remainingDistance <= 1 )
        {
            stateMashine.SwitchState(new SimpleEnemyIdleState(stateMashine));
        }


    }
}
