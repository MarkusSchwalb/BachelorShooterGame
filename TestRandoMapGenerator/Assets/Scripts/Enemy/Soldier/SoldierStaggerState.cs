
using UnityEngine;


public class SoldierStaggerState : BaseSoldierState
{
    float cooldown = 2;
    float counter = 0;

    private float previousFrameTime;
    private readonly int HitReaction = Animator.StringToHash("Soldier Reaction Hit");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    public SoldierStaggerState(SoldierStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter SoldierStaggerState");
        //play animation
        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.CrossFadeInFixedTime(HitReaction, crossFadeDuration);
            stateMashine.Animator.SetLayerWeight(1, 0); //aim Layer 
        }
            

        counter = 0;
        stateMashine.Agent.updateRotation = true;
    }

    public override void ExitState()
    {
        Debug.Log("Exit SoldierStaggerState");
    }

    public override void OnAnimatorMoveState()
    {
        Debug.Log(" SoldierStaggerState");
    }

    public override void UpdateState(float DeltaTime)
    {
        //Debug.Log("Update SoldierStaggerState");
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
