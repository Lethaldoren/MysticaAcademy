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

        public Hand hand; 

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
            float targetRotation = Vector2.Angle(Vector2.up, actionTurning.GetAxis(m_Pose.inputSource));
            directionMarker.rotation = Quaternion.AngleAxis(targetRotation, Vector3.up);

        }

        void SetDirection()
        {

        }
    }
}
