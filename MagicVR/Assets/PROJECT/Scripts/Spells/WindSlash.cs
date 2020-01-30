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

    float swingTimer;

    public new void OnEquip()
    {
        Debug.Log("I am Wind Slash");
        swingTimer = 0;
    }

    public void OnTriggerDown()
    {

    }

    public void OnTriggerHeld()
    {
        //gets velocity of hand
        velocity = position.GetVelocity(Hand);

        //checks if hand is moving ====remove after testing====
        if (velocity.x > Vector3.zero.x)
        {
            Debug.Log("you're moving the controller");
            Debug.Log(velocity);
        }

        float swingSpeed = velocity.magnitude;

        if (swingSpeed > 5)
        {
            Debug.Log("swinging");
            swingTimer += Time.deltaTime;

            //gets position of hand when it moves fast enough
            Vector3 startPos = position.localPosition;

            if (swingTimer >= 0.4f)
            {
                if (swingTimer < 5)
                {
                    Vector3 endPos = position.localPosition;
                    swingTimer = 0;
                    spawnSlash();
                }
            }
        }
        if (swingSpeed > 5)
        {
            swingTimer = 0;
        }
    }

    public void OnTriggerUp()
    {

    }

    private void spawnSlash()
    {

        /* spawn slash at point between start and end position
         * adjust angle based on those values
         * add velocity to move away from head position
         * destroy after X seconds
         */
    }
}