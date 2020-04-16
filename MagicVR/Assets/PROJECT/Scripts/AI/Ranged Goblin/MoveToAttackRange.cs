using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class MoveToAttackRange : StateBehaviour
{
    public NavMeshAgent agent;

    GameObjectVar playerObject;
    FloatVar waitRadius;

    Animator anim; 
    private void Awake() {
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        waitRadius = blackboard.GetFloatVar("WaitRadius");


    }

    // Called when the state is enabled
    void OnEnable () {
        agent.Resume();

        anim = GetComponentInChildren<Animator>();
        if (anim)
            anim.SetBool("Walk", true);

    }

    // Update is called once per frame
    void Update () {

        //moves enemy to attack radius of player
        if (!WaypointCheck()) {
            agent.SetDestination(playerObject.Value.transform.position);
        }
        else {
            agent.Stop();
            SendEvent("InRadius");
            if (anim)
                anim.SetBool("Walking", false);
        }
    }

    //checks if the enemy is within the radius of the player
    bool WaypointCheck() {

        var distanceSquared = (transform.position - playerObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < waitRadius.Value * waitRadius.Value) {
            return true;
        }
        return false;
    }

    void PlayerLeft() {
        SendEvent("LostPlayer");
    }
}


