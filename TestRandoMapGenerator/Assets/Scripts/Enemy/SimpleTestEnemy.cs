using UnityEngine;

public class SimpleTestEnemy : BaseEnemyStateMashine
{
    public override void AttackGranted()
    {
        SwitchState(new TestEnemyAttack(this));
    }

    protected override void EnemyStart()
    {
        base.EnemyStart();
        SwitchState(new TestEnemyIdle(this));
        Debug.Log(Player.gameObject.name);
    }
}
