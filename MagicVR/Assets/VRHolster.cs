using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem
{
    public class VRHolster : MonoBehaviour
    {

        Vector3 PositionFromPlayer; 
        Transform PlayerCamera;
        // Start is called before the first frame update
        void Start()
        {
            PlayerCamera = InteractionSystem.Player.instance.hmdTransforms[0];
            PositionFromPlayer = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            float CameraRotation = Vector3.SignedAngle(Vector3.forward, PlayerCamera.forward, Vector3.up);

            transform.localPosition = (Quaternion.AngleAxis(CameraRotation, Vector3.up) * PositionFromPlayer);

            transform.rotation = Quaternion.AngleAxis(CameraRotation, Vector3.up);
        }


    }
}

