﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Transform scrollContent;
    [SerializeField] private GameObject plashPrefab;
    [SerializeField] private TextMeshProUGUI lobbyTitleTxt;
    [SerializeField] private GameObject startGameBtn;

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "GameScene";

    private readonly Dictionary<int, PlayerPlashController> _uiPlashes = new Dictionary<int, PlayerPlashController>();
    // SERVER ONLY:
    private readonly Dictionary<int, LobbyPlayerController> _playersById = new Dictionary<int, LobbyPlayerController>();

    [SyncVar] private int hostConnectionId = -1;

    private int _localPlayerId = -1;

    public bool IsLocalHost =>
        NetworkServer.active && NetworkClient.isConnected;

    public int HostId => hostConnectionId;

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

    private void Start()
    {
        startGameBtn.SetActive(isHost);
    }

    public override void OnStartServer()
    {
        hostConnectionId = NetworkServer.localConnection != null
            ? NetworkServer.localConnection.connectionId
            : 0;

        StartCoroutine(ServerPingBroadcastLoop());
    }

    // SERVER API

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

    private IEnumerator ServerPingBroadcastLoop()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            yield return wait;

            List<int> ids = new List<int>();
            List<int> pings = new List<int>();

            foreach (var kv in NetworkServer.connections)
            {
                var conn = kv.Value;
                if (conn == null) continue;

                int ms = Mathf.Max(0, Mathf.RoundToInt((float)(conn.rtt * 1000.0)));
                ids.Add(conn.connectionId);
                pings.Add(ms);
            }

            RpcUpdatePings(ids.ToArray(), pings.ToArray());
        }
    }

    [ClientRpc]
    private void RpcUpdatePings(int[] ids, int[] pings)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            int id = ids[i];
            int ping = pings[i];

            if (_uiPlashes.TryGetValue(id, out var plash) && plash != null)
            {
                bool isHost = (id == hostConnectionId);
                bool isUs = (id == _localPlayerId);

                if (isHost)
                    plash.UpdatePing("-- ms");
                else if (isUs)
                {
                    plash.UpdatePing("<Ты>");
                }
                else
                    plash.UpdatePing($"{ping.ToString()} ms");
            }
        }
    }

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
    public void ClientSetLocalPlayerId(int id)
    {
        _localPlayerId = id;
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

        plash.Init(this, playerId, nickname, isAdmin, isOnHost);
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

    [Client]
    public void ClientUpdatePlashNickname(int playerId, string nickname)
    {
        if (_uiPlashes.TryGetValue(playerId, out var plash) && plash != null && plash.GetNickname() != null)
        {
            plash.UpdateNickname(nickname);
        }
        ClientUpdateLobbyTitle();
    }

    // ---------- UI HOOKS (from PlayerPlashController) ----------

    public void KickFromUI(int targetPlayerId)
    {
        if (!IsLocalHost) return; // only host sees the button anyway
        if (LobbyPlayerController.LocalInstance != null)
            LobbyPlayerController.LocalInstance.CmdRequestKick(targetPlayerId);
    }

    public void StartGame()
    {
        if (!IsLocalHost) return;
        if (!NetworkServer.active) return;

        NetworkManager.singleton.ServerChangeScene(gameSceneName);
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}