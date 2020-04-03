using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public AnalyticsValues valuesSO;

    public int amountOfFireballsUsed;
    public int timesEnemyWasHit;
    public int timesPlayerTeleports;
    public int amountOfEnemiesKilled;
    public int timesPlayerHit;

    void FireballShot() {
        amountOfFireballsUsed += 1;
    }

    void EnemyHit() {
        timesEnemyWasHit += 1;
    }

    void PlayerTeleported() {
        timesPlayerTeleports += 1;
    }

    void EnemyKilled() {
        amountOfEnemiesKilled += 1;
    }

    void PlayerHit() {
        timesPlayerHit += 1;
    }



    /*events to track:
     * how many spells are being used
     * how many hit enemies
     * how many times player teleports
     * kill count
     * time between each wave
     * damage to player
     */
}
