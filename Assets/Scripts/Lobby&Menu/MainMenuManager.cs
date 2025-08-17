using Mirror;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text nicknameTxt;
    [SerializeField] private TMP_Text versionText;
    [SerializeField] private Slider sensSlider;

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private float startGameTimeout = 5f;

    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private Button connectBtn;

    private NetworkManager _netManager;
    private float _sensitivity;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        _netManager = NetworkManager.singleton;

        var profile = PlayerProfileManager.Instance.profile;
        UpdateNickname();

        versionText.text = "v." + Application.version;

        _sensitivity = PlayerPrefs.GetFloat("sensitivity", 1.0f);
        sensSlider.value = _sensitivity;
    }

    public void HostGameRequest()
    {
        loadingPanel.SetActive(true);
        Invoke(nameof(HostGame), 1.5f);
    }

    public void JoinGameRequest()
    {
        loadingPanel.SetActive(true);
        Invoke(nameof(JoinGame), 1.5f);
    }

    private void HostGame()
    {
        _netManager.StartHost();

        Invoke(nameof(HostGameTimeout), startGameTimeout);
    }

    private void JoinGame()
    {
        _netManager.networkAddress = ipInputField.text;
        _netManager.StartClient();

        Invoke(nameof(JoinGameTimeout), startGameTimeout);
    }

    private void HostGameTimeout()
    {
        MenuPopupController.Instance.Show("Starting host timed out. Please try again.");
        loadingPanel.SetActive(false);
    }

    private void JoinGameTimeout()
    {
        MenuPopupController.Instance.Show("Failed to connect: Connection timed out.");
        loadingPanel.SetActive(false);
    }

    public void OnIPChanged(string value)
    {
        connectBtn.interactable = IsValidIP(value);
    }

    public void OnSensitivityChanged(float value)
    {
        _sensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", _sensitivity);
        PlayerPrefs.Save();
    }

    public void UpdateNickname() => UpdateNickname(PlayerProfileManager.Instance.profile);

    public void UpdateNickname(PlayerProfile profile)
    {
        nicknameTxt.text = profile.nickname;
        nicknameInputField.text = profile.nickname;
    }

    private bool IsValidIP(string ipString)
    {
        if (string.IsNullOrWhiteSpace(ipString))
            return false;

        if (IPAddress.TryParse(ipString, out _))
            return true;

        if (ipString.ToLower() == "localhost")
            return true;

        return false;
    }

    public void ExitGameRequest() => Application.Quit();
}
