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
    FloatVar waitRadius;
    Animator anim; 
    [Header("Low(x) - High(y) Range")]
    public Vector2 waitRadiusRange;

    private void Awake() {
        aiManager = blackboard.GetGameObjectVar("AIManager");
        speed = blackboard.GetFloatVar("Speed");
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        waitPosition = blackboard.GetVector3Var("WaitPosition");
        waitRadius = blackboard.GetFloatVar("WaitRadius");
        anim = GetComponentInChildren<Animator>();
    }

    // Called when the state is enabled
    void OnEnable () {
        //assigns a random radius to stop enemy at
        waitRadius.Value = Random.Range(waitRadiusRange.x, waitRadiusRange.y);
        agent.speed = speed.Value;
        agent.Resume();
        anim.GetComponentInChildren<Animator>();
        anim.SetBool("Walk", true);
    }
	
	// Update is called once per frame
	void Update () {

        //moves enemy to random radius of player
        if (!WaypointCheck()) {
            agent.SetDestination(playerObject.transform.position);
        }
        else {
            waitPosition.Value = transform.position;
            agent.Stop();
            anim.SetBool("Walk", false);
            SendEvent("InRadius");
        }
    }

    //moves the enemy towards the player and stops them when they are within the random radius 
    bool WaypointCheck() {

        var distanceSquared = (transform.position - playerObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < waitRadius.Value * waitRadius.Value) {
            return true;
        }
        return false;
    }
}


