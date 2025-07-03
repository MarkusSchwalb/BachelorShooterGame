using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyIdleState : BaseSimpleEnemyState
{
    private readonly int StandardBlendTree = Animator.StringToHash("Standard");
    private readonly int ForwardSpeedHash = Animator.StringToHash("ForwardSpeed");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    float counter = 0;

    public SimpleEnemyIdleState(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    public override void EnterState()
    {
        counter = 0;
        Debug.Log("EnterIdleStateSimpleEnemy");
        if (stateMashine.Animator!=null)
        stateMashine.Animator?.CrossFadeInFixedTime(StandardBlendTree, crossFadeDuration); 
    }

    public override void ExitState()
    {
        Debug.Log("ExitIdleState");
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    

    public override void UpdateState(float DeltaTime)
    {
        float currentSpeed = stateMashine.Agent.velocity.magnitude;
        if (stateMashine.Animator != null)
            stateMashine.Animator?.SetFloat(ForwardSpeedHash, currentSpeed, animationDampTime, DeltaTime);

        //Debug.Log("UpdateIdleState");
        if (stateMashine.Eyes.CheckIsInView(stateMashine.Player))
        {
            //Debug.Log("PlayerInView");
            //
            float distance = (stateMashine.Player.transform.position - stateMashine.transform.position).sqrMagnitude;
            if (distance < 3)
            {
                stateMashine.SwitchState(new SimpleEnemyAttackState(stateMashine));
                return;
            }

            //switch state to chaseState
            stateMashine.SwitchState(new SimpleEnemyChaseState(stateMashine));
        }

        if (counter > 5) CheckRandomPatrol();

        counter += DeltaTime;
    }

    private void CheckRandomPatrol()
    {
        int randomInt = UnityEngine.Random.Range(0, 100);

        if (randomInt > 70)
        {
            stateMashine.SwitchState(new SimpleRandoPatrol(stateMashine));
        }
    }
}
