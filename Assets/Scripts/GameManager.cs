using System;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public List<PlayerController> AllPlayers { get; private set; } = new List<PlayerController>();

    [SerializeField] private int waveStartTime = 30;
    [SerializeField] private DayNightCycle dayNightCycle;
    public DayNightCycle DayNightCycle => dayNightCycle;

    public event Action<int> OnTimerUpdate;

    [SyncVar(hook = nameof(OnTimeLeftChanged))]
    private int syncedTimeLeft;

    private float _timeLeft = 0;
    private float _timeSnapshot;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (isServer)
        {
            WaveEnd();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    [Server]
    public void WaveEnd()
    {
        dayNightCycle.SetIsDay(true);
        StartTimer(waveStartTime);
    }

    [Server]
    public void StartTimer(int seconds)
    {
        _timeLeft = seconds;
        _timeSnapshot = Time.time;

        UpdateTimer();
        InvokeRepeating(nameof(UpdateTimer), 0f, 0.5f);
    }

    [Server]
    private void UpdateTimer()
    {
        int _left = Mathf.CeilToInt(Mathf.Max(0f, _timeLeft - (Time.time - _timeSnapshot)));
        syncedTimeLeft = _left; // SyncVar will trigger hook on clients

        if (_left <= 0)
        {
            dayNightCycle.SetIsDay(false);
            WaveManager.Instance.SpawnWave();
            CancelInvoke(nameof(UpdateTimer));
        }
    }

    private void OnTimeLeftChanged(int oldValue, int newValue)
    {
        OnTimerUpdate?.Invoke(newValue);
    }

    public void PlayerConnected(PlayerController player)
    {
        AllPlayers.Add(player);
    }

    public void PlayerDisconnected(PlayerController player)
    {
        AllPlayers.Remove(player);
    }
}