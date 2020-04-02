using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviourMachine;
using UnityEngine.VFX;
using Valve.VR.InteractionSystem;

public class RangeAttack : StateBehaviour
{
    GameObjectVar playerObject;
    Vector3 playerPos;

    Vector3 shotVelocity;
    public GameObject fireballPrefab;
    public float projectileSpeed;

    private void Awake() {

        playerObject = blackboard.GetGameObjectVar("PlayerObject");
    }

    // Called when the state is enabled
    void OnEnable () {

        
	}
	
	// Update is called once per frame
	void Update () {
        //get the players position
        playerPos = playerObject.Value.transform.position;

        //spawns a fireball infront of the enemy
        Vector3 spawnPos = transform.position + Vector3.back;
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        //calculates path fireball must travel
        shotVelocity = ((playerPos - transform.position).normalized) * projectileSpeed;

        //launches fireball in direction
        fireball.GetComponent<FireballProjectile>().Launch(shotVelocity);

        SendEvent("Attacked");
    }

    /*pseudocode:
     * spawn prefab
     * get firing direction
     * dest - origin normalized * mag
     * call launch method in fireball projectile script
     * sent it forwards towards player
     * move to next state
     */
}


