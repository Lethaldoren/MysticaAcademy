using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;
using UnityEngine.AI;


public class Investigate : StateBehaviour
{
    public NavMeshAgent agent;

    GameObjectVar rockObject;

    public float investigationTime;

    WaitForSecondsRealtime investigationDelay;

    // Start is called before the first frame update
    void Awake() {

        rockObject = blackboard.GetGameObjectVar("NoisyProp");
    }

    private void Start() {

        investigationDelay = new WaitForSecondsRealtime(investigationTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!WaypointCheck()) {
            agent.SetDestination(rockObject.Value.transform.position);
        }
        else {
            StartCoroutine(InvestigateArea());
            SendEvent("InRange");
        }
    }

    bool WaypointCheck() {

        var distanceSquared = (transform.position - rockObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < 1) {
            return true;
        }
        return false;
    }

    void PlayerNear() {
        SendEvent("FoundPlayer");
    }

    IEnumerator InvestigateArea() {
        yield return investigationDelay;
        SendEvent("Investigated");

    }
}
