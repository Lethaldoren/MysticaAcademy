using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : SingletonBase<AIManager>
{
    public List<Wave> waves = new List<Wave>();

    int currentWaveNumber;
    GameObject currentWaveObject;
    bool waveComplete;
    public float timeBetweenWaves;

    public float tokenCooldownTime;

    int maxTokens;
    int currentTokens;
    int TokensWithAI;
    int tokensOnCooldown;

    WaitForSecondsRealtime waveDelay;
    WaitForSecondsRealtime tokenCooldown;

    int enemiesLeft;

    // Start is called before the first frame update
    void Start()
    {
        //gets number of waves in scene and orders them into a list then starts the first wave
        currentWaveNumber = 0;
        waves = Resources.FindObjectsOfTypeAll<Wave>().OrderBy(w => w.waveNumber).ToList();
        waveDelay = new WaitForSecondsRealtime(timeBetweenWaves);
        tokenCooldown = new WaitForSecondsRealtime(tokenCooldownTime);
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {

        //constantly checks enemies left in wave
        enemiesLeft = currentWaveObject.transform.childCount;

        //checks for 0 enemies left in wave and starts next wave
        if (!waveComplete && enemiesLeft <= 0)
        {
            waveComplete = true;
            //start next wave
            StartCoroutine(DelayNewWave());
        }
    }

    //delays next wave by X seconds
    IEnumerator DelayNewWave()
    {
        yield return waveDelay;
        StartWave();
    }

    //puts every token that comes back into a cooldown
    IEnumerator TokenCooldown() {

        yield return tokenCooldown;
        currentTokens += 1;
        tokensOnCooldown -= 1;
    }

    //starts wave by calling wave in list and activating the game object
    public void StartWave()
    {
        currentWaveNumber += 1;

        currentWaveObject = waves[currentWaveNumber - 1].gameObject;

        currentWaveObject.SetActive(true);
        enemiesLeft = currentWaveObject.transform.childCount;
        maxTokens = currentWaveObject.GetComponent<Wave>().availableTokens;
        currentTokens = maxTokens;

        waveComplete = false;
    }

    //AI will check if they can take a token for attacking
    public bool CanTakeToken() {

        if (currentTokens > 0) {
            currentTokens -= 1;
            TokensWithAI += 1;
            return true;
        }
        else {
            return false;
        }  
    }

    //for AI ot return token after attacking
    public void ReturnToken() {

        TokensWithAI -= 1;
        tokensOnCooldown += 1;
        StartCoroutine(TokenCooldown());
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
