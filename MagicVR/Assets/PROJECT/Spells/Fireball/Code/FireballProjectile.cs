﻿using System.Collections;
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

    [Header("Check if Enemy Fireball")]
    public bool enemy;

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
        velocity = velocity.normalized * Mathf.Clamp(velocity.sqrMagnitude, 0, 10);
        rb.velocity = velocity;
        launched = true;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (!enemy) {

            GameObject hitEnemy = null;
            if (collisionInfo.gameObject.CompareTag("Enemy")) {
                hitEnemy = collisionInfo.gameObject;
                collisionInfo.gameObject.GetComponent<Health>().Damage(hitDamage);
            }

            Collider[] aoeObjects = null;
            if (Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, aoeObjects, LayerMask.GetMask("Enemy")) > 0) {
                foreach (Collider c in aoeObjects) {
                    if (c.CompareTag("Enemy")) {
                        c.gameObject.GetComponent<Health>().Damage(areaDamage);
                        c.attachedRigidbody.AddExplosionForce(5, transform.position, explosionRadius, 1, ForceMode.Impulse);
                    }
                }
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            var child = transform.GetChild(0);
            child.SetParent(null, true);
            TimedDestroy.DestroyAfterTime(child.gameObject, 2);
        }

        if (enemy) {

            if (collisionInfo.gameObject.CompareTag("Player")) {
                collisionInfo.gameObject.GetComponent<PlayerHealth>().Damage(hitDamage);
            }
        }
        
        Destroy(gameObject);
    }
}
