using UnityEngine;

public class SoldierIdleState : BaseSoldierState
{
    private float previousFrameTime;
    private readonly int IdleHash = Animator.StringToHash("Standard");

    private readonly int Forward = Animator.StringToHash("Forward");
    private readonly int Sideward = Animator.StringToHash("Sideward");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    public SoldierIdleState(SoldierStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        //Debug.Log("SoldierIdleState has statemashine" + !(stateMashine == null));
        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.CrossFadeInFixedTime(IdleHash, crossFadeDuration);
            stateMashine.Animator.SetLayerWeight(1, 0); //aim Layer 
        }
        stateMashine.Agent.updateRotation = true;
    }

    public override void ExitState()
    {
        Debug.Log("SoldierIdleStateExitState");
    }

    public override void OnAnimatorMoveState()
    {

    }

    public override void UpdateState(float DeltaTime)
    {
        //Debug.Log("SoldierIdleStateUpdateState");
        
        Vector3 currentMovement = stateMashine.transform.InverseTransformDirection(stateMashine.Agent.velocity);

        float currentForward = currentMovement.z;
        float currentSideward = currentMovement.x;

        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.SetFloat(Forward, currentForward, animationDampTime, DeltaTime);
            stateMashine.Animator?.SetFloat(Sideward, currentSideward, animationDampTime, DeltaTime);
        }

        if (stateMashine.Eyes.CheckIsInView(stateMashine.player) || stateMashine.HasPlayer)
        {
            stateMashine.GotPlayer();
            //switch state to chaseState
            stateMashine.SwitchState(new SoldierGetCloserState(stateMashine));
        }
    }


}
