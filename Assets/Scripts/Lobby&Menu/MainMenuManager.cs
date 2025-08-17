using Mirror;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text nicknameTxt;
    public TMP_Text versionText;
    public Slider sensSlider;

    public GameObject loadingPanel;
    public float startGameTimeout = 5f;

    public TMP_InputField ipInputField;
    public TMP_InputField nicknameInputField;
    public Button connectBtn;

    private NetworkManager netManager;
    private float sensitivity;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        netManager = NetworkManager.singleton;

        var profile = PlayerProfileManager.Instance.profile;
        UpdateNickname();

        versionText.text = "v." + Application.version;

        sensitivity = PlayerPrefs.GetFloat("sensitivity", 1.0f);
        sensSlider.value = sensitivity;
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
        netManager.StartHost();

        Invoke(nameof(HostGameTimeout), startGameTimeout);
    }

    private void JoinGame()
    {
        netManager.networkAddress = ipInputField.text;
        netManager.StartClient();

        Invoke(nameof(JoinGameTimeout), startGameTimeout);
    }

    private void HostGameTimeout()
    {
        MenuPopupCntrl.Instance.Show("Starting host timed out. Please try again.");
        loadingPanel.SetActive(false);
    }

    private void JoinGameTimeout()
    {
        MenuPopupCntrl.Instance.Show("Failed to connect: Connection timed out.");
        loadingPanel.SetActive(false);
    }

    public void OnIPChanged(string value)
    {
        connectBtn.interactable = IsValidIP(value);
    }

    public void OnSensitivityChanged(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", sensitivity);
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
