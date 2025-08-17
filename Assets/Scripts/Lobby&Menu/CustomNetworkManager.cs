using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [Header("Custom Player Prefabs")]
    public GameObject lobbyPlayerPrefab;
    public GameObject gamePlayerPrefab;

    public override void Awake()
    {
        base.Awake();
        autoCreatePlayer = false;
        playerPrefab = null;
    }

    public override void OnClientConnect()
    {
        if (!NetworkClient.ready)
            NetworkClient.Ready();

        if (NetworkClient.localPlayer == null)
            NetworkClient.AddPlayer();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        string activeSceneName = SceneManager.GetActiveScene().name;

        if (activeSceneName == "LobbyScene")
        {
            Debug.Log("[Davilkus] Spawning lobby player");
            GameObject lobbyPlayer = Instantiate(lobbyPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
        }
        else
        {
            Debug.Log("[Davilkus] Spawning game player");
            SpawnGamePlayer(conn);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "LobbyScene" && conn.identity == null)
        {
            Debug.Log($"[Davilkus] Spawning game player for conn {conn.connectionId}");
            ReplaceWithGamePlayer(conn);
        }
    }

    private void SpawnGamePlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        Vector3 pos = startPos ? startPos.position : Vector3.zero;
        Quaternion rot = startPos ? startPos.rotation : Quaternion.identity;

        GameObject gamePlayer = Instantiate(gamePlayerPrefab, pos, rot);
        NetworkServer.AddPlayerForConnection(conn, gamePlayer);
    }

    private void ReplaceWithGamePlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        Vector3 pos = startPos ? startPos.position : Vector3.zero;
        Quaternion rot = startPos ? startPos.rotation : Quaternion.identity;

        GameObject gamePlayerObj = Instantiate(gamePlayerPrefab, pos, rot);

        /* Pass data to game player
        var gp = gamePlayerObj.GetComponent<GamePlayerController>();
        gp.nickname = LobbyPlayerController.LocalInstance.Nickname;
        */

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayerObj, ReplacePlayerOptions.KeepActive);
    }
}
