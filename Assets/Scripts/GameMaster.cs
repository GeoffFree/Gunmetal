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
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Player player;
    [HideInInspector] public int deadEnemies;

    [Header("Artillery")]
    private int artilleryTick = 1;
    [SerializeField] private int artilleryThresholdMin;
    [SerializeField] private int artilleryThresholdMax;
    private int currentArtilleryThreshold;
    private float currentArtilleryLikelihood;
    private bool hasSpawnedArtillery; // If artillery has spawned this wave
    public GameObject artillery;
    public Transform artillerySpawn;

    [Header("Other")]
    public TMP_Text waveIndicator;
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
                currentArtilleryLikelihood += artilleryTick;
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
        newEnemy.GetComponent<Enemy>().player = playerTarget;
        int randomChild = Random.Range(0, currentSpawns[randomSpawn].childCount);
        newEnemy.GetComponent<Enemy>().target = currentSpawns[randomSpawn].GetChild(randomChild);
    }

    private void SpawnArtillery()
    {
        GameObject newArtillery = Instantiate(artillery, artillerySpawn.position, Quaternion.identity);
        newArtillery.GetComponent<ArtilleryShip>().player = player.GetComponent<Player>();
        newArtillery.GetComponent<ArtilleryShip>().audioMaster = audioMaster;
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
