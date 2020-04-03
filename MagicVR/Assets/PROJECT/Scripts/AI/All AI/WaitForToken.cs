using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class WaitForToken : StateBehaviour
{
    public NavMeshAgent agent;

    GameObjectVar aiManager;
    GameObjectVar playerObject;
    BoolVar hasToken;
    FloatVar waitRadius;

    private void Awake() {
        aiManager = blackboard.GetGameObjectVar("AIManager");
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        hasToken = blackboard.GetBoolVar("HasToken");
        waitRadius = blackboard.GetFloatVar("WaitRadius");
    }

    // Called when the state is enabled
    void OnEnable () {
        //agent.Stop();
	}
	
	// Update is called once per frame
	void Update () {

        //checks if this enemy has a token
        if (hasToken.Value) {
            SendEvent("GotToken");
        }

        CheckForToken();

        //sends to state to move this enemy close to player again
        if (!RadiusCheck()) {
            SendEvent("OutOfRadius");
        }
    }

    //checks if there is a token in the pool to be taken 
    void CheckForToken() {

        bool gotToken = aiManager.Value.GetComponent<AIManager>().CanTakeToken();

        if (gotToken) {
            hasToken.Value = true;
            SendEvent("GotToken");
        }
    }

    //checks if this enemy is within the players radius in case the player moves around
    bool RadiusCheck() {

        var distanceSquared = (transform.position - playerObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < waitRadius.Value * waitRadius.Value) {
            return true;
        }
        return false;
    }
}


