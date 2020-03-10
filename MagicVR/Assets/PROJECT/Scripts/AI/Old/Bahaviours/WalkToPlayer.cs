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

    private FloatVar updateDelay;
    private FloatVar attackRange;
    WaitForSeconds wait;
    
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        player = blackboard.GetGameObjectVar("Player");
        updateDelay = blackboard.GetFloatVar("UpdateDelay");
        attackRange = blackboard.GetFloatVar("AttackRange");

        wait = new WaitForSeconds(updateDelay.Value);
        StartCoroutine("UpdateTargetPosition");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if ((this.transform.position - playerPosition).magnitude < attackRange.Value)
        {
            blackboard.SendEvent("PlayerInRange");
        }
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
        playerPosition = player.Value.transform.position;
    }
}
