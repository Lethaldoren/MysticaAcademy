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

        rockObject = blackboard.GetGameObjectVar("RockObject");
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
            SendEvent("InRange");
        }
    }

    bool WaypointCheck() {

        var distanceSquared = (transform.position - rockObject.Value.transform.position).sqrMagnitude;

        if (distanceSquared < 0.5f * 0.5f) {
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
