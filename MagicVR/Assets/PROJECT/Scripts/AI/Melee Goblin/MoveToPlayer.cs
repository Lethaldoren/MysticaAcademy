using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class MoveToPlayer : StateBehaviour
{
    public NavMeshAgent agent;

    GameObjectVar playerObject;
    FloatVar attackRadius;

    Animator anim;

    private void Awake() {
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        attackRadius = blackboard.GetFloatVar("AttackRadius");
        anim = GetComponentInChildren<Animator>();
    }

    // Called when the state is enabled
    void OnEnable () {
        agent.Resume();
        anim.GetComponentInChildren<Animator>();
        anim.SetBool("Walk", true);
    }
	
	// Update is called once per frame
	void Update () {

        //moves to the player position within radius of attack
        if (!WaypointCheck()) {
            agent.SetDestination(playerObject.transform.position);
        }
        else {
            agent.Stop();
            anim.SetBool("Walk", false);
            SendEvent("InRange");
        }
    }

    //checks if enemy is within radius of player that has beem set
    bool WaypointCheck() {

        var distanceSquared = (transform.position - playerObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < attackRadius.Value * attackRadius.Value) {
            return true;
        }
        return false;
    }
}


