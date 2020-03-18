using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellResource
{
    public int ID;
    public spRType type;

    private float floatVar;
    private Vector4 vectorVar;
    private GameObject prefabVar;

    public SpellResource()
    {
        type = spRType.FLOAT;
    }
}

public enum spRType
{
    FLOAT, VECTOR, PREFAB
}