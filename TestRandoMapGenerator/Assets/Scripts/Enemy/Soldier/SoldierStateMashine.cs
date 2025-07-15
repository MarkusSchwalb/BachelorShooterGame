using UnityEngine;


public class SoldierStateMashine : BaseEnemyStateMashine
{
    public float minDistanceToPlayer = 2;
    public GameObject Projectile;
    public Transform ProjectileSpawner;
    public GameObject Target;
    public override void AttackGranted()
    {
        SwitchState(new SoldierAttackstate(this));
    }

    public override void SwitchToStandardState()
    {
        SwitchState(new SoldierIdleState(this));
    }

    protected override void HandleDamage(float value)
    {
        GotPlayer();
        SwitchState(new SoldierStaggerState(this));
    }

    public void Shoot()
    {
        if (Projectile == null)
        {
            Debug.Log("TriedToShoot");
            return;
        }
        Vector3 toPlayer = Player.transform.position - ProjectileSpawner.position;
        Quaternion rotation = Quaternion.LookRotation(toPlayer.normalized);

        GameObject proctl = Instantiate(Projectile, ProjectileSpawner.position, rotation);

        if (proctl.TryGetComponent<ProjectileBeahvior>(out ProjectileBeahvior pB))
        {

        }
    }

    public void UpdateTargetPos()
    {
        if (Player == null) return;
        Vector3 newTargetPos = Player.transform.position;
        newTargetPos.y += 1;

        Target.transform.position = newTargetPos;

    }
}
