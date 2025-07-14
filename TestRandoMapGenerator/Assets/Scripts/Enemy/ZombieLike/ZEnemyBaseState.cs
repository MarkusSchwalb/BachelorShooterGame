using UnityEngine;

public abstract class ZEnemyBaseState : EnemyBaseState<ZEnemyStateMashine>
{
    protected ZEnemyBaseState(ZEnemyStateMashine sM) : base(sM)
    {
    }
}
