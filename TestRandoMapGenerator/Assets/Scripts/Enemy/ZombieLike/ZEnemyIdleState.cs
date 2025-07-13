using UnityEngine;

public class ZEnemyIdleState : EnemyBaseState
{
    private float previousFrameTime;
    private readonly int IdleHash = Animator.StringToHash("Standard");

    private readonly int Forward = Animator.StringToHash("Forward");
    private readonly int Sideward = Animator.StringToHash("Sideward");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;
    public ZEnemyIdleState(BaseEnemyStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("ZEnemyIdleStateEnterState");
        if (stateMashine.Animator != null)
            stateMashine.Animator?.CrossFadeInFixedTime(IdleHash, crossFadeDuration);
    }

    public override void ExitState()
    {
        Debug.Log("ZEnemyIdleStateExitState");
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    public override void UpdateState(float DeltaTime)
    {
        Debug.Log("ZEnemyIdleStateUpdateState");
        float currentForward = stateMashine.Agent.velocity.z;
        float currentSideward = stateMashine.Agent.velocity.x;

        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.SetFloat(Forward, currentForward, animationDampTime, DeltaTime);
            stateMashine.Animator?.SetFloat(Forward, currentSideward, animationDampTime, DeltaTime);
        }

        if (stateMashine.Eyes.CheckIsInView(stateMashine.Player) || stateMashine.HasPlayer)
        {
            Debug.Log("PlayerAttackable");
            //
            float distance = (stateMashine.Player.transform.position - stateMashine.transform.position).sqrMagnitude;
            if (distance < 3)
            {
                stateMashine.RequestAttack();
                return;
            }

            //switch state to chaseState
            stateMashine.SwitchState(new ZEnemyChasePlayer(stateMashine));

            
        }
    }
}
