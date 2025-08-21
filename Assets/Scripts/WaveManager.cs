// csharp
using System;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices.WindowsRuntime;

public class WaveManager : NetworkBehaviour
{
    public event Action<int, bool> OnWaveStateChanged;

    public ZombieSpawnSetting[] zombieSpawnSettings;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] public int playerCount;
    List<GameObject> Zombies { get; } = new List<GameObject>();

    private int _spawnersInProgress;
    private bool _waveEnded;

    public static WaveManager Instance { get; private set; }

    public int WaveNamber => waveNumber;
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
        _waveEnded = false;
        _spawnersInProgress = 0;

        playerCount = GameManager.Instance.AllPlayers.Count;
        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < zombieSpawnSettings.Length; ++j)
            {
                StartCoroutine(SpawnCoroutine(zombieSpawnSettings[j]));
            }
        }

        ++waveNumber;
        for (int i = 0; i < zombieSpawnSettings.Length; i++)
        {
            ++zombieSpawnSettings[i].ZombieSpawnFactor;
        }
        AnnounceWaveStateChanged(waveNumber, true);
        // SoundManager.Instance.Play($"Wave{waveNumber}");
    }

    [Server]
    private IEnumerator SpawnCoroutine(ZombieSpawnSetting spawnSetting)
    {
        for (int i = 0; i < spawnSetting.ZombieSpawnFactor; ++i)
        {
            SpawnEnemy(spawnSetting.ZombiePrefab);
            yield return new WaitForSeconds(spawnSetting.ZombieSpawnDelay);
        }

        _spawnersInProgress = Mathf.Max(0, _spawnersInProgress - 1);
        TryFinishWave();
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
        // Unsubscribe to be safe
        zombie.OnDeath -= OnEnemyDeath;

        if (Zombies.Contains(zombie.gameObject))
        {
            Zombies.Remove(zombie.gameObject);
        }

        if (Zombies.Count == 0)
        {
            TryFinishWave();
        }
    }

    [Server]
    private void TryFinishWave()
    {
        if (_waveEnded) return;                
        if (_spawnersInProgress > 0) return;  
        if (Zombies.Count > 0) return;      

        _waveEnded = true;
        AnnounceWaveStateChanged(waveNumber, false);
        // SoundManager.Instance.Play("WaveEnd");
        GameManager.Instance.WaveEnd();
    }

    [Server]
    public void ResetWave()
    {
        waveNumber = 1;
        Zombies.Clear();
        _spawnersInProgress = 0;
        _waveEnded = false;

        foreach (var spawnSetting in zombieSpawnSettings)
        {
            spawnSetting.ZombieSpawnFactor = 1;
        }
    }

    [Server]
    private void AnnounceWaveStateChanged(int waveNumber, bool started)
    {
        RpcWaveStateChanged(waveNumber, started);
    }

    [ClientRpc]
    private void RpcWaveStateChanged(int waveNumber, bool started)
    {
        Debug.Log("Wave state changed: " + waveNumber + ", started: " + started);
        OnWaveStateChanged?.Invoke(waveNumber, started);
        if (started)
            SoundManager.Instance.Play($"Wave{waveNumber}");
        else
            SoundManager.Instance.Play("WaveEnd");
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