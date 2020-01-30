
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipableSpell : MonoBehaviour
{
    public string magicWords;
    public GameObject spellPrefab;

    // what occurs when the spell is originally equiped
    public void OnEquip() { }
    public void OnTriggerDown() { }
    public void OnTriggerHeld() { }
    public void OnTriggerUp() { }
    public void OnUnequip() { }
}
