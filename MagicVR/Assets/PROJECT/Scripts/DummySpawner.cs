using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    public GameObject dummyPrefab;
    public GameObject spawnedDummy;
    public float spawnRingRadius;

    void Update()
    {
        if (!spawnedDummy) 
        {
            float a = Random.value * Mathf.PI * 2;
            spawnedDummy = Instantiate(
                dummyPrefab, 
                new Vector3(Mathf.Cos(a) * spawnRingRadius, 2.5f,Mathf.Sin(a) * spawnRingRadius), 
                Quaternion.AngleAxis(a, Vector3.up)
            );
        }
    }
}
