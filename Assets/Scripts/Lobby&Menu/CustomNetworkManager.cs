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
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "LobbyScene") 
        {
            GameObject lobbyPlayer = Instantiate(lobbyPlayerPrefab);
            NetworkServer.AddPlayerForConnection(conn, lobbyPlayer);
        }
        else
        {
            SpawnGamePlayer(conn);
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName != "LobbyScene")
        {
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                if (conn?.identity == null) continue;

                var lobby = conn.identity.GetComponent<LobbyPlayerController>();
                if (lobby != null)
                {
                    ReplaceWithGamePlayer(conn, lobby);
                }
            }
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

    private void ReplaceWithGamePlayer(NetworkConnectionToClient conn, LobbyPlayerController lobby)
    {
        Transform startPos = GetStartPosition();
        Vector3 pos = startPos ? startPos.position : Vector3.zero;
        Quaternion rot = startPos ? startPos.rotation : Quaternion.identity;

        GameObject gamePlayerObj = Instantiate(gamePlayerPrefab, pos, rot);

        /*
        var gp = gamePlayerObj.GetComponent<GamePlayerController>();
        if (gp != null && lobby != null)
        {
            gp.nickname = lobby.Nickname;
        }*/

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayerObj, ReplacePlayerOptions.Destroy);
    }
}
