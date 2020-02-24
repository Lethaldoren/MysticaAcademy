using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Fireball : MonoBehaviour
{
    public VisualEffect chargeFX;
    public float chargeLevel;
    public float chargeTime;
    
    public void StartCharge()
    {
        chargeLevel = 0;
        // chargeFX.Play();
    }

    public void Charge()
    {
        chargeLevel += Time.deltaTime;
        
        if (chargeLevel >= chargeTime)
        {
            // chargeFX.Stop();
            Debug.Log("complete");
        }
        Debug.Log("fuicklsadjkoipsafjdlkiop");
    }

    public void StopCharge()
    {
        chargeLevel = 0;
        // chargeFX.Stop();
    }
}
