using UnityEngine;
using BehaviourMachine;
using UnityEngine.AI;

public class RoamAround : StateBehaviour
{
    public NavMeshAgent agent;
    public float roamSpeed;


	// Called when the state is enabled
	void OnEnable () {
        agent.speed = roamSpeed;
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}


