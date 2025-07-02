using UnityEngine;

public class ThrowAimAt : MonoBehaviour
{
    [field: SerializeField]
    private Transform AimAt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 aimAtVector = AimAt.position - transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(aimAtVector);
    }
}
