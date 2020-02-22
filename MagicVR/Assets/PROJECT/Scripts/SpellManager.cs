using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpellManager : SingletonBase<SpellManager>
{
    public Spell[] spellList;

    public Spell GetSpell(string spellName)
    {
        Spell spell = spellList.First(s => s.magicWords == spellName);
        // if spell is not null, return it, otherwise null
        return spell ? spell : null;
    }
}
