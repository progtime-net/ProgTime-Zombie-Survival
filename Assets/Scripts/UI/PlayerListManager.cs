using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerListManager : NetworkBehaviour
{
    public static PlayerListManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Transform scrollContent;
    [SerializeField] private GameObject plashPrefab;
    [SerializeField] private TextMeshProUGUI lobbyTitleTxt;
    
    // SERVER ONLY:
    private readonly Dictionary<int, LobbyPlayerController> _playersById = new Dictionary<int, LobbyPlayerController>();
    // CLIENT ONLY:
    private readonly Dictionary<int, PlayerPlashController> _uiPlashes = new Dictionary<int, PlayerPlashController>();

    [SyncVar] private int hostConnectionId = -1;

    public bool IsLocalHost => NetworkServer.active && NetworkClient.isConnected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple LobbyManagers!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Server]
    public void ServerRegister(LobbyPlayerController p)
    {
        _playersById[p.PlayerId] = p;
    }

    [Server]
    public void ServerUnregister(LobbyPlayerController p)
    {
        if (_playersById.ContainsKey(p.PlayerId))
            _playersById.Remove(p.PlayerId);
    }
    
    [Server]
    public void ServerKick(int targetPlayerId)
    {
        foreach (var kv in NetworkServer.connections)
        {
            if (kv.Value != null && kv.Value.connectionId == targetPlayerId)
            {
                kv.Value.Disconnect();
                break;
            }
        }
    }
    
    // ---------- UI (for clients) ----------

    [Client]
    public void ClientUpdateLobbyTitle()
    {
        if (lobbyTitleTxt == null) return;

        if (_uiPlashes.TryGetValue(hostConnectionId, out var hostPlash))
        {
            lobbyTitleTxt.text = $"Лобби игрока {hostPlash.GetNickname()}";
        }
    }
    
    [Client]
    public void ClientAddPlash(int playerId, string nickname)
    {
        if (_uiPlashes.ContainsKey(playerId)) return;
        if (scrollContent == null || plashPrefab == null) return;

        GameObject go = Instantiate(plashPrefab);
  
        go.transform.SetParent(scrollContent, false);
        go.transform.localScale = Vector3.one; 

        var plash = go.GetComponent<PlayerPlashController>();
        _uiPlashes[playerId] = plash;

        bool isAdmin = (playerId == hostConnectionId);
        bool isOnHost = IsLocalHost;

        plash.Init(KickFromUI, playerId, nickname, isAdmin, isOnHost);
        ClientUpdateLobbyTitle();
    }

    [Client]
    public void ClientRemovePlash(int playerId)
    {
        if (_uiPlashes.TryGetValue(playerId, out var plash) && plash != null)
        {
            Destroy(plash.gameObject);
        }
        _uiPlashes.Remove(playerId);
    }

    // ---------- UI HOOKS (вызов от UI) ----------

    public void KickFromUI(int targetPlayerId)
    {
        if (!IsLocalHost) return; 
        if (LobbyPlayerController.LocalInstance != null)
            LobbyPlayerController.LocalInstance.CmdRequestKick(targetPlayerId);
    }
}
