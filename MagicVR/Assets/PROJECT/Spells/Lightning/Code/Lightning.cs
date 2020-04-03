using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[RequireComponent(typeof(Spell))]
public class Lightning : MonoBehaviour
{
    /// <summary>
    /// State of charging
    /// </summary>
    [Header("Charge Values")]
    private bool charging;
    /// <summary>
    /// The current charge time
    /// </summary>
    private float charge;
    /// <summary>
    /// Max charge time
    /// </summary>
    public float chargeTime;
    /// <summary>
    /// Progress from 0-1 on the charge time
    /// </summary>
    [Range(0, 1)]
    public float chargePercent;
    /// <summary>
    /// Reference to the charge effect
    /// </summary>
    public VisualEffect chargeEffect;

    /// <summary>
    /// Amount of damage to inflict
    /// </summary>
    [Header("Damage Values")]
    public float damage;
    /// <summary>
    /// Rate of damage, measured in times per second
    /// </summary>
    public float rateOfDamage;
    /// <summary>
    /// The maximum range the spell can reach
    /// </summary>
    public float maxRange;
    /// <summary>
    /// A mask of the layers to ignore in the spherecast
    /// </summary>
    public LayerMask ignoreMask;
    

    public UnityEvent OnCharging;
    public UnityEvent OnFiringStart;
    public UnityEvent OnFiring;
    public UnityEvent OnFiringStop;

    /// <summary>
    /// Executes on TriggerHeld,
    /// Checks if the spell is charged yet, then starts firing the spell
    /// </summary>
    public void CheckCharge()
    {
        // Check to see if spell hasnt been charged up yet
        if (charge < chargeTime)
        {
            // Update charging state
            charging = true;

            // Increase the charge time
            charge += Time.deltaTime;
            
            // Update the charge percent
            chargePercent = charge / chargeTime;

            OnCharging.Invoke();
        }
        // If spell is charged up, fire the spell
        else
        {
            // Check if this is the first time it's firing
            if (!charging)
            {
                // Prevent this block from executing again during this charged window
                charging = false;

                OnFiringStart.Invoke();
            }

            // Set charge percent to 1
            chargePercent = 1;
            
            // Fire lightning
            FireLightning();
        }
    }

    private void FireLightning()
    {
        OnFiring.Invoke();

        if (Time.time % 1 / rateOfDamage == 0)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 1, transform.forward, out hit, maxRange, ignoreMask))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.GetComponent<Health>().Damage(damage);
                }
                transform.GetChild(0).position = hit.point;
            }
            else
            {
                transform.GetChild(0).position = transform.forward * maxRange;
            }
        }
    }

    public void ResetCharge()
    {
        charge = 0;
        chargePercent = 0;
    }
    
}
