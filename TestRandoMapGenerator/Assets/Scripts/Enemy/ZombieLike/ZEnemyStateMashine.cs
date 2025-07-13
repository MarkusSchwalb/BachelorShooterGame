using UnityEngine;

public class ZEnemyStateMashine : Base2PedalEnemyStateMashine
{

    public override void AttackGranted()
    {
        SwitchState(new ZEnemyAttackState(this));
    }

    public override void SwitchToStandardState()
    {
        SwitchState(new ZEnemyIdleState(this));
    }

    protected override void HandleDamage(float value)
    {
        SwitchState(new ZEnemyStaggerState(this));
    }
}
