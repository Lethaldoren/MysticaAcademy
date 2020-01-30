using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class TeleportationMod : MonoBehaviour
    {

        public float slowAmount;
        public float direction;
        public Transform directionMarker;
        public Player player;
        public SteamVR_Behaviour_Pose m_Pose; 

        private float teleporting;


        public SteamVR_Action_Vector2 actionTurning; 


        void Start()
        {

            m_Pose = player.gameObject.GetComponent<SteamVR_Behaviour_Pose>();
        }


        void Update()
        {

            if (Teleport.instance.teleportAction.GetStateDown(m_Pose.inputSource))
            {

            }

        }

        void Slowtime ()
        {

        }

        void PlaceDirectionMarker ()
        {
            directionMarker = Teleport.instance.destinationReticleTransform;
           // directionMarker.rotation =
        }

        void SetDirection()
        {

        }
    }
}
