using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public int enemyCount;
    public Transform[] spawnPoints;
}

public class GameMaster : MonoBehaviour
{
    [Header("Enemy Spawns")]
    public Wave[] waves;
    private int currentWave;
    private Transform[] currentSpawns;
    [SerializeField] private float waveInterval;
    [SerializeField] private float enemySpawnInterval;
    private int currentEnemies;
    [SerializeField] private GameObject[] drones;
    [SerializeField] private Transform player;
    [HideInInspector] public int deadEnemies;

    void FixedUpdate()
    {
        if (deadEnemies >= currentEnemies)
        {
            StartWave();
            currentWave += 1;
        }
    }

    private void StartWave()
    {
        currentEnemies = waves[currentWave].enemyCount;
        currentSpawns = waves[currentWave].spawnPoints;
        deadEnemies = 0;
        StartCoroutine(WaveSpawn());
    }

    private void SpawnEnemy()
    {
        int randomSpawn = Random.Range(0, currentSpawns.Count());
        int randomDrone = Random.Range(0, drones.Count());
        GameObject newEnemy = Instantiate(drones[randomDrone], currentSpawns[randomSpawn].position, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().player = player;
    }

    IEnumerator WaveSpawn() {
        yield return new WaitForSecondsRealtime(waveInterval);
        for (int i = 0; i < currentEnemies; i++)
        {
            yield return new WaitForSecondsRealtime(enemySpawnInterval);
            SpawnEnemy();
        }
    }
}
