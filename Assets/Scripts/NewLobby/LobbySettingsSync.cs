using Mirror;
using UnityEngine;

public class LobbySettingsSync : NetworkBehaviour
{
    [Header("Defaults (set in Inspector)")]
    [SerializeField] private int defaultMapIndex = 0;
    [SerializeField] private int defaultImpostorCountIndex = 0;
    [SerializeField] private int defaultMoveSpeedIndex = 1;
    [SerializeField] private int defaultKillCooldownIndex = 1;
    [SerializeField] private int defaultMeetingsPerPlayerIndex = 1;
    [SerializeField] private bool defaultRevealImpostors = true;
    [SerializeField] private bool defaultAnonymousVotes = false;
    [SerializeField] private bool defaultTaskbarUpdates = true;

    // --- Dropdown indices ---
    [SyncVar(hook = nameof(OnMapChanged))] public int mapIndex;
    [SyncVar(hook = nameof(OnImpostorCountChanged))] public int impostorCountIndex;
    [SyncVar(hook = nameof(OnMoveSpeedChanged))] public int moveSpeedIndex;
    [SyncVar(hook = nameof(OnKillCooldownChanged))] public int killCooldownIndex;
    [SyncVar(hook = nameof(OnMeetingsPerPlayerChanged))] public int meetingsPerPlayerIndex;

    // --- Toggles ---
    [SyncVar(hook = nameof(OnRevealImpostorsChanged))] public bool revealImpostors;
    [SyncVar(hook = nameof(OnAnonymousChanged))] public bool anonymousVotes;
    [SyncVar(hook = nameof(OnTaskbarUpdatesChanged))] public bool taskbarUpdates;

    private GameSettingsSnapshot _defaults;

    public static LobbySettingsSync Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public override void OnStartServer()
    {
        // Initialize from inspector defaults on first start
        mapIndex = defaultMapIndex;
        impostorCountIndex = defaultImpostorCountIndex;
        moveSpeedIndex = defaultMoveSpeedIndex;
        killCooldownIndex = defaultKillCooldownIndex;
        meetingsPerPlayerIndex = defaultMeetingsPerPlayerIndex;
        revealImpostors = defaultRevealImpostors;
        anonymousVotes = defaultAnonymousVotes;
        taskbarUpdates = defaultTaskbarUpdates;

        // Whatever you set in the Inspector becomes the defaults snapshot
        _defaults = Export();

        // Try to load last saved; if present, apply it
        if (LobbySettingsPersistence.TryLoad(out var saved))
            ApplySnapshot(saved);
    }

    // ---- Hooks -> UI ----
    void OnMapChanged(int _, int v) => LobbySettingsUI.Instance?.SetMap(v);
    void OnImpostorCountChanged(int _, int v) => LobbySettingsUI.Instance?.SetImpostorCount(v);
    void OnMoveSpeedChanged(int _, int v) => LobbySettingsUI.Instance?.SetMoveSpeed(v);
    void OnKillCooldownChanged(int _, int v) => LobbySettingsUI.Instance?.SetKillCooldown(v);
    void OnMeetingsPerPlayerChanged(int _, int v) => LobbySettingsUI.Instance?.SetMeetingsPerPlayer(v);
    void OnRevealImpostorsChanged(bool _, bool v) => LobbySettingsUI.Instance?.SetRevealImpostors(v);
    void OnAnonymousChanged(bool _, bool v) => LobbySettingsUI.Instance?.SetAnonymous(v);
    void OnTaskbarUpdatesChanged(bool _, bool v) => LobbySettingsUI.Instance?.SetTaskbarUpdates(v);

    // ---- Host-only setters (save after each change) ----
    [Server] public void SetMap(int v) { mapIndex = v; LobbySettingsPersistence.Save(Export()); }
    [Server] public void SetImpostorCount(int v) { impostorCountIndex = v; LobbySettingsPersistence.Save(Export()); }
    [Server] public void SetMoveSpeed(int v) { moveSpeedIndex = v; LobbySettingsPersistence.Save(Export()); }
    [Server] public void SetKillCooldown(int v) { killCooldownIndex = v; LobbySettingsPersistence.Save(Export()); }
    [Server] public void SetMeetingsPerPlayer(int v) { meetingsPerPlayerIndex = v; LobbySettingsPersistence.Save(Export()); }
    [Server] public void SetRevealImpostors(bool v) { revealImpostors = v; LobbySettingsPersistence.Save(Export()); }
    [Server] public void SetAnonymous(bool v) { anonymousVotes = v; LobbySettingsPersistence.Save(Export()); }
    [Server] public void SetTaskbarUpdates(bool v) { taskbarUpdates = v; LobbySettingsPersistence.Save(Export()); }

    // ---- Reset to scene defaults ----
    [Server]
    public void ResetToDefaults()
    {
        ApplySnapshot(_defaults);
        LobbySettingsPersistence.Save(_defaults);
    }

    // ---- Snapshot helpers ----
    public GameSettingsSnapshot Export() => new GameSettingsSnapshot
    {
        mapIndex = mapIndex,
        impostorCountIndex = impostorCountIndex,
        moveSpeedIndex = moveSpeedIndex,
        killCooldownIndex = killCooldownIndex,
        meetingsPerPlayerIndex = meetingsPerPlayerIndex,
        revealImpostors = revealImpostors,
        anonymousVotes = anonymousVotes,
        taskbarUpdates = taskbarUpdates
    };

    [Server]
    public void ApplySnapshot(GameSettingsSnapshot s)
    {
        mapIndex = s.mapIndex;
        impostorCountIndex = s.impostorCountIndex;
        moveSpeedIndex = s.moveSpeedIndex;
        killCooldownIndex = s.killCooldownIndex;
        meetingsPerPlayerIndex = s.meetingsPerPlayerIndex;
        revealImpostors = s.revealImpostors;
        anonymousVotes = s.anonymousVotes;
        taskbarUpdates = s.taskbarUpdates;
    }
}

public struct GameSettingsSnapshot
{
    public int mapIndex, impostorCountIndex, moveSpeedIndex, killCooldownIndex, meetingsPerPlayerIndex;
    public bool revealImpostors, anonymousVotes, taskbarUpdates;
}

public static class GameSettingsRuntime
{
    public static GameSettingsSnapshot Current { get; set; }
}
