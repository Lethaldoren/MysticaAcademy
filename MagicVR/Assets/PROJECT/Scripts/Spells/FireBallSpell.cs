﻿
using UnityEngine;

public class FireBallSpell : SpellBase
{
    public Transform wandTip;
    GameObject fireball;
    Rigidbody frb;

    float speed = 400;

    public FireBallSpell()
    {
        base.magicWords = "Fire Ball";
    }

    public override void OnEquip()
    {
        Debug.Log("I am Fireball");
        wandTip = this.transform.GetChild(0);
        //create fireball particle effect and have it on hand
    }

    public override void OnTriggerDown()
    {
        //create fireball and change parent
        fireball = Instantiate(spellPrefab, wandTip);
        frb = fireball.GetComponent<Rigidbody>();
    }

    public override void OnTriggerHeld()
    {
        //keep fireball in hand until trigger released
        //give croshair for shot direction
    }


    public override void OnTriggerUp()
    {
        //records position at release and unparents from hand and moves to position at release
        Transform currentPosition = fireball.transform;
        fireball.transform.parent = null;
        fireball.transform.position = currentPosition.transform.position;

        //shoots fireball forward at speed
        if (frb != null)
        {
            frb.AddRelativeForce(Vector3.forward * speed);
        }
        //destroy prefab after 5 seconds or contact
    }

    public override void OnUnequip()
    {
        Destroy(this);
    }

}
