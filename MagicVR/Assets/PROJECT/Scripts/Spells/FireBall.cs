using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, EquipableSpell
{
    public GameObject fireballPrefab;
    GameObject fireball;
    Rigidbody frb;

    public float speed;


    public new void OnEquip()
    {
        Debug.Log("I am Fireball");
        //create fireball particle effect and have it on hand
    }

    public void OnTriggerDown()
    {
        //create fireball and change parent
        fireball = Instantiate(fireballPrefab, gameObject.transform);
        frb = fireball.GetComponent<Rigidbody>();
    }

    public void OnTriggerHeld()
    {
        //keep fireball in hand until trigger released
        //give croshair for shot direction
    }


    public void OnTriggerUp()
    {
        //records position at release and unparents from hand and moves to position at release
        Transform currentPosition = fireball.transform;
        fireball.transform.parent = null;
        fireball.transform.position = currentPosition.transform.position;

        //shoots fireball forward at speed
        if (frb != null) {
            frb.AddRelativeForce(Vector3.forward * speed);
        }
        //destroy prefab after 5 seconds or contact
    }

}
