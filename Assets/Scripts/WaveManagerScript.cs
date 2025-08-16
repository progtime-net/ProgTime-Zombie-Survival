using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerScript : NetworkBehaviour
{

    public ZombieSpawnSetting[] zombieSpawnSettings;
    [SerializeField] private int waveNamber = 1;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] public int playerCount = 1;
    public List<GameObject> Zombies { get; } = new List<GameObject>();
    
    public static WaveManagerScript Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("More than one WaveManager in the scene!");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void OnDestroy()
    {
        Instance = null;
    }
    [Server]
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
    [Server]
    private IEnumerator SpawnCoroutine(ZombieSpawnSetting spawnSetting)
    {
        for (int i = 0; i < spawnSetting.ZombieSpawnFactor * playerCount; ++i)
        {
            SpawnZombie(spawnSetting.ZombiePrefab);
            yield return new WaitForSeconds(spawnSetting.ZombieSpawnDelay);
        }
    }
    [Server]
    private void SpawnZombie(GameObject zombiePrefab)
    {
        if (spawnPoints.Length == 0) return;

        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var zombieObj = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        var zombie = zombieObj.GetComponent<GameObject>();

        if (zombie == null) return;

        Zombies.Add(zombie);
    }
}
