using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private Transform[] droneSpawns;
    [SerializeField] private GameObject[] drones;

    [SerializeField] private int[] waveEnemyCount;
    private int currentWave;
    [SerializeField] private float waveInterval;
    [SerializeField] private float enemySpawnInterval;

    [SerializeField] private Transform player;

    private int currentEnemies;
    [HideInInspector] public int deadEnemies;

    void FixedUpdate()
    {
        if (deadEnemies >= currentEnemies)
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        currentEnemies = waveEnemyCount[currentWave];
        deadEnemies = 0;
        StartCoroutine(WaveSpawn());
    }

    private void SpawnEnemy()
    {
        int randomSpawn = Random.Range(0, droneSpawns.Count());
        int randomDrone = Random.Range(0, drones.Count());
        GameObject newEnemy = Instantiate(drones[randomDrone], droneSpawns[randomSpawn].position, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().player = player;
    }

    IEnumerator WaveSpawn() {
        yield return new WaitForSecondsRealtime(waveInterval);
        for (int i = 0; i < waveEnemyCount[currentWave]; i++)
        {
            yield return new WaitForSecondsRealtime(enemySpawnInterval);
            SpawnEnemy();
        }
    }
}
