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
    BoolVar hasToken;
    Vector3Var waitPosition;

    float randomRadius;

    private void Awake() {
        aiManager = blackboard.GetGameObjectVar("AIManager");
        speed = blackboard.GetFloatVar("Speed");
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        hasToken = blackboard.GetBoolVar("HasToken");
        waitPosition = blackboard.GetVector3Var("WaitPosition");
    }

    // Called when the state is enabled
    void OnEnable () {
        //assigns a random radius to stop enemy at
        randomRadius = Random.Range(8, 15);
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

        CheckForToken();
    }

    //moves the enemy towards the player and stops them when they are within the random radius 
    bool WaypointCheck() {

        var distanceSquared = (transform.position - playerObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < randomRadius * randomRadius) {
            return true;
        }
        return false;
    }

    void CheckForToken() {

        bool gotToken = aiManager.Value.GetComponent<AIManager>().CanTakeToken();

        if (gotToken) {
            hasToken.Value = true;
            SendEvent("GotToken");
        }
    }
}


