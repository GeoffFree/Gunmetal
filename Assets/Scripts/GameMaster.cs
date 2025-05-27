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
    private float waveTimer;
    [SerializeField] private float enemySpawnInterval;

    [SerializeField] private Transform player;

    private List<Transform> enemyList = new();

    public void Start()
    {
        StartWave();
    }

    private void StartWave() {
        if(waveTimer >= Time.time) {
            waveTimer = waveInterval + Time.time;
            StartCoroutine(WaveSpawn());
        }
    }

    private void SpawnEnemy() {
        int randomSpawn = Random.Range(0, droneSpawns.Count());
        int randomDrone = Random.Range(0, drones.Count());
        GameObject newEnemy = Instantiate(drones[randomDrone], droneSpawns[randomSpawn].position, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().player = player;
        enemyList.Add(newEnemy.transform);
    }

    IEnumerator WaveSpawn() {
        for(int i=0; i < waveEnemyCount[currentWave]; i++) {
            yield return new WaitForSecondsRealtime(enemySpawnInterval);
            SpawnEnemy();
        }
        currentWave += 1;
    }
}
