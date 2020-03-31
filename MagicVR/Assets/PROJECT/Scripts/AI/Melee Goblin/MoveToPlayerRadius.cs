using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class MoveToPlayerRadius : StateBehaviour
{
    public NavMeshAgent agent;

    GameObjectVar aiManager;
    FloatVar speed;
    GameObjectVar playerObject;
    Vector3Var waitPosition;
    FloatVar randRadius;

    private void Awake() {
        aiManager = blackboard.GetGameObjectVar("AIManager");
        speed = blackboard.GetFloatVar("Speed");
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        waitPosition = blackboard.GetVector3Var("WaitPosition");
        randRadius = blackboard.GetFloatVar("RandRadius");
    }

    // Called when the state is enabled
    void OnEnable () {
        //assigns a random radius to stop enemy at
        randRadius.Value = Random.Range(8, 15);
        agent.speed = speed.Value;
        agent.Resume();
    }
	
	// Update is called once per frame
	void Update () {

        if (!WaypointCheck()) {
            agent.SetDestination(playerObject.transform.position);
        }
        else {
            waitPosition.Value = transform.position;
            agent.Stop();
            SendEvent("InRadius");
        }
    }

    //moves the enemy towards the player and stops them when they are within the random radius 
    bool WaypointCheck() {

        var distanceSquared = (transform.position - playerObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < randRadius.Value * randRadius.Value) {
            return true;
        }
        return false;
    }
}


