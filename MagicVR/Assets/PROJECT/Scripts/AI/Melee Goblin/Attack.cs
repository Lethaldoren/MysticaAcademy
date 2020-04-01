using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class Attack : StateBehaviour
{
	// Called when the state is enabled
	void OnEnable () {
        SendEvent("Attacked");
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}


