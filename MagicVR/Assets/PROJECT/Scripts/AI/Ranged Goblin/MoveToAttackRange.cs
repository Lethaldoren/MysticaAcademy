using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class MoveToAttackRange : StateBehaviour
{
    public NavMeshAgent agent;

    GameObjectVar aiManager;
    FloatVar speed;
    GameObjectVar playerObject;
    FloatVar waitRadius;

    private void Awake() {
        aiManager = blackboard.GetGameObjectVar("AIManager");
        speed = blackboard.GetFloatVar("Speed");
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        waitRadius = blackboard.GetFloatVar("WaitRadius");
    }

    // Called when the state is enabled
    void OnEnable () {
        agent.speed = speed.Value;
        agent.Resume();
	}

    // Update is called once per frame
    void Update () {

        //moves enemy to attack radius of player
        if (!WaypointCheck()) {
            agent.SetDestination(playerObject.transform.position);
        }
        else {
            agent.Stop();
            SendEvent("InRadius");
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
}


