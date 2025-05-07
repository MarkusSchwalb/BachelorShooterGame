using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyIdleState : BaseSimpleEnemyState
{
    private readonly int StandardBlendTree = Animator.StringToHash("Standard");
    private readonly int ForwardSpeedHash = Animator.StringToHash("GoblinSpeed");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    public SimpleEnemyIdleState(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    public override void EnterState()
    {
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

        Debug.Log("UpdateIdleState");
        if (stateMashine.Eyes.CheckIsInView(stateMashine.Player))
        {
            Debug.Log("PlayerInView");
            //


            //switch state to chaseState
            stateMashine.SwitchState(new SimpleEnemyChaseState(stateMashine));
        }
    }
}
