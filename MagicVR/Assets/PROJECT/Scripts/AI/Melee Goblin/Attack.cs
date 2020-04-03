using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class Attack : StateBehaviour
{
    GameObjectVar playerObject;
    PlayerHealth playerHealthScr;

    public float hitDamage;

    private void Awake() {
        playerObject = blackboard.GetGameObjectVar("PlayerObject");

        playerHealthScr = playerObject.Value.GetComponent<PlayerHealth>();
    }

    // Called when the state is enabled
    void OnEnable () {

        playerHealthScr.Damage(hitDamage);

        SendEvent("Attacked");
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}


