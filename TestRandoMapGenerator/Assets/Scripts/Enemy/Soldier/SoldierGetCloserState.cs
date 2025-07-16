using System.Threading;
using UnityEngine;

public class SoldierGetCloserState : BaseSoldierState
{
    private float previousFrameTime;
    private readonly int IdleHash = Animator.StringToHash("Standard");

    private readonly int Forward = Animator.StringToHash("Forward");
    private readonly int Sideward = Animator.StringToHash("Sideward");

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;

    float timer = 0;

    public SoldierGetCloserState(SoldierStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        stateMashine.Agent.updateRotation = true;
        Debug.Log("SoldierGetCloserStateEnterState");
        if (stateMashine.Animator != null)
            stateMashine.Animator?.CrossFadeInFixedTime(IdleHash, crossFadeDuration);
        float offset = (stateMashine.AttackRange + stateMashine.minDistanceToPlayer) / 2;
        stateMashine.Agent.speed = stateMashine.ChasingSpeed;
        stateMashine.Agent.SetDestination(
            OffsetTarget(
                stateMashine.transform.position,
                stateMashine.player.transform.position,
                offset));

        stateMashine.Agent.updateRotation = false; // ab hier mach ich das

    }

    public override void ExitState()
    {
        Debug.Log("SoldierGetCloserState ExitState");
    }

    public override void OnAnimatorMoveState()
    {
        
    }

    public override void UpdateState(float DeltaTime)
    {
        timer += DeltaTime;
        float distanceToPlayerSqrMgn = (stateMashine.player.transform.position - stateMashine.transform.position).sqrMagnitude;
        if (
            distanceToPlayerSqrMgn < (stateMashine.minDistanceToPlayer * stateMashine.minDistanceToPlayer) 
            || distanceToPlayerSqrMgn > (stateMashine.AttackRange * stateMashine.AttackRange)
            )
        {

            float offset = (stateMashine.AttackRange + stateMashine.minDistanceToPlayer) / 2;
            stateMashine.Agent.SetDestination(
            OffsetTarget(
                stateMashine.transform.position,
                stateMashine.player.transform.position,
                offset
                ));
        }

        bool inRange = distanceToPlayerSqrMgn < stateMashine.AttackRange * stateMashine.AttackRange;
        bool noToClose = (distanceToPlayerSqrMgn > stateMashine.minDistanceToPlayer * stateMashine.minDistanceToPlayer || timer > 3);
        bool inView = stateMashine.Eyes.CheckIsInView(stateMashine.player);



        if (distanceToPlayerSqrMgn < stateMashine.AttackRange * stateMashine.AttackRange
            && (distanceToPlayerSqrMgn > stateMashine.minDistanceToPlayer * stateMashine.minDistanceToPlayer || timer > 3)
            && stateMashine.Eyes.CheckIsInView(stateMashine.player))
        {
            stateMashine.SwitchState(new SoldierAimState(stateMashine));
        }

        /*
        if (distanceToPlayerSqrMgn < stateMashine.AttackRange 
            &&  (distanceToPlayerSqrMgn < stateMashine.minDistanceToPlayer * stateMashine.minDistanceToPlayer || timer > 3)
            && stateMashine.Eyes.CheckIsInView(stateMashine.Player))
        {
            stateMashine.RequestAttack();
        }

        if (!stateMashine.Eyes.CheckIsInView(stateMashine.Player))
        {
            stateMashine.EndAttack();
        }*/

        Vector3 currentMovement = stateMashine.transform.InverseTransformDirection(stateMashine.Agent.velocity);

        float currentForward = currentMovement.z;
        float currentSideward = currentMovement.x;

        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.SetFloat(Forward, currentForward, animationDampTime, DeltaTime);
            stateMashine.Animator?.SetFloat(Sideward, currentSideward, animationDampTime, DeltaTime);
        }

        FaceToPlayer(DeltaTime);
    }
}
