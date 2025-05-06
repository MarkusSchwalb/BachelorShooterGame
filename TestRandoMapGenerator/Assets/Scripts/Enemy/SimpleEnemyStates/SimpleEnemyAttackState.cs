using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyAttackState : BaseSimpleEnemyState
{
    float cooldown = 2;
    float counter = 0;
    public SimpleEnemyAttackState(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    public override void EnterState()
    {
        Debug.Log("EnterAttackState");
        counter = 0;
        if (stateMashine.Player == null) { stateMashine.SwitchState(new SimpleEnemyIdleState(stateMashine)); return; }
        if (stateMashine.Player.TryGetComponent<Player> (out Player player))
        {
            player.HealthComponent.TakeDamage(stateMashine.Damage);
            
        }
    }

    public override void ExitState()
    {
        Debug.Log("ExitAttackState");
        if (stateMashine.Player == null) {  return; }
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    public override void UpdateState(float DeltaTime)
    {
        counter += DeltaTime;
        if (counter > cooldown)
        {
            stateMashine.SwitchState(new SimpleEnemyChaseState(stateMashine));
        }
    }
}
