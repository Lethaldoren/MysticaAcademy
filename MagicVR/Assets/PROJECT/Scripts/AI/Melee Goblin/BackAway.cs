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
        //gets position of enemy when waiting for attack
        waitPosition = blackboard.GetVector3Var("WaitPosition");
    }

    // Called when the state is enabled
    void OnEnable () {
        agent.Resume();
	}
	
	// Update is called once per frame
	void Update () {

        //calculates distance from where they are going to wait
        float distance = Vector3.Distance(waitPosition.Value, transform.position);

        //moves to position they are going to wait to attack
        if (distance > 0.5f) {
            agent.SetDestination(waitPosition.Value);
        }
        else {
            SendEvent("BackedAway");
        }
    }
}


