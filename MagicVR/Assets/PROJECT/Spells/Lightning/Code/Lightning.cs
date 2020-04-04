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
    [Tooltip("Rate of damage, measured in times per second")]
    public float rateOfDamage;
    /// <summary>
    /// a timer that holds how long until the next time damage should be delt
    /// </summary>
    private float fireTimer;
    /// <summary>
    /// The maximum range the spell can reach
    /// </summary>
    public float maxRange;
    /// <summary>
    /// The radius of the spherecast
    /// </summary>
    public float triggerRadius;
    /// <summary>
    /// A mask of the layers to ignore in the spherecast
    /// </summary>
    public LayerMask layerMask;
    

    public UnityEvent OnCharging;
    public UnityEvent OnChargeStopped;
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
            if (charging)
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

        if (fireTimer < 0)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, triggerRadius, transform.forward, out hit, maxRange, layerMask))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    Debug.Log("damage enemy");
                    hit.transform.GetComponent<Health>().Damage(damage);
                }
                transform.GetChild(0).position = hit.point;
            }
            else
            {
                transform.GetChild(0).position = transform.position + transform.forward * maxRange;
            }

            fireTimer = 1 / rateOfDamage;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
    }

    public void ResetCharge()
    {
        charge = 0;

        if (chargePercent == 1)
        {
            OnFiringStop.Invoke();
            fireTimer = 1 / rateOfDamage;
        }    
        else 
        {
            OnChargeStopped.Invoke();
        }

        chargePercent = 0;
        charging = false;
        
    }
    
}
