using System;
using UnityEngine;
using UnityEngine.AI;
using static VolFx.OldMoviePass;

public abstract class BaseEnemyStateMashine : BaseStateMashine
{
    public event Action GotPlayerEvent;
    [field: SerializeField] public Eyes Eyes { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public HealthComponent HComp { get; private set; }

    public Player player;
    public bool HasPlayer = false;

    public CombatManager CombatM { get; private set; }

    bool hasRequesteAttack = false;

    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float WalkingSpeed { get; private set; }
    [field: SerializeField] public float ChasingSpeed { get; private set; }
    public float AttackRange = 1;

    [field: SerializeField] public float Damage { get; private set; } = 10;
    public NavMeshAgent Agent;

    [field: SerializeField] protected GameObject AudioObj;
    [field: SerializeField] protected AudioClip hurtSound;
    [field: SerializeField] protected AudioClip deathSound;

    [field: SerializeField] protected GameObject effectObject;
    [field: SerializeField] protected GameObject DeathParticle;


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
            player = CombatM.player;
        }
        CheckComponents();
        SwitchToStandardState();

        if (Agent != null)
        {
            
            //Agent.updateRotation = false; //maybe later
        }

        if (HComp != null)
        {
            HComp.DamageAction += HandleDamage;
        }
    }

    protected virtual void HandleDamage(float value)
    {
        SpeakText(hurtSound);
    }

    protected virtual void CheckComponents()
    {
        if (Eyes == null) { Eyes = GetComponent<Eyes>(); }
        if (Animator == null) { Animator = GetComponent<Animator>(); }
        if (player == null) { player = FindFirstObjectByType<Player>(); }
        if (Agent == null) { 
            Agent = GetComponent<NavMeshAgent>();
            Agent.enabled = true;
        }
        if (Agent != null) { SnapToGround(); }
        if (Controller == null) { Controller = GetComponent<CharacterController>(); }
    }

    protected void SnapToGround()
    {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(transform.position, out navHit, 5f, NavMesh.AllAreas))
        {
            Agent.Warp(navHit.position);
            Agent.enabled = true;
        }
        else
        {
            RaycastHit hit;
            Vector3 origin = transform.position + Vector3.up * 2;

            if (Physics.Raycast(origin, Vector3.down, out hit, 10f))
            {
                transform.position = hit.point;
            }
        }
    }

    public virtual void RequestAttack()
    {
        bool b =  CombatM == null || hasRequesteAttack;
        Debug.Log("RequestAttack " + b);
        if (CombatM == null || hasRequesteAttack) return;
        CombatM.WantsToAttack(this);
        hasRequesteAttack = true;
    }
    public abstract void SwitchToStandardState();

    public abstract void AttackGranted();

    public virtual void AttackDenied()
    {
        hasRequesteAttack = false;
        SwitchToStandardState() ;
    }

    public virtual void EndAttack()
    {
        if (CombatM == null) return;
        hasRequesteAttack = false;
        CombatM.DeleteAttacker(this);
    }

    private void OnDestroy()
    {
        SpeakText(deathSound);
        SpawnPartikle(DeathParticle);
        if (CombatM == null) return;
        CombatM.DeleteAttacker(this);
        GameData.KillCount++;
    }

    public void GotPlayer()
    {
        if (!HasPlayer)
        {
            HasPlayer = true;
            GotPlayerEvent?.Invoke();
        }
        
    }

    protected void SpeakText(AudioClip clip)
    {
        if (AudioObj == null) return;
        GameObject go = Instantiate(AudioObj, transform.position, transform.rotation);
        AudioObject audioObject = go.GetComponent<AudioObject>();
        if (audioObject != null)
        {
            audioObject.SoundClip = clip;
            audioObject.playSound();
        }
    }

    protected void SpawnPartikle(GameObject particle)
    {
        if (particle == null || effectObject == null) return;
        Vector3 pos = transform.position;
        pos.y += 1;
        GameObject eO = Instantiate(effectObject, transform.position, transform.rotation);
        EffectObject eObj = eO.GetComponent<EffectObject>();
        if (eO != null)
        {
            eObj.ParticleObj = particle;
            eObj.PlayEffect();
        }
    }

}

public enum EnemyType
{
    Melee, Boss, Distance, Tank, NotDefined
}
