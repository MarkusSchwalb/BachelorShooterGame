using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyIdleState : BaseSimpleEnemyState
{
    public SimpleEnemyIdleState(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    public override void EnterState()
    {
        Debug.Log("EnterIdleStateSimpleEnemy");
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
