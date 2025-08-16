using Mirror;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerScript : NetworkBehaviour
{

    [SerializeField] public ZombieSpawnSetting[] zombieSpawnSettings;
    [SerializeField] private int waveNamber = 1;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int playerCount = 1;
    private List<ZombieController> zombies = new List<ZombieController>();
    
    public WaveManagerScript Instance { get; private set; }

    public void Start()
    {
        if (Instance == null) return;
        Instance = this;
    }

    public void OnDestroy()
    {
        Instance = null;
    }

    public void SpawnWave()
    {
        for (int i = 0; i < zombieSpawnSettings.Length; ++i)
        {
            StartCoroutine(SpawnCoroutine(zombieSpawnSettings[i]));
        }
        ++waveNamber;
        for (int i = 0; i < zombieSpawnSettings.Length; i++)
        {
            ++zombieSpawnSettings[i].ZombieSpawnFactor;
        }
    }

    private IEnumerator SpawnCoroutine(ZombieSpawnSetting spawnSetting)
    {
        for (int i = 0; i < spawnSetting.ZombieSpawnFactor * playerCount; ++i)
        {
            SpawnZombie(spawnSetting.ZombiePrefab);
            yield return new WaitForSeconds(spawnSetting.ZombieSpawnDelay);
        }
    }

    private void SpawnZombie(GameObject zombiePrefab)
    {
        if (spawnPoints.Length == 0) return;

        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var zombieObj = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        var zombie = zombieObj.GetComponent<ZombieController>();

        if (zombie == null) return;

        zombies.Add(zombie);
    }
}
