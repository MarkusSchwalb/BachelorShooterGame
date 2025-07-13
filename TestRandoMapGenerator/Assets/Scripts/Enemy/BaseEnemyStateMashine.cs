using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyStateMashine : BaseStateMashine
{
    [field: SerializeField] public Eyes Eyes { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public HealthComponent HComp { get; private set; }

    public GameObject Player;

    public CombatManager CombatM { get; private set; }

    bool hasRequesteAttack = false;

    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float WalkingSpeed { get; private set; }
    [field: SerializeField] public float ChasingSpeed { get; private set; }

    [field: SerializeField] public float Damage { get; private set; } = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyStart();
        
    }

    protected virtual void EnemyStart()
    {
        CombatM = CombatManager.Instance;
        if (CombatM != null)
        {
            Player = CombatM.player.gameObject;
        }
        CheckComponents();
    }
    protected virtual void CheckComponents()
    {
        if (Eyes == null) { Eyes = GetComponent<Eyes>(); }
        if (Animator == null) { Animator = GetComponent<Animator>(); }
        if (Player == null) { Player = GameObject.FindWithTag("Player"); }
    }

    public virtual void RequestAttack()
    {
        bool b =  CombatM == null || hasRequesteAttack;
        Debug.Log("RequestAttack " + b);
        if (CombatM == null || hasRequesteAttack) return;
        CombatM.WantsToAttack(this);
        hasRequesteAttack = true;
    }

    public abstract void AttackGranted();

    public virtual void EndAttack()
    {
        if (CombatM == null) return;
        hasRequesteAttack = false;
    }

    private void OnDestroy()
    {
        if (CombatM == null) return;
        CombatM.DeleteAttacker(this);
    }
}

public enum EnemyType
{
    Melee, Boss, Distance, Tank, NotDefined
}
