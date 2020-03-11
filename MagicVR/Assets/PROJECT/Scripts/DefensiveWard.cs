using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class DefensiveWard : MonoBehaviour
    {

        public float objectMoveSpeed;
        public float targetGrabRange;

        public float forceMultplier;

        bool holdingObject;
        Rigidbody heldObject;

        Hand hand;

    void SearchForObject ()
        {

        }

    void OnObjectPickUp(Rigidbody newObject)
        {
            holdingObject = true;
            heldObject = newObject;

        }


        void DragObject()
        {
           
            Vector3 TargetPosition = hand.transform.right;
        }
        void ThrowObject()
        {

        }

        void StopAttack()
        {

        }

        void DragEnemy()
        {

        }

        void ThrowEnemy()
        {

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}

