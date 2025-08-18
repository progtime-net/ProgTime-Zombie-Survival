using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbySettingsUI : NetworkBehaviour
{
    public static LobbySettingsUI Instance { get; private set; }

    [Header("Group")]
    [SerializeField] CanvasGroup panelGroup;

    [Header("Dropdowns (TMP)")]
    [SerializeField] TMP_Dropdown mapDropdown;
    [SerializeField] TMP_Dropdown impostorCountDropdown;
    [SerializeField] TMP_Dropdown moveSpeedDropdown;
    [SerializeField] TMP_Dropdown killCooldownDropdown;
    [SerializeField] TMP_Dropdown meetingsPerPlayerDropdown;

    [Header("Toggles")]
    [SerializeField] Toggle revealImpostorsToggle;
    [SerializeField] Toggle anonymousToggle;
    [SerializeField] Toggle taskBarUpdatesToggle;

    bool _suppress; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (LobbySettingsSync.Instance != null)
        {
            var s = LobbySettingsSync.Instance;
            SetMap(s.mapIndex);
            SetImpostorCount(s.impostorCountIndex);
            SetMoveSpeed(s.moveSpeedIndex);
            SetKillCooldown(s.killCooldownIndex);
            SetMeetingsPerPlayer(s.meetingsPerPlayerIndex);

            SetRevealImpostors(s.revealImpostors);
            SetAnonymous(s.anonymousVotes);
            if (taskBarUpdatesToggle) SetTaskbarUpdates(s.taskbarUpdates);
        }

        mapDropdown.onValueChanged.AddListener(OnMapChanged);
        impostorCountDropdown.onValueChanged.AddListener(OnImpostorCountChanged);
        moveSpeedDropdown.onValueChanged.AddListener(OnMoveSpeedChanged);
        killCooldownDropdown.onValueChanged.AddListener(OnKillCooldownChanged);
        meetingsPerPlayerDropdown.onValueChanged.AddListener(OnMeetingsPerPlayerChanged);

        revealImpostorsToggle.onValueChanged.AddListener(OnRevealImpostorsChanged);
        anonymousToggle.onValueChanged.AddListener(OnAnonymousChanged);
        if (taskBarUpdatesToggle) taskBarUpdatesToggle.onValueChanged.AddListener(OnTaskbarUpdatesChanged);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        RefreshInteractable();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        RefreshInteractable();
    }

    private void RefreshInteractable()
    {
        panelGroup.interactable = isHost;
        panelGroup.blocksRaycasts = isHost;
        panelGroup.alpha = 1f; 
    }

    // ---------- UI -> Server ----------
    void OnMapChanged(int v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetMap(v); }
    void OnImpostorCountChanged(int v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetImpostorCount(v); }
    void OnMoveSpeedChanged(int v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetMoveSpeed(v); }
    void OnKillCooldownChanged(int v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetKillCooldown(v); }
    void OnMeetingsPerPlayerChanged(int v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetMeetingsPerPlayer(v); }

    void OnRevealImpostorsChanged(bool v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetRevealImpostors(v); }
    void OnAnonymousChanged(bool v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetAnonymous(v); }
    void OnTaskbarUpdatesChanged(bool v) { if (_suppress) return; if (NetworkServer.active) LobbySettingsSync.Instance.SetTaskbarUpdates(v); }

    // ---------- Server -> UI ----------
    public void SetMap(int v) { _suppress = true; mapDropdown.SetValueWithoutNotify(v); _suppress = false; }
    public void SetImpostorCount(int v) { _suppress = true; impostorCountDropdown.SetValueWithoutNotify(v); _suppress = false; }
    public void SetMoveSpeed(int v) { _suppress = true; moveSpeedDropdown.SetValueWithoutNotify(v); _suppress = false; }
    public void SetKillCooldown(int v) { _suppress = true; killCooldownDropdown.SetValueWithoutNotify(v); _suppress = false; }
    public void SetMeetingsPerPlayer(int v) { _suppress = true; meetingsPerPlayerDropdown.SetValueWithoutNotify(v); _suppress = false; }

    public void SetRevealImpostors(bool v) { _suppress = true; revealImpostorsToggle.SetIsOnWithoutNotify(v); _suppress = false; }
    public void SetAnonymous(bool v) { _suppress = true; anonymousToggle.SetIsOnWithoutNotify(v); _suppress = false; }
    public void SetTaskbarUpdates(bool v) { if (!taskBarUpdatesToggle) return; _suppress = true; taskBarUpdatesToggle.SetIsOnWithoutNotify(v); _suppress = false; }

    // ---------- Reset settings ----------
    public void OnResetClicked()
    {
        if (Mirror.NetworkServer.active)
            LobbySettingsSync.Instance.ResetToDefaults();
    }

}
