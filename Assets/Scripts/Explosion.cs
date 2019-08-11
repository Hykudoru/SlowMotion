using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float maxBlastForceMagnitude = 25f;
    [SerializeField] LayerMask layers;
    SphereCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, collider.radius, layers);
            Explode(colliders);
        }
    }

    void Explode(Collider[] colliders)
    {
        foreach (Collider col in colliders)
        {
            Vector3 targetDisplacement = col.transform.position - transform.position;
            Vector3 direction = targetDisplacement.normalized;
            float distance = targetDisplacement.magnitude;

            Vector3 explosiveForce = direction * (maxBlastForceMagnitude * (1.0f/distance));

            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity += explosiveForce;
            }
        }
    }
}