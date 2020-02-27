using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour
{
    // public variables
    public bool charging;
    public float chargeTime;
    public GameObject projectilePrefab;
    public VisualEffect chargeFX;
    public Spell spell;

    // private variables
    float charge;
    GameObject projectile;

    public UnityEvent OnStartCharge;
    public UnityEvent OnCharging;
    public UnityEvent OnCompleteCharge;

    void Awake()
    {
        chargeFX = GetComponentInChildren<VisualEffect>();
    }
    
    public void StartCharge()
    {
        charge = 0;
        charging = true;
        // projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        chargeFX.Play();
    }

    public void Charge()
    {
        if (charging)
        {
            Debug.Log("I am charging");
            charge += Time.deltaTime;
            var prog = charge / chargeTime;
            // projectile.transform.localScale = new Vector3(prog, prog, prog);
        
            if (charge >= chargeTime)
            {
                Debug.Log("Complete!");
                charge = chargeTime;
                charging = false;
                // projectile.transform.localScale = Vector3.one;

                chargeFX.Stop();
            }

            
        }
        else
        {
            Debug.Log("Done charging");
        }
    }

    public void StopCharge()
    {
        if (charging == true)
        {
            // Incomplete charge
            charging = false;

            chargeFX.Stop();
        }
        else
        {
            // Completed charge
            var proj = Instantiate(projectilePrefab, spell.wand.m_WandTip.position + (Vector3.forward * .25f), Quaternion.identity);
            proj.GetComponent<FireballProjectile>().Launch(spell.wand.velocity);
        }
        charge = 0;
    }
}
