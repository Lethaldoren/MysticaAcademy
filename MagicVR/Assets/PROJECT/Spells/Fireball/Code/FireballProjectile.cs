using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Valve.VR.InteractionSystem;

public class FireballProjectile : MonoBehaviour
{
    public float hitDamage = 10;
    public float areaDamage = 5;
    public float explosionRadius = 3;
    public GameObject explosionPrefab;
    Rigidbody rb;
    bool launched;
    VisualEffect vfx;

    [Header("DEBUG")]
    public bool debug;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        vfx = GetComponent<VisualEffect>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0)
        {
            rb.rotation.SetLookRotation(rb.velocity);
        }
    }

    public void Launch(Vector3 velocity)
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        velocity = velocity.normalized * velocity.sqrMagnitude * .5f;
        rb.velocity = velocity;
        launched = true;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        GameObject hitEnemy = null;
        if (collisionInfo.gameObject.CompareTag("Enemy"))
        {
            hitEnemy = collisionInfo.gameObject;
            collisionInfo.gameObject.GetComponent<Health>().Damage(hitDamage);
        }

        Collider[] aoeObjects = null;
        if (Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, aoeObjects) > 0)
        {
            foreach (Collider c in aoeObjects)
            {
                if(c.gameObject == hitEnemy && c.CompareTag("Enemy"))
                {
                    c.gameObject.GetComponent<Health>().Damage(areaDamage);
                    c.attachedRigidbody.AddExplosionForce(5, transform.position, explosionRadius, 1, ForceMode.Impulse);
                }
            }
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        var child = transform.GetChild(0);
        child.SetParent(null, true);
        TimedDestroy.DestroyAfterTime(child.gameObject, 2);
        Destroy(gameObject);
    }
}
