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
            Debug.Log("Add lobby player");
            GameObject lobbyPlayer = Instantiate(lobbyPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
        }
        else
        {
            Debug.Log("Add game player");
            SpawnGamePlayer(conn);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        if (conn.identity == null)
        {
            Debug.LogError("NetworkManager is broken. If you see this error - Davilkus sucks at programming. " +
                "Tell him to use GPT 5 instead of his total unskill. Mac sucks tho.");
        }

        base.OnServerReady(conn);

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "LobbyScene" && conn.identity == null)
        {
            Debug.Log($"Spawning game player for conn {conn.connectionId}");
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
