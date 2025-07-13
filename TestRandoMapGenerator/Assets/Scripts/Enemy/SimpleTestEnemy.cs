using UnityEngine;

public class SimpleTestEnemy : BaseEnemyStateMashine
{
    public override void AttackGranted()
    {
        SwitchState(new TestEnemyAttack(this));
    }

    public override void SwitchToStandardState()
    {
        SwitchState(new TestEnemyIdle(this));
    }

    protected override void EnemyStart()
    {
        base.EnemyStart();
        
    }

    protected override void HandleDamage(float value)
    {
        
    }
}
