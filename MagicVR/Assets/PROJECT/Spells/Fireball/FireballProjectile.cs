using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public float hitDamage = 10;
    public float areaDamage = 5;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Random.onUnitSphere + new Vector3(0, 7, 0), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        transform.rotation.SetLookRotation(rb.velocity);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Enemy"))
        {
            collisionInfo.gameObject.GetComponent<Health>().Damage(hitDamage);
        }
    }
}
