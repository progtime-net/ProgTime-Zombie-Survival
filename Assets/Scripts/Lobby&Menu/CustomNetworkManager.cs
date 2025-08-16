using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    [Header("Custom Player Prefabs")]
    public GameObject lobbyPlayerPrefab;
    public GameObject gamePlayerPrefab;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "LobbyScene")
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
        if (sceneName != "Lobby")
        {
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                if (conn.identity != null && conn.identity.GetComponent<LobbyPlayerController>() != null)
                {
                    ReplaceWithGamePlayer(conn);
                }
            }
        }
    }

    private void SpawnGamePlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject gamePlayer = Instantiate(
            gamePlayerPrefab,
            startPos != null ? startPos.position : Vector3.zero,
            startPos != null ? startPos.rotation : Quaternion.identity
        );

        NetworkServer.AddPlayerForConnection(conn, gamePlayer);
    }

    private void ReplaceWithGamePlayer(NetworkConnectionToClient conn)
    {
        string nickname = LobbyPlayerController.LocalInstance.Nickname;

        Transform startPos = GetStartPosition();
        GameObject gamePlayerObj = Instantiate(
            gamePlayerPrefab,
            startPos != null ? startPos.position : Vector3.zero,
            startPos != null ? startPos.rotation : Quaternion.identity
        );

        //gamePlayer.nickname = nickname;

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayerObj, ReplacePlayerOptions.Destroy);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        if (!NetworkClient.ready) NetworkClient.Ready();
        if (NetworkClient.localPlayer == null) NetworkClient.AddPlayer();
    }
}
