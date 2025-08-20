using System;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : NetworkBehaviour
{
    public event Action<int, bool> OnWaveStateChanged;
    
    public ZombieSpawnSetting[] zombieSpawnSettings;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] public int playerCount; 
    List<GameObject> Zombies { get; } = new List<GameObject>();
    
    public static WaveManager Instance { get; private set; }
    [Server]
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

    [Server]
    public void OnDestroy()
    {
        Instance = null;
    }
    [Server]
    public void SpawnWave()
    {
        playerCount = GameManager.Instance.AllPlayers.Count;
        for (int i = 0; i < zombieSpawnSettings.Length; ++i)
        {
            StartCoroutine(SpawnCoroutine(zombieSpawnSettings[i]));
        }
        ++waveNumber;
        for (int i = 0; i < zombieSpawnSettings.Length; i++)
        {
            ++zombieSpawnSettings[i].ZombieSpawnFactor;
        }
        OnWaveStateChanged?.Invoke(waveNumber, true);
    }
    [Server]
    private IEnumerator SpawnCoroutine(ZombieSpawnSetting spawnSetting)
    {
        for (int i = 0; i < spawnSetting.ZombieSpawnFactor * playerCount; ++i)
        {
            SpawnEnemy(spawnSetting.ZombiePrefab);
            yield return new WaitForSeconds(spawnSetting.ZombieSpawnDelay);
        }
    }
    [Server]
    private void SpawnEnemy(GameObject zombiePrefab)
    {
        if (spawnPoints.Length == 0) return;

        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        NetworkServer.Spawn(zombie);

        if (zombie == null) return;
        var controller = zombie.GetComponent<ZombieController>();
        controller.OnDeath += OnEnemyDeath;
        Zombies.Add(zombie);
    }
    
    [Server]
    private void OnEnemyDeath(ZombieController zombie)
    {
        if (Zombies.Contains(zombie.gameObject))
        {
            Zombies.Remove(zombie.gameObject);
        }
        if (Zombies.Count == 0)
        {
            Debug.Log("All zombies are dead, spawning next wave.");
            OnWaveStateChanged?.Invoke(waveNumber, false);
            GameManager.Instance.WaveEnd();
        }
    }
    [Server]
    public void ResetWave()
    {
        waveNumber = 1;
        Zombies.Clear();
        foreach (var spawnSetting in zombieSpawnSettings)
        {
            spawnSetting.ZombieSpawnFactor = 1;
        }
    }
    
#if UNITY_EDITOR
    [ContextMenu("Kill all enemies")]
    [Server]
    void KillAll()
    {
        var zombiesCopy = new List<GameObject>(Zombies);
        foreach (var zombie in zombiesCopy)
        {
            if (zombie != null)
            {
                var controller = zombie.GetComponent<ZombieController>();
                if (controller != null)
                {
                    Debug.Log("Killing " + zombie.name);
                    controller.Death();
                }
            }
        }
    }
#endif
}
