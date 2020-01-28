using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, EquipableSpell
{
    public GameObject fireballPrefab;
    GameObject fireball;

    public FireBall()
    {

    }

    public new void OnEquip()
    {
        Debug.Log("I am Fireball");
        //create fireball particle effect and have it on hand
    }

    public new void OnTriggerDown()
    {
        //create fireball in hand
    }

    public new void OnTriggerHeld()
    {
        //keep fireball in hand until trigger released
        //give croshair for shot direction
    }


    public new void OnTriggerUp()
    {
        //shoot fireball here
    }

}
