using JetBrains.Annotations;
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

    private float walkRange = 3;

    public SimpleRandoPatrol(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    float counter = 0;

    public override void EnterState()
    {
        float randomZ = Random.RandomRange(-walkRange, walkRange);
        float randomX = Random.RandomRange(-walkRange, walkRange);

        Vector3 destination = new Vector3(stateMashine.transform.position.x + randomX, stateMashine.transform.position.y, stateMashine.transform.position.z + randomZ);

        stateMashine.Agent.SetDestination(destination);
        counter = 0;
    }

    public override void ExitState()
    {
        
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    public override void UpdateState(float DeltaTime)
    {
        float currentSpeed = stateMashine.Agent.velocity.magnitude;
        if (stateMashine.Animator != null)
            stateMashine.Animator.SetFloat(ForwardSpeedHash, currentSpeed, animationDampTime, DeltaTime);

        counter += DeltaTime;

        if (stateMashine.Eyes.CheckIsInView(stateMashine.Player))
        {
            Debug.Log("PlayerInView");
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

        if (!stateMashine.Agent.pathPending && stateMashine.Agent.remainingDistance <= 1 || counter > 5)
        {
            stateMashine.SwitchState(new SimpleEnemyIdleState(stateMashine));
        }
    }
}
