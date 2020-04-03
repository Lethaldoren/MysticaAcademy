using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.AI;

public class BackAway : StateBehaviour
{
    public NavMeshAgent agent;

    Vector3Var waitPosition;
    Animator anim; 
    private void Awake() {
        //gets position of enemy when waiting for attack
        waitPosition = blackboard.GetVector3Var("WaitPosition");
        anim = GetComponentInChildren<Animator>(); 
    }

    // Called when the state is enabled
    void OnEnable () {
        agent.Resume();
        agent.updateRotation = false;
        anim.SetBool("WalkBack", true);
	}
	
	// Update is called once per frame
	void Update () {

        //calculates distance from where they are going to wait
        float distance = Vector3.Distance(waitPosition.Value, transform.position);

        //moves to position they are going to wait to attack
        if (distance > 0.5f) {
            agent.SetDestination(waitPosition.Value);

            Vector3 targetDirection = blackboard.GetGameObjectVar("PlayerObject").Value.transform.position - transform.position;
            targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z).normalized;

            transform.rotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, targetDirection, Vector3.up), Vector3.up);
            
        }
        else {
            agent.updateRotation = true;
            anim.SetBool("WalkBack", false);
            SendEvent("BackedAway");
        }
    }
}


