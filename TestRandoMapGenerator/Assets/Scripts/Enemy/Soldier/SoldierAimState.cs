using System.Runtime.CompilerServices;
using UnityEngine;

public class SoldierAimState : BaseSoldierState
{
    private float previousFrameTime;
    private readonly int IdleHash = Animator.StringToHash("Standard");

    private readonly int Forward = Animator.StringToHash("Forward");
    private readonly int Sideward = Animator.StringToHash("Sideward");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    public SoldierAimState(SoldierStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("EnterSoldierAim");
        stateMashine.Agent.updateRotation = false; // ab hier mach ich das

        stateMashine.Animator.SetLayerWeight(1, 1); //aim Layer 
    }

    public override void ExitState()
    {
        Debug.Log("ExitSoldierAim");
    }

    public override void OnAnimatorMoveState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(float DeltaTime)
    {
        FaceToPlayer(DeltaTime);
        if (stateMashine.Eyes.CheckIsInView(stateMashine.Player))
        {
            stateMashine.RequestAttack();
        }
        else
        {
            stateMashine.EndAttack();
        }
        Vector3 currentMovement = stateMashine.transform.InverseTransformDirection(stateMashine.Agent.velocity);

        float currentForward = currentMovement.z;
        float currentSideward = currentMovement.x;
        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.SetFloat(Forward, currentForward, animationDampTime, DeltaTime);
            stateMashine.Animator?.SetFloat(Sideward, currentSideward, animationDampTime, DeltaTime);
        }

        float distanceToPlayerSqrMgn = (stateMashine.Player.transform.position - stateMashine.transform.position).sqrMagnitude;
        if (distanceToPlayerSqrMgn < stateMashine.minDistanceToPlayer * stateMashine.minDistanceToPlayer 
            || distanceToPlayerSqrMgn > stateMashine.AttackRange * stateMashine.AttackRange)
        {
            float offset = (stateMashine.AttackRange + stateMashine.minDistanceToPlayer) / 2;
            stateMashine.Agent.SetDestination(
            OffsetTarget(
                stateMashine.transform.position,
                stateMashine.Player.transform.position,
                offset
                ));
        }

        stateMashine.UpdateTargetPos();

        FacePlayer();
    }
}
