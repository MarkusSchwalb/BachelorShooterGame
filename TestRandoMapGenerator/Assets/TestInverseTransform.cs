using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInverseTransform : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 potentialLanding = target.transform.position;

        Vector3 localTargetPos = transform.InverseTransformPoint(potentialLanding);

        float forwardDistance = localTargetPos.z;

        Debug.Log("forwardDistance = " + forwardDistance);
    }
}
