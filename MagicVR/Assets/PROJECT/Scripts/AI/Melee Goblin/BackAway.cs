using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class BackAway : StateBehaviour
{
    public NavMeshAgent agent;

    Vector3Var waitPosition;

    private void Awake() {
        waitPosition = blackboard.GetVector3Var("WaitPosition");
    }

    // Called when the state is enabled
    void OnEnable () {
        agent.Resume();
	}
	
	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(waitPosition.Value, transform.position);

        if (distance > 0.5f) {
            agent.SetDestination(waitPosition.Value);
        }
        else {
            SendEvent("BackedAway");
        }
    }
}


