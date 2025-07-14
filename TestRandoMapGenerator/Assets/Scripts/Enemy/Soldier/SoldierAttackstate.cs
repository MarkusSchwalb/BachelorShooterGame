using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SoldierAttackstate : BaseSoldierState
{
    private float previousFrameTime;
    private readonly int shootHash = Animator.StringToHash("Shoot");

    private readonly int Forward = Animator.StringToHash("Forward");
    private readonly int Sideward = Animator.StringToHash("Sideward");

    float cooldown = 2;
    float counter = 0;

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;
    public SoldierAttackstate(SoldierStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter SoldierAttackstate");
        stateMashine.Animator.SetLayerWeight(1, 1); //aim Layer 
        stateMashine.Animator.SetTrigger(shootHash);
    }

    public override void ExitState()
    {
        Debug.Log("Exit SoldierAttackstate");
        stateMashine.EndAttack();
    }

    public override void OnAnimatorMoveState()
    {
    }

    public override void UpdateState(float DeltaTime)
    {
        if (stateMashine.Animator != null) //change after animation
        {
            float normalizedTime = GetNormalizedTime(stateMashine.Animator, "Attack", 1);

            if (normalizedTime < 1)
            {

            }
            else
            {
                stateMashine.SwitchState(new SoldierAimState(stateMashine));
                //Debug.Log("1");
            }

            previousFrameTime = normalizedTime;
        }
        
        counter += DeltaTime;
        if (counter > cooldown)
        {
            stateMashine.SwitchState(new SoldierAimState(stateMashine));
        }
        

        Vector3 currentMovement = stateMashine.transform.InverseTransformDirection(stateMashine.Agent.velocity);

        float currentForward = currentMovement.z;
        float currentSideward = currentMovement.x;

        if (stateMashine.Animator != null)
        {
            stateMashine.Animator?.SetFloat(Forward, currentForward, animationDampTime, DeltaTime);
            stateMashine.Animator?.SetFloat(Sideward, currentSideward, animationDampTime, DeltaTime);
        }
    }
}
