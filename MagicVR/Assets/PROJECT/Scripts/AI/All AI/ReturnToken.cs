using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;

public class ReturnToken : StateBehaviour
{
    GameObjectVar aiManager;
    BoolVar hasToken;

    private void Awake() {
        aiManager = blackboard.GetGameObjectVar("AIManager");
        hasToken = blackboard.GetBoolVar("HasToken");
    }

    void OnEnable() {
        //returns token to the token pool 
        aiManager.Value.GetComponent<AIManager>().ReturnToken();
        hasToken.Value = false;
        SendEvent("TokenReturned");
    }
}


