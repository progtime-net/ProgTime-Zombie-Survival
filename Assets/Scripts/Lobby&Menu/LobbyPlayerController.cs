using Mirror;
using UnityEngine;

public class LobbyPlayerController : NetworkBehaviour
{
    public static LobbyPlayerController LocalInstance { get; private set; }

    public string Nickname { get; private set; } = "Player";

    public string GetNickname()
    {
        return SyncedNickname;
    }

    public void SetNickname(string nick)
    {
        Nickname = string.IsNullOrWhiteSpace(nick) ? "Player" : nick;
    }

    [SyncVar] public int PlayerId; 
    [SyncVar(hook = nameof(OnNicknameChanged))] public string SyncedNickname;

    public override void OnStartServer()
    {
        PlayerId = connectionToClient != null ? connectionToClient.connectionId : GetInstanceID();
        LobbyManager.Instance.ServerRegister(this);
    }

    public override void OnStopServer()
    {
        if (LobbyManager.Instance != null)
            LobbyManager.Instance.ServerUnregister(this);
    }

    public override void OnStartClient()
    {
        LobbyManager.Instance?.ClientAddPlash(PlayerId, SyncedNickname);
    }

    public override void OnStopClient()
    {
        LobbyManager.Instance?.ClientRemovePlash(PlayerId);
    }

    public override void OnStartLocalPlayer()
    {
        LocalInstance = this;

        LobbyManager.Instance?.ClientSetLocalPlayerId(PlayerId);

        CmdSetNickname(PlayerProfileManager.Instance.profile.nickname);
    }

    private void OnNicknameChanged(string oldNick, string newNick)
    {
        LobbyManager.Instance?.ClientUpdatePlashNickname(PlayerId, newNick);
    }

    [Command]
    public void CmdSetNickname(string nick)
    {
        SyncedNickname = string.IsNullOrWhiteSpace(nick) ? $"Player_{PlayerId}" : nick.Trim();
    }

    [Command]
    public void CmdRequestKick(int targetPlayerId)
    {
        if (connectionToClient != NetworkServer.localConnection)
            return;

        LobbyManager.Instance.ServerKick(targetPlayerId);
    }
}