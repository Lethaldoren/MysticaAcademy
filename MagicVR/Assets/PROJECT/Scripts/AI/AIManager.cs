using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : SingletonBase<AIManager>
{

    public List<Wave> waves = new List<Wave>();

    int currentWaveNumber;
    GameObject currentWaveObject;
    public int timeBetweenWaves;
    public bool waveComplete;

    WaitForSecondsRealtime waveDelay;

    public int enemiesLeft;

    // Start is called before the first frame update
    void Start()
    {
        currentWaveNumber = 0;
        waves = Resources.FindObjectsOfTypeAll<Wave>().OrderBy(w => w.waveNumber).ToList();
        waveDelay = new WaitForSecondsRealtime(timeBetweenWaves);
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {

        //constantly gets enemies left
        enemiesLeft = currentWaveObject.transform.childCount;
        //Debug.Log(enemiesLeft);
        //Debug.Log(currentWaveNumber);

        if (!waveComplete && enemiesLeft <= 0)
        {
            Debug.Log("Wave Complete");
            waveComplete = true;
            //start next wave
            StartCoroutine(DelayNewWave());
           // StartWave();
        }
    }

    IEnumerator DelayNewWave()
    {
        yield return waveDelay;
        StartWave();
    }

    //starts wave by calling wave in list and activating the game object
    public void StartWave()
    {
        Debug.Log("Wave Started");
        currentWaveNumber += 1;

        currentWaveObject = waves[currentWaveNumber - 1].gameObject;

        currentWaveObject.SetActive(true);
        enemiesLeft = currentWaveObject.transform.childCount;

        waveComplete = false;
    }
}

/* get list of ai of first wave
 * enable first wave enemies
 * get amount of enemies in the wave
 * allow the ai to take a token to attack when ready to attack
 * when attact is done move token to cooldown 
 * when cooldown is done allow token to be taken again
 * check for amount of ai per wave
 * end wave and enable enemies in next wave
 * add more tokens per wave
 */
