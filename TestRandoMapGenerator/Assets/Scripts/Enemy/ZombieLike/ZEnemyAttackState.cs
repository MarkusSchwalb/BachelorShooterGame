using UnityEngine;

public class ZEnemyAttackState : ZEnemyBaseState
{
    private readonly int Attack1 = Animator.StringToHash("Zombie Attack");
    private readonly int Attack2 = Animator.StringToHash("Zombie Attack2");

    private HitHirBoxHandeler boxHandeler;

    float cooldown = 2;
    float counter = 0;

    private float previousFrameTime;

    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.2f;
    public ZEnemyAttackState(ZEnemyStateMashine sM) : base(sM)
    {
    }

    public override void EnterState()
    {
        Debug.Log("ZEnemyAttackStateEnterState");

        Vector3 lookPostion = stateMashine.Player.transform.position;
        lookPostion.y = stateMashine.transform.position.y;
        stateMashine.transform.LookAt(lookPostion);

        int randomnmbr = UnityEngine.Random.Range(0,2);


        if (stateMashine.Animator != null)
        {
            if (randomnmbr == 0)
            {
                stateMashine.Animator?.CrossFadeInFixedTime(Attack1, crossFadeDuration);
            }
            else
            {
                stateMashine.Animator?.CrossFadeInFixedTime(Attack2, crossFadeDuration);
            }
        }
        boxHandeler = stateMashine.gameObject.GetComponent<HitHirBoxHandeler>();
        boxHandeler?.EnableBoxes();
        /*
        if (stateMashine.Player == null) { stateMashine.SwitchToStandardState(); return; }
        if (stateMashine.Player.TryGetComponent<Player>(out Player player))
        {
            float distance = (player.transform.position - stateMashine.transform.position).sqrMagnitude;
            if (distance < stateMashine.AttackRange)
                player.HealthComponent.TakeDamage(stateMashine.Damage);
        }*/
    }

    public override void ExitState()
    {
        Debug.Log("ZEnemyAttackStateExitState");
        //raus aus dem Attackers im combat
        stateMashine.EndAttack();
        boxHandeler?.DisableBoxes();
    }

    public override void OnAnimatorMoveState()
    {

    }

    public override void UpdateState(float DeltaTime)
    {
        if (stateMashine.Animator != null) //change after animation
        {
            float normalizedTime = GetNormalizedTime(stateMashine.Animator, "Attack");

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
