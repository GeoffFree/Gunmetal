using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Artillery")]
    [SerializeField] private int artilleryTick;
    [SerializeField] private int artilleryThresholdMin;
    [SerializeField] private int artilleryThresholdMax;
    private int currentArtilleryThreshold;
    private float currentArtilleryLikelihood;
    private bool hasSpawnedArtillery; // If artillery has spawned this wave
    public GameObject artillery;
    public Transform artillerySpawn;

    [Header("Other")]
    public TMP_Text waveIndicator;
    public TMP_Text score;
    public AudioMaster audioMaster;

    void FixedUpdate()
    {
        if (deadEnemies >= currentEnemies)
        {
            StartWave();
            currentWave += 1;
        }
        else if (deadEnemies < currentEnemies * 0.8f) // Only spawn artillery if wave is less than 80% done.
        {
            if (hasSpawnedArtillery)
            {
                return;
            }

            if (currentArtilleryLikelihood > currentArtilleryThreshold)
            {
                SpawnArtillery();
                hasSpawnedArtillery = true;
            }
            else
            {
                currentArtilleryThreshold += artilleryTick;
            }
        }
    }

    private void StartWave()
    {
        if (currentWave >= waves.Count())
        {
            SceneManager.LoadScene(2);
        }
        currentEnemies = waves[currentWave].enemyCount;
        currentSpawns = waves[currentWave].spawnPoints;
        deadEnemies = 0;
        waveIndicator.text = "Wave " + (currentWave + 1);
        StartCoroutine(WaveSpawn());

        currentArtilleryLikelihood = 0;
        currentArtilleryThreshold = Random.Range(artilleryThresholdMin, artilleryThresholdMax);
        hasSpawnedArtillery = false;
    }

    private void SpawnEnemy()
    {
        int randomSpawn = Random.Range(0, currentSpawns.Count());
        int randomDrone = Random.Range(0, drones.Count());
        GameObject newEnemy = Instantiate(drones[randomDrone], currentSpawns[randomSpawn].position, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().player = player;
    }

    private void SpawnArtillery()
    {
        GameObject newArtillery = Instantiate(artillery, artillerySpawn.position, Quaternion.identity);
        newArtillery.GetComponent<Artillery>().player = player.transform;
        newArtillery.GetComponent<Artillery>().audioMaster = audioMaster;
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
