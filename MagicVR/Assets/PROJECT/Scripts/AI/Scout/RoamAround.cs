using UnityEngine;
using BehaviourMachine;
using UnityEngine.AI;

public class RoamAround : StateBehaviour
{
    public NavMeshAgent agent;
    public float roamSpeed;

    //array components
    public GameObject[] waypoints;
    int arraySize;
    int i = 0;

    private void Start() {
        arraySize = waypoints.Length;
    }

    // Called when the state is enabled
    void OnEnable() {
        agent.speed = roamSpeed;
    }

    // Update is called once per frame
    void Update() {

        if (!WaypointCheck()) {
            agent.SetDestination(waypoints[i].transform.position);
        }
        else {
            if (i >= arraySize - 1) {
                i = 0;
            }
            else {
                i++;
            }
        }
    }

    void PlayerNear() {
        SendEvent("FoundPlayer");
    }

    //checks if enemy is within radius of player that has beem set
    bool WaypointCheck() {

        var distanceSquared = (transform.position - waypoints[i].transform.position).sqrMagnitude;

        if (distanceSquared < 0.5f * 0.5f) {
            return true;
        }
        return false;

    }
}


