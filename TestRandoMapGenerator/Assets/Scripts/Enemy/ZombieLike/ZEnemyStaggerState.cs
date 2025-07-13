using UnityEngine;

public class ZEnemyStaggerState : EnemyBaseState
{
    float cooldown = 2;
    float counter = 0;

    private float previousFrameTime;
    private readonly int HitReaction = Animator.StringToHash("Zombie Reaction Hit");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    public ZEnemyStaggerState(BaseEnemyStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter ZEnemyStaggerState");
        //play animation
        if (stateMashine.Animator != null)
            stateMashine.Animator?.CrossFadeInFixedTime(HitReaction, crossFadeDuration);

        counter = 0;

    }

    public override void ExitState()
    {
        Debug.Log("Exit ZEnemyStaggerState");
    }

    public override void OnAnimatorMoveState()
    {
        Debug.Log("Enter ZEnemyStaggerState");
    }

    public override void UpdateState(float DeltaTime)
    {
        //Debug.Log("Update ZEnemyStaggerState");
        if (stateMashine.Animator != null) //change after animation
        {
            float normalizedTime = GetNormalizedTime(stateMashine.Animator, "HitReaction");

            if (normalizedTime < 1)
            {

            }
            else
            {
                stateMashine.SwitchToStandardState();
                //Debug.Log("1");
            }

            previousFrameTime = normalizedTime;
        }
        else //change after time
        {
            counter += DeltaTime;
            if (counter > cooldown)
            {
                stateMashine.SwitchToStandardState();
            }
        }
    }
}
