using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
   
    private List<PlayerController> allPlayers = new List<PlayerController>();
    public List<PlayerController> AllPlayers => allPlayers;
    
    [SerializeField]
    private DayNightCycle dayNightCycle;
    public DayNightCycle DayNightCycle => dayNightCycle;
    
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
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }
    
    private void OnDestroy()
    {
        Instance = null;
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
