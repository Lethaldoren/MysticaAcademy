using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpellManager : SingletonBase<SpellManager>
{
    public EquipableSpell[] spellList;
}
