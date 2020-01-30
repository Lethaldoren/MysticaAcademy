using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlash : MonoBehaviour, EquipableSpell
{
    public new void onEquip()
    {
        Debug.Log("I am Wind Slash");
    }

    public void onTriggerDown()
    {
        //track position on controller position
    }

    public void OnTriggerHeld()
    {
        //check for velocity and if controller is moving fast
        //check if it moved 20cm 
        //get angle and direction of slash (compare head location to slash location)
        //fire slash in that direction
    }

    public void OnTriggerUp()
    {

    }
}
