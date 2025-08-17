using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> AllPlaeyrs { get; private set; } = new List<PlayerController>();

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) return;
        Instance = this;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }

    public void PlayerConnected(PlayerController player)
    {
        AllPlaeyrs.Add(player);
    }

    public void PlayerDisconnected(PlayerController player)
    {
        AllPlaeyrs.Remove(player);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}

}
