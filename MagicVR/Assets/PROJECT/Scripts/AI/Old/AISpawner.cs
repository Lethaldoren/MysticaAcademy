using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject meleePrefab;
    public GameObject RangedPrefab;

    public int spawnTurnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



/*check points available for spawning enemies
 * assign each enemy a point value
 * spawn enemies and remove points for each spawn
 * check what wave it is and spawn accordingly with percentages
 * when points run out stop spawning
 * check wave number and increase points and change spawn percentage
 */