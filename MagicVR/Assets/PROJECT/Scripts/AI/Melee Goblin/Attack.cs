using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class Attack : StateBehaviour
{
    GameObjectVar playerObject;
    PlayerHealth playerHealthScr;

    public float hitDamage;
    Animator anim; 

    private void Awake() {
        playerObject = blackboard.GetGameObjectVar("PlayerObject");
        
        playerHealthScr = playerObject.Value.GetComponent<PlayerHealth>();
        anim = GetComponentInChildren<Animator>(); 
    }

    // Called when the state is enabled
    void OnEnable () {

        playerHealthScr.Damage(hitDamage);
        anim.SetTrigger("Attack");
        SendEvent("Attacked");
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}


