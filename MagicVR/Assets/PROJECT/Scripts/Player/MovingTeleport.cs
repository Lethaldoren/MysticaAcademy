using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;


    public class MovingTeleport : MonoBehaviour
    {
        public Player player;
        public LayerMask CollisionMask;
        public GameObject TeleportMarker;

        public SteamVR_Action_Boolean teleportAction;
        public SteamVR_Action_Boolean acceptTeleportAction; 

        Hand pointerHand; 


        public float MarkerMoveSpeed;
        public float FadeTime; 
        Vector3 TargetPosition;
        Vector3 MarkerPosition;

        bool markerActive;
        bool teleporting; 

        // Start is called before the first frame update
        void Start()
        {
            //player = InteractionSystem.Player.instance;
        }

        // Update is called once per frame
        void Update()
        {
            CheckInput();

            if (markerActive)
                MoveMarker(); 

        }

        void CheckInput()
        {
            foreach (Hand hand in player.hands)
            {
                if (teleportAction.GetStateDown(hand.handType) && !markerActive)
                {
                    pointerHand = hand;
                    ActivateTeleportMarker();
                }

                if (markerActive && hand == pointerHand)
                {
                    if (acceptTeleportAction.GetStateDown(hand.handType))
                    {
                        BeginPlayerTeleport();
                    }

                    if (teleportAction.GetStateUp(hand.handType))
                    {
                        DeactivateTeleportMarker(); 
                    }                    
                }
            }
        }

        void BeginPlayerTeleport()
        {
            teleporting = false;
            SteamVR_Fade.Start(Color.clear, 0);
            SteamVR_Fade.Start(Color.black, FadeTime);

            Invoke("TeleportPlayer", FadeTime);
        }

        void TeleportPlayer ()
        {
            teleporting = false;

            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position = TeleportMarker.transform.position + playerFeetOffset;

            DeactivateTeleportMarker(); 
        }

        void ActivateTeleportMarker ()
        {
            TeleportMarker.transform.position = player.feetPositionGuess;
            markerActive = true;
            TeleportMarker.SetActive(true);
        }

        void DeactivateTeleportMarker()
        {
            markerActive = false;
            TeleportMarker.SetActive(false);
        }

        void GetTargetPosition()
        {
            Transform handTransform = pointerHand.transform;
            RaycastHit hit;

            Ray ray = new Ray(handTransform.position, handTransform.forward);

            if (Physics.Raycast (ray, out hit,Mathf.Infinity, CollisionMask))
            {
                Debug.DrawRay(handTransform.position, handTransform.forward, Color.red);
                TargetPosition = hit.point; 
            }
        }

        void MoveMarker()
        {
            GetTargetPosition();
            Vector3 TargetDirection = (TargetPosition - TeleportMarker.transform.position).normalized;
            TeleportMarker.transform.Translate(TargetDirection * (MarkerMoveSpeed * Time.deltaTime)); 
        }
    }
