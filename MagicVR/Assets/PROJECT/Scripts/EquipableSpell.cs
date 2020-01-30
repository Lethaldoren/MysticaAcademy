
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EquipableSpell : MonoBehaviour
{
    public string magicWords;
    public GameObject spellPrefab;

    // what occurs when the spell is originally equiped
    public virtual void OnEquip()
    {
        print("fuck you");
    }
    public virtual void OnTriggerDown() { }
    public virtual void OnTriggerHeld() { }
    public virtual void OnTriggerUp() { }
    public virtual void OnUnequip()
    {
        Destroy(gameObject);
    }
}
