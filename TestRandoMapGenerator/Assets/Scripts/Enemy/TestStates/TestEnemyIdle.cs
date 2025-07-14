using System.Threading;
using UnityEngine;

public class TestEnemyIdle : EnemyBaseState<BaseEnemyStateMashine>
{
    public TestEnemyIdle(BaseEnemyStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Is in Idling");
    }

    public override void ExitState()
    {
        Debug.Log("LeavesIdling");
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    float count = 0f;
    public override void UpdateState(float DeltaTime)
    {
        count = count + DeltaTime;
        if (stateMashine.Player != null && count > 2)
        {
            Debug.Log("RequestAttack");
            stateMashine.RequestAttack();
        }
    }
}
