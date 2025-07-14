using UnityEngine;

public class TestEnemyAttack : EnemyBaseState<BaseEnemyStateMashine>
{
    float count = 0;

    public TestEnemyAttack(BaseEnemyStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Is in Attacking");
        stateMashine.transform.position += new Vector3(0, 2, 0);
    }

    public override void ExitState()
    {
        Debug.Log("Leaves Attacking");
        stateMashine.transform.position += new Vector3(0, -2, 0);
        stateMashine.EndAttack();
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    public override void UpdateState(float DeltaTime)
    {
        
        count = count + DeltaTime;
        if (count > 1) stateMashine.SwitchState(new TestEnemyIdle(stateMashine));
    }
}
