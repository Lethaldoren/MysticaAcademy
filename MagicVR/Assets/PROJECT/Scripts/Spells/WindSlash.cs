using UnityEngine;
using Valve.VR;

public class WindSlash : SpellBase
{
    //gets hand input and position of hand
    public SteamVR_Input_Sources Hand;
    public SteamVR_Action_Pose handPosition;
    public SteamVR_Input_Sources Head;
    public SteamVR_Action_Pose headPosition;

    public GameObject slashPrefab;
    Rigidbody srb;
    public float speed;

    private Vector3 velocity;
    private Vector3 startPos;
    private Vector3 endPos;

    float swingTimer;

    public new void OnEquip()
    {
        Debug.Log("I am Wind Slash");
        swingTimer = 0;
    }

    public override void OnTriggerDown()
    {

    }

    public override void OnTriggerHeld()
    {
        //gets velocity of hand
        velocity = handPosition.GetVelocity(Hand);
        srb = slashPrefab.GetComponent<Rigidbody>();

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
            startPos = handPosition.localPosition;

            if (swingTimer >= 0.4f)
            {
                //chacks if hand slows down to finish the slash
                if (swingTimer < 5)
                {
                    endPos = handPosition.localPosition;
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

    public override void OnTriggerUp()
    {

    }

    public override void OnUnequip()
    {
        base.OnUnequip();
    }


    //spawns slash prefab in location, rotation, and scale that was drawn then makes it move
    private void SpawnSlash()
    {
        Vector3 midPoint = (startPos + endPos) / 2;

        //spawns object in space and adjusts rotation based on points
        GameObject slash = Instantiate(slashPrefab, midPoint, Quaternion.FromToRotation(Vector3.up, endPos - startPos));

        //change scale of prefab based on 2 points
        float scaleX = Mathf.Abs(startPos.x - endPos.x);
        float scaleY = Mathf.Abs(startPos.y - endPos.y);
        float scaleZ = Mathf.Abs(startPos.z - endPos.z);

        slash.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        //add velocity based on headset position 
        Vector3 positionOfHead = headPosition.localPosition;
        srb.AddRelativeForce((positionOfHead - slash.transform.position).normalized * speed); //swap transformes if it moves towards

        /* spawn slash at point between start and end position
         * adjust angle based on those values
         * scale based on points
         * add velocity to move away from head position (change to hand position average through swipe)
         * destroy after X seconds
         */
    }
}