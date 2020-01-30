using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;


public class WindSlash : MonoBehaviour, EquipableSpell
{
    //gets hand input and position of hand
    public SteamVR_Input_Sources Hand;
    public SteamVR_Action_Pose position;

    private Vector3 velocity;

    public new void OnEquip()
    {
        Debug.Log("I am Wind Slash");
    }

    public void OnTriggerDown()
    {

    }

    public void OnTriggerHeld()
    {
        //gets velocity of hand
        velocity = position.GetVelocity(Hand);

        //checks if hand is moving
        if (velocity.x > Vector3.zero.x)
        {
            Debug.Log("you're moving the controller");
            Debug.Log(velocity);
        }
        //check for velocity and if controller is moving fast
        //check if it moved 20cm 
        //get angle and direction of slash (compare head location to slash location)
        //fire slash in that direction
    }

    public void OnTriggerUp()
    {

    }
}