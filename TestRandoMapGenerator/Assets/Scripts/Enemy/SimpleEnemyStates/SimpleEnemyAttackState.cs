using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyAttackState : BaseSimpleEnemyState
{
    float cooldown = 2;
    float counter = 0;

    private float previousFrameTime;
    private readonly int AttackHash = Animator.StringToHash("Attack");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;
    public SimpleEnemyAttackState(SimpleEnemy simpleEnemy) : base(simpleEnemy)
    {
    }

    public override void EnterState()
    {
        Debug.Log("EnterAttackState");
        //play animation
        if (stateMashine.Animator != null) 
        stateMashine.Animator?.CrossFadeInFixedTime(AttackHash, crossFadeDuration);

        counter = 0;

        if (stateMashine.Player == null) { stateMashine.SwitchState(new SimpleEnemyIdleState(stateMashine)); return; }
        if (stateMashine.Player.TryGetComponent<Player> (out Player player))
        {
            float distance = (player.transform.position - stateMashine.transform.position).sqrMagnitude;
            if (distance < 3)
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
        if (stateMashine.Animator != null) //change after animation
        {
            float normalizedTime = GetNormalizedTime(stateMashine.Animator, "Attack");

            if (normalizedTime < 1)
            {
                
            }
            else
            {
                stateMashine.GoBackToStandardState();
                //Debug.Log("1");
            }

            previousFrameTime = normalizedTime;
        } else //change after time
        {
            counter += DeltaTime;
            if (counter > cooldown)
            {
                stateMashine.SwitchState(new SimpleEnemyChaseState(stateMashine));
            }
        }

        
    }
}
