using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleSmoothFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 1;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.001f)
        {
            rb.MovePosition(target.position);
        }
        else
        {
            float dt = Time.fixedDeltaTime;
            float ds = speed * distance * dt;
            rb.MovePosition(Vector3.MoveTowards(transform.position, target.position, ds));
        }
    }
}
