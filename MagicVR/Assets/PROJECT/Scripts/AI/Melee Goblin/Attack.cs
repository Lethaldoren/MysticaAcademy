using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class Attack : StateBehaviour
{
    public float attackTime;

    WaitForSecondsRealtime attackDelay;

    private void Start() {
        attackDelay = new WaitForSecondsRealtime(attackTime);
    }

    private void OnEnable() {
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay() {

        yield return attackDelay;
        AttackPlayer();
    }

    void AttackPlayer() {

        SendEvent("Attacked");
    }

    /* wait attack time
     * send damage player
     * move to next state
     */

}


