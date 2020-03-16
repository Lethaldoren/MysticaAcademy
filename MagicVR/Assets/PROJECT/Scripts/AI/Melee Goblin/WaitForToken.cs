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
    FloatVar randRadius;

    private void Awake() {
        aiManager = blackboard.GetGameObjectVar("AIManager");
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        hasToken = blackboard.GetBoolVar("HasToken");
        randRadius = blackboard.GetFloatVar("RandRadius");
    }

    // Called when the state is enabled
    void OnEnable () {
        agent.Stop();
	}
	
	// Update is called once per frame
	void Update () {

        if (hasToken.Value) {
            SendEvent("GotToken");
        }

        CheckForToken();

        if (!RadiusCheck()) {
            SendEvent("OutOfRadius");
        }
    }

    void CheckForToken() {

        bool gotToken = aiManager.Value.GetComponent<AIManager>().CanTakeToken();

        if (gotToken) {
            hasToken.Value = true;
            SendEvent("GotToken");
        }
    }

    bool RadiusCheck() {

        var distanceSquared = (transform.position - playerObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < randRadius.Value * randRadius.Value) {
            return true;
        }
        return false;
    }
}


