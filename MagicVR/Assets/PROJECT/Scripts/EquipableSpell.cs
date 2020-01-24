using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EquipableSpell
{
    // what occurs when the spell is originally equiped
    void OnEquip();

    void OnTriggerDown();

    void OnTriggerHeld();

    void OnTriggerUp();
}
