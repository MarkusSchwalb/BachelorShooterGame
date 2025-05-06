using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyChaseState : BaseSimpleEnemyState
{
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
            return;
        }
        stateMashine.Agent.speed = stateMashine.ChasingSpeed;
        stateMashine.Agent.destination = stateMashine.Player.transform.position;

        //stateMashine.Animator.CrossFadeInFixedTime(StandardBlendTree, crossFadeDuration);     Grad noch kein Animator
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
