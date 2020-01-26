using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, EquipableSpell
{
    public FireBall()
    {

    }

    public new void OnEquip()
    {
        Debug.Log("I am Fireball");
    }

    public new void OnTriggerDown()
    {

    }

    public new void OnTriggerHeld()
    {

    }


    public new void OnTriggerUp()
    {

    }

}
