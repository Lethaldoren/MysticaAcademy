using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class WindSlash : MonoBehaviour, EquipableSpell
    {
        SteamVR_TrackedObject trackedObject;
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);

        public new void onEquip()
        {
            Debug.Log("I am Wind Slash");
        }

        public void onTriggerDown()
        {
            //track position of controller
        }

        public void OnTriggerHeld()
        {
            Vector3 vel = device.velocity;
            if (velocity > 0)
            {
                debug.log("you're moving the controller");
                debug.log(velocity);
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
}