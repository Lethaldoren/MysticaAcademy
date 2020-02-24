using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Valve.VR.InteractionSystem;

public class FireballProjectile : MonoBehaviour
{
    public float Scale
    {
        set
        {
            transform.localScale = new Vector3(value, value, value);
            vfx.SetFloat("Spawn Sphere Radius", value);
        }
    }
    public float hitDamage = 10;
    public float areaDamage = 5;
    public float explosionRadius = 3;
    public GameObject explosionPrefab;
    Rigidbody rb;
    bool launched;
    VisualEffect vfx;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vfx = GetComponent<VisualEffect>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void FixedUpdate()
    {
        if (!launched) rb.velocity = transform.GetComponentInParent<Wand>().velocity;

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     rb.AddForce((Vector3.up + Vector3.forward) * 15, ForceMode.Impulse);
        //     Launch();
        // }

        if (rb.velocity.magnitude > 0)
        {
            rb.rotation.SetLookRotation(rb.velocity);
        }
        vfx.SetVector3("Initial Velocity", rb.velocity);
    }

    public void Launch()
    {
        transform.SetParent(null, true);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.velocity = new Vector3(Mathf.Pow(rb.velocity.x, 2), Mathf.Pow(rb.velocity.y, 2), Mathf.Pow(rb.velocity.z, 2));
        launched = true;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Enemy"))
        {
            collisionInfo.gameObject.GetComponent<Health>().Damage(hitDamage);
        }

        Collider[] aoeObjects = null;
        if (Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, aoeObjects) > 0)
        {
            foreach (Collider c in aoeObjects)
            {
                if(c.CompareTag("Enemy"))
                {
                    c.gameObject.GetComponent<Health>().Damage(areaDamage);
                    c.attachedRigidbody.AddExplosionForce(5, transform.position, explosionRadius, 1, ForceMode.Impulse);
                }
            }
        }
        Instantiate(explosionPrefab);
        Destroy(gameObject);
    }
}
