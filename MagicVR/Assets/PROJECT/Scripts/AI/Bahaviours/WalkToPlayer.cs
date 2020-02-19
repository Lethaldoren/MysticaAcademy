using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine;
using UnityEngine.AI;

public class WalkToPlayer : StateBehaviour 
{
    public GameObjectVar player;
    Vector3 playerPosition;
    private NavMeshAgent agent;

    private FloatVar UpdateDelay;
    WaitForSeconds wait;
    
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine("UpdateTargetPosition");

        wait = new WaitForSeconds(UpdateDelay.Value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public IEnumerator UpdateTargetPosition()
    {
        while(true)
        {
            GetPlayerPosition();
            agent.SetDestination(playerPosition);
            yield return wait;
        }
    }

    void GetPlayerPosition()
    {
        
    }
}
