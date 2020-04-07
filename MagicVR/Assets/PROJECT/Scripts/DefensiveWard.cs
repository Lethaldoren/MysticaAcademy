using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Hand))]
    public class DefensiveWard : MonoBehaviour
    {

        public float objectMoveSpeed;
        public float objectHeldRange;
        public float targetGrabRange;

        public float forceMultplier;
        bool holdingObject;
        public Rigidbody heldRB;

        Hand hand;

        [Header("Hit Box")]
        public float distanceFromHand;
        public float hitBoxRadius;

        public LayerMask mask; 

        public SteamVR_Action_Boolean actionGrab; 

        void Start ()
        {
            hand = GetComponent<Hand>();
        }


        // Update is called once per frame
        void Update()
        {
            //if there is no attached object get the controller input
            if (hand.currentAttachedObject == null)
            {
                bool b_grab = actionGrab.GetState(hand.handType);

                if (b_grab && !holdingObject)
                    SearchForObject();

                if (holdingObject && !b_grab)
                    ThrowObject();
            }
        }

        private void FixedUpdate()
        {
            if (holdingObject && actionGrab.GetState(hand.handType))
                DragObject();
            else if (holdingObject)
                ThrowObject();
        }

        void SearchForObject ()
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position + (transform.forward * distanceFromHand), hitBoxRadius, mask);

            Debug.Log("Searching For Object");

            foreach (Collider collider in hitColliders)
            {
                Debug.Log(collider);

                if (collider.tag == "projectile")
                {
                    OnObjectPickUp(collider.attachedRigidbody);
                }
            }

        }

        void OnObjectPickUp(Rigidbody newObject)
        {
            holdingObject = true;
            
            heldRB = newObject;
            heldRB.useGravity = false;
        }

        void DragObject()
        {
            
            Vector3 TargetPosition = transform.position + (transform.forward * objectHeldRange);
            Vector3 TargetDirection = TargetPosition - heldRB.position;
            TargetDirection.Normalize();
            heldRB.velocity = TargetDirection * objectMoveSpeed;

        }

        void ThrowObject()
        {
            Vector3 velocity;
            Vector3 angularVelocity;

            hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
            heldRB.velocity = (velocity.normalized * velocity.sqrMagnitude * .5f );
            heldRB.useGravity = true;
            holdingObject = false;
            heldRB = null;

        }

        void StopAttack()
        {
            //set the state of the enemy to stunned

            //turn on the IK
            
        }

        void DragEnemy()
        {
            //move the position of the target IK object to the hand
        }

        void ThrowEnemy()
        {
            //add force at the hand location
        }

    }
}

