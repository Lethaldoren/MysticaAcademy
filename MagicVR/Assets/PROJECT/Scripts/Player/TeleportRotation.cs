using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    
    public class TeleportRotation : MonoBehaviour
    {
        Transform teleportPoint;
        public SteamVR_Action_Vector2 inputDirection;

        public Vector3 targetDirection;
        private bool teleporting;
        

        Teleport teleport;
        Player player;
        // Start is called before the first frame update
        void Start()
        {
            player = Player.instance;
            teleport = Teleport.instance;
            teleportPoint = teleport.destinationReticleTransform;
        }

        // Update is called once per frame
        void Update()
        {
            if (teleport.visible)
            {
                UpdateRotation();
            }

            if (teleport.teleporting & !teleporting)
            {
                teleporting = true;
                Invoke("RotatePlayer", teleport.teleportFadeTime);
            }
        }

        void UpdateRotation ()
        {
            float inputRotation = Vector2.SignedAngle(inputDirection.GetAxis(teleport.pointerHand.handType), Vector2.up);
            float targetRotation = player.hmdTransform.eulerAngles.y + inputRotation;

            

            teleportPoint.rotation = Quaternion.AngleAxis(targetRotation, Vector3.up);
           // targetDirection =  Quaternion.AngleAxis(inputRotation, Vector3.up) * player.hmdTransform.transform.forward;
        }

        void RotatePlayer()
        {
            teleporting = false;
            float offsetRotation = Vector3.SignedAngle(player.hmdTransform.forward, player.transform.forward, Vector3.up);
            Quaternion targetRotation = teleportPoint.rotation * Quaternion.AngleAxis(offsetRotation, Vector3.up);
            float angle = Vector3.SignedAngle(player.transform.forward, targetRotation * Vector3.forward, Vector3.up);
          

            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position -= playerFeetOffset;
            player.transform.Rotate(Vector3.up, angle);
            playerFeetOffset = Quaternion.Euler(0.0f, angle, 0.0f) * playerFeetOffset;
            player.trackingOriginTransform.position += playerFeetOffset;

        }

    }
}