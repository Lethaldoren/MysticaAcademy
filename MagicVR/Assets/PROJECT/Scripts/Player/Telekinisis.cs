using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem; 

public class Telekinisis : MonoBehaviour
{
    Player player; 
    Hand castingHand;
    public SteamVR_Action_Boolean castAction;

    public Vector3 castExtents;
    public float maxCastDistance;
    public float castRadius;

    public LayerMask collisionMask;

    public float MinVelocity;
    public float VelocityToThrow; 

    bool searchingForObject;
    bool controllingObject;
    Rigidbody controlledRB;
    Vector3 controllerOrigin;

    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput(); 

        if (searchingForObject)
        SearchForObject();


    }

    private void FixedUpdate()
    {
        if (controllingObject)
        {
            GetControllerGesture();
        }
    }

    void GetControllerGesture ()
    {

        Vector3 velocity;
        Vector3 angularVelocity;

        castingHand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);


        Vector3 TargetDirection =  castingHand.transform.position - controllerOrigin; 

        if (velocity.magnitude > VelocityToThrow)
        {
            ThrowObject(TargetDirection, velocity.magnitude);
        }
        else if (velocity.magnitude > MinVelocity)
        {
            MoveObject(TargetDirection);
        } else
        {
            controlledRB.velocity = Vector3.zero;
        }
    }

    void MoveObject(Vector3 direction)
    {
       
        controlledRB.velocity = direction;
        particles.transform.position = controlledRB.position;
    }

    void ThrowObject(Vector3 direction, float speed)
    {
        Vector3 velocity;
        Vector3 angularVelocity;

        castingHand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
        controlledRB.velocity = (velocity.normalized * velocity.sqrMagnitude * .5f);

        ReleaseObject();
    }

    void CheckInput()
    {
        foreach(Hand hand in player.hands)
        {
            if (!controllingObject && !searchingForObject && castAction.GetStateDown(hand.handType))
            {
                castingHand = hand;
                searchingForObject = true;
            }

            if (controllingObject && castAction.GetStateUp(hand.handType))
                ReleaseObject();

            if (searchingForObject && castAction.GetStateUp(hand.handType))
                searchingForObject = false;
        }
    }

    

    void SearchForObject()
    {
        Debug.Log("Searching For Object");
        Transform handTransform = castingHand.gameObject.transform;

        RaycastHit[] hits = Physics.CapsuleCastAll(handTransform.position, handTransform.position + ((Quaternion.AngleAxis(50, handTransform.right) * handTransform.forward )* maxCastDistance), castRadius, handTransform.forward, Mathf.Infinity, collisionMask);
        if (hits.Length > 0)
        {
            Debug.Log(hits[0].collider.gameObject);
            controlledRB = hits[0].collider.attachedRigidbody;
            GrabObject();
        }
    }

    void GrabObject ()
    {
        searchingForObject = false;
        controllingObject = true;
        particles.SetActive(true);
        controlledRB.useGravity = false;
        controllerOrigin = castingHand.transform.position;
    }


    void ReleaseObject ()
    {
        particles.SetActive(false);
        controllingObject = false;
        controlledRB.useGravity = true; 
    }

}
