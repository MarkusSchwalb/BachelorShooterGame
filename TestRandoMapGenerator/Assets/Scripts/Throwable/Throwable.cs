using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    [field: SerializeField] protected bool actOnImpact;
    [field: SerializeField] protected bool actOnTime;
    [field: SerializeField] protected float time;
    [field: SerializeField] protected float throwForce = 10f;

    protected Rigidbody rb;

    float HitDamage;

    public abstract void Act();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.TryGetComponent<HealthComponent>(out HealthComponent component))
        {
            component.TakeDamage(HitDamage);
        }

        if (actOnImpact) Act();
        
    }

    private void Start()
    {
        if (actOnTime)
            Invoke("Act", time);
        rb = GetComponent<Rigidbody>();
        if (rb == null) { return; }
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }
}
