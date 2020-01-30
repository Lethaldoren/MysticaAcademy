using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class runeplace : MonoBehaviour
{
    public GameObject rune;
    public int count;
    public float radius;

    [ExecuteInEditMode]
    [MenuItem("Place Runes")]
    public void PlaceRunes()
    {
        for (int i = 0; i < count; i++)
        {
            float a = (2 * Mathf.PI) * (i / count);
            Vector3 pos = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * radius;
            Quaternion rot = Quaternion.Euler(90, 0, a * Mathf.Rad2Deg);
            Instantiate(rune, pos, rot, transform);
        }
    }
}
