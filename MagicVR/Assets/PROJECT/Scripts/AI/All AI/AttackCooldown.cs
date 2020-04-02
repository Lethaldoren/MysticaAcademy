using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class AttackCooldown : StateBehaviour
{
    public float cooldownTime;

    WaitForSecondsRealtime cooldownDelay;

    private void Start() {
        cooldownDelay = new WaitForSecondsRealtime(cooldownTime);
    }

    private void OnEnable() {
        //gives a cooldown before this enemy can attack again
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay() {

        yield return cooldownDelay;
        SendEvent("ReadyToAttack");
    }
}


